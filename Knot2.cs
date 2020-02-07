using System;
using System.Collections;
using System.Text;

[Serializable]
public class Knot2
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
    this.coord = new Vec2(xCo, yCo);
    this.foreHandle = new Vec2(xFh, yFh);
    this.rearHandle = new Vec2(xRh, yRh);
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
}