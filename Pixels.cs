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
    /// <param name="a">first color/param>
    /// <param name="b">second color/param>
    /// <param name="cols">columns/param>
    /// <param name="rows">rows/param>
    /// <param name="w">image width</param>
    /// <param name="h">image height</param>
    /// <param name="target">target pixels</param>
    /// <returns>checker pattern</returns>
    public static T[] Checker<T>(
        in T a, in T b,
        in int cols, in int rows, in int w, in int h,
        in T[] target)
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
    /// Fills the pixels target array with a color.
    /// </summary>
    /// <param name="c">fill color</param>
    /// <param name="target">target pixels</param>
    /// <returns>filled array</returns>
    public static T[] Fill<T>(in T c, in T[] target)
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
    /// <param name="w">image width</param>
    /// <param name="h">image height</param>
    /// <param name="target">target pixels</param>
    /// <returns>flipped pixels</returns>
    public static T[] FlipX<T>(in T[] source, in int w, in int h, in T[] target)
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
    /// <param name="w">image width</param>
    /// <param name="h">image height</param>
    /// <param name="target">target pixels</param>
    /// <returns>flipped pixels</returns>
    public static T[] FlipY<T>(in T[] source, in int w, in int h, in T[] target)
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
    /// <param name="wSrc">source image width</param>
    /// <param name="hSrc">source image height</param>
    /// <param name="pivot">y pivot</param>
    /// <param name="flip">flip reflection flag</param>
    /// <param name="target">target pixels</param>
    /// <returns>mirrored pixels</returns>
    public static T[] MirrorX<T>(
        in T[] source, in int wSrc, in int hSrc,
        in int pivot, in bool flip, in T[] target)
    {
        int trgLen = target.Length;
        int flipSign = flip ? 1 : -1;

        for (int i = 0; i < trgLen; ++i)
        {
            int cross = i % wSrc - pivot;
            if (flipSign * cross < 0)
            {
                target[i] = source[i];
            }
            else
            {
                int pxOpp = pivot - cross;
                if (pxOpp > -1 && pxOpp < wSrc)
                {
                    target[i] = source[i / wSrc * wSrc + pxOpp];
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
    /// <param name="wSrc">source image width</param>
    /// <param name="hSrc">source image height</param>
    /// <param name="pivot">y pivot</param>
    /// <param name="flip">flip reflection flag</param>
    /// <param name="target">target pixels</param>
    /// <returns>mirrored pixels</returns>
    public static T[] MirrorY<T>(
        in T[] source, in int wSrc, in int hSrc,
        in int pivot, in bool flip, in T[] target)
    {
        int trgLen = target.Length;
        int flipSign = flip ? 1 : -1;

        for (int i = 0; i < trgLen; ++i)
        {
            int cross = i / wSrc - pivot;
            if (flipSign * cross < 0)
            {
                target[i] = source[i];
            }
            else
            {
                int pyOpp = pivot - cross;
                if (pyOpp > -1 && pyOpp < hSrc)
                {
                    target[i] = source[pyOpp * wSrc + i % wSrc];
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
    /// <param name="w">image width</param>
    /// <param name="h">image height</param>
    /// <param name="target">target pixels</param>
    /// <returns>rotated pixels</returns>
    public static T[] Rotate270<T>(in T[] source, in int w, in int h, in T[] target)
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
    /// Rotates the source pixel array 90 degrees counter-clockwise. The
    /// rotation is stored in the target pixel array.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="w">image width</param>
    /// <param name="h">image height</param>
    /// <param name="target">target pixels</param>
    /// <returns>rotated pixels</returns>
    public static T[] Rotate90<T>(in T[] source, in int w, in int h, in T[] target)
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
    /// Blits a source image's pixels onto a target image's pixels, using
    /// integer floor modulo to wrap the source image. The source image can be
    /// offset horizontally and/or vertically, creating the illusion of
    /// parallax.
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="wSrc">source image width</param>
    /// <param name="hSrc">source image height</param>
    /// <param name="dx">horizontal pixel offset</param>
    /// <param name="dy">vertical pixel offset</param>
    /// <param name="wTrg">target image width</param>
    /// <param name="target">target pixels</param>
    /// <returns>wrapped pixels</returns>
    public static T[] Wrap<T>(
        in T[] source, in int wSrc, in int hSrc,
        in int dx, in int dy, in int wTrg, in T[] target)
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
    /// Adjusts the contrast of colors from a source pixels array by a factor.
    /// Uses the CIE LAB color space. The adjustment factor is expected to be in
    /// [-1.0, 1.0].
    /// </summary>
    /// <param name="source">source pixels</param>
    /// <param name="fac">factor</param>
    /// <param name="target">target pixels</param>
    /// <returns>wrapped pixels</returns>
    public static Clr[] AdjustContrast(in Clr[] source, in float fac, in Clr[] target)
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

    #endregion
}