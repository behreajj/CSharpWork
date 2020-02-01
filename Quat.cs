using System;
using System.Collections;
using System.Text;

/// <summary>
/// A four-dimensional complex number. The x, y and z components are coefficients
/// of the imaginary i, j, and k. Discovered by William R. Hamilton with the
/// formula i i = j j = k k = i j k = -1.0 . Quaternions with a magnitude of 1.0
/// are commonly used to rotate 3D objects from one orientation to another
/// without suffering gimbal lock.
/// </summary>
[Serializable]
public readonly struct Quat : IEquatable<Quat>, IEnumerable
{
  /// <summary>
  /// The real component.
  /// </summary>
  private readonly float real;

  /// <summary>
  /// The coefficients of the imaginary components i, j and k.
  /// </summary>
  private readonly Vec3 imag;

  /// <summary>
  /// Returns the number of elements in this quaternion.
  /// </summary>
  /// <value>the length</value>
  public int Length { get { return 4; } }

  /// <summary>
  /// The real component.
  /// </summary>
  /// <value>the real scalar</value>
  public float Real { get { return this.real; } }

  /// <summary>
  /// The coefficients of the imaginary components i, j and k.
  /// </summary>
  /// <value>the imaginary vector</value>
  public Vec3 Imag { get { return this.imag; } }

  /// <summary>
  /// The real component.
  /// </summary>
  /// <value>w</value>
  public float w { get { return this.real; } }

  /// <summary>
  /// The coefficient of the imaginary i.
  /// </summary>
  /// <value>x</value>
  public float x { get { return this.imag.x; } }

  /// <summary>
  /// The coefficient of the imaginary j.
  /// </summary>
  /// <value>y</value>
  public float y { get { return this.imag.y; } }

  /// <summary>
  /// The coefficient of the imaginary k.
  /// </summary>
  /// <value>z</value>
  public float z { get { return this.imag.z; } }

  public float this [int i]
  {
    get
    {
      switch (i)
      {
        case 0:
        case -4:
          return this.real;
        case 1:
        case -3:
          return this.imag.x;
        case 2:
        case -2:
          return this.imag.y;
        case 3:
        case -1:
          return this.imag.z;
        default:
          return 0.0f;
      }
    }
  }

  public Quat (float real = 1.0f, Vec3 imag = new Vec3 ( ))
  {
    this.real = real;
    this.imag = imag;
  }

  public Quat (float w = 1.0f, float x = 0.0f, float y = 0.0f, float z = 0.0f)
  {
    this.real = w;
    this.imag = new Vec3 (x, y, z);
  }

  public override bool Equals (object value)
  {
    if (Object.ReferenceEquals (this, value)) return true;
    if (Object.ReferenceEquals (null, value)) return false;

    if (value is Quat)
    {
      Quat q = (Quat) value;

      // return Quat.Approx (this, q);

      if (this.real.GetHashCode ( ) != q.real.GetHashCode ( ))
      {
        return false;
      }

      if (this.imag.GetHashCode ( ) != q.imag.GetHashCode ( ))
      {
        return false;
      }

      return true;
    }

    return false;
  }

  public override int GetHashCode ( )
  {
    unchecked
    {
      int hash = Utils.HashBase;
      hash = hash * Utils.HashMul ^ this.real.GetHashCode ( );
      hash = hash * Utils.HashMul ^ this.imag.GetHashCode ( );
      return hash;
    }
  }

  public override string ToString ( )
  {
    return ToString (4);
  }

  public bool Equals (Quat q)
  {
    // return Quat.Approx (this, q);

    if (this.real.GetHashCode ( ) != q.real.GetHashCode ( ))
    {
      return false;
    }

    if (this.imag.GetHashCode ( ) != q.imag.GetHashCode ( ))
    {
      return false;
    }

    return true;
  }

  public IEnumerator GetEnumerator ( )
  {
    yield return this.real;
    yield return this.imag.x;
    yield return this.imag.y;
    yield return this.imag.z;
  }

  public float[ ] ToArray ( )
  {
    return new float[ ]
    {
      this.real,
        this.imag.x,
        this.imag.y,
        this.imag.z
    };
  }

  public string ToString (int places = 4)
  {
    return new StringBuilder (128)
      .Append ("{ real: ")
      .Append (Utils.ToFixed (this.real, places))
      .Append (", imag: ")
      .Append (this.imag.ToString (places))
      .Append (" }")
      .ToString ( );
  }

  public (float w, float x, float y, float z) ToTuple ( )
  {
    return (w: this.real,
      x: this.imag.x,
      y: this.imag.y,
      z: this.imag.z);
  }

  public static implicit operator Quat (float v)
  {
    return new Quat (v, new Vec3 ( ));
  }

  public static implicit operator Quat (in Vec3 v)
  {
    return new Quat (0.0f, v.x, v.y, v.z);
  }

  public static implicit operator Quat (in Vec4 v)
  {
    return new Quat (v.w, v.x, v.y, v.z);
  }

  public static implicit operator Vec4 (in Quat q)
  {
    return new Vec4 (
      q.imag.x,
      q.imag.y,
      q.imag.z,
      q.real);
  }

  public static explicit operator bool (in Quat q)
  {
    return Quat.Any (q);
  }

  public static explicit operator float (in Quat q)
  {
    return Quat.Mag (q);
  }

  public static bool operator true (in Quat q)
  {
    return Quat.Any (q);
  }

  public static bool operator false (in Quat q)
  {
    return Quat.None (q);
  }

  public static Quat operator - (in Quat z)
  {
    return new Quat (-z.real, -z.imag);
  }

  public static Quat operator * (in Quat a, in Quat b)
  {
    return new Quat (
      (a.real * b.real) -
      Vec3.Dot (a.imag, b.imag),

      Vec3.Cross (a.imag, b.imag) +
      (a.real * b.imag) +
      (b.real * a.imag));
  }

  public static Quat operator * (in Quat a, float b)
  {
    return new Quat (a.real * b, a.imag * b);
  }

  public static Quat operator * (float a, in Quat b)
  {
    return new Quat (a * b.real, a * b.imag);
  }

  public static Quat operator * (in Quat a, in Vec3 b)
  {
    return new Quat (-Vec3.Dot (a.imag, b),
      Vec3.Cross (a.imag, b) + (a.real * b));
  }

  public static Quat operator * (in Vec3 a, in Quat b)
  {
    return new Quat (-Vec3.Dot (a, b.imag),
      Vec3.Cross (a, b.imag) + (b.real * a));
  }

  public static Quat operator / (in Quat a, in Quat b)
  {
    return a * Quat.Inverse (b);
  }

  public static Quat operator / (in Quat a, float b)
  {
    if (b == 0.0f) return new Quat (0.0f, 0.0f, 0.0f, 0.0f);
    float bInv = 1.0f / b;
    return new Quat (a.real * bInv, a.imag * bInv);
  }

  public static Quat operator / (in Quat a, in Vec3 b)
  {
    return a * (-b / Vec3.Dot (b, b));
  }

  public static Quat operator / (in Vec3 a, in Quat b)
  {
    return a * Quat.Inverse (b);
  }

  public static Quat operator / (float a, in Quat b)
  {
    return a * Quat.Inverse (b);
  }

  public static Quat operator + (in Quat a, in Quat b)
  {
    return new Quat (a.real + b.real, a.imag + b.imag);
  }

  public static Quat operator + (in Quat a, float b)
  {
    return new Quat (a.real + b, a.imag);
  }

  public static Quat operator + (float a, in Quat b)
  {
    return new Quat (a + b.real, b.imag);
  }

  public static Quat operator + (in Quat a, in Vec3 b)
  {
    return new Quat (a.real, a.imag + b);
  }

  public static Quat operator + (in Vec3 a, in Quat b)
  {
    return new Quat (b.real, a + b.imag);
  }

  public static Quat operator - (in Quat a, in Quat b)
  {
    return new Quat (a.real - b.real, a.imag - b.imag);
  }

  public static Quat operator - (in Quat a, float b)
  {
    return new Quat (a.real - b, a.imag);
  }

  public static Quat operator - (float a, in Quat b)
  {
    return new Quat (a - b.real, -b.imag);
  }

  public static Quat operator - (in Quat a, in Vec3 b)
  {
    return new Quat (a.real, a.imag - b);
  }

  public static Quat operator - (in Vec3 a, in Quat b)
  {
    return new Quat (-b.real, a - b.imag);
  }

  public static bool All (in Quat q)
  {
    return q.real != 0.0f && Vec3.All (q.imag);
  }

  public static bool Any (in Quat q)
  {
    return q.real != 0.0f || Vec3.Any (q.imag);
  }

  public static bool Approx (in Quat a, in Quat b)
  {
    return Utils.Approx (a.real, b.real) &&
      Vec3.Approx (a.imag, b.imag);
  }

  public static Quat Conj (in Quat q)
  {
    return new Quat (q.real, -q.imag);
  }

  public static float Dot (in Quat a, in Quat b)
  {
    return a.real * b.real + Vec3.Dot (a.real, b.real);
  }

  public static Quat FromAngle (float radians)
  {
    float sina = 0.0f;
    float cosa = 0.0f;
    Utils.SinCos (radians * 0.5f, out sina, out cosa);
    return new Quat (cosa, 0.0f, 0.0f, sina);
  }

  public static Quat FromAxisAngle (float radians, in Vec3 axis)
  {
    float amSq = Vec3.MagSq (axis);
    if (amSq == 0.0f) return Quat.Identity;

    float nx = axis.x;
    float ny = axis.y;
    float nz = axis.z;

    if (!Utils.Approx (amSq, 1.0f))
    {
      float amInv = (float) (1.0f / Utils.Sqrt (amSq));
      nx *= amInv;
      ny *= amInv;
      nz *= amInv;
    }

    float sinHalf = 0.0f;
    float cosHalf = 0.0f;
    Utils.SinCos (radians * 0.5f, out sinHalf, out cosHalf);
    return new Quat (cosHalf,
      nx * sinHalf,
      ny * sinHalf,
      nz * sinHalf);
  }

  public static Quat Inverse (in Quat q)
  {
    return Quat.Conj (q) / Quat.MagSq (q);
  }

  public static float Mag (in Quat q)
  {
    return Utils.Sqrt (Quat.MagSq (q));
  }

  public static float MagSq (in Quat q)
  {
    return q.real * q.real + Vec3.MagSq (q.imag);
  }

  public static Vec3 MulVector (in Quat q, in Vec3 source)
  {
    float w = q.real;
    Vec3 i = q.imag;
    float qx = i.x;
    float qy = i.y;
    float qz = i.z;

    float iw = -qx * source.x - qy * source.y - qz * source.z;
    float ix = w * source.x + qy * source.z - qz * source.y;
    float iy = w * source.y + qz * source.x - qx * source.z;
    float iz = w * source.z + qx * source.y - qy * source.x;

    return new Vec3 (
      ix * w + iz * qy - iw * qx - iy * qz,
      iy * w + ix * qz - iw * qy - iz * qx,
      iz * w + iy * qx - iw * qz - ix * qy);
  }

  public static bool None (in Quat q)
  {
    return q.real == 0.0f && Vec3.None (q.imag);
  }

  public static Quat Normalize (in Quat q)
  {
    return q / Quat.Mag (q);
  }

  public static Quat Random (System.Random rng)
  {
    float t0 = Utils.Tau * (float) rng.NextDouble ( );
    float t1 = Utils.Tau * (float) rng.NextDouble ( );

    float r1 = (float) rng.NextDouble ( );
    float x0 = Utils.Sqrt (1.0f - r1);
    float x1 = Utils.Sqrt (r1);

    float sint0 = 0.0f;
    float cost0 = 0.0f;
    Utils.SinCos (t0, out sint0, out cost0);

    float sint1 = 0.0f;
    float cost1 = 0.0f;
    Utils.SinCos (t1, out sint1, out cost1);

    return new Quat (
      x0 * sint0,
      x0 * cost0,
      x1 * sint1,
      x1 * cost1);
  }

  public static Quat Rotate (in Quat q, float radians, in Vec3 axis)
  {
    float mSq = Quat.MagSq (q);
    if (mSq == 0.0f)
    {
      return Quat.FromAxisAngle (radians, axis);
    }

    float wNorm = q.real * Utils.InvSqrtUnchecked (mSq);
    float halfAngle = Utils.Acos (wNorm);
    return Quat.FromAxisAngle (
      Utils.ModRadians (halfAngle + halfAngle + radians),
      axis);
  }

  public static Quat RotateX (in Quat q, float radians)
  {
    float sina = 0.0f;
    float cosa = 0.0f;
    Utils.SinCos (radians * 0.5f, out sina, out cosa);
    return Quat.RotateX (q, cosa, sina);
  }

  public static Quat RotateX (in Quat q, float cosah, float sinah)
  {
    Vec3 i = q.imag;
    return new Quat (
      cosah * q.real - sinah * i.x,
      cosah * i.x + sinah * q.real,
      cosah * i.y + sinah * i.z,
      cosah * i.z - sinah * i.y);
  }

  public static Quat RotateY (in Quat q, float radians)
  {
    float sina = 0.0f;
    float cosa = 0.0f;
    Utils.SinCos (radians * 0.5f, out sina, out cosa);
    return Quat.RotateY (q, cosa, sina);
  }

  public static Quat RotateY (in Quat q, float cosah, float sinah)
  {
    Vec3 i = q.imag;
    return new Quat (
      cosah * q.real - sinah * i.y,
      cosah * i.x - sinah * i.z,
      cosah * i.y + sinah * q.real,
      cosah * i.z + sinah * i.x);
  }

  public static Quat RotateZ (in Quat q, float radians)
  {
    float sina = 0.0f;
    float cosa = 0.0f;
    Utils.SinCos (radians * 0.5f, out sina, out cosa);
    return Quat.RotateZ (q, cosa, sina);
  }

  public static Quat RotateZ (in Quat q, float cosah, float sinah)
  {
    Vec3 i = q.imag;
    return new Quat (
      cosah * q.real - sinah * i.z,
      cosah * i.x + sinah * i.y,
      cosah * i.y - sinah * i.x,
      cosah * i.z + sinah * q.real);
  }

  public static (Vec3 right, Vec3 forward, Vec3 up) ToAxes (in Quat q)
  {
    float w = q.real;
    Vec3 i = q.imag;
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

    return (
      right: new Vec3 (
        1.0f - ysq2 - zsq2,
        xy2 + wz2,
        xz2 - wy2),
      forward : new Vec3 (
        xy2 - wz2,
        1.0f - xsq2 - zsq2,
        yz2 + wx2),
      up : new Vec3 (
        xz2 + wy2,
        yz2 - wx2,
        1.0f - xsq2 - ysq2));
  }

  public static (float angle, Vec3 axis) ToAxisAngle (in Quat q)
  {
    float mSq = Quat.MagSq (q);
    if (mSq == 0.0f)
    {
      return (
        angle: 0.0f,
        axis: Vec3.Forward);
    }

    float wNorm = Utils.Approx (mSq, 1.0f) ? q.real :
      q.real * Utils.InvSqrtUnchecked (mSq);
    float angle = 2.0f * Utils.Acos (wNorm);
    float wAsin = Utils.Tau - angle;

    if (wAsin == 0.0f)
    {
      return (
        angle: angle,
        axis: Vec3.Forward);
    }

    float sInv = 1.0f / wAsin;
    Vec3 i = q.imag;
    float ax = i.x * sInv;
    float ay = i.y * sInv;
    float az = i.z * sInv;

    float amSq = ax * ax + ay * ay + az * az;
    if (amSq == 0.0f)
    {
      return (
        angle: angle,
        axis: Vec3.Forward);
    }

    if (Utils.Approx (amSq, 1.0f))
    {
      return (
        angle: angle,
        axis: new Vec3 (ax, ay, az));
    }

    float mInv = Utils.InvSqrtUnchecked (amSq);
    return (
      angle: 0.0f,
      axis: new Vec3 (ax * mInv, ay * mInv, az * mInv));
  }

  public static Quat Identity
  {
    get
    {
      return new Quat (1.0f, new Vec3 (0.0f, 0.0f, 0.0f));
    }
  }
}