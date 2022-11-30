using System;
using System.Collections.Generic;

/// <summary>
/// Holds methods that operate on arrays of pixels held by images.
/// </summary>
public static class Pixels
{
    #region Generic

    /// <summary>
    /// Creates a checker pattern in an array of pixels.
    /// </summary>
    /// <param name="target">target pixels</param>
    /// <param name="w">image width</param>
    /// <param name="h">image height</param>
    /// <param name="a">first color/param>
    /// <param name="b">second color/param>
    /// <param name="cols">columns/param>
    /// <param name="rows">rows/param>
    /// <returns>checker pattern</returns>
    public static T[] Checker<T>(
        in T[] target,
        in int w, in int h,
        in T a, in T b,
        in int cols = 8, in int rows = 8)
    {
        int limit = 2;
        int vCols = cols < 2 ? 2 : cols > w / limit ? w / limit : cols;
        int vRows = rows < 2 ? 2 : rows > h / limit ? h / limit : rows;
        int wch = w / vCols;
        int hchw = w * h / vRows;

        int trgLen = target.Length;
        for (int i = 0; i < trgLen; ++i)
        {
            // % 2 can be replaced by & 1 for even or odd.
            target[i] = (i % w / wch + i / hchw & 1) == 0 ? a : b;
        }
        return target;
    }

    /// <summary>
    /// Flips the pixels source array horizontally, on the x axis, and stores
    /// the result in the target array.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="target">target pixels</param>
    /// <param name="w">image width</param>
    /// <param name="h">image height</param>
    /// <returns>flipped pixels</returns>
    public static T[] FlipX<T>(in T[] source, in T[] target, in int w, in int h)
    {
        int srcLen = source.Length;
        if (source == target)
        {
            int wd2 = w / 2;
            int wn1 = w - 1;
            int lenHalf = wd2 * h;
            for (int i = 0; i < lenHalf; ++i)
            {
                int x = i % wd2;
                int yw = w * (i / wd2);
                int idxSrc = x + yw;
                int idxTrg = yw + wn1 - x;
                T t = source[idxSrc];
                source[idxSrc] = source[idxTrg];
                source[idxTrg] = t;
            }
        }
        else if (srcLen == target.Length)
        {
            int wn1 = w - 1;
            for (int i = 0; i < srcLen; ++i)
            {
                target[i / w * w + wn1 - i % w] = source[i];
            }
        }
        return target;
    }

    /// <summary>
    /// Flips the pixels source array vertically, on the y axis, and stores the
    /// result in the target array.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="target">target pixels</param>
    /// <param name="w">image width</param>
    /// <param name="h">image height</param>
    /// <returns>flipped pixels</returns>
    public static T[] FlipY<T>(in T[] source, in T[] target, in int w, in int h)
    {
        int srcLen = source.Length;
        if (source == target)
        {
            int hd2 = h / 2;
            int hn1 = h - 1;
            int lenHalf = w * hd2;
            for (int i = 0; i < lenHalf; ++i)
            {
                int j = i % w + w * (hn1 - i / w);
                T t = source[i];
                source[i] = source[j];
                source[j] = t;
            }
        }
        else if (srcLen == target.Length)
        {
            int hn1 = h - 1;
            for (int i = 0; i < srcLen; ++i)
            {
                target[(hn1 - i / w) * w + i % w] = source[i];
            }
        }
        return target;
    }

    /// <summary>
    /// Mirrors, or reflects, pixels from a source image horizontally
    /// across a pivot. The pivot is expected to be in [-1, width + 1].
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="target">target pixels</param>
    /// <param name="w">image width</param>
    /// <param name="h">image height</param>
    /// <param name="pivot">pivot</param>
    /// <param name="flip">flip reflection flag</param>
    /// <returns>mirrored pixels</returns>
    public static T[] MirrorX<T>(
        in T[] source, in T[] target, in int w, in int h,
        in int pivot = -1, in bool flip = false)
    {
        int trgLen = target.Length;
        int flipSign = flip ? 1 : -1;

        for (int i = 0; i < trgLen; ++i)
        {
            int cross = i % w - pivot;
            if (flipSign * cross < 0)
            {
                target[i] = source[i];
            }
            else
            {
                int pxOpp = pivot - cross;
                if (pxOpp > -1 && pxOpp < w)
                {
                    target[i] = source[i / w * w + pxOpp];
                }
                else
                {
                    target[i] = default(T);
                }
            }
        }

        return target;
    }

