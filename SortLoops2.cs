using System.Collections.Generic;

/// <summary>
/// Compares two face loops by averaging the vectors
/// that they reference, then comparing the averages.
/// </summary>
public class SortLoops2 : IComparer<Loop2>
{
    /// <summary>
    /// Coordinates referenced by loop indices.
    /// </summary>
    protected readonly Vec2[] coords;

    /// <summary>
    /// Constructs a loop sorting comparator from
    /// an array to a mesh's coordinates.
    /// </summary>
    /// <param name="coords">mesh coordinates</param>
    public SortLoops2(in Vec2[] coords)
    {
        this.coords = coords;
    }

    /// <summary>
    /// Compares two loops in compliance with the IComparer interface.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public int Compare(Loop2 a, Loop2 b)
    {
        Vec2 aAvg = new Vec2();
        Index2[] aIdcs = a.Indices;
        int aLen = aIdcs.Length;
        for (int i = 0; i < aLen; ++i)
        {
            aAvg += this.coords[aIdcs[i].v];
        }
        aAvg /= aLen;

        Vec2 bAvg = new Vec2();
        Index2[] bIdcs = b.Indices;
        int bLen = bIdcs.Length;
        for (int i = 0; i < bLen; ++i)
        {
            bAvg += this.coords[bIdcs[i].v];
        }
        bAvg /= bLen;

        return aAvg.CompareTo(bAvg);
    }
}