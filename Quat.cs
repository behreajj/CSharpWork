using System;
using System.Collections;
using System.Text;

[Serializable]
public readonly struct Quat : IEquatable<Quat>, IEnumerable
{
  private readonly float real;
  private readonly Vec3 imag;

  public float Real { get { return this.real; } }
  public Vec3 Imag { get { return this.imag; } }

  public float w { get { return this.real; } }
  public float x { get { return this.imag.x; } }
  public float y { get { return this.imag.y; } }
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
    return new float[ ] { this.real, this.imag.x, this.imag.y, this.imag.z };
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

  public static implicit operator Quat (float v)
  {
    return new Quat (v, new Vec3 ( ));
  }

  public static implicit operator Quat (Vec3 v)
  {
    return new Quat (0.0f, v);
  }

  public static implicit operator Quat (Vec4 v)
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
    if (b == 0.0f) return Quat.Identity;
    float bInv = 1.0f / b;
    return new Quat (a.real * bInv, a.imag * bInv);
  }

  public static Quat operator / (in Quat a, in Vec3 b)
  {
    return a * (-b / Vec3.Dot (b, b));
  }

  public static Quat operator / (in Vec3 a, in Quat b)
  {
    return a * Inverse (b);
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

  public static bool None (in Quat q)
  {
    return q.real == 0.0f && Vec3.None (q.imag);
  }

  public static Quat Normalize (in Quat q)
  {
    return q / Quat.Mag (q);
  }

  public static Quat Random (Random rng)
  {
    float t0 = Utils.Tau * (float) rng.NextDouble ( );
    float t1 = Utils.Tau * (float) rng.NextDouble ( );

    float r1 = (float) rng.NextDouble ( );
    float x0 = Utils.Sqrt (1.0f - r1);
    float x1 = Utils.Sqrt (r1);

    return new Quat (
      x0 * Utils.Sin (t0),
      x0 * Utils.Cos (t0),
      x1 * Utils.Sin (t1),
      x1 * Utils.Cos (t1));
  }

  public static Quat Identity
  {
    get
    {
      return new Quat (1.0f, new Vec3 (0.0f, 0.0f, 0.0f));
    }
  }
}