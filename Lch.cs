using System;
using System.Text;

/// <summary>
/// A readonly struct. Represents colors in a perceptual
/// color space, such as CIE LCH, etc.
/// Lightness falls in the range [0.0, 100.0].
/// Chroma is unbounded, but has a practical range of
/// roughly [0.0, 135.0].
/// Hue is a periodic number in [0.0, 1.0].
/// Alpha is expected to be in [0.0, 1.0].
/// </summary>
[Serializable]
public readonly struct Lch
{
    /// <summary>
    /// The alpha (transparency) component.
    /// </summary>
    private readonly float alpha;

    /// <summary>
    /// The chroma, or intensity of the hue.
    /// </summary>
    private readonly float c;

    /// <summary>
    /// The hue, normalized to [0.0, 1.0].
    /// </summary>
    private readonly float h;

    /// <summary>
    /// The light component.
    /// </summary>
    private readonly float l;

    /// <summary>
    /// The alpha component.
    /// </summary>
    /// <value>alpha</value>
    public float Alpha { get { return this.alpha; } }

    /// <summary>
    /// The chroma, or intensity of the hue.
    /// </summary>
    /// <value>chroma</value>
    public float C { get { return this.c; } }

    /// <summary>
    /// The hue, normalized to [0.0, 1.0].
    /// </summary>
    /// <value>hue</value>
    public float H { get { return this.h; } }

    /// <summary>
    /// The light component.
    /// </summary>
    /// <value>light</value>
    public float L { get { return this.l; } }

    /// <summary>
    /// Creates a LCH color from single precision real numbers.
    /// </summary>
    /// <param name="l">lightness</param>
    /// <param name="c">chroma</param>
    /// <param name="h">hue</param>
    /// <param name="alpha">alpha</param>
    public Lch(in float l, in float c, in float h, in float alpha = 1.0f)
    {
        this.l = l;
        this.c = c;
        this.h = h;
        this.alpha = alpha;
    }

    /// <summary>
    /// Multiplies a color's lightness by a scalar.
    /// </summary>
    /// <param name="a">left operand, the color</param>
    /// <param name="b">right operand, the scalar</param>
    /// <returns>product</returns>
    public static Lch operator *(in Lch a, in float b)
    {
        return new Lch(l: a.l * b, c: a.c, h: a.h, alpha: a.alpha);
    }

    /// <summary>
    /// Multiplies a color's lightness by a scalar.
    /// </summary>
    /// <param name="a">left operand, the scalar</param>
    /// <param name="b">right operand, the color</param>
    /// <returns>product</returns>
    public static Lch operator *(in float a, in Lch b)
    {
        return new Lch(l: a * b.l, c: b.c, h: b.h, alpha: b.alpha);
    }

    /// <summary>
    /// Adds two colors together for the purpose of
    /// making an adjustment.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>adjustment</returns>
    public static Lch operator +(in Lch a, in Lch b)
    {
        float ch = a.h + b.h;
        return new Lch(
            l: a.l + b.l,
            c: a.c + b.c,
            h: ch - MathF.Floor(ch),
            alpha: a.alpha + b.alpha);
    }

    /// <summary>
    /// Subtracts the right color from the left
    /// for the purpose of making an adjustment.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>adjustment</returns>
    public static Lch operator -(in Lch a, in Lch b)
    {
        float ch = a.h - b.h;
        return new Lch(
            l: a.l - b.l,
            c: a.c - b.c,
            h: ch - MathF.Floor(ch),
            alpha: a.alpha - b.alpha);
    }

    /// <summary>
    /// Converts a color from LAB to LCH. If the chroma is near
	/// zero, then the hue is set to zero.
    /// </summary>
    /// <param name="c">LAB color</param>
    /// <returns>LCH color</returns>
    public static Lch FromLab(in Lab c)
    {
        float l = c.L;
        float a = c.A;
        float b = c.B;
        float alpha = c.Alpha;

        float chrSq = a * a + b * b;
        if (chrSq < Utils.Epsilon)
        {
            return new Lch(l: l, c: 0.0f, h: 0.0f, alpha: alpha);
        }

        float hue = MathF.Atan2(b, a);
        hue = (hue < -0.0f) ? hue + Utils.Tau : hue;
        hue *= Utils.OneTau;
        float chroma = MathF.Sqrt(chrSq);

        return new Lch(
            l: l,
            c: chroma,
            h: hue,
            alpha: alpha);
    }

    /// <summary>
    /// Mixes two colors together.
    /// </summary>
    /// <param name="o">origin color</param>
    /// <param name="d">destination color</param>
    /// <returns>mixed color</returns>
    public static Lch Mix(in Lch o, in Lch d)
    {
        return Lch.Mix(o, d, 0.5f);
    }

    /// <summary>
    /// Mixes two colors by a step in the range [0.0, 1.0] .
    /// </summary>
    /// <param name="o">origin color</param>
    /// <param name="d">destination color</param>
    /// <param name="t">step</param>
    /// <returns>mixed color</returns>
    public static Lch Mix(in Lch o, in Lch d, in float t)
    {
        return Lch.Mix(o, d, t,
            (x, y, z, w) => Utils.LerpAngleNear(x, y, z, w));
    }

    /// <summary>
    /// Mixes two colors by a step in the range [0.0, 1.0] .
    /// The easing function is expected to ease from an origin
    /// hue to a destination by a factor according to a range.
	/// Mixes in LAB if the chroma of either origin or destination
	/// is near zero.
    /// </summary>
    /// <param name="o">origin color</param>
    /// <param name="d">destination color</param>
    /// <param name="t">step</param>
    /// <param name="easing">easing function</param>
    /// <returns>mixed color</returns>
    public static Lch Mix(
        in Lch o,
        in Lch d,
        in float t,
        in Func<float, float, float, float, float> easing)
    {
        float oc = o.c;
        float oh = o.h;

        float dc = d.c;
        float dh = d.h;

        float u = 1.0f - t;
        float cl = u * o.l + t * d.l;
        float calpha = u * o.alpha + t * d.alpha;

        bool oGray = oc < Utils.Epsilon;
        bool dGray = dc < Utils.Epsilon;
        if (oGray || dGray)
        {
            float oa = oGray ? 0.0f : oc * MathF.Cos(oh);
            float ob = oGray ? 0.0f : oc * MathF.Sin(oh);

            float da = dGray ? 0.0f : dc * MathF.Cos(dh);
            float db = dGray ? 0.0f : dc * MathF.Sin(dh);

            float ca = u * oa + t * da;
            float cb = u * ob + t * db;

            float ccsq = ca * ca + cb * cb;
            if (ccsq < Utils.Epsilon)
            {
                return new Lch(l: cl, c: 0.0f, h: 0.0f, alpha: ca);
            }

            float ch = MathF.Atan2(cb, ca);
            ch = (ch < -0.0f) ? ch + Utils.Tau : ch;
            ch *= Utils.OneTau;
            float cc = MathF.Sqrt(ccsq);

            return new Lch(l: cl, c: cc, h: ch, alpha: calpha);
        }

        return new Lch(
            l: cl,
            c: u * oc + t * dc,
            h: easing(oh, dh, t, 1.0f),
            alpha: calpha);
    }

    /// <summary>
    /// Tests to see if the alpha channel of this color is less than or equal to
    /// zero, i.e., if it is completely transparent.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>evaluation</returns>
    public static bool None(in Lch c)
    {
        return c.alpha <= 0.0f;
    }

    /// <summary>
    /// Returns a string representation of a color.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string</returns>
    public static string ToString(in Lch c, in int places = 4)
    {
        return Lch.ToString(new StringBuilder(96), c, places).ToString();
    }

    /// <summary>
    /// Appends a representation of a color to a string builder.
    /// </summary>
    /// <param name="sb">string bulider</param>
    /// <param name="c">vector</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString(in StringBuilder sb, in Lch c, in int places = 4)
    {
        sb.Append("{ l: ");
        Utils.ToFixed(sb, c.l, places);
        sb.Append(", c: ");
        Utils.ToFixed(sb, c.c, places);
        sb.Append(", h: ");
        Utils.ToFixed(sb, c.h, places);
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
    public static Lch Black { get { return new Lch(0.0f, 0.0f, 0.0f, 1.0f); } }

    /// <summary>
    /// Returns the color clear black.
    /// </summary>
    /// <value>clear black</value>
    public static Lch ClearBlack { get { return new Lch(0.0f, 0.0f, 0.0f, 0.0f); } }

    /// <summary>
    /// Returns the color clear white.
    /// </summary>
    /// <value>clear white</value>
    public static Lch ClearWhite { get { return new Lch(100.0f, 0.0f, 0.0f, 0.0f); } }

    /// <summary>
    /// Returns the color white.
    /// </summary>
    /// <value>white</value>
    public static Lch White { get { return new Lch(100.0f, 0.0f, 0.0f, 1.0f); } }
}