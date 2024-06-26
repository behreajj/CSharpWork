using System;
using System.Runtime.InteropServices;
using System.Text;

/// <summary>
/// A readonly struct to store colors in standard and linear red, green, blue
/// and alpha. Supports conversion to and from integers.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Explicit, Pack = 16)]
public readonly struct Rgb : IComparable<Rgb>, IEquatable<Rgb>
{
    /// <summary>
    /// The alpha (transparency) channel.
    /// </summary>
    [FieldOffset(0)] private readonly float a;

    /// <summary>
    /// The blue color channel.
    /// </summary>
    [FieldOffset(12)] private readonly float b;

    /// <summary>
    /// The green color channel.
    /// </summary>
    [FieldOffset(8)] private readonly float g;

    /// <summary>
    /// The red color channel.
    /// </summary>
    [FieldOffset(4)] private readonly float r;

    /// <summary>
    /// The alpha channel.
    /// </summary>
    /// <value>alpha</value>
    public float Alpha { get { return this.a; } }

    /// <summary>
    /// The blue channel.
    /// </summary>
    /// <value>blue</value>
    public float B { get { return this.b; } }

    /// <summary>
    /// The green channel.
    /// </summary>
    /// <value>green</value>
    public float G { get { return this.g; } }

    /// <summary>
    /// The red channel.
    /// </summary>
    /// <value>red</value>
    public float R { get { return this.r; } }

    /// <summary>
    /// Creates a color from unsigned bytes. Converts each to a single precision
    /// real number in the  range [0.0, 1.0].
    /// </summary>
    /// <param name="r">red channel</param>
    /// <param name="g">green channel</param>
    /// <param name="b">blue channel</param>
    /// <param name="a">alpha channel</param>
    public Rgb(in byte r, in byte g, in byte b, in byte a = 255)
    {
        this.r = r / 255.0f;
        this.g = g / 255.0f;
        this.b = b / 255.0f;
        this.a = a / 255.0f;
    }

    /// <summary>
    /// Creates a color from signed bytes. Converts each to a single precision
    /// real number in the range [0.0, 1.0].
    /// </summary>
    /// <param name="r">red channel</param>
    /// <param name="g">green channel</param>
    /// <param name="b">blue channel</param>
    /// <param name="a">alpha channel</param>
    public Rgb(in sbyte r, in sbyte g, in sbyte b, in sbyte a = -1)
    {
        this.r = (r & 0xff) / 255.0f;
        this.g = (g & 0xff) / 255.0f;
        this.b = (b & 0xff) / 255.0f;
        this.a = (a & 0xff) / 255.0f;
    }

    /// <summary>
    /// Creates a color from single precision real numbers.
    /// </summary>
    /// <param name="r">red channel</param>
    /// <param name="g">green channel</param>
    /// <param name="b">blue channel</param>
    /// <param name="a">alpha channel</param>
    public Rgb(in float r, in float g, in float b, in float a = 1.0f)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }

    /// <summary>
    /// Tests this color for equivalence with an object.
    /// </summary>
    /// <param name="value">the object</param>
    /// <returns>equivalence</returns>
    public override bool Equals(object value)
    {
        if (Object.ReferenceEquals(this, value)) { return true; }
        if (value is null) { return false; }
        if (value is Rgb clr) { return this.Equals(clr); }
        return false;
    }

    /// <summary>
    /// Returns a hash code representing this color.
    /// </summary>
    /// <returns>hash code</returns>
    public override int GetHashCode()
    {
        return (int)Rgb.ToHexArgb(this);
    }

    /// <summary>
    /// Returns a string representation of this color.
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()
    {
        return Rgb.ToString(this);
    }

    /// <summary>
    /// Compares this color to another in compliance with the IComparable
    /// interface.
    /// </summary>
    /// <param name="c">comparisand</param>
    /// <returns>evaluation</returns>
    public int CompareTo(Rgb c)
    {
        uint left = Rgb.ToHexArgb(this);
        uint right = Rgb.ToHexArgb(c);
        return (left < right) ? -1 : (left > right) ? 1 : 0;
    }

    /// <summary>
    /// Tests this color for equivalence with another in compliance with the
    /// IEquatable interface.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>equivalence</returns>
    public bool Equals(Rgb c)
    {
        return Rgb.EqSatArith(this, c);
    }

    /// <summary>
    /// Converts a color to a boolean by returning whether its alpha is greater
    /// than zero.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>evaluation</returns>
    public static explicit operator bool(in Rgb c)
    {
        return Rgb.Any(c);
    }

    /// <summary>
    /// A color evaluates to true if its alpha channel is greater than zero.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>evaluation</returns>
    public static bool operator true(in Rgb c)
    {
        return Rgb.Any(c);
    }

    /// <summary>
    /// A color evaluates to false if its alpha channel is less than or equal
    /// to zero.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>evaluation</returns>
    public static bool operator false(in Rgb c)
    {
        return Rgb.None(c);
    }

    /// <summary>
    /// Converts a color to an integer, performs the bitwise not operation on
    /// it, then converts the result to a color.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>negated color</returns>
    public static Rgb operator ~(in Rgb c)
    {
        return Rgb.FromHexArgb(~Rgb.ToHexArgb(c));
    }

    /// <summary>
    /// Converts two colors to integers, performs the bitwise and operation on
    /// them, then converts the result to a color.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>color</returns>
    public static Rgb operator &(in Rgb a, in Rgb b)
    {
        return Rgb.FromHexArgb(Rgb.ToHexArgb(a) & Rgb.ToHexArgb(b));
    }

    /// <summary>
    /// Converts two colors to integers, performs the bitwise inclusive or
    /// operation on them, then converts the result to a color.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>color</returns>
    public static Rgb operator |(in Rgb a, in Rgb b)
    {
        return Rgb.FromHexArgb(Rgb.ToHexArgb(a) | Rgb.ToHexArgb(b));
    }

    /// <summary>
    /// Converts two colors to integers, performs the bitwise exclusive or
    /// operation on them, then converts the result to a color.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>color</returns>
    public static Rgb operator ^(in Rgb a, in Rgb b)
    {
        return Rgb.FromHexArgb(Rgb.ToHexArgb(a) ^ Rgb.ToHexArgb(b));
    }

    /// <summary>
    /// Tests to see if all color channels are greater than zero.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>evaluation</returns>
    public static bool All(in Rgb c)
    {
        return (c.a > 0.0f) &&
            (c.r > 0.0f) &&
            (c.g > 0.0f) &&
            (c.b > 0.0f);
    }

    /// <summary>
    /// Tests to see if the alpha channel of the color is greater than zero,
    /// i.e., if it has some opacity.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>evaluation</returns>
    public static bool Any(in Rgb c)
    {
        return c.a > 0.0f;
    }

    /// <summary>
    /// Clamps a color to a lower and upper bound.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="lb">lower bound</param>
    /// <param name="ub">upper bound</param>
    /// <returns>the clamped color</returns>
    public static Rgb Clamp(in Rgb c, in float lb = 0.0f, in float ub = 1.0f)
    {
        return new(
            Utils.Clamp(c.r, lb, ub),
            Utils.Clamp(c.g, lb, ub),
            Utils.Clamp(c.b, lb, ub),
            Utils.Clamp(c.a, lb, ub));
    }

    /// <summary>
    /// Tests to see if a color contains a value.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="v">value</param>
    /// <returns>evaluation</returns>
    public static bool Contains(in Rgb c, in float v)
    {
        return Utils.Approx(c.a, v) ||
            Utils.Approx(c.b, v) ||
            Utils.Approx(c.g, v) ||
            Utils.Approx(c.r, v);
    }

    /// <summary>
    /// Returns the first color argument with the alpha of the second.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>color</returns>
    public static Rgb CopyAlpha(in Rgb a, in Rgb b)
    {
        return new(a.r, a.g, a.b, b.a);
    }

    /// <summary>
    /// Checks if two colors have equivalent alpha channels when converted to
    /// bytes in [0, 255]. Uses saturation arithmetic.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static bool EqAlphaSatArith(in Rgb a, in Rgb b)
    {
        return (int)(Utils.Clamp(a.a) * 255.0f + 0.5f) ==
               (int)(Utils.Clamp(b.a) * 255.0f + 0.5f);
    }

    /// <summary>
    /// Checks if two colors have equivalent red, green and blue channels when
    /// converted to bytes in [0, 255]. Uses saturation arithmetic.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static bool EqRgbSatArith(in Rgb a, in Rgb b)
    {
        return (int)(Utils.Clamp(a.b) * 255.0f + 0.5f) ==
               (int)(Utils.Clamp(b.b) * 255.0f + 0.5f) &&
               (int)(Utils.Clamp(a.g) * 255.0f + 0.5f) ==
               (int)(Utils.Clamp(b.g) * 255.0f + 0.5f) &&
               (int)(Utils.Clamp(a.r) * 255.0f + 0.5f) ==
               (int)(Utils.Clamp(b.r) * 255.0f + 0.5f);
    }

    /// <summary>
    /// Checks if two colors have equivalent red, green, blue and alpha
    /// channels when converted to bytes in [0, 255]. Uses saturation
    /// arithmetic.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static bool EqSatArith(in Rgb a, in Rgb b)
    {
        return Rgb.EqAlphaSatArith(a, b) &&
            Rgb.EqRgbSatArith(a, b);
    }

    /// <summary>
    /// Converts a 3D vector to a color, as used in normal maps. If the
    /// vector's magnitude is less than or equal to zero, returns the color
    /// representation of up.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>color</returns>
    public static Rgb FromDir(in Vec3 v)
    {
        float mSq = Vec3.MagSq(v);
        if (mSq > 0.0f)
        {
            float mInv = 0.5f / MathF.Sqrt(mSq);
            return new(
                v.X * mInv + 0.5f,
                v.Y * mInv + 0.5f,
                v.Z * mInv + 0.5f,
                1.0f);
        }
        return Rgb.Up;
    }

    /// <summary>
    /// Converts a hexadecimal representation of a color into RGB.
    /// </summary>
    /// <param name="c">integer</param>
    /// <param name="o">color channel order</param>
    /// <returns>color</returns>
    public static Rgb FromHex(in uint c, in RgbFormat o = RgbFormat.ARGB)
    {
        switch (o)
        {
            case RgbFormat.ABGR:
                { return Rgb.FromHexAbgr(c); }
            case RgbFormat.RGBA:
                { return Rgb.FromHexRgba(c); }
            case RgbFormat.ARGB:
            default:
                { return Rgb.FromHexArgb(c); }
        }
    }

    /// <summary>
    /// Converts a hexadecimal representation of a color into RGB.
    /// The unsigned integer is expected to be ordered as 0xAABBGGRR.
    /// </summary>
    /// <param name="c">integer</param>
    /// <returns>color</returns>
    public static Rgb FromHexAbgr(in uint c)
    {
        return new(
            (c & 0xff) / 255.0f,
            ((c >> 0x08) & 0xff) / 255.0f,
            ((c >> 0x10) & 0xff) / 255.0f,
            ((c >> 0x18) & 0xff) / 255.0f);
    }

    /// <summary>
    /// Converts a hexadecimal representation of a color into RGB.
    /// The unsigned integer is expected to be ordered as 0xAARRGGBB.
    /// </summary>
    /// <param name="c">integer</param>
    /// <returns>color</returns>
    public static Rgb FromHexArgb(in uint c)
    {
        return new(
            ((c >> 0x10) & 0xff) / 255.0f,
            ((c >> 0x08) & 0xff) / 255.0f,
            (c & 0xff) / 255.0f,
            ((c >> 0x18) & 0xff) / 255.0f);
    }

    /// <summary>
    /// Converts a hexadecimal representation of a color into RGB.
    /// The unsigned integer is expected to be ordered as 0xRRGGBBAA.
    /// </summary>
    /// <param name="c">integer</param>
    /// <returns>color</returns>
    public static Rgb FromHexRgba(in uint c)
    {
        return new(
            ((c >> 0x18) & 0xff) / 255.0f,
            ((c >> 0x10) & 0xff) / 255.0f,
            ((c >> 0x08) & 0xff) / 255.0f,
            (c & 0xff) / 255.0f);
    }

    /// <summary>
    /// Generates a 3D array of colors representing a grid in standard RGB.
    /// </summary>
    /// <param name="cols">number of columns</param>
    /// <param name="rows">number of rows</param>
    /// <param name="layers">number of layers</param>
    /// <param name="alpha">alpha channel</param>
    /// <returns>array</returns>
    public static Rgb[,,] GridStandard(
        in int cols = 8,
        in int rows = 8,
        in int layers = 8,
        in float alpha = 1.0f)
    {
        int lVrf = layers < 1 ? 1 : layers;
        int rVrf = rows < 1 ? 1 : rows;
        int cVrf = cols < 1 ? 1 : cols;

        bool oneLayer = lVrf == 1;
        bool oneRow = rVrf == 1;
        bool oneCol = cVrf == 1;

        float hToStep = oneLayer ? 0.0f : 1.0f / (lVrf - 1.0f);
        float iToStep = oneRow ? 0.0f : 1.0f / (rVrf - 1.0f);
        float jToStep = oneCol ? 0.0f : 1.0f / (cVrf - 1.0f);

        Rgb[,,] result = new Rgb[lVrf, rVrf, cVrf];

        int rcVrf = rVrf * cVrf;
        int len3 = lVrf * rcVrf;
        for (int k = 0; k < len3; ++k)
        {
            int h = k / rcVrf;
            int m = k - h * rcVrf;
            int i = m / cVrf;
            int j = m % cVrf;

            result[h, i, j] = new(
                j * jToStep,
                i * iToStep,
                h * hToStep,
                alpha);
        }

        return result;
    }

    /// <summary>
    /// Evaluates whether a color's red, green and blue channels are within the
    /// range [0.0, 1.0].
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="eps">tolerance</param>
    /// <returns>evaluation</returns>
    public static bool IsInGamut(in Rgb c, in float eps = 0.0f)
    {
        float oneEps = 1.0f + eps;
        return c.r >= -eps && c.r <= oneEps
            && c.g >= -eps && c.g <= oneEps
            && c.b >= -eps && c.b <= oneEps;
    }

    /// <summary>
    /// Returns the relative luminance of the color.
    /// Assumes the color is in linear RGB.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>luminance</returns>
    public static float LinearLuminance(in Rgb c)
    {
        return 0.21264935f * c.r +
               0.71516913f * c.g +
               0.07218152f * c.b;
    }

    /// <summary>
    /// Converts a color from linear RGB to the XYZ
    /// coordinates of SR LAB 2. See
    /// See Jan Behrens, https://www.magnetkern.de/srlab2.html .
    /// The alpha channel is unaffected by the transformation.
    /// </summary>
    /// <param name="c">linear color</param>
    /// <returns>XYZ color</returns>
    public static Vec4 LinearToSrXyz(in Rgb c)
    {
        double cr = c.r;
        double cg = c.g;
        double cb = c.b;
        return new(
            (float)(0.32053d * cr + 0.63692d * cg + 0.04256d * cb),
            (float)(0.161987d * cr + 0.756636d * cg + 0.081376d * cb),
            (float)(0.017228d * cr + 0.10866d * cg + 0.874112d * cb),
            c.a);
    }

    /// <summary>
    /// Converts a color from linear RGB to standard RGB (sRGB).
    /// If the alpha flag is true, also transforms the alpha channel.
    /// </summary>
    /// <param name="c">linear color</param>
    /// <param name="alpha">transform the alpha channel</param>
    /// <returns>standard color</returns>
    public static Rgb LinearToStandard(in Rgb c, in bool alpha = false)
    {
        const float inv24 = 1.0f / 2.4f;
        return new(
            c.r > 0.0031308f ?
            MathF.Pow(c.r, inv24) * 1.055f - 0.055f :
            c.r * 12.92f,

            c.g > 0.0031308f ?
            MathF.Pow(c.g, inv24) * 1.055f - 0.055f :
            c.g * 12.92f,

            c.b > 0.0031308f ?
            MathF.Pow(c.b, inv24) * 1.055f - 0.055f :
            c.b * 12.92f,

            alpha ? c.a > 0.0031308f ?
            MathF.Pow(c.a, inv24) * 1.055f - 0.055f :
            c.a * 12.92f :
            c.a);
    }

    /// <summary>
    /// Mixes two colors by a step in the range [0.0, 1.0] .
    /// Assumes the colors are in standard RGB; converts them to
    /// linear, then mixes them, then converts to standard.
    /// </summary>
    /// <param name="o">origin color</param>
    /// <param name="d">destination color</param>
    /// <param name="t">step</param>
    /// <returns>mixed color</returns>
    public static Rgb MixRgbaLinear(in Rgb o, in Rgb d, in float t = 0.5f)
    {
        return Rgb.LinearToStandard(
            Rgb.MixRgbaStandard(
                Rgb.StandardToLinear(o),
                Rgb.StandardToLinear(d), t));
    }

    /// <summary>
    /// Mixes two colors by a step in the range [0.0, 1.0] .
    /// Does not transform the colors in any way.
    /// </summary>
    /// <param name="o">origin color</param>
    /// <param name="d">destination color</param>
    /// <param name="t">step</param>
    /// <returns>mixed color</returns>
    public static Rgb MixRgbaStandard(in Rgb o, in Rgb d, in float t = 0.5f)
    {
        float u = 1.0f - t;
        return new(
            u * o.r + t * d.r,
            u * o.g + t * d.g,
            u * o.b + t * d.b,
            u * o.a + t * d.a);
    }


    /// <summary>
    /// Mixes two colors by a step in the range [0.0, 1.0] .
    /// Converts each color from sRGB to CIE LAB, mixes according
    /// to the step, then converts back to sRGB.
    /// </summary>
    /// <param name="o">origin color</param>
    /// <param name="d">destination color</param>
    /// <param name="t">step</param>
    /// <returns>mixed color</returns>
    public static Rgb MixSrLab2(in Rgb o, in Rgb d, in float t = 0.5f)
    {
        Lab oLab = Rgb.StandardToSrLab2(o);
        Lab dLab = Rgb.StandardToSrLab2(d);
        return Rgb.SrLab2ToStandard(Lab.Mix(oLab, dLab, t));
    }

    /// <summary>
    /// Mixes two colors by a step in the range [0.0, 1.0] .
    /// Converts each color from sRGB to CIE LCH, mixes according
    /// to the step, then converts back to sRGB.
    /// </summary>
    /// <param name="o">origin color</param>
    /// <param name="d">destination color</param>
    /// <param name="t">step</param>
    /// <returns>mixed color</returns>
    public static Rgb MixSrLch(in Rgb o, in Rgb d, in float t = 0.5f)
    {
        return Rgb.MixSrLch(o, d, t,
            (x, y, z, w) => Utils.LerpAngleNear(x, y, z, w));
    }

    /// <summary>
    /// Mixes two colors by a step in the range [0.0, 1.0] .
    /// Converts each color from sRGB to CIE LCH, mixes according
    /// to the step, then converts back to sRGB.
    /// The easing function is expected to ease from an origin
    /// hue to a destination by a factor according to a range.
    /// </summary>
    /// <param name="o">origin color</param>
    /// <param name="d">destination color</param>
    /// <param name="t">step</param>
    /// <param name="easing">easing function</param>
    /// <returns>mixed color</returns>
    public static Rgb MixSrLch(
        in Rgb o,
        in Rgb d,
        in float t,
        in Func<float, float, float, float, float> easing)
    {
        Lab oLab = Rgb.StandardToSrLab2(o);
        float oa = oLab.A;
        float ob = oLab.B;
        float oChrSq = oa * oa + ob * ob;

        Lab dLab = Rgb.StandardToSrLab2(d);
        float da = dLab.A;
        float db = dLab.B;
        float dChrSq = da * da + db * db;

        Lab cLab;

        if (oChrSq < Utils.Epsilon || dChrSq < Utils.Epsilon)
        {
            cLab = Lab.Mix(oLab, dLab, t);
        }
        else
        {
            float oChr = MathF.Sqrt(oChrSq);
            float oHue = Utils.OneTau * Utils.WrapRadians(
                MathF.Atan2(ob, oa));

            float dChr = MathF.Sqrt(dChrSq);
            float dHue = Utils.OneTau * Utils.WrapRadians(
                MathF.Atan2(db, da));

            float u = 1.0f - t;
            Lch cLch = new(
                easing(oHue, dHue, t, 1.0f),
                u * oChr + t * dChr,
                u * oLab.L + t * dLab.L,
                u * oLab.Alpha + t * dLab.Alpha);
            cLab = Lab.FromLch(cLch);
        }

        return Rgb.SrLab2ToStandard(cLab);
    }

    /// <summary>
    /// Tests to see if the alpha channel of this color is less than or equal to
    /// zero, i.e., if it is completely transparent.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>evaluation</returns>
    public static bool None(in Rgb c)
    {
        return c.a <= 0.0f;
    }

    /// <summary>
    /// Returns an opaque version of the color, i.e., where its alpha is 1.0.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>opaque</returns>
    public static Rgb Opaque(in Rgb c)
    {
        return new(c.r, c.g, c.b, 1.0f);
    }

    /// <summary>
    /// Multiplies the red, green and blue color channels of a color by the
    /// alpha channel. If alpha is less than or equal to zero, returns
    /// clear black.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>premultiplied color</returns>
    public static Rgb Premul(in Rgb c)
    {
        if (c.a <= 0.0f) { return Rgb.ClearBlack; }
        if (c.a >= 1.0f) { return Rgb.Opaque(c); }
        return new(
            c.r * c.a,
            c.g * c.a,
            c.b * c.a,
            c.a);
    }

    /// <summary>
    /// Reduces the signal, or granularity, of a color's channels.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="levels">levels</param>
    /// <returns>posterized color</returns>
    public static Rgb Quantize(in Rgb c, in int levels)
    {
        return Rgb.QuantizeUnsigned(c, levels);
    }

    /// <summary>
    /// Reduces the signal, or granularity, of a color's channels.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="levels">levels</param>
    /// <returns>posterized color</returns>
    public static Rgb QuantizeUnsigned(in Rgb c, in int levels)
    {
        return Rgb.QuantizeUnsigned(c, levels, levels, levels, levels);
    }

    /// <summary>
    /// Reduces the signal, or granularity, of a color's red, green and blue
    /// channels. Does not alter the alpha channel.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="rLevels">red levels</param>
    /// <param name="gLevels">green levels</param>
    /// <param name="bLevels">blue levels</param>
    /// <returns>posterized color</returns>
    public static Rgb QuantizeUnsigned(
        in Rgb c,
        in int rLevels,
        in int gLevels,
        in int bLevels)
    {
        return new(
            Utils.QuantizeUnsigned(c.r, rLevels),
            Utils.QuantizeUnsigned(c.g, gLevels),
            Utils.QuantizeUnsigned(c.b, bLevels), c.a);
    }

    /// <summary>
    /// Reduces the signal, or granularity, of a color's channels.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="rLevels">red levels</param>
    /// <param name="gLevels">green levels</param>
    /// <param name="bLevels">blue levels</param>
    /// <param name="aLevels">alpha levels</param>
    /// <returns>posterized color</returns>
    public static Rgb QuantizeUnsigned(
        in Rgb c,
        in int rLevels,
        in int gLevels,
        in int bLevels,
        in int aLevels)
    {
        return new(
            Utils.QuantizeUnsigned(c.r, rLevels),
            Utils.QuantizeUnsigned(c.g, gLevels),
            Utils.QuantizeUnsigned(c.b, bLevels),
            Utils.QuantizeUnsigned(c.a, aLevels));
    }

    /// <summary>
    /// Converts a color from SR LAB 2 to standard RGB (sRGB).
    /// </summary>
    /// <param name="lab">LAB color</param>
    /// <returns>sRGB color</returns>
    public static Rgb SrLab2ToStandard(in Lab lab)
    {
        return Rgb.LinearToStandard(
            Rgb.SrXyzToLinear(
                Lab.ToSrXyz(lab)));
    }

    /// <summary>
    /// Converts a color from SR LCH to standard RGB (sRGB).
    /// </summary>
    /// <param name="lch">LCH color</param>
    /// <returns>LAB color</returns>
    public static Rgb SrLchToStandard(in Lch lch)
    {
        return Rgb.LinearToStandard(
            Rgb.SrXyzToLinear(
                Lab.ToSrXyz(
                    Lab.FromLch(lch))));
    }

    /// <summary>
    /// Converts a color from SR LAB 2 XYZ to linear RGB.
    /// The alpha channel is unaffected by the transformation.
    /// </summary>
    /// <param name="v">XYZ color</param>
    /// <returns>linear color</returns>
    public static Rgb SrXyzToLinear(in Vec4 v)
    {
        double vx = v.X;
        double vy = v.Y;
        double vz = v.Z;
        return new(
            (float)(5.435679d * vx - 4.599131d * vy + 0.163593d * vz),
            (float)(-1.16809d * vx + 2.327977d * vy - 0.159798d * vz),
            (float)(0.03784d * vx - 0.198564d * vy + 1.160644d * vz),
            v.W);
    }

    /// <summary>
    /// Returns the relative luminance of the color.
    /// Converts the color from standard RGB to linear.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>luminance</returns>
    public static float StandardLuminance(in Rgb c)
    {
        return Rgb.LinearLuminance(Rgb.StandardToLinear(c));
    }

    /// <summary>
    /// Converts a color from standard RGB (sRGB) to SR LAB 2.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>LAB color</returns>
    public static Lab StandardToSrLab2(in Rgb c)
    {
        return Lab.FromSrXyz(
            Rgb.LinearToSrXyz(
                Rgb.StandardToLinear(c)));
    }

    /// <summary>
    /// Converts a color from standard RGB (sRGB) to SR LCH.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>LCH color</returns>
    public static Lch StandardToSrLch(in Rgb c)
    {
        return Lch.FromLab(
            Lab.FromSrXyz(
                Rgb.LinearToSrXyz(
                    Rgb.StandardToLinear(c))));
    }

    /// <summary>
    /// Converts a color from standard RGB (sRGB) to linear RGB.
    /// If the alpha flag is true, also transforms the alpha channel.
    /// </summary>
    /// <param name="c">the standard color</param>
    /// <param name="alpha">transform the alpha channel</param>
    /// <returns>linear color</returns>
    public static Rgb StandardToLinear(in Rgb c, in bool alpha = false)
    {
        const float inv1_055 = 1.0f / 1.055f;
        const float inv12_92 = 1.0f / 12.92f;
        return new(
            c.r > 0.04045f ?
            MathF.Pow((c.r + 0.055f) * inv1_055, 2.4f) :
            c.r * inv12_92,

            c.g > 0.04045f ?
            MathF.Pow((c.g + 0.055f) * inv1_055, 2.4f) :
            c.g * inv12_92,

            c.b > 0.04045f ?
            MathF.Pow((c.b + 0.055f) * inv1_055, 2.4f) :
            c.b * inv12_92,

            alpha ? c.a > 0.04045f ?
            MathF.Pow((c.a + 0.055f) * inv1_055, 2.4f) :
            c.a * inv12_92 :
            c.a);
    }

    /// <summary>
    /// Converts a color to an integer.
    /// </summary>
    /// <param name="c">the input color</param>
    /// <param name="order">color channel order</param>
    /// <returns>hexadecimal color</returns>
    public static uint ToHex(in Rgb c, in RgbFormat order = RgbFormat.ARGB)
    {
        switch (order)
        {
            case RgbFormat.ABGR:
                { return Rgb.ToHexAbgr(c); }
            case RgbFormat.RGBA:
                { return Rgb.ToHexRgba(c); }
            case RgbFormat.ARGB:
            default:
                { return Rgb.ToHexArgb(c); }
        }
    }

    /// <summary>
    /// Converts a color to an integer. Returns an integer ordered as
    /// 0xAABBGGRR.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>hexadecimal color</returns>
    public static uint ToHexAbgr(in Rgb c)
    {
        return Rgb.ToHexAbgrUnchecked(Rgb.Clamp(c, 0.0f, 1.0f));
    }

    /// <summary>
    /// Converts a color to an integer. Returns an integer ordered as
    /// 0xAARRGGBB.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>hexadecimal color</returns>
    public static uint ToHexArgb(in Rgb c)
    {
        return Rgb.ToHexArgbUnchecked(Rgb.Clamp(c, 0.0f, 1.0f));
    }

    /// <summary>
    /// Converts a color to an integer. Returns an integer ordered as
    /// 0xRRGGBBAA.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>hexadecimal color</returns>
    public static uint ToHexRgba(in Rgb c)
    {
        return Rgb.ToHexRgbaUnchecked(Rgb.Clamp(c, 0.0f, 1.0f));
    }

    /// <summary>
    /// Converts a color to an integer. Does not check if the color's components
    /// are in a valid range, [0.0, 1.0].
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="o">color channel order</param>
    /// <returns>hexadecimal color</returns>
    public static uint ToHexUnchecked(in Rgb c, in RgbFormat o = RgbFormat.ARGB)
    {
        switch (o)
        {
            case RgbFormat.ABGR:
                { return Rgb.ToHexAbgrUnchecked(c); }
            case RgbFormat.RGBA:
                { return Rgb.ToHexRgbaUnchecked(c); }
            case RgbFormat.ARGB:
            default:
                { return Rgb.ToHexArgbUnchecked(c); }
        }
    }

    /// <summary>
    /// Converts a color to an integer. Does not check if the color's components
    /// are in a valid range, [0.0, 1.0]. Returns an integer ordered as
    /// 0xAABBGGRR.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>hexadecimal color</returns>
    public static uint ToHexAbgrUnchecked(in Rgb c)
    {
        return (uint)(c.a * 255.0f + 0.5f) << 0x18 |
               (uint)(c.b * 255.0f + 0.5f) << 0x10 |
               (uint)(c.g * 255.0f + 0.5f) << 0x08 |
               (uint)(c.r * 255.0f + 0.5f);
    }

    /// <summary>
    /// Converts a color to an integer. Does not check if the color's components
    /// are in a valid range, [0.0, 1.0]. Returns an integer ordered as
    /// 0xAARRGGBB.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>hexadecimal color</returns>
    public static uint ToHexArgbUnchecked(in Rgb c)
    {
        return (uint)(c.a * 255.0f + 0.5f) << 0x18 |
               (uint)(c.r * 255.0f + 0.5f) << 0x10 |
               (uint)(c.g * 255.0f + 0.5f) << 0x08 |
               (uint)(c.b * 255.0f + 0.5f);
    }

    /// <summary>
    /// Converts a color to an integer. Does not check if the color's components
    /// are in a valid range, [0.0, 1.0]. Returns an integer ordered as
    /// 0xRRGGBBAA.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>hexadecimal color</returns>
    public static uint ToHexRgbaUnchecked(in Rgb c)
    {
        return (uint)(c.r * 255.0f + 0.5f) << 0x18 |
               (uint)(c.g * 255.0f + 0.5f) << 0x10 |
               (uint)(c.b * 255.0f + 0.5f) << 0x08 |
               (uint)(c.a * 255.0f + 0.5f);
    }

    /// <summary>
    /// Returns a string representation of a color in web-friendly hexadecimal
    /// format, i.e., in RRGGBB order. Does not prepend a hash tag. Clamps all
    /// values to [0.0, 1.0] before converting to a byte.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>string</returns>
    public static string ToHexWeb(in Rgb c)
    {
        return Rgb.ToHexWeb(new StringBuilder(8), c,
            (x) => Rgb.Clamp(x, 0.0f, 1.0f)).ToString();
    }

    /// <summary>
    /// Appends a string representation of a color in web-friendly hexadecimal
    /// to a string builder. Does not prepend a hash tag. The tone mapper
    /// function should bring the color into [0.0, 1.0].
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="c">color</param>
    /// <param name="tm">tone mapper</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToHexWeb(
        in StringBuilder sb,
        in Rgb c,
        in Func<Rgb, Rgb> tm)
    {
        return Rgb.ToHexWebUnchecked(sb, tm(c));
    }

    /// <summary>
    /// Appends a string representation of a color in web-friendly hexadecimal
    /// to a string builder. Does not prepend a hash tag. Does not validate
    /// colors that are out of gamut.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="c">color</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToHexWebUnchecked(in StringBuilder sb, in Rgb c)
    {
        int r = (int)(c.r * 255.0f + 0.5f);
        int g = (int)(c.g * 255.0f + 0.5f);
        int b = (int)(c.b * 255.0f + 0.5f);
        sb.AppendFormat("{0:X6}", r << 0x10 | g << 0x08 | b);
        return sb;
    }

    /// <summary>
    /// For colors which exceed the range [0.0, 1.0], applies ACES tone mapping
    /// algorithm. See https://64.github.io/tonemapping/ .
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>tone mapped color</returns>
    public static Rgb ToneMapAcesLinear(in Rgb c)
    {
        float rFrwrd = 0.59719f * c.r + 0.35458f * c.g + 0.04823f * c.b;
        float gFrwrd = 0.076f * c.r + 0.90834f * c.g + 0.01566f * c.b;
        float bFrwrd = 0.0284f * c.r + 0.13383f * c.g + 0.83777f * c.b;

        float ar = rFrwrd * (rFrwrd + 0.0245786f) - 0.000090537f;
        float ag = gFrwrd * (gFrwrd + 0.0245786f) - 0.000090537f;
        float ab = bFrwrd * (bFrwrd + 0.0245786f) - 0.000090537f;

        float br = rFrwrd * (0.983729f * rFrwrd + 0.432951f) + 0.238081f;
        float bg = gFrwrd * (0.983729f * gFrwrd + 0.432951f) + 0.238081f;
        float bb = bFrwrd * (0.983729f * bFrwrd + 0.432951f) + 0.238081f;

        float cr = Utils.Div(ar, br);
        float cg = Utils.Div(ag, bg);
        float cb = Utils.Div(ab, bb);

        float rBckwd = 1.60475f * cr - 0.53108f * cg - 0.07367f * cb;
        float gBckwd = -0.10208f * cr + 1.10813f * cg - 0.00605f * cb;
        float bBckwd = -0.00327f * cr - 0.07276f * cg + 1.07602f * cb;

        return new(
            Utils.Clamp(rBckwd, 0.0f, 1.0f),
            Utils.Clamp(gBckwd, 0.0f, 1.0f),
            Utils.Clamp(bBckwd, 0.0f, 1.0f),
            Utils.Clamp(c.a, 0.0f, 1.0f));
    }

    /// <summary>
    /// For colors which exceed the range [0.0, 1.0], applies ACES tone mapping
    /// algorithm. See https://64.github.io/tonemapping/ . Assumes that the
    /// input color is in gamma sRGB, and that the expected return should be as
    /// well.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>tone mapped color</returns>
    public static Rgb ToneMapAcesStandard(in Rgb c)
    {
        return Rgb.LinearToStandard(
            Rgb.ToneMapAcesLinear(
                Rgb.StandardToLinear(c)));
    }

    /// <summary>
    /// For colors which exceed the range [0.0, 1.0], applies Uncharted 2, or
    /// Hable, tone mapping algorithm. See https://64.github.io/tonemapping/ .
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>tone mapped color</returns>
    public static Rgb ToneMapHableLinear(in Rgb c)
    {
        const float A = 0.15f;
        const float B = 0.50f;
        const float C = 0.10f;
        const float D = 0.20f;
        const float E = 0.02f;
        const float F = 0.30f;
        const float W = 11.2f;
        const float whiteScale = 1.0f / (((W * (A * W + C * B) + D * E)
            / (W * (A * W + B) + D * F)) - E / F);

        const float exposureBias = 2.0f;
        float er = c.r * exposureBias;
        float eg = c.g * exposureBias;
        float eb = c.b * exposureBias;

        float xr = ((er * (A * er + C * B) + D * E)
            / (er * (A * er + B) + D * F)) - E / F;
        float xg = ((eg * (A * eg + C * B) + D * E)
            / (eg * (A * eg + B) + D * F)) - E / F;
        float xb = ((eb * (A * eb + C * B) + D * E)
            / (eb * (A * eb + B) + D * F)) - E / F;

        return new(
            Utils.Clamp(xr * whiteScale, 0.0f, 1.0f),
            Utils.Clamp(xg * whiteScale, 0.0f, 1.0f),
            Utils.Clamp(xb * whiteScale, 0.0f, 1.0f),
            Utils.Clamp(c.a, 0.0f, 1.0f));
    }

    /// <summary>
    /// For colors which exceed the range [0.0, 1.0], applies Uncharted 2, or
    /// Hable, tone mapping algorithm. See https://64.github.io/tonemapping/ .
    /// Assumes that the input color is in gamma sRGB, and that the expected
    /// return should be as well.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>tone mapped color</returns>
    public static Rgb ToneMapHableStandard(in Rgb c)
    {
        return Rgb.LinearToStandard(
            Rgb.ToneMapHableLinear(
                Rgb.StandardToLinear(c)));
    }

    /// <summary>
    /// Returns a string representation of a color.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string</returns>
    public static string ToString(in Rgb c, in int places = 4)
    {
        return Rgb.ToString(new StringBuilder(96), c, places).ToString();
    }

    /// <summary>
    /// Appends a representation of a color to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="c">color</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString(
        in StringBuilder sb,
        in Rgb c,
        in int places = 4)
    {
        sb.Append("{\"r\":");
        Utils.ToFixed(sb, c.r, places);
        sb.Append(",\"g\":");
        Utils.ToFixed(sb, c.g, places);
        sb.Append(",\"b\":");
        Utils.ToFixed(sb, c.b, places);
        sb.Append(",\"alpha\":");
        Utils.ToFixed(sb, c.a, places);
        sb.Append('}');
        return sb;
    }

    /// <summary>
    /// Divides the red, green and blue color channels of a color by the alpha
    /// channel. Reverses pre-multiplication. If alpha is less than or equal to
    /// zero, returns  clear black.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>unpremultiplied color</returns>
    public static Rgb Unpremul(in Rgb c)
    {
        if (c.a <= 0.0f) { return Rgb.ClearBlack; }
        if (c.a >= 1.0f) { return Rgb.Opaque(c); }
        float aInv = 1.0f / c.a;
        return new(
            c.r * aInv,
            c.g * aInv,
            c.b * aInv,
            c.a);
    }

    /// <summary>
    /// Returns the normal direction back as a color,
    /// (0.5, 0.0, 0.5, 1.0).
    /// </summary>
    /// <returns>back</returns>
    public static Rgb Back { get { return new(0.5f, 0.0f, 0.5f, 1.0f); } }

    /// <summary>
    /// Returns the color black, (0.0, 0.0, 0.0, 1.0) .
    /// </summary>
    /// <value>black</value>
    public static Rgb Black { get { return new(0.0f, 0.0f, 0.0f, 1.0f); } }

    /// <summary>
    /// Returns the color blue, (0.0, 0.0, 1.0, 1.0) .
    /// </summary>
    /// <value>blue</value>
    public static Rgb Blue { get { return new(0.0f, 0.0f, 1.0f, 1.0f); } }

    /// <summary>
    /// Returns the color clear black, (0.0, 0.0, 0.0, 0.0) .
    /// </summary>
    /// <value>clear black</value>
    public static Rgb ClearBlack { get { return new(0.0f, 0.0f, 0.0f, 0.0f); } }

    /// <summary>
    /// Returns the color clear white, (1.0, 1.0, 1.0, 0.0) .
    /// </summary>
    /// <value>clear white</value>
    public static Rgb ClearWhite { get { return new(1.0f, 1.0f, 1.0f, 0.0f); } }

    /// <summary>
    /// Returns the color cyan, (0.0, 1.0, 1.0, 1.0) .
    /// </summary>
    /// <value>cyan</value>
    public static Rgb Cyan { get { return new(0.0f, 1.0f, 1.0f, 1.0f); } }

    /// <summary>
    /// Returns the normal direction down as a color,
    /// (0.5, 0.5, 0.0, 1.0).
    /// </summary>
    /// <returns>down</returns>
    public static Rgb Down { get { return new(0.5f, 0.5f, 0.0f, 1.0f); } }

    /// <summary>
    /// Returns the normal direction forward as a color,
    /// (0.5, 1.0, 0.5, 1.0).
    /// </summary>
    /// <returns>forward</returns>
    public static Rgb Forward { get { return new(0.5f, 1.0f, 0.5f, 1.0f); } }

    /// <summary>
    /// Returns the color green, (0.0, 1.0, 0.0, 1.0) .
    /// </summary>
    /// <value>green</value>
    public static Rgb Green { get { return new(0.0f, 1.0f, 0.0f, 1.0f); } }

    /// <summary>
    /// Returns the normal direction left as a color,
    /// (0.0, 0.5, 0.5, 1.0).
    /// </summary>
    /// <returns>left</returns>
    public static Rgb Left { get { return new(0.0f, 0.5f, 0.5f, 1.0f); } }

    /// <summary>
    /// Returns the color magenta, (1.0, 0.0, 1.0, 1.0) .
    /// </summary>
    /// <value>magenta</value>
    public static Rgb Magenta { get { return new(1.0f, 0.0f, 1.0f, 1.0f); } }

    /// <summary>
    /// Returns the color red, (1.0, 0.0, 0.0, 1.0) .
    /// </summary>
    /// <value>red</value>
    public static Rgb Red { get { return new(1.0f, 0.0f, 0.0f, 1.0f); } }

    /// <summary>
    /// Returns the normal direction right as a color,
    /// (1.0, 0.5, 0.5, 1.0).
    /// </summary>
    /// <returns>right</returns>
    public static Rgb Right { get { return new(1.0f, 0.5f, 0.5f, 1.0f); } }

    /// <summary>
    /// Returns the normal direction up as a color,
    /// (0.5, 0.5, 1.0, 1.0).
    /// </summary>
    /// <returns>up</returns>
    public static Rgb Up { get { return new(0.5f, 0.5f, 1.0f, 1.0f); } }

    /// <summary>
    /// Returns the color white, (1.0, 1.0, 1.0, 1.0) .
    /// </summary>
    /// <value>white</value>
    public static Rgb White { get { return new(1.0f, 1.0f, 1.0f, 1.0f); } }

    /// <summary>
    /// Returns the color yellow, (1.0, 1.0, 0.0, 1.0) .
    /// </summary>
    /// <value>yellow</value>
    public static Rgb Yellow { get { return new(1.0f, 1.0f, 0.0f, 1.0f); } }
}