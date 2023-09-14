using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Contains a list of keys which hold colors at steps in the range [0.0, 1.0].
/// Allows for smooth color transitions to be evaluated by a factor.
/// </summary>
[Serializable]
public class ClrGradient : IEnumerable<ClrKey>
{
    /// <summary>
    /// The list of color keys in the gradient.
    /// </summary>
    protected readonly List<ClrKey> keys = new(16);

    /// <summary>
    /// Returns the number of color keys in this gradient.
    /// </summary>
    /// <value>length</value>
    public int Length { get { return this.keys.Count; } }

    /// <summary>
    /// Constructs a color gradient.
    /// </summary>
    protected ClrGradient() { }

    /// <summary>
    /// Constructs a color gradient with evenly distributed
    /// keys of the provided count. The colors of each key
    /// default to a grayscale ramp.
    /// </summary>
    /// <param name="count">count</param>
    public ClrGradient(in int count)
    {
        int vc = count < 2 ? 2 : count;
        float toStep = 1.0f / (vc - 1.0f);
        for (int i = 0; i < vc; ++i)
        {
            float step = i * toStep;
            this.keys.Add(new(step, new(100.0f * step, 0.0f, 0.0f, 1.0f)));
        }
    }

    /// <summary>
    /// Constructs a color gradient from a color. Places 
    /// black at step 0.0, white at step 1.0, and the color
    /// somewhere between based on its luminance.
    /// </summary>
    /// <param name="color">color</param>
    public ClrGradient(in Lab color)
    {
        float a = color.Alpha;
        float lum = color.L * 0.01f;
        this.keys.Add(new(0.0f, new(0.0f, 0.0f, 0.0f, a)));
        if (lum > Utils.Epsilon * 2.0f && lum < 1.0f - Utils.Epsilon * 2.0f)
        {
            float middle = Utils.Mix(Utils.OneThird, Utils.TwoThirds, lum);
            this.keys.Add(new(middle, color));
        }
        this.keys.Add(new(1.0f, new(100.0f, 0.0f, 0.0f, a)));
    }

    /// <summary>
    /// Constructs a color gradient from a list of colors. The color keys will
    /// be evenly distributed across the gradient.
    /// </summary>
    /// <param name="colors">colors</param>
    public ClrGradient(params Lab[] colors)
    {
        this.AppendAll(colors);
    }

    /// <summary>
    /// Constructs a color gradient from a color key.
    /// </summary>
    /// <param name="key">color key</param>
    public ClrGradient(in ClrKey key)
    {
        float step = key.Step;
        if (step > Utils.Epsilon)
        {
            this.Insert(new(0.0f, Lab.ClearBlack));
        }
        this.Insert(key);
        if (step < 1.0 - Utils.Epsilon)
        {
            this.Insert(new(1.0f, Lab.White));
        }
    }

    /// <summary>
    /// Constructs a color gradient from a list of color keys.
    /// </summary>
    /// <param name="keys">color keys</param>
    public ClrGradient(params ClrKey[] keys)
    {
        this.InsertAll(keys);
    }

    /// <summary>
    /// Constructs a color gradient from a palette.
    /// </summary>
    /// <param name="pal">palette</param>
    public ClrGradient(in Palette pal)
    {
        int len = pal.Length;
        float denom = Utils.Div(1.0f, len - 1.0f);
        for (int i = 0; i < len; ++i)
        {
            this.keys.Add(new(i * denom, pal.GetColor(i)));
        }
    }

    /// <summary>
    /// Retrieves a color key by index. Wraps the index by the number of keys.
    /// </summary>
    /// <value>color key</value>
    public ClrKey this[int i]
    {
        get
        {
            return this.keys[Utils.RemFloor(i, this.keys.Count)];
        }
    }

    /// <summary>
    /// Retrieves a color given a step in the range [0.0, 1.0] .
    /// </summary>
    /// <value>color</value>
    public Lab this[float step]
    {
        get
        {
            return ClrGradient.Eval(this, step);
        }
    }

