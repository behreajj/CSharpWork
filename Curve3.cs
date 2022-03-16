using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Organizes a 3D Bezier curve into a list of knots. Provides a function to
/// retrieve a point and tangent on a curve from a step in the range [0.0, 1.0]
/// .
/// </summary>
public class Curve3 : IEnumerable
{
    /// <summary>
    /// A flag for whether or not the curve is a closed loop.
    /// </summary>
    protected bool closedLoop = false;

    /// <summary>
    ///  The list of knots contained by the curve.
    /// </summary>
    protected List<Knot3> knots = new List<Knot3> ( );

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
    public Knot3[ ] Knots
    {
        get
        {
            Knot3[ ] result = new Knot3[this.knots.Count];
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

    /// <summary>
    /// Retrieves a knot from this curve. If the curve is a closed loop, wraps
    /// the index by the number of elements in the list of knots.
    /// </summary>
    /// <value>knot</value>
    public Knot3 this [int i]
    {
        get
        {
            return this.closedLoop ?
                this.knots[Utils.RemFloor (i, this.knots.Count)] :
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
    public (Vec3 coord, Vec3 tangent) this [float i]
    {
        get
        {
            return Curve3.Eval (this, i);
        }
    }

    /// <summary>
    /// The default curve constructor.
    /// </summary>
    public Curve3 ( ) { }

    /// <summary>
    /// Creates a curve from a list of knots and a closed loop flag.
    /// </summary>
    /// <param name="cl">closed loop</param>
    /// <param name="kn">knots</param>
    public Curve3 (bool cl, params Knot3[ ] kn)
    {
        this.closedLoop = cl;
        this.AppendAll (kn);
    }

    /// <summary>
    /// Returns a hash code representing this curve.
    /// </summary>
    /// <returns>the hash code</returns>
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

    /// <summary>
    /// Returns a string representation of this curve.
    /// </summary>
    /// <returns>string</returns>
    public override string ToString ( )
    {
        return Curve3.ToString (this);
    }

    /// <summary>
    /// Append a knot to the curve's list of knots.
    /// </summary>
    /// <param name="knot">knot</param>
    /// <returns>this curve</returns>
    public Curve3 Append (Knot3 knot)
    {
        this.knots.Add (knot);
        return this;
    }

    /// <summary>
    /// Append an collection of 2D knots to the curve's list of knots.
    /// </summary>
    /// <param name="kn">knots</param>
    /// <returns>this curve</returns>
    public Curve3 AppendAll (params Knot2[ ] kn)
    {
        int len = kn.Length;
        for (int i = 0; i < len; ++i) { this.knots.Add (kn[i]); }
        return this;
    }

    /// <summary>
    /// Append an collection of knots to the curve's list of knots.
    /// </summary>
    /// <param name="kn">knots</param>
    /// <returns>this curve</returns>
    public Curve3 AppendAll (params Knot3[ ] kn)
    {
        this.knots.AddRange (kn);
        return this;
    }

    /// <summary>
    /// Evaluates whether a knot is contained by this curve.
    /// </summary>
    /// <param name="knot">knot</param>
    /// <returns>the evaluation</returns>
    public bool Contains (in Knot3 knot)
    {
        return this.knots.Contains (knot);
    }

    /// <summary>
    /// Flips this curve on the x axis, then reverses the curve.
    /// </summary>
    /// <returns>this curve</returns>
    public Curve3 FlipX ( )
    {
        this.knots.Reverse ( );
        foreach (Knot3 kn in this.knots) { kn.FlipX ( ); kn.Reverse ( ); }
        return this;
    }

    /// <summary>
    /// Flips this curve on the y axis, then reverses the curve.
    /// </summary>
    /// <returns>this curve</returns>
    public Curve3 FlipY ( )
    {
        this.knots.Reverse ( );
        foreach (Knot3 kn in this.knots) { kn.FlipY ( ); kn.Reverse ( ); }
        return this;
    }

    /// <summary>
    /// Flips this curve on the z axis, then reverses the curve.
    /// </summary>
    /// <returns>this curve</returns>
    public Curve3 FlipZ ( )
    {
        this.knots.Reverse ( );
        foreach (Knot3 kn in this.knots) { kn.FlipZ ( ); kn.Reverse ( ); }
        return this;
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
    public Knot3 GetFirst ( )
    {
        return this.knots[0];
    }

    /// <summary>
    /// Gets the last knot in the curve.
    /// </summary>
    /// <returns>the knot</returns>
    public Knot3 GetLast ( )
    {
        return this.knots[this.knots.Count - 1];
    }

    public Curve3 Insert (in int i, Knot3 knot)
    {
        int k = this.closedLoop ? Utils.RemFloor (i, this.knots.Count + 1) : i;
        this.knots.Insert (k, knot);
        return this;
    }

    public Curve3 Prepend (Knot3 knot)
    {
        this.knots.Insert (0, knot);
        return this;
    }

    public Curve3 PrependAll (params Knot2[ ] kn)
    {
        int len = kn.Length;
        for (int i = 0, j = 0; i < len; ++i)
        {
            Knot3 knot = (Knot3) kn[i];
            this.knots.Insert (j, knot);
            ++j;
        }
        return this;
    }

    public Curve3 PrependAll (params Knot3[ ] kn)
    {
        int len = kn.Length;
        for (int i = 0, j = 0; i < len; ++i)
        {
            Knot3 knot = kn[i];
            this.knots.Insert (j, knot);
            ++j;
        }
        return this;
    }

    public Knot3 RemoveAt (in int i)
    {
        int j = this.closedLoop ? Utils.RemFloor (i, this.knots.Count) : i;
        Knot3 knot = this.knots[j];
        this.knots.RemoveAt (j);
        return knot;
    }

    public Knot3 RemoveFirst ( )
    {
        return this.RemoveAt (0);
    }

    public Knot3 RemoveLast ( )
    {
        return this.RemoveAt (this.knots.Count - 1);
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
    protected Curve3 Resize (in int len)
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
                this.knots.Add (new Knot3 ( ));
            }
        }

        return this;
    }

    /// <summary>
    /// Reverses the curve. This is done by reversing the list of knots and
    /// swapping the fore- and rear-handle of each knot.
    /// </summary>
    /// <returns>this curve</returns>
    public Curve3 Reverse ( )
    {
        this.knots.Reverse ( );
        foreach (Knot3 kn in this.knots) { kn.Reverse ( ); }
        return this;
    }

    /// <summary>
    /// Rotates all knots in a curve by a quaternion.
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <returns>this curve</returns>
    public Curve3 Rotate (in Quat q)
    {
        foreach (Knot3 kn in this.knots) { kn.Rotate (q); }
        return this;
    }

    /// <summary>
    /// Rotates all knots in the curve by an angle in radians around an
    /// arbitrary axis.
    /// </summary>
    /// <param name="radians">angle</param>
    /// <param name="axis">axis</param>
    /// <returns>this curve</returns>
    public Curve3 Rotate (in float radians, in Vec3 axis)
    {
        float cosa = Utils.Cos (radians);
        float sina = Utils.Sin (radians);
        foreach (Knot3 kn in this.knots) { kn.Rotate (cosa, sina, axis); }
        return this;
    }

    /// <summary>
    /// Rotates all knots in the curve by an angle in radians around the x axis.
    /// </summary>
    /// <param name="radians">angle</param>
    /// <returns>this curve</returns>
    public Curve3 RotateX (in float radians)
    {
        float cosa = Utils.Cos (radians);
        float sina = Utils.Sin (radians);
        foreach (Knot3 kn in this.knots) { kn.RotateX (cosa, sina); }
        return this;
    }

    /// <summary>
    /// Rotates all knots in the curve by an angle in radians around the y axis.
    /// </summary>
    /// <param name="radians">angle</param>
    /// <returns>this curve</returns>
    public Curve3 RotateY (in float radians)
    {
        float cosa = Utils.Cos (radians);
        float sina = Utils.Sin (radians);
        foreach (Knot3 kn in this.knots) { kn.RotateY (cosa, sina); }
        return this;
    }

    /// <summary>
    /// Rotates all knots in the curve by an angle in radians around the z axis.
    /// </summary>
    /// <param name="radians">angle</param>
    /// <returns>this curve</returns>
    public Curve3 RotateZ (in float radians)
    {
        float cosa = Utils.Cos (radians);
        float sina = Utils.Sin (radians);
        foreach (Knot3 kn in this.knots) { kn.RotateZ (cosa, sina); }
        return this;
    }

    /// <summary>
    /// Scales all knots in the curve by a scalar.
    /// </summary>
    /// <param name="scale">uniform scale</param>
    /// <returns>this curve</returns>
    public Curve3 Scale (in float scale)
    {
        if (scale != 0.0f)
        {
            foreach (Knot3 kn in this.knots) { kn.Scale (scale); }
        }
        return this;
    }

    /// <summary>
    /// Scales all knots in the curve by a vector.
    /// </summary>
    /// <param name="scale">nonuniform scale</param>
    /// <returns>this curve</returns>
    public Curve3 Scale (in Vec3 scale)
    {
        if (Vec3.All (scale))
        {
            foreach (Knot3 kn in this.knots) { kn.Scale (scale); }
        }
        return this;
    }

    /// <summary>
    /// Toggles whether or not the curve is a closed loop.
    /// </summary>
    /// <returns>the loop</returns>
    public Curve3 ToggleLoop ( )
    {
        this.closedLoop = !this.closedLoop;
        return this;
    }

    /// <summary>
    /// Transforms all knots in the curve by a matrix.
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>this curve</returns>
    public Curve3 Transform (in Mat4 m)
    {
        foreach (Knot3 kn in this.knots) { kn.Transform (m); }
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
    /// <returns>this curve</returns>
    public Curve3 Transform (in Transform3 tr)
    {
        foreach (Knot3 kn in this.knots) { kn.Transform (tr); }
        return this;
    }

    /// <summary>
    /// Translates all knots in the curve by a vector.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>this curve</returns>
    public Curve3 Translate (in Vec3 v)
    {
        foreach (Knot3 kn in this.knots) { kn.Translate (v); }
        return this;
    }

    /// <summary>
    /// Converts a 2D curve to a 3D curve.
    /// </summary>
    /// <param name="c">3D curve</param>
    public static implicit operator Curve3 (in Curve2 c)
    {
        Curve3 result = new Curve3 ( );
        result.closedLoop = c.ClosedLoop;
        foreach (Knot2 kn in c) { result.Append (kn); }
        return result;
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
    public static (Vec3 coord, Vec3 tangent) Eval (in Curve3 curve, in float step)
    {
        List<Knot3> knots = curve.knots;
        int knotLength = knots.Count;

        float tScaled;
        int i;
        Knot3 a;
        Knot3 b;

        if (curve.closedLoop)
        {
            tScaled = Utils.RemFloor (step, 1.0f) * knotLength;
            i = (int) tScaled;
            a = knots[Utils.RemFloor (i, knotLength)];
            b = knots[Utils.RemFloor (i + 1, knotLength)];
        }
        else
        {
            if (knotLength == 1 || step <= 0.0f)
            {
                return Curve3.EvalFirst (curve);
            }

            if (step >= 1.0f)
            {
                return Curve3.EvalLast (curve);
            }

            tScaled = step * (knotLength - 1);
            i = (int) tScaled;
            a = knots[i];
            b = knots[i + 1];
        }

        float t = tScaled - i;
        return (
            coord: Knot3.BezierPoint (a, b, t),
            tangent : Knot3.BezierTanUnit (a, b, t));
    }

    /// <summary>
    /// Evaluates the coordinate and normalized tangent of the first knot in the
    /// curve.
    ///
    /// Returns a named value tuple containing two vectors.
    /// </summary>
    /// <param name="curve">curve</param>
    /// <returns>the tuple</returns>
    public static (Vec3 coord, Vec3 tangent) EvalFirst (in Curve3 curve)
    {
        Knot3 kn = curve.knots[0];
        return (
            coord: kn.Coord,
            tangent: Vec3.Normalize (kn.ForeHandle - kn.Coord));
    }

    /// <summary>
    /// Evaluates the coordinate and normalized tangent of the last knot in the
    /// curve.
    ///
    /// Returns a named value tuple containing two vectors.
    /// </summary>
    /// <param name="curve">curve</param>
    /// <returns>the tuple</returns>
    public static (Vec3 coord, Vec3 tangent) EvalLast (in Curve3 curve)
    {
        List<Knot3> kns = curve.knots;
        Knot3 kn = kns[kns.Count - 1];
        return (
            coord: kn.Coord,
            tangent: Vec3.Normalize (kn.Coord - kn.RearHandle));
    }

    /// <summary>
    /// Returns the string representation of a curve.
    /// </summary>
    /// <param name="c">curve</param>
    /// <param name="places">number of places</param>
    /// <returns>the string</returns>
    public static string ToString (in Curve3 c, in int places = 4)
    {
        return Curve3.ToString (new StringBuilder (1024), c, places).ToString ( );
    }

    /// <summary>
    /// Appends a string representation of a curve to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="c">curve</param>
    /// <param name="places">number of places</param>
    /// <returns>the string</returns>
    public static StringBuilder ToString (in StringBuilder sb, in Curve3 c, in int places = 4)
    {
        List<Knot3> knots = c.knots;
        int len = knots.Count;
        int last = len - 1;

        sb.Append ("{ closedLoop: ");
        sb.Append (c.closedLoop ? "false" : "true");
        sb.Append (", knots: [ ");
        for (int i = 0; i < last; ++i)
        {
            Knot3.ToString (sb, knots[i], places);
            sb.Append (", ");
        }
        Knot3.ToString (sb, knots[last], places);
        sb.Append (" ] }");
        return sb;
    }
}