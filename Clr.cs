using System;
using System.Collections;
using System.Text;

/// <summary>
/// A readonly struct. Supports RGBA and HSBA color
/// spaces. Supports conversion to and from integers where
/// color channels are in the format 0xAARRGGBB.
/// </summary>
[Serializable]
public readonly struct Clr : IComparable<Clr>, IEquatable<Clr>, IEnumerable
{
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

    public Clr (byte r = 255, byte g = 255, byte b = 255, byte a = 255)
    {
        this._r = r * Utils.One255;
        this._g = g * Utils.One255;
        this._b = b * Utils.One255;
        this._a = a * Utils.One255;
    }

    public Clr (sbyte r = -1, sbyte g = -1, sbyte b = -1, sbyte a = -1)
    {
        this._r = (((int) r) & 0xff) * Utils.One255;
        this._g = (((int) g) & 0xff) * Utils.One255;
        this._b = (((int) b) & 0xff) * Utils.One255;
        this._a = (((int) a) & 0xff) * Utils.One255;
    }

    public Clr (float r = 1.0f, float g = 1.0f, float b = 1.0f, float a = 1.0f)
    {
        this._r = Utils.Clamp (r, 0.0f, 1.0f);
        this._g = Utils.Clamp (g, 0.0f, 1.0f);
        this._b = Utils.Clamp (b, 0.0f, 1.0f);
        this._a = Utils.Clamp (a, 0.0f, 1.0f);
    }

    public override bool Equals (object value)
    {
        if (Object.ReferenceEquals (this, value)) return true;
        if (Object.ReferenceEquals (null, value)) return false;

        if (value is Clr)
        {
            Clr c = (Clr) value;
            return ((int) this) == ((int) c);
        }

        return false;
    }

    public override int GetHashCode ( )
    {
        return (int) this;
    }

    public override string ToString ( )
    {
        return ToString (4);
    }

    public int CompareTo (Clr c)
    {
        float alum = Clr.Luminance (this);
        float blum = Clr.Luminance (c);

        return (alum > blum) ? 1 :
            (alum < blum) ? -1 :
            0;
    }

    public bool Equals (Clr c)
    {
        return ((int) this) == ((int) c);
    }

    public IEnumerator GetEnumerator ( )
    {
        yield return this._r;
        yield return this._g;
        yield return this._b;
        yield return this._a;
    }

    public float[ ] ToArray ( )
    {
        return new float[ ] { this._r, this._g, this._b, this._a };
    }

    public string ToString (int places = 4)
    {
        return new StringBuilder (96)
            .Append ("{ r: ")
            .Append (Utils.ToFixed (this._r, places))
            .Append (", g: ")
            .Append (Utils.ToFixed (this._g, places))
            .Append (", b: ")
            .Append (Utils.ToFixed (this._b, places))
            .Append (", a: ")
            .Append (Utils.ToFixed (this._a, places))
            .Append (" }")
            .ToString ( );
    }

    public (float r, float g, float b, float a) ToTuple ( )
    {
        return (r: this._r, g: this._g, b: this._b, a: this._a);
    }

    public static explicit operator Clr (bool b)
    {
        float v = b ? 1.0f : 0.0f;
        return new Clr (v, v, v, v);
    }

    public static explicit operator Clr (byte ub)
    {
        return new Clr (ub, ub, ub, ub);
    }

    public static explicit operator Clr (sbyte sb)
    {
        return new Clr (sb, sb, sb, sb);
    }

    public static explicit operator Clr (int c)
    {
        return Clr.FromHex (c);
    }

    public static explicit operator Clr (uint c)
    {
        return Clr.FromHex (c);
    }

    public static explicit operator Clr (long c)
    {
        return Clr.FromHex (c);
    }

    public static explicit operator Clr (float v)
    {
        return new Clr (v, v, v, v);
    }

    public static explicit operator bool (in Clr c)
    {
        return Clr.Any (c);
    }

    public static explicit operator int (in Clr c)
    {
        return Clr.ToHexInt (c);
    }

    public static explicit operator uint (in Clr c)
    {
        int cint = (int) c;
        return (uint) cint;
    }

    public static explicit operator long (in Clr c)
    {
        int cint = (int) c;
        return ((long) cint) & 0xffffffffL;
    }

    public static explicit operator float (in Clr c)
    {
        return Clr.Luminance (c);
    }

    public static bool operator true (in Clr c)
    {
        return Clr.Any (c);
    }

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
    /// Converts two colors to integers, performs the bitwise and
    /// operation on them, then converts the result to a color.
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
    /// Converts a color to an integer, performs a bitwise left shift
    /// operation, then converts the result to a color. To shift a whole color
    /// channel, use increments of 8 (8, 16, 24).
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
    /// Inverts the color's red, green and blue channels by subtracting them
    /// from 1.0 . Similar to to using the '~' operator except for the alpha
    /// channel.
    /// </summary>
    /// <param name="c">the color</param>
    /// <returns>the inversion</returns>
    public static Clr operator - (in Clr c)
    {
        // return new Clr (
        //     Utils.Max (1.0f - c._r, 0.0f),
        //     Utils.Max (1.0f - c._g, 0.0f),
        //     Utils.Max (1.0f - c._b, 0.0f),
        //     Utils.Clamp (c._a, 0.0f, 1.0f));

        return new Clr (
            1.0f - c._r,
            1.0f - c._g,
            1.0f - c._b,
            c._a);
    }

    /// <summary>
    /// Multiplies the left and right operand, except for the alpha channel. The
    /// left operand's alpha channel is retained.
    ///
    /// For that reason, color multiplication is _not_ commutative.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the product</returns>
    public static Clr operator * (in Clr a, in Clr b)
    {
        // return new Clr (
        //     Utils.Clamp (a._r * b._r, 0.0f, 1.0f),
        //     Utils.Clamp (a._g * b._g, 0.0f, 1.0f),
        //     Utils.Clamp (a._b * b._b, 0.0f, 1.0f),
        //     Utils.Clamp (a._a, 0.0f, 1.0f));

        return new Clr (
            a._r * b._r,
            a._g * b._g,
            a._b * b._b,
            a._a);
    }

    /// <summary>
    /// Divides the left operand by the right, except for the alpha channel. The
    /// left operand's alpha channel is retained.
    /// </summary>
    /// <param name="a">left operand, numerator</param>
    /// <param name="b">right operand, denominator</param>
    /// <returns>the quotient</returns>
    public static Clr operator / (in Clr a, in Clr b)
    {
        // return new Clr (
        //     Utils.Clamp (Utils.Div (a._r, b._r), 0.0f, 1.0f),
        //     Utils.Clamp (Utils.Div (a._g, b._g), 0.0f, 1.0f),
        //     Utils.Clamp (Utils.Div (a._b, b._b), 0.0f, 1.0f),
        //     Utils.Clamp (a._a, 0.0f, 1.0f));

        return new Clr (
            Utils.Div (a._r, b._r),
            Utils.Div (a._g, b._g),
            Utils.Div (a._b, b._b),
            a._a);
    }

    /// <summary>
    /// Applies modulo to the left operand with the components of the
    /// right. The left operand's alpha channel is retained.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the color</returns>
    public static Clr operator % (in Clr a, in Clr b)
    {
        // return new Clr (
        //     Utils.Clamp (Utils.Mod (a._r, b._r), 0.0f, 1.0f),
        //     Utils.Clamp (Utils.Mod (a._g, b._g), 0.0f, 1.0f),
        //     Utils.Clamp (Utils.Mod (a._b, b._b), 0.0f, 1.0f),
        //     Utils.Clamp (a._a, 0.0f, 1.0f));

        return new Clr (
            Utils.Fmod (a._r, b._r),
            Utils.Fmod (a._g, b._g),
            Utils.Fmod (a._b, b._b),
            a._a);
    }

    /// <summary>
    /// Adds the left and right operand, except for the alpha channel. The left
    /// operand's alpha channel is retained.
    ///
    /// For that reason, color addition is _not_ commutative.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the sum</returns>
    public static Clr operator + (in Clr a, in Clr b)
    {
        // return new Clr (
        //     Utils.Clamp (a._r + b._r, 0.0f, 1.0f),
        //     Utils.Clamp (a._g + b._g, 0.0f, 1.0f),
        //     Utils.Clamp (a._b + b._b, 0.0f, 1.0f),
        //     Utils.Clamp (a._a, 0.0f, 1.0f));

        return new Clr (
            a._r + b._r,
            a._g + b._g,
            a._b + b._b,
            a._a);
    }

    /// <summary>
    /// Subtracts the right operand from the left operand, except for the alpha
    /// channel. The left operand's alpha channel is retained.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the difference</returns>
    public static Clr operator - (in Clr a, in Clr b)
    {
        // return new Clr (
        //     Utils.Clamp (a._r - b._r, 0.0f, 1.0f),
        //     Utils.Clamp (a._g - b._g, 0.0f, 1.0f),
        //     Utils.Clamp (a._b - b._b, 0.0f, 1.0f),
        //     Utils.Clamp (a._a, 0.0f, 1.0f));

        return new Clr (
            a._r - b._r,
            a._g - b._g,
            a._b - b._b,
            a._a);
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
    /// Tests to see if the alpha channel of the color is 
    /// greater than zero, i.e., if it has some opacity.
    /// </summary>
    /// <param name="c">the color</param>
    /// <returns>the evaluation</returns>
    public static bool Any (in Clr c)
    {
        return c._a > 0.0f;
    }

    public static Clr Clamp (in Clr c, in float lb = 0.0f, in float ub = 1.0f)
    {
        return new Clr (
            Utils.Clamp (c._r, lb, ub),
            Utils.Clamp (c._g, lb, ub),
            Utils.Clamp (c._b, lb, ub),
            Utils.Clamp (c._a, lb, ub));
    }

    public static Clr Clamp (in Clr c, in Clr lb, in Clr ub)
    {
        return new Clr (
            Utils.Clamp (c._r, lb._r, ub._r),
            Utils.Clamp (c._g, lb._g, ub._g),
            Utils.Clamp (c._b, lb._b, ub._b),
            Utils.Clamp (c._a, lb._a, ub._a));
    }

    public static bool Contains (in Clr c, in float v)
    {
        if (Utils.Approx (c._a, v)) { return true; }
        if (Utils.Approx (c._b, v)) { return true; }
        if (Utils.Approx (c._g, v)) { return true; }
        if (Utils.Approx (c._r, v)) { return true; }
        return false;
    }

    public static Clr FromHex (in int c)
    {
        return new Clr (
            (c >> 0x10 & 0xff) * Utils.One255,
            (c >> 0x8 & 0xff) * Utils.One255,
            (c & 0xff) * Utils.One255,
            (c >> 0x18 & 0xff) * Utils.One255);
    }

    public static Clr FromHex (in uint c)
    {
        return new Clr (
            (c >> 0x10 & 0xff) * Utils.One255,
            (c >> 0x8 & 0xff) * Utils.One255,
            (c & 0xff) * Utils.One255,
            (c >> 0x18 & 0xff) * Utils.One255);
    }

    public static Clr FromHex (in long c)
    {
        return new Clr (
            (c >> 0x10 & 0xff) * Utils.One255,
            (c >> 0x8 & 0xff) * Utils.One255,
            (c & 0xff) * Utils.One255,
            (c >> 0x18 & 0xff) * Utils.One255);
    }

    public static Clr HsbaToRgba (in Vec4 v)
    {
        return HsbaToRgba (v.x, v.y, v.z, v.w);
    }

    public static Clr HsbaToRgba (in float hue = 1.0f, in float sat = 1.0f, in float bri = 1.0f, in float alpha = 1.0f)
    {
        if (sat <= 0.0f) return new Clr (bri, bri, bri, alpha);

        float h = Utils.Mod1 (hue) * 6.0f;
        int sector = (int) h;
        float secf = (float) sector;

        float tint1 = bri * (1.0f - sat);
        float tint2 = bri * (1.0f - sat * (h - secf));
        float tint3 = bri * (1.0f - sat * (1.0f + secf - h));

        switch (sector)
        {
            case 0:
                return new Clr (bri, tint3, tint1, alpha);
            case 1:
                return new Clr (tint2, bri, tint1, alpha);
            case 2:
                return new Clr (tint1, bri, tint3, alpha);
            case 3:
                return new Clr (tint1, tint2, bri, alpha);
            case 4:
                return new Clr (tint3, tint1, bri, alpha);
            case 5:
                return new Clr (bri, tint1, tint2, alpha);
            default:
                return Clr.White;
        }
    }

    /// <summary>
    /// Generates a clamped linear step for an input factor; to
    /// be used in conjunction with a mixing function.
    /// </summary>
    /// <param name="edge0">left edge</param>
    /// <param name="edge1">right edge</param>
    /// <param name="x">factor</param>
    /// <returns>the linear step</returns>
    public static Clr LinearStep (in Clr edge0, in Clr edge1, in Clr x)
    {
        return new Clr (
            Utils.Clamp (Utils.Div (x._r - edge0._r, edge1._r - edge0._r)),
            Utils.Clamp (Utils.Div (x._g - edge0._g, edge1._g - edge0._g)),
            Utils.Clamp (Utils.Div (x._b - edge0._b, edge1._b - edge0._b)),
            Utils.Clamp (Utils.Div (x._a - edge0._a, edge1._a - edge0._a)));
    }

    public static float Luminance (in Clr c)
    {
        return 0.2126f * c._r + 0.7152f * c._g + 0.0722f * c._b;
    }

    public static Clr Max (in Clr a, in Clr b)
    {
        return new Clr (
            Utils.Clamp (Utils.Max (a._r, b._r)),
            Utils.Clamp (Utils.Max (a._g, b._g)),
            Utils.Clamp (Utils.Max (a._b, b._b)),
            Utils.Clamp (Utils.Max (a._a, b._a)));
    }

    public static Clr Min (in Clr a, in Clr b)
    {
        return new Clr (
            Utils.Clamp (Utils.Min (a._r, b._r)),
            Utils.Clamp (Utils.Min (a._g, b._g)),
            Utils.Clamp (Utils.Min (a._b, b._b)),
            Utils.Clamp (Utils.Min (a._a, b._a)));
    }

    public static Clr MixHsba (in Clr a, in Clr b, in float t = 0.5f)
    {
        return Clr.HsbaToRgba (Vec4.Mix (Clr.RgbaToHsba (a), Clr.RgbaToHsba (b), t));
    }

    public static Clr MixHsba (in Clr a, in Clr b, in Vec4 t)
    {
        return Clr.HsbaToRgba (Vec4.Mix (Clr.RgbaToHsba (a), Clr.RgbaToHsba (b), t));
    }

    /// <summary>
    /// Mixes two colors by a step in the range [0.0, 1.0] .
    /// </summary>
    /// <param name="a">the origin color</param>
    /// <param name="b">the destination color</param>
    /// <param name="t">the step</param>
    /// <returns>the mixed color</returns>
    public static Clr MixRgba (in Clr a, in Clr b, in float t = 0.5f)
    {
        float u = 1.0f - t;
        return new Clr (
            u * a._r + t * b._r,
            u * a._g + t * b._g,
            u * a._b + t * b._b,
            u * a._a + t * b._a);
    }

    /// <summary>
    /// Mixes two colors using a third as a step.
    /// </summary>
    /// <param name="a">the origin color</param>
    /// <param name="b">the destination color</param>
    /// <param name="t">the step color</param>
    /// <returns>the mixed color</returns>
    public static Clr MixRgba (in Clr a, in Clr b, in Clr t)
    {
        return new Clr (
            (1.0f - t._r) * a._r + t._r * b._r,
            (1.0f - t._g) * a._g + t._g * b._g,
            (1.0f - t._b) * a._b + t._b * b._b,
            (1.0f - t._a) * a._a + t._a * b._a);
    }

    /// <summary>
    /// Mixes two colors using a Vec4 as a step.
    /// </summary>
    /// <param name="a">the origin color</param>
    /// <param name="b">the destination color</param>
    /// <param name="t">the step vector</param>
    /// <returns>the mixed color</returns>
    public static Clr MixRgba (in Clr a, in Clr b, in Vec4 t)
    {
        return new Clr (
            (1.0f - t.x) * a._r + t.x * b._r,
            (1.0f - t.y) * a._g + t.y * b._g,
            (1.0f - t.z) * a._b + t.z * b._b,
            (1.0f - t.w) * a._a + t.w * b._a);
    }

    /// <summary>
    /// Tests to see if the alpha channel of this color is less
    /// than or equal to zero, i.e., if it is completely
    /// transparent.
    /// </summary>
    /// <param name="c">the color</param>
    /// <returns>the evaluation</returns>
    public static bool None (in Clr c)
    {
        return c._a <= 0.0f;
    }

    /// <summary>
    /// Raises each component of the color, except for the
    /// alpha channel, to a power. Useful for gamma adjustment.
    /// </summary>
    /// <param name="a">the color</param>
    /// <param name="b">the power</param>
    /// <returns>the adjusted color</returns>
    public static Clr Pow (in Clr a, in float b)
    {
        return new Clr (
            Utils.Clamp (Utils.Pow (a._r, b), 0.0f, 1.0f),
            Utils.Clamp (Utils.Pow (a._g, b), 0.0f, 1.0f),
            Utils.Clamp (Utils.Pow (a._b, b), 0.0f, 1.0f),
            Utils.Clamp (a._a, 0.0f, 1.0f));
    }

    /// <summary>
    /// Multiplies the red, green and blue color channels of a
    /// color by the alpha channel.
    /// </summary>
    /// <param name="c">the color</param>
    /// <returns>the premultiplied color</returns>
    public static Clr PreMul (in Clr c)
    {
        if (c._a <= 0.0f)
        {
            return Clr.ClearBlack;
        }
        else if (c._a >= 1.0f)
        {
            return new Clr (c._r, c._g, c._b, 1.0f);
        }

        return new Clr (
            c._r * c._a,
            c._g * c._a,
            c._b * c._a,
            c._a);
    }

    /// <summary>
    /// Reduces the signal, or granularity, of a color's
    /// channels. Any level less than 2 or greater than 255
    /// returns sets the target to the input.
    /// </summary>
    /// <param name="c">the color</param>
    /// <param name="levels">the levels</param>
    /// <returns>the posterized color</returns>
    public static Clr Quantize (in Clr c, int levels = 127)
    {
        if (levels < 2 || levels > 255)
        {
            return new Clr (c._r, c._g, c._b, c._a);
        }
        float levf = (float) levels;
        float delta = 1.0f / levf;
        return new Clr (
            delta * Utils.Floor (0.5f + c._r * levf),
            delta * Utils.Floor (0.5f + c._g * levf),
            delta * Utils.Floor (0.5f + c._b * levf),
            delta * Utils.Floor (0.5f + c._a * levf));
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
        float xFac = (float) rng.NextDouble ( );
        float yFac = (float) rng.NextDouble ( );
        float zFac = (float) rng.NextDouble ( );
        float wFac = (float) rng.NextDouble ( );

        return new Clr (
            Utils.Mix (lb, ub, xFac),
            Utils.Mix (lb, ub, yFac),
            Utils.Mix (lb, ub, zFac),
            Utils.Mix (lb, ub, wFac));
    }

    /// <summary>
    /// Creates a random color from a lower- and upper-bound.
    /// </summary>
    /// <param name="rng">the random number generator</param>
    /// <param name="lb">the lower bound</param>
    /// <param name="ub">the upper bound</param>
    /// <returns>the color</returns>
    public static Clr RandomRgba (in Random rng, in Clr lb, in Clr ub)
    {
        float xFac = (float) rng.NextDouble ( );
        float yFac = (float) rng.NextDouble ( );
        float zFac = (float) rng.NextDouble ( );
        float wFac = (float) rng.NextDouble ( );

        return new Clr (
            Utils.Mix (lb._r, ub._r, xFac),
            Utils.Mix (lb._g, ub._g, yFac),
            Utils.Mix (lb._b, ub._b, zFac),
            Utils.Mix (lb._a, ub._a, wFac));
    }

    /// <summary>
    /// Converts a color to a vector which holds hue,
    /// saturation, brightness and alpha.
    /// </summary>
    /// <param name="c">the color</param>
    /// <returns>the output vector</returns>
    public static Vec4 RgbaToHsba (in Clr c)
    {
        return RgbaToHsba (c._r, c._g, c._b, c._a);
    }

    /// <summary>
    /// Converts RGBA channels to a vector which holds hue,
    /// saturation, brightness and alpha.
    /// </summary>
    /// <param name="red">the red channel</param>
    /// <param name="green">the green channel</param>
    /// <param name="blue">the blue channel</param>
    /// <param name="alpha">the alpha channel</param>
    /// <returns>the output vector</returns>
    public static Vec4 RgbaToHsba (in float red = 1.0f, in float green = 1.0f, in float blue = 1.0f, in float alpha = 1.0f)
    {
        float bri = Utils.Max (red, green, blue);
        float mn = Utils.Min (red, green, blue);
        float delta = bri - mn;
        float hue = 0.0f;

        if (delta != 0.0f)
        {
            if (red == bri)
                hue = (green - blue) / delta;
            else if (green == bri)
                hue = 2.0f + (blue - red) / delta;
            else
                hue = 4.0f + (red - green) / delta;

            hue *= Utils.OneSix;
            if (hue < 0.0f) hue += 1.0f;
        }

        float sat = bri == 0.0f ? 0.0f : delta / bri;
        return new Vec4 (hue, sat, bri, alpha);
    }

    /// <summary>
    /// Generates a clamped Hermite step for an input factor; to
    /// be used in conjunction with a mixing function.
    /// </summary>
    /// <param name="edge0">left edge</param>
    /// <param name="edge1">right edge</param>
    /// <param name="x">factor</param>
    /// <returns>the smooth step</returns>
    public static Clr SmoothStep (in Clr edge0, in Clr edge1, in Clr x)
    {
        float tx = Utils.Clamp (Utils.Div (x._r - edge0._r, edge1._r - edge0._r));
        float ty = Utils.Clamp (Utils.Div (x._g - edge0._g, edge1._g - edge0._g));
        float tz = Utils.Clamp (Utils.Div (x._b - edge0._b, edge1._b - edge0._b));
        float tw = Utils.Clamp (Utils.Div (x._a - edge0._a, edge1._a - edge0._a));

        return new Clr (
            tx * tx * (3.0f - (tx + tx)),
            ty * ty * (3.0f - (ty + ty)),
            tz * tz * (3.0f - (tz + tz)),
            tw * tw * (3.0f - (tw + tw)));
    }

    /// <summary>
    /// Generates a clamped boolean step for an input factor; to
    /// be used in conjunction with a mixing function.
    /// </summary>
    /// <param name="edge">edge</param>
    /// <param name="x">factor</param>
    /// <returns>the step</returns>
    public static Clr Step (in Clr edge, in Clr x)
    {
        return new Clr (
            x._r < edge._r ? 0.0f : 1.0f,
            x._g < edge._g ? 0.0f : 1.0f,
            x._b < edge._b ? 0.0f : 1.0f,
            x._a < edge._a ? 0.0f : 1.0f);
    }

    /// <summary>
    /// Converts a color to an integer where hexadecimal
    /// represents the ARGB color channels: 0xAARRGGB .
    /// </summary>
    /// <param name="c">the input color</param>
    /// <returns>the color in hexadecimal</returns>
    public static int ToHexInt (in Clr c)
    {
        return (int) (c._a * 0xff + 0.5f) << 0x18 |
            (int) (c._r * 0xff + 0.5f) << 0x10 |
            (int) (c._g * 0xff + 0.5f) << 0x8 |
            (int) (c._b * 0xff + 0.5f);
    }

    /// <summary>
    /// Returns the color black, (0.0, 0.0, 0.0, 1.0) .
    /// </summary>
    /// <value>black</value>
    public static Clr Black
    {
        get
        {
            return new Clr (0.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    /// <summary>
    /// Returns the color blue, (0.0, 0.0, 1.0, 1.0) .
    /// </summary>
    /// <value>blue</value>
    public static Clr Blue
    {
        get
        {
            return new Clr (0.0f, 0.0f, 1.0f, 1.0f);
        }
    }

    /// <summary>
    /// Returns the color clear black, (0.0, 0.0, 0.0, 0.0) .
    /// </summary>
    /// <value>clear black</value>
    public static Clr ClearBlack
    {
        get
        {
            return new Clr (0.0f, 0.0f, 0.0f, 0.0f);
        }
    }

    /// <summary>
    /// Returns the color clear white, (1.0, 1.0, 1.0, 0.0) .
    /// </summary>
    /// <value>clear white</value>
    public static Clr ClearWhite
    {
        get
        {
            return new Clr (1.0f, 1.0f, 1.0f, 0.0f);
        }
    }

    /// <summary>
    /// Returns the color cyan, (0.0, 1.0, 1.0, 1.0) .
    /// </summary>
    /// <value>cyan</value>
    public static Clr Cyan
    {
        get
        {
            return new Clr (0.0f, 1.0f, 1.0f, 1.0f);
        }
    }

    /// <summary>
    /// Returns the color green, (0.0, 1.0, 0.0, 1.0) .
    /// </summary>
    /// <value>green</value>
    public static Clr Green
    {
        get
        {
            return new Clr (0.0f, 1.0f, 0.0f, 1.0f);
        }
    }

    /// <summary>
    /// Returns the color magenta, (1.0, 0.0, 1.0, 1.0) .
    /// </summary>
    /// <value>magenta</value>
    public static Clr Magenta
    {
        get
        {
            return new Clr (1.0f, 0.0f, 1.0f, 1.0f);
        }
    }

    /// <summary>
    /// Returns the color red, (1.0, 0.0, 0.0, 1.0) .
    /// </summary>
    /// <value>red</value>
    public static Clr Red
    {
        get
        {
            return new Clr (1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    /// <summary>
    /// Returns the color yellow, (1.0, 1.0, 0.0, 1.0) .
    /// </summary>
    /// <value>yellow</value>
    public static Clr Yellow
    {
        get
        {
            return new Clr (1.0f, 1.0f, 0.0f, 1.0f);
        }
    }

    /// <summary>
    /// Returns the color white, (1.0, 1.0, 1.0, 1.0) .
    /// </summary>
    /// <value>white</value>
    public static Clr White
    {
        get
        {
            return new Clr (1.0f, 1.0f, 1.0f, 1.0f);
        }
    }
}