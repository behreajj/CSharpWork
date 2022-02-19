using System;
using System.Collections;
using System.Text;

/// <summary>
/// Organizes components of a 3D mesh into an edge with an origin and
/// destination. This is not used by a mesh internally; it is created upon
/// retrieval from a mesh.
/// </summary>
[Serializable]
public readonly struct Edge3 : IEquatable<Edge3>
{
    /// <summary>
    /// The destination vertex.
    /// </summary>
    private readonly Vert3 dest;

    /// <summary>
    /// The origin vertex.
    /// </summary>
    private readonly Vert3 origin;

    /// <summary>
    /// The destination vertex.
    /// </summary>
    /// <value>destination</value>
    public Vert3 Dest { get { return this.dest; } }

    /// <summary>
    /// The origin vertex.
    /// </summary>
    /// <value>origin</value>
    public Vert3 Origin { get { return this.origin; } }

    /// <summary>
    /// Constructs an edge from two vertices, an origin and destination.
    /// </summary>
    /// <param name="origin">origin</param>
    /// <param name="dest">destination</param>
    public Edge3 (in Vert3 origin, in Vert3 dest)
    {
        this.origin = origin;
        this.dest = dest;
    }

    /// <summary>
    /// Tests this edge for equivalence with an object.
    /// </summary>
    /// <param name="value">the object</param>
    /// <returns>the equivalence</returns>
    public override bool Equals (object value)
    {
        if (Object.ReferenceEquals (this, value)) { return true; }
        if (value is null) { return false; }
        if (value is Edge3) { return this.Equals ((Edge3) value); }
        return false;
    }

    /// <summary>
    /// Returns a hash code representing this edge.
    /// </summary>
    /// <returns>the hash code</returns>
    public override int GetHashCode ( )
    {
        unchecked
        {
            return (Utils.MulBase ^ this.origin.GetHashCode ( )) *
                Utils.HashMul ^ this.dest.GetHashCode ( );
        }
    }

    /// <summary>
    /// Returns a string representation of this vertex.
    /// </summary>
    /// <returns>the string</returns>
    public override string ToString ( )
    {
        return Edge3.ToString (this);
    }

    /// <summary>
    /// Tests this edge for equivalence with another in compliance with the
    /// IEquatable interface.
    /// </summary>
    /// <param name="e">edge</param>
    /// <returns>equivalence</returns>
    public bool Equals (Edge3 e)
    {
        if (this.origin.GetHashCode ( ) != e.origin.GetHashCode ( )) { return false; }
        if (this.dest.GetHashCode ( ) != e.dest.GetHashCode ( )) { return false; }
        return true;
    }

    public static float Azimuth (in Edge3 e)
    {
        return Vec3.AzimuthSigned (e.dest.Coord - e.origin.Coord);
    }

    public static float Inclination (in Edge3 e)
    {
        return Vec3.InclinationSigned (e.dest.Coord - e.origin.Coord);
    }

    public static float Mag (in Edge3 e)
    {
        return Vec3.DistEuclidean (e.origin.Coord, e.dest.Coord);
    }

    public static float MagSq (in Edge3 e)
    {
        return Vec3.DistSq (e.origin.Coord, e.dest.Coord);
    }

    public static string ToString (in Edge3 e, in int places = 4)
    {
        return Edge3.ToString (new StringBuilder (256), e, places).ToString ( );
    }

    public static StringBuilder ToString (in StringBuilder sb, in Edge3 e, in int places = 4)
    {
        sb.Append ("{ origin: ");
        Vert3.ToString (sb, e.origin, places);
        sb.Append (", dest: ");
        Vert3.ToString (sb, e.dest, places);
        sb.Append (" }");
        return sb;
    }
}