using System;
using System.Collections;
using System.Text;

/// <summary>
/// A readonly struct influenced by GLSL, OSL and Processing's PMatrix2D.
/// Although this is a 3 x 3 matrix, it is generally assumed to be a 2D affine
/// transform matrix, where the last row is (0.0, 0.0, 1.0) .
/// </summary>
[Serializable]
public readonly struct Mat3 : IEquatable<Mat3>, IEnumerable
{
    /// <summary>
    /// Component in row 0, column 0. The right axis x component.
    /// </summary>
    private readonly float m00;

    /// <summary>
    /// Component in row 0, column 1. The forward axis x component.
    /// </summary>
    private readonly float m01;

    /// <summary>
    /// Component in row 0, column 2. The translation x component.
    /// </summary>
    private readonly float m02;

    /// <summary>
    /// Component in row 1, column 0. The right axis y component.
    /// </summary>
    private readonly float m10;

    /// <summary>
    /// Component in row 1, column 1. The forward axis y component.
    /// </summary>
    private readonly float m11;

    /// <summary>
    /// Component in row 1, column 2. The translation y component.
    /// </summary>
    private readonly float m12;

    /// <summary>
    /// Component in row 2, column 0. The right axis z component.
    /// </summary>
    private readonly float m20;

    /// <summary>
    /// Component in row 2, column 1. The forward axis z component.
    /// </summary>
    private readonly float m21;

    /// <summary>
    /// Component in row 2, column 2. The translation z component.
    /// </summary>
    private readonly float m22;

    /// <summary>
    /// Returns the number of values in this matrix.
    /// </summary>
    /// <value>length</value>
    public int Length { get { return 9; } }

    /// <summary>
    /// Component in row 0, column 0. The right axis x component.
    /// </summary>
    /// <value>right axis x</value>
    public float M00 { get { return this.m00; } }

    /// <summary>
    /// Component in row 0, column 1. The forward axis x component.
    /// </summary>
    /// <value>forward axis x</value>
    public float M01 { get { return this.m01; } }

    /// <summary>
    /// Component in row 0, column 2. The translation x component.
    /// </summary>
    /// <value>translation x</value>
    public float M02 { get { return this.m02; } }

    /// <summary>
    /// Component in row 1, column 0. The right axis y component.
    /// </summary>
    /// <value>right axis y</value>
    public float M10 { get { return this.m10; } }

    /// <summary>
    /// Component in row 1, column 1. The forward axis y component.
    /// </summary>
    /// <value>forward y</value>
    public float M11 { get { return this.m11; } }

    /// <summary>
    /// Component in row 1, column 2. The translation y component.
    /// </summary>
    /// <value>translation y</value>
    public float M12 { get { return this.m12; } }

    /// <summary>
    /// Component in row 2, column 0. The right axis z component.
    /// </summary>
    /// <value>right axis z</value>
    public float M20 { get { return this.m20; } }

    /// <summary>
    /// Component in row 2, column 1. The forward axis z component.
    /// </summary>
    /// <value>forward axis z</value>
    public float M21 { get { return this.m21; } }

    /// <summary>
    /// Component in row 2, column 2. The translation z component.
    /// </summary>
    /// <value>translation z</value>
    public float M22 { get { return this.m22; } }

    /// <summary>
    /// The first column, or right axis.
    /// </summary>
    /// <returns>right axis</returns>
    public Vec3 Right
    {
        get
        {
            return new Vec3(this.m00, this.m10, this.m20);
        }
    }

    /// <summary>
    /// The second column, or forward axis.
    /// </summary>
    /// <returns>forward axis/returns>
    public Vec3 Forward
    {
        get
        {
            return new Vec3(this.m01, this.m11, this.m21);
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
            return new Vec3(this.m02, this.m12, this.m22);
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
    public Mat3(
        in float m00 = 1.0f, in float m01 = 0.0f, in float m02 = 0.0f,
        in float m10 = 0.0f, in float m11 = 1.0f, in float m12 = 0.0f,
        in float m20 = 0.0f, in float m21 = 0.0f, in float m22 = 1.0f)
    {
        this.m00 = m00;
        this.m01 = m01;
        this.m02 = m02;

        this.m10 = m10;
        this.m11 = m11;
        this.m12 = m12;

        this.m20 = m20;
        this.m21 = m21;
        this.m22 = m22;
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
    public Mat3(
        in bool m00 = true, in bool m01 = false, in bool m02 = false,
        in bool m10 = false, in bool m11 = true, in bool m12 = false,
        in bool m20 = false, in bool m21 = false, in bool m22 = true)
    {
        this.m00 = m00 ? 1.0f : 0.0f;
        this.m01 = m01 ? 1.0f : 0.0f;
        this.m02 = m02 ? 1.0f : 0.0f;

        this.m10 = m10 ? 1.0f : 0.0f;
        this.m11 = m11 ? 1.0f : 0.0f;
        this.m12 = m12 ? 1.0f : 0.0f;

        this.m20 = m20 ? 1.0f : 0.0f;
        this.m21 = m21 ? 1.0f : 0.0f;
        this.m22 = m22 ? 1.0f : 0.0f;
    }

    /// <summary>
    /// Tests this matrix for equivalence with an object.
    /// </summary>
    /// <param name="value">the object</param>
    /// <returns>equivalence</returns>
    public override bool Equals(object value)
    {
        if (Object.ReferenceEquals(this, value)) { return true; }
        if (value is null) { return false; }
        if (value is Mat3 mat) { return this.Equals(mat); }
        return false;
    }

    /// <summary>
    /// Returns a hash code representing this matrix.
    /// </summary>
    /// <returns>hash code</returns>
    public override int GetHashCode()
    {
        int hash = Utils.MulBase ^ this.m00.GetHashCode();
        hash = hash * Utils.HashMul ^ this.m01.GetHashCode();
        hash = hash * Utils.HashMul ^ this.m02.GetHashCode();

        hash = hash * Utils.HashMul ^ this.m10.GetHashCode();
        hash = hash * Utils.HashMul ^ this.m11.GetHashCode();
        hash = hash * Utils.HashMul ^ this.m12.GetHashCode();

        hash = hash * Utils.HashMul ^ this.m20.GetHashCode();
        hash = hash * Utils.HashMul ^ this.m21.GetHashCode();
        hash = hash * Utils.HashMul ^ this.m22.GetHashCode();

        return hash;
    }

    /// <summary>
    /// Returns a string representation of this matrix.
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()
    {
        return Mat3.ToString(this);
    }

    /// <summary>
    /// Tests this matrix for equivalence with another in compliance with the
    /// IEquatable interface.
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>equivalence</returns>
    public bool Equals(Mat3 m)
    {
        if (this.m00.GetHashCode() != m.m00.GetHashCode()) { return false; }
        if (this.m01.GetHashCode() != m.m01.GetHashCode()) { return false; }
        if (this.m02.GetHashCode() != m.m02.GetHashCode()) { return false; }

        if (this.m10.GetHashCode() != m.m10.GetHashCode()) { return false; }
        if (this.m11.GetHashCode() != m.m11.GetHashCode()) { return false; }
        if (this.m12.GetHashCode() != m.m12.GetHashCode()) { return false; }

        if (this.m20.GetHashCode() != m.m20.GetHashCode()) { return false; }
        if (this.m21.GetHashCode() != m.m21.GetHashCode()) { return false; }
        if (this.m22.GetHashCode() != m.m22.GetHashCode()) { return false; }

        return true;
    }

    /// <summary>
    /// Returns an enumerator (or iterator) for this matrix, allowing its
    /// components to be accessed in a foreach loop.
    /// </summary>
    /// <returns>enumerator</returns>
    public IEnumerator GetEnumerator()
    {
        yield return this.m00;
        yield return this.m01;
        yield return this.m02;

        yield return this.m10;
        yield return this.m11;
        yield return this.m12;

        yield return this.m20;
        yield return this.m21;
        yield return this.m22;
    }

    /// <summary>
    /// Converts a boolean to a matrix by supplying the boolean to all the
    /// matrix's components: 1.0 for true; 0.0 for false.
    /// </summary>
    /// <param name="b">boolean</param>
    /// <returns>vector</returns>
    public static implicit operator Mat3(in bool b)
    {
        float eval = b ? 1.0f : 0.0f;
        return new Mat3(
            eval, eval, eval,
            eval, eval, eval,
            eval, eval, eval);
    }

    /// <summary>
    /// A matrix evaluates to true when all of its components are not equal to
    /// zero.
    /// </summary>
    /// <param name="m">the input matrix</param>
    /// <returns>evaluation</returns>
    public static bool operator true(in Mat3 m)
    {
        return Mat3.All(m);
    }

    /// <summary>
    /// A matrix evaluates to false when all of its elements are equal to zero.
    /// </summary>
    /// <param name="m">the input matrix</param>
    /// <returns>evaluation</returns>
    public static bool operator false(in Mat3 m)
    {
        return Mat3.None(m);
    }

    /// <summary>
    /// Evaluates two matrices like booleans, using the and logic gate.
    /// </summary>
    ///   <param name="a">left operand</param>
    ///   <param name="b">right operand</param>
    ///   <returns>evaluation</returns>
    public static Mat3 operator &(in Mat3 a, in Mat3 b)
    {
        return new Mat3(
            Utils.And(a.m00, b.m00),
            Utils.And(a.m01, b.m01),
            Utils.And(a.m02, b.m02),
            Utils.And(a.m10, b.m10),
            Utils.And(a.m11, b.m11),
            Utils.And(a.m12, b.m12),
            Utils.And(a.m20, b.m20),
            Utils.And(a.m21, b.m21),
            Utils.And(a.m22, b.m22));
    }

    /// <summary>
    /// Evaluates two matrices like booleans, using the inclusive or (OR) logic
    /// gate.
    /// </summary>
    ///   <param name="a">left operand</param>
    ///   <param name="b">right operand</param>
    ///   <returns>evaluation</returns>
    public static Mat3 operator |(in Mat3 a, in Mat3 b)
    {
        return new Mat3(
            Utils.Or(a.m00, b.m00),
            Utils.Or(a.m01, b.m01),
            Utils.Or(a.m02, b.m02),
            Utils.Or(a.m10, b.m10),
            Utils.Or(a.m11, b.m11),
            Utils.Or(a.m12, b.m12),
            Utils.Or(a.m20, b.m20),
            Utils.Or(a.m21, b.m21),
            Utils.Or(a.m22, b.m22));
    }

    /// <summary>
    /// Evaluates two matrices like booleans, using the exclusive or (XOR) logic
    /// gate.
    /// </summary>
    ///   <param name="a">left operand</param>
    ///   <param name="b">right operand</param>
    ///   <returns>evaluation</returns>
    public static Mat3 operator ^(in Mat3 a, in Mat3 b)
    {
        return new Mat3(
            Utils.Xor(a.m00, b.m00),
            Utils.Xor(a.m01, b.m01),
            Utils.Xor(a.m02, b.m02),
            Utils.Xor(a.m10, b.m10),
            Utils.Xor(a.m11, b.m11),
            Utils.Xor(a.m12, b.m12),
            Utils.Xor(a.m20, b.m20),
            Utils.Xor(a.m21, b.m21),
            Utils.Xor(a.m22, b.m22));
    }

    /// <summary>
    /// Negates the input matrix.
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>negation</returns>
    public static Mat3 operator -(in Mat3 m)
    {
        return new Mat3(
            -m.m00, -m.m01, -m.m02,
            -m.m10, -m.m11, -m.m12,
            -m.m20, -m.m21, -m.m22);
    }

    /// <summary>
    /// Multiplies two matrices.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>product</returns>
    public static Mat3 operator *(in Mat3 a, in Mat3 b)
    {
        return new Mat3(
            a.m00 * b.m00 + a.m01 * b.m10 + a.m02 * b.m20,
            a.m00 * b.m01 + a.m01 * b.m11 + a.m02 * b.m21,
            a.m00 * b.m02 + a.m01 * b.m12 + a.m02 * b.m22,

            a.m10 * b.m00 + a.m11 * b.m10 + a.m12 * b.m20,
            a.m10 * b.m01 + a.m11 * b.m11 + a.m12 * b.m21,
            a.m10 * b.m02 + a.m11 * b.m12 + a.m12 * b.m22,

            a.m20 * b.m00 + a.m21 * b.m10 + a.m22 * b.m20,
            a.m20 * b.m01 + a.m21 * b.m11 + a.m22 * b.m21,
            a.m20 * b.m02 + a.m21 * b.m12 + a.m22 * b.m22);
    }

    /// <summary>
    /// Multiplies each component in a matrix by a scalar.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>product</returns>
    public static Mat3 operator *(in Mat3 a, in float b)
    {
        return new Mat3(
            a.m00 * b, a.m01 * b, a.m02 * b,
            a.m10 * b, a.m11 * b, a.m12 * b,
            a.m20 * b, a.m21 * b, a.m22 * b);
    }

    /// <summary>
    /// Multiplies each component in a matrix by a scalar.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>product</returns>
    public static Mat3 operator *(in float a, in Mat3 b)
    {
        return new Mat3(
            a * b.m00, a * b.m01, a * b.m02,
            a * b.m10, a * b.m11, a * b.m12,
            a * b.m20, a * b.m21, a * b.m22);
    }

    /// <summary>
    /// Multiplies a matrix and a vector.
    /// </summary>
    /// <param name="a">matrix</param>
    /// <param name="b">vector</param>
    /// <returns>product</returns>
    public static Vec3 operator *(in Mat3 a, in Vec3 b)
    {
        return new Vec3(
            a.m00 * b.X + a.m01 * b.Y + a.m02 * b.Z,
            a.m10 * b.X + a.m11 * b.Y + a.m12 * b.Z,
            a.m20 * b.X + a.m21 * b.Y + a.m22 * b.Z);
    }

    /// <summary>
    /// Multiplies a vector and a matrix's transpose, then transposes the
    /// result.
    /// </summary>
    /// <param name="a">vector</param>
    /// <param name="b">matrix</param>
    /// <returns>product</returns>
    public static Vec3 operator *(in Vec3 a, in Mat3 b)
    {
        return new Vec3(
            b.m00 * a.X + b.m10 * a.Y + b.m20 * a.Z,
            b.m01 * a.X + b.m11 * a.Y + b.m21 * a.Z,
            b.m02 * a.X + b.m12 * a.Y + b.m22 * a.Z);
    }

    /// <summary>
    /// Adds two matrices together.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>sum</returns>
    public static Mat3 operator +(in Mat3 a, in Mat3 b)
    {
        return new Mat3(
            a.m00 + b.m00, a.m01 + b.m01, a.m02 + b.m02,
            a.m10 + b.m10, a.m11 + b.m11, a.m12 + b.m12,
            a.m20 + b.m20, a.m21 + b.m21, a.m22 + b.m22);
    }

    /// <summary>
    /// Subtracts the right matrix from the left matrix.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>result</returns>
    public static Mat3 operator -(in Mat3 a, in Mat3 b)
    {
        return new Mat3(
            a.m00 - b.m00, a.m01 - b.m01, a.m02 - b.m02,
            a.m10 - b.m10, a.m11 - b.m11, a.m12 - b.m12,
            a.m20 - b.m20, a.m21 - b.m21, a.m22 - b.m22);
    }

    /// <summary>
    /// Evaluates whether the left operand is less than the right operand.
    ///
    /// The return type is not a boolean, but a matrix, where 1.0 is true and
    /// 0.0 is false.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static Mat3 operator <(in Mat3 a, in Mat3 b)
    {
        return new Mat3(
            a.m00 < b.m00, a.m01 < b.m01, a.m02 < b.m02,
            a.m10 < b.m10, a.m11 < b.m11, a.m12 < b.m12,
            a.m20 < b.m20, a.m21 < b.m21, a.m22 < b.m22);
    }

    /// <summary>
    /// Evaluates whether the left operand is greater than the right operand.
    ///
    /// The return type is not a boolean, but a matrix, where 1.0 is true and
    /// 0.0 is false.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static Mat3 operator >(in Mat3 a, in Mat3 b)
    {
        return new Mat3(
            a.m00 > b.m00, a.m01 > b.m01, a.m02 > b.m02,
            a.m10 > b.m10, a.m11 > b.m11, a.m12 > b.m12,
            a.m20 > b.m20, a.m21 > b.m21, a.m22 > b.m22);
    }

    /// <summary>
    /// Evaluates whether the left operand is less than or equal to the right
    /// operand.
    ///
    /// The return type is not a boolean, but a matrix, where 1.0 is true and
    /// 0.0 is false.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static Mat3 operator <=(in Mat3 a, in Mat3 b)
    {
        return new Mat3(
            a.m00 <= b.m00, a.m01 <= b.m01, a.m02 <= b.m02,
            a.m10 <= b.m10, a.m11 <= b.m11, a.m12 <= b.m12,
            a.m20 <= b.m20, a.m21 <= b.m21, a.m22 <= b.m22);
    }

    /// <summary>
    /// Evaluates whether the left operand is greater than or equal to the right
    /// operand.
    ///
    /// The return type is not a boolean, but a matrix, where 1.0 is true and
    /// 0.0 is false.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static Mat3 operator >=(in Mat3 a, in Mat3 b)
    {
        return new Mat3(
            a.m00 >= b.m00, a.m01 >= b.m01, a.m02 >= b.m02,
            a.m10 >= b.m10, a.m11 >= b.m11, a.m12 >= b.m12,
            a.m20 >= b.m20, a.m21 >= b.m21, a.m22 >= b.m22);
    }

    /// <summary>
    /// Evaluates whether two matrices do not equal to each other.
    ///
    /// The return type is not a boolean, but a matrix, where 1.0 is true and
    /// 0.0 is false.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static Mat3 operator !=(in Mat3 a, in Mat3 b)
    {
        return new Mat3(
            a.m00 != b.m00, a.m01 != b.m01, a.m02 != b.m02,
            a.m10 != b.m10, a.m11 != b.m11, a.m12 != b.m12,
            a.m20 != b.m20, a.m21 != b.m21, a.m22 != b.m22);
    }

    /// <summary>
    /// Evaluates whether two matrices are equal to each other.
    ///
    /// The return type is not a boolean, but a matrix, where 1.0 is true and
    /// 0.0 is false.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static Mat3 operator ==(in Mat3 a, in Mat3 b)
    {
        return new Mat3(
            a.m00 == b.m00, a.m01 == b.m01, a.m02 == b.m02,
            a.m10 == b.m10, a.m11 == b.m11, a.m12 == b.m12,
            a.m20 == b.m20, a.m21 == b.m21, a.m22 == b.m22);
    }

    /// <summary>
    /// Evaluates whether all elements of a matrix are not equal to zero. 
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>evaluation</returns>
    public static bool All(in Mat3 m)
    {
        return (m.m00 != 0.0f) && (m.m01 != 0.0f) && (m.m02 != 0.0f) &&
            (m.m10 != 0.0f) && (m.m11 != 0.0f) && (m.m12 != 0.0f) &&
            (m.m20 != 0.0f) && (m.m21 != 0.0f) && (m.m22 != 0.0f);
    }

    /// <summary>
    /// Evaluates whether any elements of a matrix are not equal to zero. 
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>evaluation</returns>
    public static bool Any(in Mat3 m)
    {
        return (m.m00 != 0.0f) || (m.m01 != 0.0f) || (m.m02 != 0.0f) ||
            (m.m10 != 0.0f) || (m.m11 != 0.0f) || (m.m12 != 0.0f) ||
            (m.m20 != 0.0f) || (m.m21 != 0.0f) || (m.m22 != 0.0f);
    }

    /// <summary>
    /// Finds the determinant of the matrix.
    /// Equivalent to the scalar triple product of the matrix's
    /// rows or columns: dot(i, cross(j, k)). See
    /// https://en.wikipedia.org/wiki/Triple_product .
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>determinant</returns>
    public static float Determinant(in Mat3 m)
    {
        return m.m00 * (m.m11 * m.m22 - m.m12 * m.m21) +
               m.m01 * (m.m12 * m.m20 - m.m10 * m.m22) +
               m.m02 * (m.m10 * m.m21 - m.m11 * m.m20);
    }

    /// <summary>
    /// Creates a matrix from two axes. The third row and column are assumed to
    /// be (0.0, 0.0, 1.0) .
    /// </summary>
    /// <param name="right">right axis</param>
    /// <param name="forward">forward axis</param>
    /// <returns>matrix</returns>
    public static Mat3 FromAxes(
        in Vec2 right,
        in Vec2 forward)
    {
        return new Mat3(
            right.X, forward.X, 0.0f,
            right.Y, forward.Y, 0.0f,
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
    public static Mat3 FromAxes(
        in Vec2 right,
        in Vec2 forward,
        in Vec2 translation)
    {
        return new Mat3(
            right.X, forward.X, translation.X,
            right.Y, forward.Y, translation.Y,
            0.0f, 0.0f, 1.0f);
    }

    /// <summary>
    /// Creates a matrix from two axes. The third column, translation, is
    /// assumed to be (0.0, 0.0, 1.0).
    /// </summary>
    /// <param name="right">right axis</param>
    /// <param name="forward">forward axis</param>
    /// <returns>matrix</returns>
    public static Mat3 FromAxes(
        in Vec3 right,
        in Vec3 forward)
    {
        return new Mat3(
            right.X, forward.X, 0.0f,
            right.Y, forward.Y, 0.0f,
            right.Z, forward.Z, 1.0f);
    }

    /// <summary>
    /// Creates a matrix from two axes and a translation.
    /// </summary>
    /// <param name="right">right axis</param>
    /// <param name="forward">forward axis</param>
    /// <param name="translation">translation</param>
    /// <returns>matrix</returns>
    public static Mat3 FromAxes(
        in Vec3 right,
        in Vec3 forward,
        in Vec3 translation)
    {
        return new Mat3(
            right.X, forward.X, translation.X,
            right.Y, forward.Y, translation.Y,
            right.Z, forward.Z, translation.Z);
    }

    /// <summary>
    /// Creates a reflection matrix from a plane represented
    /// by an axis. The vector will be normalized by the function.
    /// </summary>
    /// <param name="v">axis</param>
    /// <returns>matrix</returns>
    public static Mat3 FromReflection(in Vec2 v)
    {
        float mSq = Vec2.MagSq(v);
        if (mSq != 0.0f)
        {
            float mInv = Utils.InvSqrtUnchecked(mSq);
            float ax = v.X * mInv;
            float ay = v.Y * mInv;

            float x = -(ax + ax);
            float y = -(ay + ay);

            float axay = x * ay;

            return new Mat3(
                x * ax + 1.0f, axay, 0.0f,
                axay, y * ay + 1.0f, 0.0f,
                0.0f, 0.0f, 1.0f);
        }
        return Mat3.Identity;
    }

    /// <summary>
    /// Creates a rotation matrix from an angle in radians around the z axis.
    /// </summary>
    /// <param name="radians">angle</param>
    /// <returns>matrix</returns>
    public static Mat3 FromRotZ(in float radians)
    {
        return Mat3.FromRotZ(MathF.Cos(radians), MathF.Sin(radians));
    }

    /// <summary>
    /// Creates a rotation matrix from a cosine and sine around the z axis.
    /// </summary>
    /// <param name="cosa">cosine of an angle</param>
    /// <param name="sina">sine of an angle</param>
    /// <returns>matrix</returns>
    public static Mat3 FromRotZ(in float cosa, in float sina)
    {
        return new Mat3(
            cosa, -sina, 0.0f,
            sina, cosa, 0.0f,
            0.0f, 0.0f, 1.0f);
    }

    /// <summary>
    /// Creates a scale matrix from a scalar. The bottom right corner, m22, is
    /// set to 1.0 .
    /// </summary>
    /// <param name="scalar">scalar</param>
    /// <returns>matrix</returns>
    public static Mat3 FromScale(in float scalar)
    {
        if (scalar != 0.0f)
        {
            return new Mat3(
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
    public static Mat3 FromScale(in Vec2 scalar)
    {
        if (Vec2.All(scalar))
        {
            return new Mat3(
                scalar.X, 0.0f, 0.0f,
                0.0f, scalar.Y, 0.0f,
                0.0f, 0.0f, 1.0f);
        }
        return Mat3.Identity;
    }

    /// <summary>
    /// Creates skew, or shear, matrix from an angle and axes. Vectors a and b
    /// are expected to be orthonormal, i.e. perpendicular and of unit length.
    /// If the angle is a multiple of 90 degrees, returns the identity.
    /// </summary>
    /// <param name="radians">angle in radians</param>
    /// <param name="a">skew axis</param>
    /// <param name="b">orthonormal axis</param>
    /// <returns>skew matrix</returns>
    public static Mat3 FromSkew(in float radians, in Vec2 a, in Vec2 b)
    {
        // TODO: Validate and normalize axes?
        if (Utils.Approx(Utils.RemFloor(radians, MathF.PI), 0.0f))
        {
            return Mat3.Identity;
        }

        float t = MathF.Tan(radians);
        float tax = a.X * t;
        float tay = a.Y * t;

        return new Mat3(
            tax * b.X + 1.0f, tax * b.Y, 0.0f,
            tay * b.X, tay * b.Y + 1.0f, 0.0f,
            0.0f, 0.0f, 1.0f);
    }

    /// <summary>
    /// Creates a translation matrix from a vector.
    /// </summary>
    /// <param name="tr">translation</param>
    /// <returns>the matrix</returns>
    public static Mat3 FromTranslation(in Vec2 tr)
    {
        return new Mat3(
            1.0f, 0.0f, tr.X,
            0.0f, 1.0f, tr.Y,
            0.0f, 0.0f, 1.0f);
    }

    /// <summary>
    /// Inverts the input matrix. Returns the identity
    /// if the matrix's determinant is zero.
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>the inverse</returns>
    public static Mat3 Inverse(in Mat3 m)
    {
        float b01 = m.m22 * m.m11 - m.m12 * m.m21;
        float b11 = m.m12 * m.m20 - m.m22 * m.m10;
        float b21 = m.m21 * m.m10 - m.m11 * m.m20;

        float det = m.m00 * b01 + m.m01 * b11 + m.m02 * b21;
        if (det != 0.0f)
        {
            float detInv = 1.0f / det;
            return new Mat3(
                b01 * detInv,
                (m.m02 * m.m21 - m.m22 * m.m01) * detInv,
                (m.m12 * m.m01 - m.m02 * m.m11) * detInv,

                b11 * detInv,
                (m.m22 * m.m00 - m.m02 * m.m20) * detInv,
                (m.m02 * m.m10 - m.m12 * m.m00) * detInv,

                b21 * detInv,
                (m.m01 * m.m20 - m.m21 * m.m00) * detInv,
                (m.m11 * m.m00 - m.m01 * m.m10) * detInv);
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
    public static Vec2 MulPoint(in Mat3 a, in Vec2 b)
    {
        float w = a.m20 * b.X + a.m21 * b.Y + a.m22;
        if (w != 0.0f)
        {
            float wInv = 1.0f / w;
            return new Vec2(
                (a.m00 * b.X + a.m01 * b.Y + a.m02) * wInv,
                (a.m10 * b.X + a.m11 * b.Y + a.m12) * wInv);
        }
        return Vec2.Zero;
    }

    /// <summary>
    /// Multiplies a matrix and a vector. The z component of the vector is
    /// assumed to be 0.0 , so the vector is not impacted by the matrix's
    /// translation.
    /// </summary>
    /// <param name="a">matrix</param>
    /// <param name="b">vector</param>
    /// <returns>product</returns>
    public static Vec2 MulVector(in Mat3 a, in Vec2 b)
    {
        float w = a.m20 * b.X + a.m21 * b.Y + a.m22;
        if (w != 0.0f)
        {
            float wInv = 1.0f / w;
            return new Vec2(
                (a.m00 * b.X + a.m01 * b.Y) * wInv,
                (a.m10 * b.X + a.m11 * b.Y) * wInv);
        }
        return Vec2.Zero;
    }

    /// <summary>
    /// Evaluates whether all elements of a matrix are equal to zero. 
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>evaluation</returns>
    public static bool None(in Mat3 m)
    {
        return (m.m00 == 0.0f) && (m.m01 == 0.0f) && (m.m02 == 0.0f) &&
            (m.m10 == 0.0f) && (m.m11 == 0.0f) && (m.m12 == 0.0f) &&
            (m.m20 == 0.0f) && (m.m21 == 0.0f) && (m.m22 == 0.0f);
    }

    /// <summary>
    /// Rotates the elements of the input matrix 90 degrees counter-clockwise.
    /// </summary>
    /// <param name="m">input matrix</param>
    /// <returns>rotated matrix</returns>
    public static Mat3 RotateElmsCCW(in Mat3 m)
    {
        return new Mat3(
            m.m02, m.m12, m.m22,
            m.m01, m.m11, m.m21,
            m.m00, m.m10, m.m20);
    }

    /// <summary>
    /// Rotates the elements of the input matrix 90 degrees clockwise.
    /// </summary>
    /// <param name="m">input matrix</param>
    /// <returns>rotated matrix</returns>
    public static Mat3 RotateElmsCW(in Mat3 m)
    {
        return new Mat3(
            m.m20, m.m10, m.m00,
            m.m21, m.m11, m.m01,
            m.m22, m.m12, m.m02);
    }

    /// <summary>
    /// Returns a float array of length 9 containing this matrix's components.
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>array</returns>
    public static float[] ToArray(in Mat3 m)
    {
        return Mat3.ToArray(m, new float[m.Length], 0);
    }

    /// <summary>
    /// Puts a matrix's components into an array at a given index.
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>array</returns>
    public static float[] ToArray(in Mat3 m, in float[] arr, in int i = 0)
    {
        arr[i] = m.m00;
        arr[i + 1] = m.m01;
        arr[i + 2] = m.m02;

        arr[i + 3] = m.m10;
        arr[i + 4] = m.m11;
        arr[i + 5] = m.m12;

        arr[i + 6] = m.m20;
        arr[i + 7] = m.m21;
        arr[i + 8] = m.m22;

        return arr;
    }

    /// <summary>
    /// Returns a string representation of a matrix.
    /// </summary>
    /// <param name="m">matrix</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string</returns>
    public static string ToString(in Mat3 m, in int places = 4)
    {
        return Mat3.ToString(new StringBuilder(256), m, places).ToString();
    }

    /// <summary>
    /// Appends a representation of a matrix to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="m">matrix</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString(in StringBuilder sb, in Mat3 m, in int places = 4)
    {
        sb.Append("{\"m00\":");
        Utils.ToFixed(sb, m.m00, places);
        sb.Append(",\"m01\":");
        Utils.ToFixed(sb, m.m01, places);
        sb.Append(",\"m02\":");
        Utils.ToFixed(sb, m.m02, places);

        sb.Append(",\"m10\":");
        Utils.ToFixed(sb, m.m10, places);
        sb.Append(",\"m11\":");
        Utils.ToFixed(sb, m.m11, places);
        sb.Append(",\"m12\":");
        Utils.ToFixed(sb, m.m12, places);

        sb.Append(",\"m20\":");
        Utils.ToFixed(sb, m.m20, places);
        sb.Append(",\"m21\":");
        Utils.ToFixed(sb, m.m21, places);
        sb.Append(",\"m22\":");
        Utils.ToFixed(sb, m.m22, places);

        sb.Append('}');
        return sb;
    }

    /// <summary>
    /// Transposes a matrix, switching its row and column elements.
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>transposition</returns>
    public static Mat3 Transpose(in Mat3 m)
    {
        return new Mat3(
            m.m00, m.m10, m.m20,
            m.m01, m.m11, m.m21,
            m.m02, m.m12, m.m22);
    }

    /// <summary>
    /// Returns the identity matrix, [ 1.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0,
    /// 1.0] .
    /// </summary>
    /// <value>identity matrix</value>
    public static Mat3 Identity
    {
        get
        {
            return new Mat3(
                1.0f, 0.0f, 0.0f,
                0.0f, 1.0f, 0.0f,
                0.0f, 0.0f, 1.0f);
        }
    }
}