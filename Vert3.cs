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
public readonly struct Vert3 : IComparable<Vert3>, IEquatable<Vert3>, IEnumerable
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
    /// Retrieves a component by index. Components are ordered as coordinate,
    /// texture coordinate normal.
    /// </summary>
    /// <value>the value</value>
    public float this [int i]
    {
        get
        {
            switch (i)
            {
                case 0:
                case -8:
                    return this.coord.x;
                case 1:
                case -7:
                    return this.coord.y;
                case 2:
                case -6:
                    return this.coord.z;
                case 3:
                case -5:
                    return this.texCoord.x;
                case 4:
                case -4:
                    return this.texCoord.y;
                case 5:
                case -3:
                    return this.normal.x;
                case 6:
                case -2:
                    return this.normal.y;
                case 7:
                case -1:
                    return this.normal.z;
                default:
                    return 0.0f;
            }
        }
    }

    /// <summary>
    /// Retrieves a component by index. Components are ordered as coordinate,
    /// texture coordinate normal.
    /// </summary>
    /// <value>the value</value>
    public float this [int i, int j]
    {
        get
        {
            switch (i)
            {
                case 0:
                case -3:
                    return this.coord[j];
                case 1:
                case -2:
                    return this.texCoord[j];
                case 2:
                case -1:
                    return this.normal[j];
                default:
                    return 0.0f;
            }
        }
    }

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
    /// Tests this vector for equivalence with an object. For approximate
    /// equality  with another vector, use the static approx function instead.
    /// </summary>
    /// <param name="value">the object</param>
    /// <returns>the equivalence</returns>
    public override bool Equals (object value)
    {
        if (Object.ReferenceEquals (this, value)) return true;
        if (Object.ReferenceEquals (null, value)) return false;
        if (value is Vert3) return this.Equals ((Vert3) value);
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
        return this.ToString (4);
    }

    /// <summary>
    ///  Compares this vector to another in compliance with the IComparable
    ///  interface.
    /// </summary>
    /// <param name="v">comparisand</param>
    /// <returns>evaluation</returns>
    public int CompareTo (Vert3 v)
    {
        int nrComp = this.normal.CompareTo (v.normal);
        int tcComp = this.texCoord.CompareTo (v.texCoord);
        int coComp = this.coord.CompareTo (v.coord);

        return nrComp != 0 ? nrComp : tcComp != 0 ? tcComp : coComp;
    }

    /// <summary>
    /// Tests this vertex for equivalence with another in compliance with the
    /// IEquatable interface.
    /// </summary>
    /// <param name="v">vertex</param>
    /// <returns>equivalence</returns>
    public bool Equals (Vert3 v)
    {
        if (this.coord.GetHashCode ( ) != v.coord.GetHashCode ( )) return false;
        if (this.texCoord.GetHashCode ( ) != v.texCoord.GetHashCode ( )) return false;
        if (this.normal.GetHashCode ( ) != v.normal.GetHashCode ( )) return false;
        return true;
    }

    /// <summary>
    /// Returns an enumerator (or iterator) for this vertex, allowing its
    /// components to be accessed in a foreach loop.
    /// </summary>
    /// <returns>the enumerator</returns>
    public IEnumerator GetEnumerator ( )
    {
        yield return this.coord.x;
        yield return this.coord.y;
        yield return this.coord.z;

        yield return this.texCoord.x;
        yield return this.texCoord.y;

        yield return this.normal.x;
        yield return this.normal.y;
        yield return this.normal.z;
    }

    /// <summary>
    /// Returns a float array containing this vertex's components.
    /// </summary>
    /// <returns>the array</returns>
    public float[ ] ToArray ( )
    {
        return this.ToArray (new float[this.Length], 0);
    }

    /// <summary>
    /// Puts this vertex's components into an array at a given index.
    /// </summary>
    /// <param name="arr">array</param>
    /// <param name="i">index</param>
    /// <returns>array</returns>
    public float[ ] ToArray (in float[ ] arr, in int i = 0)
    {
        arr[i] = this.coord.x;
        arr[i + 1] = this.coord.y;
        arr[i + 2] = this.coord.z;

        arr[i + 3] = this.texCoord.x;
        arr[i + 4] = this.texCoord.y;

        arr[i + 5] = this.normal.x;
        arr[i + 6] = this.normal.y;
        arr[i + 7] = this.normal.z;

        return arr;
    }

    /// <summary>
    /// Returns a string representation of this vertex.
    /// </summary>
    /// <param name="places">number of decimal places</param>
    /// <returns>the string</returns>
    public string ToString (in int places = 4)
    {
        return new StringBuilder (512)
            .Append ("{ coord: ")
            .Append (this.coord.ToString (places))
            .Append (", texCoord: ")
            .Append (this.texCoord.ToString (places))
            .Append (", normal: ")
            .Append (this.normal.ToString (places))
            .Append (" }")
            .ToString ( );
    }
}