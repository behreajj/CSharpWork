using System;
using System.Collections;
using System.Text;

/// <summary>
/// Organizes the vectors the shape a Bezier curve into a coordinate (or anchor
/// point), fore handle (the following control point) and rear handle (the
/// preceding control point).
/// </summary>
[Serializable]
public class Knot3
{
  /// <summary>
  /// The spatial coordinate of the knot.
  /// </summary>
  protected Vec3 coord;

  /// <summary>
  /// The handle which warps the curve segment heading away from the knot along
  /// the direction of the curve.
  /// </summary>
  protected Vec3 foreHandle;

  /// <summary>
  /// The handle which warps the curve segment heading towards the knot along
  /// the direction of the curve.
  /// </summary>
  protected Vec3 rearHandle;

  /// <summary>
  /// The spatial coordinate of the knot.
  /// </summary>
  /// <value>coordinate</value>
  public Vec3 Coord
  {
    get
    {
      return this.coord;
    }

    set
    {
      this.coord = value;
    }
  }

  /// <summary>
  /// The handle which warps the curve segment heading away from the knot along
  /// the direction of the curve.
  /// </summary>
  /// <value>fore handle</value>
  public Vec3 ForeHandle
  {
    get
    {
      return this.foreHandle;
    }

    set
    {
      this.foreHandle = value;
    }
  }

  /// <summary>
  /// The handle which warps the curve segment heading towards the knot along
  /// the direction of the curve.
  /// </summary>
  /// <value>the rear handle</value>
  public Vec3 RearHandle
  {
    get
    {
      return this.rearHandle;
    }

    set
    {
      this.rearHandle = value;
    }
  }

  /// <summary>
  /// The default constructor.
  /// </summary>
  public Knot3 ( )
  {
    this.coord = Vec3.Zero;
    this.foreHandle = Vec3.Zero;
    this.rearHandle = Vec3.Zero;
  }

  /// <summary>
  /// Creates a knot from a coordinate.
  /// </summary>
  /// <param name="coord">coordinate</param>
  public Knot3 (Vec3 coord)
  {
    this.coord = coord;
    this.foreHandle = this.coord + Utils.Epsilon;
    this.rearHandle = this.coord - Utils.Epsilon;
  }

  /// <summary>
  /// Creates a knot from a series of vectors.
  /// </summary>
  /// <param name="coord">coordinate</param>
  /// <param name="foreHandle">fore handle</param>
  /// <param name="rearHandle">rear handle</param>
  public Knot3 (
    Vec3 coord,
    Vec3 foreHandle,
    Vec3 rearHandle)
  {
    this.coord = coord;
    this.foreHandle = foreHandle;
    this.rearHandle = rearHandle;
  }

  /// <summary>
  /// Creates a knot from real numbers.
  /// </summary>
  /// <param name="xCo">x coordinate</param>
  /// <param name="yCo">y coordinate</param>
  /// <param name="zCo">y coordinate</param>
  /// <param name="xFh">fore handle x</param>
  /// <param name="yFh">fore handle y</param>
  /// <param name="zFh">fore handle z</param>
  /// <param name="xRh">rear handle x</param>
  /// <param name="yRh">rear handle y</param>
  /// <param name="zRh">rear handle z</param>
  public Knot3 (
    float xCo, float yCo, float zCo,
    float xFh, float yFh, float zFh,
    float xRh, float yRh, float zRh)
  {
    this.coord = new Vec3 (xCo, yCo, zCo);
    this.foreHandle = new Vec3 (xFh, yFh, zFh);
    this.rearHandle = new Vec3 (xRh, yRh, zRh);
  }

  /// <summary>
  /// Returns the knot's hash code based on those of its three constituent
  /// vectors.
  /// </summary>
  /// <returns>the hash code</returns>
  public override int GetHashCode ( )
  {
    unchecked
    {
      int hash = Utils.HashBase;
      hash = hash * Utils.HashMul ^ this.coord.GetHashCode ( );
      hash = hash * Utils.HashMul ^ this.foreHandle.GetHashCode ( );
      hash = hash * Utils.HashMul ^ this.rearHandle.GetHashCode ( );
      return hash;
    }
  }

  /// <summary>
  /// Returns a string representation of this knot.
  /// </summary>
  /// <returns>the string</returns>
  public override string ToString ( )
  {
    return ToString (4);
  }

  /// <summary>
  /// Adopts the fore handle of a source knot.
  /// </summary>
  /// <param name="source">source knot</param>
  /// <returns>this knot</returns>
  public Knot3 AdoptForeHandle (in Knot3 source)
  {
    this.foreHandle = this.coord + (source.foreHandle - source.coord);
    return this;
  }

