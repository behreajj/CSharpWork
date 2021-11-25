using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Contains a list of keys which hold colors at steps in the range [0.0, 1.0] .
/// Allows for smooth color transitions to be evaluated by a factor.
/// </summary>
[Serializable]
public class ClrGradient : IEnumerable
{
    /// <summary>
    /// Stores a color at a given step (or percent) in the range [0.0, 1.0] .
    /// Equality and hash are based solely on the step, not on the color it holds.
    /// </summary>
    [Serializable]
    public readonly struct Key : IComparable<Key>, IEquatable<Key>
    {
        /// <summary>
        /// The key's color.
        /// </summary>
        private readonly Clr color;

        /// <summary>
        /// The key's step, expected to be in the range [0.0, 1.0] .
        /// </summary>
        private readonly float step;

        /// <summary>
        /// The key's step, expected to be in the range [0.0, 1.0] .
        /// </summary>
        /// <value>step</value>
        public float Step { get { return this.step; } }

        /// <summary>
        /// The key's color.
        /// </summary>
        /// <value>color</value>
        public Clr Color { get { return this.color; } }

        /// <summary>
        /// Constructs a key from a step and color.
        /// </summary>
        /// <param name="step">step</param>
        /// <param name="color">color</param>
        public Key (in float step, Clr color)
        {
            this.step = Utils.Clamp (step, 0.0f, 1.0f);
            this.color = color;
        }

        /// <summary>
        /// Tests this color key for equivalence with an object.
        /// </summary>
        /// <param name="value">the object</param>
        /// <returns>the equivalence</returns>
        public override bool Equals (object value)
        {
            if (Object.ReferenceEquals (this, value)) { return true; }
            if (value is null) { return false; }
            if (value is ClrGradient.Key) { return this.Equals ((ClrGradient.Key) value); }
            return false;
        }

        /// <summary>
        /// Returns a hash code for this key based on its step, not based on its
        /// color.
        /// </summary>
        /// <returns>the hash code</returns>
        public override int GetHashCode ( )
        {
            return this.step.GetHashCode ( );
        }

        /// <summary>
        /// Returns a string representation of this key.
        /// </summary>
        /// <returns>the string</returns>
        public override string ToString ( )
        {
            return Key.ToString (this);
        }

        /// <summary>
        /// Returns -1 when this key is less than the comparisand; 1 when it is
        /// greater than; 0 when the two are 'equal'. 
        /// </summary>
        /// <param name="k">key</param>
        /// <returns>the comparison</returns>
        public int CompareTo (Key k)
        {
            return (this.step < k.step) ? -1 :
                (this.step > k.step) ? 1 :
                0;
        }

        /// <summary>
        /// Tests this key for equivalence with another in compliance with the
        /// IEquatable interface.
        /// </summary>
        /// <param name="k">key</param>
        /// <returns>the evaluation</returns>
        public bool Equals (Key k)
        {
            return this.GetHashCode ( ) == k.GetHashCode ( );
        }

        /// <summary>
        /// Evaluates whether the left comparisand is less than the right
        /// comparisand.
        /// </summary>
        /// <param name="a">left comparisand</param>
        /// <param name="b">right comparisand</param>
        /// <returns>the evaluation</returns>
        public static bool operator < (in Key a, in Key b)
        {
            return a.step < b.step;
        }

        /// <summary>
        /// Evaluates whether the left comparisand is greater than the right
        /// comparisand.
        /// </summary>
        /// <param name="a">left comparisand</param>
        /// <param name="b">right comparisand</param>
        /// <returns>the evaluation</returns>
        public static bool operator > (in Key a, in Key b)
        {
            return a.step > b.step;
        }

        /// <summary>
        /// Evaluates whether the left comparisand is less than or equal to the
        /// right comparisand.
        /// </summary>
        /// <param name="a">left comparisand</param>
        /// <param name="b">right comparisand</param>
        /// <returns>the evaluation</returns>
        public static bool operator <= (in Key a, in Key b)
        {
            return a.step <= b.step;
        }

        /// <summary>
        /// Evaluates whether the left comparisand is greater than or equal to the
        /// right comparisand.
        /// </summary>
        /// <param name="a">left comparisand</param>
        /// <param name="b">right comparisand</param>
        /// <returns>the evaluation</returns>
        public static bool operator >= (in Key a, in Key b)
        {
            return a.step >= b.step;
        }

        /// <summary>
        /// Evaluates whether two color keys do not equal each other.
        /// </summary>
        /// <param name="a">left comparisand</param>
        /// <param name="b">right comparisand</param>
        /// <returns>the evaluation</returns>
        public static bool operator != (in Key a, in Key b)
        {
            return a.step != b.step;
        }

        /// <summary>
        /// Evaluates whether two color keys equal each other.
        /// </summary>
        /// <param name="a">left comparisand</param>
        /// <param name="b">right comparisand</param>
        /// <returns>the evaluation</returns>
        public static bool operator == (in Key a, in Key b)
        {
            return a.step == b.step;
        }

        /// <summary>
        /// Returns a string representation of a key.
        /// </summary>
        /// <param name="key">color key</param>
        /// <param name="places">number of decimal places</param>
        /// <returns>string</returns>
        public static string ToString (in Key key, in int places = 4)
        {
            return Key.ToString (new StringBuilder (96), key, places).ToString ( );
        }

        /// <summary>
        /// Appends a representation of a key to a string builder.
        /// </summary>
        /// <param name="sb">string builder</param>
        /// <param name="key">color key</param>
        /// <param name="places">number of decimal places</param>
        /// <returns>string builder</returns>
        public static StringBuilder ToString (in StringBuilder sb, in Key key, in int places = 4)
        {
            sb.Append ("{ step: ");
            Utils.ToFixed (sb, key.step, places);
            sb.Append (", color: ");
            Clr.ToString (sb, key.color, places);
            sb.Append (' ');
            sb.Append ('}');
            return sb;
        }
    }

