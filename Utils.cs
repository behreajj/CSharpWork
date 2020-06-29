using System;
using System.Runtime.CompilerServices;
using System.Text;

public static class Utils
{
    /// <summary>
    /// An angle in degrees is multiplied by this constant to convert it to
    /// radians. PI / 180.0, approximately 0.0174532924 .
    /// </summary>
    public const float DegToRad = 0.0174532924f;

    /// <summary>
    /// The smallest positive non-zero value. Useful for testing approximation
    /// between two floats. Set to 0.000001 .
    /// </summary>
    public const float Epsilon = 0.000001f;

    /// <summary>
    /// Four-thirds, 4.0 / 3.0 . Approximately 1.33333333 . Useful when creating
    /// a circular shape with a series of Bezier curves.
    /// </summary>
    public const float FourThirds = 1.33333333f;

    /// <summary>
    /// An approximation of TAU / ( PHI * PHI ) , 2.39996314 . Useful for
    /// replicating phyllotaxis. In degrees, 137.50777 .
    /// </summary>
    public const float GoldenAngle = 2.39996314f;

    /// <summary>
    /// PI divided by 2.0 . Approximately 1.57079637 .
    /// </summary>
    public const float HalfPi = 1.57079637f;

    /// <summary>
    /// Base value used by hash code functions.
    /// </summary>
    public const int HashBase = -2128831035;

    /// <summary>
    /// Multiplier used by hash code functions.
    /// </summary>
    public const int HashMul = 16777619;

    /// <summary>
    /// The hash base multiplied by the hash multiplier.
    /// </summary>
    public const int MulBase = 84696351;

    /// <summary>
    /// One-255th, 1.0 / 255.0 . Useful when converting a color with channels in
    /// the range [0, 255] to a color in the range [0.0, 1.0] . Approximately
    /// 0.003921569 .
    /// </summary>
    public const float One255 = 0.003921569f;

    /// <summary>
    /// One divided by 360 degrees, 1.0 / 360.0 ; approximately 0.00277777785 .
    /// Useful for converting an index in a for-loop to an angle in degrees.
    /// </summary>
    public const float One360 = 0.00277777785f;

    /// <summary>
    /// One divided by PI . Useful when converting inclinations to the range
    /// [0.0, 1.0] . Approximately 0.318309873 .
    /// </summary>
    public const float OnePi = 0.318309873f;

    /// <summary>
    /// One-sixth, 1.0 / 6.0 . Useful when converting a color in RGB color space
    /// to one in HSB, given the six sectors formed by primary and secondary
    /// colors . Approximately 0.16666667 .
    /// </summary>
    public const float OneSix = 0.16666667f;

    /// <summary>
    /// An approximation of 1.0 / SQRT ( 2.0 ) , 0.707106769 .
    /// </summary>
    public const float OneSqrt2 = 0.707106769f;

    /// <summary>
    /// An approximation of 1.0 / SQRT ( 3.0 ) , 0.577350259 .
    /// </summary>
    public const float OneSqrt3 = 0.577350259f;

    /// <summary>
    /// One divided by TAU . Useful for converting an index in a for-loop to an
    /// angle. Approximately 0.159154937 .
    /// </summary>
    public const float OneTau = 0.159154937f;

    /// <summary>
    /// One-third, 1.0 / 3.0 . Approximately 0.333333333 . Useful for setting
    /// handles on the knot of a Bezier curve.
    /// </summary>
    public const float OneThird = 0.333333333f;

    /// <summary>
    /// An approximation of PHI , or ( 1.0 + SQRT ( 5.0 ) ) / 2.0 , 1.618034 .
    /// </summary>
    public const float Phi = 1.618034f;

    /// <summary>
    /// An approximation of PI, 3.14159274 .
    /// </summary>
    public const float Pi = 3.14159274f;

    /// <summary>
    /// An angle in radians is multiplied by this constant to convert it to
    /// degrees. 180.0 / PI , approximately 57.29578 .
    /// </summary>
    public const float RadToDeg = 57.29578f;

