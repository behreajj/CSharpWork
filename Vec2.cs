using System;
using System.Text;

[Serializable]
public readonly struct Vec2 : IComparable<Vec2>, IEquatable<Vec2>
{
        public readonly float x;
        public readonly float y;

        public float X
        {
            get
            {
                return this.x;
            }

            // set
            // {
            //     this.x = value;
            // }
        }

        public float Y
        {
            get
            {
                return this.y;
            }

            // set
            // {
            //     this.y = value;
            // }
        }

        public float this [int i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                    case -2:
                        return this.x;
                    case 1:
                    case -1:
                        return this.y;
                    default:
                        return 0.0f;
                }
            }

            // set
            // {
            //     switch (i)
            //     {
            //         case 0:
            //         case -2:
            //             this.x = value;
            //             break;
            //         case 1:
            //         case -1:
            //             this.y = value;
            //             break;
            //     }
            // }
        }

        public Vec2 (float x = 0.0f, float y = 0.0f)
        {
            this.x = x;
            this.y = y;
        }

        public Vec2 (bool x = false, bool y = false)
        {
            this.x = x ? 1.0f : 0.0f;
            this.y = y ? 1.0f : 0.0f;
        }

        public override bool Equals (object value)
        {
            if(Object.ReferenceEquals(this, value))
            {
                return true;
            }

            if(Object.ReferenceEquals(null, value))
            {
                return false;
            }

            if(value is Vec2)
            {
                return Vec2.Approx(this, (Vec2)value);
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
                hash = hash * hashMul ^ this.x.GetHashCode ( );
                hash = hash * hashMul ^ this.y.GetHashCode ( );
                return hash;
            }
        }

        public override string ToString ( )
        {
            return ToString (4);
        }

        public int CompareTo (Vec2 v)
        {
            return (this.y > v.y) ? 1 :
                (this.y < v.y) ? -1 :
                (this.x > v.x) ? 1 :
                (this.x < v.x) ? -1 :
                0;
        }

        public bool Equals (Vec2 v)
        {
            // return Vec2.Approx (this, v);

            if(this.y.GetHashCode() != v.y.GetHashCode())
            {
                return false;
            }

            if(this.x.GetHashCode() != v.x.GetHashCode())
            {
                return false;
            }

            return true;
        }

        // public Vec2 Reset ( )
        // {
        //     return this.Set (0.0f, 0.0f);
        // }

        // public Vec2 Set (float x = 0.0f, float y = 0.0f)
        // {
        //     this.x = x;
        //     this.y = y;
        //     return this;
        // }

        // public Vec2 Set (bool x = false, bool y = false)
        // {
        //     this.x = x ? 1.0f : 0.0f;
        //     this.y = y ? 1.0f : 0.0f;
        //     return this;
        // }

        public float[ ] ToArray ( )
        {
            return new float[ ] { this.x, this.y };
        }

        public string ToString (int places = 4)
        {
            return new StringBuilder (48)
                .Append ("{ x: ")
                .Append (Utils.ToFixed (this.x, places))
                .Append (", y: ")
                .Append (Utils.ToFixed (this.y, places))
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

        public static explicit operator bool (Vec2 v)
        {
            return Vec2.All (v);
        }

        public static explicit operator float (Vec2 v)
        {
            return Vec2.Mag (v);
        }

        public static bool operator true (Vec2 v)
        {
            return Vec2.All (v);
        }

        public static bool operator false (Vec2 v)
        {
            return Vec2.None (v);
        }

        public static Vec2 operator ! (Vec2 v)
        {
            return Vec2.Not (v);
        }

        public static Vec2 operator ~ (Vec2 v)
        {
            return Vec2.Not (v);
        }

        public static Vec2 operator & (Vec2 a, Vec2 b)
        {
            return new Vec2 (
                Utils.And (a.x, b.x),
                Utils.And (a.y, b.y));
        }

        public static Vec2 operator | (Vec2 a, Vec2 b)
        {
            return new Vec2 (
                Utils.Or (a.x, b.x),
                Utils.Or (a.y, b.y));
        }

        public static Vec2 operator ^ (Vec2 a, Vec2 b)
        {
            return new Vec2 (
                Utils.Xor (a.x, b.x),
                Utils.Xor (a.y, b.y));
        }

        public static Vec2 operator - (Vec2 v)
        {
            return new Vec2 (-v.x, -v.y);
        }

        public static Vec2 operator * (Vec2 a, Vec2 b)
        {
            return new Vec2 (a.x * b.x, a.y * b.y);
        }

        public static Vec2 operator * (Vec2 a, float b)
        {
            return new Vec2 (a.x * b, a.y * b);
        }

        public static Vec2 operator * (float a, Vec2 b)
        {
            return new Vec2 (a * b.x, a * b.y);
        }

        public static Vec2 operator / (Vec2 a, Vec2 b)
        {
            return new Vec2 (
                Utils.Div (a.x, b.x),
                Utils.Div (a.y, b.y));
        }

        public static Vec2 operator / (Vec2 a, float b)
        {
            if (b == 0.0f) return new Vec2 (0.0f, 0.0f);
            return new Vec2 (a.x / b, a.y / b);
        }

        public static Vec2 operator / (float a, Vec2 b)
        {
            return new Vec2 (
                Utils.Div (a, b.x),
                Utils.Div (a, b.y));
        }

        public static Vec2 operator % (Vec2 a, Vec2 b)
        {
            return Vec2.Fmod (a, b);
        }

        public static Vec2 operator % (Vec2 a, float b)
        {
            if (b == 0.0f) return a;
            return new Vec2 (a.x % b, a.y % b);
        }

        public static Vec2 operator % (float a, Vec2 b)
        {
            return new Vec2 (
                Utils.Fmod (a, b.x),
                Utils.Fmod (a, b.y));
        }

        public static Vec2 operator + (Vec2 a, Vec2 b)
        {
            return new Vec2 (a.x + b.x, a.y + b.y);
        }

        public static Vec2 operator - (Vec2 a, Vec2 b)
        {
            return new Vec2 (a.x - b.x, a.y - b.y);
        }

        public static Vec2 Abs (Vec2 v)
        {
            return new Vec2 (
                Utils.Abs (v.x),
                Utils.Abs (v.y));
        }

        public static bool All (Vec2 v)
        {
            return v.x != 0.0f && v.y != 0.0f;
        }

        public static bool Any (Vec2 v)
        {
            return v.x != 0.0f || v.y != 0.0f;
        }

        public static bool Approx (Vec2 a, Vec2 b, float tolerance = Utils.Epsilon)
        {
            return Utils.Approx (a.x, b.x, tolerance) &&
                Utils.Approx (a.y, b.y, tolerance);
        }

        public static bool ApproxMag (
            Vec2 a,
            float b = 1.0f,
            float tolerance = Utils.Epsilon)
        {
            return Utils.Approx (Vec2.MagSq (a), b * b, tolerance);
        }

        public static bool AreParallel (Vec2 a, Vec2 b)
        {
            return a.x * b.y - a.y * b.x == 0.0f;
        }

        public static Vec2 BezierPoint (
            Vec2 ap0 = new Vec2 ( ),
            Vec2 cp0 = new Vec2 ( ),
            Vec2 cp1 = new Vec2 ( ),
            Vec2 ap1 = new Vec2 ( ),
            float step = 0.5f)
        {
            if (step <= 0.0f) return ap0;
            else if (step >= 1.0f) return ap1;

            float u = 1.0f - step;
            float tcb = step * step;
            float ucb = u * u;
            float usq3t = ucb * (step + step + step);
            float tsq3u = tcb * (u + u + u);
            ucb *= u;
            tcb *= step;

            return new Vec2 (
                ap0.x * ucb +
                cp0.x * usq3t +
                cp1.x * tsq3u +
                ap1.x * tcb,

                ap0.y * ucb +
                cp0.y * usq3t +
                cp1.y * tsq3u +
                ap1.y * tcb);
        }

        public static Vec2 BezierTangent (
            Vec2 ap0 = new Vec2 ( ),
            Vec2 cp0 = new Vec2 ( ),
            Vec2 cp1 = new Vec2 ( ),
            Vec2 ap1 = new Vec2 ( ),
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
                (cp0.x - ap0.x) * usq3 +
                (cp1.x - cp0.x) * ut6 +
                (ap1.x - cp1.x) * tsq3,

                (cp0.y - ap0.y) * usq3 +
                (cp1.y - cp0.y) * ut6 +
                (ap1.y - cp1.y) * tsq3);
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

        public static Vec2 Ceil (Vec2 v)
        {
            return new Vec2 (
                Utils.Ceil (v.x),
                Utils.Ceil (v.y));
        }

        public static Vec2 Clamp (Vec2 v, float lb = 0.0f, float ub = 1.0f)
        {
            return new Vec2 (
                Utils.Clamp (v.x, lb, ub),
                Utils.Clamp (v.y, lb, ub));
        }

        public static Vec2 Clamp (Vec2 v, Vec2 lb, Vec2 ub)
        {
            return new Vec2 (
                Utils.Clamp (v.x, lb.x, ub.x),
                Utils.Clamp (v.y, lb.y, ub.y));
        }

        public static Vec2 CopySign (Vec2 a, Vec2 b)
        {
            return new Vec2 (
                Utils.CopySign (a.x, b.x),
                Utils.CopySign (a.y, b.y));
        }

        public static Vec2 CopySign (float a, Vec2 b)
        {
            int x = Utils.Sign (a);
            return new Vec2 (
                Utils.Abs (b.x) * x,
                Utils.Abs (b.y) * x);
        }

        public static Vec2 CopySign (Vec2 a, float b)
        {
            int x = Utils.Sign (b);
            return new Vec2 (
                Utils.Abs (a.x) * x,
                Utils.Abs (a.y) * x);
        }

        public static Vec2 Diff (Vec2 a, Vec2 b)
        {
            return Vec2.Abs (a - b);
        }

        public static float DistChebyshev (Vec2 a, Vec2 b)
        {
            Vec2 v = Vec2.Diff (a, b);
            return Utils.Max (v.x, v.y);
        }

        public static float DistEuclidean (Vec2 a, Vec2 b)
        {
            return Vec2.Mag (b - a);
        }

        public static float DistManhattan (Vec2 a, Vec2 b)
        {
            Vec2 v = Vec2.Diff (a, b);
            return v.x + v.y;
        }

        public static float DistMinkowski (Vec2 a, Vec2 b, float c)
        {
            Vec2 v = Vec2.Pow (Vec2.Pow (Vec2.Diff (a, b), c), 1.0f / c);
            return v.x + v.y;
        }

        public static float DistMinkowski (Vec2 a, Vec2 b, Vec2 c)
        {
            Vec2 v = Vec2.Pow (Vec2.Pow (Vec2.Diff (a, b), c), 1.0f / c);
            return v.x + v.y;
        }

        public static float DistSq (Vec2 a, Vec2 b)
        {
            return Vec2.MagSq (b - a);
        }

        public static float Dot (Vec2 a, Vec2 b)
        {
            return a.x * b.x + a.y * b.y;
        }

        public static Vec2 Floor (Vec2 v)
        {
            return new Vec2 (
                Utils.Floor (v.x),
                Utils.Floor (v.y));
        }

        public static Vec2 Fmod (Vec2 a, Vec2 b)
        {
            return new Vec2 (
                Utils.Fmod (a.x, b.x),
                Utils.Fmod (a.y, b.y));
        }

        public static Vec2 Fract (Vec2 a)
        {
            return new Vec2 (
                a.x - (int) a.x,
                a.y - (int) a.y);
        }

        public static Vec2 FromPolar (
            float heading = 0.0f,
            float radius = 1.0f)
        {
            return new Vec2 (
                radius * Utils.Cos (heading),
                radius * Utils.Sin (heading));
        }

        public static Vec2[, ] grid (
            int rows,
            int cols,
            Vec2 lowerBound,
            Vec2 upperBound)
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
                    lowerBound.x,
                    upperBound.x,
                    j * jToStep);
            }

            Vec2[, ] result = new Vec2[rval, cval];
            for (int i = 0; i < rval; ++i)
            {

                float y = Utils.LerpUnclamped (
                    lowerBound.y,
                    upperBound.y,
                    i * iToStep);

                for (int j = 0; j < cval; ++j)
                {
                    result[i, j] = new Vec2 (xs[j], y);
                }
            }
            return result;
        }

        public static float HeadingSigned (Vec2 v)
        {
            return Utils.Atan2 (v.y, v.x);
        }

        public static float HeadingUnsigned (Vec2 v)
        {
            return Utils.ModRadians (Utils.Atan2 (v.y, v.x));
        }

        public static Vec2 Limit (Vec2 v, float limit = float.MaxValue)
        {
            float mSq = Vec2.MagSq (v);
            if (mSq > (limit * limit))
            {
                return Utils.Div (limit, Vec2.Mag (v)) * v;
            }
            return v;
        }

        public static float Mag (Vec2 v)
        {
            return Utils.Sqrt (v.x * v.x + v.y * v.y);
        }

        public static float MagSq (Vec2 v)
        {
            return v.x * v.x + v.y * v.y;
        }

        public static Vec2 Map (
            Vec2 v,
            Vec2 lbOrigin,
            Vec2 ubOrigin,
            Vec2 lbDest,
            Vec2 ubDest)
        {
            return new Vec2 (
                Utils.Map (v.x, lbOrigin.x, ubOrigin.x, lbDest.x, ubDest.x),
                Utils.Map (v.y, lbOrigin.y, ubOrigin.y, lbDest.y, ubDest.y));
        }

        public static Vec2 Max (Vec2 a, Vec2 b)
        {
            return new Vec2 (
                Utils.Max (a.x, b.x),
                Utils.Max (a.y, b.y));
        }

        public static Vec2 Min (Vec2 a, Vec2 b)
        {
            return new Vec2 (
                Utils.Min (a.x, b.x),
                Utils.Min (a.y, b.y));
        }

        public static Vec2 Mix (Vec2 a, Vec2 b, bool t)
        {
            return t ? b : a;
        }

        public static Vec2 Mix (Vec2 a, Vec2 b, float t = 0.5f)
        {
            float u = 1.0f - t;
            return new Vec2 (
                u * a.x + t * b.x,
                u * a.y + t * b.y);
        }

        public static Vec2 Mix (Vec2 a, Vec2 b, Vec2 t)
        {
            return new Vec2 (
                (1.0f - t.x) * a.x + t.x * b.x,
                (1.0f - t.y) * a.y + t.y * b.y);
        }

        public static Vec2 Mod (Vec2 a, Vec2 b)
        {
            return new Vec2 (
                Utils.Mod (a.x, b.x),
                Utils.Mod (a.y, b.y));
        }

        public static Vec2 Mod1 (Vec2 v)
        {
            return new Vec2 (
                Utils.Mod1 (v.x),
                Utils.Mod1 (v.y));
        }

        public static bool None (Vec2 v)
        {
            return v.x == 0.0f && v.y == 0.0f;
        }

        public static Vec2 Normalize (Vec2 v)
        {
            return v / Vec2.Mag (v);
        }

        public static Vec2 Not (Vec2 v)
        {
            return new Vec2 (
                v.x != 0.0f ? 0.0f : 1.0f,
                v.y != 0.0f ? 0.0f : 1.0f);
        }

        public static Vec2 PerpendicularCCW (Vec2 v)
        {
            return new Vec2 (-v.y, v.x);
        }

        public static Vec2 PerpendicularCW (Vec2 v)
        {
            return new Vec2 (v.y, -v.x);
        }

        public static Vec2 Pow (Vec2 a, Vec2 b)
        {
            return new Vec2 (
                Utils.Pow (a.x, b.x),
                Utils.Pow (a.y, b.y));
        }

        public static Vec2 Pow (Vec2 a, float b)
        {
            return new Vec2 (
                Utils.Pow (a.x, b),
                Utils.Pow (a.y, b));
        }

        public static Vec2 Pow (float a, Vec2 b)
        {
            return new Vec2 (
                Utils.Pow (a, b.x),
                Utils.Pow (a, b.y));
        }

        public static float ProjectScalar (Vec2 a, Vec2 b)
        {
            float bSq = Vec2.MagSq (b);
            if (bSq != 0.0f) return Vec2.Dot (a, b) / bSq;
            return 0.0f;
        }

        public static Vec2 ProjectVector (Vec2 a, Vec2 b)
        {
            return b * Vec2.ProjectScalar (a, b);
        }

        public static Vec2 Reflect (Vec2 i, Vec2 n)
        {
            return i - ((2.0f * Vec2.Dot (n, i)) * n);
        }

        public static Vec2 Refract (Vec2 i, Vec2 n, float eta)
        {
            float iDotN = Vec2.Dot (i, n);
            float k = 1.0f - eta * eta * (1.0f - iDotN * iDotN);
            if (k < 0.0f) return new Vec2 (0.0f, 0.0f);
            return (eta * i) -
                (n * (eta * iDotN + Utils.Sqrt (k)));
        }

        public static Vec2 Rescale (Vec2 v, float scalar = 1.0f)
        {
            return Utils.Div (scalar, Vec2.Mag (v)) * v;
        }

        public static Vec2 RotateZ (Vec2 v, float radians = 0.0f)
        {
            return RotateZ (v, Utils.Cos (radians), Utils.Sin (radians));
        }

        public static Vec2 RotateZ (
            Vec2 v,
            float cosa = 1.0f,
            float sina = 0.0f)
        {
            return new Vec2 (
                cosa * v.x - sina * v.y,
                cosa * v.y + sina * v.x);
        }

        public static Vec2 Round (Vec2 v)
        {
            return new Vec2 (
                Utils.Round (v.x),
                Utils.Round (v.y));
        }

        public static Vec2 Sign (Vec2 v)
        {
            return new Vec2 (
                Utils.Sign (v.x),
                Utils.Sign (v.y));
        }

        public static Vec2 Trunc (Vec2 v)
        {
            return new Vec2 ((int) v.x, (int) v.y);
        }

        public static Vec2 Back
        {
            get
            {
                return new Vec2 (0.0f, 1.0f);
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