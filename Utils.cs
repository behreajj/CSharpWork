using System;
using System.Text;

public static class Utils
{
    public const float HalfPi = 1.57079637f;
    public const int HashBase = -2128831035;
    public const int HashMul = 16777619;
    public const float OneTau = 0.159154937f;
    public const float One255 = 0.003921569f;
    public const float One360 = 0.00277777785f;
    public const float OneSix = 0.166666667f;
    public const float Pi = 3.14159274f;
    public const float Tau = 6.28318548f;
    public const float Epsilon = 0.000001f;

    public static float Abs (float v)
    {
        return v < 0.0f ? -v : v;
    }

    public static float Acos (float v)
    {
        if (v <= -1.0f) return Utils.Pi;
        if (v >= 1.0f) return 0.0f;

        bool ltzero = v < 0.0f;
        float negate = ltzero ? 1.0f : 0.0f;
        v = ltzero ? -v : v;

        float ret = -0.0187293f;
        ret = ret * v;
        ret = ret + 0.0742610f;
        ret = ret * v;
        ret = ret - 0.2121144f;
        ret = ret * v;
        ret = ret + Utils.HalfPi;
        ret = ret * Utils.Sqrt (1.0f - v);
        ret = ret - negate * (ret + ret);
        return negate * Utils.Pi + ret;
    }

    public static int And (float a, float b)
    {
        // return (a != 0.0f ? 1 : 0) * (b != 0.0f ? 1 : 0);
        return ((a != 0.0f) & (b != 0.0f)) ? 1 : 0;
    }

    public static bool Approx (float a, float b, float tolerance = Utils.Epsilon)
    {
        float diff = b - a;
        return diff <= tolerance && diff >= -tolerance;
    }

    public static float Asin (float v)
    {
        if (v <= -1.0f) return -Utils.HalfPi;
        if (v >= 1.0f) return Utils.HalfPi;

        bool ltzero = v < 0.0f;
        float negate = ltzero ? 1.0f : 0.0f;
        v = ltzero ? -v : v;

        float ret = -0.0187293f;
        ret *= v;
        ret += 0.0742610f;
        ret *= v;
        ret -= 0.2121144f;
        ret *= v;
        ret += Utils.HalfPi;
        ret = Utils.Pi * 0.5f - Utils.Sqrt (1.0f - v) * ret;
        return ret - negate * (ret + ret);
    }

    public static float Atan2 (float y, float x)
    {
        float yAbs = Utils.Abs (y);
        float xAbs = Utils.Abs (x);
        float t1 = yAbs;
        float t2 = xAbs;
        float t0 = Utils.Max (t1, t2);
        t1 = Utils.Min (t1, t2);
        t2 = 1.0f / t0;
        t2 = t1 * t2;
        float t3 = t2 * t2;
        t0 = -0.013480470f;
        t0 = t0 * t3 + 0.057477314f;
        t0 = t0 * t3 - 0.121239071f;
        t0 = t0 * t3 + 0.195635925f;
        t0 = t0 * t3 - 0.332994597f;
        t0 = t0 * t3 + 0.999995630f;
        t2 = t0 * t2;
        t2 = yAbs > xAbs ? Utils.HalfPi - t2 : t2;
        t2 = x < 0.0f ? Utils.Pi - t2 : t2;
        return y < 0.0f ? -t2 : t2;
    }

    // public static int BisectLeft<T> (T[ ] a, T x, int lo, int hi) where T : IComparable<T>
    // {
    //     if (lo < 0) lo = 0;
    //     while (lo < hi)
    //     {
    //         int mid = (lo + hi) / 2 | 0;
    //         if (x.CompareTo (a[mid]) > 0) lo = mid + 1;
    //         else hi = mid;
    //     }
    //     return lo;
    // }

    public static int Ceil (float v)
    {
        return v > 0.0f ? (int) (v + 1) : (int) v;
    }

