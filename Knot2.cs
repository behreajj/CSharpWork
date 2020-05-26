using System;
using System.Collections;
using System.Text;

/// <summary>
/// Organizes the vectors the shape a Bezier curve into a coordinate (or anchor
/// point), fore handle (the following control point) and rear handle (the
/// preceding control point).
/// </summary>
[Serializable]
public class Knot2 : IEnumerable
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

  public Knot2 (
    Vec2 coord = new Vec2 ( ),
    Vec2 foreHandle = new Vec2 ( ),
    Vec2 rearHandle = new Vec2 ( ))
  {
    this.coord = coord;
    this.foreHandle = foreHandle;
    this.rearHandle = rearHandle;
  }

  public Knot2 (
    float xCo, float yCo,
    float xFh, float yFh,
    float xRh, float yRh)
  {
    this.coord = new Vec2 (xCo, yCo);
    this.foreHandle = new Vec2 (xFh, yFh);
    this.rearHandle = new Vec2 (xRh, yRh);
  }

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

  public override string ToString ( )
  {
    return ToString (4);
  }

  public Knot2 AdoptForeHandle (in Knot2 source)
  {
    this.foreHandle = this.coord + (source.foreHandle - source.coord);
    return this;
  }

  public Knot2 AdoptHandles (in Knot2 source)
  {
    this.AdoptForeHandle (source);
    this.AdoptRearHandle (source);
    return this;
  }

  public Knot2 AdoptRearHandle (in Knot2 source)
  {
    this.rearHandle = this.coord + (source.rearHandle - source.coord);
    return this;
  }

  public Knot2 AlignHandlesBackward ( )
  {
    Vec2 rdir = this.rearHandle - this.coord;
    Vec2 fdir = this.foreHandle - this.coord;
    float flipRescale = Utils.Div (-Vec2.Mag (fdir), Vec2.Mag (rdir));
    this.foreHandle = this.coord + (flipRescale * rdir);
    return this;
  }

  public Knot2 AlignHandlesForward ( )
  {
    Vec2 rdir = this.rearHandle - this.coord;
    Vec2 fdir = this.foreHandle - this.coord;
    float flipRescale = Utils.Div (-Vec2.Mag (rdir), Vec2.Mag (fdir));
    this.rearHandle = this.coord + (flipRescale * fdir);
    return this;
  }

  public IEnumerator GetEnumerator ( )
  {
    yield return this.coord.x;
    yield return this.coord.y;
    yield return this.foreHandle.x;
    yield return this.foreHandle.y;
    yield return this.rearHandle.x;
    yield return this.rearHandle.y;
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
  /// Gets the rear handle of a knot as a direction, rather than as a point.
  /// </summary>
  /// <param name="knot">knot</param>
  /// <returns>rear handle vector</returns>
  public static Vec2 RearDir (in Knot2 knot)
  {
    return Vec2.Normalize (Knot2.RearVec (knot));
  }

  /// <summary>
  /// Returns the magnitude of the knot's rear handle, i.e., the Euclidean distance between the rear handle and the coordinate.
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