    /// <summary>
    /// Gets the hash code for the keys of this gradient.
    /// </summary>
    /// <returns>hash code</returns>
    public override int GetHashCode()
    {
        return this.keys.GetHashCode();
    }

    /// <summary>
    /// Returns a string representation of this gradient.
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()
    {
        return ClrGradient.ToString(this);
    }

    /// <summary>
    /// Appends a color at the end of the gradient.
    /// </summary>
    /// <param name="color">color</param>
    /// <returns>this gradient</returns>
    public ClrGradient Append(Lab color)
    {
        this.CompressKeysLeft(1);
        this.keys.Add(new(1.0f, color));
        return this;
    }

    /// <summary>
    /// Appends a list of colors to the end of the gradient.
    ///
    /// The color keys will be evenly distributed across the gradient.
    /// </summary>
    /// <param name="colors">colors</param>
    /// <returns>this gradient</returns>
    public ClrGradient AppendAll(params Lab[] colors)
    {
        int len = colors.Length;
        this.CompressKeysLeft(len);
        int oldLen = this.keys.Count;
        float range = oldLen + len;
        float denom = Utils.Div(1.0f, range - 1.0f);

        for (int i = 0; i < len; ++i)
        {
            this.keys.Add(new(
                (oldLen + i) * denom,
                colors[i]));
        }
        return this;
    }

    /// <summary>
    /// Helper function that shifts existing keys to the left when a new color
    /// is appended to the gradient without a key.
    /// </summary>
    /// <param name="added">number to add</param>
    /// <returns>this gradient</returns>
    protected ClrGradient CompressKeysLeft(in int added = 1)
    {
        int len = this.keys.Count;
        float scalar = 1.0f / (len + added - 1.0f);
        for (int i = 0; i < len; ++i)
        {
            ClrKey key = this.keys[i];
            this.keys[i] = new(
                key.Step * i * scalar,
                key.Color);
        }
        return this;
    }

    /// <summary>
    /// Helper function that shifts existing keys to the right when a new color
    /// is prepended to the gradient without a key.
    /// </summary>
    /// <param name="added">number to add</param>
    /// <returns>this gradient</returns>
    protected ClrGradient CompressKeysRight(in int added = 1)
    {
        int len = this.keys.Count;
        float scalar = added / (len + added - 1.0f);
        float coeff = 1.0f - scalar;
        for (int i = 0; i < len; ++i)
        {
            ClrKey key = this.keys[i];
            this.keys[i] = new(
                scalar + coeff * key.Step,
                key.Color);
        }
        return this;
    }

    /// <summary>
    /// Evaluates whether the gradient contains a key at a given tolerance.
    /// </summary>
    /// <param name="key">color key</param>
    /// <returns>evaluation</returns>
    public int ContainsKey(ClrKey key)
    {
        return this.keys.FindIndex((ClrKey x) =>
            Utils.Approx(x.Step, key.Step, 0.0005f));
    }

    /// <summary>
    /// Gets the enumerator for the keys of this gradient.
    /// </summary>
    /// <returns>enumerator</returns>
    public IEnumerator<ClrKey> GetEnumerator()
    {
        return this.keys.GetEnumerator();
    }

    /// <summary>
    /// Gets the enumerator for the keys of this gradient.
    /// </summary>
    /// <returns>enumerator</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    /// <summary>
    /// Gets the first color key from the gradient.
    /// </summary>
    /// <returns>first key</returns>
    public ClrKey GetFirst()
    {
        return this.keys[0];
    }

    /// <summary>
    /// Gets the last color key from the gradient.
    /// </summary>
    /// <returns>last key</returns>
    public ClrKey GetLast()
    {
        return this.keys[^1];
    }

    /// <summary>
    /// Inserts a color key into this gradient.
    ///
    /// Replaces any key which already exists at the insertion step.
    /// </summary>
    /// <param name="key">color key</param>
    /// <returns>this gradient</returns>
    public ClrGradient Insert(in ClrKey key)
    {
        int query = this.ContainsKey(key);
        if (query != -1) { this.keys.RemoveAt(query); }
        this.InsortRight(key);
        return this;
    }

