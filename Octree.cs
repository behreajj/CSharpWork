using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Partitions space to improve collision and intersection tests. An octree
/// node holds a list of points up to a given capacity; when that capacity
/// is exceeded, the node is split into eight children nodes (octants) and
/// its list of points is emptied into them. The octants are indexed in an
/// array from negative (z, y, x) to positive (z, y, x).
/// </summary>
public class Octree
{
    /// <summary>
    /// The number of children created when an octree
    /// node is split.
    /// </summary>
    public const int ChildCount = 8;

    /// <summary>
    /// The default number of elements a node can hold.
    /// </summary>
    public const int DefaultCapacity = 16;

    /// <summary>
    /// The root level, or depth.
    /// </summary>
    public const int RootLevel = 0;

    /// <summary>
    /// The bounding volume.
    /// </summary>
    protected Bounds3 bounds;

    /// <summary>
    /// Children nodes.
    /// </summary>
    protected List<Octree> children = new List<Octree>();

    /// <summary>
    /// The number of elements an octree can hold before
    /// it is split into child nodes.
    /// </summary>
    protected int capacity;

    /// <summary>
    /// The depth, or level, of the octree node.
    /// </summary>
    protected readonly int level;

    /// <summary>
    /// Elements contained by this octree node if it is leaf.
    /// </summary>
    protected SortedSet<Vec3> points = new SortedSet<Vec3>();

    /// <summary>
    /// The bounding volume.
    /// </summary>
    /// <value>bounds</value>
    public Bounds3 Bounds
    {
        get
        {
            return this.bounds;
        }

        set
        {
            this.bounds = value;
        }
    }

    /// <summary>
    /// The number of elements an octree can hold before
    /// it is split into child nodes.
    /// </summary>
    /// <value>capacity</value>
    public int Capacity
    {
        get
        {
            return this.capacity;
        }

        set
        {
            this.capacity = value < 1 ? 1 : value;
        }
    }

    /// <summary>
    /// The depth, or level, of the octree node.
    /// </summary>
    /// <value>level</value>
    public int Level { get { return this.level; } }

    /// <summary>
    /// Constructs an octree within the unit cube.
    /// </summary>
    public Octree() : this(Bounds3.UnitCubeSigned) { }

    /// <summary>
    /// Constructs an octree with a boundary and capacity at a level.
    /// </summary>
    /// <param name="bounds">bounds</param>
    /// <param name="capacity">capacity</param>
    /// <param name="level">level</param>
    public Octree( //
        in Bounds3 bounds, //
        in int capacity = Octree.DefaultCapacity, //
        in int level = Octree.RootLevel)
    {
        this.Bounds = bounds;
        this.Capacity = capacity;
        this.level = level < Octree.RootLevel ? Octree.RootLevel : level;
    }

    /// <summary>
    /// Returns a string representation of this octree.
    /// </summary>
    /// <returns>the string</returns>
    public override string ToString()
    {
        return Octree.ToString(this);
    }

    /// <summary>
    /// Inserts a point into the octree. Returns true if the point
    /// was successfully inserted into the octree directly or indirectly through
    /// one of its children. Returns false if the insertion was
    /// unsuccessful.
    /// </summary>
    /// <param name="v">point</param>
    /// <returns>success</returns>
    public bool Insert(in Vec3 v)
    {
        if (Bounds3.ContainsInclExcl(this.bounds, v))
        {
            bool isLeaf = true;
            foreach (Octree child in this.children)
            {
                if (child != null)
                {
                    isLeaf = false;
                    if (child.Insert(v)) { return true; }
                }
            }

            if (isLeaf)
            {
                this.points.Add(v);
            }
            else
            {
                this.Split(this.capacity);
                return this.Insert(v);
            }
        }
        return false;
    }

    /// <summary>
    /// Inserts points into the octree. Returns true if all
    /// insertions were successful; otherwise, returns false.
    /// </summary>
    /// <param name="vs">points</param>
    /// <returns>success</returns>
    public bool InsertAll(params Vec3[] vs)
    {
        bool flag = true;
        foreach (Vec3 v in vs) { flag &= Insert(v); }
        return flag;
    }

    /// <summary>
    /// Inserts points into the octree. Returns true if all
    /// insertions were successful; otherwise, returns false.
    /// </summary>
    /// <param name="vs">points</param>
    /// <returns>success</returns>
    public bool InsertAll(in IEnumerable<Vec3> vs)
    {
        bool flag = true;
        foreach (Vec3 v in vs) { flag &= Insert(v); }
        return flag;
    }

