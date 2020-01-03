using System;
using System.Text;

[Serializable]
public struct Complex : IComparable<Complex>
{
    public float real;
    public float imag;

    public float Real
    {
        get
        {
            return this.real;
        }

        set
        {
            this.real = value;
        }
    }

    public float Imag
    {
        get
        {
            return this.imag;
        }

        set
        {
            this.imag = value;
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
                    return this.real;
                case 1:
                case -1:
                    return this.imag;
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
                    this.real = value;
                    break;
                case 1:
                case -1:
                    this.imag = value;
                    break;
            }
        }
    }

    public Complex (float real = 0.0f, float imag = 0.0f)
    {
        this.real = real;
        this.imag = imag;
    }

    public Complex (bool real = false, bool imag = false)
    {
        this.real = real ? 1.0f : 0.0f;
        this.imag = imag ? 1.0f : 0.0f;
    }

    public int CompareTo (Complex c)
    {
        return (this.imag > c.imag) ? 1 :
            (this.imag < c.imag) ? -1 :
            (this.real > c.real) ? 1 :
            (this.real < c.real) ? -1 :
            0;
    }

    public override string ToString ( )
    {
        return new StringBuilder ( )
            .Append ("{ real: ")
            .Append (this.real)
            .Append (", imag: ")
            .Append (this.imag)
            .Append (" }")
            .ToString ( );
    }

    public Complex Set (float real = 0.0f, float imag = 0.0f)
    {
        this.real = real;
        this.imag = imag;
        return this;
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

    public static Complex operator / (Complex a, float b)
    {
        if (b == 0.0f) return new Complex (0.0f, 0.0f);
        return new Complex (a.real / b, a.imag / b);
    }

    public static Complex Conj (Complex z)
    {
        return new Complex (z.real, -z.imag);
    }

    public static float AbsSq (Complex z)
    {
        return z.real * z.real + z.imag * z.imag;
    }

    public static float Abs (Complex z)
    {
        return Utils.Sqrt (z.real * z.real + z.imag * z.imag);
    }

    public static Complex Inverse (Complex z)
    {
        float absSq = z.real * z.real + z.imag * z.imag;
        if (absSq == 0.0f) return new Complex (0.0f, 0.0f);
        return new Complex (z.real / absSq, -z.imag / absSq);
    }

    public static Complex operator / (Complex a, Complex b)
    {
        return a * Complex.Inverse (b);
    }

    public static Complex operator / (float a, Complex b)
    {
        return a * Complex.Inverse (b);
    }

    public static Complex operator + (Complex a, float b)
    {
        return new Complex (a.real + b, a.imag);
    }

    public static Complex operator + (float a, Complex b)
    {
        return new Complex (a + b.real, b.imag);
    }

    public static Complex operator + (Complex a, Complex b)
    {
        return new Complex (a.real + b.real, a.imag + b.imag);
    }

    public static Complex operator - (Complex a, float b)
    {
        return new Complex (a.real - b, a.imag);
    }

    public static Complex operator - (float a, Complex b)
    {
        return new Complex (a - b.real, -b.imag);
    }

    public static Complex operator - (Complex a, Complex b)
    {
        return new Complex (a.real - b.real, a.imag - b.imag);
    }

    public static Complex Mobius (
        Complex a = new Complex ( ), Complex b = new Complex ( ),
        Complex c = new Complex ( ), Complex d = new Complex ( ),
        Complex z = new Complex ( ))
    {
        return ((a * z) + b) / ((c * z) + d);
    }

    public static Complex Rect (float r = 1.0f, float phi = 0.0f)
    {
        return new Complex (r * Utils.Cos (phi), r * Utils.Sin (phi));
    }

    public static float Phase (Complex z)
    {
        return Utils.Atan2 (z.imag, z.real);
    }

    public static Complex Exp (Complex z)
    {
        return Complex.Rect (Utils.Exp (z.real), z.imag);
    }

    public static Complex Log (Complex z)
    {
        return new Complex (
            Utils.Log (Complex.Abs (z)),
            Complex.Phase (z));
    }

    public static void Polar (Complex a, out float r, out float phi)
    {
        r = Complex.Abs (a);
        phi = Complex.Phase (a);
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

    public static Complex Cos (Complex z)
    {
        return new Complex (
            Utils.Cos (z.real) * Utils.Cosh (z.imag), -Utils.Sin (z.real) * Utils.Sinh (z.imag));
    }

    public static Complex Sin (Complex z)
    {
        return new Complex (
            Utils.Sin (z.real) * Utils.Cosh (z.imag),
            Utils.Cos (z.real) * Utils.Sinh (z.imag));
    }
}