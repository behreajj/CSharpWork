using System.Collections.Generic;

public class SortLoops3 : IComparer<Loop3>
{
    protected readonly Vec3[ ] coords;

    public SortLoops3 (in Vec3[ ] coords)
    {
        this.coords = coords;
    }

    public int Compare (Loop3 a, Loop3 b)
    {
        Vec3 aAvg = new Vec3 ( );
        Index3[ ] aIdcs = a.Indices;
        int aLen = aIdcs.Length;
        for (int i = 0; i < aLen; ++i)
        {
            aAvg += this.coords[aIdcs[i].v];
        }
        aAvg /= aLen;

        Vec3 bAvg = new Vec3 ( );
        Index3[ ] bIdcs = b.Indices;
        int bLen = bIdcs.Length;
        for (int i = 0; i < bLen; ++i)
        {
            bAvg += this.coords[bIdcs[i].v];
        }
        bAvg /= bLen;

        return aAvg.CompareTo (bAvg);
    }
}