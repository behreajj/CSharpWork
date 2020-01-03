using System;
using System.Text;

[Serializable]
public struct Clr : IComparable<Clr>
{
    public float r;
    public float g;
    public float b;
    public float a;

    public float R
    {
        get
        {
            return this.r;
        }

        set
        {
            this.r = value;
        }
    }

    public float G
    {
        get
        {
            return this.g;
        }

        set
        {
            this.g = value;
        }
    }

    public float B
    {
        get
        {
            return this.b;
        }

        set
        {
            this.b = value;
        }
    }

    public float A
    {
        get
        {
            return this.a;
        }

        set
        {
            this.a = value;
        }
    }

    public float this [int i]
    {
        get
        {
            switch (i)
            {
                case 0:
                case -4:
                    return this.r;
                case 1:
                case -3:
                    return this.g;
                case 2:
                case -2:
                    return this.b;
                case 3:
                case -1:
                    return this.a;
                default:
                    return 0.0f;
            }
        }

        set
        {
            switch (i)
            {
                case 0:
                case -4:
                    this.r = value;
                    break;
                case 1:
                case -3:
                    this.g = value;
                    break;
                case 2:
                case -2:
                    this.b = value;
                    break;
                case 3:
                case -1:
                    this.a = value;
                    break;
            }
        }
    }

    public Clr (byte r = 0x0, byte g = 0x0, byte b = 0x0, byte a = 0xff)
    {
        this.r = r * Utils.One255;
        this.g = g * Utils.One255;
        this.b = b * Utils.One255;
        this.a = a * Utils.One255;
    }

    public Clr (sbyte r = 0x0, sbyte g = 0x0, sbyte b = 0x0, sbyte a = 0x7f)
    {
        this.r = (((int) r) & 0xff) * Utils.One255;
        this.g = (((int) g) & 0xff) * Utils.One255;
        this.b = (((int) b) & 0xff) * Utils.One255;
        this.a = (((int) a) & 0xff) * Utils.One255;
    }

    public Clr (float r = 0.0f, float g = 0.0f, float b = 0.0f, float a = 0.0f)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }

    public int CompareTo (Clr c)
    {
        float alum = Clr.Luminance (this);
        float blum = Clr.Luminance (c);

        return (alum > blum) ? 1 :
            (alum < blum) ? -1 :
            0;
    }

    public override string ToString ( )
    {
        return new StringBuilder ( )
            .Append ("{ r: ")
            .Append (this.r)
            .Append (", g: ")
            .Append (this.g)
            .Append (", b: ")
            .Append (this.b)
            .Append (", a: ")
            .Append (this.a)
            .Append (" }")
            .ToString ( );
    }

    public Clr Set (byte r = 0x0, byte g = 0x0, byte b = 0x0, byte a = 0xff)
    {
        this.r = r * Utils.One255;
        this.g = g * Utils.One255;
        this.b = b * Utils.One255;
        this.a = a * Utils.One255;
        return this;
    }

    public Clr Set (sbyte r = 0x0, sbyte g = 0x0, sbyte b = 0x0, sbyte a = 0x7f)
    {
        this.r = (((int) r) & 0xff) * Utils.One255;
        this.g = (((int) g) & 0xff) * Utils.One255;
        this.b = (((int) b) & 0xff) * Utils.One255;
        this.a = (((int) a) & 0xff) * Utils.One255;
        return this;
    }

    public Clr Set (float r = 0.0f, float g = 0.0f, float b = 0.0f, float a = 0.0f)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
        return this;
    }

    public static implicit operator int (Clr c)
    {
        return (int) (c.a * 0xff + 0.5f) << 0x18 |
            (int) (c.r * 0xff + 0.5f) << 0x10 |
            (int) (c.g * 0xff + 0.5f) << 0x8 |
            (int) (c.b * 0xff + 0.5f);
    }

    public static implicit operator long (Clr c)
    {
        //TODO: This needs to be match long output.
        return int.MinValue + ((int) (c.a * 0xff + 0.5f) << 0x18 |
            (int) (c.r * 0xff + 0.5f) << 0x10 |
            (int) (c.g * 0xff + 0.5f) << 0x8 |
            (int) (c.b * 0xff + 0.5f)) - int.MaxValue;
    }

    public static implicit operator Clr (byte ub)
    {
        float v = ub * Utils.One255;
        return new Clr (v, v, v, v);
    }

    public static implicit operator Clr (sbyte sb)
    {
        float v = (((int) sb) & 0xff) * Utils.One255;
        return new Clr (v, v, v, v);
    }

    public static implicit operator Clr (int c)
    {
        return new Clr (
            (c >> 0x10 & 0xff) * Utils.One255,
            (c >> 0x8 & 0xff) * Utils.One255,
            (c & 0xff) * Utils.One255,
            (c >> 0x18 & 0xff) * Utils.One255);
    }

    public static implicit operator Clr (long c)
    {
        return new Clr (
            (c >> 0x10 & 0xff) * Utils.One255,
            (c >> 0x8 & 0xff) * Utils.One255,
            (c & 0xff) * Utils.One255,
            (c >> 0x18 & 0xff) * Utils.One255);
    }

    public static implicit operator Clr (Vec4 v)
    {
        return new Clr (v.x, v.y, v.z, v.w);
    }

    public static implicit operator Vec4 (Clr c)
    {
        return new Vec4 (c.r, c.g, c.b, c.a);
    }

    public static explicit operator float (Clr c)
    {
        return Clr.Luminance (c);
    }

    public static float Luminance (Clr c)
    {
        return 0.2126f * c.r + 0.7152f * c.g + 0.0722f * c.b;
    }

    public static Clr Black
    {
        get
        {
            return new Clr (0.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    public static Clr Blue
    {
        get
        {
            return new Clr (0.0f, 1.0f, 0.0f, 1.0f);
        }
    }

    public static Clr ClearBlack
    {
        get
        {
            return new Clr (0.0f, 0.0f, 0.0f, 0.0f);
        }
    }

    public static Clr ClearWhite
    {
        get
        {
            return new Clr (1.0f, 1.0f, 1.0f, 0.0f);
        }
    }

    public static Clr Cyan
    {
        get
        {
            return new Clr (0.0f, 1.0f, 1.0f, 1.0f);
        }
    }

    public static Clr Green
    {
        get
        {
            return new Clr (0.0f, 1.0f, 0.0f, 1.0f);
        }
    }

    public static Clr Magenta
    {
        get
        {
            return new Clr (1.0f, 0.0f, 1.0f, 1.0f);
        }
    }

    public static Clr Red
    {
        get
        {
            return new Clr (1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    public static Clr Yellow
    {
        get
        {
            return new Clr (1.0f, 1.0f, 0.0f, 1.0f);
        }
    }

    public static Clr White
    {
        get
        {
            return new Clr (1.0f, 1.0f, 1.0f, 1.0f);
        }
    }
}