    /// <summary>
    /// The list of color keys in the gradient.
    /// </summary>
    protected readonly List<Key> keys = new List<Key> (16);

    /// <summary>
    /// Returns the number of color keys in this gradient.
    /// </summary>
    /// <value>the length</value>
    public int Length { get { return this.keys.Count; } }

    /// <summary>
    /// Constructs a color gradient.
    /// </summary>
    public ClrGradient ( )
    {
        // TODO: Reconsider this...
        this.keys.Add (new Key (0.0f, Clr.ClearBlack));
        this.keys.Add (new Key (1.0f, Clr.White));
    }

    /// <summary>
    /// Constructs a color gradient from a list of color keys.
    /// </summary>
    /// <param name="keys">color keys</param>
    public ClrGradient (params Key[ ] keys)
    {
        this.InsertAll (keys);
    }

    /// <summary>
    /// Constructs a color gradient from a list of colors. The color keys will be
    /// evenly distributed across the gradient.
    /// </summary>
    /// <param name="keys">color keys</param>
    public ClrGradient (params Clr[ ] colors)
    {
        this.AppendAll (colors);
    }

    /// <summary>
    /// Retrieves a color key by index. Wraps the index by the number of color
    /// keys.
    /// </summary>
    /// <value>the color key</value>
    public Key this [int i]
    {
        get
        {
            return this.keys[Utils.RemFloor (i, this.keys.Count)];
        }
    }

    /// <summary>
    /// Retrieves a color given a step in the range [0.0, 1.0] .
    /// </summary>
    /// <value></value>
    public Clr this [float step]
    {
        get
        {
            return this.Eval (step);
        }
    }

    /// <summary>
    /// Gets the hash code for the keys of this gradient.
    /// </summary>
    /// <returns>the hash code</returns>
    public override int GetHashCode ( )
    {
        return this.keys.GetHashCode ( );
    }

