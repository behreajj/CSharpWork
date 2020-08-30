using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Organizes data needed to draw a three dimensional shape with verties and
/// faces.
/// </summary>
public class Mesh3
{
    /// <summary>
    /// An array of coordinates.
    /// </summary>
    protected Vec3[ ] coords;

    /// <summary>
    /// Loops that describe the indices which reference the coordinates, texture
    /// coordinates and normals that compose a face.
    /// </summary>
    protected Loop3[ ] loops;

    /// <summary>
    /// The mesh's name.
    /// </summary>
    protected String name = "Mesh3";

    /// <summary>
    /// An array of normals to indicate how light will bounce off the mesh's
    /// surface.
    /// </summary>
    protected Vec3[ ] normals;

    /// <summary>
    /// The texture (UV) coordinates that describe how an image is mapped onto
    /// the mesh. Typically in the range [0.0, 1.0] .
    /// </summary>
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

    public Loop3[ ] Loops
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

    /// <summary>
    /// The default constructor.
    /// </summary>
    public Mesh3 ( ) { }

    /// <summary>
    /// Constructs a mesh with a name.
    /// </summary>
    /// <param name="name"></param>
    public Mesh3 (in String name)
    {
        this.name = name;
    }

    /// <summary>
    /// Constructs a mesh from data.
    /// </summary>
    /// <param name="loops">loops</param>
    /// <param name="coords">coordinates</param>
    /// <param name="texCoords">texture coordinates</param>
    /// <param name="normals">normals</param>
    public Mesh3 (in Loop3[ ] loops, in Vec3[ ] coords, in Vec2[ ] texCoords, in Vec3[ ] normals)
    {
        this.loops = loops;
        this.coords = coords;
        this.texCoords = texCoords;
        this.normals = normals;
    }

    /// <summary>
    /// Constructs a mesh from data.
    /// </summary>
    /// <param name="name">name</param>
    /// <param name="loops">loops</param>
    /// <param name="coords">coordinates</param>
    /// <param name="texCoords">texture coordinates</param>
    /// <param name="normals">normals</param>
    public Mesh3 (in String name, in Loop3[ ] loops, in Vec3[ ] coords, in Vec2[ ] texCoords, in Vec3[ ] normals)
    {
        this.name = name;
        this.loops = loops;
        this.coords = coords;
        this.texCoords = texCoords;
        this.normals = normals;
    }

    public override string ToString ( )
    {
        return this.ToString (4);
    }

    public Mesh3 Clean ( )
    {
        Dictionary<int, Vec3> usedCoords = new Dictionary<int, Vec3> ( );
        Dictionary<int, Vec2> usedTexCoords = new Dictionary<int, Vec2> ( );
        Dictionary<int, Vec3> usedNormals = new Dictionary<int, Vec3> ( );

        int facesLen = this.loops.Length;
        for (int i = 0; i < facesLen; ++i)
        {
            Loop3 loop = this.loops[i];
            Index3[ ] verts = loop.Indices;
            int vertsLen = verts.Length;
            for (int j = 0; j < vertsLen; ++j)
            {
                Index3 vert = verts[j];
                int vIdx = vert.v;
                int vtIdx = vert.vt;
                int vnIdx = vert.vn;

                usedCoords[vIdx] = this.coords[vIdx];
                usedTexCoords[vtIdx] = this.texCoords[vtIdx];
                usedNormals[vnIdx] = this.normals[vnIdx];
            }
        }

        SortQuantized3 v3Cmp = new SortQuantized3 ( );
        SortQuantized2 v2Cmp = new SortQuantized2 ( );

        SortedSet<Vec3> coordsSet = new SortedSet<Vec3> (v3Cmp);
        SortedSet<Vec2> texCoordsSet = new SortedSet<Vec2> (v2Cmp);
        SortedSet<Vec3> normalsSet = new SortedSet<Vec3> (v3Cmp);

        coordsSet.UnionWith (usedCoords.Values);
        texCoordsSet.UnionWith (usedTexCoords.Values);
        normalsSet.UnionWith (usedNormals.Values);

        Vec3[ ] newCoords = new Vec3[coordsSet.Count];
        Vec2[ ] newTexCoords = new Vec2[texCoordsSet.Count];
        Vec3[ ] newNormals = new Vec3[normalsSet.Count];

        coordsSet.CopyTo (newCoords);
        texCoordsSet.CopyTo (newTexCoords);
        normalsSet.CopyTo (newNormals);

        for (int i = 0; i < facesLen; ++i)
        {
            Loop3 loop = this.loops[i];
            Index3[ ] verts = loop.Indices;
            int vertsLen = verts.Length;
            for (int j = 0; j < vertsLen; ++j)
            {
                Index3 oldVert = verts[j];
                Index3 newVert = new Index3 (
                    Array.BinarySearch<Vec3> (newCoords, this.coords[oldVert.v], v3Cmp),
                    Array.BinarySearch<Vec2> (newTexCoords, this.texCoords[oldVert.vt], v2Cmp),
                    Array.BinarySearch<Vec3> (newNormals, this.normals[oldVert.vn], v3Cmp));
                verts[j] = newVert;
            }
        }

        this.coords = newCoords;
        this.texCoords = newTexCoords;
        this.normals = newNormals;

        Array.Sort (this.loops, new SortLoops3 (this.coords));

        return this;
    }

