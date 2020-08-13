using System;

public class Mesh3
{
    protected Vec3[ ] coords;

    protected Index3[ ] indices;

    protected Vec3[ ] normals;

    protected String name = "Mesh3";

    protected Vec2[ ] texCoords;

    public Vec3[ ] Coords
    {
        get
        {
            return this.coords;
        }

        set
        {
            this.coords = value;
        }
    }

    public Index3[ ] Indices
    {
        get
        {
            return this.indices;
        }

        set
        {
            this.indices = value;
        }
    }

    public String Name
    {
        get
        {
            return this.name;
        }

        set
        {
            this.name = value;
        }
    }

    public Vec3[ ] Normals
    {
        get
        {
            return this.normals;
        }

        set
        {
            this.normals = value;
        }
    }

    public Vec2[ ] TexCoords
    {
        get
        {
            return this.texCoords;
        }

        set
        {
            this.texCoords = value;
        }
    }

    public Mesh3 ( ) { }

    public Mesh3 (in String name)
    {
        this.name = name;
    }

    public Mesh3 (in String name, in Index3[ ] indices, in Vec3[ ] coords, in Vec2[ ] texCoords, in Vec3[ ] normals)
    {
        this.name = name;
        this.indices = indices;
        this.coords = coords;
        this.texCoords = texCoords;
        this.normals = normals;
    }

    public static Mesh3 Cube (in float size, in Mesh3 target)
    {
        float vsz = Utils.Max (Utils.Epsilon, size);

        target.name = "Cube";

        target.coords = Vec3.Resize (target.coords, 8);
        target.coords[0] = new Vec3 (-vsz, -vsz, -vsz);
        target.coords[1] = new Vec3 (-vsz, -vsz, vsz);
        target.coords[2] = new Vec3 (-vsz, vsz, -vsz);
        target.coords[3] = new Vec3 (-vsz, vsz, vsz);
        target.coords[4] = new Vec3 (vsz, -vsz, -vsz);
        target.coords[5] = new Vec3 (vsz, -vsz, vsz);
        target.coords[6] = new Vec3 (vsz, vsz, -vsz);
        target.coords[7] = new Vec3 (vsz, vsz, vsz);

        target.texCoords = Vec2.Resize (target.texCoords, 4);
        target.texCoords[0] = new Vec2 (0.0f, 0.0f);
        target.texCoords[1] = new Vec2 (0.0f, 1.0f);
        target.texCoords[2] = new Vec2 (1.0f, 1.0f);
        target.texCoords[3] = new Vec2 (1.0f, 0.0f);

        target.normals = Vec3.Resize (target.normals, 6);
        target.normals[0] = new Vec3 (1.0f, 0.0f, 0.0f);
        target.normals[1] = new Vec3 (0.0f, 0.0f, 1.0f);
        target.normals[2] = new Vec3 (0.0f, 0.0f, -1.0f);
        target.normals[3] = new Vec3 (0.0f, -1.0f, 0.0f);
        target.normals[4] = new Vec3 (-1.0f, 0.0f, 0.0f);
        target.normals[5] = new Vec3 (0.0f, 1.0f, 0.0f);

        return target;
    }
}