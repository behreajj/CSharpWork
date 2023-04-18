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
    public static bool FilterAlpha(in Rgb c, in float lb = 0.0f, in float ub = 1.0f)
    {
        return c.Alpha >= lb && c.Alpha <= ub;
    }

    /// <summary>
    /// Filters a color by whether its blue channel is in bounds.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="lb">lower bound</param>
    /// <param name="ub">upper bound</param>
    /// <returns>evaluation</returns>
    public static bool FilterBlue(in Rgb c, in float lb = 0.0f, in float ub = 1.0f)
    {
        return c.B >= lb && c.B <= ub;
    }

    /// <summary>
    /// Filters a color by whether its chroma is in bounds.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="lb">lower bound</param>
    /// <param name="ub">upper bound</param>
    /// <returns>evaluation</returns>
    public static bool FilterChroma(in Rgb c, in float lb = 0.0f, in float ub = 135.0f)
    {
        Lab lab = Rgb.StandardToCieLab(c);
        float chromaSq = Lab.ChromaSq(lab);
        return chromaSq >= (lb * lb) && chromaSq <= (ub * ub);
    }

    /// <summary>
    /// Filters a color by whether its green channel is in bounds.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="lb">lower bound</param>
    /// <param name="ub">upper bound</param>
    /// <returns>evaluation</returns>
    public static bool FilterGreen(in Rgb c, in float lb = 0.0f, in float ub = 1.0f)
    {
        return c.G >= lb && c.G <= ub;
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
    public static bool FilterHue(in Rgb c, in float lb = 0.0f, in float ub = 1.0f)
    {
        Lab lab = Rgb.StandardToCieLab(c);
        float a = lab.A;
        float b = lab.B;
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
    public static bool FilterLightness(in Rgb c, in float lb = 0.0f, in float ub = 100.0f)
    {
        Lab lab = Rgb.StandardToCieLab(c);
        return lab.L >= lb && lab.L <= ub;
    }

    /// <summary>
    /// Filters a color by whether its red channel is in bounds.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="lb">lower bound</param>
    /// <param name="ub">upper bound</param>
    /// <returns>evaluation</returns>
    public static bool FilterRed(in Rgb c, in float lb = 0.0f, in float ub = 1.0f)
    {
        return c.R >= lb && c.R <= ub;
    }
}