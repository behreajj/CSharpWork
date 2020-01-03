using System;
using System.Text;

[Serializable]
public struct Quat
{
  public float real;
  public Vec3 imag;

  public float Real
  {
    get
    {
      return this.real;
    }

    set
    {
      this.real = value;
    }
  }

  public Vec3 Imag
  {
    get
    {
      return this.imag;
    }

    set
    {
      this.imag = value;
    }
  }

  public float X
  {
    get
    {
      return this.imag.x;
    }

    set
    {
      this.imag.x = value;
    }
  }

  public float Y
  {
    get
    {
      return this.imag.y;
    }

    set
    {
      this.imag.y = value;
    }
  }

  public float Z
  {
    get
    {
      return this.imag.z;
    }

    set
    {
      this.imag.z = value;
    }
  }

  public float W
  {
    get
    {
      return this.real;
    }

    set
    {
      this.real = value;
    }
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

    set
    {
      switch (i)
      {
        case 0:
        case -4:
          this.real = value;
          break;
        case 1:
        case -3:
          this.imag.x = value;
          break;
        case 2:
        case -2:
          this.imag.y = value;
          break;
        case 3:
        case -1:
          this.imag.z = value;
          break;
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

  public override string ToString ( )
  {
    return new StringBuilder ( )
      .Append ("{ real: ")
      .Append (this.real)
      .Append (", imag: ")
      .Append (this.imag.ToString ( ))
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

  public static Quat operator + (Quat a, Quat b)
  {
    return new Quat (a.real + b.real, a.imag + b.imag);
  }

  public static Quat operator - (Quat a, Quat b)
  {
    return new Quat (a.real - b.real, a.imag - b.imag);
  }
}