using System;
using System.Text;

[Serializable]
public struct Vec3 : IComparable<Vec3>
{
    public float x;
    public float y;
    public float z;

    public float X
    {
        get
        {
            return this.x;
        }

        set
        {
            this.x = value;
        }
    }

    public float Y
    {
        get
        {
            return this.y;
        }

        set
        {
            this.y = value;
        }
    }

    public float Z
    {
        get
        {
            return this.z;
        }

        set
        {
            this.z = value;
        }
    }

    public float this [int i]
    {
        get
        {
            switch (i)
            {
                case 0:
                case -3:
                    return this.x;
                case 1:
                case -2:
                    return this.y;
                case 2:
                case -1:
                    return this.z;
                default:
                    return 0.0f;
            }
        }

        set
        {
            switch (i)
            {
                case 0:
                case -3:
                    this.x = value;
                    break;
                case 1:
                case -2:
                    this.y = value;
                    break;
                case 2:
                case -1:
                    this.z = value;
                    break;
            }
        }
    }

    public Vec3 (float x = 0.0f, float y = 0.0f, float z = 0.0f)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vec3 (bool x = false, bool y = false, bool z = false)
    {
        this.x = x ? 1.0f : 0.0f;
        this.y = y ? 1.0f : 0.0f;
        this.z = z ? 1.0f : 0.0f;
    }

    public int CompareTo (Vec3 v)
    {
        return (this.z > v.z) ? 1 :
            (this.z < v.z) ? -1 :
            (this.y > v.y) ? 1 :
            (this.y < v.y) ? -1 :
            (this.x > v.x) ? 1 :
            (this.x < v.x) ? -1 :
            0;
    }

    public override string ToString ( )
    {
        return new StringBuilder ( )
            .Append ("{ x: ")
            .Append (this.x)
            .Append (", y: ")
            .Append (this.y)
            .Append (", z: ")
            .Append (this.z)
            .Append (" }")
            .ToString ( );
    }

