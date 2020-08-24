using System;
using System.Collections;
using System.Text;

public readonly struct Index2 : IEquatable<Index2>, IEnumerable
{
    private readonly int _v;

    private readonly int _vt;

    public int Length { get { return 2; } }

    public int v { get { return this._v; } }

    public int vt { get { return this._vt; } }

    public int this[int i]
    {
        get
        {
            switch (i)
            {
                case 0:
                case -2:
                    return this._v;
                case 1:
                case -1:
                    return this._vt;
                default:
                    return 0;
            }
        }
    }

    public Index2(in int v = 0, in int vt = 0)
    {
        this._v = v < 0 ? 0 : v;
        this._vt = vt < 0 ? 0 : vt;
    }

    public override bool Equals(object value)
    {
        if (Object.ReferenceEquals(this, value)) return true;
        if (Object.ReferenceEquals(null, value)) return false;
        if (value is Index2) return this.Equals((Index2)value);
        return false;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (Utils.MulBase ^ this._v.GetHashCode()) *
                Utils.HashMul ^ this._vt.GetHashCode();
        }
    }

    public override string ToString()
    {
        return this.ToString(3);
    }

    public bool Equals(Index2 i)
    {
        if (this._v != i._v) return false;
        if (this._vt != i._vt) return false;
        return true;
    }

    public IEnumerator GetEnumerator()
    {
        yield return this._v;
        yield return this._vt;
    }

    public int[] ToArray()
    {
        return new int[] { this._v, this._vt };
    }

    public string ToString(in int padding = 3)
    {
        return new StringBuilder(48)
            .Append("{ v: ")
            .Append(Utils.ToPadded(this._v, padding))
            .Append(", vt: ")
            .Append(Utils.ToPadded(this._vt, padding))
            .Append(" }")
            .ToString();
    }

    public (int v, int vt) ToTuple()
    {
        return (v: this._v, vt: this._vt);
    }

    public static Index2[] Resize(in Index2[] arr, in int sz)
    {
        if (sz < 1) return new Index2[] { };
        Index2[] result = new Index2[sz];

        if (arr != null)
        {
            int len = arr.Length;
            int end = sz > len ? len : sz;
            System.Array.Copy(arr, result, end);
        }

        return result;
    }
}