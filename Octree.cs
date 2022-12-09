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
    /// The number of children created when a node is split.
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
    protected readonly List<Octree> children = new List<Octree>(Octree.ChildCount);

    /// <summary>
    /// The number of elements a node can hold before
    /// it is split into child nodes.
    /// </summary>
    protected int capacity;

    /// <summary>
    /// The depth, or level, of the node.
    /// </summary>
    protected int level;

    /// <summary>
    /// Elements contained by the node if it is leaf.
    /// </summary>
    protected readonly SortedSet<Vec3> points = new SortedSet<Vec3>();

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

        protected set
        {
            this.bounds = value;
        }
    }

    /// <summary>
    /// The number of elements a node can hold before
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
            if (value > 0)
            {
                this.capacity = value;
                if (this.points.Count > this.capacity)
                {
                    this.Split(this.capacity);
                }
            }
        }
    }

    /// <summary>
    /// The depth, or level, of the node.
    /// </summary>
    /// <value>level</value>
    public int Level
    {
        get
        {
            return this.level;
        }

        protected set
        {
            this.level = value < Octree.RootLevel ? Octree.RootLevel : value;
        }
    }

    /// <summary>
    /// Constructs an octree with a boundary and capacity.
    /// </summary>
    /// <param name="bounds">bounds</param>
    /// <param name="capacity">capacity</param>
    public Octree(in Bounds3 bounds, in int capacity = Octree.DefaultCapacity) :
        this(bounds, capacity, Octree.RootLevel)
    { }

    /// <summary>
    /// Constructs an octree with a boundary and capacity at a level.
    /// </summary>
    /// <param name="bounds">bounds</param>
    /// <param name="capacity">capacity</param>
    /// <param name="level">level</param>
    protected Octree(in Bounds3 bounds, in int capacity, in int level)
    {
        this.Bounds = bounds;
        this.Capacity = capacity;
        this.Level = level;
    }

    /// <summary>
    /// Returns a string representation of this node.
    /// </summary>
    /// <returns>string</returns>
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
            foreach (Octree child in this.children)
            {
                if (child.Insert(v)) { return true; }
            }

            if (Octree.IsLeaf(this))
            {
                this.points.Add(v);
                if (this.points.Count > this.capacity)
                {
                    this.Split(this.capacity);
                }
                return true;
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
    /// Inserts points into the node. Returns true if all
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
    /// Inserts points into the node. Returns true if all
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
            for (int j = 0; !found && j < Octree.ChildCount; ++j)
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
    /// Subdivides this octree. For cases where a minimum number of children
    /// nodes is desired, independent of point insertion. The result will be
    /// the child count raised to the power of iterations, e.g., 8,
    /// 64, 512.
    /// </summary>
    /// <param name="iterations">iterations</param>
    /// <returns>this octree</returns>
    public Octree Subdivide(in int iterations = 1)
    {
        return this.Subdivide(iterations, this.capacity);
    }

    /// <summary>
    /// Subdivides this octree. For cases where a minimum number of children
    /// nodes is desired, independent of point insertion. The result will be
    /// the child count raised to the power of iterations, e.g., 8,
    /// 64, 512.
    /// </summary>
    /// <param name="iterations">iterations</param>
    /// <param name="childCapacity">child capacity</param>
    /// <returns>this octree</returns>
    public Octree Subdivide(in int iterations, in int childCapacity)
    {
        if (iterations < 1) { return this; }
        for (int i = 0; i < iterations; ++i)
        {
            foreach (Octree child in this.children)
            {
                child.Subdivide(iterations - 1, childCapacity);
            }
            if (Octree.IsLeaf(this)) { this.Split(childCapacity); }
        }

        return this;
    }

    /// <summary>
    /// Finds the average center in each leaf node.
    /// If include empty is true, then empty
    /// leaf nodes will append the center of their bounds instead.
    /// </summary>
    /// <param name="o">octree</param>
    /// <param name="includeEmpty">include empty</param>
    /// <returns>centers array</returns>
    public static Vec3[] CentersMean(
        in Octree o,
        in bool includeEmpty = false)
    {
        List<Vec3> result = new List<Vec3>();
        Octree.CentersMean(o, result, includeEmpty);
        return result.ToArray();
    }

    /// <summary>
    /// Finds the average center in each leaf node.
    /// If include empty is true, then empty
    /// leaf nodes will append the center of their bounds instead.
    /// </summary>
    /// <param name="o">octree</param>
    /// <param name="target">target</param>
    /// <param name="includeEmpty">include empty</param>
    /// <returns>target list</returns>
    protected static List<Vec3> CentersMean(
        in Octree o,
        in List<Vec3> target,
        in bool includeEmpty = false)
    {
        foreach (Octree child in o.children)
        {
            Octree.CentersMean(child, target, includeEmpty);
        }

        if (Octree.IsLeaf(o))
        {
            int ptsLen = o.points.Count;
            if (ptsLen > 1)
            {
                Vec3 sum = Vec3.Zero;
                foreach (Vec3 v in o.points) { sum += v; }
                target.Add(sum / ptsLen);
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
    /// Counts the number of leaves held by a node.
    /// Returns 1 if the node is itself a leaf.
    /// </summary>
    /// <param name="o">octree</param>
    /// <returns>sum</returns>
    public static int CountLeaves(in Octree o)
    {
        if (Octree.IsLeaf(o)) { return 1; }
        int sum = 0;
        foreach (Octree child in o.children)
        {
            sum += Octree.CountLeaves(child);
        }
        return sum;
    }

    /// <summary>
    /// Counts the number of points held by the leaf nodes.
    /// </summary>
    /// <param name="o">octree</param>
    /// <returns>sum</returns>
    public static int CountPoints(in Octree o)
    {
        if (Octree.IsLeaf(o)) { return o.points.Count; }
        int sum = 0;
        foreach (Octree child in o.children)
        {
            sum += Octree.CountPoints(child);
        }
        return sum;
    }

    /// <summary>
    /// Creates an octree from an array of points.
    /// </summary>
    /// <param name="points">points</param>
    /// <param name="capacity">capacity</param>
    /// <returns>octree</returns>
    public static Octree FromPoints(
        in IEnumerable<Vec3> points,
        in int capacity = Octree.DefaultCapacity)
    {
        Bounds3 b = Bounds3.FromPoints(points);
        Octree o = new Octree(b, capacity);
        o.InsertAll(points);
        return o;
    }

    /// <summary>
    /// Evaluates whether the node has any children.
    /// Returns true if no; otherwise false.
    /// </summary>
    /// <param name="o">octree</param>
    /// <returns>evaluation</returns>
    public static bool IsLeaf(in Octree o)
    {
        return o.children.Count < 1;
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
            int lvl = Octree.MaxLevel(child);
            if (lvl > mxLvl) { mxLvl = lvl; }
        }
        return mxLvl;
    }

    /// <summary>
    /// Queries a node with a box range.
    /// </summary>
    /// <param name="o">octree</param>
    /// <param name="range">bounds</param>
    /// <returns>found points</returns>
    public static Vec3[] Query(
        in Octree o,
        in Bounds3 range)
    {
        SortedDictionary<float, Vec3> found = new SortedDictionary<float, Vec3>();
        Octree.Query(o, range, found);
        Vec3[] arr = new Vec3[found.Count];
        found.Values.CopyTo(arr, 0);
        return arr;
    }

    /// <summary>
    /// Queries a node with a spherical range.
    /// </summary>
    /// <param name="o">octree</param>
    /// <param name="center">sphere center</param>
    /// <param name="radius">sphere radius</param>
    /// <returns>found points</returns>
    public static Vec3[] Query(
        in Octree o,
        in Vec3 center,
        in float radius)
    {
        SortedDictionary<float, Vec3> found = new SortedDictionary<float, Vec3>();
        Octree.Query(o, center, radius, found);
        Vec3[] arr = new Vec3[found.Count];
        found.Values.CopyTo(arr, 0);
        return arr;
    }

    /// <summary>
    /// Queries a node with a box range.
    /// Appends results to a dictionary where the Chebyshev
    /// distance is the key and the point is the value.
    /// </summary>
    /// <param name="o">octree</param>
    /// <param name="range">bounds</param>
    /// <param name="found">found dictionary</param>
    /// <returns>distance points dictionary</returns>
    protected static SortedDictionary<float, Vec3> Query(
        in Octree o,
        in Bounds3 range,
        in SortedDictionary<float, Vec3> found)
    {
        if (Bounds3.Intersects(o.bounds, range))
        {
            foreach (Octree child in o.children)
            {
                Octree.Query(child, range, found);
            }

            if (Octree.IsLeaf(o))
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

        return found;
    }

    /// <summary>
    /// Queries a node with a spherical range.
    /// Appends results to a dictionary where the distance
    /// squared is the key and the point is the value.
    /// </summary>
    /// <param name="o">octree</param>
    /// <param name="center">sphere center</param>
    /// <param name="radius">sphere radius</param>
    /// <param name="found">found dictionary</param>
    /// <returns>distance points dictionary</returns>
    protected static SortedDictionary<float, Vec3> Query(
        in Octree o,
        in Vec3 center,
        in float radius,
        in SortedDictionary<float, Vec3> found)
    {
        if (Bounds3.Intersects(o.bounds, center, radius))
        {
            foreach (Octree child in o.children)
            {
                Octree.Query(child, center, radius, found);
            }

            if (Octree.IsLeaf(o))
            {
                float rsq = radius * radius;
                foreach (Vec3 v in o.points)
                {
                    float dsq = Vec3.DistSq(center, v);
                    if (dsq < rsq)
                    {
                        found[dsq] = v;
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
    public static StringBuilder ToString(
        in StringBuilder sb,
        in Octree o,
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
            int i = -1;
            int last = points.Count - 1;
            foreach (Vec3 v in points)
            {
                ++i;
                Vec3.ToString(sb, v, places);
                if (i < last) sb.Append(", ");
            }
            sb.Append(' ');
            sb.Append(']');
        }
        else
        {
            sb.Append(", children: [ ");
            List<Octree> children = o.children;
            int last = children.Count - 1;
            for (int i = 0; i < last; ++i)
            {
                Octree child = children[i];
                Octree.ToString(sb, child, places);
                sb.Append(", ");
            }
            Octree.ToString(sb, children[last], places);

            sb.Append(' ');
            sb.Append(']');
        }

        sb.Append(' ');
        sb.Append('}');
        return sb;
    }
}