    /// <summary>
    /// Returns a string representation of this gradient.
    /// </summary>
    /// <returns>the string</returns>
    public override string ToString ( )
    {
        return ClrGradient.ToString (this);
    }

    /// <summary>
    /// Appends a color at the end of the gradient.
    /// </summary>
    /// <param name="color">color</param>
    /// <returns>this gradient</returns>
    public ClrGradient Append (Clr color)
    {
        this.CompressKeysLeft (1);
        this.keys.Add (new Key (1.0f, color));
        return this;
    }

    /// <summary>
    /// Appends a list of colors at the end of the gradient.
    ///
    /// The color keys will be evenly distributed across the gradient.
    /// </summary>
    /// <param name="colors">colors</param>
    /// <returns>this gradient</returns>
    public ClrGradient AppendAll (params Clr[ ] colors)
    {
        int len = colors.Length;
        this.CompressKeysLeft (len);
        int oldLen = this.keys.Count;
        float denom = 1.0f / (oldLen + len - 1.0f);
        for (int i = 0; i < len; ++i)
        {
            this.keys.Add (new Key (
                (oldLen + i) * denom,
                colors[i]));
        }
        return this;
    }

    /// <summary>
    /// Locates the insertion point for a step in the gradient that will maintain sorted order.
    /// </summary>
    /// <param name="step">step</param>
    /// <returns>the index</returns>
    protected int BisectLeft (in float step)
    {
        int low = 0;
        int high = this.keys.Count;
        while (low < high)
        {
            // The | 0 is floor div.
            int middle = (low + high) / 2 | 0;
            if (step > this.keys[middle].Step)
            {
                low = middle + 1;
            }
            else
            {
                high = middle;
            }
        }
        return low;
    }

