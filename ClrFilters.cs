using System;

/// <summary>
/// Implements filter functions for a color. Functions are to be
/// supplied to an image filter function. Both the lower and upper
/// bounds are inclusive.
/// </summary>
public static class ClrFilters
{
    /// <summary>
    /// Filters a color by whether its alpha channel is in bounds.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="lb">lower bound</param>
    /// <param name="ub">upper bound</param>
    /// <returns>evaluation</returns>
    public static bool FilterAlpha(in Clr c, in float lb = 0.0f, in float ub = 1.0f)
    {
        return c.a >= lb && c.a <= ub;
    }

    /// <summary>
    /// Filters a color by whether its blue channel is in bounds.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="lb">lower bound</param>
    /// <param name="ub">upper bound</param>
    /// <returns>evaluation</returns>
    public static bool FilterBlue(in Clr c, in float lb = 0.0f, in float ub = 1.0f)
    {
        return c.b >= lb && c.b <= ub;
    }

    /// <summary>
    /// Filters a color by whether its chroma is in bounds.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="lb">lower bound</param>
    /// <param name="ub">upper bound</param>
    /// <returns>evaluation</returns>
    public static bool FilterChroma(in Clr c, in float lb = 0.0f, in float ub = 135.0f)
    {
        Vec4 lab = Clr.StandardToCieLab(c);
        float chromaSq = lab.x * lab.x + lab.y * lab.y;
        return chromaSq >= (lb * lb) && chromaSq <= (ub * ub);
    }

    /// <summary>
    /// Filters a color by whether its green channel is in bounds.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="lb">lower bound</param>
    /// <param name="ub">upper bound</param>
    /// <returns>evaluation</returns>
    public static bool FilterGreen(in Clr c, in float lb = 0.0f, in float ub = 1.0f)
    {
        return c.g >= lb && c.g <= ub;
    }

    /// <summary>
    /// Filters a color by whether its hue is in bounds.
    /// Does not treat the hue as a periodic value.
    /// If the color's chroma is less than epsilon, returns false.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="lb">lower bound</param>
    /// <param name="ub">upper bound</param>
    /// <returns>evaluation</returns>
    public static bool FilterHue(in Clr c, in float lb = 0.0f, in float ub = 1.0f)
    {
        Vec4 lab = Clr.StandardToCieLab(c);
        float a = lab.x;
        float b = lab.y;
        if ((a * a + b * b) < Utils.Epsilon) { return false; }

        float hue = MathF.Atan2(b, a);
        hue = hue < -0.0f ? hue + Utils.Tau : hue;
        hue *= Utils.OneTau;

        return hue >= lb && hue <= ub;
    }

    /// <summary>
    /// Filters a color by whether its lightness is in bounds.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="lb">lower bound</param>
    /// <param name="ub">upper bound</param>
    /// <returns>evaluation</returns>
    public static bool FilterLightness(in Clr c, in float lb = 0.0f, in float ub = 100.0f)
    {
        Vec4 lab = Clr.StandardToCieLab(c);
        return lab.z >= lb && lab.z <= ub;
    }

    /// <summary>
    /// Filters a color by whether its red channel is in bounds.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="lb">lower bound</param>
    /// <param name="ub">upper bound</param>
    /// <returns>evaluation</returns>
    public static bool FilterRed(in Clr c, in float lb = 0.0f, in float ub = 1.0f)
    {
        return c.r >= lb && c.r <= ub;
    }
}