    /// <summary>
    /// Inserts all color keys into this gradient.
    /// </summary>
    /// <param name="keys">color keys</param>
    /// <returns>this gradient</returns>
    public ClrGradient InsertAll(params ClrKey[] keys)
    {
        foreach (ClrKey key in keys) { this.Insert(key); }
        return this;
    }

    /// <summary>
    /// Inserts all color keys into this gradient.
    /// </summary>
    /// <param name="keys">color keys</param>
    /// <returns>this gradient</returns>
    public ClrGradient InsertAll(in IEnumerable<ClrKey> keys)
    {
        foreach (ClrKey key in keys) { this.Insert(key); }
        return this;
    }

    /// <summary>
    /// Inserts a color key into the keys array based on the index returned from bisectRight.
    /// </summary>
    /// <param name="key">color key</param>
    /// <returns>this gradient</returns>
    ClrGradient InsortRight(in ClrKey key)
    {
        int i = ClrGradient.BisectRight(this, key.Step);
        this.keys.Insert(i, key);
        return this;
    }

    /// <summary>
    /// Appends a color to this gradient. Shifts existing keys to the right by 1.
    /// </summary>
    /// <param name="color">color</param>
    /// <returns>this gradient</returns>
    public ClrGradient Prepend(in Lab color)
    {
        this.CompressKeysRight(1);
        this.keys.Insert(0, new(0.0f, color));
        return this;
    }

    /// <summary>
    /// Prepends a list of colors to this gradient. Shifts existing keys to the
    /// right.
    /// </summary>
    /// <param name="colors">colors</param>
    /// <returns>this gradient</returns>
    public ClrGradient PrependAll(params Lab[] colors)
    {
        int len = colors.Length;
        this.CompressKeysRight(len);
        int oldLen = this.keys.Count;
        float range = oldLen + len;
        float denom = Utils.Div(1.0f, range - 1.0f);

        for (int i = 0; i < len; ++i)
        {
            this.keys.Insert(i, new(
                i * denom,
                colors[i]));
        }
        return this;
    }

    /// <summary>
    /// Removes a key at a given index.
    /// </summary>
    /// <param name="i">index</param>
    /// <returns>color key</returns>
    protected ClrKey RemoveAt(in int i = -1)
    {
        int j = Utils.RemFloor(i, this.keys.Count);
        ClrKey key = this.keys[j];
        this.keys.RemoveAt(j);
        return key;
    }

    /// <summary>
    /// Removes the first key from the gradient.
    /// </summary>
    /// <returns>first key</returns>
    protected ClrKey RemoveFirst()
    {
        return this.RemoveAt(0);
    }

    /// <summary>
    /// Removes the last key from the gradient.
    /// </summary>
    /// <returns>last key</returns>
    protected ClrKey RemoveLast()
    {
        return this.RemoveAt(this.keys.Count - 1);
    }

    /// <summary>
    /// Resets this gradient to an initial state.
    /// </summary>
    /// <returns>this gradient</returns>
    protected ClrGradient Reset()
    {
        this.keys.Clear();
        return this;
    }

    /// <summary>
    /// Reverses the gradient. The step of each color key is subtracted from one.
    /// </summary>
    /// <returns>this gradient</returns>
    public ClrGradient Reverse()
    {
        this.keys.Reverse();
        int len = this.keys.Count;
        for (int i = 0; i < len; ++i)
        {
            ClrKey key = this.keys[i];
            this.keys[i] = new(1.0f - key.Step, key.Color);
        }
        return this;
    }

    /// <summary>
    /// Locates the insertion point for a step in the gradient that will maintain sorted order.
    /// </summary>
    /// <param name="cg">color gradient</param>
    /// <param name="step">step</param>
    /// <returns>index</returns>
    internal static int BisectRight(in ClrGradient cg, in float step)
    {
        int low = 0;
        List<ClrKey> keys = cg.keys;
        int high = keys.Count;
        while (low < high)
        {
            int middle = (low + high) / 2;
            if (step < keys[middle].Step)
            {
                high = middle;
            }
            else
            {
                low = middle + 1;
            }
        }
        return low;
    }

