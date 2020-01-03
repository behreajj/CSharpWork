using System;
using System.Text;

[Serializable]
public struct Vec2 : IComparable<Vec2>
{
    public float x;
    public float y;

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

        set
        {
            switch (i)
            {
                case 0:
                case -2:
                    this.x = value;
                    break;
                case 1:
                case -1:
                    this.y = value;
                    break;
            }
        }
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

    public int CompareTo (Vec2 v)
    {
        return (this.y > v.y) ? 1 :
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
            .Append (" }")
            .ToString ( );
    }

    public Vec2 Set (float x = 0.0f, float y = 0.0f)
    {
        this.x = x;
        this.y = y;
        return this;
    }

    public Vec2 Set (bool x = false, bool y = false)
    {
        this.x = x ? 1.0f : 0.0f;
        this.y = y ? 1.0f : 0.0f;
        return this;
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

    public static Vec2 Sign (Vec2 v)
    {
        return new Vec2 (
            Utils.Sign (v.x),
            Utils.Sign (v.y));
    }

    public static Vec2 Abs (Vec2 v)
    {
        return new Vec2 (
            Utils.Abs (v.x),
            Utils.Abs (v.y));
    }

    public static Vec2 Ceil (Vec2 v)
    {
        return new Vec2 (
            Utils.Ceil (v.x),
            Utils.Ceil (v.y));
    }

    public static Vec2 Floor (Vec2 v)
    {
        return new Vec2 (
            Utils.Floor (v.x),
            Utils.Floor (v.y));
    }

    public static Vec2 Trunc (Vec2 v)
    {
        return new Vec2 ((int) v.x, (int) v.y);
    }

    public static Vec2 Fract (Vec2 a)
    {
        return new Vec2 (
            a.x - (int) a.x,
            a.y - (int) a.y);
    }

    public static Vec2 Fmod (Vec2 a, Vec2 b)
    {
        return new Vec2 (
            Utils.Fmod (a.x, b.x),
            Utils.Fmod (a.y, b.y));
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

    public static Vec2 Clamp (Vec2 v, float lb = 0.0f, float ub = 0.0f)
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

    public static float Dot (Vec2 a, Vec2 b)
    {
        return a.x * b.x + a.y * b.y;
    }

    public static float Mag (Vec2 v)
    {
        return Utils.Sqrt (v.x * v.x + v.y * v.y);
    }

    public static float MagSq (Vec2 v)
    {
        return v.x * v.x + v.y * v.y;
    }

    public static Vec2 Normalize (Vec2 v)
    {
        return v / Vec2.Mag (v);
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

    public static bool All (Vec2 v)
    {
        return v.x != 0.0f && v.y != 0.0f;
    }

    public static bool Any (Vec2 v)
    {
        return v.x != 0.0f || v.y != 0.0f;
    }

    public static float Heading (Vec2 v)
    {
        return Utils.Atan2 (v.y, v.x);
    }

    public static Vec2 FromPolar (
        float heading = 0.0f,
        float radius = 1.0f)
    {
        return new Vec2 (
            radius * Utils.Cos (heading),
            radius * Utils.Sin (heading));
    }

    public static bool None (Vec2 v)
    {
        return v.x == 0.0f && v.y == 0.0f;
    }

    public static Vec2 Not (Vec2 v)
    {
        return new Vec2 (
            v.x != 0.0f ? 0.0f : 1.0f,
            v.y != 0.0f ? 0.0f : 1.0f);
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