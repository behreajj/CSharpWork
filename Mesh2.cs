using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Organizes data needed to draw a two dimensional shape with vertices and
/// faces.
/// </summary>
public class Mesh2
{
    /// <summary>
    /// An array of coordinates.
    /// </summary>
    protected Vec2[ ] coords;

    /// <summary>
    /// Loops that describe the indices which reference the coordinates and
    /// texture coordinates to compose a face.
    /// </summary>
    protected Loop2[ ] loops;

    /// <summary>
    /// The texture (UV) coordinates that describe how an image is mapped onto
    /// the mesh. Typically in the range [0.0, 1.0] .
    /// </summary>
    protected Vec2[ ] texCoords;

    /// <summary>
    /// An array of coordinates.
    /// </summary>
    /// <value>coordinates</value>
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

    /// <summary>
    /// Loops that describe the indices which reference the coordinates and
    /// texture coordinates to compose a face.
    /// </summary>
    /// <value>loops</value>
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

    /// <summary>
    /// The texture (UV) coordinates that describe how an image is mapped onto
    /// the mesh. Typically in the range [0.0, 1.0] .
    /// </summary>
    /// <value>texture coordinates</value>
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

    /// <summary>
    /// The default constructor.
    /// </summary>
    public Mesh2 ( ) { }

    /// <summary>
    /// Constructs a mesh from data.
    /// </summary>
    /// <param name="loops">loops</param>
    /// <param name="coords">coordinates</param>
    /// <param name="texCoords">texture coordinates</param>
    public Mesh2 (in Loop2[ ] loops, in Vec2[ ] coords, in Vec2[ ] texCoords)
    {
        this.loops = loops;
        this.coords = coords;
        this.texCoords = texCoords;
    }

    /// <summary>
    /// Copies a source mesh to a new mesh.
    /// </summary>
    /// <param name="source">source mesh</param>
    public Mesh2 (in Mesh2 source)
    {
        this.Set (source);
    }

    /// <summary>
    /// Returns a string representation of this mesh.
    /// </summary>
    /// <returns>the string</returns>
    public override string ToString ( )
    {
        return this.ToString (1, 4);
    }

    /// <summary>
    /// Removes elements from the coordinate, texture coordinate and normal
    /// arrays of the mesh which are not visited by the face indices.
    /// </summary>
    /// <returns>mesh</returns>
    public Mesh2 Clean ( )
    {
        // TODO: Test.

        /* Transfer arrays to dictionaries where the face index is the key. */
        Dictionary<int, Vec2> usedCoords = new Dictionary<int, Vec2> ( );
        Dictionary<int, Vec2> usedTexCoords = new Dictionary<int, Vec2> ( );

        /*
         * Visit all data arrays with the faces array. Any data not used by any
         * face will be left out.
         */
        int facesLen = this.loops.Length;
        for (int i = 0; i < facesLen; ++i)
        {
            Loop2 loop = this.loops[i];
            Index2[ ] verts = loop.Indices;
            int vertsLen = verts.Length;
            for (int j = 0; j < vertsLen; ++j)
            {
                /* The dictionary should ignore repeated visitations. */
                Index2 vert = verts[j];
                usedCoords[vert.v] = this.coords[vert.v];
                usedTexCoords[vert.vt] = this.texCoords[vert.vt];
            }
        }

        /* Use a sorted set to filter out similar vectors. */
        SortQuantized2 v2Cmp = new SortQuantized2 ( );
        SortedSet<Vec2> coordsSet = new SortedSet<Vec2> (v2Cmp);
        SortedSet<Vec2> texCoordsSet = new SortedSet<Vec2> (v2Cmp);

        coordsSet.UnionWith (usedCoords.Values);
        texCoordsSet.UnionWith (usedTexCoords.Values);

        /* Dictionary's keys are no longer needed; just values. */
        Vec2[ ] newCoords = new Vec2[coordsSet.Count];
        Vec2[ ] newTexCoords = new Vec2[texCoordsSet.Count];

        /* Convert from sorted set to arrays. */
        coordsSet.CopyTo (newCoords);
        texCoordsSet.CopyTo (newTexCoords);

        for (int i = 0; i < facesLen; ++i)
        {
            Loop2 loop = this.loops[i];
            Index2[ ] verts = loop.Indices;
            int vertsLen = verts.Length;
            for (int j = 0; j < vertsLen; ++j)
            {
                /*
                 * Find index of vector in new array by using indexed value from old
                 * array as a reference.
                 */
                Index2 oldVert = verts[j];
                verts[j] = new Index2 (
                    Array.BinarySearch<Vec2> (newCoords, this.coords[oldVert.v], v2Cmp),
                    Array.BinarySearch<Vec2> (newTexCoords, this.texCoords[oldVert.vt], v2Cmp));
            }
        }

        /* Replace old arrays with the new. */
        this.coords = newCoords;
        this.texCoords = newTexCoords;

        /* Sort faces by center. */
        Array.Sort (this.loops, new SortLoops2 (this.coords));

        return this;
    }

