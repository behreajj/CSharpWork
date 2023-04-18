using System;
using System.Collections;
using System.Text;

/// <summary>
/// A readonly struct influenced by GLSL, OSL and Processing's PMatrix3D.
/// Although this is a 4 x 4 matrix, it is generally assumed to be a 3D affine
/// transform matrix, where the last row is (0.0, 0.0, 0.0, 1.0) .
/// </summary>
[Serializable]
public readonly struct Mat4 : IEquatable<Mat4>, IEnumerable
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
    /// Component in row 0, column 2. The up axis x component.
    /// </summary>
    private readonly float m02;

    /// <summary>
    /// Component in row 0, column 3. The translation x component.
    /// </summary>
    private readonly float m03;

    /// <summary>
    /// Component in row 1, column 0. The right axis y component.
    /// </summary>
    private readonly float m10;

    /// <summary>
    /// Component in row 1, column 1. The forward axis y component.
    /// </summary>
    private readonly float m11;

    /// <summary>
    /// Component in row 1, column 2. The up axis y component.
    /// </summary>
    private readonly float m12;

    /// <summary>
    /// Component in row 1, column 3. The translation y component.
    /// </summary>
    private readonly float m13;

    /// <summary>
    /// Component in row 2, column 0. The right axis z component.
    /// </summary>
    private readonly float m20;

    /// <summary>
    /// Component in row 2, column 1. The forward axis z component.
    /// </summary>
    private readonly float m21;

    /// <summary>
    /// Component in row 2, column 2. The up axis z component.
    /// </summary>
    private readonly float m22;

    /// <summary>
    /// Component in row 2, column 3. The translation z component.
    /// </summary>
    private readonly float m23;

    /// <summary>
    /// Component in row 3, column 0. The right axis w component.
    /// </summary>
    private readonly float m30;

    /// <summary>
    /// Component in row 3, column 1. The forward axis w component.
    /// </summary>
    private readonly float m31;

    /// <summary>
    /// Component in row 3, column 2. The up axis w component.
    /// </summary>
    private readonly float m32;

    /// <summary>
    /// Component in row 3, column 3. The translation w component.
    /// </summary>
    private readonly float m33;

    /// <summary>
    /// Returns the number of values in this matrix.
    /// </summary>
    /// <value>length</value>
    public int Length { get { return 16; } }

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
    /// Component in row 0, column 2. The up axis x component.
    /// </summary>
    /// <value>up axis x</value>
    public float M02 { get { return this.m02; } }

    /// <summary>
    /// Component in row 0, column 3. The translation x component.
    /// </summary>
    /// <value>translation x</value>
    public float M03 { get { return this.m03; } }

    /// <summary>
    /// Component in row 1, column 0. The right axis y component.
    /// </summary>
    /// <value>right axis y</value>
    public float M10 { get { return this.m10; } }

    /// <summary>
    /// Component in row 1, column 1. The forward axis y component.
    /// </summary>
    /// <value>forward axis y</value>
    public float M11 { get { return this.m11; } }

    /// <summary>
    /// Component in row 1, column 2. The up axis y component.
    /// </summary>
    /// <value>up axis y</value>
    public float M12 { get { return this.m12; } }

    /// <summary>
    /// Component in row 1, column 3. The translation y component.
    /// </summary>
    /// <value>translation y</value>
    public float M13 { get { return this.m13; } }

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
    /// Component in row 2, column 2. The up axis z component.
    /// </summary>
    /// <value>up axis z</value>
    public float M22 { get { return this.m22; } }

    /// <summary>
    /// Component in row 2, column 3. The translation z component.
    /// </summary>
    /// <value>translation z</value>
    public float M23 { get { return this.m23; } }

    /// <summary>
    /// Component in row 3, column 0. The right axis w component.
    /// </summary>
    /// <value>right axis w</value>
    public float M30 { get { return this.m30; } }

    /// <summary>
    /// Component in row 3, column 1. The forward axis w component.
    /// </summary>
    /// <value>forward axis w</value>
    public float M31 { get { return this.m31; } }

    /// <summary>
    /// Component in row 3, column 2. The up axis w component.
    /// </summary>
    /// <value>up axis w</value>
    public float M32 { get { return this.m32; } }

    /// <summary>
    /// Component in row 3, column 3. The translation w component.
    /// </summary>
    /// <value>translation w</value>
    public float M33 { get { return this.m33; } }

    /// <summary>
    /// The first column, or right axis.
    /// </summary>
    /// <returns>right axis</returns>
    public Vec4 Right
    {
        get
        {
            return new Vec4(this.m00, this.m10, this.m20, this.m30);
        }
    }

    /// <summary>
    /// The second column, or forward axis.
    /// </summary>
    /// <returns>forward</returns>
    public Vec4 Forward
    {
        get
        {
            return new Vec4(this.m01, this.m11, this.m21, this.m31);
        }
    }

    /// <summary>
    /// The third column, or up axis.
    /// </summary>
    /// <returns>up</returns>
    public Vec4 Up
    {
        get
        {
            return new Vec4(this.m02, this.m12, this.m22, this.m32);
        }
    }

    /// <summary>
    /// The fourth column, or translation.
    /// </summary>
    /// <returns>translation</returns>
    public Vec4 Translation
    {
        get
        {
            return new Vec4(this.m03, this.m13, this.m23, this.m33);
        }
    }

    /// <summary>
    /// Constructs a matrix from float values.
    /// </summary>
    /// <param name="m00">row 0, column 0</param>
    /// <param name="m01">row 0, column 1</param>
    /// <param name="m02">row 0, column 2</param>
    /// <param name="m03">row 0, column 3</param>
    /// <param name="m10">row 1, column 0</param>
    /// <param name="m11">row 1, column 1</param>
    /// <param name="m12">row 1, column 2</param>
    /// <param name="m13">row 1, column 3</param>
    /// <param name="m20">row 2, column 0</param>
    /// <param name="m21">row 2, column 1</param>
    /// <param name="m22">row 2, column 2</param>
    /// <param name="m23">row 2, column 3</param>
    /// <param name="m30">row 3, column 0</param>
    /// <param name="m31">row 3, column 1</param>
    /// <param name="m32">row 3, column 2</param>
    /// <param name="m33">row 3, column 3</param>
    public Mat4(
        in float m00 = 1.0f, in float m01 = 0.0f, in float m02 = 0.0f, in float m03 = 0.0f,
        in float m10 = 0.0f, in float m11 = 1.0f, in float m12 = 0.0f, in float m13 = 0.0f,
        in float m20 = 0.0f, in float m21 = 0.0f, in float m22 = 1.0f, in float m23 = 0.0f,
        in float m30 = 0.0f, in float m31 = 0.0f, in float m32 = 0.0f, in float m33 = 1.0f)
    {
        this.m00 = m00;
        this.m01 = m01;
        this.m02 = m02;
        this.m03 = m03;

        this.m10 = m10;
        this.m11 = m11;
        this.m12 = m12;
        this.m13 = m13;

        this.m20 = m20;
        this.m21 = m21;
        this.m22 = m22;
        this.m23 = m23;

        this.m30 = m30;
        this.m31 = m31;
        this.m32 = m32;
        this.m33 = m33;
    }

    /// <summary>
    /// Constructs a matrix from Boolean values.
    /// </summary>
    /// <param name="m00">row 0, column 0</param>
    /// <param name="m01">row 0, column 1</param>
    /// <param name="m02">row 0, column 2</param>
    /// <param name="m03">row 0, column 3</param>
    /// <param name="m10">row 1, column 0</param>
    /// <param name="m11">row 1, column 1</param>
    /// <param name="m12">row 1, column 2</param>
    /// <param name="m13">row 1, column 3</param>
    /// <param name="m20">row 2, column 0</param>
    /// <param name="m21">row 2, column 1</param>
    /// <param name="m22">row 2, column 2</param>
    /// <param name="m23">row 2, column 3</param>
    /// <param name="m30">row 3, column 0</param>
    /// <param name="m31">row 3, column 1</param>
    /// <param name="m32">row 3, column 2</param>
    /// <param name="m33">row 3, column 3</param>
    public Mat4(
        in bool m00 = true, in bool m01 = false, in bool m02 = false, in bool m03 = false,
        in bool m10 = false, in bool m11 = true, in bool m12 = false, in bool m13 = false,
        in bool m20 = false, in bool m21 = false, in bool m22 = true, in bool m23 = false,
        in bool m30 = false, in bool m31 = false, in bool m32 = false, in bool m33 = true)
    {
        this.m00 = m00 ? 1.0f : 0.0f;
        this.m01 = m01 ? 1.0f : 0.0f;
        this.m02 = m02 ? 1.0f : 0.0f;
        this.m03 = m03 ? 1.0f : 0.0f;

        this.m10 = m10 ? 1.0f : 0.0f;
        this.m11 = m11 ? 1.0f : 0.0f;
        this.m12 = m12 ? 1.0f : 0.0f;
        this.m13 = m13 ? 1.0f : 0.0f;

        this.m20 = m20 ? 1.0f : 0.0f;
        this.m21 = m21 ? 1.0f : 0.0f;
        this.m22 = m22 ? 1.0f : 0.0f;
        this.m23 = m23 ? 1.0f : 0.0f;

        this.m30 = m30 ? 1.0f : 0.0f;
        this.m31 = m31 ? 1.0f : 0.0f;
        this.m32 = m32 ? 1.0f : 0.0f;
        this.m33 = m33 ? 1.0f : 0.0f;
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
        if (value is Mat4 mat) { return this.Equals(mat); }
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
        hash = hash * Utils.HashMul ^ this.m03.GetHashCode();

        hash = hash * Utils.HashMul ^ this.m10.GetHashCode();
        hash = hash * Utils.HashMul ^ this.m11.GetHashCode();
        hash = hash * Utils.HashMul ^ this.m12.GetHashCode();
        hash = hash * Utils.HashMul ^ this.m13.GetHashCode();

        hash = hash * Utils.HashMul ^ this.m20.GetHashCode();
        hash = hash * Utils.HashMul ^ this.m21.GetHashCode();
        hash = hash * Utils.HashMul ^ this.m22.GetHashCode();
        hash = hash * Utils.HashMul ^ this.m23.GetHashCode();

        hash = hash * Utils.HashMul ^ this.m30.GetHashCode();
        hash = hash * Utils.HashMul ^ this.m31.GetHashCode();
        hash = hash * Utils.HashMul ^ this.m32.GetHashCode();
        hash = hash * Utils.HashMul ^ this.m33.GetHashCode();

        return hash;
    }

    /// <summary>
    /// Returns a string representation of this matrix.
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()
    {
        return Mat4.ToString(this);
    }

    /// <summary>
    /// Tests this matrix for equivalence with another in compliance with the
    /// IEquatable interface.
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>equivalence</returns>
    public bool Equals(Mat4 m)
    {
        if (this.m00.GetHashCode() != m.m00.GetHashCode()) { return false; }
        if (this.m01.GetHashCode() != m.m01.GetHashCode()) { return false; }
        if (this.m02.GetHashCode() != m.m02.GetHashCode()) { return false; }
        if (this.m03.GetHashCode() != m.m03.GetHashCode()) { return false; }

        if (this.m10.GetHashCode() != m.m10.GetHashCode()) { return false; }
        if (this.m11.GetHashCode() != m.m11.GetHashCode()) { return false; }
        if (this.m12.GetHashCode() != m.m12.GetHashCode()) { return false; }
        if (this.m13.GetHashCode() != m.m13.GetHashCode()) { return false; }

        if (this.m20.GetHashCode() != m.m20.GetHashCode()) { return false; }
        if (this.m21.GetHashCode() != m.m21.GetHashCode()) { return false; }
        if (this.m22.GetHashCode() != m.m22.GetHashCode()) { return false; }
        if (this.m23.GetHashCode() != m.m23.GetHashCode()) { return false; }

        if (this.m30.GetHashCode() != m.m30.GetHashCode()) { return false; }
        if (this.m31.GetHashCode() != m.m31.GetHashCode()) { return false; }
        if (this.m32.GetHashCode() != m.m32.GetHashCode()) { return false; }
        if (this.m33.GetHashCode() != m.m33.GetHashCode()) { return false; }

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
        yield return this.m03;

        yield return this.m10;
        yield return this.m11;
        yield return this.m12;
        yield return this.m13;

        yield return this.m20;
        yield return this.m21;
        yield return this.m22;
        yield return this.m23;

        yield return this.m30;
        yield return this.m31;
        yield return this.m32;
        yield return this.m33;
    }

    /// <summary>
    /// Returns a float array of length 16 containing this matrix's components.
    /// </summary>
    /// <returns>array</returns>
    public float[] ToArray1()
    {
        return new float[]
        {
            this.m00, this.m01, this.m02, this.m03,
                this.m10, this.m11, this.m12, this.m13,
                this.m20, this.m21, this.m22, this.m23,
                this.m30, this.m31, this.m32, this.m33
        };
    }

    /// <summary>
    /// Returns a 4 x 4 float array containing this matrix's components.
    /// </summary>
    /// <returns>array</returns>
    public float[,] ToArray2()
    {
        return new float[,]
        { { this.m00, this.m01, this.m02, this.m03 },
            { this.m10, this.m11, this.m12, this.m13 },
            { this.m20, this.m21, this.m22, this.m23 },
            { this.m30, this.m31, this.m32, this.m33 }
        };
    }

    /// <summary>
    /// Returns a named value tuple containing this matrix's components.
    /// </summary>
    /// <returns>tuple</returns>
    public (float m00, float m01, float m02, float m03,
            float m10, float m11, float m12, float m13,
            float m20, float m21, float m22, float m23,
            float m30, float m31, float m32, float m33) ToTuple()
    {
        return (
            this.m00, this.m01, this.m02, this.m03,
            this.m10, this.m11, this.m12, this.m13,
            this.m20, this.m21, this.m22, this.m23,
            this.m30, this.m31, this.m32, this.m33);
    }

    /// <summary>
    /// Converts a boolean to a matrix by supplying the boolean to all the
    /// matrix's components: 1.0 for true; 0.0 for false.
    /// </summary>
    /// <param name="b">the boolean</param>
    /// <returns>the vector</returns>
    public static implicit operator Mat4(in bool b)
    {
        float eval = b ? 1.0f : 0.0f;
        return new Mat4(
            eval, eval, eval, eval,
            eval, eval, eval, eval,
            eval, eval, eval, eval,
            eval, eval, eval, eval);
    }

    /// <summary>
    /// Converts a rotation from quaternion to matrix representation.
    /// </summary>
    /// <param name="q">quaternion</param>
    public static implicit operator Mat4(in Quat q)
    {
        return Mat4.FromRotation(q);
    }

    /// <summary>
    /// A matrix evaluates to true when all of its components are not equal to
    /// zero.
    /// </summary>
    /// <param name="m">the input matrix</param>
    /// <returns>evaluation</returns>
    public static bool operator true(in Mat4 m)
    {
        return Mat4.All(m);
    }

    /// <summary>
    /// A matrix evaluates to false when all of its elements are equal to zero.
    /// </summary>
    /// <param name="m">the input matrix</param>
    /// <returns>evaluation</returns>
    public static bool operator false(in Mat4 m)
    {
        return Mat4.None(m);
    }

    /// <summary>
    /// Evaluates two matrices like booleans, using the and logic gate.
    /// </summary>
    ///   <param name="a">left operand</param>
    ///   <param name="b">right operand</param>
    ///   <returns>evaluation</returns>
    public static Mat4 operator &(in Mat4 a, in Mat4 b)
    {
        return new Mat4(
            Utils.And(a.m00, b.m00), Utils.And(a.m01, b.m01),
            Utils.And(a.m02, b.m02), Utils.And(a.m03, b.m03),
            Utils.And(a.m10, b.m10), Utils.And(a.m11, b.m11),
            Utils.And(a.m12, b.m12), Utils.And(a.m13, b.m13),
            Utils.And(a.m20, b.m20), Utils.And(a.m21, b.m21),
            Utils.And(a.m22, b.m22), Utils.And(a.m23, b.m23),
            Utils.And(a.m30, b.m30), Utils.And(a.m31, b.m31),
            Utils.And(a.m32, b.m32), Utils.And(a.m33, b.m33));
    }

    /// <summary>
    /// Evaluates two matrices like booleans, using the inclusive or (OR) logic
    /// gate.
    /// </summary>
    ///   <param name="a">left operand</param>
    ///   <param name="b">right operand</param>
    ///   <returns>evaluation</returns>
    public static Mat4 operator |(in Mat4 a, in Mat4 b)
    {
        return new Mat4(
            Utils.Or(a.m00, b.m00), Utils.Or(a.m01, b.m01),
            Utils.Or(a.m02, b.m02), Utils.Or(a.m03, b.m03),
            Utils.Or(a.m10, b.m10), Utils.Or(a.m11, b.m11),
            Utils.Or(a.m12, b.m12), Utils.Or(a.m13, b.m13),
            Utils.Or(a.m20, b.m20), Utils.Or(a.m21, b.m21),
            Utils.Or(a.m22, b.m22), Utils.Or(a.m23, b.m23),
            Utils.Or(a.m30, b.m30), Utils.Or(a.m31, b.m31),
            Utils.Or(a.m32, b.m32), Utils.Or(a.m33, b.m33));
    }

    /// <summary>
    /// Evaluates two matrices like booleans, using the exclusive or (XOR) logic
    /// gate.
    /// </summary>
    ///   <param name="a">left operand</param>
    ///   <param name="b">right operand</param>
    ///   <returns>evaluation</returns>
    public static Mat4 operator ^(in Mat4 a, in Mat4 b)
    {
        return new Mat4(
            Utils.Xor(a.m00, b.m00), Utils.Xor(a.m01, b.m01),
            Utils.Xor(a.m02, b.m02), Utils.Xor(a.m03, b.m03),
            Utils.Xor(a.m10, b.m10), Utils.Xor(a.m11, b.m11),
            Utils.Xor(a.m12, b.m12), Utils.Xor(a.m13, b.m13),
            Utils.Xor(a.m20, b.m20), Utils.Xor(a.m21, b.m21),
            Utils.Xor(a.m22, b.m22), Utils.Xor(a.m23, b.m23),
            Utils.Xor(a.m30, b.m30), Utils.Xor(a.m31, b.m31),
            Utils.Xor(a.m32, b.m32), Utils.Xor(a.m33, b.m33));
    }

    /// <summary>
    /// Negates the input matrix.
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>negation</returns>
    public static Mat4 operator -(in Mat4 m)
    {
        return new Mat4(
            -m.m00, -m.m01, -m.m02, -m.m03,
            -m.m10, -m.m11, -m.m12, -m.m13,
            -m.m20, -m.m21, -m.m22, -m.m23,
            -m.m30, -m.m31, -m.m32, -m.m33);
    }

    /// <summary>
    /// Multiplies two matrices.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>product</returns>
    public static Mat4 operator *(in Mat4 a, in Mat4 b)
    {
        return new Mat4(
            a.m00 * b.m00 + a.m01 * b.m10 + a.m02 * b.m20 + a.m03 * b.m30,
            a.m00 * b.m01 + a.m01 * b.m11 + a.m02 * b.m21 + a.m03 * b.m31,
            a.m00 * b.m02 + a.m01 * b.m12 + a.m02 * b.m22 + a.m03 * b.m32,
            a.m00 * b.m03 + a.m01 * b.m13 + a.m02 * b.m23 + a.m03 * b.m33,

            a.m10 * b.m00 + a.m11 * b.m10 + a.m12 * b.m20 + a.m13 * b.m30,
            a.m10 * b.m01 + a.m11 * b.m11 + a.m12 * b.m21 + a.m13 * b.m31,
            a.m10 * b.m02 + a.m11 * b.m12 + a.m12 * b.m22 + a.m13 * b.m32,
            a.m10 * b.m03 + a.m11 * b.m13 + a.m12 * b.m23 + a.m13 * b.m33,

            a.m20 * b.m00 + a.m21 * b.m10 + a.m22 * b.m20 + a.m23 * b.m30,
            a.m20 * b.m01 + a.m21 * b.m11 + a.m22 * b.m21 + a.m23 * b.m31,
            a.m20 * b.m02 + a.m21 * b.m12 + a.m22 * b.m22 + a.m23 * b.m32,
            a.m20 * b.m03 + a.m21 * b.m13 + a.m22 * b.m23 + a.m23 * b.m33,

            a.m30 * b.m00 + a.m31 * b.m10 + a.m32 * b.m20 + a.m33 * b.m30,
            a.m30 * b.m01 + a.m31 * b.m11 + a.m32 * b.m21 + a.m33 * b.m31,
            a.m30 * b.m02 + a.m31 * b.m12 + a.m32 * b.m22 + a.m33 * b.m32,
            a.m30 * b.m03 + a.m31 * b.m13 + a.m32 * b.m23 + a.m33 * b.m33);
    }

    /// <summary>
    /// Multiplies each component in a matrix by a scalar.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>product</returns>
    public static Mat4 operator *(in Mat4 a, in float b)
    {
        return new Mat4(
            a.m00 * b, a.m01 * b, a.m02 * b, a.m03 * b,
            a.m10 * b, a.m11 * b, a.m12 * b, a.m13 * b,
            a.m20 * b, a.m21 * b, a.m22 * b, a.m23 * b,
            a.m30 * b, a.m31 * b, a.m32 * b, a.m33 * b);
    }

    /// <summary>
    /// Multiplies each component in a matrix by a scalar.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>product</returns>
    public static Mat4 operator *(in float a, in Mat4 b)
    {
        return new Mat4(
            a * b.m00, a * b.m01, a * b.m02, a * b.m03,
            a * b.m10, a * b.m11, a * b.m12, a * b.m13,
            a * b.m20, a * b.m21, a * b.m22, a * b.m23,
            a * b.m30, a * b.m31, a * b.m32, a * b.m33);
    }

    /// <summary>
    /// Multiplies a matrix and a vector.
    /// </summary>
    /// <param name="a">matrix</param>
    /// <param name="b">vector</param>
    /// <returns>product</returns>
    public static Vec4 operator *(in Mat4 a, in Vec4 b)
    {
        float x = b.X;
        float y = b.Y;
        float z = b.Z;
        float w = b.W;
        return new Vec4(
            a.m00 * x + a.m01 * y + a.m02 * z + a.m03 * w,
            a.m10 * x + a.m11 * y + a.m12 * z + a.m13 * w,
            a.m20 * x + a.m21 * y + a.m22 * z + a.m23 * w,
            a.m30 * x + a.m31 * y + a.m32 * z + a.m33 * w);
    }

    /// <summary>
    /// Multiplies a vector and a matrix's transpose, then transposes the
    /// result.
    /// </summary>
    /// <param name="a">vector</param>
    /// <param name="b">matrix</param>
    /// <returns>product</returns>
    public static Vec4 operator *(in Vec4 a, in Mat4 b)
    {
        float x = a.X;
        float y = a.Y;
        float z = a.Z;
        float w = a.W;
        return new Vec4(
            b.m00 * x + b.m10 * y + b.m20 * z + b.m30 * w,
            b.m01 * x + b.m11 * y + b.m21 * z + b.m31 * w,
            b.m02 * x + b.m12 * y + b.m22 * z + b.m32 * w,
            b.m03 * x + b.m13 * y + b.m23 * z + b.m33 * w);
    }

    /// <summary>
    /// Adds two matrices together.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>sum</returns>
    public static Mat4 operator +(in Mat4 a, in Mat4 b)
    {
        return new Mat4(
            a.m00 + b.m00, a.m01 + b.m01, a.m02 + b.m02, a.m03 + b.m03,
            a.m10 + b.m10, a.m11 + b.m11, a.m12 + b.m12, a.m13 + b.m13,
            a.m20 + b.m20, a.m21 + b.m21, a.m22 + b.m22, a.m23 + b.m23,
            a.m30 + b.m30, a.m31 + b.m31, a.m32 + b.m32, a.m33 + b.m33);
    }

    /// <summary>
    /// Subtracts the right matrix from the left matrix.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>result</returns>
    public static Mat4 operator -(in Mat4 a, in Mat4 b)
    {
        return new Mat4(
            a.m00 - b.m00, a.m01 - b.m01, a.m02 - b.m02, a.m03 - b.m03,
            a.m10 - b.m10, a.m11 - b.m11, a.m12 - b.m12, a.m13 - b.m13,
            a.m20 - b.m20, a.m21 - b.m21, a.m22 - b.m22, a.m23 - b.m23,
            a.m30 - b.m30, a.m31 - b.m31, a.m32 - b.m32, a.m33 - b.m33);
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
    public static Mat4 operator <(in Mat4 a, in Mat4 b)
    {
        return new Mat4(
            a.m00 < b.m00, a.m01 < b.m01, a.m02 < b.m02, a.m03 < b.m03,
            a.m10 < b.m10, a.m11 < b.m11, a.m12 < b.m12, a.m13 < b.m13,
            a.m20 < b.m20, a.m21 < b.m21, a.m22 < b.m22, a.m23 < b.m23,
            a.m30 < b.m30, a.m31 < b.m31, a.m32 < b.m32, a.m33 < b.m33);
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
    public static Mat4 operator >(in Mat4 a, in Mat4 b)
    {
        return new Mat4(
            a.m00 > b.m00, a.m01 > b.m01, a.m02 > b.m02, a.m03 > b.m03,
            a.m10 > b.m10, a.m11 > b.m11, a.m12 > b.m12, a.m13 > b.m13,
            a.m20 > b.m20, a.m21 > b.m21, a.m22 > b.m22, a.m23 > b.m23,
            a.m30 > b.m30, a.m31 > b.m31, a.m32 > b.m32, a.m33 > b.m33);
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
    public static Mat4 operator <=(in Mat4 a, in Mat4 b)
    {
        return new Mat4(
            a.m00 <= b.m00, a.m01 <= b.m01, a.m02 <= b.m02, a.m03 <= b.m03,
            a.m10 <= b.m10, a.m11 <= b.m11, a.m12 <= b.m12, a.m13 <= b.m13,
            a.m20 <= b.m20, a.m21 <= b.m21, a.m22 <= b.m22, a.m23 <= b.m23,
            a.m30 <= b.m30, a.m31 <= b.m31, a.m32 <= b.m32, a.m33 <= b.m33);
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
    public static Mat4 operator >=(in Mat4 a, in Mat4 b)
    {
        return new Mat4(
            a.m00 >= b.m00, a.m01 >= b.m01, a.m02 >= b.m02, a.m03 >= b.m03,
            a.m10 >= b.m10, a.m11 >= b.m11, a.m12 >= b.m12, a.m13 >= b.m13,
            a.m20 >= b.m20, a.m21 >= b.m21, a.m22 >= b.m22, a.m23 >= b.m23,
            a.m30 >= b.m30, a.m31 >= b.m31, a.m32 >= b.m32, a.m33 >= b.m33);
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
    public static Mat4 operator !=(in Mat4 a, in Mat4 b)
    {
        return new Mat4(
            a.m00 != b.m00, a.m01 != b.m01, a.m02 != b.m02, a.m03 != b.m03,
            a.m10 != b.m10, a.m11 != b.m11, a.m12 != b.m12, a.m13 != b.m13,
            a.m20 != b.m20, a.m21 != b.m21, a.m22 != b.m22, a.m23 != b.m23,
            a.m30 != b.m30, a.m31 != b.m31, a.m32 != b.m32, a.m33 != b.m33);
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
    public static Mat4 operator ==(in Mat4 a, in Mat4 b)
    {
        return new Mat4(
            a.m00 == b.m00, a.m01 == b.m01, a.m02 == b.m02, a.m03 == b.m03,
            a.m10 == b.m10, a.m11 == b.m11, a.m12 == b.m12, a.m13 == b.m13,
            a.m20 == b.m20, a.m21 == b.m21, a.m22 == b.m22, a.m23 == b.m23,
            a.m30 == b.m30, a.m31 == b.m31, a.m32 == b.m32, a.m33 == b.m33);
    }

    /// <summary>
    /// Evaluates whether all elements of a matrix are not equal to zero. 
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>evaluation</returns>
    public static bool All(in Mat4 m)
    {
        return (m.m00 != 0.0f) && (m.m01 != 0.0f) && (m.m02 != 0.0f) && (m.m03 != 0.0f) &&
            (m.m10 != 0.0f) && (m.m11 != 0.0f) && (m.m12 != 0.0f) && (m.m13 != 0.0f) &&
            (m.m20 != 0.0f) && (m.m21 != 0.0f) && (m.m22 != 0.0f) && (m.m23 != 0.0f) &&
            (m.m30 != 0.0f) && (m.m31 != 0.0f) && (m.m32 != 0.0f) && (m.m33 != 0.0f);
    }

    /// <summary>
    /// Evaluates whether any elements of a matrix are not equal to zero. 
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>evaluation</returns>
    public static bool Any(in Mat4 m)
    {
        return (m.m00 != 0.0f) || (m.m01 != 0.0f) || (m.m02 != 0.0f) || (m.m03 != 0.0f) ||
            (m.m10 != 0.0f) || (m.m11 != 0.0f) || (m.m12 != 0.0f) || (m.m13 != 0.0f) ||
            (m.m20 != 0.0f) || (m.m21 != 0.0f) || (m.m22 != 0.0f) || (m.m23 != 0.0f) ||
            (m.m30 != 0.0f) || (m.m31 != 0.0f) || (m.m32 != 0.0f) || (m.m33 != 0.0f);
    }

    /// <summary>
    /// Finds the determinant of the matrix.
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>determinant</returns>
    public static float Determinant(in Mat4 m)
    {
        return m.m00 * (m.m11 * m.m22 * m.m33 +
                m.m12 * m.m23 * m.m31 +
                m.m13 * m.m21 * m.m32 -
                m.m13 * m.m22 * m.m31 -
                m.m11 * m.m23 * m.m32 -
                m.m12 * m.m21 * m.m33) -
            m.m01 * (m.m10 * m.m22 * m.m33 +
                m.m12 * m.m23 * m.m30 +
                m.m13 * m.m20 * m.m32 -
                m.m13 * m.m22 * m.m30 -
                m.m10 * m.m23 * m.m32 -
                m.m12 * m.m20 * m.m33) +
            m.m02 * (m.m10 * m.m21 * m.m33 +
                m.m11 * m.m23 * m.m30 +
                m.m13 * m.m20 * m.m31 -
                m.m13 * m.m21 * m.m30 -
                m.m10 * m.m23 * m.m31 -
                m.m11 * m.m20 * m.m33) -
            m.m03 * (m.m10 * m.m21 * m.m32 +
                m.m11 * m.m22 * m.m30 +
                m.m12 * m.m20 * m.m31 -
                m.m12 * m.m21 * m.m30 -
                m.m10 * m.m22 * m.m31 -
                m.m11 * m.m20 * m.m32);
    }

    /// <summary>
    /// Creates a matrix from two axes. The third axis, up, is assumed to be
    /// (0.0, 0.0, 1.0, 0.0) . The fourth row and column are assumed to be (0.0,
    /// 0.0, 0.0, 1.0) .
    /// </summary>
    /// <param name="right">right axis</param>
    /// <param name="forward">forward axis</param>
    /// <returns>matrix</returns>
    public static Mat4 FromAxes(
        in Vec2 right,
        in Vec2 forward)
    {
        return new Mat4(
            right.X, forward.X, 0.0f, 0.0f,
            right.Y, forward.Y, 0.0f, 0.0f,
            0.0f, 0.0f, 1.0f, 0.0f,
            0.0f, 0.0f, 0.0f, 1.0f);
    }

    /// <summary>
    /// Creates a matrix from two axes and a translation. The third axis, up, is
    /// assumed to be (0.0, 0.0, 1.0, 0.0) . The fourth row, w, is assumed to be
    /// (0.0, 0.0, 0.0, 1.0) .
    /// </summary>
    /// <param name="right">right axis</param>
    /// <param name="forward">forward axis</param>
    /// <param name="translation">translation</param>
    /// <returns>matrix</returns>
    public static Mat4 FromAxes(
        in Vec2 right,
        in Vec2 forward,
        in Vec2 translation)
    {
        return new Mat4(
            right.X, forward.X, 0.0f, translation.X,
            right.Y, forward.Y, 0.0f, translation.Y,
            0.0f, 0.0f, 1.0f, 0.0f,
            0.0f, 0.0f, 0.0f, 1.0f);
    }

    /// <summary>
    /// Creates a matrix from three axes. The fourth row and column are assumed
    /// to be (0.0, 0.0, 0.0, 1.0) .
    /// </summary>
    /// <param name="right">right axis</param>
    /// <param name="forward">forward axis</param>
    /// <param name="up">up axis</param>
    /// <returns>matrix</returns>
    public static Mat4 FromAxes(
        in Vec3 right,
        in Vec3 forward,
        in Vec3 up)
    {
        return new Mat4(
            right.X, forward.X, up.X, 0.0f,
            right.Y, forward.Y, up.Y, 0.0f,
            right.Z, forward.Z, up.Z, 0.0f,
            0.0f, 0.0f, 0.0f, 1.0f);
    }

    /// <summary>
    /// Creates a matrix from three axes and a translation. The fourth row, w,
    /// is assumed to be (0.0, 0.0, 0.0, 1.0) .
    /// </summary>
    /// <param name="right">right axis</param>
    /// <param name="forward">forward axis</param>
    /// <param name="up">up axis</param>
    /// <param name="translation">translation axis</param>
    /// <returns>matrix</returns>
    public static Mat4 FromAxes(
        in Vec3 right,
        in Vec3 forward,
        in Vec3 up,
        in Vec3 translation)
    {
        return new Mat4(
            right.X, forward.X, up.X, translation.X,
            right.Y, forward.Y, up.Y, translation.Y,
            right.Z, forward.Z, up.Z, translation.Z,
            0.0f, 0.0f, 0.0f, 1.0f);
    }

    /// <summary>
    /// Creates a matrix from three axes. The fourth column, translation, is
    /// assumed to be (0.0, 0.0, 0.0, 1.0) .
    /// </summary>
    /// <param name="right">right axis</param>
    /// <param name="forward">forward axis</param>
    /// <param name="up">up axis</param>
    /// <returns>matrix</returns>
    public static Mat4 FromAxes(
        in Vec4 right,
        in Vec4 forward,
        in Vec4 up)
    {
        return new Mat4(
            right.X, forward.X, up.X, 0.0f,
            right.Y, forward.Y, up.Y, 0.0f,
            right.Z, forward.Z, up.Z, 0.0f,
            right.W, forward.W, up.W, 1.0f);
    }

    /// <summary>
    /// Creates a matrix from three axes and a translation.
    /// </summary>
    /// <param name="right">right axis</param>
    /// <param name="forward">forward axis</param>
    /// <param name="up">up axis</param>
    /// <param name="translation">translation</param>
    /// <returns>matrix</returns>
    public static Mat4 FromAxes(
        in Vec4 right,
        in Vec4 forward,
        in Vec4 up,
        in Vec4 translation)
    {
        return new Mat4(
            right.X, forward.X, up.X, translation.X,
            right.Y, forward.Y, up.Y, translation.Y,
            right.Z, forward.Z, up.Z, translation.Z,
            right.W, forward.W, up.W, translation.W);
    }

    /// <summary>
    /// Creates a reflection matrix from a plane represented
    /// by an axis. The vector will be normalized by the function.
    /// </summary>
    /// <param name="v">axis</param>
    /// <returns>matrix</returns>
    public static Mat4 FromReflection(in Vec3 v)
    {
        float mSq = Vec3.MagSq(v);
        if (mSq != 0.0f)
        {
            float mInv = Utils.InvSqrtUnchecked(mSq);
            float ax = v.X * mInv;
            float ay = v.Y * mInv;
            float az = v.Z * mInv;

            float x = -(ax + ax);
            float y = -(ay + ay);
            float z = -(az + az);

            float axay = x * ay;
            float axaz = x * az;
            float ayaz = y * az;

            return new Mat4(
                x * ax + 1.0f, axay, axaz, 0.0f,
                axay, y * ay + 1.0f, ayaz, 0.0f,
                axaz, ayaz, z * az + 1.0f, 0.0f,
                0.0f, 0.0f, 0.0f, 1.0f);
        }
        return Mat4.Identity;
    }

    /// <summary>
    /// Creates a rotation matrix from a quaternion.
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <returns>matrix</returns>
    public static Mat4 FromRotation(in Quat q)
    {
        float w = q.Real;
        Vec3 i = q.Imag;
        float x = i.X;
        float y = i.Y;
        float z = i.Z;

        float x2 = x + x;
        float y2 = y + y;
        float z2 = z + z;

        float xsq2 = x * x2;
        float ysq2 = y * y2;
        float zsq2 = z * z2;

        float xy2 = x * y2;
        float xz2 = x * z2;
        float yz2 = y * z2;

        float wx2 = w * x2;
        float wy2 = w * y2;
        float wz2 = w * z2;

        return new Mat4(
            1.0f - ysq2 - zsq2, xy2 - wz2, xz2 + wy2, 0.0f,
            xy2 + wz2, 1.0f - xsq2 - zsq2, yz2 - wx2, 0.0f,
            xz2 - wy2, yz2 + wx2, 1.0f - xsq2 - ysq2, 0.0f,
            0.0f, 0.0f, 0.0f, 1.0f);
    }

    /// <summary>
    /// Creates a rotation matrix from an angle in radians around an axis. The
    /// axis will be normalized by the function.
    /// </summary>
    /// <param name="radians">angle</param>
    /// <param name="axis">axis</param>
    /// <returns>matrix</returns>
    public static Mat4 FromRotation(in float radians, in Vec3 axis)
    {
        return Mat4.FromRotation(MathF.Cos(radians), MathF.Sin(radians), axis);
    }

    /// <summary>
    /// Creates a rotation matrix from the cosine and sine of an angle around an
    /// axis. The axis will be normalized by the function.
    /// </summary>
    /// <param name="cosa">cosine of an angle</param>
    /// <param name="sina">sine of an angle</param>
    /// <param name="axis">axis</param>
    /// <returns>matrix</returns>
    public static Mat4 FromRotation(in float cosa, in float sina, in Vec3 axis)
    {
        float mSq = Vec3.MagSq(axis);
        if (mSq != 0.0f)
        {
            float mInv = Utils.InvSqrtUnchecked(mSq);
            float ax = axis.X * mInv;
            float ay = axis.Y * mInv;
            float az = axis.Z * mInv;

            float d = 1.0f - cosa;
            float x = ax * d;
            float y = ay * d;
            float z = az * d;

            float axay = x * ay;
            float axaz = x * az;
            float ayaz = y * az;

            return new Mat4(
                cosa + x * ax, axay - sina * az, axaz + sina * ay, 0.0f,
                axay + sina * az, cosa + y * ay, ayaz - sina * ax, 0.0f,
                axaz - sina * ay, ayaz + sina * ax, cosa + z * az, 0.0f,
                0.0f, 0.0f, 0.0f, 1.0f);
        }
        return Mat4.Identity;
    }

    /// <summary>
    /// Creates a rotation matrix from an angle in radians around the x axis.
    /// </summary>
    /// <param name="radians">angle</param>
    /// <returns>matrix</returns>
    public static Mat4 FromRotX(in float radians)
    {
        return Mat4.FromRotX(MathF.Cos(radians), MathF.Sin(radians));
    }

    /// <summary>
    /// Creates a rotation matrix from a cosine and sine around the x axis.
    /// </summary>
    /// <param name="cosa">cosine of an angle</param>
    /// <param name="sina">sine of an angle</param>
    /// <returns>matrix</returns>
    public static Mat4 FromRotX(in float cosa, in float sina)
    {
        return new Mat4(
            1.0f, 0.0f, 0.0f, 0.0f,
            0.0f, cosa, -sina, 0.0f,
            0.0f, sina, cosa, 0.0f,
            0.0f, 0.0f, 0.0f, 1.0f);
    }

    /// <summary>
    /// Creates a rotation matrix from an angle in radians around the y axis.
    /// </summary>
    /// <param name="radians">angle</param>
    /// <returns>matrix</returns>
    public static Mat4 FromRotY(in float radians)
    {
        return Mat4.FromRotY(MathF.Cos(radians), MathF.Sin(radians));
    }

    /// <summary>
    /// Creates a rotation matrix from a cosine and sine around the y axis.
    /// </summary>
    /// <param name="cosa">cosine of an angle</param>
    /// <param name="sina">sine of an angle</param>
    /// <returns>matrix</returns>
    public static Mat4 FromRotY(in float cosa, in float sina)
    {
        return new Mat4(
            cosa, 0.0f, sina, 0.0f,
            0.0f, 1.0f, 0.0f, 0.0f,
            -sina, 0.0f, cosa, 0.0f,
            0.0f, 0.0f, 0.0f, 1.0f);
    }

    /// <summary>
    /// Creates a rotation matrix from an angle in radians around the z axis.
    /// </summary>
    /// <param name="radians">angle</param>
    /// <returns>matrix</returns>
    public static Mat4 FromRotZ(in float radians)
    {
        return Mat4.FromRotZ(MathF.Cos(radians), MathF.Sin(radians));
    }

    /// <summary>
    /// Creates a rotation matrix from a cosine and sine around the z axis.
    /// </summary>
    /// <param name="cosa">cosine of an angle</param>
    /// <param name="sina">sine of an angle</param>
    /// <returns>matrix</returns>
    public static Mat4 FromRotZ(in float cosa, in float sina)
    {
        return new Mat4(
            cosa, -sina, 0.0f, 0.0f,
            sina, cosa, 0.0f, 0.0f,
            0.0f, 0.0f, 1.0f, 0.0f,
            0.0f, 0.0f, 0.0f, 1.0f);
    }

    /// <summary>
    /// Creates a scale matrix from a scalar. The bottom right corner, m33, is
    /// set to 1.0 .
    /// </summary>
    /// <param name="scalar">scalar</param>
    /// <returns>matrix</returns>
    public static Mat4 FromScale(in float scalar)
    {
        if (scalar != 0.0f)
        {
            return new Mat4(
                scalar, 0.0f, 0.0f, 0.0f,
                0.0f, scalar, 0.0f, 0.0f,
                0.0f, 0.0f, scalar, 0.0f,
                0.0f, 0.0f, 0.0f, 1.0f);
        }
        return Mat4.Identity;
    }

    /// <summary>
    /// Creates a scale matrix from a nonuniform scalar stored in a vector.
    /// </summary>
    /// <param name="scalar">nonuniform scalar</param>
    /// <returns>matrix</returns>
    public static Mat4 FromScale(in Vec2 scalar)
    {
        if (Vec2.All(scalar))
        {
            return new Mat4(
                scalar.X, 0.0f, 0.0f, 0.0f,
                0.0f, scalar.Y, 0.0f, 0.0f,
                0.0f, 0.0f, 1.0f, 0.0f,
                0.0f, 0.0f, 0.0f, 1.0f);
        }
        return Mat4.Identity;
    }

    /// <summary>
    /// Creates a scale matrix from a nonuniform scalar stored in a vector.
    /// </summary>
    /// <param name="scalar">nonuniform scalar</param>
    /// <returns>matrix</returns>
    public static Mat4 FromScale(in Vec3 scalar)
    {
        if (Vec3.All(scalar))
        {
            return new Mat4(
                scalar.X, 0.0f, 0.0f, 0.0f,
                0.0f, scalar.Y, 0.0f, 0.0f,
                0.0f, 0.0f, scalar.Z, 0.0f,
                0.0f, 0.0f, 0.0f, 1.0f);
        }
        return Mat4.Identity;
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
    public static Mat4 FromSkew(in float radians, in Vec3 a, in Vec3 b)
    {
        // TODO: Validate and normalize axes?
        if (Utils.Approx(Utils.RemFloor(radians, MathF.PI), 0.0f))
        {
            return Mat4.Identity;
        }

        float t = MathF.Tan(radians);
        float tax = a.X * t;
        float tay = a.Y * t;
        float taz = a.Z * t;

        return new Mat4(
            tax * b.X + 1.0f, tax * b.Y, tax * b.Z, 0.0f,
            tay * b.X, tay * b.Y + 1.0f, tay * b.Z, 0.0f,
            taz * b.X, taz * b.Y, taz * b.Z + 1.0f, 0.0f,
            0.0f, 0.0f, 0.0f, 1.0f);
    }

    /// <summary>
    /// Creates a translation matrix from a vector.
    /// </summary>
    /// <param name="translation">translation</param>
    /// <returns>matrix</returns>
    public static Mat4 FromTranslation(in Vec2 translation)
    {
        return new Mat4(
            1.0f, 0.0f, 0.0f, translation.X,
            0.0f, 1.0f, 0.0f, translation.Y,
            0.0f, 0.0f, 1.0f, 0.0f,
            0.0f, 0.0f, 0.0f, 1.0f);
    }

    /// <summary>
    /// Creates a translation matrix from a vector.
    /// </summary>
    /// <param name="translation">translation</param>
    /// <returns>matrix</returns>
    public static Mat4 FromTranslation(in Vec3 translation)
    {
        return new Mat4(
            1.0f, 0.0f, 0.0f, translation.X,
            0.0f, 1.0f, 0.0f, translation.Y,
            0.0f, 0.0f, 1.0f, translation.Z,
            0.0f, 0.0f, 0.0f, 1.0f);
    }

    /// <summary>
    /// Creates a view frustum given the edges of the view port.
    /// </summary>
    /// <param name="left">window left edge</param>
    /// <param name="right">window right edge</param>
    /// <param name="bottom">window bottom edge</param>
    /// <param name="top">window top edge</param>
    /// <param name="near">near clip plane</param>
    /// <param name="far">far clip plane</param>
    /// <returns>orthographic projection</returns>
    public static Mat4 Frustum(
        in float left, in float right,
        in float bottom, in float top,
        in float near, in float far)
    {
        float n2 = near + near;

        float w = right - left;
        float h = top - bottom;
        float d = far - near;

        w = (w != 0.0f) ? 1.0f / w : 1.0f;
        h = (h != 0.0f) ? 1.0f / h : 1.0f;
        d = (d != 0.0f) ? 1.0f / d : 1.0f;

        return new Mat4(
            n2 * w, 0.0f, (right + left) * w, 0.0f,
            0.0f, n2 * h, (top + bottom) * h, 0.0f,
            0.0f, 0.0f, (far + near) * -d, n2 * far * -d,
            0.0f, 0.0f, -1.0f, 0.0f);
    }

    /// <summary>
    /// Inverts the input matrix. Returns the identity
    /// if the matrix's determinant is zero.
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>inverse</returns>
    public static Mat4 Inverse(in Mat4 m)
    {
        float b00 = m.m00 * m.m11 - m.m01 * m.m10;
        float b01 = m.m00 * m.m12 - m.m02 * m.m10;
        float b02 = m.m00 * m.m13 - m.m03 * m.m10;
        float b03 = m.m01 * m.m12 - m.m02 * m.m11;
        float b04 = m.m01 * m.m13 - m.m03 * m.m11;
        float b05 = m.m02 * m.m13 - m.m03 * m.m12;
        float b06 = m.m20 * m.m31 - m.m21 * m.m30;
        float b07 = m.m20 * m.m32 - m.m22 * m.m30;
        float b08 = m.m20 * m.m33 - m.m23 * m.m30;
        float b09 = m.m21 * m.m32 - m.m22 * m.m31;
        float b10 = m.m21 * m.m33 - m.m23 * m.m31;
        float b11 = m.m22 * m.m33 - m.m23 * m.m32;

        float det = b00 * b11 - b01 * b10 +
            b02 * b09 + b03 * b08 -
            b04 * b07 + b05 * b06;
        if (det != 0.0f)
        {
            float detInv = 1.0f / det;
            return new Mat4(
                (m.m11 * b11 - m.m12 * b10 + m.m13 * b09) * detInv,
                (m.m02 * b10 - m.m01 * b11 - m.m03 * b09) * detInv,
                (m.m31 * b05 - m.m32 * b04 + m.m33 * b03) * detInv,
                (m.m22 * b04 - m.m21 * b05 - m.m23 * b03) * detInv,
                (m.m12 * b08 - m.m10 * b11 - m.m13 * b07) * detInv,
                (m.m00 * b11 - m.m02 * b08 + m.m03 * b07) * detInv,
                (m.m32 * b02 - m.m30 * b05 - m.m33 * b01) * detInv,
                (m.m20 * b05 - m.m22 * b02 + m.m23 * b01) * detInv,
                (m.m10 * b10 - m.m11 * b08 + m.m13 * b06) * detInv,
                (m.m01 * b08 - m.m00 * b10 - m.m03 * b06) * detInv,
                (m.m30 * b04 - m.m31 * b02 + m.m33 * b00) * detInv,
                (m.m21 * b02 - m.m20 * b04 - m.m23 * b00) * detInv,
                (m.m11 * b07 - m.m10 * b09 - m.m12 * b06) * detInv,
                (m.m00 * b09 - m.m01 * b07 + m.m02 * b06) * detInv,
                (m.m31 * b01 - m.m30 * b03 - m.m32 * b00) * detInv,
                (m.m20 * b03 - m.m21 * b01 + m.m22 * b00) * detInv);
        }

        return Mat4.Identity;
    }

    /// <summary>
    /// Multiplies a normal with a matrix that has already
    /// been inverted.
    /// </summary>
    /// <param name="h">matrix inverse</param>
    /// <param name="n">normal</param>
    /// <returns>product</returns>
    public static Vec3 MulInverseNormal(in Mat4 h, in Vec3 n)
    {
        float x = n.X * h.m00 + n.Y * h.m10 + n.Z * h.m20;
        float y = n.X * h.m01 + n.Y * h.m11 + n.Z * h.m21;
        float z = n.X * h.m02 + n.Y * h.m12 + n.Z * h.m22;
        float mSq = x * x + y * y + z * z;
        if (mSq != 0.0f)
        {
            float mInv = Utils.InvSqrtUnchecked(mSq);
            return new Vec3(x * mInv, y * mInv, z * mInv);
        }
        return Vec3.Zero;
    }

    /// <summary>
    /// Multiplies a matrix and a normal. Calculates the
    /// inverse of the matrix to do so.
    /// </summary>
    /// <param name="a">matrix</param>
    /// <param name="n">normal</param>
    /// <returns>product</returns>
    public static Vec3 MulNormal(in Mat4 a, in Vec3 b)
    {
        return Mat4.MulInverseNormal(Mat4.Inverse(a), b);
    }

    /// <summary>
    /// Multiplies a matrix and a point. The z component of the point is assumed
    /// to be 0.0 . The w component of the point is assumed to be 1.0 , so the
    /// point is impacted by the matrix's translation.
    /// </summary>
    /// <param name="a">matrix</param>
    /// <param name="b">vector</param>
    /// <returns>product</returns>
    public static Vec3 MulPoint(in Mat4 a, in Vec2 b)
    {
        float w = a.m30 * b.X + a.m31 * b.Y + a.m33;
        if (w != 0.0f)
        {
            float wInv = 1.0f / w;
            return new Vec3(
                (a.m00 * b.X + a.m01 * b.Y + a.m03) * wInv,
                (a.m10 * b.X + a.m11 * b.Y + a.m13) * wInv,
                (a.m20 * b.X + a.m21 * b.Y + a.m23) * wInv);
        }
        return Vec3.Zero;
    }

    /// <summary>
    /// Multiplies a matrix and a point. The w component of the point is assumed
    /// to be 1.0 , so the point is impacted by the matrix's translation.
    /// </summary>
    /// <param name="a">matrix</param>
    /// <param name="b">vector</param>
    /// <returns>product</returns>
    public static Vec3 MulPoint(in Mat4 a, in Vec3 b)
    {
        float w = a.m30 * b.X + a.m31 * b.Y + a.m32 * b.Z + a.m33;
        if (w != 0.0f)
        {
            float wInv = 1.0f / w;
            return new Vec3(
                (a.m00 * b.X + a.m01 * b.Y + a.m02 * b.Z + a.m03) * wInv,
                (a.m10 * b.X + a.m11 * b.Y + a.m12 * b.Z + a.m13) * wInv,
                (a.m20 * b.X + a.m21 * b.Y + a.m22 * b.Z + a.m23) * wInv);
        }
        return Vec3.Zero;
    }

    /// <summary>
    /// Multiplies a matrix and a vector. The z and w components of the vector
    /// are assumed to be 0.0 , so the vector is not impacted by the matrix's
    /// translation.
    /// </summary>
    /// <param name="a">matrix</param>
    /// <param name="b">vector</param>
    /// <returns>product</returns>
    public static Vec3 MulVector(in Mat4 a, in Vec2 b)
    {
        float w = a.m30 * b.X + a.m31 * b.Y + a.m33;
        if (w != 0.0f)
        {
            float wInv = 1.0f / w;
            return new Vec3(
                (a.m00 * b.X + a.m01 * b.Y) * wInv,
                (a.m10 * b.X + a.m11 * b.Y) * wInv,
                (a.m20 * b.X + a.m21 * b.Y) * wInv);
        }
        return Vec3.Zero;
    }

    /// <summary>
    /// Multiplies a matrix and a vector. The w component of the vector is
    /// assumed to be 0.0 , so the vector is not impacted by the matrix's
    /// translation.
    /// </summary>
    /// <param name="a">matrix</param>
    /// <param name="b">vector</param>
    /// <returns>product</returns>
    public static Vec3 MulVector(in Mat4 a, in Vec3 b)
    {
        float w = a.m30 * b.X + a.m31 * b.Y + a.m32 * b.Z + a.m33;
        if (w != 0.0f)
        {
            float wInv = 1.0f / w;
            return new Vec3(
                (a.m00 * b.X + a.m01 * b.Y + a.m02 * b.Z) * wInv,
                (a.m10 * b.X + a.m11 * b.Y + a.m12 * b.Z) * wInv,
                (a.m20 * b.X + a.m21 * b.Y + a.m22 * b.Z) * wInv);
        }
        return Vec3.Zero;
    }

    /// <summary>
    /// Evaluates whether all elements of a matrix are equal to zero. 
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>evaluation</returns>
    public static bool None(in Mat4 m)
    {
        return (m.m00 == 0.0f) && (m.m01 == 0.0f) && (m.m02 == 0.0f) && (m.m03 == 0.0f) &&
            (m.m10 == 0.0f) && (m.m11 == 0.0f) && (m.m12 == 0.0f) && (m.m13 == 0.0f) &&
            (m.m20 == 0.0f) && (m.m21 == 0.0f) && (m.m22 == 0.0f) && (m.m23 == 0.0f) &&
            (m.m30 == 0.0f) && (m.m31 == 0.0f) && (m.m32 == 0.0f) && (m.m33 == 0.0f);
    }

    /// <summary>
    /// Creates an orthographic projection matrix, where objects maintain their
    /// size regardless of distance from the camera.
    /// </summary>
    /// <param name="left">window left edge</param>
    /// <param name="right">window right edge</param>
    /// <param name="bottom">window bottom edge</param>
    /// <param name="top">window top edge</param>
    /// <param name="near">near clip plane</param>
    /// <param name="far">far clip plane</param>
    /// <returns>orthographic projection</returns>
    public static Mat4 Orthographic(
        in float left, in float right,
        in float bottom, in float top,
        in float near, in float far)
    {
        float w = right - left;
        float h = top - bottom;
        float d = far - near;

        w = (w != 0.0f) ? 1.0f / w : 1.0f;
        h = (h != 0.0f) ? 1.0f / h : 1.0f;
        d = (d != 0.0f) ? 1.0f / d : 1.0f;

        return new Mat4(
            w + w, 0.0f, 0.0f, w * (left + right),
            0.0f, h + h, 0.0f, h * (top + bottom),
            0.0f, 0.0f, -(d + d), -d * (far + near),
            0.0f, 0.0f, 0.0f, 1.0f);
    }

    /// <summary>
    /// Creates a perspective projection matrix, where objects nearer to the
    /// camera appear larger than objects distant from the camera.
    /// </summary>
    /// <param name="fov">field of view</param>
    /// <param name="aspect">aspect ratio, width over height</param>
    /// <param name="near">near clip plane</param>
    /// <param name="far">far clip plane</param>
    /// <returns>the perspective projection</returns>
    public static Mat4 Perspective(
        in float fov, in float aspect,
        in float near, in float far)
    {
        float cotfov = Utils.Cot(fov * 0.5f);
        float d = Utils.Div(1.0f, far - near);
        return new Mat4(
            Utils.Div(cotfov, aspect), 0.0f, 0.0f, 0.0f,
            0.0f, cotfov, 0.0f, 0.0f,
            0.0f, 0.0f, (far + near) * -d, (near + near) * far * -d,
            0.0f, 0.0f, -1.0f, 0.0f);
    }

    /// <summary>
    /// Rotates the elements of the input matrix 90 degrees counter-clockwise.
    /// </summary>
    /// <param name="m">input matrix</param>
    /// <returns>rotated matrix</returns>
    public static Mat4 RotateElmsCCW(in Mat4 m)
    {
        return new Mat4(
            m.m03, m.m13, m.m23, m.m33,
            m.m02, m.m12, m.m22, m.m32,
            m.m01, m.m11, m.m21, m.m31,
            m.m00, m.m10, m.m20, m.m30);
    }

    /// <summary>
    /// Rotates the elements of the input matrix 90 degrees clockwise.
    /// </summary>
    /// <param name="m">input matrix</param>
    /// <returns>rotated matrix</returns>
    public static Mat4 RotateElmsCW(in Mat4 m)
    {
        return new Mat4(
            m.m30, m.m20, m.m10, m.m00,
            m.m31, m.m21, m.m11, m.m01,
            m.m32, m.m22, m.m12, m.m02,
            m.m33, m.m23, m.m13, m.m03);
    }

    /// <summary>
    /// Returns a string representation of a matrix.
    /// </summary>
    /// <param name="m">matrix</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string</returns>
    public static string ToString(in Mat4 m, in int places = 4)
    {
        return Mat4.ToString(new StringBuilder(512), m, places).ToString();
    }

    /// <summary>
    /// Appends a representation of a matrix to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="m">matrix</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString(in StringBuilder sb, in Mat4 m, in int places = 4)
    {
        sb.Append("{ m00: ");
        Utils.ToFixed(sb, m.m00, places);
        sb.Append(", m01: ");
        Utils.ToFixed(sb, m.m01, places);
        sb.Append(", m02: ");
        Utils.ToFixed(sb, m.m02, places);
        sb.Append(", m03: ");
        Utils.ToFixed(sb, m.m03, places);

        sb.Append(", m10: ");
        Utils.ToFixed(sb, m.m10, places);
        sb.Append(", m11: ");
        Utils.ToFixed(sb, m.m11, places);
        sb.Append(", m12: ");
        Utils.ToFixed(sb, m.m12, places);
        sb.Append(", m13: ");
        Utils.ToFixed(sb, m.m13, places);

        sb.Append(", m20: ");
        Utils.ToFixed(sb, m.m20, places);
        sb.Append(", m21: ");
        Utils.ToFixed(sb, m.m21, places);
        sb.Append(", m22: ");
        Utils.ToFixed(sb, m.m22, places);
        sb.Append(", m23: ");
        Utils.ToFixed(sb, m.m23, places);

        sb.Append(", m30: ");
        Utils.ToFixed(sb, m.m30, places);
        sb.Append(", m31: ");
        Utils.ToFixed(sb, m.m31, places);
        sb.Append(", m32: ");
        Utils.ToFixed(sb, m.m32, places);
        sb.Append(", m33: ");
        Utils.ToFixed(sb, m.m33, places);

        sb.Append(" }");
        return sb;
    }

    /// <summary>
    /// Transposes a matrix, switching its row and column elements.
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>transposition</returns>
    public static Mat4 Transpose(in Mat4 m)
    {
        return new Mat4(
            m.m00, m.m10, m.m20, m.m30,
            m.m01, m.m11, m.m21, m.m31,
            m.m02, m.m12, m.m22, m.m32,
            m.m03, m.m13, m.m23, m.m33);
    }

    /// <summary>
    /// Returns the identity matrix, [ 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0,
    /// 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0 ] .
    /// </summary>
    /// <value>identity matrix</value>
    public static Mat4 Identity
    {
        get
        {
            return new Mat4(
                1.0f, 0.0f, 0.0f, 0.0f,
                0.0f, 1.0f, 0.0f, 0.0f,
                0.0f, 0.0f, 1.0f, 0.0f,
                0.0f, 0.0f, 0.0f, 1.0f);
        }
    }
}