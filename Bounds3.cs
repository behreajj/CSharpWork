using System;
using System.Text;

/// <summary>
/// An axis aligned bounding box (AABB) for a 3D volume, represented with a
/// minimum and maximum coordinate.
/// </summary>
public readonly struct Bounds3 : IComparable<Bounds3>, IEquatable<Bounds3>
{
    /// <summary>
    /// The maximum corner.
    /// </summary>
    private readonly Vec3 max;

    /// <summary>
    /// The minimum corner.
    /// </summary>
    private readonly Vec3 min;

    /// <summary>
    /// Gets the maximum corner.
    /// </summary>
    /// <value>maximum</value>
    public Vec3 Max { get { return this.max; } }

    /// <summary>
    /// Gets the minimum corner.
    /// </summary>
    /// <value>minimum</value>
    public Vec3 Min { get { return this.min; } }

    /// <summary>
    /// Creates a bounds from a minimum and maximum.
    /// </summary>
    /// <param name="min">minimum</param>
    /// <param name="max">maximum</param>
    public Bounds3 (in float min = -0.5f, in float max = 0.5f)
    {
        this.min = new Vec3 (min, min, min);
        this.max = new Vec3 (max, max, max);
    }

    /// <summary>
    /// Creats a bounds from a nonuniform
    /// minimum and maximum expressed in floats.
    /// </summary>
    /// <param name="xMin">minimum x</param>
    /// <param name="yMin">minimum y</param>
    /// <param name="zMin">minimum z</param>
    /// <param name="xMax">maximum x</param>
    /// <param name="yMax">maximum y</param>
    /// <param name="zMax">maximum z</param>
    public Bounds3 ( //
        in float xMin, //
        in float yMin, //
        in float zMin, //
        in float xMax, //
        in float yMax, //
        in float zMax)
    {
        this.min = new Vec3 (xMin, yMin, zMin);
        this.max = new Vec3 (xMax, yMax, zMax);
    }

    /// <summary>
    /// Creats a bounds from a nonuniform
    /// minimum and maximum.
    /// </summary>
    /// <param name="min">minimum</param>
    /// <param name="max">maximum</param>
    public Bounds3 (in Vec3 min, in Vec3 max)
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
        if (Object.ReferenceEquals (this, value)) { return true; }
        if (value is null) { return false; }
        if (value is Bounds3) { return this.Equals ((Bounds3) value); }
        return false;
    }

    /// <summary>
    /// Returns a string representation of this bounds.
    /// </summary>
    /// <returns>the string</returns>
    public override string ToString ( )
    {
        return Bounds3.ToString (this);
    }

    /// <summary>
    /// Compares this bounds to another in compliance with the IComparable
    /// interface.
    /// </summary>
    /// <param name="b">comparisand</param>
    /// <returns>evaluation</returns>
    public int CompareTo (Bounds3 b)
    {
        return Bounds3.Center (this).CompareTo (Bounds3.Center (b));
    }

    /// <summary>
    /// Tests this bounds for equivalence with another in compliance with the
    /// IEquatable interface.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>equivalence</returns>
    public bool Equals (Bounds3 b)
    {
        if (this.min.GetHashCode ( ) != b.min.GetHashCode ( )) { return false; }
        if (this.max.GetHashCode ( ) != b.max.GetHashCode ( )) { return false; }
        return true;
    }

    /// <summary>
    /// A bounds evaluates to true when its minimum and maximum
    /// corners are approximately unequal in all dimensions.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>evaluation</returns>
    public static bool operator true (in Bounds3 b)
    {
        return Bounds3.All (b);
    }

    /// <summary>
    /// A bounds evaluates to false when its minimum and maximum
    /// corners are approximately equal in all dimensions.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>evaluation</returns>
    public static bool operator false (in Bounds3 b)
    {
        return Bounds3.None (b);
    }

    /// <summary>
    /// Evaluates whether the minimum and maximum corners
    /// of a bounds are approximately unequal in all
    /// dimensions.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>evaluation</returns>
    public static bool All (in Bounds3 b)
    {
        Vec3 mn = b.min;
        Vec3 mx = b.max;
        return !Utils.Approx (mn.x, mx.x, Utils.Epsilon) &&
            !Utils.Approx (mn.y, mx.y, Utils.Epsilon) &&
            !Utils.Approx (mn.z, mx.z, Utils.Epsilon);
    }

    /// <summary>
    /// Evaluates whether the minimum and maximum corners
    /// of a bounds are approximately unequal in at least
    /// one dimension.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>evaluation</returns>
    public static bool Any (in Bounds3 b)
    {
        Vec3 mn = b.min;
        Vec3 mx = b.max;
        return !Utils.Approx (mn.x, mx.x, Utils.Epsilon) ||
            !Utils.Approx (mn.y, mx.y, Utils.Epsilon) ||
            !Utils.Approx (mn.z, mx.z, Utils.Epsilon);
    }

    /// <summary>
    /// Finds the center of a bounding box.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>center</returns>
    public static Vec3 Center (in Bounds3 b)
    {
        return Vec3.Mix (b.min, b.max);
    }

    /// <summary>
    /// Evaluates whether a point is within the bounding volume, lower bounds
    /// inclusive, upper bounds exclusive. For cases where multiple bounds must
    /// cover an volume without overlap or gaps.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <param name="v">vector</param>
    /// <returns>evaluation</returns>
    public static bool ContainsInclExcl (in Bounds3 b, in Vec3 v)
    {
        return v.x >= b.min.x &&
            v.x < b.max.x &&
            v.y >= b.min.y &&
            v.y < b.max.y &&
            v.z >= b.min.z &&
            v.z < b.max.z;
    }

    /// <summary>
    /// Finds the extent of the bounds, the absolute difference between its
    /// minimum and maximum corners.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>extent</returns>
    public static Vec3 Extent (in Bounds3 b)
    {
        return Vec3.Diff (b.max, b.min);
    }

    /// <summary>
    /// Creates a bounding volume from a center and the volume's extent.
    /// </summary>
    /// <param name="center">center</param>
    /// <param name="extent">extent</param>
    /// <returns>bounds</returns>
    public static Bounds3 FromCenterExtent (in Vec3 center, in Vec3 extent)
    {
        return new Bounds3 (
            center - extent * 0.5f,
            center + extent * 0.5f);
    }

    /// <summary>
    /// Creates a bounding volume from a center and half the volume's extent.
    /// </summary>
    /// <param name="center">center</param>
    /// <param name="he">half-extent</param>
    /// <returns>bounds</returns>
    public static Bounds3 FromCenterHalfExtent (in Vec3 center, in Vec3 he)
    {
        return new Bounds3 (
            center - he,
            center + he);
    }

    /// <summary>
    /// Creates a bounding volume to encompass an array of points.
    /// </summary>
    /// <param name="points">points</param>
    /// <returns>bounds</returns>
    public static Bounds3 FromPoints (params Vec3[ ] points)
    {
        int len = points.Length;
        if (len < 1) { return new Bounds3 ( ); }

        float lbx = Single.MaxValue;
        float lby = Single.MaxValue;
        float lbz = Single.MaxValue;

        float ubx = Single.MinValue;
        float uby = Single.MinValue;
        float ubz = Single.MinValue;

        for (int i = 0; i < len; ++i)
        {
            Vec3 p = points[i];
            float x = p.x;
            float y = p.y;
            float z = p.z;
            if (x < lbx) { lbx = x; }
            if (x > ubx) { ubx = x; }
            if (y < lby) { lby = y; }
            if (y > uby) { uby = y; }
            if (z < lbz) { lbz = z; }
            if (z > ubz) { ubz = z; }
        }

        lbx -= Utils.Epsilon * 2.0f;
        lby -= Utils.Epsilon * 2.0f;
        lbz -= Utils.Epsilon * 2.0f;

        ubx += Utils.Epsilon * 2.0f;
        uby += Utils.Epsilon * 2.0f;
        ubz += Utils.Epsilon * 2.0f;

        return new Bounds3 (lbx, lby, lbz, ubx, uby, ubz);
    }

    /// <summary>
    /// Finds half the extent of the bounds.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>half extent</returns>
    public static Vec3 HalfExtent (in Bounds3 b)
    {
        return 0.5f * Bounds3.Extent (b);
    }

    /// <summary>
    /// Evaluates whether two bounding volumes intersect.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static bool Intersect (in Bounds3 a, in Bounds3 b)
    {
        return a.max.x > b.min.x ||
            a.min.x < b.max.x ||
            a.max.y > b.min.y ||
            a.min.y < b.max.y ||
            a.max.z > b.min.z ||
            a.min.z < b.max.z;
    }

    /// <summary>
    /// Evaluates whether the minimum and maximum corners
    /// of a bounds are approximately equal.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>evaluation</returns>
    public static bool None (in Bounds3 b)
    {
        return Vec3.Approx (b.min, b.max, Utils.Epsilon);
    }

    /// <summary>
    /// Splits a bounds into eight children based on a factor in [0.0, 1.0].
    /// Returns an array.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <param name="xFac">x factor</param>
    /// <param name="yFac">y factor</param>
    /// <param name="zFac">z factor</param>
    /// <returns>eight children</returns>
    public static Bounds3[ ] Split ( //
        in Bounds3 b, //
        in float xFac = 0.5f, //
        in float yFac = 0.5f, //
        in float zFac = 0.5f)
    {
        Vec3 bMin = b.min;
        Vec3 bMax = b.max;

        float tx = Utils.Clamp (xFac, Utils.Epsilon, 1.0f - Utils.Epsilon);
        float ty = Utils.Clamp (yFac, Utils.Epsilon, 1.0f - Utils.Epsilon);
        float tz = Utils.Clamp (zFac, Utils.Epsilon, 1.0f - Utils.Epsilon);

        float x = (1.0f - tx) * bMin.x + tx * bMax.x;
        float y = (1.0f - ty) * bMin.y + ty * bMax.y;
        float z = (1.0f - tz) * bMin.z + tz * bMax.z;

        return new Bounds3[ ]
        {
            new Bounds3 (bMin.x, bMin.y, bMin.z, x, y, z),
                new Bounds3 (x, bMin.y, bMin.z, bMax.x, y, z),
                new Bounds3 (bMin.x, y, bMin.z, x, bMax.y, z),
                new Bounds3 (x, y, bMin.z, bMax.x, bMax.y, z),
                new Bounds3 (bMin.x, bMin.y, z, x, y, bMax.z),
                new Bounds3 (x, bMin.y, z, bMax.x, y, bMax.z),
                new Bounds3 (bMin.x, y, z, x, bMax.y, bMax.z),
                new Bounds3 (x, y, z, bMax.x, bMax.y, bMax.z)
        };
    }

    /// <summary>
    /// Returns a string representation of a bounds.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string</returns>
    public static string ToString (in Bounds3 b, in int places = 4)
    {
        return Bounds3.ToString (new StringBuilder (128), b, places).ToString ( );
    }

    /// <summary>
    /// Appends a representation of a bounds to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="b">bounds</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString (in StringBuilder sb, in Bounds3 b, in int places = 4)
    {
        sb.Append ("{ min: ");
        Vec3.ToString (sb, b.min, places);
        sb.Append (", max: ");
        Vec3.ToString (sb, b.max, places);
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
    public static Bounds3 Verified (in Bounds3 b)
    {
        Vec3 mn = b.min;
        Vec3 mx = b.max;

        float xMin = mn.x;
        float yMin = mn.y;
        float zMin = mn.z;

        float xMax = mx.x;
        float yMax = mx.y;
        float zMax = mx.z;

        float bxMin = xMin < xMax ? xMin : xMax;
        float byMin = yMin < yMax ? yMin : yMax;
        float bzMin = zMin < zMax ? zMin : zMax;

        float bxMax = xMax > xMin ? xMax : xMin;
        float byMax = yMax > yMin ? yMax : yMin;
        float bzMax = zMax > zMin ? zMax : zMin;

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

        if (Utils.Approx (bzMin, bzMax, Utils.Epsilon))
        {
            bzMin -= Utils.Epsilon * 2.0f;
            bzMax += Utils.Epsilon * 2.0f;
        }

        return new Bounds3 (
            bxMin, byMin, bzMin,
            bxMax, byMax, bzMax);
    }

    /// <summary>
    /// Finds the volume of a bounds.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>volume</returns>
    public static float Volume (in Bounds3 b)
    {
        Vec3 df = Vec3.Diff (b.max, b.min);
        return df.x * df.y * df.z;
    }

    /// <summary>
    /// Returns a boundary encompassing the CIE L*a*b* color space, with a
    /// minimum at (-110.0, -110.0, -1.0) and a maximum of (110.0, 110.0,
    /// 101.0).
    /// </summary>
    /// <value>bounds</value>
    public static Bounds3 CieLab
    {
        get
        {
            return new Bounds3 ( //
                -110.0f, -110.0f, -1.0f,
                110.0f, 110.0f, 101.0f);
        }
    }

    /// <summary>
    /// Returns a boundary encompassing an signed unit cube in the range
    /// [-1.0, 1.0] .
    /// </summary>
    /// <value>unit cube</value>
    public static Bounds3 UnitCubeSigned
    {
        get
        {
            return new Bounds3 ( //
                -1.0f - Utils.Epsilon * 2.0f,
                1.0f + Utils.Epsilon * 2.0f);
        }
    }

    /// <summary>
    /// Returns a boundary encompassing an unsigned unit cube in the range
    /// [0.0, 1.0] .
    /// </summary>
    /// <value>unit cube</value>
    public static Bounds3 UnitCubeUnsigned
    {
        get
        {
            return new Bounds3 ( //
                -Utils.Epsilon * 2.0f,
                1.0f + Utils.Epsilon * 2.0f);
        }
    }
}