    /// <summary>
    /// Subdivides a convex face by calculating its center, then connecting its
    /// vertices to the center. This generates a triangle for the number of
    /// edges in the face.
    /// </summary>
    /// <param name="faceIdx">the face index</param>
    public (Loop3[ ] loopsNew, Vec3 vNew, Vec2 vtNew, Vec3 vnNew) SubdivFaceFan (in int faceIdx)
    {
        int facesLen = this.loops.Length;
        int i = Utils.Mod (faceIdx, facesLen);
        Index3[ ] face = this.loops[i].Indices;
        int faceLen = face.Length;

        Loop3[ ] fsNew = new Loop3[faceLen];
        Vec3 vCenter = new Vec3 ( );
        Vec2 vtCenter = new Vec2 ( );
        Vec3 vnCenter = new Vec3 ( );

        int vCenterIdx = this.coords.Length;
        int vtCenterIdx = this.texCoords.Length;
        int vnCenterIdx = this.normals.Length;

        for (int j = 0; j < faceLen; ++j)
        {
            int k = (j + 1) % faceLen;

            Index3 vertCurr = face[j];
            Index3 vertNext = face[k];

            int vCurrIdx = vertCurr.v;
            int vtCurrIdx = vertCurr.vt;
            int vnCurrIdx = vertCurr.vn;

            Vec3 vCurr = this.coords[vCurrIdx];
            Vec2 vtCurr = this.texCoords[vtCurrIdx];
            Vec3 vnCurr = this.normals[vnCurrIdx];

            vCenter += vCurr;
            vtCenter += vtCurr;
            vnCenter += vnCurr;

            fsNew[j] = new Loop3 (
                new Index3 (vCenterIdx, vtCenterIdx, vnCenterIdx),
                new Index3 (vCurrIdx, vtCurrIdx, vnCurrIdx),
                new Index3 (vertNext.v, vertNext.vt, vertNext.vn));
        }

        if (faceLen > 0)
        {
            float flInv = 1.0f / faceLen;
            vCenter *= flInv;
            vtCenter *= flInv;
            vnCenter = Vec3.Normalize (vnCenter);
        }

        this.coords = Vec3.Append (this.coords, vCenter);
        this.texCoords = Vec2.Append (this.texCoords, vtCenter);
        this.normals = Vec3.Append (this.normals, vnCenter);
        this.loops = Loop3.Splice (this.loops, i, 1, fsNew);

        return (loopsNew: fsNew,
            vNew: vCenter,
            vtNew: vtCenter,
            vnNew: vnCenter);
    }

    /// <summary>
    /// Subdivides a convex face by calculating its center, subdividing each of
    /// its edges with one cut to create a midpoint, then connecting the
    /// midpoints to the center. This generates a quadrilateral for the number
    /// of edges in the face. Returns a tuple containing the new data created.
    /// </summary>
    /// <param name="faceIdx">face index</param>
    /// <returns>the tuple</returns>
    public (Loop3[ ] loopsNew, Vec3[ ] vsNew, Vec2[ ] vtsNew, Vec3[ ] vnsNew) SubdivFaceCenter (in int faceIdx)
    {
        int facesLen = this.loops.Length;
        int i = Utils.Mod (faceIdx, facesLen);
        Index3[ ] face = this.loops[i].Indices;
        int faceLen = face.Length;

        int vsOldLen = this.coords.Length;
        int vtsOldLen = this.texCoords.Length;
        int vnsOldLen = this.normals.Length;

        Vec3[ ] vsNew = new Vec3[faceLen + 1];
        Vec2[ ] vtsNew = new Vec2[vsNew.Length];
        Vec3[ ] vnsNew = new Vec3[vsNew.Length];
        Loop3[ ] fsNew = new Loop3[faceLen];

        int vCenterIdx = vsOldLen + faceLen;
        int vtCenterIdx = vtsOldLen + faceLen;
        int vnCenterIdx = vnsOldLen + faceLen;

        Vec3 vCenter = new Vec3 ( );
        Vec2 vtCenter = new Vec2 ( );
        Vec3 vnCenter = new Vec3 ( );

        for (int j = 0; j < faceLen; ++j)
        {
            int k = (j + 1) % faceLen;
            Index3 vertCurr = face[j];
            Index3 vertNext = face[k];

            Vec3 vCurr = this.coords[vertCurr.v];
            Vec2 vtCurr = this.texCoords[vertCurr.vt];
            Vec3 vnCurr = this.normals[vertCurr.vn];

            vCenter += vCurr;
            vtCenter += vtCurr;
            vnCenter += vnCurr;

            int vNextIdx = vertNext.v;
            int vtNextIdx = vertNext.vt;
            int vnNextIdx = vertNext.vn;

            vsNew[j] = Vec3.Mix (vCurr, this.coords[vNextIdx]);
            vtsNew[j] = Vec2.Mix (vtCurr, this.texCoords[vtNextIdx]);
            vnsNew[j] = Vec3.Normalize (vnCurr + this.normals[vnNextIdx]);

            fsNew[j] = new Loop3 (
                new Index3 (vCenterIdx, vtCenterIdx, vnCenterIdx),
                new Index3 (vsOldLen + j, vtsOldLen + j, vnsOldLen + j),
                new Index3 (vNextIdx, vtNextIdx, vnNextIdx),
                new Index3 (vsOldLen + k, vtsOldLen + k, vnsOldLen + k));
        }

        if (faceLen > 0)
        {
            float flInv = 1.0f / faceLen;
            vCenter *= flInv;
            vtCenter *= flInv;
            vnCenter = Vec3.Normalize (vnCenter);
        }

        vsNew[faceLen] = vCenter;
        vtsNew[faceLen] = vtCenter;
        vnsNew[faceLen] = vnCenter;

        this.coords = Vec3.Concat (this.coords, vsNew);
        this.texCoords = Vec2.Concat (this.texCoords, vtsNew);
        this.normals = Vec3.Concat (this.normals, vnsNew);
        this.loops = Loop3.Splice (this.loops, i, 1, fsNew);

        return (loopsNew: fsNew,
            vsNew: vsNew,
            vtsNew: vtsNew,
            vnsNew: vnsNew);
    }

