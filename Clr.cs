using System;
using System.Collections;
using System.Text;

/// <summary>
/// A readonly struct. Supports RGBA and HSBA color spaces. Supports conversion
/// to and from integers where color channels are in the format 0xAARRGGBB.
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

    /// <summary>
    /// Creates a color from unsigned bytes. Converts each to a single precision
    /// real number in the  range [0.0, 1.0] .
    /// </summary>
    /// <param name="r">red channel</param>
    /// <param name="g">green channel</param>
    /// <param name="b">blue channel</param>
    /// <param name="a">alpha channel</param>
    public Clr (byte r = 255, byte g = 255, byte b = 255, byte a = 255)
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
    public Clr (sbyte r = -1, sbyte g = -1, sbyte b = -1, sbyte a = -1)
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
    public Clr (float r = 1.0f, float g = 1.0f, float b = 1.0f, float a = 1.0f)
    {
        this._r = Utils.Clamp (r, 0.0f, 1.0f);
        this._g = Utils.Clamp (g, 0.0f, 1.0f);
        this._b = Utils.Clamp (b, 0.0f, 1.0f);
        this._a = Utils.Clamp (a, 0.0f, 1.0f);
    }

    /// <summary>
    /// Tests this color for equivalence with an object.
    /// </summary>
    /// <param name="value">the object</param>
    /// <returns>the equivalence</returns>
    public override bool Equals (object value)
    {
        if (Object.ReferenceEquals (this, value)) return true;
        if (Object.ReferenceEquals (null, value)) return false;
        if (value is Clr) return this.Equals ((Clr) value);
        return false;
    }

    /// <summary>
    /// Returns a hash code representing this color.
    /// </summary>
    /// <returns>the hash code</returns>
    public override int GetHashCode ( )
    {
        return (int) this;
    }

    /// <summary>
    /// Returns a string representation of this color.
    /// </summary>
    /// <returns>the string</returns>
    public override string ToString ( )
    {
        return this.ToString (4);
    }

    /// <summary>
    /// Compares this color to another in compliance with the IComparable
    /// interface.
    /// </summary>
    /// <param name="v">the comparisand</param>
    /// <returns>the evaluation</returns>
    public int CompareTo (Clr c)
    {
        float alum = Clr.Luminance (this);
        float blum = Clr.Luminance (c);

        return (alum > blum) ? 1 :
            (alum < blum) ? -1 :
            0;
    }

    /// <summary>
    /// Tests this color for equivalence with another in compliance with the
    /// IEquatable interface.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>the equivalence</returns>
    public bool Equals (Clr c)
    {
        return ((int) this) == ((int) c);
    }

    /// <summary>
    /// Returns an enumerator (or iterator) for this color, allowing its
    /// components to be accessed in a foreach loop.
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
    /// Returns a float array of length 4 containing this color's components. The alpha channel is last.
    /// </summary>
    /// <returns>the array</returns>
    public float[ ] ToArray ( )
    {
        return new float[ ] { this._r, this._g, this._b, this._a };
    }

    /// <summary>
    /// Returns a string representation of this color.
    /// </summary>
    /// <param name="places">number of decimal places</param>
    /// <returns>the string</returns>
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

    /// <summary>
    /// Returns a named value tuple containing this color's components.
    /// </summary>
    /// <returns>the tuple</returns>
    public (float r, float g, float b, float a) ToTuple ( )
    {
        return (r: this._r, g: this._g, b: this._b, a: this._a);
    }

    /// <summary>
    /// Converts a boolean to a color by supplying the boolean to all the
    /// color's components: 1.0 for true; 0.0 for false.
    /// </summary>
    /// <param name="b">the boolean</param>
    /// <returns>the color</returns>
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
        int cint = (int) c;
        return (uint) cint;
    }

    /// <summary>
    /// Converts a color to a long.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>the long</returns>
    public static explicit operator long (in Clr c)
    {
        int cint = (int) c;
        return ((long) cint) & 0xffffffffL;
    }

    /// <summary>
    /// Converts a color to a float by returning its luminance.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>the float</returns>
    public static explicit operator float (in Clr c)
    {
        return Clr.Luminance (c);
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
    /// Inverts the color's red, green and blue channels by subtracting them
    /// from 1.0 . Similar to to using the '~' operator except for the alpha
    /// channel.
    /// </summary>
    /// <param name="c">the color</param>
    /// <returns>the inversion</returns>
    public static Clr operator - (in Clr c)
    {
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
        return new Clr (
            Utils.Div (a._r, b._r),
            Utils.Div (a._g, b._g),
            Utils.Div (a._b, b._b),
            a._a);
    }

    /// <summary>
    /// Applies modulo to the left operand with the components of the right. The
    /// left operand's alpha channel is retained.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the color</returns>
    public static Clr operator % (in Clr a, in Clr b)
    {
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
    /// Clamps a color to a lower and upper bound.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="lb">lower bound</param>
    /// <param name="ub">upper bound</param>
    /// <returns>the clamped color</returns>
    public static Clr Clamp (in Clr c, in Clr lb, in Clr ub)
    {
        return new Clr (
            Utils.Clamp (c._r, lb._r, ub._r),
            Utils.Clamp (c._g, lb._g, ub._g),
            Utils.Clamp (c._b, lb._b, ub._b),
            Utils.Clamp (c._a, lb._a, ub._a));
    }

    /// <summary>
    /// Tests to see if a color contains a value.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="v">value</param>
    /// <returns>the evaluation</returns>
    public static bool Contains (in Clr c, in float v)
    {
        if (Utils.Approx (c._a, v)) { return true; }
        if (Utils.Approx (c._b, v)) { return true; }
        if (Utils.Approx (c._g, v)) { return true; }
        if (Utils.Approx (c._r, v)) { return true; }
        return false;
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
            (c >> 0x8 & 0xff) * Utils.One255,
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
            (c >> 0x8 & 0xff) * Utils.One255,
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
            (c >> 0x8 & 0xff) * Utils.One255,
            (c & 0xff) * Utils.One255,
            (c >> 0x18 & 0xff) * Utils.One255);
    }

    /// <summary>
    /// Converts from a vector representing hue, saturation and brightness to a
    /// color with red, green and blue channels.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>the color</returns>
    public static Clr HsbaToRgba (in Vec4 v)
    {
        return HsbaToRgba (v.x, v.y, v.z, v.w);
    }

    /// <summary>
    /// Converts from hue, saturation and brightness to a color with red, green
    /// and blue channels.
    /// </summary>
    /// <param name="hue">hue</param>
    /// <param name="sat">saturation</param>
    /// <param name="bri">brightness</param>
    /// <param name="alpha">alpha</param>
    /// <returns>the color</returns>
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
    /// Generates a clamped linear step for an input factor; to be used in
    /// conjunction with a mixing function.
    /// </summary>
    /// <param name="edge0">left edge</param>
    /// <param name="edge1">right edge</param>
    /// <param name="x">factor</param>
    /// <returns>the linear step</returns>
    public static Clr LinearStep (in Clr edge0, in Clr edge1, in Clr x)
    {
        return new Clr (
            Utils.LinearStep (edge0._r, edge1._r, x._r),
            Utils.LinearStep (edge0._g, edge1._g, x._g),
            Utils.LinearStep (edge0._b, edge1._b, x._b),
            Utils.LinearStep (edge0._a, edge1._a, x._a));
    }

    /// <summary>
    /// Returns the relative luminance of the color, based on
    /// https://en.wikipedia.org/wiki/Relative_luminance .
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>the luminance</returns>
    public static float Luminance (in Clr c)
    {
        return 0.2126f * c._r + 0.7152f * c._g + 0.0722f * c._b;
    }

    /// <summary>
    /// Finds the maximum for each channel of two input colors.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the maximum</returns>
    public static Clr Max (in Clr a, in Clr b)
    {
        return new Clr (
            Utils.Max (a._r, b._r),
            Utils.Max (a._g, b._g),
            Utils.Max (a._b, b._b),
            Utils.Max (a._a, b._a));
    }

    /// <summary>
    /// Finds the minimum for each channel of two input colors.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the minimum</returns>
    public static Clr Min (in Clr a, in Clr b)
    {
        return new Clr (
            Utils.Min (a._r, b._r),
            Utils.Min (a._g, b._g),
            Utils.Min (a._b, b._b),
            Utils.Min (a._a, b._a));
    }

    /// <summary>
    /// Mixes two colors according to their hue, saturation and brightness by a
    /// step in the range [0.0, 1.0] .
    /// </summary>
    /// <param name="a">origin color</param>
    /// <param name="b">destination color</param>
    /// <param name="t">step</param>
    /// <returns>the mixed color</returns>
    public static Clr MixHsba (in Clr a, in Clr b, in float t = 0.5f)
    {
        return Clr.HsbaToRgba (Vec4.Mix (Clr.RgbaToHsba (a), Clr.RgbaToHsba (b), t));
    }

    /// <summary>
    /// Mixes two colors according to their hue, saturation and brightness by a
    /// step in the range [0.0, 1.0] .
    /// </summary>
    /// <param name="a">origin color</param>
    /// <param name="b">destination color</param>
    /// <param name="t">step</param>
    /// <returns>the mixed color</returns>
    public static Clr MixHsba (in Clr a, in Clr b, in Vec4 t)
    {
        return Clr.HsbaToRgba (Vec4.Mix (Clr.RgbaToHsba (a), Clr.RgbaToHsba (b), t));
    }

    /// <summary>
    /// Mixes two colors by a step in the range [0.0, 1.0] .
    /// </summary>
    /// <param name="a">origin color</param>
    /// <param name="b">destination color</param>
    /// <param name="t">step</param>
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
    /// <param name="a">origin color</param>
    /// <param name="b">destination color</param>
    /// <param name="t">step color</param>
    /// <returns>the mixed color</returns>
    public static Clr MixRgba (in Clr a, in Clr b, in Clr t)
    {
        return new Clr (
            Utils.Mix (a._r, b._r, t._r),
            Utils.Mix (a._g, b._g, t._g),
            Utils.Mix (a._b, b._b, t._b),
            Utils.Mix (a._a, b._a, t._a));
    }

    /// <summary>
    /// Mixes two colors using a Vec4 as a step.
    /// </summary>
    /// <param name="a">origin color</param>
    /// <param name="b">destination color</param>
    /// <param name="t">step vector</param>
    /// <returns>the mixed color</returns>
    public static Clr MixRgba (in Clr a, in Clr b, in Vec4 t)
    {
        return new Clr (
            Utils.Mix (a._r, b._r, t.x),
            Utils.Mix (a._g, b._g, t.y),
            Utils.Mix (a._b, b._b, t.z),
            Utils.Mix (a._a, b._a, t.w));
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
    /// Raises each component of the color, except for the alpha channel, to a
    /// power. Useful for gamma adjustment.
    /// </summary>
    /// <param name="a">the color</param>
    /// <param name="b">the power</param>
    /// <returns>the adjusted color</returns>
    public static Clr Pow (in Clr a, in float b)
    {
        return new Clr (
            Utils.Pow (a._r, b),
            Utils.Pow (a._g, b),
            Utils.Pow (a._b, b),
            a._a);
    }

    /// <summary>
    /// Multiplies the red, green and blue color channels of a color by the
    /// alpha channel.
    /// </summary>
    /// <param name="c">the color</param>
    /// <returns>the premultiplied color</returns>
    public static Clr PreMul (in Clr c)
    {
        return new Clr (
            c._r * c._a,
            c._g * c._a,
            c._b * c._a,
            c._a);
    }

    /// <summary>
    /// Reduces the signal, or granularity, of a color's channels. Any level
    /// less than 2 or greater than 255 returns sets the target to the input.
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
        return new Clr (
            Utils.Mix (lb, ub, (float) rng.NextDouble ( )),
            Utils.Mix (lb, ub, (float) rng.NextDouble ( )),
            Utils.Mix (lb, ub, (float) rng.NextDouble ( )),
            Utils.Mix (lb, ub, (float) rng.NextDouble ( )));
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
        return new Clr (
            Utils.Mix (lb._r, ub._r, (float) rng.NextDouble ( )),
            Utils.Mix (lb._g, ub._g, (float) rng.NextDouble ( )),
            Utils.Mix (lb._b, ub._b, (float) rng.NextDouble ( )),
            Utils.Mix (lb._a, ub._a, (float) rng.NextDouble ( )));
    }

    /// <summary>
    /// Converts a color to a vector which holds hue, saturation, brightness and
    /// alpha.
    /// </summary>
    /// <param name="c">the color</param>
    /// <returns>the output vector</returns>
    public static Vec4 RgbaToHsba (in Clr c)
    {
        return RgbaToHsba (c._r, c._g, c._b, c._a);
    }

    /// <summary>
    /// Converts RGBA channels to a vector which holds hue, saturation,
    /// brightness and alpha.
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
            if (hue < 0.0f) ++hue;
        }

        float sat = bri != 0.0f ? delta / bri : 0.0f;
        return new Vec4 (hue, sat, bri, alpha);
    }

    /// <summary>
    /// Generates a clamped Hermite step for an input factor; to be used in
    /// conjunction with a mixing function.
    /// </summary>
    /// <param name="edge0">left edge</param>
    /// <param name="edge1">right edge</param>
    /// <param name="x">factor</param>
    /// <returns>the smooth step</returns>
    public static Clr SmoothStep (in Clr edge0, in Clr edge1, in Clr x)
    {
        return new Clr (
            Utils.SmoothStep (edge0._r, edge1._r, x._r),
            Utils.SmoothStep (edge0._g, edge1._g, x._g),
            Utils.SmoothStep (edge0._b, edge1._b, x._b),
            Utils.SmoothStep (edge0._a, edge1._a, x._a));
    }

    /// <summary>
    /// Generates a clamped boolean step for an input factor; to be used in
    /// conjunction with a mixing function.
    /// </summary>
    /// <param name="edge">edge</param>
    /// <param name="x">factor</param>
    /// <returns>the step</returns>
    public static Clr Step (in Clr edge, in Clr x)
    {
        return new Clr (
            Utils.Step (edge._r, x._r),
            Utils.Step (edge._g, x._g),
            Utils.Step (edge._b, x._b),
            Utils.Step (edge._a, x._a));
    }

    /// <summary>
    /// Converts a color to an integer where hexadecimal represents the ARGB
    /// color channels: 0xAARRGGB .
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