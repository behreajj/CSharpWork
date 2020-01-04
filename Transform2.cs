using System;
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
            this.scale = value;
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
}