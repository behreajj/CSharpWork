using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;

/// <summary>
/// A two-dimensional complex number. The imaginary component is a
/// coefficient of i, or the square-root of negative one.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Explicit, Pack = 8)]
public readonly struct Complex : IComparable<Complex>, IEquatable<Complex>, IEnumerable
{
    /// <summary>
    /// The coefficient of the imaginary component i.
    /// </summary>
    [FieldOffset(4)] private readonly float imag;

    /// <summary>
    /// The real component.
    /// </summary>
    [FieldOffset(0)] private readonly float real;

    /// <summary>
    /// The coefficient of the imaginary component i.
    /// </summary>
    /// <value>imaginary</value>
    public float Imag { get { return this.imag; } }

    /// <summary>
    /// Gets the number of components held by the complex number.
    /// </summary>
    /// <value>length</value>
    public int Length { get { return 2; } }

    /// <summary>
    /// The real component.
    /// </summary>
    /// <value>real number</value>
    public float Real { get { return this.real; } }

    /// <summary>
    /// Retrieves a component by index. When the provided index is 0 or -2,
    /// returns real; 1 or -1, imaginary.
    /// </summary>
    /// <value>component</value>
    public float this[int i]
    {
        get
        {
            return i switch
            {
                0 or -2 => this.real,
                1 or -1 => this.imag,
                _ => 0.0f,
            };
        }
    }

    /// <summary>
    /// Constructs a complex number from single precision numbers.
    /// </summary>
    /// <param name="real">real number</param>
    /// <param name="imag">imaginary number</param>
    public Complex(in float real = 0.0f, in float imag = 0.0f)
    {
        this.real = real;
        this.imag = imag;
    }

    /// <summary>
    /// Tests this complex mumber for equivalence with an object. For
    /// approximate equality with another complex number, use the static approx
    /// function instead.
    /// </summary>
    /// <param name="value">object</param>
    /// <returns>equivalence</returns>
    public override bool Equals(object value)
    {
        if (Object.ReferenceEquals(this, value)) { return true; }
        if (value is null) { return false; }
        if (value is Complex complex) { return this.Equals(complex); }
        return false;
    }

    /// <summary>
    /// Returns a hash code representing this complex number.
    /// </summary>
    /// <returns>hash code</returns>
    public override int GetHashCode()
    {
        unchecked
        {
            return (Utils.MulBase ^ this.real.GetHashCode()) *
                Utils.HashMul ^ this.imag.GetHashCode();
        }
    }

    /// <summary>
    /// Returns a string representation of this complex number.
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()
    {
        return Complex.ToString(this);
    }

    /// <summary>
    /// Compares this complex number to another.
    /// Returns 1 when a component of this vector is
    /// greater than another; -1 when lesser. Prioritizes the imaginary
    /// component over the real component. Returns 0 as a last resort.
    /// </summary>
    /// <param name="z">comparisand</param>
    /// <returns>evaluation</returns>
    public int CompareTo(Complex z)
    {
        return (this.imag < z.imag) ? -1 :
            (this.imag > z.imag) ? 1 :
            (this.real < z.real) ? -1 :
            (this.real > z.real) ? 1 :
            0;
    }

    /// <summary>
    /// Tests this complex number for equivalence with another in compliance
    /// with the IEquatable interface. For approximate equality with another
    /// complex number, use the static approx function instead.
    /// </summary>
    /// <param name="z">complex number</param>
    /// <returns>equivalence</returns>
    public bool Equals(Complex z)
    {
        if (this.real.GetHashCode() != z.real.GetHashCode()) { return false; }
        if (this.imag.GetHashCode() != z.imag.GetHashCode()) { return false; }
        return true;
    }

    /// <summary>
    /// Returns an enumerator (or iterator) for this complex number, allowing
    /// its components to be accessed in a foreach loop.
    /// </summary>
    /// <returns>enumerator</returns>
    public IEnumerator GetEnumerator()
    {
        yield return this.real;
        yield return this.imag;
    }

    /// <summary>
    /// Promotes a real number to a complex number.
    /// </summary>
    /// <param name="s">real number</param>
    public static implicit operator Complex(in float s)
    {
        return new(s, 0.0f);
    }

    /// <summary>
    /// Converts a complex number to a 2D vector.
    /// </summary>
    /// <param name="z">complex number</param>
    public static implicit operator Vec2(in Complex z)
    {
        return new Vec2(z.real, z.imag);
    }

    /// <summary>
    /// Converts a 2D vector to a complex number.
    /// </summary>
    /// <param name="v">vector</param>
    public static implicit operator Complex(in Vec2 v)
    {
        return new(v.X, v.Y);
    }

    /// <summary>
    /// A complex number evaluates to true when any of its components are not
    /// equal to zero.
    /// </summary>
    /// <param name="z">complex number</param>
    /// <returns>evaluation</returns>
    public static bool operator true(in Complex z)
    {
        return Complex.Any(z);
    }

    /// <summary>
    /// A complex number evaluates to false when all of its components are equal
    /// to zero.
    /// </summary>
    /// <param name="z">complex number</param>
    /// <returns>evaluation</returns>
    public static bool operator false(in Complex z)
    {
        return Complex.None(z);
    }

    /// <summary>
    /// Negates a complex number.
    /// </summary>
    /// <param name="z">complex number</param>
    /// <returns>the negation</returns>
    public static Complex operator -(in Complex z)
    {
        return new(-z.real, -z.imag);
    }

    /// <summary>
    /// Multiplies two complex numbers. Complex multiplication is not
    /// commutative.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>product</returns>
    public static Complex operator *(in Complex a, in Complex b)
    {
        return new(
            a.real * b.real - a.imag * b.imag,
            a.real * b.imag + a.imag * b.real);
    }

    /// <summary>
    /// Multiplies a complex and real number.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>product</returns>
    public static Complex operator *(in Complex a, in float b)
    {
        return new(a.real * b, a.imag * b);
    }

    /// <summary>
    /// Multiplies a real and complex number.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>product</returns>
    public static Complex operator *(in float a, in Complex b)
    {
        return new(a * b.real, a * b.imag);
    }

    /// <summary>
    /// Divides one complex number by another. Equivalent to multiplying the
    /// numerator and the inverse of the denominator.
    /// </summary>
    /// <param name="a">numerator</param>
    /// <param name="b">denominator</param>
    /// <returns>quotient</returns>
    public static Complex operator /(in Complex a, in Complex b)
    {
        return a * Complex.Inverse(b);
    }

    /// <summary>
    /// Divides a complex number by a real number.
    /// </summary>
    /// <param name="a">numerator</param>
    /// <param name="b">denominator</param>
    /// <returns>quotient</returns>
    public static Complex operator /(in Complex a, in float b)
    {
        if (b != 0.0f) { return new(a.real / b, a.imag / b); }
        return Complex.Zero;
    }

    /// <summary>
    /// Divides a real number by a complex number.
    /// </summary>
    /// <param name="a">numerator</param>
    /// <param name="b">denominator</param>
    /// <returns>quotient</returns>
    public static Complex operator /(in float a, in Complex b)
    {
        return a * Complex.Inverse(b);
    }

    /// <summary>
    /// Adds two complex numbers.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>sum</returns>
    public static Complex operator +(in Complex a, in Complex b)
    {
        return new(a.real + b.real, a.imag + b.imag);
    }

    /// <summary>
    /// Adds a complex and real number.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>sum</returns>
    public static Complex operator +(in Complex a, in float b)
    {
        return new(a.real + b, a.imag);
    }

    /// <summary>
    /// Adds a real and complex number.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>sum</returns>
    public static Complex operator +(in float a, in Complex b)
    {
        return new(a + b.real, b.imag);
    }

    /// <summary>
    /// Subtracts the left complex number from the right.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>difference</returns>
    public static Complex operator -(in Complex a, in Complex b)
    {
        return new(a.real - b.real, a.imag - b.imag);
    }

    /// <summary>
    /// Subtracts a real number from a complex number.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>difference</returns>
    public static Complex operator -(in Complex a, in float b)
    {
        return new(a.real - b, a.imag);
    }

    /// <summary>
    /// Subtracts a complex number from a real number.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>difference</returns>
    public static Complex operator -(in float a, in Complex b)
    {
        return new(a - b.real, -b.imag);
    }

    /// <summary>
    /// Finds the absolute of a complex number. Similar to a vector's magnitude.
    /// </summary>
    /// <param name="z">complex number</param>
    /// <returns>absolute</returns>
    public static float Abs(in Complex z)
    {
        return MathF.Sqrt(Complex.AbsSq(z));
    }

    /// <summary>
    /// Finds the absolute squared of a complex number. Similar to a vector's
    /// magnitude squared.
    /// </summary>
    /// <param name="z">complex number</param>
    /// <returns>absolute squared</returns>
    public static float AbsSq(in Complex z)
    {
        return z.real * z.real + z.imag * z.imag;
    }

    /// <summary>
    /// Tests to see if all of the complex number's components are non-zero.
    /// </summary>
    /// <param name="z">complex number</param>
    /// <returns>evaluation</returns>
    public static bool All(in Complex z)
    {
        return z.real != 0.0f && z.imag != 0.0f;
    }

    /// <summary>
    /// Tests to see if any of the complex number's components are non-zero.
    /// </summary>
    /// <param name="z">complex number</param>
    /// <returns>evaluation</returns>
    public static bool Any(in Complex z)
    {
        return z.real != 0.0f || z.imag != 0.0f;
    }

    /// <summary>
    /// Evaluates whether or not two complex numbers approximate each other
    /// according to a tolerance.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <param name="tol">tolerance</param>
    /// <returns>evaluation</returns>
    public static bool Approx(
        in Complex a, in Complex b,
        in float tol = Utils.Epsilon)
    {
        return Utils.Approx(a.real, b.real, tol) &&
            Utils.Approx(a.imag, b.imag, tol);
    }

    /// <summary>
    /// Finds the conjugate of the complex number, where the imaginary component
    /// where the imaginary component is negated.
    /// </summary>
    /// <param name="z">complex number</param>
    /// <returns>conjugate</returns>
    public static Complex Conj(in Complex z)
    {
        return new(z.real, -z.imag);
    }

    /// <summary>
    /// Finds the cosine of a complex number.
    /// </summary>
    /// <param name="z">complex number</param>
    /// <returns>cosine</returns>
    public static Complex Cos(in Complex z)
    {
        double zr = z.real;
        double zi = z.imag;
        return new(
            (float)(Math.Cos(zr) * Math.Cosh(zi)),
            (float)(-Math.Sin(zr) * Math.Sinh(zi)));
    }

    /// <summary>
    /// Returns Euler's number, e, raised to a complex number.
    /// </summary>
    /// <param name="z">complex number</param>
    /// <returns>result</returns>
    public static Complex Exp(in Complex z)
    {
        return Complex.Rect(MathF.Exp(z.real), z.imag);
    }

    /// <summary>
    /// Returns the inverse, or reciprocal, of the complex number.
    /// </summary>
    /// <param name="z">complex number</param>
    /// <returns>inverse</returns>
    public static Complex Inverse(in Complex z)
    {
        float absSq = Complex.AbsSq(z);
        if (absSq > 0.0f)
        {
            return new(
                z.real / absSq, -z.imag / absSq);
        }
        return Complex.Zero;
    }

    /// <summary>
    /// Finds the complex logarithm.
    /// </summary>
    /// <param name="z">complex number</param>
    /// <returns>logarithm</returns>
    public static Complex Log(in Complex z)
    {
        return new(
            MathF.Log(Complex.Abs(z)),
            Complex.Phase(z));
    }

    /// <summary>
    /// Performs the Mobius transformation on the variable z. Uses the formula
    /// (c z + d) / (a z + b) .
    /// </summary>
    /// <returns>mobius transformation</returns>
    public static Complex Mobius(
        in Complex a,
        in Complex b,
        in Complex c,
        in Complex d,
        in Complex z)
    {
        // Denominator: (c * z) + d .
        float czdr = c.real * z.real - c.imag * z.imag + d.real;
        float czdi = c.real * z.imag + c.imag * z.real + d.imag;

        float mSq = czdr * czdr + czdi * czdi;
        if (mSq <= 0.0f) { return Complex.Zero; }

        // Numerator: (a * z) + b .
        float azbr = a.real * z.real - a.imag * z.imag + b.real;
        float azbi = a.real * z.imag + a.imag * z.real + b.imag;

        // Find inverse.
        float mSqInv = 1.0f / mSq;
        float czdrInv = czdr * mSqInv;
        float czdiInv = -czdi * mSqInv;

        // Multiply numerator with inverse of denominator.
        return new(
            azbr * czdrInv - azbi * czdiInv,
            azbr * czdiInv + azbi * czdrInv);
    }

    /// <summary>
    /// Tests to see if all the complex number's components are zero.
    /// </summary>
    /// <param name="z">complex number</param>
    /// <returns>evaluation</returns>
    public static bool None(in Complex z)
    {
        return z.real == 0.0f && z.imag == 0.0f;
    }

    /// <summary>
    /// Finds the signed phase of a complex number. Similar to a 2D vector's
    /// heading.
    /// </summary>
    /// <param name="z">complex number</param>
    /// <returns>phase</returns>
    public static float Phase(in Complex z)
    {
        return MathF.Atan2(z.imag, z.real);
    }

    /// <summary>
    /// Returns a named value tuple with the radius and angle of a complex
    /// number.
    /// </summary>
    /// <param name="z">complex number</param>
    /// <returns>tuple</returns>
    public static (float r, float phi) Polar(in Complex z)
    {
        return (r: Complex.Abs(z), phi: Complex.Phase(z));
    }

    /// <summary>
    /// Raises a complex number to the power of another. Uses the formula
    /// pow ( a, b ) := exp ( b log ( a ) )
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>result</returns>
    public static Complex Pow(in Complex a, in Complex b)
    {
        return Complex.Exp(b * Complex.Log(a));
    }

    /// <summary>
    /// Raises a complex number to the power of a real number. Uses the formula
    /// pow ( a, b ) := exp ( b log ( a ) )
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>result</returns>
    public static Complex Pow(in Complex a, in float b)
    {
        return Complex.Exp(b * Complex.Log(a));
    }

    /// <summary>
    /// Raises a real number to the power of a complex number. Uses the formula    
    /// pow ( a, b ) := exp ( b log ( a ) )
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>result</returns>
    public static Complex Pow(in float a, in Complex b)
    {
        return Complex.Exp(b * Complex.Log(a));
    }

    /// <summary>
    /// Creates a random complex number.
    /// </summary>
    /// <param name="rng">random number generator</param>
    /// <param name="radius">radius</param>
    /// <returns>random complex number</returns>
    public static Complex Random(
        in System.Random rng,
        in float radius = 1.0f)
    {
        float r = Utils.NextGaussian(rng);
        float i = Utils.NextGaussian(rng);
        float absq = r * r + i * i;
        if (absq != 0.0f)
        {
            float scalar = radius / MathF.Sqrt(absq);
            return new(r * scalar, i * scalar);
        }
        return Complex.Zero;
    }

    /// <summary>
    /// Converts from polar to rectilinear coordinates.
    /// </summary>
    /// <param name="r">radius</param>
    /// <param name="phi">angle in radians</param>
    /// <returns>complex number</returns>
    public static Complex Rect(in float r = 1.0f, in float phi = 0.0f)
    {
        return new(r * MathF.Cos(phi), r * MathF.Sin(phi));
    }

    /// <summary>
    /// Finds the sine of a complex number.
    /// </summary>
    /// <param name="z">complex number</param>
    /// <returns>sine</returns>
    public static Complex Sin(in Complex z)
    {
        double zr = z.real;
        double zi = z.imag;
        return new(
            (float)(Math.Sin(zr) * Math.Cosh(zi)),
            (float)(Math.Cos(zr) * Math.Sinh(zi)));
    }

    /// <summary>
    /// Finds the square root of a real number which could be either positive
    /// or negative.
    /// </summary>
    /// <param name="a">value</param>
    /// <returns>square root</returns>
    public static Complex Sqrt(in float a)
    {
        return (a > 0.0f) ?
            new(MathF.Sqrt(a), 0.0f) :
            (a < -0.0f) ?
            new(0.0f, MathF.Sqrt(-a)) :
            Complex.Zero;
    }

    /// <summary>
    /// Returns a string representation of a complex number.
    /// </summary>
    /// <param name="z">real number</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string</returns>
    public static string ToString(in Complex z, in int places = 4)
    {
        return Complex.ToString(new StringBuilder(64), z, places).ToString();
    }

    /// <summary>
    /// Appends a representation of a complex number to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="z">complex number</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString(
        in StringBuilder sb,
        in Complex z,
        in int places = 4)
    {
        sb.Append("{\"real\":");
        Utils.ToFixed(sb, z.real, places);
        sb.Append(",\"imag\":");
        Utils.ToFixed(sb, z.imag, places);
        sb.Append('}');
        return sb;
    }

    /// <summary>
    /// Returns a complex number with all components set to zero.
    /// </summary>
    /// <value>zero</value>
    public static Complex Zero { get { return new(0.0f, 0.0f); } }
}