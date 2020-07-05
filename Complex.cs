using System;
using System.Collections;
using System.Text;

[Serializable]
public readonly struct Complex : IComparable<Complex>, IEquatable<Complex>, IEnumerable
{
    private readonly float real;
    private readonly float imag;

    public int Length { get { return 2; } }
    public float Imag { get { return this.imag; } }
    public float Real { get { return this.real; } }

    public float this [int i]
    {
        get
        {
            switch (i)
            {
                case 0:
                case -2:
                    return this.real;
                case 1:
                case -1:
                    return this.imag;
                default:
                    return 0.0f;
            }
        }
    }

    public Complex (float real = 0.0f, float imag = 0.0f)
    {
        this.real = real;
        this.imag = imag;
    }

    public override bool Equals (object value)
    {
        if (Object.ReferenceEquals (this, value)) return true;
        if (Object.ReferenceEquals (null, value)) return false;
        if (value is Complex) return this.Equals ((Complex) value);
        return false;
    }

    public override int GetHashCode ( )
    {
        unchecked
        {
            return (Utils.MulBase ^ this.real.GetHashCode ( )) *
                Utils.HashMul ^ this.imag.GetHashCode ( );
        }
    }

    public override string ToString ( )
    {
        return this.ToString (4);
    }

    public int CompareTo (Complex z)
    {
        return (this.imag > z.imag) ? 1 :
            (this.imag < z.imag) ? -1 :
            (this.real > z.real) ? 1 :
            (this.real < z.real) ? -1 :
            0;
    }

    public bool Equals (Complex z)
    {
        if (this.real.GetHashCode ( ) != z.real.GetHashCode ( )) return false;
        if (this.imag.GetHashCode ( ) != z.imag.GetHashCode ( )) return false;
        return true;
    }

    public IEnumerator GetEnumerator ( )
    {
        yield return this.real;
        yield return this.imag;
    }

    public float[ ] ToArray ( )
    {
        return new float[ ] { this.real, this.imag };
    }

    public string ToString (in int places = 4)
    {
        return new StringBuilder (64)
            .Append ("{ real: ")
            .Append (Utils.ToFixed (this.real, places))
            .Append (", imag: ")
            .Append (Utils.ToFixed (this.imag, places))
            .Append (" }")
            .ToString ( );
    }

    public (float real, float imag) ToTuple ( )
    {
        return (real: this.real, imag: this.imag);
    }

    public static implicit operator Complex (float s)
    {
        return new Complex (s, 0.0f);
    }

    public static implicit operator Vec2 (in Complex z)
    {
        return new Vec2 (z.real, z.imag);
    }

    public static implicit operator Complex (in Vec2 v)
    {
        return new Complex (v.x, v.y);
    }

    public static explicit operator float (in Complex z)
    {
        return Complex.Abs (z);
    }

    public static bool operator true (in Complex z)
    {
        return Complex.Any (z);
    }

    public static bool operator false (in Complex z)
    {
        return Complex.None (z);
    }

    public static Complex operator - (in Complex z)
    {
        return new Complex (-z.real, -z.imag);
    }

    /// <summary>
    /// Multiplies two complex numbers. Complex multiplication is not
    /// commutative.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the product</returns>
    public static Complex operator * (in Complex a, in Complex b)
    {
        return new Complex (
            a.real * b.real - a.imag * b.imag,
            a.real * b.imag + a.imag * b.real);
    }

    /// <summary>
    /// Multiplies a complex and real number.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the product</returns>
    public static Complex operator * (in Complex a, in float b)
    {
        return new Complex (a.real * b, a.imag * b);
    }

    /// <summary>
    /// Multiplies a real and complex number.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the product</returns>
    public static Complex operator * (in float a, in Complex b)
    {
        return new Complex (a * b.real, a * b.imag);
    }

    public static Complex operator / (in Complex a, in Complex b)
    {
        return a * Complex.Inverse (b);
    }

    public static Complex operator / (in Complex a, in float b)
    {
        if (b != 0.0f) return new Complex (a.real / b, a.imag / b);
        return new Complex ( );
    }

    public static Complex operator / (in float a, in Complex b)
    {
        return a * Complex.Inverse (b);
    }

    public static Complex operator + (in Complex a, in Complex b)
    {
        return new Complex (a.real + b.real, a.imag + b.imag);
    }

    public static Complex operator + (in Complex a, in float b)
    {
        return new Complex (a.real + b, a.imag);
    }

    public static Complex operator + (in float a, in Complex b)
    {
        return new Complex (a + b.real, b.imag);
    }

    /// <summary>
    /// Subtracts the left complex number from the right.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the difference</returns>
    public static Complex operator - (in Complex a, in Complex b)
    {
        return new Complex (a.real - b.real, a.imag - b.imag);
    }

    /// <summary>
    /// Subtracts a real number from a complex number.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the difference</returns>
    public static Complex operator - (in Complex a, in float b)
    {
        return new Complex (a.real - b, a.imag);
    }

    /// <summary>
    /// Subtracts a complex number from a real number.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the difference</returns>
    public static Complex operator - (in float a, in Complex b)
    {
        return new Complex (a - b.real, -b.imag);
    }

    public static float Abs (in Complex z)
    {
        return Utils.Sqrt (Complex.AbsSq (z));
    }

    public static float AbsSq (in Complex z)
    {
        return z.real * z.real + z.imag * z.imag;
    }

    public static bool All (in Complex z)
    {
        return z.real != 0.0f && z.imag != 0.0f;
    }

    public static bool Any (in Complex z)
    {
        return z.real != 0.0f || z.imag != 0.0f;
    }

    public static bool Approx (in Complex a, in Complex b, in float tolerance = Utils.Epsilon)
    {
        return Utils.Approx (a.real, b.real, tolerance) &&
            Utils.Approx (a.imag, b.imag, tolerance);
    }

    public static Complex Conj (in Complex z)
    {
        return new Complex (z.real, -z.imag);
    }

    public static Complex Cos (in Complex z)
    {
        double zr = (double) z.real;
        double zi = (double) z.imag;

        return new Complex (
            (float) (Math.Cos (zr) * Math.Cosh (zi)),
            (float) (-Math.Sin (zr) * Math.Sinh (zi)));
    }

    public static Complex Exp (in Complex z)
    {
        return Complex.Rect (Utils.Exp (z.real), z.imag);
    }

    public static Complex Inverse (in Complex z)
    {
        float absSq = Complex.AbsSq (z);
        if (absSq > 0.0f) return new Complex (z.real / absSq, -z.imag / absSq);
        return new Complex ( );
    }

    public static Complex Log (in Complex z)
    {
        return new Complex (
            Utils.Log (Complex.Abs (z)),
            Complex.Phase (z));
    }

    public static Complex Mobius (in Complex a = new Complex ( ), in Complex b = new Complex ( ), in Complex c = new Complex ( ), in Complex d = new Complex ( ), in Complex z = new Complex ( ))
    {
        // TODO: Inline.
        return ((a * z) + b) / ((c * z) + d);
    }

    /// <summary>
    /// Tests to see if all the complex number's components are zero.
    /// </summary>
    /// <param name="z">complex number</param>
    /// <returns>the evaluation</returns>
    public static bool None (in Complex z)
    {
        return z.real == 0.0f && z.imag == 0.0f;
    }

    /// <summary>
    /// Finds the signed phase of a complex number. Similar to a 2D vector's
    /// heading.
    /// </summary>
    /// <param name="z">complex number</param>
    /// <returns>the phase</returns>
    public static float Phase (in Complex z)
    {
        return Utils.Atan2 (z.imag, z.real);
    }

    /// <summary>
    /// Returns a named value tuple with the radius and angle of a complex
    /// number.
    /// </summary>
    /// <param name="z">complex number</param>
    /// <returns>the tuple</returns>
    public static (float r, float phi) Polar (in Complex z)
    {
        return (r: Complex.Phase (z), phi: Complex.Abs (z));
    }

    /// <summary>
    /// Raises a complex number to the power of another.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the result</returns>
    public static Complex Pow (in Complex a, in Complex b)
    {
        return Complex.Exp (b * Complex.Log (a));
    }

    /// <summary>
    /// Raises a complex number to the power of a real number.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the result</returns>
    public static Complex Pow (in Complex a, in float b)
    {
        return Complex.Exp (b * Complex.Log (a));
    }

    /// <summary>
    /// Raises a real number to the power of a complex number.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>the result</returns>
    public static Complex Pow (in float a, in Complex b)
    {
        return Complex.Exp (b * Complex.Log (a));
    }

    /// <summary>
    /// Creates a random complex number.
    /// </summary>
    /// <param name="rng">random number generator</param>
    /// <param name="rhoMin">radius minimum</param>
    /// <param name="rhoMax">radius maximum</param>
    /// <returns>random complex number</returns>
    public static Complex Random (in System.Random rng, in float rhoMin = 1.0f, in float rhoMax = 1.0f)
    {
        return Complex.Rect (
            Utils.Mix (rhoMin, rhoMax,
                (float) rng.NextDouble ( )),

            Utils.Mix (-Utils.Pi, Utils.Pi,
                (float) rng.NextDouble ( )));
    }

    /// <summary>
    /// Converts from polar to rectilinear coordinates.
    /// </summary>
    /// <param name="r">radius</param>
    /// <param name="phi">angle in radians</param>
    /// <returns>complex number</returns>
    public static Complex Rect (in float r = 1.0f, in float phi = 0.0f)
    {
        float sinp = 0.0f;
        float cosp = 0.0f;
        Utils.SinCos (phi, out sinp, out cosp);
        return new Complex (r * cosp, r * sinp);
    }

    /// <summary>
    /// Finds the sine of a complex number.
    /// </summary>
    /// <param name="z">complex number</param>
    /// <returns>sine</returns>
    public static Complex Sin (in Complex z)
    {
        double zr = (double) z.real;
        double zi = (double) z.imag;

        return new Complex (
            (float) (Math.Sin (zr) * Math.Cosh (zi)),
            (float) (Math.Cos (zr) * Math.Sinh (zi)));
    }

    /// <summary>
    /// Finds the square root of a real number which could be either positive or
    /// negative.
    /// </summary>
    /// <param name="a">value</param>
    /// <returns>square root</returns>
    public static Complex Sqrt (in float a)
    {
        return (a > 0.0f) ?
            new Complex (Utils.Sqrt (a), 0.0f) :
            (a < 0.0f) ?
            new Complex (0.0f, Utils.Sqrt (-a)) :
            new Complex ( );
    }
}