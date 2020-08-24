using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class Mesh3
{
    protected Vec3[] coords;

    protected Loop3[] loops;

    protected String name = "Mesh3";

    protected Vec3[] normals;

    protected Vec2[] texCoords;

    public Vec3[] Coords
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

    public Loop3[] Loops
    {
        get
        {
            return this.loops;
        }

        set
        {
            this.loops = value;
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

    public Vec3[] Normals
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

    public Vec2[] TexCoords
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

    public Mesh3() { }

    public Mesh3(in String name)
    {
        this.name = name;
    }

    public Mesh3(in String name, in Loop3[] loops, in Vec3[] coords, in Vec2[] texCoords, in Vec3[] normals)
    {
        this.name = name;
        this.loops = loops;
        this.coords = coords;
        this.texCoords = texCoords;
        this.normals = normals;
    }

    public override string ToString()
    {
        return this.ToString(4);
    }

    public String ToString(in int places = 4)
    {
        StringBuilder sb = new StringBuilder(2048);
        sb.Append("{ name: \"");
        sb.Append(this.name);
        sb.Append("\"");

        // Append loops.
        int loopLen = this.loops.Length;
        int loopLast = loopLen - 1;
        sb.Append(", loops: [ ");
        for (int i = 0; i < loopLen; ++i)
        {
            sb.Append(this.loops[i]);
            if (i < loopLast)
            {
                sb.Append(", ");
            }
        }
        sb.Append(" ]");

        // Append coordinates.
        int coordLen = this.coords.Length;
        int coordLast = coordLen - 1;
        sb.Append(", coords: [ ");
        for (int i = 0; i < coordLen; ++i)
        {
            sb.Append(this.coords[i].ToString(places));
            if (i < coordLast)
            {
                sb.Append(", ");
            }
        }
        sb.Append(" ]");

        // Append texture coordinates.
        int texCoordLen = this.texCoords.Length;
        int texCoordLast = texCoordLen - 1;
        sb.Append(", texCoords: [ ");
        for (int i = 0; i < texCoordLen; ++i)
        {
            sb.Append(this.texCoords[i].ToString(places));
            if (i < texCoordLast)
            {
                sb.Append(", ");
            }
        }
        sb.Append(" ]");

        // Append normals.
        int normalLen = this.normals.Length;
        int normalLast = normalLen - 1;
        sb.Append(", normals: [ ");
        for (int i = 0; i < normalLen; ++i)
        {
            sb.Append(this.normals[i].ToString(places));
            if (i < normalLast)
            {
                sb.Append(", ");
            }
        }
        sb.Append(" ] }");

        return sb.ToString();
    }

    public static Mesh3 Triangulate(in Mesh3 source, in Mesh3 target)
    {
        Loop3[] loopsSrc = source.loops;
        Vec3[] vsSrc = source.coords;
        Vec2[] vtsSrc = source.texCoords;
        Vec3[] vnsSrc = source.normals;

        // Cannot anticipate how many loops in the source mesh
        // will not be triangles, so this is an expanding list.
        List<Loop3> loopsTrg = new List<Loop3>();

        int loopSrcLen = loopsSrc.Length;
        for (int i = 0; i < loopSrcLen; ++i)
        {
            Loop3 fSrc = loopsSrc[i];
            int fSrcLen = fSrc.Length;

            // If face loop is not a triangle, then split.
            if (fSrcLen > 3)
            {

                // Find last non-adjacent index. For index m,
                // neither m + 1 nor m - 1, so where m = 0, the last
                // non-adjacent would be arr.Length - 2.
                Index3 vert0 = fSrc[0];
                int lastNonAdj = fSrcLen - 2;
                for (int m = 0; m < lastNonAdj; ++m)
                {
                    // Find next two vertices.
                    Index3 vert1 = fSrc[1 + m];
                    Index3 vert2 = fSrc[2 + m];

                    // Create a new triangle which connects them.
                    Loop3 loopTrg = new Loop3(vert0, vert1, vert2);
                    loopsTrg.Add(loopTrg);
                }
            }
        }

        // If source and target are not the same mesh, then
        // copy mesh data from source to target.
        if (!Object.ReferenceEquals(source, target))
        {
            int vsLen = vsSrc.Length;
            target.coords = new Vec3[vsLen];
            System.Array.Copy(vsSrc, target.coords, vsLen);

            int vtsLen = vtsSrc.Length;
            target.texCoords = new Vec2[vtsLen];
            System.Array.Copy(vtsSrc, target.texCoords, vsLen);

            int vnsLen = vnsSrc.Length;
            target.normals = new Vec3[vnsSrc.Length];
            System.Array.Copy(vnsSrc, target.normals, vnsLen);
        }

        target.loops = loopsTrg.ToArray();

        return target;
    }

    public static Mesh3 UniformData(in Mesh3 source, in Mesh3 target)
    {
        Loop3[] fsSrc = source.loops;
        Vec3[] vsSrc = source.coords;
        Vec2[] vtsSrc = source.texCoords;
        Vec3[] vnsSrc = source.normals;

        int uniformLen = 0;
        int fsSrcLen = fsSrc.Length;
        for (int i = 0; i < fsSrcLen; ++i)
        {
            uniformLen += fsSrc[i].Length;
        }

        Loop3[] fsTrg = new Loop3[fsSrcLen];
        Vec3[] vsTrg = new Vec3[uniformLen];
        Vec2[] vtsTrg = new Vec2[uniformLen];
        Vec3[] vnsTrg = new Vec3[uniformLen];

        for (int k = 0, i = 0; i < fsSrcLen; ++i)
        {
            Index3[] fSrc = fsSrc[i].Indices;
            int fLen = fSrc.Length;
            fsTrg[i] = new Loop3(new Index3[fLen]);
            Index3[] fTrg = fsTrg[i].Indices;

            for (int j = 0; j < fLen; ++j, ++k)
            {
                Index3 vertSrc = fSrc[j];
                vsTrg[k] = vsSrc[vertSrc.v];
                vtsTrg[k] = vtsSrc[vertSrc.vt];
                vnsTrg[k] = vnsSrc[vertSrc.vn];

                fTrg[j] = new Index3(k, k, k);
            }
        }

        target.coords = vsTrg;
        target.texCoords = vtsTrg;
        target.normals = vnsTrg;
        target.loops = fsTrg;

        return target;
    }

    public static Mesh3 Cube(in float size, in PolyType poly, in Mesh3 target)
    {
        float vsz = Utils.Max(Utils.Epsilon, size);

        target.name = "Cube";

        target.coords = new Vec3[] {
            new Vec3(-vsz, -vsz, -vsz),
            new Vec3(-vsz, -vsz, vsz),
            new Vec3(-vsz, vsz, -vsz),
            new Vec3(-vsz, vsz, vsz),
            new Vec3(vsz, -vsz, -vsz),
            new Vec3(vsz, -vsz, vsz),
            new Vec3(vsz, vsz, -vsz),
            new Vec3(vsz, vsz, vsz)
        };

        target.normals = new Vec3[] {
            new Vec3(1.0f, 0.0f, 0.0f),
            new Vec3(0.0f, 0.0f, 1.0f),
            new Vec3(0.0f, 0.0f, -1.0f),
            new Vec3(0.0f, -1.0f, 0.0f),
            new Vec3(-1.0f, 0.0f, 0.0f),
            new Vec3(0.0f, 1.0f, 0.0f)
        };

        target.texCoords = new Vec2[] {
            new Vec2(0.625f, 0.0f),
            new Vec2(0.375f, 0.0f),
            new Vec2(0.375f, 0.75f),
            new Vec2(0.625f, 0.75f),
            new Vec2(0.375f, 1.0f),
            new Vec2(0.625f, 1.0f),
            new Vec2(0.625f, 0.5f),
            new Vec2(0.375f, 0.5f),
            new Vec2(0.625f, 0.25f),
            new Vec2(0.375f, 0.25f),
            new Vec2(0.125f, 0.5f),
            new Vec2(0.125f, 0.25f),
            new Vec2(0.875f, 0.25f),
            new Vec2(0.875f, 0.5f)
        };

        switch (poly)
        {
            case PolyType.Ngon:
            case PolyType.Quad:

                target.loops = new Loop3[] {
                    new Loop3(
                        new Index3(0, 4, 4),
                        new Index3(1, 5, 4),
                        new Index3(3, 3, 4),
                        new Index3(2, 2, 4)),
                    new Loop3(
                        new Index3(2, 2, 5),
                        new Index3(3, 3, 5),
                        new Index3(7, 6, 5),
                        new Index3(6, 7, 5)),
                    new Loop3(
                        new Index3(6, 7, 0),
                        new Index3(7, 6, 0),
                        new Index3(5, 8, 0),
                        new Index3(4, 9, 0)),
                    new Loop3(
                        new Index3(4, 9, 3),
                        new Index3(5, 8, 3),
                        new Index3(1, 0, 3),
                        new Index3(0, 1, 3)),
                    new Loop3(
                        new Index3(2, 10, 2),
                        new Index3(6,  7, 2),
                        new Index3(4,  9, 2),
                        new Index3(0, 11, 2)),
                    new Loop3(
                        new Index3(7,  6, 1),
                        new Index3(3, 13, 1),
                        new Index3(1, 12, 1),
                        new Index3(5,  8, 1))
                };

                break;

            case PolyType.Tri:
            default:

                target.loops = new Loop3[] {
                    new Loop3(
                        new Index3(0,  4, 4),
                        new Index3(1,  5, 4),
                        new Index3(3,  3, 4)),
                    new Loop3(
                        new Index3(0,  4, 4),
                        new Index3(3,  3, 4),
                        new Index3(2,  2, 4)),
                    new Loop3(
                        new Index3(2,  2, 5),
                        new Index3(3,  3, 5),
                        new Index3(7,  6, 5)),
                    new Loop3(
                        new Index3(2,  2, 5),
                        new Index3(7,  6, 5),
                        new Index3(6,  7, 5)),
                    new Loop3(
                        new Index3(6,  7, 0),
                        new Index3(7,  6, 0),
                        new Index3(5,  8, 0)),
                    new Loop3(
                        new Index3(6,  7, 0),
                        new Index3(5,  8, 0),
                        new Index3(4,  9, 0)),
                    new Loop3(
                        new Index3(4,  9, 3),
                        new Index3(5,  8, 3),
                        new Index3(1,  0, 3)),
                    new Loop3(
                        new Index3(4,  9, 3),
                        new Index3(1,  0, 3),
                        new Index3(0,  1, 3)),
                    new Loop3(
                        new Index3(2, 10, 2),
                        new Index3(6,  7, 2),
                        new Index3(4,  9, 2 )),
                    new Loop3(
                        new Index3(2, 10, 2),
                        new Index3(4,  9, 2),
                        new Index3(0, 11, 2 )),
                    new Loop3(
                        new Index3(7,  6, 1),
                        new Index3(3, 13, 1),
                        new Index3(1, 12, 1 )),
                    new Loop3(
                        new Index3(7,  6, 1),
                        new Index3(1, 12, 1),
                        new Index3(5,  8, 1 ))
                };

                break;
        }

        return target;
    }
}