    public static int Clamp (int v, int lb = int.MinValue, int ub = int.MaxValue)
    {
        return (v < lb) ? lb : (v > ub) ? ub : v;
    }

    public static float Clamp (float v, float lb = 0.0f, float ub = 1.0f)
    {
        return (v < lb) ? lb : (v > ub) ? ub : v;
    }

    public static float CopySign (float mag, float sign)
    {
        // return Utils.Abs (magnitude) * Utils.Sign (sign);
        return (mag < 0.0f ? -mag : mag) * ((sign < 0.0f) ? -1.0f : (sign > 0.0f) ? 1.0f : 0.0f);
    }

    public static float Cos (float radians)
    {
        return (float) Math.Cos (radians);
    }

    public static float Cosh (float radians)
    {
        return (float) Math.Cosh (radians);
    }

    public static float Diff (float a, float b)
    {
        return Utils.Abs (a - b);
    }

    public static float Div (float a, float b)
    {
        return b == 0.0f ? 0.0f : a / b;
    }

    public static float Exp (float v)
    {
        return (float) Math.Exp (v);
    }

    public static int Floor (float v)
    {
        return v > 0.0f ? (int) v : (int) v - 1;
    }

    public static float Fract (float v)
    {
        return v - (int) v;
    }

    public static int Fmod (int a, int b)
    {
        return b == 0 ? a : a % b;
    }

    public static float Fmod (float a, float b)
    {
        return b == 0.0f ? a : a % b;
    }

    public static float LerpAngle (float origin, float dest, float t = 0.5f)
    {
        float a = Utils.ModRadians (origin);
        float b = Utils.ModRadians (dest);
        float diff = b - a;
        bool modResult = false;
        if (a < b && diff > Utils.Pi)
        {
            a = a + Utils.Pi;
            modResult = true;
        }
        else if (a > b && diff < -Utils.Pi)
        {
            b = b + Utils.Pi;
            modResult = true;
        }

        float fac = (1.0f - t) * a + t * b;
        return modResult ? Utils.ModRadians (fac) : fac;
    }

    public static float LinearStep (float edge0 = 0.0f, float edge1 = 1.0f, float x = 0.5f)
    {
        return Utils.Clamp (Utils.Div (x - edge0, edge1 - edge0));
    }

    public static float Log (float v)
    {
        return (float) Math.Log (v);
    }

    public static float Map (
        float v,
        float lbOrigin = -1.0f,
        float ubOrigin = 1.0f,
        float lbDest = 0.0f,
        float ubDest = 1.0f)
    {
        float denom = ubOrigin - lbOrigin;
        return denom == 0.0f ? lbDest :
            lbDest + (ubDest - lbDest) *
            ((v - lbOrigin) / denom);
    }

    public static float Max (float a, float b)
    {
        return a >= b ? a : a < b ? b : 0.0f;
    }

    public static float Max (float a, float b, float c)
    {
        return Utils.Max (Utils.Max (a, b), c);
    }

    public static float Mix (float a, float b, float t = 0.5f)
    {
        return (1.0f - t) * a + t * b;
    }

    public static float Min (float a, float b)
    {
        return a <= b ? a : a > b ? b : 0.0f;
    }

    public static float Min (float a, float b, float c)
    {
        return Utils.Min (Utils.Min (a, b), c);
    }

    public static int Mod (int a, int b)
    {
        if (b == 0) return a;
        int result = a - b * (a / b);
        return result < 0 ? result + b : result;
    }

    public static float Mod (float a, float b)
    {
        return b == 0.0f ? a : a - b * Utils.Floor (a / b);
    }

    public static float Mod1 (float a)
    {
        return a - Utils.Floor (a);
    }

    public static float ModDegrees (float deg)
    {
        return deg - 360.0f * Utils.Floor (deg * Utils.One360);
    }

    public static float ModRadians (float rad)
    {
        return rad - Utils.Tau * Utils.Floor (rad * Utils.OneTau);
    }

