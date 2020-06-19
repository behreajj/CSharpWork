using System;
using System.Text;

/// <summary>
/// Facilitates 3D affine transformations for entities.
/// </summary>
[Serializable]
public class Transform3
{
  /// <summary>
  /// The transform's location.
  /// </summary>
  protected Vec3 location = new Vec3 (0.0f, 0.0f, 0.0f);

  /// <summary>
  /// The transform's rotation in radians.
  /// </summary>
  protected Quat rotation = new Quat (1.0f, 0.0f, 0.0f, 0.0f);

  /// <summary>
  /// The transform's scale.
  /// </summary>
  protected Vec3 scale = new Vec3 (1.0f, 1.0f, 1.0f);

  /// <summary>
  /// The transform's forward axis.
  /// </summary>
  /// <value>forward</value>
  public Vec3 Forward { get { return this.rotation.Forward; } }

  /// <summary>
  /// The transform's location.
  /// </summary>
  /// <value>location</value>
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

  /// <summary>
  /// The transform's right axis.
  /// </summary>
  /// <value>right</value>
  public Vec3 Right { get { return this.rotation.Right; } }

  /// <summary>
  /// The transform's rotation in radians.
  /// </summary>
  /// <value>rotation</value>
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

  /// <summary>
  /// The transform's scale.
  /// </summary>
  /// <value>scale</value>
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

  /// <summary>
  /// The transform's up axis.
  /// </summary>
  /// <value>up</value>
  public Vec3 Up { get { return this.rotation.Up; } }

  /// <summary>
  /// The default constructor.
  /// </summary>
  public Transform3 ( ) { }

  /// <summary>
  /// Creates a transform from a location, rotation and scale.
  /// </summary>
  /// <param name="location">location</param>
  /// <param name="rotation">rotation</param>
  /// <param name="scale">scale</param>
  public Transform3 (
    Vec3 location,
    Quat rotation,
    Vec3 scale)
  {
    this.Location = location;
    this.Rotation = rotation;
    this.Scale = scale;
  }

  /// <summary>
  /// Creates a transform from loose real numbers.
  /// </summary>
  /// <param name="x">x location</param>
  /// <param name="y">y location</param>
  /// <param name="z">z location</param>
  /// <param name="real">quaternion real</param>
  /// <param name="xImag">x imaginary</param>
  /// <param name="yImag">y imaginary</param>
  /// <param name="zImag">z imaginary</param>
  /// <param name="width">x scale</param>
  /// <param name="height">y scale</param>
  /// <param name="depth">z scale</param>
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

  /// <summary>
  /// Returns a hash code representing this transform.
  /// </summary>
  /// <returns>the hash code</returns>
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

  /// <summary>
  /// Returns a string representation of this transform.
  /// </summary>
  /// <returns>the string</returns>
  public override string ToString ( )
  {
    return this.ToString (4);
  }

  /// <summary>
  /// Moves the transform by a direction to a new location.
  /// </summary>
  /// <param name="v">direction</param>
  /// <returns>this transform</returns>
  public Transform3 MoveBy (in Vec3 v)
  {
    return this.MoveByGlobal (v);
  }

  /// <summary>
  /// Moves the transform by a direction to a new location in the global
  /// coordinate system.
  /// </summary>
  /// <param name="v">direction</param>
  /// <returns>this transform</returns>
  public Transform3 MoveByGlobal (in Vec3 v)
  {
    this.Location += v;
    return this;
  }

  /// <summary>
  /// Moves the transform by a direction rotated according to the transform's
  /// rotation.
  /// </summary>
  /// <param name="v">direction</param>
  /// <returns>transform</returns>
  public Transform3 MoveByLocal (in Vec3 v)
  {
    this.Location += Transform3.MulDir (this, v);
    return this;
  }

  /// <summary>
  /// Eases the transform to a location by a step.
  /// </summary>
  /// <param name="v">direction</param>
  /// <param name="step">step</param>
  /// <returns>this transform</returns>
  public Transform3 MoveTo (in Vec3 v, in Vec3 step)
  {
    this.Location = Vec3.Mix (this.location, v, step);
    return this;
  }

  /// <summary>
  /// Eases the transform to a location by a step according to an easing
  /// function.
  /// </summary>
  /// <param name="v">direction</param>
  /// <param name="step">step</param>
  /// <param name="easing">easing function</param>
  /// <returns>this transform</returns>
  public Transform3 MoveTo (in Vec3 v, in Vec3 step, in Func<Vec3, Vec3, Vec3, Vec3> easing)
  {
    Vec3 t = easing (this.location, v, step);
    this.Location = Vec3.Mix (this.location, v, t);
    return this;
  }

  public Transform3 RotateTo(in Quat q)
  {
    this.Rotation = q;
    return this;
  }

  public Transform3 RotateX (in float radians)
  {
    this.rotation = Quat.RotateX (this.rotation, radians);
    return this;
  }

  public Transform3 RotateY (in float radians)
  {
    this.rotation = Quat.RotateY (this.rotation, radians);
    return this;
  }

  public Transform3 RotateZ (in float radians)
  {
    this.rotation = Quat.RotateZ (this.rotation, radians);
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

  public Transform3 ScaleTo (in Vec3 v, in Vec3 step, in Func<Vec3, Vec3, Vec3, Vec3> easing)
  {
    Vec3 t = easing (this.scale, v, step);
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

  public static implicit operator Transform3 (Transform2 t)
  {
    return new Transform3 (
      t.Location,
      Quat.FromAngle (t.Rotation),
      new Vec3 (t.Scale.x, t.Scale.y, 1.0f));
  }

  public static Vec3 InvMulDir (in Transform3 transform, in Vec3 dir)
  {
    return Quat.InvMulVector (transform.rotation, dir);
  }

  public static Vec3 InvMulPoint (in Transform3 transform, in Vec3 point)
  {
    return Quat.InvMulVector (transform.rotation, (point - transform.location) / transform.scale);
  }

  public static Vec3 InvMulVector (in Transform3 transform, in Vec3 vec)
  {
    return Quat.InvMulVector (transform.rotation, vec / transform.scale);
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

  /// <summary>
  /// Returns an identity transform.
  /// </summary>
  /// <value>identity</value>
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