    /// <summary>
    /// Subdivides a convex face by cutting each of its edges once to create a
    /// midpoint, then connecting each midpoint. This generates peripheral
    /// triangles and a new central face with the same number of edges as
    /// original. This is best suited to meshes made of triangles. Returns a
    /// tuple containing the new data created.
    /// </summary>
    /// <param name="faceIdx">face index</param>
    /// <returns>the tuple</returns>
    public (Loop3[ ] loopsNew, Vec3[ ] vsNew, Vec2[ ] vtsNew, Vec3[ ] vnsNew) SubdivFaceInscribe (in int faceIdx)
    {
        int facesLen = this.loops.Length;
        int i = Utils.Mod (faceIdx, facesLen);
        Index3[ ] face = this.loops[i].Indices;
        int faceLen = face.Length;

        int vsOldLen = this.coords.Length;
        int vtsOldLen = this.texCoords.Length;
        int vnsOldLen = this.normals.Length;

        Vec3[ ] vsNew = new Vec3[faceLen];
        Vec2[ ] vtsNew = new Vec2[faceLen];
        Vec3[ ] vnsNew = new Vec3[faceLen];
        Loop3[ ] fsNew = new Loop3[faceLen + 1];
        Index3[ ] cfIdcs = new Index3[3];

        for (int j = 0; j < faceLen; ++j)
        {
            int k = (j + 1) % faceLen;
            Index3 vertCurr = face[j];
            Index3 vertNext = face[k];

            int vNextIdx = vertNext.v;
            int vtNextIdx = vertNext.vt;
            int vnNextIdx = vertNext.vn;

            vsNew[j] = Vec3.Mix (
                this.coords[vertCurr.v],
                this.coords[vNextIdx]);
            vtsNew[j] = Vec2.Mix (
                this.texCoords[vertCurr.vt],
                this.texCoords[vtNextIdx]);
            vnsNew[j] = Vec3.Normalize (
                this.normals[vertCurr.vn] +
                this.normals[vnNextIdx]);

            int vSubdivIdx = vsOldLen + j;
            int vtSubdivIdx = vtsOldLen + j;
            int vnSubdivIdx = vnsOldLen + j;

            fsNew[j] = new Loop3 (
                new Index3 (vSubdivIdx, vtSubdivIdx, vnSubdivIdx),
                new Index3 (vNextIdx, vtNextIdx, vnNextIdx),
                new Index3 (vsOldLen + k, vtsOldLen + k, vnsOldLen + k));

            cfIdcs[j] = new Index3 (vSubdivIdx, vtSubdivIdx, vnSubdivIdx);
        }

        // Center face.
        fsNew[faceLen] = new Loop3 (cfIdcs);

        this.coords = Vec3.Concat (this.coords, vsNew);
        this.texCoords = Vec2.Concat (this.texCoords, vtsNew);
        this.normals = Vec3.Concat (this.normals, vnsNew);
        this.loops = Loop3.Splice (this.loops, i, 1, fsNew);

        return (loopsNew: fsNew,
            vsNew: vsNew,
            vtsNew: vtsNew,
            vnsNew: vnsNew);
    }

    /// <summary>
    /// Subdivides all faces in the mesh by a number of iterations. Uses the center method.
    /// </summary>
    /// <param name="itr">iterations</param>
    /// <returns>this mesh</returns>
    public Mesh3 SubdivFacesCenter (in int itr = 1)
    {
        for (int i = 0; i < itr; ++i)
        {
            int k = 0;
            int len = this.loops.Length;
            for (int j = 0; j < len; ++j)
            {
                int vertLen = this.loops[k].Length;
                this.SubdivFaceCenter (k);
                k += vertLen;
            }
        }
        return this;
    }

    /// <summary>
    /// Subdivides all faces in the mesh by a number of iterations. Uses the inscription method.
    /// </summary>
    /// <param name="itr">iterations</param>
    /// <returns>this mesh</returns>
    public Mesh3 SubdivFacesInscribe (in int itr = 1)
    {
        for (int i = 0; i < itr; ++i)
        {
            int k = 0;
            int len = this.loops.Length;
            for (int j = 0; j < len; ++j)
            {
                int vertLen = this.loops[k].Length;
                this.SubdivFaceInscribe (k);
                k += vertLen + 1;
            }
        }
        return this;
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
        sb.Append (" ]");

        // Append normals.
        int normalLen = this.normals.Length;
        int normalLast = normalLen - 1;
        sb.Append (", normals: [ ");
        for (int i = 0; i < normalLen; ++i)
        {
            sb.Append (this.normals[i].ToString (places));
            if (i < normalLast)
            {
                sb.Append (", ");
            }
        }
        sb.Append (" ] }");

        return sb.ToString ( );
    }

    public static implicit operator Mesh3 (in Mesh2 source)
    {
        Loop2[ ] loopsSrc = source.Loops;
        Vec2[ ] vsSrc = source.Coords;
        Vec2[ ] vtsSrc = source.TexCoords;

        int loopsLen = loopsSrc.Length;
        int vsLen = vsSrc.Length;
        int vtsLen = vtsSrc.Length;

        Loop3[ ] loopsTrg = new Loop3[loopsLen];
        Vec3[ ] vsTrg = new Vec3[vsLen];
        Vec2[ ] vtsTrg = new Vec2[vtsLen];
        Vec3[ ] vnsTrg = new Vec3[ ] { Vec3.Up };

        // Promote coordinates.
        for (int i = 0; i < vsLen; ++i)
        {
            vsTrg[i] = vsSrc[i];
        }

        // Copy texture coordinates.
        for (int i = 0; i < vtsLen; ++i)
        {
            vtsTrg[i] = vtsSrc[i];
        }

        // Promote loops.
        for (int i = 0; i < loopsLen; ++i)
        {
            loopsTrg[i] = loopsSrc[i];
        }

        return new Mesh3 (source.Name, loopsTrg, vsTrg, vtsTrg, vnsTrg);
    }

    public static Mesh3 CastToSphere (in Mesh3 source, in float radius, in Mesh3 target)
    {
        float vrad = Utils.Max (Utils.Epsilon, radius);

        Loop3[ ] fsSrc = source.loops;
        Vec3[ ] vsSrc = source.coords;
        int fsSrcLen = fsSrc.Length;
        int vsSrcLen = vsSrc.Length;

        Loop3[ ] fsTrg = new Loop3[fsSrcLen];
        Vec3[ ] vsTrg = new Vec3[vsSrcLen];
        Vec3[ ] vnsTrg = new Vec3[vsSrcLen];

        for (int i = 0; i < vsSrcLen; ++i)
        {
            vnsTrg[i] = Vec3.Normalize (vsSrc[i]);
            vsTrg[i] = vnsTrg[i] * vrad;
        }

        for (int i = 0; i < fsSrcLen; ++i)
        {
            Loop3 loopSrc = fsSrc[i];
            Index3[ ] fSrc = loopSrc.Indices;
            int fSrcLen = fSrc.Length;

            Loop3 loopTrg = fsTrg[i] = new Loop3 (new Index3[fSrcLen]);
            Index3[ ] fTrg = loopTrg.Indices;

            for (int j = 0; j < fSrcLen; ++j)
            {
                Index3 srcVert = fSrc[j];
                fTrg[j] = new Index3 (srcVert.v, srcVert.vt, srcVert.v);
            }
        }

        if (!Object.ReferenceEquals (source, target))
        {
            int vtsLen = source.texCoords.Length;
            target.texCoords = new Vec2[vtsLen];
            System.Array.Copy (source.texCoords, target.texCoords, vtsLen);
        }

        target.loops = fsTrg;
        target.coords = vsTrg;
        target.normals = vnsTrg;

        return target;
    }

