using System;
using System.Text;

/// <summary>
/// Stores a color at a given step (or percent) in the range [0.0, 1.0] .
/// Equality and hash are based solely on the step, not on the color it holds.
/// </summary>
[Serializable]
public readonly struct ClrKey : IComparable<ClrKey>, IEquatable<ClrKey>
{
    /// <summary>
    /// The key's color.
    /// </summary>
    private readonly Lab color;

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
    public Lab Color { get { return this.color; } }

    /// <summary>
    /// Constructs a key from a step and color.
    /// </summary>
    /// <param name="step">step</param>
    /// <param name="color">color</param>
    public ClrKey(in float step, Lab color)
    {
        this.step = Utils.Clamp(step, 0.0f, 1.0f);
        this.color = color;
    }

    /// <summary>
    /// Tests this color key for equivalence with an object.
    /// </summary>
    /// <param name="value">the object</param>
    /// <returns>equivalence</returns>
    public override bool Equals(object value)
    {
        if (Object.ReferenceEquals(this, value)) { return true; }
        if (value is null) { return false; }
        if (value is ClrKey key) { return this.Equals(key); }
        return false;
    }

    /// <summary>
    /// Returns a hash code for this key based on its step, not based on its
    /// color.
    /// </summary>
    /// <returns>hash code</returns>
    public override int GetHashCode()
    {
        return this.step.GetHashCode();
    }

    /// <summary>
    /// Returns a string representation of this key.
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()
    {
        return ClrKey.ToString(this);
    }

    /// <summary>
    /// Returns -1 when this key is less than the comparisand; 1 when it is
    /// greater than; 0 when the two are 'equal'. 
    /// </summary>
    /// <param name="k">key</param>
    /// <returns>comparison</returns>
    public int CompareTo(ClrKey k)
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
    /// <returns>evaluation</returns>
    public bool Equals(ClrKey k)
    {
        return this.GetHashCode() == k.GetHashCode();
    }

    /// <summary>
    /// Evaluates whether the left comparisand is less than the right
    /// comparisand.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static bool operator <(in ClrKey a, in ClrKey b)
    {
        return a.step < b.step;
    }

    /// <summary>
    /// Evaluates whether the left comparisand is greater than the right
    /// comparisand.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static bool operator >(in ClrKey a, in ClrKey b)
    {
        return a.step > b.step;
    }

    /// <summary>
    /// Evaluates whether the left comparisand is less than or equal to the
    /// right comparisand.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static bool operator <=(in ClrKey a, in ClrKey b)
    {
        return a.step <= b.step;
    }

    /// <summary>
    /// Evaluates whether the left comparisand is greater than or equal to the
    /// right comparisand.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static bool operator >=(in ClrKey a, in ClrKey b)
    {
        return a.step >= b.step;
    }

    /// <summary>
    /// Evaluates whether two color keys do not equal each other.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static bool operator !=(in ClrKey a, in ClrKey b)
    {
        return a.step != b.step;
    }

    /// <summary>
    /// Evaluates whether two color keys equal each other.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static bool operator ==(in ClrKey a, in ClrKey b)
    {
        return a.step == b.step;
    }

    /// <summary>
    /// Returns a string representation of a key.
    /// </summary>
    /// <param name="key">color key</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string</returns>
    public static string ToString(in ClrKey key, in int places = 4)
    {
        return ClrKey.ToString(new StringBuilder(96), key, places).ToString();
    }

    /// <summary>
    /// Appends a representation of a key to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="key">color key</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString(
        in StringBuilder sb,
        in ClrKey key,
        in int places = 4)
    {
        sb.Append("{\"step\":");
        Utils.ToFixed(sb, key.step, places);
        sb.Append(",\"color\":");
        Lab.ToString(sb, key.color, places);
        sb.Append('}');
        return sb;
    }
}