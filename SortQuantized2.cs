using System.Collections.Generic;

/// <summary>
/// Sorts 2D vectors by quantizing them such that nearby vectors will be
/// treated as equal.
/// </summary>
public class SortQuantized2 : IComparer<Vec2>
{
    /// <summary>
    /// The quantization levels.
    /// </summary>
    protected readonly int levels;

    /// <summary>
    /// The quantization levels.
    /// </summary>
    /// <value>levels</value>
    public int Levels
    {
        get
        {
            return this.levels;
        }
    }

    /// <summary>
    /// Constructs a quantized comparer.
    /// </summary>
    /// <param name="levels">quantization levels</param>
    public SortQuantized2(in int levels = (int)(1.0f / Utils.Epsilon))
    {
        this.levels = levels < 2 ? 2 : levels;
    }

    /// <summary>
    /// Compares two quantized vectors.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public int Compare(Vec2 a, Vec2 b)
    {
        return Vec2.Quantize(a, levels).CompareTo(Vec2.Quantize(b, levels));
    }
}