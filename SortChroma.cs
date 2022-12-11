using System.Collections.Generic;

/// <summary>
/// Compares two colors by chroma in CIE LCH.
/// </summary>
public class SortChroma : IComparer<Clr>
{
    /// <summary>
    /// Compares two colors.
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public int Compare(Clr a, Clr b)
    {
        return Clr.StandardToCieLch(a).y
            .CompareTo(Clr.StandardToCieLch(b).y);
    }
}