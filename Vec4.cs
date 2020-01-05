using System;
using System.Collections;
using System.Text;

[Serializable]
public readonly struct Vec4 : IComparable<Vec4>, IEquatable<Vec4>
{
    private readonly float _x;
    private readonly float _y;
    private readonly float _z;
    private readonly float _w;

    public float x { get { return this._x; } }
    public float y { get { return this._y; } }
    public float z { get { return this._z; } }
    public float w { get { return this._w; } }

    public float this [int i]
    {
        get
        {
            switch (i)
            {
                case 0:
                case -4:
                    return this._x;
                case 1:
                case -3:
                    return this._y;
                case 2:
                case -2:
                    return this._z;
                case 3:
                case -1:
                    return this._w;
                default:
                    return 0.0f;
            }
        }
    }

    public Vec4 (float x = 0.0f, float y = 0.0f, float z = 0.0f, float w = 0.0f)
    {
        this._x = x;
        this._y = y;
        this._z = z;
        this._w = w;
    }

    public Vec4 (bool x = false, bool y = false, bool z = false, bool w = false)
    {
        this._x = x ? 1.0f : 0.0f;
        this._y = y ? 1.0f : 0.0f;
        this._z = z ? 1.0f : 0.0f;
        this._w = w ? 1.0f : 0.0f;
    }

    public override bool Equals (object value)
    {
        if (Object.ReferenceEquals (this, value))
        {
            return true;
        }

        if (Object.ReferenceEquals (null, value))
        {
            return false;
        }

        if (value is Vec4)
        {
            Vec4 v = (Vec4) value;

            // return Vec4.Approx (this, v);

            if (this._w.GetHashCode ( ) != v._w.GetHashCode ( ))
            {
                return false;
            }

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
            const int hashBase = -2128831035;
            const int hashMul = 16777619;
            int hash = hashBase;
            hash = hash * hashMul ^ this._x.GetHashCode ( );
            hash = hash * hashMul ^ this._y.GetHashCode ( );
            hash = hash * hashMul ^ this._z.GetHashCode ( );
            hash = hash * hashMul ^ this._w.GetHashCode ( );
            return hash;
        }
    }

    public override string ToString ( )
    {
        return ToString (4);
    }

    public int CompareTo (Vec4 v)
    {
        return (this._w > v._w) ? 1 :
            (this._w < v._w) ? -1 :
            (this._z > v._z) ? 1 :
            (this._z < v._z) ? -1 :
            (this._y > v._y) ? 1 :
            (this._y < v._y) ? -1 :
            (this._x > v._x) ? 1 :
            (this._x < v._x) ? -1 :
            0;
    }

    public bool Equals (Vec4 v)
    {
        // return Vec4.Approx (this, v);

        if (this._w.GetHashCode ( ) != v._w.GetHashCode ( ))
        {
            return false;
        }

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
        yield return this._w;
    }

    public float[ ] ToArray ( )
    {
        return new float[ ] { this._x, this._y, this._z, this._w };
    }

    public string ToString (int places = 4)
    {
        return new StringBuilder (96)
            .Append ("{ x: ")
            .Append (Utils.ToFixed (this._x, places))
            .Append (", y: ")
            .Append (Utils.ToFixed (this._y, places))
            .Append (", z: ")
            .Append (Utils.ToFixed (this._z, places))
            .Append (", w: ")
            .Append (Utils.ToFixed (this._w, places))
            .Append (" }")
            .ToString ( );
    }

    public static implicit operator Vec4 (bool b)
    {
        float eval = b ? 1.0f : 0.0f;
        return new Vec4 (eval, eval, eval, eval);
    }

    public static implicit operator Vec4 (float s)
    {
        return new Vec4 (s, s, s, s);
    }

    public static implicit operator Vec4 (in Vec2 v)
    {
        return new Vec4 (v.x, v.y, 0.0f, 0.0f);
    }

    public static implicit operator Vec4 (in Vec3 v)
    {
        return new Vec4 (v.x, v.y, v.z, 0.0f);
    }

    public static explicit operator bool (in Vec4 v)
    {
        return Vec4.All (v);
    }

    public static explicit operator float (in Vec4 v)
    {
        return Vec4.Mag (v);
    }

    public static bool operator true (in Vec4 v)
    {
        return Vec4.All (v);
    }

    public static bool operator false (in Vec4 v)
    {
        return Vec4.None (v);
    }

    public static Vec4 operator ! (in Vec4 v)
    {
        return Vec4.Not (v);
    }

    public static Vec4 operator ~ (in Vec4 v)
    {
        return Vec4.Not (v);
    }

    public static Vec4 operator & (in Vec4 a, in Vec4 b)
    {
        return new Vec4 (
            Utils.And (a._x, b._x),
            Utils.And (a._y, b._y),
            Utils.And (a._z, b._z),
            Utils.And (a._w, b._w));
    }

    public static Vec4 operator | (in Vec4 a, in Vec4 b)
    {
        return new Vec4 (
            Utils.Or (a._x, b._x),
            Utils.Or (a._y, b._y),
            Utils.Or (a._z, b._z),
            Utils.Or (a._w, b._w));
    }

    public static Vec4 operator ^ (in Vec4 a, in Vec4 b)
    {
        return new Vec4 (
            Utils.Xor (a._x, b._x),
            Utils.Xor (a._y, b._y),
            Utils.Xor (a._z, b._z),
            Utils.Xor (a._w, b._w));
    }

    public static Vec4 operator - (in Vec4 v)
    {
        return new Vec4 (-v._x, -v._y, -v._z, -v._w);
    }

    public static Vec4 operator * (in Vec4 a, in Vec4 b)
    {
        return new Vec4 (
            a._x * b._x,
            a._y * b._y,
            a._z * b._z,
            a._w * b._w);
    }

    public static Vec4 operator * (Vec4 a, float b)
    {
        return new Vec4 (
            a._x * b,
            a._y * b,
            a._z * b,
            a._w * b);
    }

    public static Vec4 operator * (float a, Vec4 b)
    {
        return new Vec4 (
            a * b._x,
            a * b._y,
            a * b._z,
            a * b._w);
    }

    public static Vec4 operator / (in Vec4 a, in Vec4 b)
    {
        return new Vec4 (
            Utils.Div (a._x, b._x),
            Utils.Div (a._y, b._y),
            Utils.Div (a._z, b._z),
            Utils.Div (a._w, b._w));
    }

    public static Vec4 operator / (Vec4 a, float b)
    {
        if (b == 0.0f) return new Vec4 (0.0f, 0.0f, 0.0f, 0.0f);
        float bInv = 1.0f / b;
        return new Vec4 (
            a._x * bInv,
            a._y * bInv,
            a._z * bInv,
            a._w * bInv);
    }

    public static Vec4 operator / (float a, Vec4 b)
    {
        return new Vec4 (
            Utils.Div (a, b._x),
            Utils.Div (a, b._y),
            Utils.Div (a, b._z),
            Utils.Div (a, b._w));
    }

    public static Vec4 operator % (in Vec4 a, in Vec4 b)
    {
        return Vec4.Fmod (a, b);
    }

    public static Vec4 operator % (Vec4 a, float b)
    {
        if (b == 0.0f) return a;
        return new Vec4 (
            a._x % b,
            a._y % b,
            a._z % b,
            a._w % b);
    }

    public static Vec4 operator % (float a, Vec4 b)
    {
        return new Vec4 (
            Utils.Fmod (a, b._x),
            Utils.Fmod (a, b._y),
            Utils.Fmod (a, b._z),
            Utils.Fmod (a, b._w));
    }

    public static Vec4 operator + (in Vec4 a, in Vec4 b)
    {
        return new Vec4 (
            a._x + b._x,
            a._y + b._y,
            a._z + b._z,
            a._w + b._w);
    }

    public static Vec4 operator - (in Vec4 a, in Vec4 b)
    {
        return new Vec4 (
            a._x - b._x,
            a._y - b._y,
            a._z - b._z,
            a._w - b._w);
    }

    public static Vec4 operator < (in Vec4 a, in Vec4 b)
    {
        return new Vec4 (a._x < b._x, a._y < b._y, a._z < b._z, a._w < b._w);
    }

    public static Vec4 operator > (in Vec4 a, in Vec4 b)
    {
        return new Vec4 (a._x > b._x, a._y > b._y, a._z > b._z, a._w > b._w);
    }

    public static Vec4 operator <= (in Vec4 a, in Vec4 b)
    {
        return new Vec4 (a._x <= b._x, a._y <= b._y, a._z <= b._z, a._w <= b._w);
    }

    public static Vec4 operator >= (in Vec4 a, in Vec4 b)
    {
        return new Vec4 (a._x >= b._x, a._y >= b._y, a._z >= b._z, a._w >= b._w);
    }

    public static Vec4 operator != (in Vec4 a, in Vec4 b)
    {
        return new Vec4 (a._x != b._x, a._y != b._y, a._z != b._z, a._w != b._w);
    }

    public static Vec4 operator == (in Vec4 a, in Vec4 b)
    {
        return new Vec4 (a._x == b._x, a._y == b._y, a._z == b._z, a._w == b._w);
    }

    public static Vec4 Abs (in Vec4 v)
    {
        return new Vec4 (
            Utils.Abs (v._x),
            Utils.Abs (v._y),
            Utils.Abs (v._z),
            Utils.Abs (v._w));
    }

    public static bool All (in Vec4 v)
    {
        return v._x != 0.0f &&
            v._y != 0.0f &&
            v._z != 0.0f &&
            v._w != 0.0f;
    }

    public static bool Any (in Vec4 v)
    {
        return v._x != 0.0f ||
            v._y != 0.0f ||
            v._z != 0.0f ||
            v._w != 0.0f;
    }

    public static bool Approx (Vec4 a, Vec4 b, float tolerance = Utils.Epsilon)
    {
        return Utils.Approx (a._x, b._x, tolerance) &&
            Utils.Approx (a._y, b._y, tolerance) &&
            Utils.Approx (a._z, b._z, tolerance) &&
            Utils.Approx (a._w, b._w, tolerance);
    }

    public static Vec4 Ceil (in Vec4 v)
    {
        return new Vec4 (
            Utils.Ceil (v._x),
            Utils.Ceil (v._y),
            Utils.Ceil (v._z),
            Utils.Ceil (v._w));
    }

    public static Vec4 Clamp (Vec4 v, float lb = 0.0f, float ub = 1.0f)
    {
        return new Vec4 (
            Utils.Clamp (v._x, lb, ub),
            Utils.Clamp (v._y, lb, ub),
            Utils.Clamp (v._z, lb, ub),
            Utils.Clamp (v._w, lb, ub));
    }

    public static Vec4 Clamp (Vec4 v, Vec4 lb, Vec4 ub)
    {
        return new Vec4 (
            Utils.Clamp (v._x, lb._x, ub._x),
            Utils.Clamp (v._y, lb._y, ub._y),
            Utils.Clamp (v._z, lb._z, ub._z),
            Utils.Clamp (v._w, lb._w, ub._w));
    }

    public static float Dot (in Vec4 a, in Vec4 b)
    {
        return a._x * b._x + a._y * b._y + a._z * b._z + a._w * b._w;
    }

    public static Vec4 Floor (in Vec4 v)
    {
        return new Vec4 (
            Utils.Floor (v._x),
            Utils.Floor (v._y),
            Utils.Floor (v._z),
            Utils.Floor (v._w));
    }

    public static Vec4 Fmod (in Vec4 a, in Vec4 b)
    {
        return new Vec4 (
            Utils.Fmod (a._x, b._x),
            Utils.Fmod (a._y, b._y),
            Utils.Fmod (a._z, b._z),
            Utils.Fmod (a._w, b._w));
    }

    public static Vec4 Fract (in Vec4 v)
    {
        return new Vec4 (
            v._x - (int) v._x,
            v._y - (int) v._y,
            v._z - (int) v._z,
            v._w - (int) v._w);
    }

    public static Vec4 LinearStep (in Vec4 edge0, in Vec4 edge1, in Vec4 x)
    {
        return Vec4.Clamp ((x - edge0) / (edge1 - edge0));
    }

    public static float Mag (in Vec4 v)
    {
        return Utils.Sqrt (
            v._x * v._x +
            v._y * v._y +
            v._z * v._z +
            v._w * v._w);
    }

    public static float MagSq (in Vec4 v)
    {
        return v._x * v._x + v._y * v._y + v._z * v._z + v._w * v._w;
    }

    public static Vec4 Max (in Vec4 a, in Vec4 b)
    {
        return new Vec4 (
            Utils.Max (a._x, b._x),
            Utils.Max (a._y, b._y),
            Utils.Max (a._z, b._z),
            Utils.Max (a._w, b._w));
    }

    public static Vec4 Min (in Vec4 a, in Vec4 b)
    {
        return new Vec4 (
            Utils.Min (a._x, b._x),
            Utils.Min (a._y, b._y),
            Utils.Min (a._z, b._z),
            Utils.Min (a._w, b._w));
    }

    public static Vec4 Mix (Vec4 a, Vec4 b, float t = 0.5f)
    {
        float u = 1.0f - t;
        return new Vec4 (
            u * a._x + t * b._x,
            u * a._y + t * b._y,
            u * a._z + t * b._z,
            u * a._w + t * b._w);
    }

    public static Vec4 Mix (Vec4 a, Vec4 b, Vec4 t)
    {
        return new Vec4 (
            (1.0f - t._x) * a._x + t._x * b._x,
            (1.0f - t._y) * a._y + t._y * b._y,
            (1.0f - t._z) * a._z + t._z * b._z,
            (1.0f - t._w) * a._w + t._w * b._w);
    }

    public static Vec4 Mod (in Vec4 a, in Vec4 b)
    {
        return new Vec4 (
            Utils.Mod (a._x, b._x),
            Utils.Mod (a._y, b._y),
            Utils.Mod (a._z, b._z),
            Utils.Mod (a._w, b._w));
    }

    public static Vec4 Mod1 (in Vec4 v)
    {
        return new Vec4 (
            Utils.Mod1 (v._x),
            Utils.Mod1 (v._y),
            Utils.Mod1 (v._z),
            Utils.Mod1 (v._w));
    }

    public static bool None (in Vec4 v)
    {
        return v._x == 0.0f &&
            v._y == 0.0f &&
            v._z == 0.0f &&
            v._w == 0.0f;
    }

    public static Vec4 Normalize (in Vec4 v)
    {
        return v / Vec4.Mag (v);
    }

    public static Vec4 Not (in Vec4 v)
    {
        return new Vec4 (
            v._x != 0.0f ? 0.0f : 1.0f,
            v._y != 0.0f ? 0.0f : 1.0f,
            v._z != 0.0f ? 0.0f : 1.0f,
            v._w != 0.0f ? 0.0f : 1.0f);
    }

    public static Vec4 Reflect (Vec4 i, Vec4 n)
    {
        return i - ((2.0f * Vec4.Dot (n, i)) * n);
    }

    public static Vec4 Refract (Vec4 i, Vec4 n, float eta)
    {
        float iDotN = Vec4.Dot (i, n);
        float k = 1.0f - eta * eta * (1.0f - iDotN * iDotN);
        if (k < 0.0f) return new Vec4 ( );
        return (eta * i) -
            (n * (eta * iDotN + Utils.Sqrt (k)));
    }

    public static Vec4 Round (in Vec4 v)
    {
        return new Vec4 (
            Utils.Round (v._x),
            Utils.Round (v._y),
            Utils.Round (v._z),
            Utils.Round (v._w));
    }

    public static Vec4 Sign (in Vec4 v)
    {
        return new Vec4 (
            Utils.Sign (v._x),
            Utils.Sign (v._y),
            Utils.Sign (v._z),
            Utils.Sign (v._w));
    }

    public static Vec4 SmoothStep (in Vec4 edge0, in Vec4 edge1, in Vec4 x)
    {
        Vec4 t = Vec4.Clamp ((x - edge0) / (edge1 - edge0));
        return t * t * (new Vec4 (3.0f, 3.0f, 3.0f, 3.0f) - (t + t));
    }

    public static Vec4 Step (in Vec4 edge, in Vec4 x)
    {
        return new Vec4 (
            x._x < edge._x ? 0.0f : 1.0f,
            x._y < edge._y ? 0.0f : 1.0f,
            x._z < edge._z ? 0.0f : 1.0f,
            x._w < edge._w ? 0.0f : 1.0f);
    }

    public static Vec4 Trunc (in Vec4 v)
    {
        return new Vec4 (
            (int) v._x,
            (int) v._y,
            (int) v._z,
            (int) v._w);
    }

    public static Vec4 Back
    {
        get
        {
            return new Vec4 (0.0f, -1.0f, 0.0f, 0.0f);
        }
    }

    public static Vec4 Down
    {
        get
        {
            return new Vec4 (0.0f, 0.0f, -1.0f, 0.0f);
        }
    }

    public static Vec4 Forward
    {
        get
        {
            return new Vec4 (0.0f, 1.0f, 0.0f, 0.0f);
        }
    }

    public static Vec4 Left
    {
        get
        {
            return new Vec4 (-1.0f, 0.0f, 0.0f, 0.0f);
        }
    }

    public static Vec4 Right
    {
        get
        {
            return new Vec4 (1.0f, 0.0f, 0.0f, 0.0f);
        }
    }

    public static Vec4 Up
    {
        get
        {
            return new Vec4 (0.0f, 0.0f, 1.0f, 0.0f);
        }
    }
}