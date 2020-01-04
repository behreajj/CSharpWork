using System;
using System.Text;

[Serializable]
public readonly struct Quat : IEquatable<Quat>
{
  public readonly float real;
  public readonly Vec3 imag;

  public float Real
  {
    get
    {
      return this.real;
    }

    // set
    // {
    //   this.real = value;
    // }
  }

  public Vec3 Imag
  {
    get
    {
      return this.imag;
    }

    // set
    // {
    //   this.imag = value;
    // }
  }

  public float W
  {
    get
    {
      return this.real;
    }

    // set
    // {
    //   this.real = value;
    // }
  }

  public float X
  {
    get
    {
      return this.imag.x;
    }

    // set
    // {
    //   this.imag.x = value;
    // }
  }

  public float Y
  {
    get
    {
      return this.imag.y;
    }

    // set
    // {
    //   this.imag.y = value;
    // }
  }

  public float Z
  {
    get
    {
      return this.imag.z;
    }

    // set
    // {
    //   this.imag.z = value;
    // }
  }

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

    // set
    // {
    //   switch (i)
    //   {
    //     case 0:
    //     case -4:
    //       this.real = value;
    //       break;
    //     case 1:
    //     case -3:
    //       this.imag.x = value;
    //       break;
    //     case 2:
    //     case -2:
    //       this.imag.y = value;
    //       break;
    //     case 3:
    //     case -1:
    //       this.imag.z = value;
    //       break;
    //   }
    // }
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

  public override int GetHashCode ( )
  {
    unchecked
    {
      const int hashBase = -2128831035;
      const int hashMul = 16777619;
      int hash = hashBase;
      hash = hash * hashMul ^ this.real.GetHashCode ( );
      hash = hash * hashMul ^ this.imag.GetHashCode ( );
      return hash;
    }
  }

  public override string ToString ( )
  {
    return ToString (4);
  }

  // public Quat Reset ( )
  // {
  //   return Set (1.0f, 0.0f, 0.0f, 0.0f);
  // }

  // public Quat Set (float real = 1.0f, Vec3 imag = new Vec3 ( ))
  // {
  //   this.real = real;
  //   this.imag = imag;
  //   return this;
  // }

  // public Quat Set (float w = 1.0f, float x = 0.0f, float y = 0.0f, float z = 0.0f)
  // {
  //   this.real = w;
  //   // this.imag = new Vec3 (x, y, z);
  //   this.Imag.Set (x, y, z);
  //   return this;
  // }

  public float[ ] ToArray ( )
  {
    return new float[ ] { this.real, this.imag.x, this.imag.y, this.imag.z };
  }

  public string ToString (int places = 4)
  {
    return new StringBuilder ( )
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

  public static implicit operator Vec4 (Quat q)
  {
    return new Vec4 (
      q.imag.x,
      q.imag.y,
      q.imag.z,
      q.real);
  }

  public static explicit operator bool (Quat q)
  {
    return Quat.Any (q);
  }

  public static explicit operator float (Quat q)
  {
    return Quat.Mag (q);
  }

  public static bool operator true (Quat q)
  {
    return Quat.Any (q);
  }

  public static bool operator false (Quat q)
  {
    return Quat.None (q);
  }

  public static Quat operator * (Quat a, Quat b)
  {
    return new Quat (
      (a.real * b.real) -
      Vec3.Dot (a.imag, b.imag),

      Vec3.Cross (a.imag, b.imag) +
      (a.real * b.imag) +
      (b.real * a.imag));
  }

  public static Quat operator * (Quat a, float b)
  {
    return new Quat (a.real * b, a.imag * b);
  }

  public static Quat operator * (float a, Quat b)
  {
    return new Quat (a * b.real, a * b.imag);
  }

  public static Quat operator * (Quat a, Vec3 b)
  {
    return new Quat (-Vec3.Dot (a.imag, b),
      Vec3.Cross (a.imag, b) + (a.real * b));
  }

  public static Quat operator * (Vec3 a, Quat b)
  {
    return new Quat (-Vec3.Dot (a, b.imag),
      Vec3.Cross (a, b.imag) + (b.real * a));
  }

  public static Quat operator / (Quat a, Quat b)
  {
    return a * Quat.Inverse (b);
  }

  public static Quat operator / (Quat a, float b)
  {
    if (b == 0.0f) return Quat.Identity;
    float bInv = 1.0f / b;
    return new Quat (a.real * bInv, a.imag * bInv);
  }

  public static Quat operator / (Quat a, Vec3 b)
  {
    return a * (-b / Vec3.Dot (b, b));
  }

  public static Quat operator / (Vec3 a, Quat b)
  {
    return a * Inverse (b);
  }

  public static Quat operator / (float a, Quat b)
  {
    return a * Quat.Inverse (b);
  }

  public static Quat operator + (Quat a, Quat b)
  {
    return new Quat (a.real + b.real, a.imag + b.imag);
  }

  public static Quat operator + (Quat a, float b)
  {
    return new Quat (a.real + b, a.imag);
  }

  public static Quat operator + (float a, Quat b)
  {
    return new Quat (a + b.real, b.imag);
  }

  public static Quat operator + (Quat a, Vec3 b)
  {
    return new Quat (a.real, a.imag + b);
  }

  public static Quat operator + (Vec3 a, Quat b)
  {
    return new Quat (b.real, a + b.imag);
  }

  public static Quat operator - (Quat a, Quat b)
  {
    return new Quat (a.real - b.real, a.imag - b.imag);
  }

  public static Quat operator - (Quat a, float b)
  {
    return new Quat (a.real - b, a.imag);
  }

  public static Quat operator - (float a, Quat b)
  {
    return new Quat (a - b.real, -b.imag);
  }

  public static Quat operator - (Quat a, Vec3 b)
  {
    return new Quat (a.real, a.imag - b);
  }

  public static Quat operator - (Vec3 a, Quat b)
  {
    return new Quat (-b.real, a - b.imag);
  }

  public static bool All (Quat q)
  {
    return q.real != 0.0f && Vec3.All (q.imag);
  }

  public static bool Any (Quat q)
  {
    return q.real != 0.0f || Vec3.Any (q.imag);
  }

  public static bool Approx (Quat a, Quat b)
  {
    return Utils.Approx (a.real, b.real) &&
      Vec3.Approx (a.imag, b.imag);
  }

  public static Quat Conj (Quat q)
  {
    return new Quat (q.real, -q.imag);
  }

  public static float Dot (Quat a, Quat b)
  {
    return a.real * b.real + Vec3.Dot (a.real, b.real);
  }

  public static Quat Inverse (Quat q)
  {
    return Conj (q) / Quat.MagSq (q);
  }

  public static float Mag (Quat q)
  {
    return Utils.Sqrt (Quat.MagSq (q));
  }

  public static float MagSq (Quat q)
  {
    return q.real * q.real + Vec3.MagSq (q.imag);
  }

  public static Quat Normalize (Quat q)
  {
    return q / Quat.Mag (q);
  }

  public static bool None (Quat q)
  {
    return q.real == 0.0f && Vec3.None (q.imag);
  }

  public static Quat Identity
  {
    get
    {
      return new Quat (1.0f, new Vec3 (0.0f, 0.0f, 0.0f));
    }
  }
}