    /// <summary>
    /// Mirrors, or reflects, pixels from a source image vertically
    /// across a pivot. The pivot is expected to be in [-1, height + 1].
    /// Positive y points down to the bottom of the image.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="w">image width</param>
    /// <param name="h">image height</param>
    /// <param name="pivot">pivot</param>
    /// <param name="flip">flip reflection flag</param>
    /// <param name="target">target pixels</param>
    /// <returns>mirrored pixels</returns>
    public static T[] MirrorY<T>(
        in T[] source, in T[] target, in int w, in int h,
        in int pivot = -1, in bool flip = false)
    {
        int trgLen = target.Length;
        int flipSign = flip ? 1 : -1;

        for (int i = 0; i < trgLen; ++i)
        {
            int cross = i / w - pivot;
            if (flipSign * cross < 0)
            {
                target[i] = source[i];
            }
            else
            {
                int pyOpp = pivot - cross;
                if (pyOpp > -1 && pyOpp < h)
                {
                    target[i] = source[pyOpp * w + i % w];
                }
                else
                {
                    target[i] = default(T);
                }
            }
        }
        return target;
    }

    /// <summary>
    /// Rotates the source pixel array 90 degrees counter-clockwise. The
    /// rotation is stored in the target pixel array.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="target">target pixels</param>
    /// <param name="w">image width</param>
    /// <param name="h">image height</param>
    /// <returns>rotated pixels</returns>
    public static T[] Rotate90<T>(in T[] source, in T[] target, in int w, in int h)
    {
        int srcLen = source.Length;
        if (srcLen == target.Length)
        {
            int srcLennh = srcLen - h;
            for (int i = 0; i < srcLen; ++i)
            {
                target[srcLennh + i / w - i % w * h] = source[i];
            }
        }
        return target;
    }

    /// <summary>
    /// Rotates the source pixel array 180 degrees counter-clockwise. The
    /// rotation is stored in the target pixel array.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="target">target pixels</param>
    /// <returns>rotated pixels</returns>
    public static T[] Rotate180<T>(in T[] source, in T[] target)
    {
        int srcLen = source.Length;
        if (source == target)
        {
            int srcHalfLen = srcLen / 2;
            int srcLenn1 = srcLen - 1;
            for (int i = 0; i < srcHalfLen; ++i)
            {
                T t = source[i];
                source[i] = source[srcLenn1 - i];
                source[srcLenn1 - i] = t;
            }
        }
        else if (srcLen == target.Length)
        {
            for (int i = 0, j = srcLen - 1; i < srcLen; ++i, --j)
            {
                target[j] = source[i];
            }
        }
        return target;
    }

    /// <summary>
    /// Rotates the source pixel array 270 degrees counter-clockwise. The
    /// rotation is stored in the target pixel array.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="target">target pixels</param>
    /// <param name="w">image width</param>
    /// <param name="h">image height</param>
    /// <returns>rotated pixels</returns>
    public static T[] Rotate270<T>(in T[] source, in T[] target, in int w, in int h)
    {
        int srcLen = source.Length;
        if (srcLen == target.Length)
        {
            int hn1 = h - 1;
            for (int i = 0; i < srcLen; ++i)
            {
                target[i % w * h + hn1 - i / w] = source[i];
            }
        }
        return target;
    }