    public static Mesh3 Cube (in float size, in PolyType poly, in Mesh3 target)
    {
        target.name = "Cube";

        float vsz = Utils.Max (Utils.Epsilon, size);
        target.coords = new Vec3[ ]
        {
            new Vec3 (-vsz, -vsz, -vsz),
            new Vec3 (-vsz, -vsz, vsz),
            new Vec3 (-vsz, vsz, -vsz),
            new Vec3 (-vsz, vsz, vsz),
            new Vec3 (vsz, -vsz, -vsz),
            new Vec3 (vsz, -vsz, vsz),
            new Vec3 (vsz, vsz, -vsz),
            new Vec3 (vsz, vsz, vsz)
        };

        target.normals = new Vec3[ ]
        {
            new Vec3 (1.0f, 0.0f, 0.0f),
            new Vec3 (0.0f, 0.0f, 1.0f),
            new Vec3 (0.0f, 0.0f, -1.0f),
            new Vec3 (0.0f, -1.0f, 0.0f),
            new Vec3 (-1.0f, 0.0f, 0.0f),
            new Vec3 (0.0f, 1.0f, 0.0f)
        };

        target.texCoords = new Vec2[ ]
        {
            new Vec2 (0.625f, 0.0f),
            new Vec2 (0.375f, 0.0f),
            new Vec2 (0.375f, 0.75f),
            new Vec2 (0.625f, 0.75f),
            new Vec2 (0.375f, 1.0f),
            new Vec2 (0.625f, 1.0f),
            new Vec2 (0.625f, 0.5f),
            new Vec2 (0.375f, 0.5f),
            new Vec2 (0.625f, 0.25f),
            new Vec2 (0.375f, 0.25f),
            new Vec2 (0.125f, 0.5f),
            new Vec2 (0.125f, 0.25f),
            new Vec2 (0.875f, 0.25f),
            new Vec2 (0.875f, 0.5f)
        };

        switch (poly)
        {
            case PolyType.Ngon:
            case PolyType.Quad:

                target.loops = new Loop3[ ]
                {
                    new Loop3 (
                    new Index3 (0, 4, 4),
                    new Index3 (1, 5, 4),
                    new Index3 (3, 3, 4),
                    new Index3 (2, 2, 4)),
                    new Loop3 (
                    new Index3 (2, 2, 5),
                    new Index3 (3, 3, 5),
                    new Index3 (7, 6, 5),
                    new Index3 (6, 7, 5)),
                    new Loop3 (
                    new Index3 (6, 7, 0),
                    new Index3 (7, 6, 0),
                    new Index3 (5, 8, 0),
                    new Index3 (4, 9, 0)),
                    new Loop3 (
                    new Index3 (4, 9, 3),
                    new Index3 (5, 8, 3),
                    new Index3 (1, 0, 3),
                    new Index3 (0, 1, 3)),
                    new Loop3 (
                    new Index3 (2, 10, 2),
                    new Index3 (6, 7, 2),
                    new Index3 (4, 9, 2),
                    new Index3 (0, 11, 2)),
                    new Loop3 (
                    new Index3 (7, 6, 1),
                    new Index3 (3, 13, 1),
                    new Index3 (1, 12, 1),
                    new Index3 (5, 8, 1))
                };

                break;

            case PolyType.Tri:
            default:

                target.loops = new Loop3[ ]
                {
                    new Loop3 (
                    new Index3 (0, 4, 4),
                    new Index3 (1, 5, 4),
                    new Index3 (3, 3, 4)),
                    new Loop3 (
                    new Index3 (0, 4, 4),
                    new Index3 (3, 3, 4),
                    new Index3 (2, 2, 4)),
                    new Loop3 (
                    new Index3 (2, 2, 5),
                    new Index3 (3, 3, 5),
                    new Index3 (7, 6, 5)),
                    new Loop3 (
                    new Index3 (2, 2, 5),
                    new Index3 (7, 6, 5),
                    new Index3 (6, 7, 5)),
                    new Loop3 (
                    new Index3 (6, 7, 0),
                    new Index3 (7, 6, 0),
                    new Index3 (5, 8, 0)),
                    new Loop3 (
                    new Index3 (6, 7, 0),
                    new Index3 (5, 8, 0),
                    new Index3 (4, 9, 0)),
                    new Loop3 (
                    new Index3 (4, 9, 3),
                    new Index3 (5, 8, 3),
                    new Index3 (1, 0, 3)),
                    new Loop3 (
                    new Index3 (4, 9, 3),
                    new Index3 (1, 0, 3),
                    new Index3 (0, 1, 3)),
                    new Loop3 (
                    new Index3 (2, 10, 2),
                    new Index3 (6, 7, 2),
                    new Index3 (4, 9, 2)),
                    new Loop3 (
                    new Index3 (2, 10, 2),
                    new Index3 (4, 9, 2),
                    new Index3 (0, 11, 2)),
                    new Loop3 (
                    new Index3 (7, 6, 1),
                    new Index3 (3, 13, 1),
                    new Index3 (1, 12, 1)),
                    new Loop3 (
                    new Index3 (7, 6, 1),
                    new Index3 (1, 12, 1),
                    new Index3 (5, 8, 1))
                };

                break;
        }

        return target;
    }

    public static Mesh3 CubeSphere (in int itrs, in float size, in PolyType poly, in Mesh3 target)
    {
        Mesh3.Cube (0.5f, PolyType.Quad, target);
        target.SubdivFacesCenter (itrs);
        if (poly == PolyType.Tri) { Mesh3.Triangulate (target, target); }
        target.Clean ( );
        Mesh3.CastToSphere (target, size, target);
        return target;
    }

