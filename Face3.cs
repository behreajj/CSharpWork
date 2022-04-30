using System;
using System.Text;

/// <summary>
/// Organizes the components of a 3D mesh into a group of edges.
/// This is not used by a mesh internally; it is created upon
/// retrieval from a mesh.
/// </summary>
[Serializable]
public readonly struct Face3 : IComparable<Face3>, IEquatable<Face3>
{
    /// <summary>
    /// The edges array.
    /// </summary>
    private readonly Edge3 [ ] edges;

    /// <summary>
    /// Constructs a face from an array of edges.
    /// </summary>
    /// <param name="edges">edges array</param>
    public Face3 (in Edge3 [ ] edges) { this.edges = edges; }

    /// <summary>
    /// The number of edges in this face.
    /// </summary>
    /// <value>the length</value>
    public int Length { get { return this.edges.Length; } }

    /// <summary>
    /// Gets an edge at i. Wraps around, so
    /// negative indices may be used.
    /// </summary>
    /// <value>the edge</value>
    public Edge3 this [ int i ]
    {
        get
        {
            return this.edges [ Utils.RemFloor (i, this.edges.Length) ];
        }
    }

    /// <summary>
    /// Tests this face for equivalence with an object.
    /// </summary>
    /// <param name="value">the object</param>
    /// <returns>the equivalence</returns>
    public override bool Equals (object value)
    {
        if (Object.ReferenceEquals (this, value)) { return true; }
        if (value is null) { return false; }
        if (value is Vert3) { return this.Equals ((Face3) value); }
        return false;
    }

    /// <summary>
    /// Returns a hash code representing this face.
    /// </summary>
    /// <returns>the hash code</returns>
    public override int GetHashCode ( )
    {
        return this.edges.GetHashCode ( );
    }

    /// <summary>
    /// Returns a string representation of this face.
    /// </summary>
    /// <returns>the string</returns>
    public override string ToString ( )
    {
        return Face3.ToString (this);
    }

    /// <summary>
    /// Compares this face to another in compliance with the IComparable
    /// interface.
    /// </summary>
    /// <param name="f">comparisand</param>
    /// <returns>evaluation</returns>
    public int CompareTo (Face3 f)
    {
        return Face3.Center (this).CompareTo (Face3.Center (f));
    }

    /// <summary>
    /// Tests this face for equivalence with another in compliance with the
    /// IEquatable interface.
    /// </summary>
    /// <param name="f">face</param>
    /// <returns>equivalence</returns>
    public bool Equals (Face3 f)
    {
        return this.edges.Equals (f.edges);
    }

    /// <summary>
    /// Finds the center of a face.
    /// </summary>
    /// <param name="f">face</param>
    /// <returns>center</returns>
    public static Vec3 Center (in Face3 f)
    {
        return Face3.CenterMean (f);
    }

    /// <summary>
    /// Finds the mean center of a face.
    /// Sums the origin coordinate of its edges,
    /// then finds the average.
    /// </summary>
    /// <param name="f">face</param>
    /// <returns>center</returns>
    public static Vec3 CenterMean (in Face3 f)
    {
        int len = f.edges.Length;
        Vec3 sum = Vec3.Zero;
        for (int i = 0; i < len; ++i)
        {
            sum += f.edges [ i ].Origin.Coord;
        }
        return sum / len;
    }

    /// <summary>
    /// Finds a point on a face's perimeter given a factor
    /// in [0.0, 1.0].
    /// </summary>
    /// <param name="f">face</param>
    /// <param name="t">factor</param>
    /// <returns>point</returns>
    public static Vec3 Eval (in Face3 f, in float t)
    {
        float tScaled = f.Length * Utils.RemFloor (t, 1.0f);
        int i = (int) tScaled;
        return Edge3.Eval (f [ i ], tScaled - i);
    }

    /// <summary>
    /// Finds the perimeter of a face. Sums the magnitude
    /// of its edges.
    /// </summary>
    /// <param name="f">face</param>
    /// <returns>perimeter</returns>
    public static float Perimeter (in Face3 f)
    {
        int len = f.Length;
        float sum = 0.0f;
        for (int i = 0; i < len; ++i)
        {
            sum += Edge3.Mag (f [ i ]);
        }
        return sum;
    }

    /// <summary>
    /// Returns a string representation of a face.
    /// </summary>
    /// <param name="f">face</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string</returns>
    public static string ToString (in Face3 f, in int places = 4)
    {
        return Face3.ToString (new StringBuilder (1024), f, places).ToString ( );
    }

    /// <summary>
    /// Appends a representation of an face to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="f">face</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString (in StringBuilder sb, in Face3 f, in int places = 4)
    {
        sb.Append ("{ edges: ");
        Edge3.ToString (sb, f.edges, places);
        sb.Append (' ');
        sb.Append ('}');
        return sb;
    }
}