    /// <summary>
    /// Blits a source image's pixels onto a target image's pixels, using
    /// integer floor modulo to wrap the source image. The source image can be
    /// offset horizontally and/or vertically, creating the illusion of
    /// parallax.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="target">target pixels</param>
    /// <param name="wSrc">source image width</param>
    /// <param name="hSrc">source image height</param>
    /// <param name="wTrg">target image width</param>
    /// <param name="dx">horizontal pixel offset</param>
    /// <param name="dy">vertical pixel offset</param>
    /// <returns>wrapped pixels</returns>
    public static T[] Wrap<T>(
        in T[] source, in T[] target,
        in int wSrc, in int hSrc, in int wTrg,
        in int dx, in int dy)
    {
        int trgLen = target.Length;
        for (int i = 0; i < trgLen; ++i)
        {
            int yMod = (i / wTrg + dy) % hSrc;
            if ((yMod ^ hSrc) < 0 && yMod != 0) { yMod += hSrc; }

            int xMod = (i % wTrg - dx) % wSrc;
            if ((xMod ^ wSrc) < 0 && xMod != 0) { xMod += wSrc; }

            target[i] = source[xMod + wSrc * yMod];
        }
        return target;
    }

    #endregion

    #region Specific

    /// <summary>
    /// Multiplies a source pixels array's alpha by the input alpha. If the alpha
    /// is less than zero, sets the target array to clear black. If the alpha is
    /// approximately one, copies the source pixels to the target.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="target">target pixels</param>
    /// <param name="alpha">alpha</param>
    /// <returns>adjusted pixels</returns>
    public static Clr[] AdjustAlpha(in Clr[] source, in Clr[] target, in float alpha)
    {
        int srcLen = source.Length;
        if (srcLen == target.Length)
        {
            if (alpha <= 0.0f)
            {
                for (int i = 0; i < srcLen; ++i) { target[i] = Clr.ClearBlack; }
                return target;
            }

            if (Utils.Approx(alpha, 1.0f))
            {
                System.Array.Copy(source, 0, target, 0, srcLen);
                return target;
            }

            for (int i = 0; i < srcLen; ++i)
            {
                Clr srcHex = source[i];
                target[i] = new Clr(srcHex.r, srcHex.g, srcHex.b, srcHex.a * alpha);
            }
        }
        return target;
    }

    /// <summary>
    /// Adjusts a source pixels array's colors in CIE LCH. Assigns the results
    /// to a target array. Adds the adjustment to the pixel's
    /// CIE LCH representation.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="target">target pixels</param>
    /// <param name="adjust">adjustment</param>
    /// <returns>adjusted pixels</returns>
    public static Clr[] AdjustCieLch(in Clr[] source, in Clr[] target, in Vec4 adjust)
    {
        int srcLen = source.Length;
        if (srcLen == target.Length)
        {
            if (Vec4.None(adjust))
            {
                System.Array.Copy(source, 0, target, 0, srcLen);
                return target;
            }

            Dictionary<Clr, Clr> dict = new Dictionary<Clr, Clr>();
            for (int i = 0; i < srcLen; ++i)
            {
                Clr c = source[i];
                if (Clr.Any(c) && !dict.ContainsKey(c))
                {
                    Vec4 lch = Clr.StandardToCieLch(c);
                    dict.Add(c, Clr.CieLchToStandard(lch + adjust));
                }
            }

            if (dict.Count > 0)
            {
                for (int i = 0; i < srcLen; ++i)
                {
                    Clr c = source[i];
                    target[i] = dict.GetValueOrDefault(c, Clr.ClearBlack);
                }
            }
            else
            {
                for (int i = 0; i < srcLen; ++i) { target[i] = Clr.ClearBlack; }
            }
        }
        return target;
    }

