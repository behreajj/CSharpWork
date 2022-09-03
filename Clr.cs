using System;
using System.Text;

/// <summary>
/// A readonly struct. Supports conversion
/// to and from integers where color channels are in the format 0xAARRGGBB.
/// </summary>
[Serializable]
public readonly struct Clr : IComparable<Clr>, IEquatable<Clr>
{
    /// <summary>
    /// Arbitrary hue in LCh assigned to desaturated colors closer to daylight.
    /// Roughly 99.0 / 360.0 degrees.
    /// </summary>
    public const float LchHueDay = 0.275f;

    /// <summary>
    /// Arbitrary hue in LCh assigned to desaturated colors that are closer to shadow.
    /// Roughly 308.0 / 360.0 degrees.
    /// </summary>
    public const float LchHueShade = 0.85555553f;

    /// <summary>
    /// The red color channel.
    /// </summary>
    private readonly float _r;

    /// <summary>
    /// The green color channel.
    /// </summary>
    private readonly float _g;

    /// <summary>
    /// The blue color channel.
    /// </summary>
    private readonly float _b;

    /// <summary>
    /// The alpha (transparency) channel.
    /// </summary>
    private readonly float _a;

    /// <summary>
    /// Returns the number of elements in this color.
    /// </summary>
    /// <value>length</value>
    public int Length { get { return 4; } }

    /// <summary>
    /// The red color channel.
    /// </summary>
    /// <value>red</value>
    public float r { get { return this._r; } }

    /// <summary>
    /// The green color channel.
    /// </summary>
    /// <value>green</value>
    public float g { get { return this._g; } }

    /// <summary>
    /// The blue color channel.
    /// </summary>
    /// <value>blue</value>
    public float b { get { return this._b; } }

    /// <summary>
    /// The alpha color channel.
    /// </summary>
    /// <value>alpha</value>
    public float a { get { return this._a; } }

    /// <summary>
    /// Creates a color from unsigned bytes. Converts each to a single precision
    /// real number in the  range [0.0, 1.0] .
    /// </summary>
    /// <param name="r">red channel</param>
    /// <param name="g">green channel</param>
    /// <param name="b">blue channel</param>
    /// <param name="a">alpha channel</param>
    public Clr(in byte r = 255, in byte g = 255, in byte b = 255, in byte a = 255)
    {
        this._r = r * Utils.One255;
        this._g = g * Utils.One255;
        this._b = b * Utils.One255;
        this._a = a * Utils.One255;
    }

    /// <summary>
    /// Creates a color from unsigned bytes. Converts each to a single precision
    /// real number in the  range [0.0, 1.0] .
    /// </summary>
    /// <param name="r">red channel</param>
    /// <param name="g">green channel</param>
    /// <param name="b">blue channel</param>
    /// <param name="a">alpha channel</param>
    public Clr(in sbyte r = -1, in sbyte g = -1, in sbyte b = -1, in sbyte a = -1)
    {
        this._r = (((int)r) & 0xff) * Utils.One255;
        this._g = (((int)g) & 0xff) * Utils.One255;
        this._b = (((int)b) & 0xff) * Utils.One255;
        this._a = (((int)a) & 0xff) * Utils.One255;
    }

    /// <summary>
    /// Creates a color from single precision real numbers. Clamps values to a
    /// range [0.0, 1.0] .
    /// </summary>
    /// <param name="r">red channel</param>
    /// <param name="g">green channel</param>
    /// <param name="b">blue channel</param>
    /// <param name="a">alpha channel</param>
    public Clr(in float r = 1.0f, in float g = 1.0f, in float b = 1.0f, in float a = 1.0f)
    {
        this._r = r;
        this._g = g;
        this._b = b;
        this._a = a;
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
        if (value is Clr) { return this.Equals((Clr)value); }
        return false;
    }

    /// <summary>
    /// Returns a hash code representing this color.
    /// </summary>
    /// <returns>hash code</returns>
    public override int GetHashCode()
    {
        return Clr.ToHexArgb(this);
    }

    /// <summary>
    /// Returns a string representation of this color.
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()
    {
        return Clr.ToString(this);
    }

    /// <summary>
    /// Compares this color to another in compliance with the IComparable
    /// interface.
    /// </summary>
    /// <param name="c">comparisand</param>
    /// <returns>evaluation</returns>
    public int CompareTo(Clr c)
    {
        int left = Clr.ToHexArgb(this);
        int right = Clr.ToHexArgb(c);
        return (left < right) ? -1 : (left > right) ? 1 : 0;
    }

    /// <summary>
    /// Tests this color for equivalence with another in compliance with the
    /// IEquatable interface.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>equivalence</returns>
    public bool Equals(Clr c)
    {
        return Clr.EqSatArith(this, c);
    }

    /// <summary>
    /// Converts a color to a boolean by returning whether its alpha is greater
    /// than zero.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>evaluation</returns>
    public static explicit operator bool(in Clr c)
    {
        return Clr.Any(c);
    }

    /// <summary>
    /// A color evaluates to true if its alpha channel is greater than zero.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>evaluation</returns>
    public static bool operator true(in Clr c)
    {
        return Clr.Any(c);
    }

    /// <summary>
    /// A color evaluates to false if its alpha channel is less than or equal to
    /// zero.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>evaluation</returns>
    public static bool operator false(in Clr c)
    {
        return Clr.None(c);
    }

    /// <summary>
    /// Converts a color to an integer, performs the bitwise not operation on
    /// it, then converts the result to a color.
    /// </summary>
    /// <param name="c">the input color</param>
    /// <returns>the negated color</returns>
    public static Clr operator ~(in Clr c)
    {
        return Clr.FromHexArgb(~Clr.ToHexArgb(c));
    }

    /// <summary>
    /// Converts two colors to integers, performs the bitwise and operation on
    /// them, then converts the result to a color.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the color</returns>
    public static Clr operator &(in Clr a, in Clr b)
    {
        return Clr.FromHexArgb(Clr.ToHexArgb(a) & Clr.ToHexArgb(b));
    }

    /// <summary>
    /// Converts two colors to integers, performs the bitwise inclusive or
    /// operation on them, then converts the result to a color.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the color</returns>
    public static Clr operator |(in Clr a, in Clr b)
    {
        return Clr.FromHexArgb(Clr.ToHexArgb(a) | Clr.ToHexArgb(b));
    }

    /// <summary>
    /// Converts two colors to integers, performs the bitwise exclusive or
    /// operation on them, then converts the result to a color.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the color</returns>
    public static Clr operator ^(in Clr a, in Clr b)
    {
        return Clr.FromHexArgb(Clr.ToHexArgb(a) ^ Clr.ToHexArgb(b));
    }

    /// <summary>
    /// Tests to see if all color channels are greater than zero.
    /// </summary>
    /// <param name="c">the color</param>
    /// <returns>evaluation</returns>
    public static bool All(in Clr c)
    {
        return (c._a > 0.0f) &&
            (c._r > 0.0f) &&
            (c._g > 0.0f) &&
            (c._b > 0.0f);
    }

    /// <summary>
    /// Tests to see if the alpha channel of the color is greater than zero,
    /// i.e., if it has some opacity.
    /// </summary>
    /// <param name="c">the color</param>
    /// <returns>evaluation</returns>
    public static bool Any(in Clr c)
    {
        return c._a > 0.0f;
    }

    /// <summary>
    /// Clamps a color to a lower and upper bound.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="lb">lower bound</param>
    /// <param name="ub">upper bound</param>
    /// <returns>the clamped color</returns>
    public static Clr Clamp(in Clr c, in float lb = 0.0f, in float ub = 1.0f)
    {
        return new Clr(
            Utils.Clamp(c._r, lb, ub),
            Utils.Clamp(c._g, lb, ub),
            Utils.Clamp(c._b, lb, ub),
            Utils.Clamp(c._a, lb, ub));
    }

    /// <summary>
    /// Tests to see if a color contains a value.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="v">value</param>
    /// <returns>evaluation</returns>
    public static bool Contains(in Clr c, in float v)
    {
        return Utils.Approx(c._a, v) ||
            Utils.Approx(c._b, v) ||
            Utils.Approx(c._g, v) ||
            Utils.Approx(c._r, v);
    }

    /// <summary>
    /// Checks if two colors have equivalent alpha channels when converted to
    /// bytes in [0, 255]. Uses saturation arithmetic.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static bool EqAlphaSatArith(in Clr a, in Clr b)
    {
        return (int)(Utils.Clamp(a._a, 0.0f, 1.0f) * 255.0f + 0.5f) ==
               (int)(Utils.Clamp(b._a, 0.0f, 1.0f) * 255.0f + 0.5f);
    }

    /// <summary>
    /// Checks if two colors have equivalent red, green and blue channels when
    /// converted to bytes in [0, 255]. Uses saturation arithmetic.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static bool EqRgbSatArith(in Clr a, in Clr b)
    {
        return (int)(Utils.Clamp(a._b, 0.0f, 1.0f) * 255.0f + 0.5f) ==
               (int)(Utils.Clamp(b._b, 0.0f, 1.0f) * 255.0f + 0.5f) &&
               (int)(Utils.Clamp(a._g, 0.0f, 1.0f) * 255.0f + 0.5f) ==
               (int)(Utils.Clamp(b._g, 0.0f, 1.0f) * 255.0f + 0.5f) &&
               (int)(Utils.Clamp(a._r, 0.0f, 1.0f) * 255.0f + 0.5f) ==
               (int)(Utils.Clamp(b._r, 0.0f, 1.0f) * 255.0f + 0.5f);
    }

    /// <summary>
    /// Checks if two colors have equivalent red, green, blue and alph
    /// channels when converted to bytes in [0, 255]. Uses saturation
    /// arithmetic.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static bool EqSatArith(in Clr a, in Clr b)
    {
        return Clr.EqAlphaSatArith(a, b) &&
            Clr.EqRgbSatArith(a, b);
    }

    /// <summary>
    /// Converts a 3D vector to a color, as used in normal maps.
    /// If the vector's magnitude is less than or equal to zero,
    /// returns the color representation of up.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>color</returns>
    public static Clr FromDir(in Vec3 v)
    {
        float mSq = Vec3.MagSq(v);
        if (mSq > 0.0f)
        {
            float mInv = 0.5f * Utils.InvSqrtUnchecked(mSq);
            return new Clr(
                v.x * mInv + 0.5f,
                v.y * mInv + 0.5f,
                v.z * mInv + 0.5f,
                1.0f);
        }
        return Clr.Up;
    }

    /// <summary>
    /// Converts a hexadecimal representation of a color into a color.
    /// </summary>
    /// <param name="c">integer</param>
    /// <param name="order">color channel order</param>
    /// <returns>color</returns>
    public static Clr FromHex(in int c, in ColorChannel order = ColorChannel.ARGB)
    {
        switch (order)
        {
            case ColorChannel.ABGR:
                { return Clr.FromHexAbgr(c); }
            case ColorChannel.RGBA:
                { return Clr.FromHexRgba(c); }
            case ColorChannel.ARGB:
            default:
                { return Clr.FromHexArgb(c); }
        }
    }

    /// <summary>
    /// Converts a hexadecimal representation of a color into a color.
    /// The integer is expected to be ordered as 0xAABBGGRR.
    /// </summary>
    /// <param name="c">integer</param>
    /// <returns>color</returns>
    public static Clr FromHexAbgr(in int c)
    {
        return new Clr(
            Utils.One255 * (c & 0xff),
            Utils.One255 * (c >> 0x08 & 0xff),
            Utils.One255 * (c >> 0x10 & 0xff),
            Utils.One255 * (c >> 0x18 & 0xff));
    }

    /// <summary>
    /// Converts a hexadecimal representation of a color into a color.
    /// The integer is expected to be ordered as 0xAARRGGBB.
    /// </summary>
    /// <param name="c">integer</param>
    /// <returns>color</returns>
    public static Clr FromHexArgb(in int c)
    {
        return new Clr(
            Utils.One255 * (c >> 0x10 & 0xff),
            Utils.One255 * (c >> 0x08 & 0xff),
            Utils.One255 * (c & 0xff),
            Utils.One255 * (c >> 0x18 & 0xff));
    }

    /// <summary>
    /// Converts a hexadecimal representation of a color into a color.
    /// The integer is expected to be ordered as 0xRRGGBBAA.
    /// </summary>
    /// <param name="c">integer</param>
    /// <returns>color</returns>
    public static Clr FromHexRgba(in int c)
    {
        return new Clr(
            Utils.One255 * (c >> 0x18 & 0xff),
            Utils.One255 * (c >> 0x10 & 0xff),
            Utils.One255 * (c >> 0x08 & 0xff),
            Utils.One255 * (c & 0xff));
    }

    /// <summary>
    /// Converts a color from CIE LAB to CIE LCH.
    /// The z component of the input vector is
    /// expected to hold the luminance, while x
    /// is expected to hold a and y, b.
    ///
    /// In the return vector, hue is assigned to
    /// the x component; chroma, to y; luminance,
    /// to z; alpha, to w.
    /// </summary>
    /// <param name="lab">Lab color</param>
    /// <returns>LCh color</returns>
    public static Vec4 LabaToLcha(in Vec4 lab)
    {
        float a = lab.x;
        float b = lab.y;
        float chromaSq = a * a + b * b;
        if (chromaSq < Utils.Epsilon)
        {
            float fac = Utils.Clamp(lab.z * 0.01f, 0.0f, 1.0f);
            float hue = Utils.LerpAngleNear(
                Clr.LchHueShade, Clr.LchHueDay, fac, 1.0f);
            return new Vec4(hue, 0.0f, lab.z, lab.w);
        }
        else
        {
            return new Vec4(
                Utils.OneTau * Utils.WrapRadians(MathF.Atan2(b, a)),
                MathF.Sqrt(chromaSq), lab.z, lab.w);
        }
    }

    /// <summary>
    /// Converts from CIE LAB to standard RGB (SRGB).
    /// The z component of the input vector is
    /// expected to hold the luminance, while x
    /// is expected to hold a and y, b.
    /// </summary>
    /// <param name="lab">lab color</param>
    /// <returns>color</returns>
    public static Clr LabaToStandard(in Vec4 lab)
    {
        return Clr.LinearToStandard(
            Clr.XyzaToLinear(
                Clr.LabaToXyza(lab)));
    }

    /// <summary>
    /// Converts from CIE LAB to CIE XYZ.
    /// The z component of the input vector is
    /// expected to hold the luminance, while x
    /// is expected to hold a and y, b.
    /// </summary>
    /// <param name="lab">lab color</param>
    /// <returns>xyz color</returns>
    public static Vec4 LabaToXyza(in Vec4 lab)
    {
        double offset = 16.0d / 116.0d;
        double one116 = 1.0d / 116.0d;
        double one7787 = 1.0d / 7.787d;

        double a = (lab.z + 16.0d) * one116;
        double b = lab.x * 0.002d + a;
        double c = a - lab.y * 0.005d;

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
            lab.w);
    }

    /// <summary>
    /// Converts a color from CIE LCH to CIE LAB.
    ///
    /// Luminance is expected to be in [0.0, 100.0].
    /// Chroma is expected to be in [0.0, 135.0].
    /// Hue is expected to be in [0.0, 1.0].
    ///
    /// The input vector is expected to store hue
    /// in the x component; chroma, in the y;
    /// luminance in the z; alpha in the w.
    /// </summary>
    /// <param name="lch">LCh color</param>
    /// <returns>Lab color</returns>
    public static Vec4 LchaToLaba(in Vec4 lch)
    {
        float hRad = lch.x * Utils.Tau;
        float chroma = MathF.Max(0.0f, lch.y);
        return new Vec4(
            chroma * MathF.Cos(hRad),
            chroma * MathF.Sin(hRad),
            lch.z,
            lch.w);
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
    /// <param name="lch">LCh color</param>
    /// <returns>Lab color</returns>
    public static Clr LchaToStandard(in Vec4 lch)
    {
        return Clr.LinearToStandard(
            Clr.XyzaToLinear(
                Clr.LabaToXyza(
                    Clr.LchaToLaba(lch))));
    }

    /// <summary>
    /// Returns the relative luminance of the color.
    /// Assumes the color is in linear RGB.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>luminance</returns>
    public static float LinearLuminance(in Clr c)
    {
        return 0.21264935f * c._r +
               0.71516913f * c._g +
               0.07218152f * c._b;
    }

    /// <summary>
    /// Converts a color from linear RGB to standard RGB (sRGB).
    /// </summary>
    /// <param name="c">linear color</param>
    /// <param name="alpha">transform the alpha channel</param>
    /// <returns>standard color</returns>
    public static Clr LinearToStandard(in Clr c, in bool alpha = false)
    {
        float inv24 = 1.0f / 2.4f;
        return new Clr(
            c._r > 0.0031308f ?
            MathF.Pow(c._r, inv24) * 1.055f - 0.055f :
            c._r * 12.92f,

            c._g > 0.0031308f ?
            MathF.Pow(c._g, inv24) * 1.055f - 0.055f :
            c._g * 12.92f,

            c._b > 0.0031308f ?
            MathF.Pow(c._b, inv24) * 1.055f - 0.055f :
            c._b * 12.92f,

            alpha ? c._a > 0.0031308f ?
            MathF.Pow(c._a, inv24) * 1.055f - 0.055f :
            c._a * 12.92f :
            c._a);
    }

    /// <summary>
    /// Converts a color from linear RGB to CIE XYZ.
    /// The alpha channel is unaffected by the transformation.
    /// </summary>
    /// <param name="c">linear color</param>
    /// <returns>XYZ color</returns>
    public static Vec4 LinearToXyza(in Clr c)
    {
        return new Vec4(
            0.41241086f * c._r + 0.35758457f * c._g + 0.1804538f * c._b,
            0.21264935f * c._r + 0.71516913f * c._g + 0.07218152f * c._b,
            0.019331759f * c._r + 0.11919486f * c._g + 0.95039004f * c._b,
            c._a);
    }

    /// <summary>
    /// Mixes two colors by a step in the range [0.0, 1.0] .
    /// Converts each color from sRGB to CIE LAB, mixes according
    /// to the step, then converts back to sRGB.
    /// </summary>
    /// <param name="a">origin color</param>
    /// <param name="b">destination color</param>
    /// <param name="t">step</param>
    /// <returns>mixed color</returns>
    public static Clr MixLaba(in Clr a, in Clr b, in float t = 0.5f)
    {
        Clr aLin = Clr.StandardToLinear(a);
        Vec4 aXyz = Clr.LinearToXyza(aLin);
        Vec4 aLab = Clr.XyzaToLaba(aXyz);

        Clr bLin = Clr.StandardToLinear(b);
        Vec4 bXyz = Clr.LinearToXyza(bLin);
        Vec4 bLab = Clr.XyzaToLaba(bXyz);

        Vec4 cLab = Vec4.Mix(aLab, bLab, t);
        Vec4 cXyz = Clr.LabaToXyza(cLab);
        Clr cLin = Clr.XyzaToLinear(cXyz);

        return Clr.LinearToStandard(cLin);
    }

    /// <summary>
    /// Mixes two colors by a step in the range [0.0, 1.0] .
    /// Converts each color from sRGB to CIE LCH, mixes according
    /// to the step, then converts back to sRGB.
    /// </summary>
    /// <param name="a">origin color</param>
    /// <param name="b">destination color</param>
    /// <param name="t">step</param>
    /// <returns>mixed color</returns>
    public static Clr MixLcha(in Clr a, in Clr b, in float t = 0.5f)
    {
        return Clr.MixLcha(a, b, t, (x, y, z, w) => Utils.LerpAngleNear(x, y, z, w));
    }

    /// <summary>
    /// Mixes two colors by a step in the range [0.0, 1.0] .
    /// Converts each color from sRGB to CIE LCH, mixes according
    /// to the step, then converts back to sRGB.
    /// The easing function is expected to ease from an origin
    /// hue to a destination by a factor according to a range.
    /// </summary>
    /// <param name="a">origin color</param>
    /// <param name="b">destination color</param>
    /// <param name="t">step</param>
    /// <param name="easing">easing function</param>
    /// <returns>mixed color</returns>
    public static Clr MixLcha(
        in Clr a,
        in Clr b,
        in float t,
        in Func<float, float, float, float, float> easing)
    {
        Clr aLin = Clr.StandardToLinear(a);
        Vec4 aXyz = Clr.LinearToXyza(aLin);
        Vec4 aLab = Clr.XyzaToLaba(aXyz);
        float aa = aLab.x;
        float ab = aLab.y;
        float aChrSq = aa * aa + ab * ab;

        Clr bLin = Clr.StandardToLinear(b);
        Vec4 bXyz = Clr.LinearToXyza(bLin);
        Vec4 bLab = Clr.XyzaToLaba(bXyz);
        float ba = bLab.x;
        float bb = bLab.y;
        float bChrSq = ba * ba + bb * bb;

        Vec4 cLab;

        if (aChrSq < Utils.Epsilon || bChrSq < Utils.Epsilon)
        {
            cLab = Vec4.Mix(aLab, bLab, t);
        }
        else
        {
            float aChr = MathF.Sqrt(aChrSq);
            float aHue = Utils.OneTau * Utils.WrapRadians(
                MathF.Atan2(ab, aa));

            float bChr = MathF.Sqrt(bChrSq);
            float bHue = Utils.OneTau * Utils.WrapRadians(
                MathF.Atan2(bb, ba));

            float u = 1.0f - t;
            Vec4 cLch = new Vec4(
                easing(aHue, bHue, t, 1.0f),
                u * aChr + t * bChr,
                u * aLab.z + t * bLab.z,
                u * aLab.w + t * bLab.w);
            cLab = Clr.LchaToLaba(cLch);
        }

        Vec4 cXyz = Clr.LabaToXyza(cLab);
        Clr cLin = Clr.XyzaToLinear(cXyz);
        return Clr.LinearToStandard(cLin);
    }

    /// <summary>
    /// Mixes two colors by a step in the range [0.0, 1.0] .
    /// Assumes the colors are in standard RGB; converts them to
    /// linear, then mixes them, then converts to standard.
    /// </summary>
    /// <param name="a">origin color</param>
    /// <param name="b">destination color</param>
    /// <param name="t">step</param>
    /// <returns>mixed color</returns>
    public static Clr MixRgbaLinear(in Clr a, in Clr b, in float t = 0.5f)
    {
        return Clr.LinearToStandard(
            Clr.MixRgbaStandard(
                Clr.StandardToLinear(a),
                Clr.StandardToLinear(b), t));
    }

    /// <summary>
    /// Mixes two colors by a step in the range [0.0, 1.0] .
    /// </summary>
    /// <param name="a">origin color</param>
    /// <param name="b">destination color</param>
    /// <param name="t">step</param>
    /// <returns>mixed color</returns>
    public static Clr MixRgbaStandard(in Clr a, in Clr b, in float t = 0.5f)
    {
        float u = 1.0f - t;
        return new Clr(
            u * a._r + t * b._r,
            u * a._g + t * b._g,
            u * a._b + t * b._b,
            u * a._a + t * b._a);
    }

    /// <summary>
    /// Tests to see if the alpha channel of this color is less than or equal to
    /// zero, i.e., if it is completely transparent.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>evaluation</returns>
    public static bool None(in Clr c)
    {
        return c._a <= 0.0f;
    }

    /// <summary>
    /// Multiplies the red, green and blue color channels of a color by the
    /// alpha channel. If alpha is less than or equal to zero, returns
    /// clear black.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>premultiplied color</returns>
    public static Clr Premul(in Clr c)
    {
        if (c.a <= 0.0f) { return Clr.ClearBlack; }
        if (c.a >= 1.0f) { return new Clr(c._r, c._g, c._b, 1.0f); }
        return new Clr(
            c._r * c._a,
            c._g * c._a,
            c._b * c._a,
            c._a);
    }

    /// <summary>
    /// Reduces the signal, or granularity, of a color's channels.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="levels">levels</param>
    /// <returns>posterized color</returns>
    public static Clr Quantize(in Clr c, in int levels)
    {
        return Clr.QuantizeUnsigned(c, levels);
    }

    /// <summary>
    /// Reduces the signal, or granularity, of a color's channels.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="levels">levels</param>
    /// <returns>posterized color</returns>
    public static Clr QuantizeSigned(in Clr c, in int levels)
    {
        return Clr.QuantizeSigned(c, levels, levels, levels, levels);
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
    public static Clr QuantizeSigned(
        in Clr c,
        in int rLevels,
        in int gLevels,
        in int bLevels)
    {
        return new Clr(
            Utils.QuantizeSigned(c._r, rLevels),
            Utils.QuantizeSigned(c._g, gLevels),
            Utils.QuantizeSigned(c._b, gLevels), c._a);
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
    public static Clr QuantizeSigned(
        in Clr c,
        in int rLevels,
        in int gLevels,
        in int bLevels,
        in int aLevels)
    {
        return new Clr(
            Utils.QuantizeSigned(c._r, rLevels),
            Utils.QuantizeSigned(c._g, gLevels),
            Utils.QuantizeSigned(c._b, bLevels),
            Utils.QuantizeSigned(c._a, aLevels));
    }

    /// <summary>
    /// Reduces the signal, or granularity, of a color's channels.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="levels">levels</param>
    /// <returns>posterized color</returns>
    public static Clr QuantizeUnsigned(in Clr c, in int levels)
    {
        return Clr.QuantizeUnsigned(c, levels, levels, levels, levels);
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
    public static Clr QuantizeUnsigned(
        in Clr c,
        in int rLevels,
        in int gLevels,
        in int bLevels)
    {
        return new Clr(
            Utils.QuantizeUnsigned(c._r, rLevels),
            Utils.QuantizeUnsigned(c._g, gLevels),
            Utils.QuantizeUnsigned(c._b, gLevels), c._a);
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
    public static Clr QuantizeUnsigned(
        in Clr c,
        in int rLevels,
        in int gLevels,
        in int bLevels,
        in int aLevels)
    {
        return new Clr(
            Utils.QuantizeUnsigned(c._r, rLevels),
            Utils.QuantizeUnsigned(c._g, gLevels),
            Utils.QuantizeUnsigned(c._b, gLevels),
            Utils.QuantizeUnsigned(c._a, gLevels));
    }

    /// <summary>
    /// Creates a random color from red, green, blue and alpha channels.
    /// </summary>
    /// <param name="rng">the random number generator</param>
    /// <param name="lb">the lower bound</param>
    /// <param name="ub">the upper bound</param>
    /// <returns>color</returns>
    public static Clr RandomRgba(in Random rng, in float lb = 0.0f, in float ub = 1.0f)
    {
        return new Clr(
            Utils.Mix(lb, ub, (float)rng.NextDouble()),
            Utils.Mix(lb, ub, (float)rng.NextDouble()),
            Utils.Mix(lb, ub, (float)rng.NextDouble()),
            Utils.Mix(lb, ub, (float)rng.NextDouble()));
    }

    /// <summary>
    /// Returns the relative luminance of the color.
    /// Converts the color from standard RGB to linear.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>luminance</returns>
    public static float StandardLuminance(in Clr c)
    {
        return Clr.LinearLuminance(Clr.StandardToLinear(c));
    }

    /// <summary>
    /// Converts a color from standard RGB (sRGB) to CIE LAB.
    /// Stores alpha in the w component, luminance in the z
    /// component, a in the x component and b in the y component.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>lab color</returns>
    public static Vec4 StandardToLaba(in Clr c)
    {
        return Clr.XyzaToLaba(
            Clr.LinearToXyza(
                Clr.StandardToLinear(c)));
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
    /// <returns>lab color</returns>
    public static Vec4 StandardToLcha(in Clr c)
    {
        return Clr.LabaToLcha(
            Clr.XyzaToLaba(
                Clr.LinearToXyza(
                    Clr.StandardToLinear(c))));
    }

    /// <summary>
    /// Converts a color from standard RGB (sRGB) to linear RGB.
    /// </summary>
    /// <param name="c">the standard color</param>
    /// <param name="alpha">transform the alpha channel</param>
    /// <returns>linear color</returns>
    public static Clr StandardToLinear(in Clr c, in bool alpha = false)
    {
        float inv1055 = 1.0f / 1.055f;
        return new Clr(
            c._r > 0.04045f ?
            MathF.Pow((c._r + 0.055f) * inv1055, 2.4f) :
            c._r * 0.07739938f,

            c._g > 0.04045f ?
            MathF.Pow((c._g + 0.055f) * inv1055, 2.4f) :
            c._g * 0.07739938f,

            c._b > 0.04045f ?
            MathF.Pow((c._b + 0.055f) * inv1055, 2.4f) :
            c._b * 0.07739938f,

            alpha ? c._a > 0.04045f ?
            MathF.Pow((c._a + 0.055f) * inv1055, 2.4f) :
            c._a * 0.07739938f :
            c._a);
    }

    /// <summary>
    /// Converts a color to an integer.
    /// </summary>
    /// <param name="c">the input color</param>
    /// <param name="order">color channel order</param>
    /// <returns>color in hexadecimal</returns>
    public static int ToHex(in Clr c, in ColorChannel order = ColorChannel.ARGB)
    {
        switch (order)
        {
            case ColorChannel.ABGR:
                { return Clr.ToHexAbgr(c); }
            case ColorChannel.RGBA:
                { return Clr.ToHexRgba(c); }
            case ColorChannel.ARGB:
            default:
                { return Clr.ToHexArgb(c); }
        }
    }

    /// <summary>
    /// Converts a color to an integer. Returns an integer ordered as 0xAABBGGRR.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>color in hexadecimal</returns>
    public static int ToHexAbgr(in Clr c)
    {
        return Clr.ToHexAbgrUnchecked(Clr.Clamp(c, 0.0f, 1.0f));
    }

    /// <summary>
    /// Converts a color to an integer. Returns an integer ordered as 0xAARRGGBB.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>color in hexadecimal</returns>
    public static int ToHexArgb(in Clr c)
    {
        return Clr.ToHexArgbUnchecked(Clr.Clamp(c, 0.0f, 1.0f));
    }

    /// <summary>
    /// Converts a color to an integer. Returns an integer ordered as 0xRRGGBBAA.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>color in hexadecimal</returns>
    public static int ToHexRgba(in Clr c)
    {
        return Clr.ToHexRgbaUnchecked(Clr.Clamp(c, 0.0f, 1.0f));
    }

    /// <summary>
    /// Converts a color to an integer. Does not check if the color's components
    /// are in a valid range, [0.0, 1.0].
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="order">color channel order</param>
    /// <returns>color in hexadecimal</returns>
    public static int ToHexUnchecked(in Clr c, in ColorChannel order = ColorChannel.ARGB)
    {
        switch (order)
        {
            case ColorChannel.ABGR:
                { return Clr.ToHexAbgrUnchecked(c); }
            case ColorChannel.RGBA:
                { return Clr.ToHexRgbaUnchecked(c); }
            case ColorChannel.ARGB:
            default:
                { return Clr.ToHexArgbUnchecked(c); }
        }
    }

    /// <summary>
    /// Converts a color to an integer. Does not check if the color's components
    /// are in a valid range, [0.0, 1.0]. Returns an integer ordered as 0xAABBGGRR.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>color in hexadecimal</returns>
    public static int ToHexAbgrUnchecked(in Clr c)
    {
        return (int)(c._a * 255.0f + 0.5f) << 0x18 |
               (int)(c._b * 255.0f + 0.5f) << 0x10 |
               (int)(c._g * 255.0f + 0.5f) << 0x08 |
               (int)(c._r * 255.0f + 0.5f);
    }

    /// <summary>
    /// Converts a color to an integer. Does not check if the color's components
    /// are in a valid range, [0.0, 1.0]. Returns an integer ordered as 0xAARRGGBB.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>color in hexadecimal</returns>
    public static int ToHexArgbUnchecked(in Clr c)
    {
        return (int)(c._a * 255.0f + 0.5f) << 0x18 |
               (int)(c._r * 255.0f + 0.5f) << 0x10 |
               (int)(c._g * 255.0f + 0.5f) << 0x08 |
               (int)(c._b * 255.0f + 0.5f);
    }

    /// <summary>
    /// Converts a color to an integer. Does not check if the color's components
    /// are in a valid range, [0.0, 1.0]. Returns an integer ordered as 0xRRGGBBAA.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>color in hexadecimal</returns>
    public static int ToHexRgbaUnchecked(in Clr c)
    {
        return (int)(c._r * 255.0f + 0.5f) << 0x18 |
               (int)(c._g * 255.0f + 0.5f) << 0x10 |
               (int)(c._b * 255.0f + 0.5f) << 0x08 |
               (int)(c._a * 255.0f + 0.5f);
    }

    /// <summary>
    /// Returns a string representation of a color in web-friendly hexadecimal
    /// format, i.e., in RRGGBB order. Does not prepend a hash tag. Clamps all
    /// values to [0.0, 1.0] before converting to a byte.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>string</returns>
    public static string ToHexWeb(in Clr c)
    {
        return Clr.ToHexWeb(new StringBuilder(8), c).ToString();
    }

    /// <summary>
    /// Appends a string representation of a color in web-friendly hexdecimal
    /// to a string builder. Does not prepend a hash tag. Clamps all values
    /// to [0.0, 1.0] before converting to a byte.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="c">color</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToHexWeb(in StringBuilder sb, in Clr c)
    {
        int r = (int)(Utils.Clamp(c._r, 0.0f, 1.0f) * 255.0f + 0.5f);
        int g = (int)(Utils.Clamp(c._g, 0.0f, 1.0f) * 255.0f + 0.5f);
        int b = (int)(Utils.Clamp(c._b, 0.0f, 1.0f) * 255.0f + 0.5f);
        sb.AppendFormat("{0:X6}", (r << 0x10 | g << 0x08 | b));
        return sb;
    }

    /// <summary>
    /// Returns a string representation of a color.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string</returns>
    public static string ToString(in Clr c, in int places = 4)
    {
        return Clr.ToString(new StringBuilder(96), c, places).ToString();
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
        in Clr c,
        in int places = 4)
    {
        sb.Append("{ r: ");
        Utils.ToFixed(sb, c._r, places);
        sb.Append(", g: ");
        Utils.ToFixed(sb, c._g, places);
        sb.Append(", b: ");
        Utils.ToFixed(sb, c._b, places);
        sb.Append(", a: ");
        Utils.ToFixed(sb, c._a, places);
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
    public static Clr Unpremul(in Clr c)
    {
        if (c.a <= 0.0f) { return Clr.ClearBlack; }
        if (c.a >= 1.0f) { return new Clr(c._r, c._g, c._b, 1.0f); }
        float aInv = 1.0f / c._a;
        return new Clr(
            c._r * aInv,
            c._g * aInv,
            c._b * aInv,
            c._a);
    }

    /// <summary>
    /// Converts a color from CIE XYZ to CIE LAB.
    /// Stores alpha in the w component,
    /// luminance in the z component,
    /// a in the x component and b in the y component.
    /// </summary>
    /// <param name="v">XYZ color</param>
    /// <returns>lab color</returns>
    public static Vec4 XyzaToLaba(in Vec4 v)
    {
        double oneThird = 1.0d / 3.0d;
        double offset = 16.0d / 116.0d;

        double a = v.x * 1.0521110608435826d;
        if (a > 0.008856d)
        {
            a = Math.Pow(a, oneThird);
        }
        else
        {
            a = 7.787d * a + offset;
        }

        double b = v.y;
        if (b > 0.008856d)
        {
            b = Math.Pow(b, oneThird);
        }
        else
        {
            b = 7.787d * b + offset;
        }

        double c = v.z * 0.9184170164304805d;
        if (c > 0.008856d)
        {
            c = Math.Pow(c, oneThird);
        }
        else
        {
            c = 7.787d * c + offset;
        }

        return new Vec4(
            (float)(500.0d * (a - b)), // a
            (float)(200.0d * (b - c)), // b
            (float)(116.0d * b - 16.0d), // l
            v.w);
    }

    /// <summary>
    /// Converts a color from CIE XYZ to linear RGB.
    /// The alpha channel is unaffected by the transformation.
    /// </summary>
    /// <param name="v">XYZ color</param>
    /// <returns>linear color</returns>
    public static Clr XyzaToLinear(in Vec4 v)
    {
        return new Clr(
            3.2408123f * v.x - 1.5373085f * v.y - 0.49858654f * v.z, //
            -0.969243f * v.x + 1.8759663f * v.y + 0.041555032f * v.z, //
            0.0556384f * v.x - 0.20400746f * v.y + 1.0571296f * v.z, //
            v.w);
    }

    /// <summary>
    /// Returns the normal direction back as a color,
    /// (0.5, 0.0, 0.5, 1.0).
    /// </summary>
    /// <returns>back</returns>
    public static Clr Back { get { return new Clr(0.5f, 0.0f, 0.5f, 1.0f); } }

    /// <summary>
    /// Returns the color black, (0.0, 0.0, 0.0, 1.0) .
    /// </summary>
    /// <value>black</value>
    public static Clr Black { get { return new Clr(0.0f, 0.0f, 0.0f, 1.0f); } }

    /// <summary>
    /// Returns the color blue, (0.0, 0.0, 1.0, 1.0) .
    /// </summary>
    /// <value>blue</value>
    public static Clr Blue { get { return new Clr(0.0f, 0.0f, 1.0f, 1.0f); } }

    /// <summary>
    /// Returns the color clear black, (0.0, 0.0, 0.0, 0.0) .
    /// </summary>
    /// <value>clear black</value>
    public static Clr ClearBlack { get { return new Clr(0.0f, 0.0f, 0.0f, 0.0f); } }

    /// <summary>
    /// Returns the color clear white, (1.0, 1.0, 1.0, 0.0) .
    /// </summary>
    /// <value>clear white</value>
    public static Clr ClearWhite { get { return new Clr(1.0f, 1.0f, 1.0f, 0.0f); } }

    /// <summary>
    /// Returns the color cyan, (0.0, 1.0, 1.0, 1.0) .
    /// </summary>
    /// <value>cyan</value>
    public static Clr Cyan { get { return new Clr(0.0f, 1.0f, 1.0f, 1.0f); } }

    /// <summary>
    /// Returns the normal direction down as a color,
    /// (0.5, 0.5, 0.0, 1.0).
    /// </summary>
    /// <returns>down</returns>
    public static Clr Down { get { return new Clr(0.5f, 0.5f, 0.0f, 1.0f); } }

    /// <summary>
    /// Returns the normal direction forward as a color,
    /// (0.5, 1.0, 0.5, 1.0).
    /// </summary>
    /// <returns>forward</returns>
    public static Clr Forward { get { return new Clr(0.5f, 1.0f, 0.5f, 1.0f); } }

    /// <summary>
    /// Returns the color green, (0.0, 1.0, 0.0, 1.0) .
    /// </summary>
    /// <value>green</value>
    public static Clr Green { get { return new Clr(0.0f, 1.0f, 0.0f, 1.0f); } }

    /// <summary>
    /// Returns the normal direction left as a color,
    /// (0.0, 0.5, 0.5, 1.0).
    /// </summary>
    /// <returns>left</returns>
    public static Clr Left { get { return new Clr(0.0f, 0.5f, 0.5f, 1.0f); } }

    /// <summary>
    /// Returns the color magenta, (1.0, 0.0, 1.0, 1.0) .
    /// </summary>
    /// <value>magenta</value>
    public static Clr Magenta { get { return new Clr(1.0f, 0.0f, 1.0f, 1.0f); } }

    /// <summary>
    /// Returns the color red, (1.0, 0.0, 0.0, 1.0) .
    /// </summary>
    /// <value>red</value>
    public static Clr Red { get { return new Clr(1.0f, 0.0f, 0.0f, 1.0f); } }

    /// <summary>
    /// Returns the normal direction right as a color,
    /// (1.0, 0.5, 0.5, 1.0).
    /// </summary>
    /// <returns>right</returns>
    public static Clr Right { get { return new Clr(1.0f, 0.5f, 0.5f, 1.0f); } }

    /// <summary>
    /// Returns the normal direction up as a color,
    /// (0.5, 0.5, 1.0, 1.0).
    /// </summary>
    /// <returns>up</returns>
    public static Clr Up { get { return new Clr(0.5f, 0.5f, 1.0f, 1.0f); } }

    /// <summary>
    /// Returns the color white, (1.0, 1.0, 1.0, 1.0) .
    /// </summary>
    /// <value>white</value>
    public static Clr White { get { return new Clr(1.0f, 1.0f, 1.0f, 1.0f); } }

    /// <summary>
    /// Returns the color yellow, (1.0, 1.0, 0.0, 1.0) .
    /// </summary>
    /// <value>yellow</value>
    public static Clr Yellow { get { return new Clr(1.0f, 1.0f, 0.0f, 1.0f); } }
}