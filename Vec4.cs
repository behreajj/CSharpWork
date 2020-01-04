using System;
using System.Text;

[Serializable]
public readonly struct Vec4 : IComparable<Vec4>, IEquatable<Vec4>
{
    public readonly float x;
    public readonly float y;
    public readonly float z;
    public readonly float w;

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

    public float Z
    {
        get
        {
            return this.z;
        }

        // set
        // {
        //     this.z = value;
        // }
    }

    public float W
    {
        get
        {
            return this.w;
        }

        // set
        // {
        //     this.w = value;
        // }
    }

    public float this [int i]
    {
        get
        {
            switch (i)
            {
                case 0:
                case -4:
                    return this.x;
                case 1:
                case -3:
                    return this.y;
                case 2:
                case -2:
                    return this.z;
                case 3:
                case -1:
                    return this.w;
                default:
                    return 0.0f;
            }
        }

        // set
        // {
        //     switch (i)
        //     {
        //         case 0:
        //         case -4:
        //             this.x = value;
        //             break;
        //         case 1:
        //         case -3:
        //             this.y = value;
        //             break;
        //         case 2:
        //         case -2:
        //             this.z = value;
        //             break;
        //         case 3:
        //         case -1:
        //             this.w = value;
        //             break;
        //     }
        // }
    }

    public Vec4 (float x = 0.0f, float y = 0.0f, float z = 0.0f, float w = 0.0f)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public Vec4 (bool x = false, bool y = false, bool z = false, bool w = false)
    {
        this.x = x ? 1.0f : 0.0f;
        this.y = y ? 1.0f : 0.0f;
        this.z = z ? 1.0f : 0.0f;
        this.w = w ? 1.0f : 0.0f;
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
            hash = hash * hashMul ^ this.z.GetHashCode ( );
            hash = hash * hashMul ^ this.w.GetHashCode ( );
            return hash;
        }
    }

    public override string ToString ( )
    {
        return ToString (4);
    }

    public int CompareTo (Vec4 v)
    {
        return (this.w > v.w) ? 1 :
            (this.w < v.w) ? -1 :
            (this.z > v.z) ? 1 :
            (this.z < v.z) ? -1 :
            (this.y > v.y) ? 1 :
            (this.y < v.y) ? -1 :
            (this.x > v.x) ? 1 :
            (this.x < v.x) ? -1 :
            0;
    }

    public bool Equals (Vec4 v)
    {
        // return Vec4.Approx (this, v);

        if (this.w.GetHashCode ( ) != v.w.GetHashCode ( ))
        {
            return false;
        }

        if (this.z.GetHashCode ( ) != v.z.GetHashCode ( ))
        {
            return false;
        }

        if (this.y.GetHashCode ( ) != v.y.GetHashCode ( ))
        {
            return false;
        }

        if (this.x.GetHashCode ( ) != v.x.GetHashCode ( ))
        {
            return false;
        }

        return true;
    }

    // public Vec4 Reset ( )
    // {
    //     return this.Set (0.0f, 0.0f, 0.0f, 0.0f);
    // }

    // public Vec4 Set (float x = 0.0f, float y = 0.0f, float z = 0.0f, float w = 0.0f)
    // {
    //     this.x = x;
    //     this.y = y;
    //     this.z = z;
    //     this.w = w;
    //     return this;
    // }

    // public Vec4 Set (bool x = false, bool y = false, bool z = false, bool w = false)
    // {
    //     this.x = x ? 1.0f : 0.0f;
    //     this.y = y ? 1.0f : 0.0f;
    //     this.z = z ? 1.0f : 0.0f;
    //     this.w = w ? 1.0f : 0.0f;
    //     return this;
    // }

    public float[ ] ToArray ( )
    {
        return new float[ ] { this.x, this.y, this.z, this.w };
    }

    public string ToString (int places = 4)
    {
        return new StringBuilder ( )
            .Append ("{ x: ")
            .Append (Utils.ToFixed (this.x, places))
            .Append (", y: ")
            .Append (Utils.ToFixed (this.y, places))
            .Append (", z: ")
            .Append (Utils.ToFixed (this.z, places))
            .Append (", w: ")
            .Append (Utils.ToFixed (this.w, places))
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

    public static implicit operator Vec4 (Vec2 v)
    {
        return new Vec4 (v.x, v.y, 0.0f, 0.0f);
    }

    public static implicit operator Vec4 (Vec3 v)
    {
        return new Vec4 (v.x, v.y, v.z, 0.0f);
    }

    public static explicit operator bool (Vec4 v)
    {
        return Vec4.All (v);
    }

    public static explicit operator float (Vec4 v)
    {
        return Vec4.Mag (v);
    }

    public static bool operator true (Vec4 v)
    {
        return Vec4.All (v);
    }

    public static bool operator false (Vec4 v)
    {
        return Vec4.None (v);
    }

    public static Vec4 operator ! (Vec4 v)
    {
        return Vec4.Not (v);
    }

    public static Vec4 operator ~ (Vec4 v)
    {
        return Vec4.Not (v);
    }

    public static Vec4 operator & (Vec4 a, Vec4 b)
    {
        return new Vec4 (
            Utils.And (a.x, b.x),
            Utils.And (a.y, b.y),
            Utils.And (a.z, b.z),
            Utils.And (a.w, b.w));
    }

    public static Vec4 operator | (Vec4 a, Vec4 b)
    {
        return new Vec4 (
            Utils.Or (a.x, b.x),
            Utils.Or (a.y, b.y),
            Utils.Or (a.z, b.z),
            Utils.Or (a.w, b.w));
    }

    public static Vec4 operator ^ (Vec4 a, Vec4 b)
    {
        return new Vec4 (
            Utils.Xor (a.x, b.x),
            Utils.Xor (a.y, b.y),
            Utils.Xor (a.z, b.z),
            Utils.Xor (a.w, b.w));
    }

    public static Vec4 operator - (Vec4 v)
    {
        return new Vec4 (-v.x, -v.y, -v.z, -v.w);
    }

    public static Vec4 operator * (Vec4 a, Vec4 b)
    {
        return new Vec4 (
            a.x * b.x,
            a.y * b.y,
            a.z * b.z,
            a.w * b.w);
    }

    public static Vec4 operator * (Vec4 a, float b)
    {
        return new Vec4 (
            a.x * b,
            a.y * b,
            a.z * b,
            a.w * b);
    }

    public static Vec4 operator * (float a, Vec4 b)
    {
        return new Vec4 (
            a * b.x,
            a * b.y,
            a * b.z,
            a * b.w);
    }

    public static Vec4 operator / (Vec4 a, Vec4 b)
    {
        return new Vec4 (
            Utils.Div (a.x, b.x),
            Utils.Div (a.y, b.y),
            Utils.Div (a.z, b.z),
            Utils.Div (a.w, b.w));
    }

    public static Vec4 operator / (Vec4 a, float b)
    {
        if (b == 0.0f) return new Vec4 (0.0f, 0.0f, 0.0f, 0.0f);
        float bInv = 1.0f / b;
        return new Vec4 (
            a.x * bInv,
            a.y * bInv,
            a.z * bInv,
            a.w * bInv);
    }

    public static Vec4 operator / (float a, Vec4 b)
    {
        return new Vec4 (
            Utils.Div (a, b.x),
            Utils.Div (a, b.y),
            Utils.Div (a, b.z),
            Utils.Div (a, b.w));
    }

    public static Vec4 operator % (Vec4 a, Vec4 b)
    {
        return Vec4.Fmod (a, b);
    }

    public static Vec4 operator % (Vec4 a, float b)
    {
        if (b == 0.0f) return a;
        return new Vec4 (
            a.x % b,
            a.y % b,
            a.z % b,
            a.w % b);
    }

    public static Vec4 operator % (float a, Vec4 b)
    {
        return new Vec4 (
            Utils.Fmod (a, b.x),
            Utils.Fmod (a, b.y),
            Utils.Fmod (a, b.z),
            Utils.Fmod (a, b.w));
    }

    public static Vec4 operator + (Vec4 a, Vec4 b)
    {
        return new Vec4 (
            a.x + b.x,
            a.y + b.y,
            a.z + b.z,
            a.w + b.w);
    }

    public static Vec4 operator - (Vec4 a, Vec4 b)
    {
        return new Vec4 (
            a.x - b.x,
            a.y - b.y,
            a.z - b.z,
            a.w - b.w);
    }

    public static Vec4 Abs (Vec4 v)
    {
        return new Vec4 (
            Utils.Abs (v.x),
            Utils.Abs (v.y),
            Utils.Abs (v.z),
            Utils.Abs (v.w));
    }

    public static bool All (Vec4 v)
    {
        return v.x != 0.0f &&
            v.y != 0.0f &&
            v.z != 0.0f &&
            v.w != 0.0f;
    }

    public static bool Any (Vec4 v)
    {
        return v.x != 0.0f ||
            v.y != 0.0f ||
            v.z != 0.0f ||
            v.w != 0.0f;
    }

    public static bool Approx (Vec4 a, Vec4 b, float tolerance = Utils.Epsilon)
    {
        return Utils.Approx (a.x, b.x, tolerance) &&
            Utils.Approx (a.y, b.y, tolerance) &&
            Utils.Approx (a.z, b.z, tolerance) &&
            Utils.Approx (a.w, b.w, tolerance);
    }

    public static Vec4 Ceil (Vec4 v)
    {
        return new Vec4 (
            Utils.Ceil (v.x),
            Utils.Ceil (v.y),
            Utils.Ceil (v.z),
            Utils.Ceil (v.w));
    }

    public static Vec4 Clamp (Vec4 v, float lb = 0.0f, float ub = 1.0f)
    {
        return new Vec4 (
            Utils.Clamp (v.x, lb, ub),
            Utils.Clamp (v.y, lb, ub),
            Utils.Clamp (v.z, lb, ub),
            Utils.Clamp (v.w, lb, ub));
    }

    public static Vec4 Clamp (Vec4 v, Vec4 lb, Vec4 ub)
    {
        return new Vec4 (
            Utils.Clamp (v.x, lb.x, ub.x),
            Utils.Clamp (v.y, lb.y, ub.y),
            Utils.Clamp (v.z, lb.z, ub.z),
            Utils.Clamp (v.w, lb.w, ub.w));
    }

    public static float Dot (Vec4 a, Vec4 b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
    }

    public static Vec4 Floor (Vec4 v)
    {
        return new Vec4 (
            Utils.Floor (v.x),
            Utils.Floor (v.y),
            Utils.Floor (v.z),
            Utils.Floor (v.w));
    }

    public static Vec4 Fmod (Vec4 a, Vec4 b)
    {
        return new Vec4 (
            Utils.Fmod (a.x, b.x),
            Utils.Fmod (a.y, b.y),
            Utils.Fmod (a.z, b.z),
            Utils.Fmod (a.w, b.w));
    }

    public static Vec4 Fract (Vec4 a)
    {
        return new Vec4 (
            a.x - (int) a.x,
            a.y - (int) a.y,
            a.z - (int) a.z,
            a.w - (int) a.w);
    }

    public static float Mag (Vec4 v)
    {
        return Utils.Sqrt (
            v.x * v.x +
            v.y * v.y +
            v.z * v.z +
            v.w * v.w);
    }

    public static float MagSq (Vec4 v)
    {
        return v.x * v.x + v.y * v.y + v.z * v.z + v.w * v.w;
    }

    public static Vec4 Max (Vec4 a, Vec4 b)
    {
        return new Vec4 (
            Utils.Max (a.x, b.x),
            Utils.Max (a.y, b.y),
            Utils.Max (a.z, b.z),
            Utils.Max (a.w, b.w));
    }

    public static Vec4 Min (Vec4 a, Vec4 b)
    {
        return new Vec4 (
            Utils.Min (a.x, b.x),
            Utils.Min (a.y, b.y),
            Utils.Min (a.z, b.z),
            Utils.Min (a.w, b.w));
    }

    public static Vec4 Mix (Vec4 a, Vec4 b, bool t)
    {
        return t ? b : a;
    }

    public static Vec4 Mix (Vec4 a, Vec4 b, float t = 0.5f)
    {
        float u = 1.0f - t;
        return new Vec4 (
            u * a.x + t * b.x,
            u * a.y + t * b.y,
            u * a.z + t * b.z,
            u * a.w + t * b.w);
    }

    public static Vec4 Mix (Vec4 a, Vec4 b, Vec4 t)
    {
        return new Vec4 (
            (1.0f - t.x) * a.x + t.x * b.x,
            (1.0f - t.y) * a.y + t.y * b.y,
            (1.0f - t.z) * a.z + t.z * b.z,
            (1.0f - t.w) * a.w + t.w * b.w);
    }

    public static Vec4 Mod (Vec4 a, Vec4 b)
    {
        return new Vec4 (
            Utils.Mod (a.x, b.x),
            Utils.Mod (a.y, b.y),
            Utils.Mod (a.z, b.z),
            Utils.Mod (a.w, b.w));
    }

    public static Vec4 Mod1 (Vec4 v)
    {
        return new Vec4 (
            Utils.Mod1 (v.x),
            Utils.Mod1 (v.y),
            Utils.Mod1 (v.z),
            Utils.Mod1 (v.w));
    }

    public static bool None (Vec4 v)
    {
        return v.x == 0.0f &&
            v.y == 0.0f &&
            v.z == 0.0f &&
            v.w == 0.0f;
    }

    public static Vec4 Normalize (Vec4 v)
    {
        return v / Vec4.Mag (v);
    }

    public static Vec4 Not (Vec4 v)
    {
        return new Vec4 (
            v.x != 0.0f ? 0.0f : 1.0f,
            v.y != 0.0f ? 0.0f : 1.0f,
            v.z != 0.0f ? 0.0f : 1.0f,
            v.w != 0.0f ? 0.0f : 1.0f);
    }

    public static Vec4 Reflect (Vec4 i, Vec4 n)
    {
        return i - ((2.0f * Vec4.Dot (n, i)) * n);
    }

    public static Vec4 Refract (Vec4 i, Vec4 n, float eta)
    {
        float iDotN = Vec4.Dot (i, n);
        float k = 1.0f - eta * eta * (1.0f - iDotN * iDotN);
        if (k < 0.0f) return new Vec4 (0.0f, 0.0f);
        return (eta * i) -
            (n * (eta * iDotN + Utils.Sqrt (k)));
    }

    public static Vec4 Round (Vec4 v)
    {
        return new Vec4 (
            Utils.Round (v.x),
            Utils.Round (v.y),
            Utils.Round (v.z),
            Utils.Round (v.w));
    }

    public static Vec4 Sign (Vec4 v)
    {
        return new Vec4 (
            Utils.Sign (v.x),
            Utils.Sign (v.y),
            Utils.Sign (v.z),
            Utils.Sign (v.w));
    }

    public static Vec4 Trunc (Vec4 v)
    {
        return new Vec4 (
            (int) v.x,
            (int) v.y,
            (int) v.z,
            (int) v.w);
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