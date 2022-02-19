using System;
using System.Collections;
using System.Text;

/// <summary>
/// A readonly struct. Supports conversion
/// to and from integers where color channels are in the format 0xAARRGGBB.
/// </summary>
[Serializable]
public readonly struct Clr : IComparable<Clr>, IEquatable<Clr>, IEnumerable
{
    /// <summary>
    /// Arbitrary hue in HSL assigned to desaturated colors closer to daylight.
    /// </summary>
    public const float HslHueDay = 48.0f / 360.0f;

    /// <summary>
    /// Arbitrary hue in HSL assigned to desaturated colors closer to shadow.
    /// </summary>
    public const float HslHueShade = 255.0f / 360.0f;

    /// <summary>
    /// Arbitrary hue in LCh assigned to desaturated colors closer to daylight.
    /// </summary>
    public const float LchHueDay = 99.0f / 360.0f;

    /// <summary>
    /// Arbitrary hue in LCh assigned to desaturated colors that are closer to shadow.
    /// </summary>
    public const float LchHueShade = 308.0f / 360.0f;

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
    /// <value>the length</value>
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
    /// Retrieves a channel by index. When the provided index is 3 or -1,
    /// returns alpha; 2 or -2, blue; 1 or -3, green; 0 or -4, red.
    /// </summary>
    /// <value>the component</value>
    public float this [int i]
    {
        get
        {
            switch (i)
            {
                case 0:
                case -4:
                    return this._r;
                case 1:
                case -3:
                    return this._g;
                case 2:
                case -2:
                    return this._b;
                case 3:
                case -1:
                    return this._a;
                default:
                    return 0.0f;
            }
        }
    }

    /// <summary>
    /// Creates a color from unsigned bytes. Converts each to a single precision
    /// real number in the  range [0.0, 1.0] .
    /// </summary>
    /// <param name="r">red channel</param>
    /// <param name="g">green channel</param>
    /// <param name="b">blue channel</param>
    /// <param name="a">alpha channel</param>
    public Clr (in byte r = 255, in byte g = 255, in byte b = 255, in byte a = 255)
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
    public Clr (in sbyte r = -1, in sbyte g = -1, in sbyte b = -1, in sbyte a = -1)
    {
        this._r = (((int) r) & 0xff) * Utils.One255;
        this._g = (((int) g) & 0xff) * Utils.One255;
        this._b = (((int) b) & 0xff) * Utils.One255;
        this._a = (((int) a) & 0xff) * Utils.One255;
    }

    /// <summary>
    /// Creates a color from single precision real numbers. Clamps values to a
    /// range [0.0, 1.0] .
    /// </summary>
    /// <param name="r">red channel</param>
    /// <param name="g">green channel</param>
    /// <param name="b">blue channel</param>
    /// <param name="a">alpha channel</param>
    public Clr (in float r = 1.0f, in float g = 1.0f, in float b = 1.0f, in float a = 1.0f)
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
    /// <returns>the equivalence</returns>
    public override bool Equals (object value)
    {
        if (Object.ReferenceEquals (this, value))
        {
            return true;
        }
        if (value is null)
        {
            return false;
        }
        if (value is Clr)
        {
            return this.Equals ((Clr) value);
        }
        return false;
    }

    /// <summary>
    /// Returns a hash code representing this color.
    /// </summary>
    /// <returns>the hash code</returns>
    public override int GetHashCode ( )
    {
        return Clr.ToHexInt (this);
    }

    /// <summary>
    /// Returns a string representation of this color.
    /// </summary>
    /// <returns>the string</returns>
    public override string ToString ( )
    {
        return Clr.ToString (this);
    }

    /// <summary>
    /// Compares this color to another in compliance with the IComparable
    /// interface.
    /// </summary>
    /// <param name="c">comparisand</param>
    /// <returns>evaluation</returns>
    public int CompareTo (Clr c)
    {
        int left = Clr.ToHexInt (this);
        int right = Clr.ToHexInt (c);
        return (left < right) ? -1 : (left > right) ? 1 : 0;
    }

    /// <summary>
    /// Tests this color for equivalence with another in compliance with the
    /// IEquatable interface.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>the equivalence</returns>
    public bool Equals (Clr c)
    {
        return Clr.EqSatArith (this, c);
    }

    /// <summary>
    /// Returns an enumerator (or iterator) for this color, allowing its
    /// components to be accessed in a foreach loop. The alpha channel is last.
    /// </summary>
    /// <returns>the enumerator</returns>
    public IEnumerator GetEnumerator ( )
    {
        yield return this._r;
        yield return this._g;
        yield return this._b;
        yield return this._a;
    }

    /// <summary>
    /// Returns a float array of length 4 containing this color's components.
    /// The alpha channel is last.
    /// </summary>
    /// <returns>the array</returns>
    public float[ ] ToArray ( )
    {
        return this.ToArray (new float[this.Length], 0);
    }

    /// <summary>
    /// Puts this colors's components into an array at a given index.
    /// </summary>
    /// <param name="arr">array</param>
    /// <param name="i">index</param>
    /// <returns>array</returns>
    public float[ ] ToArray (in float[ ] arr, in int i = 0)
    {
        arr[i] = this._r;
        arr[i + 1] = this._g;
        arr[i + 2] = this._b;
        arr[i + 3] = this._a;
        return arr;
    }

    /// <summary>
    /// Returns a named value tuple containing this color's components.
    /// </summary>
    /// <returns>the tuple</returns>
    public (float r, float g, float b, float a) ToTuple ( )
    {
        return (r: this._r, g: this._g, b: this._b, a: this._a);
    }

    /// <summary>
    /// Converts a color to a boolean by returning whether its alpha is greater
    /// than zero.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>the evaluation</returns>
    public static explicit operator bool (in Clr c)
    {
        return Clr.Any (c);
    }

    /// <summary>
    /// Converts a color to a signed integer.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>the integer</returns>
    public static explicit operator int (in Clr c)
    {
        return Clr.ToHexInt (c);
    }

    /// <summary>
    /// Converts a color to an unsigned integer.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>the integer</returns>
    public static explicit operator uint (in Clr c)
    {
        return (uint) Clr.ToHexInt (c);
    }

    /// <summary>
    /// Converts a color to a long.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>the long</returns>
    public static explicit operator long (in Clr c)
    {
        return ((long) Clr.ToHexInt (c)) & 0xffffffffL;
    }

    /// <summary>
    /// A color evaluates to true if its alpha channel is greater than zero.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>the evaluation</returns>
    public static bool operator true (in Clr c)
    {
        return Clr.Any (c);
    }

    /// <summary>
    /// A color evaluates to false if its alpha channel is less than or equal to
    /// zero.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>the evaluation</returns>
    public static bool operator false (in Clr c)
    {
        return Clr.None (c);
    }

    /// <summary>
    /// Converts a color to an integer, performs the bitwise not operation on
    /// it, then converts the result to a color.
    /// </summary>
    /// <param name="c">the input color</param>
    /// <returns>the negated color</returns>
    public static Clr operator ~ (in Clr c)
    {
        return Clr.FromHex (~Clr.ToHexInt (c));
    }

    /// <summary>
    /// Converts two colors to integers, performs the bitwise and operation on
    /// them, then converts the result to a color.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the color</returns>
    public static Clr operator & (in Clr a, in Clr b)
    {
        return Clr.FromHex (Clr.ToHexInt (a) & Clr.ToHexInt (b));
    }

    /// <summary>
    /// Converts two colors to integers, performs the bitwise inclusive or
    /// operation on them, then converts the result to a color.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the color</returns>
    public static Clr operator | (in Clr a, in Clr b)
    {
        return Clr.FromHex (Clr.ToHexInt (a) | Clr.ToHexInt (b));
    }

    /// <summary>
    /// Converts two colors to integers, performs the bitwise exclusive or
    /// operation on them, then converts the result to a color.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the color</returns>
    public static Clr operator ^ (in Clr a, in Clr b)
    {
        return Clr.FromHex (Clr.ToHexInt (a) ^ Clr.ToHexInt (b));
    }

    /// <summary>
    /// Converts a color to an integer, performs a bitwise left shift operation,
    /// then converts the result to a color. To shift a whole color channel, use
    /// increments of 8 (8, 16, 24).
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the shifted color</returns>
    public static Clr operator << (in Clr a, int b)
    {
        return Clr.FromHex (Clr.ToHexInt (a) << b);
    }

    /// <summary>
    /// Converts a color to an integer, performs a bitwise right shift
    /// operation, then converts the result to a color. To shift a whole color
    /// channel, use increments of 8 (8, 16, 24).
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the shifted color</returns>
    public static Clr operator >> (in Clr a, int b)
    {
        return Clr.FromHex (Clr.ToHexInt (a) >> b);
    }

    /// <summary>
    /// Tests to see if all color channels are greater than zero.
    /// </summary>
    /// <param name="c">the color</param>
    /// <returns>the evaluation</returns>
    public static bool All (in Clr c)
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
    /// <returns>the evaluation</returns>
    public static bool Any (in Clr c)
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
    public static Clr Clamp (in Clr c, in float lb = 0.0f, in float ub = 1.0f)
    {
        return new Clr (
            Utils.Clamp (c._r, lb, ub),
            Utils.Clamp (c._g, lb, ub),
            Utils.Clamp (c._b, lb, ub),
            Utils.Clamp (c._a, lb, ub));
    }

    /// <summary>
    /// Tests to see if a color contains a value.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="v">value</param>
    /// <returns>the evaluation</returns>
    public static bool Contains (in Clr c, in float v)
    {
        return Utils.Approx (c._a, v) ||
            Utils.Approx (c._b, v) ||
            Utils.Approx (c._g, v) ||
            Utils.Approx (c._r, v);
    }

    /// <summary>
    /// Checks if two colors have equivalent alpha channels when converted to
    /// bytes in [0, 255]. Uses saturation arithmetic.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>the evaluation</returns>
    public static bool EqAlphaSatArith (in Clr a, in Clr b)
    {
        return (int) (0.5f + Utils.Clamp (a._a, 0.0f, 1.0f) * 0xff) ==
            (int) (0.5f + Utils.Clamp (b._a, 0.0f, 1.0f) * 0xff);
    }

    /// <summary>
    /// Checks if two colors have equivalent red, green and blue channels when
    /// converted to bytes in [0, 255]. Uses saturation arithmetic.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>the evaluation</returns>
    public static bool EqRgbSatArith (in Clr a, in Clr b)
    {
        return (int) (0.5f + Utils.Clamp (a._b, 0.0f, 1.0f) * 0xff) ==
            (int) (0.5f + Utils.Clamp (b._b, 0.0f, 1.0f) * 0xff) &&
            (int) (0.5f + Utils.Clamp (a._g, 0.0f, 1.0f) * 0xff) ==
            (int) (0.5f + Utils.Clamp (b._g, 0.0f, 1.0f) * 0xff) &&
            (int) (0.5f + Utils.Clamp (a._r, 0.0f, 1.0f) * 0xff) ==
            (int) (0.5f + Utils.Clamp (b._r, 0.0f, 1.0f) * 0xff);
    }

    /// <summary>
    /// Checks if two colors have equivalent red, green, blue and alph
    /// channels when converted to bytes in [0, 255]. Uses saturation
    /// arithmetic.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>the evaluation</returns>
    public static bool EqSatArith (in Clr a, in Clr b)
    {
        return Clr.EqAlphaSatArith (a, b) &&
            Clr.EqRgbSatArith (a, b);
    }

    /// <summary>
    /// Convert a hexadecimal representation of a color stored as 0xAARRGGBB
    /// into a color.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>the color</returns>
    public static Clr FromHex (in int c)
    {
        return new Clr (
            (c >> 0x10 & 0xff) * Utils.One255,
            (c >> 0x08 & 0xff) * Utils.One255,
            (c & 0xff) * Utils.One255,
            (c >> 0x18 & 0xff) * Utils.One255);
    }

    /// <summary>
    /// Convert a hexadecimal representation of a color stored as 0xAARRGGBB
    /// into a color.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>the color</returns>
    public static Clr FromHex (in uint c)
    {
        return new Clr (
            (c >> 0x10 & 0xff) * Utils.One255,
            (c >> 0x08 & 0xff) * Utils.One255,
            (c & 0xff) * Utils.One255,
            (c >> 0x18 & 0xff) * Utils.One255);
    }

    /// <summary>
    /// Convert a hexadecimal representation of a color stored as 0xAARRGGBB
    /// into a color.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>the color</returns>
    public static Clr FromHex (in long c)
    {
        return new Clr (
            (c >> 0x10 & 0xff) * Utils.One255,
            (c >> 0x08 & 0xff) * Utils.One255,
            (c & 0xff) * Utils.One255,
            (c >> 0x18 & 0xff) * Utils.One255);
    }

    /// <summary>
    /// Converts from hue, saturation, lightness and alpha to
    /// a color with red, green, blue and alpha. All
    /// arguments are expected to be in the range [0.0, 1.0].
    /// </summary>
    /// <param name="hsla">hsla color</param>
    /// <returns>rgba color</returns>
    public static Clr HslaToRgba (in Vec4 hsla)
    {
        float aCl = Utils.Clamp (hsla.w, 0.0f, 1.0f);
        float light = hsla.z;
        if (light <= 0.0f)
        {
            return new Clr (0.0f, 0.0f, 0.0f, aCl);
        }
        if (light >= 1.0f)
        {
            return new Clr (1.0f, 1.0f, 1.0f, aCl);
        }

        float sat = hsla.y;
        if (sat <= 0.0f)
        {
            return new Clr (light, light, light, aCl);
        }

        float scl = sat > 1.0f ? 1.0f : sat;
        float q = light < 0.5f ?
            light * (1.0f + scl) :
            light + scl - light * scl;
        float p = light + light - q;
        float qnp6 = (q - p) * 6.0f;

        float rHue = Utils.RemFloor (hsla.x + Utils.OneThird, 1.0f);
        float gHue = Utils.RemFloor (hsla.x, 1.0f);
        float bHue = Utils.RemFloor (hsla.x - Utils.OneThird, 1.0f);

        float r = p;
        if (rHue < Utils.OneSix)
        {
            r = p + qnp6 * rHue;
        }
        else if (rHue < 0.5f)
        {
            r = q;
        }
        else if (rHue < Utils.TwoThirds)
        {
            r = p + qnp6 * (Utils.TwoThirds - rHue);
        }

        float g = p;
        if (gHue < Utils.OneSix)
        {
            g = p + qnp6 * gHue;
        }
        else if (gHue < 0.5f)
        {
            g = q;
        }
        else if (gHue < Utils.TwoThirds)
        {
            g = p + qnp6 * (Utils.TwoThirds - gHue);
        }

        float b = p;
        if (bHue < Utils.OneSix)
        {
            b = p + qnp6 * bHue;
        }
        else if (bHue < 0.5f)
        {
            b = q;
        }
        else if (bHue < Utils.TwoThirds)
        {
            b = p + qnp6 * (Utils.TwoThirds - bHue);
        }

        return new Clr (r, g, b, aCl);
    }

    /// <summary>
    /// Converts from hue, saturation, value and alpha to
    /// a color with red, green, blue and alpha. All
    /// arguments are expected to be in the range [0.0, 1.0].
    /// </summary>
    /// <param name="hsva">hsva color</param>
    /// <returns>rgba color</returns>
    public static Clr HsvaToRgba (in Vec4 hsva)
    {
        float h = Utils.RemFloor (hsva.x, 1.0f) * 6.0f;
        float s = Utils.Clamp (hsva.y, 0.0f, 1.0f);
        float v = Utils.Clamp (hsva.z, 0.0f, 1.0f);
        float a = Utils.Clamp (hsva.w, 0.0f, 1.0f);

        int sector = (int) h;
        float secf = sector;
        float tint1 = v * (1.0f - s);
        float tint2 = v * (1.0f - s * (h - secf));
        float tint3 = v * (1.0f - s * (1.0f + secf - h));

        switch (sector)
        {
            case 0:
                return new Clr (v, tint3, tint1, a);
            case 1:
                return new Clr (tint2, v, tint1, a);
            case 2:
                return new Clr (tint1, v, tint3, a);
            case 3:
                return new Clr (tint1, tint2, v, a);
            case 4:
                return new Clr (tint3, tint1, v, a);
            case 5:
                return new Clr (v, tint1, tint2, a);
            default:
                return Clr.White;
        }
    }

    /// <summary>
    /// Converts a color from CIE LAB to CIE LCH.
    /// The z component of the input vector is
    /// expected to hold the luminance, while x
    /// is expected to hold a and y, b.
    /// </summary>
    /// <param name="lab">lab color</param>
    /// <returns>lch color</returns>
    public static Vec4 LabaToLcha (in Vec4 lab)
    {
        double a = (double) lab.x;
        double b = (double) lab.y;
        double chromaSq = a * a + b * b;
        if (chromaSq < Utils.Epsilon)
        {
            float fac = Utils.Clamp (lab.z * 0.01f, 0.0f, 1.0f);
            float hue = Utils.LerpAngleNear (
                Clr.LchHueShade, Clr.LchHueDay, fac, 1.0f);
            return new Vec4 (hue, 0.0f, lab.z, lab.w);
        }
        else
        {
            return new Vec4 (
                Utils.OneTau * Utils.WrapRadians (
                    (float) Math.Atan2 (b, a)),
                (float) Math.Sqrt (chromaSq),
                lab.z,
                lab.w);
        }
    }

    /// <summary>
    /// Converts from CIE LAB to CIE XYZ.
    /// The z component of the input vector is
    /// expected to hold the luminance, while x
    /// is expected to hold a and y, b.
    /// </summary>
    /// <param name="lab">the lab color</param>
    /// <returns>the xyz color</returns>
    public static Vec4 LabaToXyza (in Vec4 lab)
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

        return new Vec4 (
            (float) (b * 0.95047d),
            (float) a,
            (float) (c * 1.08883d),
            lab.w);
    }

    /// <summary>
    /// Converts a color from CIE LCH to CIE LAB.
    /// Lightness is expected to be in [0.0, 100.0].
    /// Chroma is expected to be in [0.0, 135.0].
    /// Hue is expected to be in [0.0, 1.0].
    /// </summary>
    /// <param name="lch">LCh color</param>
    /// <returns>Lab color</returns>
    public static Vec4 LchaToLaba (in Vec4 lch)
    {
        double hRad = lch.x * 6.283185307179586d;
        float chroma = Utils.Max (0.0f, lch.y);
        return new Vec4 (
            chroma * (float) Math.Cos (hRad),
            chroma * (float) Math.Sin (hRad),
            lch.z,
            lch.w);
    }

    /// <summary>
    /// Returns the relative luminance of the color.
    /// Assumes the color is in linear RGB.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>the luminance</returns>
    public static float LinearLuminance (in Clr c)
    {
        return 0.21264935f * c._r +
            0.71516913f * c._g +
            0.07218152f * c._b;
    }

    /// <summary>
    /// Converts a color from linear RGB to standard RGB (sRGB).
    /// </summary>
    /// <param name="c">the linear color</param>
    /// <param name="alpha">transform the alpha channel</param>
    /// <returns>the standard color</returns>
    public static Clr LinearToStandard (in Clr c, in bool alpha = false)
    {
        double inv24 = 1.0d / 2.4d;
        return new Clr (
            c._r > 0.0031308f ?
            (float) (Math.Pow (c._r, inv24) * 1.055d - 0.055d) :
            c._r * 12.92f,

            c._g > 0.0031308f ?
            (float) (Math.Pow (c._g, inv24) * 1.055d - 0.055d) :
            c._g * 12.92f,

            c._b > 0.0031308f ?
            (float) (Math.Pow (c._b, inv24) * 1.055d - 0.055d) :
            c._b * 12.92f,

            alpha ? c._a > 0.0031308f ?
            (float) (Math.Pow (c._a, inv24) * 1.055d - 0.055d) :
            c._a * 12.92f :
            c._a);
    }

    /// <summary>
    /// Converts a color from linear RGB to CIE XYZ.
    /// The alpha channel is unaffected by the transformation.
    /// </summary>
    /// <param name="c">the linear color</param>
    /// <returns>the XYZ color</returns>
    public static Vec4 LinearToXyza (in Clr c)
    {
        return new Vec4 (
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
    /// <returns>the mixed color</returns>
    public static Clr MixLaba (in Clr a, in Clr b, in float t)
    {
        Clr aLin = Clr.StandardToLinear (a);
        Vec4 aXyz = Clr.LinearToXyza (aLin);
        Vec4 aLab = Clr.XyzaToLaba (aXyz);

        Clr bLin = Clr.StandardToLinear (b);
        Vec4 bXyz = Clr.LinearToXyza (bLin);
        Vec4 bLab = Clr.XyzaToLaba (bXyz);

        Vec4 cLab = Vec4.Mix (aLab, bLab, t);
        Vec4 cXyz = Clr.LabaToXyza (cLab);
        Clr cLin = Clr.XyzaToLinear (cXyz);

        return Clr.LinearToStandard (cLin);
    }

    /// <summary>
    /// Mixes two colors by a step in the range [0.0, 1.0] .
    /// Converts each color from sRGB to HSL, mixes according
    /// to the step, then converts back to sRGB.
    /// </summary>
    /// <param name="a">origin color</param>
    /// <param name="b">destination color</param>
    /// <param name="t">step</param>
    /// <returns>the mixed color</returns>
    public static Clr MixHsla (in Clr a, in Clr b, in float t)
    {
        return Clr.MixHsla (a, b, t, (x, y, z, w) => Utils.LerpAngleNear (x, y, z, w));
    }

    /// <summary>
    /// Mixes two colors by a step in the range [0.0, 1.0] .
    /// Converts each color from sRGB to HSV, mixes according
    /// to the step, then converts back to sRGB.
    /// </summary>
    /// <param name="a">origin color</param>
    /// <param name="b">destination color</param>
    /// <param name="t">step</param>
    /// <returns>the mixed color</returns>
    public static Clr MixHsva (in Clr a, in Clr b, in float t)
    {
        return Clr.MixHsva (a, b, t, (x, y, z, w) => Utils.LerpAngleNear (x, y, z, w));
    }

    /// <summary>
    /// Mixes two colors by a step in the range [0.0, 1.0] .
    /// Converts each color from sRGB to CIE LCH, mixes according
    /// to the step, then converts back to sRGB.
    /// </summary>
    /// <param name="a">origin color</param>
    /// <param name="b">destination color</param>
    /// <param name="t">step</param>
    /// <returns>the mixed color</returns>
    public static Clr MixLcha (in Clr a, in Clr b, in float t)
    {
        return Clr.MixLcha (a, b, t, (x, y, z, w) => Utils.LerpAngleNear (x, y, z, w));
    }

    /// <summary>
    /// Mixes two colors by a step in the range [0.0, 1.0] .
    /// Converts each color from sRGB to HSL, mixes according
    /// to the step, then converts back to sRGB.
    /// The easing function is expected to ease from an origin
    /// hue to a destination by a factor according to a range.
    /// </summary>
    /// <param name="a">origin color</param>
    /// <param name="b">destination color</param>
    /// <param name="t">step</param>
    /// <param name="easing">easing function</param>
    /// <returns>the mixed color</returns>
    public static Clr MixHsla ( //
        in Clr a, //
        in Clr b, //
        in float t, //
        in Func<float, float, float, float, float> easing)
    {
        Vec4 aHsla = Clr.RgbaToHsla (a);
        Vec4 bHsla = Clr.RgbaToHsla (b);

        if (aHsla.y < Utils.Epsilon || bHsla.y < Utils.Epsilon)
        {
            return Clr.MixRgbaLinear (a, b, t);
        }
        else
        {
            float u = 1.0f - t;
            Vec4 cHsla = new Vec4 (
                easing (aHsla.x, bHsla.x, t, 1.0f),
                u * aHsla.y + t * bHsla.y,
                u * aHsla.z + t * bHsla.z,
                u * aHsla.w + t * bHsla.w);
            return Clr.HslaToRgba (cHsla);
        }
    }

    /// <summary>
    /// Mixes two colors by a step in the range [0.0, 1.0] .
    /// Converts each color from sRGB to HSV, mixes according
    /// to the step, then converts back to sRGB.
    /// The easing function is expected to ease from an origin
    /// hue to a destination by a factor according to a range.
    /// </summary>
    /// <param name="a">origin color</param>
    /// <param name="b">destination color</param>
    /// <param name="t">step</param>
    /// <param name="easing">easing function</param>
    /// <returns>the mixed color</returns>
    public static Clr MixHsva ( //
        in Clr a, //
        in Clr b, //
        in float t, //
        in Func<float, float, float, float, float> easing)
    {
        Vec4 aHsva = Clr.RgbaToHsva (a);
        Vec4 bHsva = Clr.RgbaToHsva (b);

        if (aHsva.y < Utils.Epsilon || bHsva.y < Utils.Epsilon)
        {
            return Clr.MixRgbaLinear (a, b, t);
        }
        else
        {
            float u = 1.0f - t;
            Vec4 cHsva = new Vec4 (
                easing (aHsva.x, bHsva.x, t, 1.0f),
                u * aHsva.y + t * bHsva.y,
                u * aHsva.z + t * bHsva.z,
                u * aHsva.w + t * bHsva.w);
            return Clr.HsvaToRgba (cHsva);
        }
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
    /// <returns>the mixed color</returns>
    public static Clr MixLcha ( //
        in Clr a, //
        in Clr b, //
        in float t, //
        in Func<float, float, float, float, float> easing)
    {
        Clr aLin = Clr.StandardToLinear (a);
        Vec4 aXyz = Clr.LinearToXyza (aLin);
        Vec4 aLab = Clr.XyzaToLaba (aXyz);
        float aa = aLab.x;
        float ab = aLab.y;
        float aChrSq = aa * aa + ab * ab;

        Clr bLin = Clr.StandardToLinear (b);
        Vec4 bXyz = Clr.LinearToXyza (bLin);
        Vec4 bLab = Clr.XyzaToLaba (bXyz);
        float ba = bLab.x;
        float bb = bLab.y;
        float bChrSq = ba * ba + bb * bb;

        Vec4 cLab;

        if (aChrSq < Utils.Epsilon || bChrSq < Utils.Epsilon)
        {
            cLab = Vec4.Mix (aLab, bLab, t);
        }
        else
        {
            float aChr = (float) Math.Sqrt (aChrSq);
            float aHue = Utils.OneTau * Utils.WrapRadians (
                (float) Math.Atan2 (ab, aa));

            float bChr = (float) Math.Sqrt (bChrSq);
            float bHue = Utils.OneTau * Utils.WrapRadians (
                (float) Math.Atan2 (bb, ba));

            float u = 1.0f - t;
            Vec4 cLch = new Vec4 (
                easing (aHue, bHue, t, 1.0f),
                u * aChr + t * bChr,
                u * aLab.z + t * bLab.z,
                u * aLab.w + t * bLab.w);
            cLab = Clr.LchaToLaba (cLch);
        }

        Vec4 cXyz = Clr.LabaToXyza (cLab);
        Clr cLin = Clr.XyzaToLinear (cXyz);
        return Clr.LinearToStandard (cLin);
    }

    /// <summary>
    /// Mixes two colors by a step in the range [0.0, 1.0] .
    /// Assumes the colors are in linear RGB.
    /// </summary>
    /// <param name="a">origin color</param>
    /// <param name="b">destination color</param>
    /// <param name="t">step</param>
    /// <returns>the mixed color</returns>
    public static Clr MixRgbaLinear (in Clr a, in Clr b, in float t)
    {
        float u = 1.0f - t;
        return new Clr (
            u * a._r + t * b._r,
            u * a._g + t * b._g,
            u * a._b + t * b._b,
            u * a._a + t * b._a);
    }

    /// <summary>
    /// Mixes two colors by a step in the range [0.0, 1.0] .
    /// Assumes the colors are in standard RGB; converts them to
    /// linear, then mixes them, then converts to standard.
    /// </summary>
    /// <param name="a">origin color</param>
    /// <param name="b">destination color</param>
    /// <param name="t">step</param>
    /// <returns>the mixed color</returns>
    public static Clr MixRgbaStandard (in Clr a, in Clr b, in float t)
    {
        return Clr.LinearToStandard (
            Clr.MixRgbaLinear (
                Clr.StandardToLinear (a),
                Clr.StandardToLinear (b), t));
    }

    /// <summary>
    /// Tests to see if the alpha channel of this color is less than or equal to
    /// zero, i.e., if it is completely transparent.
    /// </summary>
    /// <param name="c">the color</param>
    /// <returns>the evaluation</returns>
    public static bool None (in Clr c)
    {
        return c._a <= 0.0f;
    }

    /// <summary>
    /// Multiplies the red, green and blue color channels of a color by the
    /// alpha channel.
    /// </summary>
    /// <param name="c">the color</param>
    /// <returns>the premultiplied color</returns>
    public static Clr Premul (in Clr c)
    {
        if (c.a <= 0.0f) { return Clr.ClearBlack; }
        if (c.a >= 1.0f) { return new Clr (c._r, c._g, c._b, 1.0f); }
        return new Clr (
            c._r * c._a,
            c._g * c._a,
            c._b * c._a,
            c._a);
    }

    /// <summary>
    /// Reduces the signal, or granularity, of a color's channels.
    /// </summary>
    /// <param name="c">the color</param>
    /// <param name="levels">the levels</param>
    /// <returns>the posterized color</returns>
    public static Clr Quantize (in Clr c, in int levels)
    {
        return Clr.QuantizeUnsigned (c, levels);
    }

    /// <summary>
    /// Reduces the signal, or granularity, of a color's channels.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="levels">levels</param>
    /// <returns>posterized color</returns>
    public static Clr QuantizeSigned (in Clr c, in int levels)
    {
        return Clr.QuantizeSigned (c, levels, levels, levels, levels);
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
    public static Clr QuantizeSigned ( //
        in Clr c, //
        in int rLevels, //
        in int gLevels, //
        in int bLevels)
    {
        return new Clr (
            Utils.QuantizeSigned (c._r, rLevels),
            Utils.QuantizeSigned (c._g, gLevels),
            Utils.QuantizeSigned (c._b, gLevels), c._a);
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
    public static Clr QuantizeSigned ( //
        in Clr c, //
        in int rLevels, //
        in int gLevels, //
        in int bLevels, //
        in int aLevels)
    {
        return new Clr (
            Utils.QuantizeSigned (c._r, rLevels),
            Utils.QuantizeSigned (c._g, gLevels),
            Utils.QuantizeSigned (c._b, bLevels),
            Utils.QuantizeSigned (c._a, aLevels));
    }

    /// <summary>
    /// Reduces the signal, or granularity, of a color's channels.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="levels">levels</param>
    /// <returns>posterized color</returns>
    public static Clr QuantizeUnsigned (in Clr c, in int levels)
    {
        return Clr.QuantizeUnsigned (c, levels, levels, levels, levels);
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
    public static Clr QuantizeUnsigned ( //
        in Clr c, //
        in int rLevels, //
        in int gLevels, //
        in int bLevels)
    {
        return new Clr (
            Utils.QuantizeUnsigned (c._r, rLevels),
            Utils.QuantizeUnsigned (c._g, gLevels),
            Utils.QuantizeUnsigned (c._b, gLevels), c._a);
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
    public static Clr QuantizeUnsigned ( //
        in Clr c, //
        in int rLevels, //
        in int gLevels, //
        in int bLevels, //
        in int aLevels)
    {
        return new Clr (
            Utils.QuantizeUnsigned (c._r, rLevels),
            Utils.QuantizeUnsigned (c._g, gLevels),
            Utils.QuantizeUnsigned (c._b, gLevels),
            Utils.QuantizeUnsigned (c._a, gLevels));
    }

    /// <summary>
    /// Creates a random color from red, green, blue and alpha channels.
    /// </summary>
    /// <param name="rng">the random number generator</param>
    /// <param name="lb">the lower bound</param>
    /// <param name="ub">the upper bound</param>
    /// <returns>the color</returns>
    public static Clr RandomRgba (in Random rng, in float lb = 0.0f, in float ub = 1.0f)
    {
        return new Clr (
            Utils.Mix (lb, ub, (float) rng.NextDouble ( )),
            Utils.Mix (lb, ub, (float) rng.NextDouble ( )),
            Utils.Mix (lb, ub, (float) rng.NextDouble ( )),
            Utils.Mix (lb, ub, (float) rng.NextDouble ( )));
    }

    /// <summary>
    /// Converts from a color's, red, green, blue and alpha
    /// channels to hue, saturation, lightness and alpha.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>hsla</returns>
    public static Vec4 RgbaToHsla (in Clr c)
    {
        float red = Utils.Clamp (c._r, 0.0f, 1.0f);
        float green = Utils.Clamp (c._g, 0.0f, 1.0f);
        float blue = Utils.Clamp (c._b, 0.0f, 1.0f);
        float alpha = Utils.Clamp (c._a, 0.0f, 1.0f);

        float gbmx = green > blue ? green : blue;
        float gbmn = green < blue ? green : blue;
        float mx = gbmx > red ? gbmx : red;
        float mn = gbmn < red ? gbmn : red;

        float sum = mx + mn;
        float diff = mx - mn;
        float light = sum * 0.5f;

        if (light <= Utils.One255)
        {
            return new Vec4 (Clr.HslHueShade, 0.0f, 0.0f, alpha);
        }
        else if (light >= 1.0f - Utils.One255)
        {
            return new Vec4 (Clr.HslHueDay, 0.0f, 1.0f, alpha);
        }
        else if (diff <= Utils.One255)
        {
            return new Vec4 (
                Utils.LerpAngleNear (
                    Clr.HslHueShade, Clr.HslHueDay,
                    light, 1.0f),
                0.0f, light, alpha);
        }
        else
        {
            float hue;
            if (Utils.Approx (red, mx, Utils.One255))
            {
                hue = (green - blue) / diff;
                if (green < blue)
                {
                    hue += 6.0f;
                }
            }
            else if (Utils.Approx (green, mx, Utils.One255))
            {
                hue = 2.0f + (blue - red) / diff;
            }
            else
            {
                hue = 4.0f + (red - green) / diff;
            }

            float sat = light > 0.5f ? diff / (2.0f - sum) : diff / sum;
            return new Vec4 (hue * Utils.OneSix, sat, light, alpha);
        }
    }

    /// <summary>
    /// Converts from a color's, red, green, blue and alpha
    /// channels to hue, saturation, value and alpha.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>hsva</returns>
    public static Vec4 RgbaToHsva (in Clr c)
    {
        float red = Utils.Clamp (c._r, 0.0f, 1.0f);
        float green = Utils.Clamp (c._g, 0.0f, 1.0f);
        float blue = Utils.Clamp (c._b, 0.0f, 1.0f);
        float alpha = Utils.Clamp (c._a, 0.0f, 1.0f);

        float gbmx = green > blue ? green : blue;
        float gbmn = green < blue ? green : blue;
        float mx = gbmx > red ? gbmx : red;

        if (mx <= Utils.One255)
        {
            return new Vec4 (Clr.HslHueShade, 0.0f, 0.0f, alpha);
        }
        else
        {
            float mn = gbmn < red ? gbmn : red;
            float diff = mx - mn;
            if (diff <= Utils.One255)
            {
                float light = (mx + mn) * 0.5f;
                if (light >= 1.0f - Utils.One255)
                {
                    return new Vec4 (Clr.HslHueDay, 0.0f, 1.0f, alpha);
                }
                else
                {
                    return new Vec4 (
                        Utils.LerpAngleNear (
                            Clr.HslHueShade, Clr.HslHueDay,
                            light, 1.0f),
                        0.0f, mx, alpha);
                }
            }
            else
            {
                float hue;
                if (Utils.Approx (red, mx, Utils.One255))
                {
                    hue = (green - blue) / diff;
                    if (green < blue) { hue += 6.0f; }
                }
                else if (Utils.Approx (green, mx, Utils.One255))
                {
                    hue = 2.0f + (blue - red) / diff;
                }
                else
                {
                    hue = 4.0f + (red - green) / diff;
                }

                return new Vec4 (hue * Utils.OneSix, diff / mx, mx, alpha);
            }
        }
    }

    /// <summary>
    /// Returns the relative luminance of the color.
    /// Converts the color from standard RGB to linear.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>the luminance</returns>
    public static float StandardLuminance (in Clr c)
    {
        return Clr.LinearLuminance (Clr.StandardToLinear (c));
    }

    /// <summary>
    /// Converts a color from standard RGB (sRGB) to linear RGB.
    /// </summary>
    /// <param name="c">the standard color</param>
    /// <param name="alpha">transform the alpha channel</param>
    /// <returns>the linear color</returns>
    public static Clr StandardToLinear (in Clr c, in bool alpha = false)
    {
        double inv1055 = 1.0d / 1.055d;
        return new Clr (
            c._r > 0.04045f ?
            (float) Math.Pow ((c._r + 0.055d) * inv1055, 2.4d) :
            c._r * 0.07739938f,

            c._g > 0.04045f ?
            (float) Math.Pow ((c._g + 0.055d) * inv1055, 2.4d) :
            c._g * 0.07739938f,

            c._b > 0.04045f ?
            (float) Math.Pow ((c._b + 0.055d) * inv1055, 2.4d) :
            c._b * 0.07739938f,

            alpha ? c._a > 0.04045f ?
            (float) Math.Pow ((c._a + 0.055d) * inv1055, 2.4d) :
            c._a * 0.07739938f :
            c._a);
    }

    /// <summary>
    /// Converts a color to an integer where hexadecimal represents the ARGB
    /// color channels: 0xAARRGGB .
    /// </summary>
    /// <param name="c">the input color</param>
    /// <returns>the color in hexadecimal</returns>
    public static int ToHexInt (in Clr c)
    {
        return Clr.ToHexIntUnchecked (Clr.Clamp (c, 0.0f, 1.0f));
    }

    /// <summary>
    /// Converts a color to an integer where hexadecimal represents the ARGB
    /// color channels: 0xAARRGGB . Does not check if the color's components
    /// are in a valid range, [0.0, 1.0].
    /// </summary>
    /// <param name="c">the input color</param>
    /// <returns>the color in hexadecimal</returns>
    public static int ToHexIntUnchecked (in Clr c)
    {
        return (int) (c._a * 0xff + 0.5f) << 0x18 |
            (int) (c._r * 0xff + 0.5f) << 0x10 |
            (int) (c._g * 0xff + 0.5f) << 0x08 |
            (int) (c._b * 0xff + 0.5f);
    }

    /// <summary>
    /// Returns a string representation of a color in web-friendly hexadecimal
    /// format, i.e., in RRGGBB order. Does not prepend a hash tag. Clamps all
    /// values to [0.0, 1.0] before converting to a byte.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>string</returns>
    public static string ToHexWeb (in Clr c)
    {
        return Clr.ToHexWeb (new StringBuilder (6), c).ToString ( );
    }

    /// <summary>
    /// Appends a string representation of a color in web-friendly hexdecimal
    /// to a string builder. Does not prepend a hash tag. Clamps all values
    /// to [0.0, 1.0] before converting to a byte.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="c">color</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToHexWeb (in StringBuilder sb, in Clr c)
    {
        int r = (int) (Utils.Clamp (c._r, 0.0f, 1.0f) * 0xff + 0.5f);
        int g = (int) (Utils.Clamp (c._g, 0.0f, 1.0f) * 0xff + 0.5f);
        int b = (int) (Utils.Clamp (c._b, 0.0f, 1.0f) * 0xff + 0.5f);
        sb.AppendFormat ("{0:X6}", (r << 0x10 | g << 0x08 | b));
        return sb;
    }

    /// <summary>
    /// Returns a string representation of a color.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string</returns>
    public static string ToString (in Clr c, in int places = 4)
    {
        return Clr.ToString (new StringBuilder (96), c, places).ToString ( );
    }

    /// <summary>
    /// Appends a representation of a color to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="c">color</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString (in StringBuilder sb, in Clr c, in int places = 4)
    {
        sb.Append ("{ r: ");
        Utils.ToFixed (sb, c._r, places);
        sb.Append (", g: ");
        Utils.ToFixed (sb, c._g, places);
        sb.Append (", b: ");
        Utils.ToFixed (sb, c._b, places);
        sb.Append (", a: ");
        Utils.ToFixed (sb, c._a, places);
        sb.Append (' ');
        sb.Append ('}');
        return sb;
    }

    /// <summary>
    /// Converts a color from CIE XYZ to CIE LAB.
    /// Stores alpha in the w component,
    /// luminance in the z component,
    /// a in the x component and b in the y component.
    /// </summary>
    /// <param name="xyz">the XYZ color</param>
    /// <returns>the lab color</returns>
    public static Vec4 XyzaToLaba (in Vec4 xyz)
    {
        double oneThird = 1.0d / 3.0d;
        double offset = 16.0d / 116.0d;

        double a = xyz.x * 1.0521110608435826d;
        if (a > 0.008856d)
        {
            a = Math.Pow (a, oneThird);
        }
        else
        {
            a = 7.787d * a + offset;
        }

        double b = xyz.y;
        if (b > 0.008856d)
        {
            b = Math.Pow (b, oneThird);
        }
        else
        {
            b = 7.787d * b + offset;
        }

        double c = xyz.z * 0.9184170164304805d;
        if (c > 0.008856d)
        {
            c = Math.Pow (c, oneThird);
        }
        else
        {
            c = 7.787d * c + offset;
        }

        return new Vec4 (
            (float) (500.0d * (a - b)), // a
            (float) (200.0d * (b - c)), // b
            (float) (116.0d * b - 16.0d), // l
            xyz.w);
    }

    /// <summary>
    /// Converts a color from CIE XYZ to linear RGB.
    /// The alpha channel is unaffected by the transformation.
    /// </summary>
    /// <param name="c">the XYZ color</param>
    /// <returns>the linear color</returns>
    public static Clr XyzaToLinear (in Vec4 v)
    {
        return new Clr (
            3.2408123f * v.x - 1.5373085f * v.y - 0.49858654f * v.z, //
            -0.969243f * v.x + 1.8759663f * v.y + 0.041555032f * v.z, //
            0.0556384f * v.x - 0.20400746f * v.y + 1.0571296f * v.z, //
            v.w);
    }

    /// <summary>
    /// Returns the color black, (0.0, 0.0, 0.0, 1.0) .
    /// </summary>
    /// <value>black</value>
    public static Clr Black { get { return new Clr (0.0f, 0.0f, 0.0f, 1.0f); } }

    /// <summary>
    /// Returns the color blue, (0.0, 0.0, 1.0, 1.0) .
    /// </summary>
    /// <value>blue</value>
    public static Clr Blue { get { return new Clr (0.0f, 0.0f, 1.0f, 1.0f); } }

    /// <summary>
    /// Returns the color clear black, (0.0, 0.0, 0.0, 0.0) .
    /// </summary>
    /// <value>clear black</value>
    public static Clr ClearBlack { get { return new Clr (0.0f, 0.0f, 0.0f, 0.0f); } }

    /// <summary>
    /// Returns the color clear white, (1.0, 1.0, 1.0, 0.0) .
    /// </summary>
    /// <value>clear white</value>
    public static Clr ClearWhite { get { return new Clr (1.0f, 1.0f, 1.0f, 0.0f); } }

    /// <summary>
    /// Returns the color cyan, (0.0, 1.0, 1.0, 1.0) .
    /// </summary>
    /// <value>cyan</value>
    public static Clr Cyan { get { return new Clr (0.0f, 1.0f, 1.0f, 1.0f); } }

    /// <summary>
    /// Returns the color green, (0.0, 1.0, 0.0, 1.0) .
    /// </summary>
    /// <value>green</value>
    public static Clr Green { get { return new Clr (0.0f, 1.0f, 0.0f, 1.0f); } }

    /// <summary>
    /// Returns the color magenta, (1.0, 0.0, 1.0, 1.0) .
    /// </summary>
    /// <value>magenta</value>
    public static Clr Magenta { get { return new Clr (1.0f, 0.0f, 1.0f, 1.0f); } }

    /// <summary>
    /// Returns the color red, (1.0, 0.0, 0.0, 1.0) .
    /// </summary>
    /// <value>red</value>
    public static Clr Red { get { return new Clr (1.0f, 0.0f, 0.0f, 1.0f); } }

    /// <summary>
    /// Returns the color yellow, (1.0, 1.0, 0.0, 1.0) .
    /// </summary>
    /// <value>yellow</value>
    public static Clr Yellow { get { return new Clr (1.0f, 1.0f, 0.0f, 1.0f); } }

    /// <summary>
    /// Returns the color white, (1.0, 1.0, 1.0, 1.0) .
    /// </summary>
    /// <value>white</value>
    public static Clr White { get { return new Clr (1.0f, 1.0f, 1.0f, 1.0f); } }
}