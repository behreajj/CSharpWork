using System;
using System.Collections;
using System.Text;

/// <summary>
/// A readonly struct influenced by GLSL and OSL.
/// </summary>
[Serializable]
public readonly struct Vec4 : IComparable<Vec4>, IEquatable<Vec4>, IEnumerable
{
    /// <summary>
    /// Component on the x axis.
    /// </summary>
    private readonly float x;

    /// <summary>
    /// Component on the y axis.
    /// </summary>
    private readonly float y;

    /// <summary>
    /// Component on the z axis.
    /// </summary>
    private readonly float z;

    /// <summary>
    /// Component on the z axis.
    /// </summary>
    private readonly float w;

    /// <summary>
    /// Returns the number of values (dimensions) in this vector.
    /// </summary>
    /// <value>length</value>
    public int Length { get { return 4; } }

    /// <summary>
    /// Component on the x axis.
    /// </summary>
    /// <value>x</value>
    public float X { get { return this.x; } }

    /// <summary>
    /// Component on the y axis.
    /// </summary>
    /// <value>y</value>
    public float Y { get { return this.y; } }

    /// <summary>
    /// Component on the z axis.
    /// </summary>
    /// <value>z</value>
    public float Z { get { return this.z; } }

    /// <summary>
    /// Component on the w axis.
    /// </summary>
    /// <value>w</value>
    public float W { get { return this.w; } }

    /// <summary>
    /// Gets the first three components as a 3D vector.
    /// </summary>
    /// <value>3D vector</value>
    public Vec3 XYZ
    {
        get
        {
            return new Vec3(this.x, this.y, this.z);
        }
    }

    /// <summary>
    /// Retrieves a component by index. When the provided index is 3 or -1,
    /// returns w; 2 or -2, z; 1 or -3, y; 0 or -4, x.
    /// </summary>
    /// <value>the component</value>
    public float this[int i]
    {
        get
        {
            return i switch
            {
                0 or -4 => this.x,
                1 or -3 => this.y,
                2 or -2 => this.z,
                3 or -1 => this.w,
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
    /// <param name="w">w component</param>
    public Vec4(in float x, in float y, in float z, in float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    /// <summary>
    /// Constructs a vector from boolean values, where true is 1.0 and false is
    /// 0.0 .
    /// </summary>
    /// <param name="x">x component</param>
    /// <param name="y">y component</param>
    /// <param name="z">z component</param>
    /// <param name="w">w component</param>
    public Vec4(in bool x, in bool y, in bool z, in bool w)
    {
        this.x = x ? 1.0f : 0.0f;
        this.y = y ? 1.0f : 0.0f;
        this.z = z ? 1.0f : 0.0f;
        this.w = w ? 1.0f : 0.0f;
    }

    /// <summary>
    /// Tests this vector for equivalence with an object. For approximate
    /// equality  with another vector, use the static approx function instead.
    /// </summary>
    /// <param name="value">the object</param>
    /// <returns>equivalence</returns>
    public override bool Equals(object value)
    {
        if (Object.ReferenceEquals(this, value)) { return true; }
        if (value is null) { return false; }
        if (value is Vec4 vec) { return this.Equals(vec); }
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
            return (((Utils.MulBase ^ this.x.GetHashCode()) *
                        Utils.HashMul ^ this.y.GetHashCode()) *
                    Utils.HashMul ^ this.z.GetHashCode()) *
                Utils.HashMul ^ this.w.GetHashCode();
        }
    }

    /// <summary>
    /// Returns a string representation of this vector.
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()
    {
        return Vec4.ToString(this);
    }

    /// <summary>
    /// Compares this vector to another.
    /// Returns 1 when a component of this vector is greater than
    /// another; -1 when lesser. Returns 0 as a last resort.
    /// Prioritizes the highest dimension first: w, z, y, x.
    /// </summary>
    /// <param name="v">the comparisand</param>
    /// <returns>evaluation</returns>
    public int CompareTo(Vec4 v)
    {
        return (this.w < v.w) ? -1 :
            (this.w > v.w) ? 1 :
            (this.z < v.z) ? -1 :
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
    public bool Equals(Vec4 v)
    {
        if (this.w.GetHashCode() != v.w.GetHashCode()) { return false; }
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
        yield return this.w;
    }

    /// <summary>
    /// Converts a boolean to a vector by supplying the boolean to all the
    /// vector's components: 1.0 for true; 0.0 for false.
    /// </summary>
    /// <param name="b">boolean</param>
    /// <returns>vector</returns>
    public static implicit operator Vec4(in bool b)
    {
        float eval = b ? 1.0f : 0.0f;
        return new(eval, eval, eval, eval);
    }

    /// <summary>
    /// Converts a float to a vector by supplying the scalar to all the vector's
    /// components.
    /// </summary>
    /// <param name="s">scalar</param>
    /// <returns>vector</returns>
    public static implicit operator Vec4(in float s)
    {
        return new(s, s, s, s);
    }

    /// <summary>
    /// Promotes a 2D vector to a 4D vector; the z and w components are assumed
    /// to be 0.0 .
    /// </summary>
    /// <param name="v">2D vector</param>
    /// <returns>vector</returns>
    public static implicit operator Vec4(in Vec2 v)
    {
        return Vec4.Promote(v, 0.0f, 0.0f);
    }

    /// <summary>
    /// Promotes a 3D vector to a 4D vector; the w component is assumed to be
    /// 0.0 .
    /// </summary>
    /// <param name="v">3D vector</param>
    /// <returns>vector</returns>
    public static implicit operator Vec4(in Vec3 v)
    {
        return Vec4.Promote(v, 0.0f);
    }

    /// <summary>
    /// Converts a vector to a boolean by finding whether all of its components
    /// are non-zero.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>boolean</returns>
    public static explicit operator bool(in Vec4 v)
    {
        return Vec4.All(v);
    }

    /// <summary>
    /// A vector evaluates to true when all of its components are not equal to
    /// zero.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>evaluation</returns>
    public static bool operator true(in Vec4 v)
    {
        return Vec4.All(v);
    }

    /// <summary>
    /// A vector evaluates to false when all of its components are equal to
    /// zero.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>evaluation</returns>
    public static bool operator false(in Vec4 v)
    {
        return Vec4.None(v);
    }

    /// <summary>
    /// Evaluates a vector as a boolean. Equivalent to using the ! operator.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>opposite</returns>
    public static Vec4 operator !(in Vec4 v)
    {
        return Vec4.Not(v);
    }

    /// <summary>
    /// Evaluates a vector as a boolean. Equivalent to using the ~ operator.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>complement</returns>
    public static Vec4 operator ~(in Vec4 v)
    {
        return Vec4.Not(v);
    }

    /// <summary>
    /// Evaluates two vectors like booleans, using the inclusive and (AND) logic
    /// gate.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>evaluation</returns>
    public static Vec4 operator &(in Vec4 a, in Vec4 b)
    {
        return new(
            Utils.And(a.x, b.x),
            Utils.And(a.y, b.y),
            Utils.And(a.z, b.z),
            Utils.And(a.w, b.w));
    }

    /// <summary>
    /// Evaluates two vectors like booleans, using the inclusive or (OR) logic
    /// gate.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>evaluation</returns>
    public static Vec4 operator |(in Vec4 a, in Vec4 b)
    {
        return new(
            Utils.Or(a.x, b.x),
            Utils.Or(a.y, b.y),
            Utils.Or(a.z, b.z),
            Utils.Or(a.w, b.w));
    }

    /// <summary>
    /// Evaluates two vectors like booleans, using the exclusive or (XOR) logic
    /// gate.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>evaluation</returns>
    public static Vec4 operator ^(in Vec4 a, in Vec4 b)
    {
        return new(
            Utils.Xor(a.x, b.x),
            Utils.Xor(a.y, b.y),
            Utils.Xor(a.z, b.z),
            Utils.Xor(a.w, b.w));
    }

    /// <summary>
    /// Negates the vector.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>negation</returns>
    public static Vec4 operator -(in Vec4 v)
    {
        return new(-v.x, -v.y, -v.z, -v.w);
    }

    /// <summary>
    /// Increments all components of a vector by 1.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>increment</returns>
    public static Vec4 operator ++(in Vec4 v)
    {
        return new(v.x + 1.0f, v.y + 1.0f, v.z + 1.0f, v.w + 1.0f);
    }

    /// <summary>
    /// Decrements all components of a vector by 1.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>decrement</returns>
    public static Vec4 operator --(in Vec4 v)
    {
        return new(v.x - 1.0f, v.y - 1.0f, v.z - 1.0f, v.w - 1.0f);
    }

    /// <summary>
    /// Multiplies two vectors, component-wise, i.e.,
    /// returns the Hadamard product.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>product</returns>
    public static Vec4 operator *(in Vec4 a, in Vec4 b)
    {
        return new(
            a.x * b.x,
            a.y * b.y,
            a.z * b.z,
            a.w * b.w);
    }

    /// <summary>
    /// Multiplies a vector by a scalar.
    /// </summary>
    /// <param name="a">left operand, the vector</param>
    /// <param name="b">right operand, the scalar</param>
    /// <returns>product</returns>
    public static Vec4 operator *(in Vec4 a, in float b)
    {
        return new(
            a.x * b,
            a.y * b,
            a.z * b,
            a.w * b);
    }

    /// <summary>
    /// Multiplies a vector by a scalar.
    /// </summary>
    /// <param name="a">left operand, the scalar</param>
    /// <param name="b">right operand, the vector</param>
    /// <returns>product</returns>
    public static Vec4 operator *(in float a, in Vec4 b)
    {
        return new(
            a * b.x,
            a * b.y,
            a * b.z,
            a * b.w);
    }

    /// <summary>
    /// Divides the left operand by the right, component-wise.
    /// </summary>
    /// <param name="a">numerator</param>
    /// <param name="b">denominator</param>
    /// <returns>quotient</returns>
    public static Vec4 operator /(in Vec4 a, in Vec4 b)
    {
        return new(
            Utils.Div(a.x, b.x),
            Utils.Div(a.y, b.y),
            Utils.Div(a.z, b.z),
            Utils.Div(a.w, b.w));
    }

    /// <summary>
    /// Divides a vector by a scalar.
    /// </summary>
    /// <param name="a">vector, numerator</param>
    /// <param name="b">scalar, denominator</param>
    /// <returns>quotient</returns>
    public static Vec4 operator /(in Vec4 a, in float b)
    {
        if (b != 0.0f)
        {
            float bInv = 1.0f / b;
            return new(
                a.x * bInv,
                a.y * bInv,
                a.z * bInv,
                a.w * bInv);
        }
        return Vec4.Zero;
    }

    /// <summary>
    /// Divides a scalar by a vector.
    /// </summary>
    /// <param name="a">scalar, numerator</param>
    /// <param name="b">vector, denominator</param>
    /// <returns>quotient</returns>
    public static Vec4 operator /(in float a, in Vec4 b)
    {
        return new(
            Utils.Div(a, b.x),
            Utils.Div(a, b.y),
            Utils.Div(a, b.z),
            Utils.Div(a, b.w));
    }

    /// <summary>
    /// Applies truncation-based modulo to the left and right vectors.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>result</returns>
    public static Vec4 operator %(in Vec4 a, in Vec4 b)
    {
        return Vec4.RemTrunc(a, b);
    }

    /// <summary>
    /// Applies truncation-based modulo to the left and right operands.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>result</returns>
    public static Vec4 operator %(in Vec4 a, in float b)
    {
        if (b != 0.0f)
        {
            return new(
                a.x % b,
                a.y % b,
                a.z % b,
                a.w % b);
        }
        return a;
    }

    /// <summary>
    /// Applies truncation-based modulo to the left and right operands.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>result</returns>
    public static Vec4 operator %(in float a, in Vec4 b)
    {
        return new(
            Utils.RemTrunc(a, b.x),
            Utils.RemTrunc(a, b.y),
            Utils.RemTrunc(a, b.z),
            Utils.RemTrunc(a, b.w));
    }

    /// <summary>
    /// Adds two vectors together.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>sum</returns>
    public static Vec4 operator +(in Vec4 a, in Vec4 b)
    {
        return new(
            a.x + b.x,
            a.y + b.y,
            a.z + b.z,
            a.w + b.w);
    }

    /// <summary>
    /// Subtracts the right vector from the left vector.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>difference</returns>
    public static Vec4 operator -(in Vec4 a, in Vec4 b)
    {
        return new(
            a.x - b.x,
            a.y - b.y,
            a.z - b.z,
            a.w - b.w);
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
    public static Vec4 operator <(in Vec4 a, in Vec4 b)
    {
        return new(
            a.x < b.x,
            a.y < b.y,
            a.z < b.z,
            a.w < b.w);
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
    public static Vec4 operator >(in Vec4 a, in Vec4 b)
    {
        return new(
            a.x > b.x,
            a.y > b.y,
            a.z > b.z,
            a.w > b.w);
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
    public static Vec4 operator <=(in Vec4 a, in Vec4 b)
    {
        return new(
            a.x <= b.x,
            a.y <= b.y,
            a.z <= b.z,
            a.w <= b.w);
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
    public static Vec4 operator >=(in Vec4 a, in Vec4 b)
    {
        return new(
            a.x >= b.x,
            a.y >= b.y,
            a.z >= b.z,
            a.w >= b.w);
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
    public static Vec4 operator !=(in Vec4 a, in Vec4 b)
    {
        return new(
            a.x != b.x,
            a.y != b.y,
            a.z != b.z,
            a.w != b.w);
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
    public static Vec4 operator ==(in Vec4 a, in Vec4 b)
    {
        return new(
            a.x == b.x,
            a.y == b.y,
            a.z == b.z,
            a.w == b.w);
    }

    /// <summary>
    /// Finds the absolute value of each vector component.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>absolute vector</returns>
    public static Vec4 Abs(in Vec4 v)
    {
        return new(
            MathF.Abs(v.x),
            MathF.Abs(v.y),
            MathF.Abs(v.z),
            MathF.Abs(v.w));
    }

    /// <summary>
    /// Tests to see if all the vector's components are non-zero. Useful when
    /// testing valid dimensions (width and depth) stored in vectors.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>evaluation</returns>
    public static bool All(in Vec4 v)
    {
        return v.x != 0.0f &&
            v.y != 0.0f &&
            v.z != 0.0f &&
            v.w != 0.0f;
    }

    /// <summary>
    /// Tests to see if any of the vector's components are non-zero.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>evaluation</returns>
    public static bool Any(in Vec4 v)
    {
        return v.x != 0.0f ||
            v.y != 0.0f ||
            v.z != 0.0f ||
            v.w != 0.0f;
    }

    /// <summary>
    /// Tests to see if two vectors approximate each other.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <param name="tol">tolerance</param>
    /// <returns>evaluation</returns>
    public static bool Approx(
        in Vec4 a, in Vec4 b,
        in float tol = Utils.Epsilon)
    {
        return Utils.Approx(a.x, b.x, tol) &&
            Utils.Approx(a.y, b.y, tol) &&
            Utils.Approx(a.z, b.z, tol) &&
            Utils.Approx(a.w, b.w, tol);
    }

    /// <summary>
    /// Returns a point on a Bezier curve described by two anchor points and two
    /// control points according to a step in [0.0, 1.0] .
    ///
    /// When the step is less than zero, returns the first anchor point. When the
    /// step is greater than one, returns the second anchor point.
    /// </summary>
    /// <param name="ap0">first anchor point</param>
    /// <param name="cp0">first control point</param>
    /// <param name="cp1">second control point</param>
    /// <param name="ap1">second anchor point</param>
    /// <param name="step">step</param>
    /// <returns>point along the curve</returns>
    public static Vec4 BezierPoint(
        in Vec4 ap0,
        in Vec4 cp0,
        in Vec4 cp1,
        in Vec4 ap1,
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
            ap1.z * tcb,

            ap0.w * ucb +
            cp0.w * usq3t +
            cp1.w * tsq3u +
            ap1.w * tcb);
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
    public static Vec4 BezierTangent(
        in Vec4 ap0,
        in Vec4 cp0,
        in Vec4 cp1,
        in Vec4 ap1,
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
            (ap1.z - cp1.z) * tsq3,

            (cp0.w - ap0.w) * usq3 +
            (cp1.w - cp0.w) * ut6 +
            (ap1.w - cp1.w) * tsq3);
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
    public static Vec4 BezierTanUnit(
        in Vec4 ap0,
        in Vec4 cp0,
        in Vec4 cp1,
        in Vec4 ap1,
        in float step)
    {
        return Vec4.Normalize(Vec4.BezierTangent(
            ap0, cp0, cp1, ap1, step));
    }

    /// <summary>
    /// Raises each component of the vector to the nearest greater integer.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>result</returns>
    public static Vec4 Ceil(in Vec4 v)
    {
        return new(
            MathF.Ceiling(v.x),
            MathF.Ceiling(v.y),
            MathF.Ceiling(v.z),
            MathF.Ceiling(v.w));
    }

    /// <summary>
    /// Clamps a vector to a range within the lower and upper bound.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="lb">range lower bound</param>
    /// <param name="ub">range upper bound</param>
    /// <returns>clamped vector</returns>
    public static Vec4 Clamp(in Vec4 v, in float lb = 0.0f, in float ub = 1.0f)
    {
        return new(
            Utils.Clamp(v.x, lb, ub),
            Utils.Clamp(v.y, lb, ub),
            Utils.Clamp(v.z, lb, ub),
            Utils.Clamp(v.w, lb, ub));
    }

    /// <summary>
    /// Tests to see if the vector contains a value
    /// </summary>
    /// <param name="a">vector</param>
    /// <param name="b">value</param>
    /// <returns>evaluation</returns>
    public static bool Contains(in Vec4 a, in float b)
    {
        return Utils.Approx(a.x, b) ||
            Utils.Approx(a.y, b) ||
            Utils.Approx(a.z, b) ||
            Utils.Approx(a.w, b);
    }

    /// <summary>
    /// Returns the first vector with the sign of the second.
    /// Returns zero where the sign is zero.
    /// </summary>
    /// <param name="a">magnitude</param>
    /// <param name="b">sign</param>
    /// <returns>signed vector</returns>
    public static Vec4 CopySign(in Vec4 a, in Vec4 b)
    {
        return new(
            Utils.CopySign(a.x, b.x),
            Utils.CopySign(a.y, b.y),
            Utils.CopySign(a.z, b.z),
            Utils.CopySign(a.w, b.w));
    }

    /// <summary>
    /// Finds the absolute value of the difference between two vectors.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>absolute difference</returns>
    public static Vec4 Diff(in Vec4 a, in Vec4 b)
    {
        return new(
            Utils.Diff(b.x, a.x),
            Utils.Diff(b.y, a.y),
            Utils.Diff(b.z, a.z),
            Utils.Diff(b.w, a.w));
    }

    /// <summary>
    /// Finds the Chebyshev distance between two vectors.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>Chebyshev distance</returns>
    public static float DistChebyshev(in Vec4 a, in Vec4 b)
    {
        return Utils.Max(Utils.Diff(b.x, a.x),
            Utils.Diff(b.y, a.y),
            Utils.Diff(b.z, a.z),
            Utils.Diff(b.w, a.w));
    }

    /// <summary>
    /// Finds the Euclidean distance between two vectors. Where possible, use
    /// distance squared to avoid the computational cost of the square-root.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>Euclidean distance</returns>
    public static float DistEuclidean(in Vec4 a, in Vec4 b)
    {
        return MathF.Sqrt(Vec4.DistSq(a, b));
    }

    /// <summary>
    /// Finds the Manhattan distance between two vectors.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>Manhattan distance</returns>
    public static float DistManhattan(in Vec4 a, in Vec4 b)
    {
        return Utils.Diff(b.x, a.x) +
            Utils.Diff(b.y, a.y) +
            Utils.Diff(b.z, a.z) +
            Utils.Diff(b.w, a.w);
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
    public static float DistMinkowski(in Vec4 a, in Vec4 b, in float c = 2.0f)
    {
        if (c != 0.0f)
        {
            double cd = c;
            return (float)Math.Pow(
                Math.Pow(Math.Abs(b.x - a.x), cd) +
                Math.Pow(Math.Abs(b.y - a.y), cd) +
                Math.Pow(Math.Abs(b.z - a.z), cd) +
                Math.Pow(Math.Abs(b.w - a.w), cd),
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
    public static float DistSq(in Vec4 a, in Vec4 b)
    {
        float dx = b.x - a.x;
        float dy = b.y - a.y;
        float dz = b.z - a.z;
        float dw = b.w - a.w;
        return dx * dx + dy * dy + dz * dz + dw * dw;
    }

    /// <summary>
    /// Finds the dot product of two vectors by summing the products of their
    /// corresponding components.
    ///
    /// dot ( a, b ) := a.x b.x + a.y b.y + a.z b.z + a.w b.w
    ///
    /// The dot product of a vector with itself is equal to its magnitude
    /// squared.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>dot product</returns>
    public static float Dot(in Vec4 a, in Vec4 b)
    {
        return a.x * b.x +
            a.y * b.y +
            a.z * b.z +
            a.w * b.w;
    }

    /// <summary>
    /// Negates a vector's x component.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>flipped</returns>
    public static Vec4 FlipX(in Vec4 v)
    {
        return new(-v.x, v.y, v.z, v.w);
    }

    /// <summary>
    /// Negates a vector's y component.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>flipped</returns>
    public static Vec4 FlipY(in Vec4 v)
    {
        return new(v.x, -v.y, v.z, v.w);
    }

    /// <summary>
    /// Negates a vector's z component.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>flipped</returns>
    public static Vec4 FlipZ(in Vec4 v)
    {
        return new(v.x, v.y, -v.z, v.w);
    }

    /// <summary>
    /// Negates a vector's w component.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>flipped</returns>
    public static Vec4 FlipW(in Vec4 v)
    {
        return new(v.x, v.y, v.z, -v.w);
    }

    /// <summary>
    /// Floors each component of the vector.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>result</returns>
    public static Vec4 Floor(in Vec4 v)
    {
        return new(
            MathF.Floor(v.x),
            MathF.Floor(v.y),
            MathF.Floor(v.z),
            MathF.Floor(v.w));
    }

    /// <summary>
    /// Returns the fractional portion of the vector's components.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>fraction</returns>
    public static Vec4 Fract(in Vec4 v)
    {
        return new(
            Utils.Fract(v.x),
            Utils.Fract(v.y),
            Utils.Fract(v.z),
            Utils.Fract(v.w));
    }

    /// <summary>
    /// Generates a 4D array of vectors.
    /// </summary>
    /// <returns>array</returns>
    public static Vec4[,,,] Grid()
    {
        return Vec4.GridCartesian(
            new(-1.0f, -1.0f, -1.0f, -1.0f),
            new(1.0f, 1.0f, 1.0f, 1.0f));
    }

    /// <summary>
    /// Generates a 4D array of vectors representing
    /// a Cartesian Grid.
    /// </summary>
    /// <param name="lowerBound">lower bound</param>
    /// <param name="upperBound">upper bound</param>
    /// <param name="cols">number of columns</param>
    /// <param name="rows">number of rows</param>
    /// <param name="layers">number of layers</param>
    /// <param name="steps">number of steps</param>
    /// <returns>array</returns>
    public static Vec4[,,,] GridCartesian(
        in Vec4 lowerBound,
        in Vec4 upperBound,
        in int cols = 8,
        in int rows = 8,
        in int layers = 8,
        in int steps = 8)
    {
        int sVrf = steps < 1 ? 1 : steps;
        int lVrf = layers < 1 ? 1 : layers;
        int rVrf = rows < 1 ? 1 : rows;
        int cVrf = cols < 1 ? 1 : cols;

        bool oneStep = sVrf == 1;
        bool oneLayer = lVrf == 1;
        bool oneRow = rVrf == 1;
        bool oneCol = cVrf == 1;

        float gToStep = oneStep ? 0.0f : 1.0f / (sVrf - 1.0f);
        float hToStep = oneLayer ? 0.0f : 1.0f / (lVrf - 1.0f);
        float iToStep = oneRow ? 0.0f : 1.0f / (rVrf - 1.0f);
        float jToStep = oneCol ? 0.0f : 1.0f / (cVrf - 1.0f);

        float gOff = oneStep ? 0.5f : 0.0f;
        float hOff = oneLayer ? 0.5f : 0.0f;
        float iOff = oneRow ? 0.5f : 0.0f;
        float jOff = oneCol ? 0.5f : 0.0f;

        float lbx = lowerBound.x;
        float lby = lowerBound.y;
        float lbz = lowerBound.z;
        float lbw = lowerBound.w;

        float ubx = upperBound.x;
        float uby = upperBound.y;
        float ubz = upperBound.z;
        float ubw = upperBound.w;

        Vec4[,,,] result = new Vec4[sVrf, lVrf, rVrf, cVrf];

        int rcVrf = rVrf * cVrf;
        int lrcVrf = lVrf * rcVrf;
        int len4 = sVrf * lrcVrf;
        for (int k = 0; k < len4; ++k)
        {
            int g = k / lrcVrf;
            int m = k - g * lrcVrf;
            int h = m / rcVrf;
            int n = m - h * rcVrf;
            int i = n / cVrf;
            int j = n % cVrf;

            float jFac = j * jToStep + jOff;
            float iFac = i * iToStep + iOff;
            float hFac = h * hToStep + hOff;
            float gFac = g * gToStep + gOff;

            result[g, h, i, j] = new(
                (1.0f - jFac) * lbx + jFac * ubx,
                (1.0f - iFac) * lby + iFac * uby,
                (1.0f - hFac) * lbz + hFac * ubz,
                (1.0f - gFac) * lbw + gFac * ubw);
        }

        return result;
    }

    /// <summary>
    /// Tests to see if the vector is on the unit sphere, i.e., has a magnitude
    /// of approximately 1.0 .
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>evaluation</returns>
    public static bool IsUnit(in Vec4 v)
    {
        return Utils.Approx(Vec4.MagSq(v), 1.0f);
    }

    /// <summary>
    /// Limits a vector's magnitude to a scalar. Does nothing if the vector is
    /// beneath the limit.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="limit">limit</param>
    /// <returns>limited vector</returns>
    public static Vec4 Limit(in Vec4 v, in float limit)
    {
        float mSq = Vec4.MagSq(v);
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
    public static Vec4 LinearStep(in Vec4 edge0, in Vec4 edge1, in Vec4 x)
    {
        return new(
            Utils.LinearStep(edge0.x, edge1.x, x.x),
            Utils.LinearStep(edge0.y, edge1.y, x.y),
            Utils.LinearStep(edge0.z, edge1.z, x.z),
            Utils.LinearStep(edge0.w, edge1.w, x.w));
    }

    /// <summary>
    /// Finds the length, or magnitude, of a vector. Also referred to as the
    /// radius when using polar coordinates. Uses the formula sqrt ( dot ( a, a
    /// ) ) Where possible, use magSq or dot to avoid the computational cost of
    /// the square-root.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>magnitude</returns>
    public static float Mag(in Vec4 v)
    {
        return MathF.Sqrt(Vec4.MagSq(v));
    }

    /// <summary>
    /// Finds the length-, or magnitude-, squared of a vector. Returns the same
    /// result as dot ( a, a ) . Useful when calculating the lengths of many
    /// vectors, so as to avoid the computational cost of the square-root.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>magnitude squared</returns>
    public static float MagSq(in Vec4 v)
    {
        return v.x * v.x +
            v.y * v.y +
            v.z * v.z +
            v.w * v.w;
    }

    /// <summary>
    /// Finds the maximum of two vectors by component.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>maximum value</returns>
    public static Vec4 Max(in Vec4 a, in Vec4 b)
    {
        return new(
            MathF.Max(a.x, b.x),
            MathF.Max(a.y, b.y),
            MathF.Max(a.z, b.z),
            MathF.Max(a.w, b.w));
    }

    /// <summary>
    /// Finds the minimum of two vectors by component.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>minimum value</returns>
    public static Vec4 Min(in Vec4 a, in Vec4 b)
    {
        return new(
            MathF.Min(a.x, b.x),
            MathF.Min(a.y, b.y),
            MathF.Min(a.z, b.z),
            MathF.Min(a.w, b.w));
    }

    /// <summary>
    /// Mixes two vectors together. Adds the vectors then divides by half.
    /// </summary>
    /// <param name="o">origin vector</param>
    /// <param name="d">destination vector</param>
    /// <returns>mix</returns>
    public static Vec4 Mix(in Vec4 o, in Vec4 d)
    {
        return new(
            0.5f * (o.x + d.x),
            0.5f * (o.y + d.y),
            0.5f * (o.z + d.z),
            0.5f * (o.w + d.w));
    }

    /// <summary>
    /// Mixes two vectors together by a step. Uses the formula (1.0 - t) a + t b
    /// . The step is unclamped.
    /// </summary>
    /// <param name="o">origin vector</param>
    /// <param name="d">destination vector</param>
    /// <param name="t">step</param>
    /// <returns>mix</returns>
    public static Vec4 Mix(in Vec4 o, in Vec4 d, in float t)
    {
        float u = 1.0f - t;
        return new(
            u * o.x + t * d.x,
            u * o.y + t * d.y,
            u * o.z + t * d.z,
            u * o.w + t * d.w);
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
    public static Vec4 Mix(in Vec4 o, in Vec4 d, in Vec4 t)
    {
        return new(
            Utils.Mix(o.x, d.x, t.x),
            Utils.Mix(o.y, d.y, t.y),
            Utils.Mix(o.z, d.z, t.z),
            Utils.Mix(o.w, d.w, t.w));
    }

    /// <summary>
    /// Tests to see if all the vector's components are zero. Useful when
    /// safeguarding against invalid directions.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>evaluation</returns>
    public static bool None(in Vec4 v)
    {
        return v.x == 0.0f &&
            v.y == 0.0f &&
            v.z == 0.0f &&
            v.w == 0.0f;
    }

    /// <summary>
    /// Divides a vector by its magnitude, such that the new magnitude is 1.0 .
    /// The result is a unit vector, as it lies on the circumference of a unit
    /// circle.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>unit vector</returns>
    public static Vec4 Normalize(in Vec4 v)
    {
        float mSq = v.x * v.x + v.y * v.y + v.z * v.z + v.w * v.w;
        if (mSq > 0.0f)
        {
            float mInv = 1.0f / MathF.Sqrt(mSq);
            return new(v.x * mInv, v.y * mInv, v.z * mInv, v.w * mInv);
        }
        return Vec4.Zero;
    }

    /// <summary>
    /// Evaluates a vector like a boolean, where n != 0.0 is true.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>truth table opposite</returns>
    public static Vec4 Not(in Vec4 v)
    {
        return new(
            v.x != 0.0f ? 0.0f : 1.0f,
            v.y != 0.0f ? 0.0f : 1.0f,
            v.z != 0.0f ? 0.0f : 1.0f,
            v.w != 0.0f ? 0.0f : 1.0f);
    }

    /// <summary>
    /// Returns the scalar projection of a onto b.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>scalar projection</returns>
    public static float ProjectScalar(in Vec4 a, in Vec4 b)
    {
        float bSq = Vec4.MagSq(b);
        if (bSq != 0.0f) { return Vec4.Dot(a, b) / bSq; }
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
    public static Vec4 ProjectVector(in Vec4 a, in Vec4 b)
    {
        return b * Vec4.ProjectScalar(a, b);
    }

    /// <summary>
    /// Promotes a 2D vector to a 4D vector.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="z">z component</param>
    /// <param name="w">w component</param>
    /// <returns>vector</returns>
    public static Vec4 Promote(in Vec2 v, in float z = 0.0f, in float w = 0.0f)
    {
        return new(v.X, v.Y, z, w);
    }

    /// <summary>
    /// Promotes a 3D vector to a 4D vector.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="w">w component</param>
    /// <returns>vector</returns>
    public static Vec4 Promote(in Vec3 v, in float w = 0.0f)
    {
        return new(v.X, v.Y, v.Z, w);
    }

    /// <summary>
    /// Reduces the signal, or granularity, of a vector's components.
    /// </summary>
    /// <param name="v">input vector</param>
    /// <param name="levels">levels</param>
    /// <returns>quantized vector</returns>
    public static Vec4 Quantize(in Vec4 v, in int levels = 8)
    {
        return new(
            Utils.QuantizeSigned(v.x, levels),
            Utils.QuantizeSigned(v.y, levels),
            Utils.QuantizeSigned(v.z, levels),
            Utils.QuantizeSigned(v.w, levels));
    }

    /// <summary>
    /// Creates a random point in the Cartesian coordinate system given a lower
    /// and an upper bound.
    /// </summary>
    /// <param name="rng">random number generator</param>
    /// <param name="lb">lower bound</param>
    /// <param name="ub">upper bound</param>
    /// <returns>random vector</returns>
    public static Vec4 RandomCartesian(
        in System.Random rng,
        in Vec4 lb,
        in Vec4 ub)
    {
        return new(
            Utils.Mix(lb.x, ub.x, (float)rng.NextDouble()),
            Utils.Mix(lb.y, ub.y, (float)rng.NextDouble()),
            Utils.Mix(lb.z, ub.z, (float)rng.NextDouble()),
            Utils.Mix(lb.w, ub.w, (float)rng.NextDouble()));
    }

    /// <summary>
    /// Creates a random point in the Cartesian coordinate system given a lower
    /// and an upper bound.
    /// </summary>
    /// <param name="rng">random number generator</param>
    /// <param name="lb">lower bound</param>
    /// <param name="ub">upper bound</param>
    /// <returns>random vector</returns>
    public static Vec4 RandomCartesian(
        in System.Random rng,
        in float lb = 0.0f,
        in float ub = 1.0f)
    {
        return new(
            Utils.Mix(lb, ub, (float)rng.NextDouble()),
            Utils.Mix(lb, ub, (float)rng.NextDouble()),
            Utils.Mix(lb, ub, (float)rng.NextDouble()),
            Utils.Mix(lb, ub, (float)rng.NextDouble()));
    }

    /// <summary>
    /// Reflects an incident vector off a normal vector. Uses the formula
    ///
    /// i - 2.0 ( dot( n, i ) n )
    /// </summary>
    /// <param name="i">incident vector</param>
    /// <param name="n">normal vector</param>
    /// <returns>reflected vector</returns>
    public static Vec4 Reflect(in Vec4 i, in Vec4 n)
    {
        return i - (2.0f * n * Vec4.Dot(n, i));
    }

    /// <summary>
    /// Refracts a vector through a volume using Snell's law.
    /// </summary>
    /// <param name="i">incident vector</param>
    /// <param name="n">normal vector</param>
    /// <param name="eta">ratio of refraction indices</param>
    /// <returns>refracted vector</returns>
    public static Vec4 Refract(in Vec4 i, in Vec4 n, in float eta)
    {
        float iDotN = Vec4.Dot(i, n);
        float k = 1.0f - eta * eta * (1.0f - iDotN * iDotN);
        if (k < 0.0f) { return Vec4.Zero; }
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
    public static Vec4 Remap(
        in Vec4 v,
        in Vec4 lbOrigin,
        in Vec4 ubOrigin,
        in Vec4 lbDest,
        in Vec4 ubDest)
    {
        return new(
            Utils.Remap(v.x, lbOrigin.x, ubOrigin.x, lbDest.x, ubDest.x),
            Utils.Remap(v.y, lbOrigin.y, ubOrigin.y, lbDest.y, ubDest.y),
            Utils.Remap(v.z, lbOrigin.z, ubOrigin.z, lbDest.z, ubDest.z),
            Utils.Remap(v.w, lbOrigin.w, ubOrigin.w, lbDest.w, ubDest.w));
    }

    /// <summary>
    /// Mods each component of the left vector by those of the right.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>result</returns>
    public static Vec4 RemFloor(in Vec4 a, in Vec4 b)
    {
        return new(
            Utils.RemFloor(a.x, b.x),
            Utils.RemFloor(a.y, b.y),
            Utils.RemFloor(a.z, b.z),
            Utils.RemFloor(a.w, b.w));
    }

    /// <summary>
    /// Applies the % operator (truncation-based modulo) to the left operand.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>result</returns>
    public static Vec4 RemTrunc(in Vec4 a, in Vec4 b)
    {
        return new(
            Utils.RemTrunc(a.x, b.x),
            Utils.RemTrunc(a.y, b.y),
            Utils.RemTrunc(a.z, b.z),
            Utils.RemTrunc(a.w, b.w));
    }

    /// <summary>
    /// Normalizes a vector, then multiplies it by a scalar, in effect setting
    /// its magnitude to that scalar.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="scalar">scalar</param>
    /// <returns>rescaled vector</returns>
    public static Vec4 Rescale(in Vec4 v, in float scalar = 1.0f)
    {
        return v * Utils.Div(scalar, Vec4.Mag(v));
    }

    /// <summary>
    /// Rounds each component of the vector to the nearest whole number.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>rounded vector</returns>
    public static Vec4 Round(in Vec4 v)
    {
        return new(
            Utils.Round(v.x),
            Utils.Round(v.y),
            Utils.Round(v.z),
            Utils.Round(v.w));
    }

    /// <summary>
    /// Finds the sign of the vector: -1, if negative; 1, if positive.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>sign</returns>
    public static Vec4 Sign(in Vec4 v)
    {
        return new(
            MathF.Sign(v.x),
            MathF.Sign(v.y),
            MathF.Sign(v.z),
            MathF.Sign(v.w));
    }

    /// <summary>
    /// Generates a clamped Hermite step for an input factor; to be used in
    /// conjunction with a mixing function.
    /// </summary>
    /// <param name="edge0">left edge</param>
    /// <param name="edge1">right edge</param>
    /// <param name="x">factor</param>
    /// <returns>smooth step</returns>
    public static Vec4 SmoothStep(in Vec4 edge0, in Vec4 edge1, in Vec4 x)
    {
        return new(
            Utils.SmoothStep(edge0.x, edge1.x, x.x),
            Utils.SmoothStep(edge0.y, edge1.y, x.y),
            Utils.SmoothStep(edge0.z, edge1.z, x.z),
            Utils.SmoothStep(edge0.w, edge1.w, x.w));
    }

    /// <summary>
    /// Generates a clamped boolean step for an input factor; to be used in
    /// conjunction with a mixing function.
    /// </summary>
    /// <param name="edge">edge</param>
    /// <param name="x">factor</param>
    /// <returns>step</returns>
    public static Vec4 Step(in Vec4 edge, in Vec4 x)
    {
        return new(
            Utils.Step(edge.x, x.x),
            Utils.Step(edge.y, x.y),
            Utils.Step(edge.z, x.z),
            Utils.Step(edge.w, x.w));
    }

    /// <summary>
    /// Returns a float array of length 4 containing a vector's components.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>array</returns>
    public static float[] ToArray(in Vec4 v)
    {
        return Vec4.ToArray(v, new float[v.Length], 0);
    }

    /// <summary>
    /// Puts a vector's components into an array at a given index.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="arr">array</param>
    /// <param name="i">index</param>
    /// <returns>array</returns>
    public static float[] ToArray(in Vec4 v, in float[] arr, in int i = 0)
    {
        arr[i] = v.x;
        arr[i + 1] = v.y;
        arr[i + 2] = v.z;
        arr[i + 3] = v.w;
        return arr;
    }

    /// <summary>
    /// Returns a string representation of a vector.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string</returns>
    public static string ToString(in Vec4 v, in int places = 4)
    {
        return Vec4.ToString(new StringBuilder(96), v, places).ToString();
    }

    /// <summary>
    /// Appends a representation of a vector to a string builder.
    /// </summary>
    /// <param name="sb">string bulider</param>
    /// <param name="v">vector</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString(in StringBuilder sb, in Vec4 v, in int places = 4)
    {
        sb.Append("{\"x\":");
        Utils.ToFixed(sb, v.x, places);
        sb.Append(",\"y\":");
        Utils.ToFixed(sb, v.y, places);
        sb.Append(",\"z\":");
        Utils.ToFixed(sb, v.z, places);
        sb.Append(",\"w\":");
        Utils.ToFixed(sb, v.w, places);
        sb.Append('}');
        return sb;
    }

    /// <summary>
    /// Truncates each component of the vector.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>truncation</returns>
    public static Vec4 Trunc(in Vec4 v)
    {
        return new(
            MathF.Truncate(v.x),
            MathF.Truncate(v.y),
            MathF.Truncate(v.z),
            MathF.Truncate(v.w));
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
    public static Vec4 Wrap(in Vec4 v, in Vec4 lb, in Vec4 ub)
    {
        return new(
            Utils.Wrap(v.x, lb.x, ub.x),
            Utils.Wrap(v.y, lb.y, ub.y),
            Utils.Wrap(v.z, lb.z, ub.z),
            Utils.Wrap(v.w, lb.w, ub.w));
    }

    /// <summary>
    /// Returns a direction facing back, (0.0, -1.0, 0.0, 0.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec4 Back { get { return new(0.0f, -1.0f, 0.0f, 0.0f); } }

    /// <summary>
    /// Returns a direction facing down, (0.0, 0.0, -1.0, 0.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec4 Down { get { return new(0.0f, 0.0f, -1.0f, 0.0f); } }

    /// <summary>
    /// Returns a direction facing forward, (0.0, 1.0, 0.0, 0.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec4 Forward { get { return new(0.0f, 1.0f, 0.0f, 0.0f); } }

    /// <summary>
    /// Returns a direction facing left, (-1.0, 0.0, 0.0, 0.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec4 Left { get { return new(-1.0f, 0.0f, 0.0f, 0.0f); } }

    /// <summary>
    /// Returns a vector with all components set to 1.0 .
    /// </summary>
    /// <value>the vector</value>
    public static Vec4 One { get { return new(1.0f, 1.0f, 1.0f, 1.0f); } }

    /// <summary>
    /// Returns a direction facing right, (1.0, 0.0, 0.0, 0.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec4 Right { get { return new(1.0f, 0.0f, 0.0f, 0.0f); } }

    /// <summary>
    /// Returns a direction facing up, (0.0, 0.0, 1.0, 0.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec4 Up { get { return new(0.0f, 0.0f, 1.0f, 0.0f); } }

    /// <summary>
    /// Returns a vector with all components set to zero.
    /// </summary>
    /// <value>the vector</value>
    public static Vec4 Zero { get { return new(0.0f, 0.0f, 0.0f, 0.0f); } }
}