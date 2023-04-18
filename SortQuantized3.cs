using System.Collections.Generic;

/// <summary>
/// Sorts 3D vectors by quantizing them such that nearby
/// vectors will be treated as equal.
/// </summary>
public class SortQuantized3 : IComparer<Vec3>
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
    public SortQuantized3(in int levels = (int)(1.0f / Utils.Epsilon))
    {
        this.levels = levels < 2 ? 2 : levels;
    }

    /// <summary>
    /// Compares two quantized vectors.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public int Compare(Vec3 a, Vec3 b)
    {
        return Vec3.Quantize(a, levels).CompareTo(Vec3.Quantize(b, levels));
    }
}