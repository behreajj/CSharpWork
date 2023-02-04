using System;

/// <summary>
/// Implements blend functions for two colors. Functions are to be
/// supplied to an image blending function.
/// </summary>
public static class ClrBlends
{
    /// <summary>
    /// Blends two colors in CIE LCH. The under color's lightness
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

        Vec4 uLch = Clr.StandardToCieLch(under);
        Vec4 oLch = Clr.StandardToCieLch(over);

        float u = 1.0f - t;
        float tuv = MathF.Min(1.0f, t + u * v);

        Vec4 cLch = new(
                u * uLch.x + t * oLch.x,
                u * uLch.y + t * oLch.y,
                uLch.z, tuv);
        return Clr.CieLchToStandard(cLch);
    }

    /// <summary>
    /// Blends two colors in CIE LCH. The under color's lightness
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

        Vec4 uLch = Clr.StandardToCieLch(under);
        Vec4 oLch = Clr.StandardToCieLch(over);

        float u = 1.0f - t;
        float tuv = MathF.Min(1.0f, t + u * v);

        Vec4 cLch = new(
                u * uLch.x + t * oLch.x,
                uLch.y, uLch.z, tuv);
        return Clr.CieLchToStandard(cLch);
    }

    /// <summary>
    /// Blends two colors in CIE LCH. The under color's hue
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

        Vec4 uLch = Clr.StandardToCieLch(under);
        Vec4 oLch = Clr.StandardToCieLch(over);

        float u = 1.0f - t;
        float tuv = MathF.Min(1.0f, t + u * v);

        Vec4 cLch = new(uLch.x, uLch.y,
                u * uLch.z + t * oLch.z,
                tuv);
        return Clr.CieLchToStandard(cLch);
    }

    /// <summary>
    /// Blends two colors in CIE LCH. The under color's hue
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

        Vec4 uLch = Clr.StandardToCieLch(under);
        Vec4 oLch = Clr.StandardToCieLch(over);

        float u = 1.0f - t;
        float tuv = MathF.Min(1.0f, t + u * v);

        Vec4 cLch = new(uLch.x,
                u * uLch.y + t * oLch.y,
                uLch.z, tuv);
        return Clr.CieLchToStandard(cLch);
    }
}