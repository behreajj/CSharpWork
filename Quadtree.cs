using System.Collections.Generic;
using System.Text;

/// <summary>
/// Partitions space to improve collision and intersection tests. A quadtree
/// node holds a list of points up to a given capacity; when that capacity
/// is exceeded, the node is split into four children nodes (quadrants) and
/// its list of points is emptied into them. The quadrants are indexed in an
/// array from negative (y, x) to positive (y, x).
/// </summary>
public class Quadtree
{
    /// <summary>
    /// The number of children created when a node is split.
    /// </summary>
    public const int ChildCount = 4;

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
    protected Bounds2 bounds;

    /// <summary>
    /// Children nodes.
    /// </summary>
    protected readonly List<Quadtree> children = new(Quadtree.ChildCount);

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
    protected readonly SortedSet<Vec2> points = new();

    /// <summary>
    /// The bounding volume.
    /// </summary>
    /// <value>bounds</value>
    public Bounds2 Bounds
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
            this.level = value < Quadtree.RootLevel ?
                Quadtree.RootLevel : value;
        }
    }

    /// <summary>
    /// Constructs an octree with a boundary and capacity.
    /// </summary>
    /// <param name="bounds">bounds</param>
    /// <param name="capacity">capacity</param>
    public Quadtree(in Bounds2 bounds, in int capacity = Quadtree.DefaultCapacity) :
        this(bounds, capacity, Quadtree.RootLevel)
    { }

    /// <summary>
    /// Constructs a quadtree with a boundary and capacity at a level.
    /// </summary>
    /// <param name="bounds">bounds</param>
    /// <param name="capacity">capacity</param>
    /// <param name="level">level</param>
    protected Quadtree(in Bounds2 bounds, in int capacity, in int level)
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
        return Quadtree.ToString(this);
    }

    /// <summary>
    /// Removes empty child nodes from the quadtree.
    /// Returns true if this quadtree node should be
    /// removed, i.e., it has no children and
    /// its points array is empty.
    /// 
    /// This should only be called after all points
    /// have been inserted into the tree.
    /// </summary>
    /// <returns>evaluation</returns>
    public bool Cull()
    {
        int cullThis = 0;
        int lenChildren = this.children.Count;
        for (int i = lenChildren - 1; i > -1; --i)
        {
            Quadtree child = this.children[i];
            if (child.Cull())
            {
                this.children.RemoveAt(i);
                ++cullThis;
            }
        }
        return cullThis >= lenChildren &&
            this.points.Count < 1;
    }

    /// <summary>
    /// Inserts a point into the quadtree. Returns true if the point
    /// was successfully inserted into the quadtree directly or indirectly through
    /// one of its children. Returns false if the insertion was
    /// unsuccessful.
    /// </summary>
    /// <param name="v">point</param>
    /// <returns>success</returns>
    public bool Insert(in Vec2 v)
    {
        if (Bounds2.ContainsInclExcl(this.bounds, v))
        {
            foreach (Quadtree child in this.children)
            {
                if (child.Insert(v)) { return true; }
            }

            if (Quadtree.IsLeaf(this))
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
    public bool InsertAll(params Vec2[] vs)
    {
        bool flag = true;
        foreach (Vec2 v in vs) { flag &= Insert(v); }
        return flag;
    }

    /// <summary>
    /// Inserts points into the node. Returns true if all
    /// insertions were successful; otherwise, returns false.
    /// </summary>
    /// <param name="vs">points</param>
    /// <returns>success</returns>
    public bool InsertAll(in IEnumerable<Vec2> vs)
    {
        bool flag = true;
        foreach (Vec2 v in vs) { flag &= Insert(v); }
        return flag;
    }

    /// <summary>
    /// Splits this quadtree node into four child nodes.
    /// </summary>
    /// <param name="childCapacity">child capacity</param>
    /// <returns>quadtree</returns>
    protected Quadtree Split(in int childCapacity = Quadtree.DefaultCapacity)
    {
        this.children.Clear();
        int nextLevel = this.level + 1;
        Bounds2[] childrenBounds = Bounds2.Split(this.bounds, 0.5f, 0.5f);
        for (int i = 0; i < Quadtree.ChildCount; ++i)
        {
            this.children.Add(new Quadtree(childrenBounds[i],
                childCapacity, nextLevel));
        }

        // Pass on points to children.
        // Begin search for the appropriate child node at the
        // index where the previous point was inserted.
        int idxOffset = 0;
        foreach (Vec2 v in this.points)
        {
            bool found = false;
            for (int j = 0; !found && j < Quadtree.ChildCount; ++j)
            {
                int k = (idxOffset + j) % Quadtree.ChildCount;
                found = this.children[k].Insert(v);
                if (found) { idxOffset = k; }
            }
        }
        this.points.Clear();

        return this;
    }

    /// <summary>
    /// Finds the average center in each leaf node.
    /// If include empty is true, then empty
    /// leaf nodes will append the center of their bounds instead.
    /// </summary>
    /// <param name="q">quadtree</param>
    /// <param name="includeEmpty">include empty</param>
    /// <returns>centers array</returns>
    public static Vec2[] CentersMean(
        in Quadtree q,
        in bool includeEmpty = false)
    {
        List<Vec2> result = new();
        Quadtree.CentersMean(q, result, includeEmpty);
        return result.ToArray();
    }

    /// <summary>
    /// Finds the average center in each leaf node.
    /// If include empty is true, then empty
    /// leaf nodes will append the center of their bounds instead.
    /// </summary>
    /// <param name="q">quadtree</param>
    /// <param name="target">target</param>
    /// <param name="includeEmpty">include empty</param>
    /// <returns>target list</returns>
    internal static List<Vec2> CentersMean(
        in Quadtree q,
        in List<Vec2> target,
        in bool includeEmpty = false)
    {
        foreach (Quadtree child in q.children)
        {
            Quadtree.CentersMean(child, target, includeEmpty);
        }

        if (Quadtree.IsLeaf(q))
        {
            int ptsLen = q.points.Count;
            if (ptsLen > 1)
            {
                Vec2 sum = Vec2.Zero;
                foreach (Vec2 v in q.points) { sum += v; }
                target.Add(sum / ptsLen);
            }
            else if (ptsLen > 0)
            {
                target.AddRange(q.points);
            }
            else if (includeEmpty)
            {
                target.Add(Bounds2.Center(q.bounds));
            }
        }

        return target;
    }

    /// <summary>
    /// Counts the number of leaves held by a node.
    /// Returns 1 if the node is itself a leaf.
    /// </summary>
    /// <param name="q">quadtree</param>
    /// <returns>sum</returns>
    public static int CountLeaves(in Quadtree q)
    {
        if (Quadtree.IsLeaf(q)) { return 1; }
        int sum = 0;
        foreach (Quadtree child in q.children)
        {
            sum += Quadtree.CountLeaves(child);
        }
        return sum;
    }

    /// <summary>
    /// Counts the number of points held by the leaf nodes.
    /// </summary>
    /// <param name="o">octree</param>
    /// <returns>sum</returns>
    public static int CountPoints(in Quadtree q)
    {
        if (Quadtree.IsLeaf(q)) { return q.points.Count; }
        int sum = 0;
        foreach (Quadtree child in q.children)
        {
            sum += Quadtree.CountPoints(child);
        }
        return sum;
    }

    /// <summary>
    /// Creates a quadtree from an array of points.
    /// </summary>
    /// <param name="points">points</param>
    /// <param name="capacity">capacity</param>
    /// <returns>quadtree</returns>
    public static Quadtree FromPoints(
        in IEnumerable<Vec2> points,
        in int capacity = Quadtree.DefaultCapacity)
    {
        Bounds2 b = Bounds2.FromPoints(points);
        Quadtree o = new(b, capacity);
        o.InsertAll(points);
        return o;
    }

    /// <summary>
    /// Evaluates whether the node has any children.
    /// Returns true if no; otherwise false.
    /// </summary>
    /// <param name="q">quadtree</param>
    /// <returns>evaluation</returns>
    public static bool IsLeaf(in Quadtree q)
    {
        return q.children.Count < 1;
    }

    /// <summary>
    /// Gets the maximum level, or depth, of the node and its children.
    /// </summary>
    /// <param name="q">quadtree</param>
    /// <returns>level</returns>
    public static int MaxLevel(in Quadtree q)
    {
        int mxLvl = q.level;
        foreach (Quadtree child in q.children)
        {
            int lvl = Quadtree.MaxLevel(child);
            if (lvl > mxLvl) { mxLvl = lvl; }
        }
        return mxLvl;
    }

    /// <summary>
    /// Subdivides this node. For cases where a minimum number of children
    /// nodes is desired, independent of point insertion. The result will be
    /// the child count raised to the power of iterations, e.g., 4,
    /// 16, 64.
    /// </summary>
    /// <param name="iterations">iterations</param>
    /// <returns>quadtree</returns>
    public Quadtree Subdivide(in int iterations = 1)
    {
        return this.Subdivide(iterations, this.capacity);
    }

    /// <summary>
    /// Subdivides this node. For cases where a minimum number of children
    /// nodes is desired, independent of point insertion. The result will be
    /// the child count raised to the power of iterations, e.g., 4,
    /// 16, 64.
    /// </summary>
    /// <param name="iterations">iterations</param>
    /// <param name="childCapacity">child capacity</param>
    /// <returns>quadtree</returns>
    public Quadtree Subdivide(in int iterations, in int childCapacity)
    {
        if (iterations < 1) { return this; }
        for (int i = 0; i < iterations; ++i)
        {
            foreach (Quadtree child in this.children)
            {
                child.Subdivide(iterations - 1, childCapacity);
            }
            if (Quadtree.IsLeaf(this)) { this.Split(childCapacity); }
        }

        return this;
    }

    /// <summary>
    /// Returns a string representation of a quadtree.
    /// </summary>
    /// <param name="q">quadtree</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string</returns>
    public static string ToString(in Quadtree q, in int places = 4)
    {
        return Quadtree.ToString(new StringBuilder(1024),
            q, places).ToString();
    }

    /// <summary>
    /// Queries a node with a rectangular range.
    /// </summary>
    /// <param name="q">quadtree</param>
    /// <param name="range">bounds</param>
    /// <returns>found points</returns>
    public static Vec2[] Query(
        in Quadtree q,
        in Bounds2 range)
    {
        SortedList<float, Vec2> found = new(q.capacity);
        Quadtree.Query(q, range, found);
        Vec2[] arr = new Vec2[found.Count];
        found.Values.CopyTo(arr, 0);
        return arr;
    }

    /// <summary>
    /// Queries a node with a circular range.
    /// </summary>
    /// <param name="q">quadtree</param>
    /// <param name="center">sphere center</param>
    /// <param name="radius">sphere radius</param>
    /// <returns>found points</returns>
    public static Vec2[] Query(
        in Quadtree q,
        in Vec2 center,
        in float radius)
    {
        SortedList<float, Vec2> found = new(q.capacity);
        Quadtree.Query(q, center, radius, found);
        Vec2[] arr = new Vec2[found.Count];
        found.Values.CopyTo(arr, 0);
        return arr;
    }

    /// <summary>
    /// Queries a node with a rectangular range.
    /// Appends results to a sorted collection where
    /// the Chebyshev distance is the key and the point
    /// is the value.
    /// </summary>
    /// <param name="q">quadtree</param>
    /// <param name="range">bounds</param>
    /// <param name="found">found dictionary</param>
    /// <returns>distance points dictionary</returns>
    internal static SortedList<float, Vec2> Query(
        in Quadtree q,
        in Bounds2 range,
        in SortedList<float, Vec2> found)
    {
        if (Bounds2.Intersects(q.bounds, range))
        {
            foreach (Quadtree child in q.children)
            {
                Quadtree.Query(child, range, found);
            }

            if (Quadtree.IsLeaf(q))
            {
                Vec2 rCenter = Bounds2.Center(range);
                foreach (Vec2 v in q.points)
                {
                    if (Bounds2.ContainsInclExcl(range, v))
                    {
                        found[Vec2.DistChebyshev(v, rCenter)] = v;
                    }
                }
            }
        }

        return found;
    }

    /// <summary>
    /// Queries a node with a circular range.
    /// Appends results to a sorted collection where 
    /// the distance squared is the key and the point 
    /// is the value.
    /// </summary>
    /// <param name="q">quadtree</param>
    /// <param name="center">sphere center</param>
    /// <param name="radius">sphere radius</param>
    /// <param name="found">found dictionary</param>
    /// <returns>distance points dictionary</returns>
    internal static SortedList<float, Vec2> Query(
        in Quadtree q,
        in Vec2 center,
        in float radius,
        in SortedList<float, Vec2> found)
    {
        if (Bounds2.Intersects(q.bounds, center, radius))
        {
            foreach (Quadtree child in q.children)
            {
                Quadtree.Query(child, center, radius, found);
            }

            if (Quadtree.IsLeaf(q))
            {
                float rsq = radius * radius;
                foreach (Vec2 v in q.points)
                {
                    float dsq = Vec2.DistSq(center, v);
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
    /// Appendsa a representation of a quadtree to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="q">quadtree</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString(
        in StringBuilder sb,
        in Quadtree q,
        in int places = 4)
    {
        sb.Append("{ bounds: ");
        Bounds2.ToString(sb, q.bounds, places);
        sb.Append(", capacity: ");
        sb.Append(q.capacity);

        if (Quadtree.IsLeaf(q))
        {
            sb.Append(", points: [ ");
            SortedSet<Vec2> points = q.points;
            int i = -1;
            int last = points.Count - 1;
            foreach (Vec2 v in points)
            {
                ++i;
                Vec2.ToString(sb, v, places);
                if (i < last) sb.Append(", ");
            }
            sb.Append(' ');
            sb.Append(']');
        }
        else
        {
            sb.Append(", children: [ ");
            List<Quadtree> children = q.children;
            int last = children.Count - 1;
            for (int i = 0; i < last; ++i)
            {
                Quadtree child = children[i];
                Quadtree.ToString(sb, child, places);
                sb.Append(", ");
            }
            Quadtree.ToString(sb, children[last], places);

            sb.Append(' ');
            sb.Append(']');
        }

        sb.Append(' ');
        sb.Append('}');
        return sb;
    }

    /// <summary>
    /// Counts the total capacity of a node, including
    /// the summed capacities of its children.
    /// </summary>
    /// <param name="q">quadtree</param>
    /// <returns>sum</returns>
    public static int TotalCapacity(in Quadtree q)
    {
        if (Quadtree.IsLeaf(q)) { return q.capacity; }
        int sum = 0;
        foreach (Quadtree child in q.children)
        {
            sum += Quadtree.TotalCapacity(child);
        }
        return sum;
    }
}