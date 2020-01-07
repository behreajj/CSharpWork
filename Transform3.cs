using System;
using System.Text;

[Serializable]
public class Transform3
{
  protected Vec3 location = new Vec3 ( );
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
    float real = 1.0f, float xImag = 0.0f, float yImag = 0.0f, float zImag = 0.0f,
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
      return new Transform3 (new Vec3 ( ), new Quat (1.0f, 0.0f, 0.0f, 0.0f), new Vec3 (1.0f, 1.0f, 1.0f));
    }
  }
}