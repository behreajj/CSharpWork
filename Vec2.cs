using System;
using System.Collections;
using System.Text;

/// <summary>
/// A readonly struct influenced by GLSL, OSL and Processing's PVector. This is
/// intended for storing points and directions in two-dimensional graphics
/// programs.
/// </summary>
[Serializable]
public readonly struct Vec2 : IComparable<Vec2>, IEquatable<Vec2>, IEnumerable
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
    /// Returns the number of values (dimensions) in this vector.
    /// </summary>
    /// <value>the length</value>
    public int Length { get { return 2; } }

    /// <summary>
    /// Component on the x axis in the Cartesian coordinate system.
    /// </summary>
    /// <value>x</value>
    public float x { get { return this._x; } }

    /// <summary>
    /// Component on the y axis in the Cartesian coordinate system.
    /// </summary>
    /// <value>y</value>
    public float y { get { return this._y; } }

    /// <summary>
    /// Retrieves a component by index. When the provided index is 1 or -1,
    /// returns y; 0 or -2, x.
    /// </summary>
    /// <value>the component</value>
    public float this [int i]
    {
        get
        {
            switch (i)
            {
                case 0:
                case -2:
                    return this._x;
                case 1:
                case -1:
                    return this._y;
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
    public Vec2 (float x = 0.0f, float y = 0.0f)
    {
        this._x = x;
        this._y = y;
    }

    /// <summary>
    /// Constructs a vector from boolean values, where true is 1.0 and false is
    /// 0.0 .
    /// </summary>
    /// <param name="x">the x component</param>
    /// <param name="y">the y component</param>
    public Vec2 (in bool x = false, in bool y = false)
    {
        this._x = x ? 1.0f : 0.0f;
        this._y = y ? 1.0f : 0.0f;
    }

    /// <summary>
    /// Tests this vector for equivalence with an object. For approximate
    /// equality  with another vector, use the static approx function instead.
    /// </summary>
    /// <param name="value">the object</param>
    /// <returns>the equivalence</returns>
    public override bool Equals (object value)
    {
        if (Object.ReferenceEquals (this, value)) return true;
        if (Object.ReferenceEquals (null, value)) return false;
        if (value is Vec2) return this.Equals ((Vec2) value);
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
            return (Utils.MulBase ^ this._x.GetHashCode ( )) *
                Utils.HashMul ^ this._y.GetHashCode ( );
        }
    }

    /// <summary>
    /// Returns a string representation of this vector.
    /// </summary>
    /// <returns>the string</returns>
    public override string ToString ( )
    {
        return this.ToString (4);
    }

    /// <summary>
    /// Compares this vector to another in compliance with the IComparable
    /// interface. Returns 1 when a component of this vector is greater than
    /// another; -1 when lesser. Prioritizes the highest dimension first: y, x .
    /// Returns 0 as a last resort.
    /// </summary>
    /// <param name="v">the comparisand</param>
    /// <returns>the evaluation</returns>
    public int CompareTo (Vec2 v)
    {
        return (this._y > v._y) ? 1 :
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
    public bool Equals (Vec2 v)
    {
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
    }

    /// <summary>
    /// Returns a float array of length 2 containing this vector's components.
    /// </summary>
    /// <returns>the array</returns>
    public float[ ] ToArray ( )
    {
        return new float[ ] { this._x, this._y };
    }

    /// <summary>
    /// Returns a string representation of this vector.
    /// </summary>
    /// <param name="places">number of decimal places</param>
    /// <returns>the string</returns>
    public string ToString (in int places = 4)
    {
        return new StringBuilder (48)
            .Append ("{ x: ")
            .Append (Utils.ToFixed (this._x, places))
            .Append (", y: ")
            .Append (Utils.ToFixed (this._y, places))
            .Append (" }")
            .ToString ( );
    }

    /// <summary>
    /// Returns a named value tuple containing this vector's components.
    /// </summary>
    /// <returns>the tuple</returns>
    public (float x, float y) ToTuple ( )
    {
        return (x: this._x, y: this._y);
    }

    /// <summary>
    /// Converts a boolean to a vector by supplying the boolean to all the
    /// vector's components: 1.0 for true; 0.0 for false.
    /// </summary>
    /// <param name="b">the boolean</param>
    public static implicit operator Vec2 (in bool b)
    {
        float eval = b ? 1.0f : 0.0f;
        return new Vec2 (eval, eval);
    }

    /// <summary>
    /// Converts a float to a vector by supplying the scalar to all the vector's
    /// components.
    /// </summary>
    /// <param name="s">the scalar</param>
    public static implicit operator Vec2 (float s)
    {
        return new Vec2 (s, s);
    }

    /// <summary>
    /// Converts a vector to a boolean by finding whether all of its components
    /// are non-zero.
    /// </summary>
    /// <param name="v">the input vector</param>
    public static explicit operator bool (in Vec2 v)
    {
        return Vec2.All (v);
    }

    /// <summary>
    /// Converts a vector to a float by finding its magnitude.
    /// </summary>
    /// <param name="v">the input vector</param>
    public static explicit operator float (in Vec2 v)
    {
        return Vec2.Mag (v);
    }

    /// <summary>
    /// A vector evaluates to true when all of its components are not equal to
    /// zero.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the evaluation</returns>
    public static bool operator true (in Vec2 v)
    {
        return Vec2.All (v);
    }

    /// <summary>
    /// A vector evaluates to false when all of its components are equal to
    /// zero.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the evaluation</returns>
    public static bool operator false (in Vec2 v)
    {
        return Vec2.None (v);
    }

    /// <summary>
    /// Evaluates a vector as a boolean. Equivalent to using the ~ operator.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the opposite</returns>
    public static Vec2 operator ! (in Vec2 v)
    {
        return Vec2.Not (v);
    }

    /// <summary>
    /// Evaluates a vector as a boolean. Equivalent to using the ! operator.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the complement</returns>
    public static Vec2 operator ~ (in Vec2 v)
    {
        return Vec2.Not (v);
    }

    /// <summary>
    /// Evaluates two vectors like booleans, using the inclusive and (AND) logic
    /// gate.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the evaluation</returns>
    public static Vec2 operator & (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (
            Utils.And (a._x, b._x),
            Utils.And (a._y, b._y));
    }

    /// <summary>
    /// Evaluates two vectors like booleans, using the inclusive or (OR) logic
    /// gate.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the evaluation</returns>
    public static Vec2 operator | (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (
            Utils.Or (a._x, b._x),
            Utils.Or (a._y, b._y));
    }

    /// <summary>
    /// Evaluates two vectors like booleans, using the exclusive or (XOR) logic
    /// gate.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the evaluation</returns>
    public static Vec2 operator ^ (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (
            Utils.Xor (a._x, b._x),
            Utils.Xor (a._y, b._y));
    }

    /// <summary>
    /// Negates the input vector
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the negation</returns>
    public static Vec2 operator - (in Vec2 v)
    {
        return new Vec2 (-v._x, -v._y);
    }

    /// <summary>
    /// Multiplies two vectors, component-wise. Such multiplication is
    /// mathematically incorrect, but serves as a shortcut for transforming a
    /// vector by a scalar matrix.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the product</returns>
    public static Vec2 operator * (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (a._x * b._x, a._y * b._y);
    }

    /// <summary>
    /// Multiplies a vector by a scalar.
    /// </summary>
    /// <param name="a">left operand, the vector</param>
    /// <param name="b">right operand, the scalar</param>
    /// <returns>the product</returns>
    public static Vec2 operator * (in Vec2 a, in float b)
    {
        return new Vec2 (a._x * b, a._y * b);
    }

    /// <summary>
    /// Multiplies a vector by a scalar.
    /// </summary>
    /// <param name="a">left operand, the scalar</param>
    /// <param name="b">right operand, the vector</param>
    /// <returns>the product</returns>
    public static Vec2 operator * (in float a, in Vec2 b)
    {
        return new Vec2 (a * b._x, a * b._y);
    }

    /// <summary>
    /// Divides the left operand by the right, component-wise. This is
    /// mathematically incorrect, but serves as a shortcut for transforming a
    /// vector by the inverse of a scalar matrix.
    /// </summary>
    /// <param name="a">numerator</param>
    /// <param name="b">denominator</param>
    /// <returns>the quotient</returns>
    public static Vec2 operator / (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (
            Utils.Div (a._x, b._x),
            Utils.Div (a._y, b._y));
    }

    /// <summary>
    /// Divides a vector by a scalar.
    /// </summary>
    /// <param name="a">vector, numerator</param>
    /// <param name="b">scalar, denominator</param>
    /// <returns>the product</returns>
    public static Vec2 operator / (in Vec2 a, in float b)
    {
        if (b != 0.0f)
        {
            float bInv = 1.0f / b;
            return new Vec2 (
                a._x * bInv,
                a._y * bInv);
        }
        return new Vec2 ( );
    }

    /// <summary>
    /// Divides a scalar by a vector.
    /// </summary>
    /// <param name="a">scalar, numerator</param>
    /// <param name="b">vector, denominator</param>
    /// <returns>the product</returns>
    public static Vec2 operator / (in float a, in Vec2 b)
    {
        return new Vec2 (
            Utils.Div (a, b._x),
            Utils.Div (a, b._y));
    }

    /// <summary>
    /// Applies truncation-based modulo (fmod) to the left and right vectors.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the result</returns>
    public static Vec2 operator % (in Vec2 a, in Vec2 b)
    {
        return Vec2.Fmod (a, b);
    }

    /// <summary>
    /// Applies truncation-based modulo (fmod) to the left and right operands.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the result</returns>
    public static Vec2 operator % (in Vec2 a, in float b)
    {
        if (b != 0.0f) return new Vec2 (a._x % b, a._y % b);
        return a;
    }

    /// <summary>
    /// Applies truncation-based modulo (fmod) to the left and right operands.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the result</returns>
    public static Vec2 operator % (in float a, in Vec2 b)
    {
        return new Vec2 (
            Utils.Fmod (a, b._x),
            Utils.Fmod (a, b._y));
    }

    /// <summary>
    /// Adds two vectors together.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the sum</returns>
    public static Vec2 operator + (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (a._x + b._x, a._y + b._y);
    }

    /// <summary>
    /// Subtracts the right vector from the left vector.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the difference</returns>
    public static Vec2 operator - (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (a._x - b._x, a._y - b._y);
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
    public static Vec2 operator < (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (a._x < b._x, a._y < b._y);
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
    public static Vec2 operator > (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (a._x > b._x, a._y > b._y);
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
    public static Vec2 operator <= (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (a._x <= b._x, a._y <= b._y);
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
    public static Vec2 operator >= (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (a._x >= b._x, a._y >= b._y);
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
    public static Vec2 operator != (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (a._x != b._x, a._y != b._y);
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
    public static Vec2 operator == (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (a._x == b._x, a._y == b._y);
    }

    /// <summary>
    /// Finds the absolute value of each vector component.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the absolute vector</returns>
    public static Vec2 Abs (in Vec2 v)
    {
        return new Vec2 (
            Utils.Abs (v._x),
            Utils.Abs (v._y));
    }

    /// <summary>
    /// Tests to see if all the vector's components are non-zero. Useful when
    /// testing valid dimensions (width and depth) stored in vectors.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the evaluation</returns>
    public static bool All (in Vec2 v)
    {
        return v._x != 0.0f && v._y != 0.0f;
    }

    /// <summary>
    /// Finds the angle between two vectors.
    /// </summary>
    /// <param name="a">the first vector</param>
    /// <param name="b">the second vector</param>
    /// <returns>the angle</returns>
    public static float AngleBetween (in Vec2 a, in Vec2 b)
    {
        if (Vec2.None (a) || Vec2.None (b)) return 0.0f;
        return Utils.Acos (Vec2.Dot (a, b) / (Vec2.Mag (a) * Vec2.Mag (b)));
    }

    /// <summary>
    /// Tests to see if any of the vector's components are non-zero.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the evaluation</returns>
    public static bool Any (in Vec2 v)
    {
        return v._x != 0.0f || v._y != 0.0f;
    }

    /// <summary>
    /// Tests to see if two vectors approximate each other.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <param name="tolerance">the tolerance</param>
    /// <returns>the evaluation</returns>
    public static bool Approx (in Vec2 a, in Vec2 b, in float tolerance = Utils.Epsilon)
    {
        return Utils.Approx (a._x, b._x, tolerance) &&
            Utils.Approx (a._y, b._y, tolerance);
    }

    /// <summary>
    /// Tests to see if a vector has, approximately, the specified magnitude.
    /// </summary>
    /// <param name="a">the input vector</param>
    /// <param name="b">the magnitude</param>
    /// <param name="tolerance">the tolerance</param>
    /// <returns>the evaluation</returns>
    public static bool ApproxMag (in Vec2 a, in float b = 1.0f, in float tolerance = Utils.Epsilon)
    {
        return Utils.Approx (Vec2.MagSq (a), b * b, tolerance);
    }

    /// <summary>
    /// Tests to see if two vectors are parallel.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>the evaluation</returns>
    public static bool AreParallel (in Vec2 a, in Vec2 b, in float tolerance = Utils.Epsilon)
    {
        return Vec2.Approx (Vec2.Cross (a, b), 0.0f, tolerance);
    }

    /// <summary>
    /// Returns a point on a Bezier curve described by two anchor points and two
    /// control points according to a step in [0.0, 1.0] .
    ///
    /// When the step is less than one, returns the first anchor point. When the
    /// step is greater than one, returns the second anchor point.
    /// </summary>
    /// <param name="ap0">first anchor point</param>
    /// <param name="cp0">first control point</param>
    /// <param name="cp1">second control point</param>
    /// <param name="ap1">second anchor point</param>
    /// <param name="step">step</param>
    /// <returns>the point along the curve</returns>
    public static Vec2 BezierPoint (in Vec2 ap0 = new Vec2 ( ), in Vec2 cp0 = new Vec2 ( ), in Vec2 cp1 = new Vec2 ( ), in Vec2 ap1 = new Vec2 ( ), in float step = 0.5f)
    {
        if (step <= 0.0f) return new Vec2 (ap0._x, ap0._y);
        else if (step >= 1.0f) return new Vec2 (ap1._x, ap1._y);

        float u = 1.0f - step;
        float tcb = step * step;
        float ucb = u * u;
        float usq3t = ucb * (step + step + step);
        float tsq3u = tcb * (u + u + u);
        ucb *= u;
        tcb *= step;

        return new Vec2 (
            ap0._x * ucb +
            cp0._x * usq3t +
            cp1._x * tsq3u +
            ap1._x * tcb,

            ap0._y * ucb +
            cp0._y * usq3t +
            cp1._y * tsq3u +
            ap1._y * tcb);
    }

    /// <summary>
    /// Returns a tangent on a Bezier curve described by two anchor points and
    /// two control points according to a step in [0.0, 1.0] .
    ///
    /// When the step is less than one, returns the first anchor point
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
    public static Vec2 BezierTangent (in Vec2 ap0 = new Vec2 ( ), in Vec2 cp0 = new Vec2 ( ), in Vec2 cp1 = new Vec2 ( ), in Vec2 ap1 = new Vec2 ( ), in float step = 0.5f)
    {

        if (step <= 0.0f) return cp0 - ap0;
        else if (step >= 1.0f) return ap1 - cp1;

        float u = 1.0f - step;
        float t3 = step + step + step;
        float usq3 = u * (u + u + u);
        float tsq3 = step * t3;
        float ut6 = u * (t3 + t3);

        return new Vec2 (
            (cp0._x - ap0._x) * usq3 +
            (cp1._x - cp0._x) * ut6 +
            (ap1._x - cp1._x) * tsq3,

            (cp0._y - ap0._y) * usq3 +
            (cp1._y - cp0._y) * ut6 +
            (ap1._y - cp1._y) * tsq3);
    }

    /// <summary>
    /// Returns a normalized tangent on a Bezier curve.
    /// </summary>
    /// <param name="ap0">the first anchor point</param>
    /// <param name="cp0">the first control point</param>
    /// <param name="cp1">the second control point</param>
    /// <param name="ap1">the second anchor point</param>
    /// <param name="step">the step</param>
    /// <returns>the tangent along the curve</returns>
    public static Vec2 BezierTanUnit (in Vec2 ap0 = new Vec2 ( ), in Vec2 cp0 = new Vec2 ( ), in Vec2 cp1 = new Vec2 ( ), in Vec2 ap1 = new Vec2 ( ), in float step = 0.5f)
    {
        return Vec2.Normalize (Vec2.BezierTangent (
            ap0, cp0, cp1, ap1, step));
    }

    /// <summary>
    /// Raises each component of the vector to the nearest greater integer.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the result</returns>
    public static Vec2 Ceil (in Vec2 v)
    {
        return new Vec2 (
            Utils.Ceil (v._x),
            Utils.Ceil (v._y));
    }

    /// <summary>
    /// Clamps a vector to a range within the lower and upper bound.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <param name="lb">the range lower bound</param>
    /// <param name="ub">the range upper bound</param>
    /// <returns>the clamped vector</returns>
    public static Vec2 Clamp (in Vec2 v, in float lb = 0.0f, in float ub = 1.0f)
    {
        return new Vec2 (
            Utils.Clamp (v._x, lb, ub),
            Utils.Clamp (v._y, lb, ub));
    }

    /// <summary>
    /// Clamps a vector to a range within the lower and upper bound.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <param name="lb">the range lower bound</param>
    /// <param name="ub">the range upper bound</param>
    /// <returns>the clamped vector</returns>
    public static Vec2 Clamp (in Vec2 v, in Vec2 lb, in Vec2 ub)
    {
        return new Vec2 (
            Utils.Clamp (v._x, lb._x, ub._x),
            Utils.Clamp (v._y, lb._y, ub._y));
    }

    /// <summary>
    /// Tests to see if the vector contains a value
    /// </summary>
    /// <param name="a">the vector</param>
    /// <param name="b">the value</param>
    /// <returns>the evaluation</returns>
    public static bool Contains (in Vec2 a, in float b)
    {
        if (Utils.Approx (a._x, b)) { return true; }
        if (Utils.Approx (a._y, b)) { return true; }
        return false;
    }

    /// <summary>
    /// Finds first vector argument with the sign of the second vector argument.
    /// </summary>
    /// <param name="a">the magnitude</param>
    /// <param name="b">the sign</param>
    /// <returns>the signed vector</returns>
    public static Vec2 CopySign (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (
            Utils.CopySign (a._x, b._x),
            Utils.CopySign (a._y, b._y));
    }

    /// <summary>
    /// Returns the z component of the cross product between two vectors. The x
    /// and y components of the cross between 2D vectors are always zero. For
    /// that reason, the normalized cross product is equal to the sign of the
    /// cross product.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the cross product z</returns>
    public static float Cross (in Vec2 a, in Vec2 b)
    {
        return a._x * b._y - a._y * b._x;
    }

    /// <summary>
    /// Finds the absolute value of the difference between two vectors.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the absolute difference</returns>
    public static Vec2 Diff (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (
            Utils.Diff (b._x, a._x),
            Utils.Diff (b._y, a._y));
    }

    /// <summary>
    /// Finds the Chebyshev distance between two vectors. Forms a square pattern
    /// when plotted.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the distance</returns>
    public static float DistChebyshev (in Vec2 a, in Vec2 b)
    {
        float dx = Utils.Diff (b._x, a._x);
        float dy = Utils.Diff (b._y, a._y);
        return Utils.Max (dx, dy);
    }

    /// <summary>
    /// Finds the Euclidean distance between two vectors. Where possible, use
    /// distance squared to avoid the computational cost of the square-root.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the Euclidean distance</returns>
    public static float DistEuclidean (in Vec2 a, in Vec2 b)
    {
        // double dx = b._x - a._x; double dy = b._y - a._y; return (float)
        // Math.Sqrt (dx * dx + dy * dy);

        return Utils.Sqrt (Vec2.DistSq (a, b));
    }

    /// <summary>
    /// Finds the Manhattan distance between two vectors. Forms a diamond
    /// pattern when plotted.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the Manhattan distance</returns>
    public static float DistManhattan (in Vec2 a, in Vec2 b)
    {
        float dx = Utils.Diff (b._x, a._x);
        float dy = Utils.Diff (b._y, a._y);
        return dx + dy;
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
    public static float DistMinkowski (in Vec2 a, in Vec2 b, in float c = 2.0f)
    {
        if (c != 0.0f)
        {
            float dx = Utils.Pow (Utils.Diff (b._x, a._x), c);
            float dy = Utils.Pow (Utils.Diff (b._y, a._y), c);
            return Utils.Pow (dx + dy, 1.0f / c);
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
    public static float DistSq (in Vec2 a, in Vec2 b)
    {
        float dx = b._x - a._x;
        float dy = b._y - a._y;
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
    /// <returns>the dot product</returns>
    public static float Dot (in Vec2 a, in Vec2 b)
    {
        return a._x * b._x + a._y * b._y;
    }

    /// <summary>
    /// Filters each component of the input vector against a lower and upper
    /// bound. If the component is within the range, its value is retained;
    /// otherwise, it is set to 0.0 .
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the filtered vector</returns>
    public static Vec2 Filter (in Vec2 v, in Vec2 lb, in Vec2 ub)
    {
        return new Vec2 (
            Utils.Filter (v._x, lb._x, ub._x),
            Utils.Filter (v._y, lb._y, ub._y));
    }

    /// <summary>
    /// Floors each component of the vector.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the floor</returns>
    public static Vec2 Floor (in Vec2 v)
    {
        return new Vec2 (
            Utils.Floor (v._x),
            Utils.Floor (v._y));
    }

    /// <summary>
    /// Applies the % operator (truncation-based modulo) to the left operand.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the result</returns>
    public static Vec2 Fmod (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (
            Utils.Fmod (a._x, b._x),
            Utils.Fmod (a._y, b._y));
    }

    /// <summary>
    /// Returns the fractional portion of the vector's components.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the fractional portion</returns>
    public static Vec2 Fract (in Vec2 v)
    {
        return new Vec2 (
            Utils.Fract (v._x),
            Utils.Fract (v._y));
    }

    /// <summary>
    /// Creates a vector from polar coordinates: (1) theta, an angle in radians,
    /// the vector's heading; (2) rho, a radius, the vector's magnitude. Uses
    /// the formula
    ///
    /// ( rho cos ( theta ), rho sin ( theta ) )
    /// </summary>
    /// <param name="heading">the angle in radians</param>
    /// <param name="radius">the radius</param>
    /// <returns>the vector</returns>
    public static Vec2 FromPolar (in float heading = 0.0f, in float radius = 1.0f)
    {
        float sina = 0.0f;
        float cosa = 0.0f;
        Utils.SinCos (heading, out sina, out cosa);

        return new Vec2 (
            radius * cosa,
            radius * sina);
    }

    /// <summary>
    /// Generates a 2D array of vectors.
    /// </summary>
    /// <param name="cols">number of columns</param>
    /// <param name="rows">number of rows</param>
    /// <param name="lowerBound">lower bound</param>
    /// <param name="upperBound">upper bound</param>
    /// <returns>the array</returns>
    public static Vec2[, ] Grid (in int cols, in int rows, in Vec2 lowerBound, in Vec2 upperBound)
    {
        int rval = rows < 2 ? 2 : rows;
        int cval = cols < 2 ? 2 : cols;

        float iToStep = 1.0f / (rval - 1.0f);
        float jToStep = 1.0f / (cval - 1.0f);

        /* Calculate x values in separate loop. */
        float[ ] xs = new float[cval];
        for (int j = 0; j < cval; ++j)
        {
            xs[j] = Utils.Mix (
                lowerBound._x,
                upperBound._x,
                (float) j * jToStep);
        }

        Vec2[, ] result = new Vec2[rval, cval];
        for (int i = 0; i < rval; ++i)
        {
            float y = Utils.Mix (
                lowerBound._y,
                upperBound._y,
                (float) i * iToStep);

            for (int j = 0; j < cval; ++j)
            {
                result[i, j] = new Vec2 (xs[j], y);
            }
        }

        return result;
    }

    /// <summary>
    /// Finds the vector's heading in the range [-PI, PI] .
    /// </summary>
    /// <param name="v">the vector</param>
    /// <returns>the angle in radians</returns>
    public static float HeadingSigned (in Vec2 v)
    {
        return Utils.Atan2 (v._y, v._x);
    }

    /// <summary>
    /// Finds the vector's heading in the range [0.0, TAU] .
    /// </summary>
    /// <param name="v">the vector</param>
    /// <returns>the angle in radians</returns>
    public static float HeadingUnsigned (in Vec2 v)
    {
        return Utils.ModRadians (Vec2.HeadingSigned (v));
    }

    /// <summary>
    /// Tests to see if the vector is on the unit circle, i.e., has a magnitude
    /// of approximately 1.0 .
    /// </summary>
    /// <param name="v"></param>
    /// <returns>the evaluation</returns>
    public static bool IsUnit (in Vec2 v)
    {
        return Utils.Approx (Vec2.MagSq (v), 1.0f);
    }

    /// <summary>
    /// Limits a vector's magnitude to a scalar. Does nothing if the vector is
    /// beneath the limit.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <param name="limit">the limit</param>
    /// <returns>the limited vector</returns>
    public static Vec2 Limit (in Vec2 v, in float limit = float.MaxValue)
    {
        float mSq = Vec2.MagSq (v);
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
    public static Vec2 LinearStep (in Vec2 edge0, in Vec2 edge1, in Vec2 x)
    {
        return new Vec2 (
            Utils.LinearStep (edge0._x, edge1._x, x._x),
            Utils.LinearStep (edge0._y, edge1._y, x._y));
    }

    /// <summary>
    /// Finds the length, or magnitude, of a vector. Also referred to as the
    /// radius when using polar coordinates. Uses the formula sqrt ( dot ( a, a
    /// ) ) Where possible, use magSq or dot to avoid the computational cost of
    /// the square-root.
    /// </summary>
    /// <param name="v"></param>
    /// <returns>the magnitude</returns>
    public static float Mag (in Vec2 v)
    {
        return Utils.Sqrt (Vec2.MagSq (v));

        // double xd = v._x; double yd = v._y;

        // return (float) Math.Sqrt (xd * xd + yd * yd);
    }

    /// <summary>
    /// Finds the length-, or magnitude-, squared of a vector. Returns the same
    /// result as dot ( a, a ) . Useful when calculating the lengths of many
    /// vectors, so as to avoid the computational cost of the square-root.
    /// </summary>
    /// <param name="v"></param>
    /// <returns>the magnitude squared</returns>
    public static float MagSq (in Vec2 v)
    {
        return v._x * v._x + v._y * v._y;
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
    public static Vec2 Map (in Vec2 v, in Vec2 lbOrigin, in Vec2 ubOrigin, in Vec2 lbDest, in Vec2 ubDest)
    {
        return new Vec2 (
            Utils.Map (v._x, lbOrigin._x, ubOrigin._x, lbDest._x, ubDest._x),
            Utils.Map (v._y, lbOrigin._y, ubOrigin._y, lbDest._y, ubDest._y));
    }

    /// <summary>
    /// Sets the target vector to the maximum of the input vector and an upper
    /// bound.
    /// </summary>
    /// <param name="a">the input value</param>
    /// <param name="b">the upper bound</param>
    /// <returns>the maximum value</returns>
    public static Vec2 Max (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (
            Utils.Max (a._x, b._x),
            Utils.Max (a._y, b._y));
    }

    /// <summary>
    /// Sets the target vector to the minimum of the input vector and a lower
    /// bound.
    /// </summary>
    /// <param name="a">the input value</param>
    /// <param name="b">the lower bound</param>
    /// <returns>the minimum value</returns>
    public static Vec2 Min (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (
            Utils.Min (a._x, b._x),
            Utils.Min (a._y, b._y));
    }

    /// <summary>
    /// Mixes two vectors together by a step. Uses the formula (1.0 - t) a + t b
    /// . The step is unclamped.
    /// </summary>
    /// <param name="a">the original vector</param>
    /// <param name="b">the destination vector</param>
    /// <param name="t">the step</param>
    /// <returns>the mix</returns>
    public static Vec2 Mix (in Vec2 a, in Vec2 b, in float t = 0.5f)
    {
        float u = 1.0f - t;
        return new Vec2 (
            u * a._x + t * b._x,
            u * a._y + t * b._y);
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
    public static Vec2 Mix (in Vec2 a, in Vec2 b, in Vec2 t)
    {
        return new Vec2 (
            Utils.Mix (a._x, b._x, t._x),
            Utils.Mix (a._y, b._y, t._y));
    }

    /// <summary>
    /// Mods each component of the left vector by those of the right.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the result</returns>
    public static Vec2 Mod (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (
            Utils.Mod (a._x, b._x),
            Utils.Mod (a._y, b._y));
    }

    /// <summary>
    /// A specialized form of mod which subtracts the floor of the vector from
    /// the vector. For Vec2s, useful for managing texture coordinates in the
    /// range [0.0, 1.0] .
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the result</returns>
    public static Vec2 Mod1 (in Vec2 v)
    {
        return new Vec2 (
            Utils.Mod1 (v._x),
            Utils.Mod1 (v._y));
    }

    /// <summary>
    /// Tests to see if all the vector's components are zero. Useful when
    /// safeguarding against invalid directions.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the evaluation</returns>
    public static bool None (in Vec2 v)
    {
        return v._x == 0.0f && v._y == 0.0f;
    }

    /// <summary>
    /// Divides a vector by its magnitude, such that the new magnitude is 1.0 .
    /// The result is a unit vector, as it lies on the circumference of a unit
    /// circle.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the unit vector</returns>
    public static Vec2 Normalize (in Vec2 v)
    {
        return v / Vec2.Mag (v);
    }

    /// <summary>
    /// Evaluates a vector like a boolean, where n != 0.0 is true.
    /// </summary>
    /// <param name="v">the vector</param>
    /// <returns>the truth table opposite</returns>
    public static Vec2 Not (in Vec2 v)
    {
        return new Vec2 (
            v._x != 0.0f ? 0.0f : 1.0f,
            v._y != 0.0f ? 0.0f : 1.0f);
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
    /// <param name="v">the input vector</param>
    /// <returns>the perpendicular</returns>
    public static Vec2 PerpendicularCCW (in Vec2 v)
    {
        return new Vec2 (-v._y, v._x);
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
    /// <param name="v">the input vector</param>
    /// <returns>the perpendicular</returns>
    public static Vec2 PerpendicularCW (in Vec2 v)
    {
        return new Vec2 (v._y, -v._x);
    }

    /// <summary>
    /// Raises a vector to the power of another vector.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the result</returns>
    public static Vec2 Pow (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (
            Utils.Pow (a._x, b._x),
            Utils.Pow (a._y, b._y));
    }

    /// <summary>
    /// Returns the scalar projection of a onto b.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the scalar projection</returns>
    public static float ProjectScalar (in Vec2 a, in Vec2 b)
    {
        float bSq = Vec2.MagSq (b);
        if (bSq != 0.0f) return Vec2.Dot (a, b) / bSq;
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
    public static Vec2 ProjectVector (in Vec2 a, in Vec2 b)
    {
        return b * Vec2.ProjectScalar (a, b);
    }

    /// <summary>
    /// Reduces the signal, or granularity, of a vector's components. Any level
    /// less than 2 returns the target set to the input.
    /// </summary>
    /// <param name="a">input vector</param>
    /// <param name="levels">the levels</param>
    /// <returns>the quantized vector</returns>
    public static Vec2 Quantize (in Vec2 a, in int levels = 8)
    {
        if (levels < 2) return new Vec2 (a._x, a._y);
        float levf = (float) levels;
        float delta = 1.0f / levf;
        return new Vec2 (
            delta * Utils.Floor (0.5f + a._x * levf),
            delta * Utils.Floor (0.5f + a._y * levf));
    }

    /// <summary>
    /// Creates a random point in the Cartesian coordinate system given a lower
    /// and an upper bound.
    /// </summary>
    /// <param name="rng">the random number generator</param>
    /// <param name="lb">the lower bound</param>
    /// <param name="ub">the upper bound</param>
    /// <returns>the random vector</returns>
    public static Vec2 RandomCartesian (in System.Random rng, in Vec2 lb, in Vec2 ub)
    {
        return new Vec2 (
            Utils.Mix (lb._x, ub._x, (float) rng.NextDouble ( )),
            Utils.Mix (lb._y, ub._y, (float) rng.NextDouble ( )));
    }

    /// <summary>
    /// Creates a random point in the Cartesian coordinate system given a lower
    /// and an upper bound.
    /// </summary>
    /// <param name="rng">the random number generator</param>
    /// <param name="lb">the lower bound</param>
    /// <param name="ub">the upper bound</param>
    /// <returns>the random vector</returns>
    public static Vec2 RandomCartesian (in System.Random rng, in float lb = 0.0f, in float ub = 1.0f)
    {
        return new Vec2 (
            Utils.Mix (lb, ub, (float) rng.NextDouble ( )),
            Utils.Mix (lb, ub, (float) rng.NextDouble ( )));
    }

    /// <summary>
    /// Creates a vector at a random heading and radius.
    /// </summary>
    /// <param name="rng">the random number generator</param>
    /// <param name="rhoMin">the minimum radius</param>
    /// <param name="rhoMax">the maximum radius</param>
    /// <returns>the output vector</returns>
    public static Vec2 RandomPolar (in System.Random rng, in float rhoMin = 1.0f, in float rhoMax = 1.0f)
    {
        return Vec2.FromPolar (
            Utils.Mix (-Utils.Pi, Utils.Pi,
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
    public static Vec2 Reflect (in Vec2 i, in Vec2 n)
    {
        return i - ((2.0f * Vec2.Dot (n, i)) * n);
    }

    /// <summary>
    /// Refracts a vector through a volume using Snell's law.
    /// </summary>
    /// <param name="i">the incident vector</param>
    /// <param name="n">the normal vector</param>
    /// <param name="eta">ratio of refraction indices</param>
    /// <returns>the refracted vector</returns>
    public static Vec2 Refract (in Vec2 i, in Vec2 n, in float eta)
    {
        float iDotN = Vec2.Dot (i, n);
        float k = 1.0f - eta * eta * (1.0f - iDotN * iDotN);
        if (k < 0.0f) return new Vec2 ( );
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
    public static Vec2 Rescale (in Vec2 v, in float scalar = 1.0f)
    {
        return Utils.Div (scalar, Vec2.Mag (v)) * v;
    }

    /// <summary>
    /// Rotates a vector around the z axis by using an angle in radians.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <param name="radians">the angle in radians</param>
    /// <returns>the rotated vector</returns>
    public static Vec2 RotateZ (in Vec2 v, in float radians)
    {
        float sina = 0.0f;
        float cosa = 0.0f;
        Utils.SinCos (radians, out sina, out cosa);
        return Vec2.RotateZ (v, cosa, sina);
    }

    /// <summary>
    /// Rotates a vector around the z axis. Accepts pre-calculated sine and
    /// cosine of an angle, so that collections of vectors can be efficiently
    /// rotated without repeatedly calling cos and sin.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <param name="cosa">the cosine of the angle</param>
    /// <param name="sina">the sine of the angle</param>
    /// <returns>the rotated vector</returns>
    public static Vec2 RotateZ (in Vec2 v, in float cosa, in float sina)
    {
        return new Vec2 (
            cosa * v._x - sina * v._y,
            cosa * v._y + sina * v._x);
    }

    /// <summary>
    /// Rounds each component of the vector to the nearest whole number.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the rounded vector</returns>
    public static Vec2 Round (in Vec2 v)
    {
        return new Vec2 (
            Utils.Round (v._x),
            Utils.Round (v._y));
    }

    /// <summary>
    /// Rounds each component of the vector to a specified number of places.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <param name="places">the number of places</param>
    /// <returns>the rounded vector</returns>
    public static Vec2 Round (in Vec2 v, in int places)
    {
        return new Vec2 (
            Utils.Round (v._x, places),
            Utils.Round (v._y, places));
    }

    /// <summary>
    /// Finds the sign of the vector: -1, if negative; 1, if positive.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the sign</returns>
    public static Vec2 Sign (in Vec2 v)
    {
        return new Vec2 (
            Utils.Sign (v._x),
            Utils.Sign (v._y));
    }

    /// <summary>
    /// Generates a clamped Hermite step for an input factor; to be used in
    /// conjunction with a mixing function.
    /// </summary>
    /// <param name="edge0">left edge</param>
    /// <param name="edge1">right edge</param>
    /// <param name="x">factor</param>
    /// <returns>the smooth step</returns>
    public static Vec2 SmoothStep (in Vec2 edge0, in Vec2 edge1, in Vec2 x)
    {
        return new Vec2 (
            Utils.SmoothStep (edge0._x, edge1._x, x._x),
            Utils.SmoothStep (edge0._y, edge1._y, x._y));
    }

    /// <summary>
    /// Generates a clamped boolean step for an input factor; to be used in
    /// conjunction with a mixing function.
    /// </summary>
    /// <param name="edge">edge</param>
    /// <param name="x">factor</param>
    /// <returns>the step</returns>
    public static Vec2 Step (in Vec2 edge, in Vec2 x)
    {
        return new Vec2 (
            Utils.Step (edge._x, x._x),
            Utils.Step (edge._y, x._y));
    }

    /// <summary>
    /// Returns a named value tuple containing the vector's signed heading,
    /// theta; and magnitude, rho.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>a tuple</returns>
    public static (float theta, float rho) ToPolar (in Vec2 v)
    {
        return (
            theta: Vec2.HeadingSigned (v),
            rho: Vec2.Mag (v));
    }

    /// <summary>
    /// Truncates each component of the vector.
    /// </summary>
    /// <param name="v">the input vector</param>
    /// <returns>the truncation</returns>
    public static Vec2 Trunc (in Vec2 v)
    {
        return new Vec2 (
            Utils.Trunc (v._x),
            Utils.Trunc (v._y));
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
    public static Vec2 Wrap (in Vec2 v, in Vec2 lb, in Vec2 ub)
    {
        return new Vec2 (
            Utils.Wrap (v._x, lb._x, ub._x),
            Utils.Wrap (v._y, lb._y, ub._y));
    }

    /// <summary>
    /// Returns a direction facing back, (0.0, -1.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec2 Back
    {
        get
        {
            return new Vec2 (0.0f, -1.0f);
        }
    }

    /// <summary>
    /// Returns a direction facing forward, (0.0, 1.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec2 Forward
    {
        get
        {
            return new Vec2 (0.0f, 1.0f);
        }
    }

    /// <summary>
    /// Returns a direction facing left, (-1.0, 0.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec2 Left
    {
        get
        {
            return new Vec2 (-1.0f, 0.0f);
        }
    }

    /// <summary>
    /// Returns a vector with all components set to 1.0 .
    /// </summary>
    /// <value>the vector</value>
    public static Vec2 One
    {
        get
        {
            return new Vec2 (1.0f, 1.0f);
        }
    }

    /// <summary>
    /// Returns a direction facing right, (1.0, 0.0) .
    /// </summary>
    /// <value>the direction</value>
    public static Vec2 Right
    {
        get
        {
            return new Vec2 (1.0f, 0.0f);
        }
    }

    /// <summary>
    /// Returns a vector with all components set to zero.
    /// </summary>
    /// <value>the vector</value>
    public static Vec2 Zero
    {
        get
        {
            return new Vec2 (0.0f, 0.0f);
        }
    }
}