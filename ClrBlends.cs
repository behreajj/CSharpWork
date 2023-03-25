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
    public static Clr Color(in Clr under, in Clr over)
    {
        float t = over.a;
        float v = under.a;

        if (v <= 0.0f) { return over; }
        if (t <= 0.0f) { return under; }

        Lch uLch = Clr.StandardToCieLch(under);
        Lch oLch = Clr.StandardToCieLch(over);

        float u = 1.0f - t;
        float tuv = MathF.Min(1.0f, t + u * v);
        float cc = u * uLch.C + t * oLch.C;
        float ch = u * uLch.H + t * oLch.H;
        ch -= MathF.Floor(ch);

        Lch cLch = new(l: uLch.L, c: cc, h: ch, alpha: tuv);
        return Clr.CieLchToStandard(cLch);
    }

    /// <summary>
    /// Blends two colors in LCH. The under color's lightness
    /// and chroma are retained while its hue is blended according
    /// to the transparency of the over and under color.
    /// </summary>
    /// <param name="under">under color</param>
    /// <param name="over">over color</param>
    /// <returns>blended color</returns>
    public static Clr Hue(in Clr under, in Clr over)
    {
        float t = over.a;
        float v = under.a;

        if (v <= 0.0f) { return over; }
        if (t <= 0.0f) { return under; }

        Lch uLch = Clr.StandardToCieLch(under);
        Lch oLch = Clr.StandardToCieLch(over);

        float u = 1.0f - t;
        float tuv = MathF.Min(1.0f, t + u * v);
        float ch = u * uLch.H + t * oLch.H;
        ch -= MathF.Floor(ch);

        Lch cLch = new(l: uLch.L, c: uLch.C, h: ch, alpha: tuv);
        return Clr.CieLchToStandard(cLch);
    }

    /// <summary>
    /// Blends two colors in LCH. The under color's hue
    /// and chroma are retained while its lightness is blended
    /// according to the transparency of the over and under color.
    /// </summary>
    /// <param name="under">under color</param>
    /// <param name="over">over color</param>
    /// <returns>blended color</returns>
    public static Clr Luminosity(in Clr under, in Clr over)
    {
        float t = over.a;
        float v = under.a;

        if (v <= 0.0f) { return over; }
        if (t <= 0.0f) { return under; }

        Lch uLch = Clr.StandardToCieLch(under);
        Lch oLch = Clr.StandardToCieLch(over);

        float u = 1.0f - t;
        float tuv = MathF.Min(1.0f, t + u * v);
        float cl = u * uLch.L + t * oLch.L;

        Lch cLch = new(l: cl, c: uLch.C, h: uLch.H, alpha: tuv);
        return Clr.CieLchToStandard(cLch);
    }

    /// <summary>
    /// Blends two colors in LCH. The under color's hue
    /// and lightness are retained while its chroma is blended
    /// according to the transparency of the over and under color.
    /// </summary>
    /// <param name="under">under color</param>
    /// <param name="over">over color</param>
    /// <returns>blended color</returns>
    public static Clr Saturation(in Clr under, in Clr over)
    {
        float t = over.a;
        float v = under.a;

        if (v <= 0.0f) { return over; }
        if (t <= 0.0f) { return under; }

        Lch uLch = Clr.StandardToCieLch(under);
        Lch oLch = Clr.StandardToCieLch(over);

        float u = 1.0f - t;
        float tuv = MathF.Min(1.0f, t + u * v);
        float cc = u * uLch.C + t * oLch.C;

        Lch cLch = new(l: uLch.L, c: cc, h: uLch.H, alpha: tuv);
        return Clr.CieLchToStandard(cLch);
    }
}