    /// <summary>
    /// Removes a given number of face indices from this mesh beginning at an
    /// index. Does not remove any data associated with the indices.
    /// </summary>
    /// <param name="faceIndex">index</param>
    /// <param name="deletions">removal count</param>
    /// <returns>mesh</returns>
    public Mesh2 DeleteFaces (in int faceIndex, in int deletions = 1)
    {
        int aLen = this.loops.Length;
        int valIdx = Utils.Mod (faceIndex, aLen);
        int valDel = Utils.Clamp (deletions, 0, aLen - valIdx);
        int bLen = aLen - valDel;
        Loop2[ ] result = new Loop2[bLen];
        System.Array.Copy (this.loops, 0, result, 0, valIdx);
        System.Array.Copy (this.loops, valIdx + valDel, result, valIdx, bLen - valIdx);
        this.loops = result;

        return this;
    }

    /// <summary>
    /// Gets a vertex from the mesh.
    /// </summary>
    /// <param name="faceIndex">face index</param>
    /// <param name="vertIndex">vertex index</param>
    /// <returns>vertex</returns>
    public Vert2 GetVertex (in int faceIndex, in int vertIndex)
    {
        Index2 index = this.loops[Utils.Mod (faceIndex, this.loops.Length)][vertIndex];
        return new Vert2 (
            this.coords[index.v],
            this.texCoords[index.vt]);
    }

    /// <summary>
    /// Gets an array of vertices from the mesh.
    /// </summary>
    /// <returns>vertices</returns>
    public Vert2[ ] GetVertices ( )
    {
        int len0 = this.loops.Length;
        SortedSet<Vert2> result = new SortedSet<Vert2> ( );
        for (int i = 0; i < len0; ++i)
        {
            Loop2 loop = this.loops[i];
            Index2[ ] indices = loop.Indices;
            int len1 = indices.Length;
            for (int j = 0; j < len1; ++j)
            {
                Index2 vert = indices[j];
                result.Add (new Vert2 (
                    this.coords[vert.v],
                    this.texCoords[vert.vt]));
            }
        }

        Vert2[ ] arr = new Vert2[result.Count];
        result.CopyTo (arr);
        return arr;
    }

