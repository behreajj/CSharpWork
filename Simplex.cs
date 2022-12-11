public static class Simplex
{
    /// <summary>
    /// A default seed set to the system current time in milliseconds.
    /// </summary>
    public static int DefaultSeed = (int)System.DateTime.Now.Ticks;

    private const float F2 = 0.36602542f;
    private const float F3 = 0.33333333f;
    private const float F4 = 0.309017f;
    private const float G2 = 0.21132487f;
    private const float G2_2 = 0.42264974f;
    private const float G3 = 0.16666667f;
    private const float G3_2 = 0.33333333f;
    private const float G3_3 = 0.5f;
    private const float G4 = 0.1381966f;
    private const float G4_2 = 0.2763932f;
    private const float G4_3 = 0.4145898f;
    private const float G4_4 = 0.5527864f;

    /// <summary>
    /// Factor by which 3D noise is scaled prior to return: 64.0 .
    /// </summary>
    private const float Scale2 = 64.0f;

    /// <summary>
    /// Factor by which 3D noise is scaled prior to return: 68.0 .
    /// </summary>
    private const float Scale3 = 68.0f;

    /// <summary>
    /// Factor by which 4D noise is scaled prior to return: 54.0 .
    /// </summary>
    private const float Scale4 = 54.0f;

    /// <summary>
    /// Factor added to 2D noise when returning a Vec2. 1.0 / Math.Sqrt(2.0); approximately 0.70710677 .
    /// </summary>
    private const float Step2 = 0.70710677f;

    /// <summary>
    /// Factor added to 3D noise when returning a Vec3. 1.0 / Math.Sqrt(3.0); approximately 0.57735026 .
    /// </summary>
    private const float Step3 = 0.57735026f;

    /// <summary>
    /// Factor added to 4D noise when returning a Vec4. 1.0 / Math.sqrt(4.0); 0.5 .
    /// </summary>
    private const float Step4 = 0.5f;

    private static readonly Vec2[] Grad2Lut = new Vec2[]
    {
        new Vec2 (-1.0f, -1.0f),
        new Vec2 (1.0f, 0.0f),
        new Vec2 (-1.0f, 0.0f),
        new Vec2 (1.0f, 1.0f),
        new Vec2 (-1.0f, 1.0f),
        new Vec2 (0.0f, -1.0f),
        new Vec2 (0.0f, 1.0f),
        new Vec2 (1.0f, -1.0f)
    };

    private static readonly Vec3[] Grad3Lut = new Vec3[]
    {
        new Vec3 (1.0f, 0.0f, 1.0f),
        new Vec3 (0.0f, 1.0f, 1.0f),
        new Vec3 (-1.0f, 0.0f, 1.0f),
        new Vec3 (0.0f, -1.0f, 1.0f),
        new Vec3 (1.0f, 0.0f, -1.0f),
        new Vec3 (0.0f, 1.0f, -1.0f),
        new Vec3 (-1.0f, 0.0f, -1.0f),
        new Vec3 (0.0f, -1.0f, -1.0f),
        new Vec3 (1.0f, -1.0f, 0.0f),
        new Vec3 (1.0f, 1.0f, 0.0f),
        new Vec3 (-1.0f, 1.0f, 0.0f),
        new Vec3 (-1.0f, -1.0f, 0.0f),
        new Vec3 (1.0f, 0.0f, 1.0f),
        new Vec3 (-1.0f, 0.0f, 1.0f),
        new Vec3 (0.0f, 1.0f, -1.0f),
        new Vec3 (0.0f, -1.0f, -1.0f)
    };

    private static readonly Vec4[] Grad4Lut = new Vec4[]
    {
        new Vec4 (0.0f, 1.0f, 1.0f, 1.0f),
        new Vec4 (0.0f, 1.0f, 1.0f, -1.0f),
        new Vec4 (0.0f, 1.0f, -1.0f, 1.0f),
        new Vec4 (0.0f, 1.0f, -1.0f, -1.0f),
        new Vec4 (0.0f, -1.0f, 1.0f, 1.0f),
        new Vec4 (0.0f, -1.0f, 1.0f, -1.0f),
        new Vec4 (0.0f, -1.0f, -1.0f, 1.0f),
        new Vec4 (0.0f, -1.0f, -1.0f, -1.0f),
        new Vec4 (1.0f, 0.0f, 1.0f, 1.0f),
        new Vec4 (1.0f, 0.0f, 1.0f, -1.0f),
        new Vec4 (1.0f, 0.0f, -1.0f, 1.0f),
        new Vec4 (1.0f, 0.0f, -1.0f, -1.0f),
        new Vec4 (-1.0f, 0.0f, 1.0f, 1.0f),
        new Vec4 (-1.0f, 0.0f, 1.0f, -1.0f),
        new Vec4 (-1.0f, 0.0f, -1.0f, 1.0f),
        new Vec4 (-1.0f, 0.0f, -1.0f, -1.0f),
        new Vec4 (1.0f, 1.0f, 0.0f, 1.0f),
        new Vec4 (1.0f, 1.0f, 0.0f, -1.0f),
        new Vec4 (1.0f, -1.0f, 0.0f, 1.0f),
        new Vec4 (1.0f, -1.0f, 0.0f, -1.0f),
        new Vec4 (-1.0f, 1.0f, 0.0f, 1.0f),
        new Vec4 (-1.0f, 1.0f, 0.0f, -1.0f),
        new Vec4 (-1.0f, -1.0f, 0.0f, 1.0f),
        new Vec4 (-1.0f, -1.0f, 0.0f, -1.0f),
        new Vec4 (1.0f, 1.0f, 1.0f, 0.0f),
        new Vec4 (1.0f, 1.0f, -1.0f, 0.0f),
        new Vec4 (1.0f, -1.0f, 1.0f, 0.0f),
        new Vec4 (1.0f, -1.0f, -1.0f, 0.0f),
        new Vec4 (-1.0f, 1.0f, 1.0f, 0.0f),
        new Vec4 (-1.0f, 1.0f, -1.0f, 0.0f),
        new Vec4 (-1.0f, -1.0f, 1.0f, 0.0f),
        new Vec4 (-1.0f, -1.0f, -1.0f, 0.0f)
    };

    private static readonly int[,] Permute = new int[,]
    { { 0, 1, 2, 3 }, { 0, 1, 3, 2 }, { 0, 0, 0, 0 }, { 0, 2, 3, 1 },
        { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 1, 2, 3, 0 },
        { 0, 2, 1, 3 }, { 0, 0, 0, 0 }, { 0, 3, 1, 2 }, { 0, 3, 2, 1 },
        { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 1, 3, 2, 0 },
        { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 },
        { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 },
        { 1, 2, 0, 3 }, { 0, 0, 0, 0 }, { 1, 3, 0, 2 }, { 0, 0, 0, 0 },
        { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 2, 3, 0, 1 }, { 2, 3, 1, 0 },
        { 1, 0, 2, 3 }, { 1, 0, 3, 2 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 },
        { 0, 0, 0, 0 }, { 2, 0, 3, 1 }, { 0, 0, 0, 0 }, { 2, 1, 3, 0 },
        { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 },
        { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 },
        { 2, 0, 1, 3 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 },
        { 3, 0, 1, 2 }, { 3, 0, 2, 1 }, { 0, 0, 0, 0 }, { 3, 1, 2, 0 },
        { 2, 1, 0, 3 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 },
        { 3, 1, 0, 2 }, { 0, 0, 0, 0 }, { 3, 2, 0, 1 }, { 3, 2, 1, 0 }
    };

    private static Vec2 Gradient2(
        int i = 0,
        int j = 0,
        int seed = Utils.HashBase)
    {
        return Simplex.Grad2Lut[Simplex.Hash(i, j, seed) & 0x7];
    }

    private static Vec3 Gradient3(
        int i = 0,
        int j = 0,
        int k = 0,
        int seed = Utils.HashBase)
    {
        return Simplex.Grad3Lut[Simplex.Hash(i, j, Simplex.Hash(k, seed, 0)) &
            0xf];
    }

    private static Vec4 Gradient4(
        int i = 0,
        int j = 0,
        int k = 0,
        int l = 0,
        int seed = Utils.HashBase)
    {
        return Simplex.Grad4Lut[Simplex.Hash(i, j, Simplex.Hash(k, l, seed)) &
            0x1f];
    }

    private static int Hash(int a = 0, int b = 0, int c = 0)
    {
        c ^= b;
        c -= b << 0xe | b >> 0x20 - 0xe;
        a ^= c;
        a -= c << 0xb | c >> 0x20 - 0xb;
        b ^= a;
        b -= a << 0x19 | a >> 0x20 - 0x19;
        c ^= b;
        c -= b << 0x10 | b >> 0x20 - 0x10;
        a ^= c;
        a -= c << 0x4 | c >> 0x20 - 0x4;
        b ^= a;
        b -= a << 0xe | a >> 0x20 - 0xe;
        c ^= b;
        c -= b << 0x18 | b >> 0x20 - 0x18;
        return c;
    }

    public static (float fac, Vec4 deriv) Eval4(in Vec4 v, int seed = Utils.HashBase)
    {
        return Simplex.Eval4(v.x, v.y, v.z, v.w, seed);
    }

    public static (float fac, Vec4 deriv) Eval4(
        float x = 0.0f,
        float y = 0.0f,
        float z = 0.0f,
        float w = 0.0f,
        int seed = Utils.HashBase)
    {
        float s = (x + y + z + w) * Simplex.F4;
        int i = Utils.Floor(x + s);
        int j = Utils.Floor(y + s);
        int k = Utils.Floor(z + s);
        int l = Utils.Floor(w + s);

        float t = (i + j + k + l) * Simplex.G4;
        float x0 = x - (i - t);
        float y0 = y - (j - t);
        float z0 = z - (k - t);
        float w0 = w - (l - t);

        int index = (x0 > y0 ? 0x20 : 0) |
            (x0 > z0 ? 0x10 : 0) | (y0 > z0 ? 0x8 : 0) |
            (x0 > w0 ? 0x4 : 0) | (y0 > w0 ? 0x2 : 0) | (z0 > w0 ? 0x1 : 0);

        // TODO: Convert this to a one-dimensional array.
        int sc0 = Simplex.Permute[index, 0];
        int sc1 = Simplex.Permute[index, 1];
        int sc2 = Simplex.Permute[index, 2];
        int sc3 = Simplex.Permute[index, 3];

        int i1 = sc0 >= 3 ? 1 : 0;
        int j1 = sc1 >= 3 ? 1 : 0;
        int k1 = sc2 >= 3 ? 1 : 0;
        int l1 = sc3 >= 3 ? 1 : 0;

        int i2 = sc0 >= 2 ? 1 : 0;
        int j2 = sc1 >= 2 ? 1 : 0;
        int k2 = sc2 >= 2 ? 1 : 0;
        int l2 = sc3 >= 2 ? 1 : 0;

        int i3 = sc0 >= 1 ? 1 : 0;
        int j3 = sc1 >= 1 ? 1 : 0;
        int k3 = sc2 >= 1 ? 1 : 0;
        int l3 = sc3 >= 1 ? 1 : 0;

        float x1 = x0 - i1 + Simplex.G4;
        float y1 = y0 - j1 + Simplex.G4;
        float z1 = z0 - k1 + Simplex.G4;
        float w1 = w0 - l1 + Simplex.G4;

        float x2 = x0 - i2 + Simplex.G4_2;
        float y2 = y0 - j2 + Simplex.G4_2;
        float z2 = z0 - k2 + Simplex.G4_2;
        float w2 = w0 - l2 + Simplex.G4_2;

        float x3 = x0 - i3 + Simplex.G4_3;
        float y3 = y0 - j3 + Simplex.G4_3;
        float z3 = z0 - k3 + Simplex.G4_3;
        float w3 = w0 - l3 + Simplex.G4_3;

        float x4 = x0 - 1.0f + Simplex.G4_4;
        float y4 = y0 - 1.0f + Simplex.G4_4;
        float z4 = z0 - 1.0f + Simplex.G4_4;
        float w4 = w0 - 1.0f + Simplex.G4_4;

        float n0 = 0.0f;
        float n1 = 0.0f;
        float n2 = 0.0f;
        float n3 = 0.0f;
        float n4 = 0.0f;

        float t20 = 0.0f;
        float t21 = 0.0f;
        float t22 = 0.0f;
        float t23 = 0.0f;
        float t24 = 0.0f;

        float t40 = 0.0f;
        float t41 = 0.0f;
        float t42 = 0.0f;
        float t43 = 0.0f;
        float t44 = 0.0f;

        Vec4 g0 = Vec4.Zero;
        Vec4 g1 = Vec4.Zero;
        Vec4 g2 = Vec4.Zero;
        Vec4 g3 = Vec4.Zero;
        Vec4 g4 = Vec4.Zero;

        float t0 = 0.5f - (x0 * x0 + y0 * y0 + z0 * z0 + w0 * w0);
        if (t0 >= 0.0f)
        {
            t20 = t0 * t0;
            t40 = t20 * t20;
            g0 = Simplex.Gradient4(i, j, k, l, seed);
            n0 = g0.x * x0 + g0.y * y0 + g0.z * z0 + g0.w * w0;
        }

        float t1 = 0.5f - (x1 * x1 + y1 * y1 + z1 * z1 + w1 * w1);
        if (t1 >= 0.0f)
        {
            t21 = t1 * t1;
            t41 = t21 * t21;
            g1 = Simplex.Gradient4(i + i1, j + j1, k + k1, l + l1, seed);
            n1 = g1.x * x1 + g1.y * y1 + g1.z * z1 + g1.w * w1;
        }

        float t2 = 0.5f - (x2 * x2 + y2 * y2 + z2 * z2 + w2 * w2);
        if (t2 >= 0.0f)
        {
            t22 = t2 * t2;
            t42 = t22 * t22;
            g2 = Simplex.Gradient4(i + i2, j + j2, k + k2, l + l2, seed);
            n2 = g2.x * x2 + g2.y * y2 + g2.z * z2 + g2.w * w2;
        }

        float t3 = 0.5f - (x3 * x3 + y3 * y3 + z3 * z3 + w3 * w3);
        if (t3 >= 0.0f)
        {
            t23 = t3 * t3;
            t43 = t23 * t23;
            g3 = Simplex.Gradient4(i + i3, j + j3, k + k3, l + l3, seed);
            n3 = g3.x * x3 + g3.y * y3 + g3.z * z3 + g3.w * w3;
        }

        float t4 = 0.5f - (x4 * x4 + y4 * y4 + z4 * z4 + w4 * w4);
        if (t4 >= 0.0f)
        {
            t24 = t4 * t4;
            t44 = t24 * t24;
            g4 = Simplex.Gradient4(i + 1, j + 1, k + 1, l + 1, seed);
            n4 = g4.x * x4 + g4.y * y4 + g4.z * z4 + g4.w * w4;
        }

        float tmp0 = t20 * t0 * n0;
        float derivx = tmp0 * x0;
        float derivy = tmp0 * y0;
        float derivz = tmp0 * z0;
        float derivw = tmp0 * w0;

        float tmp1 = t21 * t1 * n1;
        derivx += tmp1 * x1;
        derivy += tmp1 * y1;
        derivz += tmp1 * z1;
        derivw += tmp1 * w1;

        float tmp2 = t22 * t2 * n2;
        derivx += tmp2 * x2;
        derivy += tmp2 * y2;
        derivz += tmp2 * z2;
        derivw += tmp2 * w2;

        float tmp3 = t23 * t3 * n3;
        derivx += tmp3 * x3;
        derivy += tmp3 * y3;
        derivz += tmp3 * z3;
        derivw += tmp3 * w3;

        float tmp4 = t24 * t4 * n4;
        derivx += tmp4 * x4;
        derivy += tmp4 * y4;
        derivz += tmp4 * z4;
        derivw += tmp4 * w4;

        derivx *= -8.0f;
        derivy *= -8.0f;
        derivz *= -8.0f;
        derivw *= -8.0f;

        derivx += t40 * g0.x + t41 * g1.x + t42 * g2.x + t43 * g3.x +
            t44 * g4.x;
        derivy += t40 * g0.y + t41 * g1.y + t42 * g2.y + t43 * g3.y +
            t44 * g4.y;
        derivz += t40 * g0.z + t41 * g1.z + t42 * g2.z + t43 * g3.z +
            t44 * g4.z;
        derivw += t40 * g0.w + t41 * g1.w + t42 * g2.w + t43 * g3.w +
            t44 * g4.w;

        derivx *= Simplex.Scale4;
        derivy *= Simplex.Scale4;
        derivz *= Simplex.Scale4;
        derivw *= Simplex.Scale4;

        return (fac: Simplex.Scale4 * (t40 * n0 + t41 * n1 + t42 * n2 + t43 * n3 + t44 * n4),
            deriv: new Vec4(derivx, derivy, derivz, derivw));
    }

    public static (float fac, Vec3 deriv) Eval3(in Vec3 v, int seed = Utils.HashBase)
    {
        return Simplex.Eval3(v.x, v.y, v.z, seed);
    }

    public static (float fac, Vec3 deriv) Eval3(
        float x = 0.0f,
        float y = 0.0f,
        float z = 0.0f,
        int seed = Utils.HashBase)
    {
        float s = (x + y + z) * Simplex.F3;
        int i = Utils.Floor(x + s);
        int j = Utils.Floor(y + s);
        int k = Utils.Floor(z + s);

        float t = (i + j + k) * Simplex.G3;
        float x0 = x - (i - t);
        float y0 = y - (j - t);
        float z0 = z - (k - t);

        int i1 = 0;
        int j1 = 0;
        int k1 = 0;

        int i2 = 0;
        int j2 = 0;
        int k2 = 0;

        if (x0 >= y0)
        {
            if (y0 >= z0)
            {
                i1 = 1;
                i2 = 1;
                j2 = 1;
            }
            else if (x0 >= z0)
            {
                i1 = 1;
                i2 = 1;
                k2 = 1;
            }
            else
            {
                k1 = 1;
                i2 = 1;
                k2 = 1;
            }
        }
        else
        {
            if (y0 < z0)
            {
                k1 = 1;
                j2 = 1;
                k2 = 1;
            }
            else if (x0 < z0)
            {
                j1 = 1;
                j2 = 1;
                k2 = 1;
            }
            else
            {
                j1 = 1;
                i2 = 1;
                j2 = 1;
            }
        }

        float x1 = x0 - i1 + Simplex.G3;
        float y1 = y0 - j1 + Simplex.G3;
        float z1 = z0 - k1 + Simplex.G3;

        float x2 = x0 - i2 + Simplex.G3_2;
        float y2 = y0 - j2 + Simplex.G3_2;
        float z2 = z0 - k2 + Simplex.G3_2;

        float x3 = x0 - 1.0f + Simplex.G3_3;
        float y3 = y0 - 1.0f + Simplex.G3_3;
        float z3 = z0 - 1.0f + Simplex.G3_3;

        float t20 = 0.0f;
        float t21 = 0.0f;
        float t22 = 0.0f;
        float t23 = 0.0f;

        float t40 = 0.0f;
        float t41 = 0.0f;
        float t42 = 0.0f;
        float t43 = 0.0f;

        float n0 = 0.0f;
        float n1 = 0.0f;
        float n2 = 0.0f;
        float n3 = 0.0f;

        Vec3 g0 = Vec3.Zero;
        Vec3 g1 = Vec3.Zero;
        Vec3 g2 = Vec3.Zero;
        Vec3 g3 = Vec3.Zero;

        float t0 = 0.5f - (x0 * x0 + y0 * y0 + z0 * z0);
        if (t0 >= 0.0f)
        {
            g0 = Simplex.Gradient3(i, j, k, seed);
            t20 = t0 * t0;
            t40 = t20 * t20;
            n0 = g0.x * x0 + g0.y * y0 + g0.z * z0;
        }

        float t1 = 0.5f - (x1 * x1 + y1 * y1 + z1 * z1);
        if (t1 >= 0.0f)
        {
            g1 = Simplex.Gradient3(i + i1, j + j1, k + k1, seed);
            t21 = t1 * t1;
            t41 = t21 * t21;
            n1 = g1.x * x1 + g1.y * y1 + g1.z * z1;
        }

        float t2 = 0.5f - (x2 * x2 + y2 * y2 + z2 * z2);
        if (t2 >= 0.0f)
        {
            g2 = Simplex.Gradient3(i + i2, j + j2, k + k2, seed);
            t22 = t2 * t2;
            t42 = t22 * t22;
            n2 = g2.x * x2 + g2.y * y2 + g2.z * z2;
        }

        float t3 = 0.5f - (x3 * x3 + y3 * y3 + z3 * z3);
        if (t3 >= 0.0f)
        {
            g3 = Simplex.Gradient3(i + 1, j + 1, k + 1, seed);
            t23 = t3 * t3;
            t43 = t23 * t23;
            n3 = g3.x * x3 + g3.y * y3 + g3.z * z3;
        }

        float tmp0 = t20 * t0 * n0;
        float derivx = tmp0 * x0;
        float derivy = tmp0 * y0;
        float derivz = tmp0 * z0;

        float tmp1 = t21 * t1 * n1;
        derivx += tmp1 * x1;
        derivy += tmp1 * y1;
        derivz += tmp1 * z1;

        float tmp2 = t22 * t2 * n2;
        derivx += tmp2 * x2;
        derivy += tmp2 * y2;
        derivz += tmp2 * z2;

        float tmp3 = t23 * t3 * n3;
        derivx += tmp3 * x3;
        derivy += tmp3 * y3;
        derivz += tmp3 * z3;

        derivx *= -8.0f;
        derivy *= -8.0f;
        derivz *= -8.0f;

        derivx += t40 * g0.x + t41 * g1.x + t42 * g2.x + t43 * g3.x;
        derivy += t40 * g0.y + t41 * g1.y + t42 * g2.y + t43 * g3.y;
        derivz += t40 * g0.z + t41 * g1.z + t42 * g2.z + t43 * g3.z;

        derivx *= Simplex.Scale3;
        derivy *= Simplex.Scale3;
        derivz *= Simplex.Scale3;

        return (fac: Simplex.Scale3 * (t40 * n0 + t41 * n1 + t42 * n2 + t43 * n3),
            deriv: new Vec3(derivx, derivy, derivz));
    }

    public static (float fac, Vec2 deriv) Eval2(in Vec2 v, int seed = Utils.HashBase)
    {
        return Simplex.Eval2(v.x, v.y, seed);
    }

    public static (float fac, Vec2 deriv) Eval2(
        float x = 0.0f,
        float y = 0.0f,
        int seed = Utils.HashBase)
    {
        float s = (x + y) * Simplex.F2;
        int i = Utils.Floor(x + s);
        int j = Utils.Floor(y + s);

        float t = (i + j) * Simplex.G2;
        float x0 = x - (i - t);
        float y0 = y - (j - t);

        int i1 = 0;
        int j1 = 0;
        if (x0 > y0)
        {
            i1 = 1;
        }
        else
        {
            j1 = 1;
        }

        float x1 = x0 - i1 + Simplex.G2;
        float y1 = y0 - j1 + Simplex.G2;

        float x2 = x0 - 1.0f + Simplex.G2_2;
        float y2 = y0 - 1.0f + Simplex.G2_2;

        float t20 = 0.0f;
        float t21 = 0.0f;
        float t22 = 0.0f;

        float t40 = 0.0f;
        float t41 = 0.0f;
        float t42 = 0.0f;

        float n0 = 0.0f;
        float n1 = 0.0f;
        float n2 = 0.0f;

        Vec2 g0 = Vec2.Zero;
        Vec2 g1 = Vec2.Zero;
        Vec2 g2 = Vec2.Zero;

        float t0 = 0.5f - (x0 * x0 + y0 * y0);
        if (t0 >= 0.0f)
        {
            g0 = Simplex.Gradient2(i, j, seed);
            t20 = t0 * t0;
            t40 = t20 * t20;
            n0 = g0.x * x0 + g0.y * y0;
        }

        float t1 = 0.5f - (x1 * x1 + y1 * y1);
        if (t1 >= 0.0f)
        {
            g1 = Simplex.Gradient2(i + i1, j + j1, seed);
            t21 = t1 * t1;
            t41 = t21 * t21;
            n1 = g1.x * x1 + g1.y * y1;
        }

        float t2 = 0.5f - (x2 * x2 + y2 * y2);
        if (t2 >= 0.0f)
        {
            g2 = Simplex.Gradient2(i + 1, j + 1, seed);
            t22 = t2 * t2;
            t42 = t22 * t22;
            n2 = g2.x * x2 + g2.y * y2;
        }

        float tmp0 = t20 * t0 * n0;
        float derivx = tmp0 * x0;
        float derivy = tmp0 * y0;

        float tmp1 = t21 * t1 * n1;
        derivx += tmp1 * x1;
        derivy += tmp1 * y1;

        float tmp2 = t22 * t2 * n2;
        derivx += tmp2 * x2;
        derivy += tmp2 * y2;

        derivx *= -8.0f;
        derivy *= -8.0f;

        derivx += t40 * g0.x + t41 * g1.x + t42 * g2.x;
        derivy += t40 * g0.y + t41 * g1.y + t42 * g2.y;

        derivx *= Simplex.Scale2;
        derivy *= Simplex.Scale2;

        return (fac: Simplex.Scale2 * (t40 * n0 + t41 * n1 + t42 * n2),
            deriv: new Vec2(derivx, derivy));
    }

    public static (Vec2 fac, Vec2 xDeriv, Vec2 yDeriv) Noise2(in Vec2 v, int seed = Utils.HashBase)
    {
        float st = Vec2.Mag(v) * Simplex.Step2;

        (float fac, Vec2 deriv) x = Simplex.Eval2(v.x + st, v.y, seed);
        (float fac, Vec2 deriv) y = Simplex.Eval2(v.x, v.y + st, seed);

        return (fac: new Vec2(x.fac, y.fac),
            xDeriv: x.deriv,
            yDeriv: y.deriv);
    }

    public static (Vec3 fac, Vec3 xDeriv, Vec3 yDeriv, Vec3 zDeriv) Noise3(in Vec3 v, int seed = Utils.HashBase)
    {
        float st = Vec3.Mag(v) * Simplex.Step3;

        (float fac, Vec3 deriv) x = Simplex.Eval3(v.x + st, v.y, v.z, seed);
        (float fac, Vec3 deriv) y = Simplex.Eval3(v.x, v.y + st, v.z, seed);
        (float fac, Vec3 deriv) z = Simplex.Eval3(v.x, v.y, v.z + st, seed);

        return (fac: new Vec3(x.fac, y.fac, z.fac),
            xDeriv: x.deriv,
            yDeriv: y.deriv,
            zDeriv: z.deriv);
    }

    public static (Vec4 fac, Vec4 xDeriv, Vec4 yDeriv, Vec4 zDeriv, Vec4 wDeriv) Noise4(in Vec4 v, int seed = Utils.HashBase)
    {
        float st = Vec4.Mag(v) * Simplex.Step4;

        (float fac, Vec4 deriv) x = Simplex.Eval4(v.x + st, v.y, v.z, v.w, seed);
        (float fac, Vec4 deriv) y = Simplex.Eval4(v.x, v.y + st, v.z, v.w, seed);
        (float fac, Vec4 deriv) z = Simplex.Eval4(v.x, v.y, v.z + st, v.w, seed);
        (float fac, Vec4 deriv) w = Simplex.Eval4(v.x, v.y, v.z, v.w + st, seed);

        return (fac: new Vec4(x.fac, y.fac, z.fac, w.fac),
            xDeriv: x.deriv,
            yDeriv: y.deriv,
            zDeriv: z.deriv,
            wDeriv: w.deriv);
    }
}