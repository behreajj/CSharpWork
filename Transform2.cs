using System;
using System.Text;

[Serializable]
public class Transform2
{
    protected Vec2 location = new Vec2 ( );
    protected float rotation = 0.0f;
    protected Vec2 scale = new Vec2 (1.0f, 1.0f);

    public Vec2 Location
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

    public float Rotation
    {
        get
        {
            return this.rotation;
        }

        set
        {
            this.rotation = value;
        }
    }

    public Vec2 Scale
    {
        get
        {
            return this.scale;
        }

        set
        {
            if (Vec2.All (value)) this.scale = value;
        }
    }

    public Vec2 Right { get { return Vec2.FromPolar (this.rotation); } }
    
    public Vec2 Forward { get { return Vec2.FromPolar (this.rotation + Utils.HalfPi); } }

    public Transform2 ( ) { }

    public Transform2 (
        Vec2 location,
        float rotation,
        Vec2 scale)
    {
        this.Location = location;
        this.Rotation = rotation;
        this.Scale = scale;
    }

    public Transform2 (
        float x = 0.0f, float y = 0.0f,
        float rotation = 0.0f,
        float width = 1.0f, float height = 1.0f)
    {
        this.Location = new Vec2 (x, y);
        this.Rotation = rotation;
        this.Scale = new Vec2 (width, height);
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

    public Transform2 FlipX ( )
    {
        this.Scale = new Vec2 (-this.scale.x, this.scale.y);
        return this;
    }

    public Transform2 FlipY ( )
    {
        this.Scale = new Vec2 (this.scale.x, -this.scale.y);
        return this;
    }

    public Transform2 MoveBy (in Vec2 v)
    {
        this.Location += v;
        return this;
    }

    public Transform2 MoveTo (in Vec2 v, in Vec2 step)
    {
        this.Location = Vec2.Mix (this.location, v, step);
        return this;
    }

    public Transform2 MoveTo (in Vec2 v, in Vec2 step, in Func<Vec2, Vec2, Vec2, Vec2> Easing)
    {
        Vec2 t = Easing (this.location, v, step);
        this.Location = Vec2.Mix (this.location, v, t);
        return this;
    }

    public Transform2 RotateTo (in float radians, in float step = 1.0f)
    {
        this.Rotation = Utils.LerpAngle (this.rotation, radians, step);
        return this;
    }

    public Transform2 RotateZ (in float radians)
    {
        this.Rotation += radians;
        return this;
    }

    public Transform2 ScaleBy (in Vec2 v)
    {
        this.Scale += v;
        return this;
    }

    public Transform2 ScaleTo (in Vec2 v, in Vec2 step)
    {
        this.Scale = Vec2.Mix (this.scale, v, step);
        return this;
    }

    public Transform2 ScaleTo (in Vec2 v, in Vec2 step, in Func<Vec2, Vec2, Vec2, Vec2> Easing)
    {
        Vec2 t = Easing (this.scale, v, step);
        this.Scale = Vec2.Mix (this.scale, v, t);
        return this;
    }

    public string ToString (in int places = 4)
    {
        return new StringBuilder (160)
            .Append ("{ location: ")
            .Append (this.location.ToString (places))
            .Append (", rotation: ")
            .Append (Utils.ToFixed (this.rotation, places))
            .Append (", scale: ")
            .Append (this.scale.ToString (places))
            .Append (" }")
            .ToString ( );
    }

    public static Vec2 MulDir (in Transform2 transform, in Vec2 dir)
    {
        return Vec2.RotateZ (dir, transform.rotation);
    }

    public static Vec2 MulPoint (in Transform2 transform, in Vec2 point)
    {
        return transform.location + transform.scale * Vec2.RotateZ (point, transform.rotation);
    }

    public static Vec2 MulVector (in Transform2 transform, in Vec2 vec)
    {
        return transform.scale * Vec2.RotateZ (vec, transform.rotation);
    }

    public static (Vec2 right, Vec2 forward) ToAxes (in Transform2 tr)
    {
        Vec2 r = Vec2.FromPolar (tr.rotation);
        return (right: r, forward: Vec2.PerpendicularCCW (r));
    }

    public static Transform2 Identity
    {
        get
        {
            return new Transform2 (
                new Vec2 (0.0f, 0.0f),
                0.0f,
                new Vec2 (1.0f, 1.0f));
        }
    }
}