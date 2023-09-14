using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Organizes a 3D Bezier curve into a list of knots. Provides a function to
/// retrieve a point and tangent on a curve from a step in the range [0.0, 1.0]
/// .
/// </summary>
public class Curve3 : IEnumerable<Knot3>
{
    /// <summary>
    /// A flag for whether or not the curve is a closed loop.
    /// </summary>
    protected bool closedLoop = false;

    /// <summary>
    /// The list of knots contained by the curve.
    /// </summary>
    protected readonly List<Knot3> knots = new();

    /// <summary>
    /// A flag for whether or not the curve is a closed loop.
    /// </summary>
    /// <value>closed loop flag</value>
    public bool ClosedLoop
    {
        get
        {
            return this.closedLoop;
        }

        set
        {
            this.closedLoop = value;
        }
    }

    /// <summary>
    /// Gets the knots of this curve, copied to an array.
    /// </summary>
    /// <value>knots</value>
    public Knot3[] Knots
    {
        get
        {
            Knot3[] result = new Knot3[this.knots.Count];
            this.knots.CopyTo(result);
            return result;
        }
    }

    /// <summary>
    /// Gets the number of knots in the curve.
    /// </summary>
    /// <value>length</value>
    public int Length
    {
        get
        {
            return this.knots.Count;
        }
    }

    /// <summary>
    /// Retrieves a knot from this curve. If the curve is a closed loop, wraps
    /// the index by the number of elements in the list of knots.
    /// </summary>
    /// <value>knot</value>
    public Knot3 this[int i]
    {
        get
        {
            return this.closedLoop ?
                this.knots[Utils.RemFloor(i, this.knots.Count)] :
                this.knots[i];
        }
    }

    /// <summary>
    /// Evaluates the coordinate and normalized tangent given a step in the
    /// range [0.0, 1.0] .
    ///
    /// Returns a named value tuple containing two vectors.
    /// </summary>
    /// <value>tuple</value>
    public (Vec3 coord, Vec3 tangent) this[float i]
    {
        get
        {
            return Curve3.Eval(this, i);
        }
    }

    /// <summary>
    /// The default curve constructor.
    /// </summary>
    protected Curve3() { }

    /// <summary>
    /// Creates a curve from a list of knots and a closed loop flag.
    /// </summary>
    /// <param name="cl">closed loop</param>
    /// <param name="kn">knots</param>
    public Curve3(in bool cl, params Knot3[] kn)
    {
        this.closedLoop = cl;
        this.AppendAll(kn);
    }

    /// <summary>
    /// Creates a curve with a given number of knots.
    /// </summary>
    /// <param name="cl">closed loop</param>
    /// <param name="count">count</param>
    public Curve3(in bool cl, in int count)
    {
        this.closedLoop = cl;
        this.Resize(count);
    }

