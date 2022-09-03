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
    private readonly float _x;

    /// <summary>
    /// Component on the y axis.
    /// </summary>
    private readonly float _y;

    /// <summary>
    /// Component on the z axis.
    /// </summary>
    private readonly float _z;

    /// <summary>
    /// Component on the z axis.
    /// </summary>
    private readonly float _w;

    /// <summary>
    /// Returns the number of values (dimensions) in this vector.
    /// </summary>
    /// <value>length</value>
    public int Length { get { return 4; } }

    /// <summary>
    /// Component on the x axis.
    /// </summary>
    /// <value>x</value>
    public float x { get { return this._x; } }

    /// <summary>
    /// Component on the y axis.
    /// </summary>
    /// <value>y</value>
    public float y { get { return this._y; } }

    /// <summary>
    /// Component on the z axis.
    /// </summary>
    /// <value>z</value>
    public float z { get { return this._z; } }

    /// <summary>
    /// Component on the w axis.
    /// </summary>
    /// <value>w</value>
    public float w { get { return this._w; } }

    /// <summary>
    /// Gets the first three components as a 3D vector.
    /// </summary>
    /// <value>3D vector</value>
    public Vec3 xyz
    {
        get
        {
            return new Vec3(this._x, this._y, this._z);
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
            switch (i)
            {
                case 0:
                case -4:
                    return this._x;
                case 1:
                case -3:
                    return this._y;
                case 2:
                case -2:
                    return this._z;
                case 3:
                case -1:
                    return this._w;
                default:
                    return 0.0f;
            }
        }
    }

    /// <summary>
    /// Constructs a vector from float values.
    /// </summary>
    /// <param name="x">the x component</param>
    /// <param name="y">the y component</param>
    /// <param name="z">the z component</param>
    /// <param name="w">the w component</param>
    public Vec4(in float x, in float y, in float z, in float w)
    {
        this._x = x;
        this._y = y;
        this._z = z;
        this._w = w;
    }

    /// <summary>
    /// Constructs a vector from boolean values, where true is 1.0 and false is
    /// 0.0 .
    /// </summary>
    /// <param name="x">the x component</param>
    /// <param name="y">the y component</param>
    /// <param name="z">the z component</param>
    /// <param name="w">the w component</param>
    public Vec4(in bool x, in bool y, in bool z, in bool w)
    {
        this._x = x ? 1.0f : 0.0f;
        this._y = y ? 1.0f : 0.0f;
        this._z = z ? 1.0f : 0.0f;
        this._w = w ? 1.0f : 0.0f;
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
        if (value is Vec4) { return this.Equals((Vec4)value); }
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
            return (((Utils.MulBase ^ this._x.GetHashCode()) *
                        Utils.HashMul ^ this._y.GetHashCode()) *
                    Utils.HashMul ^ this._z.GetHashCode()) *
                Utils.HashMul ^ this._w.GetHashCode();
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
    /// Compares this vector to another in compliance with the IComparable
    /// interface. Returns 1 when a component of this vector is greater than
    /// another; -1 when lesser. Prioritizes the highest dimension first: w, z,
    /// y, x . Returns 0 as a last resort.
    /// </summary>
    /// <param name="v">the comparisand</param>
    /// <returns>evaluation</returns>
    public int CompareTo(Vec4 v)
    {
        return (this._w < v._w) ? -1 :
            (this._w > v._w) ? 1 :
            (this._z < v._z) ? -1 :
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
    public bool Equals(Vec4 v)
    {
        if (this._w.GetHashCode() != v._w.GetHashCode()) { return false; }
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
        yield return this._w;
    }

    /// <summary>
    /// Returns a float array of length 4 containing this vector's components.
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
        arr[i + 3] = this._w;
        return arr;
    }

    /// <summary>
    /// Returns a named value tuple containing this vector's components.
    /// </summary>
    /// <returns>tuple</returns>
    public (float x, float y, float z, float w) ToTuple()
    {
        return (x: this._x, y: this._y, z: this._z, w: this._w);
    }

    /// <summary>
    /// Converts a boolean to a vector by supplying the boolean to all the
    /// vector's components: 1.0 for true; 0.0 for false.
    /// </summary>
    /// <param name="b">the boolean</param>
    /// <returns>vector</returns>
    public static implicit operator Vec4(in bool b)
    {
        float eval = b ? 1.0f : 0.0f;
        return new Vec4(eval, eval, eval, eval);
    }

    /// <summary>
    /// Converts a float to a vector by supplying the scalar to all the vector's
    /// components.
    /// </summary>
    /// <param name="s">the scalar</param>
    /// <returns>vector</returns>
    public static implicit operator Vec4(in float s)
    {
        return new Vec4(s, s, s, s);
    }

    /// <summary>
    /// Promotes a 2D vector to a 4D vector; the z and w components are assumed
    /// to be 0.0 .
    /// </summary>
    /// <param name="v">the 2D vector</param>
    /// <returns>vector</returns>
    public static implicit operator Vec4(in Vec2 v)
    {
        return Vec4.Promote(v, 0.0f, 0.0f);
    }

    /// <summary>
    /// Promotes a 3D vector to a 4D vector; the w component is assumed to be
    /// 0.0 .
    /// </summary>
    /// <param name="v">the 2D vector</param>
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
        return new Vec4(
            Utils.And(a._x, b._x),
            Utils.And(a._y, b._y),
            Utils.And(a._z, b._z),
            Utils.And(a._w, b._w));
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
        return new Vec4(
            Utils.Or(a._x, b._x),
            Utils.Or(a._y, b._y),
            Utils.Or(a._z, b._z),
            Utils.Or(a._w, b._w));
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
        return new Vec4(
            Utils.Xor(a._x, b._x),
            Utils.Xor(a._y, b._y),
            Utils.Xor(a._z, b._z),
            Utils.Xor(a._w, b._w));
    }

    /// <summary>
    /// Negates vector
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>negation</returns>
    public static Vec4 operator -(in Vec4 v)
    {
        return new Vec4(-v._x, -v._y, -v._z, -v._w);
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
        return new Vec4(
            a._x * b._x,
            a._y * b._y,
            a._z * b._z,
            a._w * b._w);
    }

    /// <summary>
    /// Multiplies a vector by a scalar.
    /// </summary>
    /// <param name="a">left operand, the vector</param>
    /// <param name="b">right operand, the scalar</param>
    /// <returns>product</returns>
    public static Vec4 operator *(in Vec4 a, in float b)
    {
        return new Vec4(
            a._x * b,
            a._y * b,
            a._z * b,
            a._w * b);
    }

    /// <summary>
    /// Multiplies a vector by a scalar.
    /// </summary>
    /// <param name="a">left operand, the scalar</param>
    /// <param name="b">right operand, the vector</param>
    /// <returns>product</returns>
    public static Vec4 operator *(in float a, in Vec4 b)
    {
        return new Vec4(
            a * b._x,
            a * b._y,
            a * b._z,
            a * b._w);
    }

    /// <summary>
    /// Divides the left operand by the right, component-wise. This is
    /// mathematically incorrect, but serves as a shortcut for transforming a
    /// vector by the inverse of a scalar matrix.
    /// </summary>
    /// <param name="a">numerator</param>
    /// <param name="b">denominator</param>
    /// <returns>quotient</returns>
    public static Vec4 operator /(in Vec4 a, in Vec4 b)
    {
        return new Vec4(
            Utils.Div(a._x, b._x),
            Utils.Div(a._y, b._y),
            Utils.Div(a._z, b._z),
            Utils.Div(a._w, b._w));
    }

    /// <summary>
    /// Divides a vector by a scalar.
    /// </summary>
    /// <param name="a">vector, numerator</param>
    /// <param name="b">scalar, denominator</param>
    /// <returns>product</returns>
    public static Vec4 operator /(in Vec4 a, in float b)
    {
        if (b != 0.0f)
        {
            float bInv = 1.0f / b;
            return new Vec4(
                a._x * bInv,
                a._y * bInv,
                a._z * bInv,
                a._w * bInv);
        }
        return Vec4.Zero;
    }

    /// <summary>
    /// Divides a scalar by a vector.
    /// </summary>
    /// <param name="a">scalar, numerator</param>
    /// <param name="b">vector, denominator</param>
    /// <returns>product</returns>
    public static Vec4 operator /(in float a, in Vec4 b)
    {
        return new Vec4(
            Utils.Div(a, b._x),
            Utils.Div(a, b._y),
            Utils.Div(a, b._z),
            Utils.Div(a, b._w));
    }

    /// <summary>
    /// Applies truncation-based modulo (fmod) to the left and right vectors.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>result</returns>
    public static Vec4 operator %(in Vec4 a, in Vec4 b)
    {
        return Vec4.RemTrunc(a, b);
    }

    /// <summary>
    /// Applies truncation-based modulo (fmod) to the left and right operands.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>result</returns>
    public static Vec4 operator %(in Vec4 a, in float b)
    {
        if (b != 0.0f)
        {
            return new Vec4(
                a._x % b,
                a._y % b,
                a._z % b,
                a._w % b);
        }
        return a;
    }

    /// <summary>
    /// Applies truncation-based modulo (fmod) to the left and right operands.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>result</returns>
    public static Vec4 operator %(in float a, in Vec4 b)
    {
        return new Vec4(
            Utils.RemTrunc(a, b._x),
            Utils.RemTrunc(a, b._y),
            Utils.RemTrunc(a, b._z),
            Utils.RemTrunc(a, b._w));
    }

    /// <summary>
    /// Adds two vectors together.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>sum</returns>
    public static Vec4 operator +(in Vec4 a, in Vec4 b)
    {
        return new Vec4(
            a._x + b._x,
            a._y + b._y,
            a._z + b._z,
            a._w + b._w);
    }

    /// <summary>
    /// Subtracts the right vector from the left vector.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>difference</returns>
    public static Vec4 operator -(in Vec4 a, in Vec4 b)
    {
        return new Vec4(
            a._x - b._x,
            a._y - b._y,
            a._z - b._z,
            a._w - b._w);
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
        return new Vec4(
            a._x < b._x,
            a._y < b._y,
            a._z < b._z,
            a._w < b._w);
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
        return new Vec4(
            a._x > b._x,
            a._y > b._y,
            a._z > b._z,
            a._w > b._w);
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
        return new Vec4(
            a._x <= b._x,
            a._y <= b._y,
            a._z <= b._z,
            a._w <= b._w);
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
        return new Vec4(
            a._x >= b._x,
            a._y >= b._y,
            a._z >= b._z,
            a._w >= b._w);
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
        return new Vec4(
            a._x != b._x,
            a._y != b._y,
            a._z != b._z,
            a._w != b._w);
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
        return new Vec4(
            a._x == b._x,
            a._y == b._y,
            a._z == b._z,
            a._w == b._w);
    }

    /// <summary>
    /// Finds the absolute value of each vector component.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>absolute vector</returns>
    public static Vec4 Abs(in Vec4 v)
    {
        return new Vec4(
            MathF.Abs(v._x),
            MathF.Abs(v._y),
            MathF.Abs(v._z),
            MathF.Abs(v._w));
    }

    /// <summary>
    /// Tests to see if all the vector's components are non-zero. Useful when
    /// testing valid dimensions (width and depth) stored in vectors.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>evaluation</returns>
    public static bool All(in Vec4 v)
    {
        return v._x != 0.0f &&
            v._y != 0.0f &&
            v._z != 0.0f &&
            v._w != 0.0f;
    }

    /// <summary>
    /// Tests to see if any of the vector's components are non-zero.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>evaluation</returns>
    public static bool Any(in Vec4 v)
    {
        return v._x != 0.0f ||
            v._y != 0.0f ||
            v._z != 0.0f ||
            v._w != 0.0f;
    }

    /// <summary>
    /// Tests to see if two vectors approximate each other.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <param name="tol">the tolerance</param>
    /// <returns>evaluation</returns>
    public static bool Approx(in Vec4 a, in Vec4 b, in float tol = Utils.Epsilon)
    {
        return Utils.Approx(a._x, b._x, tol) &&
            Utils.Approx(a._y, b._y, tol) &&
            Utils.Approx(a._z, b._z, tol) &&
            Utils.Approx(a._w, b._w, tol);
    }

    /// <summary>
    /// Tests to see if a vector has, approximately, the specified magnitude.
    /// </summary>
    /// <param name="a">vector</param>
    /// <param name="b">the magnitude</param>
    /// <param name="tol">the tolerance</param>
    /// <returns>evaluation</returns>
    public static bool ApproxMag(in Vec4 a, in float b = 1.0f, in float tol = Utils.Epsilon)
    {
        return Utils.Approx(Vec4.MagSq(a), b * b, tol);
    }

    /// <summary>
    /// Raises each component of the vector to the nearest greater integer.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>result</returns>
    public static Vec4 Ceil(in Vec4 v)
    {
        return new Vec4(
            MathF.Ceiling(v._x),
            MathF.Ceiling(v._y),
            MathF.Ceiling(v._z),
            MathF.Ceiling(v._w));
    }

    /// <summary>
    /// Clamps a vector to a range within the lower and upper bound.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="lb">the range lower bound</param>
    /// <param name="ub">the range upper bound</param>
    /// <returns>clamped vector</returns>
    public static Vec4 Clamp(in Vec4 v, in float lb = 0.0f, in float ub = 1.0f)
    {
        return new Vec4(
            Utils.Clamp(v._x, lb, ub),
            Utils.Clamp(v._y, lb, ub),
            Utils.Clamp(v._z, lb, ub),
            Utils.Clamp(v._w, lb, ub));
    }

    /// <summary>
    /// Tests to see if the vector contains a value
    /// </summary>
    /// <param name="a">the vector</param>
    /// <param name="b">the value</param>
    /// <returns>evaluation</returns>
    public static bool Contains(in Vec4 a, in float b)
    {
        return Utils.Approx(a._x, b) ||
            Utils.Approx(a._y, b) ||
            Utils.Approx(a._z, b) ||
            Utils.Approx(a._w, b);
    }

    /// <summary>
    /// Finds first vector argument with the sign of the second vector argument.
    /// Returns zero where the sign is zero.
    /// </summary>
    /// <param name="a">the magnitude</param>
    /// <param name="b">the sign</param>
    /// <returns>signed vector</returns>
    public static Vec4 CopySign(in Vec4 a, in Vec4 b)
    {
        return new Vec4(
            Utils.CopySign(a._x, b._x),
            Utils.CopySign(a._y, b._y),
            Utils.CopySign(a._z, b._z),
            Utils.CopySign(a._w, b._w));
    }

    /// <summary>
    /// Finds the absolute value of the difference between two vectors.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>absolute difference</returns>
    public static Vec4 Diff(in Vec4 a, in Vec4 b)
    {
        return new Vec4(
            Utils.Diff(b._x, a._x),
            Utils.Diff(b._y, a._y),
            Utils.Diff(b._z, a._z),
            Utils.Diff(b._w, a._w));
    }

    /// <summary>
    /// Finds the Chebyshev distance between two vectors.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>distance</returns>
    public static float DistChebyshev(in Vec4 a, in Vec4 b)
    {
        return Utils.Max(Utils.Diff(b._x, a._x),
                         Utils.Diff(b._y, a._y),
                         Utils.Diff(b._z, a._z),
                         Utils.Diff(b._w, a._w));
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
        return Utils.Diff(b._x, a._x) +
               Utils.Diff(b._y, a._y) +
               Utils.Diff(b._z, a._z) +
               Utils.Diff(b._w, a._w);
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
    public static float DistMinkowski(in Vec4 a, in Vec4 b, in float c = 2.0f)
    {
        if (c != 0.0f)
        {
            float dx = MathF.Pow(Utils.Diff(b._x, a._x), c);
            float dy = MathF.Pow(Utils.Diff(b._y, a._y), c);
            float dz = MathF.Pow(Utils.Diff(b._z, a._z), c);
            float dw = MathF.Pow(Utils.Diff(b._w, a._w), c);
            return MathF.Pow(dx + dy + dz + dw, 1.0f / c);
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
        float dx = b._x - a._x;
        float dy = b._y - a._y;
        float dz = b._z - a._z;
        float dw = b._w - a._w;
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
        return a._x * b._x +
            a._y * b._y +
            a._z * b._z +
            a._w * b._w;
    }

    /// <summary>
    /// Floors each component of the vector.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>floor</returns>
    public static Vec4 Floor(in Vec4 v)
    {
        return new Vec4(
            MathF.Floor(v._x),
            MathF.Floor(v._y),
            MathF.Floor(v._z),
            MathF.Floor(v._w));
    }

    /// <summary>
    /// Returns the fractional portion of the vector's components.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>fractional portion</returns>
    public static Vec4 Fract(in Vec4 v)
    {
        return new Vec4(
            Utils.Fract(v._x),
            Utils.Fract(v._y),
            Utils.Fract(v._z),
            Utils.Fract(v._w));
    }

    /// <summary>
    /// Generates a 4D array of vectors.
    /// </summary>
    /// <returns>array</returns>
    public static Vec4[,,,] Grid()
    {
        return Vec4.GridCartesian(
            new Vec4(-1.0f, -1.0f, -1.0f, -1.0f),
            new Vec4(1.0f, 1.0f, 1.0f, 1.0f));
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
        int sval = steps < 2 ? 2 : steps;
        int lval = layers < 2 ? 2 : layers;
        int rval = rows < 2 ? 2 : rows;
        int cval = cols < 2 ? 2 : cols;

        float gToStep = 1.0f / (sval - 1.0f);
        float hToStep = 1.0f / (lval - 1.0f);
        float iToStep = 1.0f / (rval - 1.0f);
        float jToStep = 1.0f / (cval - 1.0f);

        Vec4[,,,] result = new Vec4[sval, lval, rval, cval];

        int rcval = rval * cval;
        int lrcval = lval * rcval;
        int len4 = sval * lrcval;
        for (int k = 0; k < len4; ++k)
        {
            int g = k / lrcval;
            int m = k - g * lrcval;
            int h = m / rcval;
            int n = m - h * rcval;
            int i = n / cval;
            int j = n % cval;

            result[g, h, i, j] = Vec4.Mix(
                lowerBound,
                upperBound,
                new Vec4(
                    (float)j * jToStep,
                    (float)i * iToStep,
                    (float)h * hToStep,
                    (float)g * gToStep));
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
        return new Vec4(
            Utils.LinearStep(edge0._x, edge1._x, x._x),
            Utils.LinearStep(edge0._y, edge1._y, x._y),
            Utils.LinearStep(edge0._z, edge1._z, x._z),
            Utils.LinearStep(edge0._w, edge1._w, x._w));
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
        return v._x * v._x +
            v._y * v._y +
            v._z * v._z +
            v._w * v._w;
    }

    /// <summary>
    /// Finds the maximum of two vectors by component.
    /// </summary>
    /// <param name="a">the input value</param>
    /// <param name="b">upper bound</param>
    /// <returns>maximum value</returns>
    public static Vec4 Max(in Vec4 a, in Vec4 b)
    {
        return new Vec4(
            MathF.Max(a._x, b._x),
            MathF.Max(a._y, b._y),
            MathF.Max(a._z, b._z),
            MathF.Max(a._w, b._w));
    }

    /// <summary>
    /// Finds the minimum of two vectors by component.
    /// </summary>
    /// <param name="a">the input value</param>
    /// <param name="b">lower bound</param>
    /// <returns>minimum value</returns>
    public static Vec4 Min(in Vec4 a, in Vec4 b)
    {
        return new Vec4(
            MathF.Min(a._x, b._x),
            MathF.Min(a._y, b._y),
            MathF.Min(a._z, b._z),
            MathF.Min(a._w, b._w));
    }

    /// <summary>
    /// Mixes two vectors together. Adds the vectors then divides by half.
    /// </summary>
    /// <param name="a">original vector</param>
    /// <param name="b">destination vector</param>
    /// <returns>mix</returns>
    public static Vec4 Mix(in Vec4 a, in Vec4 b)
    {
        return new Vec4(
            0.5f * (a._x + b._x),
            0.5f * (a._y + b._y),
            0.5f * (a._z + b._z),
            0.5f * (a._w + b._w));
    }

    /// <summary>
    /// Mixes two vectors together by a step. Uses the formula (1.0 - t) a + t b
    /// . The step is unclamped.
    /// </summary>
    /// <param name="a">original vector</param>
    /// <param name="b">destination vector</param>
    /// <param name="t">step</param>
    /// <returns>mix</returns>
    public static Vec4 Mix(in Vec4 a, in Vec4 b, in float t)
    {
        float u = 1.0f - t;
        return new Vec4(
            u * a._x + t * b._x,
            u * a._y + t * b._y,
            u * a._z + t * b._z,
            u * a._w + t * b._w);
    }

    /// <summary>
    /// Mixes two vectors together by a step. Uses the formula (1.0 - t) a + t b
    /// . The step is unclamped; to find an appropriate clamped step, use mix in
    /// conjunction with step, linearstep or smoothstep.
    /// </summary>
    /// <param name="a">original vector</param>
    /// <param name="b">destination vector</param>
    /// <param name="t">step</param>
    /// <returns>mix</returns>
    public static Vec4 Mix(in Vec4 a, in Vec4 b, in Vec4 t)
    {
        return new Vec4(
            Utils.Mix(a._x, b._x, t._x),
            Utils.Mix(a._y, b._y, t._y),
            Utils.Mix(a._z, b._z, t._z),
            Utils.Mix(a._w, b._w, t._w));
    }

    /// <summary>
    /// Tests to see if all the vector's components are zero. Useful when
    /// safeguarding against invalid directions.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>evaluation</returns>
    public static bool None(in Vec4 v)
    {
        return v._x == 0.0f &&
            v._y == 0.0f &&
            v._z == 0.0f &&
            v._w == 0.0f;
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
        return v / Vec4.Mag(v);
    }

    /// <summary>
    /// Evaluates a vector like a boolean, where n != 0.0 is true.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>truth table opposite</returns>
    public static Vec4 Not(in Vec4 v)
    {
        return new Vec4(
            v._x != 0.0f ? 0.0f : 1.0f,
            v._y != 0.0f ? 0.0f : 1.0f,
            v._z != 0.0f ? 0.0f : 1.0f,
            v._w != 0.0f ? 0.0f : 1.0f);
    }

    /// <summary>
    /// Raises a vector to the power of another vector.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>result</returns>
    public static Vec4 Pow(in Vec4 a, in Vec4 b)
    {
        return new Vec4(
            MathF.Pow(a._x, b._x),
            MathF.Pow(a._y, b._y),
            MathF.Pow(a._z, b._z),
            MathF.Pow(a._w, b._w));
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
        return new Vec4(v.x, v.y, z, w);
    }

    /// <summary>
    /// Promotes a 3D vector to a 4D vector.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="w">w component</param>
    /// <returns>vector</returns>
    public static Vec4 Promote(in Vec3 v, in float w = 0.0f)
    {
        return new Vec4(v.x, v.y, v.z, w);
    }

    /// <summary>
    /// Reduces the signal, or granularity, of a vector's components.
    /// </summary>
    /// <param name="v">input vector</param>
    /// <param name="levels">levels</param>
    /// <returns>quantized vector</returns>
    public static Vec4 Quantize(in Vec4 v, in int levels = 8)
    {
        return new Vec4(
            Utils.QuantizeSigned(v._x, levels),
            Utils.QuantizeSigned(v._y, levels),
            Utils.QuantizeSigned(v._z, levels),
            Utils.QuantizeSigned(v._w, levels));
    }

    /// <summary>
    /// Creates a random point in the Cartesian coordinate system given a lower
    /// and an upper bound.
    /// </summary>
    /// <param name="rng">the random number generator</param>
    /// <param name="lb">lower bound</param>
    /// <param name="ub">upper bound</param>
    /// <returns>random vector</returns>
    public static Vec4 RandomCartesian(
        in System.Random rng,
        in Vec4 lb,
        in Vec4 ub)
    {
        return new Vec4(
            Utils.Mix(lb._x, ub._x, (float)rng.NextDouble()),
            Utils.Mix(lb._y, ub._y, (float)rng.NextDouble()),
            Utils.Mix(lb._z, ub._z, (float)rng.NextDouble()),
            Utils.Mix(lb._w, ub._w, (float)rng.NextDouble()));
    }

    /// <summary>
    /// Creates a random point in the Cartesian coordinate system given a lower
    /// and an upper bound.
    /// </summary>
    /// <param name="rng">the random number generator</param>
    /// <param name="lb">lower bound</param>
    /// <param name="ub">upper bound</param>
    /// <returns>random vector</returns>
    public static Vec4 RandomCartesian(
        in System.Random rng,
        in float lb = 0.0f,
        in float ub = 1.0f)
    {
        return new Vec4(
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
        return i - ((2.0f * Vec4.Dot(n, i)) * n);
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
        return new Vec4(
            Utils.Remap(v._x, lbOrigin._x, ubOrigin._x, lbDest._x, ubDest._x),
            Utils.Remap(v._y, lbOrigin._y, ubOrigin._y, lbDest._y, ubDest._y),
            Utils.Remap(v._z, lbOrigin._z, ubOrigin._z, lbDest._z, ubDest._z),
            Utils.Remap(v._w, lbOrigin._w, ubOrigin._w, lbDest._w, ubDest._w));
    }

    /// <summary>
    /// Mods each component of the left vector by those of the right.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>result</returns>
    public static Vec4 RemFloor(in Vec4 a, in Vec4 b)
    {
        return new Vec4(
            Utils.RemFloor(a._x, b._x),
            Utils.RemFloor(a._y, b._y),
            Utils.RemFloor(a._z, b._z),
            Utils.RemFloor(a._w, b._w));
    }

    /// <summary>
    /// Applies the % operator (truncation-based modulo) to the left operand.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>result</returns>
    public static Vec4 RemTrunc(in Vec4 a, in Vec4 b)
    {
        return new Vec4(
            Utils.RemTrunc(a._x, b._x),
            Utils.RemTrunc(a._y, b._y),
            Utils.RemTrunc(a._z, b._z),
            Utils.RemTrunc(a._w, b._w));
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
        return new Vec4(
            Utils.Round(v._x),
            Utils.Round(v._y),
            Utils.Round(v._z),
            Utils.Round(v._w));
    }

    /// <summary>
    /// Rounds each component of the vector to a specified number of places.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="places">the number of places</param>
    /// <returns>rounded vector</returns>
    public static Vec4 Round(in Vec4 v, in int places)
    {
        return new Vec4(
            MathF.Round(v._x, places),
            MathF.Round(v._y, places),
            MathF.Round(v._z, places),
            MathF.Round(v._w, places));
    }

    /// <summary>
    /// Finds the sign of the vector: -1, if negative; 1, if positive.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>sign</returns>
    public static Vec4 Sign(in Vec4 v)
    {
        return new Vec4(
            MathF.Sign(v._x),
            MathF.Sign(v._y),
            MathF.Sign(v._z),
            MathF.Sign(v._w));
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
        return new Vec4(
            Utils.SmoothStep(edge0._x, edge1._x, x._x),
            Utils.SmoothStep(edge0._y, edge1._y, x._y),
            Utils.SmoothStep(edge0._z, edge1._z, x._z),
            Utils.SmoothStep(edge0._w, edge1._w, x._w));
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
        return new Vec4(
            Utils.Step(edge._x, x._x),
            Utils.Step(edge._y, x._y),
            Utils.Step(edge._z, x._z),
            Utils.Step(edge._w, x._w));
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
        sb.Append("{ x: ");
        Utils.ToFixed(sb, v._x, places);
        sb.Append(", y: ");
        Utils.ToFixed(sb, v._y, places);
        sb.Append(", z: ");
        Utils.ToFixed(sb, v._z, places);
        sb.Append(", w: ");
        Utils.ToFixed(sb, v._w, places);
        sb.Append(' ');
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
        return new Vec4(
            MathF.Truncate(v._x),
            MathF.Truncate(v._y),
            MathF.Truncate(v._z),
            MathF.Truncate(v._w));
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
        return new Vec4(
            Utils.Wrap(v._x, lb._x, ub._x),
            Utils.Wrap(v._y, lb._y, ub._y),
            Utils.Wrap(v._z, lb._z, ub._z),
            Utils.Wrap(v._w, lb._w, ub._w));
    }

    /// <summary>
    /// Returns a direction facing back, (0.0, -1.0, 0.0, 0.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec4 Back { get { return new Vec4(0.0f, -1.0f, 0.0f, 0.0f); } }

    /// <summary>
    /// Returns a direction facing down, (0.0, 0.0, -1.0, 0.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec4 Down { get { return new Vec4(0.0f, 0.0f, -1.0f, 0.0f); } }

    /// <summary>
    /// Returns a direction facing forward, (0.0, 1.0, 0.0, 0.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec4 Forward { get { return new Vec4(0.0f, 1.0f, 0.0f, 0.0f); } }

    /// <summary>
    /// Returns a direction facing left, (-1.0, 0.0, 0.0, 0.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec4 Left { get { return new Vec4(-1.0f, 0.0f, 0.0f, 0.0f); } }

    /// <summary>
    /// Returns a vector with all components set to 1.0 .
    /// </summary>
    /// <value>the vector</value>
    public static Vec4 One { get { return new Vec4(1.0f, 1.0f, 1.0f, 1.0f); } }

    /// <summary>
    /// Returns a direction facing right, (1.0, 0.0, 0.0, 0.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec4 Right { get { return new Vec4(1.0f, 0.0f, 0.0f, 0.0f); } }

    /// <summary>
    /// Returns a direction facing up, (0.0, 0.0, 1.0, 0.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec4 Up { get { return new Vec4(0.0f, 0.0f, 1.0f, 0.0f); } }

    /// <summary>
    /// Returns a vector with all components set to zero.
    /// </summary>
    /// <value>the vector</value>
    public static Vec4 Zero { get { return new Vec4(0.0f, 0.0f, 0.0f, 0.0f); } }
}