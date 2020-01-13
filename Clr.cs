using System;
using System.Collections;
using System.Text;

[Serializable]
public readonly struct Clr : IComparable<Clr>, IEquatable<Clr>, IEnumerable
{
    private readonly float _r;
    private readonly float _g;
    private readonly float _b;
    private readonly float _a;

    public int Length { get { return 4; } }
    public float r { get { return this._r; } }
    public float g { get { return this._g; } }
    public float b { get { return this._b; } }
    public float a { get { return this._a; } }

    public float this [int i]
    {
        get
        {
            switch (i)
            {
                case 0:
                case -4:
                    return this._r;
                case 1:
                case -3:
                    return this._g;
                case 2:
                case -2:
                    return this._b;
                case 3:
                case -1:
                    return this._a;
                default:
                    return 0.0f;
            }
        }
    }

    public Clr (byte r = 255, byte g = 255, byte b = 255, byte a = 255)
    {
        this._r = r * Utils.One255;
        this._g = g * Utils.One255;
        this._b = b * Utils.One255;
        this._a = a * Utils.One255;
    }

    public Clr (sbyte r = -1, sbyte g = -1, sbyte b = -1, sbyte a = -1)
    {
        this._r = (((int) r) & 0xff) * Utils.One255;
        this._g = (((int) g) & 0xff) * Utils.One255;
        this._b = (((int) b) & 0xff) * Utils.One255;
        this._a = (((int) a) & 0xff) * Utils.One255;
    }

    public Clr (float r = 1.0f, float g = 1.0f, float b = 1.0f, float a = 1.0f)
    {
        this._r = Utils.Clamp (r, 0.0f, 1.0f);
        this._g = Utils.Clamp (g, 0.0f, 1.0f);
        this._b = Utils.Clamp (b, 0.0f, 1.0f);
        this._a = Utils.Clamp (a, 0.0f, 1.0f);
    }

    public override bool Equals (object value)
    {
        if (Object.ReferenceEquals (this, value)) return true;
        if (Object.ReferenceEquals (null, value)) return false;

        if (value is Clr)
        {
            Clr c = (Clr) value;
            return ((int) this) == ((int) c);
        }

        return false;
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
        return ((int) this) == ((int) c);
    }

    public IEnumerator GetEnumerator ( )
    {
        yield return this._r;
        yield return this._g;
        yield return this._b;
        yield return this._a;
    }

    public float[ ] ToArray ( )
    {
        return new float[ ] { this._r, this._g, this._b, this._a };
    }

    public string ToString (int places = 4)
    {
        return new StringBuilder (96)
            .Append ("{ r: ")
            .Append (Utils.ToFixed (this._r, places))
            .Append (", g: ")
            .Append (Utils.ToFixed (this._g, places))
            .Append (", b: ")
            .Append (Utils.ToFixed (this._b, places))
            .Append (", a: ")
            .Append (Utils.ToFixed (this._a, places))
            .Append (" }")
            .ToString ( );
    }

    public (float r, float g, float b, float a) ToTuple ( )
    {
        return (r: this._r, g: this._g, b: this._b, a: this._a);
    }

    public static explicit operator Clr (bool b)
    {
        float v = b ? 1.0f : 0.0f;
        return new Clr (v, v, v, v);
    }

    public static explicit operator Clr (byte ub)
    {
        return new Clr (ub, ub, ub, ub);
    }

    public static explicit operator Clr (sbyte sb)
    {
        return new Clr (sb, sb, sb, sb);
    }

    public static explicit operator Clr (int c)
    {
        return Clr.FromHex (c);
    }

    public static explicit operator Clr (uint c)
    {
        return Clr.FromHex (c);
    }

    public static explicit operator Clr (long c)
    {
        return Clr.FromHex (c);
    }

    public static explicit operator Clr (float v)
    {
        return new Clr (v, v, v, v);
    }

    public static explicit operator bool (in Clr c)
    {
        return Clr.Any (c);
    }

    public static explicit operator int (in Clr c)
    {
        return Clr.ToHexInt (c);
    }

    public static explicit operator uint (in Clr c)
    {
        int cint = (int) c;
        return (uint) cint;
    }

    public static explicit operator long (in Clr c)
    {
        int cint = (int) c;
        return ((long) cint) & 0xffffffffL;
    }

    public static explicit operator float (in Clr c)
    {
        return Clr.Luminance (c);
    }

    public static bool operator true (in Clr c)
    {
        return Clr.Any (c);
    }

    public static bool operator false (in Clr c)
    {
        return Clr.None (c);
    }

    public static Clr operator ~ (in Clr c)
    {
        return Clr.FromHex (~Clr.ToHexInt (c));
    }

    public static Clr operator & (in Clr a, in Clr b)
    {
        return Clr.FromHex (Clr.ToHexInt (a) & Clr.ToHexInt (b));
    }

    public static Clr operator | (in Clr a, in Clr b)
    {
        return Clr.FromHex (Clr.ToHexInt (a) | Clr.ToHexInt (b));
    }

    public static Clr operator ^ (in Clr a, in Clr b)
    {
        return Clr.FromHex (Clr.ToHexInt (a) ^ Clr.ToHexInt (b));
    }

    public static Clr operator << (in Clr a, int b)
    {
        return Clr.FromHex (Clr.ToHexInt (a) << b);
    }

    public static Clr operator >> (in Clr a, int b)
    {
        return Clr.FromHex (Clr.ToHexInt (a) >> b);
    }

    public static Clr operator - (in Clr c)
    {
        return new Clr (
            Utils.Max (1.0f - c._r, 0.0f),
            Utils.Max (1.0f - c._g, 0.0f),
            Utils.Max (1.0f - c._b, 0.0f),
            Utils.Clamp (c._a, 0.0f, 1.0f));
    }

    public static Clr operator ++ (in Clr c)
    {
        return new Clr (
            Utils.Clamp (c._r + Utils.One255, 0.0f, 1.0f),
            Utils.Clamp (c._g + Utils.One255, 0.0f, 1.0f),
            Utils.Clamp (c._b + Utils.One255, 0.0f, 1.0f),
            Utils.Clamp (c._a, 0.0f, 1.0f));
    }

    public static Clr operator -- (in Clr c)
    {
        return new Clr (
            Utils.Clamp (c._r - Utils.One255, 0.0f, 1.0f),
            Utils.Clamp (c._g - Utils.One255, 0.0f, 1.0f),
            Utils.Clamp (c._b - Utils.One255, 0.0f, 1.0f),
            Utils.Clamp (c._a, 0.0f, 1.0f));
    }

    public static Clr operator * (in Clr a, in Clr b)
    {
        return new Clr (
            Utils.Clamp (a._r * b._r, 0.0f, 1.0f),
            Utils.Clamp (a._g * b._g, 0.0f, 1.0f),
            Utils.Clamp (a._b * b._b, 0.0f, 1.0f),
            Utils.Clamp (a._a, 0.0f, 1.0f));
    }

    public static Clr operator / (in Clr a, in Clr b)
    {
        return new Clr (
            Utils.Clamp (Utils.Div (a._r, b._r), 0.0f, 1.0f),
            Utils.Clamp (Utils.Div (a._g, b._g), 0.0f, 1.0f),
            Utils.Clamp (Utils.Div (a._b, b._b), 0.0f, 1.0f),
            Utils.Clamp (a._a, 0.0f, 1.0f));
    }

    public static Clr operator % (in Clr a, in Clr b)
    {
        return new Clr (
            Utils.Mod (a._r, b._r),
            Utils.Mod (a._g, b._g),
            Utils.Mod (a._b, b._b),
            Utils.Clamp (a._a, 0.0f, 1.0f));
    }

    public static Clr operator + (in Clr a, in Clr b)
    {
        return new Clr (
            Utils.Clamp (a._r + b._r, 0.0f, 1.0f),
            Utils.Clamp (a._g + b._g, 0.0f, 1.0f),
            Utils.Clamp (a._b + b._b, 0.0f, 1.0f),
            Utils.Clamp (a._a, 0.0f, 1.0f));
    }

    public static Clr operator - (in Clr a, in Clr b)
    {
        return new Clr (
            Utils.Clamp (a._r - b._r, 0.0f, 1.0f),
            Utils.Clamp (a._g - b._g, 0.0f, 1.0f),
            Utils.Clamp (a._b - b._b, 0.0f, 1.0f),
            Utils.Clamp (a._a, 0.0f, 1.0f));
    }

    public static bool All (in Clr c)
    {
        return c._a > 0.0f &&
            c._r > 0.0f &&
            c._g > 0.0f &&
            c._b > 0.0f;
    }

    public static bool Any (in Clr c)
    {
        return c._a > 0.0f;
    }

    public static Clr Clamp (in Clr c, float lb = 0.0f, float ub = 1.0f)
    {
        return new Clr (
            Utils.Clamp (c._r, lb, ub),
            Utils.Clamp (c._g, lb, ub),
            Utils.Clamp (c._b, lb, ub),
            Utils.Clamp (c._a, lb, ub));
    }

    public static Clr Clamp (in Clr c, in Clr lb, in Clr ub)
    {
        return new Clr (
            Utils.Clamp (c._r, lb._r, ub._r),
            Utils.Clamp (c._g, lb._g, ub._g),
            Utils.Clamp (c._b, lb._b, ub._b),
            Utils.Clamp (c._a, lb._a, ub._a));
    }

    public static Clr FromHex (int c)
    {
        return new Clr (
            (c >> 0x10 & 0xff) * Utils.One255,
            (c >> 0x8 & 0xff) * Utils.One255,
            (c & 0xff) * Utils.One255,
            (c >> 0x18 & 0xff) * Utils.One255);
    }

    public static Clr FromHex (uint c)
    {
        return new Clr (
            (c >> 0x10 & 0xff) * Utils.One255,
            (c >> 0x8 & 0xff) * Utils.One255,
            (c & 0xff) * Utils.One255,
            (c >> 0x18 & 0xff) * Utils.One255);
    }

    public static Clr FromHex (long c)
    {
        return new Clr (
            (c >> 0x10 & 0xff) * Utils.One255,
            (c >> 0x8 & 0xff) * Utils.One255,
            (c & 0xff) * Utils.One255,
            (c >> 0x18 & 0xff) * Utils.One255);
    }

    public static Clr HsbaToRgba (in Vec4 v)
    {
        return HsbaToRgba (v.x, v.y, v.z, v.w);
    }

    public static Clr HsbaToRgba (
        float hue = 1.0f,
        float sat = 1.0f,
        float bri = 1.0f,
        float alpha = 1.0f)
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

    public static float Luminance (in Clr c)
    {
        return 0.2126f * c._r + 0.7152f * c._g + 0.0722f * c._b;
    }

    public static Clr Max (in Clr a, in Clr b)
    {
        return new Clr (
            Utils.Max (a._r, b._r),
            Utils.Max (a._g, b._g),
            Utils.Max (a._b, b._b),
            Utils.Max (a._a, b._a));
    }

    public static Clr Min (in Clr a, in Clr b)
    {
        return new Clr (
            Utils.Min (a._r, b._r),
            Utils.Min (a._g, b._g),
            Utils.Min (a._b, b._b),
            Utils.Min (a._a, b._a));
    }

    public static Clr MixHsba (in Clr a, in Clr b, float t = 0.5f)
    {
        return Clr.HsbaToRgba (Vec4.Mix (Clr.RgbaToHsba (a), Clr.RgbaToHsba (b), t));
    }

    public static Clr MixHsba (in Clr a, in Clr b, in Vec4 t)
    {
        return Clr.HsbaToRgba (Vec4.Mix (Clr.RgbaToHsba (a), Clr.RgbaToHsba (b), t));
    }

    public static Clr MixRgba (in Clr a, in Clr b, float t = 0.5f)
    {
        float u = 1.0f - t;
        return new Clr (
            u * a._r + t * b._r,
            u * a._g + t * b._g,
            u * a._b + t * b._b,
            u * a._a + t * b._a);
    }

    public static Clr MixRgba (in Clr a, in Clr b, in Clr t)
    {
        return new Clr (
            (1.0f - t._r) * a._r + t._r * b._r,
            (1.0f - t._g) * a._g + t._g * b._g,
            (1.0f - t._b) * a._b + t._b * b._b,
            (1.0f - t._a) * a._a + t._a * b._a);
    }

    public static Clr MixRgba (in Clr a, in Clr b, in Vec4 t)
    {
        return new Clr (
            (1.0f - t.x) * a._r + t.x * b._r,
            (1.0f - t.y) * a._g + t.y * b._g,
            (1.0f - t.z) * a._b + t.z * b._b,
            (1.0f - t.w) * a._a + t.w * b._a);
    }

    public static bool None (in Clr c)
    {
        return c._a <= 0.0f;
    }

    public static Clr RandomRgba (in Random rng, float lb = 0.0f, float ub = 1.0f)
    {
        float xFac = (float) rng.NextDouble ( );
        float yFac = (float) rng.NextDouble ( );
        float zFac = (float) rng.NextDouble ( );
        float wFac = (float) rng.NextDouble ( );

        return new Clr (
            Utils.Mix (lb, ub, xFac),
            Utils.Mix (lb, ub, yFac),
            Utils.Mix (lb, ub, zFac),
            Utils.Mix (lb, ub, wFac));
    }

    public static Clr RandomRgba (in Random rng, in Clr lb, in Clr ub)
    {
        float xFac = (float) rng.NextDouble ( );
        float yFac = (float) rng.NextDouble ( );
        float zFac = (float) rng.NextDouble ( );
        float wFac = (float) rng.NextDouble ( );

        return new Clr (
            Utils.Mix (lb._r, ub._r, xFac),
            Utils.Mix (lb._g, ub._g, yFac),
            Utils.Mix (lb._b, ub._b, zFac),
            Utils.Mix (lb._a, ub._a, wFac));
    }

    public static Vec4 RgbaToHsba (in Clr c)
    {
        return RgbaToHsba (c._r, c._g, c._b, c._a);
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
                hue = (green - blue) / delta;
            else if (green == bri)
                hue = 2.0f + (blue - red) / delta;
            else
                hue = 4.0f + (red - green) / delta;

            hue *= Utils.OneSix;
            if (hue < 0.0f) hue += 1.0f;
        }

        float sat = bri == 0.0f ? 0.0f : delta / bri;
        return new Vec4 (hue, sat, bri, alpha);
    }

    public static int ToHexInt (in Clr c)
    {
        return (int) (c._a * 0xff + 0.5f) << 0x18 |
            (int) (c._r * 0xff + 0.5f) << 0x10 |
            (int) (c._g * 0xff + 0.5f) << 0x8 |
            (int) (c._b * 0xff + 0.5f);
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
            return new Clr (1.0f, 0.9568627f, 0.8392157f, 1.0f);
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