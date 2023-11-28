using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;

/// <summary>
/// Organizes indices in a mesh that refer to data at a vertex.
/// </summary>
[StructLayout(LayoutKind.Explicit)]
public readonly struct Index3 : IEquatable<Index3>, IEnumerable
{
    /// <summary>
    /// The coordinate index.
    /// </summary>
    [FieldOffset(0)] private readonly int v;

    /// <summary>
    /// The texture coordinate index.
    /// </summary>
    [FieldOffset(4)] private readonly int vt;

    /// <summary>
    /// The normal index.
    /// </summary>
    [FieldOffset(8)] private readonly int vn;

    /// <summary>
    /// The number of array element indices held by this index.
    /// </summary>
    /// <value>length</value>
    public int Length { get { return 3; } }

    /// <summary>
    /// The coordinate index.
    /// </summary>
    /// <value>coordinate index</value>
    public int V { get { return this.v; } }

    /// <summary>
    /// The texture coordinate index.
    /// </summary>
    /// <value>texture coordinate index</value>
    public int VT { get { return this.vt; } }

    /// <summary>
    /// The normal index.
    /// </summary>
    /// <value>normal index</value>
    public int VN { get { return this.vn; } }

    /// <summary>
    /// Retrieves a component by index.
    /// </summary>
    /// <value>the component</value>
    public int this[int i]
    {
        get
        {
            return i switch
            {
                0 or -3 => this.v,
                1 or -2 => this.vt,
                2 or -1 => this.vn,
                _ => 0,
            };
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
        this.v = v < 0 ? 0 : v;
        this.vt = vt < 0 ? 0 : vt;
        this.vn = vn < 0 ? 0 : vn;
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
        if (value is Index3 index) { return this.Equals(index); }
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
            return ((Utils.MulBase ^ this.v.GetHashCode()) *
                    Utils.HashMul ^ this.vt.GetHashCode()) *
                Utils.HashMul ^ this.vn.GetHashCode();
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
        if (this.v != i.v) { return false; }
        if (this.vt != i.vt) { return false; }
        if (this.vn != i.vn) { return false; }
        return true;
    }

    /// <summary>
    /// Returns an enumerator (or iterator) for this index, allowing its
    /// components to be accessed in a foreach loop.
    /// </summary>
    /// <returns>enumerator</returns>
    public IEnumerator GetEnumerator()
    {
        yield return this.v;
        yield return this.vt;
        yield return this.vn;

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
        return new Index3(i.V, i.VT, 0);
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
    public static string ToString(in Index3 i, in int padding = 3)
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
    public static StringBuilder ToString(
        in StringBuilder sb,
        in Index3 i,
        in int padding = 3)
    {
        sb.Append("{\"v\":");
        Utils.ToPadded(sb, i.v, padding);
        sb.Append(",\"vt\":");
        Utils.ToPadded(sb, i.vt, padding);
        sb.Append(",\"vn\":");
        Utils.ToPadded(sb, i.vn, padding);
        sb.Append('}');
        return sb;
    }

    /// <summary>
    /// Returns a string representation of an array of indices.
    /// </summary>
    /// <param name="arr">array</param>
    /// <param name="padding">digits to pad</param>
    /// <returns>string</returns>
    public static string ToString(
        in Index3[] arr,
        in int padding = 3)
    {
        return Index3.ToString(new StringBuilder(1024), arr, padding).ToString();
    }

    /// <summary>
    /// Appends a representation of an array of indices to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="arr">array</param>
    /// <param name="padding">digits to pad</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString(
        in StringBuilder sb,
        in Index3[] arr,
        in int padding = 3)
    {
        sb.Append('[');

        if (arr != null)
        {
            int len = arr.Length;
            int last = len - 1;

            for (int i = 0; i < last; ++i)
            {
                Index3.ToString(sb, arr[i], padding);
                sb.Append(',');
            }

            Index3.ToString(sb, arr[last], padding);
        }

        sb.Append(']');
        return sb;
    }
}