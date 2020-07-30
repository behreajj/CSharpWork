using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class Curve3 : IEnumerable
{
    protected bool closedLoop = false;

    protected List<Knot3> knots = new List<Knot3> ( );

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

    public Knot3[ ] Knots
    {
        get
        {
            Knot3[ ] result = new Knot3[this.knots.Count];
            this.knots.CopyTo (result);
            return result;
        }
    }

    public int Length
    {
        get
        {
            return this.knots.Count;
        }
    }

    public Knot3 this [int i]
    {
        get
        {
            return this.closedLoop ?
                this.knots[Utils.Mod (i, this.knots.Count)] :
                this.knots[i];
        }
    }

    public (Vec3 coord, Vec3 tangent) this [float i]
    {
        get
        {
            return Curve3.Eval (this, i);
        }
    }

    public Curve3 ( ) { }

    public Curve3 (bool cl, params Knot3[ ] kn)
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

    public Curve3 Append (Knot3 knot)
    {
        // if (knot != null) this.knots.Add (knot);
        this.knots.Add (knot);
        return this;
    }

    public Curve3 AppendAll (params Knot3[ ] kn)
    {
        int len = kn.Length;
        for (int i = 0; i < len; ++i)
        {
            // Knot3 knot = kn[i];
            // if (knot != null) this.knots.Add (knot);
            this.knots.Add (kn[i]);
        }
        return this;
    }

    public bool Contains (in Knot3 knot)
    {
        return this.knots.Contains (knot);
    }

    public IEnumerator GetEnumerator ( )
    {
        return this.knots.GetEnumerator ( );
    }

    public Knot3 GetFirst ( )
    {
        return this.knots[0];
    }

    public Knot3 GetLast ( )
    {
        return this.knots[this.knots.Count - 1];
    }

    public Curve3 Insert (in int i, Knot3 knot)
    {
        // if (knot != null)
        // {
        int k = this.closedLoop ? Utils.Mod (i, this.knots.Count + 1) : i;
        this.knots.Insert (k, knot);
        // }
        return this;
    }

    public Curve3 Prepend (Knot3 knot)
    {
        // if (knot != null) this.knots.Insert (0, knot);
        this.knots.Insert (0, knot);
        return this;
    }

    public Curve3 PrependAll (params Knot3[ ] kn)
    {
        int len = kn.Length;
        for (int i = 0, j = 0; i < len; ++i)
        {
            Knot3 knot = kn[i];
            // if (knot != null)
            // {
            //     this.knots.Insert (j, knot);
            //     ++j;
            // }

            this.knots.Insert (j, knot);
            ++j;
        }
        return this;
    }

    public Knot3 RemoveAt (in int i)
    {
        int j = this.closedLoop ? Utils.Mod (i, this.knots.Count) : i;
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

    public Curve3 Reverse ( )
    {
        this.knots.Reverse ( );
        foreach (Knot3 kn in this.knots) kn.Reverse ( );
        return this;
    }

    public Curve3 Rotate (in float radians, in Vec3 axis)
    {
        float cosa = Utils.Cos (radians);
        float sina = Utils.Sin (radians);
        foreach (Knot3 kn in this.knots) kn.Rotate (cosa, sina, axis);
        return this;
    }

    public Curve3 RotateX (in float radians)
    {
        float cosa = Utils.Cos (radians);
        float sina = Utils.Sin (radians);
        foreach (Knot3 kn in this.knots) kn.RotateX (cosa, sina);
        return this;
    }

    public Curve3 RotateY (in float radians)
    {
        float cosa = Utils.Cos (radians);
        float sina = Utils.Sin (radians);
        foreach (Knot3 kn in this.knots) kn.RotateY (cosa, sina);
        return this;
    }

    public Curve3 RotateZ (in float radians)
    {
        float cosa = Utils.Cos (radians);
        float sina = Utils.Sin (radians);
        foreach (Knot3 kn in this.knots) kn.RotateZ (cosa, sina);
        return this;
    }

    public Curve3 Scale (in float scale)
    {
        if (scale != 0.0f)
        {
            foreach (Knot3 kn in this.knots) kn.Scale (scale);
        }
        return this;
    }

    public Curve3 Scale (in Vec3 scale)
    {
        if (Vec3.All (scale))
        {
            foreach (Knot3 kn in this.knots) kn.Scale (scale);
        }
        return this;
    }

    public Curve3 ToggleLoop ( )
    {
        this.closedLoop = !this.closedLoop;
        return this;
    }

    public String ToString (in int places = 4)
    {
        int len = this.knots.Count;
        int last = len - 1;

        StringBuilder sb = new StringBuilder (64 + 512 * len);
        sb.Append ("{ closedLoop: ");
        sb.Append (this.closedLoop ? "false" : "true");
        sb.Append (", knots: [ ");
        for (int i = 0; i < len; ++i)
        {
            Knot3 knot = this.knots[i];
            sb.Append (knot.ToString (places));
            if (i < last) sb.Append (", ");
        }
        sb.Append (" ] }");
        return sb.ToString ( );
    }

    public Curve3 Transform (in Mat4 m)
    {
        foreach (Knot3 kn in this.knots) kn.Transform (m);
        return this;
    }

    public Curve3 Transform (in Transform3 tr)
    {
        foreach (Knot3 kn in this.knots) kn.Transform (tr);
        return this;
    }

    public Curve3 Translate (in Vec3 v)
    {
        foreach (Knot3 kn in this.knots) kn.Translate (v);
        return this;
    }

    public static (Vec3 coord, Vec3 tangent) Eval (in Curve3 curve, in float step)
    {
        List<Knot3> knots = curve.knots;
        int knotLength = knots.Count;

        float tScaled = 0.0f;
        int i = 0;
        Knot3 a = null;
        Knot3 b = null;

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

    public static (Vec3 coord, Vec3 tangent) EvalFirst (in Curve3 curve)
    {
        Knot3 kn = curve.knots[0];
        return (
            coord: kn.Coord,
            tangent: Vec3.Normalize (kn.ForeHandle - kn.Coord));
    }

    public static (Vec3 coord, Vec3 tangent) EvalLast (in Curve3 curve)
    {
        List<Knot3> kns = curve.knots;
        Knot3 kn = kns[kns.Count - 1];
        return (
            coord: kn.Coord,
            tangent: Vec3.Normalize (kn.Coord - kn.RearHandle));
    }
}