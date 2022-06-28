using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Organizes data needed to draw a three dimensional shape with vertices and
/// faces.
/// </summary>
public class Mesh3
{
    /// <summary>
    /// An array of coordinates.
    /// </summary>
    protected Vec3[] coords;

    /// <summary>
    /// Loops that describe the indices which reference the coordinates, texture
    /// coordinates and normals to compose a face.
    /// </summary>
    protected Loop3[] loops;

    /// <summary>
    /// An array of normals to indicate how light will bounce off the mesh's
    /// surface.
    /// </summary>
    protected Vec3[] normals;

    /// <summary>
    /// The texture (UV) coordinates that describe how an image is mapped onto
    /// the mesh. Typically in the range [0.0, 1.0] .
    /// </summary>
    protected Vec2[] texCoords;

    /// <summary>
    /// An array of coordinates.
    /// </summary>
    /// <value>coordinates</value>
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

    /// <summary>
    /// Gets the number of loops in the mesh.
    /// </summary>
    /// <value>length</value>
    public int Length { get { return this.loops.Length; } }

    /// <summary>
    /// Loops that describe the indices which reference the coordinates, texture
    /// coordinates and normals to compose a face.
    /// </summary>
    /// <value>loops</value>
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

    /// <summary>
    /// An array of normals to indicate how light will bounce off the mesh's
    /// surface.
    /// </summary>
    /// <value>normals</value>
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
    public Face3 this[int i]
    {
        get { return this.GetFace(i); }
    }

    /// <summary>
    /// The default constructor.
    /// </summary>
    public Mesh3()
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
    /// <param name="normals">normals</param>
    public Mesh3( //
        in Loop3[] loops, //
        in Vec3[] coords, //
        in Vec2[] texCoords, //
        in Vec3[] normals)
    {
        this.loops = loops;
        this.coords = coords;
        this.texCoords = texCoords;
        this.normals = normals;
    }

    /// <summary>
    /// Promotes a 2D mesh to a 3D mesh.
    /// </summary>
    /// <param name="source">source mesh</param>
    public Mesh3(in Mesh2 source)
    {
        this.Set(source);
    }

    /// <summary>
    /// Copies a source mesh to a new mesh.
    /// </summary>
    /// <param name="source">source mesh</param>
    public Mesh3(in Mesh3 source)
    {
        this.Set(source);
    }

    /// <summary>
    /// Returns a string representation of this mesh.
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()
    {
        return Mesh3.ToString(this);
    }

    /// <summary>
    /// Removes elements from the coordinate, texture coordinate and normal
    /// arrays of the mesh which are not visited by the face indices.
    /// </summary>
    /// <returns>this mesh</returns>
    public Mesh3 Clean()
    {
        Dictionary<int, Vec3> usedCoords = new Dictionary<int, Vec3>();
        Dictionary<int, Vec2> usedTexCoords = new Dictionary<int, Vec2>();
        Dictionary<int, Vec3> usedNormals = new Dictionary<int, Vec3>();

        int facesLen = this.loops.Length;
        for (int i = 0; i < facesLen; ++i)
        {
            Loop3 loop = this.loops[i];
            Index3[] verts = loop.Indices;
            int vertsLen = verts.Length;
            for (int j = 0; j < vertsLen; ++j)
            {
                Index3 vert = verts[j];
                usedCoords[vert.v] = this.coords[vert.v];
                usedTexCoords[vert.vt] = this.texCoords[vert.vt];
                usedNormals[vert.vn] = this.normals[vert.vn];
            }
        }

        SortQuantized3 v3Cmp = new SortQuantized3();
        SortQuantized2 v2Cmp = new SortQuantized2();

        SortedSet<Vec3> coordsSet = new SortedSet<Vec3>(v3Cmp);
        SortedSet<Vec2> texCoordsSet = new SortedSet<Vec2>(v2Cmp);
        SortedSet<Vec3> normalsSet = new SortedSet<Vec3>(v3Cmp);

        coordsSet.UnionWith(usedCoords.Values);
        texCoordsSet.UnionWith(usedTexCoords.Values);
        normalsSet.UnionWith(usedNormals.Values);

        Vec3[] newCoords = new Vec3[coordsSet.Count];
        Vec2[] newTexCoords = new Vec2[texCoordsSet.Count];
        Vec3[] newNormals = new Vec3[normalsSet.Count];

        coordsSet.CopyTo(newCoords);
        texCoordsSet.CopyTo(newTexCoords);
        normalsSet.CopyTo(newNormals);

        for (int i = 0; i < facesLen; ++i)
        {
            Loop3 loop = this.loops[i];
            Index3[] verts = loop.Indices;
            int vertsLen = verts.Length;
            for (int j = 0; j < vertsLen; ++j)
            {
                Index3 oldVert = verts[j];
                verts[j] = new Index3(
                    Array.BinarySearch<Vec3>(newCoords, this.coords[oldVert.v], v3Cmp),
                    Array.BinarySearch<Vec2>(newTexCoords, this.texCoords[oldVert.vt], v2Cmp),
                    Array.BinarySearch<Vec3>(newNormals, this.normals[oldVert.vn], v3Cmp));
            }
        }

        this.coords = newCoords;
        this.texCoords = newTexCoords;
        this.normals = newNormals;

        Array.Sort(this.loops, new SortLoops3(this.coords));

        return this;
    }

    /// <summary>
    /// Negates all the normals in this mesh,
    /// then reverses all the face directions.
    /// </summary>
    /// <returns>this mesh</returns>
    public Mesh3 FlipNormals()
    {
        int vnsLen = this.normals.Length;
        for (int i = 0; i < vnsLen; ++i)
        {
            this.normals[i] = -this.normals[i];
        }
        this.ReverseFaces();
        return this;
    }

    /// <summary>
    /// Negates the x component of all texture coordinates (u) in the mesh.
    /// Does so by subtracting the value from 1.0.
    /// </summary>
    /// <returns>this mesh</returns>
    public Mesh3 FlipU()
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
    public Mesh3 FlipV()
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
    /// Use this instead of scaling the mesh by (-1.0, 1.0, 1.0).
    /// </summary>
    /// <returns>this mesh</returns>
    public Mesh3 FlipX()
    {
        int vsLen = this.coords.Length;
        for (int i = 0; i < vsLen; ++i)
        {
            Vec3 v = this.coords[i];
            this.coords[i] = new Vec3(-v.x, v.y, v.z);
        }
        this.ReverseFaces();
        return this;
    }

    /// <summary>
    /// Negates the y component of all coordinates in the mesh,
    /// then reverses the mesh's faces.
    /// Use this instead of scaling the mesh by (1.0, -1.0, 1.0).
    /// </summary>
    /// <returns>this mesh</returns>
    public Mesh3 FlipY()
    {
        int vsLen = this.coords.Length;
        for (int i = 0; i < vsLen; ++i)
        {
            Vec3 v = this.coords[i];
            this.coords[i] = new Vec3(v.x, -v.y, v.z);
        }
        this.ReverseFaces();
        return this;
    }

