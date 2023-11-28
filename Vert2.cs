using System;
using System.Runtime.InteropServices;
using System.Text;

/// <summary>
/// Organizes the components of a 2D mesh into a group of coordinate and
/// texture coordinate such that they can be edited together. This is not used
/// by a mesh internally, it is created upon retrieval from a mesh.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Explicit, Pack = 16)]
public readonly struct Vert2 : IComparable<Vert2>, IEquatable<Vert2>
{
    /// <summary>
    /// The coordinate of the vertex in world space.
    /// </summary>
    [FieldOffset(0)] private readonly Vec2 coord;

    /// <summary>
    /// The texture (UV) coordinate for an image mapped onto the mesh.
    /// </summary>
    [FieldOffset(8)] private readonly Vec2 texCoord;

    /// <summary>
    /// The coordinate of the vertex in world space.
    /// </summary>
    /// <value>coord</value>
    public Vec2 Coord { get { return this.coord; } }

    /// <summary>
    /// The texture (UV) coordinate for an image mapped onto the mesh.
    /// </summary>
    /// <value>texture coordinate</value>
    public Vec2 TexCoord { get { return this.texCoord; } }

    /// <summary>
    /// Constructs a vertex from a coordinate and texture coordinate.
    /// </summary>
    /// <param name="coord">coordinate</param>
    /// <param name="texCoord">texture coordinate</param>
    public Vert2(in Vec2 coord, in Vec2 texCoord)
    {
        this.coord = coord;
        this.texCoord = texCoord;
    }

    /// <summary>
    /// Tests this vertex for equivalence with an object.
    /// </summary>
    /// <param name="value">the object</param>
    /// <returns>equivalence</returns>
    public override bool Equals(object value)
    {
        if (Object.ReferenceEquals(this, value)) { return true; }
        if (value is null) { return false; }
        if (value is Vert2 vert) { return this.Equals(vert); }
        return false;
    }

    /// <summary>
    /// Returns a hash code representing this vertex.
    /// </summary>
    /// <returns>hash code</returns>
    public override int GetHashCode()
    {
        unchecked
        {
            return (Utils.MulBase ^ this.coord.GetHashCode()) *
                Utils.HashMul ^ this.texCoord.GetHashCode();
        }
    }

    /// <summary>
    /// Returns a string representation of this vertex.
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()
    {
        return Vert2.ToString(this);
    }

    /// <summary>
    /// Compares this vertex to another in compliance with the IComparable
    /// interface.
    /// </summary>
    /// <param name="v">comparisand</param>
    /// <returns>evaluation</returns>
    public int CompareTo(Vert2 v)
    {
        return this.coord.CompareTo(v.coord);
    }

    /// <summary>
    /// Tests this vertex for equivalence with another in compliance with the
    /// IEquatable interface.
    /// </summary>
    /// <param name="v">vertex</param>
    /// <returns>equivalence</returns>
    public bool Equals(Vert2 v)
    {
        return this.coord.Equals(v.coord);
    }

    /// <summary>
    /// Returns a string representation of a vertex.
    /// </summary>
    /// <param name="v">vertex</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string</returns>
    public static string ToString(in Vert2 v, in int places = 4)
    {
        return Vert2.ToString(new StringBuilder(256), v, places).ToString();
    }

    /// <summary>
    /// Appends a representation of an vertex to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="v">vertex</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString(
        in StringBuilder sb,
        in Vert2 v,
        in int places = 4)
    {
        sb.Append("{\"coord\":");
        Vec2.ToString(sb, v.coord, places);
        sb.Append(",\"texCoord\":");
        Vec2.ToString(sb, v.texCoord, places);
        sb.Append("}");
        return sb;
    }
}