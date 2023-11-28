using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

/// <summary>
/// An axis aligned bounding box (AABB) for a 3D volume, represented with a
/// minimum and maximum coordinate.
/// </summary>
[StructLayout(LayoutKind.Explicit)]
public readonly struct Bounds3 : IComparable<Bounds3>, IEquatable<Bounds3>
{
    /// <summary>
    /// The maximum corner.
    /// </summary>
    [FieldOffset(12)] private readonly Vec3 max;

    /// <summary>
    /// The minimum corner.
    /// </summary>
    [FieldOffset(0)] private readonly Vec3 min;

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
    public Bounds3(in float min = -0.5f, in float max = 0.5f)
        : this(min, min, min, max, max, max) { }

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
    public Bounds3(
        in float xMin, in float yMin, in float zMin,
        in float xMax, in float yMax, in float zMax)
    {
        // This used to verify each bounds to be nonzero with a positive
        // volume, but that prevented bounds from signalling that an
        // intersection had a potentially negative volume.
        this.min = new(xMin, yMin, zMin);
        this.max = new(xMax, yMax, zMax);
    }

    /// <summary>
    /// Creats a bounds from a nonuniform
    /// minimum and maximum.
    /// </summary>
    /// <param name="min">minimum</param>
    /// <param name="max">maximum</param>
    public Bounds3(in Vec3 min, in Vec3 max)
    {
        this.min = min;
        this.max = max;
    }

    /// <summary>
    /// Tests this bounds for equivalence with an object.
    /// </summary>
    /// <param name="value">object</param>
    /// <returns>equivalence</returns>
    public override bool Equals(object value)
    {
        if (Object.ReferenceEquals(this, value)) { return true; }
        if (value is null) { return false; }
        if (value is Bounds3 bounds) { return this.Equals(bounds); }
        return false;
    }

    /// <summary>
    /// Returns the bounds's hash code based on those of its
    /// minimum and maximum.
    /// </summary>
    /// <returns>hash code</returns>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = Utils.HashBase;
            hash = hash * Utils.HashMul ^ this.min.GetHashCode();
            hash = hash * Utils.HashMul ^ this.max.GetHashCode();
            return hash;
        }
    }

    /// <summary>
    /// Returns a string representation of this bounds.
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()
    {
        return Bounds3.ToString(this);
    }

    /// <summary>
    /// Compares this bounds to another.
    /// Bases the comparison on the bounds' centers.
    /// </summary>
    /// <param name="b">comparisand</param>
    /// <returns>evaluation</returns>
    public int CompareTo(Bounds3 b)
    {
        return Bounds3.Center(this).CompareTo(Bounds3.Center(b));
    }

    /// <summary>
    /// Tests this bounds for equivalence with another in compliance with the
    /// IEquatable interface.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>equivalence</returns>
    public bool Equals(Bounds3 b)
    {
        return this.min.GetHashCode() == b.min.GetHashCode() &&
            this.max.GetHashCode() == b.max.GetHashCode();
    }

    /// <summary>
    /// Creates the intersection of the two operands.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>intersection</returns>
    public static Bounds3 operator &(in Bounds3 a, in Bounds3 b)
    {
        return Bounds3.FromIntersection(a, b);
    }

    /// <summary>
    /// Creates the union of the two operands.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>union</returns>
    public static Bounds3 operator |(in Bounds3 a, in Bounds3 b)
    {
        return Bounds3.FromUnion(a, b);
    }

    /// <summary>
    /// Scales a bounds by a nonuniform scalar.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>scaled bounds</returns>
    public static Bounds3 operator *(in Bounds3 a, in Vec3 b)
    {
        return Bounds3.Scale(a, b);
    }

    /// <summary>
    /// Scales a bounds by a nonuniform scalar.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>scaled bounds</returns>
    public static Bounds3 operator *(in Vec3 a, in Bounds3 b)
    {
        return Bounds3.Scale(b, a);
    }

    /// <summary>
    /// Finds the center of a bounding box.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>center</returns>
    public static Vec3 Center(in Bounds3 b)
    {
        return Vec3.Mix(b.min, b.max);
    }

    /// <summary>
    /// Evaluates whether a point is within the bounding volume, lower bounds
    /// inclusive, upper bounds exclusive. For cases where multiple bounds must
    /// cover an volume without overlap or gaps.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <param name="v">vector</param>
    /// <returns>evaluation</returns>
    public static bool ContainsInclExcl(in Bounds3 b, in Vec3 v)
    {
        return v.X >= b.min.X &&
            v.X < b.max.X &&
            v.Y >= b.min.Y &&
            v.Y < b.max.Y &&
            v.Z >= b.min.Z &&
            v.Z < b.max.Z;
    }

    /// <summary>
    /// Finds the extent of the bounds, the difference between its
    /// minimum and maximum corners.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>extent</returns>
    public static Vec3 Extent(in Bounds3 b)
    {
        return Bounds3.ExtentUnsigned(b);
    }

    /// <summary>
    /// Finds the extent of the bounds, the difference between its
    /// minimum and maximum corners.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>extent</returns>
    public static Vec3 ExtentSigned(in Bounds3 b)
    {
        return b.max - b.min;
    }

    /// <summary>
    /// Finds the extent of the bounds, the difference between its
    /// minimum and maximum corners.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>extent</returns>
    public static Vec3 ExtentUnsigned(in Bounds3 b)
    {
        return Vec3.Abs(Bounds3.ExtentSigned(b));
    }

    /// <summary>
    /// Creates a bounding box from a center and the box's extent.
    /// </summary>
    /// <param name="center">center</param>
    /// <param name="extent">extent</param>
    /// <returns>bounds</returns>
    public static Bounds3 FromCenterExtent(in Vec3 center, in Vec3 extent)
    {
        return Bounds3.FromCenterHalfExtent(center, extent * 0.5f);
    }

    /// <summary>
    /// Creates a bounding box from a center and half the box's extent.
    /// </summary>
    /// <param name="center">center</param>
    /// <param name="he">half-extent</param>
    /// <returns>bounds</returns>
    public static Bounds3 FromCenterHalfExtent(in Vec3 center, in Vec3 he)
    {
        return new(center - he, center + he);
    }

    /// <summary>
    /// Creates an intersection of the two operands.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>intersection</returns>
    public static Bounds3 FromIntersection(in Bounds3 a, in Bounds3 b)
    {
        return new(Vec3.Max(a.min, b.min), Vec3.Min(a.max, b.max));
    }

    /// <summary>
    /// Creates a bounding volume to encompass an array of points.
    /// </summary>
    /// <param name="points">points</param>
    /// <returns>bounds</returns>
    public static Bounds3 FromPoints(in IEnumerable<Vec3> points)
    {
        float lbx = float.MaxValue;
        float lby = float.MaxValue;
        float lbz = float.MaxValue;

        float ubx = float.MinValue;
        float uby = float.MinValue;
        float ubz = float.MinValue;

        int len = 0;
        foreach (Vec3 p in points)
        {
            ++len;
            float x = p.X;
            float y = p.Y;
            float z = p.Z;
            if (x < lbx) { lbx = x; }
            if (x > ubx) { ubx = x; }
            if (y < lby) { lby = y; }
            if (y > uby) { uby = y; }
            if (z < lbz) { lbz = z; }
            if (z > ubz) { ubz = z; }
        }

        if (len < 1) { return new(); }

        lbx -= Utils.Epsilon * 2.0f;
        lby -= Utils.Epsilon * 2.0f;
        lbz -= Utils.Epsilon * 2.0f;

        ubx += Utils.Epsilon * 2.0f;
        uby += Utils.Epsilon * 2.0f;
        ubz += Utils.Epsilon * 2.0f;

        return new(lbx, lby, lbz, ubx, uby, ubz);
    }

    /// <summary>
    /// Creates a union of the two operands.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>union</returns>
    public static Bounds3 FromUnion(in Bounds3 a, in Bounds3 b)
    {
        return new(Vec3.Min(a.min, b.min), Vec3.Max(a.max, b.max));
    }

    /// <summary>
    /// Evaluates whether two bounding volumes intersect.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static bool Intersects(in Bounds3 a, in Bounds3 b)
    {
        return a.max.Z > b.min.Z ||
            a.min.Z < b.max.Z ||
            a.max.Y > b.min.Y ||
            a.min.Y < b.max.Y ||
            a.max.X > b.min.X ||
            a.min.X < b.max.X;
    }

    /// <summary>
    /// Evaluates whether a bounding volume intersects a sphere.
    /// </summary>
    /// <param name="a">bounding volume</param>
    /// <param name="center">sphere center</param>
    /// <param name="radius">sphere radius</param>
    /// <returns>evaluation</returns>
    public static bool Intersects(in Bounds3 a, in Vec3 center, in float radius)
    {
        float zd = center.Z < a.min.Z ? center.Z - a.min.Z :
            center.Z > a.max.Z ? center.Z - a.max.Z :
            0.0f;
        float yd = center.Y < a.min.Y ? center.Y - a.min.Y :
            center.Y > a.max.Y ? center.Y - a.max.Y :
            0.0f;
        float xd = center.X < a.min.X ? center.X - a.min.X :
            center.X > a.max.X ? center.X - a.max.X :
            0.0f;

        return xd * xd + yd * yd + zd * zd < radius * radius;
    }

    /// <summary>
    /// Evaluates whether a bounding volume is negative.
    /// Returns a vector holding boolean values.
    /// </summary>
    /// <param name="b">bounding volume</param>
    /// <returns>evaluation</returns>
    public static Vec3 IsNegative(in Bounds3 b)
    {
        return b.max < b.min;
    }

    /// <summary>
    /// Evaluates whether a bounding volume is non-zero.
    /// Returns a vector holding boolean values.
    /// </summary>
    /// <param name="b">bounding volume</param>
    /// <returns>evaluation</returns>
    public static Vec3 IsNonZero(in Bounds3 b)
    {
        Vec3 mn = b.min;
        Vec3 mx = b.max;
        return new(
            !Utils.Approx(mn.X, mx.X),
            !Utils.Approx(mn.Y, mx.Y),
            !Utils.Approx(mn.Z, mx.Z));
    }

    /// <summary>
    /// Evaluates whether a bounding volume is positive.
    /// Returns a vector holding boolean values.
    /// </summary>
    /// <param name="b">bounding volume</param>
    /// <returns>evaluation</returns>
    public static Vec3 IsPositive(in Bounds3 b)
    {
        return b.min < b.max;
    }

    /// <summary>
    /// Evaluates whether a bounding volume is zero.
    /// Returns a vector holding boolean values.
    /// </summary>
    /// <param name="b">bounding volume</param>
    /// <returns>evaluation</returns>
    public static Vec3 IsZero(in Bounds3 b)
    {
        Vec3 mn = b.min;
        Vec3 mx = b.max;
        return new(
            Utils.Approx(mn.X, mx.X),
            Utils.Approx(mn.Y, mx.Y),
            Utils.Approx(mn.Z, mx.Z));
    }

    /// <summary>
    /// Mixes from an origin bounds to a destination by a step.
    /// </summary>
    /// <param name="a">original bounds</param>
    /// <param name="b">destination bounds</param>
    /// <param name="t">step</param>
    /// <returns>mix</returns>
    public static Bounds3 Mix(in Bounds3 a, in Bounds3 b, in float t = 0.5f)
    {
        float u = 1.0f - t;
        Vec3 aMn = a.min;
        Vec3 bMn = b.min;
        Vec3 aMx = a.max;
        Vec3 bMx = b.max;
        return new(
            u * aMn.X + t * bMn.X,
            u * aMn.Y + t * bMn.Y,
            u * aMn.Z + t * bMn.Z,
            u * aMx.X + t * bMx.X,
            u * aMx.Y + t * bMx.Y,
            u * aMx.Z + t * bMx.Z);
    }

    /// <summary>
    /// Scales a bounds from its center.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <param name="v">scale</param>
    /// <returns>scaled bounds</returns>
    public static Bounds3 Scale(in Bounds3 b, in float v)
    {
        return Bounds3.FromCenterExtent(
            Bounds3.Center(b),
            Bounds3.ExtentSigned(b) * v);
    }

    /// <summary>
    /// Scales a bounds from its center.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <param name="v">nonuniform scale</param>
    /// <returns>scaled bounds</returns>
    public static Bounds3 Scale(in Bounds3 b, in Vec3 v)
    {
        return Bounds3.FromCenterExtent(
            Bounds3.Center(b),
            Bounds3.ExtentSigned(b) * v);
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
    public static Bounds3[] Split(
        in Bounds3 b,
        in float xFac = 0.5f,
        in float yFac = 0.5f,
        in float zFac = 0.5f)
    {
        Vec3 bMin = b.min;
        Vec3 bMax = b.max;

        float tx = Utils.Clamp(xFac, Utils.Epsilon, 1.0f - Utils.Epsilon);
        float ty = Utils.Clamp(yFac, Utils.Epsilon, 1.0f - Utils.Epsilon);
        float tz = Utils.Clamp(zFac, Utils.Epsilon, 1.0f - Utils.Epsilon);

        float x = (1.0f - tx) * bMin.X + tx * bMax.X;
        float y = (1.0f - ty) * bMin.Y + ty * bMax.Y;
        float z = (1.0f - tz) * bMin.Z + tz * bMax.Z;

        return new Bounds3[]
        {
            new(bMin.X, bMin.Y, bMin.Z, x, y, z),
            new(x, bMin.Y, bMin.Z, bMax.X, y, z),
            new(bMin.X, y, bMin.Z, x, bMax.Y, z),
            new(x, y, bMin.Z, bMax.X, bMax.Y, z),
            new(bMin.X, bMin.Y, z, x, y, bMax.Z),
            new(x, bMin.Y, z, bMax.X, y, bMax.Z),
            new(bMin.X, y, z, x, bMax.Y, bMax.Z),
            new(x, y, z, bMax.X, bMax.Y, bMax.Z)
        };
    }

    /// <summary>
    /// Returns a string representation of a bounds.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string</returns>
    public static string ToString(in Bounds3 b, in int places = 4)
    {
        return Bounds3.ToString(new StringBuilder(128), b, places).ToString();
    }

    /// <summary>
    /// Appends a representation of a bounds to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="b">bounds</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString(
        in StringBuilder sb,
        in Bounds3 b,
        in int places = 4)
    {
        sb.Append("{\"min\":");
        Vec3.ToString(sb, b.min, places);
        sb.Append(",\"max\":");
        Vec3.ToString(sb, b.max, places);
        sb.Append('}');
        return sb;
    }

    /// <summary>
    /// Returns a bounds where all components of the minimum are less than
    /// those of the maximum, and that the edges of the bounds do not equal
    /// each other.
    /// </summary>
    /// <param name="b">b</param>
    /// <returns>verified bounds</returns>
    public static Bounds3 Verified(in Bounds3 b)
    {
        Vec3 mn = b.min;
        Vec3 mx = b.max;

        float xMin = mn.X;
        float yMin = mn.Y;
        float zMin = mn.Z;

        float xMax = mx.X;
        float yMax = mx.Y;
        float zMax = mx.Z;

        float bxMin = xMin < xMax ? xMin : xMax;
        float byMin = yMin < yMax ? yMin : yMax;
        float bzMin = zMin < zMax ? zMin : zMax;

        float bxMax = xMax > xMin ? xMax : xMin;
        float byMax = yMax > yMin ? yMax : yMin;
        float bzMax = zMax > zMin ? zMax : zMin;

        if (Utils.Approx(bxMin, bxMax, Utils.Epsilon))
        {
            bxMin -= Utils.Epsilon * 2.0f;
            bxMax += Utils.Epsilon * 2.0f;
        }

        if (Utils.Approx(byMin, byMax, Utils.Epsilon))
        {
            byMin -= Utils.Epsilon * 2.0f;
            byMax += Utils.Epsilon * 2.0f;
        }

        if (Utils.Approx(bzMin, bzMax, Utils.Epsilon))
        {
            bzMin -= Utils.Epsilon * 2.0f;
            bzMax += Utils.Epsilon * 2.0f;
        }

        return new(
            new Vec3(bxMin, byMin, bzMin),
            new Vec3(bxMax, byMax, bzMax));
    }

    /// <summary>
    /// Finds the volume of a bounds. Defaults to unsigned.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>volume</returns>
    public static float Volume(in Bounds3 b)
    {
        return Bounds3.VolumeUnsigned(b);
    }

    /// <summary>
    /// Finds the signed volume of a bounds.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>volume</returns>
    public static float VolumeSigned(in Bounds3 b)
    {
        return (b.max.Z - b.min.Z) *
            (b.max.Y - b.min.Y) *
            (b.max.X - b.min.X);
    }

    /// <summary>
    /// Finds the unsigned volume of a bounds.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>volume</returns>
    public static float VolumeUnsigned(in Bounds3 b)
    {
        return MathF.Abs(Bounds3.VolumeSigned(b));
    }

    /// <summary>
    /// Returns a boundary encompassing the LAB color space.
    /// </summary>
    /// <value>bounds</value>
    public static Bounds3 Lab
    {
        get
        {
            return new(
                -111.0f, -111.0f, -1.0f,
                111.0f, 111.0f, 101.0f);
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
            return new(
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
            return new(
                -Utils.Epsilon * 2.0f,
                1.0f + Utils.Epsilon * 2.0f);
        }
    }
}