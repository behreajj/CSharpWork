using System;
using System.Text;

/// <summary>
/// Represents the edge loop of vertex indices that form the
/// face of a mesh.
/// </summary>
public class Loop2
{
    /// <summary>
    /// Array of compound vertex indices.
    /// </summary>
    protected Index2[] indices;

    /// <summary>
    /// Array of compound vertex indices.
    /// </summary> 
    /// <value>indices</value>
    public Index2[] Indices
    {
        get
        {
            return this.indices;
        }

        set
        {
            this.indices = value;
        }
    }

    /// <summary>
    /// The number of vertex indices in this loop.
    /// </summary>
    /// <value>length</value>
    public int Length { get { return this.indices.Length; } }

    /// <summary>
    /// Gets or sets a mesh compound index at i. Wraps around, so
    /// negative indices may be used.
    /// </summary>
    /// <value>mesh index</value>
    public Index2 this[int i]
    {
        get
        {
            return this.indices[Utils.RemFloor(i, this.indices.Length)];
        }

        set
        {
            this.indices[Utils.RemFloor(i, this.indices.Length)] = value;
        }
    }

    /// <summary>
    /// Constructs a new loop with three indices, the minimum
    /// number to form an enclosed face.
    /// </summary>
    public Loop2()
    {
        this.indices = new Index2[3];
    }

    /// <summary>
    /// Constructs a loop with the given number of indices,
    /// a minimum of three.
    /// </summary>
    /// <param name="length">index length</param>
    public Loop2(in int length)
    {
        this.indices = new Index2[length < 3 ? 3 : length];
    }

    /// <summary>
    /// Constructs a loop from an index array.
    /// </summary>
    /// <param name="indices">indices</param>
    public Loop2(in Index2[] indices)
    {
        this.indices = indices;
    }

    /// <summary>
    /// Constructs a loop from a list of indices.
    /// </summary>
    /// <param name="indices">indices</param>
    public Loop2(params Index2[] indices)
    {
        this.indices = indices;
    }

    /// <summary>
    /// Returns a string representation of this loop.
    /// </summary>
    /// <returns>the string</returns>
    public override string ToString()
    {
        return Loop2.ToString(this);
    }

    /// <summary>
    /// Reverses the indices in this loop.
    /// </summary>
    /// <returns>this loop</returns>
    public Loop2 Reverse()
    {
        Array.Reverse(this.indices);
        return this;
    }

    /// <summary>
    /// Convenience method. Sets the target loop's indices to a new array
    /// created from the arguments. Creates a hexagon.
    /// </summary>
    /// <param name="a">first vertex</param>
    /// <param name="b">second vertex</param>
    /// <param name="c">third vertex</param>
    /// <param name="d">fourth vertex</param>
    /// <param name="e">fifth vertex</param>
    /// <param name="f">sixth vertex</param>
    /// <param name="target">target loop</param>
    /// <returns>hexagon loop</returns>
    public static Loop2 Hex( //
        in Index2 a, //
        in Index2 b, //
        in Index2 c, //
        in Index2 d, //
        in Index2 e, //
        in Index2 f, //
        in Loop2 target)
    {
        target.indices = new Index2[] { a, b, c, d, e, f };
        return target;
    }

    /// <summary>
    /// Convenience method. Sets the target loop's indices to a new array
    /// created from the arguments. Creates a quadrilateral.
    /// </summary>
    /// <param name="a">first vertex</param>
    /// <param name="b">second vertex</param>
    /// <param name="c">third vertex</param>
    /// <param name="d">fourth vertex</param>
    /// <param name="target">target loop</param>
    /// <returns>quadrilateral loop</returns>
    public static Loop2 Quad( //
        in Index2 a, //
        in Index2 b, //
        in Index2 c, //
        in Index2 d, //
        in Loop2 target)
    {
        target.indices = new Index2[] { a, b, c, d };
        return target;
    }