    /// <summary>
    /// Adjusts the contrast of colors from a source pixels array by a factor.
    /// Uses the CIE LAB color space. The adjustment factor is expected to be in
    /// [-1.0, 1.0].
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="target">target pixels</param>
    /// <param name="fac">factor</param>
    /// <returns>adjusted pixels</returns>
    public static Clr[] AdjustContrast(in Clr[] source, in Clr[] target, in float fac)
    {
        int srcLen = source.Length;
        if (srcLen == target.Length)
        {
            float valAdjust = 1.0f + Utils.Clamp(fac, -1.0f, 1.0f);
            if (Utils.Approx(valAdjust, 1.0f))
            {
                System.Array.Copy(source, 0, target, 0, srcLen);
                return target;
            }

            Dictionary<Clr, Clr> dict = new Dictionary<Clr, Clr>();
            for (int i = 0; i < srcLen; ++i)
            {
                Clr c = source[i];
                if (Clr.Any(c))
                {
                    Clr key = Clr.Opaque(c);
                    if (!dict.ContainsKey(key))
                    {
                        Vec4 lab = Clr.StandardToCieLab(key);
                        Vec4 labAdj = new Vec4(lab.x, lab.y,
                            (lab.z - 50.0f) * valAdjust + 50.0f, lab.w);
                        dict.Add(key, Clr.CieLabToStandard(labAdj));
                    }
                }
            }

            if (dict.Count > 0)
            {
                for (int i = 0; i < srcLen; ++i)
                {
                    Clr c = source[i];
                    target[i] = Clr.CopyAlpha(
                        dict.GetValueOrDefault(Clr.Opaque(c),
                            Clr.ClearBlack), c);
                }
            }
            else
            {
                for (int i = 0; i < srcLen; ++i) { target[i] = Clr.ClearBlack; }
            }
        }
        return target;
    }

    /// <summary>
    /// Blends backdrop and overlay pixels. Forms a union of the bounding area
    /// of the two inputs. Returns a tuple containing the blended pixels, the
    /// image's width and height, and the top left corner x and y.
    /// </summary>
    /// <param name="apx">under image pixels</param>
    /// <param name="bpx">over image pixels</param>
    /// <param name="aw">under width</param>
    /// <param name="ah">under height</param>
    /// <param name="bw">over width</param>
    /// <param name="bh">over height</param>
    /// <param name="ax">under top left corner x</param>
    /// <param name="ay">under top left corner y</param>
    /// <param name="bx">over top left corner x</param>
    /// <param name="by">over top left corner y</param>
    /// <returns>tuple</returns>
    public static (Clr[] cPixels, int cw, int ch, int cx, int cy) BlendCieLab(
        in Clr[] apx, in Clr[] bpx,
        in int aw, in int ah, in int bw, in int bh,
        in int ax = 0, in int ay = 0, in int bx = 0, in int by = 0)
    {
        // TODO: TEST

        Dictionary<Clr, Vec4> dict = new Dictionary<Clr, Vec4>();
        int aLen = apx.Length;
        for (int g = 0; g < aLen; ++g)
        {
            Clr aKey = apx[g];
            if (!dict.ContainsKey(aKey))
            {
                dict.Add(aKey, Clr.StandardToCieLab(aKey));
            }
        }

        int bLen = apx.Length;
        for (int h = 0; h < bLen; ++h)
        {
            Clr bKey = apx[h];
            if (!dict.ContainsKey(bKey))
            {
                dict.Add(bKey, Clr.StandardToCieLab(bKey));
            }
        }

        // Find the bottom right corner for a and b.
        int abrx = ax + aw - 1;
        int abry = ay + ah - 1;
        int bbrx = bx + bw - 1;
        int bbry = by + bh - 1;

        // The result dimensions are the union of a and b.
        int cx = ax < bx ? ax : bx;
        int cy = ay < by ? ay : by;
        int cbrx = abrx > bbrx ? abrx : bbrx;
        int cbry = abry > bbry ? abry : bbry;
        int cw = 1 + cbrx - cx;
        int ch = 1 + cbry - cy;
        int cLen = cw * ch;

        // Find the difference between the target top left and the inputs.
        int axd = ax - cx;
        int ayd = ay - cy;
        int bxd = bx - cx;
        int byd = by - cy;

        Clr[] target = new Clr[cLen];
        for (int i = 0; i < cLen; ++i)
        {
            int x = i % cw;
            int y = i / ch;

            Clr aClr = Clr.ClearBlack;
            int axs = x - axd;
            int ays = y - ayd;
            if (ays > -1 && ays < ah && axs > -1 && axs < aw)
            {
                aClr = apx[axs + ays * aw];
            }

            Clr bClr = Clr.ClearBlack;
            int bxs = x - bxd;
            int bys = y - byd;
            if (bys > -1 && bys < bh && bxs > -1 && bxs < bw)
            {
                bClr = bpx[bxs + bys * bw];
            }

            Vec4 dest = dict.GetValueOrDefault(bClr, Vec4.Zero);
            float t = dest.w;
            if (t >= 1.0f)
            {
                target[i] = bClr;
            }
            else if (t <= 0.0f)
            {
                target[i] = aClr;
            }
            else
            {
                Vec4 orig = dict.GetValueOrDefault(aClr, Vec4.Zero);
                float v = orig.w;
                if (v <= 0.0f)
                {
                    target[i] = bClr;
                }
                else
                {
                    float u = 1.0f - t;
                    float uv = u * v;
                    float tuv = t + uv;
                    Vec4 cLab = new Vec4(
                        u * orig.x + t * dest.x,
                        u * orig.y + t * dest.y,
                        u * orig.z + t * dest.z, tuv);
                    target[i] = Clr.CieLabToStandard(cLab);
                }
            }
        }
        return (cPixels: target, cw: cw, ch: ch, cx: cx, cy: cy);
    }

