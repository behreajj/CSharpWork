using System;
using System.Text;

/// <summary>
/// An axis aligned bounding box (AABB) for a 2D area, represented with a
/// minimum and maximum coordinate.
/// </summary>
public readonly struct Bounds2 : IComparable<Bounds2>, IEquatable<Bounds2>
{
    /// <summary>
    /// The maximum corner.
    /// </summary>
    private readonly Vec2 max;

    /// <summary>
    /// The minimum corner.
    /// </summary>
    private readonly Vec2 min;

    /// <summary>
    /// Gets the maximum corner.
    /// </summary>
    /// <value>maximum</value>
    public Vec2 Max { get { return this.max; } }

    /// <summary>
    /// Gets the minimum corner.
    /// </summary>
    /// <value>minimum</value>
    public Vec2 Min { get { return this.min; } }

    /// <summary>
    /// Creates a bounds from a minimum and maximum.
    /// </summary>
    /// <param name="min">minimum</param>
    /// <param name="max">maximum</param>
    public Bounds2 (in float min = -0.5f, in float max = 0.5f)
    {
        this.min = new Vec2 (min, min);
        this.max = new Vec2 (max, max);
    }

    /// <summary>
    /// Creats a bounds from a nonuniform
    /// minimum and maximum expressed in floats.
    /// </summary>
    /// <param name="xMin">minimum x</param>
    /// <param name="yMin">minimum y</param>
    /// <param name="xMax">maximum x</param>
    /// <param name="yMax">maximum y</param>
    public Bounds2 ( //
        in float xMin, //
        in float yMin, //
        in float xMax, //
        in float yMax)
    {
        this.min = new Vec2 (xMin, yMin);
        this.max = new Vec2 (xMax, yMax);
    }

    /// <summary>
    /// Creats a bounds from a nonuniform
    /// minimum and maximum.
    /// </summary>
    /// <param name="min">minimum</param>
    /// <param name="max">maximum</param>
    public Bounds2 (in Vec2 min, in Vec2 max)
    {
        this.min = min;
        this.max = max;
    }

    /// <summary>
    /// Returns the bounds's hash code based on those of its
    /// minimum and maximum.
    /// </summary>
    /// <returns>hash code</returns>
    public override int GetHashCode ( )
    {
        unchecked
        {
            int hash = Utils.HashBase;
            hash = hash * Utils.HashMul ^ this.min.GetHashCode ( );
            hash = hash * Utils.HashMul ^ this.max.GetHashCode ( );
            return hash;
        }
    }

    /// <summary>
    /// Tests this bounds for equivalence with an object.
    /// </summary>
    /// <param name="value">object</param>
    /// <returns>equivalence</returns>
    public override bool Equals (object value)
    {
        if (Object.ReferenceEquals (this, value))
        {
            return true;
        }
        if (value is null)
        {
            return false;
        }
        if (value is Bounds2)
        {
            return this.Equals ((Bounds2) value);
        }
        return false;
    }

    /// <summary>
    /// Returns a string representation of this bounds.
    /// </summary>
    /// <returns>the string</returns>
    public override string ToString ( )
    {
        return Bounds2.ToString (this);
    }

    /// <summary>
    /// Compares this bounds to another in compliance with the IComparable
    /// interface.
    /// </summary>
    /// <param name="b">comparisand</param>
    /// <returns>evaluation</returns>
    public int CompareTo (Bounds2 b)
    {
        return Bounds2.Center (this).CompareTo (Bounds2.Center (b));
    }

    /// <summary>
    /// Tests this bounds for equivalence with another in compliance with the
    /// IEquatable interface.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>equivalence</returns>
    public bool Equals (Bounds2 b)
    {
        return this.min.GetHashCode ( ) == b.min.GetHashCode ( ) &&
            this.max.GetHashCode ( ) == b.max.GetHashCode ( );
    }

    /// <summary>
    /// A bounds evaluates to true when its minimum and maximum
    /// corners are approximately unequal in all dimensions.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>evaluation</returns>
    public static bool operator true (in Bounds2 b)
    {
        return Bounds2.All (b);
    }

    /// <summary>
    /// A bounds evaluates to false when its minimum and maximum
    /// corners are approximately equal in all dimensions.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>evaluation</returns>
    public static bool operator false (in Bounds2 b)
    {
        return Bounds2.None (b);
    }

    /// <summary>
    /// Evaluates whether the minimum and maximum corners
    /// of a bounds are approximately unequal in all
    /// dimensions.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>evaluation</returns>
    public static bool All (in Bounds2 b)
    {
        Vec2 mn = b.min;
        Vec2 mx = b.max;
        return !Utils.Approx (mn.x, mx.x) &&
            !Utils.Approx (mn.y, mx.y);
    }

    /// <summary>
    /// Evaluates whether the minimum and maximum corners
    /// of a bounds are approximately unequal in at least
    /// one dimension.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>evaluation</returns>
    public static bool Any (in Bounds2 b)
    {
        Vec2 mn = b.min;
        Vec2 mx = b.max;
        return !Utils.Approx (mn.x, mx.x) ||
            !Utils.Approx (mn.y, mx.y);
    }

    /// <summary>
    /// Finds the area of a bounds.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>area</returns>
    public static float Area (in Bounds2 b)
    {
        return Utils.Diff (b.min.x, b.max.x) * Utils.Diff (b.min.y, b.max.y);
    }

    /// <summary>
    /// Finds the center of a bounding box.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>center</returns>
    public static Vec2 Center (in Bounds2 b)
    {
        return Vec2.Mix (b.min, b.max);
    }

    /// <summary>
    /// Evaluates whether a point is within the bounding box, lower bounds
    /// inclusive, upper bounds exclusive. For cases where multiple bounds must
    /// cover an box without overlap or gaps.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <param name="v">vector</param>
    /// <returns>evaluation</returns>
    public static bool ContainsInclExcl (in Bounds2 b, in Vec2 v)
    {
        return v.x >= b.min.x &&
            v.x < b.max.x &&
            v.y >= b.min.y &&
            v.y < b.max.y;
    }

    /// <summary>
    /// Finds the extent of the bounds, the absolute difference between its
    /// minimum and maximum corners.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>extent</returns>
    public static Vec2 Extent (in Bounds2 b)
    {
        return Vec2.Diff (b.max, b.min);
    }

    /// <summary>
    /// Creates a bounding box from a center and the box's extent.
    /// </summary>
    /// <param name="center">center</param>
    /// <param name="extent">extent</param>
    /// <returns>bounds</returns>
    public static Bounds2 FromCenterExtent (in Vec2 center, in Vec2 extent)
    {
        return Bounds2.FromCenterHalfExtent (center, extent * 0.5f);
    }

    /// <summary>
    /// Creates a bounding box from a center and half the box's extent.
    /// </summary>
    /// <param name="center">center</param>
    /// <param name="he">half-extent</param>
    /// <returns>bounds</returns>
    public static Bounds2 FromCenterHalfExtent (in Vec2 center, in Vec2 he)
    {
        return new Bounds2 (center - he, center + he);
    }

    /// <summary>
    /// Creates a bounding box to encompass an array of points.
    /// </summary>
    /// <param name="points">points</param>
    /// <returns>bounds</returns>
    public static Bounds2 FromPoints (params Vec2[ ] points)
    {
        int len = points.Length;
        if (len < 1) { return new Bounds2 ( ); }

        float lbx = float.MaxValue;
        float lby = float.MaxValue;

        float ubx = float.MinValue;
        float uby = float.MinValue;

        for (int i = 0; i < len; ++i)
        {
            Vec2 p = points[i];
            float x = p.x;
            float y = p.y;
            if (x < lbx)
            {
                lbx = x;
            }
            if (x > ubx)
            {
                ubx = x;
            }
            if (y < lby)
            {
                lby = y;
            }
            if (y > uby)
            {
                uby = y;
            }
        }

        lbx -= Utils.Epsilon * 2.0f;
        lby -= Utils.Epsilon * 2.0f;

        ubx += Utils.Epsilon * 2.0f;
        uby += Utils.Epsilon * 2.0f;

        return new Bounds2 (lbx, lby, ubx, uby);
    }

    /// <summary>
    /// Finds half the extent of the bounds.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>half extent</returns>
    public static Vec2 HalfExtent (in Bounds2 b)
    {
        return 0.5f * Bounds2.Extent (b);
    }

    /// <summary>
    /// Evaluates whether two bounding boxs intersect.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static bool Intersect (in Bounds2 a, in Bounds2 b)
    {
        return a.max.y > b.min.y ||
            a.min.y < b.max.y ||
            a.max.x > b.min.x ||
            a.min.x < b.max.x;
    }

    /// <summary>
    /// Evaluates whether the minimum and maximum corners
    /// of a bounds are approximately equal.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>evaluation</returns>
    public static bool None (in Bounds2 b)
    {
        return Vec2.Approx (b.min, b.max);
    }

    /// <summary>
    /// Splits a bounds into four children based on a factor in [0.0, 1.0].
    /// Returns an array.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <param name="xFac">x factor</param>
    /// <param name="yFac">y factor</param>
    /// <returns>four children</returns>
    public static Bounds2[ ] Split (in Bounds2 b, in float xFac = 0.5f, in float yFac = 0.5f)
    {
        Vec2 bMin = b.min;
        Vec2 bMax = b.max;

        float tx = Utils.Clamp (xFac, Utils.Epsilon, 1.0f - Utils.Epsilon);
        float ty = Utils.Clamp (yFac, Utils.Epsilon, 1.0f - Utils.Epsilon);

        float x = (1.0f - tx) * bMin.x + tx * bMax.x;
        float y = (1.0f - ty) * bMin.y + ty * bMax.y;

        return new Bounds2[ ]
        {
            new Bounds2 (bMin.x, bMin.y, x, y),
                new Bounds2 (x, bMin.y, bMax.x, y),
                new Bounds2 (bMin.x, y, x, bMax.y),
                new Bounds2 (x, y, bMax.x, bMax.y)
        };
    }

    /// <summary>
    /// Returns a string representation of a bounds.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string</returns>
    public static string ToString (in Bounds2 b, in int places = 4)
    {
        return Bounds2.ToString (new StringBuilder (128), b, places).ToString ( );
    }

    /// <summary>
    /// Appends a representation of a bounds to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="b">bounds</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString (in StringBuilder sb, in Bounds2 b, in int places = 4)
    {
        sb.Append ("{ min: ");
        Vec2.ToString (sb, b.min, places);
        sb.Append (", max: ");
        Vec2.ToString (sb, b.max, places);
        sb.Append (' ');
        sb.Append ('}');
        return sb;
    }

    /// <summary>
    /// Returns the validated version of the bounds, where
    /// the minimum corner is less than the maximum corner
    /// and the minimum is not equal to the maximum.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>validated</returns>
    public static Bounds2 Verified (in Bounds2 b)
    {
        Vec2 mn = b.min;
        Vec2 mx = b.max;

        float xMin = mn.x;
        float yMin = mn.y;

        float xMax = mx.x;
        float yMax = mx.y;

        float bxMin = xMin < xMax ? xMin : xMax;
        float byMin = yMin < yMax ? yMin : yMax;

        float bxMax = xMax > xMin ? xMax : xMin;
        float byMax = yMax > yMin ? yMax : yMin;

        if (Utils.Approx (bxMin, bxMax, Utils.Epsilon))
        {
            bxMin -= Utils.Epsilon * 2.0f;
            bxMax += Utils.Epsilon * 2.0f;
        }

        if (Utils.Approx (byMin, byMax, Utils.Epsilon))
        {
            byMin -= Utils.Epsilon * 2.0f;
            byMax += Utils.Epsilon * 2.0f;
        }

        return new Bounds2 (
            bxMin, byMin,
            bxMax, byMax);
    }

    /// <summary>
    /// Returns a boundary encompassing an signed unit square in the range
    /// [-1.0, 1.0] .
    /// </summary>
    /// <value>unit cube</value>
    public static Bounds2 UnitSquareSigned
    {
        get
        {
            return new Bounds2 ( //
                -1.0f - Utils.Epsilon * 2.0f,
                1.0f + Utils.Epsilon * 2.0f);
        }
    }

    /// <summary>
    /// Returns a boundary encompassing an unsigned unit square in the range
    /// [0.0, 1.0] .
    /// </summary>
    /// <value>unit cube</value>
    public static Bounds2 UnitSquareUnsigned
    {
        get
        {
            return new Bounds2 ( //
                -Utils.Epsilon * 2.0f,
                1.0f + Utils.Epsilon * 2.0f);
        }
    }
}