    /// <summary>
    /// Returns a hash code representing this curve.
    /// </summary>
    /// <returns>hash code</returns>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = Utils.HashBase;
            hash = hash * Utils.HashMul ^ this.closedLoop.GetHashCode();
            hash = hash * Utils.HashMul ^ this.knots.GetHashCode();
            return hash;
        }
    }

    /// <summary>
    /// Returns a string representation of this curve.
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()
    {
        return Curve3.ToString(this);
    }

    /// <summary>
    /// Append a knot to the curve's list of knots.
    /// </summary>
    /// <param name="knot">knot</param>
    /// <returns>this curve</returns>
    public Curve3 Append(in Knot3 knot)
    {
        this.knots.Add(knot);

        return this;
    }

    /// <summary>
    /// Append an collection of 2D knots to the curve's list of knots.
    /// </summary>
    /// <param name="kn">knots</param>
    /// <returns>this curve</returns>
    public Curve3 AppendAll(params Knot2[] kn)
    {
        int len = kn.Length;
        for (int i = 0; i < len; ++i) { this.knots.Add(kn[i]); }

        return this;
    }

    /// <summary>
    /// Append an collection of knots to the curve's list of knots.
    /// </summary>
    /// <param name="kn">knots</param>
    /// <returns>this curve</returns>
    public Curve3 AppendAll(params Knot3[] kn)
    {
        this.knots.AddRange(kn);

        return this;
    }

    /// <summary>
    /// Evaluates whether a knot is contained by this curve.
    /// </summary>
    /// <param name="kn">knot</param>
    /// <returns>evaluation</returns>
    public bool Contains(in Knot3 kn)
    {
        return this.knots.Contains(kn);
    }

    /// <summary>
    /// Flips this curve on the x axis, then reverses the curve.
    /// </summary>
    /// <returns>this curve</returns>
    public Curve3 FlipX()
    {
        this.knots.Reverse();
        foreach (Knot3 kn in this.knots) { kn.FlipX(); kn.Reverse(); }

        return this;
    }

    /// <summary>
    /// Flips this curve on the y axis, then reverses the curve.
    /// </summary>
    /// <returns>this curve</returns>
    public Curve3 FlipY()
    {
        this.knots.Reverse();
        foreach (Knot3 kn in this.knots) { kn.FlipY(); kn.Reverse(); }

        return this;
    }

    /// <summary>
    /// Flips this curve on the z axis, then reverses the curve.
    /// </summary>
    /// <returns>this curve</returns>
    public Curve3 FlipZ()
    {
        this.knots.Reverse();
        foreach (Knot3 kn in this.knots) { kn.FlipZ(); kn.Reverse(); }

        return this;
    }

    /// <summary>
    /// Gets the enumerator for this curve.
    /// </summary>
    /// <returns>enumerator</returns>
    public IEnumerator<Knot3> GetEnumerator()
    {
        return this.knots.GetEnumerator();
    }

    /// <summary>
    /// Gets the enumerator for this curve.
    /// </summary>
    /// <returns>enumerator</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    /// <summary>
    /// Gets the first knot in the curve.
    /// </summary>
    /// <returns>the knot</returns>
    public Knot3 GetFirst()
    {
        return this.knots[0];
    }

    /// <summary>
    /// Gets the last knot in the curve.
    /// </summary>
    /// <returns>the knot</returns>
    public Knot3 GetLast()
    {
        return this.knots[^1];
    }

    /// <summary>
    /// Inserts a knot at a given index. When the curve is a closed loop, the
    /// index wraps around; this means negative indices are accepted.
    /// </summary>
    /// <param name="i">index</param>
    /// <param name="kn">knot</param>
    /// <returns>this curve</returns>
    public Curve3 Insert(in int i, in Knot3 kn)
    {
        int k = this.closedLoop ? Utils.RemFloor(i, this.knots.Count + 1) : i;
        this.knots.Insert(k, kn);

        return this;
    }

    /// <summary>
    /// Prepend a knot to the curve's list of knots.
    /// </summary>
    /// <param name="kn">knot</param>
    /// <returns>this curve</returns>
    public Curve3 Prepend(in Knot3 kn)
    {
        this.knots.Insert(0, kn);

        return this;
    }

    /// <summary>
    /// Prepend an array of knots to the curve's list of knots.
    /// Promotes the knots from 2D to 3D.
    /// </summary>
    /// <param name="kn">array of knots</param>
    /// <returns>this curve</returns>
    public Curve3 PrependAll(params Knot2[] kn)
    {
        int len = kn.Length;
        for (int i = 0, j = 0; i < len; ++i)
        {
            Knot3 knot = (Knot3)kn[i];
            this.knots.Insert(j, knot);
            ++j;
        }

        return this;
    }

    /// <summary>
    /// Prepend an array of knots to the curve's list of knots.
    /// </summary>
    /// <param name="kn">array of knots</param>
    /// <returns>this curve</returns>
    public Curve3 PrependAll(params Knot3[] kn)
    {
        int len = kn.Length;
        for (int i = 0, j = 0; i < len; ++i)
        {
            Knot3 knot = kn[i];
            this.knots.Insert(j, knot);
            ++j;
        }

        return this;
    }

    /// <summary>
    /// Returns and removes a knot at a given index.
    /// </summary>
    /// <param name="i">index</param>
    /// <returns>the knot</returns>
    protected Knot3 RemoveAt(in int i)
    {
        int j = this.closedLoop ? Utils.RemFloor(i, this.knots.Count) : i;
        Knot3 knot = this.knots[j];
        this.knots.RemoveAt(j);
        return knot;
    }

    /// <summary>
    /// Removes and returns the first knot in the curve.
    /// </summary>
    /// <returns>first knot</returns>
    protected Knot3 RemoveFirst()
    {
        return this.RemoveAt(0);
    }

    /// <summary>
    /// Removes and returns the last knot in the curve.
    /// </summary>
    /// <returns>last knot</returns>
    protected Knot3 RemoveLast()
    {
        return this.RemoveAt(this.knots.Count - 1);
    }

    /// <summary>
    /// For internal use. Resizes a curve to the specified length. The length
    /// may be no less than 2. When the new length is greater than the old, new
    /// Knot2s are added.
    ///
    /// This does not check if remaining elements in the list are null.
    /// </summary>
    /// <param name="len">length</param>
    /// <returns>this curve</returns>
    protected Curve3 Resize(in int len)
    {
        int vlen = len < 2 ? 2 : len;
        int oldLen = this.knots.Count;
        int diff = vlen - oldLen;

        if (diff < 0)
        {
            int last = oldLen - 1;
            for (int i = 0; i < -diff; ++i)
            {
                this.knots.RemoveAt(last - i);
            }
        }
        else if (diff > 0)
        {
            this.knots.Capacity = vlen;
            for (int i = 0; i < diff; ++i)
            {
                this.knots.Add(new Knot3());
            }
        }

        return this;
    }

    /// <summary>
    /// Reverses the curve. This is done by reversing the list of knots and
    /// swapping the fore- and rear-handle of each knot.
    /// </summary>
    /// <returns>this curve</returns>
    public Curve3 Reverse()
    {
        this.knots.Reverse();
        foreach (Knot3 kn in this.knots) { kn.Reverse(); }
        return this;
    }

    /// <summary>
    /// Rotates all knots in a curve by a quaternion.
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <returns>this curve</returns>
    public Curve3 Rotate(in Quat q)
    {
        foreach (Knot3 kn in this.knots) { kn.Rotate(q); }
        return this;
    }

    /// <summary>
    /// Rotates all knots in the curve by an angle in radians around an
    /// arbitrary axis.
    /// </summary>
    /// <param name="radians">angle</param>
    /// <param name="axis">axis</param>
    /// <returns>this curve</returns>
    public Curve3 Rotate(in float radians, in Vec3 axis)
    {
        float cosa = MathF.Cos(radians);
        float sina = MathF.Sin(radians);
        foreach (Knot3 kn in this.knots) { kn.Rotate(cosa, sina, axis); }
        return this;
    }

    /// <summary>
    /// Rotates all knots in the curve by an angle in radians around the x axis.
    /// </summary>
    /// <param name="radians">angle</param>
    /// <returns>this curve</returns>
    public Curve3 RotateX(in float radians)
    {
        float cosa = MathF.Cos(radians);
        float sina = MathF.Sin(radians);
        foreach (Knot3 kn in this.knots) { kn.RotateX(cosa, sina); }
        return this;
    }

    /// <summary>
    /// Rotates all knots in the curve by an angle in radians around the y axis.
    /// </summary>
    /// <param name="radians">angle</param>
    /// <returns>this curve</returns>
    public Curve3 RotateY(in float radians)
    {
        float cosa = MathF.Cos(radians);
        float sina = MathF.Sin(radians);
        foreach (Knot3 kn in this.knots) { kn.RotateY(cosa, sina); }
        return this;
    }

    /// <summary>
    /// Rotates all knots in the curve by an angle in radians around the z axis.
    /// </summary>
    /// <param name="radians">angle</param>
    /// <returns>this curve</returns>
    public Curve3 RotateZ(in float radians)
    {
        float cosa = MathF.Cos(radians);
        float sina = MathF.Sin(radians);
        foreach (Knot3 kn in this.knots) { kn.RotateZ(cosa, sina); }
        return this;
    }

    /// <summary>
    /// Scales all knots in the curve by a scalar.
    /// </summary>
    /// <param name="scale">uniform scale</param>
    /// <returns>this curve</returns>
    public Curve3 Scale(in float scale)
    {
        if (scale != 0.0f)
        {
            foreach (Knot3 kn in this.knots) { kn.Scale(scale); }
        }
        return this;
    }

    /// <summary>
    /// Scales all knots in the curve by a vector.
    /// </summary>
    /// <param name="scale">nonuniform scale</param>
    /// <returns>this curve</returns>
    public Curve3 Scale(in Vec3 scale)
    {
        if (Vec3.All(scale))
        {
            foreach (Knot3 kn in this.knots) { kn.Scale(scale); }
        }
        return this;
    }

    /// <summary>
    /// Toggles whether or not the curve is a closed loop.
    /// </summary>
    /// <returns>this curve</returns>
    public Curve3 ToggleLoop()
    {
        this.closedLoop = !this.closedLoop;
        return this;
    }

    /// <summary>
    /// Transforms all knots in the curve by a matrix.
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>this curve</returns>
    public Curve3 Transform(in Mat4 m)
    {
        foreach (Knot3 kn in this.knots) { kn.Transform(m); }
        return this;
    }

    /// <summary>
    /// Transforms all coordinates in the curve permanently by a transform.
    ///
    /// Not to be confused with the temporary transformations applied by a curve
    /// entity's transform to the meshes contained within the entity.
    ///
    /// Useful when consolidating multiple curve entities into one curve entity.
    /// </summary>
    /// <param name="tr">transform</param>
    /// <returns>this curve</returns>
    public Curve3 Transform(in Transform3 tr)
    {
        foreach (Knot3 kn in this.knots) { kn.Transform(tr); }
        return this;
    }

    /// <summary>
    /// Translates all knots in the curve by a vector.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>this curve</returns>
    public Curve3 Translate(in Vec3 v)
    {
        foreach (Knot3 kn in this.knots) { kn.Translate(v); }
        return this;
    }

    /// <summary>
    /// Converts a 2D curve to a 3D curve.
    /// </summary>
    /// <param name="c">2D curve</param>
    public static implicit operator Curve3(in Curve2 c)
    {
        Curve3 result = new()
        {
            closedLoop = c.ClosedLoop
        };
        foreach (Knot2 kn in c) { result.Append(kn); }
        return result;
    }

    /// <summary>
    /// Evaluates the coordinate and normalized tangent given a step in the
    /// range [0.0, 1.0] .
    ///
    /// Returns a named value tuple containing two vectors.
    /// </summary>
    /// <param name="c">curve</param>
    /// <param name="step">step</param>
    /// <returns>tuple</returns>
    public static (Vec3 coord, Vec3 tangent) Eval(in Curve3 c, in float step)
    {
        List<Knot3> knots = c.knots;
        int knotLength = knots.Count;

        float tScaled;
        int i;
        Knot3 a;
        Knot3 b;

        if (c.closedLoop)
        {
            tScaled = Utils.RemFloor(step, 1.0f) * knotLength;
            i = (int)tScaled;
            a = knots[Utils.RemFloor(i, knotLength)];
            b = knots[Utils.RemFloor(i + 1, knotLength)];
        }
        else
        {
            if (knotLength == 1 || step <= 0.0f)
            {
                return Curve3.EvalFirst(c);
            }

            if (step >= 1.0f)
            {
                return Curve3.EvalLast(c);
            }

            tScaled = step * (knotLength - 1);
            i = (int)tScaled;
            a = knots[i];
            b = knots[i + 1];
        }

        float t = tScaled - i;
        return (
            coord: Knot3.BezierPoint(a, b, t),
            tangent: Knot3.BezierTanUnit(a, b, t));
    }

    /// <summary>
    /// Evaluates the coordinate and normalized tangent of the first knot in the
    /// curve.
    ///
    /// Returns a named value tuple containing two vectors.
    /// </summary>
    /// <param name="c">curve</param>
    /// <returns>tuple</returns>
    public static (Vec3 coord, Vec3 tangent) EvalFirst(in Curve3 c)
    {
        Knot3 kn = c.knots[0];
        return (
            coord: kn.Coord,
            tangent: Vec3.Normalize(kn.ForeHandle - kn.Coord));
    }

    /// <summary>
    /// Evaluates the coordinate and normalized tangent of the last knot in the
    /// curve.
    ///
    /// Returns a named value tuple containing two vectors.
    /// </summary>
    /// <param name="c">curve</param>
    /// <returns>tuple</returns>
    public static (Vec3 coord, Vec3 tangent) EvalLast(in Curve3 c)
    {
        List<Knot3> kns = c.knots;
        Knot3 kn = kns[^1];
        return (
            coord: kn.Coord,
            tangent: Vec3.Normalize(kn.Coord - kn.RearHandle));
    }

    /// <summary>
    /// Evaluates a range of points and tangents on a curve
    /// at a resolution, or count.
    /// </summary>
    /// <param name="c">curve</param>    
    /// <param name="count">count</param>
    /// <returns>range</returns>
    public static (Vec3 coord, Vec3 tangent)[] EvalRange(in Curve3 c, in int count)
    {
        return Curve3.EvalRange(c, count, 0.0f,
            c.closedLoop ? 1.0f - 1.0f / Utils.Max(3, count) : 1.0f);
    }

    /// <summary>
    /// Evaluates a range of points and tangents on a curve
    /// for a given origin and destination factor at a
    /// resolution, or count.
    ///
    /// For closed loops, an origin and destination of
    /// 0.0 and 1.0 would yield a duplicate point, and so
    /// should be calculated accordingly. I.e., 0.0 to
    /// 1.0 - 1.0 / count would avoid the duplicate.
    /// </summary>
    /// <param name="c">curve</param>    
    /// <param name="count">count</param>
    /// <param name="origin">origin</param>
    /// <param name="dest">destination</param>
    /// <returns>range</returns>
    public static (Vec3 coord, Vec3 tangent)[] EvalRange(
        in Curve3 c,
        in int count,
        in float origin,
        in float dest)
    {
        int vCount = count < 3 ? 3 : count;
        float vOrigin = origin;
        float vDest = dest;
        if (c.closedLoop)
        {
            vOrigin = Utils.Clamp(vOrigin, 0.0f, 1.0f);
            vDest = Utils.Clamp(vDest, 0.0f, 1.0f);
        }

        (Vec3 coord, Vec3 tangent)[] result = new (Vec3 coord, Vec3 tangent)[vCount];

        float toPercent = 1.0f / (vCount - 1.0f);
        for (int i = 0; i < vCount; ++i)
        {
            float fac = Utils.Mix(vOrigin, vDest, i * toPercent);
            result[i] = Curve3.Eval(c, fac);
        }

        return result;
    }

    /// <summary>
    /// Sets a curve to a series of points.
    /// </summary>
    /// <param name="target">target curve</param>
    /// <param name="points">points array</param>
    /// <param name="closedLoop">closed loop flag</param>
    /// <returns>curve</returns>
    public static Curve3 FromLinear(
        in Curve3 target,
        in Vec3[] points,
        in bool closedLoop = false)
    {
        int len = points.Length;
        if (len < 2) { return target; }

        target.Resize(len);
        target.closedLoop = closedLoop;

        List<Knot3> knots = target.knots;
        for (int i = 0; i < len; ++i)
        {
            Vec3 v = points[i];
            Knot3 kn = knots[i];
            kn.Coord = v;
            kn.ForeHandle = v;
            kn.RearHandle = v;
        }

        return Curve3.StraightHandles(target);
    }

    ///<summary>
    ///Adjust knot handles to create a smooth, continuous curve.
    ///</summary>
    ///<param name="target">target curve</param>
    ///<returns>smoothed curve</returns>
    public static Curve3 SmoothHandles(in Curve3 target)
    {
        List<Knot3> knots = target.knots;
        int len = knots.Count;
        if (len < 3) { return target; }

        Vec3 carry = Vec3.Zero;
        Knot3 first = knots[0];

        if (target.closedLoop)
        {
            Knot3 prev = knots[len - 1];
            Knot3 curr = first;
            for (int i = 1; i < len; ++i)
            {
                Knot3 next = knots[i];
                carry = Knot3.SmoothHandles(prev, curr, next, carry);
                prev = curr;
                curr = next;
            }
            Knot3.SmoothHandles(prev, curr, first, carry);
        }
        else
        {
            Knot3 prev = first;
            Knot3 curr = knots[1];

            carry = Knot3.SmoothHandlesFirst(prev, curr, carry);
            curr.MirrorHandlesForward();

            for (int i = 2; i < len; ++i)
            {
                Knot3 next = knots[i];
                carry = Knot3.SmoothHandles(prev, curr, next, carry);
                prev = curr;
                curr = next;
            }

            Knot3.SmoothHandlesLast(prev, curr, carry);
            curr.MirrorHandlesBackward();
        }

        return target;
    }

    /// <summary>
    /// Straightens the fore handles and rear handles of
    /// a curve's knots so they are collinear with its
    /// coordinates.
    /// </summary>
    /// <param name="target">target curve</param>
    /// <returns>curve</returns>
    public static Curve3 StraightHandles(in Curve3 target)
    {
        List<Knot3> knots = target.knots;
        int len = knots.Count;
        if (len < 2) { return target; }

        for (int i = 1; i < len; ++i)
        {
            Knot3 prev = knots[i - 1];
            Knot3 next = knots[i];
            prev.ForeHandle = Vec3.Mix(
                prev.Coord, next.Coord, Utils.OneThird);
            next.RearHandle = Vec3.Mix(
                next.Coord, prev.Coord, Utils.OneThird);
        }

        Knot3 first = knots[0];
        Knot3 last = knots[len - 1];
        if (target.closedLoop)
        {
            first.RearHandle = Vec3.Mix(
                first.Coord, last.Coord, Utils.OneThird);
            last.ForeHandle = Vec3.Mix(
                last.Coord, first.Coord, Utils.OneThird);
        }
        else
        {
            first.MirrorHandlesForward();
            last.MirrorHandlesBackward();
        }

        return target;
    }

    /// <summary>
    /// Returns the string representation of a curve.
    /// </summary>
    /// <param name="c">curve</param>
    /// <param name="places">number of places</param>
    /// <returns>string</returns>
    public static string ToString(in Curve3 c, in int places = 4)
    {
        return Curve3.ToString(new StringBuilder(1024), c, places).ToString();
    }

    /// <summary>
    /// Appends a string representation of a curve to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="c">curve</param>
    /// <param name="places">number of places</param>
    /// <returns>string</returns>
    public static StringBuilder ToString(
        in StringBuilder sb,
        in Curve3 c,
        in int places = 4)
    {
        List<Knot3> knots = c.knots;
        int len = knots.Count;
        int last = len - 1;

        sb.Append("{\"closedLoop\":");
        sb.Append(c.closedLoop ? "true" : "false");
        sb.Append(",\"knots\":[");
        for (int i = 0; i < last; ++i)
        {
            Knot3.ToString(sb, knots[i], places);
            sb.Append(',');
        }
        Knot3.ToString(sb, knots[last], places);
        sb.Append("]}");
        return sb;
    }
}