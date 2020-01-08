using System;
using System.Text;

[Serializable]
public class Transform3
{
  protected Vec3 location = new Vec3 (0.0f, 0.0f, 0.0f);
  protected Quat rotation = new Quat (1.0f, 0.0f, 0.0f, 0.0f);
  protected Vec3 scale = new Vec3 (1.0f, 1.0f, 1.0f);

  public Vec3 Location
  {
    get
    {
      return this.location;
    }

    set
    {
      this.location = value;
    }
  }

  public Quat Rotation
  {
    get
    {
      return this.rotation;
    }

    set
    {
      if (Quat.Any (value))
      {
        this.rotation = value;
      }
    }
  }

  public Vec3 Scale
  {
    get
    {
      return this.scale;
    }

    set
    {
      this.scale = Vec3.Max (value, Utils.Epsilon);
    }
  }

  public Transform3 ( ) { }

  public Transform3 (
    Vec3 location,
    Quat rotation,
    Vec3 scale)
  {
    this.location = location;
    this.Rotation = rotation;
    this.Scale = scale;
  }

  public Transform3 (
    float x = 0.0f, float y = 0.0f, float z = 0.0f,
    float real = 1.0f,
    float xImag = 0.0f, float yImag = 0.0f, float zImag = 0.0f,
    float width = 1.0f, float height = 1.0f, float depth = 1.0f)
  {
    this.location = new Vec3 (x, y, z);
    this.Rotation = new Quat (real, xImag, yImag, zImag);
    this.Scale = new Vec3 (width, height, depth);
  }

  public override int GetHashCode ( )
  {
    unchecked
    {
      int hash = Utils.HashBase;
      hash = hash * Utils.HashMul ^ this.location.GetHashCode ( );
      hash = hash * Utils.HashMul ^ this.rotation.GetHashCode ( );
      hash = hash * Utils.HashMul ^ this.scale.GetHashCode ( );
      return hash;
    }
  }

  public override string ToString ( )
  {
    return ToString (4);
  }

  public Transform3 MoveBy (in Vec3 v)
  {
    this.location += v;
    return this;
  }

  public Transform3 MoveTo (in Vec3 v, float step = 1.0f)
  {
    if (step <= 0.0f) return this;
    if (step >= 1.0f) { this.location = v; return this; }

    this.location = Vec3.Mix (this.location, v, step);
    return this;
  }

  public Transform3 MoveTo (in Vec3 v, in Vec3 step, Func<Vec3, Vec3, Vec3, Vec3> Easing)
  {
    Vec3 t = Easing (this.location, v, step);
    this.location = Vec3.Mix (this.location, v, t);
    return this;
  }

  public Transform3 Reset ( )
  {
    this.location = new Vec3 (0.0f, 0.0f, 0.0f);
    this.rotation = new Quat (1.0f, 0.0f, 0.0f, 0.0f);
    this.scale = new Vec3 (1.0f, 1.0f, 1.0f);

    return this;
  }

  public Transform3 ScaleBy (in Vec3 v)
  {
    this.Scale = this.scale + v;
    return this;
  }

  public Transform3 ScaleTo (in Vec3 v, float step = 1.0f)
  {
    if (step <= 0.0f) return this;
    if (step >= 1.0f) { this.Scale = v; return this; }

    this.Scale = Vec3.Mix (this.scale, v, step);
    return this;
  }

  public Transform3 ScaleTo (in Vec3 v, in Vec3 step, Func<Vec3, Vec3, Vec3, Vec3> Easing)
  {
    Vec3 t = Easing (this.scale, v, step);
    this.Scale = Vec3.Mix (this.scale, v, t);
    return this;
  }

  public Transform3 Set (in Transform2 t)
  {
    float halfAng = 0.5f * t.Rotation;
    Vec2 scl2 = t.Scale;
    this.location = t.Location;
    this.rotation = new Quat (
      Utils.Cos (halfAng), 0.0f,
      0.0f, Utils.Sin (halfAng));
    this.Scale = new Vec3 (scl2.x, scl2.y, 1.0f);
    return this;
  }

  public Transform3 Set (
    Vec3 location,
    Quat rotation,
    Vec3 scale)
  {
    this.location = location;
    this.Rotation = rotation;
    this.Scale = scale;
    return this;
  }

  public Transform3 Set (
    float x = 0.0f, float y = 0.0f, float z = 0.0f,
    float real = 1.0f,
    float xImag = 0.0f, float yImag = 0.0f, float zImag = 0.0f,
    float width = 1.0f, float height = 1.0f, float depth = 1.0f)
  {
    this.location = new Vec3 (x, y, z);
    this.Rotation = new Quat (real, xImag, yImag, zImag);
    this.Scale = new Vec3 (width, height, depth);
    return this;
  }

  public string ToString (int places = 4)
  {
    return new StringBuilder ( )
      .Append ("{ location: ")
      .Append (this.location.ToString (places))
      .Append (", rotation: ")
      .Append (this.rotation.ToString (places))
      .Append (", scale: ")
      .Append (this.scale.ToString (places))
      .Append (" }")
      .ToString ( );
  }

  static Transform3 Identity
  {
    get
    {
      return new Transform3 (
        new Vec3 (0.0f, 0.0f, 0.0f),
        new Quat (1.0f, 0.0f, 0.0f, 0.0f),
        new Vec3 (1.0f, 1.0f, 1.0f));
    }
  }
}