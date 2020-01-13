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
            if(Vec2.All(value)) this.scale = value;
        }
    }

    public Transform2 ( ) { }

    public Transform2 (
        Vec2 location,
        float rotation,
        Vec2 scale)
    {
        this.location = location;
        this.rotation = rotation;
        this.Scale = scale;
    }

    public Transform2 (
        float x = 0.0f, float y = 0.0f,
        float rotation = 0.0f,
        float width = 1.0f, float height = 1.0f)
    {
        this.location = new Vec2 (x, y);
        this.rotation = rotation;
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
        return ToString (4);
    }

    public Transform2 MoveBy (in Vec2 v)
    {
        this.location += v;
        return this;
    }

    public Transform2 MoveTo (in Vec2 v, float step = 1.0f)
    {
        if (step <= 0.0f) return this;
        if (step >= 1.0f) { this.location = v; return this; }

        this.location = Vec2.Mix (this.location, v, step);
        return this;
    }

    public Transform2 MoveTo (in Vec2 v, in Vec2 step, Func<Vec2, Vec2, Vec2, Vec2> Easing)
    {
        Vec2 t = Easing (this.location, v, step);
        this.location = Vec2.Mix (this.location, v, t);
        return this;
    }

    public static Vec2 MulDir (in Transform2 transform, in Vec2 dir)
    {
        return Vec2.RotateZ (dir, transform.rotation);
    }

    public static Vec2 MulPoint (in Transform2 transform, in Vec2 vec)
    {
        return transform.location + transform.scale * Vec2.RotateZ (vec, transform.rotation);
    }

    public static Vec2 MulVector (in Transform2 transform, in Vec2 vec)
    {
        return transform.scale * Vec2.RotateZ (vec, transform.rotation);
    }

    public Transform2 Reset ( )
    {
        this.location = new Vec2 (0.0f, 0.0f);
        this.rotation = 0.0f;
        this.scale = new Vec2 (1.0f, 1.0f);

        return this;
    }

    public Transform2 RotateTo (float radians, float step = 1.0f)
    {
        if (step <= 0.0f) return this;
        if (step >= 1.0f) { this.rotation = radians; return this; }

        this.rotation = Utils.LerpAngle (this.rotation, radians, step);
        return this;
    }

    public Transform2 RotateZ (float radians)
    {
        this.rotation += radians;
        return this;
    }

    public Transform2 ScaleBy (in Vec2 v)
    {
        this.Scale = this.scale + v;
        return this;
    }

    public Transform2 ScaleTo (in Vec2 v, float step = 1.0f)
    {
        if (step <= 0.0f) return this;
        if (step >= 1.0f) { this.Scale = v; return this; }

        this.Scale = Vec2.Mix (this.scale, v, step);
        return this;
    }

    public Transform2 ScaleTo (in Vec2 v, in Vec2 step, Func<Vec2, Vec2, Vec2, Vec2> Easing)
    {
        Vec2 t = Easing (this.scale, v, step);
        this.Scale = Vec2.Mix (this.scale, v, t);
        return this;
    }

    public Transform2 Set (
        Vec2 location,
        float rotation,
        Vec2 scale)
    {
        this.location = location;
        this.rotation = rotation;
        this.Scale = scale;
        return this;
    }

    public Transform2 Set (
        float x = 0.0f, float y = 0.0f,
        float rotation = 0.0f,
        float width = 1.0f, float height = 1.0f)
    {
        this.location = new Vec2 (x, y);
        this.rotation = rotation;
        this.Scale = new Vec2 (width, height);
        return this;
    }

    public string ToString (int places = 4)
    {
        return new StringBuilder ( )
            .Append ("{ location: ")
            .Append (this.location.ToString (places))
            .Append (", rotation: ")
            .Append (Utils.ToFixed (this.rotation, places))
            .Append (", scale: ")
            .Append (this.scale.ToString (places))
            .Append (" }")
            .ToString ( );
    }

    static Transform2 Identity
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