    /// <summary>
    /// Resizes an array of loops. If the size is less than one, returns an empty array.
    /// If the input array is null, creates an array of new loops. Existing loops in the
    /// old array are passed to the resized array by reference. There is an option to
    /// resize the length of these existing loops to match the length of new loops.
    /// </summary>
    /// <param name="arr">array</param>
    /// <param name="sz">size</param>
    /// <param name="vertsPerLoop">vertices per loop</param>
    /// <param name="resizeExisting">change existing loop length</param>
    /// <returns>resized array</returns>
    public static Loop2[] Resize( //
        in Loop2[] arr, // 
        in int sz, //
        in int vertsPerLoop = 3, //
        in bool resizeExisting = false)
    {
        if (sz < 1) { return new Loop2[] { }; }
        Loop2[] result = new Loop2[sz];

        int vplVal = vertsPerLoop < 3 ? 3 : vertsPerLoop;
        if (arr == null)
        {
            for (int i = 0; i < sz; ++i)
            {
                result[i] = new Loop2(vplVal);
            }
            return result;
        }

        int last = arr.Length - 1;
        for (int i = 0; i < sz; ++i)
        {
            if (i > last || arr[i] == null)
            {
                result[i] = new Loop2(vplVal);
            }
            else
            {
                result[i] = arr[i];
                if (resizeExisting)
                {
                    result[i].indices = Index2.Resize(result[i].indices, vplVal);
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Splices an array of loops into the midst of another. For use by
    /// subdivision functions. If the number of deletions exceeds the length of
    /// the target array, then a copy of the insert array is returned.
    /// </summary>
    /// <param name="arr">array</param>
    /// <param name="index">insertion point</param>
    /// <param name="deletions">deletion count</param>
    /// <param name="insert">insert</param>
    /// <returns>spliced array</returns>
    public static Loop2[] Splice(in Loop2[] arr, in int index, in int deletions, in Loop2[] insert)
    {
        int aLen = arr.Length;
        if (deletions >= aLen)
        {
            Loop2[] result0 = new Loop2[insert.Length];
            System.Array.Copy(insert, 0, result0, 0, insert.Length);
            return result0;
        }

        int bLen = insert.Length;
        int valIdx = Utils.RemFloor(index, aLen + 1);
        if (deletions < 1)
        {
            Loop2[] result1 = new Loop2[aLen + bLen];
            System.Array.Copy(arr, 0, result1, 0, valIdx);
            System.Array.Copy(insert, 0, result1, valIdx, bLen);
            System.Array.Copy(arr, valIdx, result1, valIdx + bLen, aLen - valIdx);
            return result1;
        }

        int idxOff = valIdx + deletions;
        Loop2[] result = new Loop2[aLen + bLen - deletions];
        System.Array.Copy(arr, 0, result, 0, valIdx);
        System.Array.Copy(insert, 0, result, valIdx, bLen);
        System.Array.Copy(arr, idxOff, result, valIdx + bLen, aLen - idxOff);
        return result;
    }

    /// <summary>
    /// Returns a string representation of a loop.
    /// </summary>
    /// <param name="l">loop</param>
    /// <param name="padding">padding</param>
    /// <returns>string</returns>
    public static string ToString(in Loop2 l, in int padding = 3)
    {
        return Loop2.ToString(new StringBuilder(1024), l, padding).ToString();
    }

    /// <summary>
    /// Appends a string representation of a loop
    /// to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="l">loop</param>
    /// <param name="padding">padding</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString(in StringBuilder sb, in Loop2 l, in int padding = 3)
    {
        sb.Append("{ indices: ");
        Index2.ToString(sb, l.indices, padding);
        sb.Append(' ');
        sb.Append('}');
        return sb;
    }

    /// <summary>
    /// Returns a string representation of an array of loops.
    /// </summary>
    /// <param name="arr">array</param>
    /// <param name="padding">padding</param>
    /// <returns>string</returns>
    public static string ToString(in Loop2[] arr, in int padding = 3)
    {
        return Loop2.ToString(new StringBuilder(1024), arr, padding).ToString();
    }

    /// <summary>
    /// Appends a string representation of an array of loops
    /// to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="arr">array</param>
    /// <param name="padding">padding</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString(in StringBuilder sb, in Loop2[] arr, in int padding = 3)
    {
        sb.Append('[');
        sb.Append(' ');

        if (arr != null)
        {
            int len = arr.Length;
            int last = len - 1;

            for (int i = 0; i < last; ++i)
            {
                Loop2.ToString(sb, arr[i], padding);
                sb.Append(',');
                sb.Append(' ');
            }

            Loop2.ToString(sb, arr[last], padding);
            sb.Append(' ');
        }

        sb.Append(']');
        return sb;
    }

    /// <summary>
    /// Convenience method. Sets the target loop's indices to a new array
    /// created from the arguments. Creates a triangle.
    /// </summary>
    /// <param name="a">first vertex</param>
    /// <param name="b">second vertex</param>
    /// <param name="c">third vertex</param>
    /// <param name="target">target loop</param>
    /// <returns>triangle loop</returns>
    public static Loop2 Tri( //
        in Index2 a, //
        in Index2 b, //
        in Index2 c, //
        in Loop2 target)
    {
        target.indices = new Index2[] { a, b, c };
        return target;
    }
}