    /// <summary>
    /// Finds a color given a step in the range [0.0, 1.0].
    /// Chooses a color based on Interleaved Gradient Noise
    /// as developed by Jorge Jiminez. See
    /// https://blog.demofox.org/2022/01/01/
    /// interleaved-gradient-noise-a-different-
    /// kind-of-low-discrepancy-sequence/ .
    /// </summary>
    /// <param name="cg">color gradient</param>
    /// <param name="step">step</param>
    /// <param name="x">x pixel</param>
    /// <param name="y">y pixel</param>
    /// <param name="frame">frame</param>
    /// <returns>color</returns>
    public static Lab DitherNoise(
        in ClrGradient cg,
        in float step,
        in int x, in int y,
        in int frame = 0)
    {
        int frMod = Utils.RemFloor(frame, 64);
        float xz = x + 5.588238f * frMod;
        float yz = y + 5.588238f * frMod;
        float ign = Utils.Fract(52.9829189f * Utils.Fract(
            0.06711056f * xz + 0.00583715f * yz));

        var (prev, next, tScaled) = ClrGradient.FindKeys(cg, step);
        if (tScaled > ign) { return next.Color; }
        return prev.Color;
    }

    /// <summary>
    /// Finds a color given a step in the range [0.0, 1.0].
    /// Chooses a color between keys according to an ordered
    /// dither matrix. Pixel coordinates wrap around matrix
    /// dimensions.
    /// </summary>
    /// <param name="cg">color gradient</param>
    /// <param name="step">step</param>
    /// <param name="x">x pixel</param>
    /// <param name="y">y pixel</param>
    /// <param name="matrix">dither matrix</param>
    /// <param name="cols">matrix columns, or width</param>
    /// <param name="rows">matrix rows, or height</param>
    /// <returns>color</returns>
    public static Lab DitherOrdered(
        in ClrGradient cg,
        in float step,
        in int x, in int y,
        in float[] matrix,
        in int cols,
        in int rows)
    {
        int matIdx = Utils.RemFloor(x, cols)
                   + Utils.RemFloor(y, rows) * cols;
        float matElm = matrix[matIdx];

        var (prev, next, tScaled) = ClrGradient.FindKeys(cg, step);
        if (tScaled > matElm) { return next.Color; }
        return prev.Color;
    }

    /// <summary>
    /// Finds a color given a step in the range [0.0, 1.0]. When the step falls
    /// between color keys, the resultant color is created by an easing function.
    /// </summary>
    /// <param name="cg">color gradient</param>
    /// <param name="step">step</param>
    /// <returns>color</returns>
    public static Lab Eval(in ClrGradient cg, in float step)
    {
        return ClrGradient.Eval(cg, step,
            (x, y, z) => Lab.Mix(x, y, z));
    }

    /// <summary>
    /// Finds a color given a step in the range [0.0, 1.0]. When the step falls
    /// between color keys, the resultant color is created by an easing function.
    /// </summary>
    /// <param name="cg">color gradient</param>
    /// <param name="step">step</param>
    /// <param name="easing">easing function</param>
    /// <returns>color</returns>
    public static Lab Eval(
        in ClrGradient cg,
        in float step,
        in Func<Lab, Lab, float, Lab> easing)
    {
        var (prev, next, tScaled) = ClrGradient.FindKeys(cg, step);
        return easing(
            prev.Color,
            next.Color,
            tScaled);
    }

    /// <summary>
    /// Evaluates an array of colors for a step distributed evenly across the
    /// range [0.0, 1.0] for the supplied count.
    /// </summary>
    /// <param name="cg">color gradient</param>
    /// <param name="count">count</param>
    /// <param name="origin">origin</param>
    /// <param name="dest">destination</param>
    /// <returns>colors</returns>
    public static Lab[] EvalRange(
        in ClrGradient cg,
        in int count,
        in float origin = 0.0f,
        in float dest = 1.0f)
    {
        return ClrGradient.EvalRange(cg, count, origin, dest,
            (x, y, z) => Lab.Mix(x, y, z));
    }

