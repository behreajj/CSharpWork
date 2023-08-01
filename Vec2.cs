using System;
using System.Collections;
using System.Text;

/// <summary>
/// A readonly struct influenced by GLSL and OSL. This is intended
/// for storing points and directions in two-dimensional graphics
/// programs.
/// </summary>
[Serializable]
public readonly struct Vec2 : IComparable<Vec2>, IEquatable<Vec2>, IEnumerable
{
    /// <summary>
    /// Component on the x axis in the Cartesian coordinate system.
    /// </summary>
    private readonly float x;

    /// <summary>
    /// Component on the y axis in the Cartesian coordinate system.
    /// </summary>
    private readonly float y;

    /// <summary>
    /// The number of values (dimensions) in this vector.
    /// </summary>
    /// <value>length</value>
    public int Length { get { return 2; } }

    /// <summary>
    /// Component on the x axis in the Cartesian coordinate system.
    /// </summary>
    /// <value>x</value>
    public float X { get { return this.x; } }

    /// <summary>
    /// Component on the y axis in the Cartesian coordinate system.
    /// </summary>
    /// <value>y</value>
    public float Y { get { return this.y; } }

    /// <summary>
    /// Retrieves a component by index. When the provided index is 1 or -1,
    /// returns y; 0 or -2, x.
    /// </summary>
    /// <value>the component</value>
    public float this[int i]
    {
        get
        {
            return i switch
            {
                0 or -2 => this.x,
                1 or -1 => this.y,
                _ => 0.0f,
            };
        }
    }

    /// <summary>
    /// Constructs a vector from single precision real numbers.
    /// </summary>
    /// <param name="x">x component</param>
    /// <param name="y">y component</param>
    public Vec2(in float x, in float y)
    {
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// Constructs a vector from boolean values, where true is 1.0 and false is
    /// 0.0 .
    /// </summary>
    /// <param name="x">x component</param>
    /// <param name="y">y component</param>
    public Vec2(in bool x, in bool y)
    {
        this.x = x ? 1.0f : 0.0f;
        this.y = y ? 1.0f : 0.0f;
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
        if (value is Vec2 vec) { return this.Equals(vec); }
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
            return (Utils.MulBase ^ this.x.GetHashCode()) *
                Utils.HashMul ^ this.y.GetHashCode();
        }
    }

    /// <summary>
    /// Returns a string representation of this vector.
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()
    {
        return Vec2.ToString(this);
    }

    /// <summary>
    /// Compares this vector to another.
    /// Returns 1 when a component of this vector is greater than
    /// another; -1 when lesser. Returns 0 as a last resort. 
    /// Prioritizes the highest dimension first: y, x.
    /// </summary>
    /// <param name="v">the comparisand</param>
    /// <returns>evaluation</returns>
    public int CompareTo(Vec2 v)
    {
        return (this.y < v.y) ? -1 :
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
    public bool Equals(Vec2 v)
    {
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
    }

    /// <summary>
    /// Converts a boolean to a vector by supplying the boolean to all the
    /// vector's components: 1.0 for true; 0.0 for false.
    /// </summary>
    /// <param name="b">boolean</param>
    public static implicit operator Vec2(in bool b)
    {
        float eval = b ? 1.0f : 0.0f;
        return new(eval, eval);
    }

    /// <summary>
    /// Converts a float to a vector by supplying the scalar to all the vector's
    /// components.
    /// </summary>
    /// <param name="s">scalar</param>
    public static implicit operator Vec2(in float s)
    {
        return new(s, s);
    }

    /// <summary>
    /// Converts a vector to a boolean by finding whether all of its components
    /// are non-zero.
    /// </summary>
    /// <param name="v">vector</param>
    public static explicit operator bool(in Vec2 v)
    {
        return Vec2.All(v);
    }

    /// <summary>
    /// A vector evaluates to true when all of its components are not equal to
    /// zero.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>evaluation</returns>
    public static bool operator true(in Vec2 v)
    {
        return Vec2.All(v);
    }

    /// <summary>
    /// A vector evaluates to false when all of its components are equal to
    /// zero.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>evaluation</returns>
    public static bool operator false(in Vec2 v)
    {
        return Vec2.None(v);
    }

    /// <summary>
    /// Evaluates a vector as a boolean. Equivalent to using the ! operator.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>opposite</returns>
    public static Vec2 operator !(in Vec2 v)
    {
        return Vec2.Not(v);
    }

    /// <summary>
    /// Evaluates a vector as a boolean. Equivalent to using the ~ operator.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>complement</returns>
    public static Vec2 operator ~(in Vec2 v)
    {
        return Vec2.Not(v);
    }

    /// <summary>
    /// Evaluates two vectors like booleans, using the inclusive and (AND) logic
    /// gate.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>evaluation</returns>
    public static Vec2 operator &(in Vec2 a, in Vec2 b)
    {
        return new(
            Utils.And(a.x, b.x),
            Utils.And(a.y, b.y));
    }

    /// <summary>
    /// Evaluates two vectors like booleans, using the inclusive or (OR) logic
    /// gate.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>evaluation</returns>
    public static Vec2 operator |(in Vec2 a, in Vec2 b)
    {
        return new(
            Utils.Or(a.x, b.x),
            Utils.Or(a.y, b.y));
    }

    /// <summary>
    /// Evaluates two vectors like booleans, using the exclusive or (XOR) logic
    /// gate.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>evaluation</returns>
    public static Vec2 operator ^(in Vec2 a, in Vec2 b)
    {
        return new(
            Utils.Xor(a.x, b.x),
            Utils.Xor(a.y, b.y));
    }

    /// <summary>
    /// Negates the vector.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>negation</returns>
    public static Vec2 operator -(in Vec2 v)
    {
        return new(-v.x, -v.y);
    }

    /// <summary>
    /// Increments all components of a vector by 1.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>increment</returns>
    public static Vec2 operator ++(in Vec2 v)
    {
        return new(v.x + 1.0f, v.y + 1.0f);
    }

    /// <summary>
    /// Decrements all components of a vector by 1.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>decrement</returns>
    public static Vec2 operator --(in Vec2 v)
    {
        return new(v.x - 1.0f, v.y - 1.0f);
    }

    /// <summary>
    /// Multiplies two vectors, component-wise, i.e.,
    /// returns the Hadamard product.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>product</returns>
    public static Vec2 operator *(in Vec2 a, in Vec2 b)
    {
        return new(a.x * b.x, a.y * b.y);
    }

    /// <summary>
    /// Multiplies a vector by a scalar.
    /// </summary>
    /// <param name="a">left operand, the vector</param>
    /// <param name="b">right operand, the scalar</param>
    /// <returns>product</returns>
    public static Vec2 operator *(in Vec2 a, in float b)
    {
        return new(a.x * b, a.y * b);
    }

    /// <summary>
    /// Multiplies a vector by a scalar.
    /// </summary>
    /// <param name="a">left operand, the scalar</param>
    /// <param name="b">right operand, the vector</param>
    /// <returns>product</returns>
    public static Vec2 operator *(in float a, in Vec2 b)
    {
        return new(a * b.x, a * b.y);
    }

    /// <summary>
    /// Divides the left operand by the right, component-wise.
    /// </summary>
    /// <param name="a">numerator</param>
    /// <param name="b">denominator</param>
    /// <returns>quotient</returns>
    public static Vec2 operator /(in Vec2 a, in Vec2 b)
    {
        return new(
            Utils.Div(a.x, b.x),
            Utils.Div(a.y, b.y));
    }

    /// <summary>
    /// Divides a vector by a scalar.
    /// </summary>
    /// <param name="a">vector, numerator</param>
    /// <param name="b">scalar, denominator</param>
    /// <returns>quotient</returns>
    public static Vec2 operator /(in Vec2 a, in float b)
    {
        if (b != 0.0f)
        {
            float bInv = 1.0f / b;
            return new(
                a.x * bInv,
                a.y * bInv);
        }
        return Vec2.Zero;
    }

    /// <summary>
    /// Divides a scalar by a vector.
    /// </summary>
    /// <param name="a">scalar, numerator</param>
    /// <param name="b">vector, denominator</param>
    /// <returns>quotient</returns>
    public static Vec2 operator /(in float a, in Vec2 b)
    {
        return new(
            Utils.Div(a, b.x),
            Utils.Div(a, b.y));
    }

    /// <summary>
    /// Applies truncation-based modulo to the left and right vectors.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>result</returns>
    public static Vec2 operator %(in Vec2 a, in Vec2 b)
    {
        return Vec2.RemTrunc(a, b);
    }

    /// <summary>
    /// Applies truncation-based modulo to the left and right operands.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>result</returns>
    public static Vec2 operator %(in Vec2 a, in float b)
    {
        if (b != 0.0f) { return new(a.x % b, a.y % b); }
        return a;
    }

    /// <summary>
    /// Applies truncation-based modulo to the left and right operands.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>result</returns>
    public static Vec2 operator %(in float a, in Vec2 b)
    {
        return new(
            Utils.RemTrunc(a, b.x),
            Utils.RemTrunc(a, b.y));
    }

    /// <summary>
    /// Adds two vectors together.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>sum</returns>
    public static Vec2 operator +(in Vec2 a, in Vec2 b)
    {
        return new(a.x + b.x, a.y + b.y);
    }

    /// <summary>
    /// Subtracts the right vector from the left vector.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>difference</returns>
    public static Vec2 operator -(in Vec2 a, in Vec2 b)
    {
        return new(a.x - b.x, a.y - b.y);
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
    public static Vec2 operator <(in Vec2 a, in Vec2 b)
    {
        return new(a.x < b.x, a.y < b.y);
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
    public static Vec2 operator >(in Vec2 a, in Vec2 b)
    {
        return new(a.x > b.x, a.y > b.y);
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
    public static Vec2 operator <=(in Vec2 a, in Vec2 b)
    {
        return new(a.x <= b.x, a.y <= b.y);
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
    public static Vec2 operator >=(in Vec2 a, in Vec2 b)
    {
        return new(a.x >= b.x, a.y >= b.y);
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
    public static Vec2 operator !=(in Vec2 a, in Vec2 b)
    {
        return new(a.x != b.x, a.y != b.y);
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
    public static Vec2 operator ==(in Vec2 a, in Vec2 b)
    {
        return new(a.x == b.x, a.y == b.y);
    }

    /// <summary>
    /// Finds the absolute value of each vector component.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>absolute vector</returns>
    public static Vec2 Abs(in Vec2 v)
    {
        return new(
            MathF.Abs(v.x),
            MathF.Abs(v.y));
    }

    /// <summary>
    /// Tests to see if all the vector's components are non-zero. Useful when
    /// testing valid dimensions (width and depth) stored in vectors.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>evaluation</returns>
    public static bool All(in Vec2 v)
    {
        return v.x != 0.0f && v.y != 0.0f;
    }

    /// <summary>
    /// Finds the angle between two vectors.
    /// </summary>
    /// <param name="a">the first vector</param>
    /// <param name="b">the second vector</param>
    /// <returns>angle</returns>
    public static float AngleBetween(in Vec2 a, in Vec2 b)
    {
        // Double precision is required for accurate angle distance.
        if (Vec2.Any(a) && Vec2.Any(b))
        {
            double ax = a.x;
            double ay = a.y;

            double bx = b.x;
            double by = b.y;

            return (float)Math.Acos(
                (ax * bx + ay * by) /
                (Math.Sqrt(ax * ax + ay * ay) *
                    Math.Sqrt(bx * bx + by * by)));
        }
        return 0.0f;
    }

    /// <summary>
    /// Tests to see if any of the vector's components are non-zero.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>evaluation</returns>
    public static bool Any(in Vec2 v)
    {
        return v.x != 0.0f || v.y != 0.0f;
    }

    /// <summary>
    /// Appends a vector to a one-dimensional vector array. Returns a new array.
    /// </summary>
    /// <param name="a">array</param>
    /// <param name="b">vector</param>
    /// <returns>array</returns>
    public static Vec2[] Append(in Vec2[] a, in Vec2 b)
    {
        bool aNull = a == null;
        if (aNull) { return new Vec2[] { b }; }
        int aLen = a.Length;
        Vec2[] result = new Vec2[aLen + 1];
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
        in Vec2 a, in Vec2 b,
        in float tol = Utils.Epsilon)
    {
        return Utils.Approx(a.x, b.x, tol) &&
            Utils.Approx(a.y, b.y, tol);
    }

    /// <summary>
    /// Tests to see if two vectors are parallel.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <param name="tol">tolerance</param>
    /// <returns>evaluation</returns>
    public static bool AreParallel(
        in Vec2 a, in Vec2 b,
        in float tol = Utils.Epsilon)
    {
        return Utils.Approx(Vec2.Cross(a, b), 0.0f, tol);
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
    public static Vec2 BezierPoint(
        in Vec2 ap0,
        in Vec2 cp0,
        in Vec2 cp1,
        in Vec2 ap1,
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
            ap1.y * tcb);
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
    public static Vec2 BezierTangent(
        in Vec2 ap0,
        in Vec2 cp0,
        in Vec2 cp1,
        in Vec2 ap1,
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
            (ap1.y - cp1.y) * tsq3);
    }

    /// <summary>
    /// Returns a normalized tangent on a Bezier curve.
    /// </summary>
    /// <param name="ap0">the first anchor point</param>
    /// <param name="cp0">the first control point</param>
    /// <param name="cp1">the second control point</param>
    /// <param name="ap1">the second anchor point</param>
    /// <param name="step">the step</param>
    /// <returns>tangent along the curve</returns>
    public static Vec2 BezierTanUnit(
        in Vec2 ap0,
        in Vec2 cp0,
        in Vec2 cp1,
        in Vec2 ap1,
        in float step)
    {
        return Vec2.Normalize(Vec2.BezierTangent(
            ap0, cp0, cp1, ap1, step));
    }

    /// <summary>
    /// Raises each component of the vector to the nearest greater integer.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>result</returns>
    public static Vec2 Ceil(in Vec2 v)
    {
        return new(
            MathF.Ceiling(v.x),
            MathF.Ceiling(v.y));
    }

    /// <summary>
    /// Clamps a vector to a range within the lower and upper bound.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="lb">range lower bound</param>
    /// <param name="ub">range upper bound</param>
    /// <returns>clamped vector</returns>
    public static Vec2 Clamp(in Vec2 v, in float lb = 0.0f, in float ub = 1.0f)
    {
        return new(
            Utils.Clamp(v.x, lb, ub),
            Utils.Clamp(v.y, lb, ub));
    }

    /// <summary>
    /// Concatenates two one-dimensional Vec2 arrays.
    /// </summary>
    /// <param name="a">left array</param>
    /// <param name="b">right array</param>
    /// <returns>concatenation</returns>
    public static Vec2[] Concat(in Vec2[] a, in Vec2[] b)
    {
        bool aNull = a == null;
        bool bNull = b == null;

        if (aNull && bNull) { return new Vec2[] { }; }

        if (aNull)
        {
            Vec2[] result0 = new Vec2[b.Length];
            System.Array.Copy(b, 0, result0, 0, b.Length);
            return result0;
        }

        if (bNull)
        {
            Vec2[] result1 = new Vec2[a.Length];
            System.Array.Copy(a, 0, result1, 0, a.Length);
            return result1;
        }

        int aLen = a.Length;
        int bLen = b.Length;
        Vec2[] result2 = new Vec2[aLen + bLen];
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
    public static bool Contains(in Vec2 a, in float b)
    {
        return Utils.Approx(a.x, b) ||
            Utils.Approx(a.y, b);
    }

    /// <summary>
    /// Returns the first vector with the sign of the second.
    /// Returns zero where the sign is zero.
    /// </summary>
    /// <param name="a">magnitude</param>
    /// <param name="b">sign</param>
    /// <returns>signed vector</returns>
    public static Vec2 CopySign(in Vec2 a, in Vec2 b)
    {
        return new(
            Utils.CopySign(a.x, b.x),
            Utils.CopySign(a.y, b.y));
    }

    /// <summary>
    /// Returns the z component of the cross product between two vectors. The x
    /// and y components of the cross between 2D vectors are always zero. For
    /// that reason, the normalized cross product is equal to the sign of the
    /// cross product.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>cross product z</returns>
    public static float Cross(in Vec2 a, in Vec2 b)
    {
        return a.x * b.y - a.y * b.x;
    }

    /// <summary>
    /// Finds the absolute value of the difference between two vectors.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>absolute difference</returns>
    public static Vec2 Diff(in Vec2 a, in Vec2 b)
    {
        return new(
            Utils.Diff(b.x, a.x),
            Utils.Diff(b.y, a.y));
    }

    /// <summary>
    /// Finds the Chebyshev distance between two vectors. Forms a square pattern
    /// when plotted.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>Chebyshev distance</returns>
    public static float DistChebyshev(in Vec2 a, in Vec2 b)
    {
        return MathF.Max(Utils.Diff(b.x, a.x),
            Utils.Diff(b.y, a.y));
    }

    /// <summary>
    /// Finds the Euclidean distance between two vectors. Where possible, use
    /// distance squared to avoid the computational cost of the square-root.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>Euclidean distance</returns>
    public static float DistEuclidean(in Vec2 a, in Vec2 b)
    {
        return MathF.Sqrt(Vec2.DistSq(a, b));
    }

    /// <summary>
    /// Finds the Manhattan distance between two vectors. Forms a diamond
    /// pattern when plotted.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>Manhattan distance</returns>
    public static float DistManhattan(in Vec2 a, in Vec2 b)
    {
        return Utils.Diff(b.x, a.x) + Utils.Diff(b.y, a.y);
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
    public static float DistMinkowski(in Vec2 a, in Vec2 b, in float c = 2.0f)
    {
        if (c != 0.0f)
        {
            double cd = c;
            return (float)Math.Pow(
                Math.Pow(Math.Abs(b.x - a.x), cd) +
                Math.Pow(Math.Abs(b.y - a.y), cd),
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
    public static float DistSq(in Vec2 a, in Vec2 b)
    {
        float dx = b.x - a.x;
        float dy = b.y - a.y;
        return dx * dx + dy * dy;
    }

    /// <summary>
    /// Finds the dot product of two vectors by summing the products of their
    /// corresponding components.
    ///
    /// dot ( a, b ) := a.x b.x + a.y b.y
    ///
    /// The dot product of a vector with itself is equal to its magnitude
    /// squared.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>dot product</returns>
    public static float Dot(in Vec2 a, in Vec2 b)
    {
        return a.x * b.x + a.y * b.y;
    }

    /// <summary>
    /// Negates a vector's x component.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>flipped</returns>
    public static Vec2 FlipX(in Vec2 v)
    {
        return new(-v.x, v.y);
    }

    /// <summary>
    /// Negates a vector's y component.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>flipped</returns>
    public static Vec2 FlipY(in Vec2 v)
    {
        return new(v.x, -v.y);
    }

    /// <summary>
    /// Floors each component of the vector.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>result</returns>
    public static Vec2 Floor(in Vec2 v)
    {
        return new(
            MathF.Floor(v.x),
            MathF.Floor(v.y));
    }

    /// <summary>
    /// Returns the fractional portion of the vector's components.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>fraction</returns>
    public static Vec2 Fract(in Vec2 v)
    {
        return new(
            Utils.Fract(v.x),
            Utils.Fract(v.y));
    }

    /// <summary>
    /// Creates a vector from polar coordinates: (1) theta, an angle in radians,
    /// the vector's heading; (2) rho, a radius, the vector's magnitude. Uses
    /// the formula
    ///
    /// ( rho cos ( theta ), rho sin ( theta ) )
    /// </summary>
    /// <param name="heading">angle in radians</param>
    /// <param name="radius">radius</param>
    /// <returns>vector</returns>
    public static Vec2 FromPolar(
        in float heading = 0.0f,
        in float radius = 1.0f)
    {
        return new(
            radius * MathF.Cos(heading),
            radius * MathF.Sin(heading));
    }

    /// <summary>
    /// Generates a 2D array of vectors.
    /// </summary>
    /// <returns>array</returns>
    public static Vec2[,] Grid()
    {
        return Vec2.GridCartesian(
            new(-1.0f, -1.0f),
            new(1.0f, 1.0f));
    }

    /// <summary>
    /// Generates a 2D array of vectors representing a Cartesian Grid.
    /// </summary>
    /// <param name="lowerBound">lower bound</param>
    /// <param name="upperBound">upper bound</param>
    /// <param name="cols">number of columns</param>
    /// <param name="rows">number of rows</param>
    /// <returns>array</returns>
    public static Vec2[,] GridCartesian(
        in Vec2 lowerBound,
        in Vec2 upperBound,
        in int cols = 8,
        in int rows = 8)
    {
        int rVrf = rows < 1 ? 1 : rows;
        int cVrf = cols < 1 ? 1 : cols;

        bool oneRow = rVrf == 1;
        bool oneCol = cVrf == 1;

        float iToStep = oneRow ? 0.0f : 1.0f / (rVrf - 1.0f);
        float jToStep = oneCol ? 0.0f : 1.0f / (cVrf - 1.0f);

        float iOff = oneRow ? 0.5f : 0.0f;
        float jOff = oneCol ? 0.5f : 0.0f;

        float lbx = lowerBound.x;
        float lby = lowerBound.y;

        float ubx = upperBound.x;
        float uby = upperBound.y;

        Vec2[,] result = new Vec2[rVrf, cVrf];

        int len2 = cVrf * rVrf;
        for (int k = 0; k < len2; ++k)
        {
            int i = k / cVrf;
            int j = k % cVrf;

            float jFac = j * jToStep + jOff;
            float iFac = i * iToStep + iOff;

            result[i, j] = new(
                (1.0f - jFac) * lbx + jFac * ubx,
                (1.0f - iFac) * lby + iFac * uby);
        }

        return result;
    }

    /// <summary>
    /// Finds the vector's heading in the range [-PI, PI] .
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>angle in radians</returns>
    public static float HeadingSigned(in Vec2 v)
    {
        return MathF.Atan2(v.y, v.x);
    }

    /// <summary>
    /// Finds the vector's heading in the range [0.0, TAU] .
    /// </summary>
    /// <param name="v">the vector</param>
    /// <returns>angle in radians</returns>
    public static float HeadingUnsigned(in Vec2 v)
    {
        float h = Vec2.HeadingSigned(v);
        return h < -0.0f ? h + Utils.Tau : h;
    }

    /// <summary>
    /// Tests to see if the vector is on the unit circle, i.e., has a magnitude
    /// of approximately 1.0 .
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>evaluation</returns>
    public static bool IsUnit(in Vec2 v)
    {
        return Utils.Approx(Vec2.MagSq(v), 1.0f);
    }

    /// <summary>
    /// Limits a vector's magnitude to a scalar. Does nothing if the vector is
    /// beneath the limit.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="limit">limit</param>
    /// <returns>limited vector</returns>
    public static Vec2 Limit(in Vec2 v, in float limit)
    {
        float mSq = Vec2.MagSq(v);
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
    public static Vec2 LinearStep(in Vec2 edge0, in Vec2 edge1, in Vec2 x)
    {
        return new(
            Utils.LinearStep(edge0.x, edge1.x, x.x),
            Utils.LinearStep(edge0.y, edge1.y, x.y));
    }

    /// <summary>
    /// Finds the length, or magnitude, of a vector. Also referred to as the
    /// radius when using polar coordinates. Uses the formula sqrt ( dot ( a, a)
    /// ) Where possible, use magSq or dot to avoid the computational cost of
    /// the square-root.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>magnitude</returns>
    public static float Mag(in Vec2 v)
    {
        return MathF.Sqrt(Vec2.MagSq(v));
    }

    /// <summary>
    /// Finds the length-, or magnitude-, squared of a vector. Returns the same
    /// result as dot ( a, a ) . Useful when calculating the lengths of many
    /// vectors, so as to avoid the computational cost of the square-root.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>magnitude squared</returns>
    public static float MagSq(in Vec2 v)
    {
        return v.x * v.x + v.y * v.y;
    }

    /// <summary>
    /// Finds the maximum of two vectors by component.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>maximum value</returns>
    public static Vec2 Max(in Vec2 a, in Vec2 b)
    {
        return new(
            MathF.Max(a.x, b.x),
            MathF.Max(a.y, b.y));
    }

    /// <summary>
    /// Finds the minimum of two vectors by component.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>minimum value</returns>
    public static Vec2 Min(in Vec2 a, in Vec2 b)
    {
        return new(
            MathF.Min(a.x, b.x),
            MathF.Min(a.y, b.y));
    }

    /// <summary>
    /// Mixes two vectors together. Adds the vectors then divides by half.
    /// </summary>
    /// <param name="o">origin vector</param>
    /// <param name="d">destination vector</param>
    /// <returns>mix</returns>
    public static Vec2 Mix(in Vec2 o, in Vec2 d)
    {
        return new(
            0.5f * (o.x + d.x),
            0.5f * (o.y + d.y));
    }

    /// <summary>
    /// Mixes two vectors together by a step. Uses the formula (1.0 - t) a + t b
    /// . The step is unclamped.
    /// </summary>
    /// <param name="o">origin vector</param>
    /// <param name="d">destination vector</param>
    /// <param name="t">step</param>
    /// <returns>mix</returns>
    public static Vec2 Mix(in Vec2 o, in Vec2 d, in float t)
    {
        float u = 1.0f - t;
        return new(
            u * o.x + t * d.x,
            u * o.y + t * d.y);
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
    public static Vec2 Mix(in Vec2 o, in Vec2 d, in Vec2 t)
    {
        return new(
            Utils.Mix(o.x, d.x, t.x),
            Utils.Mix(o.y, d.y, t.y));
    }

    /// <summary>
    /// Tests to see if all the vector's components are zero. Useful when
    /// safeguarding against invalid directions.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>evaluation</returns>
    public static bool None(in Vec2 v)
    {
        return v.x == 0.0f && v.y == 0.0f;
    }

    /// <summary>
    /// Divides a vector by its magnitude, such that the new magnitude is 1.0 .
    /// The result is a unit vector, as it lies on the circumference of a unit
    /// circle.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>unit vector</returns>
    public static Vec2 Normalize(in Vec2 v)
    {
        float mSq = v.x * v.x + v.y * v.y;
        if (mSq > 0.0f)
        {
            float mInv = 1.0f / MathF.Sqrt(mSq);
            return new(v.x * mInv, v.y * mInv);
        }
        return Vec2.Zero;
    }

    /// <summary>
    /// Evaluates a vector like a boolean, where n != 0.0 is true.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>truth table opposite</returns>
    public static Vec2 Not(in Vec2 v)
    {
        return new(
            v.x != 0.0f ? 0.0f : 1.0f,
            v.y != 0.0f ? 0.0f : 1.0f);
    }

    /// <summary>
    /// Finds the perpendicular of a vector in the counter-clockwise direction,
    /// such that
    ///
    /// perp ( right ) = forward,
    ///
    /// perp ( 1.0, 0.0 ) = ( 0.0, 1.0 )
    ///
    /// perp ( forward ) = left,
    ///
    /// perp ( 0.0, 1.0 ) = ( -1.0, 0.0 )
    ///
    /// perp ( left ) = back,
    ///
    /// perp ( -1.0, 0.0 ) = ( 0.0, -1.0 )
    ///
    /// perp ( back ) = right,
    ///
    /// perp ( 0.0, -1.0 ) = ( 1.0, 0.0 )
    ///
    /// In terms of the components, perp ( x, y ) = ( -y, x ) .
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>perpendicular</returns>
    public static Vec2 PerpendicularCCW(in Vec2 v)
    {
        return new(-v.y, v.x);
    }

    /// <summary>
    /// Finds the perpendicular of a vector in the clockwise direction, such
    /// that
    ///
    /// perp ( right ) = back,
    ///
    /// perp( 1.0, 0.0 ) = ( 0.0, -1.0 )
    ///
    /// perp ( back ) = left,
    ///
    /// perp( 0.0, -1.0 ) = ( -1.0, 0.0 )
    ///
    /// perp ( left ) = forward,
    ///
    /// perp( -1.0, 0.0 ) = ( 0.0, 1.0 )
    ///
    /// perp ( forward ) = right,
    ///
    /// perp( 0.0, 1.0 ) = ( 1.0, 0.0 )
    ///
    /// In terms of the components, perp ( x, y ) = ( y, -x ) .
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>perpendicular</returns>
    public static Vec2 PerpendicularCW(in Vec2 v)
    {
        return new(v.y, -v.x);
    }

    /// <summary>
    /// Returns the scalar projection of a onto b.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>scalar projection</returns>
    public static float ProjectScalar(in Vec2 a, in Vec2 b)
    {
        float bSq = Vec2.MagSq(b);
        if (bSq != 0.0f) { return Vec2.Dot(a, b) / bSq; }
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
    public static Vec2 ProjectVector(in Vec2 a, in Vec2 b)
    {
        return b * Vec2.ProjectScalar(a, b);
    }

    /// <summary>
    /// Reduces the signal, or granularity, of a vector's components.
    /// </summary>
    /// <param name="v">input vector</param>
    /// <param name="levels">levels</param>
    /// <returns>quantized vector</returns>
    public static Vec2 Quantize(in Vec2 v, in int levels = 8)
    {
        return new(
            Utils.QuantizeSigned(v.x, levels),
            Utils.QuantizeSigned(v.y, levels));
    }

    /// <summary>
    /// Creates a random vector.
    /// </summary>
    /// <param name="rng">random number generator</param>
    /// <returns>random vector</returns>
    public static Vec2 Random(in System.Random rng)
    {
        return Vec2.RandomPolar(rng);
    }

    /// <summary>
    /// Creates a random point in the Cartesian coordinate system given a lower
    /// and an upper bound.
    /// </summary>
    /// <param name="rng">random number generator</param>
    /// <param name="lb">lower bound</param>
    /// <param name="ub">upper bound</param>
    /// <returns>random vector</returns>
    public static Vec2 RandomCartesian(
        in System.Random rng,
        in Vec2 lb,
        in Vec2 ub)
    {
        return new(
            Utils.Mix(lb.x, ub.x, (float)rng.NextDouble()),
            Utils.Mix(lb.y, ub.y, (float)rng.NextDouble()));
    }

    /// <summary>
    /// Creates a random point in the Cartesian coordinate system
    /// given a lower and an upper bound.
    /// </summary>
    /// <param name="rng">random number generator</param>
    /// <param name="lb">lower bound</param>
    /// <param name="ub">upper bound</param>
    /// <returns>random vector</returns>
    public static Vec2 RandomCartesian(
        in System.Random rng,
        in float lb = 0.0f,
        in float ub = 1.0f)
    {
        return new(
            Utils.Mix(lb, ub, (float)rng.NextDouble()),
            Utils.Mix(lb, ub, (float)rng.NextDouble()));
    }

    /// <summary>
    /// Creates a vector that lies on a circle.
    /// Finds three random numbers with normal distribution,
    /// then normalizes them.
    /// </summary>
    /// <param name="rng">random number generator</param>
    /// <param name="radius">radius</param>
    /// <returns>random vector</returns>
    public static Vec2 RandomPolar(
        in System.Random rng,
        in float radius = 1.0f)
    {
        float x = Utils.NextGaussian(rng);
        float y = Utils.NextGaussian(rng);
        float mSq = x * x + y * y;
        if (mSq != 0.0f)
        {
            float scalar = radius / MathF.Sqrt(mSq);
            return new(x * scalar, y * scalar);
        }
        return Vec2.Zero;
    }

    /// <summary>
    /// Reflects an incident vector off a normal vector. Uses the formula
    ///
    /// i - 2.0 ( dot( n, i ) n )
    /// </summary>
    /// <param name="i">incident vector</param>
    /// <param name="n">normal vector</param>
    /// <returns>reflected vector</returns>
    public static Vec2 Reflect(in Vec2 i, in Vec2 n)
    {
        return i - (2.0f * Vec2.Dot(n, i)) * n;
    }

    /// <summary>
    /// Refracts a vector through a volume using Snell's law.
    /// </summary>
    /// <param name="i">incident vector</param>
    /// <param name="n">normal vector</param>
    /// <param name="eta">ratio of refraction indices</param>
    /// <returns>refracted vector</returns>
    public static Vec2 Refract(in Vec2 i, in Vec2 n, in float eta)
    {
        float iDotN = Vec2.Dot(i, n);
        float k = 1.0f - eta * eta * (1.0f - iDotN * iDotN);
        if (k < 0.0f) { return Vec2.Zero; }
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
    public static Vec2 Remap(
        in Vec2 v,
        in Vec2 lbOrigin,
        in Vec2 ubOrigin,
        in Vec2 lbDest,
        in Vec2 ubDest)
    {
        return new(
            Utils.Remap(v.x, lbOrigin.x, ubOrigin.x, lbDest.x, ubDest.x),
            Utils.Remap(v.y, lbOrigin.y, ubOrigin.y, lbDest.y, ubDest.y));
    }

    /// <summary>
    /// Mods each component of the left vector by those of the right.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>result</returns>
    public static Vec2 RemFloor(in Vec2 a, in Vec2 b)
    {
        return new(
            Utils.RemFloor(a.x, b.x),
            Utils.RemFloor(a.y, b.y));
    }

    /// <summary>
    /// Applies the % operator (truncation-based modulo) to the left operand.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>result</returns>
    public static Vec2 RemTrunc(in Vec2 a, in Vec2 b)
    {
        return new(
            Utils.RemTrunc(a.x, b.x),
            Utils.RemTrunc(a.y, b.y));
    }

    /// <summary>
    /// Normalizes a vector, then multiplies it by a scalar, in effect setting
    /// its magnitude to that scalar.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="scalar">scalar</param>
    /// <returns>rescaled vector</returns>
    public static Vec2 Rescale(in Vec2 v, in float scalar = 1.0f)
    {
        return v * Utils.Div(scalar, Vec2.Mag(v));
    }

    /// <summary>
    /// Resizes an array of vectors to a requested length.
    /// </summary>
    /// <param name="arr">array</param>
    /// <param name="sz">new size</param>
    /// <returns>resized array</returns>
    public static Vec2[] Resize(in Vec2[] arr, in int sz)
    {
        if (sz < 1) { return new Vec2[] { }; }
        Vec2[] result = new Vec2[sz];

        if (arr != null)
        {
            int len = arr.Length;
            int end = sz > len ? len : sz;
            System.Array.Copy(arr, result, end);
        }

        return result;
    }

    /// <summary>
    /// Rotates a vector around the z axis by using an angle in radians.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="radians">the angle in radians</param>
    /// <returns>rotated vector</returns>
    public static Vec2 RotateZ(in Vec2 v, in float radians)
    {
        return Vec2.RotateZ(v, MathF.Cos(radians), MathF.Sin(radians));
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
    public static Vec2 RotateZ(in Vec2 v, in float cosa, in float sina)
    {
        return new(
            cosa * v.x - sina * v.y,
            cosa * v.y + sina * v.x);
    }

    /// <summary>
    /// Rounds each component of the vector to the nearest whole number.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>rounded vector</returns>
    public static Vec2 Round(in Vec2 v)
    {
        return new(
            Utils.Round(v.x),
            Utils.Round(v.y));
    }

    /// <summary>
    /// Finds the sign of the vector: -1, if negative; 1, if positive.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>sign</returns>
    public static Vec2 Sign(in Vec2 v)
    {
        return new(
            MathF.Sign(v.x),
            MathF.Sign(v.y));
    }

    /// <summary>
    /// Generates a clamped Hermite step for an input factor; to be used in
    /// conjunction with a mixing function.
    /// </summary>
    /// <param name="edge0">left edge</param>
    /// <param name="edge1">right edge</param>
    /// <param name="x">factor</param>
    /// <returns>smooth step</returns>
    public static Vec2 SmoothStep(in Vec2 edge0, in Vec2 edge1, in Vec2 x)
    {
        return new(
            Utils.SmoothStep(edge0.x, edge1.x, x.x),
            Utils.SmoothStep(edge0.y, edge1.y, x.y));
    }

    /// <summary>
    /// Generates a clamped boolean step for an input factor; to be used in
    /// conjunction with a mixing function.
    /// </summary>
    /// <param name="edge">edge</param>
    /// <param name="x">factor</param>
    /// <returns>step</returns>
    public static Vec2 Step(in Vec2 edge, in Vec2 x)
    {
        return new(
            Utils.Step(edge.x, x.x),
            Utils.Step(edge.y, x.y));
    }

    /// <summary>
    /// Returns a float array of length 2 containing a vector's components.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>array</returns>
    public static float[] ToArray(in Vec2 v)
    {
        return Vec2.ToArray(v, new float[v.Length], 0);
    }

    /// <summary>
    /// Puts a vector's components into an array at a given index.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="arr">array</param>
    /// <param name="i">index</param>
    /// <returns>array</returns>
    public static float[] ToArray(in Vec2 v, in float[] arr, in int i = 0)
    {
        arr[i] = v.x;
        arr[i + 1] = v.y;
        return arr;
    }

    /// <summary>
    /// Returns a named value tuple containing the vector's heading,
    /// theta; and magnitude, rho.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>a tuple</returns>
    public static (float theta, float rho) ToPolar(in Vec2 v)
    {
        float mSq = Vec2.MagSq(v);
        if (mSq > 0.0)
        {
            return (
                theta: MathF.Atan2(v.y, v.x),
                rho: MathF.Sqrt(mSq));
        }
        return (theta: 0.0f, rho: 0.0f);
    }

    /// <summary>
    /// Returns a string representation of a vector.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string</returns>
    public static string ToString(in Vec2 v, in int places = 4)
    {
        return Vec2.ToString(new StringBuilder(64), v, places).ToString();
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
        in Vec2 v,
        in int places = 4)
    {
        sb.Append("{\"x\":");
        Utils.ToFixed(sb, v.x, places);
        sb.Append(",\"y\":");
        Utils.ToFixed(sb, v.y, places);
        sb.Append('}');
        return sb;
    }

    /// <summary>
    /// Returns a string representation of an array of vectors.
    /// </summary>
    /// <param name="arr">array</param>
    /// <param name="places">print precision</param>
    /// <returns>string</returns>
    public static string ToString(in Vec2[] arr, in int places = 4)
    {
        return Vec2.ToString(new StringBuilder(arr.Length * 64), arr, places).ToString();
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
        in Vec2[] arr,
        in int places = 4)
    {
        sb.Append('[');
        if (arr != null)
        {
            int len = arr.Length;
            int last = len - 1;

            for (int i = 0; i < last; ++i)
            {
                Vec2.ToString(sb, arr[i], places);
                sb.Append(',');
            }

            Vec2.ToString(sb, arr[last], places);
        }

        sb.Append(']');
        return sb;
    }

    /// <summary>
    /// Truncates each component of the vector.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>truncation</returns>
    public static Vec2 Trunc(in Vec2 v)
    {
        return new(
            MathF.Truncate(v.x),
            MathF.Truncate(v.y));
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
    public static Vec2 Wrap(in Vec2 v, in Vec2 lb, in Vec2 ub)
    {
        return new(
            Utils.Wrap(v.x, lb.x, ub.x),
            Utils.Wrap(v.y, lb.y, ub.y));
    }

    /// <summary>
    /// Returns a direction facing back, (0.0, -1.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec2 Back { get { return new(0.0f, -1.0f); } }

    /// <summary>
    /// Returns a direction facing forward, (0.0, 1.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec2 Forward { get { return new(0.0f, 1.0f); } }

    /// <summary>
    /// Returns a direction facing left, (-1.0, 0.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec2 Left { get { return new(-1.0f, 0.0f); } }

    /// <summary>
    /// Returns a vector with all components set to 1.0 .
    /// </summary>
    /// <value>the vector</value>
    public static Vec2 One { get { return new(1.0f, 1.0f); } }

    /// <summary>
    /// Returns a direction facing right, (1.0, 0.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec2 Right { get { return new(1.0f, 0.0f); } }

    /// <summary>
    /// Returns a vector at the center of a texture coordinate system, (0.5,
    /// 0.5) .
    /// </summary>
    /// <value>the center</value>
    public static Vec2 UvCenter { get { return new(0.5f, 0.5f); } }

    /// <summary>
    /// Returns a vector with all components set to zero.
    /// </summary>
    /// <value>the vector</value>
    public static Vec2 Zero { get { return new(0.0f, 0.0f); } }
}