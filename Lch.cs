using System;
using System.Text;

/// <summary>
/// A readonly struct. Represents colors in a perceptual color space, such as
/// CIE LCH, SR LCH, OK LCH, etc., with polar coordinates. Lightness falls in
/// the range [0.0, 100.0]. Chroma is unbounded, but has a practical range of
/// roughly [0.0, 135.0]. Hue is a periodic number in [0.0, 1.0]. Alpha is
/// expected to be in [0.0, 1.0].
/// </summary>
[Serializable]
public readonly struct Lch
{
    /// <summary>
    /// The upper bound for the chroma of a color converted from standard RGB
    /// to SR LCH, sampled without respect for lightness.
    /// </summary>
    public const float AbsMaxC = 119.07602f;

    /// <summary>
    /// Denominator used when normalizing the chroma of a color in SR LCH.
    /// Equivalent to 1.0 / AbsMaxC.
    /// </summary>
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
    /// Adds two colors together for the purpose of making an adjustment.
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
    /// Subtracts the right color from the left for the purpose of making
    /// an adjustment.
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
    /// Returns the first color argument with the alpha of the second.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>color</returns>
    public static Lch CopyAlpha(in Lch a, in Lch b)
    {
        return new Lch(l: a.l, c: a.c, h: a.h, alpha: b.alpha);
    }

    /// <summary>
    /// Returns the first color argument with the light of the second.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>color</returns>
    public static Lch CopyLight(in Lch a, in Lch b)
    {
        return new Lch(l: b.l, c: a.c, h: a.h, alpha: a.alpha);
    }

    /// <summary>
    /// Attempts to fit an LCH color to gamut by iteratively reducing its
    /// chroma until its standard RGB representation has channels within gamut.
    /// </summary>
    /// <param name="c">LCH color</param>
    /// <param name="step">chroma step</param>
    /// <returns>fitted color</returns>
    public static Lch FitToGamut(
        in Lch c,
        in float step = 0.25f)
    {
        float stepVrf = MathF.Max(Utils.Epsilon, MathF.Abs(step));

        float lTrg = Utils.Clamp(c.l, 0.0f, 100.0f);
        float cTrg = MathF.Max(0.0f, c.c);
        float hTrg = c.h - MathF.Floor(c.h);
        float alphaTrg = Utils.Clamp(c.alpha, 0.0f, 1.0f);
        Lch target = new(l: lTrg, c: cTrg, h: hTrg, alpha: alphaTrg);

        while (cTrg > 0.0f)
        {
            Rgb srgb = Rgb.SrLchToStandard(target);
            if (Rgb.IsInGamut(srgb))
            {
                return target;
            }

            cTrg -= stepVrf;
            target = new Lch(l: lTrg, c: cTrg, h: hTrg, alpha: alphaTrg);
        }
        return new Lch(l: lTrg, c: 0.0f, h: hTrg, alpha: alphaTrg);
    }

    /// <summary>
    /// Converts a color from LAB to LCH. If the chroma is near zero, then the
    /// hue is set to zero.
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
    /// Returns a gray version of the color, where chroma and hue are zero.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>gray</returns>
    public static Lch Gray(in Lch c)
    {
        return new Lch(
            l: c.l,
            c: 0.0f,
            h: 0.0f,
            alpha: c.alpha);
    }

    /// <summary>
    /// Finds the analogous color harmonies for the color.
    /// Returns an array containing two colors.
    /// </summary>
    /// <param name="c">LCH color</param>
    /// <returns>analogues</returns>
    public static Lch[] HarmonyAnalogous(in Lch c)
    {
        float lAna = (c.l * 2.0f + 50.0f) / 3.0f;

        float h30 = c.h + 0.083333333f;
        float h330 = c.h - 0.083333333f;

        return new Lch[] {
            new(lAna, c.c, h30 - MathF.Floor(h30), c.alpha),
            new(lAna, c.c, h330 - MathF.Floor(h330), c.alpha)
        };
    }

    /// <summary>
    /// Finds the complementary color harmony for the color.
    /// Returns an array containing one color.
    /// </summary>
    /// <param name="c">LCH color</param>
    /// <returns>complement</returns>
    public static Lch[] HarmonyComplement(in Lch c)
    {
        float lCmp = 100.0f - c.l;

        float h180 = c.h + 0.5f;

        return new Lch[] {
            new(lCmp, c.c, h180 - MathF.Floor(h180), c.alpha)
        };
    }

    /// <summary>
    /// Finds the split color harmonies for the color.
    /// Returns an array containing two colors.
    /// </summary>
    /// <param name="c">LCH color</param>
    /// <returns>split</returns>
    public static Lch[] HarmonySplit(in Lch c)
    {
        float lSpl = (250.0f - c.l * 2.0f) / 3.0f;

        float h150 = c.h + 0.41666667f;
        float h210 = c.h - 0.41666667f;

        return new Lch[] {
            new(lSpl, c.c, h150 - MathF.Floor(h150), c.alpha),
            new(lSpl, c.c, h210 - MathF.Floor(h210), c.alpha)
        };
    }

    /// <summary>
    /// Finds the square color harmonies for the color.
    /// Returns an array containing three colors.
    /// </summary>
    /// <param name="c">LCH color</param>
    /// <returns>square</returns>
    public static Lch[] HarmonySquare(in Lch c)
    {
        float lCmp = 100.0f - c.l;

        float h90 = c.h + 0.25f;
        float h180 = c.h + 0.5f;
        float h270 = c.h - 0.25f;

        return new Lch[] {
            new(50.0f, c.c, h90 - MathF.Floor(h90), c.alpha),
            new(lCmp, c.c, h180 - MathF.Floor(h180), c.alpha),
            new(50.0f, c.c, h270 - MathF.Floor(h270), c.alpha)
        };
    }

    /// <summary>
    /// Finds the tetradic color harmonies for the color.
    /// Returns an array containing three colors.
    /// </summary>
    /// <param name="c">LCH color</param>
    /// <returns>tetrad</returns>
    public static Lch[] HarmonyTetradic(in Lch c)
    {
        float lTri = (200.0f - c.l) / 3.0f;
        float lCmp = 100.0f - c.l;
        float lTet = (100.0f + c.l) / 3.0f;

        float h120 = c.h + Utils.OneThird;
        float h180 = c.h + 0.5f;
        float h300 = c.h - 0.16666667f;

        return new Lch[] {
            new(lTri, c.c, h120 - MathF.Floor(h120), c.alpha),
            new(lCmp, c.c, h180 - MathF.Floor(h180), c.alpha),
            new(lTet, c.c, h300 - MathF.Floor(h300), c.alpha)
        };
    }

    /// <summary>
    /// Finds the triadic color harmonies for the color.
    /// Returns an array containing two colors.
    /// </summary>
    /// <param name="c">LCH color</param>
    /// <returns>triad</returns>
    public static Lch[] HarmonyTriadic(in Lch c)
    {
        float lTri = (200.0f - c.l) / 3.0f;

        float h120 = c.h + Utils.OneThird;
        float h240 = c.h - Utils.OneThird;

        return new Lch[] {
            new(lTri, c.c, h120 - MathF.Floor(h120), c.alpha),
            new(lTri, c.c, h240 - MathF.Floor(h240), c.alpha)
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
    /// Mixes two colors by a step in the range [0.0, 1.0].
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
    /// Mixes two colors by a step in the range [0.0, 1.0]. The easing function
    /// is expected to ease from an origin hue to a destination by a factor
    /// according to a range [0.0, 1.0]. Mixes in LAB if the chroma of either
    /// origin or destination is near zero.
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
        float u = 1.0f - t;
        float cl = u * o.l + t * d.l;
        float calpha = u * o.alpha + t * d.alpha;

        bool oIsGray = o.c < Utils.Epsilon;
        bool dIsGray = d.c < Utils.Epsilon;
        if (oIsGray && dIsGray)
        {
            return new Lch(l: cl, c: 0.0f, h: 0.0f, alpha: calpha);
        }

        if (oIsGray || dIsGray)
        {
            float ohRadians = o.h * Utils.Tau;
            float oa = oIsGray ? 0.0f : o.c * MathF.Cos(ohRadians);
            float ob = oIsGray ? 0.0f : o.c * MathF.Sin(ohRadians);

            float dhRadians = d.h * Utils.Tau;
            float da = dIsGray ? 0.0f : d.c * MathF.Cos(dhRadians);
            float db = dIsGray ? 0.0f : d.c * MathF.Sin(dhRadians);

            // Mix in LAB.
            float ca = u * oa + t * da;
            float cb = u * ob + t * db;

            // Zero chroma should already be taken care of by the early return
            // above for both o and d gray.
            float cc = MathF.Sqrt(ca * ca + cb * cb);

            float ch = MathF.Atan2(cb, ca);
            ch = (ch < -0.0f) ? ch + Utils.Tau : ch;
            ch *= Utils.OneTau;

            return new Lch(l: cl, c: cc, h: ch, alpha: calpha);
        }

        return new Lch(
            l: cl,
            c: u * o.c + t * d.c,
            h: easing(o.h, d.h, t, 1.0f),
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
    /// Finds the saturation of a color as defined by Manfred Richter and Eva
    /// Lubbe. See https://www.wikiwand.com/en/Colorfulness#CIELUV_and_CIELAB .
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>saturation</returns>
    public static float Saturation(in Lch c)
    {
        double ld = c.l;
        double cd = c.c;

        double clsq = cd * cd + ld * ld;
        if (clsq > 0.0d) { return (float)(cd / Math.Sqrt(clsq)); }
        return 0.0f;
    }

    /// <summary>
    /// Finds an array of colors suitable for shading with the base.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="count">count</param>
    /// <param name="chromaStep">chromaStep</param>
    /// <returns>shades</returns>
    public static Lch[] Shades(
        in Lch c,
        in int count = 7,
        in float lightStep = 31.25f,
        in float chromaStep = 7.5f,
        in float hueStep = 0.5f)
    {
        float hueStepVrf = MathF.Max(0.0f, MathF.Abs(hueStep));
        float chromaStepVrf = MathF.Max(0.0f, MathF.Abs(chromaStep));
        float lightStepVrf = MathF.Max(0.0f, MathF.Abs(lightStep));
        int countVrf = count < 3 ? 3 : count;

        float hViolet = 0.80922841685655f + Utils.Epsilon;
        float hYellow = hViolet - 0.5f;

        float minLight = MathF.Max(0.25f, c.l - lightStepVrf);
        float maxLight = MathF.Min(99.75f, c.l + lightStepVrf);
        float midLight = (minLight + maxLight) * 0.5f;
        float minChromaShd = MathF.Max(Utils.Epsilon, c.c - chromaStepVrf);
        float minChromaLgt = MathF.Max(Utils.Epsilon, c.c - chromaStepVrf);
        float toShdFac = MathF.Abs(50.0f - minLight) * 0.02f;
        float toLgtFac = MathF.Abs(50.0f - maxLight) * 0.02f;

        float shdHue = Utils.LerpAngleNear(c.h, hViolet,
            hueStepVrf * toShdFac, 1.0f);
        float lgtHue = Utils.LerpAngleNear(c.h, hYellow,
            hueStepVrf * toLgtFac, 1.0f);

        float shdCrm = Utils.Mix(c.c, minChromaShd, toShdFac);
        float lgtCrm = Utils.Mix(c.c, minChromaLgt, toLgtFac);

        Lab labShd = Lab.FromLch(new(minLight, shdCrm, shdHue, c.alpha));
        Lab labKey = Lab.FromLch(new(midLight, c.c, c.h, c.alpha));
        Lab labLgt = Lab.FromLch(new(maxLight, lgtCrm, lgtHue, c.alpha));

        Curve3 curve = new(false, 3);
        Curve3.FromLinear(curve, new Vec3[] {
                new(labShd.A, labShd.B, labShd.L),
                new(labKey.A, labKey.B, labKey.L),
                new(labLgt.A, labLgt.B, labLgt.L)
            }, false);
        Curve3.SmoothHandles(curve);

        Lch[] swatches = new Lch[countVrf];
        float iToFac = 1.0f / countVrf;
        for (int i = 0; i < countVrf; ++i)
        {
            (Vec3 co, _) = Curve3.Eval(curve, i * iToFac);
            swatches[i] = Lch.FromLab(new(co.Z, co.X, co.Y, c.alpha));
        }

        return swatches;
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
    public static StringBuilder ToString(
        in StringBuilder sb,
        in Lch c,
        in int places = 4)
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
    public static Lch Black
    {
        get
        {
            return new Lch(
                l: 0.0f,
                c: 0.0f,
                h: 0.0f,
                alpha: 1.0f);
        }
    }

    /// <summary>
    /// Returns the color clear black.
    /// </summary>
    /// <value>clear black</value>
    public static Lch ClearBlack
    {
        get
        {
            return new Lch(
                l: 0.0f,
                c: 0.0f,
                h: 0.0f,
                alpha: 0.0f);
        }
    }

    /// <summary>
    /// Returns the color clear white.
    /// </summary>
    /// <value>clear white</value>
    public static Lch ClearWhite
    {
        get
        {
            return new Lch(
                l: 100.0f,
                c: 0.0f,
                h: 0.0f,
                alpha: 0.0f);
        }
    }

    /// <summary>
    /// Returns the color blue in SR LCH.
    /// </summary>
    /// <value>blue</value>
    public static Lch SrBlue
    {
        get
        {
            return new Lch(
                l: 30.6439499148523f,
                c: 111.458462917368f,
                h: 0.73279449277552f,
                alpha: 1.0f);
        }
    }

    /// <summary>
    /// Returns the color cyan in SR LCH.
    /// </summary>
    /// <value>cyan</value>
    public static Lch SrCyan
    {
        get
        {
            return new Lch(
                l: 90.624702543393f,
                c: 46.3021884777733f,
                h: 0.55254010973227f,
                alpha: 1.0f);
        }
    }

    /// <summary>
    /// Returns the color green in SR LCH.
    /// </summary>
    /// <value>green</value>
    public static Lch SrGreen
    {
        get
        {
            return new Lch(
                l: 87.5151869060628f,
                c: 117.374612112472f,
                h: 0.37492251819407f,
                alpha: 1.0f);
        }
    }

    /// <summary>
    /// Returns the color magenta in SR LCH.
    /// </summary>
    /// <value>magenta</value>
    public static Lch SrMagenta
    {
        get
        {
            return new Lch(
                l: 60.2552107535831f,
                c: 119.431303173551f,
                h: 0.91467999408849f,
                alpha: 1.0f);
        }
    }

    /// <summary>
    /// Returns the color red in SR LCH.
    /// </summary>
    /// <value>red</value>
    public static Lch SrRed
    {
        get
        {
            return new Lch(
                l: 53.225973948503f,
                c: 103.437344089924f,
                h: 0.11356219478123f,
                alpha: 1.0f);
        }
    }

    /// <summary>
    /// Returns the color yellow in SR LCH.
    /// </summary>
    /// <value>yellow</value>
    public static Lch SrYellow
    {
        get
        {
            return new Lch(
                l: 97.3452582060733f,
                c: 102.180881444855f,
                h: 0.30922841685654f,
                alpha: 1.0f);
        }
    }

    /// <summary>
    /// Returns the color white.
    /// </summary>
    /// <value>white</value>
    public static Lch White
    {
        get
        {
            return new Lch(
                l: 100.0f,
                c: 0.0f,
                h: 0.0f,
                alpha: 1.0f);
        }
    }
}