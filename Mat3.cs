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

  public float m00 { get { return this._m00; } }
  public float m01 { get { return this._m01; } }
  public float m02 { get { return this._m02; } }
  public float m10 { get { return this._m10; } }
  public float m11 { get { return this._m11; } }
  public float m12 { get { return this._m12; } }
  public float m20 { get { return this._m20; } }
  public float m21 { get { return this._m21; } }
  public float m22 { get { return this._m22; } }

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

  public override string ToString ( )
  {
    return ToString (4);
  }

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

  public float[ ] ToArray ( )
  {
    return new float[ ]
    {
      this._m00, this._m01, this._m02,
        this._m10, this._m11, this._m12,
        this._m20, this._m21, this._m22
    };
  }

  public string ToString (in int places = 4)
  {
    return new StringBuilder (512)
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
    return new Vec2 (0.0f, 0.0f);
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
    return new Vec2 (0.0f, 0.0f);
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