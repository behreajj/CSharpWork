using System;
using System.Text;

/// <summary>
/// Organizes components of a 3D mesh into an edge with an origin and
/// destination. This is not used by a mesh internally; it is created upon
/// retrieval from a mesh.
/// </summary>
[Serializable]
public readonly struct Edge3 : IComparable<Edge3>, IEquatable<Edge3>
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
    /// Compares this edge to another in compliance with the IComparable interface.
    /// Compares edges according to their origin, then destination.
    /// </summary>
    /// <param name="e">the comparisand</param>
    /// <returns>the evaluation</returns>
    public int CompareTo (Edge3 e)
    {
        int a = this.origin.CompareTo (e.origin);
        int b = this.dest.CompareTo (e.dest);
        return a != 0 ? a : b;
    }

    /// <summary>
    /// Tests this edge for equivalence with another in compliance with the
    /// IEquatable interface.
    /// </summary>
    /// <param name="e">edge</param>
    /// <returns>equivalence</returns>
    public bool Equals (Edge3 e)
    {
        return this.origin.Equals (e.origin) &&
            this.dest.Equals (e.dest);
    }

    /// <summary>
    /// Evaluates whether two edges are neighbors, i.e., both
    /// share the same vertex coordinate but wind in different
    /// directions.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <param name="tolerance">tolerance</param>
    /// <returns>the evaluation</returns>
    public static bool AreNeighbors ( //
        in Edge3 a, //
        in Edge3 b, //
        in float tolerance = Utils.Epsilon)
    {
        return Vec3.Approx (a.origin.Coord, b.dest.Coord, tolerance) &&
            Vec3.Approx (a.dest.Coord, b.origin.Coord, tolerance);
    }

    /// <summary>
    /// Finds the azimuth of an edge based on its origin
    /// and destination coordinates.
    /// </summary>
    /// <param name="e">edge</param>
    /// <returns>inclination</returns>
    public static float Azimuth (in Edge3 e)
    {
        return Vec3.AzimuthSigned (e.dest.Coord - e.origin.Coord);
    }

    /// <summary>
    /// Finds the inclination of an edge based on its origin
    /// and destination coordinates.
    /// </summary>
    /// <param name="e">edge</param>
    /// <returns>inclination</returns>
    public static float Inclination (in Edge3 e)
    {
        return Vec3.InclinationSigned (e.dest.Coord - e.origin.Coord);
    }

    /// <summary>
    /// Finds the magnitude of an edge based on its origin
    /// and destination coordinates.
    /// </summary>
    /// <param name="e">edge</param>
    /// <returns>magnitude</returns>
    public static float Mag (in Edge3 e)
    {
        return Vec3.DistEuclidean (e.origin.Coord, e.dest.Coord);
    }

    /// <summary>
    /// Finds the square magnitude of an edge based on its origin
    /// and destination coordinates.
    /// </summary>
    /// <param name="e">edge</param>
    /// <returns>magnitude squared</returns>
    public static float MagSq (in Edge3 e)
    {
        return Vec3.DistSq (e.origin.Coord, e.dest.Coord);
    }

    ///<summary>
    ///Projects a vector onto an edge. Returns a point.
    ///The scalar projection is clamped to [0.0, 1.0], so
    ///the extrema are limited to the edge's origin and
    ///destination coordinate. 
    ///</summary>
    ///<param name="a">vector</param>
    ///<param name="b">edge</param>
    ///<returns>point</returns>
    public static Vec3 Project (in Vec3 a, in Edge3 b)
    {
        Vec3 orig = b.origin.Coord;
        Vec3 dest = b.dest.Coord;
        return Vec3.Mix (orig, dest, Utils.Clamp (
            Vec3.ProjectScalar (
                a, dest - orig), 0.0f, 1.0f));
    }

    /// <summary>
    /// Returns a string representation of an edge.
    /// </summary>
    /// <param name="e">edge</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string</returns>
    public static string ToString (in Edge3 e, in int places = 4)
    {
        return Edge3.ToString (new StringBuilder (256), e, places).ToString ( );
    }

    /// <summary>
    /// Appends a representation of an edge to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="e">edge</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string builder</returns>
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