    public static int Not (float v)
    {
        return v != 0.0f ? 1 : 0;
    }

    public static int Or (float a, float b)
    {
        // int aBool = a != 0.0f ? 1 : 0;
        // int bBool = b != 0.0f ? 1 : 0;
        // return aBool + bBool - aBool * bBool;

        return ((a != 0.0f) | (b != 0.0f)) ? 1 : 0;
    }

    public static float Pow (float a, float b)
    {
        return (float) Math.Pow (a, b);
    }

    public static int Round (float v)
    {
        return (v < 0.0f) ? (int) (v - 0.5f) :
            (v > 0.0f) ? (int) (v + 0.5f) : 0;
    }

    public static float Round (float v, int places)
    {
        return (float) Math.Round (v, places);
    }

    public static int Sign (float v)
    {
        return (v < 0.0f) ? -1 : (v > 0.0f) ? 1 : 0;
    }

    public static float Sin (float radians)
    {
        return (float) Math.Sin (radians);
    }

    public static float Sinh (float radians)
    {
        return (float) Math.Sinh (radians);
    }

    public static float SmoothStep (float edge0 = 0.0f, float edge1 = 1.0f, float x = 0.5f)
    {
        float t = Utils.LinearStep (edge0, edge1, x);
        return t * t * (3.0f - (t + t));
    }

    public static float Sqrt (float v)
    {
        return (float) Math.Sqrt (v);
    }

    public static float Step (float edge, float x)
    {
        return x < edge ? 0.0f : 1.0f;
    }

    public static float Trunc (float a)
    {
        return (int) a;
    }

    public static int Xor (float a, float b)
    {
        // int aBool = (a != 0.0f) ? 1 : 0;
        // int bBool = (b != 0.0f) ? 1 : 0;
        // return aBool + bBool - 2 * aBool * bBool;

        return ((a != 0.0f) ^ (b != 0.0f)) ? 1 : 0;
    }

    public static string ToFixed (float v, int places = 7)
    {
        /*
         * Dispense with v and places edge cases.
         */
        if (float.IsNaN (v)) return "0.0";
        if (places < 0) return ((int) v).ToString ( );
        if (places < 1) return ((float) ((int) v)).ToString ( );
        if (v < float.MinValue || v > float.MaxValue)
        {
            return v.ToString ( );
        }

        /*
         * Find the sign, the unsigned v and the
         * unsigned integral.
         */
        bool ltZero = v < 0.0f;
        int sign = ltZero ? -1 : (v > 0.0f) ? 1 : 0;
        float abs = ltZero ? -v : v;
        int trunc = (int) abs;
        int len = 0;

        /*
         * Start the string builder with the integral
         * and the sign.
         */
        StringBuilder sb = new StringBuilder (16);
        if (sign < 0)
        {
            sb.Append ('-').Append (trunc);
            len = sb.Length - 1;
        }
        else
        {
            sb.Append (trunc);
            len = sb.Length;
        }
        sb.Append ('.');

        /*
         * Find the number of places left to work with after the
         * integral. Any more than 9 and single-precision's
         * inaccuracy would make the effort worthless.
         *
         * For numbers with a big integral, there may not be much
         * left to work with, and so fewer than the requested
         * number of places will be used.
         */
        int maxPlaces = 9 - len;
        if (maxPlaces < 1) return v.ToString ( );
        int vetPlaces = places < maxPlaces ?
            places : maxPlaces;

        /* 
         * Separate each digit by subtracting the truncation from
         * the v (fract), then multiplying by 10 to shift the
         * next digit past the decimal point.
         */
        float frac = abs - trunc;
        for (int i = 0; i < vetPlaces; ++i)
        {
            frac *= 10.0f;
            int tr = (int) frac;
            frac -= tr;
            sb.Append (tr);
        }
        return sb.ToString ( );
    }
}