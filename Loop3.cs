using System;
using System.Text;

public class Loop3
{
    protected Index3[ ] indices;

    public Index3[ ] Indices
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

    public Index3 this [int i]
    {
        get
        {
            return this.indices[Utils.Mod (i, this.indices.Length)];
        }

        set
        {
            this.indices[Utils.Mod (i, this.indices.Length)] = value;
        }
    }

    public int this [int i, int j]
    {
        get
        {
            return this [i][j];
        }
    }

    public Loop3 ( )
    {
        this.indices = new Index3[3];
    }

    public Loop3 (in int length)
    {
        this.indices = new Index3[length < 3 ? 3 : length];
    }

    public Loop3 (in Index3[ ] indices)
    {
        this.indices = indices;
    }

    public Loop3 (params Index3[ ] indices)
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

    public static implicit operator Index3[ ] (in Loop3 source)
    {
        return source.indices;
    }

    public static implicit operator Loop3 (in Index3[ ] source)
    {
        return new Loop3 (source);
    }

    public static explicit operator Loop3 (in Loop2 source)
    {
        int len = source.Length;
        Loop3 result = new Loop3 (new Index3[len]);
        Index2[ ] srcIdcs = source.Indices;
        Index3[ ] trgIdcs = result.Indices;
        for (int i = 0; i < len; ++i)
        {
            trgIdcs[i] = (Index3) srcIdcs[i];
        }
        return result;
    }

    public static Loop3 Quad (in Index3 a, in Index3 b, in Index3 c, in Index3 d, in Loop3 target)
    {
        target.indices = new Index3[ ] { a, b, c, d };
        return target;
    }

    public static Loop3[ ] Resize (in Loop3[ ] arr, in int sz)
    {
        if (sz < 1) return new Loop3[ ] { };
        Loop3[ ] result = new Loop3[sz];

        if (arr != null)
        {
            int len = arr.Length;
            int end = sz > len ? len : sz;
            System.Array.Copy (arr, result, end);
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
    public static Loop3[ ] Splice (in Loop3[ ] arr, in int index, in int deletions, in Loop3[ ] insert)
    {
        int aLen = arr.Length;
        if (deletions >= aLen)
        {
            Loop3[ ] result0 = new Loop3[insert.Length];
            System.Array.Copy (insert, 0, result0, 0, insert.Length);
            return result0;
        }

        int bLen = insert.Length;
        int valIdx = Utils.Mod (index, aLen + 1);
        if (deletions < 1)
        {
            Loop3[ ] result1 = new Loop3[aLen + bLen];
            System.Array.Copy (arr, 0, result1, 0, valIdx);
            System.Array.Copy (insert, 0, result1, valIdx, bLen);
            System.Array.Copy (arr, valIdx, result1, valIdx + bLen, aLen - valIdx);
            return result1;
        }

        int idxOff = valIdx + deletions;
        Loop3[ ] result = new Loop3[aLen + bLen - deletions];
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
    public static string ToString (in Loop3[ ] arr, in int padding = 1)
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

    public static Loop3 Tri (in Index3 a, in Index3 b, in Index3 c, in Loop3 target)
    {
        target.indices = new Index3[ ] { a, b, c };
        return target;
    }
}