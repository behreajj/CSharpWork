using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Organizes a 2D Bezier curve into a list of knots. Provides a function to
/// retrieve a point and tangent on a curve from a step in the range [0.0, 1.0]
/// .
/// </summary>
public class Curve2 : IEnumerable
{
    /// <summary>
    /// A flag for whether or not the curve is a closed loop.
    /// </summary>
    protected bool closedLoop = false;

    /// <summary>
    ///  The list of knots contained by the curve.
    /// </summary>
    protected List<Knot2> knots = new List<Knot2> ( );

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
    public Knot2[ ] Knots
    {
        get
        {
            Knot2[ ] result = new Knot2[this.knots.Count];
            this.knots.CopyTo (result);
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

    public Knot2 this [int i]
    {
        get
        {
            return this.closedLoop ?
                this.knots[Utils.Mod (i, this.knots.Count)] :
                this.knots[i];
        }
    }

    public (Vec2 coord, Vec2 tangent) this [float i]
    {
        get
        {
            return Curve2.Eval (this, i);
        }
    }

    public Curve2 ( )
    {
        this.Reset ( );
    }

    public Curve2 (bool cl, params Knot2[ ] kn)
    {
        this.closedLoop = cl;
        this.AppendAll (kn);
    }

    public override int GetHashCode ( )
    {
        unchecked
        {
            int hash = Utils.HashBase;
            hash = hash * Utils.HashMul ^ this.closedLoop.GetHashCode ( );
            hash = hash * Utils.HashMul ^ this.knots.GetHashCode ( );
            return hash;
        }
    }

    public override string ToString ( )
    {
        return this.ToString (4);
    }

    /// <summary>
    /// Append a knot to the curve's list of knots.
    /// </summary>
    /// <param name="knot">knot</param>
    /// <returns>this curve</returns>
    public Curve2 Append (Knot2 knot)
    {
        if (knot != null) this.knots.Add (knot);
        return this;
    }

    /// <summary>
    /// Append an collection of knots to the curve's list of knots.
    /// </summary>
    /// <param name="kn">knots</param>
    /// <returns>this curve</returns>
    public Curve2 AppendAll (params Knot2[ ] kn)
    {
        int len = kn.Length;
        for (int i = 0; i < len; ++i)
        {
            Knot2 knot = kn[i];
            if (knot != null) this.knots.Add (knot);
        }
        return this;
    }

    /// <summary>
    /// Evaluates whether a knot is contained by this curve.
    /// </summary>
    /// <param name="knot">knot</param>
    /// <returns>the evaluation</returns>
    public bool Contains (in Knot2 knot)
    {
        return this.knots.Contains (knot);
    }

    /// <summary>
    /// Gets the enumerator for this curve.
    /// </summary>
    /// <returns>the enumerator</returns>
    public IEnumerator GetEnumerator ( )
    {
        return this.knots.GetEnumerator ( );
    }

    /// <summary>
    /// Gets the first knot in the curve.
    /// </summary>
    /// <returns>the knot</returns>
    public Knot2 GetFirst ( )
    {
        return this.knots[0];
    }

    /// <summary>
    /// Gets the last knot in the curve.
    /// </summary>
    /// <returns>the knot</returns>
    public Knot2 GetLast ( )
    {
        return this.knots[this.knots.Count - 1];
    }

    /// <summary>
    /// Inserts a knot at a given index. When the curve is a closed loop, the
    /// index wraps around; this means negative indices are accepted.
    /// </summary>
    /// <param name="i">index</param>
    /// <param name="knot">knot</param>
    /// <returns>the curve</returns>
    public Curve2 Insert (in int i, Knot2 knot)
    {
        if (knot != null)
        {
            int k = this.closedLoop ? Utils.Mod (i, this.knots.Count + 1) : i;
            this.knots.Insert (k, knot);
        }
        return this;
    }

    /// <summary>
    /// Prepend a knot to the curve's list of knots.
    /// </summary>
    /// <param name="knot">knot</param>
    /// <returns>this curve</returns>
    public Curve2 Prepend (Knot2 knot)
    {
        if (knot != null) this.knots.Insert (0, knot);
        return this;
    }

    /// <summary>
    /// Prepend an array of knots to the curve's list of knots.
    /// </summary>
    /// <param name="kn">array of knots</param>
    /// <returns>this curve</returns>
    public Curve2 PrependAll (params Knot2[ ] kn)
    {
        int len = kn.Length;
        for (int i = 0, j = 0; i < len; ++i)
        {
            Knot2 knot = kn[i];
            if (knot != null)
            {
                this.knots.Insert (j, knot);
                ++j;
            }
        }
        return this;
    }

    /// <summary>
    /// Returns and removes a knot at a given index.
    /// </summary>
    /// <param name="i">index</param>
    /// <returns>the knot</returns>
    public Knot2 RemoveAt (in int i)
    {
        int j = this.closedLoop ? Utils.Mod (i, this.knots.Count) : i;
        Knot2 knot = this.knots[j];
        this.knots.RemoveAt (j);
        return knot;
    }

    /// <summary>
    /// Removes and returns the first knot in the curve.
    /// </summary>
    /// <returns>the knot</returns>
    public Knot2 RemoveFirst ( )
    {
        return this.RemoveAt (0);
    }

    /// <summary>
    /// Removes and returns the last knot in the curve.
    /// </summary>
    /// <returns>the knot</returns>
    public Knot2 RemoveLast ( )
    {
        return this.RemoveAt (this.knots.Count - 1);
    }

    /// <summary>
    /// Resets the curve, leaving two default knots.
    /// </summary>
    /// <returns>this curve</returns>
    public Curve2 Reset ( )
    {
        this.Resize (2);
        Knot2 kn0 = this.knots[0];
        Knot2 kn1 = this.knots[1];

        this.knots[0].Set (-0.5f, 0.0f, -0.25f, 0.25f, -0.75f, -0.25f);
        this.knots[1].Set (0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f);

        this.closedLoop = false;

        return this;
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
    protected Curve2 Resize (in int len)
    {
        int vlen = len < 2 ? 2 : len;
        int oldLen = this.knots.Count;
        int diff = vlen - oldLen;

        if (diff < 0)
        {
            int last = oldLen - 1;
            for (int i = 0; i < -diff; ++i)
            {
                this.knots.RemoveAt (last - i);
            }
        }
        else if (diff > 0)
        {
            this.knots.Capacity = vlen;
            for (int i = 0; i < diff; ++i)
            {
                this.knots.Add (new Knot2 ( ));
            }
        }

        return this;
    }

    /// <summary>
    /// Reverses the curve. This is done by reversing the list of knots and
    /// swapping the fore- and rear-handle of each knot.
    /// </summary>
    /// <returns>this curve</returns>
    public Curve2 Reverse ( )
    {
        this.knots.Reverse ( );
        foreach (Knot2 kn in this.knots) kn.Reverse ( );
        return this;
    }

    /// <summary>
    /// Rotates all knots in the curve by an angle in radians around the z axis.
    /// </summary>
    /// <param name="radians">angle</param>
    /// <returns>this curve</returns>
    public Curve2 RotateZ (in float radians)
    {
        float cosa = Utils.Cos (radians);
        float sina = Utils.Sin (radians);
        foreach (Knot2 kn in this.knots) kn.RotateZ (cosa, sina);
        return this;
    }

    /// <summary>
    /// Scales all knots in the curve by a scalar.
    /// </summary>
    /// <param name="scale">uniform scale</param>
    /// <returns>this curve</returns>
    public Curve2 Scale (in float scale)
    {
        if (scale != 0.0f)
        {
            foreach (Knot2 kn in this.knots) kn.Scale (scale);
        }
        return this;
    }

    /// <summary>
    /// Scales all knots in the curve by a vector.
    /// </summary>
    /// <param name="scale">nonuniform scale</param>
    /// <returns>this curve</returns>
    public Curve2 Scale (in Vec2 scale)
    {
        if (Vec2.All (scale))
        {
            foreach (Knot2 kn in this.knots) kn.Scale (scale);
        }
        return this;
    }

    /// <summary>
    /// Toggles whether or not the curve is a closed loop.
    /// </summary>
    /// <returns>the loop</returns>
    public Curve2 ToggleLoop ( )
    {
        this.closedLoop = !this.closedLoop;
        return this;
    }

    /// <summary>
    /// Returns the string representation of this curve.
    /// </summary>
    /// <param name="places">number of places</param>
    /// <returns>the string</returns>
    public String ToString (in int places = 4)
    {
        int len = this.knots.Count;
        int last = len - 1;

        StringBuilder sb = new StringBuilder (64 + 256 * len);
        sb.Append ("{ closedLoop: ");
        sb.Append (this.closedLoop ? "false" : "true");
        sb.Append (", knots: [ ");
        for (int i = 0; i < len; ++i)
        {
            Knot2 knot = this.knots[i];
            sb.Append (knot.ToString (places));
            if (i < last) sb.Append (", ");
        }
        sb.Append (" ] }");
        return sb.ToString ( );
    }

    /// <summary>
    /// Transforms all knots in the curve by a matrix.
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns></returns>
    public Curve2 Transform (in Mat3 m)
    {
        foreach (Knot2 kn in this.knots) kn.Transform (m);
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
    /// <param name="tr"></param>
    /// <returns></returns>
    public Curve2 Transform (in Transform2 tr)
    {
        foreach (Knot2 kn in this.knots) kn.Transform (tr);
        return this;
    }

    /// <summary>
    /// Translates all knots in the curve by a vector.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>this curve</returns>
    public Curve2 Translate (in Vec2 v)
    {
        foreach (Knot2 kn in this.knots) kn.Translate (v);
        return this;
    }

    /// <summary>
    /// Evaluates the coordinate and normalized tangent given a step in the
    /// range [0.0, 1.0] .
    ///
    /// Returns a named value tuple containing two vectors.
    /// </summary>
    /// <param name="curve">curve</param>
    /// <param name="step">step</param>
    /// <returns>the tuple</returns>
    public static (Vec2 coord, Vec2 tangent) Eval (in Curve2 curve, in float step)
    {
        List<Knot2> knots = curve.knots;
        int knotLength = knots.Count;

        float tScaled = 0.0f;
        int i = 0;
        Knot2 a = null;
        Knot2 b = null;

        if (curve.closedLoop)
        {
            tScaled = Utils.Mod1 (step) * knotLength;
            i = (int) tScaled;
            a = knots[Utils.Mod (i, knotLength)];
            b = knots[Utils.Mod (i + 1, knotLength)];
        }
        else
        {
            if (knotLength == 1 || step <= 0.0f)
            {
                return Curve2.EvalFirst (curve);
            }

            if (step >= 1.0f)
            {
                return Curve2.EvalLast (curve);
            }

            tScaled = step * (knotLength - 1);
            i = (int) tScaled;
            a = knots[i];
            b = knots[i + 1];
        }

        float t = tScaled - i;
        return (
            coord: Knot2.BezierPoint (a, b, t),
            tangent : Knot2.BezierTanUnit (a, b, t));
    }

    /// <summary>
    /// Evaluates the coordinate and normalized tangent of the first knot in the
    /// curve.
    ///
    /// Returns a named value tuple containing two vectors.
    /// </summary>
    /// <param name="curve">curve</param>
    /// <returns>the tuple</returns>
    public static (Vec2 coord, Vec2 tangent) EvalFirst (in Curve2 curve)
    {
        Knot2 kn = curve.knots[0];
        return (
            coord: kn.Coord,
            tangent: Vec2.Normalize (kn.ForeHandle - kn.Coord));
    }

    /// <summary>
    /// Evaluates the coordinate and normalized tangent of the last knot in the
    /// curve.
    ///
    /// Returns a named value tuple containing two vectors.
    /// </summary>
    /// <param name="curve">curve</param>
    /// <returns>the tuple</returns>
    public static (Vec2 coord, Vec2 tangent) EvalLast (in Curve2 curve)
    {
        List<Knot2> kns = curve.knots;
        Knot2 kn = kns[kns.Count - 1];
        return (
            coord: kn.Coord,
            tangent: Vec2.Normalize (kn.Coord - kn.RearHandle));
    }

    /// <summary>
    /// Creates a curve that approximates Bernoulli's lemniscate, which
    /// resembles an infinity loop (with equally proportioned lobes).
    /// </summary>
    /// <param name="target">output curve</param>
    /// <returns>lemniscate</returns>
    public static Curve2 Infinity (in Curve2 target)
    {
        target.Resize (6);
        target.closedLoop = true;
        
        target[0].Set (0.5f, 0.0f, 0.5f, 0.1309615f, 0.5f, -0.1309615f);
        target[1].Set (0.235709f, 0.166627f, 0.0505335f, 0.114256f, 0.361728f, 0.2022675f);
        target[2].Set (-0.235709f, -0.166627f, -0.361728f, -0.2022675f, -0.0505335f, -0.114256f);
        target[3].Set (-0.5f, 0.0f, -0.5f, 0.1309615f, -0.5f, -0.1309615f);
        target[4].Set (-0.235709f, 0.166627f, -0.0505335f, 0.114256f, -0.361728f, 0.2022675f);
        target[5].Set (0.235709f, -0.166627f, 0.361728f, -0.2022675f, 0.0505335f, -0.114256f);
        return target;
    }
}