using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class Mesh2
{
    protected Vec2[ ] coords;

    protected Loop2[ ] loops;

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

    public Mesh2 (in Loop2[ ] loops, in Vec2[ ] coords, in Vec2[ ] texCoords)
    {
        this.loops = loops;
        this.coords = coords;
        this.texCoords = texCoords;
    }

    public override string ToString ( )
    {
        return this.ToString (1, 4);
    }

    public string ToString (in int padding = 1, in int places = 4)
    {
        return new StringBuilder (2048)
            .Append ("{ loops: ")
            .Append (Loop2.ToString (this.loops, padding))
            .Append (", coords: ")
            .Append (Vec2.ToString (this.coords, places))
            .Append (", texCoords: ")
            .Append (Vec2.ToString (this.texCoords, places))
            .Append (" }")
            .ToString ( );
    }

    public static Mesh2 GridHex (in int rings, in float cellRadius, in float cellMargin, in Mesh2 target)
    {
        int vRings = Utils.Max (1, rings);
        float vRad = Utils.Max (Utils.Epsilon, cellRadius);

        float extent = Utils.Sqrt3 * vRad;
        float halfExt = extent * 0.5f;

        float rad15 = vRad * 1.5f;
        float padRad = Utils.Max (Utils.Epsilon, vRad - cellMargin);
        float halfRad = padRad * 0.5f;
        float radrt32 = padRad * Utils.Sqrt32;

        int iMax = vRings - 1;
        int iMin = -iMax;

        Vec2[ ] vts = target.texCoords = new Vec2[ ]
        {
            new Vec2 (0.5f, 1.0f),
            new Vec2 (0.0669873f, 0.75f),
            new Vec2 (0.0669873f, 0.25f),
            new Vec2 (0.5f, 0.0f),
            new Vec2 (0.9330127f, 0.25f),
            new Vec2 (0.9330127f, 0.75f)
        };

        int fsLen = 1 + iMax * vRings * 3;
        Vec2[ ] vs = target.coords = Vec2.Resize (target.coords, fsLen * 6);
        Loop2[ ] fs = target.loops = Loop2.Resize (target.loops, fsLen);

        int vIdx = 0;
        int fIdx = 0;
        for (int i = iMin; i <= iMax; ++i)
        {
            int jMin = Utils.Max (iMin, iMin - i);
            int jMax = Utils.Min (iMax, iMax - i);
            float iExt = i * extent;

            for (int j = jMin; j <= jMax; ++j)
            {
                float jf = j;
                float x = iExt + jf * halfExt;
                float y = jf * rad15;

                float left = x - radrt32;
                float right = x + radrt32;
                float top = y + halfRad;
                float bottom = y - halfRad;

                vs[vIdx] = new Vec2 (x, y + padRad);
                vs[vIdx + 1] = new Vec2 (left, top);
                vs[vIdx + 2] = new Vec2 (left, bottom);
                vs[vIdx + 3] = new Vec2 (x, y - padRad);
                vs[vIdx + 4] = new Vec2 (right, bottom);
                vs[vIdx + 5] = new Vec2 (right, top);

                fs[fIdx] = new Loop2 (
                    new Index2 (vIdx, 0),
                    new Index2 (vIdx + 1, 1),
                    new Index2 (vIdx + 2, 2),
                    new Index2 (vIdx + 3, 3),
                    new Index2 (vIdx + 4, 4),
                    new Index2 (vIdx + 5, 5));

                ++fIdx;
                vIdx += 6;
            }
        }

        return target;
    }

    public static Mesh2 Polygon (in int sectors, in float radius, float rotation, in PolyType poly, in Mesh2 target)
    {
        int seg = Utils.Max (3, sectors);
        int newLen = poly == PolyType.Ngon ? seg : poly == PolyType.Quad ?
            seg + seg + 1 : seg + 1;
        float rad = Utils.Max (Utils.Epsilon, radius);
        float offset = Utils.ModRadians (rotation);
        float toTheta = Utils.Tau / seg;

        Vec2[ ] vs = target.coords = Vec2.Resize (target.coords, newLen);
        Vec2[ ] vts = target.texCoords = Vec2.Resize (target.texCoords,
            newLen);

        switch (poly)
        {
            case PolyType.Ngon:

                target.loops = Loop2.Resize (target.loops, 1);
                target.loops[0] = new Loop2 (new Index2[seg]);

                for (int i = 0; i < seg; ++i)
                {
                    float theta = offset + i * toTheta;
                    float cosTheta = Utils.Cos (theta);
                    float sinTheta = Utils.Sin (theta);
                    Vec2 v = vs[i] = new Vec2 (
                        rad * cosTheta,
                        rad * sinTheta);
                    vts[i] = new Vec2 (
                        cosTheta * 0.5f + 0.5f,
                        0.5f - sinTheta * 0.5f);
                    target.loops[0][i] = new Index2 (i, i);
                }

                break;

            case PolyType.Quad:

                target.loops = Loop2.Resize (target.loops, seg);

                vs[0] = Vec2.Zero;
                vts[0] = Vec2.UvCenter;

                // Find corners.
                for (int i = 0, j = 1; i < seg; ++i, j += 2)
                {
                    float theta = offset + i * toTheta;
                    float cosTheta = Utils.Cos (theta);
                    float sinTheta = Utils.Sin (theta);
                    vs[j] = new Vec2 (
                        rad * cosTheta,
                        rad * sinTheta);
                    vts[j] = new Vec2 (
                        cosTheta * 0.5f + 0.5f,
                        0.5f - sinTheta * 0.5f);
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

                    target.loops[i] = new Loop2 (
                        new Index2 (0, 0),
                        new Index2 (s, s),
                        new Index2 (t, t),
                        new Index2 (u, u));
                }

                break;

            case PolyType.Tri:
            default:

                target.loops = Loop2.Resize (target.loops, seg);

                vs[0] = Vec2.Zero;
                vts[0] = Vec2.UvCenter;

                for (int i = 0, j = 1; i < seg; ++i, ++j)
                {
                    int k = 1 + j % seg;
                    float theta = i * toTheta;
                    float cosTheta = Utils.Cos (theta);
                    float sinTheta = Utils.Sin (theta);
                    vs[j] = new Vec2 (
                        rad * cosTheta,
                        rad * sinTheta);
                    vts[j] = new Vec2 (
                        cosTheta * 0.5f + 0.5f,
                        0.5f - sinTheta * 0.5f);

                    target.loops[i] = new Loop2 (
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