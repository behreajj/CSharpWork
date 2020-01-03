using System;
using System.Text;

public static class Utils
{
    public const float HalfPi = 1.570796326794897f;

    public const float OneTau = 0.15915494309189535f;

    public const float One255 = 0.003921568627450980f;

    public const float One360 = 0.002777777777777778f;

    public const float Pi = 3.141592653589793f;

    public const float Tau = 6.283185307179586f;

    public const float Epsilon = 0.000001f;

    public static float Abs (float v)
    {
        return v < 0.0f ? -v : v;
    }

    public static int And (float a, float b)
    {
        return (a != 0.0f ? 1 : 0) * (b != 0.0f ? 1 : 0);
    }

    public static bool Approx (float a, float b, float tolerance = 0.000001f)
    {
        float diff = b - a;
        return diff <= tolerance && diff >= -tolerance;
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

    public static int Ceil (float v)
    {
        return -Utils.Floor (-v);
    }

    public static float Clamp (float v, float lb = 0.0f, float ub = 1.0f)
    {
        return v<lb ? lb : v> ub ? ub : v;
    }

    public static float Cos (float radians)
    {
        return (float) Math.Cos (radians);
    }

    public static float Cosh (float radians)
    {
        return (float) Math.Cosh (radians);
    }

    public static float Div (float a, float b)
    {
        return b == 0.0f ? 0.0f : a / b;
    }

    public static float Exp (float v)
    {
        return (float) Math.Exp (v);
    }

    public static int Floor (float a)
    {
        return a > 0.0f ? (int) a : (int) a - 1;
    }

    public static float Fract (float a)
    {
        return a - (int) a;
    }

    public static float Fmod (float a, float b)
    {
        return b == 0.0f ? a : a % b;
    }

    public static float Log (float v)
    {
        return (float) Math.Log (v);
    }

    public static float Max (float a, float b)
    {
        return a >= b ? a : a < b ? b : 0.0f;
    }

    public static float Min (float a, float b)
    {
        return a <= b ? a : a > b ? b : 0.0f;
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

    public static int Not (float a)
    {
        return a != 0.0f ? 1 : 0;
    }

    public static int Or (float a, float b)
    {
        int aBool = a != 0.0f ? 1 : 0;
        int bBool = b != 0.0f ? 1 : 0;
        return aBool + bBool - aBool * bBool;
    }

    public static int Sign (float a)
    {
        return a<0.0f ? -1 : a> 0.0f ? 1 : 0;
    }

    public static float Sqrt (float v)
    {
        return (float) Math.Sqrt (v);
    }

    public static float Sin (float radians)
    {
        return (float) Math.Sin (radians);
    }

    public static float Sinh (float radians)
    {
        return (float) Math.Sinh (radians);
    }

    public static float Trunc (float a)
    {
        return (int) a;
    }

    public static int Xor (float a, float b)
    {
        int aBool = a != 0.0f ? 1 : 0;
        int bBool = b != 0.0f ? 1 : 0;
        return aBool + bBool - 2 * aBool * bBool;
    }

    // public static string ToFixed(float value, int places)
    // {
    //     if (value != value) {
    //      return "0.0";
    //   }

    //   if (places < 0) {
    //      return Integer.ToString((int) value);
    //   }

    //   if (places < 1) {
    //      return Float.ToString((int) value);
    //   }

    //   /* Value is too big. */
    //   if (value <= -3.4028235E38f || value >= 3.4028235E38f) {
    //      return Float.ToString(value);
    //   }

    //   /*
    //    * Hard-coded values from FloatConsts class for fast
    //    * absolute value and sign.
    //    */
    //   int raw = Float.FloatToRawIntBits(value);
    //   float sign = Float.IntBitsToFloat(raw & -2147483648 | 1065353216);
    //   float abs = Float.IntBitsToFloat(raw & 2147483647);
    //   int trunc = (int) abs;
    //   StringBuilder sb = new StringBuilder(16);

    //   /*
    //    * Append integral to StringBuilder.
    //    */
    //   int len = 0;
    //   if (sign < 0.0f) {
    //      sb.append('-').append(trunc);
    //      len = sb.length() - 1;
    //   } else {
    //      sb.append(trunc);
    //      len = sb.length();
    //   }
    //   sb.append('.');

    //   /*
    //    * Hard-coded limit on the number of worthwhile decimal
    //    * places beyond which single precision is no longer worth
    //    * representing accurately.
    //    */
    //   int maxPlaces = 9 - len;

    //   /*
    //    * The integral has so many digits that it has consumed the
    //    * allotment. (Might be scientific notation?)
    //    */
    //   if (maxPlaces < 1) {
    //      return Float.ToString(value);
    //   }

    //   int vetPlaces = places < maxPlaces ? places : maxPlaces;
    //   float frac = abs - trunc;

    //   /* Truncation. */
    //   for (int i = 0; i < vetPlaces; ++i) {
    //      frac *= 10.0f;
    //      int tr = (int) frac;
    //      frac -= tr;
    //      sb.Append(tr);
    //   }

    //     return sb.toString();
    // }
}