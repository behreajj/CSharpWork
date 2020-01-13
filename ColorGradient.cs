using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

[Serializable]
public class ColorGradient : IEnumerable
{
  public readonly struct Key : IComparable<Key>, IEquatable<Key>
  {
    private readonly float step;
    private readonly Clr color;

    public int Length { get { return 5; } }
    public float Step { get { return this.step; } }
    public Clr Color { get { return this.color; } }

    public Key (
      float step = 0.0f,
      Clr color = new Clr ( ))
    {
      this.step = Utils.Clamp (step, 0.0f, 1.0f);
      this.color = color;
    }

    public override bool Equals (object value)
    {
      if (Object.ReferenceEquals (this, value))
      {
        return true;
      }

      if (Object.ReferenceEquals (null, value))
      {
        return false;
      }

      if (value is Key)
      {
        Key k = (Key) value;

        if (this.step.GetHashCode ( ) != k.step.GetHashCode ( ))
        {
          return false;
        }

        return true;
      }
      return true;
    }

    public override int GetHashCode ( )
    {
      unchecked
      {
        int hash = Utils.HashBase;
        hash = hash * Utils.HashMul ^ this.step.GetHashCode ( );
        return hash;
      }
    }

    public override string ToString ( )
    {
      return ToString (4);
    }

    public int CompareTo (Key k)
    {
      return (this.step > k.step) ? 1 :
        (this.step < k.step) ? -1 :
        0;
    }

    public bool Equals (Key k)
    {
      if (this.step.GetHashCode ( ) != k.step.GetHashCode ( ))
      {
        return false;
      }

      return true;
    }

    public string ToString (int places = 4)
    {
      return new StringBuilder ( )
        .Append ("{ step: ")
        .Append (Utils.ToFixed (this.step, 3))
        .Append (", color: ")
        .Append (this.color.ToString (places))
        .Append (" }")
        .ToString ( );
    }

    public static implicit operator Key (float step)
    {
      float s = Utils.Clamp (step, 0.0f, 1.0f);
      return new Key (s, new Clr (s, s, s, s));
    }

    public static bool operator < (in Key a, in Key b)
    {
      return a.step < b.step;
    }

    public static bool operator > (in Key a, in Key b)
    {
      return a.step > b.step;
    }

    public static bool operator <= (in Key a, in Key b)
    {
      return a.step <= b.step;
    }

    public static bool operator >= (in Key a, in Key b)
    {
      return a.step >= b.step;
    }

    public static bool operator != (in Key a, in Key b)
    {
      return a.step != b.step;
    }

    public static bool operator == (in Key a, in Key b)
    {
      return a.step == b.step;
    }
  }

  protected readonly List<Key> keys = new List<Key> (16);

  public int Length { get { return this.keys.Count; } }

  public ColorGradient ( )
  {
    this.keys.Add (new Key (0.0f, Clr.ClearBlack));
    this.keys.Add (new Key (1.0f, Clr.White));
  }

  public ColorGradient (params Key[ ] keys)
  {
    this.AppendAll (keys);
  }

  public ColorGradient (params Clr[ ] colors)
  {
    this.AppendAll (colors);
  }

  public Key this [int i]
  {
    get
    {
      return this.keys[Utils.Mod (i, this.keys.Count)];
    }
  }

  public Clr this [float step]
  {
    get
    {
      return this.Eval (step);
    }
  }

  public IEnumerator GetEnumerator ( )
  {
    return this.keys.GetEnumerator ( );
  }

  public override int GetHashCode ( )
  {
    return this.keys.GetHashCode ( );
  }

  public override string ToString ( )
  {
    return this.ToString (4);
  }

  public ColorGradient Append (Key key)
  {
    int query = this.ContainsKey (key);
    if (query != -1) this.keys.RemoveAt (query);
    this.InsortRight (key);
    return this;
  }

  public ColorGradient Append (Clr color)
  {
    this.ShiftKeysLeft (1);
    this.keys.Add (new Key (1.0f, color));
    return this;
  }

  public ColorGradient AppendAll (params Key[ ] keys)
  {
    foreach (Key key in keys) this.Append (key);
    return this;
  }

