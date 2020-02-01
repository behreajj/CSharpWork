using System;
using System.Runtime.InteropServices;
using System.Text;

public static class Utils
{
    /// <summary>
    /// CF. https://docs.microsoft.com/en-us/dotnet/csharp/
    /// programming-guide/concepts/attributes/
    /// how-to-create-a-c-cpp-union-by-using-attributes
    /// </summary>
    [StructLayout (LayoutKind.Explicit)]
    private struct Union
    {
        [FieldOffset (0)]
        public float f;

        [FieldOffset (0)]
        public int i;
    }

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

    public static float Acos (float value)
    {
        if (value <= -1.0f) return Utils.Pi;
        if (value >= 1.0f) return 0.0f;

        bool ltZero = value < 0.0f;
        float x = ltZero ? -value : value;
        float ret = -0.0187293f;
        ret *= x;
        ret += 0.074261f;
        ret *= x;
        ret -= 0.2121144f;
        ret *= x;
        ret += Utils.HalfPi;
        ret *= (float) Math.Sqrt (1.0d - x);
        return ltZero ? Utils.Pi - ret : ret;
    }

    public static int And (float a, float b)
    {
        return ((a != 0.0f) & (b != 0.0f)) ? 1 : 0;
    }

    public static bool Approx (float a, float b, float tolerance = Utils.Epsilon)
    {
        float diff = b - a;
        return diff <= tolerance && diff >= -tolerance;
    }

    public static float Asin (float value)
    {
        if (value <= -1.0f) return -Utils.HalfPi;
        if (value >= 1.0f) return Utils.HalfPi;

        bool ltZero = value < 0.0f;
        float x = ltZero ? -value : value;
        float ret = -0.0187293f;
        ret *= x;
        ret += 0.074261f;
        ret *= x;
        ret -= 0.2121144f;
        ret *= x;
        ret += Utils.HalfPi;
        ret = Utils.HalfPi - ret * (float) Math.Sqrt (1.0d - x);
        return ltZero ? -ret : ret;
    }

    public static float Atan2 (float y, float x)
    {
        bool yLtZero = y < 0.0f;
        bool xLtZero = x < 0.0f;
        float yAbs = yLtZero ? -y : y;
        float xAbs = xLtZero ? -x : x;

        bool yGtX = yAbs > xAbs;
        float t0 = yGtX ? yAbs : xAbs;
        if (t0 == 0.0f) return 0.0f;
        float t2 = (yGtX ? xAbs : yAbs) / t0;

        float t3 = t2 * t2;
        t0 = -0.01348047f;
        t0 = t0 * t3 + 0.057477314f;
        t0 = t0 * t3 - 0.121239071f;
        t0 = t0 * t3 + 0.195635925f;
        t0 = t0 * t3 - 0.332994597f;
        t0 = t0 * t3 + 0.99999563f;
        t2 = t0 * t2;
        t2 = yGtX ? Utils.HalfPi - t2 : t2;
        t2 = xLtZero ? Utils.Pi - t2 : t2;
        return yLtZero ? -t2 : t2;
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
        return (mag < 0.0f ? -mag : mag) *
            ((sign < 0.0f) ? -1.0f : (sign > 0.0f) ? 1.0f : 0.0f);
    }

