using System;
using System.Text;

/// <summary>
/// Implements geometric math utilities for single-precision numbers.
/// </summary>
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
    /// Magnitude for orthogonal handles when four curve
    /// knots are used to approximate an ellipse or circle
    /// (90 degrees per knot), Derived from
    /// (Math.Sqrt(2.0) - 1.0) * 4.0 / 3.0 .
    /// </summary>
    public const float Kappa = 0.552285f;

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
    /// An angle in radians is multiplied by this constant to convert it to
    /// degrees. 180.0 / PI , approximately 57.29578 .
    /// </summary>
    public const float RadToDeg = 57.29578f;

    /// <summary>
    /// An approximation of sqrt ( 3.0 ) , 1.7320508 .
    /// </summary>
    public const float Sqrt3 = 1.7320508f;

    /// <summary>
    /// An approximation of sqrt ( 3.0 ) / 2.0 , 0.8660254 .
    /// </summary>
    public const float Sqrt32 = 0.8660254f;

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
    /// Evaluates two floats like booleans using the AND logic gate.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>evaluation</returns>
    public static int And(in float a, in float b)
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
    /// <param name="tolerance">tolerance</param>
    /// <returns>evaluation</returns>
    public static bool Approx(in float a, in float b, in float tolerance = Utils.Epsilon)
    {
        return Utils.Diff(a, b) <= tolerance;
    }

    /// <summary>
    /// Raises a real number to the next greatest integer.
    /// </summary>
    /// <param name="v">input value</param>
    /// <returns>raised value</returns>
    public static int Ceil(in float v)
    {
        return (v > 0.0f) ? (int)v + 1 : (int)v;
    }

    /// <summary>
    /// Clamps an real number between a lower and an upper bound.
    /// </summary>
    /// <param name="v">input value</param>
    /// <param name="lb">lower bound</param>
    /// <param name="ub">upper bound</param>
    /// <returns>clamped value</returns>
    public static float Clamp(in float v, in float lb = 0.0f, in float ub = 1.0f)
    {
        return (v < lb) ? lb : (v > ub) ? ub : v;
    }

    /// <summary>
    /// Clamps an integer between a lower and an upper bound.
    /// </summary>
    /// <param name="v">input value</param>
    /// <param name="lb">lower bound</param>
    /// <param name="ub">upper bound</param>
    /// <returns>clamped value</returns>
    public static int Clamp(in int v, in int lb = int.MinValue, in int ub = int.MaxValue)
    {
        return (v < lb) ? lb : (v > ub) ? ub : v;
    }

    /// <summary>
    /// Returns the first floating-point argument with the sign of the second
    /// floating-point argument. Returns zero if the sign is zero.
    /// </summary>
    /// <param name="mag">magnitude</param>
    /// <param name="sign">sign</param>
    /// <returns>magnified sign</returns>
    public static float CopySign(in float mag, in float sign)
    {
        // Don't use abs * sign, as the latter has more
        // flexibility in terms of how you deal with zero sign.
        // return Utils.Abs (mag) * Utils.Sign (sign);
        return (sign < -0.0f) ? -MathF.Abs(mag) :
               (sign > 0.0f) ? MathF.Abs(mag) :
               0.0f;
    }

    /// <summary>
    /// Finds the approximate cotangent of the angle in radians. Equivalent to
    /// dividing the cosine of the angle by the sine, or to 1.0 / tan ( a ) .
    /// </summary>
    /// <param name="radians">angle in radians</param>
    /// <returns>cotangent</returns>
    public static float Cot(in float radians)
    {
        double rd = (double)radians;
        double sint = Math.Sin(rd);
        return (sint != 0.0d) ? (float)(Math.Cos(rd) / sint) : 0.0f;
    }

    /// <summary>
    /// Finds the absolute value of the right operand minus the left.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>difference</returns>
    public static float Diff(in float a, in float b)
    {
        return MathF.Abs(b - a);
    }

    /// <summary>
    /// Finds the distance between two angles.
    /// Angles are expected to be in radians.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>distance</returns>
    public static float DistAngle(in float a, in float b)
    {
        return Utils.DistAngleSigned(a, b);
    }

    /// <summary>
    /// Finds the distance between two periodic values.
    /// Example ranges are 360.0 for degrees, 1.0 for hues or
    /// Tau for radians.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>distance</returns>
    public static float DistAngle(in float a, in float b, in float range)
    {
        return Utils.DistAngleSigned(a, b, range);
    }

    /// <summary>
    /// Finds the signed distance between two angles.
    /// Angles are expected to be in radians.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>signed distance</returns>
    public static float DistAngleSigned(in float a, in float b)
    {
        float diff = (a - b + MathF.PI) % Utils.Tau - MathF.PI;
        return diff < -MathF.PI ? diff + Utils.Tau : diff;
    }

    /// <summary>
    /// Finds the signed distance between two periodic values.
    /// Example ranges are 360.0 for degrees, 1.0 for hues or
    /// Tau for radians.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <param name="range">range</param>
    /// <returns>signed distance</returns>
    public static float DistAngleSigned(in float a, in float b, in float range)
    {
        float halfRange = range * 0.5f;
        float diff = (a - b + halfRange) % range - halfRange;
        return diff < -halfRange ? diff + range : diff;
    }

    /// <summary>
    /// Finds the unsigned distance between two angles.
    /// Angles are expected to be in radians.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>unsigned distance</returns>
    public static float DistAngleUnsigned(in float a, in float b)
    {
        return MathF.PI - MathF.Abs(MathF.Abs(Utils.WrapRadians(b) -
            Utils.WrapRadians(a)) - MathF.PI);
    }

    /// <summary>
    /// Finds the unsigned distance between two periodic values.
    /// Example ranges are 360.0 for degrees, 1.0 for hues or
    /// Tau for radians.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <param name="range">range</param>
    /// <returns>unsigned distance</returns>
    public static float DistAngleUnsigned(in float a, in float b, in float range)
    {
        float halfRange = range * 0.5f;
        return halfRange - MathF.Abs(MathF.Abs(
            Utils.RemFloor(b, range) -
            Utils.RemFloor(a, range)) - halfRange);
    }

    /// <summary>
    /// Divides the left operand by the right, but returns zero when the
    /// denominator is zero.
    /// </summary>
    /// <param name="a">numerator</param>
    /// <param name="b">denominator</param>
    /// <returns>quotient</returns>
    public static int Div(in int a, in int b)
    {
        return b != 0 ? a / b : 0;
    }

    /// <summary>
    /// Divides the left operand by the right, but returns zero when the
    /// denominator is zero.
    /// </summary>
    /// <param name="a">numerator</param>
    /// <param name="b">denominator</param>
    /// <returns>quotient</returns>
    public static float Div(in float a, in float b)
    {
        return b != 0.0f ? a / b : 0.0f;
    }

    /// <summary>
    /// Floors a real number to the next least integer.
    /// </summary>
    /// <param name="v">input value</param>
    /// <returns>floored value</returns>
    public static int Floor(in float v)
    {
        return (v > 0.0f) ? (int)v : (int)v - 1;
    }

    /// <summary>
    /// Finds the signed fractional portion of the input value by subtracting
    /// the value's truncation from the value. Not the same as GLSL fract.
    /// </summary>
    /// <param name="v">input value</param>
    /// <returns>fractional portion</returns>
    public static float Fract(in float v)
    {
        // TODO: Distinguish between FractTrunc and FractFloor?
        // Make Fract an alias for FractTrunc?
        return v - MathF.Truncate(v);
    }

    /// <summary>
    /// Finds one divided by the square root of an input value. Returns zero if
    /// the input is zero.
    /// </summary>
    /// <param name="v">input value</param>
    /// <returns>inverse square root</returns>
    public static float InvSqrt(in float v)
    {
        return (v > 0.0f) ? Utils.InvSqrtUnchecked(v) : 0.0f;
    }

    /// <summary>
    /// Finds one divided by the square root of an input value. Does not check
    /// if the input is zero.
    /// </summary>
    /// <param name="v">input value</param>
    /// <returns>inverse square root</returns>
    public static float InvSqrtUnchecked(in float v)
    {
        return (float)(1.0d / Math.Sqrt(v));
    }

    /// <summary>
    /// Eases from an origin angle in radians to a destination angle according to
    /// a factor in [0.0, 1.0] .
    /// </summary>
    /// <param name="origin">origin angle</param>
    /// <param name="dest">destination angle</param>
    /// <param name="t">factor</param>
    /// <param name="range">range</param>
    /// <returns>angle</returns>
    public static float LerpAngle(
        in float origin,
        in float dest,
        in float t = 0.5f,
        in float range = Utils.Tau)
    {
        return Utils.LerpAngleNear(origin, dest, t, range);
    }

    /// <summary>
    /// Eases from an origin angle in radians to a destination angle using the
    /// shortest direction (either clockwise or counter clockwise) according to
    /// a factor in [0.0, 1.0] .
    /// </summary>
    /// <param name="origin">origin angle</param>
    /// <param name="dest">destination angle</param>
    /// <param name="t">factor</param>
    /// <param name="range">range</param>
    /// <returns>angle</returns>
    public static float LerpAngleNear(
        in float origin,
        in float dest,
        in float t = 0.5f,
        in float range = Utils.Tau)
    {
        float o = Utils.RemFloor(origin, range);
        float d = Utils.RemFloor(dest, range);
        float diff = d - o;
        float halfRange = range * 0.5f;

        if (diff == 0.0f) { return o; }

        if (o < d && diff > halfRange)
        {
            return Utils.RemFloor(
                (1.0f - t) * (o + range) +
                t * d,
                range);
        }

        if (o > d && diff < -halfRange)
        {
            return Utils.RemFloor(
                (1.0f - t) * o +
                t * (d + range),
                range);
        }

        return (1.0f - t) * o + t * d;
    }

    /// <summary>
    /// Eases from an origin angle in radians to a destination angle using the
    /// furthest direction (either clockwise or counter clockwise) according to
    /// a factor in [0.0, 1.0] .
    /// </summary>
    /// <param name="origin">origin angle</param>
    /// <param name="dest">destination angle</param>
    /// <param name="t">factor</param>
    /// <param name="range">range</param>
    /// <returns>angle</returns>
    public static float LerpAngleFar(
        in float origin,
        in float dest,
        in float t = 0.5f,
        in float range = Utils.Tau)
    {
        float o = Utils.RemFloor(origin, range);
        float d = Utils.RemFloor(dest, range);
        float diff = d - o;
        float halfRange = range * 0.5f;

        if (diff == 0.0f || (o < diff && diff < -halfRange))
        {
            return Utils.RemFloor(
                (1.0f - t) * (o + range) + t * d,
                range);
        }

        if (o > d && diff > -halfRange)
        {
            return Utils.RemFloor(
                (1.0f - t) * o + t * (d + range),
                range);
        }

        return (1.0f - t) * o + t * d;
    }

    /// <summary>
    /// Eases from an origin angle in radians to a destination angle using the
    /// counter clockwise direction according to
    /// a factor in [0.0, 1.0] .
    /// </summary>
    /// <param name="origin">origin angle</param>
    /// <param name="dest">destination angle</param>
    /// <param name="t">factor</param>
    /// <param name="range">range</param>
    /// <returns>angle</returns>
    public static float LerpAngleCCW(
        in float origin,
        in float dest,
        in float t = 0.5f,
        in float range = Utils.Tau)
    {
        float o = Utils.RemFloor(origin, range);
        float d = Utils.RemFloor(dest, range);
        float diff = d - o;

        if (diff == 0.0f) { return o; }

        if (o > d)
        {
            return Utils.RemFloor(
                (1.0f - t) * o +
                t * (d + range),
                range);
        }

        return (1.0f - t) * o + t * d;
    }

    /// <summary>
    /// Eases from an origin angle in radians to a destination angle using the
    /// clockwise direction according to
    /// a factor in [0.0, 1.0] .
    /// </summary>
    /// <param name="origin">origin angle</param>
    /// <param name="dest">destination angle</param>
    /// <param name="t">factor</param>
    /// <param name="range">range</param>
    /// <returns>angle</returns>
    public static float LerpAngleCW(
        in float origin,
        in float dest,
        in float t = 0.5f,
        in float range = Utils.Tau)
    {
        float o = Utils.RemFloor(origin, range);
        float d = Utils.RemFloor(dest, range);
        float diff = d - o;

        if (diff == 0.0f) { return d; }

        if (o < d)
        {
            return Utils.RemFloor(
                (1.0f - t) * (o + range) +
                t * d,
                range);
        }

        return (1.0f - t) * o + t * d;
    }

    /// <summary>
    /// Finds the linear step between a left and right edge given an input
    /// factor.
    /// </summary>
    /// <param name="edge0">left edge</param>
    /// <param name="edge1">right edge</param>
    /// <param name="x">factor</param>
    /// <returns>linear step</returns>
    public static float LinearStep(
        in float edge0 = 0.0f,
        in float edge1 = 1.0f,
        in float x = 0.5f)
    {
        float denom = edge1 - edge0;
        if (denom != 0.0f) { return Utils.Clamp((x - edge0) / denom, 0.0f, 1.0f); }
        return 0.0f;
    }

    /// <summary>
    /// Finds the greater, or maximum, of two values.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>maximum value</returns>
    public static int Max(in int a, in int b)
    {
        return (a > b) ? a : b;
    }

    /// <summary>
    /// Finds the greatest, or maximum, among three values.
    /// </summary>
    /// <param name="a">first operand</param>
    /// <param name="b">second operand</param>
    /// <param name="c">third operand</param>
    /// <returns>maximum value</returns>
    public static int Max(in int a, in int b, in int c)
    {
        return Utils.Max(Utils.Max(a, b), c);
    }

    /// <summary>
    /// Finds the greatest, or maximum, among three values.
    /// </summary>
    /// <param name="a">first operand</param>
    /// <param name="b">second operand</param>
    /// <param name="c">third operand</param>
    /// <returns>maximum value</returns>
    public static float Max(in float a, in float b, in float c)
    {
        return MathF.Max(MathF.Max(a, b), c);
    }

    /// <summary>
    /// Finds the greatest, or maximum, among a list of values.
    /// Returns zero if the list contains no values.
    /// </summary>
    /// <param name="values">list of values</param>
    /// <returns>maximum value</returns>
    public static float Max(params float[] values)
    {
        int len = values.Length;
        if (len < 1) { return 0.0f; }
        float result = float.MinValue;
        for (int i = 0; i < len; ++i)
        {
            float v = values[i];
            if (v > result) { result = v; }
        }
        return result;
    }

    /// <summary>
    /// Finds the lesser, or minimum, of two values.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>minimum value</returns>
    public static int Min(in int a, in int b)
    {
        return (a < b) ? a : b;
    }

    /// <summary>
    /// Finds the lesser, or minimum, among three values.
    /// </summary>
    /// <param name="a">first operand</param>
    /// <param name="b">second operand</param>
    /// <param name="c">third operand</param>
    /// <returns>maximum value</returns>
    public static int Min(in int a, in int b, in int c)
    {
        return Utils.Min(Utils.Min(a, b), c);
    }

    /// <summary>
    /// Finds the least, or minimum, among three values.
    /// </summary>
    /// <param name="a">first operand</param>
    /// <param name="b">second operand</param>
    /// <param name="c">third operand</param>
    /// <returns>minimum value</returns>
    public static float Min(in float a, in float b, in float c)
    {
        return MathF.Min(MathF.Min(a, b), c);
    }

    /// <summary>
    /// Finds the least, or minimum, among a list of values.
    /// Returns zero if the list contains no values.
    /// </summary>
    /// <param name="values">list of values</param>
    /// <returns>minimum value</returns>
    public static float Min(params float[] values)
    {
        int len = values.Length;
        if (len < 1) { return 0.0f; }
        float result = float.MaxValue;
        for (int i = 0; i < len; ++i)
        {
            float v = values[i];
            if (v < result) { result = v; }
        }
        return result;
    }

    /// <summary>
    /// Mixes two values by a factor. The mix is unclamped by default.
    /// </summary>
    /// <param name="o">origin</param>
    /// <param name="d">destination</param>
    /// <param name="t">factor</param>
    /// <returns>mix</returns>
    public static float Mix(in float o, in float d, in float t = 0.5f)
    {
        return (1.0f - t) * o + t * d;
    }

    /// <summary>
    /// Generates a random number with normal distribution.
    /// Based on the Box-Muller transform as described here:
    /// https://www.wikiwand.com/en/Box%E2%80%93Muller_transform
    /// Could generate a tuple of numbers, but only returns the
    /// x coordinate.
    /// </summary>
    /// <param name="rng">input value</param>
    /// <param name="sigma">standard deviation</param>
    /// <param name="mu">mean</param>
    /// <returns>random number</returns>
    public static float NextGaussian(
        in System.Random rng,
        in float sigma = 1.0f,
        in float mu = 0.0f)
    {
        double u1, u2;
        do
        {
            u1 = rng.NextDouble();
        } while (u1 <= Utils.Epsilon);
        u2 = rng.NextDouble();

        double mag = sigma * Math.Sqrt(-2.0d * Math.Log(u1));
        double tau = 6.283185307179586d;
        double x = mag * Math.Cos(tau * u2) + mu;
        // double y = mag * Math.Sin(tau * u2) + mu;
        return (float)x;
    }

    /// <summary>
    /// Finds the next power of 2 for a signed integer, i.e., multiplies
    /// the next power by the integer's sign. Returns zero if the input
    /// is zero.
    /// </summary>
    /// <param name="v">value</param>
    /// <returns>next power of two</returns>
    public static int NextPowerOf2(in int v)
    {
        if (v != 0)
        {
            int vSgn = 1;
            int vAbs = v;
            if (v < 0) { vAbs = -v; vSgn = -1; }
            int p = 1;
            while (p < vAbs) { p <<= 1; }
            return p * vSgn;
        }
        return 0;
    }

    /// <summary>
    /// Finds the negation of a float holding a boolean value.
    /// </summary>
    /// <param name="v">input value</param>
    /// <returns>negation</returns>
    public static int Not(in float v)
    {
        return (v != 0.0f) ? 0 : 1;
    }

    /// <summary>
    /// Evaluates two floats like booleans, using the inclusive or (OR) logic
    /// gate.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>evaluation</returns>
    public static int Or(in float a, in float b)
    {
        return ((a != 0.0f) | (b != 0.0f)) ? 1 : 0;
    }

    /// <summary>
    /// Oscillates between [0.0, 1.0] based on an input step.
    /// </summary>
    /// <param name="t">factor</param>
    /// <param name="pause">pause</param>
    /// <returns>oscillation</returns>
    public static float PingPong(in float t, in float pause = 1.0f)
    {
        return Utils.PingPong(0.0f, 1.0f, t, pause);
    }

    /// <summary>
    /// Oscillates between a lower and upper bound based on an input step.
    /// </summary>
    /// <param name="a">lower bound</param>
    /// <param name="b">upper bound</param>
    /// <param name="t">factor</param>
    /// <returns>oscillation</returns>
    public static int PingPong(in int a, in int b, in float t)
    {
        return (int)Utils.PingPong((float)a, (float)b, t, 1.0f);
    }

    /// <summary>
    /// Oscillates between a lower and upper bound based on an input step.
    /// </summary>
    /// <param name="a">lower bound</param>
    /// <param name="b">upper bound</param>
    /// <param name="t">factor</param>
    /// <param name="pause">pause</param>
    /// <returns>oscillation</returns>
    public static float PingPong(in float a, in float b, in float t, in float pause = 1.0f)
    {
        float x = 0.5f + 0.5f * pause * MathF.Sin(MathF.PI * (t - 0.5f));
        if (t <= 0.0f) { return a; }
        if (t >= 1.0f) { return b; }
        return (1.0f - x) * a + x * b;
    }

    /// <summary>
    /// Quantizes a signed number according to a number of levels.
    /// </summary>
    /// <param name="v">input value</param>
    /// <param name="levels">levels</param>
    /// <returns>quantized value</returns>
    public static float Quantize(in float v, in int levels)
    {
        return Utils.QuantizeSigned(v, levels);
    }

    /// <summary>
    /// Quantizes a signed number according to a number of levels.
    /// The quantization is centered about the range.
    /// </summary>
    /// <param name="v">input value</param>
    /// <param name="levels">levels</param>
    /// <returns>quantized value</returns>
    public static float QuantizeSigned(in float v, in int levels)
    {
        if (levels > 0)
        {
            float lf = (float)levels;
            return MathF.Floor(0.5f + v * lf) / lf;
        }
        return v;
    }

    /// <summary>
    /// Quantizes a positive number according to a number of levels.
    /// The quantization is based on the left edge.
    /// </summary>
    /// <param name="v">input value</param>
    /// <param name="levels">levels</param>
    /// <returns>quantized value</returns>
    public static float QuantizeUnsigned(in float v, in int levels)
    {
        if (levels > 1)
        {
            float lf = (float)levels;
            return MathF.Max(0.0f,
                (MathF.Ceiling(v * lf) - 1.0f) / (lf - 1.0f));
        }
        return MathF.Max(0.0f, v);
    }

    /// <summary>
    /// Maps an input value from an original range to a target range. If the
    /// upper and lower bound of the original range are equal, will return the
    /// value unchanged.
    /// </summary>
    /// <param name="v">input value</param>
    /// <param name="lbOrigin">lower bound of origin range</param>
    /// <param name="ubOrigin">upper bound of origin range</param>
    /// <param name="lbDest">lower bound of destination range</param>
    /// <param name="ubDest">upper bound of destination range</param>
    /// <returns>mapped value</returns>
    public static float Remap(
        in float v,
        in float lbOrigin = -1.0f,
        in float ubOrigin = 1.0f,
        in float lbDest = 0.0f,
        in float ubDest = 1.0f)
    {
        float denom = ubOrigin - lbOrigin;
        return (denom != 0.0f) ?
            lbDest + (ubDest - lbDest) *
            ((v - lbOrigin) / denom) : v;
    }

    /// <summary>
    /// Applies floor modulo to the operands. Returns the left operand when the
    /// right operand is zero.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>result</returns>
    public static int RemFloor(in int a, in int b)
    {
        return b != 0 ? (a % b + b) % b : a;
    }

    /// <summary>
    /// Applies floor modulo to the operands. Returns the left operand when the
    /// right operand is zero.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>result</returns>
    public static float RemFloor(in float a, in float b)
    {
        return b != 0.0f ? a - b * MathF.Floor(a / b) : a;
    }

    /// <summary>
    /// Applies the modulo operator (%) to the operands.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>result</returns>
    public static int RemTrunc(in int a, in int b)
    {
        return (b != 0) ? a % b : a;
    }

    /// <summary>
    /// Applies the modulo operator (%) to the operands, implicitly using the
    /// formula a - b trunc ( a / b ) .
    ///
    /// When the left operand is negative and the right operand is positive, the
    /// result will be negative. For periodic values, such as an angle, where
    /// the direction of change could be either clockwise or counterclockwise,
    /// use RemFloor.
    ///
    /// If the right operand is one, use fract ( a ) or a - trunc ( a ) instead.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>result</returns>
    public static float RemTrunc(in float a, in float b)
    {
        return (b != 0.0f) ? a % b : a;
    }

    /// <summary>
    /// Rounds a value to an integer based on whether its fractional portion is
    /// greater than or equal to plus or minus 0.5 .
    /// </summary>
    /// <param name="v">input value</param>
    /// <returns>rounded value</returns>
    public static int Round(in float v)
    {
        return (v < -0.0f) ? (int)(v - 0.5f) :
            (v > 0.0f) ? (int)(v + 0.5f) : 0;
    }

    /// <summary>
    /// Finds the smooth step between a left and right edge given an input
    /// factor.
    /// </summary>
    /// <param name="edge0">left edge</param>
    /// <param name="edge1">right edge</param>
    /// <param name="x">factor</param>
    /// <returns>smooth step</returns>
    public static float SmoothStep(
        in float edge0 = 0.0f,
        in float edge1 = 1.0f,
        in float x = 0.5f)
    {
        float t = Utils.LinearStep(edge0, edge1, x);
        return t * t * (3.0f - (t + t));
    }

    /// <summary>
    /// Finds a step, either 0.0 or 1.0, based on an edge and factor.
    /// </summary>
    /// <param name="edge">edge</param>
    /// <param name="x">factor</param>
    /// <returns>step</returns>
    public static float Step(in float edge, in float x = 0.5f)
    {
        return x < edge ? 0.0f : 1.0f;
    }

    /// <summary>
    /// Returns a string representation of a single precision real number
    /// truncated to the number of places.
    /// </summary>
    /// <param name="v">input value</param>
    /// <param name="places">number of places</param>
    /// <returns>string</returns>
    public static string ToFixed(in float v, in int places = 7)
    {
        return Utils.ToFixed(new StringBuilder(16), v, places).ToString();
    }

    /// <summary>
    /// Appends a string representation of a single precision real number
    /// truncated to the number of places to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="v">input value</param>
    /// <param name="places">number of places</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToFixed(in StringBuilder sb, in float v, in int places = 7)
    {
        // Dispense with v and places edge cases.
        if (float.IsNaN(v)) { return sb.Append("0.0"); }
        if (places < 0) { return sb.Append((int)v); }
        if (places < 1) { return sb.Append((float)((int)v)); }
        if (v <= float.MinValue || v >= float.MaxValue)
        {
            return sb.Append(v);
        }

        // Find the sign, the unsigned v and the
        // unsigned integral.
        bool ltZero = v < 0.0f;
        int sign = ltZero ? -1 : (v > 0.0f) ? 1 : 0;
        float abs = ltZero ? -v : v;
        int trunc = (int)abs;
        int oldLen = sb.Length;
        int len;

        // Start the string builder with the integral
        // and the sign.
        if (sign < 0)
        {
            sb.Append('-').Append(trunc);
            len = sb.Length - oldLen - 1;
        }
        else
        {
            sb.Append(trunc);
            len = sb.Length - oldLen;
        }
        sb.Append('.');

        // Find the number of places left to work with after the
        // integral. Any more than 9 and single-precision's
        // inaccuracy would make the effort worthless.
        //
        // For numbers with a big integral, there may not be much
        // left to work with, and so fewer than the requested
        // number of places will be used.
        int maxPlaces = 9 - len;
        if (maxPlaces < 1) { return sb.Append(v); }
        int vetPlaces = places < maxPlaces ? places : maxPlaces;

        // Separate each digit by subtracting the truncation from
        // the v (fract), then multiplying by 10 to shift the
        // next digit past the decimal point.
        float frac = abs - trunc;
        for (int i = 0; i < vetPlaces; ++i)
        {
            frac *= 10.0f;
            int tr = (int)frac;
            frac -= (float)tr;
            sb.Append(tr);
        }
        return sb;
    }

    /// <summary>
    /// Returns an integer formatted as a string padded with initial zeroes.
    /// </summary>
    /// <param name="v">integer</param>
    /// <param name="padding">leading zeroes</param>
    /// <returns>string</returns>
    public static string ToPadded(in int v, in int padding = 3)
    {
        return Utils.ToPadded(new StringBuilder(16), v, padding).ToString();
    }

    /// <summary>
    /// Appends a string representation of an integer
    /// padded with initial zeroes to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="v">input value</param>
    /// <param name="padding">leading zeroes</param>
    /// <returns>string builder</returns>
    public static String ToPadded(in StringBuilder sb, in int v, in int padding = 3)
    {
        // Double precision is needed to preserve accuracy. The max integer value
        // is 2147483647, which is 10 digits long. The sign needs to be flipped
        // because working with positive absolute value would allow
        // the minimum value to overflow to zero.
        bool isNeg = v < 0;
        int nAbsVal = isNeg ? v : -v;
        int[] digits = new int[10];
        int filled = 0;
        while (nAbsVal < 0)
        {
            double y = nAbsVal * 0.1d;
            nAbsVal = (int)y;
            digits[filled] = -(int)((y - nAbsVal) * 10.0d - 0.5d);
            ++filled;
        }

        if (isNeg) { sb.Append('-'); }
        int vplaces = padding < 1 ? 1 : padding;
        vplaces = filled > vplaces ? filled : vplaces;
        for (int n = vplaces - 1; n > -1; --n)
        {
            sb.Append(digits[n]);
        }

        return sb.ToString();
    }

    /// <summary>
    /// Finds an approximate integer ratio to reprsent a real number.
    /// The steps defines the maximum iteration of a search loop,
    /// while the tolerance governs an early return.
    /// Returns a tuple containing the antecedent and consequent.
    /// </summary>
    /// <param name="number">input value</param>
    /// <param name="steps">lower bound</param>
    /// <param name="tol">tolerance</param>
    /// <returns>ratio</returns>
    public static (int antecedent, int consequent) ToRatio(
        in float number,
        in int steps = 10,
        in float tol = 0.0005f)
    {
        int sgnNum = 0;
        if (number == 0.0f) { return (0, 0); }
        if (number > 0.0f) { sgnNum = 1; }
        if (number < -0.0f) { sgnNum = -1; }

        int cVerif = steps < 1 ? 1 : steps;
        float pVerif = MathF.Max(Utils.Epsilon, tol);

        float absNum = MathF.Abs(number);
        int integer = (int)absNum;
        float fraction = absNum - integer;

        int a0 = integer;
        int a1 = 1;
        int b0 = 1;
        int b1 = 0;

        int counter = 0;
        while (fraction > pVerif && counter < cVerif)
        {
            float newNum = 1.0f / fraction;
            integer = (int)newNum;
            fraction = newNum - integer;

            int t0 = a0;
            a0 = integer * a0 + b0;
            b0 = t0;

            int t1 = a1;
            a1 = integer * a1 + b1;
            b1 = t1;

            ++counter;
        }

        return (antecedent: sgnNum * a0, consequent: a1);
    }

    /// <summary>
    /// Wraps a value around a periodic range as defined by an upper and lower
    /// bound: lower bounds inclusive; upper bounds exclusive. Due to single
    /// precision accuracy, results will be inexact. In cases where the lower
    /// bound is greater than the upper bound, the two will be swapped. In cases
    /// where the range is 0.0, the value will be returned.
    /// </summary>
    /// <param name="v">input value</param>
    /// <param name="lb">lower bound</param>
    /// <param name="ub">upper bound</param>
    /// <returns>wrapped value</returns>
    public static float Wrap(in float v, in float lb = -1.0f, in float ub = 1.0f)
    {
        float range = ub - lb;
        return (range != 0.0f) ? v - range * MathF.Floor((v - lb) / range) : v;
    }

    /// <summary>
    /// A specialized version of RemFloor which wraps an angle in degrees to the
    /// range [0.0, 360.0) .
    /// </summary>
    /// <param name="deg">angle in degrees</param>
    /// <returns>output angle</returns>
    public static float WrapDegrees(in float deg)
    {
        return deg - 360.0f * MathF.Floor(deg * Utils.One360);
    }

    /// <summary>
    /// A specialized version of RemFloor which wraps an angle in radians to the
    /// range [0.0, TAU) .
    /// </summary>
    /// <param name="rad">angle in radians</param>
    /// <returns>output angle</returns>
    public static float WrapRadians(in float rad)
    {
        return rad - Utils.Tau * MathF.Floor(rad * Utils.OneTau);
    }

    /// <summary>
    /// Evaluates two floats like booleans, using the exclusive or (XOR) logic gate.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>evaluation</returns>
    public static int Xor(in float a, in float b)
    {
        return ((a != 0.0f) ^ (b != 0.0f)) ? 1 : 0;
    }
}