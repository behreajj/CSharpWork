using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class Curve2 : IEnumerable
{
    protected bool closedLoop = false;

    protected List<Knot2> knots = new List<Knot2> ( );

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

    public Knot2[ ] Knots
    {
        get
        {
            Knot2[ ] result = new Knot2[this.knots.Count];
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

    public Knot2 this [int i]
    {
        get
        {
            return this.closedLoop ?
                this.knots[Utils.Mod (i, this.knots.Count)] :
                this.knots[i];
        }
    }

    public Curve2 (params Knot2[ ] kn)
    {
        this.AppendAll (kn);
    }

    public override string ToString ( )
    {
        return ToString (4);
    }

    public Curve2 Append (Knot2 knot)
    {
        if (knot != null) this.knots.Add (knot);
        return this;
    }

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

    public bool Contains (in Knot2 knot)
    {
        return this.knots.Contains (knot);
    }

    public IEnumerator GetEnumerator ( )
    {
        return this.knots.GetEnumerator ( );
    }

    public Knot2 GetFirst ( )
    {
        return this.knots[0];
    }

    public Knot2 GetLast ( )
    {
        return this.knots[this.knots.Count - 1];
    }

    public Curve2 Prepend (Knot2 knot)
    {
        if (knot != null) this.knots.Insert (0, knot);
        return this;
    }

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

    public Knot2 RemoveAt (in int i)
    {
        int j = this.closedLoop ? Utils.Mod (i, this.knots.Count) : i;
        Knot2 knot = this.knots[j];
        this.knots.RemoveAt (j);
        return knot;
    }

    public Knot2 RemoveFirst ( )
    {
        return this.RemoveAt (0);
    }

    public Knot2 RemoveLast ( )
    {
        return this.RemoveAt (this.knots.Count - 1);
    }

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

    public Curve2 Reverse ( )
    {
        this.knots.Reverse ( );
        foreach (Knot2 kn in this.knots) kn.Reverse ( );
        return this;
    }

    public Curve2 RotateZ (in float radians)
    {
        float cosa = Utils.Cos (radians);
        float sina = Utils.Sin (radians);
        foreach (Knot2 kn in this.knots) kn.RotateZ (cosa, sina);
        return this;
    }

    public Curve2 Scale (in float scale)
    {
        if (scale != 0.0f)
        {
            foreach (Knot2 kn in this.knots) kn.Scale (scale);
        }
        return this;
    }

    public Curve2 Scale (in Vec2 scale)
    {
        if (Vec2.All (scale))
        {
            foreach (Knot2 kn in this.knots) kn.Scale (scale);
        }
        return this;
    }

    public Curve2 ToggleLoop ( )
    {
        this.closedLoop = !this.closedLoop;
        return this;
    }

    public String ToString (in int places = 4)
    {
        int len = this.knots.Count;
        int last = len - 1;

        StringBuilder sb = new StringBuilder (64 + 256 * this.knots.Count);
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

    public Curve2 Transform (in Mat3 m)
    {
        foreach (Knot2 kn in this.knots) kn.Transform (m);
        return this;
    }

    public Curve2 Transform (in Transform2 tr)
    {
        foreach (Knot2 kn in this.knots) kn.Transform (tr);
        return this;
    }

    public Curve2 Translate (in Vec2 v)
    {
        foreach (Knot2 kn in this.knots) kn.Translate (v);
        return this;
    }

    public static (Vec2 coord, Vec2 tangent) Eval (in Curve2 curve, in float step)
    {
        List<Knot2> knots = curve.knots;
        int knotLength = knots.Count;

        float tScaled = 0.0f;
        int i = 0;
        Knot2 a = null;
        Knot2 b = null;

        if (curve.ClosedLoop)
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

    public static (Vec2 coord, Vec2 tangent) EvalFirst (in Curve2 curve)
    {
        Knot2 kn = curve.knots[0];
        return (
            coord: kn.Coord,
            tangent: Vec2.Normalize (kn.ForeHandle - kn.Coord));
    }

    public static (Vec2 coord, Vec2 tangent) EvalLast (in Curve2 curve)
    {
        List<Knot2> kns = curve.knots;
        Knot2 kn = kns[kns.Count - 1];
        return (
            coord: kn.Coord,
            tangent: Vec2.Normalize (kn.Coord - kn.RearHandle));
    }

    public static Curve2 Infinity (in Curve2 target)
    {
        target.Resize (6);
        target[0].Set (0.5f, 0.0f, 0.5f, 0.1309615f, 0.5f, -0.1309615f);
        target[1].Set (0.235709f, 0.166627f, 0.0505335f, 0.114256f, 0.361728f, 0.2022675f);
        target[2].Set (-0.235709f, -0.166627f, -0.361728f, -0.2022675f, -0.0505335f, -0.114256f);
        target[3].Set (-0.5f, 0.0f, -0.5f, 0.1309615f, -0.5f, -0.1309615f);
        target[4].Set (-0.235709f, 0.166627f, -0.0505335f, 0.114256f, -0.361728f, 0.2022675f);
        target[5].Set (0.235709f, -0.166627f, 0.361728f, -0.2022675f, 0.0505335f, -0.114256f);
        return target;
    }
}