using System;
using System.Text;

/// <summary>
/// A readonly struct. Represents colors in a perceptual
/// color space, such as CIE LAB, SR LAB 2, OK LAB, etc.
/// The a and b axes are signed, unbounded values.
/// Negative a indicates a green hue; positive, magenta.
/// Negative b indicates a blue hue; positive, yellow.
/// Lightness falls in the range [0.0, 100.0]; for a and
/// b, the practical range is roughly [-111.0, 111.0].
/// Alpha is expected to be in [0.0, 1.0].
/// </summary>
[Serializable]
public readonly struct Lab : IComparable<Lab>, IEquatable<Lab>
{
    /// <summary>
    /// A scalar to convert a number in [0, 255] to
    /// lightness in [0, 100]. Equivalent to 100.0 / 255.0.
    /// </summary>
    public const float LFrom255 = 0.39215687f;

    /// <summary>
    /// A scalar to convert lightness in [0, 100] to
    /// a number in [0, 255]. Equivalent to 255.0 / 100.0.
    /// </summary>
    public const float LTo255 = 2.55f;

    /// <summary>
    /// The green-magenta component.
    /// </summary>
    private readonly float a;

    /// <summary>
    /// The alpha (transparency) component.
    /// </summary>
    private readonly float alpha;

    /// <summary>
    /// The blue-yellow component.
    /// </summary>
    private readonly float b;

    /// <summary>
    /// The light component.
    /// </summary>
    private readonly float l;

    /// <summary>
    /// The green-magenta component.
    /// </summary>
    /// <value>green-magenta</value>
    public float A { get { return this.a; } }

    /// <summary>
    /// The alpha component.
    /// </summary>
    /// <value>alpha</value>
    public float Alpha { get { return this.alpha; } }

    /// <summary>
    /// The blue-yellow component.
    /// </summary>
    /// <value>blue-yellow</value>
    public float B { get { return this.b; } }

    /// <summary>
    /// The light component.
    /// </summary>
    /// <value>light</value>
    public float L { get { return this.l; } }

    /// <summary>
    /// Creates a LAB color from single precision real numbers.
    /// </summary>
    /// <param name="l">lightness</param>
    /// <param name="a">green-magenta</param>
    /// <param name="b">blue-yellow</param>
    /// <param name="alpha">alpha</param>
    public Lab(in float l, in float a, in float b, in float alpha = 1.0f)
    {
        this.l = l;
        this.a = a;
        this.b = b;
        this.alpha = alpha;
    }

    /// <summary>
    /// Creates a LAB color from signed bytes.
    /// Converts each to a single precision real number.
    /// Scales lightness from [0, 255] to [0.0, 100.0].
    /// Shifts a and b from [0, 255] to [-128, 127].
    /// Scales alpha from [0, 255] to [0.0, 1.0].
    /// </summary>
    /// <param name="l">lightness</param>
    /// <param name="a">green-magenta</param>
    /// <param name="b">blue-yellow</param>
    /// <param name="alpha">alpha</param>
    public Lab(in byte l, in byte a, in byte b, in byte alpha = 255)
    {
        this.l = l * Lab.LFrom255;
        this.a = a - 128.0f;
        this.b = b - 128.0f;
        this.alpha = alpha * Utils.One255;
    }

    /// <summary>
    /// Creates a LAB color from signed bytes.
    /// Converts each to a single precision real number.
    /// Scales lightness from [0, 255] to [0.0, 100.0].
    /// Scales alpha from [0, 255] to [0.0, 1.0].
    /// </summary>
    /// <param name="l">lightness</param>
    /// <param name="a">green-magenta</param>
    /// <param name="b">blue-yellow</param>
    /// <param name="alpha">alpha</param>
    public Lab(in sbyte l, in sbyte a, in sbyte b, in sbyte alpha = -1)
    {
        this.l = (l & 0xff) * Lab.LFrom255;
        this.a = a;
        this.b = b;
        this.alpha = (alpha & 0xff) * Utils.One255;
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
        if (value is Lab lab) { return this.Equals(lab); }
        return false;
    }

    /// <summary>
    /// Returns a hash code representing this color.
    /// </summary>
    /// <returns>hash code</returns>
    public override int GetHashCode()
    {
        return Lab.ToHex(this);
    }

    /// <summary>
    /// Returns a string representation of this color.
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()
    {
        return Lab.ToString(this);
    }

    /// <summary>
    /// Compares this color to another in compliance with the IComparable
    /// interface. Returns 1 when a component of this color is greater than
    /// another; -1 when lesser.  Returns 0 as a last resort.
    /// Priority is alpha, lightness, b and a.
    /// </summary>
    /// <param name="c">comparisand</param>
    /// <returns>evaluation</returns>
    public int CompareTo(Lab c)
    {
        int left = Lab.ToHex(this);
        int right = Lab.ToHex(c);
        return (left < right) ? -1 : (left > right) ? 1 : 0;
    }

    /// <summary>
    /// Tests this color for equivalence with another in compliance with the
    /// IEquatable interface.
    /// </summary>
    /// <param name="c">vector</param>
    /// <returns>equivalence</returns>
    public bool Equals(Lab c)
    {
        return Lab.EqSatArith(this, c);
    }

    /// <summary>
    /// Converts a color to a boolean by returning whether its alpha is greater
    /// than zero.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>evaluation</returns>
    public static explicit operator bool(in Lab c)
    {
        return Lab.Any(c);
    }

    /// <summary>
    /// Converts a real number to a Lab color.
    /// </summary>
    /// <param name="v">real number</param>
    public static implicit operator Lab(in float v)
    {
        return new Lab(l: v, a: 0.0f, b: 0.0f, alpha: 1.0f);
    }

    /// <summary>
    /// Converts a four dimensional vector to a Lab color.
    /// The vector's w component is interpreted as alpha;
    /// z, as lightness; x, as green-magenta; y, as blue-yellow.
    /// </summary>
    /// <param name="v">vector</param>
    public static explicit operator Lab(in Vec4 v)
    {
        return new Lab(l: v.Z, a: v.X, b: v.Y, alpha: v.W);
    }

    /// <summary>
    /// Converts a Lab color to a four dimensional vector.
    /// The alpha channel is interpreted as w;
    /// lightness, as z; green-magenta, as x; blue-yellow, as y.
    /// </summary>
    /// <param name="c">color</param>
    public static explicit operator Vec4(in Lab c)
    {
        return new Vec4(x: c.a, y: c.b, z: c.l, w: c.alpha);
    }

    /// <summary>
    /// Adds two colors together for the purpose of
    /// making an adjustment.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>adjustment</returns>
    public static Lab operator +(in Lab a, in Lab b)
    {
        return new Lab(
            l: a.l + b.l,
            a: a.a + b.b,
            b: a.b + b.b,
            alpha: a.alpha + b.alpha);
    }

    /// <summary>
    /// Subtracts the right color from the left
    /// for the purpose of making an adjustment.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>adjustment</returns>
    public static Lab operator -(in Lab a, in Lab b)
    {
        return new Lab(
            l: a.l - b.l,
            a: a.a - b.b,
            b: a.b - b.b,
            alpha: a.alpha - b.alpha);
    }

    /// <summary>
    /// A color evaluates to true if its alpha channel is greater than zero.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>evaluation</returns>
    public static bool operator true(in Lab c)
    {
        return Lab.Any(c);
    }

    /// <summary>
    /// A color evaluates to false if its alpha channel is less than or equal to
    /// zero.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>evaluation</returns>
    public static bool operator false(in Lab c)
    {
        return Lab.None(c);
    }

    /// <summary>
    /// Tests to see if a color's alpha and lightness are greater
    /// than zero; tests to see if its a and b components are
    /// not zero.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>evaluation</returns>
    public static bool All(in Lab c)
    {
        return (c.alpha > 0.0f) &&
            (c.l > 0.0f) &&
            (c.a != 0.0f) &&
            (c.b != 0.0f);
    }

    /// <summary>
    /// Tests to see if the alpha channel of the color is greater than zero,
    /// i.e., if it has some opacity.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>evaluation</returns>
    public static bool Any(in Lab c)
    {
        return c.alpha > 0.0f;
    }

    ///<summary>
    ///Finds the chroma of a color.
    ///</summary>
    /// <param name="c">color</param>
    /// <returns>chroma squared</returns>
    public static float Chroma(in Lab c)
    {
        return MathF.Sqrt(Lab.ChromaSq(c));
    }

    ///<summary>
    ///Finds the chroma squared of a color.
    ///</summary>
    /// <param name="c">color</param>
    /// <returns>chroma squared</returns>
    public static float ChromaSq(in Lab c)
    {
        return c.a * c.a + c.b * c.b;
    }

    /// <summary>
    /// Returns the first color argument with the alpha
    /// of the second.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>color</returns>
    public static Lab CopyAlpha(in Lab a, in Lab b)
    {
        return new Lab(a.l, a.a, a.b, b.alpha);
    }

    /// <summary>
    /// Finds the distance between two colors.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>distance</returns>
    public static float Dist(in Lab o, in Lab d)
    {
        // Alternatively, see
        // https://github.com/svgeesus/svgeesus.github.io/blob/master/Color/OKLab-notes.md
        return Lab.DistEuclideanAlpha(o, d);
    }

    /// <summary>
    /// Finds the Euclidean distance between two colors.
    /// Includes the colors' alpha channel in the calculation.
    /// Since the alpha range is considerably less than
    /// that of the other channels, a scalar is provided
    /// to increase its weight.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <param name="alphaScalar">alpha scalar</param>
    /// <returns>Euclidean distance</returns>
    public static float DistEuclideanAlpha(
        in Lab a, in Lab b,
        in float alphaScalar = 100.0f)
    {
        float dt = alphaScalar * (b.alpha - a.alpha);
        float dl = b.l - a.l;
        float da = b.a - a.a;
        float db = b.b - a.b;
        return MathF.Sqrt(
            dt * dt +
            dl * dl +
            da * da +
            db * db);
    }

    /// <summary>
    /// Finds the Euclidean distance between two colors.
    /// Does not include the colors' alpha channel in the calculation.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>Euclidean distance</returns>
    public static float DistEuclideanNoAlpha(in Lab a, in Lab b)
    {
        float dl = b.l - a.l;
        float da = b.a - a.a;
        float db = b.b - a.b;
        return MathF.Sqrt(dl * dl + da * da + db * db);
    }

    /// <summary>
    /// Checks if two colors have equivalent alpha channels when converted to
    /// bytes in [0, 255]. Uses saturation arithmetic.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static bool EqAlphaSatArith(in Lab a, in Lab b)
    {
        return (int)(Utils.Clamp(a.alpha, 0.0f, 1.0f) * 255.0f + 0.5f) ==
               (int)(Utils.Clamp(b.alpha, 0.0f, 1.0f) * 255.0f + 0.5f);
    }

    /// <summary>
    /// Checks if two colors have equivalent l, a and b channels when
    /// converted to bytes (unsigned for l, signed for a and b).
    /// Uses saturation arithmetic.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static bool EqLabSatArith(in Lab a, in Lab b)
    {
        return (int)(Utils.Clamp(a.l, 0.0f, 100.0f) * Lab.LTo255 + 0.5f) ==
            (int)(Utils.Clamp(b.l, 0.0f, 100.0f) * Lab.LTo255 + 0.5f) &&
            Utils.Floor(Utils.Clamp(a.a, -127.5f, 127.5f)) ==
            Utils.Floor(Utils.Clamp(b.a, -127.5f, 127.5f)) &&
            Utils.Floor(Utils.Clamp(a.b, -127.5f, 127.5f)) ==
            Utils.Floor(Utils.Clamp(b.b, -127.5f, 127.5f));
    }

    /// <summary>
    /// Checks if two colors have equivalent l, a, b and alpha
    /// channels when converted to bytes.
    /// Uses saturation arithmetic.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static bool EqSatArith(in Lab a, in Lab b)
    {
        return Lab.EqAlphaSatArith(a, b) &&
            Lab.EqLabSatArith(a, b);
    }

    /// <summary>
    /// Converts from CIE XYZ to CIE LAB.
    /// Assumes that alpha is stored in the w component.
    /// </summary>
    /// <param name="v">CIE XYZ color</param>
    /// <returns>CIE LAB color</returns>
    public static Lab FromCieXyz(in Vec4 v)
    {
        double oneThird = 1.0d / 3.0d;
        double offset = 16.0d / 116.0d;

        double a = v.X * 1.0521110608435826d;
        if (a > 0.008856d)
        {
            a = Math.Pow(a, oneThird);
        }
        else
        {
            a = 7.787d * a + offset;
        }

        double b = v.Y;
        if (b > 0.008856d)
        {
            b = Math.Pow(b, oneThird);
        }
        else
        {
            b = 7.787d * b + offset;
        }

        double c = v.Z * 0.9184170164304805d;
        if (c > 0.008856d)
        {
            c = Math.Pow(c, oneThird);
        }
        else
        {
            c = 7.787d * c + offset;
        }

        return new Lab(
            l: (float)(116.0d * b - 16.0d),
            a: (float)(500.0d * (a - b)),
            b: (float)(200.0d * (b - c)),
            alpha: v.W);
    }

    /// <summary>
    /// Converts a hexadecimal representation of a color into
    /// LAB. Assumes the integer is packed in the order alpha
    /// in the 0x18 place, l in 0x10, a in 0x08, b in 0x00.
    /// Scales lightness from [0, 255] to [0.0, 100.0].
    /// Shifts a and b from [0, 255] to [-128, 127].
    /// Scales alpha from [0, 255] to [0.0, 1.0].
    /// </summary>
    /// <param name="c">integer</param>
    /// <returns>color</returns>
    public static Lab FromHex(in int c)
    {
        int t = c >> 0x18 & 0xff;
        int l = c >> 0x10 & 0xff;
        int a = c >> 0x08 & 0xff;
        int b = c >> 0x00 & 0xff;
        return new(
            l * Lab.LFrom255,
            a - 128.0f,
            b - 128.0f,
            t * Utils.One255);
    }

    /// <summary>
    /// Converts a color from LCH to LAB.
    /// </summary>
    /// <param name="c">LCH color</param>
    /// <returns>LAB color</returns>
    public static Lab FromLch(in Lch c)
    {
        float chroma = MathF.Max(0.0f, c.C);
        float hRad = c.H * Utils.Tau;
        return new Lab(
            l: c.L,
            a: chroma * MathF.Cos(hRad),
            b: chroma * MathF.Sin(hRad),
            alpha: c.Alpha);
    }

    ///<summary>
    ///Finds the hue of a color.
    ///</summary>
    /// <param name="c">color</param>
    /// <returns>chroma squared</returns>
    public static float Hue(in Lab c)
    {
        float h = MathF.Atan2(c.b, c.a);
        h = h < -0.0f ? h + Utils.Tau : h;
        return h * Utils.OneTau;
    }

    /// <summary>
    /// Mixes two colors together. Adds the colors then divides by half.
    /// </summary>
    /// <param name="o">origin</param>
    /// <param name="d">destination</param>
    /// <returns>mix</returns>
    public static Lab Mix(in Lab o, in Lab d)
    {
        return new Lab(
            l: 0.5f * (o.l + d.l),
            a: 0.5f * (o.a + d.a),
            b: 0.5f * (o.b + d.b),
            alpha: 0.5f * (o.alpha + d.alpha));
    }

    /// <summary>
    /// Mixes two colors together by a step.
    /// The step is unclamped.
    /// </summary>
    /// <param name="o">origin</param>
    /// <param name="d">destination</param>
    /// <param name="t">step</param>
    /// <returns>mix</returns>
    public static Lab Mix(in Lab o, in Lab d, in float t)
    {
        float u = 1.0f - t;
        return new Lab(
            l: u * o.l + t * d.l,
            a: u * o.a + t * d.a,
            b: u * o.b + t * d.b,
            alpha: u * o.alpha + t * d.alpha);
    }

    /// <summary>
    /// Mixes two colors by a step in the range [0.0, 1.0].
    /// If the chroma of both colors is greater than zero,
    /// interpolates by chroma and hue.
    /// </summary>
    /// <param name="o">origin color</param>
    /// <param name="d">destination color</param>
    /// <param name="t">step</param>
    /// <returns>mixed color</returns>
    public static Lab MixPolar(
        in Lab o,
        in Lab d,
        in float t)
    {
        return Lab.MixPolar(o, d, t,
        (x, y, z, w) => Utils.LerpAngleNear(x, y, z, w));
    }

    /// <summary>
    /// Mixes two colors by a step in the range [0.0, 1.0].
    /// If the chroma of both colors is greater than zero,
    /// interpolates by chroma and hue.
    /// </summary>
    /// <param name="o">origin color</param>
    /// <param name="d">destination color</param>
    /// <param name="t">step</param>
    /// <returns>mixed color</returns>
    public static Lab MixPolar(
        in Lab o,
        in Lab d,
        in float t,
        in Func<float, float, float, float, float> easing)
    {
        float ocsq = Lab.ChromaSq(o);
        float dcsq = Lab.ChromaSq(d);
        if (ocsq < Utils.Epsilon || dcsq < Utils.Epsilon)
        {
            return Lab.Mix(o, d, t);
        }

        float u = 1.0f - t;
        float cc = u * MathF.Sqrt(ocsq) + t * MathF.Sqrt(dcsq);
        float ch = easing(
            MathF.Atan2(o.b, o.a),
            MathF.Atan2(d.b, d.a),
            t, Utils.Tau);

        return new Lab(
            l: u * o.l + t * d.l,
            a: cc * MathF.Cos(ch),
            b: cc * MathF.Sin(ch),
            alpha: u * o.alpha + t * d.alpha);
    }

    /// <summary>
    /// Tests to see if the alpha channel of this color is less than or equal to
    /// zero, i.e., if it is completely transparent.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>evaluation</returns>
    public static bool None(in Lab c)
    {
        return c.alpha <= 0.0f;
    }

    /// <summary>
    /// Returns an opaque version of the color, i.e., where its alpha is 1.0.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>opaque</returns>
    public static Lab Opaque(in Lab c)
    {
        return new Lab(c.l, c.a, c.b, 1.0f);
    }

    /// <summary>
    /// Converts a color to an integer. Clamps the a and b
    /// components to [-127.5, 127.5], floors, then adds 128.
    /// Scales lightness from [0.0, 100.0] to [0, 255].
    /// Packs the integer, from most to least significant,
    /// in the order alpha in the 0x18 place, l in 0x10,
    // a in 0x08, b in 0x00.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>integer</returns>
    public static int ToHex(in Lab c)
    {
        int t = (int)(Utils.Clamp(c.a, 0.0f, 1.0f) * 255.0f + 0.5f);
        int l = (int)(Utils.Clamp(c.l, 0.0f, 100.0f) * Lab.LTo255 + 0.5f);
        int a = 128 + Utils.Floor(Utils.Clamp(c.a, -127.5f, 127.5f));
        int b = 128 + Utils.Floor(Utils.Clamp(c.b, -127.5f, 127.5f));
        return t << 0x18 | l << 0x10 | a << 0x08 | b;
    }

    /// <summary>
    /// Returns a string representation of a color.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string</returns>
    public static string ToString(in Lab c, in int places = 4)
    {
        return Lab.ToString(new StringBuilder(96), c, places).ToString();
    }

    /// <summary>
    /// Appends a representation of a color to a string builder.
    /// </summary>
    /// <param name="sb">string bulider</param>
    /// <param name="c">vector</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString(
        in StringBuilder sb,
        in Lab c,
        in int places = 4)
    {
        sb.Append("{ l: ");
        Utils.ToFixed(sb, c.l, places);
        sb.Append(", a: ");
        Utils.ToFixed(sb, c.a, places);
        sb.Append(", b: ");
        Utils.ToFixed(sb, c.b, places);
        sb.Append(", alpha: ");
        Utils.ToFixed(sb, c.alpha, places);
        sb.Append(' ');
        sb.Append('}');
        return sb;
    }

    /// <summary>
    /// Returns the color black.
    /// </summary>
    /// <value>black</value>
    public static Lab Black { get { return new(0.0f, 0.0f, 0.0f, 1.0f); } }

    /// <summary>
    /// Returns the color blue in CIE LAB.
    /// </summary>
    /// <value>CIE blue</value>
    public static Lab CieBlue
    {
        get
        {
            return new(32.29847f, 79.1899f, -107.8634f, 1.0f);
        }
    }

    /// <summary>
    /// Returns the color cyan in CIE LAB.
    /// </summary>
    /// <value>CIE cyan</value>
    public static Lab CieCyan
    {
        get
        {
            return new(91.11428f, -48.08577f, -14.13485f, 1.0f);
        }
    }

    /// <summary>
    /// Returns the color green in CIE LAB.
    /// </summary>
    /// <value>CIE green</value>
    public static Lab CieGreen
    {
        get
        {
            return new(87.73554f, -86.1834f, 83.17997f, 1.0f);
        }
    }

    /// <summary>
    /// Returns the color magenta in CIE LAB.
    /// </summary>
    /// <value>CIE magenta</value>
    public static Lab CieMagenta
    {
        get
        {
            return new(60.32269f, 98.23384f, -60.83306f, 1.0f);
        }
    }

    /// <summary>
    /// Returns the color red in CIE LAB.
    /// </summary>
    /// <value>CIE red</value>
    public static Lab CieRed
    {
        get
        {
            return new(53.23824f, 80.08955f, 67.20071f, 1.0f);
        }
    }

    /// <summary>
    /// Returns the color yellow in CIE LAB.
    /// </summary>
    /// <value>CIE yellow</value>
    public static Lab CieYellow
    {
        get
        {
            return new(97.139f, -21.56006f, 94.47734f, 1.0f);
        }
    }

    /// <summary>
    /// Returns the color clear black.
    /// </summary>
    /// <value>clear black</value>
    public static Lab ClearBlack { get { return new(0.0f, 0.0f, 0.0f, 0.0f); } }

    /// <summary>
    /// Returns the color clear white.
    /// </summary>
    /// <value>clear white</value>
    public static Lab ClearWhite { get { return new(100.0f, 0.0f, 0.0f, 0.0f); } }

    /// <summary>
    /// Returns the color white.
    /// </summary>
    /// <value>white</value>
    public static Lab White { get { return new(100.0f, 0.0f, 0.0f, 1.0f); } }
}