using System;
using System.Collections;
using System.Text;

public class Knot3
{
  protected Vec3 coord;
  protected Vec3 foreHandle;
  protected Vec3 rearHandle;

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

  public Knot3 ( ) { }

  public Knot3 (Vec3 coord)
  {
    this.coord = coord;
    this.foreHandle = this.coord + Utils.Epsilon;
    this.rearHandle = this.coord - Utils.Epsilon;
  }

  public Knot3 (
    Vec3 coord,
    Vec3 foreHandle,
    Vec3 rearHandle)
  {
    this.coord = coord;
    this.foreHandle = foreHandle;
    this.rearHandle = rearHandle;
  }

  public Knot3 (
    float xCo, float yCo, float zCo,
    float xFh, float yFh, float zFh,
    float xRh, float yRh, float zRh)
  {
    this.coord = new Vec3 (xCo, yCo, zCo);
    this.foreHandle = new Vec3 (xFh, yFh, zFh);
    this.rearHandle = new Vec3 (xRh, yRh, zRh);
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

  public Knot3 AdoptForeHandle (in Knot3 source)
  {
    this.foreHandle = this.coord + (source.foreHandle - source.coord);
    return this;
  }

  public Knot3 AdoptHandles (in Knot3 source)
  {
    this.AdoptForeHandle (source);
    this.AdoptRearHandle (source);
    return this;
  }

  public Knot3 AdoptRearHandle (in Knot3 source)
  {
    this.rearHandle = this.coord + (source.rearHandle - source.coord);
    return this;
  }

  public Knot3 AlignHandlesBackward ( )
  {
    Vec3 rdir = this.rearHandle - this.coord;
    Vec3 fdir = this.foreHandle - this.coord;
    float flipRescale = Utils.Div (-Vec3.Mag (fdir), Vec3.Mag (rdir));
    this.foreHandle = this.coord + (flipRescale * rdir);
    return this;
  }

  public Knot3 AlignHandlesForward ( )
  {
    Vec3 rdir = this.rearHandle - this.coord;
    Vec3 fdir = this.foreHandle - this.coord;
    float flipRescale = Utils.Div (-Vec3.Mag (rdir), Vec3.Mag (fdir));
    this.rearHandle = this.coord + (flipRescale * fdir);
    return this;
  }

  public Knot3 MirrorHandlesBackward ( )
  {
    this.foreHandle = this.coord - (this.rearHandle - this.coord);
    return this;
  }

  public Knot3 MirrorHandlesForward ( )
  {
    this.rearHandle = this.coord - (this.foreHandle - this.coord);
    return this;
  }

  public Knot3 RotateZ (in float radians)
  {
    float sina = 0.0f;
    float cosa = 1.0f;
    Utils.SinCos (radians, out sina, out cosa);
    return this.RotateZ (cosa, sina);
  }

  public Knot3 RotateZ (in float cosa, in float sina)
  {
    Vec3.RotateZ (this.coord, cosa, sina);
    Vec3.RotateZ (this.foreHandle, cosa, sina);
    Vec3.RotateZ (this.rearHandle, cosa, sina);

    return this;
  }

  public Knot3 Scale (in float scale)
  {
    this.coord *= scale;
    this.foreHandle *= scale;
    this.rearHandle *= scale;

    return this;
  }

  public Knot3 Scale (in Vec3 scale)
  {
    this.coord *= scale;
    this.foreHandle *= scale;
    this.rearHandle *= scale;

    return this;
  }

  public Knot3 ScaleForeHandleBy (in float scalar = 1.0f)
  {
    this.foreHandle -= this.coord;
    this.foreHandle *= scalar;
    this.foreHandle += this.coord;

    return this;
  }

  public Knot3 ScaleForeHandleTo (in float magnitude)
  {
    this.foreHandle -= this.coord;
    this.foreHandle = Vec3.Rescale (this.foreHandle, magnitude);
    this.foreHandle += this.coord;

    return this;
  }

  public Knot3 ScaleHandlesBy (in float scalar)
  {
    this.ScaleForeHandleBy (scalar);
    this.ScaleRearHandleBy (scalar);

    return this;
  }

  public Knot3 ScaleHandlesTo (in float magnitude)
  {
    this.ScaleForeHandleTo (magnitude);
    this.ScaleRearHandleTo (magnitude);

    return this;
  }

  public Knot3 ScaleRearHandleBy (in float scalar = 1.0f)
  {
    this.rearHandle -= this.coord;
    this.rearHandle *= scalar;
    this.rearHandle += this.coord;

    return this;
  }

  public Knot3 ScaleRearHandleTo (in float magnitude = 1.0f)
  {
    this.rearHandle -= this.coord;
    this.rearHandle = Vec3.Rescale (this.rearHandle, magnitude);
    this.rearHandle += this.coord;

    return this;
  }

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

  public Knot3 Translate (in Vec3 v)
  {
    this.coord += v;
    this.foreHandle += v;
    this.rearHandle += v;

    return this;
  }

  public static implicit operator Knot3 (Knot2 k)
  {
    return new Knot3 (k.Coord, k.ForeHandle, k.RearHandle);
  }

  public static Vec3 ForeDir (in Knot3 knot)
  {
    return Vec3.Normalize (Knot3.ForeVec (knot));
  }

  public static float ForeMag (in Knot3 knot)
  {
    return Vec3.DistEuclidean (knot.foreHandle, knot.coord);
  }

  public static Vec3 ForeVec (in Knot3 knot)
  {
    return knot.foreHandle - knot.coord;
  }

  public static Vec3 RearDir (in Knot3 knot)
  {
    return Vec3.Normalize (Knot3.RearVec (knot));
  }

  public static float RearMag (in Knot3 knot)
  {
    return Vec3.DistEuclidean (knot.rearHandle, knot.coord);
  }

  public static Vec3 RearVec (in Knot3 knot)
  {
    return knot.rearHandle - knot.coord;
  }
}