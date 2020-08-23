using System;
using System.Text;

public class Loop3
{
    protected Index3[] indices;

    public Index3[] Indices
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

    public Index3 this[int i]
    {
        get
        {
            return this.indices[i];
        }

        set
        {
            this.indices[i] = value;
        }
    }

    public int this[int i, int j]
    {
        get
        {
            return this.indices[i][j];
        }
    }

    public Loop3() { }

    public Loop3(in Index3[] indices)
    {
        this.indices = indices;
    }

    public Loop3(params Index3[] indices)
    {
        this.indices = indices;
    }

    public override string ToString()
    {
        return this.ToString(1);
    }

    public string ToString(in int padding = 1)
    {
        int len = this.indices.Length;
        int last = len - 1;
        StringBuilder sb = new StringBuilder(96 * len);
        sb.Append("{ indices: [ ");
        for (int i = 0; i < len; ++i)
        {
            sb.Append(this.indices[i].ToString(padding));
            if (i < last)
            {
                sb.Append(", ");
            }
        }
        sb.Append(" ] }");
        return sb.ToString();
    }

    public static Loop3[] Resize(in Loop3[] arr, in int sz)
    {
        if (sz < 1) return new Loop3[] { };
        Loop3[] result = new Loop3[sz];

        if (arr != null)
        {
            int len = arr.Length;
            int end = sz > len ? len : sz;
            System.Array.Copy(arr, result, end);
        }

        return result;
    }
}