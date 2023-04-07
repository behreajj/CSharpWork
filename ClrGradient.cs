using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Contains a list of keys which hold colors at steps in the range [0.0, 1.0] .
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
            float v = step * step; // Approximate pow(step, 2.2).
            this.keys.Add(new ClrKey(step, new Rgb(v, v, v, 1.0f)));
        }
    }

    /// <summary>
    /// Constructs a color gradient from a color. Places 
    /// black at step 0.0, white at step 1.0, and the color
    /// somewhere between based on its luminance.
    /// </summary>
    /// <param name="color">color</param>
    public ClrGradient(in Rgb color)
    {
        float a = color.a;
        this.keys.Add(new ClrKey(0.0f, new Rgb(0.0f, 0.0f, 0.0f, a)));

        float lum = Rgb.StandardLuminance(color);
        if (lum > Utils.Epsilon * 2.0f && lum < 1.0f - Utils.Epsilon * 2.0f)
        {
            float gray = MathF.Pow(lum, 1.0f / 2.2f);
            float middle = Utils.Mix(Utils.OneThird, Utils.TwoThirds, gray);
            this.keys.Add(new ClrKey(middle, color));
        }

        this.keys.Add(new ClrKey(1.0f, new Rgb(1.0f, 1.0f, 1.0f, a)));
    }

    /// <summary>
    /// Constructs a color gradient from a list of colors. The color keys will be
    /// evenly distributed across the gradient.
    /// </summary>
    /// <param name="colors">colors</param>
    public ClrGradient(params Rgb[] colors)
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
            this.Insert(new ClrKey(0.0f, Rgb.ClearBlack));
        }
        this.Insert(key);
        if (step < 1.0 - Utils.Epsilon)
        {
            this.Insert(new ClrKey(1.0f, Rgb.White));
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
    public Rgb this[float step]
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
    public ClrGradient Append(Rgb color)
    {
        this.CompressKeysLeft(1);
        this.keys.Add(new ClrKey(1.0f, color));
        return this;
    }

    /// <summary>
    /// Appends a list of colors at the end of the gradient.
    ///
    /// The color keys will be evenly distributed across the gradient.
    /// </summary>
    /// <param name="colors">colors</param>
    /// <returns>this gradient</returns>
    public ClrGradient AppendAll(params Rgb[] colors)
    {
        int len = colors.Length;
        this.CompressKeysLeft(len);
        int oldLen = this.keys.Count;
        float denom = 1.0f / (oldLen + len - 1.0f);
        for (int i = 0; i < len; ++i)
        {
            this.keys.Add(new ClrKey(
                (oldLen + i) * denom,
                colors[i]));
        }
        return this;
    }

    /// <summary>
    /// Helper function that shifts existing keys to the left when a new color is
    /// appended to the gradient without a key.
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
            this.keys[i] = new ClrKey(
                key.Step * i * scalar,
                key.Color);
        }
        return this;
    }

    /// <summary>
    /// Helper function that shifts existing keys to the right when a new color is
    /// prepended to the gradient without a key.
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
            this.keys[i] = new ClrKey(
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
        return this.keys.FindIndex((ClrKey x) => Utils.Approx(x.Step, key.Step, 0.0005f));
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
    public ClrGradient Prepend(in Rgb color)
    {
        this.CompressKeysRight(1);
        this.keys.Insert(0, new ClrKey(0.0f, color));
        return this;
    }

    /// <summary>
    /// Prepends a list of colors to this gradient. Shifts existing keys to the
    /// right.
    /// </summary>
    /// <param name="colors">colors</param>
    /// <returns>this gradient</returns>
    public ClrGradient PrependAll(params Rgb[] colors)
    {
        int len = colors.Length;
        this.CompressKeysRight(len);
        int oldLen = this.keys.Count;
        float denom = 1.0f / (oldLen + len - 1.0f);
        for (int i = 0; i < len; ++i)
        {
            this.keys.Insert(i, new ClrKey(
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
            this.keys[i] = new ClrKey(1.0f - key.Step, key.Color);
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
    /// Finds a color given a step in the range [0.0, 1.0]. When the step falls
    /// between color keys, the resultant color is created by an easing function.
    /// </summary>
    /// <param name="cg">color gradient</param>
    /// <param name="step">step</param>
    /// <returns>color</returns>
    public static Rgb Eval(in ClrGradient cg, in float step)
    {
        return ClrGradient.Eval(cg, step,
            (x, y, z) => Rgb.MixRgbaStandard(x, y, z));
    }

    /// <summary>
    /// Finds a color given a step in the range [0.0, 1.0]. When the step falls
    /// between color keys, the resultant color is created by an easing function.
    /// </summary>
    /// <param name="cg">color gradient</param>
    /// <param name="step">step</param>
    /// <param name="easing">easing function</param>
    /// <returns>color</returns>
    public static Rgb Eval(
        in ClrGradient cg,
        in float step,
        in Func<Rgb, Rgb, float, Rgb> easing)
    {
        // TODO: If you include mixNormal, then this needs to be
        // reconsidered. See Lua implementation.

        List<ClrKey> keys = cg.keys;
        int len = keys.Count;
        if (step <= keys[0].Step || len < 2) { return keys[0].Color; }
        if (step >= keys[len - 1].Step) { return keys[len - 1].Color; }

        int nextIdx = ClrGradient.BisectRight(cg, step);
        int prevIdx = nextIdx - 1;

        ClrKey nextKey = keys[nextIdx];
        ClrKey prevKey = keys[prevIdx];

        float prevStep = prevKey.Step;
        float nextStep = nextKey.Step;
        if (prevStep != nextStep)
        {
            return easing(prevKey.Color, nextKey.Color,
                (step - prevStep) / (nextStep - prevStep));
        }
        return nextKey.Color;
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
    public static Rgb[] EvalRange(
        in ClrGradient cg,
        in int count,
        in float origin = 0.0f,
        in float dest = 1.0f)
    {
        return ClrGradient.EvalRange(cg, count, origin, dest,
            (x, y, z) => Rgb.MixRgbaStandard(x, y, z));
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
    public static Rgb[] EvalRange(
        in ClrGradient cg,
        in int count,
        in float origin,
        in float dest,
        in Func<Rgb, Rgb, float, Rgb> easing)
    {
        int vCount = count < 3 ? 3 : count;
        float vOrigin = Utils.Clamp(origin, 0.0f, 1.0f);
        float vDest = Utils.Clamp(dest, 0.0f, 1.0f);

        Rgb[] result = new Rgb[vCount];
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
    /// Returns the Magma color palette, consisting of 16 keys.
    /// </summary>
    /// <param name="target">output gradient</param>
    /// <returns>gradient</returns>
    public static ClrGradient PaletteMagma(in ClrGradient target)
    {
        List<ClrKey> keys = target.keys;
        keys.Clear();
        keys.Capacity = 16;

        keys.Add(new ClrKey(0.000f, new Rgb(0.000000f, 0.000000f, 0.019608f)));
        keys.Add(new ClrKey(0.067f, new Rgb(0.040784f, 0.028758f, 0.110327f)));
        keys.Add(new ClrKey(0.167f, new Rgb(0.093856f, 0.036863f, 0.232941f)));
        keys.Add(new ClrKey(0.200f, new Rgb(0.174118f, 0.006275f, 0.357647f)));

        keys.Add(new ClrKey(0.267f, new Rgb(0.267974f, 0.002353f, 0.416732f)));
        keys.Add(new ClrKey(0.333f, new Rgb(0.367320f, 0.045752f, 0.432680f)));
        keys.Add(new ClrKey(0.400f, new Rgb(0.471373f, 0.080784f, 0.430588f)));
        keys.Add(new ClrKey(0.467f, new Rgb(0.584052f, 0.110588f, 0.413856f)));

        keys.Add(new ClrKey(0.533f, new Rgb(0.703268f, 0.142484f, 0.383007f)));
        keys.Add(new ClrKey(0.600f, new Rgb(0.824314f, 0.198431f, 0.334902f)));
        keys.Add(new ClrKey(0.667f, new Rgb(0.912418f, 0.286275f, 0.298039f)));
        keys.Add(new ClrKey(0.733f, new Rgb(0.962353f, 0.412549f, 0.301176f)));

        keys.Add(new ClrKey(0.800f, new Rgb(0.981176f, 0.548235f, 0.354510f)));
        keys.Add(new ClrKey(0.867f, new Rgb(0.984314f, 0.694118f, 0.446275f)));
        keys.Add(new ClrKey(0.933f, new Rgb(0.987190f, 0.843137f, 0.562092f)));
        keys.Add(new ClrKey(1.000f, new Rgb(0.988235f, 1.000000f, 0.698039f)));

        return target;
    }

    /// <summary>
    /// Returns seven primary and secondary colors: red, yellow, green, cyan,
    /// blue, magenta and red. Red is repeated so the gradient is periodic.
    /// </summary>
    /// <param name="target">output gradient</param>
    /// <returns>gradient</returns>
    public static ClrGradient PaletteRgb(in ClrGradient target)
    {
        List<ClrKey> keys = target.keys;
        keys.Clear();
        keys.Capacity = 7;

        keys.Add(new ClrKey(0.000f, Rgb.Red));
        keys.Add(new ClrKey(0.167f, Rgb.Yellow));
        keys.Add(new ClrKey(0.333f, Rgb.Green));
        keys.Add(new ClrKey(0.500f, Rgb.Cyan));
        keys.Add(new ClrKey(0.667f, Rgb.Blue));
        keys.Add(new ClrKey(0.833f, Rgb.Magenta));
        keys.Add(new ClrKey(1.000f, Rgb.Red));

        return target;
    }

    /// <summary>
    /// Returns thirteen colors in the red yellow blue color wheel. Red is
    /// repeated so that the gradient is periodic.
    /// </summary>
    /// <param name="target">output gradient</param>
    /// <returns>gradient</returns>
    public static ClrGradient PaletteRyb(in ClrGradient target)
    {
        List<ClrKey> keys = target.keys;
        keys.Clear();
        keys.Capacity = 13;

        keys.Add(new ClrKey(0.000f, new Rgb(1.000000f, 0.000000f, 0.000000f)));
        keys.Add(new ClrKey(0.083f, new Rgb(1.000000f, 0.250000f, 0.000000f)));
        keys.Add(new ClrKey(0.167f, new Rgb(1.000000f, 0.500000f, 0.000000f)));
        keys.Add(new ClrKey(0.250f, new Rgb(1.000000f, 0.750000f, 0.000000f)));
        keys.Add(new ClrKey(0.333f, new Rgb(1.000000f, 1.000000f, 0.000000f)));
        keys.Add(new ClrKey(0.417f, new Rgb(0.505882f, 0.831373f, 0.101961f)));
        keys.Add(new ClrKey(0.500f, new Rgb(0.000000f, 0.662745f, 0.200000f)));
        keys.Add(new ClrKey(0.583f, new Rgb(0.082353f, 0.517647f, 0.400000f)));
        keys.Add(new ClrKey(0.667f, new Rgb(0.164706f, 0.376471f, 0.600000f)));
        keys.Add(new ClrKey(0.750f, new Rgb(0.333333f, 0.188235f, 0.552941f)));
        keys.Add(new ClrKey(0.833f, new Rgb(0.500000f, 0.000000f, 0.500000f)));
        keys.Add(new ClrKey(0.917f, new Rgb(0.750000f, 0.000000f, 0.250000f)));
        keys.Add(new ClrKey(1.000f, new Rgb(1.000000f, 0.000000f, 0.000000f)));

        return target;
    }

    /// <summary>
    /// Returns the Viridis color palette, consisting of 16 keys.
    /// </summary>
    /// <param name="target">output gradient</param>
    /// <returns>gradient</returns>
    public static ClrGradient PaletteViridis(in ClrGradient target)
    {
        List<ClrKey> keys = target.keys;
        keys.Clear();
        keys.Capacity = 16;

        keys.Add(new ClrKey(0.000f, new Rgb(0.266667f, 0.003922f, 0.329412f)));
        keys.Add(new ClrKey(0.067f, new Rgb(0.282353f, 0.100131f, 0.420654f)));
        keys.Add(new ClrKey(0.167f, new Rgb(0.276078f, 0.184575f, 0.487582f)));
        keys.Add(new ClrKey(0.200f, new Rgb(0.254902f, 0.265882f, 0.527843f)));

        keys.Add(new ClrKey(0.267f, new Rgb(0.221961f, 0.340654f, 0.549281f)));
        keys.Add(new ClrKey(0.333f, new Rgb(0.192157f, 0.405229f, 0.554248f)));
        keys.Add(new ClrKey(0.400f, new Rgb(0.164706f, 0.469804f, 0.556863f)));
        keys.Add(new ClrKey(0.467f, new Rgb(0.139869f, 0.534379f, 0.553464f)));

        keys.Add(new ClrKey(0.533f, new Rgb(0.122092f, 0.595033f, 0.543007f)));
        keys.Add(new ClrKey(0.600f, new Rgb(0.139608f, 0.658039f, 0.516863f)));
        keys.Add(new ClrKey(0.667f, new Rgb(0.210458f, 0.717647f, 0.471895f)));
        keys.Add(new ClrKey(0.733f, new Rgb(0.326797f, 0.773595f, 0.407582f)));

        keys.Add(new ClrKey(0.800f, new Rgb(0.477647f, 0.821961f, 0.316863f)));
        keys.Add(new ClrKey(0.867f, new Rgb(0.648366f, 0.858039f, 0.208889f)));
        keys.Add(new ClrKey(0.933f, new Rgb(0.825098f, 0.884967f, 0.114771f)));
        keys.Add(new ClrKey(1.000f, new Rgb(0.992157f, 0.905882f, 0.145098f)));

        return target;
    }

    /// <summary>
    /// Finds the span, or range, of the color gradient, typically
    /// [0.0, 1.0] and under. Equal to the step of the last color
    /// key minus the step of the first.
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
        return ClrGradient.ToString(new StringBuilder(1024), cg, places).ToString();
    }

    /// <summary>
    /// Appendsa a representation of a color gradient to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="cg">color gradient</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString(in StringBuilder sb, in ClrGradient cg, in int places = 4)
    {
        List<ClrKey> keys = cg.keys;
        int len = keys.Count;
        int last = len - 1;

        sb.Append("{ keys: [ ");
        for (int i = 0; i < last; ++i)
        {
            ClrKey.ToString(sb, keys[i], places);
            sb.Append(',');
            sb.Append(' ');
        }
        ClrKey.ToString(sb, keys[last], places);
        sb.Append(" ] }");
        return sb;
    }
}