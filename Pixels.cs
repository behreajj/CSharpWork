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
    /// Expands the image to the nearest greater power of two.
    /// Centers the source image within the expanded dimensions.
    /// Returns a tuple with the expanded pixels and the 
    /// new width and height.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="w">image width</param>
    /// <param name="h">image height</param>
    /// <param name="uniform">use uniform size</param>
    /// <returns>tuple</returns>
    public static (T[] pixels, int w, int h) ExpandToPow2<T>(
        in T[] source, in int w, in int h, in bool uniform = false)
    {
        int wTrg;
        int hTrg;
        if (uniform)
        {
            wTrg = Utils.NextPowerOf2(w);
            hTrg = Utils.NextPowerOf2(h);
        }
        else
        {
            int u = Utils.NextPowerOf2(Utils.Max(w, h));
            wTrg = u;
            hTrg = u;
        }

        int trgLen = wTrg * hTrg;
        T[] target = new T[trgLen];
        int srcLen = source.Length;

        if (w == wTrg && h == hTrg)
        {
            System.Array.Copy(source, 0, target, 0, srcLen);
            return (pixels: target, w: wTrg, h: hTrg);
        }

        int xtl = (int)(0.5f + wTrg * 0.5f) - (int)(0.5f + w * 0.5f);
        int ytl = (int)(0.5f + hTrg * 0.5f) - (int)(0.5f + h * 0.5f);
        for (int i = 0; i < trgLen; ++i)
        {
            int xSrc = (i % wTrg) - xtl;
            int ySrc = (i / hTrg) - ytl;
            if (ySrc > -1 && ySrc < h && xSrc > -1 && xSrc < w)
            {
                target[i] = source[xSrc + ySrc * w];
            }
            else
            {
                target[i] = default;
            }
        }
        return (pixels: target, w: wTrg, h: hTrg);
    }

    /// <summary>
    /// Fills the pixels target array with a color.
    /// </summary>
    /// <param name="c">fill color</param>
    /// <param name="target">target pixels</param>
    /// <returns>filled array</returns>
    public static T[] Fill<T>(in T[] target, in T c)
    {
        int len = target.Length;
        for (int i = 0; i < len; ++i) { target[i] = c; }
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
                (source[idxTrg], source[idxSrc]) = (source[idxSrc], source[idxTrg]);
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
                (source[j], source[i]) = (source[i], source[j]);
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
    /// <param name="pivot">pivot</param>
    /// <param name="flip">flip reflection flag</param>
    /// <returns>mirrored pixels</returns>
    public static T[] MirrorX<T>(
        in T[] source, in T[] target, in int w,
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
                    target[i] = default;
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
                    target[i] = default;
                }
            }
        }
        return target;
    }

    /// <summary>
    /// Rotates the source pixel array 90 degrees counter-clockwise. The
    /// rotation is stored in the target pixel array. The rotated image's
    /// width and height are swapped.
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
                (source[srcLenn1 - i], source[i]) = (source[i], source[srcLenn1 - i]);
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
    /// rotation is stored in the target pixel array. The rotated image's
    /// width and height are swapped.
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
                Pixels.Fill(target, Clr.ClearBlack);
                return target;
            }

            if (Utils.Approx(alpha, 1.0f))
            {
                System.Array.Copy(source, 0, target, 0, srcLen);
                return target;
            }

            for (int i = 0; i < srcLen; ++i)
            {
                Clr c = source[i];
                target[i] = new Clr(c.r, c.g, c.b, c.a * alpha);
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

            Dictionary<Clr, Clr> dict = new();
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
                Pixels.Fill(target, Clr.ClearBlack);
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

            Dictionary<Clr, Clr> dict = new();
            for (int i = 0; i < srcLen; ++i)
            {
                Clr c = source[i];
                if (Clr.Any(c))
                {
                    Clr key = Clr.Opaque(c);
                    if (!dict.ContainsKey(key))
                    {
                        Vec4 lab = Clr.StandardToCieLab(key);
                        Vec4 labAdj = new(lab.x, lab.y,
                            (lab.z - 50.0f) * valAdjust + 50.0f, lab.w);
                        dict.Add(key, Clr.CieLabToStandard(labAdj));
                    }
                }
            }

            if (dict.Count > 0)
            {
                Clr clearBlack = Clr.ClearBlack;
                for (int i = 0; i < srcLen; ++i)
                {
                    Clr c = source[i];
                    target[i] = Clr.CopyAlpha(
                        dict.GetValueOrDefault(Clr.Opaque(c),
                            clearBlack), c);
                }
            }
            else
            {
                Pixels.Fill(target, Clr.ClearBlack);
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
    /// <returns>tuple</returns>
    public static (Clr[] pixels, int w, int h, int x, int y) BlendCieLab(
        in Clr[] apx, in Clr[] bpx,
        in int aw, in int ah, in int bw, in int bh)
    {
        int wLrg = aw > bw ? aw : bw;
        int hLrg = ah > bh ? ah : bh;

        // The 0.5 is to bias the rounding.
        float cx = 0.5f + wLrg * 0.5f;
        float cy = 0.5f + hLrg * 0.5f;

        int ax = aw == wLrg ? 0 : (int)(cx - aw * 0.5f);
        int ay = ah == hLrg ? 0 : (int)(cy - ah * 0.5f);
        int bx = bw == wLrg ? 0 : (int)(cx - bw * 0.5f);
        int by = bh == hLrg ? 0 : (int)(cy - bh * 0.5f);

        return Pixels.BlendCieLab(apx, bpx,
            aw, ah, bw, bh,
            ax, ay, bx, by);
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
    public static (Clr[] pixels, int w, int h, int x, int y) BlendCieLab(
        in Clr[] apx, in Clr[] bpx,
        in int aw, in int ah, in int bw, in int bh,
        in int ax, in int ay, in int bx, in int by)
    {
        // Find the bottom right corner for a and b.
        int abrx = ax + aw - 1;
        int abry = ay + ah - 1;
        int bbrx = bx + bw - 1;
        int bbry = by + bh - 1;

        Dictionary<Clr, Vec4> dict = new()
        {
            { Clr.ClearBlack, Vec4.Zero }
        };

        // Blending only necessary at the intersection of a and b.
        int dx = ax > bx ? ax : bx;
        int dy = ay > by ? ay : by;
        int dbrx = abrx < bbrx ? abrx : bbrx;
        int dbry = abry < bbry ? abry : bbry;
        int dw = 1 + dbrx - dx;
        int dh = 1 + dbry - dy;
        if (dw > 0 && dh > 0)
        {
            // Find the difference between the intersection top left corner
            // and the top left corners of a and b.
            int axid = ax - dx;
            int ayid = ay - dy;
            int bxid = bx - dx;
            int byid = by - dy;

            int dLen = dw * dh;
            for (int h = 0; h < dLen; ++h)
            {
                int x = h % dw;
                int y = h / dw;

                int axs = x - axid;
                int ays = y - ayid;
                Clr aClr = apx[axs + ays * aw];
                if (!dict.ContainsKey(aClr))
                {
                    dict.Add(aClr, Clr.StandardToCieLab(aClr));
                }

                int bxs = x - bxid;
                int bys = y - byid;
                Clr bClr = bpx[bxs + bys * bw];
                if (!dict.ContainsKey(bClr))
                {
                    dict.Add(bClr, Clr.StandardToCieLab(bClr));
                }
            }
        }

        // The result dimensions are the union of a and b.
        int cx = ax < bx ? ax : bx;
        int cy = ay < by ? ay : by;
        int cbrx = abrx > bbrx ? abrx : bbrx;
        int cbry = abry > bbry ? abry : bbry;
        int cw = 1 + cbrx - cx;
        int ch = 1 + cbry - cy;
        int cLen = cw * ch;

        // Find the difference between the union top left corner and the
        // top left corners of a and b.
        int axud = ax - cx;
        int ayud = ay - cy;
        int bxud = bx - cx;
        int byud = by - cy;

        Vec4 zero = Vec4.Zero;
        Clr[] target = new Clr[cLen];
        for (int i = 0; i < cLen; ++i)
        {
            int x = i % cw;
            int y = i / ch;

            Clr aClr = Clr.ClearBlack;
            int axs = x - axud;
            int ays = y - ayud;
            if (ays > -1 && ays < ah && axs > -1 && axs < aw)
            {
                aClr = apx[axs + ays * aw];
            }

            Clr bClr = Clr.ClearBlack;
            int bxs = x - bxud;
            int bys = y - byud;
            if (bys > -1 && bys < bh && bxs > -1 && bxs < bw)
            {
                bClr = bpx[bxs + bys * bw];
            }

            float t = bClr.a;
            if (t >= 1.0f) { target[i] = bClr; }
            else if (t <= 0.0f) { target[i] = aClr; }
            else
            {
                float v = aClr.a;
                if (v <= 0.0f) { target[i] = bClr; }
                else
                {
                    float u = 1.0f - t;
                    float uv = u * v;
                    float tuv = t + uv;

                    Vec4 orig = dict.GetValueOrDefault(aClr, zero);
                    Vec4 dest = dict.GetValueOrDefault(bClr, zero);
                    Vec4 cLab = new(
                        u * orig.x + t * dest.x,
                        u * orig.y + t * dest.y,
                        u * orig.z + t * dest.z, tuv);
                    target[i] = Clr.CieLabToStandard(cLab);
                }
            }
        }
        return (pixels: target, w: cw, h: ch, x: cx, y: cy);
    }

    /// <summary>
    /// Blurs an array of pixels by averaging each color with its
    /// neighbors in 8 directions. The step determines the size of the kernel,
    /// where the minimum step of 1 will make a 3x3, 9 pixel kernel. Averages
    /// the color's CIE LAB representation.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="target">target pixels</param>  
    /// <param name="w">image width</param>
    /// <param name="h">image height</param>
    /// <param name="step">kernel step</param>
    /// <returns>blurred pixels</returns>
    public static Clr[] BlurBoxCieLab(
        in Clr[] source, in Clr[] target,
        in int w, in int h, in int step = 1)
    {
        int srcLen = source.Length;
        if (srcLen == target.Length)
        {
            Dictionary<Clr, Vec4> dict = new();
            for (int i = 0; i < srcLen; ++i)
            {
                Clr srgb = source[i];
                if (!dict.ContainsKey(srgb))
                {
                    dict.Add(srgb, Clr.StandardToCieLab(srgb));
                }
            }

            int stepVal = step < 1 ? 1 : step;
            int wKrn = 1 + stepVal * 2;
            int krnLen = wKrn * wKrn;
            float denom = 1.0f / krnLen;
            Vec4 zero = Vec4.Zero;

            for (int i = 0; i < srcLen; ++i)
            {
                // Subtract step to center the kernel in the inner for loop.
                int xSrc = i % w - stepVal;
                int ySrc = i / w - stepVal;
                Clr srgb = source[i];

                float lSum = 0.0f;
                float aSum = 0.0f;
                float bSum = 0.0f;
                float tSum = 0.0f;

                for (int j = 0; j < krnLen; ++j)
                {
                    int xComp = xSrc + j % wKrn;
                    int yComp = ySrc + j / wKrn;
                    if (yComp > -1 && yComp < h && xComp > -1 && xComp < w)
                    {
                        Vec4 labNgbr = dict.GetValueOrDefault(
                            source[xComp + yComp * w], zero);
                        lSum += labNgbr.z;
                        aSum += labNgbr.x;
                        bSum += labNgbr.y;
                        tSum += labNgbr.w;
                    }
                    else
                    {
                        // When the kernel is out of bounds, sample the
                        // central color but do not tally alpha.
                        Vec4 labCtr = dict.GetValueOrDefault(srgb, zero);
                        lSum += labCtr.z;
                        aSum += labCtr.x;
                        bSum += labCtr.y;
                    }
                }

                Vec4 labAvg = new(
                    aSum * denom,
                    bSum * denom,
                    lSum * denom,
                    tSum * denom);
                target[i] = Clr.CieLabToStandard(labAvg);
            }
        }
        return target;
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
        in float lb = 0.0f, in float ub = 1.0f)
    {
        int srcLen = source.Length;
        if (srcLen == target.Length)
        {
            for (int i = 0; i < srcLen; ++i)
            {
                target[i] = Clr.Clamp(source[i], lb, ub);
            }
        }
        return target;
    }

    /// <summary>
    /// Internal helper function to sample a source image with a bilinear color
    /// mix. Clamps the result to [0.0, 1.0].
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <returns>mixed color</returns>
    static Clr FilterBilinear(
        in Clr[] source, in float xSrc, in float ySrc,
        in int wSrc, in int hSrc)
    {
        bool yPos = ySrc > 0.0f;
        bool yNeg = ySrc < 0.0f;
        int yi = (int)ySrc;
        int yf = yPos ? yi : yNeg ? yi - 1 : 0;
        int yc = yPos ? yi + 1 : yNeg ? yi : 0;

        bool yfInBounds = yf > -1 && yf < hSrc;
        bool ycInBounds = yc > -1 && yc < hSrc;

        bool xPos = xSrc > 0.0f;
        bool xNeg = xSrc < 0.0f;
        int xi = (int)xSrc;
        int xf = xPos ? xi : xNeg ? xi - 1 : 0;
        int xc = xPos ? xi + 1 : xNeg ? xi : 0;

        bool xfInBounds = xf > -1 && xf < wSrc;
        bool xcInBounds = xc > -1 && xc < wSrc;

        // Pixel corners colors.
        Clr c00 = xfInBounds && yfInBounds ? source[xf + yf * wSrc] : Clr.ClearBlack;
        Clr c10 = xcInBounds && yfInBounds ? source[xc + yf * wSrc] : Clr.ClearBlack;
        Clr c11 = xcInBounds && ycInBounds ? source[xc + yc * wSrc] : Clr.ClearBlack;
        Clr c01 = xfInBounds && ycInBounds ? source[xf + yc * wSrc] : Clr.ClearBlack;

        float xErr = xSrc - xf;

        float a0 = 0.0f;
        float r0 = 0.0f;
        float g0 = 0.0f;
        float b0 = 0.0f;

        float a00 = c00.a;
        float a10 = c10.a;
        if (a00 > 0.0f || a10 > 0.0f)
        {
            float u = 1.0f - xErr;
            a0 = u * a00 + xErr * a10;
            if (a0 > 0.0f)
            {
                r0 = u * c00.r + xErr * c10.r;
                g0 = u * c00.g + xErr * c10.g;
                b0 = u * c00.b + xErr * c10.b;
            }
        }

        float a1 = 0.0f;
        float r1 = 0.0f;
        float g1 = 0.0f;
        float b1 = 0.0f;

        float a01 = c01.a;
        float a11 = c11.a;
        if (a01 > 0.0f || a11 > 0.0f)
        {
            float u = 1.0f - xErr;
            a1 = u * a01 + xErr * a11;
            if (a1 > 0.0f)
            {
                r1 = u * c01.r + xErr * c11.r;
                g1 = u * c01.g + xErr * c11.g;
                b1 = u * c01.b + xErr * c11.b;
            }
        }

        if (a0 > 0.0f || a1 > 0.0f)
        {
            float yErr = ySrc - yf;
            float u = 1.0f - yErr;
            float a2 = u * a0 + yErr * a1;
            if (a2 > 0.0f)
            {
                return Clr.Clamp(new Clr(
                    u * r0 + yErr * r1,
                    u * g0 + yErr * g1,
                    u * b0 + yErr * b1, a2));
            }
        }

        return Clr.ClearBlack;
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
            Dictionary<Clr, Clr> dict = new();
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
                Clr clearBlack = Clr.ClearBlack;
                for (int i = 0; i < srcLen; ++i)
                {
                    Clr c = source[i];
                    target[i] = Clr.CopyAlpha(
                        dict.GetValueOrDefault(Clr.Opaque(c),
                            clearBlack), c);
                }
            }
            else
            {
                Pixels.Fill(target, Clr.ClearBlack);
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
        in Clr[] source, in Clr[] target, in Vec4 inv)
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
        in bool l, in bool a, in bool b, in bool alpha)
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

            Dictionary<Clr, Clr> dict = new();
            for (int i = 0; i < srcLen; ++i)
            {
                Clr c = source[i];
                if (!dict.ContainsKey(c))
                {
                    Vec4 lab = Clr.StandardToCieLab(c);
                    Vec4 inv = new(
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
    /// Masks backdrop according to overlay pixels.
    /// Returns a tuple containing the blended pixels, the
    /// image's width and height, and the top left corner x and y.
    /// </summary>
    /// <param name="apx">under image pixels</param>
    /// <param name="bpx">over image pixels</param>
    /// <param name="aw">under width</param>
    /// <param name="ah">under height</param>
    /// <param name="bw">over width</param>
    /// <param name="bh">over height</param>
    /// <returns>tuple</returns>
    public static (Clr[] pixels, int w, int h, int x, int y) Mask(
            in Clr[] apx, in Clr[] bpx,
            in int aw, in int ah, in int bw, in int bh)
    {
        int wLrg = aw > bw ? aw : bw;
        int hLrg = ah > bh ? ah : bh;

        // The 0.5 is to bias the rounding.
        float cx = 0.5f + wLrg * 0.5f;
        float cy = 0.5f + hLrg * 0.5f;

        int ax = aw == wLrg ? 0 : (int)(cx - aw * 0.5f);
        int ay = ah == hLrg ? 0 : (int)(cy - ah * 0.5f);
        int bx = bw == wLrg ? 0 : (int)(cx - bw * 0.5f);
        int by = bh == hLrg ? 0 : (int)(cy - bh * 0.5f);

        return Pixels.Mask(apx, bpx,
            aw, ah, bw, bh,
            ax, ay, bx, by);
    }

    /// <summary>
    /// Masks backdrop according to overlay pixels.
    /// Returns a tuple containing the blended pixels, the
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
    public static (Clr[] pixels, int w, int h, int x, int y) Mask(
        in Clr[] apx, in Clr[] bpx,
        in int aw, in int ah, in int bw, in int bh,
        in int ax, in int ay, in int bx, in int by)
    {
        // Find the bottom right corner for a and b.
        int abrx = ax + aw - 1;
        int abry = ay + ah - 1;
        int bbrx = bx + bw - 1;
        int bbry = by + bh - 1;

        // The result dimensions are the intersection of a and b.
        int cx = ax > bx ? ax : bx;
        int cy = ay > by ? ay : by;
        int cbrx = abrx < bbrx ? abrx : bbrx;
        int cbry = abry < bbry ? abry : bbry;
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
            int bxs = x - bxd;
            int bys = y - byd;
            if (bys > -1 && bys < bh && bxs > -1 && bxs < bw)
            {
                Clr bClr = bpx[bxs + bys * bw];
                if (bClr.a > 0.0f)
                {
                    int axs = x - axd;
                    int ays = y - ayd;
                    if (ays > -1 && ays < ah && axs > -1 && axs < aw)
                    {
                        Clr aClr = apx[axs + ays * aw];
                        if (aClr.a > 0.0f)
                        {
                            target[i] = new Clr(aClr.r, aClr.g, aClr.b,
                               aClr.a * bClr.a);
                        }
                        else { target[i] = Clr.ClearBlack; }
                    }
                    else { target[i] = Clr.ClearBlack; }
                }
                else { target[i] = Clr.ClearBlack; }
            }
            else { target[i] = Clr.ClearBlack; }
        }
        return (pixels: target, w: cw, h: ch, x: cx, y: cy);
    }

    /// <summary>
    /// Mirrors, or reflects, pixels from a source image across the axis
    /// described by an origin and destination. Coordinates are expected to be
    /// in the range [-1.0, 1.0]. Out-of-bounds pixels are omitted from the
    /// mirror.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="target">target pixels</param>
    /// <param name="w">image width</param>
    /// <param name="h">image height</param>
    /// <param name="orig">origin</param>
    /// <param name="dest">destination</param>
    /// <param name="flip">flip mirror</param>
    /// <returns>palette</returns>    
    public static Clr[] MirrorBilinear(
        in Clr[] source, in Clr[] target,
        in int w, in int h,
        in Vec2 orig, in Vec2 dest,
        in bool flip = false)
    {
        return Pixels.MirrorBilinear(
            source, target, w, h,
            orig.x, orig.y, dest.x, dest.y,
            flip);
    }

    /// <summary>
    /// Mirrors, or reflects, pixels from a source image across the axis
    /// described by an origin and destination. Coordinates are expected to be
    /// in the range [-1.0, 1.0]. Out-of-bounds pixels are omitted from the
    /// mirror.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="target">target pixels</param>
    /// <param name="w">image width</param>
    /// <param name="h">image height</param>
    /// <param name="xOrig">x origin</param>
    /// <param name="yOrig">y origin</param>
    /// <param name="xDest">x destination</param>
    /// <param name="yDest">y destination</param>
    /// <param name="flip">flip mirror</param>
    /// <returns>palette</returns>    
    public static Clr[] MirrorBilinear(
        in Clr[] source, in Clr[] target,
        in int w, in int h,
        in float xOrig = -1.0f, in float yOrig = 0.0f,
        in float xDest = 1.0f, in float yDest = 0.0f,
        in bool flip = false)
    {
        float wfn1 = w - 1.0f;
        float hfn1 = h - 1.0f;
        float wfp1Half = (w + 1.0f) * 0.5f;
        float hfp1Half = (h + 1.0f) * 0.5f;

        float ax = (xOrig + 1.0f) * wfp1Half - 0.5f;
        float bx = (xDest + 1.0f) * wfp1Half - 0.5f;
        float ay = (yOrig - 1.0f) * -hfp1Half - 0.5f;
        float by = (yDest - 1.0f) * -hfp1Half - 0.5f;

        float dx = bx - ax;
        float dy = by - ay;
        bool dxZero = Utils.Approx(dx, 0.0f, 0.5f);
        bool dyZero = Utils.Approx(dy, 0.0f, 0.5f);

        if (dxZero && dyZero)
        {
            System.Array.Copy(source, 0, target, 0,
                source.Length < target.Length ?
                source.Length : target.Length);
            return target;
        }

        if (dxZero)
        {
            return Pixels.MirrorX(source, target, w,
                Utils.Round(bx), flip ? ay > by : by > ay);
        }

        if (dyZero)
        {
            return Pixels.MirrorY(source, target, w, h,
                Utils.Round(by), flip ? bx > ax : ax > bx);
        }

        float dMagSqInv = 1.0f / (dx * dx + dy * dy);
        float flipSign = flip ? -1.0f : 1.0f;

        int trgLen = target.Length;
        for (int k = 0; k < trgLen; ++k)
        {
            float cy = k / w;
            float ey = cy - ay;
            float cx = k % w;
            float ex = cx - ax;

            float cross = ex * dy - ey * dx;
            if (flipSign * cross < 0.0f)
            {
                target[k] = source[k];
            }
            else
            {
                float t = (ex * dx + ey * dy) * dMagSqInv;
                float u = 1.0f - t;

                float pyProj = u * ay + t * by;
                float pyOpp = pyProj + pyProj - cy;
                float pxProj = u * ax + t * bx;
                float pxOpp = pxProj + pxProj - cx;

                // Default to omitting pixels that are out-of-bounds, rather than
                // wrapping with floor modulo or clamping.
                if (pyOpp >= 0.0f && pyOpp <= hfn1 &&
                    pxOpp >= 0.0f && pxOpp <= wfn1)
                {
                    target[k] = Pixels.FilterBilinear(source, pxOpp, pyOpp, w, h);
                }
                else
                {
                    target[k] = Clr.ClearBlack;
                }
            }
        }
        return target;
    }

    /// <summary>
    /// Sets the alpha channel of all colors from the source array to 1.0.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="target">target pixels</param>
    /// <returns>opaque array</returns>
    public static Clr[] Opaque(in Clr[] source, in Clr[] target)
    {
        int srcLen = source.Length;
        if (srcLen == target.Length)
        {
            for (int i = 0; i < srcLen; ++i) { target[i] = Clr.Opaque(source[i]); }
        }
        return target;
    }

    /// <summary>
    /// Extracts a palette from a source pixels array using an octree in CIE
    /// LAB. The size of the palette depends on the capacity of each node in the
    /// octree. Does not retain alpha component of image pixels. The threshold
    /// describes the minimum number of unique colors in the image beneath which
    /// it is preferable to not engage the octree. Once the octree has been
    /// used, colors produced may not be in gamut.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="capacity">node capacity</param>
    /// <param name="threshold">threshold</param>
    /// <returns>palette</returns>
    public static Clr[] PaletteExtract(
        in Clr[] source, in int capacity,
        in int threshold = 256)
    {
        SortedSet<Clr> uniqueColors = new();
        int srcLen = source.Length;
        for (int h = 0; h < srcLen; ++h)
        {
            Clr c = source[h];
            if (Clr.Any(c))
            {
                uniqueColors.Add(Clr.Opaque(c));
            }
        }

        // If under threshold, do not engage octree.
        int valThresh = threshold < 3 ? 3 : threshold;
        int uniquesLen = uniqueColors.Count;
        if (uniquesLen < valThresh)
        {
            Clr[] under = new Clr[1 + uniquesLen];
            under[0] = Clr.ClearBlack;
            int i = 0;
            foreach (Clr unique in uniqueColors)
            {
                under[++i] = unique;
            }
            return under;
        }

        Octree oct = new(Bounds3.Lab, capacity);
        foreach (Clr unique in uniqueColors)
        {
            Vec4 v = Clr.StandardToCieLab(unique);
            oct.Insert(v.xyz);
        }
        oct.Cull();

        List<Vec3> centers = new();
        Octree.CentersMean(oct, centers, false);
        int centersLen = centers.Count;
        Clr[] over = new Clr[1 + centersLen];
        over[0] = Clr.ClearBlack;
        for (int j = 0; j < centersLen; ++j)
        {
            Vec3 center = centers[j];
            over[1 + j] = Clr.CieLabToStandard(Vec4.Promote(center, 1.0f));
        }
        return over;
    }

    /// <summary>
    /// Applies a palette to an array of pixels using an Octree to find the
    /// nearest match in Euclidean space. Retains the original color's
    /// transparency.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="target">target pixels</param>
    /// <param name="palette">color palette</param>
    /// <param name="radius">query radius</param>
    /// <param name="capacity">node capacity</param>
    /// <returns>palette map</returns>
    public static Clr[] PaletteMap(
        in Clr[] source, in Clr[] target, in Clr[] palette,
        in float radius = 128.0f, in int capacity = 16)
    {
        int srcLen = source.Length;
        if (srcLen == target.Length)
        {
            Octree oct = new(Bounds3.Lab, capacity);
            oct.Subdivide(1, capacity);

            Dictionary<Vec3, Clr> lookup = new();

            int palLen = palette.Length;
            for (int h = 0; h < palLen; ++h)
            {
                Clr c = palette[h];
                if (Clr.Any(c))
                {
                    Vec4 lab = Clr.StandardToCieLab(c);
                    Vec3 point = lab.xyz;
                    oct.Insert(point);
                    lookup.Add(point, c);
                }
            }
            oct.Cull();

            Clr clearBlack = Clr.ClearBlack;
            Dictionary<Clr, Clr> dict = new();
            SortedList<float, Vec3> found = new(32);
            for (int i = 0; i < srcLen; ++i)
            {
                Clr srgb = source[i];
                if (Clr.Any(srgb))
                {
                    Clr opaque = Clr.Opaque(srgb);
                    if (dict.ContainsKey(opaque))
                    {
                        Clr match = dict.GetValueOrDefault(opaque, clearBlack);
                        target[i] = Clr.CopyAlpha(match, srgb);
                    }
                    else
                    {
                        found.Clear();
                        Vec4 lab = Clr.StandardToCieLab(opaque);
                        Octree.Query(oct, lab.xyz, radius, found);
                        if (found.Count > 0)
                        {
                            Vec3 nearest = found.Values[0];
                            if (lookup.ContainsKey(nearest))
                            {
                                Clr match = lookup.GetValueOrDefault(nearest, clearBlack);
                                target[i] = Clr.CopyAlpha(match, srgb);
                                dict.Add(opaque, match);
                            }
                            else { target[i] = Clr.ClearBlack; }
                        }
                        else { target[i] = Clr.ClearBlack; }
                    }
                }
                else { target[i] = Clr.ClearBlack; }
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
    /// Resizes an image to the target width and height.
    /// Uses a bilinear filter.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="wSrc">image width</param>
    /// <param name="hSrc">image height</param>
    /// <param name="wTrg">rescaled width</param>
    /// <param name="hTrg">rescaled height</param>
    /// <returns>resized array</returns>
    public static Clr[] ResizeBilinear(
        in Clr[] source,
        in int wSrc, in int hSrc,
        in int wTrg, in int hTrg)
    {
        int srcLen = source.Length;
        if (wSrc == wTrg && hSrc == hTrg)
        {
            Clr[] sameSize = new Clr[srcLen];
            System.Array.Copy(source, 0, sameSize, 0, srcLen);
            return sameSize;
        }

        float tx = (wSrc - 1.0f) / (wTrg - 1.0f);
        float ty = (hSrc - 1.0f) / (hTrg - 1.0f);

        int trgLen = wTrg * hTrg;
        Clr[] target = new Clr[trgLen];
        for (int i = 0; i < trgLen; ++i)
        {
            target[i] = Pixels.FilterBilinear(source,
                tx * (i % wTrg), ty * (i / wTrg),
                wSrc, hSrc);
        }
        return target;
    }

    /// <summary>
    /// Rotates the pixels of a source image around the image center by an angle
    /// in radians. Where the angle is approximately 0, 90, 180 and 270 degrees,
    /// resorts to faster methods.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="w">image width</param>
    /// <param name="h">image height</param>
    /// <param name="radians">angle in radians</param>
    /// <returns>rotated pixels</returns>
    public static (Clr[] pixels, int w, int h) RotateBilinear(
        in Clr[] source, in int w, in int h, in float radians)
    {
        int srcLen = source.Length;
        int deg = Utils.RemFloor(Utils.Round(radians * Utils.RadToDeg), 360);

        switch (deg)
        {
            case 0:
                {
                    Clr[] t0 = new Clr[srcLen];
                    System.Array.Copy(source, 0, t0, 0, srcLen);
                    return (pixels: t0, w, h);
                }
            case 90:
                {
                    Clr[] t90 = Pixels.Rotate90(source, new Clr[srcLen], w, h);
                    return (pixels: t90, w: h, h: w);
                }
            case 180:
                {
                    Clr[] t180 = Pixels.Rotate180(source, new Clr[srcLen]);
                    return (pixels: t180, w, h);
                }
            case 270:
                {
                    Clr[] t270 = Pixels.Rotate270(source, new Clr[srcLen], w, h);
                    return (pixels: t270, w: h, h: w);
                }
            default:
                {
                    return Pixels.RotateBilinear(source, w, h,
                        MathF.Cos(radians), MathF.Sin(radians));
                }
        }
    }

    /// <summary>
    /// Rotates the pixels of a source image around the image center by an angle
    /// in radians. Assumes that the sine and cosine of the angle have already
    /// been calculated and simple cases (0, 90, 180, 270 degrees) have been
    /// filtered out.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="w">image width</param>
    /// <param name="h">image height</param>
    /// <param name="cosa">cosine of the angle</param>
    /// <param name="sina">sine of the angle</param>
    /// <returns>rotated pixels</returns>
    public static (Clr[] pixels, int w, int h) RotateBilinear(
        in Clr[] source, in int w, in int h,
        in float cosa, in float sina)
    {
        // Do not generalize to pass in filter as a function,
        // as nearest neighbor may need to calculate the center
        // and round from decimals to integers differently.

        float wSrcf = w;
        float hSrcf = h;
        float absCosa = MathF.Abs(cosa);
        float absSina = MathF.Abs(sina);

        int wTrg = (int)(0.5f + hSrcf * absSina + wSrcf * absCosa);
        int hTrg = (int)(0.5f + hSrcf * absCosa + wSrcf * absSina);
        float wTrgf = wTrg;
        float hTrgf = hTrg;

        float xSrcCenter = wSrcf * 0.5f;
        float ySrcCenter = hSrcf * 0.5f;
        float xTrgCenter = wTrgf * 0.5f;
        float yTrgCenter = hTrgf * 0.5f;

        int trgLen = wTrg * hTrg;
        Clr[] target = new Clr[trgLen];
        for (int i = 0; i < trgLen; ++i)
        {
            float ySgn = i / wTrg - yTrgCenter;
            float xSgn = i % wTrg - xTrgCenter;
            target[i] = Pixels.FilterBilinear(
                source,
                xSrcCenter + cosa * xSgn - sina * ySgn,
                ySrcCenter + cosa * ySgn + sina * xSgn,
                w, h);
        }

        return (pixels: target, w: wTrg, h: hTrg);
    }

    /// <summary>
    /// Scales the source pixels. The scalar is expected
    /// to be a non-zero positive value. Returns a tuple
    /// containing the pixels, the new width and height.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="w">image width/param>
    /// <param name="h">image height</param>
    /// <returns>tuple</returns>
    public static (Clr[] pixels, int w, int h) ScaleBilinear(
    in Clr[] source, in int w, in int h, in float scalar)
    {
        float sVrf = MathF.Abs(scalar);
        int wTrg = Utils.Max(1, (int)(0.5f + w * sVrf));
        int hTrg = Utils.Max(1, (int)(0.5f + h * sVrf));
        return (pixels: Pixels.ResizeBilinear(source, w, h,
            wTrg, hTrg), w: wTrg, h: hTrg);
    }

    /// <summary>
    /// Scales the source pixels. The scalar is expected
    /// to be a non-zero positive value. Returns a tuple
    /// containing the pixels, the new width and height.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="w">image width/param>
    /// <param name="h">image height</param>
    /// <returns>tuple</returns>
    public static (Clr[] pixels, int w, int h) ScaleBilinear(
    in Clr[] source, in int w, in int h, in Vec2 scalar)
    {
        Vec2 sVrf = Vec2.Abs(scalar);
        int wTrg = Utils.Max(1, (int)(0.5f + w * sVrf.x));
        int hTrg = Utils.Max(1, (int)(0.5f + h * sVrf.y));
        return (pixels: Pixels.ResizeBilinear(source, w, h,
            wTrg, hTrg), w: wTrg, h: hTrg);
    }

    /// <summary>
    /// Skews the pixels of a source image horizontally. If the angle is
    /// approximately 0 degrees, copies the source array. If the angle is
    /// approximately 90 degrees, returns an empty array.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="w">image width/param>
    /// <param name="h">image height</param>
    /// <param name="radians">radians</param>
    /// <returns>tuple</returns>
    public static (Clr[] pixels, int w, int h) SkewXBilinear(
        in Clr[] source, in int w, in int h, in float radians)
    {
        int srcLen = source.Length;
        int deg = Utils.RemFloor(Utils.Round(radians * Utils.RadToDeg), 180);

        switch (deg)
        {
            case 0:
                {
                    Clr[] t0 = new Clr[srcLen];
                    System.Array.Copy(source, 0, t0, 0, srcLen);
                    return (pixels: t0, w, h);
                }
            case 89:
            case 90:
            case 91:
                {
                    Clr[] t90 = Pixels.Fill(new Clr[srcLen], Clr.ClearBlack);
                    return (pixels: t90, w, h);
                }
            default:
                {
                    float wSrcf = w;
                    float hSrcf = h;

                    float tana = MathF.Tan(radians);
                    int wTrg = (int)(0.5f + wSrcf + MathF.Abs(tana) * hSrcf);
                    float wTrgf = wTrg;
                    float yCenter = hSrcf * 0.5f;
                    float xDiff = (wSrcf - wTrgf) * 0.5f;

                    int trgLen = wTrg * h;
                    Clr[] target = new Clr[trgLen];
                    for (int i = 0; i < trgLen; ++i)
                    {
                        float yTrg = i / wTrg;
                        target[i] = Pixels.FilterBilinear(
                            source, xDiff + i % wTrg + tana
                           * (yTrg - yCenter), yTrg, w, h);
                    }

                    return (pixels: target, w: wTrg, h);
                }
        }
    }

    /// <summary>
    /// Skews the pixels of a source image vertically. If the angle is
    /// approximately 0 degrees, copies the source array. If the angle is
    /// approximately 90 degrees, returns an empty array.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="w">image width/param>
    /// <param name="h">image height</param>
    /// <param name="radians">radians</param>
    /// <returns>tuple</returns>
    public static (Clr[] pixels, int w, int h) SkewYBilinear(
    in Clr[] source, in int w, in int h, in float radians)
    {
        int srcLen = source.Length;
        int deg = Utils.RemFloor(Utils.Round(radians * Utils.RadToDeg), 180);

        switch (deg)
        {
            case 0:
                {
                    Clr[] t0 = new Clr[srcLen];
                    System.Array.Copy(source, 0, t0, 0, srcLen);
                    return (pixels: t0, w, h);
                }
            case 89:
            case 90:
            case 91:
                {
                    Clr[] t90 = Pixels.Fill(new Clr[srcLen], Clr.ClearBlack);
                    return (pixels: t90, w, h);
                }
            default:
                {
                    float wSrcf = w;
                    float hSrcf = h;

                    float tana = MathF.Tan(radians);
                    int hTrg = (int)(0.5f + hSrcf + MathF.Abs(tana) * wSrcf);
                    float hTrgf = hTrg;
                    float xCenter = wSrcf * 0.5f;
                    float yDiff = (hSrcf - hTrgf) * 0.5f;

                    int trgLen = w * hTrg;
                    Clr[] target = new Clr[trgLen];
                    for (int i = 0; i < trgLen; ++i)
                    {
                        float xTrg = i % w;
                        target[i] = Pixels.FilterBilinear(
                            source, xTrg, yDiff + i / w + tana
                           * (xTrg - xCenter), w, h);
                    }
                    return (pixels: target, w, h);
                }
        }
    }

    /// <summary>
    /// Finds the minimum, maximum and mean lightness in a source pixels array.
    /// If factor is positive, stretches color to maximum lightness range in
    /// [0.0, 100.0]. If factor is negative, compresses color to mean. Assigns
    /// result to target array. The factor is expected to be in [-1.0, 1.0].
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="target">target pixels</param>
    /// <param name="fac">factor</param>
    /// <returns>adjusted pixels</returns>
    public static Clr[] StretchContrast(
        in Clr[] source, in Clr[] target, in float fac)
    {
        int srcLen = source.Length;
        if (srcLen == target.Length)
        {
            float valFac = Utils.Clamp(fac, -1.0f, 1.0f);
            if (Utils.Approx(valFac, 0.0f))
            {
                System.Array.Copy(source, 0, target, 0, srcLen);
                return target;
            }

            float lumMin = Single.MaxValue;
            float lumMax = Single.MinValue;
            float lumSum = 0.0f;

            Dictionary<Clr, Vec4> dict = new();
            for (int i = 0; i < srcLen; ++i)
            {
                Clr c = source[i];
                if (Clr.Any(c))
                {
                    Clr key = Clr.Opaque(c);
                    if (!dict.ContainsKey(key))
                    {
                        Vec4 lab = Clr.StandardToCieLab(key);
                        dict.Add(key, lab);

                        float lum = lab.z;
                        if (lum < lumMin) { lumMin = lum; }
                        if (lum > lumMax) { lumMax = lum; }
                        lumSum += lum;
                    }
                }
            }

            int dictLen = dict.Count;
            if (dictLen > 0)
            {
                float diff = MathF.Abs(lumMax - lumMin);
                if (diff > Utils.Epsilon)
                {
                    float t = MathF.Abs(valFac);
                    float u = 1.0f - t;
                    bool gtZero = valFac > 0.0f;
                    bool ltZero = valFac < -0.0f;

                    float lumAvg = lumSum / dictLen;
                    float tDenom = t * (100.0f / diff);
                    float lumMintDenom = lumMin * tDenom;

                    Dictionary<Clr, Clr> stretched = new();
                    foreach (KeyValuePair<Clr, Vec4> kv in dict)
                    {
                        Vec4 sourceLab = kv.Value;
                        Vec4 stretchedLab = sourceLab;
                        if (gtZero)
                        {
                            stretchedLab = new Vec4(
                                sourceLab.x, sourceLab.y,
                                u * sourceLab.z + sourceLab.z * tDenom - lumMintDenom,
                                sourceLab.w);
                        }
                        else if (ltZero)
                        {
                            stretchedLab = new Vec4(
                                sourceLab.x, sourceLab.y,
                                u * stretchedLab.z + t * lumAvg,
                                sourceLab.w);
                        }

                        stretched.Add(kv.Key, Clr.CieLabToStandard(stretchedLab));
                    }

                    Clr clearBlack = Clr.ClearBlack;
                    for (int i = 0; i < srcLen; ++i)
                    {
                        Clr c = source[i];
                        target[i] = Clr.CopyAlpha(
                            stretched.GetValueOrDefault(Clr.Opaque(c),
                                clearBlack), c);
                    }
                }
                else
                {
                    System.Array.Copy(source, 0, target, 0, srcLen);
                    return target;
                }
            }
            else
            {
                Pixels.Fill(target, Clr.ClearBlack);
            }
        }
        return target;
    }

    /// <summary>
    /// Mixes an image with a color in CIE LAB according to a factor.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="target">target pixels</param>
    /// <param name="tint">tint color</param>
    /// <param name="fac">factor</param>
    /// <returns>tinted pixels</returns>
    public static Clr[] TintCieLab(
        in Clr[] source, in Clr[] target,
        in Clr tint, in float fac = 0.5f)
    {
        int srcLen = source.Length;
        if (srcLen == target.Length)
        {
            if (fac <= 0.0f)
            {
                System.Array.Copy(source, 0, target, 0, srcLen);
                return target;
            }

            if (fac >= 1.0f)
            {
                for (int i = 0; i < srcLen; ++i)
                {
                    target[i] = Clr.CopyAlpha(tint, source[i]);
                }
                return target;
            }

            Vec4 tintLab = Clr.StandardToCieLab(tint);
            Dictionary<Clr, Clr> dict = new();
            for (int i = 0; i < srcLen; ++i)
            {
                Clr c = source[i];
                if (Clr.Any(c) && !dict.ContainsKey(c))
                {
                    Vec4 mixedLab = Vec4.Mix(Clr.StandardToCieLab(c), tintLab, fac);
                    dict.Add(c, Clr.CieLabToStandard(mixedLab));
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
                Pixels.Fill(target, Clr.ClearBlack);
            }
        }
        return target;
    }

    /// <summary>
    /// Removes excess transparent pixels from an array of pixels. Adapted from
    /// the implementation by Oleg Mikhailov: https://stackoverflow.com/a/36938923 .
    /// Returns a tuple with the trimmed image's pixels,
    /// width, height and top-left corner.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="w">image width</param>
    /// <param name="h">image height</param>
    /// <param name="tlx">top left corner x</param>
    /// <param name="tly">top left corner y</param>
    /// <returns>tuple</returns>
    public static (Clr[] pixels, int w, int h, int x, int y) TrimAlpha(
        in Clr[] source, in int w, in int h, in int tlx = 0, in int tly = 0)
    {
        int srcLen = source.Length;

        if (w < 2 && h < 2)
        {
            Clr[] trg0 = new Clr[srcLen];
            System.Array.Copy(source, 0, trg0, 0, srcLen);
            return (pixels: trg0, w, h, x: tlx, y: tly);
        }

        int wn1 = w > 1 ? w - 1 : 0;
        int hn1 = h > 1 ? h - 1 : 0;

        int minRight = wn1;
        int minBottom = hn1;

        // Top search. y is outer loop, x is inner loop.
        int top = -1;
        bool goTop = true;
        while (goTop && top < hn1)
        {
            ++top;
            int wtop = w * top;
            int x = -1;
            while (goTop && x < wn1)
            {
                ++x;
                if (Clr.Any(source[wtop + x]))
                {
                    minRight = x;
                    minBottom = top;
                    goTop = false;
                }
            }
        }

        // Left search. x is outer loop, y is inner loop.
        int left = -1;
        bool goLeft = true;
        while (goLeft && left < minRight)
        {
            ++left;
            int y = h;
            while (goLeft && y > top)
            {
                --y;
                if (Clr.Any(source[y * w + left]))
                {
                    minBottom = y;
                    goLeft = false;
                }
            }
        }

        // Bottom search. y is outer loop, x is inner loop.
        int bottom = h;
        bool goBottom = true;
        while (goBottom && bottom > minBottom)
        {
            --bottom;
            int wbottom = w * bottom;
            int x = w;
            while (goBottom && x > left)
            {
                --x;
                if (Clr.Any(source[wbottom + x]))
                {
                    minRight = x;
                    goBottom = false;
                }
            }
        }

        // Right search. x is outer loop, y is inner loop.
        int right = w;
        bool goRight = true;
        while (goRight && right > minRight)
        {
            --right;
            int y = bottom + 1;
            while (goRight && y > top)
            {
                --y;
                if (Clr.Any(source[y * w + right]))
                {
                    goRight = false;
                }
            }
        }

        int wTrg = 1 + right - left;
        int hTrg = 1 + bottom - top;
        if (wTrg < 1 || hTrg < 1)
        {
            Clr[] trg1 = new Clr[srcLen];
            System.Array.Copy(source, 0, trg1, 0, srcLen);
            return (pixels: trg1, w, h, x: tlx, y: tly);
        }

        int trgLen = wTrg * hTrg;
        Clr[] target = new Clr[trgLen];
        for (int i = 0; i < trgLen; ++i)
        {
            target[i] = source[w * (top + i / wTrg) + left + i % wTrg];
        }
        return (pixels: target, w: wTrg, h: hTrg, x: tlx + left, y: tly + top);
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