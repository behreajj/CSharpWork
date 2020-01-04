using System;
using System.Text;

[Serializable]
public readonly struct Clr : IComparable<Clr>, IEquatable<Clr>
{
    public readonly float r;
    public readonly float g;
    public readonly float b;
    public readonly float a;

    public float R
    {
        get
        {
            return this.r;
        }

        // set
        // {
        //     this.r = value;
        // }
    }

    public float G
    {
        get
        {
            return this.g;
        }

        // set
        // {
        //     this.g = value;
        // }
    }

    public float B
    {
        get
        {
            return this.b;
        }

        // set
        // {
        //     this.b = value;
        // }
    }

    public float A
    {
        get
        {
            return this.a;
        }

        // set
        // {
        //     this.a = value;
        // }
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

        // set
        // {
        //     switch (i)
        //     {
        //         case 0:
        //         case -4:
        //             this.r = value;
        //             break;
        //         case 1:
        //         case -3:
        //             this.g = value;
        //             break;
        //         case 2:
        //         case -2:
        //             this.b = value;
        //             break;
        //         case 3:
        //         case -1:
        //             this.a = value;
        //             break;
        //     }
        // }
    }

    public Clr (byte r = 255, byte g = 255, byte b = 255, byte a = 255)
    {
        this.r = r * Utils.One255;
        this.g = g * Utils.One255;
        this.b = b * Utils.One255;
        this.a = a * Utils.One255;
    }

    public Clr (sbyte r = -1, sbyte g = -1, sbyte b = -1, sbyte a = -1)
    {
        this.r = (((int) r) & 0xff) * Utils.One255;
        this.g = (((int) g) & 0xff) * Utils.One255;
        this.b = (((int) b) & 0xff) * Utils.One255;
        this.a = (((int) a) & 0xff) * Utils.One255;
    }

    public Clr (float r = 1.0f, float g = 1.0f, float b = 1.0f, float a = 1.0f)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }

    public override int GetHashCode ( )
    {
        return (int) this;
    }

    public override string ToString ( )
    {
        return ToString (4);
    }

    public int CompareTo (Clr c)
    {
        float alum = Clr.Luminance (this);
        float blum = Clr.Luminance (c);

        return (alum > blum) ? 1 :
            (alum < blum) ? -1 :
            0;
    }

    public bool Equals (Clr c)
    {
        return ((int)this) == ((int)c);
    }

    // public Clr Reset ( )
    // {
    //     return this.Set (1.0f, 1.0f, 1.0f, 1.0f);
    // }

    // public Clr Set (byte r = 255, byte g = 255, byte b = 255, byte a = 255)
    // {
    //     this.r = r * Utils.One255;
    //     this.g = g * Utils.One255;
    //     this.b = b * Utils.One255;
    //     this.a = a * Utils.One255;
    //     return this;
    // }

    // public Clr Set (sbyte r = -1, sbyte g = -1, sbyte b = -1, sbyte a = -1)
    // {
    //     this.r = (((int) r) & 0xff) * Utils.One255;
    //     this.g = (((int) g) & 0xff) * Utils.One255;
    //     this.b = (((int) b) & 0xff) * Utils.One255;
    //     this.a = (((int) a) & 0xff) * Utils.One255;
    //     return this;
    // }

    // public Clr Set (float r = 1.0f, float g = 1.0f, float b = 1.0f, float a = 1.0f)
    // {
    //     this.r = r;
    //     this.g = g;
    //     this.b = b;
    //     this.a = a;
    //     return this;
    // }

    public float[ ] ToArray ( )
    {
        return new float[ ] { this.r, this.g, this.b, this.a };
    }

    public string ToString (int places = 4)
    {
        return new StringBuilder ( )
            .Append ("{ r: ")
            .Append (Utils.ToFixed (this.r, places))
            .Append (", g: ")
            .Append (Utils.ToFixed (this.g, places))
            .Append (", b: ")
            .Append (Utils.ToFixed (this.b, places))
            .Append (", a: ")
            .Append (Utils.ToFixed (this.a, places))
            .Append (" }")
            .ToString ( );
    }

    public static implicit operator int (Clr c)
    {
        return (int) (c.a * 0xff + 0.5f) << 0x18 |
            (int) (c.r * 0xff + 0.5f) << 0x10 |
            (int) (c.g * 0xff + 0.5f) << 0x8 |
            (int) (c.b * 0xff + 0.5f);
    }

    public static implicit operator uint (Clr c)
    {
        int cint = (int) c;
        return (uint) cint;
    }

    public static implicit operator long (Clr c)
    {
        int cint = (int) c;
        return ((long) cint) & 0xffffffffL;
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

    public static implicit operator Clr (uint c)
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

    public static implicit operator Clr (float v)
    {
        return new Clr (v, v, v, v);
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

    public static Clr operator ~ (Clr c)
    {
        return ~((int) c);
    }

    public static Clr operator & (Clr a, Clr b)
    {
        return ((int) a) & ((int) b);
    }

    public static Clr operator & (Clr a, int b)
    {
        return ((int) a) & b;
    }

    public static Clr operator & (int a, Clr b)
    {
        return a & ((int) b);
    }

    public static Clr operator & (Clr a, long b)
    {
        return ((long) a) & b;
    }

    public static Clr operator & (long a, Clr b)
    {
        return a & ((long) b);
    }

    public static Clr operator | (Clr a, Clr b)
    {
        return ((int) a) | ((int) b);
    }

    public static Clr operator | (Clr a, int b)
    {
        return ((int) a) | b;
    }

    public static Clr operator | (int a, Clr b)
    {
        return a | ((int) b);
    }

    public static Clr operator | (Clr a, long b)
    {
        return ((long) a) | b;
    }

    public static Clr operator | (long a, Clr b)
    {
        return a | ((long) b);
    }

    public static Clr operator ^ (Clr a, Clr b)
    {
        return ((int) a) ^ ((int) b);
    }

    public static Clr operator ^ (Clr a, int b)
    {
        return ((int) a) ^ b;
    }

    public static Clr operator ^ (int a, Clr b)
    {
        return a ^ ((int) b);
    }

    public static Clr operator ^ (Clr a, long b)
    {
        return ((long) a) ^ b;
    }

    public static Clr operator ^ (long a, Clr b)
    {
        return a ^ ((long) b);
    }

    public static Clr operator << (Clr a, int b)
    {
        return ((int) a) << b;
    }

    public static Clr operator >> (Clr a, int b)
    {
        return ((int) a) >> b;
    }

    public static Clr operator - (Clr c)
    {
        return new Clr (
            Utils.Max (1.0f - c.r, 0.0f),
            Utils.Max (1.0f - c.g, 0.0f),
            Utils.Max (1.0f - c.b, 0.0f),
            c.a);
    }

    public static Clr operator * (Clr a, Clr b)
    {
        return new Clr (
            Utils.Clamp (a.r * b.r, 0.0f, 1.0f),
            Utils.Clamp (a.g * b.g, 0.0f, 1.0f),
            Utils.Clamp (a.b * b.b, 0.0f, 1.0f),
            Utils.Clamp (a.a, 0.0f, 1.0f));
    }

    public static Clr operator * (Clr a, float b)
    {
        return new Clr (
            Utils.Clamp (a.r * b, 0.0f, 1.0f),
            Utils.Clamp (a.g * b, 0.0f, 1.0f),
            Utils.Clamp (a.b * b, 0.0f, 1.0f),
            Utils.Clamp (a.a, 0.0f, 1.0f));
    }

    public static Clr operator * (float a, Clr b)
    {
        return new Clr (
            Utils.Clamp (a * b.r, 0.0f, 1.0f),
            Utils.Clamp (a * b.g, 0.0f, 1.0f),
            Utils.Clamp (a * b.b, 0.0f, 1.0f),
            Utils.Clamp (a, 0.0f, 1.0f));
    }

    // public static Clr operator * (Clr a, int b)
    // {
    //     return a * (Clr)b;
    // }

    // public static Clr operator * (int a, Clr b)
    // {
    //     return ((Clr)a) * b;
    // }

    public static Clr operator / (Clr a, Clr b)
    {
        return new Clr (
            Utils.Clamp (Utils.Div (a.r, b.r), 0.0f, 1.0f),
            Utils.Clamp (Utils.Div (a.g, b.g), 0.0f, 1.0f),
            Utils.Clamp (Utils.Div (a.b, b.b), 0.0f, 1.0f),
            Utils.Clamp (a.a, 0.0f, 1.0f));
    }

    public static Clr operator / (Clr a, float b)
    {
        if (b == 0.0f) return Clr.White;
        float bInv = 1.0f / b;
        return new Clr (
            Utils.Clamp (a.r * bInv, 0.0f, 1.0f),
            Utils.Clamp (a.g * bInv, 0.0f, 1.0f),
            Utils.Clamp (a.b * bInv, 0.0f, 1.0f),
            Utils.Clamp (a.a, 0.0f, 1.0f));
    }

    public static Clr operator / (float a, Clr b)
    {
        return new Clr (
            Utils.Clamp (Utils.Div (a, b.r), 0.0f, 1.0f),
            Utils.Clamp (Utils.Div (a, b.g), 0.0f, 1.0f),
            Utils.Clamp (Utils.Div (a, b.b), 0.0f, 1.0f),
            Utils.Clamp (a, 0.0f, 1.0f));
    }

    // public static Clr operator / (Clr a, int b)
    // {
    //     return a / (Clr)b;
    // }

    // public static Clr operator / (int a, Clr b)
    // {
    //     return ((Clr)a) / b;
    // }

    public static Clr operator % (Clr a, Clr b)
    {
        return new Clr (
            Utils.Mod (a.r, b.r),
            Utils.Mod (a.g, b.g),
            Utils.Mod (a.b, b.b),
            a.a);
    }

    public static Clr operator % (Clr a, float b)
    {
        return new Clr (
            Utils.Mod (a.r, b),
            Utils.Mod (a.g, b),
            Utils.Mod (a.b, b),
            a.a);
    }

    public static Clr operator % (float a, Clr b)
    {
        return new Clr (
            Utils.Mod (a, b.r),
            Utils.Mod (a, b.g),
            Utils.Mod (a, b.b),
            a);
    }

    // public static Clr operator % (Clr a, int b)
    // {
    //     return a % (Clr)b;
    // }

    // public static Clr operator % (int a, Clr b)
    // {
    //     return ((Clr)a) % b;
    // }

    public static Clr operator + (Clr a, Clr b)
    {
        return new Clr (
            Utils.Clamp (a.r + b.r, 0.0f, 1.0f),
            Utils.Clamp (a.g + b.g, 0.0f, 1.0f),
            Utils.Clamp (a.b + b.b, 0.0f, 1.0f),
            Utils.Clamp (a.a, 0.0f, 1.0f));
    }

    public static Clr operator + (Clr a, float b)
    {
        return new Clr (
            Utils.Clamp (a.r + b, 0.0f, 1.0f),
            Utils.Clamp (a.g + b, 0.0f, 1.0f),
            Utils.Clamp (a.b + b, 0.0f, 1.0f),
            Utils.Clamp (a.a, 0.0f, 1.0f));
    }

    public static Clr operator + (float a, Clr b)
    {
        return new Clr (
            Utils.Clamp (a + b.r, 0.0f, 1.0f),
            Utils.Clamp (a + b.g, 0.0f, 1.0f),
            Utils.Clamp (a + b.b, 0.0f, 1.0f),
            Utils.Clamp (a, 0.0f, 1.0f));
    }

    // public static Clr operator + (Clr a, int b)
    // {
    //     return a + (Clr)b;
    // }

    // public static Clr operator + (int a, Clr b)
    // {
    //     return ((Clr)a) + b;
    // }

    public static Clr operator - (Clr a, Clr b)
    {
        return new Clr (
            Utils.Clamp (a.r - b.r, 0.0f, 1.0f),
            Utils.Clamp (a.g - b.g, 0.0f, 1.0f),
            Utils.Clamp (a.b - b.b, 0.0f, 1.0f),
            Utils.Clamp (a.a, 0.0f, 1.0f));
    }

    public static Clr operator - (Clr a, float b)
    {
        return new Clr (
            Utils.Clamp (a.r - b, 0.0f, 1.0f),
            Utils.Clamp (a.g - b, 0.0f, 1.0f),
            Utils.Clamp (a.b - b, 0.0f, 1.0f),
            Utils.Clamp (a.a, 0.0f, 1.0f));
    }

    public static Clr operator - (float a, Clr b)
    {
        return new Clr (
            Utils.Clamp (a - b.r, 0.0f, 1.0f),
            Utils.Clamp (a - b.g, 0.0f, 1.0f),
            Utils.Clamp (a - b.b, 0.0f, 1.0f),
            Utils.Clamp (a, 0.0f, 1.0f));
    }

    // public static Clr operator - (Clr a, int b)
    // {
    //     return a - (Clr)b;
    // }

    // public static Clr operator - (int a, Clr b)
    // {
    //     return ((Clr)a) - b;
    // }

    public static float Luminance (Clr c)
    {
        return 0.2126f * c.r + 0.7152f * c.g + 0.0722f * c.b;
    }

    public static Clr Clamp (Clr v, float lb = 0.0f, float ub = 1.0f)
    {
        return new Clr (
            Utils.Clamp (v.r, lb, ub),
            Utils.Clamp (v.g, lb, ub),
            Utils.Clamp (v.b, lb, ub),
            Utils.Clamp (v.a, lb, ub));
    }

    public static Clr Clamp (Clr v, Clr lb, Clr ub)
    {
        return new Clr (
            Utils.Clamp (v.r, lb.r, ub.r),
            Utils.Clamp (v.g, lb.g, ub.g),
            Utils.Clamp (v.b, lb.b, ub.b),
            Utils.Clamp (v.a, lb.a, ub.a));
    }

    public static Clr HsbaToRgba (Vec4 v)
    {
        return HsbaToRgba (v.x, v.y, v.z, v.w);
    }

    public static Clr HsbaToRgba (
        float hue,
        float sat,
        float bri,
        float alpha)
    {
        if (sat <= 0.0f)
        {
            return new Clr (bri, bri, bri, alpha);
        }

        float h = Utils.Mod1 (hue) * 6.0f;
        int sector = (int) h;

        float tint1 = bri * (1.0f - sat);
        float tint2 = bri * (1.0f - sat * (h - sector));
        float tint3 = bri * (1.0f - sat * (1.0f + sector - h));

        switch (sector)
        {
            case 0:
                return new Clr (bri, tint3, tint1, alpha);
            case 1:
                return new Clr (tint2, bri, tint1, alpha);
            case 2:
                return new Clr (tint1, bri, tint3, alpha);
            case 3:
                return new Clr (tint1, tint2, bri, alpha);
            case 4:
                return new Clr (tint3, tint1, bri, alpha);
            case 5:
                return new Clr (bri, tint1, tint2, alpha);
            default:
                return Clr.White;
        }
    }

    public static Clr Max (Clr a, Clr b)
    {
        return new Clr (
            Utils.Max (a.r, b.r),
            Utils.Max (a.g, b.g),
            Utils.Max (a.b, b.b),
            Utils.Max (a.a, b.a));
    }

    public static Clr Min (Clr a, Clr b)
    {
        return new Clr (
            Utils.Min (a.r, b.r),
            Utils.Min (a.g, b.g),
            Utils.Min (a.b, b.b),
            Utils.Min (a.a, b.a));
    }

    public static Clr Mix (Clr a, Clr b, bool t)
    {
        return t ? b : a;
    }

    public static Clr Mix (Clr a, Clr b, float t = 0.5f)
    {
        float u = 1.0f - t;
        return new Clr (
            u * a.r + t * b.r,
            u * a.g + t * b.g,
            u * a.b + t * b.b,
            u * a.a + t * b.a);
    }

    public static Clr Mix (Clr a, Clr b, Clr t)
    {
        return new Clr (
            (1.0f - t.r) * a.r + t.r * b.r,
            (1.0f - t.g) * a.g + t.g * b.g,
            (1.0f - t.b) * a.b + t.b * b.b,
            (1.0f - t.a) * a.a + t.a * b.a);
    }

    public static Vec4 RgbaToHsba (Clr c)
    {
        return RgbaToHsba (c.r, c.g, c.b, c.a);
    }

    public static Vec4 RgbaToHsba (
        float red = 1.0f,
        float green = 1.0f,
        float blue = 1.0f,
        float alpha = 1.0f)
    {
        float bri = Utils.Max (red, green, blue);
        float mn = Utils.Min (red, green, blue);
        float delta = bri - mn;
        float hue = 0.0f;

        if (delta != 0.0f)
        {
            if (red == bri)
            {
                hue = (green - blue) / delta;
            }
            else if (green == bri)
            {
                hue = 2.0f + (blue - red) / delta;
            }
            else
            {
                hue = 4.0f + (red - green) / delta;
            }

            hue *= Utils.OneSix;
            if (hue < 0.0f)
            {
                hue += 1.0f;
            }
        }

        float sat = bri == 0.0f ? 0.0f : delta / bri;
        return new Vec4 (hue, sat, bri, alpha);
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
            return new Clr (0.0f, 0.0f, 1.0f, 1.0f);
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

    public static Clr Light
    {
        get
        {
            return new Clr (1.0f, 0.9686275f, 0.8352942f, 1.0f);
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