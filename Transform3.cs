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
      if (Quat.Any (value)) this.rotation = value;
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
      if (Vec3.All (value)) this.scale = value;
    }
  }

  public Vec3 Right { get { return this.rotation.Right; } }

  public Vec3 Forward { get { return this.rotation.Forward; } }

  public Vec3 Up { get { return this.rotation.Up; } }

  public Transform3 ( ) { }

  public Transform3 (
    Vec3 location,
    Quat rotation,
    Vec3 scale)
  {
    this.Location = location;
    this.Rotation = rotation;
    this.Scale = scale;
  }

  public Transform3 (
    float x = 0.0f, float y = 0.0f, float z = 0.0f,
    float real = 1.0f,
    float xImag = 0.0f, float yImag = 0.0f, float zImag = 0.0f,
    float width = 1.0f, float height = 1.0f, float depth = 1.0f)
  {
    this.Location = new Vec3 (x, y, z);
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
    return this.ToString (4);
  }

  public Transform3 MoveBy (in Vec3 v)
  {
    this.Location += v;
    return this;
  }

  public Transform3 MoveTo (in Vec3 v, in Vec3 step)
  {
    this.Location = Vec3.Mix (this.location, v, step);
    return this;
  }

  public Transform3 MoveTo (in Vec3 v, in Vec3 step, in Func<Vec3, Vec3, Vec3, Vec3> Easing)
  {
    Vec3 t = Easing (this.location, v, step);
    this.Location = Vec3.Mix (this.location, v, t);
    return this;
  }

  public Transform3 ScaleBy (in Vec3 v)
  {
    this.Scale += v;
    return this;
  }

  public Transform3 ScaleTo (in Vec3 v, in Vec3 step)
  {
    this.Scale = Vec3.Mix (this.scale, v, step);
    return this;
  }

  public Transform3 ScaleTo (in Vec3 v, in Vec3 step, in Func<Vec3, Vec3, Vec3, Vec3> Easing)
  {
    Vec3 t = Easing (this.scale, v, step);
    this.Scale = Vec3.Mix (this.scale, v, t);
    return this;
  }

  public string ToString (in int places = 4)
  {
    return new StringBuilder (354)
      .Append ("{ location: ")
      .Append (this.location.ToString (places))
      .Append (", rotation: ")
      .Append (this.rotation.ToString (places))
      .Append (", scale: ")
      .Append (this.scale.ToString (places))
      .Append (" }")
      .ToString ( );
  }

  public static Vec3 MulDir (in Transform3 transform, in Vec3 dir)
  {
    return Quat.MulVector (transform.rotation, dir);
  }

  public static Vec3 MulPoint (in Transform3 transform, in Vec3 point)
  {
    return transform.location + transform.scale * Quat.MulVector (transform.rotation, point);
  }

  public static Vec3 MulVector (in Transform3 transform, in Vec3 vec)
  {
    return transform.scale * Quat.MulVector (transform.rotation, vec);
  }

  public static (Vec3 right, Vec3 forward, Vec3 up) ToAxes (in Transform3 tr)
  {
    return Quat.ToAxes (tr.rotation);
  }

  public static Transform3 Identity
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