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
  private readonly float _m00;

  /// <summary>
  /// Component in row 0, column 1. The forward axis x component.
  /// </summary>
  private readonly float _m01;

  /// <summary>
  /// Component in row 0, column 2. The up axis x component.
  /// </summary>
  private readonly float _m02;

  /// <summary>
  /// Component in row 0, column 3. The translation x component.
  /// </summary>
  private readonly float _m03;

  /// <summary>
  /// Component in row 1, column 0. The right axis y component.
  /// </summary>
  private readonly float _m10;

  /// <summary>
  /// Component in row 1, column 1. The forward axis y component.
  /// </summary>
  private readonly float _m11;

  /// <summary>
  /// Component in row 1, column 2. The up axis y component.
  /// </summary>
  private readonly float _m12;

  /// <summary>
  /// Component in row 1, column 3. The translation y component.
  /// </summary>
  private readonly float _m13;

  /// <summary>
  /// Component in row 2, column 0. The right axis z component.
  /// </summary>
  private readonly float _m20;

  /// <summary>
  /// Component in row 2, column 1. The forward axis z component.
  /// </summary>
  private readonly float _m21;

  /// <summary>
  /// Component in row 2, column 2. The up axis z component.
  /// </summary>
  private readonly float _m22;

  /// <summary>
  /// Component in row 2, column 3. The translation z component.
  /// </summary>
  private readonly float _m23;

  /// <summary>
  /// Component in row 3, column 0. The right axis w component.
  /// </summary>
  private readonly float _m30;

  /// <summary>
  /// Component in row 3, column 1. The forward axis w component.
  /// </summary>
  private readonly float _m31;

  /// <summary>
  /// Component in row 3, column 2. The up axis w component.
  /// </summary>
  private readonly float _m32;

  /// <summary>
  /// Component in row 3, column 3. The translation w component.
  /// </summary>
  private readonly float _m33;

  /// <summary>
  /// Returns the number of values in this matrix.
  /// </summary>
  /// <value>the length</value>
  public int Length { get { return 16; } }

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
  /// Component in row 0, column 2. The up axis x component.
  /// </summary>
  /// <value>up axis x</value>
  public float m02 { get { return this._m02; } }

  /// <summary>
  /// Component in row 0, column 3. The translation x component.
  /// </summary>
  /// <value>translation x</value>
  public float m03 { get { return this._m03; } }

  /// <summary>
  /// Component in row 1, column 0. The right axis y component.
  /// </summary>
  /// <value>right axis y</value>
  public float m10 { get { return this._m10; } }

  /// <summary>
  /// Component in row 1, column 1. The forward axis y component.
  /// </summary>
  /// <value>forward axis y</value>
  public float m11 { get { return this._m11; } }

  /// <summary>
  /// Component in row 1, column 2. The up axis y component.
  /// </summary>
  /// <value>up axis y</value>
  public float m12 { get { return this._m12; } }

  /// <summary>
  /// Component in row 1, column 3. The translation y component.
  /// </summary>
  /// <value>translation y</value>
  public float m13 { get { return this._m13; } }

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
  /// Component in row 2, column 2. The up axis z component.
  /// </summary>
  /// <value>up axis z</value>
  public float m22 { get { return this._m22; } }

  /// <summary>
  /// Component in row 2, column 3. The translation z component.
  /// </summary>
  /// <value>translation z</value>
  public float m23 { get { return this._m23; } }

  /// <summary>
  /// Component in row 3, column 0. The right axis w component.
  /// </summary>
  /// <value>right axis w</value>
  public float m30 { get { return this._m30; } }

  /// <summary>
  /// Component in row 3, column 1. The forward axis w component.
  /// </summary>
  /// <value>forward axis w</value>
  public float m31 { get { return this._m31; } }

  /// <summary>
  /// Component in row 3, column 2. The up axis w component.
  /// </summary>
  /// <value>up axis w</value>
  public float m32 { get { return this._m32; } }

  /// <summary>
  /// Component in row 3, column 3. The translation w component.
  /// </summary>
  /// <value>translation w</value>
  public float m33 { get { return this._m33; } }

  /// <summary>
  /// The first column, or right axis.
  /// </summary>
  /// <returns>right axis</returns>
  public Vec4 Right
  {
    get
    {
      return new Vec4 (this._m00, this._m10, this._m20, this._m30);
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
      return new Vec4 (this._m01, this._m11, this._m21, this._m31);
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
      return new Vec4 (this._m02, this._m12, this._m22, this._m32);
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
      return new Vec4 (this._m03, this._m13, this._m23, this._m33);
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
        case -16:
          return this._m00;
        case 1:
        case -15:
          return this._m01;
        case 2:
        case -14:
          return this._m02;
        case 3:
        case -13:
          return this._m03;

        case 4:
        case -12:
          return this._m10;
        case 5:
        case -11:
          return this._m11;
        case 6:
        case -10:
          return this._m12;
        case 7:
        case -9:
          return this._m13;

        case 8:
        case -8:
          return this._m20;
        case 9:
        case -7:
          return this._m21;
        case 10:
        case -6:
          return this._m22;
        case 11:
        case -5:
          return this._m23;

        case 12:
        case -4:
          return this._m30;
        case 13:
        case -3:
          return this._m31;
        case 14:
        case -2:
          return this._m32;
        case 15:
        case -1:
          return this._m33;

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
        case -4:
          switch (j)
          {
            case 0:
            case -4:
              return this._m00;
            case 1:
            case -3:
              return this._m01;
            case 2:
            case -2:
              return this._m02;
            case 3:
            case -1:
              return this._m03;
            default:
              return 0.0f;
          }
        case 1:
        case -3:
          switch (j)
          {
            case 0:
            case -4:
              return this._m10;
            case 1:
            case -3:
              return this._m11;
            case 2:
            case -2:
              return this._m12;
            case 3:
            case -1:
              return this._m13;
            default:
              return 0.0f;
          }
        case 2:
        case -2:
          switch (j)
          {
            case 0:
            case -4:
              return this._m20;
            case 1:
            case -3:
              return this._m21;
            case 2:
            case -2:
              return this._m22;
            case 3:
            case -1:
              return this._m23;
            default:
              return 0.0f;
          }
        case 3:
        case -1:
          switch (j)
          {
            case 0:
            case -4:
              return this._m30;
            case 1:
            case -3:
              return this._m31;
            case 2:
            case -2:
              return this._m32;
            case 3:
            case -1:
              return this._m33;
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
  public Mat4 (
    float m00 = 1.0f, float m01 = 0.0f, float m02 = 0.0f, float m03 = 0.0f,
    float m10 = 0.0f, float m11 = 1.0f, float m12 = 0.0f, float m13 = 0.0f,
    float m20 = 0.0f, float m21 = 0.0f, float m22 = 1.0f, float m23 = 0.0f,
    float m30 = 0.0f, float m31 = 0.0f, float m32 = 0.0f, float m33 = 1.0f)
  {
    this._m00 = m00;
    this._m01 = m01;
    this._m02 = m02;
    this._m03 = m03;

    this._m10 = m10;
    this._m11 = m11;
    this._m12 = m12;
    this._m13 = m13;

    this._m20 = m20;
    this._m21 = m21;
    this._m22 = m22;
    this._m23 = m23;

    this._m30 = m30;
    this._m31 = m31;
    this._m32 = m32;
    this._m33 = m33;
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
  public Mat4 (
    bool m00 = true, bool m01 = false, bool m02 = false, bool m03 = false,
    bool m10 = false, bool m11 = true, bool m12 = false, bool m13 = false,
    bool m20 = false, bool m21 = false, bool m22 = true, bool m23 = false,
    bool m30 = false, bool m31 = false, bool m32 = false, bool m33 = true)
  {
    this._m00 = m00 ? 1.0f : 0.0f;
    this._m01 = m01 ? 1.0f : 0.0f;
    this._m02 = m02 ? 1.0f : 0.0f;
    this._m03 = m03 ? 1.0f : 0.0f;

    this._m10 = m10 ? 1.0f : 0.0f;
    this._m11 = m11 ? 1.0f : 0.0f;
    this._m12 = m12 ? 1.0f : 0.0f;
    this._m13 = m13 ? 1.0f : 0.0f;

    this._m20 = m20 ? 1.0f : 0.0f;
    this._m21 = m21 ? 1.0f : 0.0f;
    this._m22 = m22 ? 1.0f : 0.0f;
    this._m23 = m23 ? 1.0f : 0.0f;

    this._m30 = m30 ? 1.0f : 0.0f;
    this._m31 = m31 ? 1.0f : 0.0f;
    this._m32 = m32 ? 1.0f : 0.0f;
    this._m33 = m33 ? 1.0f : 0.0f;
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
    if (value is Mat4) return this.Equals ((Mat4) value);
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
    hash = hash * Utils.HashMul ^ this._m03.GetHashCode ( );

    hash = hash * Utils.HashMul ^ this._m10.GetHashCode ( );
    hash = hash * Utils.HashMul ^ this._m11.GetHashCode ( );
    hash = hash * Utils.HashMul ^ this._m12.GetHashCode ( );
    hash = hash * Utils.HashMul ^ this._m13.GetHashCode ( );

    hash = hash * Utils.HashMul ^ this._m20.GetHashCode ( );
    hash = hash * Utils.HashMul ^ this._m21.GetHashCode ( );
    hash = hash * Utils.HashMul ^ this._m22.GetHashCode ( );
    hash = hash * Utils.HashMul ^ this._m23.GetHashCode ( );

    hash = hash * Utils.HashMul ^ this._m30.GetHashCode ( );
    hash = hash * Utils.HashMul ^ this._m31.GetHashCode ( );
    hash = hash * Utils.HashMul ^ this._m32.GetHashCode ( );
    hash = hash * Utils.HashMul ^ this._m33.GetHashCode ( );

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
  public bool Equals (Mat4 m)
  {
    if (this._m00.GetHashCode ( ) != m._m00.GetHashCode ( )) return false;
    if (this._m01.GetHashCode ( ) != m._m01.GetHashCode ( )) return false;
    if (this._m02.GetHashCode ( ) != m._m02.GetHashCode ( )) return false;
    if (this._m03.GetHashCode ( ) != m._m03.GetHashCode ( )) return false;

    if (this._m10.GetHashCode ( ) != m._m10.GetHashCode ( )) return false;
    if (this._m11.GetHashCode ( ) != m._m11.GetHashCode ( )) return false;
    if (this._m12.GetHashCode ( ) != m._m12.GetHashCode ( )) return false;
    if (this._m13.GetHashCode ( ) != m._m13.GetHashCode ( )) return false;

    if (this._m20.GetHashCode ( ) != m._m20.GetHashCode ( )) return false;
    if (this._m21.GetHashCode ( ) != m._m21.GetHashCode ( )) return false;
    if (this._m22.GetHashCode ( ) != m._m22.GetHashCode ( )) return false;
    if (this._m23.GetHashCode ( ) != m._m23.GetHashCode ( )) return false;

    if (this._m30.GetHashCode ( ) != m._m30.GetHashCode ( )) return false;
    if (this._m31.GetHashCode ( ) != m._m31.GetHashCode ( )) return false;
    if (this._m32.GetHashCode ( ) != m._m32.GetHashCode ( )) return false;
    if (this._m33.GetHashCode ( ) != m._m33.GetHashCode ( )) return false;

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
    yield return this._m03;

    yield return this._m10;
    yield return this._m11;
    yield return this._m12;
    yield return this._m13;

    yield return this._m20;
    yield return this._m21;
    yield return this._m22;
    yield return this._m23;

    yield return this._m30;
    yield return this._m31;
    yield return this._m32;
    yield return this._m33;
  }

  /// <summary>
  /// Returns a float array of length 16 containing this matrix's components.
  /// </summary>
  /// <returns>the array</returns>
  public float[ ] ToArray1 ( )
  {
    return new float[ ]
    {
      this._m00, this._m01, this._m02, this._m03,
        this._m10, this._m11, this._m12, this._m13,
        this._m20, this._m21, this._m22, this._m23,
        this._m30, this._m31, this._m32, this._m33
    };
  }

  /// <summary>
  /// Returns a 4 x 4 float array containing this matrix's components.
  /// </summary>
  /// <returns>the array</returns>
  public float[, ] ToArray2 ( )
  {
    return new float[, ]
    { { this._m00, this._m01, this._m02, this._m03 }, { this._m10, this._m11, this._m12, this._m13 }, { this._m20, this._m21, this._m22, this._m23 }, { this._m30, this._m31, this._m32, this._m33 }
    };
  }

  /// <summary>
  /// Returns a string representation of this matrix.
  /// </summary>
  /// <param name="places">number of decimal places</param>
  /// <returns>the string</returns>
  public string ToString (in int places = 4)
  {
    return new StringBuilder (512)
      .Append ("{ m00: ")
      .Append (Utils.ToFixed (this._m00, places))
      .Append (", m01: ")
      .Append (Utils.ToFixed (this._m01, places))
      .Append (", m02: ")
      .Append (Utils.ToFixed (this._m02, places))
      .Append (", m03: ")
      .Append (Utils.ToFixed (this._m03, places))

      .Append (", m10: ")
      .Append (Utils.ToFixed (this._m10, places))
      .Append (", m11: ")
      .Append (Utils.ToFixed (this._m11, places))
      .Append (", m12: ")
      .Append (Utils.ToFixed (this._m12, places))
      .Append (", m13: ")
      .Append (Utils.ToFixed (this._m13, places))

      .Append (", m20: ")
      .Append (Utils.ToFixed (this._m20, places))
      .Append (", m21: ")
      .Append (Utils.ToFixed (this._m21, places))
      .Append (", m22: ")
      .Append (Utils.ToFixed (this._m22, places))
      .Append (", m23: ")
      .Append (Utils.ToFixed (this._m23, places))

      .Append (", m30: ")
      .Append (Utils.ToFixed (this._m30, places))
      .Append (", m31: ")
      .Append (Utils.ToFixed (this._m31, places))
      .Append (", m32: ")
      .Append (Utils.ToFixed (this._m32, places))
      .Append (", m33: ")
      .Append (Utils.ToFixed (this._m33, places))

      .Append (" }")
      .ToString ( );
  }

  /// <summary>
  /// Returns a named value tuple containing this matrix's components.
  /// </summary>
  /// <returns>the tuple</returns>
  public (float m00, float m01, float m02, float m03, float m10, float m11, float m12, float m13, float m20, float m21, float m22, float m23, float m30, float m31, float m32, float m33) ToTuple ( )
  {
    return (
      m00: this._m00, m01: this._m01, m02: this._m02, m03: this._m03,
      m10: this._m10, m11: this._m11, m12: this._m12, m13: this._m13,
      m20: this._m20, m21: this._m21, m22: this._m22, m23: this._m23,
      m30: this._m30, m31: this._m31, m32: this._m32, m33: this._m33);
  }

  /// <summary>
  /// Converts a boolean to a matrix by supplying the boolean to all the
  /// matrix's components: 1.0 for true; 0.0 for false.
  /// </summary>
  /// <param name="b">the boolean</param>
  /// <returns>the vector</returns>
  public static implicit operator Mat4 (in bool b)
  {
    float eval = b ? 1.0f : 0.0f;
    return new Mat4 (
      eval, eval, eval, eval,
      eval, eval, eval, eval,
      eval, eval, eval, eval,
      eval, eval, eval, eval);
  }

  /// <summary>
  /// Converts a rotation from quaternion to matrix representation.
  /// </summary>
  /// <param name="q">quaternion</param>
  public static implicit operator Mat4 (in Quat q)
  {
    return Mat4.FromRotation (q);
  }

  /// <summary>
  /// A matrix evaluates to true when all of its components are not equal to
  /// zero.
  /// </summary>
  /// <param name="m">the input matrix</param>
  /// <returns>the evaluation</returns>
  public static bool operator true (in Mat4 m)
  {
    return Mat4.All (m);
  }

  /// <summary>
  /// A matrix evaluates to false when all of its elements are equal to zero.
  /// </summary>
  /// <param name="m">the input matrix</param>
  /// <returns>the evaluation</returns>
  public static bool operator false (in Mat4 m)
  {
    return Mat4.None (m);
  }

  /// <summary>
  /// Evaluates two matrices like booleans, using the and logic gate.
  /// </summary>
  ///   <param name="a">left operand</param>
  ///   <param name="b">right operand</param>
  ///   <returns>the evaluation</returns>
  public static Mat4 operator & (in Mat4 a, in Mat4 b)
  {
    return new Mat4 (
      Utils.And (a._m00, b._m00), Utils.And (a._m01, b._m01),
      Utils.And (a._m02, b._m02), Utils.And (a._m03, b._m03),
      Utils.And (a._m10, b._m10), Utils.And (a._m11, b._m11),
      Utils.And (a._m12, b._m12), Utils.And (a._m13, b._m13),
      Utils.And (a._m20, b._m20), Utils.And (a._m21, b._m21),
      Utils.And (a._m22, b._m22), Utils.And (a._m23, b._m23),
      Utils.And (a._m30, b._m30), Utils.And (a._m31, b._m31),
      Utils.And (a._m32, b._m32), Utils.And (a._m33, b._m33));
  }

  /// <summary>
  /// Evaluates two matrices like booleans, using the inclusive or (OR) logic
  /// gate.
  /// </summary>
  ///   <param name="a">left operand</param>
  ///   <param name="b">right operand</param>
  ///   <returns>the evaluation</returns>
  public static Mat4 operator | (in Mat4 a, in Mat4 b)
  {
    return new Mat4 (
      Utils.Or (a._m00, b._m00), Utils.Or (a._m01, b._m01),
      Utils.Or (a._m02, b._m02), Utils.Or (a._m03, b._m03),
      Utils.Or (a._m10, b._m10), Utils.Or (a._m11, b._m11),
      Utils.Or (a._m12, b._m12), Utils.Or (a._m13, b._m13),
      Utils.Or (a._m20, b._m20), Utils.Or (a._m21, b._m21),
      Utils.Or (a._m22, b._m22), Utils.Or (a._m23, b._m23),
      Utils.Or (a._m30, b._m30), Utils.Or (a._m31, b._m31),
      Utils.Or (a._m32, b._m32), Utils.Or (a._m33, b._m33));
  }

  /// <summary>
  /// Evaluates two matrices like booleans, using the exclusive or (XOR) logic
  /// gate.
  /// </summary>
  ///   <param name="a">left operand</param>
  ///   <param name="b">right operand</param>
  ///   <returns>the evaluation</returns>
  public static Mat4 operator ^ (in Mat4 a, in Mat4 b)
  {
    return new Mat4 (
      Utils.Xor (a._m00, b._m00), Utils.Xor (a._m01, b._m01),
      Utils.Xor (a._m02, b._m02), Utils.Xor (a._m03, b._m03),
      Utils.Xor (a._m10, b._m10), Utils.Xor (a._m11, b._m11),
      Utils.Xor (a._m12, b._m12), Utils.Xor (a._m13, b._m13),
      Utils.Xor (a._m20, b._m20), Utils.Xor (a._m21, b._m21),
      Utils.Xor (a._m22, b._m22), Utils.Xor (a._m23, b._m23),
      Utils.Xor (a._m30, b._m30), Utils.Xor (a._m31, b._m31),
      Utils.Xor (a._m32, b._m32), Utils.Xor (a._m33, b._m33));
  }

  /// <summary>
  /// Negates the input matrix.
  /// </summary>
  /// <param name="m">matrix</param>
  /// <returns>negation</returns>
  public static Mat4 operator - (in Mat4 m)
  {
    return new Mat4 (-m._m00, -m._m01, -m._m02, -m._m03, -m._m10, -m._m11, -m._m12, -m._m13, -m._m20, -m._m21, -m._m22, -m._m23, -m._m30, -m._m31, -m._m32, -m._m33);
  }

  /// <summary>
  /// Multiplies two matrices.
  /// </summary>
  /// <param name="a">left operand</param>
  /// <param name="b">right operand</param>
  /// <returns>product</returns>
  public static Mat4 operator * (in Mat4 a, in Mat4 b)
  {
    return new Mat4 (
      a._m00 * b._m00 + a._m01 * b._m10 + a._m02 * b._m20 + a._m03 * b._m30,
      a._m00 * b._m01 + a._m01 * b._m11 + a._m02 * b._m21 + a._m03 * b._m31,
      a._m00 * b._m02 + a._m01 * b._m12 + a._m02 * b._m22 + a._m03 * b._m32,
      a._m00 * b._m03 + a._m01 * b._m13 + a._m02 * b._m23 + a._m03 * b._m33,

      a._m10 * b._m00 + a._m11 * b._m10 + a._m12 * b._m20 + a._m13 * b._m30,
      a._m10 * b._m01 + a._m11 * b._m11 + a._m12 * b._m21 + a._m13 * b._m31,
      a._m10 * b._m02 + a._m11 * b._m12 + a._m12 * b._m22 + a._m13 * b._m32,
      a._m10 * b._m03 + a._m11 * b._m13 + a._m12 * b._m23 + a._m13 * b._m33,

      a._m20 * b._m00 + a._m21 * b._m10 + a._m22 * b._m20 + a._m23 * b._m30,
      a._m20 * b._m01 + a._m21 * b._m11 + a._m22 * b._m21 + a._m23 * b._m31,
      a._m20 * b._m02 + a._m21 * b._m12 + a._m22 * b._m22 + a._m23 * b._m32,
      a._m20 * b._m03 + a._m21 * b._m13 + a._m22 * b._m23 + a._m23 * b._m33,

      a._m30 * b._m00 + a._m31 * b._m10 + a._m32 * b._m20 + a._m33 * b._m30,
      a._m30 * b._m01 + a._m31 * b._m11 + a._m32 * b._m21 + a._m33 * b._m31,
      a._m30 * b._m02 + a._m31 * b._m12 + a._m32 * b._m22 + a._m33 * b._m32,
      a._m30 * b._m03 + a._m31 * b._m13 + a._m32 * b._m23 + a._m33 * b._m33);
  }

  /// <summary>
  /// Multiplies each component in a matrix by a scalar.
  /// </summary>
  /// <param name="a">left operand</param>
  /// <param name="b">right operand</param>
  /// <returns>product</returns>
  public static Mat4 operator * (in Mat4 a, in float b)
  {
    return new Mat4 (
      a._m00 * b, a._m01 * b, a._m02 * b, a._m03 * b,
      a._m10 * b, a._m11 * b, a._m12 * b, a._m13 * b,
      a._m20 * b, a._m21 * b, a._m22 * b, a._m23 * b,
      a._m30 * b, a._m31 * b, a._m32 * b, a._m33 * b);
  }

  /// <summary>
  /// Multiplies each component in a matrix by a scalar.
  /// </summary>
  /// <param name="a">left operand</param>
  /// <param name="b">right operand</param>
  /// <returns>product</returns>
  public static Mat4 operator * (in float a, in Mat4 b)
  {
    return new Mat4 (
      a * b._m00, a * b._m01, a * b._m02, a * b._m03,
      a * b._m10, a * b._m11, a * b._m12, a * b._m13,
      a * b._m20, a * b._m21, a * b._m22, a * b._m23,
      a * b._m30, a * b._m31, a * b._m32, a * b._m33);
  }

  /// <summary>
  /// Multiplies a matrix and a vector.
  /// </summary>
  /// <param name="a">matrix</param>
  /// <param name="b">vector</param>
  /// <returns>product</returns>
  public static Vec4 operator * (in Mat4 a, in Vec4 b)
  {
    return new Vec4 (
      a._m00 * b.x + a._m01 * b.y + a._m02 * b.z + a._m03 * b.w,
      a._m10 * b.x + a._m11 * b.y + a._m12 * b.z + a._m13 * b.w,
      a._m20 * b.x + a._m21 * b.y + a._m22 * b.z + a._m23 * b.w,
      a._m30 * b.x + a._m31 * b.y + a._m32 * b.z + a._m33 * b.w);
  }

  /// <summary>
  /// Divides one matrix by another. Equivalent to multiplying the numerator and
  /// the inverse of the denominator.
  /// </summary>
  /// <param name="a">numerator</param>
  /// <param name="b">denominator</param>
  /// <returns>quotient</returns>
  public static Mat4 operator / (in Mat4 a, in Mat4 b)
  {
    return a * Mat4.Inverse (b);
  }

  /// <summary>
  /// Divides a matrix by a scalar.
  /// </summary>
  /// <param name="a">numerator</param>
  /// <param name="b">denominator</param>
  /// <returns>quotient</returns>
  public static Mat4 operator / (in Mat4 a, in float b)
  {
    return (b != 0.0f) ? a * (1.0f / b) : Mat4.Identity;
  }

  /// <summary>
  /// Divides a scalar by a matrix. Equivalent to multiplying the numerator and
  /// the inverse of the denominator.
  /// </summary>
  /// <param name="a">numerator</param>
  /// <param name="b">denominator</param>
  /// <returns>quotient</returns>
  public static Mat4 operator / (in float a, in Mat4 b)
  {
    return a * Mat4.Inverse (b);
  }

  /// <summary>
  /// Adds two matrices together.
  /// </summary>
  /// <param name="a">left operand</param>
  /// <param name="b">right operand</param>
  /// <returns>sum</returns>
  public static Mat4 operator + (in Mat4 a, in Mat4 b)
  {
    return new Mat4 (
      a._m00 + b._m00, a._m01 + b._m01, a._m02 + b._m02, a._m03 + b._m03,
      a._m10 + b._m10, a._m11 + b._m11, a._m12 + b._m12, a._m13 + b._m13,
      a._m20 + b._m20, a._m21 + b._m21, a._m22 + b._m22, a._m23 + b._m23,
      a._m30 + b._m30, a._m31 + b._m31, a._m32 + b._m32, a._m33 + b._m33);
  }

  /// <summary>
  /// Subtracts the right matrix from the left matrix.
  /// </summary>
  /// <param name="a">left operand</param>
  /// <param name="b">right operand</param>
  /// <returns>result</returns>
  public static Mat4 operator - (in Mat4 a, in Mat4 b)
  {
    return new Mat4 (
      a._m00 - b._m00, a._m01 - b._m01, a._m02 - b._m02, a._m03 - b._m03,
      a._m10 - b._m10, a._m11 - b._m11, a._m12 - b._m12, a._m13 - b._m13,
      a._m20 - b._m20, a._m21 - b._m21, a._m22 - b._m22, a._m23 - b._m23,
      a._m30 - b._m30, a._m31 - b._m31, a._m32 - b._m32, a._m33 - b._m33);
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
  public static Mat4 operator < (in Mat4 a, in Mat4 b)
  {
    return new Mat4 (
      a._m00 < b._m00, a._m01 < b._m01, a._m02 < b._m02, a._m03 < b._m03,
      a._m10 < b._m10, a._m11 < b._m11, a._m12 < b._m12, a._m13 < b._m13,
      a._m20 < b._m20, a._m21 < b._m21, a._m22 < b._m22, a._m23 < b._m23,
      a._m30 < b._m30, a._m31 < b._m31, a._m32 < b._m32, a._m33 < b._m33);
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
  public static Mat4 operator > (in Mat4 a, in Mat4 b)
  {
    return new Mat4 (
      a._m00 > b._m00, a._m01 > b._m01, a._m02 > b._m02, a._m03 > b._m03,
      a._m10 > b._m10, a._m11 > b._m11, a._m12 > b._m12, a._m13 > b._m13,
      a._m20 > b._m20, a._m21 > b._m21, a._m22 > b._m22, a._m23 > b._m23,
      a._m30 > b._m30, a._m31 > b._m31, a._m32 > b._m32, a._m33 > b._m33);
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
  public static Mat4 operator <= (in Mat4 a, in Mat4 b)
  {
    return new Mat4 (
      a._m00 <= b._m00, a._m01 <= b._m01, a._m02 <= b._m02, a._m03 <= b._m03,
      a._m10 <= b._m10, a._m11 <= b._m11, a._m12 <= b._m12, a._m13 <= b._m13,
      a._m20 <= b._m20, a._m21 <= b._m21, a._m22 <= b._m22, a._m23 <= b._m23,
      a._m30 <= b._m30, a._m31 <= b._m31, a._m32 <= b._m32, a._m33 <= b._m33);
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
  public static Mat4 operator >= (in Mat4 a, in Mat4 b)
  {
    return new Mat4 (
      a._m00 >= b._m00, a._m01 >= b._m01, a._m02 >= b._m02, a._m03 >= b._m03,
      a._m10 >= b._m10, a._m11 >= b._m11, a._m12 >= b._m12, a._m13 >= b._m13,
      a._m20 >= b._m20, a._m21 >= b._m21, a._m22 >= b._m22, a._m23 >= b._m23,
      a._m30 >= b._m30, a._m31 >= b._m31, a._m32 >= b._m32, a._m33 >= b._m33);
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
  public static Mat4 operator != (in Mat4 a, in Mat4 b)
  {
    return new Mat4 (
      a._m00 != b._m00, a._m01 != b._m01, a._m02 != b._m02, a._m03 != b._m03,
      a._m10 != b._m10, a._m11 != b._m11, a._m12 != b._m12, a._m13 != b._m13,
      a._m20 != b._m20, a._m21 != b._m21, a._m22 != b._m22, a._m23 != b._m23,
      a._m30 != b._m30, a._m31 != b._m31, a._m32 != b._m32, a._m33 != b._m33);
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
  public static Mat4 operator == (in Mat4 a, in Mat4 b)
  {
    return new Mat4 (
      a._m00 == b._m00, a._m01 == b._m01, a._m02 == b._m02, a._m03 == b._m03,
      a._m10 == b._m10, a._m11 == b._m11, a._m12 == b._m12, a._m13 == b._m13,
      a._m20 == b._m20, a._m21 == b._m21, a._m22 == b._m22, a._m23 == b._m23,
      a._m30 == b._m30, a._m31 == b._m31, a._m32 == b._m32, a._m33 == b._m33);
  }

  /// <summary>
  /// Evaluates whether all elements of a matrix are not equal to zero. 
  /// </summary>
  /// <param name="m">matrix</param>
  /// <returns>the evaluation</returns>
  public static bool All (in Mat4 m)
  {
    return (m._m00 != 0.0f) && (m._m01 != 0.0f) && (m._m02 != 0.0f) && (m._m03 != 0.0f) &&
      (m._m10 != 0.0f) && (m._m11 != 0.0f) && (m._m12 != 0.0f) && (m._m13 != 0.0f) &&
      (m._m20 != 0.0f) && (m._m21 != 0.0f) && (m._m22 != 0.0f) && (m._m23 != 0.0f) &&
      (m._m30 != 0.0f) && (m._m31 != 0.0f) && (m._m32 != 0.0f) && (m._m33 != 0.0f);
  }

  /// <summary>
  /// Evaluates whether any elements of a matrix are not equal to zero. 
  /// </summary>
  /// <param name="m">matrix</param>
  /// <returns>the evaluation</returns>
  public static bool Any (in Mat4 m)
  {
    return (m._m00 != 0.0f) || (m._m01 != 0.0f) || (m._m02 != 0.0f) || (m._m03 != 0.0f) ||
      (m._m10 != 0.0f) || (m._m11 != 0.0f) || (m._m12 != 0.0f) || (m._m13 != 0.0f) ||
      (m._m20 != 0.0f) || (m._m21 != 0.0f) || (m._m22 != 0.0f) || (m._m23 != 0.0f) ||
      (m._m30 != 0.0f) || (m._m31 != 0.0f) || (m._m32 != 0.0f) || (m._m33 != 0.0f);
  }

  /// <summary>
  /// Finds the determinant of the matrix.
  /// </summary>
  /// <param name="m">matrix</param>
  /// <returns>determinant</returns>
  public static float Determinant (in Mat4 m)
  {
    return m._m00 * (m._m11 * m._m22 * m._m33 +
        m._m12 * m._m23 * m._m31 +
        m._m13 * m._m21 * m._m32 -
        m._m13 * m._m22 * m._m31 -
        m._m11 * m._m23 * m._m32 -
        m._m12 * m._m21 * m._m33) -
      m._m01 * (m._m10 * m._m22 * m._m33 +
        m._m12 * m._m23 * m._m30 +
        m._m13 * m._m20 * m._m32 -
        m._m13 * m._m22 * m._m30 -
        m._m10 * m._m23 * m._m32 -
        m._m12 * m._m20 * m._m33) +
      m._m02 * (m._m10 * m._m21 * m._m33 +
        m._m11 * m._m23 * m._m30 +
        m._m13 * m._m20 * m._m31 -
        m._m13 * m._m21 * m._m30 -
        m._m10 * m._m23 * m._m31 -
        m._m11 * m._m20 * m._m33) -
      m._m03 * (m._m10 * m._m21 * m._m32 +
        m._m11 * m._m22 * m._m30 +
        m._m12 * m._m20 * m._m31 -
        m._m12 * m._m21 * m._m30 -
        m._m10 * m._m22 * m._m31 -
        m._m11 * m._m20 * m._m32);
  }

  /// <summary>
  /// Creates a matrix from two axes. The third axis, up, is assumed to be (0.0,
  /// 0.0, 1.0, 0.0) . The fourth row and column are assumed to be (0.0, 0.0,
  /// 0.0, 1.0) .
  /// </summary>
  /// <param name="right">right axis</param>
  /// <param name="forward">forward axis</param>
  /// <returns>matrix</returns>
  public static Mat4 FromAxes (in Vec2 right, in Vec2 forward)
  {
    return new Mat4 (
      right.x, forward.x, 0.0f, 0.0f,
      right.y, forward.y, 0.0f, 0.0f,
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
  public static Mat4 FromAxes (in Vec2 right, in Vec2 forward, in Vec2 translation)
  {
    return new Mat4 (
      right.x, forward.x, 0.0f, translation.x,
      right.y, forward.y, 0.0f, translation.y,
      0.0f, 0.0f, 1.0f, 0.0f,
      0.0f, 0.0f, 0.0f, 1.0f);
  }

  /// <summary>
  /// Creates a matrix from three axes. The fourth row and column are assumed to
  /// be (0.0, 0.0, 0.0, 1.0) .
  /// </summary>
  /// <param name="right">right axis</param>
  /// <param name="forward">forward axis</param>
  /// <param name="up">up axis</param>
  /// <returns>matrix</returns>
  public static Mat4 FromAxes (in Vec3 right, in Vec3 forward, in Vec3 up)
  {
    return new Mat4 (
      right.x, forward.x, up.x, 0.0f,
      right.y, forward.y, up.y, 0.0f,
      right.z, forward.z, up.z, 0.0f,
      0.0f, 0.0f, 0.0f, 1.0f);
  }

  /// <summary>
  /// Creates a matrix from three axes and a translation. The fourth row, w, is
  /// assumed to be (0.0, 0.0, 0.0, 1.0) .
  /// </summary>
  /// <param name="right">right axis</param>
  /// <param name="forward">forward axis</param>
  /// <param name="up">up axis</param>
  /// <param name="translation">translation axis</param>
  /// <returns>matrix</returns>
  public static Mat4 FromAxes (in Vec3 right, in Vec3 forward, in Vec3 up, in Vec3 translation)
  {
    return new Mat4 (
      right.x, forward.x, up.x, translation.x,
      right.y, forward.y, up.y, translation.y,
      right.z, forward.z, up.z, translation.z,
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
  public static Mat4 FromAxes (in Vec4 right, in Vec4 forward, in Vec4 up)
  {
    return new Mat4 (
      right.x, forward.x, up.x, 0.0f,
      right.y, forward.y, up.y, 0.0f,
      right.z, forward.z, up.z, 0.0f,
      right.w, forward.w, up.w, 1.0f);
  }

  /// <summary>
  /// Creates a matrix from three axes and a translation.
  /// </summary>
  /// <param name="right">right axis</param>
  /// <param name="forward">forward axis</param>
  /// <param name="up">up axis</param>
  /// <param name="translation">translation</param>
  /// <returns>matrix</returns>
  public static Mat4 FromAxes (in Vec4 right, in Vec4 forward, in Vec4 up, in Vec4 translation)
  {
    return new Mat4 (
      right.x, forward.x, up.x, translation.x,
      right.y, forward.y, up.y, translation.y,
      right.z, forward.z, up.z, translation.z,
      right.w, forward.w, up.w, translation.w);
  }

  /// <summary>
  /// Creates a rotation matrix from a quaternion.
  /// </summary>
  /// <param name="source">quaternion</param>
  /// <returns>matrix</returns>
  public static Mat4 FromRotation (in Quat source)
  {
    float w = source.Real;
    Vec3 i = source.Imag;
    float x = i.x;
    float y = i.y;
    float z = i.z;

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

    return new Mat4 (
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
  public static Mat4 FromRotation (in float radians, in Vec3 axis)
  {
    float sina = 0.0f;
    float cosa = 0.0f;
    Utils.SinCos (radians, out sina, out cosa);
    return Mat4.FromRotation (cosa, sina, axis);
  }

  /// <summary>
  /// Creates a rotation matrix from the cosine and sine of an angle around an
  /// axis. The axis will be normalized by the function.
  /// </summary>
  /// <param name="cosa">cosine of an angle</param>
  /// <param name="sina">sine of an angle</param>
  /// <param name="axis">axis</param>
  /// <returns>matrix</returns>
  public static Mat4 FromRotation (in float cosa, in float sina, in Vec3 axis)
  {
    float mSq = Vec3.MagSq (axis);
    if (mSq != 0.0f)
    {
      float mInv = Utils.InvSqrtUnchecked (mSq);
      float ax = axis.x * mInv;
      float ay = axis.y * mInv;
      float az = axis.z * mInv;

      float d = 1.0f - cosa;
      float x = ax * d;
      float y = ay * d;
      float z = az * d;

      float axay = x * ay;
      float axaz = x * az;
      float ayaz = y * az;

      return new Mat4 (
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
  public static Mat4 FromRotX (in float radians)
  {
    float sina = 0.0f;
    float cosa = 0.0f;
    Utils.SinCos (radians, out sina, out cosa);
    return Mat4.FromRotX (cosa, sina);
  }

  /// <summary>
  /// Creates a rotation matrix from a cosine and sine around the x axis.
  /// </summary>
  /// <param name="cosa">cosine of an angle</param>
  /// <param name="sina">sine of an angle</param>
  /// <returns>matrix</returns>
  public static Mat4 FromRotX (in float cosa, in float sina)
  {
    return new Mat4 (
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
  public static Mat4 FromRotY (in float radians)
  {
    float sina = 0.0f;
    float cosa = 0.0f;
    Utils.SinCos (radians, out sina, out cosa);
    return Mat4.FromRotY (cosa, sina);
  }

  /// <summary>
  /// Creates a rotation matrix from a cosine and sine around the y axis.
  /// </summary>
  /// <param name="cosa">cosine of an angle</param>
  /// <param name="sina">sine of an angle</param>
  /// <returns>matrix</returns>
  public static Mat4 FromRotY (in float cosa, in float sina)
  {
    return new Mat4 (
      cosa, 0.0f, sina, 0.0f,
      0.0f, 1.0f, 0.0f, 0.0f, -sina, 0.0f, cosa, 0.0f,
      0.0f, 0.0f, 0.0f, 1.0f);
  }

  /// <summary>
  /// Creates a rotation matrix from an angle in radians around the z axis.
  /// </summary>
  /// <param name="radians">angle</param>
  /// <returns>matrix</returns>
  public static Mat4 FromRotZ (in float radians)
  {
    float sina = 0.0f;
    float cosa = 0.0f;
    Utils.SinCos (radians, out sina, out cosa);
    return Mat4.FromRotZ (cosa, sina);
  }

  /// <summary>
  /// Creates a rotation matrix from a cosine and sine around the z axis.
  /// </summary>
  /// <param name="cosa">cosine of an angle</param>
  /// <param name="sina">sine of an angle</param>
  /// <returns>matrix</returns>
  public static Mat4 FromRotZ (in float cosa, in float sina)
  {
    return new Mat4 (
      cosa, -sina, 0.0f, 0.0f,
      sina, cosa, 0.0f, 0.0f,
      0.0f, 0.0f, 1.0f, 0.0f,
      0.0f, 0.0f, 0.0f, 1.0f);
  }

  /// <summary>
  /// Creates a scale matrix from a scalar. The bottom right corner, m33, is set
  /// to 1.0 .
  /// </summary>
  /// <param name="scalar">scalar</param>
  /// <returns>matrix</returns>
  public static Mat4 FromScale (in float scalar)
  {
    if (scalar != 0.0f)
    {
      return new Mat4 (
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
  public static Mat4 FromScale (in Vec2 scalar)
  {
    if (Vec2.All (scalar))
    {
      return new Mat4 (
        scalar.x, 0.0f, 0.0f, 0.0f,
        0.0f, scalar.y, 0.0f, 0.0f,
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
  public static Mat4 FromScale (in Vec3 scalar)
  {
    if (Vec3.All (scalar))
    {
      return new Mat4 (
        scalar.x, 0.0f, 0.0f, 0.0f,
        0.0f, scalar.y, 0.0f, 0.0f,
        0.0f, 0.0f, scalar.z, 0.0f,
        0.0f, 0.0f, 0.0f, 1.0f);
    }
    return Mat4.Identity;
  }

  /// <summary>
  /// Creates a translation matrix from a vector.
  /// </summary>
  /// <param name="translation">translation</param>
  /// <returns>matrix</returns>
  public static Mat4 FromTranslation (in Vec2 translation)
  {
    return new Mat4 (
      1.0f, 0.0f, 0.0f, translation.x,
      0.0f, 1.0f, 0.0f, translation.y,
      0.0f, 0.0f, 1.0f, 0.0f,
      0.0f, 0.0f, 0.0f, 1.0f);
  }

  /// <summary>
  /// Creates a translation matrix from a vector.
  /// </summary>
  /// <param name="translation">translation</param>
  /// <returns>matrix</returns>
  public static Mat4 FromTranslation (in Vec3 translation)
  {
    return new Mat4 (
      1.0f, 0.0f, 0.0f, translation.x,
      0.0f, 1.0f, 0.0f, translation.y,
      0.0f, 0.0f, 1.0f, translation.z,
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
  public static Mat4 Frustum (in float left, in float right, in float bottom, in float top, in float near, in float far)
  {
    float n2 = near + near;

    float w = right - left;
    float h = top - bottom;
    float d = far - near;

    w = (w != 0.0f) ? 1.0f / w : 1.0f;
    h = (h != 0.0f) ? 1.0f / h : 1.0f;
    d = (d != 0.0f) ? 1.0f / d : 1.0f;

    return new Mat4 (
      n2 * w, 0.0f, (right + left) * w, 0.0f,
      0.0f, n2 * h, (top + bottom) * h, 0.0f,
      0.0f, 0.0f, (far + near) * -d, n2 * far * -d,
      0.0f, 0.0f, -1.0f, 0.0f);
  }

  /// <summary>
  /// Inverts the input matrix.
  /// </summary>
  /// <param name="m">matrix</param>
  /// <returns>inverse</returns>
  public static Mat4 Inverse (in Mat4 m)
  {
    float b00 = m._m00 * m._m11 - m._m01 * m._m10;
    float b01 = m._m00 * m._m12 - m._m02 * m._m10;
    float b02 = m._m00 * m._m13 - m._m03 * m._m10;
    float b03 = m._m01 * m._m12 - m._m02 * m._m11;
    float b04 = m._m01 * m._m13 - m._m03 * m._m11;
    float b05 = m._m02 * m._m13 - m._m03 * m._m12;
    float b06 = m._m20 * m._m31 - m._m21 * m._m30;
    float b07 = m._m20 * m._m32 - m._m22 * m._m30;
    float b08 = m._m20 * m._m33 - m._m23 * m._m30;
    float b09 = m._m21 * m._m32 - m._m22 * m._m31;
    float b10 = m._m21 * m._m33 - m._m23 * m._m31;
    float b11 = m._m22 * m._m33 - m._m23 * m._m32;

    float det = b00 * b11 - b01 * b10 +
      b02 * b09 + b03 * b08 -
      b04 * b07 + b05 * b06;
    if (det != 0.0f)
    {
      float detInv = 1.0f / det;
      return new Mat4 (
        (m._m11 * b11 - m._m12 * b10 + m._m13 * b09) * detInv,
        (m._m02 * b10 - m._m01 * b11 - m._m03 * b09) * detInv,
        (m._m31 * b05 - m._m32 * b04 + m._m33 * b03) * detInv,
        (m._m22 * b04 - m._m21 * b05 - m._m23 * b03) * detInv,
        (m._m12 * b08 - m._m10 * b11 - m._m13 * b07) * detInv,
        (m._m00 * b11 - m._m02 * b08 + m._m03 * b07) * detInv,
        (m._m32 * b02 - m._m30 * b05 - m._m33 * b01) * detInv,
        (m._m20 * b05 - m._m22 * b02 + m._m23 * b01) * detInv,
        (m._m10 * b10 - m._m11 * b08 + m._m13 * b06) * detInv,
        (m._m01 * b08 - m._m00 * b10 - m._m03 * b06) * detInv,
        (m._m30 * b04 - m._m31 * b02 + m._m33 * b00) * detInv,
        (m._m21 * b02 - m._m20 * b04 - m._m23 * b00) * detInv,
        (m._m11 * b07 - m._m10 * b09 - m._m12 * b06) * detInv,
        (m._m00 * b09 - m._m01 * b07 + m._m02 * b06) * detInv,
        (m._m31 * b01 - m._m30 * b03 - m._m32 * b00) * detInv,
        (m._m20 * b03 - m._m21 * b01 + m._m22 * b00) * detInv);
    }

    return Mat4.Identity;
  }

  /// <summary>
  /// Multiplies a matrix and a point. The z component of the point is assumed
  /// to be 0.0 . The w component of the point is assumed to be 1.0 , so the
  /// point is impacted by the matrix's translation.
  /// </summary>
  /// <param name="a">matrix</param>
  /// <param name="b">vector</param>
  /// <returns>product</returns>
  public static Vec3 MulPoint (in Mat4 a, in Vec2 b)
  {
    float w = a._m30 * b.x + a._m31 * b.y + a._m33;
    if (w != 0.0f)
    {
      float wInv = 1.0f / w;
      return new Vec3 (
        (a._m00 * b.x + a._m01 * b.y + a._m03) * wInv,
        (a._m10 * b.x + a._m11 * b.y + a._m13) * wInv,
        (a._m20 * b.x + a._m21 * b.y + a._m23) * wInv);
    }
    return new Vec3 (0.0f, 0.0f, 0.0f);
  }

  /// <summary>
  /// Multiplies a matrix and a point. The w component of the point is assumed
  /// to be 1.0 , so the point is impacted by the matrix's translation.
  /// </summary>
  /// <param name="a">matrix</param>
  /// <param name="b">vector</param>
  /// <returns>product</returns>
  public static Vec3 MulPoint (in Mat4 a, in Vec3 b)
  {
    float w = a._m30 * b.x + a._m31 * b.y + a._m32 * b.z + a._m33;
    if (w != 0.0f)
    {
      float wInv = 1.0f / w;
      return new Vec3 (
        (a._m00 * b.x + a._m01 * b.y + a._m02 * b.z + a._m03) * wInv,
        (a._m10 * b.x + a._m11 * b.y + a._m12 * b.z + a._m13) * wInv,
        (a._m20 * b.x + a._m21 * b.y + a._m22 * b.z + a._m23) * wInv);
    }
    return new Vec3 (0.0f, 0.0f, 0.0f);
  }

  /// <summary>
  /// Multiplies a matrix and a vector. The z and w components of the vector are
  /// assumed to be 0.0 , so the vector is not impacted by the matrix's
  /// translation.
  /// </summary>
  /// <param name="a">matrix</param>
  /// <param name="b">vector</param>
  /// <returns>product</returns>
  public static Vec3 MulVector (in Mat4 a, in Vec2 b)
  {
    float w = a._m30 * b.x + a._m31 * b.y + a._m33;
    if (w != 0.0f)
    {
      float wInv = 1.0f / w;
      return new Vec3 (
        (a._m00 * b.x + a._m01 * b.y) * wInv,
        (a._m10 * b.x + a._m11 * b.y) * wInv,
        (a._m20 * b.x + a._m21 * b.y) * wInv);
    }
    return new Vec3 (0.0f, 0.0f, 0.0f);
  }

  /// <summary>
  /// Multiplies a matrix and a vector. The w component of the vector is assumed
  /// to be 0.0 , so the vector is not impacted by the matrix's translation.
  /// </summary>
  /// <param name="a">matrix</param>
  /// <param name="b">vector</param>
  /// <returns>product</returns>
  public static Vec3 MulVector (in Mat4 a, in Vec3 b)
  {
    float w = a._m30 * b.x + a._m31 * b.y + a._m32 * b.z + a._m33;
    if (w != 0.0f)
    {
      float wInv = 1.0f / w;
      return new Vec3 (
        (a._m00 * b.x + a._m01 * b.y + a._m02 * b.z) * wInv,
        (a._m10 * b.x + a._m11 * b.y + a._m12 * b.z) * wInv,
        (a._m20 * b.x + a._m21 * b.y + a._m22 * b.z) * wInv);
    }
    return new Vec3 (0.0f, 0.0f, 0.0f);
  }

  /// <summary>
  /// Evaluates whether all elements of a matrix are equal to zero. 
  /// </summary>
  /// <param name="m">matrix</param>
  /// <returns>the evaluation</returns>
  public static bool None (in Mat4 m)
  {
    return (m._m00 == 0.0f) && (m._m01 == 0.0f) && (m._m02 == 0.0f) && (m._m03 == 0.0f) &&
      (m._m10 == 0.0f) && (m._m11 == 0.0f) && (m._m12 == 0.0f) && (m._m13 == 0.0f) &&
      (m._m20 == 0.0f) && (m._m21 == 0.0f) && (m._m22 == 0.0f) && (m._m23 == 0.0f) &&
      (m._m30 == 0.0f) && (m._m31 == 0.0f) && (m._m32 == 0.0f) && (m._m33 == 0.0f);
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
  public static Mat4 Orthographic (in float left, in float right, in float bottom, in float top, in float near, in float far)
  {
    float w = right - left;
    float h = top - bottom;
    float d = far - near;

    w = (w != 0.0f) ? 1.0f / w : 1.0f;
    h = (h != 0.0f) ? 1.0f / h : 1.0f;
    d = (d != 0.0f) ? 1.0f / d : 1.0f;

    return new Mat4 (
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
  public static Mat4 Perspective (in float fov, in float aspect, in float near, in float far)
  {
    float cotfov = Utils.Cot (fov * 0.5f);
    float d = Utils.Div (1.0f, far - near);
    return new Mat4 (
      Utils.Div (cotfov, aspect), 0.0f, 0.0f, 0.0f,
      0.0f, cotfov, 0.0f, 0.0f,
      0.0f, 0.0f, (far + near) * -d, (near + near) * far * -d,
      0.0f, 0.0f, -1.0f, 0.0f);
  }

  /// <summary>
  /// Rotates the elements of the input matrix 90 degrees counter-clockwise.
  /// </summary>
  /// <param name="m">input matrix</param>
  /// <returns>rotated matrix</returns>
  public static Mat4 RotateElmsCcw (in Mat4 m)
  {
    return new Mat4 (
      m._m03, m._m13, m._m23, m._m33,
      m._m02, m._m12, m._m22, m._m32,
      m._m01, m._m11, m._m21, m._m31,
      m._m00, m._m10, m._m20, m._m30);
  }

  /// <summary>
  /// Rotates the elements of the input matrix 90 degrees clockwise.
  /// </summary>
  /// <param name="m">input matrix</param>
  /// <returns>rotated matrix</returns>
  public static Mat4 RotateElmsCw (in Mat4 m)
  {
    return new Mat4 (
      m._m30, m._m20, m._m10, m._m00,
      m._m31, m._m21, m._m11, m._m01,
      m._m32, m._m22, m._m12, m._m02,
      m._m33, m._m23, m._m13, m._m03);
  }

  /// <summary>
  /// Transposes a matrix, switching its row and column elements.
  /// </summary>
  /// <param name="m">matrix</param>
  /// <returns>transposition</returns>
  public static Mat4 Transpose (in Mat4 m)
  {
    return new Mat4 (
      m._m00, m._m10, m._m20, m._m30,
      m._m01, m._m11, m._m21, m._m31,
      m._m02, m._m12, m._m22, m._m32,
      m._m03, m._m13, m._m23, m._m33);
  }

  /// <summary>
  /// Returns a matrix set to the Bezier curve basis, [ -1.0,  3.0, -3.0, 1.0,
  /// 3.0, -6.0,  3.0, 0.0, -3.0,  3.0,  0.0, 0.0, 1.0,  0.0,  0.0, 0.0 ] .
  /// </summary>
  /// <value>Bezier Basis</value>
  public static Mat4 BezierBasis
  {
    get
    {
      return new Mat4 (-1.0f, 3.0f, -3.0f, 1.0f,
        3.0f, -6.0f, 3.0f, 0.0f, -3.0f, 3.0f, 0.0f, 0.0f,
        1.0f, 0.0f, 0.0f, 0.0f);
    }
  }

  /// <summary>
  /// Returns a matrix set to the Bezier curve basis inverse, [ 0.0, 0.0, 0.0,
  /// 1.0, 0.0, 0.0, 0.33333334, 1.0, 0.0, 0.33333334, 0.66666666, 1.0, 1.0,
  /// 1.0, 1.0, 1.0 ] .
  /// </summary>
  /// <value>Bezier basis inverse</value>
  public static Mat4 BezierBasisInverse
  {
    get
    {
      return new Mat4 (
        0.0f, 0.0f, 0.0f, 1.0f,
        0.0f, 0.0f, Utils.OneThird, 1.0f,
        0.0f, Utils.OneThird, Utils.TwoThirds, 1.0f,
        1.0f, 1.0f, 1.0f, 1.0f);
    }
  }

  /// <summary>
  /// Returns the identity matrix, [ 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0,
  /// 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0 ] .
  /// </summary>
  /// <value>the identity matrix</value>
  public static Mat4 Identity
  {
    get
    {
      return new Mat4 (
        1.0f, 0.0f, 0.0f, 0.0f,
        0.0f, 1.0f, 0.0f, 0.0f,
        0.0f, 0.0f, 1.0f, 0.0f,
        0.0f, 0.0f, 0.0f, 1.0f);
    }
  }
}