    public static float Cos (float radians)
    {
        return Utils.SinCosEval (Utils.OneTau * radians);
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

    public static float Exp (float a)
    {
        return (float) Math.Exp (a);
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

    public static float InvSqrt (float a)
    {
        return a > 0.0f ? Utils.InvSqrtUnchecked (a) : 0.0f;
    }

    public static float InvSqrtUnchecked (float a)
    {
        // Union u = new Union ( );
        // u.f = a;
        // u.i = 0x5f375a86 - (u.i >> 1);

        // float y = u.f;
        // float vhalf = a * 0.5f;
        // y *= 1.5f - vhalf * y * y;
        // y *= 1.5f - vhalf * y * y;
        // y *= 1.5f - vhalf * y * y;
        // return y;

        return (float) (1.0d / Math.Sqrt (a));
    }

    public static float LerpAngle (float origin, float dest, float t = 0.5f)
    {
        // TODO: Can this be simplified by dividing by TAU, then using mod1?
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

    public static float LinearStep (
        float edge0 = 0.0f,
        float edge1 = 1.0f,
        float x = 0.5f)
    {
        float denom = edge1 - edge0;
        if (denom == 0.0f) return 0.0f;
        return Utils.Clamp ((x - edge0) / denom, 0.0f, 1.0f);
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

    public static float Max (params float[ ] values)
    {
        int len = values.Length;
        float result = float.MinValue;
        for (int i = 0; i < len; ++i)
        {
            float v = values[i];
            result = (v >= result) ? v : result;
        }
        return result;
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

    public static float Min (params float[ ] values)
    {
        int len = values.Length;
        float result = float.MaxValue;
        for (int i = 0; i < len; ++i)
        {
            float v = values[i];
            result = (v <= result) ? v : result;
        }
        return result;
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
        return ((a != 0.0f) | (b != 0.0f)) ? 1 : 0;
    }

    public static float Pow (float a, float b)
    {
        return (float) Math.Pow (a, b);
    }

    public static float Quantize (float v, int levels = 8)
    {
        if (levels < 2) return v;
        return Utils.Floor (0.5f + v * levels) / levels;
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
        return Utils.SinCosEval (Utils.OneTau * radians - 0.25f);
    }

    public static void SinCos (float radians, out float sina, out float cosa)
    {
        float nrm = Utils.OneTau * radians;
        sina = Utils.SinCosEval (nrm - 0.25f);
        cosa = Utils.SinCosEval (nrm);
    }

    public static (float sin, float cos) SinCos (float radians)
    {
        float nrm = Utils.OneTau * radians;
        return (
            sin: Utils.SinCosEval (nrm - 0.25f),
            cos: Utils.SinCosEval (nrm));
    }

    private static float SinCosEval (float normRad)
    {
        float r1y = normRad - (int) normRad;

        bool r2x = r1y < 0.25f;
        float r1x = 0.0f;
        if (r2x)
        {
            float r0x = r1y * r1y;
            r1x = 24.980804f * r0x - 60.14581f;
            r1x = r1x * r0x + 85.45379f;
            r1x = r1x * r0x - 64.939354f;
            r1x = r1x * r0x + 19.739208f;
            r1x = r1x * r0x - 1.0f;
        }

        bool r2z = r1y >= 0.75f;
        float r1z = 0.0f;
        if (r2z)
        {
            float r0z = 1.0f - r1y;
            r0z = r0z * r0z;
            r1z = 24.980804f * r0z - 60.14581f;
            r1z = r1z * r0z + 85.45379f;
            r1z = r1z * r0z - 64.939354f;
            r1z = r1z * r0z + 19.739208f;
            r1z = r1z * r0z - 1.0f;
        }

        float r0y = 0.5f - r1y;
        r1y = 0.0f;
        if (r1y >= -9.0f ^ (r2x | r2z))
        {
            r0y = r0y * r0y;
            r1y = 60.14581f - r0y * 24.980804f;
            r1y = r1y * r0y - 85.45379f;
            r1y = r1y * r0y + 64.939354f;
            r1y = r1y * r0y - 19.739208f;
            r1y = r1y * r0y + 1.0f;
        }

        return -r1x - r1z - r1y;
    }

    public static float Sinh (float radians)
    {
        return (float) Math.Sinh (radians);
    }

    public static float SmoothStep (
        float edge0 = 0.0f,
        float edge1 = 1.0f,
        float x = 0.5f)
    {
        float t = Utils.LinearStep (edge0, edge1, x);
        return t * t * (3.0f - (t + t));
    }

    public static float Sqrt (float v)
    {
        return v > 0.0f ? Utils.Sqrt (v) : 0.0f;
    }

    public static float SqrtUnchecked (float v)
    {
        // return v * Utils.InvSqrtUnchecked (v);
        return (float) Math.Sqrt (v);
    }

    public static float Step (float edge, float x)
    {
        return x < edge ? 0.0f : 1.0f;
    }

    public static bool ToBool (int v)
    {
        return v != 0;
    }

    public static bool ToBool (float v)
    {
        return v != 0.0f;
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

    public static float ToFloat (bool v)
    {
        return v ? 1.0f : 0.0f;
    }

    public static int ToInt (bool v)
    {
        return v ? 1 : 0;
    }

    public static float Trunc (float a)
    {
        return (int) a;
    }

    public static float Wrap (
        float value,
        float lb = -1.0f,
        float ub = 1.0f)
    {
        float lbc = 0.0f;
        float ubc = 0.0f;
        float span = ub - lb;

        if (span < 0.0f)
        {
            lbc = ub;
            ubc = lb;
        }
        else if (span > 0.0f)
        {
            lbc = lb;
            ubc = ub;
        }
        else
        {
            return 0.0f;
        }

        if (value < lbc)
        {
            return ubc - (lbc - value) % span;
        }
        else if (value >= ubc)
        {
            return lbc + (value - lbc) % span;
        }
        return value;
    }

    public static int Xor (float a, float b)
    {
        return ((a != 0.0f) ^ (b != 0.0f)) ? 1 : 0;
    }
}