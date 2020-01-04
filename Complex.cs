using System;
using System.Text;

[Serializable]
public readonly struct Complex : IComparable<Complex>, IEquatable<Complex>
{
    public readonly float real;
    public readonly float imag;

    public float Real
    {
        get
        {
            return this.real;
        }

        // set
        // {
        //     this.real = value;
        // }
    }

    public float Imag
    {
        get
        {
            return this.imag;
        }

        // set
        // {
        //     this.imag = value;
        // }
    }

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

        // set
        // {
        //     switch (i)
        //     {
        //         case 0:
        //         case -2:
        //             this.real = value;
        //             break;
        //         case 1:
        //         case -1:
        //             this.imag = value;
        //             break;
        //     }
        // }
    }

    public Complex (float real = 0.0f, float imag = 0.0f)
    {
        this.real = real;
        this.imag = imag;
    }

    public override int GetHashCode ( )
    {
        unchecked
        {
            const int hashBase = -2128831035;
            const int hashMul = 16777619;
            int hash = hashBase;
            hash = hash * hashMul ^ this.real.GetHashCode ( );
            hash = hash * hashMul ^ this.imag.GetHashCode ( );
            return hash;
        }
    }

    public override string ToString ( )
    {
        return ToString (4);
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
        // return Complex.Approx (this, z);

        if (this.real.GetHashCode ( ) != z.real.GetHashCode ( ))
        {
            return false;
        }

        if (this.imag.GetHashCode ( ) != z.imag.GetHashCode ( ))
        {
            return false;
        }

        return true;
    }

    // public Complex Reset ( )
    // {
    //     return this.Set (0.0f, 0.0f);
    // }

    // public Complex Set (float real = 0.0f, float imag = 0.0f)
    // {
    //     this.real = real;
    //     this.imag = imag;
    //     return this;
    // }

    public float[ ] ToArray ( )
    {
        return new float[ ] { this.real, this.imag };
    }

    public string ToString (int places = 4)
    {
        return new StringBuilder ( )
            .Append ("{ real: ")
            .Append (Utils.ToFixed (this.real, places))
            .Append (", imag: ")
            .Append (Utils.ToFixed (this.imag, places))
            .Append (" }")
            .ToString ( );
    }

    public static implicit operator Complex (float s)
    {
        return new Complex (s, 0.0f);
    }

    public static implicit operator Vec2 (Complex z)
    {
        return new Vec2 (z.real, z.imag);
    }

    public static implicit operator Complex (Vec2 v)
    {
        return new Complex (v.x, v.y);
    }

    public static explicit operator float (Complex z)
    {
        return Complex.Abs (z);
    }

    public static Complex operator * (Complex a, Complex b)
    {
        return new Complex (
            a.real * b.real - a.imag * b.imag,
            a.real * b.imag + a.imag * b.real);
    }

    public static Complex operator * (Complex a, float b)
    {
        return new Complex (a.real * b, a.imag * b);
    }

    public static Complex operator * (float a, Complex b)
    {
        return new Complex (a * b.real, a * b.imag);
    }

    public static Complex operator / (Complex a, Complex b)
    {
        return a * Complex.Inverse (b);
    }

    public static Complex operator / (Complex a, float b)
    {
        if (b == 0.0f) return new Complex (0.0f, 0.0f);
        return new Complex (a.real / b, a.imag / b);
    }

    public static Complex operator / (float a, Complex b)
    {
        return a * Complex.Inverse (b);
    }

    public static Complex operator + (Complex a, Complex b)
    {
        return new Complex (a.real + b.real, a.imag + b.imag);
    }

    public static Complex operator + (Complex a, float b)
    {
        return new Complex (a.real + b, a.imag);
    }

    public static Complex operator + (float a, Complex b)
    {
        return new Complex (a + b.real, b.imag);
    }

    public static Complex operator - (Complex a, Complex b)
    {
        return new Complex (a.real - b.real, a.imag - b.imag);
    }

    public static Complex operator - (Complex a, float b)
    {
        return new Complex (a.real - b, a.imag);
    }

    public static Complex operator - (float a, Complex b)
    {
        return new Complex (a - b.real, -b.imag);
    }

    public static float Abs (Complex z)
    {
        return Utils.Sqrt (z.real * z.real + z.imag * z.imag);
    }

    public static float AbsSq (Complex z)
    {
        return z.real * z.real + z.imag * z.imag;
    }

    public static bool Approx (Complex a, Complex b, float tolerance = Utils.Epsilon)
    {
        return Utils.Approx (a.real, b.real, tolerance) &&
            Utils.Approx (a.imag, b.imag, tolerance);
    }

    public static Complex Conj (Complex z)
    {
        return new Complex (z.real, -z.imag);
    }

    public static Complex Cos (Complex z)
    {
        return new Complex (
            Utils.Cos (z.real) * Utils.Cosh (z.imag), -Utils.Sin (z.real) * Utils.Sinh (z.imag));
    }

    public static Complex Exp (Complex z)
    {
        return Complex.Rect (Utils.Exp (z.real), z.imag);
    }

    public static Complex Inverse (Complex z)
    {
        float absSq = z.real * z.real + z.imag * z.imag;
        if (absSq == 0.0f) return new Complex (0.0f, 0.0f);
        return new Complex (z.real / absSq, -z.imag / absSq);
    }

    public static Complex Log (Complex z)
    {
        return new Complex (
            Utils.Log (Complex.Abs (z)),
            Complex.Phase (z));
    }

    public static Complex Mobius (
        Complex a = new Complex ( ), Complex b = new Complex ( ),
        Complex c = new Complex ( ), Complex d = new Complex ( ),
        Complex z = new Complex ( ))
    {
        return ((a * z) + b) / ((c * z) + d);
    }

    public static float Phase (Complex z)
    {
        return Utils.Atan2 (z.imag, z.real);
    }

    public static Complex Rect (float r = 1.0f, float phi = 0.0f)
    {
        return new Complex (r * Utils.Cos (phi), r * Utils.Sin (phi));
    }

    public static Complex Pow (Complex a, Complex b)
    {
        return Complex.Exp (b * Complex.Log (a));
    }

    public static Complex Pow (Complex a, float b)
    {
        return Complex.Exp (b * Complex.Log (a));
    }

    public static Complex Pow (float a, Complex b)
    {
        return Complex.Exp (b * Complex.Log (a));
    }

    public static Complex Sin (Complex z)
    {
        return new Complex (
            Utils.Sin (z.real) * Utils.Cosh (z.imag),
            Utils.Cos (z.real) * Utils.Sinh (z.imag));
    }
}