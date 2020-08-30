using System.Collections.Generic;

public class SortLoops2 : IComparer<Loop2>
{
    protected readonly Vec2[ ] coords;

    public SortLoops2 (in Vec2[ ] coords)
    {
        this.coords = coords;
    }

    public int Compare (Loop2 a, Loop2 b)
    {
        Vec2 aAvg = new Vec2 ( );
        Index2[ ] aIdcs = a.Indices;
        int aLen = aIdcs.Length;
        for (int i = 0; i < aLen; ++i)
        {
            aAvg += this.coords[aIdcs[i].v];
        }
        aAvg /= aLen;

        Vec2 bAvg = new Vec2 ( );
        Index2[ ] bIdcs = b.Indices;
        int bLen = bIdcs.Length;
        for (int i = 0; i < bLen; ++i)
        {
            bAvg += this.coords[bIdcs[i].v];
        }
        bAvg /= bLen;

        return aAvg.CompareTo (bAvg);
    }
}