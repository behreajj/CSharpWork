using System;
using System.Collections;
using System.Text;

/// <summary>
/// Organizes indices in a mesh that refer to data at a vertex.
/// </summary>
public readonly struct Index2 : IEquatable<Index2>, IEnumerable
{
    /// <summary>
    /// The coordinate index.
    /// </summary>
    private readonly int _v;

    /// <summary>
    /// The texture coordinate index.
    /// </summary>
    private readonly int _vt;

    /// <summary>
    /// The number of array element indices held by this index.
    /// </summary>
    /// <value>length</value>
    public int Length { get { return 2; } }

    /// <summary>
    /// The coordinate index.
    /// </summary>
    /// <value>coordinate index</value>
    public int v { get { return this._v; } }

    /// <summary>
    /// The texture coordinate index.
    /// </summary>
    /// <value>texture coordinate index</value>
    public int vt { get { return this._vt; } }

    /// <summary>
    /// Retrieves a component by index.
    /// </summary>
    /// <value>the component</value>
    public int this [int i]
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

    /// <summary>
    /// Constructs an index from integers for the coordinate and texture
    /// coordinate.
    /// </summary>
    /// <param name="v">coordinate index</param>
    /// <param name="vt">texture coordinate index</param>
    public Index2 (in int v = 0, in int vt = 0)
    {
        this._v = v < 0 ? 0 : v;
        this._vt = vt < 0 ? 0 : vt;
    }

    public override bool Equals (object value)
    {
        if (Object.ReferenceEquals (this, value)) { return true; }
        if (value is null) { return false; }
        if (value is Index2) { return this.Equals ((Index2) value); }
        return false;
    }

    public override int GetHashCode ( )
    {
        unchecked
        {
            return (Utils.MulBase ^ this._v.GetHashCode ( )) *
                Utils.HashMul ^ this._vt.GetHashCode ( );
        }
    }

    public override string ToString ( )
    {
        return Index2.ToString (this);
    }

    public bool Equals (Index2 i)
    {
        if (this._v != i._v) { return false; }
        if (this._vt != i._vt) { return false; }
        return true;
    }

    public IEnumerator GetEnumerator ( )
    {
        yield return this._v;
        yield return this._vt;
    }

    public int[ ] ToArray ( )
    {
        return new int[ ] { this._v, this._vt };
    }

    public (int v, int vt) ToTuple ( )
    {
        return (v: this._v, vt: this._vt);
    }

    public static Index2[ ] Resize (in Index2[ ] arr, in int sz)
    {
        if (sz < 1) { return new Index2[ ] { }; }
        Index2[ ] result = new Index2[sz];

        if (arr != null)
        {
            int len = arr.Length;
            int end = sz > len ? len : sz;
            System.Array.Copy (arr, result, end);
        }

        return result;
    }

    public static string ToString (in Index2 i, in int padding = 3)
    {
        return Index2.ToString (new StringBuilder (48), i, padding).ToString ( );
    }

    public static StringBuilder ToString (in StringBuilder sb, in Index2 i, in int padding = 3)
    {
        sb.Append ("{ v: ");
        Utils.ToPadded (sb, i._v, padding);
        sb.Append (", vt: ");
        Utils.ToPadded (sb, i._vt, padding);
        sb.Append (" }");
        return sb;
    }

    public static string ToString (in Index2[ ] arr, in int padding = 3)
    {
        return Index2.ToString (new StringBuilder (1024), arr, padding).ToString ( );
    }

    public static StringBuilder ToString (in StringBuilder sb, in Index2[ ] arr, in int padding = 3)
    {
        sb.Append ('[');
        sb.Append (' ');

        if (arr != null)
        {
            int len = arr.Length;
            int last = len - 1;

            for (int i = 0; i < last; ++i)
            {
                Index2.ToString (sb, arr[i], padding);
                sb.Append (',');
                sb.Append (' ');
            }

            Index2.ToString (sb, arr[last], padding);
            sb.Append (' ');
        }

        sb.Append (']');
        return sb;
    }
}