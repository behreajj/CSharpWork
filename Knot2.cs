using System;
using System.Collections;
using System.Text;

[Serializable]
public class Knot2 : IEnumerable
{
  protected Vec2 coord;
  protected Vec2 foreHandle;
  protected Vec2 rearHandle;

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

  public IEnumerator GetEnumerator ( )
  {
    yield return this.coord.x;
    yield return this.coord.y;
    yield return this.foreHandle.x;
    yield return this.foreHandle.y;
    yield return this.rearHandle.x;
    yield return this.rearHandle.y;
  }

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

  public static Vec2 ForeDir (in Knot2 knot)
  {
    return Vec2.Normalize (knot.foreHandle - knot.coord);
  }

  public static float ForeMag (in Knot2 knot)
  {
    return Vec2.DistEuclidean (knot.foreHandle, knot.coord);
  }

  public static Vec2 ForeVec (in Knot2 knot)
  {
    return knot.foreHandle - knot.coord;
  }

  public static Vec2 RearDir (in Knot2 knot)
  {
    return Vec2.Normalize (knot.rearHandle - knot.coord);
  }

  public static float RearMag (in Knot2 knot)
  {
    return Vec2.DistEuclidean (knot.rearHandle, knot.coord);
  }

  public static Vec2 RearVec (in Knot2 knot)
  {
    return knot.rearHandle - knot.coord;
  }
}