using System;
using System.Collections;
using System.Text;

/// <summary>
/// A readonly struct influenced by GLSL, OSL and Processing's PMatrix2D.
/// Although this is a 3 x 3 matrix, it is generally assumed to be a 2D affine
/// transform matrix, where the last row is (0.0, 0.0, 1.0) .
/// </summary>
[Serializable]
public readonly struct Mat3 : IEnumerable
{
    /// <summary>
    /// Component in row 0, column 0. The right axis x component.
    /// </summary>
    private readonly float _m00;

    /// <summary>
    /// Component in row 0, column 1. The forward axis x component.
    /// </summary>
    private readonly float _m01;

    /// <summary>
    /// Component in row 0, column 2. The translation x component.
    /// </summary>
    private readonly float _m02;

    /// <summary>
    /// Component in row 1, column 0. The right axis y component.
    /// </summary>
    private readonly float _m10;

    /// <summary>
    /// Component in row 1, column 1. The forward axis y component.
    /// </summary>
    private readonly float _m11;

    /// <summary>
    /// Component in row 1, column 2. The translation y component.
    /// </summary>
    private readonly float _m12;

    /// <summary>
    /// Component in row 2, column 0. The right axis z component.
    /// </summary>
    private readonly float _m20;

    /// <summary>
    /// Component in row 2, column 1. The forward axis z component.
    /// </summary>
    private readonly float _m21;

    /// <summary>
    /// Component in row 2, column 2. The translation z component.
    /// </summary>
    private readonly float _m22;

    /// <summary>
    /// Returns the number of values in this matrix.
    /// </summary>
    /// <value>the length</value>
    public int Length { get { return 9; } }

    /// <summary>
    /// Component in row 0, column 0. The right axis x component.
    /// </summary>
    /// <value>right axis x</value>
    public float m00 { get { return this._m00; } }

    /// <summary>
    /// Component in row 0, column 1. The forward axis x component.
    /// </summary>
    /// <value>forward axis x</value>
    public float m01 { get { return this._m01; } }

    /// <summary>
    /// Component in row 0, column 2. The translation x component.
    /// </summary>
    /// <value>translation x</value>
    public float m02 { get { return this._m02; } }

    /// <summary>
    /// Component in row 1, column 0. The right axis y component.
    /// </summary>
    /// <value>right axis y</value>
    public float m10 { get { return this._m10; } }

    /// <summary>
    /// Component in row 1, column 1. The forward axis y component.
    /// </summary>
    /// <value>forward y</value>
    public float m11 { get { return this._m11; } }

    /// <summary>
    /// Component in row 1, column 2. The translation y component.
    /// </summary>
    /// <value>translation y</value>
    public float m12 { get { return this._m12; } }

    /// <summary>
    /// Component in row 2, column 0. The right axis z component.
    /// </summary>
    /// <value>right axis z</value>
    public float m20 { get { return this._m20; } }

    /// <summary>
    /// Component in row 2, column 1. The forward axis z component.
    /// </summary>
    /// <value>forward axis z</value>
    public float m21 { get { return this._m21; } }

    /// <summary>
    /// Component in row 2, column 2. The translation z component.
    /// </summary>
    /// <value>translation z</value>
    public float m22 { get { return this._m22; } }

    /// <summary>
    /// The first column, or right axis.
    /// </summary>
    /// <returns>right axis</returns>
    public Vec3 Right
    {
        get
        {
            return new Vec3 (this._m00, this._m10, this._m20);
        }
    }

    /// <summary>
    /// The second column, or forward axis.
    /// </summary>
    /// <returns>forward</returns>
    public Vec3 Forward
    {
        get
        {
            return new Vec3 (this._m01, this._m11, this._m21);
        }
    }

    /// <summary>
    /// The third column, or translation.
    /// </summary>
    /// <returns>translation</returns>
    public Vec3 Translation
    {
        get
        {
            return new Vec3 (this._m02, this._m12, this._m22);
        }
    }

    /// <summary>
    /// Retrieves an element by index.
    /// </summary>
    /// <value>the element</value>
    public float this [int i]
    {
        get
        {
            switch (i)
            {
                case 0:
                case -9:
                    return this._m00;
                case 1:
                case -8:
                    return this._m01;
                case 2:
                case -7:
                    return this._m02;

                case 3:
                case -6:
                    return this._m10;
                case 4:
                case -5:
                    return this._m11;
                case 5:
                case -4:
                    return this._m12;

                case 6:
                case -3:
                    return this._m20;
                case 7:
                case -2:
                    return this._m21;
                case 8:
                case -1:
                    return this._m22;

                default:
                    return 0.0f;
            }
        }
    }

    /// <summary>
    /// Retrieves an element by indices.
    /// </summary>
    /// <value>the element</value>
    public float this [int i, int j]
    {
        get
        {
            switch (i)
            {
                case 0:
                case -3:
                    switch (j)
                    {
                        case 0:
                        case -3:
                            return this._m00;
                        case 1:
                        case -2:
                            return this._m01;
                        case 2:
                        case -1:
                            return this._m02;
                        default:
                            return 0.0f;
                    }
                case 1:
                case -2:
                    switch (j)
                    {
                        case 0:
                        case -3:
                            return this._m10;
                        case 1:
                        case -2:
                            return this._m11;
                        case 2:
                        case -1:
                            return this._m12;
                        default:
                            return 0.0f;
                    }
                case 2:
                case -1:
                    switch (j)
                    {
                        case 0:
                        case -3:
                            return this._m20;
                        case 1:
                        case -2:
                            return this._m21;
                        case 2:
                        case -1:
                            return this._m22;
                        default:
                            return 0.0f;
                    }
                default:
                    return 0.0f;
            }
        }
    }

    /// <summary>
    /// Constructs a matrix from float values.
    /// </summary>
    /// <param name="m00">row 0, column 0</param>
    /// <param name="m01">row 0, column 1</param>
    /// <param name="m02">row 0, column 2</param>
    /// <param name="m10">row 1, column 0</param>
    /// <param name="m11">row 1, column 1</param>
    /// <param name="m12">row 1, column 2</param>
    /// <param name="m20">row 2, column 0</param>
    /// <param name="m21">row 2, column 1</param>
    /// <param name="m22">row 2, column 2</param>
    public Mat3 (
        float m00 = 1.0f, float m01 = 0.0f, float m02 = 0.0f,
        float m10 = 0.0f, float m11 = 1.0f, float m12 = 0.0f,
        float m20 = 0.0f, float m21 = 0.0f, float m22 = 1.0f)
    {
        this._m00 = m00;
        this._m01 = m01;
        this._m02 = m02;

        this._m10 = m10;
        this._m11 = m11;
        this._m12 = m12;

        this._m20 = m20;
        this._m21 = m21;
        this._m22 = m22;
    }

    /// <summary>
    /// Constructs a matrix from Boolean values.
    /// </summary>
    /// <param name="m00">row 0, column 0</param>
    /// <param name="m01">row 0, column 1</param>
    /// <param name="m02">row 0, column 2</param>
    /// <param name="m10">row 1, column 0</param>
    /// <param name="m11">row 1, column 1</param>
    /// <param name="m12">row 1, column 2</param>
    /// <param name="m20">row 2, column 0</param>
    /// <param name="m21">row 2, column 1</param>
    /// <param name="m22">row 2, column 2</param>
    public Mat3 (
        bool m00 = true, bool m01 = false, bool m02 = false,
        bool m10 = false, bool m11 = true, bool m12 = false,
        bool m20 = false, bool m21 = false, bool m22 = true)
    {
        this._m00 = m00 ? 1.0f : 0.0f;
        this._m01 = m01 ? 1.0f : 0.0f;
        this._m02 = m02 ? 1.0f : 0.0f;

        this._m10 = m10 ? 1.0f : 0.0f;
        this._m11 = m11 ? 1.0f : 0.0f;
        this._m12 = m12 ? 1.0f : 0.0f;

        this._m20 = m20 ? 1.0f : 0.0f;
        this._m21 = m21 ? 1.0f : 0.0f;
        this._m22 = m22 ? 1.0f : 0.0f;
    }

    /// <summary>
    /// Tests this matrix for equivalence with an object.
    /// </summary>
    /// <param name="value">the object</param>
    /// <returns>the equivalence</returns>
    public override bool Equals (object value)
    {
        if (Object.ReferenceEquals (this, value)) return true;
        if (Object.ReferenceEquals (null, value)) return false;
        if (value is Mat3) return this.Equals ((Mat3) value);
        return false;
    }

    /// <summary>
    /// Returns a hash code representing this matrix.
    /// </summary>
    /// <returns>the hash code</returns>
    public override int GetHashCode ( )
    {
        int hash = Utils.MulBase ^ this._m00.GetHashCode ( );
        hash = hash * Utils.HashMul ^ this._m01.GetHashCode ( );
        hash = hash * Utils.HashMul ^ this._m02.GetHashCode ( );

        hash = hash * Utils.HashMul ^ this._m10.GetHashCode ( );
        hash = hash * Utils.HashMul ^ this._m11.GetHashCode ( );
        hash = hash * Utils.HashMul ^ this._m12.GetHashCode ( );

        hash = hash * Utils.HashMul ^ this._m20.GetHashCode ( );
        hash = hash * Utils.HashMul ^ this._m21.GetHashCode ( );
        hash = hash * Utils.HashMul ^ this._m22.GetHashCode ( );

        return hash;
    }

    /// <summary>
    /// Returns a string representation of this matrix.
    /// </summary>
    /// <returns>the string</returns>
    public override string ToString ( )
    {
        return this.ToString (4);
    }

    /// <summary>
    /// Tests this matrix for equivalence with another in compliance with the
    /// IEquatable interface.
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>the equivalence</returns>
    public bool Equals (Mat3 m)
    {
        if (this._m00.GetHashCode ( ) != m._m00.GetHashCode ( )) return false;
        if (this._m01.GetHashCode ( ) != m._m01.GetHashCode ( )) return false;
        if (this._m02.GetHashCode ( ) != m._m02.GetHashCode ( )) return false;

        if (this._m10.GetHashCode ( ) != m._m10.GetHashCode ( )) return false;
        if (this._m11.GetHashCode ( ) != m._m11.GetHashCode ( )) return false;
        if (this._m12.GetHashCode ( ) != m._m12.GetHashCode ( )) return false;

        if (this._m20.GetHashCode ( ) != m._m20.GetHashCode ( )) return false;
        if (this._m21.GetHashCode ( ) != m._m21.GetHashCode ( )) return false;
        if (this._m22.GetHashCode ( ) != m._m22.GetHashCode ( )) return false;

        return true;
    }

    /// <summary>
    /// Returns an enumerator (or iterator) for this matrix, allowing its
    /// components to be accessed in a foreach loop.
    /// </summary>
    /// <returns>the enumerator</returns>
    public IEnumerator GetEnumerator ( )
    {
        yield return this._m00;
        yield return this._m01;
        yield return this._m02;

        yield return this._m10;
        yield return this._m11;
        yield return this._m12;

        yield return this._m20;
        yield return this._m21;
        yield return this._m22;
    }

    /// <summary>
    /// Returns a float array of length 9 containing this matrix's components.
    /// </summary>
    /// <returns>the array</returns>
    public float[ ] ToArray1 ( )
    {
        return new float[ ]
        {
            this._m00, this._m01, this._m02,
                this._m10, this._m11, this._m12,
                this._m20, this._m21, this._m22
        };
    }

    /// <summary>
    /// Returns a 3 x 3 float array containing this matrix's components.
    /// </summary>
    /// <returns>the array</returns>
    public float[, ] ToArray2 ( )
    {
        return new float[, ]
        { { this._m00, this._m01, this._m02 }, { this._m10, this._m11, this._m12 }, { this._m20, this._m21, this._m22 }
        };
    }

    /// <summary>
    /// Returns a string representation of this matrix.
    /// </summary>
    /// <param name="places">number of decimal places</param>
    /// <returns>the string</returns>
    public string ToString (in int places = 4)
    {
        return new StringBuilder (128)
            .Append ("{ m00: ")
            .Append (Utils.ToFixed (this._m00, places))
            .Append (", m01: ")
            .Append (Utils.ToFixed (this._m01, places))
            .Append (", m02: ")
            .Append (Utils.ToFixed (this._m02, places))

            .Append (", m10: ")
            .Append (Utils.ToFixed (this._m10, places))
            .Append (", m11: ")
            .Append (Utils.ToFixed (this._m11, places))
            .Append (", m12: ")
            .Append (Utils.ToFixed (this._m12, places))

            .Append (", m20: ")
            .Append (Utils.ToFixed (this._m20, places))
            .Append (", m21: ")
            .Append (Utils.ToFixed (this._m21, places))
            .Append (", m22: ")
            .Append (Utils.ToFixed (this._m22, places))

            .Append (" }")
            .ToString ( );
    }

    /// <summary>
    /// Returns a named value tuple containing this matrix's components.
    /// </summary>
    /// <returns>the tuple</returns>
    public (float m00, float m01, float m02, float m10, float m11, float m12, float m20, float m21, float m22) ToTuple ( )
    {
        return (
            m00: this._m00, m01: this._m01, m02: this._m02,
            m10: this._m10, m11: this._m11, m12: this._m12,
            m20: this._m20, m21: this._m21, m22: this._m22);
    }

    /// <summary>
    /// Converts a boolean to a matrix by supplying the boolean to all the
    /// matrix's components: 1.0 for true; 0.0 for false.
    /// </summary>
    /// <param name="b">the boolean</param>
    /// <returns>the vector</returns>
    public static implicit operator Mat3 (in bool b)
    {
        float eval = b ? 1.0f : 0.0f;
        return new Mat3 (
            eval, eval, eval,
            eval, eval, eval,
            eval, eval, eval);
    }

    /// <summary>
    /// A matrix evaluates to true when all of its components are not equal to
    /// zero.
    /// </summary>
    /// <param name="m">the input matrix</param>
    /// <returns>the evaluation</returns>
    public static bool operator true (in Mat3 m)
    {
        return Mat3.All (m);
    }

    /// <summary>
    /// A matrix evaluates to false when all of its elements are equal to zero.
    /// </summary>
    /// <param name="m">the input matrix</param>
    /// <returns>the evaluation</returns>
    public static bool operator false (in Mat3 m)
    {
        return Mat3.None (m);
    }

    /// <summary>
    /// Evaluates two matrices like booleans, using the and logic gate.
    /// </summary>
    ///   <param name="a">left operand</param>
    ///   <param name="b">right operand</param>
    ///   <returns>the evaluation</returns>
    public static Mat3 operator & (in Mat3 a, in Mat3 b)
    {
        return new Mat3 (
            Utils.And (a._m00, b._m00),
            Utils.And (a._m01, b._m01),
            Utils.And (a._m02, b._m02),
            Utils.And (a._m10, b._m10),
            Utils.And (a._m11, b._m11),
            Utils.And (a._m12, b._m12),
            Utils.And (a._m20, b._m20),
            Utils.And (a._m21, b._m21),
            Utils.And (a._m22, b._m22));
    }

    /// <summary>
    /// Evaluates two matrices like booleans, using the inclusive or (OR) logic
    /// gate.
    /// </summary>
    ///   <param name="a">left operand</param>
    ///   <param name="b">right operand</param>
    ///   <returns>the evaluation</returns>
    public static Mat3 operator | (in Mat3 a, in Mat3 b)
    {
        return new Mat3 (
            Utils.Or (a._m00, b._m00),
            Utils.Or (a._m01, b._m01),
            Utils.Or (a._m02, b._m02),
            Utils.Or (a._m10, b._m10),
            Utils.Or (a._m11, b._m11),
            Utils.Or (a._m12, b._m12),
            Utils.Or (a._m20, b._m20),
            Utils.Or (a._m21, b._m21),
            Utils.Or (a._m22, b._m22));
    }

    /// <summary>
    /// Evaluates two matrices like booleans, using the exclusive or (XOR) logic
    /// gate.
    /// </summary>
    ///   <param name="a">left operand</param>
    ///   <param name="b">right operand</param>
    ///   <returns>the evaluation</returns>
    public static Mat3 operator ^ (in Mat3 a, in Mat3 b)
    {
        return new Mat3 (
            Utils.Xor (a._m00, b._m00),
            Utils.Xor (a._m01, b._m01),
            Utils.Xor (a._m02, b._m02),
            Utils.Xor (a._m10, b._m10),
            Utils.Xor (a._m11, b._m11),
            Utils.Xor (a._m12, b._m12),
            Utils.Xor (a._m20, b._m20),
            Utils.Xor (a._m21, b._m21),
            Utils.Xor (a._m22, b._m22));
    }

    /// <summary>
    /// Negates the input matrix.
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>negation</returns>
    public static Mat3 operator - (in Mat3 m)
    {
        return new Mat3 (-m._m00, -m._m01, -m._m02, -m._m10, -m._m11, -m._m12, -m._m20, -m._m21, -m._m22);
    }

    /// <summary>
    /// Multiplies two matrices.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>product</returns>
    public static Mat3 operator * (in Mat3 a, in Mat3 b)
    {
        return new Mat3 (
            a._m00 * b._m00 + a._m01 * b._m10 + a._m02 * b._m20,
            a._m00 * b._m01 + a._m01 * b._m11 + a._m02 * b._m21,
            a._m00 * b._m02 + a._m01 * b._m12 + a._m02 * b._m22,

            a._m10 * b._m00 + a._m11 * b._m10 + a._m12 * b._m20,
            a._m10 * b._m01 + a._m11 * b._m11 + a._m12 * b._m21,
            a._m10 * b._m02 + a._m11 * b._m12 + a._m12 * b._m22,

            a._m20 * b._m00 + a._m21 * b._m10 + a._m22 * b._m20,
            a._m20 * b._m01 + a._m21 * b._m11 + a._m22 * b._m21,
            a._m20 * b._m02 + a._m21 * b._m12 + a._m22 * b._m22);
    }

    /// <summary>
    /// Multiplies each component in a matrix by a scalar.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>product</returns>
    public static Mat3 operator * (in Mat3 a, in float b)
    {
        return new Mat3 (
            a._m00 * b, a._m01 * b, a._m02 * b,
            a._m10 * b, a._m11 * b, a._m12 * b,
            a._m20 * b, a._m21 * b, a._m22 * b);
    }

    /// <summary>
    /// Multiplies each component in a matrix by a scalar.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>product</returns>
    public static Mat3 operator * (in float a, in Mat3 b)
    {
        return new Mat3 (
            a * b._m00, a * b._m01, a * b._m02,
            a * b._m10, a * b._m11, a * b._m12,
            a * b._m20, a * b._m21, a * b._m22);
    }

    /// <summary>
    /// Multiplies a matrix and a vector.
    /// </summary>
    /// <param name="a">matrix</param>
    /// <param name="b">vector</param>
    /// <returns>product</returns>
    public static Vec3 operator * (in Mat3 a, in Vec3 b)
    {
        return new Vec3 (
            a._m00 * b.x + a._m01 * b.y + a._m02 * b.z,
            a._m10 * b.x + a._m11 * b.y + a._m12 * b.z,
            a._m20 * b.x + a._m21 * b.y + a._m22 * b.z);
    }

    /// <summary>
    /// Divides one matrix by another. Equivalent to multiplying the numerator and
    /// the inverse of the denominator.
    /// </summary>
    /// <param name="a">numerator</param>
    /// <param name="b">denominator</param>
    /// <returns>quotient</returns>
    public static Mat3 operator / (in Mat3 a, in Mat3 b)
    {
        return a * Mat3.Inverse (b);
    }

    /// <summary>
    /// Divides a matrix by a scalar.
    /// </summary>
    /// <param name="a">numerator</param>
    /// <param name="b">denominator</param>
    /// <returns>quotient</returns>
    public static Mat3 operator / (in Mat3 a, in float b)
    {
        return (b != 0.0f) ? a * (1.0f / b) : Mat3.Identity;
    }

    /// <summary>
    /// Divides a scalar by a matrix. Equivalent to multiplying the numerator and
    /// the inverse of the denominator.
    /// </summary>
    /// <param name="a">numerator</param>
    /// <param name="b">denominator</param>
    /// <returns>quotient</returns>
    public static Mat3 operator / (in float a, in Mat3 b)
    {
        return a * Mat3.Inverse (b);
    }

    /// <summary>
    /// Adds two matrices together.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>sum</returns>
    public static Mat3 operator + (in Mat3 a, in Mat3 b)
    {
        return new Mat3 (
            a._m00 + b._m00, a._m01 + b._m01, a._m02 + b._m02,
            a._m10 + b._m10, a._m11 + b._m11, a._m12 + b._m12,
            a._m20 + b._m20, a._m21 + b._m21, a._m22 + b._m22);
    }

    /// <summary>
    /// Subtracts the right matrix from the left matrix.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>result</returns>
    public static Mat3 operator - (in Mat3 a, in Mat3 b)
    {
        return new Mat3 (
            a._m00 - b._m00, a._m01 - b._m01, a._m02 - b._m02,
            a._m10 - b._m10, a._m11 - b._m11, a._m12 - b._m12,
            a._m20 - b._m20, a._m21 - b._m21, a._m22 - b._m22);
    }

    /// <summary>
    /// Evaluates whether the left operand is less than the right operand.
    ///
    /// The return type is not a boolean, but a matrix, where 1.0 is true and 0.0
    /// is false.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static Mat3 operator < (in Mat3 a, in Mat3 b)
    {
        return new Mat3 (
            a._m00 < b._m00, a._m01 < b._m01, a._m02 < b._m02,
            a._m10 < b._m10, a._m11 < b._m11, a._m12 < b._m12,
            a._m20 < b._m20, a._m21 < b._m21, a._m22 < b._m22);
    }

    /// <summary>
    /// Evaluates whether the left operand is greater than the right operand.
    ///
    /// The return type is not a boolean, but a matrix, where 1.0 is true and 0.0
    /// is false.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static Mat3 operator > (in Mat3 a, in Mat3 b)
    {
        return new Mat3 (
            a._m00 > b._m00, a._m01 > b._m01, a._m02 > b._m02,
            a._m10 > b._m10, a._m11 > b._m11, a._m12 > b._m12,
            a._m20 > b._m20, a._m21 > b._m21, a._m22 > b._m22);
    }

    /// <summary>
    /// Evaluates whether the left operand is less than or equal to the right
    /// operand.
    ///
    /// The return type is not a boolean, but a matrix, where 1.0 is true and 0.0
    /// is false.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static Mat3 operator <= (in Mat3 a, in Mat3 b)
    {
        return new Mat3 (
            a._m00 <= b._m00, a._m01 <= b._m01, a._m02 <= b._m02,
            a._m10 <= b._m10, a._m11 <= b._m11, a._m12 <= b._m12,
            a._m20 <= b._m20, a._m21 <= b._m21, a._m22 <= b._m22);
    }

    /// <summary>
    /// Evaluates whether the left operand is greater than or equal to the right
    /// operand.
    ///
    /// The return type is not a boolean, but a matrix, where 1.0 is true and 0.0
    /// is false.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static Mat3 operator >= (in Mat3 a, in Mat3 b)
    {
        return new Mat3 (
            a._m00 >= b._m00, a._m01 >= b._m01, a._m02 >= b._m02,
            a._m10 >= b._m10, a._m11 >= b._m11, a._m12 >= b._m12,
            a._m20 >= b._m20, a._m21 >= b._m21, a._m22 >= b._m22);
    }

    /// <summary>
    /// Evaluates whether two matrices do not equal to each other.
    /// 
    /// The return type is not a boolean, but a matrix, where 1.0 is true and 0.0
    /// is false.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static Mat3 operator != (in Mat3 a, in Mat3 b)
    {
        return new Mat3 (
            a._m00 != b._m00, a._m01 != b._m01, a._m02 != b._m02,
            a._m10 != b._m10, a._m11 != b._m11, a._m12 != b._m12,
            a._m20 != b._m20, a._m21 != b._m21, a._m22 != b._m22);
    }

    /// <summary>
    /// Evaluates whether two matrices are equal to each other.
    ///
    /// The return type is not a boolean, but a matrix, where 1.0 is true and 0.0
    /// is false.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static Mat3 operator == (in Mat3 a, in Mat3 b)
    {
        return new Mat3 (
            a._m00 == b._m00, a._m01 == b._m01, a._m02 == b._m02,
            a._m10 == b._m10, a._m11 == b._m11, a._m12 == b._m12,
            a._m20 == b._m20, a._m21 == b._m21, a._m22 == b._m22);
    }

    /// <summary>
    /// Evaluates whether all elements of a matrix are not equal to zero. 
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>the evaluation</returns>
    public static bool All (in Mat3 m)
    {
        return (m._m00 != 0.0f) && (m._m01 != 0.0f) && (m._m02 != 0.0f) &&
            (m._m10 != 0.0f) && (m._m11 != 0.0f) && (m._m12 != 0.0f) &&
            (m._m20 != 0.0f) && (m._m21 != 0.0f) && (m._m22 != 0.0f);
    }

    /// <summary>
    /// Evaluates whether any elements of a matrix are not equal to zero. 
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>the evaluation</returns>
    public static bool Any (in Mat3 m)
    {
        return (m._m00 != 0.0f) || (m._m01 != 0.0f) || (m._m02 != 0.0f) ||
            (m._m10 != 0.0f) || (m._m11 != 0.0f) || (m._m12 != 0.0f) ||
            (m._m20 != 0.0f) || (m._m21 != 0.0f) || (m._m22 != 0.0f);
    }

    /// <summary>
    /// Finds the determinant of the matrix.
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>determinant</returns>
    public static float Determinant (in Mat3 m)
    {
        return m._m00 * (m._m22 * m._m11 - m._m12 * m._m21) +
            m._m01 * (m._m12 * m._m20 - m._m22 * m._m10) +
            m._m02 * (m._m21 * m._m10 - m._m11 * m._m20);
    }

    /// <summary>
    /// Creates a matrix from two axes. The third row and column are assumed to be
    /// (0.0, 0.0, 1.0) .
    /// </summary>
    /// <param name="right">right axis</param>
    /// <param name="forward">forward axis</param>
    /// <returns>matrix</returns>
    public static Mat3 FromAxes (in Vec2 right, in Vec2 forward)
    {
        return new Mat3 (
            right.x, forward.x, 0.0f,
            right.y, forward.y, 0.0f,
            0.0f, 0.0f, 1.0f);
    }

    /// <summary>
    /// Creates a matrix from two axes and a translation. The third row, z or w,
    /// is assumed to be (0.0, 0.0, 1.0) .
    /// </summary>
    /// <param name="right">right axis</param>
    /// <param name="forward">forward axis</param>
    /// <param name="translation">translation</param>
    /// <returns>matrix</returns>
    public static Mat3 FromAxes (in Vec2 right, in Vec2 forward, in Vec2 translation)
    {
        return new Mat3 (
            right.x, forward.x, translation.x,
            right.y, forward.y, translation.y,
            0.0f, 0.0f, 1.0f);
    }

    /// <summary>
    /// Creates a matrix from two axes. The third column, translation, is assumed
    /// to be (0.0, 0.0, 1.0).
    /// </summary>
    /// <param name="right">right axis</param>
    /// <param name="forward">forward axis</param>
    /// <returns>matrix</returns>
    public static Mat3 FromAxes (in Vec3 right, in Vec3 forward)
    {
        return new Mat3 (
            right.x, forward.x, 0.0f,
            right.y, forward.y, 0.0f,
            right.z, forward.z, 1.0f);
    }

    /// <summary>
    /// Creates a matrix from two axes and a translation.
    /// </summary>
    /// <param name="right">right axis</param>
    /// <param name="forward">forward axis</param>
    /// <param name="translation">translation</param>
    /// <returns>matrix</returns>
    public static Mat3 FromAxes (in Vec3 right, in Vec3 forward, in Vec3 translation)
    {
        return new Mat3 (
            right.x, forward.x, translation.x,
            right.y, forward.y, translation.y,
            right.z, forward.z, translation.z);
    }

    /// <summary>
    /// Creates a rotation matrix from an angle in radians around the z axis.
    /// </summary>
    /// <param name="radians">angle</param>
    /// <returns>matrix</returns>
    public static Mat3 FromRotZ (in float radians)
    {
        float sina = 0.0f;
        float cosa = 0.0f;
        Utils.SinCos (radians, out sina, out cosa);
        return Mat3.FromRotZ (cosa, sina);
    }

    /// <summary>
    /// Creates a rotation matrix from a cosine and sine around the z axis.
    /// </summary>
    /// <param name="cosa">cosine of an angle</param>
    /// <param name="sina">sine of an angle</param>
    /// <returns>matrix</returns>
    public static Mat3 FromRotZ (in float cosa, in float sina)
    {
        return new Mat3 (
            cosa, -sina, 0.0f,
            sina, cosa, 0.0f,
            0.0f, 0.0f, 1.0f);
    }

    /// <summary>
    /// Creates a scale matrix from a scalar. The bottom right corner, m22, is set
    /// to 1.0 .
    /// </summary>
    /// <param name="scalar">scalar</param>
    /// <returns>matrix</returns>
    public static Mat3 FromScale (in float scalar)
    {
        if (scalar != 0.0f)
        {
            return new Mat3 (
                scalar, 0.0f, 0.0f,
                0.0f, scalar, 0.0f,
                0.0f, 0.0f, 1.0f);
        }
        return Mat3.Identity;
    }

    /// <summary>
    /// Creates a scale matrix from a nonuniform scalar, stored in a vector.
    /// </summary>
    /// <param name="scalar">nonuniform scalar</param>
    /// <returns>matrix</returns>
    public static Mat3 FromScale (in Vec2 scalar)
    {
        if (Vec2.All (scalar))
        {
            return new Mat3 (
                scalar.x, 0.0f, 0.0f,
                0.0f, scalar.y, 0.0f,
                0.0f, 0.0f, 1.0f);
        }
        return Mat3.Identity;
    }

    /// <summary>
    /// Creates a translation matrix from a vector.
    /// </summary>
    /// <param name="tr">translation</param>
    /// <returns>the matrix</returns>
    public static Mat3 FromTranslation (in Vec2 tr)
    {
        return new Mat3 (
            1.0f, 0.0f, tr.x,
            0.0f, 1.0f, tr.y,
            0.0f, 0.0f, 1.0f);
    }

    /// <summary>
    /// Inverts the input matrix.
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>the inverse</returns>
    public static Mat3 Inverse (in Mat3 m)
    {
        float b01 = m._m22 * m._m11 - m._m12 * m._m21;
        float b11 = m._m12 * m._m20 - m._m22 * m._m10;
        float b21 = m._m21 * m._m10 - m._m11 * m._m20;

        float det = m._m00 * b01 + m._m01 * b11 + m._m02 * b21;
        if (det != 0.0f)
        {
            float detInv = 1.0f / det;
            return new Mat3 (
                b01 * detInv,
                (m._m02 * m._m21 - m._m22 * m._m01) * detInv,
                (m._m12 * m._m01 - m._m02 * m._m11) * detInv,

                b11 * detInv,
                (m._m22 * m._m00 - m._m02 * m._m20) * detInv,
                (m._m02 * m._m10 - m._m12 * m._m00) * detInv,

                b21 * detInv,
                (m._m01 * m._m20 - m._m21 * m._m00) * detInv,
                (m._m11 * m._m00 - m._m01 * m._m10) * detInv);
        }

        return Mat3.Identity;
    }

    /// <summary>
    /// Multiplies a matrix and a point. The z component of the point is assumed
    /// to be 1.0 , so the point is impacted by the matrix's translation.
    /// </summary>
    /// <param name="a">matrix</param>
    /// <param name="b">point</param>
    /// <returns>product</returns>
    public static Vec2 MulPoint (in Mat3 a, in Vec2 b)
    {
        float w = a._m20 * b.x + a._m21 * b.y + a._m22;
        if (w != 0.0f)
        {
            float wInv = 1.0f / w;
            return new Vec2 (
                (a._m00 * b.x + a._m01 * b.y + a._m02) * wInv,
                (a._m10 * b.x + a._m11 * b.y + a._m12) * wInv);
        }
        return new Vec2 ( );
    }

    /// <summary>
    /// Multiplies a matrix and a vector. The z component of the vector is assumed
    /// to be 0.0 , so the vector is not impacted by the matrix's translation.
    /// </summary>
    /// <param name="a">matrix</param>
    /// <param name="b">vector</param>
    /// <returns>product</returns>
    public static Vec2 MulVector (in Mat3 a, in Vec2 b)
    {
        float w = a._m20 * b.x + a._m21 * b.y + a._m22;
        if (w != 0.0f)
        {
            float wInv = 1.0f / w;
            return new Vec2 (
                (a._m00 * b.x + a._m01 * b.y) * wInv,
                (a._m10 * b.x + a._m11 * b.y) * wInv);
        }
        return new Vec2 ( );
    }

    /// <summary>
    /// Evaluates whether all elements of a matrix are equal to zero. 
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>the evaluation</returns>
    public static bool None (in Mat3 m)
    {
        return (m._m00 == 0.0f) && (m._m01 == 0.0f) && (m._m02 == 0.0f) &&
            (m._m10 == 0.0f) && (m._m11 == 0.0f) && (m._m12 == 0.0f) &&
            (m._m20 == 0.0f) && (m._m21 == 0.0f) && (m._m22 == 0.0f);
    }

    /// <summary>
    /// Rotates the elements of the input matrix 90 degrees counter-clockwise.
    /// </summary>
    /// <param name="m">input matrix</param>
    /// <returns>rotated matrix</returns>
    public static Mat3 RotateElmsCcw (in Mat3 m)
    {
        return new Mat3 (
            m._m02, m._m12, m._m22,
            m._m01, m._m11, m._m21,
            m._m00, m._m10, m._m20);
    }

    /// <summary>
    /// Rotates the elements of the input matrix 90 degrees clockwise.
    /// </summary>
    /// <param name="m">input matrix</param>
    /// <returns>rotated matrix</returns>
    public static Mat3 RotateElmsCw (in Mat3 m)
    {
        return new Mat3 (
            m._m20, m._m10, m._m00,
            m._m21, m._m11, m._m01,
            m._m22, m._m12, m._m02);
    }

    /// <summary>
    /// Transposes a matrix, switching its row and column elements.
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>transposition</returns>
    public static Mat3 Transpose (in Mat3 m)
    {
        return new Mat3 (
            m._m00, m._m10, m._m20,
            m._m01, m._m11, m._m21,
            m._m02, m._m12, m._m22);
    }

    /// <summary>
    /// Returns the identity matrix, [ 1.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0,
    /// 1.0] .
    /// </summary>
    /// <value>the identity matrix</value>
    public static Mat3 Identity
    {
        get
        {
            return new Mat3 (
                1.0f, 0.0f, 0.0f,
                0.0f, 1.0f, 0.0f,
                0.0f, 0.0f, 1.0f);
        }
    }
}