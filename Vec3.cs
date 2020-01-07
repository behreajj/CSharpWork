using System;
using System.Collections;
using System.Text;

[Serializable]
public readonly struct Vec3 : IComparable<Vec3>, IEquatable<Vec3>, IEnumerable
{
    private readonly float _x;
    private readonly float _y;
    private readonly float _z;

    public float x { get { return this._x; } }
    public float y { get { return this._y; } }
    public float z { get { return this._z; } }

    public float this [int i]
    {
        get
        {
            switch (i)
            {
                case 0:
                case -3:
                    return this._x;
                case 1:
                case -2:
                    return this._y;
                case 2:
                case -1:
                    return this._z;
                default:
                    return 0.0f;
            }
        }
    }

    public Vec3 (float x = 0.0f, float y = 0.0f, float z = 0.0f)
    {
        this._x = x;
        this._y = y;
        this._z = z;
    }

    public Vec3 (bool x = false, bool y = false, bool z = false)
    {
        this._x = x ? 1.0f : 0.0f;
        this._y = y ? 1.0f : 0.0f;
        this._z = z ? 1.0f : 0.0f;
    }

    public override bool Equals (object value)
    {
        if (Object.ReferenceEquals (this, value)) return true;
        if (Object.ReferenceEquals (null, value)) return false;

        if (value is Vec3)
        {
            Vec3 v = (Vec3) value;

            // return Vec3.Approx (this, v);

            if (this._z.GetHashCode ( ) != v._z.GetHashCode ( ))
            {
                return false;
            }

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
            int hash = Utils.HashBase;
            hash = hash * Utils.HashMul ^ this._x.GetHashCode ( );
            hash = hash * Utils.HashMul ^ this._y.GetHashCode ( );
            hash = hash * Utils.HashMul ^ this._z.GetHashCode ( );
            return hash;
        }
    }

    public override string ToString ( )
    {
        return ToString (4);
    }

    public int CompareTo (Vec3 v)
    {
        return (this._z > v._z) ? 1 :
            (this._z < v._z) ? -1 :
            (this._y > v._y) ? 1 :
            (this._y < v._y) ? -1 :
            (this._x > v._x) ? 1 :
            (this._x < v._x) ? -1 :
            0;
    }

    public bool Equals (Vec3 v)
    {
        // return Vec3.Approx (this, v);

        if (this._z.GetHashCode ( ) != v._z.GetHashCode ( ))
        {
            return false;
        }

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
        yield return this._z;
    }

    public float[ ] ToArray ( )
    {
        return new float[ ] { this._x, this._y, this._z };
    }

    public string ToString (int places = 4)
    {
        return new StringBuilder (80)
            .Append ("{ x: ")
            .Append (Utils.ToFixed (this._x, places))
            .Append (", y: ")
            .Append (Utils.ToFixed (this._y, places))
            .Append (", z: ")
            .Append (Utils.ToFixed (this._z, places))
            .Append (" }")
            .ToString ( );
    }

    public (float, float, float) ToTuple ( )
    {
        return (x: this._x, y: this._y, z: this._z);
    }

    public static implicit operator Vec3 (bool b)
    {
        float eval = b ? 1.0f : 0.0f;
        return new Vec3 (eval, eval, eval);
    }

    public static implicit operator Vec3 (float s)
    {
        return new Vec3 (s, s, s);
    }

    public static implicit operator Vec3 (in Vec2 v)
    {
        return new Vec3 (v.x, v.y, 0.0f);
    }

    public static explicit operator bool (in Vec3 v)
    {
        return Vec3.All (v);
    }

    public static explicit operator float (in Vec3 v)
    {
        return Vec3.Mag (v);
    }

    public static bool operator true (in Vec3 v)
    {
        return Vec3.All (v);
    }

    public static bool operator false (in Vec3 v)
    {
        return Vec3.None (v);
    }

    public static Vec3 operator ! (in Vec3 v)
    {
        return Vec3.Not (v);
    }

    public static Vec3 operator ~ (in Vec3 v)
    {
        return Vec3.Not (v);
    }

    public static Vec3 operator & (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (
            Utils.And (a._x, b._x),
            Utils.And (a._y, b._y),
            Utils.And (a._z, b._z));
    }

    public static Vec3 operator | (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (
            Utils.Or (a._x, b._x),
            Utils.Or (a._y, b._y),
            Utils.Or (a._z, b._z));
    }

    public static Vec3 operator ^ (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (
            Utils.Xor (a._x, b._x),
            Utils.Xor (a._y, b._y),
            Utils.Xor (a._z, b._z));
    }

    public static Vec3 operator - (in Vec3 v)
    {
        return new Vec3 (-v._x, -v._y, -v._z);
    }

    public static Vec3 operator * (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (a._x * b._x, a._y * b._y, a._z * b._z);
    }

    public static Vec3 operator * (in Vec3 a, float b)
    {
        return new Vec3 (a._x * b, a._y * b, a._z * b);
    }

    public static Vec3 operator * (float a, in Vec3 b)
    {
        return new Vec3 (a * b._x, a * b._y, a * b._z);
    }

    public static Vec3 operator / (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (
            Utils.Div (a._x, b._x),
            Utils.Div (a._y, b._y),
            Utils.Div (a._z, b._z));
    }

    public static Vec3 operator / (in Vec3 a, float b)
    {
        if (b == 0.0f) return new Vec3 ( );
        float bInv = 1.0f / b;
        return new Vec3 (
            a._x * bInv,
            a._y * bInv,
            a._z * bInv);
    }

    public static Vec3 operator / (float a, in Vec3 b)
    {
        return new Vec3 (
            Utils.Div (a, b._x),
            Utils.Div (a, b._y),
            Utils.Div (a, b._z));
    }

    public static Vec3 operator % (in Vec3 a, in Vec3 b)
    {
        return Vec3.Fmod (a, b);
    }

    public static Vec3 operator % (in Vec3 a, float b)
    {
        if (b == 0.0f) return a;
        return new Vec3 (a._x % b, a._y % b, a._z % b);
    }

    public static Vec3 operator % (float a, in Vec3 b)
    {
        return new Vec3 (
            Utils.Fmod (a, b._x),
            Utils.Fmod (a, b._y),
            Utils.Fmod (a, b._z));
    }

    public static Vec3 operator + (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (a._x + b._x, a._y + b._y, a._z + b._z);
    }

    public static Vec3 operator - (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (a._x - b._x, a._y - b._y, a._z - b._z);
    }

    public static Vec3 operator < (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (a._x < b._x, a._y < b._y, a._z < b._z);
    }

    public static Vec3 operator > (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (a._x > b._x, a._y > b._y, a._z > b._z);
    }

    public static Vec3 operator <= (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (a._x <= b._x, a._y <= b._y, a._z <= b._z);
    }

    public static Vec3 operator >= (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (a._x >= b._x, a._y >= b._y, a._z >= b._z);
    }

    public static Vec3 operator != (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (a._x != b._x, a._y != b._y, a._z != b._z);
    }

    public static Vec3 operator == (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (a._x == b._x, a._y == b._y, a._z == b._z);
    }

    public static Vec3 Abs (in Vec3 v)
    {
        return new Vec3 (
            Utils.Abs (v._x),
            Utils.Abs (v._y),
            Utils.Abs (v._z));
    }

    public static bool All (in Vec3 v)
    {
        return v._x != 0.0f &&
            v._y != 0.0f &&
            v._z != 0.0f;
    }

    public static bool Any (in Vec3 v)
    {
        return v._x != 0.0f ||
            v._y != 0.0f ||
            v._z != 0.0f;
    }

    public static bool Approx (in Vec3 a, in Vec3 b, float tolerance = Utils.Epsilon)
    {
        return Utils.Approx (a._x, b._x, tolerance) &&
            Utils.Approx (a._y, b._y, tolerance) &&
            Utils.Approx (a._z, b._z, tolerance);
    }

    public static bool ApproxMag (in Vec3 a,
        float b = 1.0f,
        float tolerance = Utils.Epsilon)
    {
        return Utils.Approx (Vec3.MagSq (a), b * b, tolerance);
    }

    public static float AzimuthSigned (in Vec3 v)
    {
        return Utils.Atan2 (v._y, v._x);
    }

    public static float AzimuthUnsigned (in Vec3 v)
    {
        return Utils.ModRadians (Utils.Atan2 (v._y, v._x));
    }

    public static Vec3 Ceil (in Vec3 v)
    {
        return new Vec3 (
            Utils.Ceil (v._x),
            Utils.Ceil (v._y),
            Utils.Ceil (v._z));
    }

    public static Vec3 Clamp (in Vec3 v, float lb = 0.0f, float ub = 1.0f)
    {
        return new Vec3 (
            Utils.Clamp (v._x, lb, ub),
            Utils.Clamp (v._y, lb, ub),
            Utils.Clamp (v._z, lb, ub));
    }

    public static Vec3 Clamp (in Vec3 v, in Vec3 lb, in Vec3 ub)
    {
        return new Vec3 (
            Utils.Clamp (v._x, lb._x, ub._x),
            Utils.Clamp (v._y, lb._y, ub._y),
            Utils.Clamp (v._z, lb._z, ub._z));
    }

    public static Vec3 CopySign (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (
            Utils.CopySign (a._x, b._x),
            Utils.CopySign (a._y, b._y),
            Utils.CopySign (a._z, b._z));
    }

    public static Vec3 CopySign (float a, in Vec3 b)
    {
        int n = Utils.Sign (a);
        return new Vec3 (
            Utils.Abs (b._x) * n,
            Utils.Abs (b._y) * n,
            Utils.Abs (b._z) * n);
    }

    public static Vec3 CopySign (in Vec3 a, float b)
    {
        int n = Utils.Sign (b);
        return new Vec3 (
            Utils.Abs (a._x) * n,
            Utils.Abs (a._y) * n,
            Utils.Abs (a._z) * n);
    }

    public static Vec3 Cross (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (
            a._y * b._z - a._z * b._y,
            a._z * b._x - a._x * b._z,
            a._x * b._y - a._y * b._x);
    }

    public static Vec3 Diff (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (
            Utils.Diff (b._x, a._x),
            Utils.Diff (b._y, a._y),
            Utils.Diff (b._z, a._z));
    }

    public static float Dot (in Vec3 a, in Vec3 b)
    {
        return a._x * b._x + a._y * b._y + a._z * b._z;
    }

    public static Vec3 Floor (in Vec3 v)
    {
        return new Vec3 (
            Utils.Floor (v._x),
            Utils.Floor (v._y),
            Utils.Floor (v._z));
    }

    public static Vec3 Fmod (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (
            Utils.Fmod (a._x, b._x),
            Utils.Fmod (a._y, b._y),
            Utils.Fmod (a._z, b._z));
    }

    public static Vec3 Fract (in Vec3 v)
    {
        return new Vec3 (
            v._x - (int) v._x,
            v._y - (int) v._y,
            v._z - (int) v._z);
    }

    public static Vec3 FromPolar (
        float heading = 0.0f,
        float radius = 1.0f)
    {
        return new Vec3 (
            radius * Utils.Cos (heading),
            radius * Utils.Sin (heading), 0.0f);
    }

    public static Vec3 FromSpherical (
        float azimuth = 0.0f,
        float inclination = 0.0f,
        float radius = 1.0f)
    {
        float rcp = radius * Utils.Cos (inclination);
        return new Vec3 (
            rcp * Utils.Cos (azimuth),
            rcp * Utils.Sin (azimuth),
            radius * -Utils.Sin (inclination));
    }

    public static Vec3[, , ] Grid (
        int rows,
        int cols,
        int layers, in Vec3 lowerBound, in Vec3 upperBound)
    {

        int rval = rows < 3 ? 3 : rows;
        int cval = cols < 3 ? 3 : cols;
        int lval = layers < 3 ? 3 : layers;

        float hToStep = 1.0f / (lval - 1.0f);
        float iToStep = 1.0f / (rval - 1.0f);
        float jToStep = 1.0f / (cval - 1.0f);

        /* Calculate x values in separate loop. */
        float[ ] xs = new float[cval];
        for (int j = 0; j < cval; ++j)
        {
            xs[j] = Utils.Mix (
                lowerBound.x,
                upperBound.x,
                j * jToStep);
        }

        /* Calculate y values in separate loop. */
        float[ ] ys = new float[rval];
        for (int i = 0; i < rval; ++i)
        {
            ys[i] = Utils.Mix (
                lowerBound.y,
                upperBound.y,
                i * iToStep);
        }

        Vec3[, , ] result = new Vec3[lval, rval, cval];
        for (int h = 0; h < lval; ++h)
        {
            float z = Utils.Mix (
                lowerBound.z,
                upperBound.z,
                h * hToStep);

            for (int i = 0; i < rval; ++i)
            {
                float y = ys[i];
                for (int j = 0; j < cval; ++j)
                {
                    result[h, i, j] = new Vec3 (xs[j], y, z);
                }
            }
        }
        return result;
    }

    public static float InclinationSigned (in Vec3 v)
    {
        float mSq = Vec3.MagSq (v);
        if (mSq == 0.0f)
        {
            return 0.0f;
        }
        return Utils.Asin (v._z / Utils.Sqrt (mSq));
    }

    public static float InclinationUnsigned (in Vec3 v)
    {
        return Utils.ModRadians (Vec3.InclinationSigned (v));
    }

    public static bool IsUnit (in Vec3 v)
    {
        return Utils.Approx (Vec3.MagSq (v), 1.0f);
    }

    public static Vec3 Limit (in Vec3 v, float limit = float.MaxValue)
    {
        float mSq = Vec3.MagSq (v);
        if (mSq > (limit * limit))
        {
            return Utils.Div (limit, Vec3.Mag (v)) * v;
        }
        return v;
    }

    public static Vec3 LinearStep (in Vec3 edge0, in Vec3 edge1, in Vec3 x)
    {
        return Vec3.Clamp ((x - edge0) / (edge1 - edge0));
    }

    public static float Mag (in Vec3 v)
    {
        return Utils.Sqrt (
            v._x * v._x +
            v._y * v._y +
            v._z * v._z);
    }

    public static float MagSq (in Vec3 v)
    {
        return v._x * v._x + v._y * v._y + v._z * v._z;
    }

    public static Vec3 Map (in Vec3 v, in Vec3 lbOrigin, in Vec3 ubOrigin, in Vec3 lbDest, in Vec3 ubDest)
    {
        return new Vec3 (
            Utils.Map (v._x, lbOrigin._x, ubOrigin._x, lbDest._x, ubDest._x),
            Utils.Map (v._y, lbOrigin._y, ubOrigin._y, lbDest._y, ubDest._y),
            Utils.Map (v._z, lbOrigin._z, ubOrigin._z, lbDest._z, ubDest._z));
    }

    public static Vec3 Max (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (
            Utils.Max (a._x, b._x),
            Utils.Max (a._y, b._y),
            Utils.Max (a._z, b._z));
    }

    public static Vec3 Min (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (
            Utils.Min (a._x, b._x),
            Utils.Min (a._y, b._y),
            Utils.Min (a._z, b._z));
    }

    public static Vec3 Mix (in Vec3 a, in Vec3 b, float t = 0.5f)
    {
        float u = 1.0f - t;
        return new Vec3 (
            u * a._x + t * b._x,
            u * a._y + t * b._y,
            u * a._z + t * b._z);
    }

    public static Vec3 Mix (in Vec3 a, in Vec3 b, in Vec3 t)
    {
        return new Vec3 (
            (1.0f - t._x) * a._x + t._x * b._x,
            (1.0f - t._y) * a._y + t._y * b._y,
            (1.0f - t._z) * a._z + t._z * b._z);
    }

    public static Vec3 Mod (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (
            Utils.Mod (a._x, b._x),
            Utils.Mod (a._y, b._y),
            Utils.Mod (a._z, b._z));
    }

    public static Vec3 Mod1 (in Vec3 v)
    {
        return new Vec3 (
            Utils.Mod1 (v._x),
            Utils.Mod1 (v._y),
            Utils.Mod1 (v._z));
    }

    public static bool None (in Vec3 v)
    {
        return v._x == 0.0f &&
            v._y == 0.0f &&
            v._z == 0.0f;
    }

    public static Vec3 Normalize (in Vec3 v)
    {
        return v / Vec3.Mag (v);
    }

    public static Vec3 Not (in Vec3 v)
    {
        return new Vec3 (
            v._x != 0.0f ? 0.0f : 1.0f,
            v._y != 0.0f ? 0.0f : 1.0f,
            v._z != 0.0f ? 0.0f : 1.0f);
    }

    public static Vec3 Pow (in Vec3 a, in Vec3 b)
    {
        return new Vec3 (
            Utils.Pow (a._x, b._x),
            Utils.Pow (a._y, b._y),
            Utils.Pow (a._z, b._z));
    }

    public static Vec3 Pow (in Vec3 a, float b)
    {
        return new Vec3 (
            Utils.Pow (a._x, b),
            Utils.Pow (a._y, b),
            Utils.Pow (a._z, b));
    }

    public static Vec3 Pow (float a, in Vec3 b)
    {
        return new Vec3 (
            Utils.Pow (a, b._x),
            Utils.Pow (a, b._y),
            Utils.Pow (a, b._z));
    }

    public static float ProjectScalar (in Vec3 a, in Vec3 b)
    {
        float bSq = Vec3.MagSq (b);
        if (bSq != 0.0f) return Vec3.Dot (a, b) / bSq;
        return 0.0f;
    }

    public static Vec3 ProjectVector (in Vec3 a, in Vec3 b)
    {
        return b * Vec3.ProjectScalar (a, b);
    }

    public static Vec3 RandomCartesian (in Random rng, in Vec3 lb, in Vec3 ub)
    {
        float xFac = (float) rng.NextDouble ( );
        float yFac = (float) rng.NextDouble ( );
        float zFac = (float) rng.NextDouble ( );

        return new Vec3 (
            Utils.Mix (lb._x, ub._x, xFac),
            Utils.Mix (lb._y, ub._y, yFac),
            Utils.Mix (lb._z, ub._z, zFac));
    }

    public static Vec3 RandomCartesian (in Random rng, float lb = 0.0f, float ub = 1.0f)
    {
        float xFac = (float) rng.NextDouble ( );
        float yFac = (float) rng.NextDouble ( );
        float zFac = (float) rng.NextDouble ( );

        return new Vec3 (
            Utils.Mix (lb, ub, xFac),
            Utils.Mix (lb, ub, yFac),
            Utils.Mix (lb, ub, zFac));
    }

    public static Vec3 RandomPolar (in Random rng)
    {
        return Vec3.FromPolar (
            Utils.Mix (-Utils.Pi, Utils.Pi, (float) rng.NextDouble ( )), 1.0f);
    }

    public static Vec3 RandomSpherical (in Random rng)
    {
        return Vec3.FromSpherical (
            Utils.Mix (-Utils.Pi, Utils.Pi, (float) rng.NextDouble ( )),
            Utils.Mix (-Utils.HalfPi, Utils.HalfPi, (float) rng.NextDouble ( )),
            1.0f);
    }

    public static Vec3 Reflect (in Vec3 i, in Vec3 n)
    {
        return i - ((2.0f * Vec3.Dot (n, i)) * n);
    }

    public static Vec3 Refract (in Vec3 i, in Vec3 n, float eta)
    {
        float iDotN = Vec3.Dot (i, n);
        float k = 1.0f - eta * eta * (1.0f - iDotN * iDotN);
        if (k < 0.0f) return new Vec3 ( );
        return (eta * i) -
            (n * (eta * iDotN + Utils.Sqrt (k)));
    }

    public static Vec3 Rescale (in Vec3 v, float scalar = 1.0f)
    {
        return Utils.Div (scalar, Vec3.Mag (v)) * v;
    }

    public static Vec3 Rotate (in Vec3 v, float radians, in Vec3 axis)
    {
        return Vec3.Rotate (v, Utils.Cos (radians), Utils.Sin (radians), axis);
    }

    public static Vec3 Rotate (in Vec3 v, float cosa, float sina, in Vec3 axis)
    {
        float complcos = 1.0f - cosa;
        float complxy = complcos * axis._x * axis._y;
        float complxz = complcos * axis._x * axis._z;
        float complyz = complcos * axis._y * axis._z;

        float sinx = sina * axis._x;
        float siny = sina * axis._y;
        float sinz = sina * axis._z;

        return new Vec3 (
            (complcos * axis._x * axis._x + cosa) * v._x +
            (complxy - sinz) * v._y +
            (complxz + siny) * v._z,

            (complxy + sinz) * v._x +
            (complcos * axis._y * axis._y + cosa) * v._y +
            (complyz - sinx) * v._z,

            (complxz - siny) * v._x +
            (complyz + sinx) * v._y +
            (complcos * axis._z * axis._z + cosa) * v._z);
    }

    public static Vec3 RotateX (in Vec3 v, float radians = 0.0f)
    {
        return Vec3.RotateX (v, Utils.Cos (radians), Utils.Sin (radians));
    }

    public static Vec3 RotateX (in Vec3 v,
        float cosa = 1.0f,
        float sina = 0.0f)
    {
        return new Vec3 (
            v._x,
            cosa * v._y - sina * v._z,
            cosa * v._z + sina * v._y);
    }

    public static Vec3 RotateY (in Vec3 v, float radians = 0.0f)
    {
        return Vec3.RotateY (v, Utils.Cos (radians), Utils.Sin (radians));
    }

    public static Vec3 RotateY (in Vec3 v,
        float cosa = 1.0f,
        float sina = 0.0f)
    {
        return new Vec3 (
            cosa * v._x + sina * v._z,
            v._y,
            cosa * v._z - sina * v._x);
    }

    public static Vec3 RotateZ (in Vec3 v, float radians = 0.0f)
    {
        return Vec3.RotateZ (v, Utils.Cos (radians), Utils.Sin (radians));
    }

    public static Vec3 RotateZ (in Vec3 v,
        float cosa = 1.0f,
        float sina = 0.0f)
    {
        return new Vec3 (
            cosa * v._x - sina * v._y,
            cosa * v._y + sina * v._x,
            v._z);
    }

    public static Vec3 Round (in Vec3 v)
    {
        return new Vec3 (
            Utils.Round (v._x),
            Utils.Round (v._y),
            Utils.Round (v._z));
    }

    public static Vec3 SmoothStep (in Vec3 edge0, in Vec3 edge1, in Vec3 x)
    {
        Vec3 t = Vec3.Clamp ((x - edge0) / (edge1 - edge0));
        return t * t * (new Vec3 (3.0f, 3.0f, 3.0f) - (t + t));
    }

    public static Vec3 Sign (in Vec3 v)
    {
        return new Vec3 (
            Utils.Sign (v._x),
            Utils.Sign (v._y),
            Utils.Sign (v._z));
    }

    public static Vec3 Step (in Vec3 edge, in Vec3 x)
    {
        return new Vec3 (
            x._x < edge._x ? 0.0f : 1.0f,
            x._y < edge._y ? 0.0f : 1.0f,
            x._z < edge._z ? 0.0f : 1.0f);
    }

    public static (float, float, float) ToSpherical (in Vec3 v)
    {
        float mSq = Vec3.MagSq (v);
        if (mSq == 0.0f)
        {
            return (0.0f, 0.0f, 0.0f);
        }
        float m = Utils.Sqrt (mSq);
        return (Vec3.AzimuthSigned (v), v._z / m, m);
    }

    public static Vec3 Trunc (in Vec3 v)
    {
        return new Vec3 ((int) v._x, (int) v._y, (int) v._z);
    }

    public static Vec3 Back
    {
        get
        {
            return new Vec3 (0.0f, -1.0f, 0.0f);
        }
    }

    public static Vec3 Down
    {
        get
        {
            return new Vec3 (0.0f, 0.0f, -1.0f);
        }
    }

    public static Vec3 Forward
    {
        get
        {
            return new Vec3 (0.0f, 1.0f, 0.0f);
        }
    }

    public static Vec3 Left
    {
        get
        {
            return new Vec3 (-1.0f, 0.0f, 0.0f);
        }
    }

    public static Vec3 Right
    {
        get
        {
            return new Vec3 (1.0f, 0.0f, 0.0f);
        }
    }

    public static Vec3 Up
    {
        get
        {
            return new Vec3 (0.0f, 0.0f, 1.0f);
        }
    }
}