  public ColorGradient AppendAll (params Clr[ ] colors)
  {
    int len = colors.Length;
    this.ShiftKeysLeft (len);
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

  public int BisectLeft (float step = 0.5f)
  {
    int low = 0;
    int high = this.keys.Count;
    while (low < high)
    {
      int middle = (low + high) / 2 | 0;
      if (step > this.keys[middle].Step)
        low = middle + 1;
      else
        high = middle;
    }
    return low;
  }

  public int BisectRight (float step = 0.5f)
  {
    int low = 0;
    int high = this.keys.Count;
    while (low < high)
    {
      int middle = (low + high) / 2 | 0;
      if (step < this.keys[middle].Step)
        high = middle;
      else
        low = middle + 1;
    }
    return low;
  }

  public int ContainsKey (Key key)
  {
    return this.keys.FindIndex ((Key x) => Utils.Approx (x.Step, key.Step, 0.0005f));
  }

  public Clr Eval (float step)
  {
    Key prevKey = this.FindLe (step);
    Key nextKey = this.FindGe (step);
    float prevStep = prevKey.Step;
    float nextStep = nextKey.Step;
    if (prevStep == nextStep) return prevKey.Color;
    float fac = (step - nextStep) / (prevStep - nextStep);
    return Clr.MixRgba (prevKey.Color, nextKey.Color, fac);
  }

  public Clr[ ] EvalRange (int count = 8)
  {
    int vCount = count < 3 ? 3 : count;
    Clr[ ] result = new Clr[vCount];
    float toPercent = 1.0f / (vCount - 1.0f);
    for (int i = 0; i < vCount; ++i)
    {
      result[i] = this.Eval (i * toPercent);
    }
    return result;
  }

  public Key FindGe (float query = 0.5f)
  {
    int i = this.BisectLeft (query);
    if (i < this.keys.Count) return this.keys[i];
    return this.keys[this.keys.Count - 1];
  }

  public Key FindLe (float query = 0.5f)
  {
    int i = this.BisectRight (query);
    if (i > 0) return this.keys[i - 1];
    return this.keys[0];
  }

  public Key GetFirst ( )
  {
    return this.keys[0];
  }

  public Key GetLast ( )
  {
    return this.keys[this.keys.Count - 1];
  }

  public ColorGradient InsortLeft (Key key)
  {
    int i = this.BisectLeft (key.Step);
    this.keys.Insert (i, key);
    return this;
  }

  public ColorGradient InsortRight (Key key)
  {
    int i = this.BisectRight (key.Step);
    this.keys.Insert (i, key);
    return this;
  }

  public Key RemoveAt (int i = -1)
  {
    int j = Utils.Mod (i, this.keys.Count);
    Key key = this.keys[j];
    this.keys.RemoveAt (j);
    return key;
  }

  public Key RemoveFirst ( )
  {
    return this.RemoveAt (0);
  }

  public Key RemoveLast ( )
  {
    return this.RemoveAt (this.keys.Count - 1);
  }

  public ColorGradient Reset ( )
  {
    this.keys.Clear ( );
    this.keys.Add (new Key (0.0f, Clr.ClearBlack));
    this.keys.Add (new Key (1.0f, Clr.White));
    return this;
  }

  public ColorGradient Reverse ( )
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

  protected ColorGradient ShiftKeysLeft (int added = 1)
  {
    int len = this.keys.Count;
    float scalar = 1.0f / (len + added - 1.0f);
    for (int i = 0; i < len; ++i)
    {
      Key key = this.keys[i];
      this.keys[i] = new Key (key.Step * i * scalar, key.Color);
    }
    return this;
  }

  public Key[ ] ToArray ( )
  {
    return this.keys.ToArray ( );
  }

  public string ToString (int places = 4)
  {
    StringBuilder sb = new StringBuilder ( );
    sb.Append ("{ keys: [ ");
    int len = this.keys.Count;
    int last = len - 1;
    for (int i = 0; i < len; ++i)
    {
      Key key = this.keys[i];
      sb.Append (key.ToString (places));
      if (i < last)
        sb.Append (", ");
    }
    sb.Append (" ] }");
    return sb.ToString ( );
  }

  public static ColorGradient PaletteMagma (ColorGradient target)
  {
    target.keys.Clear ( );
    target.keys.Capacity = 16;

    target.keys.Add (new Key (0.000f, new Clr (0.988235f, 1.000000f, 0.698039f)));
    target.keys.Add (new Key (0.067f, new Clr (0.987190f, 0.843137f, 0.562092f)));
    target.keys.Add (new Key (0.167f, new Clr (0.984314f, 0.694118f, 0.446275f)));
    target.keys.Add (new Key (0.200f, new Clr (0.981176f, 0.548235f, 0.354510f)));

    target.keys.Add (new Key (0.267f, new Clr (0.962353f, 0.412549f, 0.301176f)));
    target.keys.Add (new Key (0.333f, new Clr (0.912418f, 0.286275f, 0.298039f)));
    target.keys.Add (new Key (0.400f, new Clr (0.824314f, 0.198431f, 0.334902f)));
    target.keys.Add (new Key (0.467f, new Clr (0.703268f, 0.142484f, 0.383007f)));

    target.keys.Add (new Key (0.533f, new Clr (0.584052f, 0.110588f, 0.413856f)));
    target.keys.Add (new Key (0.600f, new Clr (0.471373f, 0.080784f, 0.430588f)));
    target.keys.Add (new Key (0.667f, new Clr (0.367320f, 0.045752f, 0.432680f)));
    target.keys.Add (new Key (0.733f, new Clr (0.267974f, 0.002353f, 0.416732f)));

    target.keys.Add (new Key (0.800f, new Clr (0.174118f, 0.006275f, 0.357647f)));
    target.keys.Add (new Key (0.867f, new Clr (0.093856f, 0.036863f, 0.232941f)));
    target.keys.Add (new Key (0.933f, new Clr (0.040784f, 0.028758f, 0.110327f)));
    target.keys.Add (new Key (0.100f, new Clr (0.000000f, 0.000000f, 0.019608f)));

    return target;
  }

  public static ColorGradient PaletteRgb (ColorGradient target)
  {
    target.keys.Clear ( );
    target.keys.Capacity = 7;

    target.keys.Add (new Key (0.000f, Clr.Red));
    target.keys.Add (new Key (0.167f, Clr.Yellow));
    target.keys.Add (new Key (0.333f, Clr.Green));
    target.keys.Add (new Key (0.500f, Clr.Cyan));
    target.keys.Add (new Key (0.667f, Clr.Blue));
    target.keys.Add (new Key (0.833f, Clr.Magenta));
    target.keys.Add (new Key (1.000f, Clr.Red));

    return target;
  }

  public static ColorGradient PaletteViridis (ColorGradient target)
  {
    target.keys.Clear ( );
    target.keys.Capacity = 16;

    target.keys.Add (new Key (0.000f, new Clr (0.266667f, 0.003922f, 0.329412f)));
    target.keys.Add (new Key (0.067f, new Clr (0.282353f, 0.100131f, 0.420654f)));
    target.keys.Add (new Key (0.167f, new Clr (0.276078f, 0.184575f, 0.487582f)));
    target.keys.Add (new Key (0.200f, new Clr (0.254902f, 0.265882f, 0.527843f)));

    target.keys.Add (new Key (0.267f, new Clr (0.221961f, 0.340654f, 0.549281f)));
    target.keys.Add (new Key (0.333f, new Clr (0.192157f, 0.405229f, 0.554248f)));
    target.keys.Add (new Key (0.400f, new Clr (0.164706f, 0.469804f, 0.556863f)));
    target.keys.Add (new Key (0.467f, new Clr (0.139869f, 0.534379f, 0.553464f)));

    target.keys.Add (new Key (0.533f, new Clr (0.122092f, 0.595033f, 0.543007f)));
    target.keys.Add (new Key (0.600f, new Clr (0.139608f, 0.658039f, 0.516863f)));
    target.keys.Add (new Key (0.667f, new Clr (0.210458f, 0.717647f, 0.471895f)));
    target.keys.Add (new Key (0.733f, new Clr (0.326797f, 0.773595f, 0.407582f)));

    target.keys.Add (new Key (0.800f, new Clr (0.477647f, 0.821961f, 0.316863f)));
    target.keys.Add (new Key (0.867f, new Clr (0.648366f, 0.858039f, 0.208889f)));
    target.keys.Add (new Key (0.933f, new Clr (0.825098f, 0.884967f, 0.114771f)));
    target.keys.Add (new Key (0.100f, new Clr (0.992157f, 0.905882f, 0.145098f)));

    return target;
  }
}