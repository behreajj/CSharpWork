using System;
using System.Collections;
using System.Text;

/// <summary>
/// Organizes the components of a 2D mesh into a group of coordinate and
/// texture coordinate such that they can be edited together. This is not used
/// by a mesh internally; it is created upon retrieval from a mesh. All of its
/// components should be treated as references to data within the mesh, not as
/// independent values.
/// </summary>
[Serializable]
public readonly struct Vert2 : IComparable<Vert2>, IEquatable<Vert2>, IEnumerable
{
    /// <summary>
    /// The coordinate of the vertex in world space.
    /// </summary>
    private readonly Vec2 coord;

    /// <summary>
    /// The texture (UV) coordinate for an image mapped onto the mesh.
    /// </summary>
    private readonly Vec2 texCoord;

    /// <summary>
    /// The coordinate of the vertex in world space.
    /// </summary>
    /// <value>coord</value>
    public Vec2 Coord { get { return this.coord; } }

    /// <summary>
    /// The number of values (dimensions) in this vertex.
    /// </summary>
    /// <value>the length</value>
    public int Length { get { return 4; } }

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
                case -4:
                    return this.coord.x;
                case 1:
                case -3:
                    return this.coord.y;
                case 2:
                case -2:
                    return this.texCoord.x;
                case 3:
                case -1:
                    return this.texCoord.y;
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
                case -2:
                    return this.coord[j];
                case 1:
                case -1:
                    return this.texCoord[j];
                default:
                    return 0.0f;
            }
        }
    }

    /// <summary>
    /// Constructs a vertex from a coordinate and texture coordinate.
    /// </summary>
    /// <param name="coord">coordinate</param>
    /// <param name="texCoord">texture coordinate</param>
    public Vert2 (in Vec2 coord, in Vec2 texCoord)
    {
        this.coord = coord;
        this.texCoord = texCoord;
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
        if (value is Vert2) return this.Equals ((Vert2) value);
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
            return (Utils.MulBase ^ this.coord.GetHashCode ( )) *
                Utils.HashMul ^ this.texCoord.GetHashCode ( );
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
    public int CompareTo (Vert2 v)
    {
        int tcComp = this.texCoord.CompareTo (v.texCoord);
        int coComp = this.coord.CompareTo (v.coord);

        return tcComp != 0 ? tcComp : coComp;
    }

    /// <summary>
    /// Tests this vertex for equivalence with another in compliance with the
    /// IEquatable interface.
    /// </summary>
    /// <param name="v">vertex</param>
    /// <returns>equivalence</returns>
    public bool Equals (Vert2 v)
    {
        if (this.coord.GetHashCode ( ) != v.coord.GetHashCode ( )) return false;
        if (this.texCoord.GetHashCode ( ) != v.texCoord.GetHashCode ( )) return false;
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

        yield return this.texCoord.x;
        yield return this.texCoord.y;
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

        arr[i + 2] = this.texCoord.x;
        arr[i + 3] = this.texCoord.y;

        return arr;
    }

    /// <summary>
    /// Returns a string representation of this vertex.
    /// </summary>
    /// <param name="places">number of decimal places</param>
    /// <returns>the string</returns>
    public string ToString (in int places = 4)
    {
        return new StringBuilder (256)
            .Append ("{ coord: ")
            .Append (this.coord.ToString (places))
            .Append (", texCoord: ")
            .Append (this.texCoord.ToString (places))
            .Append (" }")
            .ToString ( );
    }
}