using System;
using System.Collections;
using System.Text;

/// <summary>
/// Organizes indices in a mesh that refer to data at a vertex.
/// </summary>
public readonly struct Index3 : IEquatable<Index3>, IEnumerable
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
    /// The normal index.
    /// </summary>
    private readonly int _vn;

    /// <summary>
    /// The number of array element indices held by this index.
    /// </summary>
    /// <value>length</value>
    public int Length { get { return 3; } }

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
    /// The normal index.
    /// </summary>
    /// <value>normal index</value>
    public int vn { get { return this._vn; } }

    /// <summary>
    /// Retrieves a component by index.
    /// </summary>
    /// <value>the component</value>
    public int this[int i]
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

    /// <summary>
    /// Constructs an index from integers for the coordinate, texture coordinate
    /// and normal element.
    /// 
    /// Any argument less than zero is clamped to zero.
    /// </summary>
    /// <param name="v">coordinate index</param>
    /// <param name="vt">texture coordinate index</param>
    /// <param name="vn">normal index</param>
    public Index3(in int v, in int vt, in int vn)
    {
        this._v = v < 0 ? 0 : v;
        this._vt = vt < 0 ? 0 : vt;
        this._vn = vn < 0 ? 0 : vn;
    }

    /// <summary>
    /// Tests this index for equivalence with an object.
    /// </summary>
    /// <param name="value">the object</param>
    /// <returns>equivalence</returns>
    public override bool Equals(object value)
    {
        if (Object.ReferenceEquals(this, value)) { return true; }
        if (value is null) { return false; }
        if (value is Index3) { return this.Equals((Index3)value); }
        return false;
    }

    /// <summary>
    /// Returns a hash code representing this index.
    /// </summary>
    /// <returns>hash code</returns>
    public override int GetHashCode()
    {
        unchecked
        {
            return ((Utils.MulBase ^ this._v.GetHashCode()) *
                    Utils.HashMul ^ this._vt.GetHashCode()) *
                Utils.HashMul ^ this._vn.GetHashCode();
        }
    }

    /// <summary>
    /// Returns a string representation of this index.
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()
    {
        return Index3.ToString(this);
    }

    /// <summary>
    /// Tests this index for equivalence with another in compliance with the
    /// IEquatable interface.
    /// </summary>
    /// <param name="i">index</param>
    /// <returns>equivalence</returns>
    public bool Equals(Index3 i)
    {
        if (this._v != i._v) { return false; }
        if (this._vt != i._vt) { return false; }
        if (this._vn != i._vn) { return false; }
        return true;
    }

    /// <summary>
    /// Returns an enumerator (or iterator) for this index, allowing its
    /// components to be accessed in a foreach loop.
    /// </summary>
    /// <returns>enumerator</returns>
    public IEnumerator GetEnumerator()
    {
        yield return this._v;
        yield return this._vt;
        yield return this._vn;

    }

    /// <summary>
    /// Returns an integer array of length 3 containing this index's components.
    /// </summary>
    /// <returns>the array</returns>
    public int[] ToArray()
    {
        return this.ToArray(new int[this.Length], 0);
    }

    /// <summary>
    /// Puts this index's components into an array at a given index.
    /// </summary>
    /// <param name="arr">array</param>
    /// <param name="i">index</param>
    /// <returns>array</returns>
    public int[] ToArray(in int[] arr, in int i)
    {
        arr[i] = this._v;
        arr[i + 1] = this._vt;
        arr[i + 2] = this._vn;
        return arr;
    }

    /// <summary>
    /// Returns a named value tuple containing this index's components.
    /// </summary>
    /// <returns>tuple</returns>
    public (int v, int vt, int vn) ToTuple()
    {
        return (v: this._v, vt: this._vt, vn: this._vn);
    }

    /// <summary>
    /// Sets all elements of an index to the input.
    /// </summary>
    /// <param name="i">simple index</param>
    /// <returns>the index</returns>
    public static implicit operator Index3(in int i)
    {
        return new Index3(i, i, i);
    }

    /// <summary>
    /// Promotes a 2D index to a 3D index. Sets the normal
    /// index to zero.
    /// </summary>
    /// <param name="i">2D index</param>
    /// <returns>the promotion</returns>
    public static explicit operator Index3(in Index2 i)
    {
        return new Index3(i.v, i.vt, 0);
    }

    /// <summary>
    /// Resizes an array of indices to a requested length.
    /// </summary>
    /// <param name="arr">array</param>
    /// <param name="sz">new size</param>
    /// <returns>resized array</returns>
    public static Index3[] Resize(in Index3[] arr, in int sz)
    {
        if (sz < 1) { return new Index3[] { }; }
        Index3[] result = new Index3[sz];

        if (arr != null)
        {
            int len = arr.Length;
            int end = sz > len ? len : sz;
            System.Array.Copy(arr, result, end);
        }

        return result;
    }

    /// <summary>
    /// Returns a string representation of an index.
    /// </summary>
    /// <param name="i">index</param>
    /// <param name="padding">digits to pad</param>
    /// <returns>string</returns>
    public static string ToString( //
        in Index3 i, //
        in int padding = 3)
    {
        return Index3.ToString(new StringBuilder(96), i, padding).ToString();
    }

    /// <summary>
    /// Appends a representation of an index to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="i">index</param>
    /// <param name="padding">digits to pad</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString( //
        in StringBuilder sb, //
        in Index3 i, //
        in int padding = 3)
    {
        sb.Append("{ v: ");
        Utils.ToPadded(sb, i._v, padding);
        sb.Append(", vt: ");
        Utils.ToPadded(sb, i._vt, padding);
        sb.Append(", vn: ");
        Utils.ToPadded(sb, i._vn, padding);
        sb.Append(" }");
        return sb;
    }

    /// <summary>
    /// Returns a string representation of an array of indices.
    /// </summary>
    /// <param name="arr">array</param>
    /// <param name="padding">digits to pad</param>
    /// <returns>string</returns>
    public static string ToString( //
        in Index3[] arr, //
        in int padding = 3)
    {
        return Index3.ToString(new StringBuilder(1024), arr, padding).ToString();
    }

    /// <summary>
    /// Appends a representation of an array of indices to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="i">index</param>
    /// <param name="padding">digits to pad</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString( //
        in StringBuilder sb, //
        in Index3[] arr, //
        in int padding = 3)
    {
        sb.Append('[');
        sb.Append(' ');

        if (arr != null)
        {
            int len = arr.Length;
            int last = len - 1;

            for (int i = 0; i < last; ++i)
            {
                Index3.ToString(sb, arr[i], padding);
                sb.Append(',');
                sb.Append(' ');
            }

            Index3.ToString(sb, arr[last], padding);
            sb.Append(' ');
        }

        sb.Append(']');
        return sb;
    }
}