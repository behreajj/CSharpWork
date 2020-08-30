using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class Mesh2
{
    protected Vec2[ ] coords;

    protected Loop2[ ] loops;

    protected String name = "Mesh2";

    protected Vec2[ ] texCoords;

    public Vec2[ ] Coords
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

    public Loop2[ ] Loops
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

    public Mesh2 ( ) { }

    public Mesh2 (in String name)
    {
        this.name = name;
    }

    public Mesh2 (in Loop2[ ] loops, in Vec2[ ] coords, in Vec2[ ] texCoords)
    {
        this.loops = loops;
        this.coords = coords;
        this.texCoords = texCoords;
    }

    public Mesh2 (in String name, in Loop2[ ] loops, in Vec2[ ] coords, in Vec2[ ] texCoords)
    {
        this.name = name;
        this.loops = loops;
        this.coords = coords;
        this.texCoords = texCoords;
    }

    public override string ToString ( )
    {
        return this.ToString (4);
    }

    public String ToString (in int places = 4)
    {
        StringBuilder sb = new StringBuilder (2048);
        sb.Append ("{ name: \"");
        sb.Append (this.name);
        sb.Append ("\"");

        // Append loops.
        int loopLen = this.loops.Length;
        int loopLast = loopLen - 1;
        sb.Append (", loops: [ ");
        for (int i = 0; i < loopLen; ++i)
        {
            sb.Append (this.loops[i]);
            if (i < loopLast)
            {
                sb.Append (", ");
            }
        }
        sb.Append (" ]");

        // Append coordinates.
        int coordLen = this.coords.Length;
        int coordLast = coordLen - 1;
        sb.Append (", coords: [ ");
        for (int i = 0; i < coordLen; ++i)
        {
            sb.Append (this.coords[i].ToString (places));
            if (i < coordLast)
            {
                sb.Append (", ");
            }
        }
        sb.Append (" ]");

        // Append texture coordinates.
        int texCoordLen = this.texCoords.Length;
        int texCoordLast = texCoordLen - 1;
        sb.Append (", texCoords: [ ");
        for (int i = 0; i < texCoordLen; ++i)
        {
            sb.Append (this.texCoords[i].ToString (places));
            if (i < texCoordLast)
            {
                sb.Append (", ");
            }
        }
        sb.Append (" ] }");

        return sb.ToString ( );
    }

    public static Mesh2 Polygon (in int sectors, in PolyType poly, in Mesh2 target)
    {
        target.name = "Polygon";

        int seg = sectors < 3 ? 3 : sectors;
        int newLen = poly == PolyType.Ngon ? seg : poly == PolyType.Quad ?
            seg + seg + 1 : seg + 1;
        int fLen = poly == PolyType.Ngon ? 1 : seg;
        float toTheta = Utils.Tau / seg;

        Vec2[ ] vs = target.coords = Vec2.Resize (target.coords, newLen);
        Vec2[ ] vts = target.texCoords = Vec2.Resize (target.texCoords,
            newLen);
        Loop2[ ] fs = target.loops = Loop2.Resize (target.loops, fLen);

        switch (poly)
        {
            case PolyType.Ngon:

                Loop2 f = fs[0] = new Loop2 (new Index2[seg]);
                Index2[ ] verts = f.Indices;
                for (int i = 0; i < seg; ++i)
                {
                    float theta = i * toTheta;
                    Vec2 v = vs[i] = new Vec2 (
                        0.5f * Utils.Cos (theta),
                        0.5f * Utils.Sin (theta));
                    vts[i] = new Vec2 (
                        v.x + 0.5f,
                        0.5f - v.y);
                    verts[i] = new Index2 (i, i);
                }

                break;

            case PolyType.Quad:

                vs[0] = Vec2.Zero;
                vts[0] = Vec2.UvCenter;

                // Find corners.
                for (int i = 0, j = 1; i < seg; ++i, j += 2)
                {
                    float theta = i * toTheta;
                    Vec2 v = vs[j] = new Vec2 (
                        0.5f * Utils.Cos (theta),
                        0.5f * Utils.Sin (theta));
                    vts[j] = new Vec2 (
                        v.x + 0.5f,
                        0.5f - v.y);
                }

                // Find midpoints.
                int last = newLen - 1;
                for (int i = 0, j = 1, k = 2; i < seg; ++i, j += 2, k += 2)
                {
                    int m = (j + 2) % last;
                    vs[k] = Vec2.Mix (vs[j], vs[m]);
                    vts[k] = Vec2.Mix (vts[j], vts[m]);
                }

                // Find faces.
                for (int i = 0, j = 0; i < seg; ++i, j += 2)
                {
                    int s = 1 + Utils.Mod (j - 1, last);
                    int t = 1 + j % last;
                    int u = 1 + (j + 1) % last;

                    fs[i] = new Loop2 (
                        new Index2 (0, 0),
                        new Index2 (s, s),
                        new Index2 (t, t),
                        new Index2 (u, u));
                }

                break;

            case PolyType.Tri:
            default:

                vs[0] = Vec2.Zero;
                vts[0] = Vec2.UvCenter;

                for (int i = 0, j = 1; i < seg; ++i, ++j)
                {
                    float theta = i * toTheta;
                    int k = 1 + j % seg;

                    Vec2 v = vs[j] = new Vec2 (
                        0.5f * Utils.Cos (theta),
                        0.5f * Utils.Sin (theta));
                    vts[j] = new Vec2 (
                        v.x + 0.5f,
                        0.5f - v.y);

                    fs[i] = new Loop2 (
                        new Index2 (0, 0),
                        new Index2 (j, j),
                        new Index2 (k, k));
                }

                break;
        }

        return target;
    }

    public static Mesh2 Triangulate (in Mesh2 source, in Mesh2 target)
    {
        Loop2[ ] loopsSrc = source.loops;
        Vec2[ ] vsSrc = source.coords;
        Vec2[ ] vtsSrc = source.texCoords;

        // Cannot anticipate how many loops in the source mesh will not be
        // triangles, so this is an expanding list.
        List<Loop2> loopsTrg = new List<Loop2> ( );

        int loopSrcLen = loopsSrc.Length;
        for (int i = 0; i < loopSrcLen; ++i)
        {
            Loop2 fSrc = loopsSrc[i];
            int fSrcLen = fSrc.Length;

            // If face loop is not a triangle, then split.
            if (fSrcLen > 3)
            {

                // Find last non-adjacent index. For index m, neither m + 1 nor
                // m - 1, so where m = 0, the last non-adjacent would be
                // arr.Length - 2.
                Index2 vert0 = fSrc[0];
                int lastNonAdj = fSrcLen - 2;
                for (int m = 0; m < lastNonAdj; ++m)
                {
                    // Find next two vertices.
                    Index2 vert1 = fSrc[1 + m];
                    Index2 vert2 = fSrc[2 + m];

                    // Create a new triangle which connects them.
                    Loop2 loopTrg = new Loop2 (vert0, vert1, vert2);
                    loopsTrg.Add (loopTrg);
                }
            }
            else
            {
                loopsTrg.Add (fSrc);
            }
        }

        // If source and target are not the same mesh, then copy mesh data from
        // source to target.
        if (!Object.ReferenceEquals (source, target))
        {
            int vsLen = vsSrc.Length;
            target.coords = new Vec2[vsLen];
            System.Array.Copy (vsSrc, target.coords, vsLen);

            int vtsLen = vtsSrc.Length;
            target.texCoords = new Vec2[vtsLen];
            System.Array.Copy (vtsSrc, target.texCoords, vtsLen);
        }
        target.loops = loopsTrg.ToArray ( );

        return target;
    }

    public static Mesh2 UniformData (in Mesh2 source, in Mesh2 target)
    {
        Loop2[ ] fsSrc = source.loops;
        Vec2[ ] vsSrc = source.coords;
        Vec2[ ] vtsSrc = source.texCoords;

        int uniformLen = 0;
        int fsSrcLen = fsSrc.Length;
        for (int i = 0; i < fsSrcLen; ++i)
        {
            uniformLen += fsSrc[i].Length;
        }

        Loop2[ ] fsTrg = new Loop2[fsSrcLen];
        Vec2[ ] vsTrg = new Vec2[uniformLen];
        Vec2[ ] vtsTrg = new Vec2[uniformLen];

        for (int k = 0, i = 0; i < fsSrcLen; ++i)
        {
            Index2[ ] fSrc = fsSrc[i].Indices;
            int fLen = fSrc.Length;
            fsTrg[i] = new Loop2 (new Index2[fLen]);
            Index2[ ] fTrg = fsTrg[i].Indices;

            for (int j = 0; j < fLen; ++j, ++k)
            {
                Index2 vertSrc = fSrc[j];
                vsTrg[k] = vsSrc[vertSrc.v];
                vtsTrg[k] = vtsSrc[vertSrc.vt];
                fTrg[j] = new Index2 (k, k);
            }
        }

        target.coords = vsTrg;
        target.texCoords = vtsTrg;
        target.loops = fsTrg;

        return target;
    }
}