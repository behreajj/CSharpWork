using System;
using System.Collections;
using System.Text;

[Serializable]
public readonly struct Vec2 : IComparable<Vec2>, IEquatable<Vec2>, IEnumerable
{
    private readonly float _x;
    private readonly float _y;

    public float x { get { return this._x; } }
    public float y { get { return this._y; } }

    public float this [int i]
    {
        get
        {
            switch (i)
            {
                case 0:
                case -2:
                    return this._x;
                case 1:
                case -1:
                    return this._y;
                default:
                    return 0.0f;
            }
        }
    }

    public Vec2 (float x = 0.0f, float y = 0.0f)
    {
        this._x = x;
        this._y = y;
    }

    public Vec2 (bool x = false, bool y = false)
    {
        this._x = x ? 1.0f : 0.0f;
        this._y = y ? 1.0f : 0.0f;
    }

    public override bool Equals (object value)
    {
        if (Object.ReferenceEquals (this, value)) return true;
        if (Object.ReferenceEquals (null, value)) return false;

        if (value is Vec2)
        {
            Vec2 v = (Vec2) value;

            // return Vec2.Approx (this, v);

            if (this._y.GetHashCode ( ) != v._y.GetHashCode ( ))
            {
                return false;
            }

            if (this._x.GetHashCode ( ) != v._x.GetHashCode ( ))
            {
                return false;
            }

            return true;
        }

        return false;
    }

    public override int GetHashCode ( )
    {
        unchecked
        {
            const int hashBase = -2128831035;
            const int hashMul = 16777619;
            int hash = hashBase;
            hash = hash * hashMul ^ this._x.GetHashCode ( );
            hash = hash * hashMul ^ this._y.GetHashCode ( );
            return hash;
        }
    }

    public override string ToString ( )
    {
        return ToString (4);
    }

    public int CompareTo (Vec2 v)
    {
        return (this._y > v._y) ? 1 :
            (this._y < v._y) ? -1 :
            (this._x > v._x) ? 1 :
            (this._x < v._x) ? -1 :
            0;
    }

    public bool Equals (Vec2 v)
    {
        // return Vec2.Approx (this, v);

        if (this._y.GetHashCode ( ) != v._y.GetHashCode ( ))
        {
            return false;
        }

        if (this._x.GetHashCode ( ) != v._x.GetHashCode ( ))
        {
            return false;
        }

        return true;
    }

    public IEnumerator GetEnumerator ( )
    {
        yield return this._x;
        yield return this._y;
    }

    public float[ ] ToArray ( )
    {
        return new float[ ] { this._x, this._y };
    }

    public string ToString (int places = 4)
    {
        return new StringBuilder (48)
            .Append ("{ x: ")
            .Append (Utils.ToFixed (this._x, places))
            .Append (", y: ")
            .Append (Utils.ToFixed (this._y, places))
            .Append (" }")
            .ToString ( );
    }

    public static implicit operator Vec2 (bool b)
    {
        float eval = b ? 1.0f : 0.0f;
        return new Vec2 (eval, eval);
    }

    public static implicit operator Vec2 (float s)
    {
        return new Vec2 (s, s);
    }

    public static explicit operator bool (in Vec2 v)
    {
        return Vec2.All (v);
    }

    public static explicit operator float (in Vec2 v)
    {
        return Vec2.Mag (v);
    }

    public static bool operator true (in Vec2 v)
    {
        return Vec2.All (v);
    }

    public static bool operator false (in Vec2 v)
    {
        return Vec2.None (v);
    }

    public static Vec2 operator ! (in Vec2 v)
    {
        return Vec2.Not (v);
    }

    public static Vec2 operator ~ (in Vec2 v)
    {
        return Vec2.Not (v);
    }

    public static Vec2 operator & (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (
            Utils.And (a._x, b._x),
            Utils.And (a._y, b._y));
    }

    public static Vec2 operator | (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (
            Utils.Or (a._x, b._x),
            Utils.Or (a._y, b._y));
    }

    public static Vec2 operator ^ (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (
            Utils.Xor (a._x, b._x),
            Utils.Xor (a._y, b._y));
    }

    public static Vec2 operator - (in Vec2 v)
    {
        return new Vec2 (-v._x, -v._y);
    }

    public static Vec2 operator * (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (a._x * b._x, a._y * b._y);
    }

    public static Vec2 operator * (in Vec2 a, float b)
    {
        return new Vec2 (a._x * b, a._y * b);
    }

    public static Vec2 operator * (float a, in Vec2 b)
    {
        return new Vec2 (a * b._x, a * b._y);
    }

    public static Vec2 operator / (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (
            Utils.Div (a._x, b._x),
            Utils.Div (a._y, b._y));
    }

    public static Vec2 operator / (in Vec2 a, float b)
    {
        if (b == 0.0f) return new Vec2 (0.0f, 0.0f);
        return new Vec2 (a._x / b, a._y / b);
    }

    public static Vec2 operator / (float a, in Vec2 b)
    {
        return new Vec2 (
            Utils.Div (a, b._x),
            Utils.Div (a, b._y));
    }

    public static Vec2 operator % (in Vec2 a, in Vec2 b)
    {
        return Vec2.Fmod (a, b);
    }

    public static Vec2 operator % (in Vec2 a, float b)
    {
        if (b == 0.0f) return a;
        return new Vec2 (a._x % b, a._y % b);
    }

    public static Vec2 operator % (float a, in Vec2 b)
    {
        return new Vec2 (
            Utils.Fmod (a, b._x),
            Utils.Fmod (a, b._y));
    }

    public static Vec2 operator + (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (a._x + b._x, a._y + b._y);
    }

    public static Vec2 operator - (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (a._x - b._x, a._y - b._y);
    }

    public static Vec2 operator < (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (a._x < b._x, a._y < b._y);
    }

    public static Vec2 operator > (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (a._x > b._x, a._y > b._y);
    }

    public static Vec2 operator <= (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (a._x <= b._x, a._y <= b._y);
    }

    public static Vec2 operator >= (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (a._x >= b._x, a._y >= b._y);
    }

    public static Vec2 operator != (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (a._x != b._x, a._y != b._y);
    }

    public static Vec2 operator == (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (a._x == b._x, a._y == b._y);
    }

    public static Vec2 Abs (in Vec2 v)
    {
        return new Vec2 (
            Utils.Abs (v._x),
            Utils.Abs (v._y));
    }

    public static bool All (in Vec2 v)
    {
        return v._x != 0.0f && v._y != 0.0f;
    }

    public static bool Any (in Vec2 v)
    {
        return v._x != 0.0f || v._y != 0.0f;
    }

    public static bool Approx (in Vec2 a, in Vec2 b, float tolerance = Utils.Epsilon)
    {
        return Utils.Approx (a._x, b._x, tolerance) &&
            Utils.Approx (a._y, b._y, tolerance);
    }

    public static bool ApproxMag (in Vec2 a,
        float b = 1.0f,
        float tolerance = Utils.Epsilon)
    {
        return Utils.Approx (Vec2.MagSq (a), b * b, tolerance);
    }

    public static bool AreParallel (in Vec2 a, in Vec2 b)
    {
        return a._x * b._y - a._y * b._x == 0.0f;
    }

    public static Vec2 BezierPoint (in Vec2 ap0 = new Vec2 ( ), in Vec2 cp0 = new Vec2 ( ), in Vec2 cp1 = new Vec2 ( ), in Vec2 ap1 = new Vec2 ( ),
        float step = 0.5f)
    {
        if (step <= 0.0f) return new Vec2 (ap0._x, ap0._y);
        else if (step >= 1.0f) return new Vec2 (ap1._x, ap1._y);

        float u = 1.0f - step;
        float tcb = step * step;
        float ucb = u * u;
        float usq3t = ucb * (step + step + step);
        float tsq3u = tcb * (u + u + u);
        ucb *= u;
        tcb *= step;

        return new Vec2 (
            ap0._x * ucb +
            cp0._x * usq3t +
            cp1._x * tsq3u +
            ap1._x * tcb,

            ap0._y * ucb +
            cp0._y * usq3t +
            cp1._y * tsq3u +
            ap1._y * tcb);
    }

    public static Vec2 BezierTangent (in Vec2 ap0 = new Vec2 ( ), in Vec2 cp0 = new Vec2 ( ), in Vec2 cp1 = new Vec2 ( ), in Vec2 ap1 = new Vec2 ( ),
        float step = 0.5f)
    {

        if (step <= 0.0f) return cp0 - ap0;
        else if (step >= 1.0f) return ap1 - cp1;

        float u = 1.0f - step;
        float t3 = step + step + step;
        float usq3 = u * (u + u + u);
        float tsq3 = step * t3;
        float ut6 = u * (t3 + t3);

        return new Vec2 (
            (cp0._x - ap0._x) * usq3 +
            (cp1._x - cp0._x) * ut6 +
            (ap1._x - cp1._x) * tsq3,

            (cp0._y - ap0._y) * usq3 +
            (cp1._y - cp0._y) * ut6 +
            (ap1._y - cp1._y) * tsq3);
    }

    public static Vec2 BezierTanUnit (
        Vec2 ap0 = new Vec2 ( ),
        Vec2 cp0 = new Vec2 ( ),
        Vec2 cp1 = new Vec2 ( ),
        Vec2 ap1 = new Vec2 ( ),
        float step = 0.5f)
    {
        return Vec2.Normalize (Vec2.BezierTangent (
            ap0, cp0, cp1, ap1, step));
    }

    public static Vec2 Ceil (in Vec2 v)
    {
        return new Vec2 (
            Utils.Ceil (v._x),
            Utils.Ceil (v._y));
    }

    public static Vec2 Clamp (in Vec2 v, float lb = 0.0f, float ub = 1.0f)
    {
        return new Vec2 (
            Utils.Clamp (v._x, lb, ub),
            Utils.Clamp (v._y, lb, ub));
    }

    public static Vec2 Clamp (in Vec2 v, in Vec2 lb, in Vec2 ub)
    {
        return new Vec2 (
            Utils.Clamp (v._x, lb._x, ub._x),
            Utils.Clamp (v._y, lb._y, ub._y));
    }

    public static Vec2 CopySign (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (
            Utils.CopySign (a._x, b._x),
            Utils.CopySign (a._y, b._y));
    }

    public static Vec2 CopySign (float a, in Vec2 b)
    {
        int n = Utils.Sign (a);
        return new Vec2 (
            Utils.Abs (b._x) * n,
            Utils.Abs (b._y) * n);
    }

    public static Vec2 CopySign (in Vec2 a, float b)
    {
        int n = Utils.Sign (b);
        return new Vec2 (
            Utils.Abs (a._x) * n,
            Utils.Abs (a._y) * n);
    }

    public static Vec2 Diff (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (
            Utils.Diff (b._x, a._x),
            Utils.Diff (b._y, a._y));
    }

    public static float DistChebyshev (in Vec2 a, in Vec2 b)
    {
        float dx = Utils.Diff (b._x, a._x);
        float dy = Utils.Diff (b._y, a._y);
        return Utils.Max (dx, dy);
    }

    public static float DistEuclidean (in Vec2 a, in Vec2 b)
    {
        float dx = b._x - a._x;
        float dy = b._y - a._y;
        return Utils.Sqrt (dx * dx + dy * dy);
    }

    public static float DistManhattan (in Vec2 a, in Vec2 b)
    {
        float dx = Utils.Diff (b._x, a._x);
        float dy = Utils.Diff (b._y, a._y);
        return dx + dy;
    }

    public static float DistMinkowski (in Vec2 a, in Vec2 b, float c = 2.0f)
    {
        if (c == 0.0f) return 0.0f;

        float dx = Utils.Pow (Utils.Diff (b._x, a._x), c);
        float dy = Utils.Pow (Utils.Diff (b._y, a._y), c);
        return Utils.Pow (dx + dy, 1.0f / c);
    }

    public static float DistSq (in Vec2 a, in Vec2 b)
    {
        float dx = b._x - a._x;
        float dy = b._y - a._y;
        return dx * dx + dy * dy;
    }

    public static float Dot (in Vec2 a, in Vec2 b)
    {
        return a._x * b._x + a._y * b._y;
    }

    public static Vec2 Floor (in Vec2 v)
    {
        return new Vec2 (
            Utils.Floor (v._x),
            Utils.Floor (v._y));
    }

    public static Vec2 Fmod (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (
            Utils.Fmod (a._x, b._x),
            Utils.Fmod (a._y, b._y));
    }

    public static Vec2 Fract (in Vec2 v)
    {
        return new Vec2 (
            v._x - (int) v._x,
            v._y - (int) v._y);
    }

    public static Vec2 FromPolar (
        float heading = 0.0f,
        float radius = 1.0f)
    {
        return new Vec2 (
            radius * Utils.Cos (heading),
            radius * Utils.Sin (heading));
    }

    public static Vec2[, ] Grid (int rows, int cols, in Vec2 lowerBound, in Vec2 upperBound)
    {
        int rval = rows < 3 ? 3 : rows;
        int cval = cols < 3 ? 3 : cols;

        float iToStep = 1.0f / (rval - 1.0f);
        float jToStep = 1.0f / (cval - 1.0f);

        /* Calculate x values in separate loop. */
        float[ ] xs = new float[cval];
        for (int j = 0; j < cval; ++j)
        {
            xs[j] = Utils.LerpUnclamped (
                lowerBound._x,
                upperBound._x,
                j * jToStep);
        }

        Vec2[, ] result = new Vec2[rval, cval];
        for (int i = 0; i < rval; ++i)
        {

            float y = Utils.LerpUnclamped (
                lowerBound._y,
                upperBound._y,
                i * iToStep);

            for (int j = 0; j < cval; ++j)
            {
                result[i, j] = new Vec2 (xs[j], y);
            }
        }
        return result;
    }

    public static float HeadingSigned (in Vec2 v)
    {
        return Utils.Atan2 (v._y, v._x);
    }

    public static float HeadingUnsigned (in Vec2 v)
    {
        return Utils.ModRadians (Utils.Atan2 (v._y, v._x));
    }

    public static Vec2 Limit (in Vec2 v, float limit = float.MaxValue)
    {
        float mSq = Vec2.MagSq (v);
        if (mSq > (limit * limit))
        {
            return Utils.Div (limit, Vec2.Mag (v)) * v;
        }
        return v;
    }

    public static Vec2 LinearStep (in Vec2 edge0, in Vec2 edge1, in Vec2 x)
    {
        return Vec2.Clamp ((x - edge0) / (edge1 - edge0));
    }

    public static float Mag (in Vec2 v)
    {
        return Utils.Sqrt (v._x * v._x + v._y * v._y);
    }

    public static float MagSq (in Vec2 v)
    {
        return v._x * v._x + v._y * v._y;
    }

    public static Vec2 Map (in Vec2 v, in Vec2 lbOrigin, in Vec2 ubOrigin, in Vec2 lbDest, in Vec2 ubDest)
    {
        return new Vec2 (
            Utils.Map (v._x, lbOrigin._x, ubOrigin._x, lbDest._x, ubDest._x),
            Utils.Map (v._y, lbOrigin._y, ubOrigin._y, lbDest._y, ubDest._y));
    }

    public static Vec2 Max (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (
            Utils.Max (a._x, b._x),
            Utils.Max (a._y, b._y));
    }

    public static Vec2 Min (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (
            Utils.Min (a._x, b._x),
            Utils.Min (a._y, b._y));
    }

    public static Vec2 Mix (in Vec2 a, in Vec2 b, float t = 0.5f)
    {
        float u = 1.0f - t;
        return new Vec2 (
            u * a._x + t * b._x,
            u * a._y + t * b._y);
    }

    public static Vec2 Mix (in Vec2 a, in Vec2 b, in Vec2 t)
    {
        return new Vec2 (
            (1.0f - t._x) * a._x + t._x * b._x,
            (1.0f - t._y) * a._y + t._y * b._y);
    }

    public static Vec2 Mod (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (
            Utils.Mod (a._x, b._x),
            Utils.Mod (a._y, b._y));
    }

    public static Vec2 Mod1 (in Vec2 v)
    {
        return new Vec2 (
            Utils.Mod1 (v._x),
            Utils.Mod1 (v._y));
    }

    public static bool None (in Vec2 v)
    {
        return v._x == 0.0f && v._y == 0.0f;
    }

    public static Vec2 Normalize (in Vec2 v)
    {
        return v / Vec2.Mag (v);
    }

    public static Vec2 Not (in Vec2 v)
    {
        return new Vec2 (
            v._x != 0.0f ? 0.0f : 1.0f,
            v._y != 0.0f ? 0.0f : 1.0f);
    }

    public static Vec2 PerpendicularCCW (in Vec2 v)
    {
        return new Vec2 (-v._y, v._x);
    }

    public static Vec2 PerpendicularCW (in Vec2 v)
    {
        return new Vec2 (v._y, -v._x);
    }

    public static Vec2 Pow (in Vec2 a, in Vec2 b)
    {
        return new Vec2 (
            Utils.Pow (a._x, b._x),
            Utils.Pow (a._y, b._y));
    }

    public static Vec2 Pow (in Vec2 a, float b)
    {
        return new Vec2 (
            Utils.Pow (a._x, b),
            Utils.Pow (a._y, b));
    }

    public static Vec2 Pow (float a, in Vec2 b)
    {
        return new Vec2 (
            Utils.Pow (a, b._x),
            Utils.Pow (a, b._y));
    }

    public static float ProjectScalar (in Vec2 a, in Vec2 b)
    {
        float bSq = Vec2.MagSq (b);
        if (bSq != 0.0f) return Vec2.Dot (a, b) / bSq;
        return 0.0f;
    }

    public static Vec2 ProjectVector (in Vec2 a, in Vec2 b)
    {
        return b * Vec2.ProjectScalar (a, b);
    }

    public static Vec2 Reflect (in Vec2 i, in Vec2 n)
    {
        return i - ((2.0f * Vec2.Dot (n, i)) * n);
    }

    public static Vec2 Refract (in Vec2 i, in Vec2 n, float eta)
    {
        float iDotN = Vec2.Dot (i, n);
        float k = 1.0f - eta * eta * (1.0f - iDotN * iDotN);
        if (k < 0.0f) return new Vec2 ( );
        return (eta * i) -
            (n * (eta * iDotN + Utils.Sqrt (k)));
    }

    public static Vec2 Rescale (in Vec2 v, float scalar = 1.0f)
    {
        return Utils.Div (scalar, Vec2.Mag (v)) * v;
    }

    public static Vec2 RotateZ (in Vec2 v, float radians = 0.0f)
    {
        return Vec2.RotateZ (v, Utils.Cos (radians), Utils.Sin (radians));
    }

    public static Vec2 RotateZ (in Vec2 v,
        float cosa = 1.0f,
        float sina = 0.0f)
    {
        return new Vec2 (
            cosa * v._x - sina * v._y,
            cosa * v._y + sina * v._x);
    }

    public static Vec2 Round (in Vec2 v)
    {
        return new Vec2 (
            Utils.Round (v._x),
            Utils.Round (v._y));
    }

    public static Vec2 SmoothStep (in Vec2 edge0, in Vec2 edge1, in Vec2 x)
    {
        Vec2 t = Vec2.Clamp ((x - edge0) / (edge1 - edge0));
        return t * t * (new Vec2 (3.0f, 3.0f) - (t + t));
    }

    public static Vec2 Sign (in Vec2 v)
    {
        return new Vec2 (
            Utils.Sign (v._x),
            Utils.Sign (v._y));
    }

    public static Vec2 Step (in Vec2 edge, in Vec2 x)
    {
        return new Vec2 (
            x._x < edge._x ? 0.0f : 1.0f,
            x._y < edge._y ? 0.0f : 1.0f);
    }

    public static Vec2 Trunc (in Vec2 v)
    {
        return new Vec2 ((int) v._x, (int) v._y);
    }

    public static Vec2 Back
    {
        get
        {
            return new Vec2 (0.0f, -1.0f);
        }
    }

    public static Vec2 Forward
    {
        get
        {
            return new Vec2 (0.0f, 1.0f);
        }
    }

    public static Vec2 Left
    {
        get
        {
            return new Vec2 (-1.0f, 0.0f);
        }
    }

    public static Vec2 Right
    {
        get
        {
            return new Vec2 (1.0f, 0.0f);
        }
    }
}