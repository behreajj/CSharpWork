using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;

/// <summary>
/// A readonly struct influenced by GLSL, OSL and Processing's PVector. This is
/// intended for storing points and directions in three-dimensional graphics
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
    /// /// <value>the length</value>
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
    /// <returns>2D vector</returns>
    public Vec2 xy
    {
        get
        {
            return new Vec2 (this._x, this._y);
        }
    }

    /// <summary>
    /// Gets the x and z components as a 2D vector.
    /// </summary>
    /// <returns>2D vector</returns>
    public Vec2 xz
    {
        get
        {
            return new Vec2 (this._x, this._z);
        }
    }

    /// <summary>
    /// Retrieves a component by index. When the provided index is 2 or -1,
    /// returns z; 1 or -2, y; 0 or -3, x.
    /// </summary>
    /// <value>the component</value>
    public float this [int i]
    {
        get
        {
            switch (i)
            {
                case 0:
                case -3:
                    return this._x;
                case 1:
                case -2:
                    return this._y;
                case 2:
                case -1:
                    return this._z;
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
    public Vec3 (in float x, in float y, in float z)
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
    public Vec3 (in bool x, in bool y, in bool z)
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
    /// <returns>the equivalence</returns>
    public override bool Equals (object value)
    {
        if (Object.ReferenceEquals (this, value)) return true;
        if (Object.ReferenceEquals (null, value)) return false;
        if (value is Vec3) return this.Equals ((Vec3) value);
        return false;
    }

    /// <summary>
    /// Returns a hash code representing this vector.
    /// </summary>
    /// <returns>the hash code</returns>
    public override int GetHashCode ( )
    {
        unchecked
        {
            return ((Utils.MulBase ^ this._x.GetHashCode ( )) *
                    Utils.HashMul ^ this._y.GetHashCode ( )) *
                Utils.HashMul ^ this._z.GetHashCode ( );
        }
    }

    /// <summary>
    /// Returns a string representation of this vector.
    /// </summary>
    /// <returns>the string</returns>
    public override string ToString ( )
    {
        return Vec3.ToString (this);
    }

    /// <summary>
    /// Compares this vector to another in compliance with the IComparable
    /// interface. Returns 1 when a component of this vector is greater than
    /// another; -1 when lesser. Prioritizes the highest dimension first: z, y,
    /// x . Returns 0 as a last resort.
    /// </summary>
    /// <param name="v">the comparisand</param>
    /// <returns>the evaluation</returns>
    public int CompareTo (Vec3 v)
    {
        return (this._z > v._z) ? 1 :
            (this._z < v._z) ? -1 :
            (this._y > v._y) ? 1 :
            (this._y < v._y) ? -1 :
            (this._x > v._x) ? 1 :
            (this._x < v._x) ? -1 :
            0;
    }

    /// <summary>
    /// Tests this vector for equivalence with another in compliance with the
    /// IEquatable interface. For approximate equality with another vector, use
    /// the static approx function instead.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>the equivalence</returns>
    public bool Equals (Vec3 v)
    {
        if (this._z.GetHashCode ( ) != v._z.GetHashCode ( )) return false;
        if (this._y.GetHashCode ( ) != v._y.GetHashCode ( )) return false;
        if (this._x.GetHashCode ( ) != v._x.GetHashCode ( )) return false;
        return true;
    }

    /// <summary>
    /// Returns an enumerator (or iterator) for this vector, allowing its
    /// components to be accessed in a foreach loop.
    /// </summary>
    /// <returns>the enumerator</returns>
    public IEnumerator GetEnumerator ( )
    {
        yield return this._x;
        yield return this._y;
        yield return this._z;
    }

    /// <summary>
    /// Returns a float array of length 3 containing this vector's components.
    /// </summary>
    /// <returns>the array</returns>
    public float[ ] ToArray ( )
    {
        return this.ToArray (new float[this.Length], 0);
    }

    /// <summary>
    /// Puts this vector's components into an array at a given index.
    /// </summary>
    /// <param name="arr">array</param>
    /// <param name="i">index</param>
    /// <returns>array</returns>
    public float[ ] ToArray (in float[ ] arr, in int i = 0)
    {
        arr[i] = this._x;
        arr[i + 1] = this._y;
        arr[i + 2] = this._z;
        return arr;
    }

    /// <summary>
    /// Returns a named value tuple containing this vector's components.
    /// </summary>
    /// <returns>the tuple</returns>
    public (float x, float y, float z) ToTuple ( )
    {
        return (x: this._x, y: this._y, z: this._z);
    }

    /// <summary>
    /// Converts a boolean to a vector by supplying the boolean to all the
    /// vector's components: 1.0 for true; 0.0 for false.
    /// </summary>
    /// <param name="b">the boolean</param>
    public static implicit operator Vec3 (in bool b)
    {
        float eval = b ? 1.0f : 0.0f;
        return new Vec3 (eval, eval, eval);
    }

    /// <summary>
    /// Converts a float to a vector by supplying the scalar to all the vector's
    /// components.
    /// </summary>
    /// <param name="s">the scalar</param>
    public static implicit operator Vec3 (in float s)
    {
        return new Vec3 (s, s, s);
    }

    /// <summary>
    /// Promotes a 2D vector to a 3D vector; the z component is assumed to be
    /// 0.0 .
    /// </summary>
    /// <param name="v">the 2D vector</param>
    public static implicit operator Vec3 (in Vec2 v)
    {
        return Vec3.Promote (v, 0.0f);
    }

    /// <summary>
    /// Converts a vector to a boolean by finding whether all of its components
    /// are non-zero.
    /// </summary>
    /// <param name="v">the input vector</param>
    public static explicit operator bool (in Vec3 v)
    {
        return Vec3.All (v);
    }

    /// <summary>
    /// Converts a vector to a float by finding its magnitude.
    /// </summary>
    /// <param name="v">the input vector</param>
    public static explicit operator float (in Vec3 v)
    {
        return Vec3.Mag (v);
    }

    /// <summary>
    /// A vector evaluates to true when all of its components are not equal to
    /// zero.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the evaluation</returns>
    public static bool operator true (in Vec3 v)
    {
        return Vec3.All (v);
    }

    /// <summary>
    /// A vector evaluates to false when all of its components are equal to
    /// zero.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the evaluation</returns>
    public static bool operator false (in Vec3 v)
    {
        return Vec3.None (v);
    }

    /// <summary>
    /// Evaluates a vector as a boolean. Equivalent to using the ~ operator.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the opposite</returns>
    public static Vec3 operator ! (in Vec3 v)
    {
        return Vec3.Not (v);
    }

    /// <summary>
    /// Evaluates a vector as a boolean. Equivalent to using the ! operator.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the complement</returns>
    public static Vec3 operator ~ (in Vec3 v)
    {
        return Vec3.Not (v);
    }

    /// <summary>
    /// Evaluates two vectors like booleans, using the inclusive and (AND) logic
    /// gate.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the evaluation</returns>
    public static Vec3 operator & (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (
            Utils.And (a._x, b._x),
            Utils.And (a._y, b._y),
            Utils.And (a._z, b._z));
    }

    /// <summary>
    /// Evaluates two vectors like booleans, using the inclusive or (OR) logic
    /// gate.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the evaluation</returns>
    public static Vec3 operator | (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (
            Utils.Or (a._x, b._x),
            Utils.Or (a._y, b._y),
            Utils.Or (a._z, b._z));
    }

    /// <summary>
    /// Evaluates two vectors like booleans, using the exclusive or (XOR) logic
    /// gate.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the evaluation</returns>
    public static Vec3 operator ^ (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (
            Utils.Xor (a._x, b._x),
            Utils.Xor (a._y, b._y),
            Utils.Xor (a._z, b._z));
    }

    /// <summary>
    /// Negates the input vector
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the negation</returns>
    public static Vec3 operator - (in Vec3 v)
    {
        return new Vec3 (-v._x, -v._y, -v._z);
    }

    /// <summary>
    /// Multiplies two vectors, component-wise, i.e.,
    /// returns the Hadamard product.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the product</returns>
    public static Vec3 operator * (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (
            a._x * b._x,
            a._y * b._y,
            a._z * b._z);
    }

    /// <summary>
    /// Multiplies a vector by a scalar.
    /// </summary>
    /// <param name="a">left operand, the vector</param>
    /// <param name="b">right operand, the scalar</param>
    /// <returns>the product</returns>
    public static Vec3 operator * (in Vec3 a, in float b)
    {
        return new Vec3 (
            a._x * b,
            a._y * b,
            a._z * b);
    }

    /// <summary>
    /// Multiplies a vector by a scalar.
    /// </summary>
    /// <param name="a">left operand, the scalar</param>
    /// <param name="b">right operand, the vector</param>
    /// <returns>the product</returns>
    public static Vec3 operator * (in float a, in Vec3 b)
    {
        return new Vec3 (
            a * b._x,
            a * b._y,
            a * b._z);
    }

    /// <summary>
    /// Divides the left operand by the right, component-wise. This is
    /// mathematically incorrect, but serves as a shortcut for transforming a
    /// vector by the inverse of a scalar matrix.
    /// </summary>
    /// <param name="a">numerator</param>
    /// <param name="b">denominator</param>
    /// <returns>the quotient</returns>
    public static Vec3 operator / (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (
            Utils.Div (a._x, b._x),
            Utils.Div (a._y, b._y),
            Utils.Div (a._z, b._z));
    }

    /// <summary>
    /// Divides a vector by a scalar.
    /// </summary>
    /// <param name="a">vector, numerator</param>
    /// <param name="b">scalar, denominator</param>
    /// <returns>the product</returns>
    public static Vec3 operator / (in Vec3 a, in float b)
    {
        if (b != 0.0f)
        {
            float bInv = 1.0f / b;
            return new Vec3 (
                a._x * bInv,
                a._y * bInv,
                a._z * bInv);
        }
        return new Vec3 ( );
    }

    /// <summary>
    /// Divides a scalar by a vector.
    /// </summary>
    /// <param name="a">scalar, numerator</param>
    /// <param name="b">vector, denominator</param>
    /// <returns>the product</returns>
    public static Vec3 operator / (in float a, in Vec3 b)
    {
        return new Vec3 (
            Utils.Div (a, b._x),
            Utils.Div (a, b._y),
            Utils.Div (a, b._z));
    }

    /// <summary>
    /// Applies truncation-based modulo (fmod) to the left and right vectors.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the result</returns>
    public static Vec3 operator % (in Vec3 a, in Vec3 b)
    {
        return Vec3.Fmod (a, b);
    }

    /// <summary>
    /// Applies truncation-based modulo (fmod) to the left and right operands.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the result</returns>
    public static Vec3 operator % (in Vec3 a, in float b)
    {
        if (b != 0.0f) return new Vec3 (a._x % b, a._y % b, a._z % b);
        return a;
    }

    /// <summary>
    /// Applies truncation-based modulo (fmod) to the left and right operands.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the result</returns>
    public static Vec3 operator % (in float a, in Vec3 b)
    {
        return new Vec3 (
            Utils.Fmod (a, b._x),
            Utils.Fmod (a, b._y),
            Utils.Fmod (a, b._z));
    }

    /// <summary>
    /// Adds two vectors together.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the sum</returns>
    public static Vec3 operator + (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (
            a._x + b._x,
            a._y + b._y,
            a._z + b._z);
    }

    /// <summary>
    /// Subtracts the right vector from the left vector.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the difference</returns>
    public static Vec3 operator - (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (
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
    /// <returns>the evaluation</returns>
    public static Vec3 operator < (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (
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
    /// <returns>the evaluation</returns>
    public static Vec3 operator > (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (
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
    /// <returns>the evaluation</returns>
    public static Vec3 operator <= (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (
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
    /// <returns>the evaluation</returns>
    public static Vec3 operator >= (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (
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
    /// <returns>the evaluation</returns>
    public static Vec3 operator != (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (
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
    /// <returns>the evaluation</returns>
    public static Vec3 operator == (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (
            a._x == b._x,
            a._y == b._y,
            a._z == b._z);
    }

    /// <summary>
    /// Finds the absolute value of each vector component.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the absolute vector</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 Abs (in Vec3 v)
    {
        return new Vec3 (
            Utils.Abs (v._x),
            Utils.Abs (v._y),
            Utils.Abs (v._z));
    }

    /// <summary>
    /// Tests to see if all the vector's components are non-zero. Useful when
    /// testing valid dimensions (width and depth) stored in vectors.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the evaluation</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static bool All (in Vec3 v)
    {
        return v._x != 0.0f &&
            v._y != 0.0f &&
            v._z != 0.0f;
    }

    /// <summary>
    /// Finds the angle between two vectors.
    /// </summary>
    /// <param name="a">the first vector</param>
    /// <param name="b">the second vector</param>
    /// <returns>the angle</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float AngleBetween (in Vec3 a, in Vec3 b)
    {
        return Vec3.Any (a) && Vec3.Any (b) ?
            Utils.Acos (Vec3.Dot (a, b) /
                (Vec3.Mag (a) * Vec3.Mag (b))) :
            0.0f;
    }

    /// <summary>
    /// Tests to see if any of the vector's components are non-zero.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the evaluation</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static bool Any (in Vec3 v)
    {
        return v._x != 0.0f ||
            v._y != 0.0f ||
            v._z != 0.0f;
    }

    /// <summary>
    /// Appends a vector to a one-dimensional vector array. Returns a new array.
    /// </summary>
    /// <param name="a">array</param>
    /// <param name="b">vector</param>
    /// <returns>the array</returns>
    public static Vec3[ ] Append (in Vec3[ ] a, in Vec3 b)
    {
        bool aNull = a == null;
        if (aNull) { return new Vec3[ ] { b }; }
        int aLen = a.Length;
        Vec3[ ] result = new Vec3[aLen + 1];
        System.Array.Copy (a, 0, result, 0, aLen);
        result[aLen] = b;
        return result;
    }

    /// <summary>
    /// Tests to see if two vectors approximate each other.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <param name="tolerance">the tolerance</param>
    /// <returns>the evaluation</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static bool Approx (in Vec3 a, in Vec3 b, in float tolerance = Utils.Epsilon)
    {
        return Utils.Approx (a._x, b._x, tolerance) &&
            Utils.Approx (a._y, b._y, tolerance) &&
            Utils.Approx (a._z, b._z, tolerance);
    }

    /// <summary>
    /// Tests to see if a vector has, approximately, the specified magnitude.
    /// </summary>
    /// <param name="a">the input vector</param>
    /// <param name="b">the magnitude</param>
    /// <param name="tolerance">the tolerance</param>
    /// <returns>the evaluation</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static bool ApproxMag (in Vec3 a, in float b = 1.0f, in float tolerance = Utils.Epsilon)
    {
        return Utils.Approx (Vec3.MagSq (a), b * b, tolerance);
    }

    /// <summary>
    /// Tests to see if two vectors are parallel.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>the evaluation</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static bool AreParallel (in Vec3 a, in Vec3 b, in float tolerance = Utils.Epsilon)
    {
        return (Utils.Abs (a._y * b._z - a._z * b._y) <= tolerance) &&
            (Utils.Abs (a._z * b._x - a._x * b._z) <= tolerance) &&
            (Utils.Abs (a._x * b._y - a._y * b._x) <= tolerance);
    }

    /// <summary>
    /// Finds the vector's azimuth in the range [-PI, PI] .
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the angle in radians</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float AzimuthSigned (in Vec3 v)
    {
        return Utils.Atan2 (v._y, v._x);
    }

    /// <summary>
    /// Finds the vector's azimuth in the range [0.0, TAU] .
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the angle in radians</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float AzimuthUnsigned (in Vec3 v)
    {
        return Utils.ModRadians (Vec3.AzimuthSigned (v));
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
    /// <returns>the point along the curve</returns>
    public static Vec3 BezierPoint ( //
        in Vec3 ap0, //
        in Vec3 cp0, //
        in Vec3 cp1, //
        in Vec3 ap1, //
        in float step)
    {
        if (step <= 0.0f) return new Vec3 (ap0._x, ap0._y, ap0._z);
        else if (step >= 1.0f) return new Vec3 (ap1._x, ap1._y, ap1._z);

        float u = 1.0f - step;
        float tcb = step * step;
        float ucb = u * u;
        float usq3t = ucb * (step + step + step);
        float tsq3u = tcb * (u + u + u);
        ucb *= u;
        tcb *= step;

        return new Vec3 (
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
    /// <returns>the tangent along the curve</returns>
    public static Vec3 BezierTangent ( //
        in Vec3 ap0, //
        in Vec3 cp0, //
        in Vec3 cp1, //
        in Vec3 ap1, //
        in float step)
    {
        if (step <= 0.0f) return cp0 - ap0;
        else if (step >= 1.0f) return ap1 - cp1;

        float u = 1.0f - step;
        float t3 = step + step + step;
        float usq3 = u * (u + u + u);
        float tsq3 = step * t3;
        float ut6 = u * (t3 + t3);

        return new Vec3 (
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
    /// <returns>the tangent along the curve</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 BezierTanUnit ( //
        in Vec3 ap0, //
        in Vec3 cp0, //
        in Vec3 cp1, //
        in Vec3 ap1, //
        in float step)
    {
        return Vec3.Normalize (Vec3.BezierTangent (
            ap0, cp0, cp1, ap1, step));
    }

    /// <summary>
    /// Raises each component of the vector to the nearest greater integer.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the result</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 Ceil (in Vec3 v)
    {
        return new Vec3 (
            Utils.Ceil (v._x),
            Utils.Ceil (v._y),
            Utils.Ceil (v._z));
    }

    /// <summary>
    /// Clamps a vector to a range within the lower and upper bound.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <param name="lb">the range lower bound</param>
    /// <param name="ub">the range upper bound</param>
    /// <returns>the clamped vector</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 Clamp (in Vec3 v, in float lb = 0.0f, in float ub = 1.0f)
    {
        return new Vec3 (
            Utils.Clamp (v._x, lb, ub),
            Utils.Clamp (v._y, lb, ub),
            Utils.Clamp (v._z, lb, ub));
    }

    /// <summary>
    /// Clamps a vector to a range within the lower and upper bound.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <param name="lb">the range lower bound</param>
    /// <param name="ub">the range upper bound</param>
    /// <returns>the clamped vector</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 Clamp (in Vec3 v, in Vec3 lb, in Vec3 ub)
    {
        return new Vec3 (
            Utils.Clamp (v._x, lb._x, ub._x),
            Utils.Clamp (v._y, lb._y, ub._y),
            Utils.Clamp (v._z, lb._z, ub._z));
    }

    /// <summary>
    /// Concatenates two one-dimensional Vec2 arrays.
    /// </summary>
    /// <param name="a">left array</param>
    /// <param name="b">right array</param>
    /// <returns>the concatenation</returns>
    public static Vec3[ ] Concat (in Vec3[ ] a, in Vec3[ ] b)
    {
        bool aNull = a == null;
        bool bNull = b == null;

        if (aNull && bNull) { return new Vec3[ ] { }; }

        if (aNull)
        {
            Vec3[ ] result0 = new Vec3[b.Length];
            System.Array.Copy (b, 0, result0, 0, b.Length);
            return result0;
        }

        if (bNull)
        {
            Vec3[ ] result1 = new Vec3[a.Length];
            System.Array.Copy (a, 0, result1, 0, a.Length);
            return result1;
        }

        int aLen = a.Length;
        int bLen = b.Length;
        Vec3[ ] result = new Vec3[aLen + bLen];
        System.Array.Copy (a, 0, result, 0, aLen);
        System.Array.Copy (b, 0, result, aLen, bLen);
        return result;
    }

    /// <summary>
    /// Tests to see if the vector contains a value
    /// </summary>
    /// <param name="a">the vector</param>
    /// <param name="b">the value</param>
    /// <returns>the evaluation</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static bool Contains (in Vec3 a, in float b)
    {
        return Utils.Approx (a._x, b) ||
            Utils.Approx (a._y, b) ||
            Utils.Approx (a._z, b);
    }

    /// <summary>
    /// Finds first vector argument with the sign of the second vector argument.
    /// </summary>
    /// <param name="a">the magnitude</param>
    /// <param name="b">the sign</param>
    /// <returns>the signed vector</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 CopySign (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (
            Utils.CopySign (a._x, b._x),
            Utils.CopySign (a._y, b._y),
            Utils.CopySign (a._z, b._z));
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
    /// <returns>the cross product</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 Cross (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (
            a._y * b._z - a._z * b._y,
            a._z * b._x - a._x * b._z,
            a._x * b._y - a._y * b._x);
    }

    /// <summary>
    /// Finds the absolute value of the difference between two vectors.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the absolute difference</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 Diff (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (
            Utils.Diff (b._x, a._x),
            Utils.Diff (b._y, a._y),
            Utils.Diff (b._z, a._z));
    }

    /// <summary>
    /// Finds the Chebyshev distance between two vectors. Forms a cube pattern
    /// when plotted.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the distance</returns>
    public static float DistChebyshev (in Vec3 a, in Vec3 b)
    {
        float dx = Utils.Diff (b._x, a._x);
        float dy = Utils.Diff (b._y, a._y);
        float dz = Utils.Diff (b._z, a._z);
        return Utils.Max (dx, dy, dz);
    }

    /// <summary>
    /// Finds the Euclidean distance between two vectors. Where possible, use
    /// distance squared to avoid the computational cost of the square-root.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the Euclidean distance</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float DistEuclidean (in Vec3 a, in Vec3 b)
    {
        // double dx = b._x - a._x; double dy = b._y - a._y; double dz = b._z -
        // a._z; return (float) Math.Sqrt (dx * dx + dy * dy + dz * dz);

        return Utils.Sqrt (Vec3.DistSq (a, b));
    }

    /// <summary>
    /// Finds the Manhattan distance between two vectors. Forms an octahdral
    /// pattern when plotted.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the Manhattan distance</returns>
    public static float DistManhattan (in Vec3 a, in Vec3 b)
    {
        float dx = Utils.Diff (b._x, a._x);
        float dy = Utils.Diff (b._y, a._y);
        float dz = Utils.Diff (b._z, a._z);
        return dx + dy + dz;
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
    /// <returns>the Minkowski distance</returns>
    public static float DistMinkowski (in Vec3 a, in Vec3 b, in float c = 2.0f)
    {
        if (c != 0.0f)
        {
            float dx = Utils.Pow (Utils.Diff (b._x, a._x), c);
            float dy = Utils.Pow (Utils.Diff (b._y, a._y), c);
            float dz = Utils.Pow (Utils.Diff (b._z, a._z), c);
            return Utils.Pow (dx + dy + dz, 1.0f / c);
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
    /// <returns>the distance squared</returns>
    public static float DistSq (in Vec3 a, in Vec3 b)
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
    /// <returns>the dot product</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float Dot (in Vec3 a, in Vec3 b)
    {
        return a._x * b._x +
            a._y * b._y +
            a._z * b._z;
    }

    /// <summary>
    /// Filters each component of the input vector against a lower and upper
    /// bound. If the component is within the range, its value is retained;
    /// otherwise, it is set to 0.0 .
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the filtered vector</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 Filter (in Vec3 v, in Vec3 lb, in Vec3 ub)
    {
        return new Vec3 (
            Utils.Filter (v._x, lb._x, ub._x),
            Utils.Filter (v._y, lb._y, ub._y),
            Utils.Filter (v._z, lb._z, ub._z));
    }

    /// <summary>
    /// Floors each component of the vector.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the floor</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 Floor (in Vec3 v)
    {
        return new Vec3 (
            Utils.Floor (v._x),
            Utils.Floor (v._y),
            Utils.Floor (v._z));
    }

    /// <summary>
    /// Applies the % operator (truncation-based modulo) to the left operand.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the result</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 Fmod (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (
            Utils.Fmod (a._x, b._x),
            Utils.Fmod (a._y, b._y),
            Utils.Fmod (a._z, b._z));
    }

    /// <summary>
    /// Returns the fractional portion of the vector's components.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the fractional portion</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 Fract (in Vec3 v)
    {
        return new Vec3 (
            Utils.Fract (v._x),
            Utils.Fract (v._y),
            Utils.Fract (v._z));
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
    /// <returns>the vector</returns>
    public static Vec3 FromSpherical ( //
        in float azimuth = 0.0f, // 
        in float inclination = 0.0f, // 
        in float radius = 1.0f)
    {
        float sint = 0.0f;
        float cost = 0.0f;
        Utils.SinCos (azimuth, out sint, out cost);

        float sinp = 0.0f;
        float cosp = 0.0f;
        Utils.SinCos (inclination, out sinp, out cosp);

        float rsp = radius * sinp;
        return new Vec3 (
            rsp * cost,
            rsp * sint,
            radius * cosp);
    }

    /// <summary>
    /// Generates a 3D array of vectors.
    /// </summary>
    /// <param name="cols">number of columns</param>
    /// <param name="rows">number of rows</param>
    /// <param name="layers">number of layers</param>
    /// <param name="lowerBound">lower bound</param>
    /// <param name="upperBound">upper bound</param>
    /// <returns>the array</returns>
    public static Vec3[, , ] Grid ( //
        in int cols, //
        in int rows, //
        in int layers, //
        in Vec3 lowerBound, //
        in Vec3 upperBound)
    {
        int lval = layers < 2 ? 2 : layers;
        int rval = rows < 2 ? 2 : rows;
        int cval = cols < 2 ? 2 : cols;

        float hToStep = 1.0f / (lval - 1.0f);
        float iToStep = 1.0f / (rval - 1.0f);
        float jToStep = 1.0f / (cval - 1.0f);

        Vec3[, , ] result = new Vec3[lval, rval, cval];

        int rcval = rval * cval;
        int len3 = lval * rcval;
        for (int k = 0; k < len3; ++k)
        {
            int h = k / rcval;
            int m = k - h * rcval;
            int i = m / cval;
            int j = m % cval;

            result[h, i, j] = Vec3.Mix (
                lowerBound,
                upperBound,
                new Vec3 (
                    (float) j * jToStep,
                    (float) i * iToStep,
                    (float) h * hToStep));
        }

        return result;
    }

    /// <summary>
    /// Finds the vector's signed inclination.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the signed inclination</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float InclinationSigned (in Vec3 v)
    {
        return Utils.HalfPi - Vec3.InclinationUnsigned (v);
    }

    /// <summary>
    /// Finds the vector's unsigned inclination.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the unsigned inclination</returns>
    public static float InclinationUnsigned (in Vec3 v)
    {
        float mSq = Vec3.MagSq (v);
        return (mSq > 0.0f) ? Utils.Acos (v._z / Utils.SqrtUnchecked (mSq)) : Utils.HalfPi;
    }

    /// <summary>
    /// Tests to see if the vector is on the unit circle, i.e., has a magnitude
    /// of approximately 1.0 .
    /// </summary>
    /// <param name="v"></param>
    /// <returns>the evaluation</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static bool IsUnit (in Vec3 v)
    {
        return Utils.Approx (Vec3.MagSq (v), 1.0f);
    }

    /// <summary>
    /// Limits a vector's magnitude to a scalar. Does nothing if the vector is
    /// beneath the limit.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <param name="limit">the limit</param>
    /// <returns>the limited vector</returns>
    public static Vec3 Limit (in Vec3 v, in float limit)
    {
        float mSq = Vec3.MagSq (v);
        if (mSq > (limit * limit))
        {
            return Utils.Div (limit, Utils.SqrtUnchecked (mSq)) * v;
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
    /// <returns>the linear step</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 LinearStep (in Vec3 edge0, in Vec3 edge1, in Vec3 x)
    {
        return new Vec3 (
            Utils.LinearStep (edge0._x, edge1._x, x._x),
            Utils.LinearStep (edge0._y, edge1._y, x._y),
            Utils.LinearStep (edge0._z, edge1._z, x._z));
    }

    /// <summary>
    /// Finds the length, or magnitude, of a vector. Also referred to as the
    /// radius when using polar coordinates. Uses the formula sqrt ( dot ( a,
    /// a)) Where possible, use magSq or dot to avoid the computational cost of
    /// the square-root.
    /// </summary>
    /// <param name="v"></param>
    /// <returns>the magnitude</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float Mag (in Vec3 v)
    {
        return Utils.Sqrt (Vec3.MagSq (v));

        // double xd = v._x; double yd = v._y; double zd = v._z;
        // return (float) Math.Sqrt (xd * xd + yd * yd + zd * zd);
    }

    /// <summary>
    /// Finds the length-, or magnitude-, squared of a vector. Returns the same
    /// result as dot ( a, a ) . Useful when calculating the lengths of many
    /// vectors, so as to avoid the computational cost of the square-root.
    /// </summary>
    /// <param name="v"></param>
    /// <returns>the magnitude squared</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float MagSq (in Vec3 v)
    {
        return v._x * v._x +
            v._y * v._y +
            v._z * v._z;
    }

    /// <summary>
    /// Maps an input vector from an original range to a target range.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <param name="lbOrigin">lower bound of original range</param>
    /// <param name="ubOrigin">upper bound of original range</param>
    /// <param name="lbDest">lower bound of destination range</param>
    /// <param name="ubDest">upper bound of destination range</param>
    /// <returns>the mapped value</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 Map (in Vec3 v, in Vec3 lbOrigin, in Vec3 ubOrigin, in Vec3 lbDest, in Vec3 ubDest)
    {
        return new Vec3 (
            Utils.Map (v._x, lbOrigin._x, ubOrigin._x, lbDest._x, ubDest._x),
            Utils.Map (v._y, lbOrigin._y, ubOrigin._y, lbDest._y, ubDest._y),
            Utils.Map (v._z, lbOrigin._z, ubOrigin._z, lbDest._z, ubDest._z));
    }

    /// <summary>
    /// Sets the target vector to the maximum of the input vector and an upper
    /// bound.
    /// </summary>
    /// <param name="a">the input value</param>
    /// <param name="b">the upper bound</param>
    /// <returns>the maximum value</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 Max (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (
            Utils.Max (a._x, b._x),
            Utils.Max (a._y, b._y),
            Utils.Max (a._z, b._z));
    }

    /// <summary>
    /// Sets the target vector to the minimum of the input vector and a lower
    /// bound.
    /// </summary>
    /// <param name="a">the input value</param>
    /// <param name="b">the lower bound</param>
    /// <returns>the minimum value</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 Min (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (
            Utils.Min (a._x, b._x),
            Utils.Min (a._y, b._y),
            Utils.Min (a._z, b._z));
    }

    /// <summary>
    /// Mixes two vectors together. Adds the vectors then divides by half.
    /// </summary>
    /// <param name="a">the original vector</param>
    /// <param name="b">the destination vector</param>
    /// <returns>the mix</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 Mix (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (
            0.5f * (a._x + b._x),
            0.5f * (a._y + b._y),
            0.5f * (a._z + b._z));
    }

    /// <summary>
    /// Mixes two vectors together by a step. Uses the formula (1.0 - t) a + t b
    /// . The step is unclamped.
    /// </summary>
    /// <param name="a">the original vector</param>
    /// <param name="b">the destination vector</param>
    /// <param name="t">the step</param>
    /// <returns>the mix</returns>
    public static Vec3 Mix (in Vec3 a, in Vec3 b, in float t)
    {
        float u = 1.0f - t;
        return new Vec3 (
            u * a._x + t * b._x,
            u * a._y + t * b._y,
            u * a._z + t * b._z);
    }

    /// <summary>
    /// Mixes two vectors together by a step. Uses the formula (1.0 - t) a + t b
    /// . The step is unclamped; to find an appropriate clamped step, use mix in
    /// conjunction with step, linearstep or smoothstep.
    /// </summary>
    /// <param name="a">the original vector</param>
    /// <param name="b">the destination vector</param>
    /// <param name="t">the step</param>
    /// <returns>the mix</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 Mix (in Vec3 a, in Vec3 b, in Vec3 t)
    {
        return new Vec3 (
            Utils.Mix (a._x, b._x, t._x),
            Utils.Mix (a._y, b._y, t._y),
            Utils.Mix (a._z, b._z, t._z));
    }

    /// <summary>
    /// Mods each component of the left vector by those of the right.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the result</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 Mod (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (
            Utils.Mod (a._x, b._x),
            Utils.Mod (a._y, b._y),
            Utils.Mod (a._z, b._z));
    }

    /// <summary>
    /// A specialized form of mod which subtracts the floor of the vector from
    /// the vector.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the result</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 Mod1 (in Vec3 v)
    {
        return new Vec3 (
            Utils.Mod1 (v._x),
            Utils.Mod1 (v._y),
            Utils.Mod1 (v._z));
    }

    /// <summary>
    /// Tests to see if all the vector's components are zero. Useful when
    /// safeguarding against invalid directions.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the evaluation</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static bool None (in Vec3 v)
    {
        return v._x == 0.0f &&
            v._y == 0.0f &&
            v._z == 0.0f;
    }

    /// <summary>
    /// Divides a vector by its magnitude, such that the new magnitude is 1.0 .
    /// The result is a unit vector, as it lies on the circumference of a unit
    /// circle.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the unit vector</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 Normalize (in Vec3 v)
    {
        return v / Vec3.Mag (v);
    }

    /// <summary>
    /// Evaluates a vector like a boolean, where n != 0.0 is true.
    /// </summary>
    /// <param name="v">the vector</param>
    /// <returns>the truth table opposite</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 Not (in Vec3 v)
    {
        return new Vec3 (
            v._x != 0.0f ? 0.0f : 1.0f,
            v._y != 0.0f ? 0.0f : 1.0f,
            v._z != 0.0f ? 0.0f : 1.0f);
    }

    /// <summary>
    /// Raises a vector to the power of another vector.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the result</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 Pow (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (
            Utils.Pow (a._x, b._x),
            Utils.Pow (a._y, b._y),
            Utils.Pow (a._z, b._z));
    }

    /// <summary>
    /// Returns the scalar projection of a onto b.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the scalar projection</returns>
    public static float ProjectScalar (in Vec3 a, in Vec3 b)
    {
        float bSq = Vec3.MagSq (b);
        if (bSq != 0.0f) return Vec3.Dot (a, b) / bSq;
        return 0.0f;
    }

    /// <summary>
    /// Projects one vector onto another. Defined as
    ///
    /// proj ( a , b ) := b ( dot( a, b ) / dot ( b, b ) )
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the vector projection</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 ProjectVector (in Vec3 a, in Vec3 b)
    {
        return b * Vec3.ProjectScalar (a, b);
    }

    /// <summary>
    /// Promotes a 2D vector to a 3D vector.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="z">z component</param>
    /// <returns>vector</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 Promote (in Vec2 v, in float z = 0.0f)
    {
        return new Vec3 (v.x, v.y, z);
    }

    /// <summary>
    /// Reduces the signal, or granularity, of a vector's components. Any level
    /// less than 2 returns the target set to the input.
    /// </summary>
    /// <param name="v">input vector</param>
    /// <param name="levels">levels</param>
    /// <returns>the quantized vector</returns>
    public static Vec3 Quantize (in Vec3 v, in int levels = 8)
    {
        if (levels < 2) return new Vec3 (v._x, v._y, v._z);
        float levf = (float) levels;
        float delta = 1.0f / levf;
        return new Vec3 (
            delta * Utils.Floor (0.5f + v._x * levf),
            delta * Utils.Floor (0.5f + v._y * levf),
            delta * Utils.Floor (0.5f + v._z * levf));
    }

    /// <summary>
    /// Creates a random point in the Cartesian coordinate system given a lower
    /// and an upper bound.
    /// </summary>
    /// <param name="rng">the random number generator</param>
    /// <param name="lb">the lower bound</param>
    /// <param name="ub">the upper bound</param>
    /// <returns>the random vector</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 RandomCartesian (in System.Random rng, in Vec3 lb, in Vec3 ub)
    {
        return new Vec3 (
            Utils.Mix (lb._x, ub._x, (float) rng.NextDouble ( )),
            Utils.Mix (lb._y, ub._y, (float) rng.NextDouble ( )),
            Utils.Mix (lb._z, ub._z, (float) rng.NextDouble ( )));
    }

    /// <summary>
    /// Creates a random point in the Cartesian coordinate system given a lower
    /// and an upper bound.
    /// </summary>
    /// <param name="rng">the random number generator</param>
    /// <param name="lb">the lower bound</param>
    /// <param name="ub">the upper bound</param>
    /// <returns>the random vector</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 RandomCartesian (in System.Random rng, in float lb = 0.0f, in float ub = 1.0f)
    {
        return new Vec3 (
            Utils.Mix (lb, ub, (float) rng.NextDouble ( )),
            Utils.Mix (lb, ub, (float) rng.NextDouble ( )),
            Utils.Mix (lb, ub, (float) rng.NextDouble ( )));
    }

    /// <summary>
    /// Creates a vector at a random azimuth, inclination and radius.
    /// </summary>
    /// <param name="rng">the random number generator</param>
    /// <param name="rhoMin">the minimum radius</param>
    /// <param name="rhoMax">the maximum radius</param>
    /// <returns>the output vector</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 RandomSpherical (in System.Random rng, in float rhoMin = 1.0f, in float rhoMax = 1.0f)
    {
        return Vec3.FromSpherical (
            Utils.Mix (-Utils.Pi, Utils.Pi,
                (float) rng.NextDouble ( )),

            Utils.Mix (Utils.Pi, 0.0f,
                (float) rng.NextDouble ( )),

            Utils.Mix (rhoMin, rhoMax,
                (float) rng.NextDouble ( )));
    }

    /// <summary>
    /// Reflects an incident vector off a normal vector. Uses the formula
    ///
    /// i - 2.0 ( dot( n, i ) n )
    /// </summary>
    /// <param name="i">the incident vector</param>
    /// <param name="n">the normal vector</param>
    /// <returns>the reflected vector</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 Reflect (in Vec3 i, in Vec3 n)
    {
        return i - ((2.0f * Vec3.Dot (n, i)) * n);
    }

    /// <summary>
    /// Refracts a vector through a volume using Snell's law.
    /// </summary>
    /// <param name="i">the incident vector</param>
    /// <param name="n">the normal vector</param>
    /// <param name="eta">ratio of refraction indices</param>
    /// <returns>the refracted vector</returns>
    public static Vec3 Refract (in Vec3 i, in Vec3 n, in float eta)
    {
        float iDotN = Vec3.Dot (i, n);
        float k = 1.0f - eta * eta * (1.0f - iDotN * iDotN);
        if (k < 0.0f) return new Vec3 ( );
        return (eta * i) -
            (n * (eta * iDotN + Utils.Sqrt (k)));
    }

    /// <summary>
    /// Normalizes a vector, then multiplies it by a scalar, in effect setting
    /// its magnitude to that scalar.
    /// </summary>
    /// <param name="v">the vector</param>
    /// <param name="scalar">the scalar</param>
    /// <returns>the rescaled vector</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 Rescale (in Vec3 v, in float scalar)
    {
        return Utils.Div (scalar, Vec3.Mag (v)) * v;
    }

    /// <summary>
    /// Resizes an array of vectors to a requested length.
    /// </summary>
    /// <param name="arr">array</param>
    /// <param name="sz">new size</param>
    /// <returns>the resized array</returns>
    public static Vec3[ ] Resize (in Vec3[ ] arr, in int sz)
    {
        if (sz < 1) return new Vec3[ ] { };
        Vec3[ ] result = new Vec3[sz];

        if (arr != null)
        {
            int len = arr.Length;
            int end = sz > len ? len : sz;
            System.Array.Copy (arr, result, end);
        }

        return result;
    }

    /// <summary>
    /// Rotates a vector around an arbitrary axis by using an angle in radians.
    ///
    /// The axis is assumed to have already been normalized prior to being given
    /// to the function.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <param name="radians">the angle in radians</param>
    /// <param name="axis">the axis</param>
    /// <returns>the rotated vector</returns>
    public static Vec3 Rotate (in Vec3 v, in float radians, in Vec3 axis)
    {
        float sina = 0.0f;
        float cosa = 0.0f;
        Utils.SinCos (radians, out sina, out cosa);
        return Vec3.Rotate (v, cosa, sina, axis);
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
    /// <param name="v">the input vector</param>
    /// <param name="cosa">the cosine of the angle</param>
    /// <param name="sina">the sine of the angle</param>
    /// <param name="axis">the axis</param>
    /// <returns>the rotated vector</returns>
    public static Vec3 Rotate (in Vec3 v, in float cosa, in float sina, in Vec3 axis)
    {
        float complcos = 1.0f - cosa;
        float complxy = complcos * axis._x * axis._y;
        float complxz = complcos * axis._x * axis._z;
        float complyz = complcos * axis._y * axis._z;

        float sinx = sina * axis._x;
        float siny = sina * axis._y;
        float sinz = sina * axis._z;

        return new Vec3 (
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
    /// <param name="v">the input vector</param>
    /// <param name="radians">the angle in radians</param>
    /// <returns>the rotated vector</returns>
    public static Vec3 RotateX (in Vec3 v, in float radians)
    {
        float sina = 0.0f;
        float cosa = 0.0f;
        Utils.SinCos (radians, out sina, out cosa);
        return Vec3.RotateX (v, cosa, sina);
    }

    /// <summary>
    /// Rotates a vector around the x axis.
    ///
    /// Accepts pre-calculated sine and cosine of an angle, so that collections
    /// of vectors can be efficiently rotated without repeatedly calling cos and
    /// sin.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <param name="cosa">the cosine of the angle</param>
    /// <param name="sina">the sine of the angle</param>
    /// <returns>the rotated vector</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 RotateX (in Vec3 v, in float cosa, in float sina)
    {
        return new Vec3 (
            v._x,
            cosa * v._y - sina * v._z,
            cosa * v._z + sina * v._y);
    }

    /// <summary>
    /// Rotates a vector around the y axis by using an angle in radians.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <param name="radians">the angle in radians</param>
    /// <returns>the rotated vector</returns>
    public static Vec3 RotateY (in Vec3 v, in float radians)
    {
        float sina = 0.0f;
        float cosa = 0.0f;
        Utils.SinCos (radians, out sina, out cosa);
        return Vec3.RotateY (v, cosa, sina);
    }

    /// <summary>
    /// Rotates a vector around the y axis.
    ///
    /// Accepts pre-calculated sine and cosine of an angle, so that collections
    /// of vectors can be efficiently rotated without repeatedly calling cos and
    /// sin.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <param name="cosa">the cosine of the angle</param>
    /// <param name="sina">the sine of the angle</param>
    /// <returns>the rotated vector</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 RotateY (in Vec3 v, in float cosa, in float sina)
    {
        return new Vec3 (
            cosa * v._x + sina * v._z,
            v._y,
            cosa * v._z - sina * v._x);
    }

    /// <summary>
    /// Rotates a vector around the z axis by using an angle in radians.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <param name="radians">the angle in radians</param>
    /// <returns>the rotated vector</returns>
    public static Vec3 RotateZ (in Vec3 v, in float radians)
    {
        float sina = 0.0f;
        float cosa = 0.0f;
        Utils.SinCos (radians, out sina, out cosa);
        return Vec3.RotateZ (v, cosa, sina);
    }

    /// <summary>
    /// Rotates a vector around the z axis.
    ///
    /// Accepts pre-calculated sine and cosine of an angle, so that collections
    /// of vectors can be efficiently rotated without repeatedly calling cos and
    /// sin.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <param name="cosa">the cosine of the angle</param>
    /// <param name="sina">the sine of the angle</param>
    /// <returns>the rotated vector</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 RotateZ (in Vec3 v, in float cosa, in float sina)
    {
        return new Vec3 (
            cosa * v._x - sina * v._y,
            cosa * v._y + sina * v._x,
            v._z);
    }

    /// <summary>
    /// Rounds each component of the vector to the nearest whole number.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the rounded vector</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 Round (in Vec3 v)
    {
        return new Vec3 (
            Utils.Round (v._x),
            Utils.Round (v._y),
            Utils.Round (v._z));
    }

    /// <summary>
    /// Rounds each component of the vector to a specified number of places.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <param name="places">the number of places</param>
    /// <returns>the rounded vector</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 Round (in Vec3 v, in int places)
    {
        return new Vec3 (
            Utils.Round (v._x, places),
            Utils.Round (v._y, places),
            Utils.Round (v._z, places));
    }

    /// <summary>
    /// Finds the sign of the vector: -1, if negative; 1, if positive.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the sign</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 Sign (in Vec3 v)
    {
        return new Vec3 (
            Utils.Sign (v._x),
            Utils.Sign (v._y),
            Utils.Sign (v._z));
    }

    /// <summary>
    /// Generates a clamped Hermite step for an input factor; to be used in
    /// conjunction with a mixing function.
    /// </summary>
    /// <param name="edge0">left edge</param>
    /// <param name="edge1">right edge</param>
    /// <param name="x">factor</param>
    /// <returns>the smooth step</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 SmoothStep (in Vec3 edge0, in Vec3 edge1, in Vec3 x)
    {
        return new Vec3 (
            Utils.SmoothStep (edge0._x, edge1._x, x._x),
            Utils.SmoothStep (edge0._y, edge1._y, x._y),
            Utils.SmoothStep (edge0._z, edge1._z, x._z));
    }

    /// <summary>
    /// Generates a clamped boolean step for an input factor; to be used in
    /// conjunction with a mixing function.
    /// </summary>
    /// <param name="edge">edge</param>
    /// <param name="x">factor</param>
    /// <returns>the step</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 Step (in Vec3 edge, in Vec3 x)
    {
        return new Vec3 (
            Utils.Step (edge._x, x._x),
            Utils.Step (edge._y, x._y),
            Utils.Step (edge._z, x._z));
    }

    /// <summary>
    /// Returns a named value tuple containing the vector's azimuth,
    /// theta; inclination, phi; and magnitude, rho.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the tuple</returns>
    public static (float theta, float phi, float rho) ToSpherical (in Vec3 v)
    {
        return (
            theta: Vec3.AzimuthUnsigned (v),
            phi: Vec3.InclinationUnsigned (v),
            rho: Vec3.Mag (v));
    }

    /// <summary>
    /// Returns a string representation of a vector.
    /// </summary>
    /// <param name="v">vector</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static string ToString (in Vec3 v, in int places = 4)
    {
        return Vec3.ToString (new StringBuilder (64), v, places).ToString ( );
    }

    /// <summary>
    /// Appends a representation of a vector to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="v">vector</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString (in StringBuilder sb, in Vec3 v, in int places = 4)
    {
        sb.Append ("{ x: ");
        Utils.ToFixed (sb, v._x, places);
        sb.Append (", y: ");
        Utils.ToFixed (sb, v._y, places);
        sb.Append (", z: ");
        Utils.ToFixed (sb, v._z, places);
        sb.Append (' ');
        sb.Append ('}');
        return sb;
    }

    /// <summary>
    /// Returns a string representation of an array of vectors.
    /// </summary>
    /// <param name="arr">array</param>
    /// <param name="places">print precision</param>
    /// <returns>string</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static string ToString (in Vec3[ ] arr, in int places = 4)
    {
        return Vec3.ToString (new StringBuilder (arr.Length * 64), arr, places).ToString ( );
    }

    /// <summary>
    /// Appends a representation of an array of vectors to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="arr">array</param>
    /// <param name="places">print precision</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString (in StringBuilder sb, in Vec3[ ] arr, in int places = 4)
    {
        sb.Append ('[');
        sb.Append (' ');

        if (arr != null)
        {
            int len = arr.Length;
            int last = len - 1;

            for (int i = 0; i < last; ++i)
            {
                Vec3.ToString (sb, arr[i], places);
                sb.Append (',');
                sb.Append (' ');
            }

            Vec3.ToString (sb, arr[last], places);
            sb.Append (' ');
        }

        sb.Append (']');
        return sb;
    }

    /// <summary>
    /// Truncates each component of the vector.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the truncation</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 Trunc (in Vec3 v)
    {
        return new Vec3 (
            Utils.Trunc (v._x),
            Utils.Trunc (v._y),
            Utils.Trunc (v._z));
    }

    /// <summary>
    /// Wraps a vector around a periodic range, as represented by a lower and
    /// upper bound. The lower bound is inclusive; the upper bound, exclusive.
    ///
    /// In cases where the lower bound is 0.0, use Mod instead.
    /// </summary>
    /// <param name="v">the vector</param>
    /// <param name="lb">the lower bound</param>
    /// <param name="ub">the upper bound</param>
    /// <returns>the wrapped vector</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static Vec3 Wrap (in Vec3 v, in Vec3 lb, in Vec3 ub)
    {
        return new Vec3 (
            Utils.Wrap (v._x, lb._x, ub._x),
            Utils.Wrap (v._y, lb._y, ub._y),
            Utils.Wrap (v._z, lb._z, ub._z));
    }

    /// <summary>
    /// Returns a direction facing back, (0.0, -1.0, 0.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec3 Back { get { return new Vec3 (0.0f, -1.0f, 0.0f); } }

    /// <summary>
    /// Returns a direction facing down, (0.0, 0.0, -1.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec3 Down { get { return new Vec3 (0.0f, 0.0f, -1.0f); } }

    /// <summary>
    /// Returns a direction facing forward, (0.0, 1.0, 0.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec3 Forward { get { return new Vec3 (0.0f, 1.0f, 0.0f); } }

    /// <summary>
    /// Returns a direction facing left, (-1.0, 0.0, 0.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec3 Left { get { return new Vec3 (-1.0f, 0.0f, 0.0f); } }

    /// <summary>
    /// Returns a vector with all components set to 1.0 .
    /// </summary>
    /// <value>the vector</value>
    public static Vec3 One { get { return new Vec3 (1.0f, 1.0f, 1.0f); } }

    /// <summary>
    /// Returns a direction facing right, (1.0, 0.0, 0.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec3 Right { get { return new Vec3 (1.0f, 0.0f, 0.0f); } }

    /// <summary>
    /// Returns a direction facing up, (0.0, 0.0, 1.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec3 Up { get { return new Vec3 (0.0f, 0.0f, 1.0f); } }

    /// <summary>
    /// Returns a vector with all components set to zero.
    /// </summary>
    /// <value>the vector</value>
    public static Vec3 Zero { get { return new Vec3 (0.0f, 0.0f, 0.0f); } }
}