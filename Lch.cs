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
    /// The upper bound for the chroma of a color
    /// converted from standard RGB to SR LCH, sampled
    /// without respect for lightness.
    /// </summary>
    public const float AbsMaxC = 119.07602f;

    /// Denominator used when normalizing the chroma of
    /// a color in SR LCH. Equivalent to 1.0 / AbsMaxC.
    public const float NormDenomC = 0.008397996f;

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
    /// Returns the first color argument with the alpha
    /// of the second.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>color</returns>
    public static Lch CopyAlpha(in Lch a, in Lch b)
    {
        return new(a.l, a.c, a.h, b.alpha);
    }

    /// <summary>
    /// Returns the first color argument with the light
    /// of the second.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>color</returns>
    public static Lch CopyLight(in Lch a, in Lch b)
    {
        return new(b.l, a.c, a.h, a.alpha);
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
    /// Finds the analogous color harmonies for the color.
    /// Returns an array containing two colors.
    /// </summary>
    /// <param name="a">LCH color</param>
    /// <returns>analogies</returns>
    public static (Lch, Lch) HarmonyAnalogous(in Lch a)
    {
        float lAna = (a.l * 2.0f + 50.0f) / 3.0f;
        float h30 = a.h + 0.08333333333333f;
        float h330 = a.h - 0.08333333333333f;
        return (
            new(lAna, a.c, h30 - MathF.Floor(h30), a.alpha),
            new(lAna, a.c, h330 - MathF.Floor(h330), a.alpha));
    }

    /// <summary>
    /// Finds the complementary color harmony for the color.
    /// Returns an array containing one color.
    /// </summary>
    /// <param name="a">LCH color</param>
    /// <returns>complement</returns>
    public static Lch[] HarmonyComplement(in Lch a)
    {
        float lCmp = 100.0f - a.l;
        float h180 = a.h + 0.5f;
        return new Lch[] { 
            new(lCmp, a.c, h180 - MathF.Floor(h180), a.alpha)
        };
    }

    /// <summary>
    /// Finds the split color harmonies for the color.
    /// Returns an array containing two colors.
    /// </summary>
    /// <param name="a">LCH color</param>
    /// <returns>split</returns>
    public static Lch[] HarmonySplit(in Lch a)
    {
        float lSpl = (250.0f - a.l * 2.0f) / 3.0f;
        float h150 = a.h + 0.41666666666667f;
        float h210 = a.h - 0.41666666666667f;
        return new Lch[] {
            new(lSpl, a.c, h150 - MathF.Floor(h150), a.alpha),
            new(lSpl, a.c, h210 - MathF.Floor(h210), a.alpha)
        };
    }

    /// <summary>
    /// Finds the square color harmonies for the color.
    /// Returns an array containing three colors.
    /// </summary>
    /// <param name="a">LCH color</param>
    /// <returns>square</returns>
    public static Lch[] HarmonySquare(in Lch a)
    {
        float lCmp = 100.0f - a.l;
        float h90 = a.h + 0.25f;
        float h180 = a.h + 0.5f;
        float h270 = a.h - 0.25f;
        return new Lch[] {
            new(50.0f, a.c, h90 - MathF.Floor(h90), a.alpha),
            new(lCmp, a.c, h180 - MathF.Floor(h180), a.alpha),
            new(50.0f, a.c, h270 - MathF.Floor(h270), a.alpha)
        };
    }

    /// <summary>
    /// Finds the tetradic color harmonies for the color.
    /// Returns an array containing three colors.
    /// </summary>
    /// <param name="a">LCH color</param>
    /// <returns>tetrad</returns>
    public static Lch[] HarmonyTetradic(in Lch a)
    {
        float lTri = (200.0f - a.l) / 3.0f;
        float lCmp = 100.0f - a.l;
        float lTet = (100.0f + a.l) / 3.0f;
        float h120 = a.h + Utils.OneThird;
        float h180 = a.h + 0.5f;
        float h300 = a.h - 0.16666666666667f;
        return new Lch[] {
            new(lTri, a.c, h120 - MathF.Floor(h120), a.alpha),
            new(lCmp, a.c, h180 - MathF.Floor(h180), a.alpha),
            new(lTet, a.c, h300 - MathF.Floor(h300), a.alpha)
        };
    }

    /// <summary>
    /// Finds the triadic color harmonies for the color.
    /// Returns an array containing two colors.
    /// </summary>
    /// <param name="a">LCH color</param>
    /// <returns>triad</returns>
    public static Lch[] HarmonyTriadic(in Lch a)
    {
        float lTri = (200.0f - a.l) / 3.0f;
        float h120 = a.h + Utils.OneThird;
        float h240 = a.h - Utils.OneThird;
        return new Lch[] {
            new(lTri, a.c, h120 - MathF.Floor(h120), a.alpha),
            new(lTri, a.c, h240 - MathF.Floor(h240), a.alpha)
        };
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
        sb.Append("{\"l\":");
        Utils.ToFixed(sb, c.l, places);
        sb.Append(",\"c\":");
        Utils.ToFixed(sb, c.c, places);
        sb.Append(",\"h\":");
        Utils.ToFixed(sb, c.h, places);
        sb.Append(",\"alpha\":");
        Utils.ToFixed(sb, c.alpha, places);
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
    /// Returns the color red in SR LCH.
    /// </summary>
    /// <value>red</value>
    public static Lch SrRed { get { return new(53.22598f, 103.4373f, 0.1135622f, 1.0f); } }

    /// <summary>
    /// Returns the color yellow in SR LCH.
    /// </summary>
    /// <value>yellow</value>
    public static Lch SrYellow { get { return new(97.34526f, 102.1809f, 0.3092285f, 1.0f); } }

    /// <summary>
    /// Returns the color green in SR LCH.
    /// </summary>
    /// <value>green</value>
    public static Lch SrGreen { get { return new(87.51519f, 117.3746f, 0.3749225f, 1.0f); } }

    /// <summary>
    /// Returns the color cyan in SR LCH.
    /// </summary>
    /// <value>cyan</value>
    public static Lch SrCyan { get { return new(90.6247f, 46.30222f, 0.5525401f, 1.0f); } }

    /// <summary>
    /// Returns the color blue in SR LCH.
    /// </summary>
    /// <value>blue</value>
    public static Lch SrBlue { get { return new(30.64395f, 111.4585f, 0.7327945f, 1.0f); } }

    /// <summary>
    /// Returns the color magenta in SR LCH.
    /// </summary>
    /// <value>magenta</value>
    public static Lch SrMagenta { get { return new(60.25521f, 119.4313f, 0.91468f, 1.0f); } }

    /// <summary>
    /// Returns the color white.
    /// </summary>
    /// <value>white</value>
    public static Lch White { get { return new Lch(100.0f, 0.0f, 0.0f, 1.0f); } }
}