using System;
using System.Collections;
using System.Text;

public readonly struct Index3 : IEquatable<Index3>, IEnumerable
{
    private readonly int _v;

    private readonly int _vt;

    private readonly int _vn;

    public int Length { get { return 3; } }

    public int v { get { return this._v; } }

    public int vt { get { return this._vt; } }

    public int vn { get { return this._vn; } }

    public int this [int i]
    {
        get
        {
            switch (i)
            {
                case 0:
                case -3:
                    return this._v;
                case 1:
                case -2:
                    return this._vt;
                case 2:
                case -1:
                    return this._vn;
                default:
                    return 0;
            }
        }
    }

    public Index3 (int v = 0, int vt = 0, int vn = 0)
    {
        this._v = v;
        this._vt = vt;
        this._vn = vn;
    }

    public override bool Equals (object value)
    {
        if (Object.ReferenceEquals (this, value)) return true;
        if (Object.ReferenceEquals (null, value)) return false;
        if (value is Index3) return this.Equals ((Index3) value);
        return false;
    }

    public override int GetHashCode ( )
    {
        unchecked
        {
            return ((Utils.MulBase ^ this._v.GetHashCode ( )) *
                    Utils.HashMul ^ this._vt.GetHashCode ( )) *
                Utils.HashMul ^ this._vn.GetHashCode ( );
        }
    }

    public override string ToString ( )
    {
        return this.ToString (3);
    }

    public bool Equals (Index3 i)
    {
        if (this._v != i._v) return false;
        if (this._vt != i._vt) return false;
        if (this._vn != i._vn) return false;
        return true;
    }

    public IEnumerator GetEnumerator ( )
    {
        yield return this._v;
        yield return this._vt;
        yield return this._vn;
    }

    public int[ ] ToArray ( )
    {
        return new int[ ] { this._v, this._vt, this._vn };
    }

    public string ToString (in int padding = 3)
    {
        return new StringBuilder (96)
            .Append ("{ v: ")
            .Append (Utils.ToPadded (this._v, padding))
            .Append (", vt: ")
            .Append (Utils.ToPadded (this._vt, padding))
            .Append (", vn: ")
            .Append (Utils.ToPadded (this._vn, padding))
            .Append (" }")
            .ToString ( );
    }

    public (int v, int vt, int vn) ToTuple ( )
    {
        return (v: this._v, vt: this._vt, vn: this._vn);
    }

    public static implicit operator Index3 (in Index2 i)
    {
        return new Index3 (i.v, i.vt, 0);
    }

    public static Index3[ ] Resize (in Index3[ ] arr, in int sz)
    {
        if (sz < 1) return new Index3[ ] { };
        Index3[ ] result = new Index3[sz];

        if (arr != null)
        {
            int len = arr.Length;
            int end = sz > len ? len : sz;
            System.Array.Copy (arr, result, end);
        }

        return result;
    }
}