    /// <summary>
    /// Splits this octree node into eight child nodes.
    /// </summary>
    /// <param name="childCapacity">child capacity</param>
    /// <returns>this octree</returns>
    protected Octree Split(in int childCapacity = Octree.DefaultCapacity)
    {
        this.children.Clear();
        int nextLevel = this.level + 1;
        Bounds3[] childrenBounds = Bounds3.Split(this.bounds, 0.5f, 0.5f, 0.5f);
        for (int i = 0; i < Octree.ChildCount; ++i)
        {
            this.children.Add(new Octree(childrenBounds[i], childCapacity, nextLevel));
        }

        // Pass on points to children.
        // Begin search for the appropriate child node at the
        // index where the previous point was inserted.
        int idxOffset = 0;
        foreach (Vec3 v in this.points)
        {
            bool found = false;
            for (int j = 0; j < Octree.ChildCount && !found; ++j)
            {
                int k = (idxOffset + j) % Octree.ChildCount;
                found = this.children[k].Insert(v);
                if (found) { idxOffset = k; }
            }
        }
        this.points.Clear();

        return this;
    }

    /// <summary>
    /// Finds the average center in each leaf node of an octree.
    /// If include empty is true, then empty
    /// leaf nodes will append the center of their bounds instead.
    /// </summary>
    /// <param name="o">octree</param>
    /// <param name="includeEmpty">include empty</param>
    /// <returns>centers array</returns>
    public static Vec3[] CentersMean( //
        in Octree o, //
        in bool includeEmpty = false)
    {
        List<Vec3> result = new List<Vec3>();
        Octree.CentersMean(o, result, includeEmpty);
        return result.ToArray();
    }

    /// <summary>
    /// Finds the average center in each leaf node of an octree.
    /// If include empty is true, then empty
    /// leaf nodes will append the center of their bounds instead.
    /// </summary>
    /// <param name="o">octree</param>
    /// <param name="target">target</param>
    /// <param name="includeEmpty">include empty</param>
    /// <returns>target list</returns>
    protected static List<Vec3> CentersMean( //
        in Octree o, //
        in List<Vec3> target, //
        in bool includeEmpty = false)
    {
        bool isLeaf = true;
        foreach (Octree child in o.children)
        {
            if (child != null)
            {
                isLeaf = false;
                Octree.CentersMean(child, target, includeEmpty);
            }
        }

        if (isLeaf)
        {
            int ptsLen = o.points.Count;
            if (ptsLen > 1)
            {
                Vec3 sum = Vec3.Zero;
                foreach (Vec3 v in o.points) { sum += v; }
                Vec3 avg = sum / ptsLen;
                target.Add(avg);
            }
            else if (ptsLen > 0)
            {
                target.AddRange(o.points);
            }
            else if (includeEmpty)
            {
                target.Add(Bounds3.Center(o.bounds));
            }
        }

        return target;
    }

    /// <summary>
    /// Counts the number of leaves held by an octree.
    /// Returns 1 if the node is itself a leaf.
    /// </summary>
    /// <param name="o">octree</param>
    /// <returns>sum</returns>
    public static int CountLeaves(in Octree o)
    {
        int sum = 0;
        bool isLeaf = true;
        foreach (Octree child in o.children)
        {
            if (child != null)
            {
                isLeaf = false;
                sum += Octree.CountLeaves(child);
            }
        }

        if (isLeaf) { return 1; }
        return sum;
    }

    /// <summary>
    /// Counts the number of points held by an octree's leaf nodes.
    /// </summary>
    /// <param name="o">octree</param>
    /// <returns>sum</returns>
    public static int CountPoints(in Octree o)
    {
        int sum = 0;
        bool isLeaf = true;
        foreach (Octree child in o.children)
        {
            if (child != null)
            {
                isLeaf = false;
                sum += Octree.CountPoints(child);
            }
        }

        if (isLeaf) { sum += o.points.Count; }
        return sum;
    }

    /// <summary>
    /// Creates an octree from an array of points.
    /// </summary>
    /// <param name="points">points</param>
    /// <param name="capacity">capacity</param>
    /// <returns>octree</returns>
    public static Octree FromPoints(
        in IEnumerable<Vec3> points, //
        in int capacity = Octree.DefaultCapacity)
    {
        Bounds3 b = Bounds3.FromPoints(points);
        Octree o = new Octree(b, capacity, Octree.RootLevel);
        o.InsertAll(points);
        return o;
    }

    /// <summary>
    /// Evaluates whether this quadtree node has any children.
    /// Returns true if no; otherwise false.
    /// </summary>
    /// <param name="o">octree</param>
    /// <returns>evaluation</returns>
    public static bool IsLeaf(in Octree o)
    {
        foreach (Octree child in o.children)
        {
            if (child != null) { return false; }
        }
        return true;
    }

    /// <summary>
    /// Gets the maximum level, or depth, of the node and its children.
    /// </summary>
    /// <param name="o">octree</param>
    /// <returns>level</returns>
    public static int MaxLevel(in Octree o)
    {
        int mxLvl = o.level;
        foreach (Octree child in o.children)
        {
            if (child != null)
            {
                int lvl = Octree.MaxLevel(child);
                if (lvl > mxLvl) { mxLvl = lvl; }
            }
        }
        return mxLvl;
    }

