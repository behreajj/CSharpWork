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

    public static Loop2[ ] Resize (in Loop2[ ] arr, in int sz)
    {
        if (sz < 1) return new Loop2[ ] { };
        Loop2[ ] result = new Loop2[sz];

        if (arr != null)
        {
            int len = arr.Length;
            int end = sz > len ? len : sz;
            System.Array.Copy (arr, result, end);
        }

        return result;
    }

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
        int valIdx = Utils.Mod (index, aLen + 1);
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
}