    public Mesh2 Set (in Mesh2 source)
    {
        // TODO: Test

        int vsLen = source.coords.Length;
        this.coords = new Vec2[vsLen];
        System.Array.Copy (source.coords, this.coords, vsLen);

        int vtsLen = source.texCoords.Length;
        this.texCoords = new Vec2[vtsLen];
        System.Array.Copy (source.texCoords, this.texCoords, vtsLen);

        int loopsLen = source.loops.Length;
        this.loops = new Loop2[loopsLen];
        for (int i = 0; i < loopsLen; ++i)
        {
            Index2[ ] sourceIndices = source.loops[i].Indices;
            int loopLen = sourceIndices.Length;
            Index2[ ] targetIndices = new Index2[loopLen];
            System.Array.Copy (sourceIndices, targetIndices, loopLen);
            this.loops[i] = new Loop2 (targetIndices);
        }

        return this;
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

    public Mesh2 InsetFace (in int faceIndex = 0, in float fac = 0.5f)
    {
        if (fac <= 0.0f) { return this; }
        if (fac >= 1.0f)
        {
            // TODO: Implement subdivFaceFan.
            return this;
        }

        int loopsLen = this.loops.Length;
        int i = Utils.Mod (faceIndex, loopsLen);
        Loop2 loop = this.loops[i];
        Index2[ ] indices = loop.Indices;
        int loopLen = indices.Length;

        int vsOldLen = this.coords.Length;
        int vtsOldLen = this.texCoords.Length;
        Loop2[ ] loopsNew = new Loop2[loopLen + 1];
        Loop2 centerLoop = loopsNew[loopLen] = new Loop2 (loopLen);

        /* Sum centers. */
        Vec2 vCenter = new Vec2 ( );
        Vec2 vtCenter = new Vec2 ( );
        for (int j = 0; j < loopLen; ++j)
        {
            Index2 curr = indices[j];
            vCenter += this.coords[curr.v];
            vtCenter += this.texCoords[curr.vt];
        }

        /* Find average. */
        if (loopLen > 0)
        {
            float flInv = 1.0f / loopLen;
            vCenter *= flInv;
            vtCenter *= flInv;
        }

        Vec2[ ] vsNew = new Vec2[loopLen];
        Vec2[ ] vtsNew = new Vec2[loopLen];

        /* Find new corners. */
        for (int j = 0; j < loopLen; ++j)
        {
            int k = (j + 1) % loopLen;

            Index2 vertCurr = indices[j];
            Index2 vertNext = indices[k];

            int vCornerIdx = vertCurr.v;
            int vtCornerIdx = vertCurr.vt;

            vsNew[j] = Vec2.Mix (this.coords[vCornerIdx], vCenter, fac);
            vtsNew[j] = Vec2.Mix (this.texCoords[vtCornerIdx], vtCenter, fac);

            int vSubdivIdx = vsOldLen + j;
            int vtSubdivIdx = vtsOldLen + j;

            // TODO: This should be a set, since Loop2 is a class.
            loopsNew[j] = new Loop2 (
                new Index2 (vCornerIdx, vtCornerIdx),
                new Index2 (vertNext.v, vertNext.vt),
                new Index2 (vsOldLen + k, vtsOldLen + k),
                new Index2 (vSubdivIdx, vtSubdivIdx));

            centerLoop[j] = new Index2 (vSubdivIdx, vtSubdivIdx);
        }

        this.coords = Vec2.Concat (this.coords, vsNew);
        this.texCoords = Vec2.Concat (this.texCoords, vtsNew);
        this.loops = Loop2.Splice (this.loops, i, 1, loopsNew);

        return this;
    }

    public static Mesh2 Arc ( //
        in Mesh2 target, //
        in int sectors = 32, //
        in float radius = 0.5f, //
        float oculus = 0.5f, //
        in float startAngle = 0.0f, //
        in float stopAngle = Utils.Pi, //
        in PolyType poly = PolyType.Tri)
    {
        float a1 = Utils.Mod1 (startAngle * Utils.OneTau);
        float b1 = Utils.Mod1 (stopAngle * Utils.OneTau);
        float arcLen1 = Utils.Mod1 (b1 - a1);

        /* 1.0 / 720.0 = 0.001388889f */
        if (arcLen1 < 0.00139f)
        {
            Mesh2.Polygon (
                target: target,
                sectors: sectors,
                radius: radius,
                rotation: startAngle,
                poly: PolyType.Ngon);
            target.InsetFace (0, 1.0f - oculus);
            target.DeleteFaces (-1, 1);
            return target;
        }

        int sctCount = Utils.Ceil (1.0f + Utils.Max (3.0f, sectors) * arcLen1);
        int sctCount2 = sctCount + sctCount;

        Vec2[ ] vs = target.coords = Vec2.Resize (target.coords, sctCount2);
        Vec2[ ] vts = target.texCoords = Vec2.Resize (target.texCoords,
            sctCount2);

        float rad = Utils.Max (Utils.Epsilon, radius);
        float oculFac = Utils.Clamp (oculus, Utils.Epsilon, 1.0f - Utils.Epsilon);
        float oculRad = oculFac * rad;
        float oculRadVt = oculFac * 0.5f;

        float toStep = 1.0f / (sctCount - 1.0f);
        float origAngle = Utils.Tau * a1;
        float destAngle = Utils.Tau * (a1 + arcLen1);

        for (int k = 0, i = 0, j = 1; k < sctCount; ++k, i += 2, j += 2)
        {
            float step = k * toStep;
            float theta = Utils.Mix (origAngle, destAngle, step);
            float cosTheta = Utils.Cos (theta);
            float sinTheta = Utils.Sin (theta);

            vs[i] = new Vec2 (
                cosTheta * rad,
                sinTheta * rad);

            vs[j] = new Vec2 (
                cosTheta * oculRad,
                sinTheta * oculRad);

            vts[i] = new Vec2 (
                cosTheta * 0.5f + 0.5f,
                0.5f - sinTheta * 0.5f);

            vts[j] = new Vec2 (
                cosTheta * oculRadVt + 0.5f,
                0.5f - sinTheta * oculRadVt);
        }

        int len;
        switch (poly)
        {
            case PolyType.Ngon:

                len = sctCount2;
                int last = len - 1;
                target.loops = new Loop2[ ] { new Loop2 (len) };
                Loop2 indices = target.loops[0];

                for (int i = 0, j = 0; i < sctCount; ++i, j += 2)
                {
                    int k = sctCount + i;
                    int m = last - j;
                    indices[i] = new Index2 (j, j);
                    indices[k] = new Index2 (m, m);
                }

                break;

            case PolyType.Quad:

                len = sctCount - 1;
                target.loops = Loop2.Resize (target.loops, len);

                for (int k = 0, i = 0, j = 1; k < len; ++k, i += 2, j += 2)
                {
                    int m = i + 2;
                    int n = j + 2;

                    // TODO: This should be a set, since Loop2 is a class.
                    target.loops[k] = new Loop2 (
                        new Index2 (i, i),
                        new Index2 (m, m),
                        new Index2 (n, n),
                        new Index2 (j, j));
                }

                break;

            case PolyType.Tri:
            default:

                len = sctCount2 - 2;
                target.loops = Loop2.Resize (target.loops, len);

                for (int i = 0, j = 1; i < len; i += 2, j += 2)
                {
                    int m = i + 2;
                    int n = j + 2;

                    // TODO: This should be a set, since Loop2 is a class.
                    target.loops[i] = new Loop2 (
                        new Index2 (i, i),
                        new Index2 (m, m),
                        new Index2 (j, j));

                    target.loops[j] = new Loop2 (
                        new Index2 (m, m),
                        new Index2 (n, n),
                        new Index2 (j, j));
                }

                break;
        }

        return target;

    }

    /// <summary>
    /// Generates a grid of hexagons arranged in rings around a central cell.
    /// The number of cells follows the formula n = 1 + (rings - 1) * 3 * rings,
    /// meaning 1 ring: 1 cell; 2 rings: 7 cells; 3 rings: 19 cells; 4 rings: 37
    /// cells; and so on. See
    /// https://www.redblobgames.com/grids/hexagons/implementation.html , Red
    /// Blob Games' Implementation of Hex Grids</a> .
    /// </summary>
    /// <param name="target">output mesh</param>
    /// <param name="rings">number of rings</param>
    /// <param name="cellRadius">cell radius</param>
    /// <param name="cellMargin">margin between cells</param>
    /// <returns>hexagon grid</returns>
    public static Mesh2 GridHex ( //
        in Mesh2 target, //
        in int rings = 4, //
        in float cellRadius = 0.5f, //
        in float cellMargin = 0.0325f)
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

        target.texCoords = new Vec2[ ]
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

                // TODO: This should be a set, since Loop2 is a class.
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

    /// <summary>
    /// Creates a plane subdivided into either triangles or quadrilaterals,
    /// depending on the polygon type. Useful for meshes which later will be
    /// augmented by noise or height maps to simulate terrain.
    /// </summary>
    /// <param name="cols">number of columns</param>
    /// <param name="rows">number of rows</param>
    /// <param name="poly">polygon type</param>
    /// <param name="target">output mesh</param>
    /// <returns>plane</returns>
    public static Mesh2 Plane ( //
        in Mesh2 target, //
        in int cols = 3, //
        in int rows = 3, //
        in PolyType poly = PolyType.Tri)
    {
        int rval = Utils.Max (1, rows);
        int cval = Utils.Max (1, cols);

        int rval1 = rval + 1;
        int cval1 = cval + 1;

        float iToStep = 1.0f / rval;
        float jToStep = 1.0f / cval;

        Vec2[ ] vs = target.coords = Vec2.Resize (target.coords, rval1 * cval1);
        Vec2[ ] vts = target.texCoords = Vec2.Resize (target.texCoords, vs.Length);
        int flen = rval * cval;

        /* Calculate x values in separate loop. */
        float[ ] xs = new float[cval1];
        float[ ] us = new float[cval1];
        for (int j = 0; j < cval1; ++j)
        {
            float xPrc = j * jToStep;
            xs[j] = xPrc - 0.5f;
            us[j] = xPrc;
        }

        for (int k = 0, i = 0; i < rval1; ++i)
        {
            float yPrc = i * iToStep;
            float y = yPrc - 0.5f;
            float v = 1.0f - yPrc;

            for (int j = 0; j < cval1; ++j, ++k)
            {
                vs[k] = new Vec2 (xs[j], y);
                vts[k] = new Vec2 (us[j], v);
            }
        }

        switch (poly)
        {
            case PolyType.Ngon:
            case PolyType.Quad:

                target.loops = Loop2.Resize (target.loops, flen);

                // TODO: Flatten array.
                for (int k = 0, i = 0; i < rval; ++i)
                {
                    int noff0 = i * cval1;
                    int noff1 = noff0 + cval1;

                    for (int j = 0; j < cval; ++j, ++k)
                    {
                        int n00 = noff0 + j;
                        int n10 = n00 + 1;
                        int n01 = noff1 + j;
                        int n11 = n01 + 1;

                        Loop2.Quad (
                            new Index2 (n00, n00),
                            new Index2 (n10, n10),
                            new Index2 (n11, n11),
                            new Index2 (n01, n01),
                            target.loops[k]);
                    }
                }

                break;

            case PolyType.Tri:
            default:

                target.loops = Loop2.Resize (target.loops, flen + flen);

                // TODO: Flatten array.
                for (int k = 0, i = 0; i < rval; ++i)
                {
                    int noff0 = i * cval1;
                    int noff1 = noff0 + cval1;

                    for (int j = 0; j < cval; ++j, k += 2)
                    {
                        int n00 = noff0 + j;
                        int n10 = n00 + 1;
                        int n01 = noff1 + j;
                        int n11 = n01 + 1;

                        Loop2.Tri (
                            new Index2 (n00, n00),
                            new Index2 (n10, n10),
                            new Index2 (n11, n11),
                            target.loops[k]);

                        Loop2.Tri (
                            new Index2 (n11, n11),
                            new Index2 (n01, n01),
                            new Index2 (n00, n00),
                            target.loops[k + 1]);
                    }
                }

                break;
        }

        return target;
    }

    public static Mesh2 Polygon ( //
        in Mesh2 target, //
        in int sectors = 32, //
        in float radius = 0.5f, //
        float rotation = Utils.HalfPi, //
        in PolyType poly = PolyType.Tri)
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
                    vs[i] = new Vec2 (
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

                /* Find corners. */
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

                /* Find midpoints. */
                int last = newLen - 1;
                for (int i = 0, j = 1, k = 2; i < seg; ++i, j += 2, k += 2)
                {
                    int m = (j + 2) % last;
                    vs[k] = Vec2.Mix (vs[j], vs[m]);
                    vts[k] = Vec2.Mix (vts[j], vts[m]);
                }

                /* Find faces. */
                for (int i = 0, j = 0; i < seg; ++i, j += 2)
                {
                    int s = 1 + Utils.Mod (j - 1, last);
                    int t = 1 + j % last;
                    int u = 1 + (j + 1) % last;

                    Loop2.Quad (
                        new Index2 (0, 0),
                        new Index2 (s, s),
                        new Index2 (t, t),
                        new Index2 (u, u),
                        target.loops[i]);
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

                    // TODO: This should be a set, since Loop2 is a class.
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

    /// <summary>
    /// Restructures the mesh so that each face index refers to unique data,
    /// indifferent to redundancies. As a consequence, coordinate and texture
    /// coordinate arrays are of equal length and face indices are easier to
    /// read and understand. Useful for making a mesh similar to those in Unity
    /// or p5. Similar to 'ripping' vertices or 'tearing' edges in Blender.
    /// </summary>
    /// <param name="source">source mesh</param>
    /// <param name="target">target mesh</param>
    /// <returns>uniform mesh</returns>
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