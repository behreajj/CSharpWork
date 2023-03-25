using System;
using System.Text;

/// <summary>
/// A readonly struct. Represents colors in a perceptual
/// color space, such as CIE LAB, SR LAB 2, OK LAB, etc.
/// The a and b axes are signed, unbounded values.
/// Negative a indicates a green hue; positive, red.
/// Negative b indicates a blue hue; positive, yellow.
/// Lightness falls in the range [0.0, 100.0]; for a and
/// b, the practical range is roughly [-111.0, 111.0].
/// Alpha is expected to be in [0.0, 1.0].
/// </summary>
[Serializable]
public readonly struct Lab : IComparable<Lab>, IEquatable<Lab>
{
    /// <summary>
    /// The green-red component.
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
    /// The green-red component.
    /// </summary>
    /// <value>green-red</value>
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
    /// <param name="a">red-green</param>
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
    /// Returns a hash code representing this vector.
    /// </summary>
    /// <returns>hash code</returns>
    public override int GetHashCode()
    {
        unchecked
        {
            return (((Utils.MulBase ^ this.alpha.GetHashCode()) *
                        Utils.HashMul ^ this.l.GetHashCode()) *
                    Utils.HashMul ^ this.b.GetHashCode()) *
                Utils.HashMul ^ this.a.GetHashCode();
        }
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
        return (this.alpha < c.alpha) ? -1 :
            (this.alpha > c.alpha) ? 1 :
            (this.l < c.l) ? -1 :
            (this.l > c.l) ? 1 :
            (this.b < c.b) ? -1 :
            (this.b > c.b) ? 1 :
            (this.a < c.a) ? -1 :
            (this.a > c.a) ? 1 :
            0;
    }

    /// <summary>
    /// Tests this color for equivalence with another in compliance with the
    /// IEquatable interface.
    /// </summary>
    /// <param name="c">vector</param>
    /// <returns>equivalence</returns>
    public bool Equals(Lab c)
    {
        if (this.alpha.GetHashCode() != c.alpha.GetHashCode()) { return false; }
        if (this.l.GetHashCode() != c.l.GetHashCode()) { return false; }
        if (this.b.GetHashCode() != c.b.GetHashCode()) { return false; }
        if (this.a.GetHashCode() != c.a.GetHashCode()) { return false; }
        return true;
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
    /// z, as lightness; x, as green-red; y, as blue-yellow.
    /// </summary>
    /// <param name="v">vector</param>
    public static explicit operator Lab(in Vec4 v)
    {
        return new Lab(l: v.z, a: v.x, b: v.y, alpha: v.w);
    }

    /// <summary>
    /// Converts a Lab color to a four dimensional vector.
    /// The alpha channel is interpreted as w;
    /// lightness, as z; green-red, as x; blue-yellow, as y.
    /// </summary>
    /// <param name="c">color</param>
    public static explicit operator Vec4(in Lab c)
    {
        return new Vec4(x: c.a, y: c.b, z: c.l, w: c.alpha);
    }

    /// <summary>
    /// Multiplies a color's lightness by a scalar.
    /// </summary>
    /// <param name="a">left operand, the color</param>
    /// <param name="b">right operand, the scalar</param>
    /// <returns>product</returns>
    public static Lab operator *(in Lab a, in float b)
    {
        return new Lab(l: a.l * b, a: a.a, b: a.b, alpha: a.alpha);
    }

    /// <summary>
    /// Multiplies a color's lightness by a scalar.
    /// </summary>
    /// <param name="a">left operand, the scalar</param>
    /// <param name="b">right operand, the color</param>
    /// <returns>product</returns>
    public static Lab operator *(in float a, in Lab b)
    {
        return new Lab(l: a * b.l, a: b.a, b: b.b, alpha: b.alpha);
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
            (c.b != 0.0f) &&
            (c.a != 0.0f);
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
    public static float Dist(in Lab a, in Lab b)
    {
        // Alternatively, see
        // https://github.com/svgeesus/svgeesus.github.io/blob/master/Color/OKLab-notes.md
        return Lab.DistEuclidean(a, b);
    }

    /// <summary>
    /// Finds the Euclidean distance between two colors.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>Euclidean distance</returns>
    public static float DistEuclidean(in Lab a, in Lab b)
    {
        float dl = b.l - a.l;
        float da = b.a - a.a;
        float db = b.b - a.b;
        return MathF.Sqrt(dl * dl + da * da + db * db);
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

        return new Lab(
            l: (float)(116.0d * b - 16.0d),
            a: (float)(500.0d * (a - b)),
            b: (float)(200.0d * (b - c)),
            alpha: v.w);
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
    /// Mixes two colors together by a step. Uses the formula (1.0 - t) a + t b
    /// . The step is unclamped.
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
    public static StringBuilder ToString(in StringBuilder sb, in Lab c, in int places = 4)
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
    public static Lab Black { get { return new Lab(0.0f, 0.0f, 0.0f, 1.0f); } }

    /// <summary>
    /// Returns the color clear black.
    /// </summary>
    /// <value>clear black</value>
    public static Lab ClearBlack { get { return new Lab(0.0f, 0.0f, 0.0f, 0.0f); } }

    /// <summary>
    /// Returns the color clear white.
    /// </summary>
    /// <value>clear white</value>
    public static Lab ClearWhite { get { return new Lab(100.0f, 0.0f, 0.0f, 0.0f); } }

    /// <summary>
    /// Returns the color white.
    /// </summary>
    /// <value>white</value>
    public static Lab White { get { return new Lab(100.0f, 0.0f, 0.0f, 1.0f); } }
}