    /// <summary>
    /// Clamps a pixel array to a lower and upper bounds.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="target">target pixels</param>
    /// <param name="lb">lower bound</param>
    /// <param name="ub">upper bound</param>
    /// <returns>clamped array</returns>
    public static Clr[] Clamp(
        in Clr[] source, in Clr[] target,
        float lb = 0.0f, float ub = 1.0f)
    {
        int srcLen = source.Length;
        if (srcLen == target.Length)
        {
            for (int i = 0; i < srcLen; ++i) { target[i] = Clr.Clamp(source[i], lb, ub); }
        }
        return target;
    }

    /// <summary>
    /// Generates a conic gradient, where the factor rotates on the z axis
    /// around an origin point. Best used with square images; for other aspect
    /// ratios, the origin should be adjusted accordingly.
    /// </summary>
    /// <param name="target">target pixels</param>
    /// <param name="w">image width</param>
    /// <param name="h">image height</param>
    /// <param name="cg">color gradient</param>
    /// <returns>conic gradient</returns>
    public static Clr[] GradientConic(
        in Clr[] target, in int w, in int h, in ClrGradient cg)
    {
        return Pixels.GradientConic(target, w, h, cg,
            (x, y, z) => Clr.MixRgbaStandard(x, y, z));
    }

    /// <summary>
    /// Generates a conic gradient, where the factor rotates on the z axis
    /// around an origin point. Best used with square images; for other aspect
    /// ratios, the origin should be adjusted accordingly.
    /// </summary>
    /// <param name="target">target pixels</param>
    /// <param name="w">image width</param>
    /// <param name="h">image height</param>
    /// <param name="cg">color gradient</param>
    /// <param name="easing">easing function</param>
    /// <param name="radians">radians</param>
    /// <param name="orig">origin</param>
    /// <returns>conic gradient</returns>
    public static Clr[] GradientConic(
        in Clr[] target, in int w, in int h,
        in ClrGradient cg, in Func<Clr, Clr, float, Clr> easing,
        in Vec2 orig, in float radians = 0.0f)
    {
        return Pixels.GradientConic(target, w, h, cg, easing,
            orig.x, orig.y, radians);
    }