    public static Mesh3 Dodecahedron (in Mesh3 target)
    {
        target.name = "Dodecahedron";

        target.coords = new Vec3[ ]
        {
            new Vec3 (0.0f, 0.33614415f, -0.4165113f),
            new Vec3 (-0.19098301f, 0.47552827f, 0.15450847f),
            new Vec3 (0.19098301f, 0.47552827f, 0.15450847f),
            new Vec3 (0.309017f, 0.19840115f, 0.38938415f),
            new Vec3 (-0.309017f, 0.19840115f, 0.38938415f),
            new Vec3 (-0.19098301f, -0.47552827f, -0.15450847f),
            new Vec3 (-0.309017f, -0.38938415f, 0.19840115f),
            new Vec3 (0.19098301f, -0.47552827f, -0.15450847f),
            new Vec3 (0.309017f, -0.19840115f, -0.38938415f),
            new Vec3 (0.0f, -0.02712715f, -0.53454524f),
            new Vec3 (0.309017f, 0.38938415f, -0.19840115f),
            new Vec3 (0.5f, 0.05901699f, -0.18163565f),
            new Vec3 (-0.309017f, -0.19840115f, -0.38938415f),
            new Vec3 (-0.5f, 0.05901699f, -0.18163565f),
            new Vec3 (-0.309017f, 0.38938415f, -0.19840115f),
            new Vec3 (0.0f, 0.02712715f, 0.53454524f),
            new Vec3 (0.0f, -0.33614415f, 0.4165113f),
            new Vec3 (0.309017f, -0.38938415f, 0.19840115f),
            new Vec3 (0.5f, -0.05901699f, 0.18163565f),
            new Vec3 (-0.5f, -0.05901699f, 0.18163565f)
        };

        target.texCoords = new Vec2[ ]
        {
            new Vec2 (0.5f, 0.0f),
            new Vec2 (0.79389268f, 0.90450847f),
            new Vec2 (0.02447176f, 0.34549153f),
            new Vec2 (0.20610738f, 0.90450853f),
            new Vec2 (0.97552824f, 0.34549141f)
        };

        target.normals = new Vec3[ ]
        {
            new Vec3 (-0.8506508f, 0.5f, 0.16245979f),
            new Vec3 (0.0f, -0.9714768f, 0.23713443f),
            new Vec3 (0.0f, 0.9714768f, -0.23713443f),
            new Vec3 (0.0f, -0.64655715f, -0.7628655f),
            new Vec3 (0.5257311f, 0.2628655f, -0.80901694f),
            new Vec3 (0.0f, 0.64655715f, 0.7628655f),
            new Vec3 (-0.5257311f, 0.2628655f, -0.80901694f),
            new Vec3 (-0.5257311f, -0.2628655f, 0.80901694f),
            new Vec3 (0.5257311f, -0.2628655f, 0.80901694f),
            new Vec3 (0.8506508f, 0.5f, 0.16245979f),
            new Vec3 (0.8506508f, -0.5f, -0.16245979f),
            new Vec3 (-0.8506508f, -0.5f, -0.16245979f)
        };

        target.loops = new Loop3[ ]
        {
            new Loop3 (
            new Index3 (2, 0, 2),
            new Index3 (10, 2, 2),
            new Index3 (0, 3, 2),
            new Index3 (14, 1, 2),
            new Index3 (1, 4, 2)),
            new Loop3 (
            new Index3 (1, 0, 5),
            new Index3 (4, 2, 5),
            new Index3 (15, 3, 5),
            new Index3 (3, 1, 5),
            new Index3 (2, 4, 5)),
            new Loop3 (
            new Index3 (7, 0, 1),
            new Index3 (17, 2, 1),
            new Index3 (16, 3, 1),
            new Index3 (6, 1, 1),
            new Index3 (5, 4, 1)),
            new Loop3 (
            new Index3 (5, 0, 3),
            new Index3 (12, 2, 3),
            new Index3 (9, 3, 3),
            new Index3 (8, 1, 3),
            new Index3 (7, 4, 3)),
            new Loop3 (
            new Index3 (9, 0, 4),
            new Index3 (0, 2, 4),
            new Index3 (10, 3, 4),
            new Index3 (11, 1, 4),
            new Index3 (8, 4, 4)),
            new Loop3 (
            new Index3 (0, 0, 6),
            new Index3 (9, 2, 6),
            new Index3 (12, 3, 6),
            new Index3 (13, 1, 6),
            new Index3 (14, 4, 6)),
            new Loop3 (
            new Index3 (16, 0, 7),
            new Index3 (15, 2, 7),
            new Index3 (4, 3, 7),
            new Index3 (19, 1, 7),
            new Index3 (6, 4, 7)),
            new Loop3 (
            new Index3 (15, 0, 8),
            new Index3 (16, 2, 8),
            new Index3 (17, 3, 8),
            new Index3 (18, 1, 8),
            new Index3 (3, 4, 8)),
            new Loop3 (
            new Index3 (11, 0, 9),
            new Index3 (10, 2, 9),
            new Index3 (2, 3, 9),
            new Index3 (3, 1, 9),
            new Index3 (18, 4, 9)),
            new Loop3 (
            new Index3 (18, 0, 10),
            new Index3 (17, 2, 10),
            new Index3 (7, 3, 10),
            new Index3 (8, 1, 10),
            new Index3 (11, 4, 10)),
            new Loop3 (
            new Index3 (13, 0, 11),
            new Index3 (12, 2, 11),
            new Index3 (5, 3, 11),
            new Index3 (6, 1, 11),
            new Index3 (19, 4, 11)),
            new Loop3 (
            new Index3 (19, 0, 0),
            new Index3 (4, 2, 0),
            new Index3 (1, 3, 0),
            new Index3 (14, 1, 0),
            new Index3 (13, 4, 0))
        };

        return target;
    }