    /// <summary>
    /// Negates the z component of all coordinates in the mesh,
    /// then reverses the mesh's faces.
    /// Use this instead of scaling the mesh by (1.0, 1.0, -1.0).
    /// </summary>
    /// <returns>this mesh</returns>
    public Mesh3 FlipZ()
    {
        int vsLen = this.coords.Length;
        for (int i = 0; i < vsLen; ++i)
        {
            Vec3 v = this.coords[i];
            this.coords[i] = new Vec3(v.x, v.y, -v.z);
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
    public Edge3 GetEdge(in int faceIndex = -1, in int edgeIndex = -1)
    {
        Loop3 loop = this.loops[Utils.RemFloor(faceIndex, this.loops.Length)];
        Index3 idxOrigin = loop[edgeIndex];
        Index3 idxDest = loop[edgeIndex + 1];

        return new Edge3(
            new Vert3(
                this.coords[idxOrigin.v],
                this.texCoords[idxOrigin.vt],
                this.normals[idxOrigin.v]),

            new Vert3(
                this.coords[idxDest.v],
                this.texCoords[idxDest.vt],
                this.normals[idxDest.vn]));
    }

    /// <summary>
    /// Gets an array of edges from the mesh.
    /// </summary>
    /// <returns>edges array</returns>
    public Edge3[] GetEdges()
    {
        return this.GetEdgesUndirected();
    }

    /// <summary>
    /// Gets an array of edges from the mesh. Edges are treated as directed, so
    /// (origin, destination) and (destination, edge) are considered to be
    /// different.
    /// </summary>
    /// <returns>edges array</returns>
    public Edge3[] GetEdgesDirected()
    {
        int loopsLen = this.loops.Length;
        SortedSet<Edge3> result = new SortedSet<Edge3>();

        for (int i = 0; i < loopsLen; ++i)
        {
            Loop3 loop = this.loops[i];
            Index3[] indices = loop.Indices;
            int indicesLen = indices.Length;

            for (int j = 0; j < indicesLen; ++j)
            {
                Index3 idxOrigin = indices[j];
                Index3 idxDest = indices[(j + 1) % indicesLen];

                Edge3 trial = new Edge3(
                    new Vert3(
                        this.coords[idxOrigin.v],
                        this.texCoords[idxOrigin.vt],
                        this.normals[idxOrigin.vn]),

                    new Vert3(
                        this.coords[idxDest.v],
                        this.texCoords[idxDest.vt],
                        this.normals[idxDest.vn]));

                result.Add(trial);
            }
        }

        Edge3[] arr = new Edge3[result.Count];
        result.CopyTo(arr, 0);
        return arr;
    }

    /// <summary>
    /// Gets an array of edges from the mesh. Edges are treated as undirected,
    /// so (origin, destination) and (destination, edge) are considered to be
    /// equal
    /// </summary>
    /// <returns>edges array</returns>
    public Edge3[] GetEdgesUndirected()
    {
        int loopsLen = this.loops.Length;
        Dictionary<int, Edge3> result = new Dictionary<int, Edge3>();

        for (int i = 0; i < loopsLen; ++i)
        {
            Loop3 loop = this.loops[i];
            Index3[] indices = loop.Indices;
            int indicesLen = indices.Length;

            for (int j = 0; j < indicesLen; ++j)
            {
                Index3 idxOrigin = indices[j];
                Index3 idxDest = indices[(j + 1) % indicesLen];

                int vIdxOrigin = idxOrigin.v;
                int vIdxDest = idxDest.v;

                int aHsh = (Utils.MulBase ^ vIdxOrigin) *
                    Utils.HashMul ^ vIdxDest;
                int bHsh = (Utils.MulBase ^ vIdxDest) *
                    Utils.HashMul ^ vIdxOrigin;

                if (!result.ContainsKey(aHsh) && !result.ContainsKey(bHsh))
                {
                    result[vIdxOrigin < vIdxDest ? aHsh : bHsh] = new Edge3(
                    new Vert3(
                        this.coords[idxOrigin.v],
                        this.texCoords[idxOrigin.vt],
                        this.normals[idxOrigin.vn]),

                    new Vert3(
                        this.coords[idxDest.v],
                        this.texCoords[idxDest.vt],
                        this.normals[idxDest.vn]));
                }
            }
        }

        Edge3[] arr = new Edge3[result.Count];
        result.Values.CopyTo(arr, 0);
        return arr;
    }

    /// <summary>
    /// Gets a face from a mesh.
    /// </summary>
    /// <param name="faceIndex">face index</param>
    /// <returns>face</returns>
    public Face3 GetFace(in int faceIndex = -1)
    {
        Loop3 loop = this.loops[Utils.RemFloor(faceIndex, this.loops.Length)];
        Index3[] indices = loop.Indices;
        int indicesLen = indices.Length;
        Edge3[] edges = new Edge3[indicesLen];
        for (int i = 0; i < indicesLen; ++i)
        {
            Index3 idxOrigin = indices[i];
            Index3 idxDest = indices[(i + 1) % indicesLen];
            edges[i] = new Edge3(
                new Vert3(
                    this.coords[idxOrigin.v],
                    this.texCoords[idxOrigin.vt],
                    this.normals[idxOrigin.vn]),
                new Vert3(
                    this.coords[idxDest.v],
                    this.texCoords[idxDest.vt],
                    this.normals[idxDest.vn]));
        }
        return new Face3(edges);
    }

    /// <summary>
    /// Gets an array of faces from the mesh.
    /// </summary>
    /// <returns>faces array</returns>
    public Face3[] GetFaces()
    {
        int loopsLen = this.loops.Length;
        Face3[] faces = new Face3[loopsLen];
        for (int i = 0; i < loopsLen; ++i)
        {
            Loop3 loop = this.loops[i];
            Index3[] indices = loop.Indices;
            int indicesLen = indices.Length;
            Edge3[] edges = new Edge3[indicesLen];

            for (int j = 0; j < indicesLen; ++j)
            {
                Index3 idxOrigin = indices[j];
                Index3 idxDest = indices[(j + 1) % indicesLen];
                edges[j] = new Edge3(
                    new Vert3(
                        this.coords[idxOrigin.v],
                        this.texCoords[idxOrigin.vt],
                        this.normals[idxOrigin.vn]),
                    new Vert3(
                        this.coords[idxDest.v],
                        this.texCoords[idxDest.vt],
                        this.normals[idxDest.vn]));
            }

            faces[i] = new Face3(edges);
        }
        return faces;
    }

    /// <summary>
    /// Gets a vertex from the mesh.
    /// </summary>
    /// <param name="faceIndex">face index</param>
    /// <param name="vertIndex">vertex index</param>
    /// <returns>vertex</returns>
    public Vert3 GetVertex(in int faceIndex = -1, in int vertIndex = -1)
    {
        Index3 index = this.loops[Utils.RemFloor(faceIndex, this.loops.Length)][vertIndex];
        return new Vert3(
            this.coords[index.v],
            this.texCoords[index.vt],
            this.normals[index.vn]);
    }

    /// <summary>
    /// Gets an array of vertices from the mesh.
    /// </summary>
    /// <returns>vertices</returns>
    public Vert3[] GetVertices()
    {
        int len0 = this.loops.Length;
        SortedSet<Vert3> result = new SortedSet<Vert3>();
        for (int i = 0; i < len0; ++i)
        {
            Loop3 loop = this.loops[i];
            Index3[] indices = loop.Indices;
            int len1 = indices.Length;
            for (int j = 0; j < len1; ++j)
            {
                Index3 vert = indices[j];
                result.Add(new Vert3(
                    this.coords[vert.v],
                    this.texCoords[vert.vt],
                    this.normals[vert.vn]));
            }
        }

        Vert3[] arr = new Vert3[result.Count];
        result.CopyTo(arr);
        return arr;
    }

    /// <summary>
    /// Reverses all the face loops in this mesh.
    /// </summary>
    /// <returns>this mesh</returns>
    public Mesh3 ReverseFaces()
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
    public Mesh3 Scale(in float s)
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
    public Mesh3 Scale(in Vec3 s)
    {
        if (Vec3.All(s))
        {
            int vsLen = this.coords.Length;
            for (int i = 0; i < vsLen; ++i) { this.coords[i] *= s; }

            int vnsLen = this.normals.Length;
            for (int i = 0; i < vsLen; ++i)
            {
                this.normals[i] = Vec3.Normalize(this.normals[i] / s);
            }
        }

        return this;
    }

    /// <summary>
    /// Promotes a 2D mesh to a 3D mesh.
    /// </summary>
    /// <param name="source">source mesh</param>
    /// <returns>this mesh</returns>
    public Mesh3 Set(in Mesh2 source)
    {
        Loop2[] loopsSrc = source.Loops;
        Vec2[] vsSrc = source.Coords;
        Vec2[] vtsSrc = source.TexCoords;

        int loopsLen = loopsSrc.Length;
        int vsLen = vsSrc.Length;
        int vtsLen = vtsSrc.Length;

        // Promote coordinates.
        this.coords = Vec3.Resize(this.coords, vsLen);
        for (int i = 0; i < vsLen; ++i)
        {
            this.coords[i] = vsSrc[i];
        }

        // Copy texture coordinates.
        this.texCoords = new Vec2[vtsLen];
        System.Array.Copy(vtsSrc, this.texCoords, vtsLen);

        // Only one normal needed.
        this.normals = new Vec3[] { Vec3.Up };

        // Promote loops.
        this.loops = Loop3.Resize(this.loops, loopsLen, 3, false);
        for (int i = 0; i < loopsLen; ++i)
        {
            Loop2 loopSrc = loopsSrc[i];
            Index2[] idcsSource = loopSrc.Indices;
            int loopLen = idcsSource.Length;
            Index3[] idcsTarget = new Index3[loopLen];
            for (int j = 0; j < loopLen; ++j)
            {
                idcsTarget[j] = (Index3)idcsSource[j];
            }
            this.loops[i].Indices = idcsTarget;
        }

        return this;
    }

    /// <summary>
    /// Sets this mesh from a source.
    /// </summary>
    /// <param name="source">source</param>
    /// <returns>this mesh</returns>
    public Mesh3 Set(in Mesh3 source)
    {
        int vsLen = source.coords.Length;
        this.coords = new Vec3[vsLen];
        System.Array.Copy(source.coords, this.coords, vsLen);

        int vtsLen = source.texCoords.Length;
        this.texCoords = new Vec2[vtsLen];
        System.Array.Copy(source.texCoords, this.texCoords, vtsLen);

        int vnsLen = source.normals.Length;
        this.normals = new Vec3[vnsLen];
        System.Array.Copy(source.normals, this.normals, vnsLen);

        int loopsLen = source.loops.Length;
        this.loops = Loop3.Resize(this.loops, loopsLen, 3, false);
        for (int i = 0; i < loopsLen; ++i)
        {
            Index3[] sourceIndices = source.loops[i].Indices;
            int loopLen = sourceIndices.Length;
            Index3[] targetIndices = new Index3[loopLen];
            System.Array.Copy(sourceIndices, targetIndices, loopLen);
            this.loops[i].Indices = targetIndices;
        }

        return this;
    }

    /// <summary>
    /// Calculates this mesh's normals per face, resulting in flat shading.
    /// The normals array is reallocated. Sums the cross products of edges
    /// in a face ( b - a ) x ( c - a ) then normalizes the sum.
    /// </summary>
    /// <returns>this mesh</returns>
    public Mesh3 ShadeFlat()
    {
        int facesLen = this.loops.Length;
        this.normals = new Vec3[facesLen];
        for (int i = 0; i < facesLen; ++i)
        {
            Index3[] face = this.loops[i].Indices;
            int faceLen = face.Length;

            float vnx = 0.0f;
            float vny = 0.0f;
            float vnz = 0.0f;
            Vec3 prev = this.coords[face[faceLen - 1].v];
            for (int j = 0; j < faceLen; ++j)
            {
                Index3 vert = face[j];
                Vec3 curr = this.coords[vert.v];
                Vec3 next = this.coords[face[(j + 1) % faceLen].v];

                float edge0x = prev.x - curr.x;
                float edge0y = prev.y - curr.y;
                float edge0z = prev.z - curr.z;

                float edge1x = curr.x - next.x;
                float edge1y = curr.y - next.y;
                float edge1z = curr.z - next.z;

                vnx += edge0y * edge1z - edge0z * edge1y;
                vny += edge0z * edge1x - edge0x * edge1z;
                vnz += edge0x * edge1y - edge0y * edge1x;

                face[j] = new Index3(vert.v, vert.vt, i);
                prev = curr;
            }

            this.normals[i] = Vec3.Normalize(new Vec3(vnx, vny, vnz));
        }

        return this;
    }

    /// <summary>
    /// Subdivides a convex face by calculating its center, subdividing each of
    /// its edges with one cut to create a midpoint, then connecting the
    /// midpoints to the center. This generates a quadrilateral for the number
    /// of edges in the face. Returns a tuple containing the new data created.
    /// </summary>
    /// <param name="faceIdx">face index</param>
    /// <returns>the tuple</returns>
    public (Loop3[] loopsNew,
            Vec3[] vsNew,
            Vec2[] vtsNew,
            Vec3[] vnsNew) SubdivFaceCenter(in int faceIdx)
    {
        int facesLen = this.loops.Length;
        int i = Utils.RemFloor(faceIdx, facesLen);
        Index3[] face = this.loops[i].Indices;
        int faceLen = face.Length;

        int vsOldLen = this.coords.Length;
        int vtsOldLen = this.texCoords.Length;
        int vnsOldLen = this.normals.Length;

        Vec3[] vsNew = new Vec3[faceLen + 1];
        Vec2[] vtsNew = new Vec2[vsNew.Length];
        Vec3[] vnsNew = new Vec3[vsNew.Length];
        Loop3[] fsNew = new Loop3[faceLen];

        int vCenterIdx = vsOldLen + faceLen;
        int vtCenterIdx = vtsOldLen + faceLen;
        int vnCenterIdx = vnsOldLen + faceLen;

        Vec3 vCenter = new Vec3();
        Vec2 vtCenter = new Vec2();
        Vec3 vnCenter = new Vec3();

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

            vsNew[j] = Vec3.Mix(vCurr, this.coords[vNextIdx]);
            vtsNew[j] = Vec2.Mix(vtCurr, this.texCoords[vtNextIdx]);
            vnsNew[j] = Vec3.Normalize(vnCurr + this.normals[vnNextIdx]);

            fsNew[j] = new Loop3(
                new Index3(vCenterIdx, vtCenterIdx, vnCenterIdx),
                new Index3(vsOldLen + j, vtsOldLen + j, vnsOldLen + j),
                new Index3(vNextIdx, vtNextIdx, vnNextIdx),
                new Index3(vsOldLen + k, vtsOldLen + k, vnsOldLen + k));
        }

        if (faceLen > 0)
        {
            float flInv = 1.0f / faceLen;
            vCenter *= flInv;
            vtCenter *= flInv;
            vnCenter = Vec3.Normalize(vnCenter);
        }

        vsNew[faceLen] = vCenter;
        vtsNew[faceLen] = vtCenter;
        vnsNew[faceLen] = vnCenter;

        this.coords = Vec3.Concat(this.coords, vsNew);
        this.texCoords = Vec2.Concat(this.texCoords, vtsNew);
        this.normals = Vec3.Concat(this.normals, vnsNew);
        this.loops = Loop3.Splice(this.loops, i, 1, fsNew);

        return (loopsNew: fsNew,
            vsNew: vsNew,
            vtsNew: vtsNew,
            vnsNew: vnsNew);
    }

    /// <summary>
    /// Subdivides a convex face by calculating its center, then connecting its
    /// vertices to the center. This generates a triangle for the number of
    /// edges in the face.
    /// </summary>
    /// <param name="faceIdx">the face index</param>
    /// <returns>new data</returns>
    public (Loop3[] loopsNew,
            Vec3[] vsNew,
            Vec2[] vtsNew,
            Vec3[] vnsNew) SubdivFaceFan(in int faceIdx)
    {
        int facesLen = this.loops.Length;
        int i = Utils.RemFloor(faceIdx, facesLen);
        Index3[] face = this.loops[i].Indices;
        int faceLen = face.Length;

        Loop3[] fsNew = new Loop3[faceLen];
        Vec3 vCenter = new Vec3();
        Vec2 vtCenter = new Vec2();
        Vec3 vnCenter = new Vec3();

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

            fsNew[j] = new Loop3(
                new Index3(vCenterIdx, vtCenterIdx, vnCenterIdx),
                new Index3(vCurrIdx, vtCurrIdx, vnCurrIdx),
                new Index3(vertNext.v, vertNext.vt, vertNext.vn));
        }

        if (faceLen > 0)
        {
            float flInv = 1.0f / faceLen;
            vCenter *= flInv;
            vtCenter *= flInv;
            vnCenter = Vec3.Normalize(vnCenter);
        }

        this.coords = Vec3.Append(this.coords, vCenter);
        this.texCoords = Vec2.Append(this.texCoords, vtCenter);
        this.normals = Vec3.Append(this.normals, vnCenter);
        this.loops = Loop3.Splice(this.loops, i, 1, fsNew);

        return (loopsNew: fsNew,
            vsNew: new Vec3[] { vCenter },
            vtsNew: new Vec2[] { vtCenter },
            vnsNew: new Vec3[] { vnCenter });
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
    public (Loop3[] loopsNew,
            Vec3[] vsNew,
            Vec2[] vtsNew,
            Vec3[] vnsNew) SubdivFaceInscribe(in int faceIdx)
    {
        int facesLen = this.loops.Length;
        int i = Utils.RemFloor(faceIdx, facesLen);
        Index3[] face = this.loops[i].Indices;
        int faceLen = face.Length;

        int vsOldLen = this.coords.Length;
        int vtsOldLen = this.texCoords.Length;
        int vnsOldLen = this.normals.Length;

        Vec3[] vsNew = new Vec3[faceLen];
        Vec2[] vtsNew = new Vec2[faceLen];
        Vec3[] vnsNew = new Vec3[faceLen];
        Loop3[] fsNew = new Loop3[faceLen + 1];
        Index3[] cfIdcs = new Index3[3];

        for (int j = 0; j < faceLen; ++j)
        {
            int k = (j + 1) % faceLen;
            Index3 vertCurr = face[j];
            Index3 vertNext = face[k];

            int vNextIdx = vertNext.v;
            int vtNextIdx = vertNext.vt;
            int vnNextIdx = vertNext.vn;

            vsNew[j] = Vec3.Mix(
                this.coords[vertCurr.v],
                this.coords[vNextIdx]);
            vtsNew[j] = Vec2.Mix(
                this.texCoords[vertCurr.vt],
                this.texCoords[vtNextIdx]);
            vnsNew[j] = Vec3.Normalize(
                this.normals[vertCurr.vn] +
                this.normals[vnNextIdx]);

            int vSubdivIdx = vsOldLen + j;
            int vtSubdivIdx = vtsOldLen + j;
            int vnSubdivIdx = vnsOldLen + j;

            fsNew[j] = new Loop3(
                new Index3(vSubdivIdx, vtSubdivIdx, vnSubdivIdx),
                new Index3(vNextIdx, vtNextIdx, vnNextIdx),
                new Index3(vsOldLen + k, vtsOldLen + k, vnsOldLen + k));

            cfIdcs[j] = new Index3(vSubdivIdx, vtSubdivIdx, vnSubdivIdx);
        }

        // Center face.
        fsNew[faceLen] = new Loop3(cfIdcs);

        this.coords = Vec3.Concat(this.coords, vsNew);
        this.texCoords = Vec2.Concat(this.texCoords, vtsNew);
        this.normals = Vec3.Concat(this.normals, vnsNew);
        this.loops = Loop3.Splice(this.loops, i, 1, fsNew);

        return (loopsNew: fsNew,
            vsNew: vsNew,
            vtsNew: vtsNew,
            vnsNew: vnsNew);
    }

    /// <summary>
    /// Subdivides all faces in the mesh by a number of iterations. Uses the
    /// center method.
    /// </summary>
    /// <param name="itr">iterations</param>
    /// <returns>this mesh</returns>
    public (Loop3[] loopsNew,
            Vec3[] vsNew,
            Vec2[] vtsNew,
            Vec3[] vnsNew) SubdivFacesCenter(in int itr = 1)
    {
        List<Loop3> loopsNew = new List<Loop3>();
        List<Vec3> vsNew = new List<Vec3>();
        List<Vec2> vtsNew = new List<Vec2>();
        List<Vec3> vnsNew = new List<Vec3>();

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
                vnsNew.AddRange(result.vnsNew);

                k += vertLen;
            }
        }

