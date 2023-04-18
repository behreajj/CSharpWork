using System;
using System.Text;

/// <summary>
/// A readonly struct to store colors in standard and linear
/// red, green, blue and alpha. Supports conversion to and from
/// integers.
/// </summary>
[Serializable]
public readonly struct Rgb : IComparable<Rgb>, IEquatable<Rgb>
{
    /// <summary>
    /// The alpha (transparency) channel.
    /// </summary>
    private readonly float a;

    /// <summary>
    /// The blue color channel.
    /// </summary>
    private readonly float b;

    /// <summary>
    /// The green color channel.
    /// </summary>
    private readonly float g;

    /// <summary>
    /// The red color channel.
    /// </summary>
    private readonly float r;

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
    /// real number in the  range [0.0, 1.0] .
    /// </summary>
    /// <param name="r">red channel</param>
    /// <param name="g">green channel</param>
    /// <param name="b">blue channel</param>
    /// <param name="a">alpha channel</param>
    public Rgb(in byte r, in byte g, in byte b, in byte a = 255)
    {
        this.r = r * Utils.One255;
        this.g = g * Utils.One255;
        this.b = b * Utils.One255;
        this.a = a * Utils.One255;
    }

    /// <summary>
    /// Creates a color from signed bytes. Converts each to a single precision
    /// real number in the  range [0.0, 1.0] .
    /// </summary>
    /// <param name="r">red channel</param>
    /// <param name="g">green channel</param>
    /// <param name="b">blue channel</param>
    /// <param name="a">alpha channel</param>
    public Rgb(in sbyte r, in sbyte g, in sbyte b, in sbyte a = -1)
    {
        this.r = (r & 0xff) * Utils.One255;
        this.g = (g & 0xff) * Utils.One255;
        this.b = (b & 0xff) * Utils.One255;
        this.a = (a & 0xff) * Utils.One255;
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
        return Rgb.ToHexArgb(this);
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
        int left = Rgb.ToHexArgb(this);
        int right = Rgb.ToHexArgb(c);
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
    /// A color evaluates to false if its alpha channel is less than or equal to
    /// zero.
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
    /// <param name="c">the input color</param>
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
    /// Converts from CIE LAB to standard RGB (SRGB).
    /// The z component of the input vector is
    /// expected to hold the luminance, while x
    /// is expected to hold a and y, b.
    /// </summary>
    /// <param name="lab">CIE LAB color</param>
    /// <returns>sRGB color</returns>
    public static Rgb CieLabToStandard(in Lab lab)
    {
        return Rgb.LinearToStandard(
            Rgb.CieXyzToLinear(
                Rgb.CieLabToCieXyz(lab)));
    }

    /// <summary>
    /// Converts from CIE LAB to CIE XYZ.
    /// The z component of the input vector is
    /// expected to hold the luminance, while x
    /// is expected to hold a and y, b.
    /// </summary>
    /// <param name="lab">CIE LAB color</param>
    /// <returns>CIE XYZ color</returns>
    public static Vec4 CieLabToCieXyz(in Lab lab)
    {
        double offset = 16.0d / 116.0d;
        double one116 = 1.0d / 116.0d;
        double one7787 = 1.0d / 7.787d;

        double a = (lab.L + 16.0d) * one116;
        double b = lab.A * 0.002d + a;
        double c = a - lab.B * 0.005d;

        double acb = a * a * a;
        if (acb > 0.008856d) { a = acb; }
        else { a = (a - offset) * one7787; }

        double bcb = b * b * b;
        if (bcb > 0.008856d) { b = bcb; }
        else { b = (b - offset) * one7787; }

        double ccb = c * c * c;
        if (ccb > 0.008856d) { c = ccb; }
        else { c = (c - offset) * one7787; }

        return new Vec4(
            (float)(b * 0.95047d),
            (float)a,
            (float)(c * 1.08883d),
            lab.Alpha);
    }

    /// <summary>
    /// Converts a color from CIE LCH to standard
    /// RGB (sRGB).
    ///
    /// Luminance is expected to be in [0.0, 100.0].
    /// Chroma is expected to be in [0.0, 135.0].
    /// Hue is expected to be in [0.0, 1.0].
    ///
    /// The input vector is expected to store hue
    /// in the x component; chroma, in the y;
    /// luminance in the z; alpha in the w.
    /// </summary>
    /// <param name="lch">CIE LCH color</param>
    /// <returns>CIE LAB color</returns>
    public static Rgb CieLchToStandard(in Lch lch)
    {
        return Rgb.LinearToStandard(
            Rgb.CieXyzToLinear(
                Rgb.CieLabToCieXyz(
                    Lab.FromLch(lch))));
    }

    /// <summary>
    /// Converts a color from CIE XYZ to linear RGB.
    /// The alpha channel is unaffected by the transformation.
    /// </summary>
    /// <param name="v">CIE XYZ color</param>
    /// <returns>linear color</returns>
    public static Rgb CieXyzToLinear(in Vec4 v)
    {
        return new(
            3.2408123f * v.X - 1.5373085f * v.Y - 0.49858654f * v.Z,
            -0.969243f * v.X + 1.8759663f * v.Y + 0.041555032f * v.Z,
            0.0556384f * v.X - 0.20400746f * v.Y + 1.0571296f * v.Z,
            v.W);
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
    /// Returns the first color argument with the alpha
    /// of the second.
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
    /// Converts a 3D vector to a color, as used in normal maps.
    /// If the vector's magnitude is less than or equal to zero,
    /// returns the color representation of up.
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
    /// <param name="order">color channel order</param>
    /// <returns>color</returns>
    public static Rgb FromHex(in int c, in ClrChannel order = ClrChannel.ARGB)
    {
        switch (order)
        {
            case ClrChannel.ABGR:
                { return Rgb.FromHexAbgr(c); }
            case ClrChannel.RGBA:
                { return Rgb.FromHexRgba(c); }
            case ClrChannel.ARGB:
            default:
                { return Rgb.FromHexArgb(c); }
        }
    }

    /// <summary>
    /// Converts a hexadecimal representation of a color into RGB.
    /// The integer is expected to be ordered as 0xAABBGGRR.
    /// </summary>
    /// <param name="c">integer</param>
    /// <returns>color</returns>
    public static Rgb FromHexAbgr(in int c)
    {
        return new(
            Utils.One255 * (c & 0xff),
            Utils.One255 * (c >> 0x08 & 0xff),
            Utils.One255 * (c >> 0x10 & 0xff),
            Utils.One255 * (c >> 0x18 & 0xff));
    }

    /// <summary>
    /// Converts a hexadecimal representation of a color into RGB.
    /// The integer is expected to be ordered as 0xAARRGGBB.
    /// </summary>
    /// <param name="c">integer</param>
    /// <returns>color</returns>
    public static Rgb FromHexArgb(in int c)
    {
        return new(
            Utils.One255 * (c >> 0x10 & 0xff),
            Utils.One255 * (c >> 0x08 & 0xff),
            Utils.One255 * (c & 0xff),
            Utils.One255 * (c >> 0x18 & 0xff));
    }

    /// <summary>
    /// Converts a hexadecimal representation of a color into RGB.
    /// The integer is expected to be ordered as 0xRRGGBBAA.
    /// </summary>
    /// <param name="c">integer</param>
    /// <returns>color</returns>
    public static Rgb FromHexRgba(in int c)
    {
        return new(
            Utils.One255 * (c >> 0x18 & 0xff),
            Utils.One255 * (c >> 0x10 & 0xff),
            Utils.One255 * (c >> 0x08 & 0xff),
            Utils.One255 * (c & 0xff));
    }

    /// <summary>
    /// Generates a 3D array of colors representing
    /// a grid in standard RGB.
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
        int lval = layers < 2 ? 2 : layers;
        int rval = rows < 2 ? 2 : rows;
        int cval = cols < 2 ? 2 : cols;

        float hToStep = 1.0f / (lval - 1.0f);
        float iToStep = 1.0f / (rval - 1.0f);
        float jToStep = 1.0f / (cval - 1.0f);

        Rgb[,,] result = new Rgb[lval, rval, cval];

        int rcval = rval * cval;
        int len3 = lval * rcval;
        for (int k = 0; k < len3; ++k)
        {
            int h = k / rcval;
            int m = k - h * rcval;
            int i = m / cval;
            int j = m % cval;

            result[h, i, j] = new(
                j * jToStep,
                i * iToStep,
                h * hToStep,
                alpha);
        }

        return result;
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
    /// Converts a color from linear RGB to CIE XYZ.
    /// The alpha channel is unaffected by the transformation.
    /// </summary>
    /// <param name="c">linear color</param>
    /// <returns>XYZ color</returns>
    public static Vec4 LinearToCieXyz(in Rgb c)
    {
        return new Vec4(
            0.41241086f * c.r + 0.35758457f * c.g + 0.1804538f * c.b,
            0.21264935f * c.r + 0.71516913f * c.g + 0.07218152f * c.b,
            0.019331759f * c.r + 0.11919486f * c.g + 0.95039004f * c.b,
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
        float inv24 = 1.0f / 2.4f;
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
    /// Converts each color from sRGB to CIE LAB, mixes according
    /// to the step, then converts back to sRGB.
    /// </summary>
    /// <param name="o">origin color</param>
    /// <param name="d">destination color</param>
    /// <param name="t">step</param>
    /// <returns>mixed color</returns>
    public static Rgb MixCieLab(in Rgb o, in Rgb d, in float t = 0.5f)
    {
        Lab oLab = Rgb.StandardToCieLab(o);
        Lab dLab = Rgb.StandardToCieLab(d);
        return Rgb.CieLabToStandard(Lab.Mix(oLab, dLab, t));
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
    public static Rgb MixCieLch(in Rgb o, in Rgb d, in float t = 0.5f)
    {
        return Rgb.MixCieLch(o, d, t,
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
    public static Rgb MixCieLch(
        in Rgb o,
        in Rgb d,
        in float t,
        in Func<float, float, float, float, float> easing)
    {
        Lab oLab = Rgb.StandardToCieLab(o);
        float oa = oLab.A;
        float ob = oLab.B;
        float oChrSq = oa * oa + ob * ob;

        Lab dLab = Rgb.StandardToCieLab(d);
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

        return Rgb.CieLabToStandard(cLab);
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
        if (c.a >= 1.0f) { return new(c.r, c.g, c.b, 1.0f); }
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
    public static Rgb QuantizeSigned(in Rgb c, in int levels)
    {
        return Rgb.QuantizeSigned(c, levels, levels, levels, levels);
    }

    /// <summary>
    /// Reduces the signal, or granularity, of a color's red, green
    /// and blue channels. Does not alter the alpha channel.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="rLevels">red levels</param>
    /// <param name="gLevels">green levels</param>
    /// <param name="bLevels">blue levels</param>
    /// <returns>posterized color</returns>
    public static Rgb QuantizeSigned(
        in Rgb c,
        in int rLevels,
        in int gLevels,
        in int bLevels)
    {
        return new(
            Utils.QuantizeSigned(c.r, rLevels),
            Utils.QuantizeSigned(c.g, gLevels),
            Utils.QuantizeSigned(c.b, bLevels), c.a);
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
    public static Rgb QuantizeSigned(
        in Rgb c,
        in int rLevels,
        in int gLevels,
        in int bLevels,
        in int aLevels)
    {
        return new(
            Utils.QuantizeSigned(c.r, rLevels),
            Utils.QuantizeSigned(c.g, gLevels),
            Utils.QuantizeSigned(c.b, bLevels),
            Utils.QuantizeSigned(c.a, aLevels));
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
    /// Reduces the signal, or granularity, of a color's red, green
    /// and blue channels. Does not alter the alpha channel.
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
    /// Converts a color from standard RGB (sRGB) to CIE LAB.
    /// Stores alpha in the w component, luminance in the z
    /// component, a in the x component and b in the y component.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>CIE LAB color</returns>
    public static Lab StandardToCieLab(in Rgb c)
    {
        return Lab.FromCieXyz(
            Rgb.LinearToCieXyz(
                Rgb.StandardToLinear(c)));
    }

    /// <summary>
    /// Converts a color from standard RGB (sRGB)
    /// to CIE LCH.
    ///
    /// In the return vector, hue is assigned to
    /// the x component; chroma, to y; luminance,
    /// to z; alpha, to w.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>CIE LCH color</returns>
    public static Lch StandardToCieLch(in Rgb c)
    {
        return Lch.FromLab(
            Lab.FromCieXyz(
                Rgb.LinearToCieXyz(
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
        float inv1055 = 1.0f / 1.055f;
        return new(
            c.r > 0.04045f ?
            MathF.Pow((c.r + 0.055f) * inv1055, 2.4f) :
            c.r * 0.07739938f,

            c.g > 0.04045f ?
            MathF.Pow((c.g + 0.055f) * inv1055, 2.4f) :
            c.g * 0.07739938f,

            c.b > 0.04045f ?
            MathF.Pow((c.b + 0.055f) * inv1055, 2.4f) :
            c.b * 0.07739938f,

            alpha ? c.a > 0.04045f ?
            MathF.Pow((c.a + 0.055f) * inv1055, 2.4f) :
            c.a * 0.07739938f :
            c.a);
    }

    /// <summary>
    /// Converts a color to an integer.
    /// </summary>
    /// <param name="c">the input color</param>
    /// <param name="order">color channel order</param>
    /// <returns>hexadecimal color</returns>
    public static int ToHex(in Rgb c, in ClrChannel order = ClrChannel.ARGB)
    {
        switch (order)
        {
            case ClrChannel.ABGR:
                { return Rgb.ToHexAbgr(c); }
            case ClrChannel.RGBA:
                { return Rgb.ToHexRgba(c); }
            case ClrChannel.ARGB:
            default:
                { return Rgb.ToHexArgb(c); }
        }
    }

    /// <summary>
    /// Converts a color to an integer. Returns an integer ordered as 0xAABBGGRR.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>hexadecimal color</returns>
    public static int ToHexAbgr(in Rgb c)
    {
        return Rgb.ToHexAbgrUnchecked(Rgb.Clamp(c, 0.0f, 1.0f));
    }

    /// <summary>
    /// Converts a color to an integer. Returns an integer ordered as 0xAARRGGBB.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>hexadecimal color</returns>
    public static int ToHexArgb(in Rgb c)
    {
        return Rgb.ToHexArgbUnchecked(Rgb.Clamp(c));
    }

    /// <summary>
    /// Converts a color to an integer. Returns an integer ordered as 0xRRGGBBAA.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>hexadecimal color</returns>
    public static int ToHexRgba(in Rgb c)
    {
        return Rgb.ToHexRgbaUnchecked(Rgb.Clamp(c));
    }

    /// <summary>
    /// Converts a color to an integer. Does not check if the color's components
    /// are in a valid range, [0.0, 1.0].
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="order">color channel order</param>
    /// <returns>hexadecimal color</returns>
    public static int ToHexUnchecked(in Rgb c, in ClrChannel order = ClrChannel.ARGB)
    {
        switch (order)
        {
            case ClrChannel.ABGR:
                { return Rgb.ToHexAbgrUnchecked(c); }
            case ClrChannel.RGBA:
                { return Rgb.ToHexRgbaUnchecked(c); }
            case ClrChannel.ARGB:
            default:
                { return Rgb.ToHexArgbUnchecked(c); }
        }
    }

    /// <summary>
    /// Converts a color to an integer. Does not check if the color's components
    /// are in a valid range, [0.0, 1.0]. Returns an integer ordered as 0xAABBGGRR.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>hexadecimal color</returns>
    public static int ToHexAbgrUnchecked(in Rgb c)
    {
        return (int)(c.a * 255.0f + 0.5f) << 0x18 |
               (int)(c.b * 255.0f + 0.5f) << 0x10 |
               (int)(c.g * 255.0f + 0.5f) << 0x08 |
               (int)(c.r * 255.0f + 0.5f);
    }

    /// <summary>
    /// Converts a color to an integer. Does not check if the color's components
    /// are in a valid range, [0.0, 1.0]. Returns an integer ordered as 0xAARRGGBB.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>hexadecimal color</returns>
    public static int ToHexArgbUnchecked(in Rgb c)
    {
        return (int)(c.a * 255.0f + 0.5f) << 0x18 |
               (int)(c.r * 255.0f + 0.5f) << 0x10 |
               (int)(c.g * 255.0f + 0.5f) << 0x08 |
               (int)(c.b * 255.0f + 0.5f);
    }

    /// <summary>
    /// Converts a color to an integer. Does not check if the color's components
    /// are in a valid range, [0.0, 1.0]. Returns an integer ordered as 0xRRGGBBAA.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>hexadecimal color</returns>
    public static int ToHexRgbaUnchecked(in Rgb c)
    {
        return (int)(c.r * 255.0f + 0.5f) << 0x18 |
               (int)(c.g * 255.0f + 0.5f) << 0x10 |
               (int)(c.b * 255.0f + 0.5f) << 0x08 |
               (int)(c.a * 255.0f + 0.5f);
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
        return Rgb.ToHexWeb(new StringBuilder(8), c).ToString();
    }

    /// <summary>
    /// Appends a string representation of a color in web-friendly hexdecimal
    /// to a string builder. Does not prepend a hash tag. Clamps all values
    /// to [0.0, 1.0] before converting to a byte.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="c">color</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToHexWeb(in StringBuilder sb, in Rgb c)
    {
        int r = (int)(Utils.Clamp(c.r, 0.0f, 1.0f) * 255.0f + 0.5f);
        int g = (int)(Utils.Clamp(c.g, 0.0f, 1.0f) * 255.0f + 0.5f);
        int b = (int)(Utils.Clamp(c.b, 0.0f, 1.0f) * 255.0f + 0.5f);
        sb.AppendFormat("{0:X6}", r << 0x10 | g << 0x08 | b);
        return sb;
    }

    /// <summary>
    /// For colors which exceed the range [0.0, 1.0], applies ACES
    /// tone mapping algorithm. See https://64.github.io/tonemapping/ .
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>tone mapped color</returns>
    public static Rgb ToneMapAcesLinear(in Rgb c)
    {
        float rFrwrd = 0.59719f * c.r + 0.35458f * c.g + 0.04823f * c.b;
        float gFrwrd = 0.07600f * c.r + 0.90834f * c.g + 0.01566f * c.b;
        float bFrwrd = 0.02840f * c.r + 0.13383f * c.g + 0.83777f * c.b;

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
    /// For colors which exceed the range [0.0, 1.0], applies ACES
    /// tone mapping algorithm. See https://64.github.io/tonemapping/ .
    /// Assumes that the input color is in gamma sRGB, and that the
    /// expected return of should be as well.
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
    /// For colors which exceed the range [0.0, 1.0], applies
    /// Uncharted 2, or Hable, tone mapping algorithm. See
    /// https://64.github.io/tonemapping/ .
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>tone mapped color</returns>
    public static Rgb ToneMapHableLinear(in Rgb c)
    {
        float A = 0.15f;
        float B = 0.50f;
        float C = 0.10f;
        float D = 0.20f;
        float E = 0.02f;
        float F = 0.30f;
        float W = 11.2f;
        float whiteScale = 1.0f / (((W * (A * W + C * B) + D * E)
            / (W * (A * W + B) + D * F)) - E / F);

        float exposureBias = 2.0f;
        float er = c.r * exposureBias;
        float eg = c.g * exposureBias;
        float eb = c.b * exposureBias;

        float xr = ((er * (A * er + C * B) + D * E) / (er * (A * er + B) + D * F)) - E / F;
        float xg = ((eg * (A * eg + C * B) + D * E) / (eg * (A * eg + B) + D * F)) - E / F;
        float xb = ((eb * (A * eb + C * B) + D * E) / (eb * (A * eb + B) + D * F)) - E / F;

        return new(
            Utils.Clamp(xr * whiteScale, 0.0f, 1.0f),
            Utils.Clamp(xg * whiteScale, 0.0f, 1.0f),
            Utils.Clamp(xb * whiteScale, 0.0f, 1.0f),
            Utils.Clamp(c.a, 0.0f, 1.0f));
    }

    /// <summary>
    /// For colors which exceed the range [0.0, 1.0], applies
    /// Uncharted 2, or Hable, tone mapping algorithm. See
    /// https://64.github.io/tonemapping/ . Assumes that the
    /// input color is in gamma sRGB, and that the
    /// expected return of should be as well.
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
        sb.Append("{ r: ");
        Utils.ToFixed(sb, c.r, places);
        sb.Append(", g: ");
        Utils.ToFixed(sb, c.g, places);
        sb.Append(", b: ");
        Utils.ToFixed(sb, c.b, places);
        sb.Append(", a: ");
        Utils.ToFixed(sb, c.a, places);
        sb.Append(' ');
        sb.Append('}');
        return sb;
    }

    /// <summary>
    /// Divides the red, green and blue color channels of a color by the
    /// alpha channel. Reverses pre-multiplication. If alpha is less than
    /// or equal to zero, returns  clear black.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>unpremultiplied color</returns>
    public static Rgb Unpremul(in Rgb c)
    {
        if (c.a <= 0.0f) { return Rgb.ClearBlack; }
        if (c.a >= 1.0f) { return new(c.r, c.g, c.b, 1.0f); }
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