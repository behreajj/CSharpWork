using System;
using System.Text;

public class Loop2
{
    protected Index2[ ] indices;

    public Index2[ ] Indices
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

    public int Length { get { return this.indices.Length; } }

    public Index2 this [int i]
    {
        get
        {
            return this.indices[Utils.RemFloor (i, this.indices.Length)];
        }

        set
        {
            this.indices[Utils.RemFloor (i, this.indices.Length)] = value;
        }
    }

    public int this [int i, int j]
    {
        get
        {
            return this [i][j];
        }
    }

    public Loop2 ( )
    {
        this.indices = new Index2[3];
    }

    public Loop2 (in int length)
    {
        this.indices = new Index2[length < 3 ? 3 : length];
    }

    public Loop2 (in Index2[ ] indices)
    {
        this.indices = indices;
    }

    public Loop2 (params Index2[ ] indices)
    {
        this.indices = indices;
    }

    public override string ToString ( )
    {
        return this.ToString (1);
    }

    public string ToString (in int padding = 1)
    {
        int len = this.indices.Length;
        int last = len - 1;
        StringBuilder sb = new StringBuilder (96 * len);
        sb.Append ("{ indices: [ ");
        for (int i = 0; i < len; ++i)
        {
            sb.Append (this.indices[i].ToString (padding));
            if (i < last)
            {
                sb.Append (", ");
            }
        }
        sb.Append (" ] }");
        return sb.ToString ( );
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
    public static Loop2 Hex ( //
        in Index2 a, //
        in Index2 b, //
        in Index2 c, //
        in Index2 d, //
        in Index2 e, //
        in Index2 f, //
        in Loop2 target)
    {
        target.indices = new Index2[ ] { a, b, c, d, e, f };
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
    public static Loop2 Quad ( //
        in Index2 a, //
        in Index2 b, //
        in Index2 c, //
        in Index2 d, //
        in Loop2 target)
    {
        target.indices = new Index2[ ] { a, b, c, d };
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
    public static Loop2[ ] Resize ( //
        in Loop2[ ] arr, // 
        in int sz, //
        in int vertsPerLoop = 3, //
        in bool resizeExisting = false)
    {
        if (sz < 1) { return new Loop2[ ] { }; }
        Loop2[ ] result = new Loop2[sz];

        int vplVal = vertsPerLoop < 3 ? 3 : vertsPerLoop;
        if (arr == null)
        {
            for (int i = 0; i < sz; ++i)
            {
                result[i] = new Loop2 (vplVal);
            }
            return result;
        }

        int last = arr.Length - 1;
        for (int i = 0; i < sz; ++i)
        {
            if (i > last || arr[i] == null)
            {
                result[i] = new Loop2 (vplVal);
            }
            else
            {
                result[i] = arr[i];
                if (resizeExisting)
                {
                    result[i].indices = Index2.Resize (result[i].indices, vplVal);
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
    /// <returns>the spliced array</returns>
    public static Loop2[ ] Splice (in Loop2[ ] arr, in int index, in int deletions, in Loop2[ ] insert)
    {
        int aLen = arr.Length;
        if (deletions >= aLen)
        {
            Loop2[ ] result0 = new Loop2[insert.Length];
            System.Array.Copy (insert, 0, result0, 0, insert.Length);
            return result0;
        }

        int bLen = insert.Length;
        int valIdx = Utils.RemFloor (index, aLen + 1);
        if (deletions < 1)
        {
            Loop2[ ] result1 = new Loop2[aLen + bLen];
            System.Array.Copy (arr, 0, result1, 0, valIdx);
            System.Array.Copy (insert, 0, result1, valIdx, bLen);
            System.Array.Copy (arr, valIdx, result1, valIdx + bLen, aLen - valIdx);
            return result1;
        }

        int idxOff = valIdx + deletions;
        Loop2[ ] result = new Loop2[aLen + bLen - deletions];
        System.Array.Copy (arr, 0, result, 0, valIdx);
        System.Array.Copy (insert, 0, result, valIdx, bLen);
        System.Array.Copy (arr, idxOff, result, valIdx + bLen, aLen - idxOff);
        return result;
    }

    /// <summary>
    /// Returns a string representation of an array of loops.
    /// </summary>
    /// <param name="arr">array</param>
    /// <param name="padding">padding</param>
    /// <returns>the string</returns>
    public static string ToString (in Loop2[ ] arr, in int padding = 1)
    {
        StringBuilder sb = new StringBuilder (1024);
        sb.Append ('[');
        sb.Append (' ');

        if (arr != null)
        {
            int len = arr.Length;
            int last = len - 1;

            for (int i = 0; i < last; ++i)
            {
                sb.Append (arr[i].ToString (padding));
                sb.Append (',');
                sb.Append (' ');
            }

            sb.Append (arr[last].ToString (padding));
            sb.Append (' ');
        }

        sb.Append (']');
        return sb.ToString ( );
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
    public static Loop2 Tri ( //
        in Index2 a, //
        in Index2 b, //
        in Index2 c, //
        in Loop2 target)
    {
        target.indices = new Index2[ ] { a, b, c };
        return target;
    }
}