  /// <summary>
  /// Adopts the fore handle and rear handle of a source knot.
  /// </summary>
  /// <param name="source">source knot</param>
  /// <returns>this knot</returns>
  public Knot3 AdoptHandles (in Knot3 source)
  {
    this.AdoptForeHandle (source);
    this.AdoptRearHandle (source);
    return this;
  }

  /// <summary>
  /// Adopts the rear handle of a source knot.
  /// </summary>
  /// <param name="source">source knot</param>
  /// <returns>this knot</returns>
  public Knot3 AdoptRearHandle (in Knot3 source)
  {
    this.rearHandle = this.coord + (source.rearHandle - source.coord);
    return this;
  }

  /// <summary>
  /// Aligns this knot's fore handle to its rear handle while preserving
  /// magnitude.
  /// </summary>
  /// <returns>this knot</returns>
  public Knot3 AlignHandlesBackward ( )
  {
    Vec3 rdir = this.rearHandle - this.coord;
    Vec3 fdir = this.foreHandle - this.coord;
    float flipRescale = Utils.Div (-Vec3.Mag (fdir), Vec3.Mag (rdir));
    this.foreHandle = this.coord + (flipRescale * rdir);
    return this;
  }

  /// <summary>
  /// Aligns this knot's rear handle to its fore handle while preserving
  /// magnitude.
  /// </summary>
  /// <returns>this knot</returns>
  public Knot3 AlignHandlesForward ( )
  {
    Vec3 rdir = this.rearHandle - this.coord;
    Vec3 fdir = this.foreHandle - this.coord;
    float flipRescale = Utils.Div (-Vec3.Mag (rdir), Vec3.Mag (fdir));
    this.rearHandle = this.coord + (flipRescale * fdir);
    return this;
  }

  /// <summary>
  /// Sets the forward-facing handle to mirror the rear-facing handle: the fore
  /// will have the same magnitude and negated direction of the rear.
  /// </summary>
  /// <returns>this knot</returns>
  public Knot3 MirrorHandlesBackward ( )
  {
    this.foreHandle = this.coord - (this.rearHandle - this.coord);
    return this;
  }

  /// <summary>
  /// Sets the rear-facing handle to mirror the forward-facing handle: the rear
  /// will have the same magnitude and negated direction of the fore.
  /// </summary>
  /// <returns>this knot</returns>
  public Knot3 MirrorHandlesForward ( )
  {
    this.rearHandle = this.coord - (this.foreHandle - this.coord);
    return this;
  }

  public Knot3 Rotate (in float radians, in Vec3 axis)
  {
    float sina = 0.0f;
    float cosa = 1.0f;
    Utils.SinCos (radians, out sina, out cosa);
    return this.Rotate (cosa, sina, axis);
  }

  public Knot3 Rotate (in float cosa, in float sina, in Vec3 axis)
  {
    this.coord = Vec3.Rotate (this.coord, cosa, sina, axis);
    this.foreHandle = Vec3.Rotate (this.foreHandle, cosa, sina, axis);
    this.rearHandle = Vec3.Rotate (this.rearHandle, cosa, sina, axis);

    return this;
  }

  /// <summary>
  /// Rotates this knot around the x axis by an angle in radians.
  /// </summary>
  /// <param name="radians">radians</param>
  /// <returns>this knot</returns>
  public Knot3 RotateX (in float radians)
  {
    float sina = 0.0f;
    float cosa = 1.0f;
    Utils.SinCos (radians, out sina, out cosa);
    return this.RotateX (cosa, sina);
  }

  /// <summary>
  /// Rotates a knot around the x axis. Accepts calculated sine and cosine of an
  /// angle, so that collections of knots can be efficiently rotated without
  /// repeatedly calling cos and sin.
  /// </summary>
  /// <param name="cosa">cosine of the angle</param>
  /// <param name="sina">sine of the angle</param>
  /// <returns>this knot</returns>
  public Knot3 RotateX (in float cosa, in float sina)
  {
    this.coord = Vec3.RotateX (this.coord, cosa, sina);
    this.foreHandle = Vec3.RotateX (this.foreHandle, cosa, sina);
    this.rearHandle = Vec3.RotateX (this.rearHandle, cosa, sina);

    return this;
  }

  /// <summary>
  /// Rotates this knot around the y axis by an angle in radians.
  /// </summary>
  /// <param name="radians">radians</param>
  /// <returns>this knot</returns>
  public Knot3 RotateY (in float radians)
  {
    float sina = 0.0f;
    float cosa = 1.0f;
    Utils.SinCos (radians, out sina, out cosa);
    return this.RotateY (cosa, sina);
  }

