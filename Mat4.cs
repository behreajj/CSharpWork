using System;
using System.Collections;
using System.Text;

/// <summary>
/// A readonly struct influenced by GLSL, OSL and Processing's PMatrix3D.
/// Although this is a 4 x 4 matrix, it is generally assumed to be a 3D affine
/// transform matrix, where the last row is (0.0, 0.0, 0.0, 1.0) .
/// </summary>
[Serializable]
public readonly struct Mat4 : IEnumerable
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

  public float m00 { get { return this._m00; } }
  public float m01 { get { return this._m01; } }
  public float m02 { get { return this._m02; } }
  public float m03 { get { return this._m03; } }
  public float m10 { get { return this._m10; } }
  public float m11 { get { return this._m11; } }
  public float m12 { get { return this._m12; } }
  public float m13 { get { return this._m13; } }
  public float m20 { get { return this._m20; } }
  public float m21 { get { return this._m21; } }
  public float m22 { get { return this._m22; } }
  public float m23 { get { return this._m23; } }
  public float m30 { get { return this._m30; } }
  public float m31 { get { return this._m31; } }
  public float m32 { get { return this._m32; } }
  public float m33 { get { return this._m33; } }

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

  public override string ToString ( )
  {
    return ToString (4);
  }

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

  public float[ ] ToArray ( )
  {
    return new float[ ]
    {
      this._m00, this._m01, this._m02, this._m03,
        this._m10, this._m11, this._m12, this._m13,
        this._m20, this._m21, this._m22, this._m23,
        this._m30, this._m31, this._m32, this._m33
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

  public static implicit operator Mat4 (in Quat q)
  {
    return Mat4.FromRotation(q);
  }

  public static Mat4 operator - (in Mat4 a, in Mat4 b)
  {
    return new Mat4 (
      a._m00 - b._m00, a._m01 - b._m01, a._m02 - b._m02, a._m03 - b._m03,
      a._m10 - b._m10, a._m11 - b._m11, a._m12 - b._m12, a._m13 - b._m13,
      a._m20 - b._m20, a._m21 - b._m21, a._m22 - b._m22, a._m23 - b._m23,
      a._m30 - b._m30, a._m31 - b._m31, a._m32 - b._m32, a._m33 - b._m33);
  }

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

  public static Mat4 operator * (in Mat4 a, in float b)
  {
    return new Mat4 (
      a._m00 * b, a._m01 * b, a._m02 * b, a._m03 * b,
      a._m10 * b, a._m11 * b, a._m12 * b, a._m13 * b,
      a._m20 * b, a._m21 * b, a._m22 * b, a._m23 * b,
      a._m30 * b, a._m31 * b, a._m32 * b, a._m33 * b);
  }

  public static Mat4 operator * (in float a, in Mat4 b)
  {
    return new Mat4 (
      a * b._m00, a * b._m01, a * b._m02, a * b._m03,
      a * b._m10, a * b._m11, a * b._m12, a * b._m13,
      a * b._m20, a * b._m21, a * b._m22, a * b._m23,
      a * b._m30, a * b._m31, a * b._m32, a * b._m33);
  }

  public static Vec4 operator * (in Mat4 a, in Vec4 b)
  {
    return new Vec4 (
      a._m00 * b.x + a._m01 * b.y + a._m02 * b.z + a._m03 * b.w,
      a._m10 * b.x + a._m11 * b.y + a._m12 * b.z + a._m13 * b.w,
      a._m20 * b.x + a._m21 * b.y + a._m22 * b.z + a._m23 * b.w,
      a._m30 * b.x + a._m31 * b.y + a._m32 * b.z + a._m33 * b.w);
  }

  public static Mat4 operator / (in Mat4 a, in Mat4 b)
  {
    return a * Mat4.Inverse (b);
  }

  public static Mat4 operator / (in Mat4 a, in float b)
  {
    return (b != 0.0f) ? a * (1.0f / b) : Mat4.Identity;
  }

  public static Mat4 operator / (in float a, in Mat4 b)
  {
    return a * Mat4.Inverse (b);
  }

  public static Mat4 operator + (in Mat4 a, in Mat4 b)
  {
    return new Mat4 (
      a._m00 + b._m00, a._m01 + b._m01, a._m02 + b._m02, a._m03 + b._m03,
      a._m10 + b._m10, a._m11 + b._m11, a._m12 + b._m12, a._m13 + b._m13,
      a._m20 + b._m20, a._m21 + b._m21, a._m22 + b._m22, a._m23 + b._m23,
      a._m30 + b._m30, a._m31 + b._m31, a._m32 + b._m32, a._m33 + b._m33);
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

  public static Mat4 FromAxes (in Vec2 right, in Vec2 forward)
  {
    return new Mat4 (
      right.x, forward.x, 0.0f, 0.0f,
      right.y, forward.y, 0.0f, 0.0f,
      0.0f, 0.0f, 1.0f, 0.0f,
      0.0f, 0.0f, 0.0f, 1.0f);
  }

  public static Mat4 FromAxes (in Vec2 right, in Vec2 forward, in Vec2 translation)
  {
    return new Mat4 (
      right.x, forward.x, 0.0f, translation.x,
      right.y, forward.y, 0.0f, translation.y,
      0.0f, 0.0f, 1.0f, 0.0f,
      0.0f, 0.0f, 0.0f, 1.0f);
  }

  public static Mat4 FromAxes (in Vec3 right, in Vec3 forward, in Vec3 up)
  {
    return new Mat4 (
      right.x, forward.x, up.x, 0.0f,
      right.y, forward.y, up.y, 0.0f,
      right.z, forward.z, up.z, 0.0f,
      0.0f, 0.0f, 0.0f, 1.0f);
  }

  public static Mat4 FromAxes (in Vec3 right, in Vec3 forward, in Vec3 up, in Vec3 translation)
  {
    return new Mat4 (
      right.x, forward.x, up.x, translation.x,
      right.y, forward.y, up.y, translation.y,
      right.z, forward.z, up.z, translation.z,
      0.0f, 0.0f, 0.0f, 1.0f);
  }

  public static Mat4 FromAxes (in Vec4 right, in Vec4 forward, in Vec4 up)
  {
    return new Mat4 (
      right.x, forward.x, up.x, 0.0f,
      right.y, forward.y, up.y, 0.0f,
      right.z, forward.z, up.z, 0.0f,
      right.w, forward.w, up.w, 1.0f);
  }

  public static Mat4 FromAxes (in Vec4 right, in Vec4 forward, in Vec4 up, in Vec4 translation)
  {
    return new Mat4 (
      right.x, forward.x, up.x, translation.x,
      right.y, forward.y, up.y, translation.y,
      right.z, forward.z, up.z, translation.z,
      right.w, forward.w, up.w, translation.w);
  }

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

  public static Mat4 FromRotX (in float radians)
  {
    float sina = 0.0f;
    float cosa = 0.0f;
    Utils.SinCos (radians, out sina, out cosa);
    return Mat4.FromRotX (cosa, sina);
  }

  public static Mat4 FromRotX (in float cosa, in float sina)
  {
    return new Mat4 (
      1.0f, 0.0f, 0.0f, 0.0f,
      0.0f, cosa, -sina, 0.0f,
      0.0f, sina, cosa, 0.0f,
      0.0f, 0.0f, 0.0f, 1.0f);
  }

  public static Mat4 FromRotY (in float radians)
  {
    float sina = 0.0f;
    float cosa = 0.0f;
    Utils.SinCos (radians, out sina, out cosa);
    return Mat4.FromRotY (cosa, sina);
  }

  public static Mat4 FromRotY (in float cosa, in float sina)
  {
    return new Mat4 (
      cosa, 0.0f, sina, 0.0f,
      0.0f, 1.0f, 0.0f, 0.0f, -sina, 0.0f, cosa, 0.0f,
      0.0f, 0.0f, 0.0f, 1.0f);
  }

  public static Mat4 FromRotZ (in float radians)
  {
    float sina = 0.0f;
    float cosa = 0.0f;
    Utils.SinCos (radians, out sina, out cosa);
    return Mat4.FromRotZ (cosa, sina);
  }

  public static Mat4 FromRotZ (in float cosa, in float sina)
  {
    return new Mat4 (
      cosa, -sina, 0.0f, 0.0f,
      sina, cosa, 0.0f, 0.0f,
      0.0f, 0.0f, 1.0f, 0.0f,
      0.0f, 0.0f, 0.0f, 1.0f);
  }

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

  public static Mat4 FromTranslation (in Vec2 translation)
  {
    return new Mat4 (
      1.0f, 0.0f, 0.0f, translation.x,
      0.0f, 1.0f, 0.0f, translation.y,
      0.0f, 0.0f, 1.0f, 0.0f,
      0.0f, 0.0f, 0.0f, 1.0f);
  }

  public static Mat4 FromTranslation (in Vec3 translation)
  {
    return new Mat4 (
      1.0f, 0.0f, 0.0f, translation.x,
      0.0f, 1.0f, 0.0f, translation.y,
      0.0f, 0.0f, 1.0f, translation.z,
      0.0f, 0.0f, 0.0f, 1.0f);
  }

  public static Mat4 Frustum (in float left, in float right, in float bottom, in float top, in float near, in float far)
  {
    float n2 = near + near;

    float w = right - left;
    float h = top - bottom;
    float d = far - near;

    w = w != 0.0f ? 1.0f / w : 1.0f;
    h = h != 0.0f ? 1.0f / h : 1.0f;
    d = d != 0.0f ? 1.0f / d : 1.0f;

    return new Mat4 (
      n2 * w, 0.0f, (right + left) * w, 0.0f,
      0.0f, n2 * h, (top + bottom) * h, 0.0f,
      0.0f, 0.0f, (far + near) * -d, n2 * far * -d,
      0.0f, 0.0f, -1.0f, 0.0f);
  }

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

    w = w != 0.0f ? 1.0f / w : 1.0f;
    h = h != 0.0f ? 1.0f / h : 1.0f;
    d = d != 0.0f ? 1.0f / d : 1.0f;

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