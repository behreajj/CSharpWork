using System;
using System.Collections;
using System.Text;

/// <summary>
/// Organizes the components of a 3D mesh into a group of coordinate, normal and
/// texture coordinate such that they can be edited together. This is not used
/// by a mesh internally; it is created upon retrieval from a mesh. All of its
/// components should be treated as references to data within the mesh, not as
/// independent values.
/// </summary>
[Serializable]
public readonly struct Vert3 : IComparable<Vert3>, IEquatable<Vert3>
{
    /// <summary>
    /// The coordinate of the vertex in world space.
    /// </summary>
    private readonly Vec3 coord;

    /// <summary>
    /// The direction in which light will bounce from the surface of the mesh at
    /// the vertex.
    /// </summary>
    private readonly Vec3 normal;

    /// <summary>
    /// The texture (UV) coordinate for an image mapped onto the mesh.
    /// </summary>
    private readonly Vec2 texCoord;

    /// <summary>
    /// The coordinate of the vertex in world space.
    /// </summary>
    /// <value>coord</value>
    public Vec3 Coord { get { return this.coord; } }

    /// <summary>
    /// The number of values (dimensions) in this vertex.
    /// </summary>
    /// <value>the length</value>
    public int Length { get { return 8; } }

    /// <summary>
    /// The direction in which light will bounce from the surface of the mesh at
    /// the vertex.
    /// </summary>
    /// <value>normal</value>
    public Vec3 Normal { get { return this.normal; } }

    /// <summary>
    /// The texture (UV) coordinate for an image mapped onto the mesh.
    /// </summary>
    /// <value>texture coordinate</value>
    public Vec2 TexCoord { get { return this.texCoord; } }

    /// <summary>
    /// Constructs a vertex from a coordinate, texture coordinate and normal.
    /// </summary>
    /// <param name="coord">coordinate</param>
    /// <param name="texCoord">texture coordinate</param>
    /// <param name="normal">normal</param>
    public Vert3 (in Vec3 coord, in Vec2 texCoord, in Vec3 normal)
    {
        this.coord = coord;
        this.texCoord = texCoord;
        this.normal = normal;
    }

    /// <summary>
    /// Tests this vertex for equivalence with an object.
    /// </summary>
    /// <param name="value">the object</param>
    /// <returns>the equivalence</returns>
    public override bool Equals (object value)
    {
        if (Object.ReferenceEquals (this, value)) { return true; }
        if (value is null) { return false; }
        if (value is Vert3) { return this.Equals ((Vert3) value); }
        return false;
    }

    /// <summary>
    /// Returns a hash code representing this vertex.
    /// </summary>
    /// <returns>the hash code</returns>
    public override int GetHashCode ( )
    {
        unchecked
        {
            return ((Utils.MulBase ^ this.coord.GetHashCode ( )) *
                    Utils.HashMul ^ this.texCoord.GetHashCode ( )) *
                Utils.HashMul ^ this.normal.GetHashCode ( );
        }
    }

    /// <summary>
    /// Returns a string representation of this vertex.
    /// </summary>
    /// <returns>the string</returns>
    public override string ToString ( )
    {
        return Vert3.ToString (this);
    }

    /// <summary>
    ///  Compares this vector to another in compliance with the IComparable
    ///  interface.
    /// </summary>
    /// <param name="v">comparisand</param>
    /// <returns>evaluation</returns>
    public int CompareTo (Vert3 v)
    {
        return this.coord.CompareTo (v.coord);
    }

    /// <summary>
    /// Tests this vertex for equivalence with another in compliance with the
    /// IEquatable interface.
    /// </summary>
    /// <param name="v">vertex</param>
    /// <returns>equivalence</returns>
    public bool Equals (Vert3 v)
    {
        return this.coord.Equals(v.coord);
    }

    /// <summary>
    /// Returns a string representation of a vertex.
    /// </summary>
    /// <param name="v">vertex</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string</returns>
    public static string ToString (in Vert3 v, in int places = 4)
    {
        return Vert3.ToString (new StringBuilder (256), v, places).ToString ( );
    }

    /// <summary>
    /// Appends a representation of an vertex to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="v">vertex</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString (in StringBuilder sb, in Vert3 v, in int places = 4)
    {
        sb.Append ("{ coord: ");
        Vec3.ToString (sb, v.coord, places);
        sb.Append (", texCoord: ");
        Vec2.ToString (sb, v.texCoord, places);
        sb.Append (", normal: ");
        Vec3.ToString (sb, v.normal, places);
        sb.Append (" }");
        return sb;
    }
}