  /// <summary>
  /// Rotates a knot around the y axis. Accepts calculated sine and cosine of an
  /// angle, so that collections of knots can be efficiently rotated without
  /// repeatedly calling cos and sin.
  /// </summary>
  /// <param name="cosa">cosine of the angle</param>
  /// <param name="sina">sine of the angle</param>
  /// <returns>this knot</returns>
  public Knot3 RotateY (in float cosa, in float sina)
  {
    this.coord = Vec3.RotateY (this.coord, cosa, sina);
    this.foreHandle = Vec3.RotateY (this.foreHandle, cosa, sina);
    this.rearHandle = Vec3.RotateY (this.rearHandle, cosa, sina);

    return this;
  }

  /// <summary>
  /// Rotates this knot around the z axis by an angle in radians.
  /// </summary>
  /// <param name="radians">radians</param>
  /// <returns>this knot</returns>
  public Knot3 RotateZ (in float radians)
  {
    float sina = 0.0f;
    float cosa = 1.0f;
    Utils.SinCos (radians, out sina, out cosa);
    return this.RotateZ (cosa, sina);
  }

  /// <summary>
  /// Rotates a knot around the z axis. Accepts calculated sine and cosine of an
  /// angle, so that collections of knots can be efficiently rotated without
  /// repeatedly calling cos and sin.
  /// </summary>
  /// <param name="cosa">cosine of the angle</param>
  /// <param name="sina">sine of the angle</param>
  /// <returns>this knot</returns>
  public Knot3 RotateZ (in float cosa, in float sina)
  {
    this.coord = Vec3.RotateZ (this.coord, cosa, sina);
    this.foreHandle = Vec3.RotateZ (this.foreHandle, cosa, sina);
    this.rearHandle = Vec3.RotateZ (this.rearHandle, cosa, sina);

    return this;
  }

  /// <summary>
  /// Scales this knot by a factor.
  /// </summary>
  /// <param name="scale">factor</param>
  /// <returns>this knot</returns>
  public Knot3 Scale (in float scale)
  {
    this.coord *= scale;
    this.foreHandle *= scale;
    this.rearHandle *= scale;

    return this;
  }

  /// <summary>
  /// Scales this knot by a non uniform scalar.
  /// </summary>
  /// <param name="scale">non uniform scalar</param>
  /// <returns>this knot</returns>
  public Knot3 Scale (in Vec3 scale)
  {
    this.coord *= scale;
    this.foreHandle *= scale;
    this.rearHandle *= scale;

    return this;
  }

  /// <summary>
  /// Scales the fore handle by a factor.
  /// </summary>
  /// <param name="scalar">scalar</param>
  /// <returns>this knot</returns>
  public Knot3 ScaleForeHandleBy (in float scalar = 1.0f)
  {
    this.foreHandle -= this.coord;
    this.foreHandle *= scalar;
    this.foreHandle += this.coord;

    return this;
  }

  /// <summary>
  /// Scales the fore handle to a magnitude.
  /// </summary>
  /// <param name="magnitude">magnitude</param>
  /// <returns>this knot</returns>
  public Knot3 ScaleForeHandleTo (in float magnitude)
  {
    this.foreHandle -= this.coord;
    this.foreHandle = Vec3.Rescale (this.foreHandle, magnitude);
    this.foreHandle += this.coord;

    return this;
  }

  /// <summary>
  /// Scales both the fore and rear handle by a factor.
  /// </summary>
  /// <param name="magnitude">magnitude</param>
  /// <returns>the knot</returns>
  public Knot3 ScaleHandlesBy (in float scalar)
  {
    this.ScaleForeHandleBy (scalar);
    this.ScaleRearHandleBy (scalar);

    return this;
  }

  /// <summary>
  /// Scales both the fore and rear handle to a magnitude.
  /// </summary>
  /// <param name="magnitude">magnitude</param>
  /// <returns>the knot</returns>
  public Knot3 ScaleHandlesTo (in float magnitude)
  {
    this.ScaleForeHandleTo (magnitude);
    this.ScaleRearHandleTo (magnitude);

    return this;
  }

  /// <summary>
  /// Scales the rear handle by a factor.
  /// </summary>
  /// <param name="scalar">scalar</param>
  /// <returns>this knot</returns>
  public Knot3 ScaleRearHandleBy (in float scalar = 1.0f)
  {
    this.rearHandle -= this.coord;
    this.rearHandle *= scalar;
    this.rearHandle += this.coord;

    return this;
  }