    /// <summary>
    /// An approximation of TAU, 6.28318548 . Equal to 2.0 PI .
    /// </summary>
    public const float Tau = 6.28318548f;

    /// <summary>
    /// PI divided by 3.0 , 1.04719758 . Useful for describing the field of view
    /// in a perspective camera.
    /// </summary>
    public const float ThirdPi = 1.04719758f;

    /// <summary>
    /// Two-thirds, 2.0 / 3.0 . Approximately 0.6666667 .
    /// </summary>
    public const float TwoThirds = 0.6666667f;

    /// <summary>
    /// Finds the absolute value of a single precision real number. Equivalent
    /// to MAX(-a, a).
    /// </summary>
    /// <param name="v">the input value</param>
    /// <returns>the absolute value</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float Abs (in float v)
    {
        return v < 0.0f ? -v : v;
    }

    /// <summary>
    /// A bounds checked approximation of the arc cosine for single precision
    /// real numbers. Returns a value in the range [0.0, PI] : PI when the input
    /// is less than or equal to -1.0; PI / 2.0 when the input is 0.0; 0.0 when
    /// the input is greater than or equal to 1.0. 
    ///
    /// Based on the algorithm at the Nvidia Cg 3.1 Toolkit Documentation,
    /// https://developer.download.nvidia.com/cg/acos.html . This cites M.
    /// Abramowitz and I.A. Stegun, Eds., Handbook of Mathematical Functions,
    /// possibly p. 83, which cites Approximations for Digital Computers by C.
    /// Hastings, Jr.
    /// </summary>
    /// <param name="value">the input value</param>
    /// <returns>the angle in radians</returns>
    public static float Acos (in float value)
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
        ret *= (float) Math.Sqrt (1.0d - (double) x);
        return ltZero ? Utils.Pi - ret : ret;
    }

    /// <summary>
    /// Evaluates two floats like booleans using the AND logic gate.
    /// </summary>
    /// <param name="a">the left operand</param>
    /// <param name="b">the right operand</param>
    /// <returns>the evaluation</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static int And (in float a, in float b)
    {
        return ((a != 0.0f) & (b != 0.0f)) ? 1 : 0;
    }

    /// <summary>
    /// A quick approximation test. Tests to see if the absolute of the
    /// difference between two values is less than a tolerance. Does not handle
    /// edge cases.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <param name="tolerance">the tolerance</param>
    /// <returns>the evaluation</returns>
    public static bool Approx (in float a, in float b, in float tolerance = Utils.Epsilon)
    {
        float diff = b - a;
        return diff <= tolerance && diff >= -tolerance;
    }

    /// <summary>
    /// A bounds checked approximation of the arc-sine for single precision real
    /// numbers. Returns a value in the range [-PI / 2.0, PI / 2.0] : -PI / 2.0
    /// when the input is less than or equal to -1.0; 0.0 when the input is 0.0;
    /// PI / 2.0 when the input is greater than or equal to 1.0.
    ///
    /// Based on the algorithm at the Nvidia Cg 3.1 Toolkit Documentation,
    /// https://developer.download.nvidia.com/cg/acos.html . This cites M.
    /// Abramowitz and I.A. Stegun, Eds., Handbook of Mathematical Functions,
    /// possibly p. 83, which cites Approximations for Digital Computers by C.
    /// Hastings, Jr.
    /// </summary>
    /// <param name="value">the input value</param>
    /// <returns>the angle in radians</returns>
    public static float Asin (in float value)
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
        ret = Utils.HalfPi - ret * (float) Math.Sqrt (1.0d - (double) x);
        return ltZero ? -ret : ret;
    }

    /// <summary>
    /// Finds a single precision approximation of a signed angle given a
    /// vertical and horizontal component. Note that the vertical component
    /// precedes the horizontal. The return value falls in the range [-PI, PI] .
    ///
    /// Based on the algorithm at the Nvidia Cg 3.1 Toolkit Documentation,
    /// https://developer.download.nvidia.com/cg/atan2.html .
    /// </summary>
    /// <param name="y">the y coordinate (the ordinate)</param>
    /// <param name="x">the x coordinate (the abscissa)</param>
    /// <returns>the angle in radians</returns>
    public static float Atan2 (in float y, in float x)
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

    /// <summary>
    /// Raises a real number to the next greatest integer.
    /// </summary>
    /// <param name="v">the input value</param>
    /// <returns>the raised value</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static int Ceil (in float v)
    {
        return v > 0.0f ? (int) (v + 1) : (int) v;
    }

    /// <summary>
    /// Clamps an real number between a lower and an upper bound.
    /// </summary>
    /// <param name="v">the input value</param>
    /// <param name="lb">the lower bound</param>
    /// <param name="ub">the upper bound</param>
    /// <returns>the clamped value</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float Clamp (in float v, in float lb = 0.0f, in float ub = 1.0f)
    {
        return (v < lb) ? lb : (v > ub) ? ub : v;
    }

    /// <summary>
    /// Clamps an integer between a lower and an upper bound.
    /// </summary>
    /// <param name="v">the input value</param>
    /// <param name="lb">the lower bound</param>
    /// <param name="ub">the upper bound</param>
    /// <returns>the clamped value</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static int Clamp (in int v, in int lb = int.MinValue, in int ub = int.MaxValue)
    {
        return (v < lb) ? lb : (v > ub) ? ub : v;
    }

    /// <summary>
    /// Returns the first floating-point argument with the sign of the second
    /// floating-point argument. 
    /// </summary>
    /// <param name="mag">the magnitude</param>
    /// <param name="sign">the sign</param>
    /// <returns>the magnified sign</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float CopySign (in float mag, in float sign)
    {
        return Utils.Abs (mag) * Utils.Sign (sign);
    }

    /// <summary>
    /// Finds the single-precision cosine of an angle in radians. Returns a
    /// value in the range [-1.0, 1.0] .
    /// </summary>
    /// <param name="radians">the angle in radians</param>
    /// <returns>the cosine of the angle</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float Cos (in float radians)
    {
        return (float) Math.Cos ((double) radians);
    }

    /// <summary>
    /// Finds the hyperbolic cosine of an angle in radians.
    /// </summary>
    /// <param name="radians">the angle in radians</param>
    /// <returns>the cosine of the angle</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float Cosh (in float radians)
    {
        return (float) Math.Cosh ((double) radians);
    }

    /// <summary>
    /// Finds the approximate cotangent of the angle in radians. Equivalent to
    /// dividing the cosine of the angle by the sine, or to 1.0 / tan ( a ) .
    /// </summary>
    /// <param name="radians">the angle in radians</param>
    /// <returns>the cotangent</returns>
    public static float Cot (in float radians)
    {
        double rd = (double) radians;
        double sint = Math.Sin (rd);
        return (sint != 0.0d) ? (float) (Math.Cos (rd) / sint) : 0.0f;
    }

    /// <summary>
    /// Finds the absolute value of the left operand minus the right.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the difference</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float Diff (in float a, in float b)
    {
        return Utils.Abs (a - b);
    }

    /// <summary>
    /// Divides the left operand by the right, but returns zero when the
    /// denominator is zero.
    /// </summary>
    /// <param name="a">numerator</param>
    /// <param name="b">denominator</param>
    /// <returns>the quotient</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static int Div (in int a, in int b)
    {
        return (b != 0) ? a / b : 0;
    }

    /// <summary>
    /// Divides the left operand by the right, but returns zero when the
    /// denominator is zero.
    /// </summary>
    /// <param name="a">numerator</param>
    /// <param name="b">denominator</param>
    /// <returns>the quotient</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float Div (in float a, in float b)
    {
        return (b != 0.0f) ? a / b : 0.0f;
    }

    /// <summary>
    /// Finds Euler's number, 2.7182817, raised to power of the input value.
    /// </summary>
    /// <param name="v">the input value</param>
    /// <returns>the result</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float Exp (in float v)
    {
        return (float) Math.Exp ((double) v);
    }

    /// <summary>
    /// Returns the value if it is greater than the lower bound, inclusive, and
    /// less than the upper bound, exclusive. Otherwise, returns 0.0 .
    /// </summary>
    /// <param name="v">input value</param>
    /// <param name="lb">lower bound</param>
    /// <param name="ub">upper bound</param>
    /// <returns>the filtered value</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float Filter (in float v, in float lb = 0.0f, in float ub = 1.0f)
    {
        return v >= lb && v < ub ? v : 0.0f;
    }

    /// <summary>
    /// Floors a real number to the next least integer.
    /// </summary>
    /// <param name="v">the input value</param>
    /// <returns>the floored value</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static int Floor (in float v)
    {
        return v > 0.0f ? (int) v : (int) v - 1;
    }

    /// <summary>
    /// Applies the modulo operator (%) to the operands.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the result</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static int Fmod (in int a, in int b)
    {
        return (b != 0) ? a % b : a;
    }

    /// <summary>
    /// Applies the modulo operator (%) to the operands, implicitly using the
    /// formula fmod ( a, b ) := a - b trunc ( a / b ) .
    ///
    /// When the left operand is negative and the right operand is positive, the
    /// result will be negative. For periodic values, such as an angle, where
    /// the direction of change could be either clockwise or counterclockwise,
    /// use mod.
    ///
    /// If the right operand is one, use fract ( a ) or a - trunc ( a ) instead.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the result</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float Fmod (in float a, in float b)
    {
        return (b != 0.0f) ? a % b : a;
    }

    /// <summary>
    /// Finds the signed fractional portion of the input value by subtracting the value's truncation from the value.
    /// </summary>
    /// <param name="v">the input value</param>
    /// <returns>the fractional portion</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float Fract (in float v)
    {
        return v - Utils.Trunc (v);
    }

    /// <summary>
    /// Finds one divided by the square root of an input value. Returns zero if
    /// the input is zero.
    /// </summary>
    /// <param name="v">input value</param>
    /// <returns>the inverse square root</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float InvSqrt (in float v)
    {
        return (v > 0.0f) ? Utils.InvSqrtUnchecked (v) : 0.0f;
    }

    /// <summary>
    /// Finds one divided by the square root of an input value. Does not check
    /// if the input is zero.
    /// </summary>
    /// <param name="a">the input value</param>
    /// <returns>the inverse square root</returns>
    public static float InvSqrtUnchecked (in float a)
    {
        // return (float) (1.0d / Math.Sqrt ((double) a));

        float vhalf = a * 0.5f;
        float y = 1.792843f - 0.8537347f * a;
        y *= 1.5f - vhalf * y * y;
        y *= 1.5f - vhalf * y * y;
        y *= 1.5f - vhalf * y * y;
        return y;
    }

    /// <summary>
    /// Eases from an origin angle in radians to a destination angle using the
    /// shortest direction (either clockwise or counter clockwise) according to
    /// a factor in [0.0, 1.0] .
    /// </summary>
    /// <param name="origin">origin angle</param>
    /// <param name="dest">destination angle</param>
    /// <param name="t">factor</param>
    /// <returns>the angle</returns>
    public static float LerpAngle (in float origin, in float dest, in float t = 0.5f)
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

    /// <summary>
    /// Finds the linear step between a left and right edge given an input
    /// factor.
    /// </summary>
    /// <param name="edge0">left edge</param>
    /// <param name="edge1">right edge</param>
    /// <param name="x">factor</param>
    /// <returns>the linear step</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float LinearStep (in float edge0 = 0.0f, in float edge1 = 1.0f, in float x = 0.5f)
    {
        float denom = edge1 - edge0;
        if (denom != 0.0f) return Utils.Clamp ((x - edge0) / denom, 0.0f, 1.0f);
        return 0.0f;
    }

    /// <summary>
    /// Finds the natural logarithm of the input value cast to a single
    /// precision real number.
    /// </summary>
    /// <param name="v">input value</param>
    /// <returns>the natural logarithm</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float Log (in float v)
    {
        return (v > 0.0f) ? (float) Math.Log ((double) v) : 0.0f;
    }

    /// <summary>
    /// Maps an input value from an original range to a target range. If the
    /// upper and lower bound of the original range are equal, will return the
    /// lower bound of the destination range.
    /// </summary>
    /// <param name="v">the input value</param>
    /// <param name="lbOrigin">lower bound of original range</param>
    /// <param name="ubOrigin">upper bound of original range</param>
    /// <param name="lbDest">lower bound of destination range</param>
    /// <param name="ubDest">upper bound of destination range</param>
    /// <returns>the mapped value</returns>
    public static float Map (in float v, in float lbOrigin = -1.0f, in float ubOrigin = 1.0f, in float lbDest = 0.0f, in float ubDest = 1.0f)
    {
        float denom = ubOrigin - lbOrigin;
        return denom != 0.0f ?
            lbDest + (ubDest - lbDest) *
            ((v - lbOrigin) / denom) : lbDest;
    }

    /// <summary>
    /// Finds the greater, or maximum, of two values.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the maximum value</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float Max (in float a, in float b)
    {
        return a >= b ? a : a < b ? b : 0.0f;
    }

    /// <summary>
    /// Finds the greatest, or maximum, among three values.
    /// </summary>
    /// <param name="a">first operand</param>
    /// <param name="b">second operand</param>
    /// <param name="c">third operand</param>
    /// <returns>the maximum value</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float Max (in float a, in float b, in float c)
    {
        return Utils.Max (Utils.Max (a, b), c);
    }

    /// <summary>
    /// Finds the greatest, or maximum, among a list of values.
    /// </summary>
    /// <param name="values">the list of values</param>
    /// <returns>the maximum value</returns>
    public static float Max (params float[ ] values)
    {
        int len = values.Length;
        float result = float.MinValue;
        for (int i = 0; i < len; ++i)
        {
            float v = values[i];
            result = (v > result) ? v : result;
        }
        return result;
    }

    /// <summary>
    /// Finds the lesser, or minimum, of two values.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the minimum value</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float Min (in float a, in float b)
    {
        return a <= b ? a : a > b ? b : 0.0f;
    }

    /// <summary>
    /// Finds the least, or minimum, among three values.
    /// </summary>
    /// <param name="a">first operand</param>
    /// <param name="b">second operand</param>
    /// <param name="c">third operand</param>
    /// <returns>the minimum value</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float Min (in float a, in float b, in float c)
    {
        return Utils.Min (Utils.Min (a, b), c);
    }

    /// <summary>
    /// Finds the least, or minimum, among a list of values.
    /// </summary>
    /// <param name="values">the list of values</param>
    /// <returns>the minimum value</returns>
    public static float Min (params float[ ] values)
    {
        int len = values.Length;
        float result = float.MaxValue;
        for (int i = 0; i < len; ++i)
        {
            float v = values[i];
            result = (v < result) ? v : result;
        }
        return result;
    }

    /// <summary>
    /// Mixes two values by a factor. The mix is unclamped by default.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <param name="t">factor</param>
    /// <returns>the mix</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float Mix (in float a, in float b, in float t = 0.5f)
    {
        return (1.0f - t) * a + t * b;
    }

    /// <summary>
    /// Applies floor modulo to the operands. Returns the left operand when the
    /// right operand is zero.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the result</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static int Mod (in int a, in int b)
    {
        if (b != 0)
        {
            int result = a - b * (a / b);
            return result < 0 ? result + b : result;
        }
        return a;
    }

    /// <summary>
    /// Applies floor modulo to the operands. Returns the left operand when the
    /// right operand is zero.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the result</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float Mod (in float a, in float b)
    {
        return b != 0.0f ? a - b * Utils.Floor (a / b) : a;
    }

    /// <summary>
    /// Subtracts the floor of the input value from the value. Returns a
    /// positive value in the range [0.0, 1.0] .
    /// </summary>
    /// <param name="v">input value</param>
    /// <returns>the wrapped value</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float Mod1 (in float v)
    {
        return v - Utils.Floor (v);
    }

    /// <summary>
    /// A specialized version of mod which shifts an angle in degrees to the
    /// range [0.0, 360.0] .
    /// </summary>
    /// <param name="deg">angle in degrees</param>
    /// <returns>the output angle</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float ModDegrees (in float deg)
    {
        return deg - 360.0f * Utils.Floor (deg * Utils.One360);
    }

    /// <summary>
    /// A specialized version of mod which shifts an angle in radians to the
    /// range [0.0, TAU] .
    /// </summary>
    /// <param name="rad">angle in radians</param>
    /// <returns>the output angle</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float ModRadians (in float rad)
    {
        return rad - Utils.Tau * Utils.Floor (rad * Utils.OneTau);
    }

    /// <summary>
    /// Finds the negation of a float holding a boolean value.
    /// </summary>
    /// <param name="v">the input value</param>
    /// <returns>the negation</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static int Not (in float v)
    {
        return v != 0.0f ? 0 : 1;
    }

    /// <summary>
    /// Evaluates two floats like booleans, using the inclusive or (OR) logic
    /// gate.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the evaluation</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static int Or (in float a, in float b)
    {
        return ((a != 0.0f) | (b != 0.0f)) ? 1 : 0;
    }

    /// <summary>
    /// Oscillates between [0.0, 1.0] based on an input step.
    ///
    /// Uses a different formula than the Unity math function of the same name:
    /// 0.5 + 0.5 * cos ( step / TAU ) .
    /// </summary>
    /// <param name="t">the input value</param>
    /// <returns>the oscillation</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float PingPong (in float t)
    {
        return 0.5f + 0.5f * Utils.SinCosEval (t);
    }

    /// <summary>
    /// Oscillates between a lower and upper bound based on an input step.
    /// </summary>
    /// <param name="a">lower bound</param>
    /// <param name="b">upper bound</param>
    /// <param name="t">factor</param>
    /// <returns>the oscillation</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static int PingPong (in int a, in int b, in float t)
    {
        return (int) Utils.PingPong ((float) a, (float) b, t);
    }

    /// <summary>
    /// Oscillates between a lower and upper bound based on an input step.
    /// </summary>
    /// <param name="a">lower bound</param>
    /// <param name="b">upper bound</param>
    /// <param name="t">factor</param>
    /// <returns>the oscillation</returns>
    public static float PingPong (in float a, in float b, in float t)
    {
        float x = 0.5f + 0.5f * Utils.SinCosEval (t);
        return (1.0f - x) * a + x * b;
    }

    /// <summary>
    /// Finds the single-precision of a number raised to the power of another.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the power</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float Pow (in float a, in float b)
    {
        return (float) Math.Pow ((double) a, (double) b);
    }

    /// <summary>
    /// Reduces the signal, or granularity, of a value. Applied to a color, this
    /// yields the 'posterization' effect. Applied to a vector, this yields a
    /// crenelated effect. Any level less than 2 returns the value unaltered.
    /// </summary>
    /// <param name="v">input value</param>
    /// <param name="levels">levels</param>
    /// <returns>the quantized value</returns>
    public static float Quantize (in float v, in int levels)
    {
        if (levels < 2) return v;
        float lf = (float) levels;
        return Utils.Floor (0.5f + v * lf) / lf;
    }

    /// <summary>
    /// Rounds a value to an integer based on whether its fractional portion is
    /// greater than or equal to plus or minus 0.5 .
    /// </summary>
    /// <param name="v">input value</param>
    /// <returns>rounded value</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static int Round (in float v)
    {
        return (v < -0.0f) ? (int) (v - 0.5f) :
            (v > 0.0f) ? (int) (v + 0.5f) : 0;
    }

    /// <summary>
    /// Rounds a value to a number of places right of the decimal point.
    /// Promotes the float to a double, rounds it, then demotes back to a
    /// float.</summary>
    /// <param name="v">input value</param>
    /// <param name="places">number of places</param>
    /// <returns>the rounded value</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float Round (in float v, in int places)
    {
        return (float) Math.Round ((double) v, places);
    }

    /// <summary>
    /// Finds the sign of an input value. Returns the integer 0 for both -0.0
    /// (signed negative zero) and 0.0 (signed positive zero).
    /// </summary>
    /// <param name="v">input value</param>
    /// <returns>the sign</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static int Sign (in float v)
    {
        return (v < -0.0f) ? -1 : (v > 0.0f) ? 1 : 0;
    }

    /// <summary>
    /// Finds the single precision sine of an angle in radians. Returns a value
    /// in the range [-1.0, 1.0] .
    /// </summary>
    /// <param name="radians">angle in radians</param>
    /// <returns>the sine of the angle</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float Sin (in float radians)
    {
        return (float) Math.Sin ((double) radians);
    }

    /// <summary>
    /// Finds the sine and cosine of an angle in radians. Assigns the values to
    /// output variables.
    /// </summary>
    /// <param name="radians">angle in radians</param>
    /// <param name="sina">sine</param>
    /// <param name="cosa">cosine</param>
    public static void SinCos (in float radians, out float sina, out float cosa)
    {
        float nrm = Utils.OneTau * radians;
        sina = Utils.SinCosEval (nrm - 0.25f);
        cosa = Utils.SinCosEval (nrm);
    }

    /// <summary>
    /// Finds the sine and cosine of an angle in radians. Returns a named tuple.
    /// </summary>
    /// <param name="radians">angle in radians</param>
    /// <returns>the tuple</returns>
    public static (float sin, float cos) SinCos (in float radians)
    {
        float nrm = Utils.OneTau * radians;
        return (
            sin: Utils.SinCosEval (nrm - 0.25f),
            cos: Utils.SinCosEval (nrm));
    }

    /// <summary>
    /// A helper method to facilitate the approximate sine and cosine of an
    /// angle with single precision real numbers. The radians supplied to this
    /// function should be normalized through division by Tau . Subtract 0.25
    /// from the input value to return the sine instead of the cosine. 
    ///
    /// This is based on the algorithm described at
    /// Nvidia Cg 3.1 Toolkit Documentation,
    /// https://developer.download.nvidia.com/cg/sin.html .
    /// </summary>
    /// <param name="normRad">the normalized radians</param>
    /// <returns>the approximate value</returns>
    private static float SinCosEval (in float normRad)
    {
        float r1y = Utils.Mod1 (normRad);

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

    /// <summary>
    /// Finds the hyperbolic sine of an angle in radians cast to a single
    /// precision real number.
    /// </summary>
    /// <param name="radians">the angle</param>
    /// <returns>the hyperbolic sine</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float Sinh (in float radians)
    {
        return (float) Math.Sinh ((double) radians);
    }

    /// <summary>
    /// Finds the smooth step between a left and right edge given an input
    /// factor.
    /// </summary>
    /// <param name="edge0">left edge</param>
    /// <param name="edge1">right edge</param>
    /// <param name="x">factor</param>
    /// <returns>the smooth step</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float SmoothStep (in float edge0 = 0.0f, in float edge1 = 1.0f, in float x = 0.5f)
    {
        float t = Utils.LinearStep (edge0, edge1, x);
        return t * t * (3.0f - (t + t));
    }

    /// <summary>
    /// Returns the square root of a value cast to a float. If the value is less
    /// than or equal to zero, returns zero.
    /// </summary>
    /// <param name="v">the input value</param>
    /// <returns>the square root</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float Sqrt (in float v)
    {
        return v > 0.0f ? Utils.SqrtUnchecked (v) : 0.0f;
    }

    /// <summary>
    /// Returns the square root of a value cast to a float. Does not check to
    /// see if the input is greater than zero.
    /// </summary>
    /// <param name="v">the input value</param>
    /// <returns>the square root</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float SqrtUnchecked (in float v)
    {
        // return v * Utils.InvSqrtUnchecked (v);
        return (float) Math.Sqrt ((double) v);
    }

    /// <summary>
    /// Finds a step, either 0.0 or 1.0, based on an edge and factor.
    /// </summary>
    /// <param name="edge">the edge</param>
    /// <param name="x">the factor</param>
    /// <returns>the step</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float Step (in float edge, in float x = 0.5f)
    {
        return x < edge ? 0.0f : 1.0f;
    }

    /// <summary>
    /// Returns a String representation of a single precision real number
    /// truncated to the number of places.
    /// </summary>
    /// <param name="v">the input value</param>
    /// <param name="places">the number of places</param>
    /// <returns>the string</returns>
    public static string ToFixed (in float v, in int places = 7)
    {
        /*
         * Dispense with v and places edge cases.
         */
        if (float.IsNaN (v)) return "0.0";
        if (places < 0) return ((int) v).ToString ( );
        if (places < 1) return ((float) ((int) v)).ToString ( );
        if (v <= float.MinValue || v >= float.MaxValue)
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
            frac -= (float) tr;
            sb.Append (tr);
        }
        return sb.ToString ( );
    }

    /// <summary>
    /// Returns an integer formatted as a string padded by initial zeroes.
    /// </summary>
    /// <param name="value">integer</param>
    /// <param name="places">number of places</param>
    /// <returns>the string</returns>
    public static String ToPadded (in int value, in int places = 3)
    {
        /*
         * Double precision is needed to preserve accuracy. The max integer value
         * is 2147483647, which is 10 digits long. The sign needs to be flipped
         * because working with positive absolute value would allow
         * the minimum value to overflow to zero.
         */

        bool isNeg = value < 0;
        int nAbsVal = isNeg ? value : -value;
        int[ ] digits = new int[10];
        int filled = 0;
        while (nAbsVal < 0)
        {
            double y = nAbsVal * 0.1d;
            nAbsVal = (int) y;
            digits[filled++] = -(int) ((y - nAbsVal) * 10.0d - 0.5d);
        }

        StringBuilder sb = new StringBuilder (16);
        if (isNeg) { sb.Append ('-'); }
        int vplaces = filled > places ? filled : places;
        for (int n = vplaces - 1; n > -1; --n)
        {
            sb.Append (digits[n]);
        }

        return sb.ToString ( );
    }

    /// <summary>
    /// Truncates the input value. This is an alias for explicitly casting a
    /// float to an integer, then implicitly casting the integral to a float.
    /// </summary>
    /// <param name="a">the input value</param>
    /// <returns>the truncation</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static float Trunc (in float a)
    {
        return (float) ((int) a);
    }

    /// <summary>
    /// Wraps a value around a periodic range as defined by an upper and lower
    /// bound: lower bounds inclusive; upper bounds exclusive. Due to single
    /// precision accuracy, results will be inexact. In cases where the lower
    /// bound is greater than the upper bound, the two will be swapped. In cases
    /// where the range is 0.0, 0.0 will be returned.
    /// </summary>
    /// <param name="value">input value</param>
    /// <param name="lb">lower bound</param>
    /// <param name="ub">upper bound</param>
    /// <returns>wrapped value</returns>
    public static float Wrap (in float value, in float lb = -1.0f, in float ub = 1.0f)
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

    /// <summary>
    /// Evaluates two floats like booleans, using the exclusive or (XOR) logic gate.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the evaluation</returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static int Xor (in float a, in float b)
    {
        return ((a != 0.0f) ^ (b != 0.0f)) ? 1 : 0;
    }
}