    /// <summary>
    /// Generates a conic gradient, where the factor rotates on the z axis
    /// around an origin point. Best used with square images; for other aspect
    /// ratios, the origin should be adjusted accordingly.
    /// </summary>
    /// <param name="target">target pixels</param>
    /// <param name="w">image width</param>
    /// <param name="h">image height</param>
    /// <param name="cg">color gradient</param>
    /// <param name="easing">easing function</param>
    /// <param name="radians">radians</param>
    /// <param name="xOrig">x origin</param>
    /// <param name="yOrig">y origin</param>
    /// <returns>conic gradient</returns>
    public static Clr[] GradientConic(
        in Clr[] target, in int w, in int h,
        in ClrGradient cg, in Func<Clr, Clr, float, Clr> easing,
        in float xOrig = 0.0f, in float yOrig = 0.0f,
        in float radians = 0.0f)
    {
        float aspect = w / (float)h;
        float wInv = aspect / (w - 1.0f);
        float hInv = 1.0f / (h - 1.0f);
        float xo = (xOrig * 0.5f + 0.5f) * aspect * 2.0f - 1.0f;
        float yo = yOrig;
        float rd = radians;

        int trgLen = target.Length;
        for (int i = 0; i < trgLen; ++i)
        {
            float xn = wInv * (i % w);
            float yn = hInv * (i / w);
            float fac = Utils.OneTau * Utils.WrapRadians(
                MathF.Atan2(
                    1.0f - (yn + yn + yo),
                    xn + xn - xo - 1.0f) - rd);
            target[i] = ClrGradient.Eval(cg, fac, easing);
        }
        return target;
    }

    /// <summary>
    /// Generates a linear gradient from an origin point to a destination point.
    /// The origin and destination should be in the range [-1.0, 1.0]. The
    /// scalar projection is clamped to [0.0, 1.0].
    /// </summary>
    /// <param name="target">target pixels</param>
    /// <param name="w">image width</param>
    /// <param name="h">image height</param>
    /// <param name="cg">color gradient</param>
    /// <returns>linear gradient</returns>
    public static Clr[] GradientLinear(
            in Clr[] target, in int w, in int h, in ClrGradient cg)
    {
        return Pixels.GradientLinear(target, w, h, cg,
            (x, y, z) => Clr.MixRgbaStandard(x, y, z));
    }

    /// <summary>
    /// Generates a linear gradient from an origin point to a destination point.
    /// The origin and destination should be in the range [-1.0, 1.0]. The
    /// scalar projection is clamped to [0.0, 1.0].
    /// </summary>
    /// <param name="target">target pixels</param>
    /// <param name="w">image width</param>
    /// <param name="h">image height</param>
    /// <param name="cg">color gradient</param>
    /// <param name="easing">easing function</param>
    /// <param name="orig">origin</param>
    /// <param name="dest">destination</param>
    /// <returns>linear gradient</returns>
    public static Clr[] GradientLinear(
            in Clr[] target, in int w, in int h,
            in ClrGradient cg, in Func<Clr, Clr, float, Clr> easing,
            in Vec2 orig, in Vec2 dest)
    {
        return Pixels.GradientLinear(target, w, h, cg, easing,
            orig.x, orig.y, dest.x, dest.y);
    }

    /// <summary>
    /// Generates a linear gradient from an origin point to a destination point.
    /// The origin and destination should be in the range [-1.0, 1.0]. The
    /// scalar projection is clamped to [0.0, 1.0].
    /// </summary>
    /// <param name="target">target pixels</param>
    /// <param name="w">image width</param>
    /// <param name="h">image height</param>
    /// <param name="cg">color gradient</param>
    /// <param name="easing">easing function</param>
    /// <param name="xOrig">x origin</param>
    /// <param name="yOrig">y origin</param>
    /// <param name="xDest">x destination</param>
    /// <param name="yDest">y destination</param>
    /// <returns>linear gradient</returns>
    public static Clr[] GradientLinear(
        in Clr[] target, in int w, in int h,
        in ClrGradient cg, in Func<Clr, Clr, float, Clr> easing,
        in float xOrig = -1.0f, in float yOrig = 0.0f,
        in float xDest = 1.0f, in float yDest = 0.0f)
    {
        float bx = xOrig - xDest;
        float by = yOrig - yDest;

        float bbInv = 1.0f / MathF.Max(Utils.Epsilon, bx * bx + by * by);

        float bxbbinv = bx * bbInv;
        float bybbinv = by * bbInv;

        float xobx = xOrig * bxbbinv;
        float yoby = yOrig * bybbinv;
        float bxwInv2 = 2.0f / (w - 1.0f) * bxbbinv;
        float byhInv2 = 2.0f / (h - 1.0f) * bybbinv;

        int trgLen = target.Length;
        for (int i = 0; i < trgLen; ++i)
        {
            float fac = Utils.Clamp(xobx + bxbbinv - bxwInv2 * (i % w)
                                 + (yoby + byhInv2 * (i / w) - bybbinv));
            target[i] = ClrGradient.Eval(cg, fac, easing);
        }
        return target;
    }

