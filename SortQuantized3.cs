using System.Collections.Generic;

public class SortQuantized3 : IComparer<Vec3>
{
    protected readonly int levels;

    public int Levels
    {
        get
        {
            return this.levels;
        }
    }

    public SortQuantized3(in int levels = (int)(1.0f / Utils.Epsilon))
    {
        // TODO: Add comments.
        this.levels = levels < 2 ? 2 : levels;
    }

    public int Compare(Vec3 a, Vec3 b)
    {
        return Vec3.Quantize(a, levels).CompareTo(Vec3.Quantize(b, levels));
    }
}