    public Vec3 Set (float x = 0.0f, float y = 0.0f, float z = 0.0f)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        return this;
    }

    public Vec3 Set (bool x = false, bool y = false, bool z = false)
    {
        this.x = x ? 1.0f : 0.0f;
        this.y = y ? 1.0f : 0.0f;
        this.z = z ? 1.0f : 0.0f;
        return this;
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

    public static implicit operator Vec3 (Vec2 v)
    {
        return new Vec3 (v.x, v.y, 0.0f);
    }

    public static explicit operator bool (Vec3 v)
    {
        return Vec3.All (v);
    }

    public static explicit operator float (Vec3 v)
    {
        return Vec3.Mag (v);
    }

    public static bool operator true (Vec3 v)
    {
        return Vec3.All (v);
    }

    public static bool operator false (Vec3 v)
    {
        return Vec3.None (v);
    }

    public static Vec3 operator ! (Vec3 v)
    {
        return Vec3.Not (v);
    }

    public static Vec3 operator ~ (Vec3 v)
    {
        return Vec3.Not (v);
    }

    public static Vec3 operator & (Vec3 a, Vec3 b)
    {
        return new Vec3 (
            Utils.And (a.x, b.x),
            Utils.And (a.y, b.y),
            Utils.And (a.z, b.z));
    }

    public static Vec3 operator | (Vec3 a, Vec3 b)
    {
        return new Vec3 (
            Utils.Or (a.x, b.x),
            Utils.Or (a.y, b.y),
            Utils.Or (a.z, b.z));
    }

    public static Vec3 operator ^ (Vec3 a, Vec3 b)
    {
        return new Vec3 (
            Utils.Xor (a.x, b.x),
            Utils.Xor (a.y, b.y),
            Utils.Xor (a.z, b.z));
    }

    public static Vec3 operator - (Vec3 v)
    {
        return new Vec3 (-v.x, -v.y, -v.z);
    }

    public static Vec3 operator * (Vec3 a, Vec3 b)
    {
        return new Vec3 (a.x * b.x, a.y * b.y, a.z * b.z);
    }

    public static Vec3 operator * (Vec3 a, float b)
    {
        return new Vec3 (a.x * b, a.y * b, a.z * b);
    }

    public static Vec3 operator * (float a, Vec3 b)
    {
        return new Vec3 (a * b.x, a * b.y, a * b.z);
    }

    public static Vec3 operator / (Vec3 a, Vec3 b)
    {
        return new Vec3 (
            Utils.Div (a.x, b.x),
            Utils.Div (a.y, b.y),
            Utils.Div (a.z, b.z));
    }

    public static Vec3 operator / (Vec3 a, float b)
    {
        if (b == 0.0f) return new Vec3 (0.0f, 0.0f, 0.0f);
        float bInv = 1.0f / b;
        return new Vec3 (
            a.x * bInv,
            a.y * bInv,
            a.z * bInv);
    }

    public static Vec3 operator / (float a, Vec3 b)
    {
        return new Vec3 (
            Utils.Div (a, b.x),
            Utils.Div (a, b.y),
            Utils.Div (a, b.z));
    }

    public static Vec3 operator % (Vec3 a, Vec3 b)
    {
        return Vec3.Fmod (a, b);
    }

    public static Vec3 operator % (Vec3 a, float b)
    {
        if (b == 0.0f) return a;
        return new Vec3 (a.x % b, a.y % b, a.z % b);
    }

    public static Vec3 operator % (float a, Vec3 b)
    {
        return new Vec3 (
            Utils.Fmod (a, b.x),
            Utils.Fmod (a, b.y),
            Utils.Fmod (a, b.z));
    }

    public static Vec3 operator + (Vec3 a, Vec3 b)
    {
        return new Vec3 (a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public static Vec3 operator - (Vec3 a, Vec3 b)
    {
        return new Vec3 (a.x - b.x, a.y - b.y, a.z - b.z);
    }

    public static Vec3 Sign (Vec3 v)
    {
        return new Vec3 (
            Utils.Sign (v.x),
            Utils.Sign (v.y),
            Utils.Sign (v.z));
    }

    public static Vec3 Abs (Vec3 v)
    {
        return new Vec3 (
            Utils.Abs (v.x),
            Utils.Abs (v.y),
            Utils.Abs (v.z));
    }

    public static Vec3 Ceil (Vec3 v)
    {
        return new Vec3 (
            Utils.Ceil (v.x),
            Utils.Ceil (v.y),
            Utils.Ceil (v.z));
    }

    public static Vec3 Floor (Vec3 v)
    {
        return new Vec3 (
            Utils.Floor (v.x),
            Utils.Floor (v.y),
            Utils.Floor (v.z));
    }

    public static Vec3 Trunc (Vec3 v)
    {
        return new Vec3 ((int) v.x, (int) v.y, (int) v.z);
    }

    public static Vec3 Fract (Vec3 a)
    {
        return new Vec3 (
            a.x - (int) a.x,
            a.y - (int) a.y,
            a.z - (int) a.z);
    }

    public static Vec3 Fmod (Vec3 a, Vec3 b)
    {
        return new Vec3 (
            Utils.Fmod (a.x, b.x),
            Utils.Fmod (a.y, b.y),
            Utils.Fmod (a.z, b.z));
    }

    public static Vec3 Mod (Vec3 a, Vec3 b)
    {
        return new Vec3 (
            Utils.Mod (a.x, b.x),
            Utils.Mod (a.y, b.y),
            Utils.Mod (a.z, b.z));
    }

    public static Vec3 Mod1 (Vec3 v)
    {
        return new Vec3 (
            Utils.Mod1 (v.x),
            Utils.Mod1 (v.y),
            Utils.Mod1 (v.z));
    }

    public static Vec3 Max (Vec3 a, Vec3 b)
    {
        return new Vec3 (
            Utils.Max (a.x, b.x),
            Utils.Max (a.y, b.y),
            Utils.Max (a.z, b.z));
    }

    public static Vec3 Min (Vec3 a, Vec3 b)
    {
        return new Vec3 (
            Utils.Min (a.x, b.x),
            Utils.Min (a.y, b.y),
            Utils.Min (a.z, b.z));
    }

    public static Vec3 Clamp (Vec3 v, float lb = 0.0f, float ub = 0.0f)
    {
        return new Vec3 (
            Utils.Clamp (v.x, lb, ub),
            Utils.Clamp (v.y, lb, ub),
            Utils.Clamp (v.z, lb, ub));
    }

    public static Vec3 Clamp (Vec3 v, Vec3 lb, Vec3 ub)
    {
        return new Vec3 (
            Utils.Clamp (v.x, lb.x, ub.x),
            Utils.Clamp (v.y, lb.y, ub.y),
            Utils.Clamp (v.z, lb.z, ub.z));
    }

    public static Vec3 Mix (Vec3 a, Vec3 b, bool t)
    {
        return t ? b : a;
    }

    public static Vec3 Mix (Vec3 a, Vec3 b, float t = 0.5f)
    {
        float u = 1.0f - t;
        return new Vec3 (
            u * a.x + t * b.x,
            u * a.y + t * b.y,
            u * a.z + t * b.z);
    }

    public static Vec3 Mix (Vec3 a, Vec3 b, Vec3 t)
    {
        return new Vec3 (
            (1.0f - t.x) * a.x + t.x * b.x,
            (1.0f - t.y) * a.y + t.y * b.y,
            (1.0f - t.z) * a.z + t.z * b.z);
    }

    public static Vec3 Cross (Vec3 a, Vec3 b)
    {
        return new Vec3 (
            a.y * b.z - a.z * b.y,
            a.z * b.x - a.x * b.z,
            a.x * b.y - a.y * b.x);
    }

    public static float Dot (Vec3 a, Vec3 b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }

    public static float Mag (Vec3 v)
    {
        return Utils.Sqrt (
            v.x * v.x +
            v.y * v.y +
            v.z * v.z);
    }

    public static float MagSq (Vec3 v)
    {
        return v.x * v.x + v.y * v.y + v.z * v.z;
    }

    public static Vec3 Normalize (Vec3 v)
    {
        return v / Vec3.Mag (v);
    }

    public static Vec3 Reflect (Vec3 i, Vec3 n)
    {
        return i - ((2.0f * Vec3.Dot (n, i)) * n);
    }

    public static Vec3 Refract (Vec3 i, Vec3 n, float eta)
    {
        float iDotN = Vec3.Dot (i, n);
        float k = 1.0f - eta * eta * (1.0f - iDotN * iDotN);
        if (k < 0.0f) return new Vec3 (0.0f, 0.0f);
        return (eta * i) -
            (n * (eta * iDotN + Utils.Sqrt (k)));
    }

    public static bool All (Vec3 v)
    {
        return v.x != 0.0f &&
            v.y != 0.0f &&
            v.z != 0.0f;
    }

    public static bool Any (Vec3 v)
    {
        return v.x != 0.0f ||
            v.y != 0.0f ||
            v.z != 0.0f;
    }

    public static float Azimuth (Vec3 v)
    {
        return Utils.Atan2 (v.y, v.x);
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

    public static bool None (Vec3 v)
    {
        return v.x == 0.0f &&
            v.y == 0.0f &&
            v.z == 0.0f;
    }

    public static Vec3 Not (Vec3 v)
    {
        return new Vec3 (
            v.x != 0.0f ? 0.0f : 1.0f,
            v.y != 0.0f ? 0.0f : 1.0f,
            v.z != 0.0f ? 0.0f : 1.0f);
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