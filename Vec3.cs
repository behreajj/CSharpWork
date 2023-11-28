using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;

/// <summary>
/// A readonly struct influenced by GLSL and OSL. This is intended
/// for storing points and directions in three-dimensional graphics
/// programs.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Explicit)]
public readonly struct Vec3 : IComparable<Vec3>, IEquatable<Vec3>, IEnumerable
{
    /// <summary>
    /// Component on the x axis in the Cartesian coordinate system.
    /// </summary>
    [FieldOffset(0)] private readonly float x;

    /// <summary>
    /// Component on the y axis in the Cartesian coordinate system.
    /// </summary>
    [FieldOffset(4)] private readonly float y;

    /// <summary>
    /// Component on the z axis in the Cartesian coordinate system.
    /// </summary>
    [FieldOffset(8)] private readonly float z;

    /// <summary>
    /// The number of values (dimensions) in this vector.
    /// </summary>
    /// <value>length</value>
    public int Length { get { return 3; } }

    /// <summary>
    /// Component on the z axis in the Cartesian coordinate system.
    /// </summary>
    /// <value>x</value>
    public float X { get { return this.x; } }

    /// <summary>
    /// Component on the y axis in the Cartesian coordinate system.
    /// </summary>
    /// <value>y</value>
    public float Y { get { return this.y; } }

    /// <summary>
    /// Component on the z axis in the Cartesian coordinate system.
    /// </summary>
    /// <value>z</value>
    public float Z { get { return this.z; } }

    /// <summary>
    /// Gets the x and y components as a 2D vector.
    /// </summary>
    /// <value>2D vector</value>
    public Vec2 XY
    {
        get
        {
            return new Vec2(this.x, this.y);
        }
    }

    /// <summary>
    /// Gets the x and z components as a 2D vector.
    /// </summary>
    /// <value>2D vector</value>
    public Vec2 XZ
    {
        get
        {
            return new Vec2(this.x, this.z);
        }
    }

    /// <summary>
    /// Retrieves a component by index. When the provided index is 2 or -1,
    /// returns z; 1 or -2, y; 0 or -3, x.
    /// </summary>
    /// <value>the component</value>
    public float this[int i]
    {
        get
        {
            return i switch
            {
                0 or -3 => this.x,
                1 or -2 => this.y,
                2 or -1 => this.z,
                _ => 0.0f,
            };
        }
    }

    /// <summary>
    /// Constructs a vector from single precision real numbers.
    /// </summary>
    /// <param name="x">x component</param>
    /// <param name="y">y component</param>
    /// <param name="z">z component</param>
    public Vec3(in float x, in float y, in float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    /// <summary>
    /// Constructs a vector from boolean values, where true is 1.0 and false is
    /// 0.0 .
    /// </summary>
    /// <param name="x">x component</param>
    /// <param name="y">y component</param>
    /// <param name="z">z component</param>
    public Vec3(in bool x, in bool y, in bool z)
    {
        this.x = x ? 1.0f : 0.0f;
        this.y = y ? 1.0f : 0.0f;
        this.z = z ? 1.0f : 0.0f;
    }

    /// <summary>
    /// Tests this vector for equivalence with an object. For approximate
    /// equality with another vector, use the static approx function instead.
    /// </summary>
    /// <param name="value">the object</param>
    /// <returns>equivalence</returns>
    public override bool Equals(object value)
    {
        if (Object.ReferenceEquals(this, value)) { return true; }
        if (value is null) { return false; }
        if (value is Vec3 vec) { return this.Equals(vec); }
        return false;
    }

    /// <summary>
    /// Returns a hash code representing this vector.
    /// </summary>
    /// <returns>hash code</returns>
    public override int GetHashCode()
    {
        unchecked
        {
            return ((Utils.MulBase ^ this.x.GetHashCode()) *
                    Utils.HashMul ^ this.y.GetHashCode()) *
                Utils.HashMul ^ this.z.GetHashCode();
        }
    }

    /// <summary>
    /// Returns a string representation of this vector.
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()
    {
        return Vec3.ToString(this);
    }

    /// <summary>
    /// Compares this vector to another.
    /// Returns 1 when a component of this vector is greater than
    /// another; -1 when lesser. Returns 0 as a last resort.
    /// Prioritizes the highest dimension first: z, y, x. 
    /// </summary>
    /// <param name="v">the comparisand</param>
    /// <returns>evaluation</returns>
    public int CompareTo(Vec3 v)
    {
        return (this.z < v.z) ? -1 :
            (this.z > v.z) ? 1 :
            (this.y < v.y) ? -1 :
            (this.y > v.y) ? 1 :
            (this.x < v.x) ? -1 :
            (this.x > v.x) ? 1 :
            0;
    }

    /// <summary>
    /// Tests this vector for equivalence with another in compliance with the
    /// IEquatable interface. For approximate equality with another vector, use
    /// the static approx function instead.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>equivalence</returns>
    public bool Equals(Vec3 v)
    {
        if (this.z.GetHashCode() != v.z.GetHashCode()) { return false; }
        if (this.y.GetHashCode() != v.y.GetHashCode()) { return false; }
        if (this.x.GetHashCode() != v.x.GetHashCode()) { return false; }
        return true;
    }

    /// <summary>
    /// Returns an enumerator (or iterator) for this vector, allowing its
    /// components to be accessed in a foreach loop.
    /// </summary>
    /// <returns>enumerator</returns>
    public IEnumerator GetEnumerator()
    {
        yield return this.x;
        yield return this.y;
        yield return this.z;
    }

    /// <summary>
    /// Converts a boolean to a vector by supplying the boolean to all the
    /// vector's components: 1.0 for true; 0.0 for false.
    /// </summary>
    /// <param name="b">boolean</param>
    public static implicit operator Vec3(in bool b)
    {
        float eval = b ? 1.0f : 0.0f;
        return new(eval, eval, eval);
    }

    /// <summary>
    /// Converts a float to a vector by supplying the scalar to all the vector's
    /// components.
    /// </summary>
    /// <param name="s">scalar</param>
    public static implicit operator Vec3(in float s)
    {
        return new(s, s, s);
    }

    /// <summary>
    /// Promotes a 2D vector to a 3D vector; the z component is assumed to be
    /// 0.0 .
    /// </summary>
    /// <param name="v">2D vector</param>
    public static implicit operator Vec3(in Vec2 v)
    {
        return Vec3.Promote(v, 0.0f);
    }

    /// <summary>
    /// Converts a vector to a boolean by finding whether all of its components
    /// are non-zero.
    /// </summary>
    /// <param name="v">vector</param>
    public static explicit operator bool(in Vec3 v)
    {
        return Vec3.All(v);
    }

    /// <summary>
    /// A vector evaluates to true when all of its components are not equal to
    /// zero.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>evaluation</returns>
    public static bool operator true(in Vec3 v)
    {
        return Vec3.All(v);
    }

    /// <summary>
    /// A vector evaluates to false when all of its components are equal to
    /// zero.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>evaluation</returns>
    public static bool operator false(in Vec3 v)
    {
        return Vec3.None(v);
    }

    /// <summary>
    /// Evaluates a vector as a boolean. Equivalent to using the ! operator.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>opposite</returns>
    public static Vec3 operator !(in Vec3 v)
    {
        return Vec3.Not(v);
    }

    /// <summary>
    /// Evaluates a vector as a boolean. Equivalent to using the ~ operator.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>complement</returns>
    public static Vec3 operator ~(in Vec3 v)
    {
        return Vec3.Not(v);
    }

    /// <summary>
    /// Evaluates two vectors like booleans, using the inclusive and (AND) logic
    /// gate.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>evaluation</returns>
    public static Vec3 operator &(in Vec3 a, in Vec3 b)
    {
        return new(
            Utils.And(a.x, b.x),
            Utils.And(a.y, b.y),
            Utils.And(a.z, b.z));
    }

    /// <summary>
    /// Evaluates two vectors like booleans, using the inclusive or (OR) logic
    /// gate.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>evaluation</returns>
    public static Vec3 operator |(in Vec3 a, in Vec3 b)
    {
        return new(
            Utils.Or(a.x, b.x),
            Utils.Or(a.y, b.y),
            Utils.Or(a.z, b.z));
    }

    /// <summary>
    /// Evaluates two vectors like booleans, using the exclusive or (XOR) logic
    /// gate.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>evaluation</returns>
    public static Vec3 operator ^(in Vec3 a, in Vec3 b)
    {
        return new(
            Utils.Xor(a.x, b.x),
            Utils.Xor(a.y, b.y),
            Utils.Xor(a.z, b.z));
    }

    /// <summary>
    /// Negates the vector.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>negation</returns>
    public static Vec3 operator -(in Vec3 v)
    {
        return new(-v.x, -v.y, -v.z);
    }

    /// <summary>
    /// Increments all components of a vector by 1.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>increment</returns>
    public static Vec3 operator ++(in Vec3 v)
    {
        return new(v.x + 1.0f, v.y + 1.0f, v.z + 1.0f);
    }

    /// <summary>
    /// Decrements all components of a vector by 1.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>decrement</returns>
    public static Vec3 operator --(in Vec3 v)
    {
        return new(v.x - 1.0f, v.y - 1.0f, v.z - 1.0f);
    }

    /// <summary>
    /// Multiplies two vectors, component-wise, i.e.,
    /// returns the Hadamard product.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>product</returns>
    public static Vec3 operator *(in Vec3 a, in Vec3 b)
    {
        return new(
            a.x * b.x,
            a.y * b.y,
            a.z * b.z);
    }

    /// <summary>
    /// Multiplies a vector by a scalar.
    /// </summary>
    /// <param name="a">left operand, the vector</param>
    /// <param name="b">right operand, the scalar</param>
    /// <returns>product</returns>
    public static Vec3 operator *(in Vec3 a, in float b)
    {
        return new(
            a.x * b,
            a.y * b,
            a.z * b);
    }

    /// <summary>
    /// Multiplies a vector by a scalar.
    /// </summary>
    /// <param name="a">left operand, the scalar</param>
    /// <param name="b">right operand, the vector</param>
    /// <returns>product</returns>
    public static Vec3 operator *(in float a, in Vec3 b)
    {
        return new(
            a * b.x,
            a * b.y,
            a * b.z);
    }

    /// <summary>
    /// Divides the left operand by the right, component-wise.
    /// </summary>
    /// <param name="a">numerator</param>
    /// <param name="b">denominator</param>
    /// <returns>quotient</returns>
    public static Vec3 operator /(in Vec3 a, in Vec3 b)
    {
        return new(
            Utils.Div(a.x, b.x),
            Utils.Div(a.y, b.y),
            Utils.Div(a.z, b.z));
    }

    /// <summary>
    /// Divides a vector by a scalar.
    /// </summary>
    /// <param name="a">vector, numerator</param>
    /// <param name="b">scalar, denominator</param>
    /// <returns>quotient</returns>
    public static Vec3 operator /(in Vec3 a, in float b)
    {
        if (b != 0.0f)
        {
            float bInv = 1.0f / b;
            return new(
                a.x * bInv,
                a.y * bInv,
                a.z * bInv);
        }
        return Vec3.Zero;
    }

    /// <summary>
    /// Divides a scalar by a vector.
    /// </summary>
    /// <param name="a">scalar, numerator</param>
    /// <param name="b">vector, denominator</param>
    /// <returns>quotient</returns>
    public static Vec3 operator /(in float a, in Vec3 b)
    {
        return new(
            Utils.Div(a, b.x),
            Utils.Div(a, b.y),
            Utils.Div(a, b.z));
    }

    /// <summary>
    /// Applies truncation-based modulo to the left and right vectors.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>result</returns>
    public static Vec3 operator %(in Vec3 a, in Vec3 b)
    {
        return Vec3.RemTrunc(a, b);
    }

    /// <summary>
    /// Applies truncation-based modulo to the left and right operands.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>result</returns>
    public static Vec3 operator %(in Vec3 a, in float b)
    {
        if (b != 0.0f)
        {
            return new(
                a.x % b,
                a.y % b,
                a.z % b);
        }
        return a;
    }

    /// <summary>
    /// Applies truncation-based modulo to the left and right operands.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>result</returns>
    public static Vec3 operator %(in float a, in Vec3 b)
    {
        return new(
            Utils.RemTrunc(a, b.x),
            Utils.RemTrunc(a, b.y),
            Utils.RemTrunc(a, b.z));
    }

    /// <summary>
    /// Adds two vectors together.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>sum</returns>
    public static Vec3 operator +(in Vec3 a, in Vec3 b)
    {
        return new(
            a.x + b.x,
            a.y + b.y,
            a.z + b.z);
    }

    /// <summary>
    /// Subtracts the right vector from the left vector.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>difference</returns>
    public static Vec3 operator -(in Vec3 a, in Vec3 b)
    {
        return new(
            a.x - b.x,
            a.y - b.y,
            a.z - b.z);
    }

    /// <summary>
    /// Evaluates whether the left comparisand is less than the right
    /// comparisand.
    ///
    /// The return type is not a boolean, but a vector, where 1.0 is true and
    /// 0.0 is false.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static Vec3 operator <(in Vec3 a, in Vec3 b)
    {
        return new(
            a.x < b.x,
            a.y < b.y,
            a.z < b.z);
    }

    /// <summary>
    /// Evaluates whether the left comparisand is greater than the right
    /// comparisand.
    ///
    /// The return type is not a boolean, but a vector, where 1.0 is true and
    /// 0.0 is false.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static Vec3 operator >(in Vec3 a, in Vec3 b)
    {
        return new(
            a.x > b.x,
            a.y > b.y,
            a.z > b.z);
    }

    /// <summary>
    /// Evaluates whether the left comparisand is less than or equal to the
    /// right comparisand.
    ///
    /// The return type is not a boolean, but a vector, where 1.0 is true and
    /// 0.0 is false.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static Vec3 operator <=(in Vec3 a, in Vec3 b)
    {
        return new(
            a.x <= b.x,
            a.y <= b.y,
            a.z <= b.z);
    }

    /// <summary>
    /// Evaluates whether the left comparisand is greater than or equal to the
    /// right comparisand.
    ///
    /// The return type is not a boolean, but a vector, where 1.0 is true and
    /// 0.0 is false.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static Vec3 operator >=(in Vec3 a, in Vec3 b)
    {
        return new(
            a.x >= b.x,
            a.y >= b.y,
            a.z >= b.z);
    }

    /// <summary>
    /// Evaluates whether two vectors are not equal to each other.
    ///
    /// The return type is not a boolean, but a vector, where 1.0 is true and
    /// 0.0 is false.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static Vec3 operator !=(in Vec3 a, in Vec3 b)
    {
        return new(
            a.x != b.x,
            a.y != b.y,
            a.z != b.z);
    }

    /// <summary>
    /// Evaluates whether two vectors are equal to each other.
    ///
    /// The return type is not a boolean, but a vector, where 1.0 is true and
    /// 0.0 is false.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static Vec3 operator ==(in Vec3 a, in Vec3 b)
    {
        return new(
            a.x == b.x,
            a.y == b.y,
            a.z == b.z);
    }

    /// <summary>
    /// Finds the absolute value of each vector component.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>absolute vector</returns>
    public static Vec3 Abs(in Vec3 v)
    {
        return new(
            MathF.Abs(v.x),
            MathF.Abs(v.y),
            MathF.Abs(v.z));
    }

    /// <summary>
    /// Tests to see if all the vector's components are non-zero. Useful when
    /// testing valid dimensions (width and depth) stored in vectors.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>evaluation</returns>
    public static bool All(in Vec3 v)
    {
        return v.x != 0.0f && v.y != 0.0f && v.z != 0.0f;
    }

    /// <summary>
    /// Finds the angle between two vectors.
    /// </summary>
    /// <param name="a">the first vector</param>
    /// <param name="b">the second vector</param>
    /// <returns>angle</returns>
    public static float AngleBetween(in Vec3 a, in Vec3 b)
    {
        // Double precision is required for accurate angle distance.
        if (Vec3.Any(a) && Vec3.Any(b))
        {
            double ax = a.x;
            double ay = a.y;
            double az = a.z;

            double bx = b.x;
            double by = b.y;
            double bz = b.z;

            return (float)Math.Acos(
                (ax * bx + ay * by + az * bz) /
                (Math.Sqrt(ax * ax + ay * ay + az * az) *
                    Math.Sqrt(bx * bx + by * by + bz * bz)));
        }
        return 0.0f;
    }

    /// <summary>
    /// Tests to see if any of the vector's components are non-zero.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>evaluation</returns>
    public static bool Any(in Vec3 v)
    {
        return v.x != 0.0f || v.y != 0.0f || v.z != 0.0f;
    }

    /// <summary>
    /// Appends a vector to a one-dimensional vector array. Returns a new array.
    /// </summary>
    /// <param name="a">array</param>
    /// <param name="b">vector</param>
    /// <returns>array</returns>
    public static Vec3[] Append(in Vec3[] a, in Vec3 b)
    {
        bool aNull = a == null;
        if (aNull) { return new Vec3[] { b }; }
        int aLen = a.Length;
        Vec3[] result = new Vec3[aLen + 1];
        System.Array.Copy(a, 0, result, 0, aLen);
        result[aLen] = b;
        return result;
    }

    /// <summary>
    /// Tests to see if two vectors approximate each other.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <param name="tol">tolerance</param>
    /// <returns>evaluation</returns>
    public static bool Approx(
        in Vec3 a, in Vec3 b,
        in float tol = Utils.Epsilon)
    {
        return Utils.Approx(a.x, b.x, tol) &&
            Utils.Approx(a.y, b.y, tol) &&
            Utils.Approx(a.z, b.z, tol);
    }

    /// <summary>
    /// Finds the vector's azimuth in the range [-PI, PI] .
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>angle in radians</returns>
    public static float AzimuthSigned(in Vec3 v)
    {
        return MathF.Atan2(v.y, v.x);
    }

    /// <summary>
    /// Finds the vector's azimuth in the range [0.0, TAU] .
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>angle in radians</returns>
    public static float AzimuthUnsigned(in Vec3 v)
    {
        float h = Vec3.AzimuthSigned(v);
        return h < -0.0f ? h + Utils.Tau : h;
    }

    /// <summary>
    /// Returns a point on a Bezier curve described by two anchor points and two
    /// control points according to a step in [0.0, 1.0] .
    ///
    /// When the step is less than zero, returns the first anchor point. When
    /// the step is greater than one, returns the second anchor point.
    /// </summary>
    /// <param name="ap0">first anchor point</param>
    /// <param name="cp0">first control point</param>
    /// <param name="cp1">second control point</param>
    /// <param name="ap1">second anchor point</param>
    /// <param name="step">step</param>
    /// <returns>point along the curve</returns>
    public static Vec3 BezierPoint(
        in Vec3 ap0,
        in Vec3 cp0,
        in Vec3 cp1,
        in Vec3 ap1,
        in float step)
    {
        if (step <= 0.0f) { return ap0; }
        else if (step >= 1.0f) { return ap1; }

        float u = 1.0f - step;
        float tcb = step * step;
        float ucb = u * u;
        float usq3t = ucb * (step + step + step);
        float tsq3u = tcb * (u + u + u);
        ucb *= u;
        tcb *= step;

        return new(
            ap0.x * ucb +
            cp0.x * usq3t +
            cp1.x * tsq3u +
            ap1.x * tcb,

            ap0.y * ucb +
            cp0.y * usq3t +
            cp1.y * tsq3u +
            ap1.y * tcb,

            ap0.z * ucb +
            cp0.z * usq3t +
            cp1.z * tsq3u +
            ap1.z * tcb);
    }

    /// <summary>
    /// Returns a tangent on a Bezier curve described by two anchor points and
    /// two control points according to a step in [0.0, 1.0] .
    ///
    /// When the step is less than zero, returns the first anchor point
    /// subtracted from the first control point. When the step is greater than
    /// one, returns the second anchor point subtracted from the second control
    /// point.
    /// </summary>
    /// <param name="ap0">first anchor point</param>
    /// <param name="cp0">first control point</param>
    /// <param name="cp1">second control point</param>
    /// <param name="ap1">second anchor point</param>
    /// <param name="step">step</param>
    /// <returns>tangent along the curve</returns>
    public static Vec3 BezierTangent(
        in Vec3 ap0,
        in Vec3 cp0,
        in Vec3 cp1,
        in Vec3 ap1,
        in float step)
    {
        if (step <= 0.0f) { return cp0 - ap0; }
        else if (step >= 1.0f) { return ap1 - cp1; }

        float u = 1.0f - step;
        float t3 = step + step + step;
        float usq3 = u * (u + u + u);
        float tsq3 = step * t3;
        float ut6 = u * (t3 + t3);

        return new(
            (cp0.x - ap0.x) * usq3 +
            (cp1.x - cp0.x) * ut6 +
            (ap1.x - cp1.x) * tsq3,

            (cp0.y - ap0.y) * usq3 +
            (cp1.y - cp0.y) * ut6 +
            (ap1.y - cp1.y) * tsq3,

            (cp0.z - ap0.z) * usq3 +
            (cp1.z - cp0.z) * ut6 +
            (ap1.z - cp1.z) * tsq3);
    }

    /// <summary>
    /// Returns a normalized tangent on a Bezier curve.
    /// </summary>
    /// <param name="ap0">first anchor point</param>
    /// <param name="cp0">first control point</param>
    /// <param name="cp1">second control point</param>
    /// <param name="ap1">second anchor point</param>
    /// <param name="step">step</param>
    /// <returns>tangent along the curve</returns>
    public static Vec3 BezierTanUnit(
        in Vec3 ap0,
        in Vec3 cp0,
        in Vec3 cp1,
        in Vec3 ap1,
        in float step)
    {
        return Vec3.Normalize(Vec3.BezierTangent(
            ap0, cp0, cp1, ap1, step));
    }

    /// <summary>
    /// Raises each component of the vector to the nearest greater integer.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>result</returns>
    public static Vec3 Ceil(in Vec3 v)
    {
        return new(
            MathF.Ceiling(v.x),
            MathF.Ceiling(v.y),
            MathF.Ceiling(v.z));
    }

    /// <summary>
    /// Clamps a vector to a range within the lower and upper bound.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="lb">range lower bound</param>
    /// <param name="ub">range upper bound</param>
    /// <returns>clamped vector</returns>
    public static Vec3 Clamp(in Vec3 v, in float lb = 0.0f, in float ub = 1.0f)
    {
        return new(
            Utils.Clamp(v.x, lb, ub),
            Utils.Clamp(v.y, lb, ub),
            Utils.Clamp(v.z, lb, ub));
    }

    /// <summary>
    /// Concatenates two one-dimensional Vec2 arrays.
    /// </summary>
    /// <param name="a">left array</param>
    /// <param name="b">right array</param>
    /// <returns>concatenation</returns>
    public static Vec3[] Concat(in Vec3[] a, in Vec3[] b)
    {
        bool aNull = a == null;
        bool bNull = b == null;

        if (aNull && bNull) { return new Vec3[] { }; }

        if (aNull)
        {
            Vec3[] result0 = new Vec3[b.Length];
            System.Array.Copy(b, 0, result0, 0, b.Length);
            return result0;
        }

        if (bNull)
        {
            Vec3[] result1 = new Vec3[a.Length];
            System.Array.Copy(a, 0, result1, 0, a.Length);
            return result1;
        }

        int aLen = a.Length;
        int bLen = b.Length;
        Vec3[] result2 = new Vec3[aLen + bLen];
        System.Array.Copy(a, 0, result2, 0, aLen);
        System.Array.Copy(b, 0, result2, aLen, bLen);
        return result2;
    }

    /// <summary>
    /// Tests to see if the vector contains a value
    /// </summary>
    /// <param name="a">vector</param>
    /// <param name="b">value</param>
    /// <returns>evaluation</returns>
    public static bool Contains(in Vec3 a, in float b)
    {
        return Utils.Approx(a.x, b) ||
            Utils.Approx(a.y, b) ||
            Utils.Approx(a.z, b);
    }

    /// <summary>
    /// Returns the first vector with the sign of the second.
    /// Returns zero where the sign is zero.
    /// </summary>
    /// <param name="a">magnitude</param>
    /// <param name="b">sign</param>
    /// <returns>signed vector</returns>
    public static Vec3 CopySign(in Vec3 a, in Vec3 b)
    {
        return new(
            Utils.CopySign(a.x, b.x),
            Utils.CopySign(a.y, b.y),
            Utils.CopySign(a.z, b.z));
    }

    /// <summary>
    /// The cross product returns a vector perpendicular to both a and b, and
    /// therefore normal to the plane on which a and b rest. The cross product
    /// is anti-commutative, meaning a x b = - ( b x a ) . A unit vector does
    /// not necessarily result from the cross of two unit vectors.
    ///
    /// Crossed orthonormal vectors are as follows:
    ///
    /// right x forward = up,
    ///
    /// ( 1.0, 0.0, 0.0 ) x ( 0.0, 1.0, 0.0 ) = ( 0.0, 0.0, 1.0 )
    ///
    /// forward x up = right,
    ///
    /// ( 0.0, 1.0, 0.0 ) x ( 0.0, 0.0, 1.0 ) = ( 1.0, 0.0, 0.0 )
    ///
    /// up x right = forward,
    ///
    /// ( 0.0, 0.0, 1.0 ) x ( 1.0, 0.0, 0.0 ) = ( 0.0, 1.0, 0.0 )
    ///
    /// The 3D equivalent to the 2D vector's perpendicular.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>cross product</returns>
    public static Vec3 Cross(in Vec3 a, in Vec3 b)
    {
        return new(
            a.y * b.z - a.z * b.y,
            a.z * b.x - a.x * b.z,
            a.x * b.y - a.y * b.x);
    }

    /// <summary>
    /// Finds the absolute value of the difference between two vectors.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>absolute difference</returns>
    public static Vec3 Diff(in Vec3 a, in Vec3 b)
    {
        return new(
            Utils.Diff(b.x, a.x),
            Utils.Diff(b.y, a.y),
            Utils.Diff(b.z, a.z));
    }

    /// <summary>
    /// Finds the Chebyshev distance between two vectors. Forms a cube pattern
    /// when plotted.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>Chebyshev distance</returns>
    public static float DistChebyshev(in Vec3 a, in Vec3 b)
    {
        return Utils.Max(Utils.Diff(b.x, a.x),
            Utils.Diff(b.y, a.y),
            Utils.Diff(b.z, a.z));
    }

    /// <summary>
    /// Finds the Euclidean distance between two vectors. Where possible, use
    /// distance squared to avoid the computational cost of the square-root.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>Euclidean distance</returns>
    public static float DistEuclidean(in Vec3 a, in Vec3 b)
    {
        return MathF.Sqrt(Vec3.DistSq(a, b));
    }

    /// <summary>
    /// Finds the Manhattan distance between two vectors. Forms an octahdral
    /// pattern when plotted.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>Manhattan distance</returns>
    public static float DistManhattan(in Vec3 a, in Vec3 b)
    {
        return Utils.Diff(b.x, a.x) +
            Utils.Diff(b.y, a.y) +
            Utils.Diff(b.z, a.z);
    }

    /// <summary>
    /// Finds the Minkowski distance between two vectors. This is a
    /// generalization of other distance formulae. When the exponent value, c,
    /// is 1.0, the Minkowski distance equals the Manhattan distance; when it is
    /// 2.0, Minkowski equals the Euclidean distance.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <param name="c">exponent</param>
    /// <returns>Minkowski distance</returns>
    public static float DistMinkowski(in Vec3 a, in Vec3 b, in float c = 2.0f)
    {
        if (c != 0.0f)
        {
            double cd = c;
            return (float)Math.Pow(
                Math.Pow(Math.Abs(b.x - a.x), cd) +
                Math.Pow(Math.Abs(b.y - a.y), cd) +
                Math.Pow(Math.Abs(b.z - a.z), cd),
                1.0d / cd);
        }
        return 0.0f;
    }

    /// <summary>
    /// Finds the Euclidean distance squared between two vectors. Equivalent to
    /// subtracting one vector from the other, then finding the dot product of
    /// the difference with itself.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>distance squared</returns>
    public static float DistSq(in Vec3 a, in Vec3 b)
    {
        float dx = b.x - a.x;
        float dy = b.y - a.y;
        float dz = b.z - a.z;
        return dx * dx + dy * dy + dz * dz;
    }

    /// <summary>
    /// Finds the dot product of two vectors by summing the products of their
    /// corresponding components.
    ///
    /// dot ( a, b ) := a.x b.x + a.y b.y + a.z b.z
    ///
    /// The dot product of a vector with itself is equal to its magnitude
    /// squared.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>dot product</returns>
    public static float Dot(in Vec3 a, in Vec3 b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }

    /// <summary>
    /// Negates a vector's x component.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>flipped</returns>
    public static Vec3 FlipX(in Vec3 v)
    {
        return new(-v.x, v.y, v.z);
    }

    /// <summary>
    /// Negates a vector's y component.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>flipped</returns>
    public static Vec3 FlipY(in Vec3 v)
    {
        return new(v.x, -v.y, v.z);
    }

    /// <summary>
    /// Negates a vector's z component.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>flipped</returns>
    public static Vec3 FlipZ(in Vec3 v)
    {
        return new(v.x, v.y, -v.z);
    }

    /// <summary>
    /// Floors each component of the vector.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>result</returns>
    public static Vec3 Floor(in Vec3 v)
    {
        return new(
            MathF.Floor(v.x),
            MathF.Floor(v.y),
            MathF.Floor(v.z));
    }

    /// <summary>
    /// Returns the fractional portion of the vector's components.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>fraction</returns>
    public static Vec3 Fract(in Vec3 v)
    {
        return new(
            Utils.Fract(v.x),
            Utils.Fract(v.y),
            Utils.Fract(v.z));
    }

    /// <summary>
    /// Creates a vector from spherical coordinates: (1) theta, the azimuth or
    /// longitude; (2) phi, the inclination or latitude; (3) rho, the radius or
    /// magnitude. The poles will be upright in a z-up coordinate system;
    /// sideways in a y-up coordinate system.
    /// </summary>
    /// <param name="azimuth">the angle theta in radians</param>
    /// <param name="inclination">the angle phi in radians</param>
    /// <param name="radius">rho, the vector's magnitude</param>
    /// <returns>vector</returns>
    public static Vec3 FromSpherical(
        in float azimuth = 0.0f,
        in float inclination = 0.0f,
        in float radius = 1.0f)
    {
        float rsp = radius * MathF.Sin(inclination);
        return new(
            rsp * MathF.Cos(azimuth),
            rsp * MathF.Sin(azimuth),
            radius * MathF.Cos(inclination));
    }

    /// <summary>
    /// Converts a color to a direction.
    /// If the color is transparent, or its magnitude is
    /// too small, returns up.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>direction</returns>
    public static Vec3 FromColor(in Rgb c)
    {
        if (c.Alpha > 0)
        {
            float x = c.R + c.R - 1.0f;
            float y = c.G + c.G - 1.0f;
            float z = c.B + c.B - 1.0f;
            float mSq = x * x + y * y + z * z;
            if (mSq > 0.0f)
            {
                float mInv = Utils.InvSqrtUnchecked(mSq);
                return new(x * mInv, y * mInv, z * mInv);
            }
        }
        return Vec3.Up;
    }

    /// <summary>
    /// Generates a 3D array of vectors.
    /// </summary>
    /// <returns>array</returns>
    public static Vec3[,,] Grid()
    {
        return Vec3.GridCartesian(
            new(-1.0f, -1.0f, -1.0f),
            new(1.0f, 1.0f, 1.0f));
    }

    /// <summary>
    /// Generates a 3D array of vectors representing a Cartesian Grid.
    /// </summary>
    /// <param name="lowerBound">lower bound</param>
    /// <param name="upperBound">upper bound</param>
    /// <param name="cols">number of columns</param>
    /// <param name="rows">number of rows</param>
    /// <param name="layers">number of layers</param>
    /// <returns>array</returns>
    public static Vec3[,,] GridCartesian(
        in Vec3 lowerBound,
        in Vec3 upperBound,
        in int cols = 8,
        in int rows = 8,
        in int layers = 8)
    {
        int lVrf = layers < 1 ? 1 : layers;
        int rVrf = rows < 1 ? 1 : rows;
        int cVrf = cols < 1 ? 1 : cols;

        bool oneLayer = lVrf == 1;
        bool oneRow = rVrf == 1;
        bool oneCol = cVrf == 1;

        float hToStep = oneLayer ? 0.0f : 1.0f / (lVrf - 1.0f);
        float iToStep = oneRow ? 0.0f : 1.0f / (rVrf - 1.0f);
        float jToStep = oneCol ? 0.0f : 1.0f / (cVrf - 1.0f);

        float hOff = oneLayer ? 0.5f : 0.0f;
        float iOff = oneRow ? 0.5f : 0.0f;
        float jOff = oneCol ? 0.5f : 0.0f;

        float lbx = lowerBound.x;
        float lby = lowerBound.y;
        float lbz = lowerBound.z;

        float ubx = upperBound.x;
        float uby = upperBound.y;
        float ubz = upperBound.z;

        Vec3[,,] result = new Vec3[lVrf, rVrf, cVrf];

        int rcVrf = rVrf * cVrf;
        int len3 = lVrf * rcVrf;
        for (int k = 0; k < len3; ++k)
        {
            int h = k / rcVrf;
            int m = k - h * rcVrf;
            int i = m / cVrf;
            int j = m % cVrf;

            float jFac = j * jToStep + jOff;
            float iFac = i * iToStep + iOff;
            float hFac = h * hToStep + hOff;

            result[h, i, j] = new(
                (1.0f - jFac) * lbx + jFac * ubx,
                (1.0f - iFac) * lby + iFac * uby,
                (1.0f - hFac) * lbz + hFac * ubz);
        }

        return result;
    }

    /// <summary>
    /// Finds the vector's signed inclination.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>signed inclination</returns>
    public static float InclinationSigned(in Vec3 v)
    {
        return Utils.HalfPi - Vec3.InclinationUnsigned(v);
    }

    /// <summary>
    /// Finds the vector's unsigned inclination.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>unsigned inclination</returns>
    public static float InclinationUnsigned(in Vec3 v)
    {
        float mSq = Vec3.MagSq(v);
        return (mSq > 0.0f) ? MathF.Acos(v.z / MathF.Sqrt(mSq)) : Utils.HalfPi;
    }

    /// <summary>
    /// Tests to see if the vector is on the unit circle, i.e., has a magnitude
    /// of approximately 1.0 .
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>evaluation</returns>
    public static bool IsUnit(in Vec3 v)
    {
        return Utils.Approx(Vec3.MagSq(v), 1.0f);
    }

    /// <summary>
    /// Limits a vector's magnitude to a scalar. Does nothing if the vector is
    /// beneath the limit.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="limit">limit</param>
    /// <returns>limited vector</returns>
    public static Vec3 Limit(in Vec3 v, in float limit)
    {
        float mSq = Vec3.MagSq(v);
        if (mSq > (limit * limit))
        {
            return Utils.Div(limit, MathF.Sqrt(mSq)) * v;
        }
        return v;
    }

    /// <summary>
    /// Generates a clamped linear step for an input factor; to be used in
    /// conjunction with a mixing function.
    /// </summary>
    /// <param name="edge0">left edge</param>
    /// <param name="edge1">right edge</param>
    /// <param name="x">factor</param>
    /// <returns>linear step</returns>
    public static Vec3 LinearStep(in Vec3 edge0, in Vec3 edge1, in Vec3 x)
    {
        return new(
            Utils.LinearStep(edge0.x, edge1.x, x.x),
            Utils.LinearStep(edge0.y, edge1.y, x.y),
            Utils.LinearStep(edge0.z, edge1.z, x.z));
    }

    /// <summary>
    /// Finds the length, or magnitude, of a vector. Also referred to as the
    /// radius when using polar coordinates. Uses the formula sqrt ( dot ( a,
    /// a)) Where possible, use magSq or dot to avoid the computational cost of
    /// the square-root.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>magnitude</returns>
    public static float Mag(in Vec3 v)
    {
        return MathF.Sqrt(Vec3.MagSq(v));
    }

    /// <summary>
    /// Finds the length-, or magnitude-, squared of a vector. Returns the same
    /// result as dot ( a, a ) . Useful when calculating the lengths of many
    /// vectors, so as to avoid the computational cost of the square-root.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>magnitude squared</returns>
    public static float MagSq(in Vec3 v)
    {
        return v.x * v.x +
            v.y * v.y +
            v.z * v.z;
    }

    /// <summary>
    /// Finds the maximum of two vectors by component.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>maximum value</returns>
    public static Vec3 Max(in Vec3 a, in Vec3 b)
    {
        return new(
            MathF.Max(a.x, b.x),
            MathF.Max(a.y, b.y),
            MathF.Max(a.z, b.z));
    }

    /// <summary>
    /// Finds the minimum of two vectors by component.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>minimum value</returns>
    public static Vec3 Min(in Vec3 a, in Vec3 b)
    {
        return new(
            MathF.Min(a.x, b.x),
            MathF.Min(a.y, b.y),
            MathF.Min(a.z, b.z));
    }

    /// <summary>
    /// Mixes two vectors together. Adds the vectors then divides by half.
    /// </summary>
    /// <param name="o">origin vector</param>
    /// <param name="d">destination vector</param>
    /// <returns>mix</returns>
    public static Vec3 Mix(in Vec3 o, in Vec3 d)
    {
        return new(
            0.5f * (o.x + d.x),
            0.5f * (o.y + d.y),
            0.5f * (o.z + d.z));
    }

    /// <summary>
    /// Mixes two vectors together by a step. Uses the formula (1.0 - t) a + t b
    /// . The step is unclamped.
    /// </summary>
    /// <param name="o">origin vector</param>
    /// <param name="d">destination vector</param>
    /// <param name="t">step</param>
    /// <returns>mix</returns>
    public static Vec3 Mix(in Vec3 o, in Vec3 d, in float t)
    {
        float u = 1.0f - t;
        return new(
            u * o.x + t * d.x,
            u * o.y + t * d.y,
            u * o.z + t * d.z);
    }

    /// <summary>
    /// Mixes two vectors together by a step. Uses the formula (1.0 - t) a + t b
    /// . The step is unclamped; to find an appropriate clamped step, use mix in
    /// conjunction with step, linearstep or smoothstep.
    /// </summary>
    /// <param name="o">origin vector</param>
    /// <param name="d">destination vector</param>
    /// <param name="t">step</param>
    /// <returns>mix</returns>
    public static Vec3 Mix(in Vec3 o, in Vec3 d, in Vec3 t)
    {
        return new(
            Utils.Mix(o.x, d.x, t.x),
            Utils.Mix(o.y, d.y, t.y),
            Utils.Mix(o.z, d.z, t.z));
    }

    /// <summary>
    /// Tests to see if all the vector's components are zero. Useful when
    /// safeguarding against invalid directions.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>evaluation</returns>
    public static bool None(in Vec3 v)
    {
        return v.x == 0.0f && v.y == 0.0f && v.z == 0.0f;
    }

    /// <summary>
    /// Divides a vector by its magnitude, such that the new magnitude is 1.0 .
    /// The result is a unit vector, as it lies on the circumference of a unit
    /// circle.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>unit vector</returns>
    public static Vec3 Normalize(in Vec3 v)
    {
        float mSq = v.x * v.x + v.y * v.y + v.z * v.z;
        if (mSq > 0.0f)
        {
            float mInv = 1.0f / MathF.Sqrt(mSq);
            return new(v.x * mInv, v.y * mInv, v.z * mInv);
        }
        return Vec3.Zero;
    }

    /// <summary>
    /// Evaluates a vector like a boolean, where n != 0.0 is true.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>truth table opposite</returns>
    public static Vec3 Not(in Vec3 v)
    {
        return new(
            v.x != 0.0f ? 0.0f : 1.0f,
            v.y != 0.0f ? 0.0f : 1.0f,
            v.z != 0.0f ? 0.0f : 1.0f);
    }

    /// <summary>
    /// Returns the scalar projection of a onto b.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>scalar projection</returns>
    public static float ProjectScalar(in Vec3 a, in Vec3 b)
    {
        float bSq = Vec3.MagSq(b);
        if (bSq != 0.0f) { return Vec3.Dot(a, b) / bSq; }
        return 0.0f;
    }

    /// <summary>
    /// Projects one vector onto another. Defined as
    ///
    /// proj ( a , b ) := b ( dot( a, b ) / dot ( b, b ) )
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>vector projection</returns>
    public static Vec3 ProjectVector(in Vec3 a, in Vec3 b)
    {
        return b * Vec3.ProjectScalar(a, b);
    }

    /// <summary>
    /// Promotes a 2D vector to a 3D vector.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="z">z component</param>
    /// <returns>vector</returns>
    public static Vec3 Promote(in Vec2 v, in float z = 0.0f)
    {
        return new(v.X, v.Y, z);
    }

    /// <summary>
    /// Reduces the signal, or granularity, of a vector's components.
    /// </summary>
    /// <param name="v">input vector</param>
    /// <param name="levels">levels</param>
    /// <returns>quantized vector</returns>
    public static Vec3 Quantize(in Vec3 v, in int levels = 8)
    {
        return new(
            Utils.QuantizeSigned(v.x, levels),
            Utils.QuantizeSigned(v.y, levels),
            Utils.QuantizeSigned(v.z, levels));
    }

    /// <summary>
    /// Creates a random vector.
    /// </summary>
    /// <param name="rng">random number generator</param>
    /// <returns>random vector</returns>
    public static Vec3 Random(in System.Random rng)
    {
        return Vec3.RandomSpherical(rng);
    }

    /// <summary>
    /// Creates a random point in the Cartesian coordinate system given a lower
    /// and an upper bound.
    /// </summary>
    /// <param name="rng">random number generator</param>
    /// <param name="lb">lower bound</param>
    /// <param name="ub">upper bound</param>
    /// <returns>random vector</returns>
    public static Vec3 RandomCartesian(
        in System.Random rng,
        in Vec3 lb,
        in Vec3 ub)
    {
        return new(
            Utils.Mix(lb.x, ub.x, (float)rng.NextDouble()),
            Utils.Mix(lb.y, ub.y, (float)rng.NextDouble()),
            Utils.Mix(lb.z, ub.z, (float)rng.NextDouble()));
    }

    /// <summary>
    /// Creates a random point in the Cartesian coordinate system
    /// given a lower and an upper bound.
    /// </summary>
    /// <param name="rng">random number generator</param>
    /// <param name="lb">lower bound</param>
    /// <param name="ub">upper bound</param>
    /// <returns>random vector</returns>
    public static Vec3 RandomCartesian(
        in System.Random rng,
        in float lb = 0.0f,
        in float ub = 1.0f)
    {
        return new(
            Utils.Mix(lb, ub, (float)rng.NextDouble()),
            Utils.Mix(lb, ub, (float)rng.NextDouble()),
            Utils.Mix(lb, ub, (float)rng.NextDouble()));
    }

    /// <summary>
    /// Creates a vector that lies on a sphere.
    /// Finds three random numbers with normal distribution,
    /// then normalizes them.
    /// </summary>
    /// <param name="rng">random number generator</param>
    /// <param name="radius">radius</param>
    /// <returns>random vector</returns>
    public static Vec3 RandomSpherical(
        in System.Random rng,
        in float radius = 1.0f)
    {
        float x = Utils.NextGaussian(rng);
        float y = Utils.NextGaussian(rng);
        float z = Utils.NextGaussian(rng);
        float mSq = x * x + y * y + z * z;
        if (mSq != 0.0f)
        {
            float scalar = radius / MathF.Sqrt(mSq);
            return new(x * scalar, y * scalar, z * scalar);
        }
        return Vec3.Zero;
    }

    /// <summary>
    /// Reflects an incident vector off a normal vector. Uses the formula
    ///
    /// i - 2.0 ( dot( n, i ) n )
    /// </summary>
    /// <param name="i">incident vector</param>
    /// <param name="n">normal vector</param>
    /// <returns>reflected vector</returns>
    public static Vec3 Reflect(in Vec3 i, in Vec3 n)
    {
        return i - (2.0f * Vec3.Dot(n, i)) * n;
    }

    /// <summary>
    /// Refracts a vector through a volume using Snell's law.
    /// </summary>
    /// <param name="i">incident vector</param>
    /// <param name="n">normal vector</param>
    /// <param name="eta">ratio of refraction indices</param>
    /// <returns>refracted vector</returns>
    public static Vec3 Refract(in Vec3 i, in Vec3 n, in float eta)
    {
        float iDotN = Vec3.Dot(i, n);
        float k = 1.0f - eta * eta * (1.0f - iDotN * iDotN);
        if (k < 0.0f) { return Vec3.Zero; }
        return (eta * i) - (n * (eta * iDotN + MathF.Sqrt(k)));
    }

    /// <summary>
    /// Maps an input vector from an original range to a target range.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="lbOrigin">lower bound of original range</param>
    /// <param name="ubOrigin">upper bound of original range</param>
    /// <param name="lbDest">lower bound of destination range</param>
    /// <param name="ubDest">upper bound of destination range</param>
    /// <returns>mapped value</returns>
    public static Vec3 Remap(
        in Vec3 v,
        in Vec3 lbOrigin,
        in Vec3 ubOrigin,
        in Vec3 lbDest,
        in Vec3 ubDest)
    {
        return new(
            Utils.Remap(v.x, lbOrigin.x, ubOrigin.x, lbDest.x, ubDest.x),
            Utils.Remap(v.y, lbOrigin.y, ubOrigin.y, lbDest.y, ubDest.y),
            Utils.Remap(v.z, lbOrigin.z, ubOrigin.z, lbDest.z, ubDest.z));
    }

    /// <summary>
    /// Mods each component of the left vector by those of the right.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>result</returns>
    public static Vec3 RemFloor(in Vec3 a, in Vec3 b)
    {
        return new(
            Utils.RemFloor(a.x, b.x),
            Utils.RemFloor(a.y, b.y),
            Utils.RemFloor(a.z, b.z));
    }

    /// <summary>
    /// Applies the % operator (truncation-based modulo) to the left operand.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>result</returns>
    public static Vec3 RemTrunc(in Vec3 a, in Vec3 b)
    {
        return new(
            Utils.RemTrunc(a.x, b.x),
            Utils.RemTrunc(a.y, b.y),
            Utils.RemTrunc(a.z, b.z));
    }

    /// <summary>
    /// Normalizes a vector, then multiplies it by a scalar, in effect setting
    /// its magnitude to that scalar.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="scalar">scalar</param>
    /// <returns>rescaled vector</returns>
    public static Vec3 Rescale(in Vec3 v, in float scalar)
    {
        return v * Utils.Div(scalar, Vec3.Mag(v));
    }

    /// <summary>
    /// Resizes an array of vectors to a requested length.
    /// </summary>
    /// <param name="arr">array</param>
    /// <param name="sz">new size</param>
    /// <returns>resized array</returns>
    public static Vec3[] Resize(in Vec3[] arr, in int sz)
    {
        if (sz < 1) { return new Vec3[] { }; }
        Vec3[] result = new Vec3[sz];

        if (arr != null)
        {
            int len = arr.Length;
            int end = sz > len ? len : sz;
            System.Array.Copy(arr, result, end);
        }

        return result;
    }

    /// <summary>
    /// Rotates a vector around an arbitrary axis by using an angle in radians.
    ///
    /// The axis is assumed to have already been normalized prior to being given
    /// to the function.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="radians">the angle in radians</param>
    /// <param name="axis">the axis</param>
    /// <returns>rotated vector</returns>
    public static Vec3 Rotate(in Vec3 v, in float radians, in Vec3 axis)
    {
        return Vec3.Rotate(v, MathF.Cos(radians), MathF.Sin(radians), axis);
    }

    /// <summary>
    /// Rotates a vector around the an arbirary axis.
    ///
    /// Accepts pre-calculated sine and cosine of an angle, so that collections
    /// of vectors can be efficiently rotated without repeatedly calling cos and
    /// sin.
    ///
    /// The axis is assumed to have already been normalized prior to being given
    /// to the function.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="cosa">the cosine of the angle</param>
    /// <param name="sina">the sine of the angle</param>
    /// <param name="axis">the axis</param>
    /// <returns>rotated vector</returns>
    public static Vec3 Rotate(in Vec3 v, in float cosa, in float sina, in Vec3 axis)
    {
        float complcos = 1.0f - cosa;
        float complxy = complcos * axis.x * axis.y;
        float complxz = complcos * axis.x * axis.z;
        float complyz = complcos * axis.y * axis.z;

        float sinx = sina * axis.x;
        float siny = sina * axis.y;
        float sinz = sina * axis.z;

        return new(
            (complcos * axis.x * axis.x + cosa) * v.x +
            (complxy - sinz) * v.y +
            (complxz + siny) * v.z,

            (complxy + sinz) * v.x +
            (complcos * axis.y * axis.y + cosa) * v.y +
            (complyz - sinx) * v.z,

            (complxz - siny) * v.x +
            (complyz + sinx) * v.y +
            (complcos * axis.z * axis.z + cosa) * v.z);
    }

    /// <summary>
    /// Rotates a vector around the x axis by using an angle in radians.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="radians">the angle in radians</param>
    /// <returns>rotated vector</returns>
    public static Vec3 RotateX(in Vec3 v, in float radians)
    {
        return Vec3.RotateX(v, MathF.Cos(radians), MathF.Sin(radians));
    }

    /// <summary>
    /// Rotates a vector around the x axis.
    ///
    /// Accepts pre-calculated sine and cosine of an angle, so that collections
    /// of vectors can be efficiently rotated without repeatedly calling cos and
    /// sin.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="cosa">the cosine of the angle</param>
    /// <param name="sina">the sine of the angle</param>
    /// <returns>rotated vector</returns>
    public static Vec3 RotateX(in Vec3 v, in float cosa, in float sina)
    {
        return new(
            v.x,
            cosa * v.y - sina * v.z,
            cosa * v.z + sina * v.y);
    }

    /// <summary>
    /// Rotates a vector around the y axis by using an angle in radians.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="radians">the angle in radians</param>
    /// <returns>rotated vector</returns>
    public static Vec3 RotateY(in Vec3 v, in float radians)
    {
        return Vec3.RotateY(v, MathF.Cos(radians), MathF.Sin(radians));
    }

    /// <summary>
    /// Rotates a vector around the y axis.
    ///
    /// Accepts pre-calculated sine and cosine of an angle, so that collections
    /// of vectors can be efficiently rotated without repeatedly calling cos and
    /// sin.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="cosa">the cosine of the angle</param>
    /// <param name="sina">the sine of the angle</param>
    /// <returns>rotated vector</returns>
    public static Vec3 RotateY(in Vec3 v, in float cosa, in float sina)
    {
        return new(
            cosa * v.x + sina * v.z,
            v.y,
            cosa * v.z - sina * v.x);
    }

    /// <summary>
    /// Rotates a vector around the z axis by using an angle in radians.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="radians">the angle in radians</param>
    /// <returns>rotated vector</returns>
    public static Vec3 RotateZ(in Vec3 v, in float radians)
    {
        return Vec3.RotateZ(v, MathF.Cos(radians), MathF.Sin(radians));
    }

    /// <summary>
    /// Rotates a vector around the z axis.
    ///
    /// Accepts pre-calculated sine and cosine of an angle, so that collections
    /// of vectors can be efficiently rotated without repeatedly calling cos and
    /// sin.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="cosa">the cosine of the angle</param>
    /// <param name="sina">the sine of the angle</param>
    /// <returns>rotated vector</returns>
    public static Vec3 RotateZ(in Vec3 v, in float cosa, in float sina)
    {
        return new(
            cosa * v.x - sina * v.y,
            cosa * v.y + sina * v.x,
            v.z);
    }

    /// <summary>
    /// Rounds each component of the vector to the nearest whole number.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>rounded vector</returns>
    public static Vec3 Round(in Vec3 v)
    {
        return new(
            Utils.Round(v.x),
            Utils.Round(v.y),
            Utils.Round(v.z));
    }

    /// <summary>
    /// Finds the sign of the vector: -1, if negative; 1, if positive.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>sign</returns>
    public static Vec3 Sign(in Vec3 v)
    {
        return new(
            MathF.Sign(v.x),
            MathF.Sign(v.y),
            MathF.Sign(v.z));
    }

    /// <summary>
    /// Mixes two vectors together with spherical linear interpolation
    /// by a factor.
    /// </summary>
    /// <param name="o">origin</param>
    /// <param name="d">destination</param>
    /// <param name="t">factor</param>
    /// <returns>vector</returns>
    public static Vec3 Slerp(in Vec3 o, in Vec3 d, in float t = 0.5f)
    {
        double ox = o.x;
        double oy = o.y;
        double oz = o.z;

        double dx = d.x;
        double dy = d.y;
        double dz = d.z;

        double odDot = ox * dx + oy * dy + oz * dz;
        odDot = Math.Min(Math.Max(odDot, -0.999999d), 0.999999d);

        double omega = Math.Acos(odDot);
        double omSin = Math.Sin(omega);
        double omSinInv = (omSin != 0.0d) ? 1.0d / omSin : 1.0d;

        double td = t;
        double oFac = Math.Sin((1.0d - td) * omega) * omSinInv;
        double dFac = Math.Sin(td * omega) * omSinInv;

        return new(
            (float)(oFac * ox + dFac * dx),
            (float)(oFac * oy + dFac * dy),
            (float)(oFac * oz + dFac * dz));
    }

    /// <summary>
    /// Generates a clamped Hermite step for an input factor; to be used in
    /// conjunction with a mixing function.
    /// </summary>
    /// <param name="edge0">left edge</param>
    /// <param name="edge1">right edge</param>
    /// <param name="x">factor</param>
    /// <returns>smooth step</returns>
    public static Vec3 SmoothStep(in Vec3 edge0, in Vec3 edge1, in Vec3 x)
    {
        return new(
            Utils.SmoothStep(edge0.x, edge1.x, x.x),
            Utils.SmoothStep(edge0.y, edge1.y, x.y),
            Utils.SmoothStep(edge0.z, edge1.z, x.z));
    }

    /// <summary>
    /// Generates a clamped boolean step for an input factor; to be used in
    /// conjunction with a mixing function.
    /// </summary>
    /// <param name="edge">edge</param>
    /// <param name="x">factor</param>
    /// <returns>step</returns>
    public static Vec3 Step(in Vec3 edge, in Vec3 x)
    {
        return new(
            Utils.Step(edge.x, x.x),
            Utils.Step(edge.y, x.y),
            Utils.Step(edge.z, x.z));
    }

    /// <summary>
    /// Returns a float array of length 3 containing a vector's components.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>array</returns>
    public static float[] ToArray(in Vec3 v)
    {
        return Vec3.ToArray(v, new float[v.Length], 0);
    }

    /// <summary>
    /// Puts a vector's components into an array at a given index.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="arr">array</param>
    /// <param name="i">index</param>
    /// <returns>array</returns>
    public static float[] ToArray(in Vec3 v, in float[] arr, in int i = 0)
    {
        arr[i] = v.x;
        arr[i + 1] = v.y;
        arr[i + 2] = v.z;
        return arr;
    }

    /// <summary>
    /// Returns a named value tuple containing the vector's azimuth,
    /// theta; inclination, phi; and magnitude, rho.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>tuple</returns>
    public static (float theta, float phi, float rho) ToSpherical(in Vec3 v)
    {
        float mSq = Vec3.MagSq(v);
        if (mSq > 0.0)
        {
            float m = MathF.Sqrt(mSq);
            return (
                theta: MathF.Atan2(v.y, v.x),
                phi: Utils.HalfPi - MathF.Acos(v.z / m),
                rho: m);
        }
        return (theta: 0.0f, phi: 0.0f, rho: 0.0f);
    }

    /// <summary>
    /// Returns a string representation of a vector.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string</returns>
    public static string ToString(in Vec3 v, in int places = 4)
    {
        return Vec3.ToString(new StringBuilder(64), v, places).ToString();
    }

    /// <summary>
    /// Appends a representation of a vector to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="v">vector</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString(
        in StringBuilder sb,
        in Vec3 v,
        in int places = 4)
    {
        sb.Append("{\"x\":");
        Utils.ToFixed(sb, v.x, places);
        sb.Append(",\"y\":");
        Utils.ToFixed(sb, v.y, places);
        sb.Append(",\"z\":");
        Utils.ToFixed(sb, v.z, places);
        sb.Append('}');
        return sb;
    }

    /// <summary>
    /// Returns a string representation of an array of vectors.
    /// </summary>
    /// <param name="arr">array</param>
    /// <param name="places">print precision</param>
    /// <returns>string</returns>
    public static string ToString(in Vec3[] arr, in int places = 4)
    {
        return Vec3.ToString(new StringBuilder(arr.Length * 64), arr, places).ToString();
    }

    /// <summary>
    /// Appends a representation of an array of vectors to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="arr">array</param>
    /// <param name="places">print precision</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString(
        in StringBuilder sb,
        in Vec3[] arr,
        in int places = 4)
    {
        sb.Append('[');

        if (arr != null)
        {
            int len = arr.Length;
            int last = len - 1;

            for (int i = 0; i < last; ++i)
            {
                Vec3.ToString(sb, arr[i], places);
                sb.Append(',');
            }

            Vec3.ToString(sb, arr[last], places);
        }

        sb.Append(']');
        return sb;
    }

    /// <summary>
    /// Truncates each component of the vector.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>truncation</returns>
    public static Vec3 Trunc(in Vec3 v)
    {
        return new(
            MathF.Truncate(v.x),
            MathF.Truncate(v.y),
            MathF.Truncate(v.z));
    }

    /// <summary>
    /// Wraps a vector around a periodic range, as represented by a lower and
    /// upper bound. The lower bound is inclusive; the upper bound, exclusive.
    ///
    /// In cases where the lower bound is 0.0, use Mod instead.
    /// </summary>
    /// <param name="v">the vector</param>
    /// <param name="lb">lower bound</param>
    /// <param name="ub">upper bound</param>
    /// <returns>wrapped vector</returns>
    public static Vec3 Wrap(in Vec3 v, in Vec3 lb, in Vec3 ub)
    {
        return new(
            Utils.Wrap(v.x, lb.x, ub.x),
            Utils.Wrap(v.y, lb.y, ub.y),
            Utils.Wrap(v.z, lb.z, ub.z));
    }

    /// <summary>
    /// Returns a direction facing back, (0.0, -1.0, 0.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec3 Back { get { return new(0.0f, -1.0f, 0.0f); } }

    /// <summary>
    /// Returns a direction facing down, (0.0, 0.0, -1.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec3 Down { get { return new(0.0f, 0.0f, -1.0f); } }

    /// <summary>
    /// Returns a direction facing forward, (0.0, 1.0, 0.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec3 Forward { get { return new(0.0f, 1.0f, 0.0f); } }

    /// <summary>
    /// Returns a direction facing left, (-1.0, 0.0, 0.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec3 Left { get { return new(-1.0f, 0.0f, 0.0f); } }

    /// <summary>
    /// Returns a vector with all components set to 1.0 .
    /// </summary>
    /// <value>the vector</value>
    public static Vec3 One { get { return new(1.0f, 1.0f, 1.0f); } }

    /// <summary>
    /// Returns a direction facing right, (1.0, 0.0, 0.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec3 Right { get { return new(1.0f, 0.0f, 0.0f); } }

    /// <summary>
    /// Returns a direction facing up, (0.0, 0.0, 1.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec3 Up { get { return new(0.0f, 0.0f, 1.0f); } }

    /// <summary>
    /// Returns a vector with all components set to zero.
    /// </summary>
    /// <value>the vector</value>
    public static Vec3 Zero { get { return new(0.0f, 0.0f, 0.0f); } }
}