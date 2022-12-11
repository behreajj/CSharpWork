using System;
using System.Collections;
using System.Text;

/// <summary>
/// A readonly struct influenced by GLSL and OSL. This is intended
/// for storing points and directions in three-dimensional graphics
/// programs.
/// </summary>
[Serializable]
public readonly struct Vec3 : IComparable<Vec3>, IEquatable<Vec3>, IEnumerable
{
    /// <summary>
    /// Component on the x axis in the Cartesian coordinate system.
    /// </summary>
    private readonly float _x;

    /// <summary>
    /// Component on the y axis in the Cartesian coordinate system.
    /// </summary>
    private readonly float _y;

    /// <summary>
    /// Component on the z axis in the Cartesian coordinate system.
    /// </summary>
    private readonly float _z;

    /// <summary>
    /// The number of values (dimensions) in this vector.
    /// </summary>
    /// /// <value>length</value>
    public int Length { get { return 3; } }

    /// <summary>
    /// Component on the z axis in the Cartesian coordinate system.
    /// </summary>
    /// <value>x</value>
    public float x { get { return this._x; } }

    /// <summary>
    /// Component on the y axis in the Cartesian coordinate system.
    /// </summary>
    /// <value>y</value>
    public float y { get { return this._y; } }

    /// <summary>
    /// Component on the z axis in the Cartesian coordinate system.
    /// </summary>
    /// <value>z</value>
    public float z { get { return this._z; } }

    /// <summary>
    /// Gets the x and y components as a 2D vector.
    /// </summary>
    /// <value>2D vector</value>
    public Vec2 xy
    {
        get
        {
            return new Vec2(this._x, this._y);
        }
    }

    /// <summary>
    /// Gets the x and z components as a 2D vector.
    /// </summary>
    /// <value>2D vector</value>
    public Vec2 xz
    {
        get
        {
            return new Vec2(this._x, this._z);
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
                0 or -3 => this._x,
                1 or -2 => this._y,
                2 or -1 => this._z,
                _ => 0.0f,
            };
        }
    }

    /// <summary>
    /// Constructs a vector from float values.
    /// </summary>
    /// <param name="x">the x component</param>
    /// <param name="y">the y component</param>
    /// <param name="z">the z component</param>
    public Vec3(in float x, in float y, in float z)
    {
        this._x = x;
        this._y = y;
        this._z = z;
    }

    /// <summary>
    /// Constructs a vector from boolean values, where true is 1.0 and false is
    /// 0.0 .
    /// </summary>
    /// <param name="x">the x component</param>
    /// <param name="y">the y component</param>
    /// <param name="z">the z component</param>
    public Vec3(in bool x, in bool y, in bool z)
    {
        this._x = x ? 1.0f : 0.0f;
        this._y = y ? 1.0f : 0.0f;
        this._z = z ? 1.0f : 0.0f;
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
            return ((Utils.MulBase ^ this._x.GetHashCode()) *
                    Utils.HashMul ^ this._y.GetHashCode()) *
                Utils.HashMul ^ this._z.GetHashCode();
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
    /// Compares this vector to another in compliance with the IComparable
    /// interface. Returns 1 when a component of this vector is greater than
    /// another; -1 when lesser. Prioritizes the highest dimension first: z, y,
    /// x . Returns 0 as a last resort.
    /// </summary>
    /// <param name="v">the comparisand</param>
    /// <returns>evaluation</returns>
    public int CompareTo(Vec3 v)
    {
        return (this._z < v._z) ? -1 :
            (this._z > v._z) ? 1 :
            (this._y < v._y) ? -1 :
            (this._y > v._y) ? 1 :
            (this._x < v._x) ? -1 :
            (this._x > v._x) ? 1 :
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
        if (this._z.GetHashCode() != v._z.GetHashCode()) { return false; }
        if (this._y.GetHashCode() != v._y.GetHashCode()) { return false; }
        if (this._x.GetHashCode() != v._x.GetHashCode()) { return false; }
        return true;
    }

    /// <summary>
    /// Returns an enumerator (or iterator) for this vector, allowing its
    /// components to be accessed in a foreach loop.
    /// </summary>
    /// <returns>enumerator</returns>
    public IEnumerator GetEnumerator()
    {
        yield return this._x;
        yield return this._y;
        yield return this._z;
    }

    /// <summary>
    /// Returns a float array of length 3 containing this vector's components.
    /// </summary>
    /// <returns>array</returns>
    public float[] ToArray()
    {
        return this.ToArray(new float[this.Length], 0);
    }

    /// <summary>
    /// Puts this vector's components into an array at a given index.
    /// </summary>
    /// <param name="arr">array</param>
    /// <param name="i">index</param>
    /// <returns>array</returns>
    public float[] ToArray(in float[] arr, in int i = 0)
    {
        arr[i] = this._x;
        arr[i + 1] = this._y;
        arr[i + 2] = this._z;
        return arr;
    }

    /// <summary>
    /// Returns a named value tuple containing this vector's components.
    /// </summary>
    /// <returns>tuple</returns>
    public (float x, float y, float z) ToTuple()
    {
        return (x: this._x, y: this._y, z: this._z);
    }

    /// <summary>
    /// Converts a boolean to a vector by supplying the boolean to all the
    /// vector's components: 1.0 for true; 0.0 for false.
    /// </summary>
    /// <param name="b">the boolean</param>
    public static implicit operator Vec3(in bool b)
    {
        float eval = b ? 1.0f : 0.0f;
        return new Vec3(eval, eval, eval);
    }

    /// <summary>
    /// Converts a float to a vector by supplying the scalar to all the vector's
    /// components.
    /// </summary>
    /// <param name="s">the scalar</param>
    public static implicit operator Vec3(in float s)
    {
        return new Vec3(s, s, s);
    }

    /// <summary>
    /// Promotes a 2D vector to a 3D vector; the z component is assumed to be
    /// 0.0 .
    /// </summary>
    /// <param name="v">the 2D vector</param>
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
        return new Vec3(
            Utils.And(a._x, b._x),
            Utils.And(a._y, b._y),
            Utils.And(a._z, b._z));
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
        return new Vec3(
            Utils.Or(a._x, b._x),
            Utils.Or(a._y, b._y),
            Utils.Or(a._z, b._z));
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
        return new Vec3(
            Utils.Xor(a._x, b._x),
            Utils.Xor(a._y, b._y),
            Utils.Xor(a._z, b._z));
    }

    /// <summary>
    /// Negates vector
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>negation</returns>
    public static Vec3 operator -(in Vec3 v)
    {
        return new Vec3(-v._x, -v._y, -v._z);
    }

    /// <summary>
    /// Increments all components of a vector by 1.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>increment</returns>
    public static Vec3 operator ++(in Vec3 v)
    {
        return new Vec3(v._x + 1.0f, v._y + 1.0f, v._z + 1.0f);
    }

    /// <summary>
    /// Decrements all components of a vector by 1.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>decrement</returns>
    public static Vec3 operator --(in Vec3 v)
    {
        return new Vec3(v._x - 1.0f, v._y - 1.0f, v._z - 1.0f);
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
        return new Vec3(
            a._x * b._x,
            a._y * b._y,
            a._z * b._z);
    }

    /// <summary>
    /// Multiplies a vector by a scalar.
    /// </summary>
    /// <param name="a">left operand, the vector</param>
    /// <param name="b">right operand, the scalar</param>
    /// <returns>product</returns>
    public static Vec3 operator *(in Vec3 a, in float b)
    {
        return new Vec3(
            a._x * b,
            a._y * b,
            a._z * b);
    }

    /// <summary>
    /// Multiplies a vector by a scalar.
    /// </summary>
    /// <param name="a">left operand, the scalar</param>
    /// <param name="b">right operand, the vector</param>
    /// <returns>product</returns>
    public static Vec3 operator *(in float a, in Vec3 b)
    {
        return new Vec3(
            a * b._x,
            a * b._y,
            a * b._z);
    }

    /// <summary>
    /// Divides the left operand by the right, component-wise.
    /// </summary>
    /// <param name="a">numerator</param>
    /// <param name="b">denominator</param>
    /// <returns>quotient</returns>
    public static Vec3 operator /(in Vec3 a, in Vec3 b)
    {
        return new Vec3(
            Utils.Div(a._x, b._x),
            Utils.Div(a._y, b._y),
            Utils.Div(a._z, b._z));
    }

    /// <summary>
    /// Divides a vector by a scalar.
    /// </summary>
    /// <param name="a">vector, numerator</param>
    /// <param name="b">scalar, denominator</param>
    /// <returns>product</returns>
    public static Vec3 operator /(in Vec3 a, in float b)
    {
        if (b != 0.0f)
        {
            float bInv = 1.0f / b;
            return new Vec3(
                a._x * bInv,
                a._y * bInv,
                a._z * bInv);
        }
        return Vec3.Zero;
    }

    /// <summary>
    /// Divides a scalar by a vector.
    /// </summary>
    /// <param name="a">scalar, numerator</param>
    /// <param name="b">vector, denominator</param>
    /// <returns>product</returns>
    public static Vec3 operator /(in float a, in Vec3 b)
    {
        return new Vec3(
            Utils.Div(a, b._x),
            Utils.Div(a, b._y),
            Utils.Div(a, b._z));
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
            return new Vec3(
                a._x % b,
                a._y % b,
                a._z % b);
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
        return new Vec3(
            Utils.RemTrunc(a, b._x),
            Utils.RemTrunc(a, b._y),
            Utils.RemTrunc(a, b._z));
    }

    /// <summary>
    /// Adds two vectors together.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>sum</returns>
    public static Vec3 operator +(in Vec3 a, in Vec3 b)
    {
        return new Vec3(
            a._x + b._x,
            a._y + b._y,
            a._z + b._z);
    }

    /// <summary>
    /// Subtracts the right vector from the left vector.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>difference</returns>
    public static Vec3 operator -(in Vec3 a, in Vec3 b)
    {
        return new Vec3(
            a._x - b._x,
            a._y - b._y,
            a._z - b._z);
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
        return new Vec3(
            a._x < b._x,
            a._y < b._y,
            a._z < b._z);
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
        return new Vec3(
            a._x > b._x,
            a._y > b._y,
            a._z > b._z);
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
        return new Vec3(
            a._x <= b._x,
            a._y <= b._y,
            a._z <= b._z);
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
        return new Vec3(
            a._x >= b._x,
            a._y >= b._y,
            a._z >= b._z);
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
        return new Vec3(
            a._x != b._x,
            a._y != b._y,
            a._z != b._z);
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
        return new Vec3(
            a._x == b._x,
            a._y == b._y,
            a._z == b._z);
    }

    /// <summary>
    /// Finds the absolute value of each vector component.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>absolute vector</returns>
    public static Vec3 Abs(in Vec3 v)
    {
        return new Vec3(
            MathF.Abs(v._x),
            MathF.Abs(v._y),
            MathF.Abs(v._z));
    }

    /// <summary>
    /// Tests to see if all the vector's components are non-zero. Useful when
    /// testing valid dimensions (width and depth) stored in vectors.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>evaluation</returns>
    public static bool All(in Vec3 v)
    {
        return v._x != 0.0f && v._y != 0.0f && v._z != 0.0f;
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
            double ax = a._x;
            double ay = a._y;
            double az = a._z;

            double bx = b._x;
            double by = b._y;
            double bz = b._z;

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
        return v._x != 0.0f || v._y != 0.0f || v._z != 0.0f;
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
    /// <param name="tol">the tolerance</param>
    /// <returns>evaluation</returns>
    public static bool Approx(in Vec3 a, in Vec3 b, in float tol = Utils.Epsilon)
    {
        return Utils.Approx(a._x, b._x, tol) &&
            Utils.Approx(a._y, b._y, tol) &&
            Utils.Approx(a._z, b._z, tol);
    }

    /// <summary>
    /// Tests to see if a vector has, approximately, the specified magnitude.
    /// </summary>
    /// <param name="a">vector</param>
    /// <param name="b">the magnitude</param>
    /// <param name="tol">the tolerance</param>
    /// <returns>evaluation</returns>
    public static bool ApproxMag(in Vec3 a, in float b = 1.0f, in float tol = Utils.Epsilon)
    {
        return Utils.Approx(Vec3.MagSq(a), b * b, tol);
    }

    /// <summary>
    /// Finds the vector's azimuth in the range [-PI, PI] .
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>angle in radians</returns>
    public static float AzimuthSigned(in Vec3 v)
    {
        return MathF.Atan2(v._y, v._x);
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
    /// When the step is less than zero, returns the first anchor point. When the
    /// step is greater than one, returns the second anchor point.
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
        if (step <= 0.0f) { return new Vec3(ap0._x, ap0._y, ap0._z); }
        else if (step >= 1.0f) { return new Vec3(ap1._x, ap1._y, ap1._z); }

        float u = 1.0f - step;
        float tcb = step * step;
        float ucb = u * u;
        float usq3t = ucb * (step + step + step);
        float tsq3u = tcb * (u + u + u);
        ucb *= u;
        tcb *= step;

        return new Vec3(
            ap0._x * ucb +
            cp0._x * usq3t +
            cp1._x * tsq3u +
            ap1._x * tcb,

            ap0._y * ucb +
            cp0._y * usq3t +
            cp1._y * tsq3u +
            ap1._y * tcb,

            ap0._z * ucb +
            cp0._z * usq3t +
            cp1._z * tsq3u +
            ap1._z * tcb);
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

        return new Vec3(
            (cp0._x - ap0._x) * usq3 +
            (cp1._x - cp0._x) * ut6 +
            (ap1._x - cp1._x) * tsq3,

            (cp0._y - ap0._y) * usq3 +
            (cp1._y - cp0._y) * ut6 +
            (ap1._y - cp1._y) * tsq3,

            (cp0._z - ap0._z) * usq3 +
            (cp1._z - cp0._z) * ut6 +
            (ap1._z - cp1._z) * tsq3);
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
        return new Vec3(
            MathF.Ceiling(v._x),
            MathF.Ceiling(v._y),
            MathF.Ceiling(v._z));
    }

    /// <summary>
    /// Clamps a vector to a range within the lower and upper bound.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="lb">the range lower bound</param>
    /// <param name="ub">the range upper bound</param>
    /// <returns>clamped vector</returns>
    public static Vec3 Clamp(in Vec3 v, in float lb = 0.0f, in float ub = 1.0f)
    {
        return new Vec3(
            Utils.Clamp(v._x, lb, ub),
            Utils.Clamp(v._y, lb, ub),
            Utils.Clamp(v._z, lb, ub));
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
        return Utils.Approx(a._x, b) ||
            Utils.Approx(a._y, b) ||
            Utils.Approx(a._z, b);
    }

    /// <summary>
    /// Returns the first vector with the sign of the second.
    /// Returns zero where the sign is zero.
    /// </summary>
    /// <param name="a">the magnitude</param>
    /// <param name="b">the sign</param>
    /// <returns>signed vector</returns>
    public static Vec3 CopySign(in Vec3 a, in Vec3 b)
    {
        return new Vec3(
            Utils.CopySign(a._x, b._x),
            Utils.CopySign(a._y, b._y),
            Utils.CopySign(a._z, b._z));
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
        return new Vec3(
            a._y * b._z - a._z * b._y,
            a._z * b._x - a._x * b._z,
            a._x * b._y - a._y * b._x);
    }

    /// <summary>
    /// Finds the absolute value of the difference between two vectors.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>absolute difference</returns>
    public static Vec3 Diff(in Vec3 a, in Vec3 b)
    {
        return new Vec3(
            Utils.Diff(b._x, a._x),
            Utils.Diff(b._y, a._y),
            Utils.Diff(b._z, a._z));
    }

    /// <summary>
    /// Finds the Chebyshev distance between two vectors. Forms a cube pattern
    /// when plotted.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>distance</returns>
    public static float DistChebyshev(in Vec3 a, in Vec3 b)
    {
        return Utils.Max(Utils.Diff(b._x, a._x),
                         Utils.Diff(b._y, a._y),
                         Utils.Diff(b._z, a._z));
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
        return Utils.Diff(b._x, a._x) +
               Utils.Diff(b._y, a._y) +
               Utils.Diff(b._z, a._z);
    }

    /// <summary>
    /// Finds the Minkowski distance between two vectors. This is a
    /// generalization of other distance formulae. When the exponent value, c,
    /// is 1.0, the Minkowski distance equals the Manhattan distance; when it is
    /// 2.0, Minkowski equals the Euclidean distance.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <param name="c">the exponent</param>
    /// <returns>Minkowski distance</returns>
    public static float DistMinkowski(in Vec3 a, in Vec3 b, in float c = 2.0f)
    {
        if (c != 0.0f)
        {
            double cd = c;
            return (float)Math.Pow(
                Math.Pow(Math.Abs(b._x - a._x), cd) +
                Math.Pow(Math.Abs(b._y - a._y), cd) +
                Math.Pow(Math.Abs(b._z - a._z), cd),
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
        float dx = b._x - a._x;
        float dy = b._y - a._y;
        float dz = b._z - a._z;
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
        return a._x * b._x + a._y * b._y + a._z * b._z;
    }

    /// <summary>
    /// Negates a vector's x component.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>flipped</returns>
    public static Vec3 FlipX(in Vec3 v)
    {
        return new Vec3(-v._x, v._y, v._z);
    }

    /// <summary>
    /// Negates a vector's y component.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>flipped</returns>
    public static Vec3 FlipY(in Vec3 v)
    {
        return new Vec3(v._x, -v._y, v._z);
    }

    /// <summary>
    /// Negates a vector's z component.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>flipped</returns>
    public static Vec3 FlipZ(in Vec3 v)
    {
        return new Vec3(v._x, v._y, -v._z);
    }

    /// <summary>
    /// Floors each component of the vector.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>result</returns>
    public static Vec3 Floor(in Vec3 v)
    {
        return new Vec3(
            MathF.Floor(v._x),
            MathF.Floor(v._y),
            MathF.Floor(v._z));
    }

    /// <summary>
    /// Returns the fractional portion of the vector's components.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>fractional portion</returns>
    public static Vec3 Fract(in Vec3 v)
    {
        return new Vec3(
            Utils.Fract(v._x),
            Utils.Fract(v._y),
            Utils.Fract(v._z));
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
        return new Vec3(
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
    public static Vec3 FromColor(in Clr c)
    {
        if (c.a > 0)
        {
            float x = c.r + c.r - 1.0f;
            float y = c.g + c.g - 1.0f;
            float z = c.b + c.b - 1.0f;
            float mSq = x * x + y * y + z * z;
            if (mSq > 0.0f)
            {
                float mInv = Utils.InvSqrtUnchecked(mSq);
                return new Vec3(x * mInv, y * mInv, z * mInv);
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
            new Vec3(-1.0f, -1.0f, -1.0f),
            new Vec3(1.0f, 1.0f, 1.0f));
    }

    /// <summary>
    /// Generates a 3D array of vectors representing
    /// a Cartesian Grid.
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
        int lval = layers < 2 ? 2 : layers;
        int rval = rows < 2 ? 2 : rows;
        int cval = cols < 2 ? 2 : cols;

        float hToStep = 1.0f / (lval - 1.0f);
        float iToStep = 1.0f / (rval - 1.0f);
        float jToStep = 1.0f / (cval - 1.0f);

        Vec3[,,] result = new Vec3[lval, rval, cval];

        int rcval = rval * cval;
        int len3 = lval * rcval;
        for (int k = 0; k < len3; ++k)
        {
            int h = k / rcval;
            int m = k - h * rcval;
            int i = m / cval;
            int j = m % cval;

            result[h, i, j] = Vec3.Mix(
                lowerBound,
                upperBound,
                new Vec3(
                    j * jToStep,
                    i * iToStep,
                    h * hToStep));
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
        return (mSq > 0.0f) ? MathF.Acos(v._z / MathF.Sqrt(mSq)) : Utils.HalfPi;
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
        return new Vec3(
            Utils.LinearStep(edge0._x, edge1._x, x._x),
            Utils.LinearStep(edge0._y, edge1._y, x._y),
            Utils.LinearStep(edge0._z, edge1._z, x._z));
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
        return v._x * v._x +
            v._y * v._y +
            v._z * v._z;
    }

    /// <summary>
    /// Finds the maximum of two vectors by component.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>maximum value</returns>
    public static Vec3 Max(in Vec3 a, in Vec3 b)
    {
        return new Vec3(
            MathF.Max(a._x, b._x),
            MathF.Max(a._y, b._y),
            MathF.Max(a._z, b._z));
    }

    /// <summary>
    /// Finds the minimum of two vectors by component.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>minimum value</returns>
    public static Vec3 Min(in Vec3 a, in Vec3 b)
    {
        return new Vec3(
            MathF.Min(a._x, b._x),
            MathF.Min(a._y, b._y),
            MathF.Min(a._z, b._z));
    }

    /// <summary>
    /// Mixes two vectors together. Adds the vectors then divides by half.
    /// </summary>
    /// <param name="o">origin vector</param>
    /// <param name="d">destination vector</param>
    /// <returns>mix</returns>
    public static Vec3 Mix(in Vec3 o, in Vec3 d)
    {
        return new Vec3(
            0.5f * (o._x + d._x),
            0.5f * (o._y + d._y),
            0.5f * (o._z + d._z));
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
        return new Vec3(
            u * o._x + t * d._x,
            u * o._y + t * d._y,
            u * o._z + t * d._z);
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
        return new Vec3(
            Utils.Mix(o._x, d._x, t._x),
            Utils.Mix(o._y, d._y, t._y),
            Utils.Mix(o._z, d._z, t._z));
    }

    /// <summary>
    /// Tests to see if all the vector's components are zero. Useful when
    /// safeguarding against invalid directions.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>evaluation</returns>
    public static bool None(in Vec3 v)
    {
        return v._x == 0.0f && v._y == 0.0f && v._z == 0.0f;
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
        float mSq = v._x * v._x + v._y * v._y + v._z * v._z;
        if (mSq > 0.0f)
        {
            float mInv = 1.0f / MathF.Sqrt(mSq);
            return new Vec3(v._x * mInv, v._y * mInv, v._z * mInv);
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
        return new Vec3(
            v._x != 0.0f ? 0.0f : 1.0f,
            v._y != 0.0f ? 0.0f : 1.0f,
            v._z != 0.0f ? 0.0f : 1.0f);
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
        return new Vec3(v.x, v.y, z);
    }

    /// <summary>
    /// Reduces the signal, or granularity, of a vector's components.
    /// </summary>
    /// <param name="v">input vector</param>
    /// <param name="levels">levels</param>
    /// <returns>quantized vector</returns>
    public static Vec3 Quantize(in Vec3 v, in int levels = 8)
    {
        return new Vec3(
            Utils.QuantizeSigned(v._x, levels),
            Utils.QuantizeSigned(v._y, levels),
            Utils.QuantizeSigned(v._z, levels));
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
        return new Vec3(
            Utils.Mix(lb._x, ub._x, (float)rng.NextDouble()),
            Utils.Mix(lb._y, ub._y, (float)rng.NextDouble()),
            Utils.Mix(lb._z, ub._z, (float)rng.NextDouble()));
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
        in float lb = 0.0f,
        in float ub = 1.0f)
    {
        return new Vec3(
            Utils.Mix(lb, ub, (float)rng.NextDouble()),
            Utils.Mix(lb, ub, (float)rng.NextDouble()),
            Utils.Mix(lb, ub, (float)rng.NextDouble()));
    }

    /// <summary>
    /// Creates a vector at a random azimuth, inclination and radius.
    /// </summary>
    /// <param name="rng">random number generator</param>
    /// <param name="rhoMin">minimum radius</param>
    /// <param name="rhoMax">maximum radius</param>
    /// <returns>output vector</returns>
    public static Vec3 RandomSpherical(
        in System.Random rng,
        in float rhoMin = 1.0f,
        in float rhoMax = 1.0f)
    {
        return Vec3.FromSpherical(
            Utils.Mix(-MathF.PI, MathF.PI,
                (float)rng.NextDouble()),

            Utils.Mix(MathF.PI, 0.0f,
                (float)rng.NextDouble()),

            Utils.Mix(rhoMin, rhoMax,
                (float)rng.NextDouble()));
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
        return i - ((2.0f * Vec3.Dot(n, i)) * n);
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
        return new Vec3(
            Utils.Remap(v._x, lbOrigin._x, ubOrigin._x, lbDest._x, ubDest._x),
            Utils.Remap(v._y, lbOrigin._y, ubOrigin._y, lbDest._y, ubDest._y),
            Utils.Remap(v._z, lbOrigin._z, ubOrigin._z, lbDest._z, ubDest._z));
    }

    /// <summary>
    /// Mods each component of the left vector by those of the right.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>result</returns>
    public static Vec3 RemFloor(in Vec3 a, in Vec3 b)
    {
        return new Vec3(
            Utils.RemFloor(a._x, b._x),
            Utils.RemFloor(a._y, b._y),
            Utils.RemFloor(a._z, b._z));
    }

    /// <summary>
    /// Applies the % operator (truncation-based modulo) to the left operand.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>result</returns>
    public static Vec3 RemTrunc(in Vec3 a, in Vec3 b)
    {
        return new Vec3(
            Utils.RemTrunc(a._x, b._x),
            Utils.RemTrunc(a._y, b._y),
            Utils.RemTrunc(a._z, b._z));
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
        float complxy = complcos * axis._x * axis._y;
        float complxz = complcos * axis._x * axis._z;
        float complyz = complcos * axis._y * axis._z;

        float sinx = sina * axis._x;
        float siny = sina * axis._y;
        float sinz = sina * axis._z;

        return new Vec3(
            (complcos * axis._x * axis._x + cosa) * v._x +
            (complxy - sinz) * v._y +
            (complxz + siny) * v._z,

            (complxy + sinz) * v._x +
            (complcos * axis._y * axis._y + cosa) * v._y +
            (complyz - sinx) * v._z,

            (complxz - siny) * v._x +
            (complyz + sinx) * v._y +
            (complcos * axis._z * axis._z + cosa) * v._z);
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
        return new Vec3(
            v._x,
            cosa * v._y - sina * v._z,
            cosa * v._z + sina * v._y);
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
        return new Vec3(
            cosa * v._x + sina * v._z,
            v._y,
            cosa * v._z - sina * v._x);
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
        return new Vec3(
            cosa * v._x - sina * v._y,
            cosa * v._y + sina * v._x,
            v._z);
    }

    /// <summary>
    /// Rounds each component of the vector to the nearest whole number.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>rounded vector</returns>
    public static Vec3 Round(in Vec3 v)
    {
        return new Vec3(
            Utils.Round(v._x),
            Utils.Round(v._y),
            Utils.Round(v._z));
    }

    /// <summary>
    /// Rounds each component of the vector to a specified number of places.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="places">number of places</param>
    /// <returns>rounded vector</returns>
    public static Vec3 Round(in Vec3 v, in int places)
    {
        return new Vec3(
            MathF.Round(v._x, places),
            MathF.Round(v._y, places),
            MathF.Round(v._z, places));
    }

    /// <summary>
    /// Finds the sign of the vector: -1, if negative; 1, if positive.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>sign</returns>
    public static Vec3 Sign(in Vec3 v)
    {
        return new Vec3(
            MathF.Sign(v._x),
            MathF.Sign(v._y),
            MathF.Sign(v._z));
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
        double ox = o._x;
        double oy = o._y;
        double oz = o._z;

        double dx = d._x;
        double dy = d._y;
        double dz = d._z;

        double odDot = ox * dx + oy * dy + oz * dz;
        odDot = Math.Min(Math.Max(odDot, -0.999999d), 0.999999d);

        double omega = Math.Acos(odDot);
        double omSin = Math.Sin(omega);
        double omSinInv = (omSin != 0.0d) ? 1.0d / omSin : 1.0d;

        double td = t;
        double oFac = Math.Sin((1.0d - td) * omega) * omSinInv;
        double dFac = Math.Sin(td * omega) * omSinInv;

        return new Vec3(
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
        return new Vec3(
            Utils.SmoothStep(edge0._x, edge1._x, x._x),
            Utils.SmoothStep(edge0._y, edge1._y, x._y),
            Utils.SmoothStep(edge0._z, edge1._z, x._z));
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
        return new Vec3(
            Utils.Step(edge._x, x._x),
            Utils.Step(edge._y, x._y),
            Utils.Step(edge._z, x._z));
    }

    /// <summary>
    /// Returns a named value tuple containing the vector's azimuth,
    /// theta; inclination, phi; and magnitude, rho.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>tuple</returns>
    public static (float theta, float phi, float rho) ToSpherical(in Vec3 v)
    {
        float mSq = v._x * v._x + v._y * v._y + v._z * v._z;
        if (mSq > 0.0)
        {
            float m = MathF.Sqrt(mSq);
            return (
                theta: MathF.Atan2(v._y, v._x),
                phi: Utils.HalfPi - MathF.Acos(v._z / m),
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
    public static StringBuilder ToString(in StringBuilder sb, in Vec3 v, in int places = 4)
    {
        sb.Append("{ x: ");
        Utils.ToFixed(sb, v._x, places);
        sb.Append(", y: ");
        Utils.ToFixed(sb, v._y, places);
        sb.Append(", z: ");
        Utils.ToFixed(sb, v._z, places);
        sb.Append(' ');
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
    public static StringBuilder ToString(in StringBuilder sb, in Vec3[] arr, in int places = 4)
    {
        sb.Append('[');
        sb.Append(' ');

        if (arr != null)
        {
            int len = arr.Length;
            int last = len - 1;

            for (int i = 0; i < last; ++i)
            {
                Vec3.ToString(sb, arr[i], places);
                sb.Append(',');
                sb.Append(' ');
            }

            Vec3.ToString(sb, arr[last], places);
            sb.Append(' ');
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
        return new Vec3(
            MathF.Truncate(v._x),
            MathF.Truncate(v._y),
            MathF.Truncate(v._z));
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
        return new Vec3(
            Utils.Wrap(v._x, lb._x, ub._x),
            Utils.Wrap(v._y, lb._y, ub._y),
            Utils.Wrap(v._z, lb._z, ub._z));
    }

    /// <summary>
    /// Returns a direction facing back, (0.0, -1.0, 0.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec3 Back { get { return new Vec3(0.0f, -1.0f, 0.0f); } }

    /// <summary>
    /// Returns a direction facing down, (0.0, 0.0, -1.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec3 Down { get { return new Vec3(0.0f, 0.0f, -1.0f); } }

    /// <summary>
    /// Returns a direction facing forward, (0.0, 1.0, 0.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec3 Forward { get { return new Vec3(0.0f, 1.0f, 0.0f); } }

    /// <summary>
    /// Returns a direction facing left, (-1.0, 0.0, 0.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec3 Left { get { return new Vec3(-1.0f, 0.0f, 0.0f); } }

    /// <summary>
    /// Returns a vector with all components set to 1.0 .
    /// </summary>
    /// <value>the vector</value>
    public static Vec3 One { get { return new Vec3(1.0f, 1.0f, 1.0f); } }

    /// <summary>
    /// Returns a direction facing right, (1.0, 0.0, 0.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec3 Right { get { return new Vec3(1.0f, 0.0f, 0.0f); } }

    /// <summary>
    /// Returns a direction facing up, (0.0, 0.0, 1.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec3 Up { get { return new Vec3(0.0f, 0.0f, 1.0f); } }

    /// <summary>
    /// Returns a vector with all components set to zero.
    /// </summary>
    /// <value>the vector</value>
    public static Vec3 Zero { get { return new Vec3(0.0f, 0.0f, 0.0f); } }
}