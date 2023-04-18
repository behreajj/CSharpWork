using System.Collections.Generic;

/// <summary>
/// Compares two face loops by averaging the vectors
/// that they reference, then comparing the averages.
/// </summary>
public class SortLoops3 : IComparer<Loop3>
{
    /// <summary>
    /// Coordinates referenced by loop indices.
    /// </summary>
    protected readonly Vec3[] coords;

    /// <summary>
    /// Constructs a loop sorting comparator from
    /// an array to a mesh's coordinates.
    /// </summary>
    /// <param name="coords">mesh coordinates</param>
    public SortLoops3(in Vec3[] coords)
    {
        this.coords = coords;
    }

    /// <summary>
    /// Compares two loops in compliance with the IComparer interface.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public int Compare(Loop3 a, Loop3 b)
    {
        Vec3 aAvg = Vec3.Zero;
        Index3[] aIdcs = a.Indices;
        int aLen = aIdcs.Length;
        for (int i = 0; i < aLen; ++i)
        {
            aAvg += this.coords[aIdcs[i].V];
        }
        aAvg /= aLen;

        Vec3 bAvg = Vec3.Zero;
        Index3[] bIdcs = b.Indices;
        int bLen = bIdcs.Length;
        for (int i = 0; i < bLen; ++i)
        {
            bAvg += this.coords[bIdcs[i].V];
        }
        bAvg /= bLen;

        return aAvg.CompareTo(bAvg);
    }
}