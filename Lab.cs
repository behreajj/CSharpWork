using System;
using System.Text;

/// <summary>
/// A readonly struct. Represents colors in a perceptual color space, such as
/// CIE LAB, SR LAB 2, OK LAB, etc. The a and b axes are signed, unbounded
/// values. Negative a indicates a green hue; positive, magenta. Negative b
/// indicates a blue hue; positive, yellow. Lightness falls in the range
/// [0.0, 100.0]. For a and b, the practical range is roughly [-111.0, 111.0].
/// Alpha is expected to be in [0.0, 1.0].
/// </summary>
[Serializable]
public readonly struct Lab : IComparable<Lab>, IEquatable<Lab>
{
    /// <summary>
    /// The lower bound for the a color channel of a color converted from
    /// standard RGB to SR LAB 2, sampled without respect for lightness.
    /// </summary>
    public const float AbsMinA = -82.70919f;

    /// <summary>
    /// The upper bound for the a color channel of a color converted from
    /// standard RGB to SR LAB 2, sampled without respect for lightness.
    /// </summary>
    public const float AbsMaxA = 104.18851f;

    /// <summary>
    /// The lower bound for the b color channel of a color converted from
    /// standard RGB to SR LAB 2, sampled without respect for lightness.
    /// </summary>
    public const float AbsMinB = -110.47817f;

    /// <summary>
    /// The upper bound for the b color channel of a color converted from
    /// standard RGB to SR LAB 2, sampled without respect for lightness.
    /// </summary>
    public const float AbsMaxB = 94.90346f;

    /// <summary>
    /// A scalar to convert a number in [0, 255] to lightness in [0, 100].
    /// Equivalent to 100.0 / 255.0.
    /// </summary>
    public const float LFrom255 = 0.39215687f;

    /// <summary>
    /// A scalar to convert lightness in [0, 100] to a number in [0, 255].
    /// Equivalent to 255.0 / 100.0.
    /// </summary>
    public const float LTo255 = 2.55f;

    /// <summary>
    /// The scalar used to normalize the A channel in SR LCH 2 to a range
    /// [0.0, 1.0]. Equivalent to 0.5 / max(abs(AbsMinA), abs(AbsMinA)). Takes
    /// the larger number to center the range at 0.
    /// </summary>
    public const float NormDenomA = 0.004798994f;

    /// <summary>
    /// The offset added to normalize the A channel in  SR LCH 2 to a range
    /// [0.0, 1.0]. Equivalent to max(abs(AbsMinA), abs(AbsMinA). Takes the
    /// larger number so as to center the range at 0.
    /// </summary>
    public const float NormOffsetA = 104.18851f;

    /// <summary>
    /// The scalar used to restore the A channel in  SR LCH 2 from a range
    /// [0.0, 1.0]. Equivalent to 2.0 * max(abs(AbsMinA), abs(AbsMinA)). Takes
    /// the larger number so as to center the range at 0.
    /// </summary>
    public const float NormScaleA = 208.37701f;

    /// <summary>
    /// The scalar used to normalize the B channel in  SR LCH 2 to a range
    /// [0.0, 1.0]. Equivalent to 0.5 / max(abs(AbsMinB), abs(AbsMinB)). Takes
    /// the larger number to center the range at 0.
    /// </summary>
    public const float NormDenomB = 0.004525781f;

    /// <summary>
    /// The offset added to normalize the B channel in SR LCH 2 to a range
    /// [0.0, 1.0]. Equivalent to max(abs(AbsMinB), abs(AbsMinB). Takes the
    /// larger number so as to center the range at 0.
    /// </summary>
    public const float NormOffsetB = 110.47817f;

    /// <summary>
    /// The scalar used to restore the B channel in  SR LCH 2 from a range
    /// [0.0, 1.0]. Equivalent to 2.0 * max(abs(AbsMinB), abs(AbsMinB)). Takes
    /// the larger number so as to center the range at 0.
    /// </summary>
    public const float NormScaleB = 220.95634f;

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
        this.alpha = alpha / 255.0f;
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
        this.alpha = (alpha & 0xff) / 255.0f;
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
    /// another; -1 when lesser. Returns 0 as a last resort.
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
    /// Adds two colors together for the purpose of making an adjustment.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>adjustment</returns>
    public static Lab operator +(in Lab a, in Lab b)
    {
        return new Lab(
            l: a.l + b.l,
            a: a.a + b.a,
            b: a.b + b.b,
            alpha: a.alpha + b.alpha);
    }

    /// <summary>
    /// Subtracts the right color from the left for the purpose of making an
    /// adjustment.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>adjustment</returns>
    public static Lab operator -(in Lab a, in Lab b)
    {
        return new Lab(
            l: a.l - b.l,
            a: a.a - b.a,
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
    /// Tests to see if a color's alpha and lightness are greater than zero.
    /// Tests to see if its a and b components are not zero.
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

    /// <summary>
    /// Finds the chroma of a color.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>chroma squared</returns>
    public static float Chroma(in Lab c)
    {
        return MathF.Sqrt(Lab.ChromaSq(c));
    }

    /// <summary>
    /// Finds the chroma squared of a color.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>chroma squared</returns>
    public static float ChromaSq(in Lab c)
    {
        return c.a * c.a + c.b * c.b;
    }

    /// <summary>
    /// Returns the first color argument with the alpha of the second.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>color</returns>
    public static Lab CopyAlpha(in Lab a, in Lab b)
    {
        return new Lab(l: a.l, a: a.a, b: a.b, alpha: b.alpha);
    }

    /// <summary>
    /// Returns the first color argument with the light of the second.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>color</returns>
    public static Lab CopyLight(in Lab a, in Lab b)
    {
        return new Lab(l: b.l, a: a.a, b: a.b, alpha: a.alpha);
    }

    /// <summary>
    /// Finds the distance between two colors.
    /// </summary>
    /// <param name="o">left operand</param>
    /// <param name="d">right operand</param>
    /// <returns>distance</returns>
    public static float Dist(in Lab o, in Lab d)
    {
        // See
        // https://github.com/svgeesus/svgeesus.github.io/blob/master/Color/OKLab-notes.md

        float da = d.a - o.a;
        float db = d.b - o.b;
        return Math.Abs(100.0f * (d.alpha - o.alpha))
            + Math.Abs(d.l - o.l)
            + MathF.Sqrt(da * da + db * db);
    }

    /// <summary>
    /// Finds the Euclidean distance between two colors. Includes the colors'
    /// alpha channel in the calculation. Since the alpha range is  less than
    /// that of the other channels, a scalar is provided to increase its weight.
    /// </summary>
    /// <param name="o">left operand</param>
    /// <param name="d">right operand</param>
    /// <param name="alphaScalar">alpha scalar</param>
    /// <returns>Euclidean distance</returns>
    public static float DistEuclideanAlpha(
        in Lab o, in Lab d,
        in float alphaScalar = 100.0f)
    {
        float dt = alphaScalar * (d.alpha - o.alpha);
        float dl = d.l - o.l;
        float da = d.a - o.a;
        float db = d.b - o.b;
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
    /// <param name="o">left operand</param>
    /// <param name="d">right operand</param>
    /// <returns>Euclidean distance</returns>
    public static float DistEuclideanNoAlpha(in Lab o, in Lab d)
    {
        float dl = d.l - o.l;
        float da = d.a - o.a;
        float db = d.b - o.b;
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
    /// Checks if two colors have equivalent l, a and b channels when converted
    /// to bytes (unsigned for l, signed for a and b).
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
    /// Checks if two colors have equivalent l, a, b and alpha channels when
    /// converted to bytes. Uses saturation arithmetic.
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
    /// Converts a hexadecimal representation of a color into LAB. Assumes the
    /// integer is packed in the order alpha in the 0x18 place, l in 0x10,
    /// a in 0x08, b in 0x00.
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
        return new Lab(
            l: l * Lab.LFrom255,
            a: a - 128.0f,
            b: b - 128.0f,
            alpha: t / 255.0f);
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

    /// <summary>
    /// Converts from SR XYZ to SR LAB 2.
    /// Assumes that alpha is stored in the w component.
    /// </summary>
    /// <param name="v">XYZ color</param>
    /// <returns>LAB color</returns>
    public static Lab FromSrXyz(in Vec4 v)
    {
        double x = v.X;
        double y = v.Y;
        double z = v.Z;

        double comparisand = 216.0d / 24389.0d;
        double scalar = 24389.0d / 2700.0d;
        double oneThird = 1.0d / 3.0d;

        x = (x <= comparisand) ? x * scalar : Math.Pow(x, oneThird) * 1.16d - 0.16d;
        y = (y <= comparisand) ? y * scalar : Math.Pow(y, oneThird) * 1.16d - 0.16d;
        z = (z <= comparisand) ? z * scalar : Math.Pow(z, oneThird) * 1.16d - 0.16d;

        return new Lab(
            l: (float)(37.0950d * x + 62.9054d * y - 0.0008d * z),
            a: (float)(663.4684d * x - 750.5078d * y + 87.0328d * z),
            b: (float)(63.9569d * x + 108.4576d * y - 172.4152d * z),
            alpha: v.W);
    }

    /// <summary>
    /// Returns a gray version of the color, where a and b are zero.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>gray</returns>
    public static Lab Gray(in Lab c)
    {
        return new Lab(
            l: c.l,
            a: 0.0f,
            b: 0.0f,
            alpha: c.alpha);
    }

    /// <summary>
    /// Finds the analogous color harmonies for the color.
    /// Returns an array containing two colors.
    /// </summary>
    /// <param name="c">LAB color</param>
    /// <returns>analogues</returns>
    public static Lab[] HarmonyAnalogous(in Lab c)
    {
        float lAna = (c.l * 2.0f + 50.0f) / 3.0f;

        float cos30 = Utils.Sqrt32;
        float sin30 = 0.5f;
        float a30 = cos30 * c.a - sin30 * c.b;
        float b30 = cos30 * c.b + sin30 * c.a;

        float cos330 = Utils.Sqrt32;
        float sin330 = -0.5f;
        float a330 = cos330 * c.a - sin330 * c.b;
        float b330 = cos330 * c.b + sin330 * c.a;

        return new Lab[] {
            new(lAna, a30, b30, c.alpha),
            new(lAna, a330, b330, c.alpha)
        };
    }

    /// <summary>
    /// Finds the complementary color harmony for the color.
    /// Returns an array containing one color.
    /// </summary>
    /// <param name="c">LAB color</param>
    /// <returns>complement</returns>
    public static Lab[] HarmonyComplement(in Lab c)
    {
        return new Lab[] {
            new(100.0f - c.l, -c.a, -c.b, c.alpha)
        };
    }

    /// <summary>
    /// Finds the split color harmonies for the color.
    /// Returns an array containing two colors.
    /// </summary>
    /// <param name="c">LAB color</param>
    /// <returns>split</returns>
    public static Lab[] HarmonySplit(in Lab c)
    {
        float lSpl = (250.0f - c.l * 2.0f) / 3.0f;

        float cos150 = -Utils.Sqrt32;
        float sin150 = 0.5f;
        float a150 = cos150 * c.a - sin150 * c.b;
        float b150 = cos150 * c.b + sin150 * c.a;

        float cos210 = -Utils.Sqrt32;
        float sin210 = -0.5f;
        float a210 = cos210 * c.a - sin210 * c.b;
        float b210 = cos210 * c.b + sin210 * c.a;

        return new Lab[] {
            new(lSpl, a150, b150, c.alpha),
            new(lSpl, a210, b210, c.alpha)
        };
    }

    /// <summary>
    /// Finds the square color harmonies for the color.
    /// Returns an array containing three colors.
    /// </summary>
    /// <param name="c">LAB color</param>
    /// <returns>square</returns>
    public static Lab[] HarmonySquare(in Lab c)
    {
        return new Lab[] {
            new(50.0f, -c.b, c.a, c.alpha),
            new(100.0f - c.l, -c.a, -c.b, c.alpha),
            new(50.0f, c.b, -c.a, c.alpha)
        };
    }

    /// <summary>
    /// Finds the tetradic color harmonies for the color.
    /// Returns an array containing three colors.
    /// </summary>
    /// <param name="c">LAB color</param>
    /// <returns>tetrad</returns>
    public static Lab[] HarmonyTetradic(in Lab c)
    {
        float lTri = (200.0f - c.l) / 3.0f;
        float lCmp = 100.0f - c.l;
        float lTet = (100.0f + c.l) / 3.0f;

        float cos120 = -0.5f;
        float sin120 = Utils.Sqrt32;
        float a120 = cos120 * c.a - sin120 * c.b;
        float b120 = cos120 * c.b + sin120 * c.a;

        float cos300 = 0.5f;
        float sin300 = -Utils.Sqrt32;
        float a300 = cos300 * c.a - sin300 * c.b;
        float b300 = cos300 * c.b + sin300 * c.a;

        return new Lab[] {
            new(lTri, a120, b120, c.alpha),
            new(lCmp, -c.a, -c.b, c.alpha),
            new(lTet, a300, b300, c.alpha)
        };
    }

    /// <summary>
    /// Finds the triadic color harmonies for the color.
    /// Returns an array containing two colors.
    /// </summary>
    /// <param name="c">LAB color</param>
    /// <returns>triad</returns>
    public static Lab[] HarmonyTriadic(in Lab c)
    {
        float lTri = (200.0f - c.l) / 3.0f;

        float cos120 = -0.5f;
        float sin120 = Utils.Sqrt32;
        float a120 = cos120 * c.a - sin120 * c.b;
        float b120 = cos120 * c.b + sin120 * c.a;

        float cos240 = -0.5f;
        float sin240 = -Utils.Sqrt32;
        float a240 = cos240 * c.a - sin240 * c.b;
        float b240 = cos240 * c.b + sin240 * c.a;

        return new Lab[] {
            new(lTri, a120, b120, c.alpha),
            new(lTri, a240, b240, c.alpha)
        };
    }

    /// <summary>
    /// Finds the hue of a color.
    /// </summary>
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
    /// Mixes two colors by a step in the range [0.0, 1.0]. If the chroma of
    /// both colors is greater than zero, interpolates by chroma and hue.
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
    /// Returns an opaque version of the color, i.e.,
    /// where alpha is 1.0.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>opaque</returns>
    public static Lab Opaque(in Lab c)
    {
        return new Lab(l: c.l, a: c.a, b: c.b, alpha: 1.0f);
    }

    /// <summary>
    /// Normalizes the color's a and b components, then multiplies
    /// by a scalar, in effect setting the color's chroma.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>rescaled color</returns>
    public static Lab RescaleChroma(in Lab c, in float scalar = 1.0f)
    {
        float cSq = Lab.ChromaSq(c);
        if (cSq > Utils.Epsilon)
        {
            float scInv = scalar / MathF.Sqrt(cSq);
            return new Lab(
                l: c.l,
                a: c.a * scInv,
                b: c.b * scInv,
                alpha: c.alpha);
        }
        return Lab.Gray(c);
    }

    /// <summary>
    /// Rotates a color's a and b components.
    /// Accepts a normalized hue, or angle, in [0.0, 1.0].
    /// </summary>
    /// <param name="c">LAB color</param>
    /// <param name="amt">hue, angle</param>
    /// <returns>rotated color</returns>
    public static Lab RotateHue(in Lab c, in float amt)
    {
        float radians = amt * Utils.Tau;
        return Lab.RotateHue(c, MathF.Cos(radians), MathF.Sin(radians));
    }

    /// <summary>
    /// Rotates a color's a and b components.
    ///
    /// Accepts pre-calculated sine and cosine of an angle, so that collections
    /// of colors can be efficiently rotated without repeatedly calling cos and
    /// sin.
    /// </summary>
    /// <param name="c">LAB color</param>
    /// <param name="cosa">the cosine of the angle</param>
    /// <param name="sina">the sine of the angle</param>
    /// <returns>rotated color</returns>
    public static Lab RotateHue(in Lab c, in float cosa, in float sina)
    {
        return new Lab(
            l: c.l,
            a: cosa * c.a - sina * c.b,
            b: cosa * c.b + sina * c.a,
            alpha: c.alpha);
    }

    /// <summary>
    /// Converts from SR LAB 2 to SR XYZ.
    /// The z component of the input vector is
    /// expected to hold the luminance, while x
    /// is expected to hold a and y, b.
    /// </summary>
    /// <param name="lab">LAB color</param>
    /// <returns>XYZ color</returns>
    public static Vec4 ToSrXyz(in Lab lab)
    {
        double ld = lab.l * 0.01d;
        double ad = lab.a;
        double bd = lab.b;

        double x = ld + 0.000904127d * ad + 0.000456344d * bd;
        double y = ld - 0.000533159d * ad - 0.000269178d * bd;
        double z = ld - 0.0058d * bd;

        // 2700.0 / 24389.0 = 0.11070564598795
        // 1.0 / 1.16 = 0.86206896551724
        double ltScale = 2700.0d / 24389.0d;
        double gtScale = 1.0d / 1.16d;
        if (x <= 0.08d)
        {
            x *= ltScale;
        }
        else
        {
            x = (x + 0.16d) * gtScale;
            x = x * x * x;
        }

        if (y <= 0.08d)
        {
            y *= ltScale;
        }
        else
        {
            y = (y + 0.16d) * gtScale;
            y = y * y * y;
        }

        if (z <= 0.08d)
        {
            z *= ltScale;
        }
        else
        {
            z = (z + 0.16d) * gtScale;
            z = z * z * z;
        }

        return new Vec4(
            x: (float)x,
            y: (float)y,
            z: (float)z,
            w: lab.Alpha);
    }

    /// <summary>
    /// Converts a color to an integer. Clamps the a and b components to
    /// [-127.5, 127.5], floors, then adds 128. Scales lightness from
    /// [0.0, 100.0] to [0, 255]. Packs the integer, from most to least
    /// significant, in the order alpha in the 0x18 place, l in 0x10, a in
    /// 0x08, b in 0x00.
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
        sb.Append("{\"l\":");
        Utils.ToFixed(sb, c.l, places);
        sb.Append(",\"a\":");
        Utils.ToFixed(sb, c.a, places);
        sb.Append(",\"b\":");
        Utils.ToFixed(sb, c.b, places);
        sb.Append(",\"alpha\":");
        Utils.ToFixed(sb, c.alpha, places);
        sb.Append('}');
        return sb;
    }

    /// <summary>
    /// Returns the color black.
    /// </summary>
    /// <value>black</value>
    public static Lab Black { get { return new(0.0f, 0.0f, 0.0f, 1.0f); } }

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
    /// Returns the color red in SR LAB 2.
    /// </summary>
    /// <value>red</value>
    public static Lab SrRed { get { return new(53.22598f, 78.20428f, 67.70062f, 1.0f); } }

    /// <summary>
    /// Returns the color yellow in SR LAB 2.
    /// </summary>
    /// <value>yellow</value>
    public static Lab SrYellow { get { return new(97.34526f, -37.15428f, 95.18662f, 1.0f); } }

    /// <summary>
    /// Returns the color green in SR LAB 2.
    /// </summary>
    /// <value>green</value>
    public static Lab SrGreen { get { return new(87.51519f, -82.95599f, 83.03678f, 1.0f); } }

    /// <summary>
    /// Returns the color cyan in SR LAB 2.
    /// </summary>
    /// <value>cyan</value>
    public static Lab SrCyan { get { return new(90.6247f, -43.80207f, -15.00912f, 1.0f); } }

    /// <summary>
    /// Returns the color blue in SR LAB 2.
    /// </summary>
    /// <value>blue</value>
    public static Lab SrBlue { get { return new(30.64395f, -12.02581f, -110.8078f, 1.0f); } }

    /// <summary>
    /// Returns the color magenta in SR LAB 2.
    /// </summary>
    /// <value>magenta</value>
    public static Lab SrMagenta { get { return new(60.25521f, 102.6771f, -61.00205f, 1.0f); } }

    /// <summary>
    /// Returns the color white.
    /// </summary>
    /// <value>white</value>
    public static Lab White { get { return new(100.0f, 0.0f, 0.0f, 1.0f); } }
}