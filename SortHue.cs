using System.Collections.Generic;

/// <summary>
/// Compares two colors by hue in CIE LCH. If either color
/// is gray, resorts to a comparison by lightness. If hues
/// are approximately equal, resorts to a comparison by chroma.
/// </summary>
public class SortHue : IComparer<Clr>
{
    /// <summary>
    /// Default tolerance for deciding whether
    /// two hues are approximately equal. Equals
    /// a half of a degree, or one over 720.
    /// </summary>
    public const float DefaultTol = 1.0f / 720.0f;

    /// <summary>
    /// Tolerance for deciding whether two hues
    /// are approximately equal.
    /// </summary>
    protected readonly float tolerance;

    /// <summary>
    /// Tolerance for deciding whether two hues
    /// are approximately equal.
    /// </summary>
    /// <value>tolerance</value>
    public float Tolerance { get { return this.tolerance; } }

    /// <summary>
    /// Constructs a hue comparator.
    /// </summary>
    /// <param name="tolerance">hue tolerance</param>
    public SortHue(in float tolerance = SortHue.DefaultTol)
    {
        this.tolerance = Utils.Clamp(tolerance,
            float.Epsilon, 1.0f - float.Epsilon);
    }

    /// <summary>
    /// Compares two colors.
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public int Compare(Clr a, Clr b)
    {
        // Due to how Unity displays color swatches,
        // as opaque regardless of alpha, zero alpha
        // colors are treated differently here.
        Vec4 aLch = Clr.StandardToCieLch(a);
        Vec4 bLch = Clr.StandardToCieLch(b);

        if (aLch.y <= Utils.Epsilon ||
            bLch.y <= Utils.Epsilon)
        {
            return aLch.z.CompareTo(bLch.z);
        }

        if (Utils.Approx(aLch.x, bLch.x, this.tolerance))
        {
            return aLch.y.CompareTo(bLch.y);
        }

        return aLch.x.CompareTo(bLch.x);
    }
}