    /// <summary>
    /// Maps the colors of a source pixels array to those of a gradient using
    /// the source's luminance. Retains the original color's transparency.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="target">target pixels</param>
    /// <param name="cg">color gradient</param>
    /// <returns>gradient map</returns>
    public static Clr[] GradientMap(
        in Clr[] source, in Clr[] target, in ClrGradient cg)
    {
        return Pixels.GradientMap(source, target, cg,
            (x, y, z) => Clr.MixRgbaStandard(x, y, z));
    }

    /// <summary>
    /// Maps the colors of a source pixels array to those of a gradient using
    /// the source's luminance. Retains the original color's transparency.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="target">target pixels</param>
    /// <param name="cg">color gradient</param>
    /// <param name="easing">easing function</param>
    /// <returns>gradient map</returns>
    public static Clr[] GradientMap(
        in Clr[] source, in Clr[] target,
        in ClrGradient cg, in Func<Clr, Clr, float, Clr> easing)
    {
        int srcLen = source.Length;
        if (srcLen == target.Length)
        {
            Dictionary<Clr, Clr> dict = new Dictionary<Clr, Clr>();
            for (int i = 0; i < srcLen; ++i)
            {
                Clr c = source[i];
                if (Clr.Any(c))
                {
                    Clr key = Clr.Opaque(c);
                    if (!dict.ContainsKey(key))
                    {
                        float lum = Clr.StandardLuminance(key);
                        float fac = lum <= 0.0031308f ?
                            lum * 12.92f :
                            MathF.Pow(lum, 1.0f / 2.4f) * 1.055f - 0.055f;
                        dict.Add(key, ClrGradient.Eval(cg, fac, easing));
                    }
                }
            }

            if (dict.Count > 0)
            {
                for (int i = 0; i < srcLen; ++i)
                {
                    Clr c = source[i];
                    target[i] = Clr.CopyAlpha(
                        dict.GetValueOrDefault(Clr.Opaque(c),
                            Clr.ClearBlack), c);
                }
            }
            else
            {
                for (int i = 0; i < srcLen; ++i) { target[i] = Clr.ClearBlack; }
            }
        }
        return target;
    }

    /// <summary>
    /// Generates a radial gradient from an origin point. The origin should be
    /// in the range [-1.0, 1.0]. Does not account for aspect ratio, so an image
    /// that isn't 1:1 will result in an ellipsoid.
    /// </summary>
    /// <param name="target">target pixels</param>
    /// <param name="w">image width</param>
    /// <param name="h">image height</param>
    /// <param name="cg">color gradient</param>
    /// <param name="easing">easing function</param>
    /// <param name="xOrig">x origin</param>
    /// <param name="yOrig">y origin</param>
    /// <param name="radius">radius</param>
    /// <returns>linear gradient</returns>
    public static Clr[] GradientRadial(
        in Clr[] target, in int w, in int h,
        in ClrGradient cg, in Func<Clr, Clr, float, Clr> easing,
        in float xOrig, in float yOrig, in float radius)
    {
        float hInv2 = 2.0f / (h - 1.0f);
        float wInv2 = 2.0f / (w - 1.0f);

        float r2 = radius + radius;
        float rsqInv = 1.0f / MathF.Max(Utils.Epsilon, r2 * r2);

        float yon1 = yOrig - 1.0f;
        float xop1 = xOrig + 1.0f;

        int trgLen = target.Length;
        for (int i = 0; i < trgLen; ++i)
        {
            float ay = yon1 + hInv2 * (i / w);
            float ax = xop1 - wInv2 * (i % w);
            float fac = 1.0f - (ax * ax + ay * ay) * rsqInv;
            target[i] = ClrGradient.Eval(cg, fac, easing);
        }
        return target;
    }

