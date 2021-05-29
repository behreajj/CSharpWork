using System;
using System.Collections;
using System.Runtime.CompilerServices;
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
        // this._r = Utils.Clamp (r, 0.0f, 1.0f);
        // this._g = Utils.Clamp (g, 0.0f, 1.0f);
        // this._b = Utils.Clamp (b, 0.0f, 1.0f);
        // this._a = Utils.Clamp (a, 0.0f, 1.0f);

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
        return Clr.ToHexInt (this);
    }

    /// <summary>
    /// Returns a string representation of this color.
    /// </summary>
    /// <returns>the string</returns>
    public override string ToString ( )
    {
        // TODO: Update ToString to match Vec2,3,4
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
        int aint = Clr.ToHexInt (this);
        int bint = Clr.ToHexInt (c);
        return (aint > bint) ? 1 : (aint < bint) ? -1 : 0;
    }

    /// <summary>
    /// Tests this color for equivalence with another in compliance with the
    /// IEquatable interface.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>the equivalence</returns>
    public bool Equals (Clr c)
    {
        return Clr.ToHexInt (this) == Clr.ToHexInt (c);
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
    /// Returns a string representation of this color.
    /// </summary>
    /// <param name="places">number of decimal places</param>
    /// <returns>the string</returns>
    public string ToString (in int places = 4)
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
    /// color's channels: 1.0 for true; 0.0 for false.
    /// </summary>
    /// <param name="b">the boolean</param>
    /// <returns>the color</returns>
    public static explicit operator Clr (in bool b)
    {
        float v = b ? 1.0f : 0.0f;
        return new Clr (v, v, v, v);
    }

    /// <summary>
    /// Converts an unsign ed byte to a color by supplying it to all the color's
    /// channels.
    /// </summary>
    /// <param name="ub">value</param>
    public static explicit operator Clr (in byte ub)
    {
        return new Clr (ub, ub, ub, ub);
    }

    /// <summary>
    /// Converts a signed byte to a color by supplying it to all the color's
    /// channels.
    /// </summary>
    /// <param name="sb">value</param>
    public static explicit operator Clr (in sbyte sb)
    {
        return new Clr (sb, sb, sb, sb);
    }

    /// <summary>
    /// Converts a float to a color by supplying it to all the color's channels.
    /// </summary>
    /// <param name="v">value</param>
    public static explicit operator Clr (in float v)
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
        int cint = Clr.ToHexInt (c);
        return (uint) cint;
    }

    /// <summary>
    /// Converts a color to a long.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>the long</returns>
    public static explicit operator long (in Clr c)
    {
        int cint = Clr.ToHexInt (c);
        return ((long) cint) & 0xffffffffL;
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
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
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
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
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
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
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
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
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
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static bool Contains (in Clr c, in float v)
    {
        return Utils.Approx (c._a, v) ||
            Utils.Approx (c._b, v) ||
            Utils.Approx (c._g, v) ||
            Utils.Approx (c._r, v);
    }

    /// <summary>
    /// Convert a hexadecimal representation of a color stored as 0xAARRGGBB
    /// into a color.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>the color</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
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
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
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
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
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
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
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
    public static Clr HsbaToRgba ( //
        in float hue = 1.0f, //
        in float sat = 1.0f, //
        in float bri = 1.0f, //
        in float alpha = 1.0f)
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
    /// Converts from CIE LAB to CIE XYZ.
    /// </summary>
    /// <param name="lab">the lab color</param>
    /// <returns>the xyz color</returns>
    public static Vec4 LabToXYZ (Vec4 lab)
    {
        double offset = 16.0d / 116.0d;
        double one116 = 1.0d / 116.0d;
        double one7787 = 1.0d / 7.787d;

        double a = (lab.x + 16.0d) * one116;
        double b = lab.y * 0.002d + a;
        double c = a - lab.z * 0.005d;

        double acb = a * a * a;
        if (acb > 0.008856d) a = acb;
        else a = (a - offset) * one7787;

        double bcb = b * b * b;
        if (bcb > 0.008856d) b = bcb;
        else b = (b - offset) * one7787;

        double ccb = c * c * c;
        if (ccb > 0.008856d) c = ccb;
        else c = (c - offset) * one7787;

        return new Vec4 (
            (float) (b * 0.95047d),
            (float) a,
            (float) (c * 1.08883d),
            lab.w);
    }

    /// <summary>
    /// Returns the relative luminance of the color.
    /// Assumes the color is in linear RGB.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>the luminance</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float LinearLuminance (in Clr c)
    {
        return 0.21264935f * c.r +
            0.71516913f * c.g +
            0.07218152f * c.b;
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
            c.r <= 0.0031308f ?
            c.r * 12.92f :
            (float) (Math.Pow (c.r, inv24) * 1.055d - 0.055d),

            c.g <= 0.0031308f ?
            c.g * 12.92f :
            (float) (Math.Pow (c.g, inv24) * 1.055d - 0.055d),

            c.b <= 0.0031308f ?
            c.b * 12.92f :
            (float) (Math.Pow (c.b, inv24) * 1.055d - 0.055d),

            alpha ? c.a <= 0.0031308f ?
            c.a * 12.92f :
            (float) (Math.Pow (c.a, inv24) * 1.055d - 0.055d) :
            c.a);
    }

    /// <summary>
    /// Converts a color from linear RGB to CIE XYZ.
    /// The alpha channel is unaffected by the transformation.
    /// </summary>
    /// <param name="c">the linear color</param>
    /// <returns>the XYZ color</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec4 LinearToXYZ (in Clr c)
    {
        return new Vec4 (
            0.41241086f * c.r + 0.35758457f * c.g + 0.1804538f * c.b,
            0.21264935f * c.r + 0.71516913f * c.g + 0.07218152f * c.b,
            0.019331759f * c.r + 0.11919486f * c.g + 0.95039004f * c.b,
            c.a);
    }

    /// <summary>
    /// Finds the maximum for each channel of two input colors.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the maximum</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
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
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
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
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Clr MixHsba (in Clr a, in Clr b, in float t = 0.5f)
    {
        //TODO: Refactor to use lerpnear for hue?
        return Clr.HsbaToRgba (Vec4.Mix (Clr.RgbaToHsba (a), Clr.RgbaToHsba (b), t));
    }

    /// <summary>
    /// Mixes two colors by a 50-50 ratio.
    /// Assumes the colors are in linear RGB.
    /// </summary>
    /// <param name="a">origin color</param>
    /// <param name="b">destination color</param>
    /// <param name="t">step</param>
    /// <returns>the mixed color</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Clr MixRgbaLinear (in Clr a, in Clr b)
    {
        return new Clr (
            0.5f * (a._r + b._r),
            0.5f * (a._g + b._g),
            0.5f * (a._b + b._b),
            0.5f * (a._a + b._a));
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
    /// Mixes two colors by a 50-50 ratio.
    /// Assumes the colors are in standard RGB; converts them to
    /// linear, then mixes them, then converts to standard.
    /// </summary>
    /// <param name="a">origin color</param>
    /// <param name="b">destination color</param>
    /// <param name="t">step</param>
    /// <returns>the mixed color</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Clr MixRgbaStandard (in Clr a, in Clr b)
    {
        return Clr.LinearToStandard (
            Clr.MixRgbaLinear (
                Clr.StandardToLinear (a),
                Clr.StandardToLinear (b)));
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
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
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
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
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
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Clr Premul (in Clr c)
    {
        if (c.a <= 0.0f) return Clr.ClearBlack;
        if (c.a >= 1.0f) return new Clr (c.r, c.g, c.b, 1.0f);
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
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
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
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
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
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
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
    public static Vec4 RgbaToHsba ( //
        in float red = 1.0f, //
        in float green = 1.0f, //
        in float blue = 1.0f, // 
        in float alpha = 1.0f)
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

        float sat = (bri != 0.0f) ? delta / bri : 0.0f;
        return new Vec4 (hue, sat, bri, alpha);
    }

    /// <summary>
    /// Returns the relative luminance of the color.
    /// Converts the color from standard RGB to linear.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>the luminance</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
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
            c.r <= 0.04045f ?
            c.r * 0.07739938f :
            (float) Math.Pow ((c.r + 0.055d) * inv1055, 2.4d),

            c.g <= 0.04045f ?
            c.g * 0.07739938f :
            (float) Math.Pow ((c.g + 0.055d) * inv1055, 2.4d),

            c.b <= 0.04045f ?
            c.b * 0.07739938f :
            (float) Math.Pow ((c.b + 0.055d) * inv1055, 2.4d),

            alpha ? c.a <= 0.04045f ?
            c.a * 0.07739938f :
            (float) Math.Pow ((c.a + 0.055d) * inv1055, 2.4d) :
            c.a);
    }

    /// <summary>
    /// Converts a color to an integer where hexadecimal represents the ARGB
    /// color channels: 0xAARRGGB .
    /// </summary>
    /// <param name="c">the input color</param>
    /// <returns>the color in hexadecimal</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
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
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static int ToHexIntUnchecked (in Clr c)
    {
        return (int) (c._a * 0xff + 0.5f) << 0x18 |
            (int) (c._r * 0xff + 0.5f) << 0x10 |
            (int) (c._g * 0xff + 0.5f) << 0x8 |
            (int) (c._b * 0xff + 0.5f);
    }

    /// <summary>
    /// Converts a color from CIE XYZ to CIE LAB.
    /// </summary>
    /// <param name="xyz">the XYZ color</param>
    /// <returns>the lab color</returns>
    public static Vec4 XYZToLab (in Vec4 xyz)
    {
        double oneThird = 1.0d / 3.0d;
        double offset = 16.0d / 116.0d;

        double a = xyz.x * 1.0521110608435826d;
        if (a > 0.008856d) a = Math.Pow (a, oneThird);
        else a = 7.787d * a + offset;

        double b = xyz.y;
        if (b > 0.008856d) b = Math.Pow (b, oneThird);
        else b = 7.787d * b + offset;

        double c = xyz.z * 0.9184170164304805d;
        if (c > 0.008856d) c = Math.Pow (c, oneThird);
        else c = 7.787d * c + offset;

        return new Vec4 (
            (float) (116.0d * b - 16.0d),
            (float) (500.0d * (a - b)),
            (float) (200.0d * (b - c)),
            xyz.w);
    }

    /// <summary>
    /// Converts a color from CIE XYZ to linear RGB.
    /// The alpha channel is unaffected by the transformation.
    /// </summary>
    /// <param name="c">the XYZ color</param>
    /// <returns>the linear color</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Clr XYZToLinear (in Vec4 v)
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