using System.Collections.Generic;

/// <summary>
/// Compares two colors by lightness in CIE LCH.
/// </summary>
public class SortLight : IComparer<Clr>
{
    /// <summary>
    /// Compares two colors.
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public int Compare(Clr a, Clr b)
    {
        return Clr.StandardToCieLch(a).z
            .CompareTo(Clr.StandardToCieLch(b).z);
    }
}