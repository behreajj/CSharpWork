using System;
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
    protected Vec2[] coords;

    /// <summary>
    /// Loops that describe the indices which reference the coordinates and
    /// texture coordinates to compose a face.
    /// </summary>
    protected Loop2[] loops;

    /// <summary>
    /// The texture (UV) coordinates that describe how an image is mapped onto
    /// the mesh. Typically in the range [0.0, 1.0] .
    /// </summary>
    protected Vec2[] texCoords;

    /// <summary>
    /// An array of coordinates.
    /// </summary>
    /// <value>coordinates</value>
    public Vec2[] Coords
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
    public Loop2[] Loops
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

    /// <summary>
    /// Gets a face from the mesh.
    /// </summary>
    /// <value>face</value>
    public Face2 this[int i]
    {
        get { return this.GetFace(i); }
    }

    /// <summary>
    /// The default constructor.
    /// </summary>
    public Mesh2()
    {
        // TODO: All loops are classes and so need to be resized,
        // then reset as references, rather than making new allocations.
    }

    /// <summary>
    /// Constructs a mesh from data.
    /// </summary>
    /// <param name="loops">loops</param>
    /// <param name="coords">coordinates</param>
    /// <param name="texCoords">texture coordinates</param>
    public Mesh2(
        in Loop2[] loops,
        in Vec2[] coords,
        in Vec2[] texCoords)
    {
        this.loops = loops;
        this.coords = coords;
        this.texCoords = texCoords;
    }

    /// <summary>
    /// Copies a source mesh to a new mesh.
    /// </summary>
    /// <param name="source">source mesh</param>
    public Mesh2(in Mesh2 source)
    {
        this.Set(source);
    }

    /// <summary>
    /// Returns a string representation of this mesh.
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()
    {
        return Mesh2.ToString(this);
    }

    /// <summary>
    /// Calculates texture coordinates (UVs) for this mesh. Finds the
    /// object-space dimensions of each coordinate, then uses the frame as a
    /// reference for new UVs, such that the shape acts as a mask for the
    /// texture (or, the texture fills the shape without repeating).
    /// </summary>
    /// <returns>this mesh/returns>
    public Mesh2 CalcUvs()
    {
        // TODO: Test.
        Bounds2 aabb = Mesh2.CalcBounds(this);
        Vec2 dim = Bounds2.ExtentSigned(aabb);
        Vec2 lb = aabb.Min;

        float lbx = lb.x;
        float lby = lb.y;
        float xInv = 1.0f / dim.x;
        float yInv = 1.0f / dim.y;

        float sAspect = 1.0f;
        float tAspect = 1.0f;
        if (dim.x < dim.y) { sAspect = dim.x / dim.y; }
        else if (dim.x > dim.y) { tAspect = dim.y / dim.x; }

        int vsLen = this.coords.Length;
        this.texCoords = Vec2.Resize(this.texCoords, vsLen);
        for (int i = 0; i < vsLen; ++i)
        {
            Vec2 v = this.coords[i];
            float sStretch = (v.x - lbx) * xInv;
            float tStretch = (v.y - lby) * yInv;
            float s = (sStretch - 0.5f) * sAspect + 0.5f;
            float t = (tStretch - 0.5f) * tAspect + 0.5f;
            this.texCoords[i] = new Vec2(s, 1.0f - t);
        }

        int facesLen = this.loops.Length;
        for (int i = 0; i < facesLen; ++i)
        {
            Loop2 loop = this.loops[i];
            Index2[] srcIndices = loop.Indices;
            int idcsLen = srcIndices.Length;
            for (int j = 0; j < idcsLen; ++j)
            {
                Index2 src = srcIndices[j];
                loop.Indices[j] = new Index2(src.v, src.v);
            }
        }

        return this;
    }

    /// <summary>
    /// Removes elements from the coordinate, texture coordinate and normal
    /// arrays of the mesh which are not visited by the face indices.
    /// </summary>
    /// <returns>this mesh</returns>
    public Mesh2 Clean()
    {
        // TODO: Test.

        // Transfer arrays to dictionaries where the face index is the key.
        Dictionary<int, Vec2> usedCoords = new();
        Dictionary<int, Vec2> usedTexCoords = new();

        // Visit all data arrays with the faces array. Any data not used by any
        // face will be left out.
        int facesLen = this.loops.Length;
        for (int i = 0; i < facesLen; ++i)
        {
            Loop2 loop = this.loops[i];
            Index2[] verts = loop.Indices;
            int vertsLen = verts.Length;
            for (int j = 0; j < vertsLen; ++j)
            {
                // The dictionary should ignore repeated visitations.
                Index2 vert = verts[j];
                usedCoords[vert.v] = this.coords[vert.v];
                usedTexCoords[vert.vt] = this.texCoords[vert.vt];
            }
        }

        // Use a sorted set to filter out similar vectors.
        SortQuantized2 v2Cmp = new();
        SortedSet<Vec2> coordsSet = new(v2Cmp);
        SortedSet<Vec2> texCoordsSet = new(v2Cmp);

        coordsSet.UnionWith(usedCoords.Values);
        texCoordsSet.UnionWith(usedTexCoords.Values);

        // Dictionary's keys are no longer needed; just values.
        Vec2[] newCoords = new Vec2[coordsSet.Count];
        Vec2[] newTexCoords = new Vec2[texCoordsSet.Count];

        // Convert from sorted set to arrays.
        coordsSet.CopyTo(newCoords);
        texCoordsSet.CopyTo(newTexCoords);

        for (int i = 0; i < facesLen; ++i)
        {
            Loop2 loop = this.loops[i];
            Index2[] verts = loop.Indices;
            int vertsLen = verts.Length;
            for (int j = 0; j < vertsLen; ++j)
            {
                // Find index of vector in new array by using indexed value from old
                // array as a reference.
                Index2 oldVert = verts[j];
                verts[j] = new Index2(
                    Array.BinarySearch<Vec2>(newCoords, this.coords[oldVert.v], v2Cmp),
                    Array.BinarySearch<Vec2>(newTexCoords, this.texCoords[oldVert.vt], v2Cmp));
            }
        }

        // Replace old arrays with the new.
        this.coords = newCoords;
        this.texCoords = newTexCoords;

        // Sort faces by center.
        Array.Sort(this.loops, new SortLoops2(this.coords));

        return this;
    }

    /// <summary>
    /// Removes a given number of face indices from this mesh beginning at an
    /// index. Does not remove any data associated with the indices.
    /// </summary>
    /// <param name="faceIndex">index</param>
    /// <param name="deletions">removal count</param>
    /// <returns>this mesh</returns>
    public Mesh2 DeleteFaces(in int faceIndex, in int deletions = 1)
    {
        int aLen = this.loops.Length;
        int valIdx = Utils.RemFloor(faceIndex, aLen);
        int valDel = Utils.Clamp(deletions, 0, aLen - valIdx);
        int bLen = aLen - valDel;
        Loop2[] result = new Loop2[bLen];
        System.Array.Copy(this.loops, 0, result, 0, valIdx);
        System.Array.Copy(this.loops, valIdx + valDel, result, valIdx, bLen - valIdx);
        this.loops = result;

        return this;
    }

    /// <summary>
    /// Negates the x component of all texture coordinates (u) in the mesh.
    /// Does so by subtracting the value from 1.0.
    /// </summary>
    /// <returns>this mesh</returns>
    public Mesh2 FlipU()
    {
        int vtsLen = this.texCoords.Length;
        for (int i = 0; i < vtsLen; ++i)
        {
            Vec2 vt = this.texCoords[i];
            this.texCoords[i] = new Vec2(1.0f - vt.x, vt.y);
        }
        return this;
    }

    /// <summary>
    /// Negates the y component of all texture coordinates (v) in the mesh.
    /// Does so by subtracting the value from 1.0.
    /// </summary>
    /// <returns>this mesh</returns>
    public Mesh2 FlipV()
    {
        int vtsLen = this.texCoords.Length;
        for (int i = 0; i < vtsLen; ++i)
        {
            Vec2 vt = this.texCoords[i];
            this.texCoords[i] = new Vec2(vt.x, 1.0f - vt.y);
        }
        return this;
    }

    /// <summary>
    /// Negates the x component of all coordinates in the mesh,
    /// then reverses the mesh's faces.
    /// Use this instead of scaling the mesh by (-1.0, 1.0).
    /// </summary>
    /// <returns>this mesh</returns>
    public Mesh2 FlipX()
    {
        int vsLen = this.coords.Length;
        for (int i = 0; i < vsLen; ++i)
        {
            this.coords[i] = Vec2.FlipX(this.coords[i]);
        }
        this.ReverseFaces();
        return this;
    }

    /// <summary>
    /// Negates the y component of all coordinates in the mesh,
    /// then reverses the mesh's faces.
    /// Use this instead of scaling the mesh by (1.0, -1.0).
    /// </summary>
    /// <returns>this mesh</returns>
    public Mesh2 FlipY()
    {
        int vsLen = this.coords.Length;
        for (int i = 0; i < vsLen; ++i)
        {
            this.coords[i] = Vec2.FlipY(this.coords[i]);
        }
        this.ReverseFaces();
        return this;
    }

    /// <summary>
    /// Gets an edge from the mesh.
    /// </summary>
    /// <param name="faceIndex">face index</param>
    /// <param name="edgeIndex">edge index</param>
    /// <returns>edge</returns>
    public Edge2 GetEdge(in int faceIndex = -1, in int edgeIndex = -1)
    {
        Loop2 loop = this.loops[Utils.RemFloor(faceIndex, this.loops.Length)];
        Index2 idxOrigin = loop[edgeIndex];
        Index2 idxDest = loop[edgeIndex + 1];

        return new Edge2(
            new Vert2(
                this.coords[idxOrigin.v],
                this.texCoords[idxOrigin.vt]),

            new Vert2(
                this.coords[idxDest.v],
                this.texCoords[idxDest.vt]));
    }

    ///<summary>
    ///Gets an array of edges from the mesh.
    ///</summary>
    ///<returns>edges array</returns>
    public Edge2[] GetEdges()
    {
        return this.GetEdgesUndirected();
    }

    ///<summary>
    ///Gets an array of edges from the mesh. Edges are treated as directed, so
    ///(origin, destination) and (destination, edge) are considered to be
    ///different.
    ///</summary>
    ///<returns>edges array</returns>
    public Edge2[] GetEdgesDirected()
    {
        int loopsLen = this.loops.Length;
        SortedSet<Edge2> result = new();

        for (int i = 0; i < loopsLen; ++i)
        {
            Loop2 loop = this.loops[i];
            Index2[] indices = loop.Indices;
            int indicesLen = indices.Length;

            for (int j = 0; j < indicesLen; ++j)
            {
                Index2 idxOrigin = indices[j];
                Index2 idxDest = indices[(j + 1) % indicesLen];

                Edge2 trial = new(
                    new Vert2(
                        this.coords[idxOrigin.v],
                        this.texCoords[idxOrigin.vt]),

                    new Vert2(
                        this.coords[idxDest.v],
                        this.texCoords[idxDest.vt]));

                result.Add(trial);
            }
        }

        Edge2[] arr = new Edge2[result.Count];
        result.CopyTo(arr);
        return arr;
    }

    ///<summary>
    ///Gets an array of edges from the mesh. Edges are treated as undirected,
    ///so (origin, destination) and (destination, edge) are considered to be
    ///equal
    ///</summary>
    ///<returns>edges array</returns>
    public Edge2[] GetEdgesUndirected()
    {
        int loopsLen = this.loops.Length;
        Dictionary<int, Edge2> result = new();

        for (int i = 0; i < loopsLen; ++i)
        {
            Loop2 loop = this.loops[i];
            Index2[] indices = loop.Indices;
            int indicesLen = indices.Length;

            for (int j = 0; j < indicesLen; ++j)
            {
                Index2 idxOrigin = indices[j];
                Index2 idxDest = indices[(j + 1) % indicesLen];

                int vIdxOrigin = idxOrigin.v;
                int vIdxDest = idxDest.v;

                int aHsh = (Utils.MulBase ^ vIdxOrigin) *
                    Utils.HashMul ^ vIdxDest;
                int bHsh = (Utils.MulBase ^ vIdxDest) *
                    Utils.HashMul ^ vIdxOrigin;

                if (!result.ContainsKey(aHsh) && !result.ContainsKey(bHsh))
                {
                    result[vIdxOrigin < vIdxDest ? aHsh : bHsh] = new Edge2(
                    new Vert2(
                        this.coords[idxOrigin.v],
                        this.texCoords[idxOrigin.vt]),

                    new Vert2(
                        this.coords[idxDest.v],
                        this.texCoords[idxDest.vt]));
                }
            }
        }

        Edge2[] arr = new Edge2[result.Count];
        result.Values.CopyTo(arr, 0);
        return arr;
    }

    /// <summary>
    /// Gets a face from a mesh.
    /// </summary>
    /// <param name="faceIndex">face index</param>
    /// <returns>face</returns>
    public Face2 GetFace(in int faceIndex = -1)
    {
        Loop2 loop = this.loops[Utils.RemFloor(faceIndex, this.loops.Length)];
        Index2[] indices = loop.Indices;
        int indicesLen = indices.Length;
        Edge2[] edges = new Edge2[indicesLen];
        for (int i = 0; i < indicesLen; ++i)
        {
            Index2 idxOrigin = indices[i];
            Index2 idxDest = indices[(i + 1) % indicesLen];
            edges[i] = new Edge2(
                new Vert2(
                    this.coords[idxOrigin.v],
                    this.texCoords[idxOrigin.vt]),
                new Vert2(
                    this.coords[idxDest.v],
                    this.texCoords[idxDest.vt]));
        }
        return new Face2(edges);
    }

    /// <summary>
    /// Gets an array of faces from the mesh.
    /// </summary>
    /// <returns>faces array</returns>
    public Face2[] GetFaces()
    {
        int loopsLen = this.loops.Length;
        Face2[] faces = new Face2[loopsLen];
        for (int i = 0; i < loopsLen; ++i)
        {
            Loop2 loop = this.loops[i];
            Index2[] indices = loop.Indices;
            int indicesLen = indices.Length;
            Edge2[] edges = new Edge2[indicesLen];

            for (int j = 0; j < indicesLen; ++j)
            {
                Index2 idxOrigin = indices[j];
                Index2 idxDest = indices[(j + 1) % indicesLen];
                edges[j] = new Edge2(
                    new Vert2(
                        this.coords[idxOrigin.v],
                        this.texCoords[idxOrigin.vt]),
                    new Vert2(
                        this.coords[idxDest.v],
                        this.texCoords[idxDest.vt]));
            }

            faces[i] = new Face2(edges);
        }
        return faces;
    }

    /// <summary>
    /// Gets a vertex from the mesh.
    /// </summary>
    /// <param name="faceIndex">face index</param>
    /// <param name="vertIndex">vertex index</param>
    /// <returns>vertex</returns>
    public Vert2 GetVertex(in int faceIndex = -1, in int vertIndex = -1)
    {
        Index2 index = this.loops[Utils.RemFloor(faceIndex, this.loops.Length)][vertIndex];
        return new Vert2(
            this.coords[index.v],
            this.texCoords[index.vt]);
    }

    /// <summary>
    /// Gets an array of vertices from the mesh.
    /// </summary>
    /// <returns>vertices</returns>
    public Vert2[] GetVertices()
    {
        int len0 = this.loops.Length;
        SortedSet<Vert2> result = new();
        for (int i = 0; i < len0; ++i)
        {
            Loop2 loop = this.loops[i];
            Index2[] indices = loop.Indices;
            int len1 = indices.Length;
            for (int j = 0; j < len1; ++j)
            {
                Index2 vert = indices[j];
                result.Add(new Vert2(
                    this.coords[vert.v],
                    this.texCoords[vert.vt]));
            }
        }

        Vert2[] arr = new Vert2[result.Count];
        result.CopyTo(arr);
        return arr;
    }

    public (Loop2[] loopsNew, Vec2[] vsNew, Vec2[] vtsNew) InsetFace(
        in int faceIndex = 0,
        in float fac = 0.5f)
    {
        // TODO: Add comment.

        if (fac <= 0.0f)
        {
            return (loopsNew: new Loop2[0],
                vsNew: new Vec2[0],
                vtsNew: new Vec2[0]);
        }
        if (fac >= 1.0f)
        {
            return this.SubdivFaceFan(faceIndex);
        }

        int loopsLen = this.loops.Length;
        int i = Utils.RemFloor(faceIndex, loopsLen);
        Loop2 loop = this.loops[i];
        Index2[] indices = loop.Indices;
        int loopLen = indices.Length;

        int vsOldLen = this.coords.Length;
        int vtsOldLen = this.texCoords.Length;
        Loop2[] loopsNew = new Loop2[loopLen + 1];
        Loop2 centerLoop = loopsNew[loopLen] = new Loop2(loopLen);

        // Sum centers.
        Vec2 vCenter = Vec2.Zero;
        Vec2 vtCenter = Vec2.Zero;
        for (int j = 0; j < loopLen; ++j)
        {
            Index2 curr = indices[j];
            vCenter += this.coords[curr.v];
            vtCenter += this.texCoords[curr.vt];
        }

        // Find average.
        if (loopLen > 0)
        {
            float flInv = 1.0f / loopLen;
            vCenter *= flInv;
            vtCenter *= flInv;
        }

        Vec2[] vsNew = new Vec2[loopLen];
        Vec2[] vtsNew = new Vec2[loopLen];

        // Find new corners.
        for (int j = 0; j < loopLen; ++j)
        {
            int k = (j + 1) % loopLen;

            Index2 vertCurr = indices[j];
            Index2 vertNext = indices[k];

            int vCornerIdx = vertCurr.v;
            int vtCornerIdx = vertCurr.vt;

            vsNew[j] = Vec2.Mix(this.coords[vCornerIdx], vCenter, fac);
            vtsNew[j] = Vec2.Mix(this.texCoords[vtCornerIdx], vtCenter, fac);

            int vSubdivIdx = vsOldLen + j;
            int vtSubdivIdx = vtsOldLen + j;

            loopsNew[j] = new Loop2(
                new Index2(vCornerIdx, vtCornerIdx),
                new Index2(vertNext.v, vertNext.vt),
                new Index2(vsOldLen + k, vtsOldLen + k),
                new Index2(vSubdivIdx, vtSubdivIdx));

            centerLoop[j] = new Index2(vSubdivIdx, vtSubdivIdx);
        }

        this.coords = Vec2.Concat(this.coords, vsNew);
        this.texCoords = Vec2.Concat(this.texCoords, vtsNew);
        this.loops = Loop2.Splice(this.loops, i, 1, loopsNew);

        return (loopsNew, vsNew, vtsNew);
    }

    /// <summary>
    /// Reverses all the face loops in this mesh.
    /// </summary>
    /// <returns>this mesh</returns>
    public Mesh2 ReverseFaces()
    {
        int fsLen = this.loops.Length;
        for (int i = 0; i < fsLen; ++i)
        {
            this.loops[i].Reverse();
        }
        return this;
    }

    /// <summary>
    /// Scales this mesh by a uniform scalar.
    /// Scalar must not be zero.
    /// </summary>
    /// <param name="s">scalar</param>
    /// <returns>this mesh</returns>
    public Mesh2 Scale(in float s)
    {
        if (s != 0.0f)
        {
            int vsLen = this.coords.Length;
            for (int i = 0; i < vsLen; ++i) { this.coords[i] *= s; }
        }
        return this;
    }

    /// <summary>
    /// Scales this mesh by a nonuniform scalar.
    /// All components of the scalar must be nonzero.
    /// </summary>
    /// <param name="s">scalar</param>
    /// <returns>this mesh</returns>
    public Mesh2 Scale(in Vec2 s)
    {
        if (Vec2.All(s))
        {
            int vsLen = this.coords.Length;
            for (int i = 0; i < vsLen; ++i) { this.coords[i] *= s; }
        }
        return this;
    }

    /// <summary>
    /// Copies a source mesh by value.
    /// </summary>
    /// <param name="source">source mesh</param>
    /// <returns>this mesh</returns>
    public Mesh2 Set(in Mesh2 source)
    {
        // TODO: Test

        int vsLen = source.coords.Length;
        this.coords = new Vec2[vsLen];
        System.Array.Copy(source.coords, this.coords, vsLen);

        int vtsLen = source.texCoords.Length;
        this.texCoords = new Vec2[vtsLen];
        System.Array.Copy(source.texCoords, this.texCoords, vtsLen);

        int loopsLen = source.loops.Length;
        this.loops = new Loop2[loopsLen];
        for (int i = 0; i < loopsLen; ++i)
        {
            Index2[] sourceIndices = source.loops[i].Indices;
            int loopLen = sourceIndices.Length;
            Index2[] targetIndices = new Index2[loopLen];
            System.Array.Copy(sourceIndices, targetIndices, loopLen);
            this.loops[i] = new Loop2(targetIndices);
        }

        return this;
    }

    /// <summary>
    /// Subdivides a convex face by calculating its center, subdividing each of
    /// its edges with one cut to create a midpoint, then connecting the
    /// midpoints to the center. This generates a quadrilateral for the number
    /// of edges in the face. Returns a tuple containing the new data created.
    /// </summary>
    /// <param name="faceIndex">face index</param>
    /// <returns>tuple</returns>
    public (Loop2[] loopsNew,
            Vec2[] vsNew,
            Vec2[] vtsNew) SubdivFaceCenter(in int faceIndex = 0)
    {
        int facesLen = this.loops.Length;
        int i = Utils.RemFloor(faceIndex, facesLen);
        Index2[] face = this.loops[i].Indices;
        int faceLen = face.Length;

        int vsOldLen = this.coords.Length;
        int vtsOldLen = this.texCoords.Length;

        Vec2[] vsNew = new Vec2[faceLen + 1];
        Vec2[] vtsNew = new Vec2[vsNew.Length];
        Loop2[] fsNew = new Loop2[faceLen];

        int vCenterIdx = vsOldLen + faceLen;
        int vtCenterIdx = vtsOldLen + faceLen;

        Vec2 vCenter = new Vec2();
        Vec2 vtCenter = new Vec2();

        for (int j = 0; j < faceLen; ++j)
        {
            int k = (j + 1) % faceLen;
            Index2 vertCurr = face[j];
            Index2 vertNext = face[k];

            Vec2 vCurr = this.coords[vertCurr.v];
            Vec2 vtCurr = this.texCoords[vertCurr.vt];

            vCenter += vCurr;
            vtCenter += vtCurr;

            int vNextIdx = vertNext.v;
            int vtNextIdx = vertNext.vt;

            vsNew[j] = Vec2.Mix(vCurr, this.coords[vNextIdx]);
            vtsNew[j] = Vec2.Mix(vtCurr, this.texCoords[vtNextIdx]);

            fsNew[j] = new Loop2(
                new Index2(vCenterIdx, vtCenterIdx),
                new Index2(vsOldLen + j, vtsOldLen + j),
                new Index2(vNextIdx, vtNextIdx),
                new Index2(vsOldLen + k, vtsOldLen + k));
        }

        if (faceLen > 0)
        {
            float flInv = 1.0f / faceLen;
            vCenter *= flInv;
            vtCenter *= flInv;
        }

        vsNew[faceLen] = vCenter;
        vtsNew[faceLen] = vtCenter;

        this.coords = Vec2.Concat(this.coords, vsNew);
        this.texCoords = Vec2.Concat(this.texCoords, vtsNew);
        this.loops = Loop2.Splice(this.loops, i, 1, fsNew);

        return (loopsNew: fsNew,
            vsNew: vsNew,
            vtsNew: vtsNew);
    }

    /// <summary>
    /// Subdivides a convex face by calculating its center, then connecting its
    /// vertices to the center. This generates a triangle for the number of
    /// edges in the face.
    /// </summary>
    /// <param name="faceIndex">the face index</param>
    /// <returns>new data</returns>
    public (Loop2[] loopsNew,
            Vec2[] vsNew,
            Vec2[] vtsNew) SubdivFaceFan(in int faceIndex = 0)
    {
        int facesLen = this.loops.Length;
        int i = Utils.RemFloor(faceIndex, facesLen);
        Index2[] face = this.loops[i].Indices;
        int faceLen = face.Length;

        Loop2[] fsNew = new Loop2[faceLen];
        Vec2 vCenter = Vec2.Zero;
        Vec2 vtCenter = Vec2.Zero;

        int vCenterIdx = this.coords.Length;
        int vtCenterIdx = this.texCoords.Length;

        for (int j = 0; j < faceLen; ++j)
        {
            int k = (j + 1) % faceLen;

            Index2 vertCurr = face[j];
            Index2 vertNext = face[k];

            int vCurrIdx = vertCurr.v;
            int vtCurrIdx = vertCurr.vt;

            Vec2 vCurr = this.coords[vCurrIdx];
            Vec2 vtCurr = this.texCoords[vtCurrIdx];

            vCenter += vCurr;
            vtCenter += vtCurr;

            fsNew[j] = new Loop2(
                new Index2(vCenterIdx, vtCenterIdx),
                new Index2(vCurrIdx, vtCurrIdx),
                new Index2(vertNext.v, vertNext.vt));
        }

        if (faceLen > 0)
        {
            float flInv = 1.0f / faceLen;
            vCenter *= flInv;
            vtCenter *= flInv;
        }

        this.coords = Vec2.Append(this.coords, vCenter);
        this.texCoords = Vec2.Append(this.texCoords, vtCenter);
        this.loops = Loop2.Splice(this.loops, i, 1, fsNew);

        return (loopsNew: fsNew,
            vsNew: new Vec2[] { vCenter },
            vtsNew: new Vec2[] { vtCenter });
    }

    /// <summary>
    /// Subdivides a convex face by cutting each of its edges once to create a
    /// midpoint, then connecting each midpoint. This generates peripheral
    /// triangles and a new central face with the same number of edges as
    /// original. This is best suited to meshes made of triangles. Returns a
    /// tuple containing the new data created.
    /// </summary>
    /// <param name="faceIndex">face index</param>
    /// <returns>tuple</returns>
    public (Loop2[] loopsNew,
            Vec2[] vsNew,
            Vec2[] vtsNew) SubdivFaceInscribe(in int faceIndex = 0)
    {
        int facesLen = this.loops.Length;
        int i = Utils.RemFloor(faceIndex, facesLen);
        Index2[] face = this.loops[i].Indices;
        int faceLen = face.Length;

        int vsOldLen = this.coords.Length;
        int vtsOldLen = this.texCoords.Length;

        Vec2[] vsNew = new Vec2[faceLen];
        Vec2[] vtsNew = new Vec2[faceLen];
        Loop2[] fsNew = new Loop2[faceLen + 1];
        Index2[] cfIdcs = new Index2[3];

        for (int j = 0; j < faceLen; ++j)
        {
            int k = (j + 1) % faceLen;
            Index2 vertCurr = face[j];
            Index2 vertNext = face[k];

            int vNextIdx = vertNext.v;
            int vtNextIdx = vertNext.vt;

            vsNew[j] = Vec2.Mix(
                this.coords[vertCurr.v],
                this.coords[vNextIdx]);
            vtsNew[j] = Vec2.Mix(
                this.texCoords[vertCurr.vt],
                this.texCoords[vtNextIdx]);

            int vSubdivIdx = vsOldLen + j;
            int vtSubdivIdx = vtsOldLen + j;

            fsNew[j] = new Loop2(
                new Index2(vSubdivIdx, vtSubdivIdx),
                new Index2(vNextIdx, vtNextIdx),
                new Index2(vsOldLen + k, vtsOldLen + k));

            cfIdcs[j] = new Index2(vSubdivIdx, vtSubdivIdx);
        }

        // Center face.
        fsNew[faceLen] = new Loop2(cfIdcs);

        this.coords = Vec2.Concat(this.coords, vsNew);
        this.texCoords = Vec2.Concat(this.texCoords, vtsNew);
        this.loops = Loop2.Splice(this.loops, i, 1, fsNew);

        return (loopsNew: fsNew,
            vsNew: vsNew,
            vtsNew: vtsNew);
    }

    /// <summary>
    /// Subdivides all faces in the mesh by a number of iterations. Uses the
    /// center method.
    /// </summary>
    /// <param name="itr">iterations</param>
    /// <returns>this mesh</returns>
    public (Loop2[] loopsNew,
            Vec2[] vsNew,
            Vec2[] vtsNew) SubdivFacesCenter(in int itr = 1)
    {
        List<Loop2> loopsNew = new();
        List<Vec2> vsNew = new();
        List<Vec2> vtsNew = new();

        for (int i = 0; i < itr; ++i)
        {
            int k = 0;
            int len = this.loops.Length;
            for (int j = 0; j < len; ++j)
            {
                int vertLen = this.loops[k].Length;
                var result = this.SubdivFaceCenter(k);

                loopsNew.AddRange(result.loopsNew);
                vsNew.AddRange(result.vsNew);
                vtsNew.AddRange(result.vtsNew);

                k += vertLen;
            }
        }

        return (loopsNew: loopsNew.ToArray(),
            vsNew: vsNew.ToArray(),
            vtsNew: vtsNew.ToArray());
    }

    /// <summary>
    /// Subdivides all faces in the mesh by a number of iterations. Uses the
    /// triangle fan method.
    /// </summary>
    /// <param name="itr">iterations</param>
    /// <returns>this mesh</returns>
    public (Loop2[] loopsNew,
            Vec2[] vsNew,
            Vec2[] vtsNew) SubdivFacesFan(in int itr = 1)
    {
        List<Loop2> loopsNew = new();
        List<Vec2> vsNew = new();
        List<Vec2> vtsNew = new();

        for (int i = 0; i < itr; ++i)
        {
            int k = 0;
            int len = this.loops.Length;
            for (int j = 0; j < len; ++j)
            {
                int vertLen = this.loops[k].Length;
                var result = this.SubdivFaceFan(k);

                loopsNew.AddRange(result.loopsNew);
                vsNew.AddRange(result.vsNew);
                vtsNew.AddRange(result.vtsNew);

                k += vertLen;
            }
        }

        return (loopsNew: loopsNew.ToArray(),
            vsNew: vsNew.ToArray(),
            vtsNew: vtsNew.ToArray());
    }

    /// <summary>
    /// Subdivides all faces in the mesh by a number of iterations. Uses the
    /// inscription method.
    /// </summary>
    /// <param name="itr">iterations</param>
    /// <returns>this mesh</returns>
    public (Loop2[] loopsNew,
            Vec2[] vsNew,
            Vec2[] vtsNew) SubdivFacesInscribe(in int itr = 1)
    {
        List<Loop2> loopsNew = new();
        List<Vec2> vsNew = new();
        List<Vec2> vtsNew = new();

        for (int i = 0; i < itr; ++i)
        {
            int k = 0;
            int len = this.loops.Length;
            for (int j = 0; j < len; ++j)
            {
                int vertLen = this.loops[k].Length;
                var result = this.SubdivFaceInscribe(k);

                loopsNew.AddRange(result.loopsNew);
                vsNew.AddRange(result.vsNew);
                vtsNew.AddRange(result.vtsNew);

                k += vertLen + 1;
            }
        }

        return (loopsNew: loopsNew.ToArray(),
            vsNew: vsNew.ToArray(),
            vtsNew: vtsNew.ToArray());
    }

    /// <summary>
    /// Transforms a mesh by an affine transform matrix.
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>this mesh</returns>
    public Mesh2 Transform(in Mat3 m)
    {
        int vsLen = this.coords.Length;
        for (int i = 0; i < vsLen; ++i)
        {
            this.coords[i] = Mat3.MulPoint(m, this.coords[i]);
        }

        return this;
    }

    /// <summary>
    /// Transforms a mesh by a transform
    /// </summary>
    /// <param name="t">transform</param>
    /// <returns>this mesh</returns>
    public Mesh2 Transform(in Transform2 t)
    {
        int vsLen = this.coords.Length;
        for (int i = 0; i < vsLen; ++i)
        {
            this.coords[i] = Transform2.MulPoint(t, this.coords[i]);
        }

        return this;
    }

    /// <summary>
    /// Moves this mesh's coordinates by a vector.
    /// </summary>
    /// <param name="t">translation</param>
    /// <returns>this mesh</returns>
    public Mesh2 Translate(in Vec2 t)
    {
        int vsLen = this.coords.Length;
        for (int i = 0; i < vsLen; ++i) { this.coords[i] += t; }

        return this;
    }

    /// <summary>
    /// Creates an arc from a start angle to a stop angle.
    /// If the start and stop angle are approximately equal,
    /// returns an inset polygon with the interior face removed.
    /// </summary>
    /// <param name="target">target mesh</param>
    /// <param name="sectors">number of sides</param>
    /// <param name="radius">radius</param>
    /// <param name="oculus">oculus</param>
    /// <param name="startAngle">start angle</param>
    /// <param name="stopAngle">stop angle</param>
    /// <param name="poly">polygon type</param>
    /// <returns>polygon</returns>
    public static Mesh2 Arc(
        in Mesh2 target,
        in int sectors = 32,
        in float radius = 0.5f,
        float oculus = 0.5f,
        in float startAngle = 0.0f,
        in float stopAngle = MathF.PI,
        in PolyType poly = PolyType.Tri)
    {
        float a1 = Utils.WrapRadians(startAngle) * Utils.OneTau;
        float b1 = Utils.WrapRadians(stopAngle) * Utils.OneTau;
        float arcLen1 = Utils.RemFloor(b1 - a1, 1.0f);
        float oculFac = Utils.Clamp(oculus, Utils.Epsilon, 1.0f - Utils.Epsilon);

        // 1.0 / 720.0 = 0.001388889f 
        if (arcLen1 < 0.00139f)
        {
            Mesh2.Polygon(
                target: target,
                sectors: sectors,
                radius: radius,
                rotation: startAngle,
                poly: PolyType.Ngon);
            target.InsetFace(0, oculFac);
            target.DeleteFaces(-1, 1);
            return target;
        }

        int sctCount = Utils.Ceil(1.0f + MathF.Max(3.0f, sectors) * arcLen1);
        int sctCount2 = sctCount + sctCount;

        Vec2[] vs = target.coords = Vec2.Resize(target.coords, sctCount2);
        Vec2[] vts = target.texCoords = Vec2.Resize(target.texCoords,
            sctCount2);

        float rad = MathF.Max(Utils.Epsilon, radius);
        float oculRad = oculFac * rad;
        float oculRadVt = oculFac * 0.5f;

        float toStep = 1.0f / (sctCount - 1.0f);
        float origAngle = Utils.Tau * a1;
        float destAngle = Utils.Tau * (a1 + arcLen1);

        for (int k = 0, i = 0, j = 1; k < sctCount; ++k, i += 2, j += 2)
        {
            float step = k * toStep;
            float theta = Utils.Mix(origAngle, destAngle, step);
            float cosTheta = MathF.Cos(theta);
            float sinTheta = MathF.Sin(theta);

            vs[i] = new Vec2(
                cosTheta * rad,
                sinTheta * rad);

            vs[j] = new Vec2(
                cosTheta * oculRad,
                sinTheta * oculRad);

            vts[i] = new Vec2(
                cosTheta * 0.5f + 0.5f,
                0.5f - sinTheta * 0.5f);

            vts[j] = new Vec2(
                cosTheta * oculRadVt + 0.5f,
                0.5f - sinTheta * oculRadVt);
        }

        int len;
        switch (poly)
        {
            case PolyType.Ngon:
                {
                    len = sctCount2;
                    int last = len - 1;
                    target.loops = new Loop2[] { new Loop2(len) };
                    Loop2 indices = target.loops[0];

                    for (int i = 0, j = 0; i < sctCount; ++i, j += 2)
                    {
                        int k = sctCount + i;
                        int m = last - j;
                        indices[i] = new Index2(j, j);
                        indices[k] = new Index2(m, m);
                    }
                }
                break;
            case PolyType.Quad:
                {
                    len = sctCount - 1;
                    target.loops = Loop2.Resize(target.loops, len, 4, true);

                    for (int k = 0, i = 0, j = 1; k < len; ++k, i += 2, j += 2)
                    {
                        int m = i + 2;
                        int n = j + 2;

                        Loop2.Quad(
                            new Index2(i, i),
                            new Index2(m, m),
                            new Index2(n, n),
                            new Index2(j, j),
                            target.loops[k]);
                    }
                }
                break;
            case PolyType.Tri:
            default:
                {
                    len = sctCount2 - 2;
                    target.loops = Loop2.Resize(target.loops, len, 3, true);

                    for (int i = 0, j = 1; i < len; i += 2, j += 2)
                    {
                        int m = i + 2;
                        int n = j + 2;

                        Loop2.Tri(
                            new Index2(i, i),
                            new Index2(m, m),
                            new Index2(j, j),
                            target.loops[i]);

                        Loop2.Tri(
                            new Index2(m, m),
                            new Index2(n, n),
                            new Index2(j, j),
                            target.loops[j]);
                    }
                }
                break;
        }

        return target;

    }

    /// <summary>
    /// Finds the axis aligned bounding box for a mesh.
    /// </summary>
    /// <param name="mesh">mesh</param>
    /// <returns>bounds</returns>
    public static Bounds2 CalcBounds(in Mesh2 mesh)
    {
        Vec2[] coords = mesh.coords;
        int len = coords.Length;
        float lbx = float.MaxValue;
        float lby = float.MaxValue;
        float ubx = float.MinValue;
        float uby = float.MinValue;
        for (int i = 0; i < len; ++i)
        {
            Vec2 coord = coords[i];
            float x = coord.x;
            float y = coord.y;
            if (x < lbx) { lbx = x; }
            if (x > ubx) { ubx = x; }
            if (y < lby) { lby = y; }
            if (y > uby) { uby = y; }
        }
        return new Bounds2(lbx, lby, ubx, uby);
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
    public static Mesh2 GridHex(
        in Mesh2 target,
        in int rings = 4,
        in float cellRadius = 0.5f,
        in float cellMargin = 0.0325f)
    {
        int vRings = Utils.Max(1, rings);
        float vRad = MathF.Max(Utils.Epsilon, cellRadius);

        float extent = Utils.Sqrt3 * vRad;
        float halfExt = extent * 0.5f;

        float rad15 = vRad * 1.5f;
        float padRad = MathF.Max(Utils.Epsilon, vRad - cellMargin);
        float halfRad = padRad * 0.5f;
        float radrt32 = padRad * Utils.Sqrt32;

        int iMax = vRings - 1;
        int iMin = -iMax;

        target.texCoords = new Vec2[]
        {
            new Vec2 (0.5f, 1.0f),
            new Vec2 (0.0669873f, 0.75f),
            new Vec2 (0.0669873f, 0.25f),
            new Vec2 (0.5f, 0.0f),
            new Vec2 (0.9330127f, 0.25f),
            new Vec2 (0.9330127f, 0.75f)
        };

        int fsLen = 1 + iMax * vRings * 3;
        Vec2[] vs = target.coords = Vec2.Resize(target.coords, fsLen * 6);
        Loop2[] fs = target.loops = Loop2.Resize(target.loops, fsLen, 6, true);

        int vIdx = 0;
        int fIdx = 0;
        for (int i = iMin; i <= iMax; ++i)
        {
            int jMin = Utils.Max(iMin, iMin - i);
            int jMax = Utils.Min(iMax, iMax - i);
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

                vs[vIdx] = new Vec2(x, y + padRad);
                vs[vIdx + 1] = new Vec2(left, top);
                vs[vIdx + 2] = new Vec2(left, bottom);
                vs[vIdx + 3] = new Vec2(x, y - padRad);
                vs[vIdx + 4] = new Vec2(right, bottom);
                vs[vIdx + 5] = new Vec2(right, top);

                Loop2.Hex(
                    new Index2(vIdx, 0),
                    new Index2(vIdx + 1, 1),
                    new Index2(vIdx + 2, 2),
                    new Index2(vIdx + 3, 3),
                    new Index2(vIdx + 4, 4),
                    new Index2(vIdx + 5, 5),
                    fs[fIdx]);

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
    public static Mesh2 Plane(
        in Mesh2 target,
        in int cols = 3,
        in int rows = 3,
        in PolyType poly = PolyType.Tri)
    {
        int rVal = Utils.Max(1, rows);
        int cVal = Utils.Max(1, cols);
        int rVal1 = rVal + 1;
        int cVal1 = cVal + 1;
        int fLen = rVal * cVal;
        int fLen1 = rVal1 * cVal1;

        Vec2[] vs = target.coords = Vec2.Resize(target.coords, fLen1);
        Vec2[] vts = target.texCoords = Vec2.Resize(target.texCoords, fLen1);

        // Set coordinates and texture coordinates.
        float iToStep = 1.0f / rVal;
        float jToStep = 1.0f / cVal;
        for (int k = 0; k < fLen1; ++k)
        {
            float iStep = k / cVal1 * iToStep;
            float jStep = k % cVal1 * jToStep;
            vs[k] = new Vec2(jStep - 0.5f, iStep - 0.5f);
            vts[k] = new Vec2(jStep, 1.0f - iStep);
        }

        switch (poly)
        {
            case PolyType.Ngon:
            case PolyType.Quad:
                {
                    target.loops = Loop2.Resize(target.loops, fLen, 4, true);
                    for (int k = 0; k < fLen; ++k)
                    {
                        int i = k / cVal;
                        int j = k % cVal;

                        int cOff0 = i * cVal1;
                        int c00 = cOff0 + j;
                        int c10 = c00 + 1;
                        int c01 = cOff0 + cVal1 + j;
                        int c11 = c01 + 1;

                        Loop2.Quad(
                            new Index2(c00, c00),
                            new Index2(c10, c10),
                            new Index2(c11, c11),
                            new Index2(c01, c01),
                            target.loops[k]);
                    }
                }
                break;
            case PolyType.Tri:
            default:
                {
                    target.loops = Loop2.Resize(target.loops, fLen * 2, 3, true);
                    for (int m = 0, k = 0; k < fLen; ++k, m += 2)
                    {
                        int i = k / cVal;
                        int j = k % cVal;

                        int cOff0 = i * cVal1;
                        int c00 = cOff0 + j;
                        int c10 = c00 + 1;
                        int c01 = cOff0 + cVal1 + j;
                        int c11 = c01 + 1;

                        Loop2.Tri(
                            new Index2(c00, c00),
                            new Index2(c10, c10),
                            new Index2(c11, c11),
                            target.loops[m]);

                        Loop2.Tri(
                            new Index2(c11, c11),
                            new Index2(c01, c01),
                            new Index2(c00, c00),
                            target.loops[m + 1]);
                    }
                }
                break;
        }

        return target;
    }

    /// <summary>
    /// Creates a regular convex polygon.
    /// </summary>
    /// <param name="target">target mesh</param>
    /// <param name="sectors">number of sides</param>
    /// <param name="radius">radius</param>
    /// <param name="rotation">rotation</param>
    /// <param name="poly">polygon type</param>
    /// <returns>polygon</returns>
    public static Mesh2 Polygon(
        in Mesh2 target,
        in int sectors = 32,
        in float radius = 0.5f,
        float rotation = Utils.HalfPi,
        in PolyType poly = PolyType.Tri)
    {
        int seg = Utils.Max(3, sectors);
        int newLen = poly == PolyType.Ngon ? seg : poly == PolyType.Quad ?
            seg + seg + 1 : seg + 1;
        float rad = MathF.Max(Utils.Epsilon, radius);
        float offset = Utils.WrapRadians(rotation);
        float toTheta = Utils.Tau / seg;

        Vec2[] vs = target.coords = Vec2.Resize(target.coords, newLen);
        Vec2[] vts = target.texCoords = Vec2.Resize(target.texCoords,
            newLen);

        switch (poly)
        {
            case PolyType.Ngon:
                {
                    target.loops = Loop2.Resize(target.loops, 1, seg, true);

                    for (int i = 0; i < seg; ++i)
                    {
                        float theta = offset + i * toTheta;
                        float cosTheta = MathF.Cos(theta);
                        float sinTheta = MathF.Sin(theta);
                        vs[i] = new Vec2(
                            rad * cosTheta,
                            rad * sinTheta);
                        vts[i] = new Vec2(
                            cosTheta * 0.5f + 0.5f,
                            0.5f - sinTheta * 0.5f);
                        target.loops[0][i] = new Index2(i, i);
                    }
                }
                break;
            case PolyType.Quad:
                {
                    target.loops = Loop2.Resize(target.loops, seg, 4, true);

                    vs[0] = Vec2.Zero;
                    vts[0] = Vec2.UvCenter;

                    // Find corners.
                    for (int i = 0, j = 1; i < seg; ++i, j += 2)
                    {
                        float theta = offset + i * toTheta;
                        float cosTheta = MathF.Cos(theta);
                        float sinTheta = MathF.Sin(theta);
                        vs[j] = new Vec2(
                            rad * cosTheta,
                            rad * sinTheta);
                        vts[j] = new Vec2(
                            cosTheta * 0.5f + 0.5f,
                            0.5f - sinTheta * 0.5f);
                    }

                    // Find midpoints.
                    int last = newLen - 1;
                    for (int i = 0, j = 1, k = 2; i < seg; ++i, j += 2, k += 2)
                    {
                        int m = (j + 2) % last;
                        vs[k] = Vec2.Mix(vs[j], vs[m]);
                        vts[k] = Vec2.Mix(vts[j], vts[m]);
                    }

                    // Find faces.
                    for (int i = 0, j = 0; i < seg; ++i, j += 2)
                    {
                        int s = 1 + Utils.RemFloor(j - 1, last);
                        int t = 1 + j % last;
                        int u = 1 + (j + 1) % last;

                        Loop2.Quad(
                            new Index2(0, 0),
                            new Index2(s, s),
                            new Index2(t, t),
                            new Index2(u, u),
                            target.loops[i]);
                    }
                }
                break;
            case PolyType.Tri:
            default:
                {
                    target.loops = Loop2.Resize(target.loops, seg, 3, true);

                    vs[0] = Vec2.Zero;
                    vts[0] = Vec2.UvCenter;

                    for (int i = 0, j = 1; i < seg; ++i, ++j)
                    {
                        int k = 1 + j % seg;
                        float theta = i * toTheta;
                        float cosTheta = MathF.Cos(theta);
                        float sinTheta = MathF.Sin(theta);
                        vs[j] = new Vec2(
                            rad * cosTheta,
                            rad * sinTheta);
                        vts[j] = new Vec2(
                            cosTheta * 0.5f + 0.5f,
                            0.5f - sinTheta * 0.5f);

                        Loop2.Tri(
                            new Index2(0, 0),
                            new Index2(j, j),
                            new Index2(k, k),
                            target.loops[i]);
                    }
                }
                break;
        }

        return target;
    }

    /// <summary>
    /// Creates a rounded rectangle. The rounding is a factor,
    /// expected to fall in the range [0.0, 1.0].
    /// </summary>
    /// <param name="target">target mesh</param>
    /// <param name="lb">lower bound</param>
    /// <param name="ub">upper bound</param>
    /// <param name="rnd">rounding</param>
    /// <param name="res">resolution</param>
    /// <param name="poly">polygon type</param>
    /// <param name="profile">UV Profile</param>
    /// <returns>rectangle</returns>
    public static Mesh2 Rect(
        in Mesh2 target,
        in Vec2 lb, in Vec2 ub,
        in float rnd = 0.25f,
        in int res = 8,
        in PolyType poly = PolyType.Tri,
        in UvProfiles.Rect profile = UvProfiles.Rect.Stretch)
    {
        return Mesh2.Rect(target,
            lb, ub,
            rnd, rnd, rnd, rnd,
            res, res, res, res,
            poly, profile);
    }

    /// <summary>
    /// Creates a rounded rectangle. The rounding is a factor,
    /// expected to fall in the range [0.0, 1.0].
    /// </summary>
    /// <param name="target">target mesh</param>
    /// <param name="lb">lower bound</param>
    /// <param name="ub">upper bound</param>
    /// <param name="tl">top left corner rounding</param>
    /// <param name="tr">top right corner rounding</param>
    /// <param name="br">bottom right corner rounding</param>
    /// <param name="bl">bottom left corner rounding</param>
    /// <param name="tlRes">top left resolution</param>
    /// <param name="trRes">top right resolution</param>
    /// <param name="brRes">bottom right resolution</param>
    /// <param name="blRes">bottom left resolution</param>
    /// <param name="poly">polygon type</param>
    /// <param name="profile">UV Profile</param>
    /// <returns>rectangle</returns>
    public static Mesh2 Rect(
        in Mesh2 target,
        in Vec2 lb, in Vec2 ub,
        in float tl = 0.25f, in float tr = 0.25f,
        in float br = 0.25f, in float bl = 0.25f,
        in int tlRes = 8, in int trRes = 8,
        in int brRes = 8, in int blRes = 8,
        in PolyType poly = PolyType.Tri,
        in UvProfiles.Rect profile = UvProfiles.Rect.Stretch)
    {
        // Validate corners.
        float lft = MathF.Min(lb.x, ub.x);
        float rgt = MathF.Max(lb.x, ub.x);
        float btm = MathF.Min(lb.y, ub.y);
        float top = MathF.Max(lb.y, ub.y);

        // Protect from zero dimension meshes.
        float w = rgt - lft;
        float h = top - btm;
        bool wInval = w < Utils.Epsilon;
        bool hInval = h < Utils.Epsilon;
        if (wInval && hInval)
        {
            float cx = (lft + rgt) * 0.5f;
            float cy = (top + btm) * 0.5f;
            lft = cx - 0.5f;
            rgt = cx + 0.5f;
            btm = cy - 0.5f;
            top = cy + 0.5f;
            w = 1.0f;
            h = 1.0f;
        }
        else if (wInval)
        {
            // Use vertical proportions.
            float cx = (lft + rgt) * 0.5f;
            float hHalf = h * 0.5f;
            lft = cx - hHalf;
            rgt = cx + hHalf;
            w = h;
        }
        else if (hInval)
        {
            // Use horizontal proportions.
            float cy = (top + btm) * 0.5f;
            float wHalf = w * 0.5f;
            btm = cy - wHalf;
            top = cy + wHalf;
            h = w;
        }

        // Calculate width and height for vts.
        float wInv = 1.0f / w;
        float hInv = 1.0f / h;

        // Calculate UV scalar.
        float uScl = 1.0f;
        float vScl = 1.0f;
        if (profile == UvProfiles.Rect.Contain)
        {
            if (w < h) { uScl = w / h; }
            else if (w > h) { vScl = h / w; }
        }
        else if (profile == UvProfiles.Rect.Cover)
        {
            if (w < h) { vScl = h / w; }
            else if (w > h) { uScl = w / h; }
        }

        // Validate corner insetting factor.
        float vtlFac = MathF.Min(MathF.Abs(tl), 1.0f - Utils.Epsilon);
        float vtrFac = MathF.Min(MathF.Abs(tr), 1.0f - Utils.Epsilon);
        float vbrFac = MathF.Min(MathF.Abs(br), 1.0f - Utils.Epsilon);
        float vblFac = MathF.Min(MathF.Abs(bl), 1.0f - Utils.Epsilon);

        // Booleans to store whether the corner is round.
        bool tlIsRnd = vtlFac > 0.0f;
        bool trIsRnd = vtrFac > 0.0f;
        bool brIsRnd = vbrFac > 0.0f;
        bool blIsRnd = vblFac > 0.0f;

        // Half the short edge is the maximum size.
        // If the corner insetting is zero, then push
        // the insets in by 25 percent.
        float se = 0.5f * MathF.Min(w, h);
        float vtl = se * (tlIsRnd ? vtlFac : 0.25f);
        float vtr = se * (trIsRnd ? vtrFac : 0.25f);
        float vbr = se * (brIsRnd ? vbrFac : 0.25f);
        float vbl = se * (blIsRnd ? vblFac : 0.25f);

        // Validate corner resolution.
        int vtlRes = tlIsRnd ? Utils.Max(tlRes, 0) : 1;
        int vtrRes = trIsRnd ? Utils.Max(trRes, 0) : 1;
        int vbrRes = brIsRnd ? Utils.Max(brRes, 0) : 1;
        int vblRes = blIsRnd ? Utils.Max(blRes, 0) : 1;

        // Calculate insets.
        float btmIns0 = btm + vbr;
        float topIns0 = top - vtr;
        float rgtIns0 = rgt - vtr;
        float lftIns0 = lft + vtl;
        float topIns1 = top - vtl;
        float btmIns1 = btm + vbl;
        float lftIns1 = lft + vbl;
        float rgtIns1 = rgt - vbr;

        // Resize arrays of data.
        int vsLen = 8 + vtlRes + vblRes + vbrRes + vtrRes;
        if (poly != PolyType.Ngon) { vsLen += 4; }

        Vec2[] vs = target.coords = Vec2.Resize(target.coords, vsLen);
        Vec2[] vts = target.texCoords = Vec2.Resize(target.texCoords, vsLen);

        // Calculate vertex index offsets.
        int tlCrnrIdxStr = 0;
        int tlCrnrIdxEnd = tlCrnrIdxStr + 1 + vtlRes;
        int blCrnrIdxStr = tlCrnrIdxEnd + 1;
        int blCrnrIdxEnd = blCrnrIdxStr + 1 + vblRes;
        int brCrnrIdxStr = blCrnrIdxEnd + 1;
        int brCrnrIdxEnd = brCrnrIdxStr + 1 + vbrRes;
        int trCrnrIdxStr = brCrnrIdxEnd + 1;
        int trCrnrIdxEnd = trCrnrIdxStr + 1 + vtrRes;

        // Coordinate corners at start and end of arc.
        vs[tlCrnrIdxStr] = new Vec2(lftIns0, top);
        vs[tlCrnrIdxEnd] = new Vec2(lft, topIns1);
        vs[blCrnrIdxStr] = new Vec2(lft, btmIns1);
        vs[blCrnrIdxEnd] = new Vec2(lftIns1, btm);
        vs[brCrnrIdxStr] = new Vec2(rgtIns1, btm);
        vs[brCrnrIdxEnd] = new Vec2(rgt, btmIns0);
        vs[trCrnrIdxStr] = new Vec2(rgt, topIns0);
        vs[trCrnrIdxEnd] = new Vec2(rgtIns0, top);

        // Texture coordinate corners at start and end of arc.
        float u0 = vtl * wInv;
        float v0 = 1.0f;
        float u1 = 0.0f;
        float v1 = (topIns1 - btm) * hInv;
        float u2 = 0.0f;
        float v2 = vbl * hInv;
        float u3 = vbl * wInv;
        float v3 = 0.0f;
        float u4 = (rgtIns1 - lft) * wInv;
        float v4 = 0.0f;
        float u5 = 1.0f;
        float v5 = vbr * hInv;
        float u6 = 1.0f;
        float v6 = (topIns0 - btm) * hInv;
        float u7 = (rgtIns0 - lft) * wInv;
        float v7 = 1.0f;

        vts[tlCrnrIdxStr] = new Vec2(
            (u0 - 0.5f) * uScl + 0.5f,
            1.0f - ((v0 - 0.5f) * vScl + 0.5f));
        vts[tlCrnrIdxEnd] = new Vec2(
            (u1 - 0.5f) * uScl + 0.5f,
            1.0f - ((v1 - 0.5f) * vScl + 0.5f));
        vts[blCrnrIdxStr] = new Vec2(
            (u2 - 0.5f) * uScl + 0.5f,
            1.0f - ((v2 - 0.5f) * vScl + 0.5f));
        vts[blCrnrIdxEnd] = new Vec2(
            (u3 - 0.5f) * uScl + 0.5f,
            1.0f - ((v3 - 0.5f) * vScl + 0.5f));
        vts[brCrnrIdxStr] = new Vec2(
            (u4 - 0.5f) * uScl + 0.5f,
            1.0f - ((v4 - 0.5f) * vScl + 0.5f));
        vts[brCrnrIdxEnd] = new Vec2(
            (u5 - 0.5f) * uScl + 0.5f,
            1.0f - ((v5 - 0.5f) * vScl + 0.5f));
        vts[trCrnrIdxStr] = new Vec2(
            (u6 - 0.5f) * uScl + 0.5f,
            1.0f - ((v6 - 0.5f) * vScl + 0.5f));
        vts[trCrnrIdxEnd] = new Vec2(
            (u7 - 0.5f) * uScl + 0.5f,
            1.0f - ((v7 - 0.5f) * vScl + 0.5f));

        // Find conversion from resolution to theta.
        float tlToTheta = Utils.HalfPi / (vtlRes + 1.0f);
        float blToTheta = Utils.HalfPi / (vblRes + 1.0f);
        float brToTheta = Utils.HalfPi / (vbrRes + 1.0f);
        float trToTheta = Utils.HalfPi / (vtrRes + 1.0f);

        // Top left corner. Reverse theta progress.
        if (tlIsRnd)
        {
            for (int i = 0; i < vtlRes; ++i)
            {
                int j = vtlRes - 1 - i;
                float theta = (j + 1.0f) * tlToTheta;
                float x = lftIns0 - vtl * MathF.Cos(theta);
                float y = topIns1 + vtl * MathF.Sin(theta);
                float u = (x - lft) * wInv;
                float v = (y - btm) * hInv;
                u = (u - 0.5f) * uScl + 0.5f;
                v = (v - 0.5f) * vScl + 0.5f;
                v = 1.0f - v;
                vs[tlCrnrIdxStr + 1 + i] = new Vec2(x, y);
                vts[tlCrnrIdxStr + 1 + i] = new Vec2(u, v);
            }
        }
        else
        {
            float u = 0.0f;
            float v = 1.0f;
            u = (u - 0.5f) * uScl + 0.5f;
            v = (v - 0.5f) * vScl + 0.5f;
            v = 1.0f - v;
            vs[tlCrnrIdxStr + 1] = new Vec2(lft, top);
            vts[tlCrnrIdxStr + 1] = new Vec2(u, v);
        }

        // Bottom left corner.
        if (blIsRnd)
        {
            for (int i = 0; i < vblRes; ++i)
            {
                float theta = (i + 1.0f) * blToTheta;
                float x = lftIns1 - vbl * MathF.Cos(theta);
                float y = btmIns1 - vbl * MathF.Sin(theta);
                float u = (x - lft) * wInv;
                float v = (y - btm) * hInv;
                u = (u - 0.5f) * uScl + 0.5f;
                v = (v - 0.5f) * vScl + 0.5f;
                v = 1.0f - v;
                vs[blCrnrIdxStr + 1 + i] = new Vec2(x, y);
                vts[blCrnrIdxStr + 1 + i] = new Vec2(u, v);
            }
        }
        else
        {
            float u = 0.0f;
            float v = 0.0f;
            u = (u - 0.5f) * uScl + 0.5f;
            v = (v - 0.5f) * vScl + 0.5f;
            v = 1.0f - v;
            vs[blCrnrIdxStr + 1] = new Vec2(lft, btm);
            vts[blCrnrIdxStr + 1] = new Vec2(u, v);
        }

        // Bottom right corner. Reverse theta progress.
        if (brIsRnd)
        {
            for (int i = 0; i < vbrRes; ++i)
            {
                int j = vbrRes - 1 - i;
                float theta = (j + 1.0f) * brToTheta;
                float x = rgtIns1 + vbr * MathF.Cos(theta);
                float y = btmIns0 - vbr * MathF.Sin(theta);
                float u = (x - lft) * wInv;
                float v = (y - btm) * hInv;
                u = (u - 0.5f) * uScl + 0.5f;
                v = (v - 0.5f) * vScl + 0.5f;
                v = 1.0f - v;
                vs[brCrnrIdxStr + 1 + i] = new Vec2(x, y);
                vts[brCrnrIdxStr + 1 + i] = new Vec2(u, v);
            }
        }
        else
        {
            float u = 1.0f;
            float v = 0.0f;
            u = (u - 0.5f) * uScl + 0.5f;
            v = (v - 0.5f) * vScl + 0.5f;
            v = 1.0f - v;
            vs[brCrnrIdxStr + 1] = new Vec2(rgt, btm);
            vts[brCrnrIdxStr + 1] = new Vec2(u, v);
        }

        // Top right corner.
        if (trIsRnd)
        {
            for (int i = 0; i < vtrRes; ++i)
            {
                float theta = (i + 1.0f) * trToTheta;
                float x = rgtIns0 + vtr * MathF.Cos(theta);
                float y = topIns0 + vtr * MathF.Sin(theta);
                float u = (x - lft) * wInv;
                float v = (y - btm) * hInv;
                u = (u - 0.5f) * uScl + 0.5f;
                v = (v - 0.5f) * vScl + 0.5f;
                v = 1.0f - v;
                vs[trCrnrIdxStr + 1 + i] = new Vec2(x, y);
                vts[trCrnrIdxStr + 1 + i] = new Vec2(u, v);
            }
        }
        else
        {
            float u = 1.0f;
            float v = 1.0f;
            u = (u - 0.5f) * uScl + 0.5f;
            v = (v - 0.5f) * vScl + 0.5f;
            v = 1.0f - v;
            vs[trCrnrIdxStr + 1] = new Vec2(rgt, top);
            vts[trCrnrIdxStr + 1] = new Vec2(u, v);
        }

        if (poly == PolyType.Ngon)
        {
            Loop2[] fs = target.loops = Loop2.Resize(target.loops, 1, vsLen, true);
            Loop2 loop = fs[0];
            for (int i = 0; i < vsLen; ++i)
            {
                loop.Indices[i] = new Index2(i, i);
            }
        }
        else
        {
            // Insert inner vertices for quad and tri.
            int tlTnCrnrIdx = vsLen - 4;
            int blInCrnrIdx = vsLen - 3;
            int brInCrnrIdx = vsLen - 2;
            int trInCrnrIdx = vsLen - 1;

            // Inner coordinate corners.
            vs[tlTnCrnrIdx] = new Vec2(lftIns0, topIns1);
            vs[blInCrnrIdx] = new Vec2(lftIns1, btmIns1);
            vs[brInCrnrIdx] = new Vec2(rgtIns1, btmIns0);
            vs[trInCrnrIdx] = new Vec2(rgtIns0, topIns0);

            // Inner texture coordinate corners.
            float ui0 = vtl * wInv;
            float vi0 = (topIns1 - btm) * hInv;
            float ui1 = vbl * wInv;
            float vi1 = vbl * hInv;
            float ui2 = (rgtIns1 - lft) * wInv;
            float vi2 = vbr * hInv;
            float ui3 = (rgtIns0 - lft) * wInv;
            float vi3 = (topIns0 - btm) * hInv;

            vts[tlTnCrnrIdx] = new Vec2(
                (ui0 - 0.5f) * uScl + 0.5f,
                1.0f - ((vi0 - 0.5f) * vScl + 0.5f));
            vts[blInCrnrIdx] = new Vec2(
                (ui1 - 0.5f) * uScl + 0.5f,
                1.0f - ((vi1 - 0.5f) * vScl + 0.5f));
            vts[brInCrnrIdx] = new Vec2(
                (ui2 - 0.5f) * uScl + 0.5f,
                1.0f - ((vi2 - 0.5f) * vScl + 0.5f));
            vts[trInCrnrIdx] = new Vec2(
                (ui3 - 0.5f) * uScl + 0.5f,
                1.0f - ((vi3 - 0.5f) * vScl + 0.5f));

            // For calculating the number of face indices.
            int fsLen;
            int nonCornerFaces;
            int vResTotal = vtlRes +
                vblRes +
                vbrRes +
                vtrRes;
            int fResTotal = vResTotal + 4;
            Loop2[] fs;

            if (poly == PolyType.Quad)
            {
                // Face count will be non-uniform, with main faces
                // being size 4, corner faces being size 3.
                nonCornerFaces = 5;
                fsLen = nonCornerFaces + fResTotal;
                fs = target.loops = Loop2.Resize(target.loops, fsLen, 3, true);

                fs[0].Indices = new Index2[]
                {
                    new Index2 (tlTnCrnrIdx, tlTnCrnrIdx),
                    new Index2 (blInCrnrIdx, blInCrnrIdx),
                    new Index2 (brInCrnrIdx, brInCrnrIdx),
                    new Index2 (trInCrnrIdx, trInCrnrIdx)
                };

                fs[1].Indices = new Index2[]
                {
                    new Index2 (tlCrnrIdxEnd, tlCrnrIdxEnd),
                    new Index2 (blCrnrIdxStr, blCrnrIdxStr),
                    new Index2 (blInCrnrIdx, blInCrnrIdx),
                    new Index2 (tlTnCrnrIdx, tlTnCrnrIdx)
                };

                fs[2].Indices = new Index2[]
                {
                    new Index2 (blInCrnrIdx, blInCrnrIdx),
                    new Index2 (blCrnrIdxEnd, blCrnrIdxEnd),
                    new Index2 (brCrnrIdxStr, brCrnrIdxStr),
                    new Index2 (brInCrnrIdx, brInCrnrIdx)
                };

                fs[3].Indices = new Index2[]
                {
                    new Index2 (trInCrnrIdx, trInCrnrIdx),
                    new Index2 (brInCrnrIdx, brInCrnrIdx),
                    new Index2 (brCrnrIdxEnd, brCrnrIdxEnd),
                    new Index2 (trCrnrIdxStr, trCrnrIdxStr)
                };

                fs[4].Indices = new Index2[]
                {
                    new Index2 (tlCrnrIdxStr, tlCrnrIdxStr),
                    new Index2 (tlTnCrnrIdx, tlTnCrnrIdx),
                    new Index2 (trInCrnrIdx, trInCrnrIdx),
                    new Index2 (trCrnrIdxEnd, trCrnrIdxEnd)
                };
            }
            else
            {
                nonCornerFaces = 10;
                fsLen = nonCornerFaces + fResTotal;
                fs = target.loops = Loop2.Resize(target.loops, fsLen, 3, true);

                Loop2 f0 = fs[0];
                f0.Indices[0] = new Index2(tlTnCrnrIdx, tlTnCrnrIdx);
                f0.Indices[1] = new Index2(blInCrnrIdx, blInCrnrIdx);
                f0.Indices[2] = new Index2(trInCrnrIdx, trInCrnrIdx);

                Loop2 f1 = fs[1];
                f1.Indices[0] = new Index2(blInCrnrIdx, blInCrnrIdx);
                f1.Indices[1] = new Index2(brInCrnrIdx, brInCrnrIdx);
                f1.Indices[2] = new Index2(trInCrnrIdx, trInCrnrIdx);

                Loop2 f2 = fs[2];
                f2.Indices[0] = new Index2(tlCrnrIdxEnd, tlCrnrIdxEnd);
                f2.Indices[1] = new Index2(blCrnrIdxStr, blCrnrIdxStr);
                f2.Indices[2] = new Index2(tlTnCrnrIdx, tlTnCrnrIdx);

                Loop2 f3 = fs[3];
                f3.Indices[0] = new Index2(blCrnrIdxStr, blCrnrIdxStr);
                f3.Indices[1] = new Index2(blInCrnrIdx, blInCrnrIdx);
                f3.Indices[2] = new Index2(tlTnCrnrIdx, tlTnCrnrIdx);

                Loop2 f4 = fs[4];
                f4.Indices[0] = new Index2(blInCrnrIdx, blInCrnrIdx);
                f4.Indices[1] = new Index2(blCrnrIdxEnd, blCrnrIdxEnd);
                f4.Indices[2] = new Index2(brInCrnrIdx, brInCrnrIdx);

                Loop2 f5 = fs[5];
                f5.Indices[0] = new Index2(blCrnrIdxEnd, blCrnrIdxEnd);
                f5.Indices[1] = new Index2(brCrnrIdxStr, brCrnrIdxStr);
                f5.Indices[2] = new Index2(brInCrnrIdx, brInCrnrIdx);

                Loop2 f6 = fs[6];
                f6.Indices[0] = new Index2(trInCrnrIdx, trInCrnrIdx);
                f6.Indices[1] = new Index2(brInCrnrIdx, brInCrnrIdx);
                f6.Indices[2] = new Index2(trCrnrIdxStr, trCrnrIdxStr);

                Loop2 f7 = fs[7];
                f7.Indices[0] = new Index2(brInCrnrIdx, brInCrnrIdx);
                f7.Indices[1] = new Index2(brCrnrIdxEnd, brCrnrIdxEnd);
                f7.Indices[2] = new Index2(trCrnrIdxStr, trCrnrIdxStr);

                Loop2 f8 = fs[8];
                f8.Indices[0] = new Index2(tlCrnrIdxStr, tlCrnrIdxStr);
                f8.Indices[1] = new Index2(tlTnCrnrIdx, tlTnCrnrIdx);
                f8.Indices[2] = new Index2(trCrnrIdxEnd, trCrnrIdxEnd);

                Loop2 f9 = fs[9];
                f9.Indices[0] = new Index2(tlTnCrnrIdx, tlTnCrnrIdx);
                f9.Indices[1] = new Index2(trInCrnrIdx, trInCrnrIdx);
                f9.Indices[2] = new Index2(trCrnrIdxEnd, trCrnrIdxEnd);
            }

            // Face count.
            int fTlRes = vtlRes + 1;
            int fBlRes = vblRes + 1;
            int fBrRes = vbrRes + 1;
            int fTrRes = vtrRes + 1;

            // Index offsets.
            int fsTlIdxStr = nonCornerFaces;
            int fsBlIdxStr = fsTlIdxStr + fTlRes;
            int fsBrIdxStr = fsBlIdxStr + fBlRes;
            int fs_tr_idx_start = fsBrIdxStr + fBrRes;

            // Top left corner.
            for (int i = 0; i < fTlRes; ++i)
            {
                int b = tlCrnrIdxStr + i;
                int c = b + 1;
                Loop2 f = fs[fsTlIdxStr + i];
                f.Indices[0] = new Index2(tlTnCrnrIdx, tlTnCrnrIdx);
                f.Indices[1] = new Index2(b, b);
                f.Indices[2] = new Index2(c, c);
            }

            // Bottom left corner.
            for (int i = 0; i < fBlRes; ++i)
            {
                int b = blCrnrIdxStr + i;
                int c = b + 1;
                Loop2 f = fs[fsBlIdxStr + i];
                f.Indices[0] = new Index2(blInCrnrIdx, blInCrnrIdx);
                f.Indices[1] = new Index2(b, b);
                f.Indices[2] = new Index2(c, c);
            }

            // Bottom right corner.
            for (int i = 0; i < fBrRes; ++i)
            {
                int b = brCrnrIdxStr + i;
                int c = b + 1;
                Loop2 f = fs[fsBrIdxStr + i];
                f.Indices[0] = new Index2(brInCrnrIdx, brInCrnrIdx);
                f.Indices[1] = new Index2(b, b);
                f.Indices[2] = new Index2(c, c);
            }

            // Top right corner.
            for (int i = 0; i < fTrRes; ++i)
            {
                int b = trCrnrIdxStr + i;
                int c = b + 1;
                Loop2 f = fs[fs_tr_idx_start + i];
                f.Indices[0] = new Index2(trInCrnrIdx, trInCrnrIdx);
                f.Indices[1] = new Index2(b, b);
                f.Indices[2] = new Index2(c, c);
            }
        }

        return target;
    }

    /// <summary>
    /// Returns a string representation of a mesh.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="m">mesh</param>
    /// <param name="padding">integer padding</param>
    /// <param name="places">real number decimals</param>
    /// <returns>string</returns>
    public static string ToString(
        in Mesh2 m,
        in int padding = 1,
        in int places = 4)
    {
        return Mesh2.ToString(new StringBuilder(2048),
            m, padding, places).ToString();
    }

    /// <summary>
    /// Appends a string representation of a mesh to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="m">mesh</param>
    /// <param name="padding">integer padding</param>
    /// <param name="places">real number decimals</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString(
        in StringBuilder sb,
        in Mesh2 m,
        in int padding = 1,
        in int places = 4)
    {
        sb.Append("{ loops: ");
        Loop2.ToString(sb, m.loops, padding);
        sb.Append(", coords: ");
        Vec2.ToString(sb, m.coords, places);
        sb.Append(", texCoords: ");
        Vec2.ToString(sb, m.texCoords, places);
        sb.Append(" }");
        return sb;
    }

    /// <summary>
    /// Triangulates all faces in a mesh by drawing diagonals
    /// from the face's first vertex to all non-adjacent vertices.
    /// </summary>
    /// <param name="source">source</param>
    /// <param name="target">target</param>
    /// <returns>triangulated mesh</returns>
    public static Mesh2 Triangulate(in Mesh2 source, in Mesh2 target)
    {
        Loop2[] loopsSrc = source.loops;
        Vec2[] vsSrc = source.coords;
        Vec2[] vtsSrc = source.texCoords;

        // Cannot anticipate how many loops in the source mesh will not be
        // triangles, so this is an expanding list.
        List<Loop2> loopsTrg = new();

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
                    Loop2 loopTrg = new(vert0, vert1, vert2);
                    loopsTrg.Add(loopTrg);
                }
            }
            else
            {
                loopsTrg.Add(fSrc);
            }
        }

        // If source and target are not the same mesh, then copy mesh data from
        // source to target.
        if (!Object.ReferenceEquals(source, target))
        {
            int vsLen = vsSrc.Length;
            target.coords = new Vec2[vsLen];
            System.Array.Copy(vsSrc, target.coords, vsLen);

            int vtsLen = vtsSrc.Length;
            target.texCoords = new Vec2[vtsLen];
            System.Array.Copy(vtsSrc, target.texCoords, vtsLen);
        }
        target.loops = loopsTrg.ToArray();

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
    public static Mesh2 UniformData(in Mesh2 source, in Mesh2 target)
    {
        // TODO: Account for cases where source == target
        // vs. source != target.

        Loop2[] fsSrc = source.loops;
        Vec2[] vsSrc = source.coords;
        Vec2[] vtsSrc = source.texCoords;

        int uniformLen = 0;
        int fsSrcLen = fsSrc.Length;
        for (int i = 0; i < fsSrcLen; ++i)
        {
            uniformLen += fsSrc[i].Length;
        }

        Loop2[] fsTrg = new Loop2[fsSrcLen];
        Vec2[] vsTrg = new Vec2[uniformLen];
        Vec2[] vtsTrg = new Vec2[uniformLen];

        for (int k = 0, i = 0; i < fsSrcLen; ++i)
        {
            Index2[] fSrc = fsSrc[i].Indices;
            int fLen = fSrc.Length;
            fsTrg[i] = new Loop2(new Index2[fLen]);
            Index2[] fTrg = fsTrg[i].Indices;

            for (int j = 0; j < fLen; ++j, ++k)
            {
                Index2 vertSrc = fSrc[j];
                vsTrg[k] = vsSrc[vertSrc.v];
                vtsTrg[k] = vtsSrc[vertSrc.vt];
                fTrg[j] = new Index2(k, k);
            }
        }

        target.coords = vsTrg;
        target.texCoords = vtsTrg;
        target.loops = fsTrg;

        return target;
    }
}