    /// <summary>
    /// Inverts colors from a source pixels array in CIE LAB.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="target">target pixels</param>
    /// <returns>inverted pixels</returns>
    public static Clr[] InvertCieLab(
        in Clr[] source, in Clr[] target)
    {
        return Pixels.InvertCieLab(source, target,
            true, true, true, false);
    }

    /// <summary>
    /// Inverts colors from a source pixels array in CIE LAB.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="target">target pixels</param>
    /// <param name="inv">inversion</param>
    /// <returns>inverted pixels</returns>
    public static Clr[] InvertCieLab(
        in Clr[] source, in Clr[] target, Vec4 inv)
    {
        return Pixels.InvertCieLab(source, target,
            inv.z != 0.0f, inv.x != 0.0f, inv.y != 0.0f, inv.w != 0.0f);
    }

    /// <summary>
    /// Inverts colors from a source pixels array in CIE LAB.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="target">target pixels</param>
    /// <param name="l">invert lightness</param>
    /// <param name="a">invert a</param>
    /// <param name="b">invert b</param>
    /// <param name="alpha">invert alpha</param>
    /// <returns>inverted pixels</returns>
    public static Clr[] InvertCieLab(
        in Clr[] source, in Clr[] target,
        bool l, bool a, bool b, bool alpha)
    {
        int srcLen = source.Length;
        if (srcLen == target.Length)
        {
            if (!alpha && !l && !a && !b)
            {
                System.Array.Copy(source, 0, target, 0, srcLen);
                return target;
            }

            float aSign = a ? -1.0f : 1.0f;
            float bSign = b ? -1.0f : 1.0f;

            Dictionary<Clr, Clr> dict = new Dictionary<Clr, Clr>();
            for (int i = 0; i < srcLen; ++i)
            {
                Clr c = source[i];
                if (!dict.ContainsKey(c))
                {
                    Vec4 lab = Clr.StandardToCieLab(c);
                    Vec4 inv = new Vec4(
                        lab.x * aSign,
                        lab.y * bSign,
                        l ? 100.0f - lab.z : lab.z,
                        alpha ? 1.0f - lab.w : lab.w);
                    dict.Add(c, Clr.CieLabToStandard(inv));
                }
            }

            for (int i = 0; i < srcLen; ++i)
            {
                Clr c = source[i];
                target[i] = dict.GetValueOrDefault(c, Clr.ClearBlack);
            }
        }
        return target;
    }

    /// <summary>
    /// Multiplies the red, green and blue color channels of a pixel array
    /// by the alpha channel. If alpha is less than or equal to zero,
    /// returns clear black.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="target">target pixels</param>
    /// <returns>premultiplied array</returns>
    public static Clr[] Premul(in Clr[] source, in Clr[] target)
    {
        int srcLen = source.Length;
        if (srcLen == target.Length)
        {
            for (int i = 0; i < srcLen; ++i) { target[i] = Clr.Premul(source[i]); }
        }
        return target;
    }

    /// <summary>
    /// Divides the red, green and blue color channels of a pixel array
    /// by the alpha channel. Reverses pre-multiplication. If alpha is
    /// less than or equal to zero, returns clear black.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="target">target pixels</param>
    /// <returns>premultiplied array</returns>
    public static Clr[] Unpremul(in Clr[] source, in Clr[] target)
    {
        int srcLen = source.Length;
        if (srcLen == target.Length)
        {
            for (int i = 0; i < srcLen; ++i) { target[i] = Clr.Unpremul(source[i]); }
        }
        return target;
    }

    #endregion
}