    /// <summary>
    /// Evaluates an array of colors for a step distributed evenly across the
    /// range [0.0, 1.0] for the supplied count.
    /// </summary>
    /// <param name="cg">color gradient</param>
    /// <param name="count">count</param>
    /// <param name="origin">origin</param>
    /// <param name="dest">destination</param>
    /// <param name="easing">easing function</param>
    /// <returns>colors</returns>
    public static Lab[] EvalRange(
        in ClrGradient cg,
        in int count,
        in float origin,
        in float dest,
        in Func<Lab, Lab, float, Lab> easing)
    {
        int vCount = count < 3 ? 3 : count;
        float vOrigin = Utils.Clamp(origin, 0.0f, 1.0f);
        float vDest = Utils.Clamp(dest, 0.0f, 1.0f);

        Lab[] result = new Lab[vCount];
        float toPercent = 1.0f / (vCount - 1.0f);
        for (int i = 0; i < vCount; ++i)
        {
            float prc = i * toPercent;
            result[i] = ClrGradient.Eval(cg,
                (1.0f - prc) * vOrigin +
                prc * vDest, easing);
        }
        return result;
    }

    /// <summary>
    /// Internal helper function to find the previous key, next key
    /// and scaled step based on an input step in [0.0, 1.0].
    /// </summary>
    /// <param name="cg">color gradient</param>
    /// <param name="step">step</param>
    /// <returns>tuple</returns>
    internal static (ClrKey prev, ClrKey next, float tScaled) FindKeys(
        in ClrGradient cg,
        in float step)
    {
        List<ClrKey> keys = cg.keys;
        int len = keys.Count;
        ClrKey first = keys[0];
        if (len < 2)
        {
            return (prev: first, next: first, tScaled: 0.0f);
        }

        if (step <= first.Step)
        {
            return (prev: first, next: keys[1], tScaled: 0.0f);
        }

        ClrKey last = keys[^1];
        if (step >= last.Step)
        {
            return (prev: keys[^2], next: last, tScaled: 1.0f);
        }

        int nextIdx = ClrGradient.BisectRight(cg, step);
        int prevIdx = nextIdx - 1;

        ClrKey next = keys[nextIdx];
        ClrKey prev = keys[prevIdx];

        float prevStep = prev.Step;
        float nextStep = next.Step;
        if (prevStep != nextStep)
        {
            float tScaled = (step - prevStep) / (nextStep - prevStep);
            return (prev, next, tScaled);
        }
        return (prev, next, tScaled: 0.0f);
    }

    /// <summary>
    /// Finds the span, or range, of the color gradient, typically [0.0, 1.0]
    /// and under. Equal to the step of the last color key minus the step of
    /// the first.
    /// </summary>
    /// <param name="cg">color gradient</param>
    /// <returns>range</returns>
    public static float Range(in ClrGradient cg)
    {
        return cg.keys[^1].Step - cg.keys[0].Step;
    }

    /// <summary>
    /// Returns a string representation of a color gradient.
    /// </summary>
    /// <param name="cg">color gradient</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string</returns>
    public static string ToString(in ClrGradient cg, in int places = 4)
    {
        return ClrGradient.ToString(
            new StringBuilder(1024), cg, places).ToString();
    }

    /// <summary>
    /// Appendsa a representation of a color gradient to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="cg">color gradient</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString(
        in StringBuilder sb,
        in ClrGradient cg,
        in int places = 4)
    {
        List<ClrKey> keys = cg.keys;
        int len = keys.Count;
        int last = len - 1;

        sb.Append("{\"keys\":[");
        for (int i = 0; i < last; ++i)
        {
            ClrKey.ToString(sb, keys[i], places);
            sb.Append(',');
        }
        ClrKey.ToString(sb, keys[last], places);
        sb.Append("]}");
        return sb;
    }
}