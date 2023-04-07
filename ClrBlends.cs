using System;

/// <summary>
/// Implements blend functions for two colors. Functions are to be
/// supplied to an image blending function.
/// </summary>
public static class ClrBlends
{
    /// <summary>
    /// Blends two colors in LCH. The under color's lightness
    /// is retained while its hue and saturation are blended according
    /// to the transparency of the over and under color.
    /// </summary>
    /// <param name="under">under color</param>
    /// <param name="over">over color</param>
    /// <returns>blended color</returns>
    public static Rgb Color(in Rgb under, in Rgb over)
    {
        float t = over.a;
        float v = under.a;

        if (t <= 0.0f) { return under; }
        if (v <= 0.0f) { return over; }

        Lch uLch = Rgb.StandardToCieLch(under);
        Lch oLch = Rgb.StandardToCieLch(over);

        float u = 1.0f - t;
        float tuv = MathF.Min(1.0f, t + u * v);
        float cc = u * uLch.C + t * oLch.C;
        float ch = u * uLch.H + t * oLch.H;
        ch -= MathF.Floor(ch);

        Lch cLch = new(l: uLch.L, c: cc, h: ch, alpha: tuv);
        return Rgb.CieLchToStandard(cLch);
    }

    /// <summary>
    /// Blends two colors in LCH. The under color's lightness
    /// and chroma are retained while its hue is blended according
    /// to the transparency of the over and under color.
    /// </summary>
    /// <param name="under">under color</param>
    /// <param name="over">over color</param>
    /// <returns>blended color</returns>
    public static Rgb Hue(in Rgb under, in Rgb over)
    {
        float t = over.a;
        float v = under.a;

        if (t <= 0.0f) { return under; }
        if (v <= 0.0f) { return over; }

        Lch uLch = Rgb.StandardToCieLch(under);
        Lch oLch = Rgb.StandardToCieLch(over);

        float u = 1.0f - t;
        float tuv = MathF.Min(1.0f, t + u * v);
        float ch = u * uLch.H + t * oLch.H;
        ch -= MathF.Floor(ch);

        Lch cLch = new(l: uLch.L, c: uLch.C, h: ch, alpha: tuv);
        return Rgb.CieLchToStandard(cLch);
    }

    /// <summary>
    /// Blends two colors in LCH. The under color's hue
    /// and chroma are retained while its lightness is blended
    /// according to the transparency of the over and under color.
    /// </summary>
    /// <param name="under">under color</param>
    /// <param name="over">over color</param>
    /// <returns>blended color</returns>
    public static Rgb Luminosity(in Rgb under, in Rgb over)
    {
        float t = over.a;
        float v = under.a;

        if (t <= 0.0f) { return under; }
        if (v <= 0.0f) { return over; }

        Lch uLch = Rgb.StandardToCieLch(under);
        Lch oLch = Rgb.StandardToCieLch(over);

        float u = 1.0f - t;
        float tuv = MathF.Min(1.0f, t + u * v);
        float cl = u * uLch.L + t * oLch.L;

        Lch cLch = new(l: cl, c: uLch.C, h: uLch.H, alpha: tuv);
        return Rgb.CieLchToStandard(cLch);
    }

    /// <summary>
    /// Replaces the under color with the over color except
    /// when the over color is transparent.
    /// </summary>
    /// <param name="under">under color</param>
    /// <param name="over">over color</param>
    /// <returns>replacement color</returns>
    public static Rgb Replace(in Rgb under, in Rgb over)
    {
        if (over.a <= 0.0f) { return under; }
        return over;
    }

    /// <summary>
    /// Blends two colors in LCH. The under color's hue
    /// and lightness are retained while its chroma is blended
    /// according to the transparency of the over and under color.
    /// </summary>
    /// <param name="under">under color</param>
    /// <param name="over">over color</param>
    /// <returns>blended color</returns>
    public static Rgb Saturation(in Rgb under, in Rgb over)
    {
        float t = over.a;
        float v = under.a;

        if (t <= 0.0f) { return under; }
        if (v <= 0.0f) { return over; }

        Lch uLch = Rgb.StandardToCieLch(under);
        Lch oLch = Rgb.StandardToCieLch(over);

        float u = 1.0f - t;
        float tuv = MathF.Min(1.0f, t + u * v);
        float cc = u * uLch.C + t * oLch.C;

        Lch cLch = new(l: uLch.L, c: cc, h: uLch.H, alpha: tuv);
        return Rgb.CieLchToStandard(cLch);
    }

    /// <summary>
    /// Blends two colors in standard RGB.
    /// </summary>
    /// <param name="under">under color</param>
    /// <param name="over">over color</param>
    /// <returns>blended color</returns>
    public static Rgb Standard(in Rgb under, in Rgb over)
    {
        float t = over.a;
        float u = 1.0f - t;
        float v = under.a;
        float uv = u * v;
        float tuv = t + uv;
        if (tuv >= 1.0f)
        {
            return new Rgb(
                uv * under.r + t * over.r,
                uv * under.g + t * over.g,
                uv * under.b + t * over.b,
                1.0f);
        }
        else if (tuv > 0.0f)
        {
            float tuvInv = 1.0f / tuv;
            return new Rgb(
                (uv * under.r + t * over.r) * tuvInv,
                (uv * under.g + t * over.g) * tuvInv,
                (uv * under.b + t * over.b) * tuvInv,
                tuv);
        }
        return Rgb.ClearBlack;
    }
}