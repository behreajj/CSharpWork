using System;
using System.Collections.Generic;
using System.Text;

[Serializable]
public class ColorGradient
{
  public readonly struct Key : IComparable<Key>, IEquatable<Key>
  {
    private readonly float step;
    private readonly Clr color;

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
        .Append (Utils.ToFixed (this.step, places))
        .Append (", color: ")
        .Append (this.color.ToString (places))
        .Append (" }")
        .ToString ( );
    }
  }

  protected SortedSet<Key> keys = new SortedSet<Key> ( );

  public ColorGradient (params Key[ ] keys)
  {
    this.Append (keys);
  }

  // public Key Get (float step = 0.0f)
  // {
  //   Key query = new Key(step);
  //   Key result = new Key(0.0f);
  //   this.keys.TryGetValue(query, out result);
  //   bool success = false;
  //   if(success) {
  //     return result;
  //   }
  //   return step > 0.5f ? this.keys.Max : this.keys.Min;
  // }

  public ColorGradient Append (params Key[ ] keys)
  {
    int len = keys.Length;
    for (int i = 0; i < len; ++i)
    {
      this.keys.Add (keys[i]);
    }
    return this;
  }

  public string ToString(int places = 4)
  {
    StringBuilder result = new StringBuilder();
    IEnumerator<Key> itr = this.keys.GetEnumerator();
    
    return result.ToString();
  }
}