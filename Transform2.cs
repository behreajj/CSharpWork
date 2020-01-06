﻿using System;
using System.Text;

[Serializable]
public class Transform2
{
    public Vec2 location = new Vec2 ( );
    public float rotation = 0.0f;
    public Vec2 scale = new Vec2 (1.0f, 1.0f);

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
            this.scale = Vec2.Max (value, Utils.Epsilon);
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
        this.scale = scale;
    }

    public Transform2 MoveBy (Vec2 v)
    {
        this.location += v;
        return this;
    }

    public Transform2 MoveTo (Vec2 v, float step = 1.0f)
    {
        if (step <= 0.0f) return this;
        if (step >= 1.0f) { this.location = v; return this; }

        this.location = Vec2.Mix (this.location, v, step);
        return this;
    }

    public Transform2 MoveTo (Vec2 v, Vec2 step, Func<Vec2, Vec2, Vec2, Vec2> Easing)
    {
        Vec2 t = Easing (this.location, v, step);
        this.location = Vec2.Mix (this.location, v, t);
        return this;
    }

    public Transform2 ScaleBy (Vec2 v)
    {
        this.Scale = this.scale + v;
        return this;
    }

    public Transform2 ScaleTo (Vec2 v, float step = 1.0f)
    {
        if (step <= 0.0f) return this;
        if (step >= 1.0f) { this.Scale = v; return this; }

        this.Scale = Vec2.Mix (this.scale, v, step);
        return this;
    }
}