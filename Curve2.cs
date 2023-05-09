using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Organizes a 2D Bezier curve into a list of knots. Provides a function to
/// retrieve a point and tangent on a curve from a step in the range [0.0, 1.0].
/// </summary>
public class Curve2 : IEnumerable<Knot2>
{
    /// <summary>
    /// A flag for whether or not the curve is a closed loop.
    /// </summary>
    protected bool closedLoop = false;

    /// <summary>
    ///  The list of knots contained by the curve.
    /// </summary>
    protected readonly List<Knot2> knots = new();

    /// <summary>
    /// A flag for whether or not the curve is a closed loop.
    /// </summary>
    /// <value>the closed loop flag</value>
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
    /// <value>the knots</value>
    public Knot2[] Knots
    {
        get
        {
            Knot2[] result = new Knot2[this.knots.Count];
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
    public Knot2 this[int i]
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
    public (Vec2 coord, Vec2 tangent) this[float i]
    {
        get
        {
            return Curve2.Eval(this, i);
        }
    }

    /// <summary>
    /// The default curve constructor.
    /// </summary>
    protected Curve2()
    {
        // TODO: Implement rounded rect.
    }

    /// <summary>
    /// Creates a curve from a list of knots and a closed loop flag.
    /// </summary>
    /// <param name="cl">closed loop</param>
    /// <param name="kn">knots</param>
    public Curve2(in bool cl, params Knot2[] kn)
    {
        this.closedLoop = cl;
        this.AppendAll(kn);
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
        return Curve2.ToString(this);
    }

    /// <summary>
    /// Append a knot to the curve's list of knots.
    /// </summary>
    /// <param name="kn">knot</param>
    /// <returns>this curve</returns>
    public Curve2 Append(in Knot2 kn)
    {
        this.knots.Add(kn);

        return this;
    }

    /// <summary>
    /// Append an collection of knots to the curve's list of knots.
    /// </summary>
    /// <param name="kn">knots</param>
    /// <returns>this curve</returns>
    public Curve2 AppendAll(params Knot2[] kn)
    {
        this.knots.AddRange(kn);

        return this;
    }

    /// <summary>
    /// Evaluates whether a knot is contained by this curve.
    /// </summary>
    /// <param name="kn">knot</param>
    /// <returns>evaluation</returns>
    public bool Contains(in Knot2 kn)
    {
        return this.knots.Contains(kn);
    }

    /// <summary>
    /// Flips this curve on the x axis, then reverses the curve.
    /// </summary>
    /// <returns>this curve</returns>
    public Curve2 FlipX()
    {
        this.knots.Reverse();
        foreach (Knot2 kn in this.knots) { kn.FlipX(); kn.Reverse(); }

        return this;
    }

    /// <summary>
    /// Flips this curve on the y axis, then reverses the curve.
    /// </summary>
    /// <returns>this curve</returns>
    public Curve2 FlipY()
    {
        this.knots.Reverse();
        foreach (Knot2 kn in this.knots) { kn.FlipY(); kn.Reverse(); }

        return this;
    }

    /// <summary>
    /// Gets the enumerator for this curve.
    /// </summary>
    /// <returns>enumerator</returns>
    public IEnumerator<Knot2> GetEnumerator()
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
    public Knot2 GetFirst()
    {
        return this.knots[0];
    }

    /// <summary>
    /// Gets the last knot in the curve.
    /// </summary>
    /// <returns>the knot</returns>
    public Knot2 GetLast()
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
    public Curve2 Insert(in int i, in Knot2 kn)
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
    public Curve2 Prepend(in Knot2 kn)
    {
        this.knots.Insert(0, kn);

        return this;
    }

    /// <summary>
    /// Prepend an array of knots to the curve's list of knots.
    /// </summary>
    /// <param name="kn">array of knots</param>
    /// <returns>this curve</returns>
    public Curve2 PrependAll(params Knot2[] kn)
    {
        int len = kn.Length;
        for (int i = 0, j = 0; i < len; ++i)
        {
            Knot2 knot = kn[i];
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
    protected Knot2 RemoveAt(in int i)
    {
        int j = this.closedLoop ? Utils.RemFloor(i, this.knots.Count) : i;
        Knot2 knot = this.knots[j];
        this.knots.RemoveAt(j);
        return knot;
    }

    /// <summary>
    /// Removes and returns the first knot in the curve.
    /// </summary>
    /// <returns>first knot</returns>
    protected Knot2 RemoveFirst()
    {
        return this.RemoveAt(0);
    }

    /// <summary>
    /// Removes and returns the last knot in the curve.
    /// </summary>
    /// <returns>last knot</returns>
    protected Knot2 RemoveLast()
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
    protected Curve2 Resize(in int len)
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
                this.knots.Add(new Knot2());
            }
        }

        return this;
    }

    /// <summary>
    /// Reverses the curve. This is done by reversing the list of knots and
    /// swapping the fore- and rear-handle of each knot.
    /// </summary>
    /// <returns>this curve</returns>
    public Curve2 Reverse()
    {
        this.knots.Reverse();
        foreach (Knot2 kn in this.knots) { kn.Reverse(); }
        return this;
    }

    /// <summary>
    /// Rotates all knots in the curve by an angle in radians around the z axis.
    /// </summary>
    /// <param name="radians">angle</param>
    /// <returns>this curve</returns>
    public Curve2 RotateZ(in float radians)
    {
        float cosa = MathF.Cos(radians);
        float sina = MathF.Sin(radians);
        foreach (Knot2 kn in this.knots) { kn.RotateZ(cosa, sina); }
        return this;
    }

    /// <summary>
    /// Scales all knots in the curve by a scalar.
    /// </summary>
    /// <param name="scale">uniform scale</param>
    /// <returns>this curve</returns>
    public Curve2 Scale(in float scale)
    {
        if (scale != 0.0f)
        {
            foreach (Knot2 kn in this.knots) { kn.Scale(scale); }
        }
        return this;
    }

    /// <summary>
    /// Scales all knots in the curve by a vector.
    /// </summary>
    /// <param name="scale">nonuniform scale</param>
    /// <returns>this curve</returns>
    public Curve2 Scale(in Vec2 scale)
    {
        if (Vec2.All(scale))
        {
            foreach (Knot2 kn in this.knots) { kn.Scale(scale); }
        }
        return this;
    }

    /// <summary>
    /// Toggles whether or not the curve is a closed loop.
    /// </summary>
    /// <returns>this curve</returns>
    public Curve2 ToggleLoop()
    {
        this.closedLoop = !this.closedLoop;
        return this;
    }

    /// <summary>
    /// Transforms all knots in the curve by a matrix.
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>this curve</returns>
    public Curve2 Transform(in Mat3 m)
    {
        foreach (Knot2 kn in this.knots) { kn.Transform(m); }
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
    public Curve2 Transform(in Transform2 tr)
    {
        foreach (Knot2 kn in this.knots) { kn.Transform(tr); }
        return this;
    }

    /// <summary>
    /// Translates all knots in the curve by a vector.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>this curve</returns>
    public Curve2 Translate(in Vec2 v)
    {
        foreach (Knot2 kn in this.knots) { kn.Translate(v); }
        return this;
    }

    /// <summary>
    /// Sets a curve to an ease animation curve.
    /// </summary>
    /// <param name="target">output curve</param>
    /// <returns>ease animation</returns>
    public static Curve2 AnimEase(in Curve2 target)
    {
        target.Resize(2);
        target.closedLoop = false;
        target.knots[0].Set(0.0f, 0.0f,
            0.25f, 0.1f,
            -0.25f, -0.1f);
        target.knots[1].Set(1.0f, 1.0f,
            0.25f, 1.0f,
            1.75f, 1.0f);
        return target;
    }

    /// <summary>
    /// Sets a curve to an ease-in animation curve.
    /// </summary>
    /// <param name="target">output curve</param>
    /// <returns>ease-in animation</returns>
    public static Curve2 AnimEaseIn(in Curve2 target)
    {
        target.Resize(2);
        target.closedLoop = false;
        target.knots[0].Set(0.0f, 0.0f,
            0.42f, 0.0f,
            -0.42f, 0.0f);
        target.knots[1].Set(1.0f, 1.0f,
            1.0f, 1.0f,
            1.0f, 1.0f);
        return target;
    }

    /// <summary>
    /// Sets a curve to an ease-out animation curve.
    /// </summary>
    /// <param name="target">output curve</param>
    /// <returns>ease-out animation</returns>
    public static Curve2 AnimEaseOut(in Curve2 target)
    {
        target.Resize(2);
        target.closedLoop = false;
        target.knots[0].Set(0.0f, 0.0f,
            0.0f, 0.0f,
            0.0f, 0.0f);
        target.knots[1].Set(1.0f, 1.0f,
            0.58f, 1.0f,
            1.42f, 1.0f);
        return target;
    }

    /// <summary>
    /// Sets a curve to an ease-in-out animation curve.
    /// </summary>
    /// <param name="target">output curve</param>
    /// <returns>ease-in-out animation</returns>
    public static Curve2 AnimEaseInOut(in Curve2 target)
    {
        target.Resize(2);
        target.closedLoop = false;
        target.knots[0].Set(0.0f, 0.0f,
            0.42f, 0.0f,
            -0.42f, 0.0f);
        target.knots[1].Set(1.0f, 1.0f,
            0.58f, 1.0f,
            1.42f, 1.0f);
        return target;
    }

    /// <summary>
    /// Sets a curve to a linear animation curve.
    /// </summary>
    /// <param name="target">output curve</param>
    /// <returns>linear animation</returns>
    public static Curve2 AnimLinear(in Curve2 target)
    {
        target.Resize(2);
        target.closedLoop = false;
        target.knots[0].Set(0.0f, 0.0f,
            Utils.OneThird, Utils.OneThird,
            -Utils.OneThird, -Utils.OneThird);
        target.knots[1].Set(1.0f, 1.0f,
            Utils.TwoThirds, Utils.TwoThirds,
            Utils.FourThirds, Utils.FourThirds);
        return target;
    }

    /// <summary>
    /// Sets a curve to approximate a circle.
    /// </summary>
    /// <param name="target">output curve</param>
    /// <param name="sectors">sectors</param>
    /// <param name="offsetAngle">offset angle</param>
    /// <returns>circle</returns>
    public static Curve2 Circle(
        in Curve2 target,
        in int sectors = 4,
        in float offsetAngle = Utils.HalfPi)
    {
        int vKnCt = sectors < 3 ? 3 : sectors;

        float cx = 0.0f;
        float cy = 0.0f;
        float r = 0.5f;
        float toTheta = Utils.Tau / vKnCt;

        float invKnCt = 1.0f / vKnCt;
        float hndlTan = 0.25f * invKnCt;
        float magHandle = MathF.Tan(hndlTan * Utils.Tau)
           * Utils.FourThirds * r;

        target.Resize(vKnCt);
        target.closedLoop = true;
        List<Knot2> knots = target.knots;

        for (int i = 0; i < vKnCt; ++i)
        {
            float angle = offsetAngle + i * toTheta;
            float cosAngle = MathF.Cos(angle);
            float sinAngle = MathF.Sin(angle);
            float hmCosa = cosAngle * magHandle;
            float hmSina = sinAngle * magHandle;
            float cox = cx + r * cosAngle;
            float coy = cy + r * sinAngle;

            knots[i].Set(
                cox, coy,
                cox - hmSina, coy + hmCosa,
                cox + hmSina, coy - hmCosa);
        }

        return target;
    }

    /// <summary>
    /// Sets a curve to approximate an ellipse.
    /// The horizontal radius is assumed to be 0.5,
    /// while the vertical radius is the horizontal
    /// divided by the aspect ratio.
    /// The curve has four knots.
    /// </summary>
    /// <param name="target">output curve</param>
    /// <param name="aspect">aspect ratio</param>
    /// <returns>the ellipse</returns>
    public static Curve2 Ellipse(
        in Curve2 target,
        in float aspect = 1.0f)
    {
        float vAsp = aspect != 0.0f ? aspect : 1.0f;

        float cx = 0.0f;
        float cy = 0.0f;
        float rx = 0.5f;
        float ry = rx / vAsp;

        float right = cx + rx;
        float top = cy + ry;
        float left = cx - rx;
        float bottom = cy - ry;

        float horizHandle = rx * Utils.Kappa;
        float vertHandle = ry * Utils.Kappa;

        float xHandlePos = cx + horizHandle;
        float xHandleNeg = cx - horizHandle;
        float yHandlePos = cy + vertHandle;
        float yHandleNeg = cy - vertHandle;

        target.Resize(4);
        target.closedLoop = true;
        List<Knot2> knots = target.knots;

        knots[0].Set(
            right, cy,
            right, yHandlePos,
            right, yHandleNeg);
        knots[1].Set(
            cx, top,
            xHandleNeg, top,
            xHandlePos, top);
        knots[2].Set(
            left, cy,
            left, yHandleNeg,
            left, yHandlePos);
        knots[3].Set(
            cx, bottom,
            xHandlePos, bottom,
            xHandleNeg, bottom);

        return target;
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
    public static (Vec2 coord, Vec2 tangent) Eval(in Curve2 c, in float step)
    {
        List<Knot2> knots = c.knots;
        int knotLength = knots.Count;

        float tScaled;
        int i;
        Knot2 a;
        Knot2 b;

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
                return Curve2.EvalFirst(c);
            }

            if (step >= 1.0f)
            {
                return Curve2.EvalLast(c);
            }

            tScaled = step * (knotLength - 1);
            i = (int)tScaled;
            a = knots[i];
            b = knots[i + 1];
        }

        float t = tScaled - i;
        return (
            coord: Knot2.BezierPoint(a, b, t),
            tangent: Knot2.BezierTanUnit(a, b, t));
    }

    /// <summary>
    /// Evaluates the coordinate and normalized tangent of the first knot in the
    /// curve.
    ///
    /// Returns a named value tuple containing two vectors.
    /// </summary>
    /// <param name="c">curve</param>
    /// <returns>tuple</returns>
    public static (Vec2 coord, Vec2 tangent) EvalFirst(in Curve2 c)
    {
        Knot2 kn = c.knots[0];
        return (
            coord: kn.Coord,
            tangent: Vec2.Normalize(kn.ForeHandle - kn.Coord));
    }

    /// <summary>
    /// Evaluates the coordinate and normalized tangent of the last knot in the
    /// curve.
    ///
    /// Returns a named value tuple containing two vectors.
    /// </summary>
    /// <param name="c">curve</param>
    /// <returns>tuple</returns>
    public static (Vec2 coord, Vec2 tangent) EvalLast(in Curve2 c)
    {
        List<Knot2> kns = c.knots;
        Knot2 kn = kns[^1];
        return (
            coord: kn.Coord,
            tangent: Vec2.Normalize(kn.Coord - kn.RearHandle));
    }

    /// <summary>
    /// Evaluates a range of points and tangents on a curve
    /// at a resolution, or count.
    /// </summary>
    /// <param name="c">curve</param>    
    /// <param name="count">count</param>
    /// <returns>range</returns>
    public static (Vec2 coord, Vec2 tangent)[] EvalRange(in Curve2 c, in int count)
    {
        return Curve2.EvalRange(c, count, 0.0f,
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
    public static (Vec2 coord, Vec2 tangent)[] EvalRange(
        in Curve2 c,
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

        (Vec2 coord, Vec2 tangent)[] result = new (Vec2 coord, Vec2 tangent)[vCount];

        float toPercent = 1.0f / (vCount - 1.0f);
        for (int i = 0; i < vCount; ++i)
        {
            float fac = Utils.Mix(vOrigin, vDest, i * toPercent);
            result[i] = Curve2.Eval(c, fac);
        }

        return result;
    }

    /// <summary>
    /// Sets a curve to a series of points.
    /// </summary>
    /// <param name="target">target curve</param>
    /// <param name="points">points array</param>
    /// <param name="closedLoop">closed loop flag</param>
    /// <returns>the curve</returns>
    public static Curve2 FromLinear(
        in Curve2 target,
        in Vec2[] points,
        in bool closedLoop = false)
    {
        int len = points.Length;
        if (len < 2) { return target; }

        target.Resize(len);
        target.closedLoop = closedLoop;

        List<Knot2> knots = target.knots;
        for (int i = 0; i < len; ++i)
        {
            Vec2 v = points[i];
            Knot2 kn = knots[i];
            kn.Coord = v;
            kn.ForeHandle = v;
            kn.RearHandle = v;
        }

        return Curve2.StraightHandles(target);
    }

    /// <summary>
    /// Sets a curve to approximate Bernoulli's lemniscate, which
    /// resembles an infinity loop (with equally proportioned lobes).
    /// </summary>
    /// <param name="target">output curve</param>
    /// <returns>lemniscate</returns>
    public static Curve2 Infinity(in Curve2 target)
    {
        target.Resize(6);
        target.closedLoop = true;
        List<Knot2> knots = target.knots;

        knots[0].Set(0.5f, 0.0f, 0.5f, 0.1309615f, 0.5f, -0.1309615f);
        knots[1].Set(0.235709f, 0.166627f, 0.0505335f, 0.114256f, 0.361728f, 0.2022675f);
        knots[2].Set(-0.235709f, -0.166627f, -0.361728f, -0.2022675f, -0.0505335f, -0.114256f);
        knots[3].Set(-0.5f, 0.0f, -0.5f, 0.1309615f, -0.5f, -0.1309615f);
        knots[4].Set(-0.235709f, 0.166627f, -0.0505335f, 0.114256f, -0.361728f, 0.2022675f);
        knots[5].Set(0.235709f, -0.166627f, 0.361728f, -0.2022675f, 0.0505335f, -0.114256f);

        return target;
    }

    /// <summary>
    /// Sets a curve to a regular convex polygon.
    /// </summary>
    /// <param name="target">output curve</param>
    /// <param name="sectors">sectors</param>
    /// <param name="offsetAngle">offset angle</param>
    /// <returns>the polygon</returns>
    public static Curve2 Polygon(
        in Curve2 target,
        in int sectors = 3,
        in float offsetAngle = Utils.HalfPi)
    {
        int vKnCt = sectors < 3 ? 3 : sectors;

        float cx = 0.0f;
        float cy = 0.0f;
        float r = 0.5f;
        float toTheta = Utils.Tau / vKnCt;

        target.Resize(vKnCt);
        target.closedLoop = true;
        List<Knot2> knots = target.knots;

        Knot2 first = knots[0];
        first.Coord = new Vec2(
            cx + r * MathF.Cos(offsetAngle),
            cy + r * MathF.Sin(offsetAngle));

        Knot2 prev = first;
        for (int i = 1; i < vKnCt; ++i)
        {
            Vec2 coPrev = prev.Coord;
            float angle = offsetAngle + i * toTheta;
            Vec2 coCurr = new(
                cx + r * MathF.Cos(angle),
                cy + r * MathF.Sin(angle));

            Knot2 curr = knots[i];
            curr.Coord = coCurr;
            curr.RearHandle = Vec2.Mix(
                coCurr,
                coPrev,
                Utils.OneThird);
            prev.ForeHandle = Vec2.Mix(
                coPrev,
                coCurr,
                Utils.OneThird);
            prev = curr;
        }

        first.RearHandle = Vec2.Mix(
            first.Coord,
            prev.Coord,
            Utils.OneThird);
        prev.ForeHandle = Vec2.Mix(
            prev.Coord,
            first.Coord,
            Utils.OneThird);

        return target;
    }

    ///<summary>
    ///Straightens the fore handles and rear handles of
    ///a curve's knots so they are collinear with its
    ///coordinates.
    ///</summary>
    ///<param name="target">target curve</param>
    ///<returns>the curve</returns>
    public static Curve2 StraightHandles(in Curve2 target)
    {
        List<Knot2> knots = target.knots;
        int len = knots.Count;
        if (len < 2) { return target; }

        for (int i = 1; i < len; ++i)
        {
            Knot2 prev = knots[i - 1];
            Knot2 next = knots[i];
            prev.ForeHandle = Vec2.Mix(prev.Coord, next.Coord, Utils.OneThird);
            next.RearHandle = Vec2.Mix(next.Coord, prev.Coord, Utils.OneThird);
        }

        Knot2 first = knots[0];
        Knot2 last = knots[len - 1];
        if (target.closedLoop)
        {
            first.RearHandle = Vec2.Mix(first.Coord, last.Coord, Utils.OneThird);
            last.ForeHandle = Vec2.Mix(last.Coord, first.Coord, Utils.OneThird);
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
    public static string ToString(in Curve2 c, in int places = 4)
    {
        return Curve2.ToString(new StringBuilder(1024), c, places).ToString();
    }

    /// <summary>
    /// Appends a string representation of a curve to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="c">curve</param>
    /// <param name="places">number of places</param>
    /// <returns>string</returns>
    public static StringBuilder ToString(in StringBuilder sb, in Curve2 c, in int places = 4)
    {
        List<Knot2> knots = c.knots;
        int len = knots.Count;
        int last = len - 1;

        sb.Append("{\"closedLoop\":");
        sb.Append(c.closedLoop ? "true" : "false");
        sb.Append(",\"knots\":[");
        for (int i = 0; i < last; ++i)
        {
            Knot2.ToString(sb, knots[i], places);
            sb.Append(',');
        }
        Knot2.ToString(sb, knots[last], places);
        sb.Append("]}");
        return sb;
    }
}