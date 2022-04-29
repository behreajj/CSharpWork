using System.Collections.Generic;

public class SortQuantized2 : IComparer<Vec2>
{
    protected readonly int levels;

    public int Levels
    {
        get
        {
            return this.levels;
        }
    }

    public SortQuantized2 (in int levels = (int) (1.0f / Utils.Epsilon))
    {
        // TODO: Add comments.
        this.levels = levels < 2 ? 2 : levels;
    }

    public int Compare (Vec2 a, Vec2 b)
    {
        return Vec2.Quantize (a, levels).CompareTo (Vec2.Quantize (b, levels));
    }
}