    /// <summary>
    /// Queries an octree with a box range.
    /// </summary>
    /// <param name="o">octree</param>
    /// <param name="range">bounds</param>
    /// <returns>found points</returns>
    public static Vec3[] Query( //
        in Octree o, //
        in Bounds3 range)
    {
        SortedDictionary<float, Vec3> found = new SortedDictionary<float, Vec3>();
        Octree.Query(o, range, found);
        Vec3[] arr = new Vec3[found.Count];
        found.Values.CopyTo(arr, 0);
        return arr;
    }

    /// <summary>
    /// Queries an octree with a spherical range.
    /// </summary>
    /// <param name="o">octree</param>
    /// <param name="center">sphere center</param>
    /// <param name="radius">sphere radius</param>
    /// <returns>found points</returns>
    public static Vec3[] Query( //
        in Octree o, //
        in Vec3 center, //
        in float radius)
    {
        SortedDictionary<float, Vec3> found = new SortedDictionary<float, Vec3>();
        Octree.Query(o, center, radius, found);
        Vec3[] arr = new Vec3[found.Count];
        found.Values.CopyTo(arr, 0);
        return arr;
    }

    /// <summary>
    /// Queries an octree with a box range.
    /// Appends results to a dictionary where the distance
    /// is the key and the point is the value.
    /// </summary>
    /// <param name="o">octree</param>
    /// <param name="range">bounds</param>
    /// <param name="found">found dictionary</param>
    /// <returns>distance points dictionary</returns>
    protected static SortedDictionary<float, Vec3> Query( //
        in Octree o, //
        in Bounds3 range, //
        in SortedDictionary<float, Vec3> found)
    {
        if (Bounds3.Intersect(o.bounds, range))
        {
            bool isLeaf = false;
            foreach (Octree child in o.children)
            {
                if (child != null)
                {
                    isLeaf = false;
                    Octree.Query(child, range, found);
                }

                if (isLeaf)
                {
                    Vec3 rCenter = Bounds3.Center(range);
                    foreach (Vec3 v in o.points)
                    {
                        if (Bounds3.ContainsInclExcl(range, v))
                        {
                            found[Vec3.DistChebyshev(v, rCenter)] = v;
                        }
                    }
                }
            }
        }

        return found;
    }

    /// <summary>
    /// Queries an octree with a spherical range.
    /// Appends results to a dictionary where the distance
    /// is the key and the point is the value.
    /// </summary>
    /// <param name="o">octree</param>
    /// <param name="center">sphere center</param>
    /// <param name="radius">sphere radius</param>
    /// <param name="found">found dictionary</param>
    /// <returns>distance points dictionary</returns>
    protected static SortedDictionary<float, Vec3> Query( //
        in Octree o, //
        in Vec3 center, //
        in float radius, //
        in SortedDictionary<float, Vec3> found)
    {
        if (Bounds3.Intersect(o.bounds, center, radius))
        {
            bool isLeaf = false;
            foreach (Octree child in o.children)
            {
                if (child != null)
                {
                    isLeaf = false;
                    Octree.Query(child, center, radius, found);
                }
            }

            if (isLeaf)
            {
                float rsq = radius * radius;
                foreach (Vec3 v in o.points)
                {
                    float dsq = Vec3.DistSq(center, v);
                    if (dsq < rsq)
                    {
                        found[MathF.Sqrt(dsq)] = v;
                    }
                }
            }
        }

        return found;
    }

    /// <summary>
    /// Returns a string representation of an octree.
    /// </summary>
    /// <param name="o">octree</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string</returns>
    public static string ToString(in Octree o, in int places = 4)
    {
        return Octree.ToString(new StringBuilder(2048),
            o, places).ToString();
    }

    /// <summary>
    /// Appendsa a representation of an octree to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="o">octree</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString( //
        in StringBuilder sb, //
        in Octree o, //
        in int places = 4)
    {
        sb.Append("{ bounds: ");
        Bounds3.ToString(sb, o.bounds, places);
        sb.Append(", capacity: ");
        sb.Append(o.capacity);

        if (Octree.IsLeaf(o))
        {
            sb.Append(", points: [ ");
            SortedSet<Vec3> points = o.points;
            int i = 0;
            int last = points.Count - 1;
            foreach (Vec3 v in points)
            {
                Vec3.ToString(v, places);
                if (i < last) sb.Append(", ");
                ++i;
            }
            sb.Append(' ');
            sb.Append(']');
        }
        else
        {
            sb.Append(", children: [ ");
            List<Octree> children = o.children;
            int last = children.Count;
            for (int i = 0; i < last; ++i)
            {
                Octree child = children[i];
                if (child != null)
                {
                    Octree.ToString(child, places);
                    sb.Append(", ");
                }
            }

            if (children[last] != null)
            {
                Octree.ToString(children[last], places);
            }

            sb.Append(' ');
            sb.Append(']');
        }

        sb.Append(' ');
        sb.Append('}');
        return sb;
    }
}