    /// <summary>
    /// Locates the insertion point for a step in the gradient that will maintain sorted order.
    /// </summary>
    /// <param name="step">step</param>
    /// <returns>the index</returns>
    protected int BisectRight (in float step)
    {
        int low = 0;
        int high = this.keys.Count;
        while (low < high)
        {
            // The | 0 is floor div.
            int middle = (low + high) / 2 | 0;
            if (step < this.keys[middle].Step)
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
    /// Helper function that shifts existing keys to the left when a new color is
    /// appended to the gradient without a key.
    /// </summary>
    /// <param name="added">number to add</param>
    /// <returns>this gradient</returns>
    protected ClrGradient CompressKeysLeft (in int added = 1)
    {
        int len = this.keys.Count;
        float scalar = 1.0f / (len + added - 1.0f);
        for (int i = 0; i < len; ++i)
        {
            Key key = this.keys[i];
            this.keys[i] = new Key (
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
    protected ClrGradient CompressKeysRight (in int added = 1)
    {
        int len = this.keys.Count;
        float scalar = added / (len + added - 1.0f);
        float coeff = 1.0f - scalar;
        for (int i = 0; i < len; ++i)
        {
            Key key = this.keys[i];
            this.keys[i] = new Key (
                scalar + coeff * key.Step,
                key.Color);
        }
        return this;
    }

    /// <summary>
    /// Evaluates whether the gradient contains a key at a given tolerance.
    /// </summary>
    /// <param name="key">the key</param>
    /// <returns>the evaluation</returns>
    public int ContainsKey (Key key)
    {
        return this.keys.FindIndex ((Key x) => Utils.Approx (x.Step, key.Step, 0.0005f));
    }

    /// <summary>
    /// Gets the enumerator for the keys of this gradient.
    /// </summary>
    /// <returns>the enumerator</returns>
    public IEnumerator GetEnumerator ( )
    {
        return this.keys.GetEnumerator ( );
    }

    /// <summary>
    /// Finds a color given a step in the range [0.0, 1.0]. When the step falls
    /// between color keys, the resultant color is created by an easing function.
    /// </summary>
    /// <param name="step">step</param>
    /// <returns>color</returns>
    public Clr Eval (in float step)
    {
        return this.Eval (step, (x, y, z) => Clr.MixRgbaLinear (x, y, z));
    }

    /// <summary>
    /// Finds a color given a step in the range [0.0, 1.0]. When the step falls
    /// between color keys, the resultant color is created by an easing function.
    /// </summary>
    /// <param name="step">step</param>
    /// <param name="easing">easing function</param>
    /// <returns>color</returns>
    public Clr Eval (in float step, in Func<Clr, Clr, float, Clr> easing)
    {
        Key prevKey = this.FindLe (step);
        Key nextKey = this.FindGe (step);
        float prevStep = prevKey.Step;
        float nextStep = nextKey.Step;
        if (prevStep == nextStep) { return prevKey.Color; }
        float fac = (step - nextStep) / (prevStep - nextStep);
        return easing (prevKey.Color, nextKey.Color, fac);
    }

    /// <summary>
    /// Evaluates an array of colors for a step distributed evenly across the
    /// range [0.0, 1.0] for the supplied count.
    /// </summary>
    /// <param name="count">count</param>
    /// <param name="origin">origin</param>
    /// <param name="dest">destination</param>
    /// <returns>the colors</returns>
    public Clr[ ] EvalRange (in int count, in float origin = 0.0f, in float dest = 1.0f)
    {
        return this.EvalRange (count, origin, dest, (x, y, z) => Clr.MixRgbaLinear (x, y, z));
    }

    /// <summary>
    /// Evaluates an array of colors for a step distributed evenly across the
    /// range [0.0, 1.0] for the supplied count.
    /// </summary>
    /// <param name="count">count</param>
    /// <param name="origin">origin</param>
    /// <param name="dest">destination</param>
    /// <param name="easing">easing function</param>
    /// <returns>the colors</returns>
    public Clr[ ] EvalRange ( //
        in int count, //
        in float origin, //
        in float dest, //
        in Func<Clr, Clr, float, Clr> easing)
    {
        int vCount = count < 3 ? 3 : count;
        float vOrigin = Utils.Clamp (origin, 0.0f, 1.0f);
        float vDest = Utils.Clamp (dest, 0.0f, 1.0f);

        Clr[ ] result = new Clr[vCount];
        float toPercent = 1.0f / (vCount - 1.0f);
        for (int i = 0; i < vCount; ++i)
        {
            float prc = i * toPercent;
            result[i] = this.Eval (
                (1.0f - prc) * vOrigin +
                prc * vDest, easing);
        }
        return result;
    }

    /// <summary>
    /// Finds a key greater than or equal to the query.
    /// </summary>
    /// <param name="query">query</param>
    /// <returns>the key</returns>
    protected Key FindGe (in float query)
    {
        int i = this.BisectLeft (query);
        if (i < this.keys.Count) { return this.keys[i]; }
        return this.keys[this.keys.Count - 1];
    }

    /// <summary>
    /// Finds a key less than or equal to the query.
    /// </summary>
    /// <param name="query">query</param>
    /// <returns>the key</returns>
    protected Key FindLe (in float query)
    {
        int i = this.BisectRight (query);
        if (i > 0) { return this.keys[i - 1]; }
        return this.keys[0];
    }

    /// <summary>
    /// Gets the first color key from the gradient.
    /// </summary>
    /// <returns>the first key</returns>
    public Key GetFirst ( )
    {
        return this.keys[0];
    }

    /// <summary>
    /// Gets the last color key from the gradient.
    /// </summary>
    /// <returns>the first key</returns>
    public Key GetLast ( )
    {
        return this.keys[this.keys.Count - 1];
    }

    /// <summary>
    /// Inserts a color key into this gradient.
    ///
    /// Replaces any key which already exists at the insertion step.
    /// </summary>
    /// <param name="key">color key</param>
    /// <returns>this gradient</returns>
    public ClrGradient Insert (in Key key)
    {
        int query = this.ContainsKey (key);
        if (query != -1) { this.keys.RemoveAt (query); }
        this.InsortRight (key);
        return this;
    }

    /// <summary>
    /// Inserts all color keys into this gradient.
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    public ClrGradient InsertAll (params Key[ ] keys)
    {
        foreach (Key key in keys) { this.Insert (key); }
        return this;
    }

    /// <summary>
    /// Inserts a color key into the keys array based on the index returned from bisectLeft .
    /// </summary>
    /// <param name="key">color key</param>
    /// <returns>this gradient</returns>
    protected ClrGradient InsortLeft (in Key key)
    {
        int i = this.BisectLeft (key.Step);
        this.keys.Insert (i, key);
        return this;
    }

    /// <summary>
    /// Inserts a color key into the keys array based on the index returned from bisectRight .
    /// </summary>
    /// <param name="key">color key</param>
    /// <returns>this gradient</returns>
    protected ClrGradient InsortRight (in Key key)
    {
        int i = this.BisectRight (key.Step);
        this.keys.Insert (i, key);
        return this;
    }

    /// <summary>
    /// Appends a color to this gradient. Shifts existing keys to the right by 1.
    /// </summary>
    /// <param name="color">color</param>
    /// <returns>this gradient</returns>
    public ClrGradient Prepend (in Clr color)
    {
        this.CompressKeysRight (1);
        this.keys.Insert (0, new Key (0.0f, color));
        return this;
    }

    /// <summary>
    /// Prepends a list of colors to this gradient. Shifts existing keys to the
    /// right.
    /// </summary>
    /// <param name="colors">colors</param>
    /// <returns>this gradient</returns>
    public ClrGradient PrependAll (params Clr[ ] colors)
    {
        int len = colors.Length;
        this.CompressKeysRight (len);
        int oldLen = this.keys.Count;
        float denom = 1.0f / (oldLen + len - 1.0f);
        for (int i = 0; i < len; ++i)
        {
            this.keys.Insert (i, new Key (
                i * denom,
                colors[i]));
        }
        return this;
    }

    /// <summary>
    /// Removes a key at a given index.
    /// </summary>
    /// <param name="i">index</param>
    /// <returns>the key</returns>
    public Key RemoveAt (in int i = -1)
    {
        int j = Utils.RemFloor (i, this.keys.Count);
        Key key = this.keys[j];
        this.keys.RemoveAt (j);
        return key;
    }

    /// <summary>
    /// Removes the first key from the gradient.
    /// </summary>
    /// <returns>the key</returns>
    public Key RemoveFirst ( )
    {
        return this.RemoveAt (0);
    }

    /// <summary>
    /// Removes the last key from the gradient.
    /// </summary>
    /// <returns>the key</returns>
    public Key RemoveLast ( )
    {
        return this.RemoveAt (this.keys.Count - 1);
    }

    /// <summary>
    /// Resets this gradient to an initial state, with two color keys: clear black
    /// at 0.0 and opaque white at 1.0 .
    /// </summary>
    /// <returns>this gradient</returns>
    public ClrGradient Reset ( )
    {
        this.keys.Clear ( );
        this.keys.Add (new Key (0.0f, Clr.ClearBlack));
        this.keys.Add (new Key (1.0f, Clr.White));
        return this;
    }

    /// <summary>
    /// Reverses the gradient. The step of each color key is subtracted from one.
    /// </summary>
    /// <returns>this gradient</returns>
    public ClrGradient Reverse ( )
    {
        this.keys.Reverse ( );
        int len = this.keys.Count;
        for (int i = 0; i < len; ++i)
        {
            Key key = this.keys[i];
            this.keys[i] = new Key (1.0f - key.Step, key.Color);
        }
        return this;
    }

    /// <summary>
    /// Returns the Magma color palette, consisting of 16 keys.
    /// </summary>
    /// <param name="target">output gradient</param>
    /// <returns>the gradient</returns>
    public static ClrGradient PaletteMagma (in ClrGradient target)
    {
        List<Key> keys = target.keys;
        keys.Clear ( );
        keys.Capacity = 16;

        keys.Add (new Key (0.000f, new Clr (0.988235f, 1.000000f, 0.698039f)));
        keys.Add (new Key (0.067f, new Clr (0.987190f, 0.843137f, 0.562092f)));
        keys.Add (new Key (0.167f, new Clr (0.984314f, 0.694118f, 0.446275f)));
        keys.Add (new Key (0.200f, new Clr (0.981176f, 0.548235f, 0.354510f)));

        keys.Add (new Key (0.267f, new Clr (0.962353f, 0.412549f, 0.301176f)));
        keys.Add (new Key (0.333f, new Clr (0.912418f, 0.286275f, 0.298039f)));
        keys.Add (new Key (0.400f, new Clr (0.824314f, 0.198431f, 0.334902f)));
        keys.Add (new Key (0.467f, new Clr (0.703268f, 0.142484f, 0.383007f)));

        keys.Add (new Key (0.533f, new Clr (0.584052f, 0.110588f, 0.413856f)));
        keys.Add (new Key (0.600f, new Clr (0.471373f, 0.080784f, 0.430588f)));
        keys.Add (new Key (0.667f, new Clr (0.367320f, 0.045752f, 0.432680f)));
        keys.Add (new Key (0.733f, new Clr (0.267974f, 0.002353f, 0.416732f)));

        keys.Add (new Key (0.800f, new Clr (0.174118f, 0.006275f, 0.357647f)));
        keys.Add (new Key (0.867f, new Clr (0.093856f, 0.036863f, 0.232941f)));
        keys.Add (new Key (0.933f, new Clr (0.040784f, 0.028758f, 0.110327f)));
        keys.Add (new Key (1.000f, new Clr (0.000000f, 0.000000f, 0.019608f)));

        return target;
    }

    /// <summary>
    /// Returns seven primary and secondary colors: red, yellow, green, cyan,
    /// blue, magenta and red. Red is repeated so the gradient is periodic.
    /// </summary>
    /// <param name="target">output gradient</param>
    /// <returns>the gradient</returns>
    public static ClrGradient PaletteRgb (in ClrGradient target)
    {
        List<Key> keys = target.keys;
        keys.Clear ( );
        keys.Capacity = 7;

        keys.Add (new Key (0.000f, Clr.Red));
        keys.Add (new Key (0.167f, Clr.Yellow));
        keys.Add (new Key (0.333f, Clr.Green));
        keys.Add (new Key (0.500f, Clr.Cyan));
        keys.Add (new Key (0.667f, Clr.Blue));
        keys.Add (new Key (0.833f, Clr.Magenta));
        keys.Add (new Key (1.000f, Clr.Red));

        return target;
    }

    /// <summary>
    /// Returns thirteen colors in the red yellow blue color wheel. Red is
    /// repeated so that the gradient is periodic.
    /// </summary>
    /// <param name="target">output gradient</param>
    /// <returns>the gradient</returns>
    public static ClrGradient PaletteRyb (in ClrGradient target)
    {
        List<Key> keys = target.keys;
        keys.Clear ( );
        keys.Capacity = 13;

        keys.Add (new Key (0.000f, new Clr (1.000000f, 0.000000f, 0.000000f)));
        keys.Add (new Key (0.083f, new Clr (1.000000f, 0.250000f, 0.000000f)));
        keys.Add (new Key (0.167f, new Clr (1.000000f, 0.500000f, 0.000000f)));
        keys.Add (new Key (0.250f, new Clr (1.000000f, 0.750000f, 0.000000f)));
        keys.Add (new Key (0.333f, new Clr (1.000000f, 1.000000f, 0.000000f)));
        keys.Add (new Key (0.417f, new Clr (0.505882f, 0.831373f, 0.101961f)));
        keys.Add (new Key (0.500f, new Clr (0.000000f, 0.662745f, 0.200000f)));
        keys.Add (new Key (0.583f, new Clr (0.082353f, 0.517647f, 0.400000f)));
        keys.Add (new Key (0.667f, new Clr (0.164706f, 0.376471f, 0.600000f)));
        keys.Add (new Key (0.750f, new Clr (0.333333f, 0.188235f, 0.552941f)));
        keys.Add (new Key (0.833f, new Clr (0.500000f, 0.000000f, 0.500000f)));
        keys.Add (new Key (0.917f, new Clr (0.750000f, 0.000000f, 0.250000f)));
        keys.Add (new Key (1.000f, new Clr (1.000000f, 0.000000f, 0.000000f)));

        return target;
    }

    /// <summary>
    /// Returns the Viridis color palette, consisting of 16 keys.
    /// </summary>
    /// <param name="target">output gradient</param>
    /// <returns>the gradient</returns>
    public static ClrGradient PaletteViridis (in ClrGradient target)
    {
        List<Key> keys = target.keys;
        keys.Clear ( );
        keys.Capacity = 16;

        keys.Add (new Key (0.000f, new Clr (0.266667f, 0.003922f, 0.329412f)));
        keys.Add (new Key (0.067f, new Clr (0.282353f, 0.100131f, 0.420654f)));
        keys.Add (new Key (0.167f, new Clr (0.276078f, 0.184575f, 0.487582f)));
        keys.Add (new Key (0.200f, new Clr (0.254902f, 0.265882f, 0.527843f)));

        keys.Add (new Key (0.267f, new Clr (0.221961f, 0.340654f, 0.549281f)));
        keys.Add (new Key (0.333f, new Clr (0.192157f, 0.405229f, 0.554248f)));
        keys.Add (new Key (0.400f, new Clr (0.164706f, 0.469804f, 0.556863f)));
        keys.Add (new Key (0.467f, new Clr (0.139869f, 0.534379f, 0.553464f)));

        keys.Add (new Key (0.533f, new Clr (0.122092f, 0.595033f, 0.543007f)));
        keys.Add (new Key (0.600f, new Clr (0.139608f, 0.658039f, 0.516863f)));
        keys.Add (new Key (0.667f, new Clr (0.210458f, 0.717647f, 0.471895f)));
        keys.Add (new Key (0.733f, new Clr (0.326797f, 0.773595f, 0.407582f)));

        keys.Add (new Key (0.800f, new Clr (0.477647f, 0.821961f, 0.316863f)));
        keys.Add (new Key (0.867f, new Clr (0.648366f, 0.858039f, 0.208889f)));
        keys.Add (new Key (0.933f, new Clr (0.825098f, 0.884967f, 0.114771f)));
        keys.Add (new Key (1.000f, new Clr (0.992157f, 0.905882f, 0.145098f)));

        return target;
    }

    /// <summary>
    /// Returns a string representation of a color gradient.
    /// </summary>
    /// <param name="pal">color gradient</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string</returns>
    public static string ToString (in ClrGradient c, in int places = 4)
    {
        return ClrGradient.ToString (new StringBuilder (1024), c, places).ToString ( );
    }

    /// <summary>
    /// Appendsa a representation of a color gradient to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="c">color gradient</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString (in StringBuilder sb, in ClrGradient c, in int places = 4)
    {
        List<Key> keys = c.keys;
        int len = keys.Count;
        int last = len - 1;

        sb.Append ("{ keys: [ ");
        for (int i = 0; i < last; ++i)
        {
            Key.ToString (sb, keys[i], places);
            sb.Append (',');
            sb.Append (' ');
        }
        Key.ToString (sb, keys[last], places);
        sb.Append (" ] }");
        return sb;
    }
}