  /// <summary>
  /// Scales the rear handle to a magnitude.
  /// </summary>
  /// <param name="magnitude">magnitude</param>
  /// <returns>this knot</returns>
  public Knot3 ScaleRearHandleTo (in float magnitude = 1.0f)
  {
    this.rearHandle -= this.coord;
    this.rearHandle = Vec3.Rescale (this.rearHandle, magnitude);
    this.rearHandle += this.coord;

    return this;
  }

  /// <summary>
  /// Returns a string representation of this vector.
  /// </summary>
  /// <param name="places">number of decimal places</param>
  /// <returns>the string</returns>
  public string ToString (int places = 4)
  {
    return new StringBuilder (512)
      .Append ("{ coord: ")
      .Append (this.coord.ToString (places))
      .Append (", foreHandle: ")
      .Append (this.foreHandle.ToString (places))
      .Append (", rearHandle: ")
      .Append (this.rearHandle.ToString (places))
      .Append (' ')
      .Append ('}')
      .ToString ( );
  }

  /// <summary>
  /// Transforms this knot by a matrix.
  /// </summary>
  /// <param name="tr">transform</param>
  /// <returns>this knot</returns>
  public Knot3 Transform (in Mat4 m)
  {
    this.coord = Mat4.MulPoint (m, this.coord);
    this.foreHandle = Mat4.MulPoint (m, this.foreHandle);
    this.rearHandle = Mat4.MulPoint (m, this.rearHandle);

    return this;
  }

  /// <summary>
  /// Transforms this knot by a transform.
  /// </summary>
  /// <param name="tr">transform</param>
  /// <returns>this knot</returns>
  public Knot3 Transform (in Transform3 tr)
  {
    this.coord = Transform3.MulPoint (tr, this.coord);
    this.foreHandle = Transform3.MulPoint (tr, this.foreHandle);
    this.rearHandle = Transform3.MulPoint (tr, this.rearHandle);

    return this;
  }

  /// <summary>
  /// Translates this knot by a vector.
  /// </summary>
  /// <param name="v">vector</param>
  /// <returns>this knot</returns>
  public Knot3 Translate (in Vec3 v)
  {
    this.coord += v;
    this.foreHandle += v;
    this.rearHandle += v;

    return this;
  }

  /// <summary>
  /// Promotes a 2D knot to a 3D knot.
  /// </summary>
  /// <param name="k">knot</param>
  public static implicit operator Knot3 (Knot2 k)
  {
    return new Knot3 (k.Coord, k.ForeHandle, k.RearHandle);
  }

  /// <summary>
  /// Gets the fore handle of a knot as a direction, rather than as a point.
  /// </summary>
  /// <param name="knot">knot</param>
  /// <returns>fore handle vector</returns>
  public static Vec3 ForeDir (in Knot3 knot)
  {
    return Vec3.Normalize (Knot3.ForeVec (knot));
  }

  /// <summary>
  /// Returns the magnitude of the knot's fore handle, i.e., the Euclidean
  /// distance between the fore handle and the coordinate.
  /// </summary>
  /// <param name="knot">knot</param>
  /// <returns>magnitude</returns>
  public static float ForeMag (in Knot3 knot)
  {
    return Vec3.DistEuclidean (knot.foreHandle, knot.coord);
  }

  /// <summary>
  /// Gets the fore handle of a knot as a vector, rather than as a point.
  /// </summary>
  /// <param name="knot">knot</param>
  /// <returns>fore handle vector</returns>
  public static Vec3 ForeVec (in Knot3 knot)
  {
    return knot.foreHandle - knot.coord;
  }

  /// <summary>
  /// Gets the rear handle of a knot as a direction, rather than as a point.
  /// </summary>
  /// <param name="knot">knot</param>
  /// <returns>rear handle vector</returns>
  public static Vec3 RearDir (in Knot3 knot)
  {
    return Vec3.Normalize (Knot3.RearVec (knot));
  }

  /// <summary>
  /// Returns the magnitude of the knot's rear handle, i.e., the Euclidean
  /// distance between the rear handle and the coordinate.
  /// </summary>
  /// <param name="knot">knot</param>
  /// <returns>magnitude</returns>
  public static float RearMag (in Knot3 knot)
  {
    return Vec3.DistEuclidean (knot.rearHandle, knot.coord);
  }

  /// <summary>
  /// Gets the rear handle of a knot as a vector, rather than as a point.
  /// </summary>
  /// <param name="knot">knot</param>
  /// <returns>rear handle vector</returns>
  public static Vec3 RearVec (in Knot3 knot)
  {
    return knot.rearHandle - knot.coord;
  }
}