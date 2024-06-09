using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

/// <summary>
/// An axis aligned bounding box (AABB) for a 2D area, represented with a
/// minimum and maximum coordinate.
/// </summary>
[StructLayout(LayoutKind.Explicit, Pack = 16)]
public readonly struct Bounds2 : IComparable<Bounds2>, IEquatable<Bounds2>
{
    /// <summary>
    /// The maximum corner.
    /// </summary>
    [FieldOffset(8)] private readonly Vec2 max;

    /// <summary>
    /// The minimum corner.
    /// </summary>
    [FieldOffset(0)] private readonly Vec2 min;

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
    public Bounds2(in float min = -0.5f, in float max = 0.5f)
        : this(min, min, max, max) { }

    /// <summary>
    /// Creats a bounds from a nonuniform
    /// minimum and maximum expressed in floats.
    /// </summary>
    /// <param name="xMin">minimum x</param>
    /// <param name="yMin">minimum y</param>
    /// <param name="xMax">maximum x</param>
    /// <param name="yMax">maximum y</param>
    public Bounds2(
        in float xMin, in float yMin,
        in float xMax, in float yMax)
    {
        // This used to verify each bounds to be nonzero with positive area,
        // but that prevented bounds from signalling that an intersection had
        // a potentially negative area.
        this.min = new(xMin, yMin);
        this.max = new(xMax, yMax);
    }

    /// <summary>
    /// Creats a bounds from a nonuniform
    /// minimum and maximum.
    /// </summary>
    /// <param name="min">minimum</param>
    /// <param name="max">maximum</param>
    public Bounds2(in Vec2 min, in Vec2 max)
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
        if (value is Bounds2 bounds) { return this.Equals(bounds); }
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
        return Bounds2.ToString(this);
    }

    /// <summary>
    /// Compares this bounds to another.
    /// Bases the comparison on the bounds' centers.
    /// </summary>
    /// <param name="b">comparisand</param>
    /// <returns>evaluation</returns>
    public int CompareTo(Bounds2 b)
    {
        return Bounds2.Center(this).CompareTo(Bounds2.Center(b));
    }

    /// <summary>
    /// Tests this bounds for equivalence with another in compliance with the
    /// IEquatable interface.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>equivalence</returns>
    public bool Equals(Bounds2 b)
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
    public static Bounds2 operator &(in Bounds2 a, in Bounds2 b)
    {
        return Bounds2.FromIntersection(a, b);
    }

    /// <summary>
    /// Creates the union of the two operands.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>union</returns>
    public static Bounds2 operator |(in Bounds2 a, in Bounds2 b)
    {
        return Bounds2.FromUnion(a, b);
    }

    /// <summary>
    /// Scales a bounds by a nonuniform scalar.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>scaled bounds</returns>
    public static Bounds2 operator *(in Bounds2 a, in Vec2 b)
    {
        return Bounds2.Scale(a, b);
    }

    /// <summary>
    /// Scales a bounds by a nonuniform scalar.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>scaled bounds</returns>
    public static Bounds2 operator *(in Vec2 a, in Bounds2 b)
    {
        return Bounds2.Scale(b, a);
    }

    /// <summary>
    /// Finds the area of a bounds. Defaults to unsigned.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>area</returns>
    public static float Area(in Bounds2 b)
    {
        return Bounds2.AreaUnsigned(b);
    }

    /// <summary>
    /// Finds the signed area of a bounds.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>area</returns>
    public static float AreaSigned(in Bounds2 b)
    {
        return (b.max.Y - b.min.Y) * (b.max.X - b.min.X);
    }

    /// <summary>
    /// Finds the unsigned area of a bounds.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>area</returns>
    public static float AreaUnsigned(in Bounds2 b)
    {
        return MathF.Abs(Bounds2.AreaSigned(b));
    }

    /// <summary>
    /// Finds the center of a bounding box.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>center</returns>
    public static Vec2 Center(in Bounds2 b)
    {
        return Vec2.Mix(b.min, b.max);
    }

    /// <summary>
    /// Evaluates whether a point is within the bounding box, lower bounds
    /// inclusive, upper bounds exclusive. For cases where multiple bounds must
    /// cover an box without overlap or gaps.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <param name="v">vector</param>
    /// <returns>evaluation</returns>
    public static bool ContainsInclExcl(in Bounds2 b, in Vec2 v)
    {
        return v.X >= b.min.X &&
            v.X < b.max.X &&
            v.Y >= b.min.Y &&
            v.Y < b.max.Y;
    }

    /// <summary>
    /// Finds the extent of the bounds, the difference between its
    /// minimum and maximum corners.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>extent</returns>
    public static Vec2 Extent(in Bounds2 b)
    {
        return Bounds2.ExtentUnsigned(b);
    }

    /// <summary>
    /// Finds the extent of the bounds, the difference between its
    /// minimum and maximum corners.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>extent</returns>
    public static Vec2 ExtentSigned(in Bounds2 b)
    {
        return b.max - b.min;
    }

    /// <summary>
    /// Finds the extent of the bounds, the difference between its
    /// minimum and maximum corners.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>extent</returns>
    public static Vec2 ExtentUnsigned(in Bounds2 b)
    {
        return Vec2.Abs(Bounds2.ExtentSigned(b));
    }

    /// <summary>
    /// Creates a bounding box from a center and the box's extent.
    /// </summary>
    /// <param name="center">center</param>
    /// <param name="extent">extent</param>
    /// <returns>bounds</returns>
    public static Bounds2 FromCenterExtent(in Vec2 center, in Vec2 extent)
    {
        return Bounds2.FromCenterHalfExtent(center, extent * 0.5f);
    }

    /// <summary>
    /// Creates a bounding box from a center and half the box's extent.
    /// </summary>
    /// <param name="center">center</param>
    /// <param name="he">half-extent</param>
    /// <returns>bounds</returns>
    public static Bounds2 FromCenterHalfExtent(in Vec2 center, in Vec2 he)
    {
        return new(center - he, center + he);
    }

    /// <summary>
    /// Creates an intersection of the two operands.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>intersection</returns>
    public static Bounds2 FromIntersection(in Bounds2 a, in Bounds2 b)
    {
        return new(Vec2.Max(a.min, b.min), Vec2.Min(a.max, b.max));
    }

    /// <summary>
    /// Creates a bounding box to encompass an array of points.
    /// </summary>
    /// <param name="points">points</param>
    /// <returns>bounds</returns>
    public static Bounds2 FromPoints(in IEnumerable<Vec2> points)
    {
        float lbx = float.MaxValue;
        float lby = float.MaxValue;

        // Beware MinValue in C# differs from MIN_VALUE in Java.
        // https://learn.microsoft.com/en-us/dotnet/api/system.single.minvalue
        float ubx = float.MinValue;
        float uby = float.MinValue;

        int len = 0;
        foreach (Vec2 p in points)
        {
            ++len;
            float x = p.X;
            float y = p.Y;
            if (x < lbx) { lbx = x; }
            if (x > ubx) { ubx = x; }
            if (y < lby) { lby = y; }
            if (y > uby) { uby = y; }
        }

        if (len < 1) { return new(); }

        lbx -= Utils.Epsilon * 2.0f;
        lby -= Utils.Epsilon * 2.0f;

        ubx += Utils.Epsilon * 2.0f;
        uby += Utils.Epsilon * 2.0f;

        return new(lbx, lby, ubx, uby);
    }

    /// <summary>
    /// Creates a union of the two operands.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>union</returns>
    public static Bounds2 FromUnion(in Bounds2 a, in Bounds2 b)
    {
        return new(Vec2.Min(a.min, b.min), Vec2.Max(a.max, b.max));
    }

    /// <summary>
    /// Evaluates whether two bounding boxs intersect.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static bool Intersects(in Bounds2 a, in Bounds2 b)
    {
        return a.max.Y > b.min.Y ||
            a.min.Y < b.max.Y ||
            a.max.X > b.min.X ||
            a.min.X < b.max.X;
    }

    /// <summary>
    /// Evaluates whether a bounding area intersects a circle.
    /// </summary>
    /// <param name="a">bounding area</param>
    /// <param name="center">circle center</param>
    /// <param name="radius">circle radius</param>
    /// <returns>evaluation</returns>
    public static bool Intersects(in Bounds2 a, in Vec2 center, in float radius)
    {
        float yd = center.Y < a.min.Y ? center.Y - a.min.Y :
            center.Y > a.max.Y ? center.Y - a.max.Y :
            0.0f;
        float xd = center.X < a.min.X ? center.X - a.min.X :
            center.X > a.max.X ? center.X - a.max.X :
            0.0f;

        return xd * xd + yd * yd < radius * radius;
    }

    /// <summary>
    /// Evaluates whether a bounding area is negative.
    /// Returns a vector holding boolean values.
    /// </summary>
    /// <param name="b">bounding area</param>
    /// <returns>evaluation</returns>
    public static Vec2 IsNegative(in Bounds2 b)
    {
        return b.max < b.min;
    }

    /// <summary>
    /// Evaluates whether a bounding area is non-zero.
    /// Returns a vector holding boolean values.
    /// </summary>
    /// <param name="b">bounding area</param>
    /// <returns>evaluation</returns>
    public static Vec2 IsNonZero(in Bounds2 b)
    {
        Vec2 mn = b.min;
        Vec2 mx = b.max;
        return new(
            !Utils.Approx(mn.X, mx.X),
            !Utils.Approx(mn.Y, mx.Y));
    }

    /// <summary>
    /// Evaluates whether a bounding area is positive.
    /// Returns a vector holding boolean values.
    /// </summary>
    /// <param name="b">bounding area</param>
    /// <returns>evaluation</returns>
    public static Vec2 IsPositive(in Bounds2 b)
    {
        return b.min < b.max;
    }

    /// <summary>
    /// Evaluates whether a bounding area is zero.
    /// Returns a vector holding boolean values.
    /// </summary>
    /// <param name="b">bounding area</param>
    /// <returns>evaluation</returns>
    public static Vec2 IsZero(in Bounds2 b)
    {
        Vec2 mn = b.min;
        Vec2 mx = b.max;
        return new(
            Utils.Approx(mn.X, mx.X),
            Utils.Approx(mn.Y, mx.Y));
    }

    /// <summary>
    /// Mixes from an origin bounds to a destination by a step.
    /// </summary>
    /// <param name="a">original bounds</param>
    /// <param name="b">destination bounds</param>
    /// <param name="t">step</param>
    /// <returns>mix</returns>
    public static Bounds2 Mix(in Bounds2 a, in Bounds2 b, in float t = 0.5f)
    {
        float u = 1.0f - t;
        Vec2 aMn = a.min;
        Vec2 bMn = b.min;
        Vec2 aMx = a.max;
        Vec2 bMx = b.max;
        return new(
            u * aMn.X + t * bMn.X,
            u * aMn.Y + t * bMn.Y,
            u * aMx.X + t * bMx.X,
            u * aMx.Y + t * bMx.Y);
    }

    /// <summary>
    /// Scales a bounds from its center.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <param name="v">scale</param>
    /// <returns>scaled bounds</returns>
    public static Bounds2 Scale(in Bounds2 b, in float v)
    {
        return Bounds2.FromCenterExtent(
            Bounds2.Center(b),
            Bounds2.ExtentSigned(b) * v);
    }

    /// <summary>
    /// Scales a bounds from its center.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <param name="v">nonuniform scale</param>
    /// <returns>scaled bounds</returns>
    public static Bounds2 Scale(in Bounds2 b, in Vec2 v)
    {
        return Bounds2.FromCenterExtent(
            Bounds2.Center(b),
            Bounds2.ExtentSigned(b) * v);
    }

    /// <summary>
    /// Splits a bounds into four children based on a factor in [0.0, 1.0].
    /// Returns an array.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <param name="xFac">x factor</param>
    /// <param name="yFac">y factor</param>
    /// <returns>four children</returns>
    public static Bounds2[] Split(
        in Bounds2 b,
        in float xFac = 0.5f,
        in float yFac = 0.5f)
    {
        Vec2 bMin = b.min;
        Vec2 bMax = b.max;

        float tx = Utils.Clamp(xFac, Utils.Epsilon, 1.0f - Utils.Epsilon);
        float ty = Utils.Clamp(yFac, Utils.Epsilon, 1.0f - Utils.Epsilon);

        float x = (1.0f - tx) * bMin.X + tx * bMax.X;
        float y = (1.0f - ty) * bMin.Y + ty * bMax.Y;

        return new Bounds2[]
        {
            new(bMin.X, bMin.Y, x, y),
            new(x, bMin.Y, bMax.X, y),
            new(bMin.X, y, x, bMax.Y),
            new(x, y, bMax.X, bMax.Y)
        };
    }

    /// <summary>
    /// Returns a string representation of a bounds.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string</returns>
    public static string ToString(in Bounds2 b, in int places = 4)
    {
        return Bounds2.ToString(new StringBuilder(128), b, places).ToString();
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
        in Bounds2 b,
        in int places = 4)
    {
        sb.Append("{\"min\":");
        Vec2.ToString(sb, b.min, places);
        sb.Append(",\"max\":");
        Vec2.ToString(sb, b.max, places);
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
    public static Bounds2 Verified(in Bounds2 b)
    {
        Vec2 mn = b.min;
        Vec2 mx = b.max;

        float xMin = mn.X;
        float yMin = mn.Y;

        float xMax = mx.X;
        float yMax = mx.Y;

        float bxMin = xMin < xMax ? xMin : xMax;
        float byMin = yMin < yMax ? yMin : yMax;

        float bxMax = xMax > xMin ? xMax : xMin;
        float byMax = yMax > yMin ? yMax : yMin;

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

        return new(
            new Vec2(bxMin, byMin),
            new Vec2(bxMax, byMax));
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
            return new(-1.0f, 1.0f);
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
            return new(0.0f, 1.0f);
        }
    }
}