    public static Mesh3 Octahedron (in Mesh3 target)
    {
        target.name = "Octahedron";

        target.coords = new Vec3[ ]
        {
            new Vec3 (0.0f, -0.5f, 0.0f),
            new Vec3 (0.5f, 0.0f, 0.0f),
            new Vec3 (-0.5f, 0.0f, 0.0f),
            new Vec3 (0.0f, 0.5f, 0.0f),
            new Vec3 (0.0f, 0.0f, 0.5f),
            new Vec3 (0.0f, 0.0f, -0.5f)
        };

        target.texCoords = new Vec2[ ]
        {
            new Vec2 (0.5f, 0.0f),
            new Vec2 (1.0f, 1.0f),
            new Vec2 (0.0f, 1.0f)
        };

        target.normals = new Vec3[ ]
        {
            new Vec3 (0.57735026f, -0.57735026f, 0.57735026f),
            new Vec3 (-0.57735026f, 0.57735026f, 0.57735026f),
            new Vec3 (-0.57735026f, -0.57735026f, 0.57735026f),
            new Vec3 (0.57735026f, 0.57735026f, 0.57735026f),
            new Vec3 (-0.57735026f, 0.57735026f, -0.57735026f),
            new Vec3 (0.57735026f, 0.57735026f, -0.57735026f),
            new Vec3 (0.57735026f, -0.57735026f, -0.57735026f),
            new Vec3 (-0.57735026f, -0.57735026f, -0.57735026f)
        };

        target.loops = new Loop3[ ]
        {
            new Loop3 (
            new Index3 (0, 2, 0),
            new Index3 (1, 1, 0),
            new Index3 (4, 0, 0)),
            new Loop3 (
            new Index3 (1, 2, 3),
            new Index3 (3, 1, 3),
            new Index3 (4, 0, 3)),
            new Loop3 (
            new Index3 (3, 2, 1),
            new Index3 (2, 1, 1),
            new Index3 (4, 0, 1)),
            new Loop3 (
            new Index3 (2, 2, 2),
            new Index3 (0, 1, 2),
            new Index3 (4, 0, 2)),
            new Loop3 (
            new Index3 (2, 2, 4),
            new Index3 (3, 1, 4),
            new Index3 (5, 0, 4)),
            new Loop3 (
            new Index3 (3, 2, 5),
            new Index3 (1, 1, 5),
            new Index3 (5, 0, 5)),
            new Loop3 (
            new Index3 (1, 2, 6),
            new Index3 (0, 1, 6),
            new Index3 (5, 0, 6)),
            new Loop3 (
            new Index3 (0, 2, 7),
            new Index3 (2, 1, 7),
            new Index3 (5, 0, 7))
        };

        return target;
    }

    public static Mesh3 Icosahedron (in Mesh3 target)
    {
        target.name = "Icosahedron";

        target.coords = new Vec3[ ]
        {
            new Vec3 (0.0f, 0.0f, -0.5f),
            new Vec3 (0.0f, -0.4472122f, -0.22360958f),
            new Vec3 (-0.4253254f, -0.13819478f, -0.22360958f),
            new Vec3 (0.4253254f, -0.13819478f, -0.22360958f),
            new Vec3 (-0.26286554f, 0.3618018f, -0.22360958f),
            new Vec3 (0.26286554f, 0.3618018f, -0.22360958f),
            new Vec3 (-0.26286554f, -0.3618018f, 0.22360958f),
            new Vec3 (0.26286554f, -0.3618018f, 0.22360958f),
            new Vec3 (-0.4253254f, 0.13819478f, 0.22360958f),
            new Vec3 (0.4253254f, 0.13819478f, 0.22360958f),
            new Vec3 (0.0f, 0.4472122f, 0.22360958f),
            new Vec3 (0.0f, 0.0f, 0.5f)
        };

        target.texCoords = new Vec2[ ]
        {
            new Vec2 (0.0f, 0.578613f),
            new Vec2 (0.090909f, 0.421387f),
            new Vec2 (0.090909f, 0.735839f),
            new Vec2 (0.181818f, 0.264161f),
            new Vec2 (0.181818f, 0.578613f),
            new Vec2 (0.272727f, 0.421387f),
            new Vec2 (0.272727f, 0.735839f),
            new Vec2 (0.363636f, 0.264161f),
            new Vec2 (0.363636f, 0.578613f),
            new Vec2 (0.454545f, 0.421387f),
            new Vec2 (0.454545f, 0.735839f),
            new Vec2 (0.545455f, 0.264161f),
            new Vec2 (0.545455f, 0.578613f),
            new Vec2 (0.636364f, 0.421387f),
            new Vec2 (0.636364f, 0.735839f),
            new Vec2 (0.727273f, 0.264161f),
            new Vec2 (0.727273f, 0.578613f),
            new Vec2 (0.818182f, 0.421387f),
            new Vec2 (0.818182f, 0.735839f),
            new Vec2 (0.909091f, 0.264161f),
            new Vec2 (0.909091f, 0.578613f),
            new Vec2 (1.0f, 0.421387f)
        };

        target.normals = new Vec3[ ]
        {
            new Vec3 (0.0f, -0.60706145f, 0.79465485f),
            new Vec3 (0.57735217f, -0.7946537f, -0.18758972f),
            new Vec3 (0.934172f, -0.30353418f, 0.18758905f),
            new Vec3 (0.934172f, 0.30353418f, -0.18758905f),
            new Vec3 (0.57735217f, 0.7946537f, 0.18758972f),
            new Vec3 (0.35682237f, 0.4911218f, 0.79465526f),
            new Vec3 (-0.35682237f, 0.4911218f, 0.79465526f),
            new Vec3 (0.0f, 0.60706145f, -0.79465485f),
            new Vec3 (-0.35682237f, -0.4911218f, -0.79465526f),
            new Vec3 (0.35682237f, -0.4911218f, -0.79465526f),
            new Vec3 (-0.57734716f, 0.18759352f, -0.7946564f),
            new Vec3 (0.5773471f, 0.18759349f, -0.7946564f),
            new Vec3 (0.0f, 0.98224694f, -0.1875924f),
            new Vec3 (-0.57735217f, 0.7946537f, 0.18758972f),
            new Vec3 (-0.934172f, 0.30353418f, -0.18758905f),
            new Vec3 (-0.934172f, -0.30353418f, 0.18758905f),
            new Vec3 (-0.57735217f, -0.7946537f, -0.18758972f),
            new Vec3 (0.0f, -0.98224694f, 0.1875924f),
            new Vec3 (-0.57734716f, -0.18759352f, 0.7946564f),
            new Vec3 (0.5773471f, -0.18759349f, 0.7946564f)
        };

        target.loops = new Loop3[ ]
        {
            new Loop3 (
            new Index3 (0, 2, 7),
            new Index3 (4, 4, 7),
            new Index3 (5, 0, 7)),
            new Loop3 (
            new Index3 (0, 6, 10),
            new Index3 (2, 8, 10),
            new Index3 (4, 4, 10)),
            new Loop3 (
            new Index3 (0, 10, 8),
            new Index3 (1, 12, 8),
            new Index3 (2, 8, 8)),
            new Loop3 (
            new Index3 (0, 14, 9),
            new Index3 (3, 16, 9),
            new Index3 (1, 12, 9)),
            new Loop3 (
            new Index3 (0, 18, 11),
            new Index3 (5, 20, 11),
            new Index3 (3, 16, 11)),
            new Loop3 (
            new Index3 (10, 1, 12),
            new Index3 (5, 0, 12),
            new Index3 (4, 4, 12)),
            new Loop3 (
            new Index3 (8, 5, 13),
            new Index3 (10, 1, 13),
            new Index3 (4, 4, 13)),
            new Loop3 (
            new Index3 (8, 5, 14),
            new Index3 (4, 4, 14),
            new Index3 (2, 8, 14)),
            new Loop3 (
            new Index3 (6, 9, 15),
            new Index3 (8, 5, 15),
            new Index3 (2, 8, 15)),
            new Loop3 (
            new Index3 (6, 9, 16),
            new Index3 (2, 8, 16),
            new Index3 (1, 12, 16)),
            new Loop3 (
            new Index3 (7, 13, 17),
            new Index3 (6, 9, 17),
            new Index3 (1, 12, 17)),
            new Loop3 (
            new Index3 (7, 13, 1),
            new Index3 (1, 12, 1),
            new Index3 (3, 16, 1)),
            new Loop3 (
            new Index3 (9, 17, 2),
            new Index3 (7, 13, 2),
            new Index3 (3, 16, 2)),
            new Loop3 (
            new Index3 (9, 17, 3),
            new Index3 (3, 16, 3),
            new Index3 (5, 20, 3)),
            new Loop3 (
            new Index3 (10, 21, 4),
            new Index3 (9, 17, 4),
            new Index3 (5, 20, 4)),
            new Loop3 (
            new Index3 (11, 19, 5),
            new Index3 (9, 17, 5),
            new Index3 (10, 21, 5)),
            new Loop3 (
            new Index3 (11, 3, 6),
            new Index3 (10, 1, 6),
            new Index3 (8, 5, 6)),
            new Loop3 (
            new Index3 (11, 7, 18),
            new Index3 (8, 5, 18),
            new Index3 (6, 9, 18)),
            new Loop3 (
            new Index3 (11, 11, 0),
            new Index3 (6, 9, 0),
            new Index3 (7, 13, 0)),
            new Loop3 (
            new Index3 (11, 15, 19),
            new Index3 (7, 13, 19),
            new Index3 (9, 17, 19))
        };

        return target;
    }

