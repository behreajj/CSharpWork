using System;
using System.Collections;
using System.Text;

/// <summary>
/// Organizes the vectors the shape a Bezier curve into a coordinate (or anchor
/// point), fore handle (the following control point) and rear handle (the
/// preceding control point).
/// </summary>
[Serializable]
public class Knot2
{
  /// <summary>
  /// The spatial coordinate of the knot.
  /// </summary>
  protected Vec2 coord;

  /// <summary>
  /// The handle which warps the curve segment heading away from the knot along
  /// the direction of the curve.
  /// </summary>
  protected Vec2 foreHandle;

  /// <summary>
  /// The handle which warps the curve segment heading towards the knot along
  /// the direction of the curve.
  /// </summary>
  protected Vec2 rearHandle;

  /// <summary>
  /// The spatial coordinate of the knot.
  /// </summary>
  /// <value>coordinate</value>
  public Vec2 Coord
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
  public Vec2 ForeHandle
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
  public Vec2 RearHandle
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
  public Knot2 ( ) { }

  /// <summary>
  /// Creates a knot from a vector.
  /// </summary>
  /// <param name="coord">coordinate</param>
  public Knot2 (Vec2 coord)
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
  public Knot2 (
    Vec2 coord,
    Vec2 foreHandle,
    Vec2 rearHandle)
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
  /// <param name="xFh">fore handle x</param>
  /// <param name="yFh">fore handle y</param>
  /// <param name="xRh">rear handle x</param>
  /// <param name="yRh">rear handle y</param>
  public Knot2 (
    float xCo, float yCo,
    float xFh, float yFh,
    float xRh, float yRh)
  {
    this.coord = new Vec2 (xCo, yCo);
    this.foreHandle = new Vec2 (xFh, yFh);
    this.rearHandle = new Vec2 (xRh, yRh);
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
  public Knot2 AdoptForeHandle (in Knot2 source)
  {
    this.foreHandle = this.coord + (source.foreHandle - source.coord);
    return this;
  }

  /// <summary>
  /// Adopts the fore handle and rear handle of a source knot.
  /// </summary>
  /// <param name="source">source knot</param>
  /// <returns>this knot</returns>
  public Knot2 AdoptHandles (in Knot2 source)
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
  public Knot2 AdoptRearHandle (in Knot2 source)
  {
    this.rearHandle = this.coord + (source.rearHandle - source.coord);
    return this;
  }

  /// <summary>
  /// Aligns this knot's fore handle to its rear handle while preserving
  /// magnitude.
  /// </summary>
  /// <returns>this knot</returns>
  public Knot2 AlignHandlesBackward ( )
  {
    Vec2 rdir = this.rearHandle - this.coord;
    Vec2 fdir = this.foreHandle - this.coord;
    float flipRescale = Utils.Div (-Vec2.Mag (fdir), Vec2.Mag (rdir));
    this.foreHandle = this.coord + (flipRescale * rdir);
    return this;
  }

  /// <summary>
  /// Aligns this knot's rear handle to its fore handle while preserving
  /// magnitude.
  /// </summary>
  /// <returns>this knot</returns>
  public Knot2 AlignHandlesForward ( )
  {
    Vec2 rdir = this.rearHandle - this.coord;
    Vec2 fdir = this.foreHandle - this.coord;
    float flipRescale = Utils.Div (-Vec2.Mag (rdir), Vec2.Mag (fdir));
    this.rearHandle = this.coord + (flipRescale * fdir);
    return this;
  }

  /// <summary>
  /// Sets the forward-facing handle to mirror the rear-facing handle: the fore
  /// will have the same magnitude and negated direction of the rear.
  /// </summary>
  /// <returns>this knot</returns>
  public Knot2 MirrorHandlesBackward ( )
  {
    this.foreHandle = this.coord - (this.rearHandle - this.coord);
    return this;
  }

  /// <summary>
  /// Sets the rear-facing handle to mirror the forward-facing handle: the rear
  /// will have the same magnitude and negated direction of the fore.
  /// </summary>
  /// <returns>this knot</returns>
  public Knot2 MirrorHandlesForward ( )
  {
    this.rearHandle = this.coord - (this.foreHandle - this.coord);
    return this;
  }

  /// <summary>
  /// Rotates this knot around the z axis by an angle in radians.
  /// </summary>
  /// <param name="radians">radians</param>
  /// <returns>this knot</returns>
  public Knot2 RotateZ (in float radians)
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
  public Knot2 RotateZ (in float cosa, in float sina)
  {
    Vec2.RotateZ (this.coord, cosa, sina);
    Vec2.RotateZ (this.foreHandle, cosa, sina);
    Vec2.RotateZ (this.rearHandle, cosa, sina);

    return this;
  }

  /// <summary>
  /// Scales this knot by a factor.
  /// </summary>
  /// <param name="scale">factor</param>
  /// <returns>this knot</returns>
  public Knot2 Scale (in float scale)
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
  public Knot2 Scale (in Vec2 scale)
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
  public Knot2 ScaleForeHandleBy (in float scalar = 1.0f)
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
  public Knot2 ScaleForeHandleTo (in float magnitude)
  {
    this.foreHandle -= this.coord;
    this.foreHandle = Vec2.Rescale (this.foreHandle, magnitude);
    this.foreHandle += this.coord;

    return this;
  }

  /// <summary>
  /// Scales both the fore and rear handle by a factor.
  /// </summary>
  /// <param name="magnitude">magnitude</param>
  /// <returns>the knot</returns>
  public Knot2 ScaleHandlesBy (in float scalar)
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
  public Knot2 ScaleHandlesTo (in float magnitude)
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
  public Knot2 ScaleRearHandleBy (in float scalar = 1.0f)
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
  public Knot2 ScaleRearHandleTo (in float magnitude = 1.0f)
  {
    this.rearHandle -= this.coord;
    this.rearHandle = Vec2.Rescale (this.rearHandle, magnitude);
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
    return new StringBuilder (256)
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
  /// Translates this knot by a vector.
  /// </summary>
  /// <param name="v">vector</param>
  /// <returns>this knot</returns>
  public Knot2 Translate (in Vec2 v)
  {
    this.coord += v;
    this.foreHandle += v;
    this.rearHandle += v;

    return this;
  }

  /// <summary>
  /// Gets the fore handle of a knot as a direction, rather than as a point.
  /// </summary>
  /// <param name="knot">knot</param>
  /// <returns>fore handle vector</returns>
  public static Vec2 ForeDir (in Knot2 knot)
  {
    return Vec2.Normalize (Knot2.ForeVec (knot));
  }

  /// <summary>
  /// Returns the magnitude of the knot's fore handle, i.e., the Euclidean
  /// distance between the fore handle and the coordinate.
  /// </summary>
  /// <param name="knot">knot</param>
  /// <returns>magnitude</returns>
  public static float ForeMag (in Knot2 knot)
  {
    return Vec2.DistEuclidean (knot.foreHandle, knot.coord);
  }

  /// <summary>
  /// Gets the fore handle of a knot as a vector, rather than as a point.
  /// </summary>
  /// <param name="knot">knot</param>
  /// <returns>fore handle vector</returns>
  public static Vec2 ForeVec (in Knot2 knot)
  {
    return knot.foreHandle - knot.coord;
  }

  /// <summary>
  /// Creates a knot from polar coordinates, where the knot's fore handle is
  /// tangent to the radius.
  /// </summary>
  /// <param name="cosa">cosine of the angle</param>
  /// <param name="sina">sine of the angle</param>
  /// <param name="radius">radius</param>
  /// <param name="handleMag">length of handles</param>
  /// <param name="xCenter">x center</param>
  /// <param name="yCenter">y center</param>
  /// <returns>the knot</returns>
  public static Knot2 FromPolar (in float cosa = 1.0f, in float sina = 0.0f, in float radius = 1.0f, in float handleMag = Utils.FourThirds, in float xCenter = 0.0f, in float yCenter = 0.0f)
  {
    float cox = xCenter + radius * cosa;
    float coy = yCenter + radius * sina;

    float hmsina = sina * handleMag;
    float hmcosa = cosa * handleMag;

    float fhx = cox - hmsina;
    float fhy = coy + hmcosa;

    float rhx = cox + hmsina;
    float rhy = coy - hmcosa;

    return new Knot2 (cox, coy, fhx, fhy, rhx, rhy);
  }

  /// <summary>
  /// Gets the rear handle of a knot as a direction, rather than as a point.
  /// </summary>
  /// <param name="knot">knot</param>
  /// <returns>rear handle vector</returns>
  public static Vec2 RearDir (in Knot2 knot)
  {
    return Vec2.Normalize (Knot2.RearVec (knot));
  }

  /// <summary>
  /// Returns the magnitude of the knot's rear handle, i.e., the Euclidean
  /// distance between the rear handle and the coordinate.
  /// </summary>
  /// <param name="knot">knot</param>
  /// <returns>magnitude</returns>
  public static float RearMag (in Knot2 knot)
  {
    return Vec2.DistEuclidean (knot.rearHandle, knot.coord);
  }

  /// <summary>
  /// Gets the rear handle of a knot as a vector, rather than as a point.
  /// </summary>
  /// <param name="knot">knot</param>
  /// <returns>rear handle vector</returns>
  public static Vec2 RearVec (in Knot2 knot)
  {
    return knot.rearHandle - knot.coord;
  }
}