        return (loopsNew: loopsNew.ToArray(),
            vsNew: vsNew.ToArray(),
            vtsNew: vtsNew.ToArray(),
            vnsNew: vnsNew.ToArray());
    }

    /// <summary>
    /// Subdivides all faces in the mesh by a number of iterations. Uses the
    /// triangle fan method.
    /// </summary>
    /// <param name="itr">iterations</param>
    /// <returns>this mesh</returns>
    public (Loop3[] loopsNew,
            Vec3[] vsNew,
            Vec2[] vtsNew,
            Vec3[] vnsNew) SubdivFacesFan(in int itr = 1)
    {
        List<Loop3> loopsNew = new List<Loop3>();
        List<Vec3> vsNew = new List<Vec3>();
        List<Vec2> vtsNew = new List<Vec2>();
        List<Vec3> vnsNew = new List<Vec3>();

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
                vnsNew.AddRange(result.vnsNew);

                k += vertLen;
            }
        }

        return (loopsNew: loopsNew.ToArray(),
            vsNew: vsNew.ToArray(),
            vtsNew: vtsNew.ToArray(),
            vnsNew: vnsNew.ToArray());
    }

    /// <summary>
    /// Subdivides all faces in the mesh by a number of iterations. Uses the
    /// inscription method.
    /// </summary>
    /// <param name="itr">iterations</param>
    /// <returns>this mesh</returns>
    public (Loop3[] loopsNew,
            Vec3[] vsNew,
            Vec2[] vtsNew,
            Vec3[] vnsNew) SubdivFacesInscribe(in int itr = 1)
    {
        List<Loop3> loopsNew = new List<Loop3>();
        List<Vec3> vsNew = new List<Vec3>();
        List<Vec2> vtsNew = new List<Vec2>();
        List<Vec3> vnsNew = new List<Vec3>();

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
                vnsNew.AddRange(result.vnsNew);

                k += vertLen + 1;
            }
        }

        return (loopsNew: loopsNew.ToArray(),
            vsNew: vsNew.ToArray(),
            vtsNew: vtsNew.ToArray(),
            vnsNew: vnsNew.ToArray());
    }

    /// <summary>
    /// Transforms a mesh by an affine transform matrix.
    /// </summary>
    /// <param name="m">affine transform</param>
    /// <returns>this mesh</returns>
    public Mesh3 Transform(in Mat4 m)
    {
        return this.Transform(m, Mat4.Inverse(m));
    }

    /// <summary>
    /// Transforms a mesh by an affine transform matrix.
    /// The matrix inverse is required to transform the
    /// mesh's normals.
    /// </summary>
    /// <param name="m">affine transform</param>
    /// <param name="h">inverse transform</param>
    /// <returns>this mesh</returns>
    public Mesh3 Transform(in Mat4 m, in Mat4 h)
    {
        int vsLen = this.coords.Length;
        for (int i = 0; i < vsLen; ++i)
        {
            this.coords[i] = Mat4.MulPoint(m, this.coords[i]);
        }

        int vnsLen = this.normals.Length;
        for (int j = 0; j < vnsLen; ++j)
        {
            this.normals[j] = Mat4.MulInverseNormal(h, this.normals[j]);
        }

        return this;
    }

    /// <summary>
    /// Transforms a mesh by a transform
    /// </summary>
    /// <param name="t">transform</param>
    /// <returns>this mesh</returns>
    public Mesh3 Transform(in Transform3 t)
    {
        int vsLen = this.coords.Length;
        for (int i = 0; i < vsLen; ++i)
        {
            this.coords[i] = Transform3.MulPoint(t, this.coords[i]);
        }

        int vnsLen = this.normals.Length;
        for (int j = 0; j < vnsLen; ++j)
        {
            this.normals[j] = Transform3.MulNormal(t, this.normals[j]);
        }

        return this;
    }

    /// <summary>
    /// Moves this mesh's coordinates by a vector.
    /// </summary>
    /// <param name="t">translation</param>
    /// <returns>this mesh</returns>
    public Mesh3 Translate(in Vec3 t)
    {
        int vsLen = this.coords.Length;
        for (int i = 0; i < vsLen; ++i) { this.coords[i] += t; }

        return this;
    }

    /// <summary>
    /// Finds the axis aligned bounding box for a mesh.
    /// </summary>
    /// <param name="mesh">mesh</param>
    /// <returns>bounds</returns>
    public static Bounds3 CalcBounds(in Mesh3 mesh)
    {
        Vec3[] coords = mesh.coords;
        int len = coords.Length;
        float lbx = Single.MaxValue;
        float lby = Single.MaxValue;
        float lbz = Single.MaxValue;

        float ubx = Single.MinValue;
        float uby = Single.MinValue;
        float ubz = Single.MinValue;

        for (int i = 0; i < len; ++i)
        {
            Vec3 coord = coords[i];
            float x = coord.x;
            float y = coord.y;
            float z = coord.z;
            if (x < lbx) { lbx = x; }
            if (x > ubx) { ubx = x; }
            if (y < lby) { lby = y; }
            if (y > uby) { uby = y; }
            if (z < lbz) { lbz = z; }
            if (z > ubz) { ubz = z; }
        }

        return new Bounds3(lbx, lby, lbz, ubx, uby, ubz);
    }

    /// <summary>
    /// Creates a capsule, with a UV hemisphere on the top and bottom of a
    /// cylinder.
    /// </summary>
    /// <param name="target">output mesh</param>
    /// <param name="longitudes">longitudes</param>
    /// <param name="latitudes">latitudes</param>
    /// <param name="rings">rings</param>
    /// <param name="depth">depth</param>
    /// <param name="radius">radius</param>
    /// <param name="poly">polygon type</param>
    /// <param name="profile">UV profile</param>
    /// <returns>capsule</returns>
    public static Mesh3 Capsule( //
        in Mesh3 target, //
        in int longitudes = 32, //
        in int latitudes = 16, //
        in int rings = 0, //
        in float depth = 1.0f, //
        in float radius = 0.5f, //
        in PolyType poly = PolyType.Tri, //
        UvProfiles.Capsule profile = UvProfiles.Capsule.Aspect)
    {
        // TODO: REFACTOR to build faces from bottom.

        int verifLats = latitudes < 2 ? 2 : latitudes % 2 != 0 ? latitudes + 1 : latitudes;
        int verifLons = Utils.Max(3, longitudes);
        int verifRings = Utils.Max(0, rings);
        float verifDepth = MathF.Max(Utils.Epsilon, depth);
        float verifRad = MathF.Max(Utils.Epsilon, radius);

        bool useQuads = poly == PolyType.Quad;
        bool calcMid = verifRings > 0;

        int halfLats = verifLats / 2;
        int halfLatsN1 = halfLats - 1;
        int halfLatsN2 = halfLats - 2;
        int verifRingsP1 = verifRings + 1;
        int verifLonsP1 = verifLons + 1;

        int lonsHalfLatN1 = halfLatsN1 * verifLons;
        int lonsRingsP1 = verifRingsP1 * verifLons;

        float halfDepth = verifDepth * 0.5f;
        float summit = halfDepth + verifRad;

        // Index offsets for coordinates.
        int idxVNEquator = verifLonsP1 + verifLons * halfLatsN2;
        int idxVCyl = idxVNEquator + verifLons;
        int idxVSEquator = calcMid ? idxVCyl + verifLons *
            verifRings : idxVCyl;
        int idxVSouth = idxVSEquator + verifLons;
        int idxVSouthCap = idxVSouth + verifLons * halfLatsN2;
        int idxVSouthPole = idxVSouthCap + verifLons;

        // Index offsets for texture coordinates.
        int idxVtNEquator = verifLons + verifLonsP1 * halfLatsN1;
        int idxVtCyl = idxVtNEquator + verifLonsP1;
        int idxVtSEquator = calcMid ? idxVtCyl + verifLonsP1 *
            verifRings : idxVtCyl;
        int idxVtSHemi = idxVtSEquator + verifLonsP1;
        int idxVtSPolar = idxVtSHemi + verifLonsP1 * halfLatsN2;
        int idxVtSCap = idxVtSPolar + verifLonsP1;

        // Index offsets for normals.
        int idxVnSouth = idxVNEquator + verifLons;
        int idxVnSouthCap = idxVnSouth + verifLons * halfLatsN2;
        int idxVnSouthPole = idxVnSouthCap + verifLons;

        // Array lengths.
        int vsLen = idxVSouthPole + 1;
        int vtsLen = idxVtSCap + verifLons;
        int vnsLen = idxVnSouthPole + 1;

        // Allocate mesh data.
        Vec3[] vs = target.coords = new Vec3[vsLen];
        Vec2[] vts = target.texCoords = new Vec2[vtsLen];
        Vec3[] vns = target.normals = new Vec3[vnsLen];

        // North pole.
        vs[0] = new Vec3(0.0f, 0.0f, summit);
        vns[0] = new Vec3(0.0f, 0.0f, 1.0f);

        // South pole.
        vs[idxVSouthPole] = new Vec3(0.0f, 0.0f, -summit);
        vns[idxVnSouthPole] = new Vec3(0.0f, 0.0f, -1.0f);

        Vec2[] sinThetaCache = new Vec2[verifLons];
        float toTheta = Utils.Tau / verifLons;
        float toPhi = MathF.PI / verifLats;
        float toTexHorizontal = 1.0f / verifLons;
        float toTexVertical = 1.0f / halfLats;
        float vtPoleNorth = 0.0f;
        float vtPoleSouth = 1.0f;

        for (int j = 0; j < verifLons; ++j)
        {
            float jf = j;

            // Coordinates.
            float theta = jf * toTheta;
            float sinTheta = MathF.Sin(theta);
            float cosTheta = MathF.Cos(theta);
            sinThetaCache[j] = new Vec2(cosTheta, sinTheta);

            // Texture coordinates at North and South pole.
            float sTex = (jf + 0.5f) * toTexHorizontal;
            vts[j] = new Vec2(sTex, vtPoleNorth);
            vts[idxVtSCap + j] = new Vec2(sTex, vtPoleSouth);

            // Multiply by radius to get equatorial x and y.
            float x = verifRad * cosTheta;
            float y = verifRad * sinTheta;

            // Equatorial coordinates. Offset by cylinder depth.
            vs[idxVNEquator + j] = new Vec3(x, y, halfDepth);
            vs[idxVSEquator + j] = new Vec3(x, y, -halfDepth);

            // Equatorial normals.
            vns[idxVNEquator + j] = new Vec3(cosTheta, sinTheta, 0.0f);
        }

        // Determine aspect ratio.
        float vtAspectRatio;
        switch (profile)
        {
            case UvProfiles.Capsule.Aspect:
                { vtAspectRatio = verifRad / (verifDepth + 2 * verifRad); }
                break;

            case UvProfiles.Capsule.Uniform:
                { vtAspectRatio = (float)halfLats / (verifRingsP1 + verifLats); }
                break;

            case UvProfiles.Capsule.Fixed:
            default:
                { vtAspectRatio = Utils.OneThird; }
                break;
        }
        float vtAspectNorth = vtAspectRatio;
        float vtAspectSouth = 1.0f - vtAspectRatio;

        // Calculate equatorial texture coordinates. Cache horizontal measure.
        float[] sTexCache = new float[verifLonsP1];
        for (int j = 0; j < verifLonsP1; ++j)
        {
            float sTex = j * toTexHorizontal;
            sTexCache[j] = sTex;
            vts[idxVtNEquator + j] = new Vec2(sTex, vtAspectNorth);
            vts[idxVtSEquator + j] = new Vec2(sTex, vtAspectSouth);
        }

        // Divide latitudes into hemispheres. Start at i = 1 due to the poles.
        int vHemiOffsetNorth = 1;
        int vHemiOffsetSouth = idxVSouth;
        int vtHemiOffsetNorth = verifLons;
        int vtHemiOffsetSouth = idxVtSHemi;
        int vnHemiOffsetSouth = idxVnSouth;

        for (int i = 1; i < halfLats; ++i)
        {
            float phi = i * toPhi;

            float sinPhiSouth = MathF.Sin(phi);
            float cosPhiSouth = MathF.Cos(phi);

            // Use trigonometric symmetries to avoid calculating another sine
            // and
            float sinPhiNorth = -cosPhiSouth;
            float cosPhiNorth = sinPhiSouth;

            // For North coordinates, multiply by radius and offset.
            float rhoCosPhiNorth = verifRad * cosPhiNorth;
            float rhoSinPhiNorth = verifRad * sinPhiNorth;
            float zOffsetNorth = halfDepth - rhoSinPhiNorth;

            // For South coordinates, multiply by radius and offset.
            float rhoCosPhiSouth = verifRad * cosPhiSouth;
            float rhoSinPhiSouth = verifRad * sinPhiSouth;
            float zOffsetSouth = -halfDepth - rhoSinPhiSouth;

            // Coordinates.
            for (int j = 0; j < verifLons; ++j)
            {
                Vec2 scTheta = sinThetaCache[j];

                // North coordinate.
                vs[vHemiOffsetNorth] = new Vec3(
                    rhoCosPhiNorth * scTheta.x,
                    rhoCosPhiNorth * scTheta.y,
                    zOffsetNorth);

                // North normal.
                vns[vHemiOffsetNorth] = new Vec3(
                    cosPhiNorth * scTheta.x,
                    cosPhiNorth * scTheta.y, //
                    -sinPhiNorth);

                // South coordinate.
                vs[vHemiOffsetSouth] = new Vec3(
                    rhoCosPhiSouth * scTheta.x,
                    rhoCosPhiSouth * scTheta.y,
                    zOffsetSouth);

                // South normal. 
                vns[vnHemiOffsetSouth] = new Vec3(
                    cosPhiSouth * scTheta.x,
                    cosPhiSouth * scTheta.y, //
                    -sinPhiSouth);

                ++vHemiOffsetNorth;
                ++vHemiOffsetSouth;
                ++vnHemiOffsetSouth;
            }

            float tTexFac = i * toTexVertical;
            float tTexNorth = tTexFac * vtAspectNorth;
            float tTexSouth = (1.0f - tTexFac) * vtAspectSouth + tTexFac;

            // Texture coordinates.
            for (int j = 0; j < verifLonsP1; ++j)
            {
                float sTex = sTexCache[j];
                vts[vtHemiOffsetNorth] = new Vec2(sTex, tTexNorth);
                vts[vtHemiOffsetSouth] = new Vec2(sTex, tTexSouth);

                ++vtHemiOffsetNorth;
                ++vtHemiOffsetSouth;
            }
        }

        // Calculate sections of cylinder in middle.
        if (calcMid)
        {
            float toFac = 1.0f / verifRingsP1;
            int vCylOffset = idxVCyl;
            int vtCylOffset = idxVtCyl;
            for (int m = 1; m < verifRingsP1; ++m)
            {
                float fac = m * toFac;
                float cmplFac = 1.0f - fac;

                // Coordinates.
                for (int j = 0; j < verifLons; ++j)
                {
                    Vec3 vEquatorNorth = vs[idxVNEquator + j];
                    Vec3 vEquatorSouth = vs[idxVSEquator + j];

                    // xy should be the same for both North and South. North z
                    // should equal half_depth while South z should equal
                    // -half_depth. For clarity, this is left as a linear
                    // interpolation.
                    vs[vCylOffset] = new Vec3(
                        cmplFac * vEquatorNorth.x + fac * vEquatorSouth.x,
                        cmplFac * vEquatorNorth.y + fac * vEquatorSouth.y,
                        cmplFac * vEquatorNorth.z + fac * vEquatorSouth.z);

                    ++vCylOffset;
                }

                // Texture coordinates.
                float tTex = cmplFac * vtAspectNorth + fac * vtAspectSouth;
                for (int j = 0; j < verifLonsP1; ++j)
                {
                    float sTex = sTexCache[j];
                    vts[vtCylOffset] = new Vec2(sTex, tTex);
                    ++vtCylOffset;
                }
            }
        }

        // Find index offsets for face indices.
        int idxFsCyl = useQuads ?
            verifLons + lonsHalfLatN1 :
            verifLons + lonsHalfLatN1 * 2;
        int idxFsSouthEquat = useQuads ?
            idxFsCyl + lonsRingsP1 :
            idxFsCyl + lonsRingsP1 * 2;
        int idxFsSouthHemi = useQuads ?
            idxFsSouthEquat + lonsHalfLatN1 :
            idxFsSouthEquat + lonsHalfLatN1 * 2;

        int lenIndices = idxFsSouthHemi + verifLons;
        Loop3[] fs = target.loops = new Loop3[lenIndices];

        // North & South cap indices (triangles).
        for (int j = 0; j < verifLons; ++j)
        {
            int jNextVt = j + 1;
            int jNextV = jNextVt % verifLons;

            fs[j] = new Loop3(
                new Index3(0, j, 0),
                new Index3(jNextVt, verifLons + j, jNextVt),
                new Index3(
                    1 + jNextV,
                    verifLons + jNextVt,
                    1 + jNextV));

            fs[idxFsSouthHemi + j] = new Loop3(
                new Index3(
                    idxVSouthPole,
                    idxVtSCap + j,
                    idxVnSouthPole),
                new Index3(
                    idxVSouthCap + jNextV,
                    idxVtSPolar + jNextVt,
                    idxVnSouthCap + jNextV),
                new Index3(
                    idxVSouthCap + j,
                    idxVtSPolar + j,
                    idxVnSouthCap + j));
        }

        // Hemisphere indices.
        int fHemiOffsetNorth = verifLons;
        int fHemiOffsetSouth = idxFsSouthEquat;
        for (int i = 0; i < halfLatsN1; ++i)
        {
            int iLonsCurr = i * verifLons;

            // North coordinate index offset.
            int vCurrLatN = 1 + iLonsCurr;
            int vNextLatN = vCurrLatN + verifLons;

            // South coordinate index offset.
            int vCurrLatS = idxVSEquator + iLonsCurr;
            int vNextLatS = vCurrLatS + verifLons;

            // North texture coordinate index offset.
            int vtCurrLatN = verifLons + i * verifLonsP1;
            int vtNextLatN = vtCurrLatN + verifLonsP1;

            // South texture coordinate index offset.
            int vtCurrLatS = idxVtSEquator + i * verifLonsP1;
            int vtNextLatS = vtCurrLatS + verifLonsP1;

            // North normal index offset.
            int vnCurrLatN = 1 + iLonsCurr;
            int vnNextLatN = vnCurrLatN + verifLons;

            // South normal index offset.
            int vnCurrLatS = idxVNEquator + iLonsCurr;
            int vnNextLatS = vnCurrLatS + verifLons;

            for (int j = 0; j < verifLons; ++j)
            {
                int jNextVt = j + 1;
                int jNextV = jNextVt % verifLons;

                // North coordinate indices.
                int vn00 = vCurrLatN + j;
                int vn01 = vNextLatN + j;
                int vn11 = vNextLatN + jNextV;
                int vn10 = vCurrLatN + jNextV;

                // South coordinate indices.
                int vs00 = vCurrLatS + j;
                int vs01 = vNextLatS + j;
                int vs11 = vNextLatS + jNextV;
                int vs10 = vCurrLatS + jNextV;

                // North texture coordinate indices.
                int vtn00 = vtCurrLatN + j;
                int vtn01 = vtNextLatN + j;
                int vtn11 = vtNextLatN + jNextVt;
                int vtn10 = vtCurrLatN + jNextVt;

                // South texture coordinate indices.
                int vts00 = vtCurrLatS + j;
                int vts01 = vtNextLatS + j;
                int vts11 = vtNextLatS + jNextVt;
                int vts10 = vtCurrLatS + jNextVt;

                // North normal indices.
                int vnn00 = vnCurrLatN + j;
                int vnn01 = vnNextLatN + j;
                int vnn11 = vnNextLatN + jNextV;
                int vnn10 = vnCurrLatN + jNextV;

                // South normal indices.
                int vns00 = vnCurrLatS + j;
                int vns01 = vnNextLatS + j;
                int vns11 = vnNextLatS + jNextV;
                int vns10 = vnCurrLatS + jNextV;

                // TODO: Quads version.

                // North triangles.
                fs[fHemiOffsetNorth] = new Loop3(
                    new Index3(vn00, vtn00, vnn00),
                    new Index3(vn11, vtn11, vnn11),
                    new Index3(vn10, vtn10, vnn10));

                fs[fHemiOffsetNorth + 1] = new Loop3(
                    new Index3(vn00, vtn00, vnn00),
                    new Index3(vn01, vtn01, vnn01),
                    new Index3(vn11, vtn11, vnn11));

                // South triangles.
                fs[fHemiOffsetSouth] = new Loop3(
                    new Index3(vs00, vts00, vns00),
                    new Index3(vs11, vts11, vns11),
                    new Index3(vs10, vts10, vns10));

                fs[fHemiOffsetSouth + 1] = new Loop3(
                    new Index3(vs00, vts00, vns00),
                    new Index3(vs01, vts01, vns01),
                    new Index3(vs11, vts11, vns11));

                fHemiOffsetNorth += 2;
                fHemiOffsetSouth += 2;
            }
        }

        // Cylinder face indices.
        int fCylOffset = idxFsCyl;
        for (int m = 0; m < verifRingsP1; ++m)
        {
            int vCurrRing = idxVNEquator + m * verifLons;
            int vNextRing = vCurrRing + verifLons;

            int vtCurrRing = idxVtNEquator + m * verifLonsP1;
            int vtNextRing = vtCurrRing + verifLonsP1;

            for (int j = 0; j < verifLons; ++j)
            {
                int jNextVt = j + 1;
                int jNextV = jNextVt % verifLons;

                // Coordinate corners.
                int v00 = vCurrRing + j;
                int v01 = vNextRing + j;
                int v11 = vNextRing + jNextV;
                int v10 = vCurrRing + jNextV;

                // Texture coordinate corners.
                int vt00 = vtCurrRing + j;
                int vt01 = vtNextRing + j;
                int vt11 = vtNextRing + jNextVt;
                int vt10 = vtCurrRing + jNextVt;

                // Normal corners.
                int vn0 = idxVNEquator + j;
                int vn1 = idxVNEquator + jNextV;

                // TODO: Quads version.

                fs[fCylOffset] = new Loop3(
                    new Index3(v00, vt00, vn0),
                    new Index3(v11, vt11, vn1),
                    new Index3(v10, vt10, vn1));

                fs[fCylOffset + 1] = new Loop3(
                    new Index3(v00, vt00, vn0),
                    new Index3(v01, vt01, vn0),
                    new Index3(v11, vt11, vn1));

                fCylOffset += 2;
            }
        }

        return target;
    }
    public static Mesh3 CastToSphere(in Mesh3 source, in Mesh3 target, in float radius = 0.5f)
    {
        float vrad = MathF.Max(Utils.Epsilon, radius);

        Loop3[] fsSrc = source.loops;
        Vec3[] vsSrc = source.coords;
        int fsSrcLen = fsSrc.Length;
        int vsSrcLen = vsSrc.Length;

        Loop3[] fsTrg = new Loop3[fsSrcLen];
        Vec3[] vsTrg = new Vec3[vsSrcLen];
        Vec3[] vnsTrg = new Vec3[vsSrcLen];

        for (int i = 0; i < vsSrcLen; ++i)
        {
            vnsTrg[i] = Vec3.Normalize(vsSrc[i]);
            vsTrg[i] = vnsTrg[i] * vrad;
        }

        for (int i = 0; i < fsSrcLen; ++i)
        {
            Loop3 loopSrc = fsSrc[i];
            Index3[] fSrc = loopSrc.Indices;
            int fSrcLen = fSrc.Length;

            Loop3 loopTrg = fsTrg[i] = new Loop3(new Index3[fSrcLen]);
            Index3[] fTrg = loopTrg.Indices;

            for (int j = 0; j < fSrcLen; ++j)
            {
                Index3 srcVert = fSrc[j];
                fTrg[j] = new Index3(srcVert.v, srcVert.vt, srcVert.v);
            }
        }

        if (!Object.ReferenceEquals(source, target))
        {
            int vtsLen = source.texCoords.Length;
            target.texCoords = new Vec2[vtsLen];
            System.Array.Copy(source.texCoords, target.texCoords, vtsLen);
        }

        target.loops = fsTrg;
        target.coords = vsTrg;
        target.normals = vnsTrg;

        return target;
    }

    /// <summary>
    /// Generates a cube mesh. In the context of Platonic solids, also known as
    /// a hexahedron, as it has 6 faces and 8 vertices.
    /// </summary>
    /// <param name="target">output mesh</param>
    /// <param name="size">scalar</param>
    /// <param name="poly">polygon type</param>
    /// <returns>cube</returns>
    public static Mesh3 Cube( //
        in Mesh3 target, //
        in float size = 0.5f, //
        in PolyType poly = PolyType.Tri)
    {
        float vsz = MathF.Max(Utils.Epsilon, size);
        target.coords = new Vec3[]
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

        target.normals = new Vec3[]
        {
            new Vec3 (1.0f, 0.0f, 0.0f),
            new Vec3 (0.0f, 0.0f, 1.0f),
            new Vec3 (0.0f, 0.0f, -1.0f),
            new Vec3 (0.0f, -1.0f, 0.0f),
            new Vec3 (-1.0f, 0.0f, 0.0f),
            new Vec3 (0.0f, 1.0f, 0.0f)
        };

        // TODO: Cube UV Profile options.
        target.texCoords = new Vec2[]
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
                {
                    target.loops = new Loop3[]
                    {
                        new Loop3 (
                        new Index3 (0, 4, 4), new Index3 (1, 5, 4),
                        new Index3 (3, 3, 4), new Index3 (2, 2, 4)),
                        new Loop3 (
                        new Index3 (2, 2, 5), new Index3 (3, 3, 5),
                        new Index3 (7, 6, 5), new Index3 (6, 7, 5)),
                        new Loop3 (
                        new Index3 (6, 7, 0), new Index3 (7, 6, 0),
                        new Index3 (5, 8, 0), new Index3 (4, 9, 0)),
                        new Loop3 (
                        new Index3 (4, 9, 3), new Index3 (5, 8, 3),
                        new Index3 (1, 0, 3), new Index3 (0, 1, 3)),
                        new Loop3 (
                        new Index3 (2, 10, 2), new Index3 (6, 7, 2),
                        new Index3 (4, 9, 2), new Index3 (0, 11, 2)),
                        new Loop3 (
                        new Index3 (7, 6, 1), new Index3 (3, 13, 1),
                        new Index3 (1, 12, 1), new Index3 (5, 8, 1))
                    };
                }
                break;

            case PolyType.Tri:
            default:
                {
                    target.loops = new Loop3[]
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
                }
                break;
        }

        return target;
    }

    /// <summary>
    /// Creates a cube, subdivides it, casts its vertices to a sphere, then
    /// triangulates its faces. The higher the iteration, the more spherical the
    /// result at the cost of performance.
    /// </summary>
    /// <param name="target">output mesh</param>
    /// <param name="itrs">iterations</param>
    /// <param name="size">size</param>
    /// <param name="poly">polygon type</param>
    /// <returns>dodecahedron</returns>
    public static Mesh3 CubeSphere( //
        in Mesh3 target, //
        in int itrs = 3, //
        in float size = 0.5f, //
        in PolyType poly = PolyType.Tri)
    {
        Mesh3.Cube(
            target: target,
            size: 0.5f,
            poly: PolyType.Quad);
        target.SubdivFacesCenter(itrs);
        if (poly == PolyType.Tri) { Mesh3.Triangulate(target, target); }
        target.Clean();

        // TODO: Redo to use cube-specific algorithm?
        // https://math.stackexchange.com/questions/118760/can-someone-please-explain-the-cube-to-sphere-mapping-formula-to-me
        Mesh3.CastToSphere(target, target, size);
        return target;
    }

    /// <summary>
    /// Creates an dodecahedron, a Platonic solid with 12 faces and 20
    /// coordinates.
    /// </summary>
    /// <param name="target">output mesh</param>
    /// <returns>dodecahedron</returns>
    public static Mesh3 Dodecahedron(in Mesh3 target)
    {
        target.coords = new Vec3[]
        {
            new Vec3 (0.0f, -0.02712715f, -0.53454524f),
            new Vec3 (0.0f, 0.33614415f, -0.4165113f),
            new Vec3 (-0.309017f, -0.19840115f, -0.38938415f),
            new Vec3 (0.309017f, -0.19840115f, -0.38938415f),
            new Vec3 (-0.309017f, 0.38938415f, -0.19840115f),
            new Vec3 (0.309017f, 0.38938415f, -0.19840115f),
            new Vec3 (-0.5f, 0.05901699f, -0.18163565f),
            new Vec3 (0.5f, 0.05901699f, -0.18163565f),
            new Vec3 (-0.19098301f, -0.47552827f, -0.15450847f),
            new Vec3 (0.19098301f, -0.47552827f, -0.15450847f),
            new Vec3 (-0.19098301f, 0.47552827f, 0.15450847f),
            new Vec3 (0.19098301f, 0.47552827f, 0.15450847f),
            new Vec3 (-0.5f, -0.05901699f, 0.18163565f),
            new Vec3 (0.5f, -0.05901699f, 0.18163565f),
            new Vec3 (-0.309017f, -0.38938415f, 0.19840115f),
            new Vec3 (0.309017f, -0.38938415f, 0.19840115f),
            new Vec3 (-0.309017f, 0.19840115f, 0.38938415f),
            new Vec3 (0.309017f, 0.19840115f, 0.38938415f),
            new Vec3 (0.0f, -0.33614415f, 0.4165113f),
            new Vec3 (0.0f, 0.02712715f, 0.53454524f)
        };

        target.texCoords = new Vec2[]
        {
            new Vec2 (0.099106364f, 0.25323522f),
            new Vec2 (0.22160856f, 0.25323522f),
            new Vec2 (0.29731908f, 0.25323522f),
            new Vec2 (0.4198213f, 0.25323522f),
            new Vec2 (0.7405362f, 0.25323522f),
            new Vec2 (0.64142984f, 0.3252402f),
            new Vec2 (0.8396426f, 0.3252402f),
            new Vec2 (0.06125109f, 0.36974174f),
            new Vec2 (0.25946382f, 0.36974174f),
            new Vec2 (0.45767656f, 0.36974174f),
            new Vec2 (0.58017874f, 0.36974174f),
            new Vec2 (0.90089375f, 0.36974174f),
            new Vec2 (0.03785526f, 0.4417467f),
            new Vec2 (0.16035746f, 0.4417467f),
            new Vec2 (0.3585702f, 0.4417467f),
            new Vec2 (0.4810724f, 0.4417467f),
            new Vec2 (0.6792851f, 0.4417467f),
            new Vec2 (0.8017873f, 0.4417467f),
            new Vec2 (1.0f, 0.4417467f),
            new Vec2 (0.0f, 0.5582533f),
            new Vec2 (0.19821273f, 0.5582533f),
            new Vec2 (0.32071492f, 0.5582533f),
            new Vec2 (0.51892763f, 0.5582533f),
            new Vec2 (0.64142984f, 0.5582533f),
            new Vec2 (0.8396426f, 0.5582533f),
            new Vec2 (0.9621448f, 0.5582533f),
            new Vec2 (0.099106364f, 0.63025826f),
            new Vec2 (0.4198213f, 0.63025826f),
            new Vec2 (0.54232347f, 0.63025826f),
            new Vec2 (0.7405362f, 0.63025826f),
            new Vec2 (0.93874896f, 0.63025826f),
            new Vec2 (0.16035746f, 0.67475975f),
            new Vec2 (0.3585702f, 0.67475975f),
            new Vec2 (0.25946382f, 0.7467648f),
            new Vec2 (0.58017874f, 0.7467648f),
            new Vec2 (0.70268095f, 0.7467648f),
            new Vec2 (0.7783915f, 0.7467648f),
            new Vec2 (0.90089375f, 0.7467648f)
        };

        target.normals = new Vec3[]
        {
            new Vec3 (-0.5257311f, 0.2628655f, -0.80901694f),
            new Vec3 (0.5257311f, 0.2628655f, -0.80901694f),
            new Vec3 (0.0f, -0.64655715f, -0.7628655f),
            new Vec3 (0.0f, 0.9714768f, -0.23713443f),
            new Vec3 (-0.8506508f, -0.5f, -0.16245979f),
            new Vec3 (0.8506508f, -0.5f, -0.16245979f),
            new Vec3 (-0.8506508f, 0.5f, 0.16245979f),
            new Vec3 (0.8506508f, 0.5f, 0.16245979f),
            new Vec3 (0.0f, -0.9714768f, 0.23713443f),
            new Vec3 (0.0f, 0.64655715f, 0.7628655f),
            new Vec3 (-0.5257311f, -0.2628655f, 0.80901694f),
            new Vec3 (0.5257311f, -0.2628655f, 0.80901694f)
        };

        Loop3[] fs = target.loops = Loop3.Resize(target.loops, 12, 5, true);
        Loop3.Pentagon(
            new Index3(0, 36, 0),
            new Index3(2, 37, 0),
            new Index3(6, 30, 0),
            new Index3(4, 24, 0),
            new Index3(1, 29, 0), fs[0]);
        Loop3.Pentagon(
            new Index3(0, 35, 1),
            new Index3(1, 29, 1),
            new Index3(5, 23, 1),
            new Index3(7, 28, 1),
            new Index3(3, 34, 1), fs[1]);
        Loop3.Pentagon(
            new Index3(0, 33, 2),
            new Index3(3, 32, 2),
            new Index3(9, 21, 2),
            new Index3(8, 20, 2),
            new Index3(2, 31, 2), fs[2]);
        Loop3.Pentagon(
            new Index3(1, 29, 3),
            new Index3(4, 24, 3),
            new Index3(10, 17, 3),
            new Index3(11, 16, 3),
            new Index3(5, 23, 3), fs[3]);
        Loop3.Pentagon(
            new Index3(3, 27, 5),
            new Index3(7, 22, 5),
            new Index3(13, 15, 5),
            new Index3(15, 14, 5),
            new Index3(9, 21, 5), fs[4]);
        Loop3.Pentagon(
            new Index3(2, 26, 4),
            new Index3(8, 20, 4),
            new Index3(14, 13, 4),
            new Index3(12, 12, 4),
            new Index3(6, 19, 4), fs[5]);
        Loop3.Pentagon(
            new Index3(4, 24, 6),
            new Index3(6, 25, 6),
            new Index3(12, 18, 6),
            new Index3(16, 11, 6),
            new Index3(10, 17, 6), fs[6]);
        Loop3.Pentagon(
            new Index3(5, 23, 7),
            new Index3(11, 16, 7),
            new Index3(17, 10, 7),
            new Index3(13, 15, 7),
            new Index3(7, 22, 7), fs[7]);
        Loop3.Pentagon(
            new Index3(8, 20, 8),
            new Index3(9, 21, 8),
            new Index3(15, 14, 8),
            new Index3(18, 8, 8),
            new Index3(14, 13, 8), fs[8]);
        Loop3.Pentagon(
            new Index3(17, 5, 9),
            new Index3(11, 16, 9),
            new Index3(10, 17, 9),
            new Index3(16, 6, 9),
            new Index3(19, 4, 9), fs[9]);
        Loop3.Pentagon(
            new Index3(16, 0, 10),
            new Index3(12, 7, 10),
            new Index3(14, 13, 10),
            new Index3(18, 8, 10),
            new Index3(19, 1, 10), fs[10]);
        Loop3.Pentagon(
            new Index3(18, 8, 11),
            new Index3(15, 14, 11),
            new Index3(13, 9, 11),
            new Index3(17, 3, 11),
            new Index3(19, 2, 11), fs[11]);

        return target;
    }

    /// <summary>
    /// Creates an icosahedron, a Platonic solid with 20 faces and 12
    /// coordinates.
    /// </summary>
    /// <param name="target">output mesh</param>
    /// <returns>icosahedron</returns>
    public static Mesh3 Icosahedron(in Mesh3 target)
    {
        // TODO: Reorganize?

        target.coords = new Vec3[]
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

        target.texCoords = new Vec2[]
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

        target.normals = new Vec3[]
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

        Loop3[] fs = target.loops = Loop3.Resize(target.loops, 20, 3, true);
        Loop3.Tri(new Index3(0, 2, 7), new Index3(4, 4, 7), new Index3(5, 0, 7), fs[0]);
        Loop3.Tri(new Index3(0, 6, 10), new Index3(2, 8, 10), new Index3(4, 4, 10), fs[1]);
        Loop3.Tri(new Index3(0, 10, 8), new Index3(1, 12, 8), new Index3(2, 8, 8), fs[2]);
        Loop3.Tri(new Index3(0, 14, 9), new Index3(3, 16, 9), new Index3(1, 12, 9), fs[3]);
        Loop3.Tri(new Index3(0, 18, 11), new Index3(5, 20, 11), new Index3(3, 16, 11), fs[4]);
        Loop3.Tri(new Index3(10, 1, 12), new Index3(5, 0, 12), new Index3(4, 4, 12), fs[5]);
        Loop3.Tri(new Index3(8, 5, 13), new Index3(10, 1, 13), new Index3(4, 4, 13), fs[6]);
        Loop3.Tri(new Index3(8, 5, 14), new Index3(4, 4, 14), new Index3(2, 8, 14), fs[7]);
        Loop3.Tri(new Index3(6, 9, 15), new Index3(8, 5, 15), new Index3(2, 8, 15), fs[8]);
        Loop3.Tri(new Index3(6, 9, 16), new Index3(2, 8, 16), new Index3(1, 12, 16), fs[9]);
        Loop3.Tri(new Index3(7, 13, 17), new Index3(6, 9, 17), new Index3(1, 12, 17), fs[10]);
        Loop3.Tri(new Index3(7, 13, 1), new Index3(1, 12, 1), new Index3(3, 16, 1), fs[11]);
        Loop3.Tri(new Index3(9, 17, 2), new Index3(7, 13, 2), new Index3(3, 16, 2), fs[12]);
        Loop3.Tri(new Index3(9, 17, 3), new Index3(3, 16, 3), new Index3(5, 20, 3), fs[13]);
        Loop3.Tri(new Index3(10, 21, 4), new Index3(9, 17, 4), new Index3(5, 20, 4), fs[14]);
        Loop3.Tri(new Index3(11, 19, 5), new Index3(9, 17, 5), new Index3(10, 21, 5), fs[15]);
        Loop3.Tri(new Index3(11, 3, 6), new Index3(10, 1, 6), new Index3(8, 5, 6), fs[16]);
        Loop3.Tri(new Index3(11, 7, 18), new Index3(8, 5, 18), new Index3(6, 9, 18), fs[17]);
        Loop3.Tri(new Index3(11, 11, 0), new Index3(6, 9, 0), new Index3(7, 13, 0), fs[18]);
        Loop3.Tri(new Index3(11, 15, 19), new Index3(7, 13, 19), new Index3(9, 17, 19), fs[19]);

        return target;
    }

    /// <summary>
    /// Creates an icosahedron, subdivides through inscription, then casts the
    /// vertices to a sphere.
    /// </summary>
    /// <param name="target">output mesh</param>
    /// <param name="itrs">iterations</param>
    /// <param name="size">size</param>
    /// <returns>icosphere</returns>
    public static Mesh3 Icosphere( //
        in Mesh3 target, //
        in int itrs = 3, //
        in float size = 0.5f)
    {
        Mesh3.Icosahedron(target);
        target.SubdivFacesInscribe(itrs);
        target.Clean();
        Mesh3.CastToSphere(target, target, size);
        return target;
    }

    /// <summary>
    /// Creates an octahedron, a Platonic solid with 8 faces and 6 coordinates.
    /// </summary>
    /// <param name="target">output mesh</param>
    /// <returns>octahedron</returns>
    public static Mesh3 Octahedron(in Mesh3 target)
    {
        target.coords = new Vec3[]
        {
            new Vec3 (0.0f, 0.0f, -0.5f),
            new Vec3 (0.0f, -0.5f, 0.0f),
            new Vec3 (-0.5f, 0.0f, 0.0f),
            new Vec3 (0.5f, 0.0f, 0.0f),
            new Vec3 (0.0f, 0.5f, 0.0f),
            new Vec3 (0.0f, 0.0f, 0.5f)
        };

        // To maintain aspect, scale y by sqrt(3)/4
        target.texCoords = new Vec2[]
        {
            new Vec2 (0.125f, 0.28349364f),
            new Vec2 (0.375f, 0.28349364f),
            new Vec2 (0.625f, 0.28349364f),
            new Vec2 (0.875f, 0.28349364f),
            new Vec2 (0.0f, 0.5f),
            new Vec2 (0.25f, 0.5f),
            new Vec2 (0.5f, 0.5f),
            new Vec2 (0.75f, 0.5f),
            new Vec2 (1.0f, 0.5f),
            new Vec2 (0.125f, 0.71650636f),
            new Vec2 (0.375f, 0.71650636f),
            new Vec2 (0.625f, 0.71650636f),
            new Vec2 (0.875f, 0.71650636f),
        };

        target.normals = new Vec3[]
        {
            new Vec3 (-0.57735026f, -0.57735026f, -0.57735026f),
            new Vec3 (0.57735026f, -0.57735026f, -0.57735026f),
            new Vec3 (-0.57735026f, 0.57735026f, -0.57735026f),
            new Vec3 (0.57735026f, 0.57735026f, -0.57735026f),
            new Vec3 (-0.57735026f, -0.57735026f, 0.57735026f),
            new Vec3 (0.57735026f, -0.57735026f, 0.57735026f),
            new Vec3 (-0.57735026f, 0.57735026f, 0.57735026f),
            new Vec3 (0.57735026f, 0.57735026f, 0.57735026f),
        };

        Loop3[] fs = target.loops = Loop3.Resize(target.loops, 8, 3, true);
        Loop3.Tri(new Index3(0, 9, 0), new Index3(1, 5, 0), new Index3(2, 4, 0), fs[0]);
        Loop3.Tri(new Index3(0, 10, 1), new Index3(3, 6, 1), new Index3(1, 5, 1), fs[1]);
        Loop3.Tri(new Index3(0, 11, 2), new Index3(4, 7, 2), new Index3(3, 6, 2), fs[3]);
        Loop3.Tri(new Index3(0, 12, 3), new Index3(2, 8, 3), new Index3(4, 7, 3), fs[2]);
        Loop3.Tri(new Index3(2, 4, 4), new Index3(1, 5, 4), new Index3(5, 0, 4), fs[4]);
        Loop3.Tri(new Index3(1, 5, 5), new Index3(3, 6, 5), new Index3(5, 1, 5), fs[5]);
        Loop3.Tri(new Index3(4, 7, 6), new Index3(2, 8, 6), new Index3(5, 3, 6), fs[6]);
        Loop3.Tri(new Index3(3, 6, 7), new Index3(4, 7, 7), new Index3(5, 2, 7), fs[7]);

        return target;
    }

    /// <summary>
    /// Creates a torus, or doughnut. The hole opens onto the y axis.
    /// </summary>
    /// <param name="target">output mesh</param>
    /// <param name="sectors">number of sectors</param>
    /// <param name="panels">number of panels</param>
    /// <param name="thickness">thickness</param>
    /// <param name="radius">radius</param>
    /// <param name="poly">polygon type</param>
    /// <returns>the torus</returns>
    public static Mesh3 Torus( //
        in Mesh3 target, //
        in int sectors = 32, //
        in int panels = 16, //
        in float thickness = 0.5f, //
        in float radius = 0.5f, //
        in PolyType poly = PolyType.Tri)
    {
        // Validate arguments.
        int vSect = sectors < 3 ? 3 : sectors;
        int vPanl = panels < 3 ? 3 : panels;
        float vThick = Utils.Clamp(thickness, Utils.Epsilon, 1.0f -
            Utils.Epsilon);

        // Radii.
        float rho0 = MathF.Max(Utils.Epsilon, radius);
        float rho1 = rho0 * vThick;

        // Values for array accesses. 
        int vSect1 = vSect + 1;
        int vPanl1 = vPanl + 1;
        int vLen = vPanl * vSect;
        int vtLen = vPanl1 * vSect1;
        bool isTri = poly == PolyType.Tri;
        int fsLen = isTri ? vLen + vLen : vLen;

        // Allocate mesh data.
        Vec3[] vs = target.coords = new Vec3[vLen];
        Vec2[] vts = target.texCoords = new Vec2[vtLen];
        Vec3[] vns = target.normals = new Vec3[vLen];
        Loop3[] fs = target.loops = Loop3.Resize(target.loops, fsLen, isTri ? 3 : 4, true);

        // Index conversions.
        float toU = 1.0f / vSect;
        float toV = 1.0f / vPanl;
        float toTheta = Utils.Tau / vSect;
        float toPhi = Utils.Tau / vPanl;

        int faceIdx = 0;
        int faceStride = isTri ? 2 : 1;

        for (int k = 0; k < vLen; ++k)
        {
            int i = k / vSect;
            int j = k % vSect;

            // Find theta and phi.
            float phi = -MathF.PI + i * toPhi;
            float theta = j * toTheta;

            float cosPhi = MathF.Cos(phi);
            float sinPhi = MathF.Sin(phi);
            float cosTheta = MathF.Cos(theta);
            float sinTheta = MathF.Sin(theta);

            float rhoCosPhi = rho0 + rho1 * cosPhi;
            float rhoSinPhi = rho1 * sinPhi;

            vs[k] = new Vec3(rhoCosPhi * cosTheta, -rhoSinPhi, rhoCosPhi * sinTheta);
            vns[k] = new Vec3(cosPhi * cosTheta, -sinPhi, cosPhi * sinTheta);

            int iVNext = (i + 1) % vPanl;

            int vOffCurr = i * vSect;
            int vOffNext = iVNext * vSect;

            int vtOffCurr = i * vSect1;
            int vtOffNext = vtOffCurr + vSect1;

            int jVtNext = j + 1;
            int jVNext = jVtNext % vSect;

            /* Coordinate and normal indices. */
            int v00 = vOffCurr + j;
            int v10 = vOffCurr + jVNext;
            int v11 = vOffNext + jVNext;
            int v01 = vOffNext + j;

            /* Texture coordinate indices. */
            int vt00 = vtOffCurr + j;
            int vt10 = vtOffCurr + jVtNext;
            int vt11 = vtOffNext + jVtNext;
            int vt01 = vtOffNext + j;

            if (isTri)
            {
                Loop3 tri0 = fs[faceIdx];
                tri0.Indices[0] = new Index3(v00, vt00, v00);
                tri0.Indices[1] = new Index3(v10, vt10, v10);
                tri0.Indices[2] = new Index3(v11, vt11, v11);

                Loop3 tri1 = fs[faceIdx];
                tri1.Indices[0] = new Index3(v00, vt00, v00);
                tri1.Indices[1] = new Index3(v11, vt11, v11);
                tri1.Indices[2] = new Index3(v01, vt01, v01);
            }
            else
            {
                Loop3 quad = fs[faceIdx];
                quad.Indices[0] = new Index3(v00, vt00, v00);
                quad.Indices[1] = new Index3(v10, vt10, v10);
                quad.Indices[2] = new Index3(v11, vt11, v11);
                quad.Indices[3] = new Index3(v01, vt01, v01);
            }

            faceIdx += faceStride;
        }

        for (int k = 0; k < vtLen; ++k)
        {
            vts[k] = new Vec2(
                k % vSect1 * toU,
                1.0f - k / vSect1 * toV);
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
    public static string ToString(in Mesh3 m, //
        in int padding = 1, //
        in int places = 4)
    {
        return Mesh3.ToString(new StringBuilder(2048),
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
    public static StringBuilder ToString( //
        in StringBuilder sb, //
        in Mesh3 m, //
        in int padding = 1, //
        in int places = 4)
    {
        sb.Append("{ loops: ");
        Loop3.ToString(sb, m.loops, padding);
        sb.Append(", coords: ");
        Vec3.ToString(sb, m.coords, places);
        sb.Append(", texCoords: ");
        Vec2.ToString(sb, m.texCoords, places);
        sb.Append(", normals: ");
        Vec3.ToString(sb, m.normals, places);
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
    public static Mesh3 Triangulate(in Mesh3 source, in Mesh3 target)
    {
        Loop3[] loopsSrc = source.loops;
        Vec3[] vsSrc = source.coords;
        Vec2[] vtsSrc = source.texCoords;
        Vec3[] vnsSrc = source.normals;

        // Cannot anticipate how many loops in the source mesh will not be
        // triangles, so this is an expanding list.
        List<Loop3> loopsTrg = new List<Loop3>();

        int loopSrcLen = loopsSrc.Length;
        for (int i = 0; i < loopSrcLen; ++i)
        {
            Loop3 fSrc = loopsSrc[i];
            Index3[] vertsSrc = fSrc.Indices;
            int fSrcLen = vertsSrc.Length;

            // If face loop is not a triangle, then split.
            if (fSrcLen > 3)
            {

                // Find last non-adjacent index. For index m, neither m + 1 nor
                // m - 1, so where m = 0, the last non-adjacent would be
                // arr.Length - 2.
                Index3 vert0 = vertsSrc[0];
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
            target.coords = new Vec3[vsLen];
            System.Array.Copy(vsSrc, target.coords, vsLen);

            int vtsLen = vtsSrc.Length;
            target.texCoords = new Vec2[vtsLen];
            System.Array.Copy(vtsSrc, target.texCoords, vtsLen);

            int vnsLen = vnsSrc.Length;
            target.normals = new Vec3[vnsSrc.Length];
            System.Array.Copy(vnsSrc, target.normals, vnsLen);
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
    public static Mesh3 UniformData(in Mesh3 source, in Mesh3 target)
    {
        // TODO: Account for cases where source == target
        // vs. source != target.

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
}