    public static Mesh3 Icosphere (in int itrs, in float size, in Mesh3 target)
    {
        Mesh3.Icosahedron (target);
        target.SubdivFacesInscribe (itrs);
        target.Clean ( );
        Mesh3.CastToSphere (target, size, target);
        target.name = "Icosphere";
        return target;
    }

    public static Mesh3 Triangulate (in Mesh3 source, in Mesh3 target)
    {
        Loop3[ ] loopsSrc = source.loops;
        Vec3[ ] vsSrc = source.coords;
        Vec2[ ] vtsSrc = source.texCoords;
        Vec3[ ] vnsSrc = source.normals;

        // Cannot anticipate how many loops in the source mesh will not be
        // triangles, so this is an expanding list.
        List<Loop3> loopsTrg = new List<Loop3> ( );

        int loopSrcLen = loopsSrc.Length;
        for (int i = 0; i < loopSrcLen; ++i)
        {
            Loop3 fSrc = loopsSrc[i];
            int fSrcLen = fSrc.Length;

            // If face loop is not a triangle, then split.
            if (fSrcLen > 3)
            {

                // Find last non-adjacent index. For index m, neither m + 1 nor
                // m - 1, so where m = 0, the last non-adjacent would be
                // arr.Length - 2.
                Index3 vert0 = fSrc[0];
                int lastNonAdj = fSrcLen - 2;
                for (int m = 0; m < lastNonAdj; ++m)
                {
                    // Find next two vertices.
                    Index3 vert1 = fSrc[1 + m];
                    Index3 vert2 = fSrc[2 + m];

                    // Create a new triangle which connects them.
                    Loop3 loopTrg = new Loop3 (vert0, vert1, vert2);
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
            target.coords = new Vec3[vsLen];
            System.Array.Copy (vsSrc, target.coords, vsLen);

            int vtsLen = vtsSrc.Length;
            target.texCoords = new Vec2[vtsLen];
            System.Array.Copy (vtsSrc, target.texCoords, vtsLen);

            int vnsLen = vnsSrc.Length;
            target.normals = new Vec3[vnsSrc.Length];
            System.Array.Copy (vnsSrc, target.normals, vnsLen);
        }
        target.loops = loopsTrg.ToArray ( );

        return target;
    }

    public static Mesh3 UniformData (in Mesh3 source, in Mesh3 target)
    {
        Loop3[ ] fsSrc = source.loops;
        Vec3[ ] vsSrc = source.coords;
        Vec2[ ] vtsSrc = source.texCoords;
        Vec3[ ] vnsSrc = source.normals;

        int uniformLen = 0;
        int fsSrcLen = fsSrc.Length;
        for (int i = 0; i < fsSrcLen; ++i)
        {
            uniformLen += fsSrc[i].Length;
        }

        Loop3[ ] fsTrg = new Loop3[fsSrcLen];
        Vec3[ ] vsTrg = new Vec3[uniformLen];
        Vec2[ ] vtsTrg = new Vec2[uniformLen];
        Vec3[ ] vnsTrg = new Vec3[uniformLen];

        for (int k = 0, i = 0; i < fsSrcLen; ++i)
        {
            Index3[ ] fSrc = fsSrc[i].Indices;
            int fLen = fSrc.Length;
            fsTrg[i] = new Loop3 (new Index3[fLen]);
            Index3[ ] fTrg = fsTrg[i].Indices;

            for (int j = 0; j < fLen; ++j, ++k)
            {
                Index3 vertSrc = fSrc[j];
                vsTrg[k] = vsSrc[vertSrc.v];
                vtsTrg[k] = vtsSrc[vertSrc.vt];
                vnsTrg[k] = vnsSrc[vertSrc.vn];

                fTrg[j] = new Index3 (k, k, k);
            }
        }

        target.coords = vsTrg;
        target.texCoords = vtsTrg;
        target.normals = vnsTrg;
        target.loops = fsTrg;

        return target;
    }

    public static Mesh3 UvSphere (in int longitudes, in int latitudes, in float size, in PolyType poly, in Mesh3 target)
    {
        target.name = "UV Sphere";

        int lons = longitudes < 3 ? 3 : longitudes;
        int lats = latitudes < 1 ? 1 : latitudes;
        float radius = Utils.Max (Utils.Epsilon, size);
        int lons1 = lons + 1;
        int lats1 = lats + 1;
        int vLen = lons * lats + 2;
        int vtLen = lons1 * lats + lons + lons;
        int last0 = vLen - 1;

        Vec3[ ] vs = target.coords = Vec3.Resize (target.coords, vLen);
        Vec2[ ] vts = target.texCoords = Vec2.Resize (target.texCoords, vtLen);
        Vec3[ ] vns = target.normals = Vec3.Resize (target.normals, vLen);

        float toTexS = 1.0f / lons;
        float toTexT = 1.0f / lats1;
        float toTheta = Utils.Tau / lons;
        float toPhi = Utils.Pi / lats1;

        vs[0] = new Vec3 (0.0f, 0.5f, 0.0f);
        vns[0] = new Vec3 (0.0f, 1.0f, 0.0f);

        vs[last0] = new Vec3 (0.0f, -0.5f, 0.0f);
        vns[last0] = new Vec3 (0.0f, -1.0f, 0.0f);

        Vec2[ ] sincost = new Vec2[lons];
        for (int j = 0, k = vtLen - lons; j < lons; ++j, ++k)
        {
            float jf = (float) j;

            float theta = jf * toTheta;
            sincost[j] = new Vec2 (Utils.Cos (theta), Utils.Sin (theta));

            float sTex = (jf + 0.5f) * toTexS;
            vts[j] = new Vec2 (sTex, 1.0f);
            vts[k] = new Vec2 (sTex, 0.0f);
        }

        float[ ] tcxs = new float[lons1];
        for (int j = 0; j < lons1; ++j) { tcxs[j] = j * toTexS; }

        for (int i = 0, vIdx = 1, vtIdx = lons; i < lats; ++i)
        {
            float spOff = i + 1.0f;
            float tTex = 1.0f - spOff * toTexT;

            float phi = spOff * toPhi;
            float cosPhi = Utils.Cos (phi);
            float sinPhi = Utils.Sin (phi);

            float rhoCosPhi = radius * cosPhi;
            float rhoSinPhi = radius * sinPhi;

            for (int j = 0; j < lons; ++j, ++vIdx)
            {
                Vec2 sct = sincost[j];
                float cosTheta = sct.x;
                float sinTheta = sct.y;

                vs[vIdx] = new Vec3 (
                    rhoSinPhi * cosTheta,
                    rhoCosPhi,
                    rhoSinPhi * sinTheta);
                vns[vIdx] = Vec3.Normalize (vs[vIdx]);
            }

            for (int j = 0; j < lons1; ++j, ++vtIdx)
            {
                vts[vtIdx] = new Vec2 (tcxs[j], tTex);
            }
        }

        bool isQuad = poly != PolyType.Tri;
        int latsn1 = lats - 1;
        int midCount = isQuad ? latsn1 * lons : latsn1 * lons * 2;
        int fsLen = lons + lons + midCount;

        Loop3[ ] fs = target.loops = Loop3.Resize (target.loops, fsLen);

        int vIdxOff = last0 - lons;
        int vtPoleOff = vtLen - lons;
        int vtIdxOff = vtPoleOff - lons1;
        int fsIdxOff = fsLen - lons;

        for (int i = 0; i < lons; ++i)
        {
            int k = i + 1;
            int n = k % lons;
            int j = 1 + n;
            int m = lons + i;

            Loop3 northTri = fs[i] = new Loop3 (
                new Index3 (j, m + 1, j),
                new Index3 (k, m, k),
                new Index3 (0, i, 0));

            Loop3 southTri = fs[fsIdxOff + i] = new Loop3 (
                new Index3 (
                    vIdxOff + i,
                    vtIdxOff + i,
                    vIdxOff + i),
                new Index3 (
                    vIdxOff + n,
                    vtIdxOff + k,
                    vIdxOff + n),
                new Index3 (
                    last0,
                    vtPoleOff + i,
                    last0));
        }

        int stride = isQuad ? 1 : 2;
        for (int i = 0, k = lons; i < latsn1; ++i)
        {
            int currentLat0 = 1 + i * lons;
            int nextLat0 = currentLat0 + lons;
            int currentLat1 = lons + i * lons1;
            int nextLat1 = currentLat1 + lons1;

            for (int j = 0; j < lons; ++j, k += stride)
            {
                int nextLon1 = j + 1;
                int nextLon0 = nextLon1 % lons;

                int v00 = currentLat0 + j;
                int v10 = currentLat0 + nextLon0;
                int v11 = nextLat0 + nextLon0;
                int v01 = nextLat0 + j;

                int vt00 = currentLat1 + j;
                int vt10 = currentLat1 + nextLon1;
                int vt11 = nextLat1 + nextLon1;
                int vt01 = nextLat1 + j;

                if (isQuad)
                {
                    Loop3 quad = fs[k] = new Loop3 (
                        new Index3 (v00, vt00, v00),
                        new Index3 (v10, vt10, v10),
                        new Index3 (v11, vt11, v11),
                        new Index3 (v01, vt01, v01));
                }
                else
                {
                    Loop3 tri0 = fs[k] = new Loop3 (
                        new Index3 (v00, vt00, v00),
                        new Index3 (v10, vt10, v10),
                        new Index3 (v11, vt11, v11));

                    Loop3 tri1 = fs[k + 1] = new Loop3 (
                        new Index3 (v00, vt00, v00),
                        new Index3 (v11, vt11, v11),
                        new Index3 (v01, vt01, v01));
                }
            }
        }

        return target;
    }
}