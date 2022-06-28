using System;
using System.Text;

/// <summary>
/// Associates a color with a name in a palette.
/// </summary>
[Serializable]
public class PalEntry : IComparable<PalEntry>, IEquatable<PalEntry>
{
    /// <summary>
    /// Character limit for entry names.
    /// </summary>
    public const int NameCharLimit = 64;

    /// <summary>
    /// The entry's color.
    /// </summary>
    protected Clr color;

    /// <summary>
    /// The entry's name.
    /// </summary>
    protected string name;

    /// <summary>
    /// The entry's color.
    /// If the color's alpha is less than
    /// zero, clear black is used instead.
    /// </summary>
    /// <value>color</value>
    public Clr Color
    {
        get
        {
            return this.color;
        }

        set
        {
            this.color = Clr.None(value) ? Clr.ClearBlack : value;
        }
    }

    /// <summary>
    /// The entry's name.
    /// If the name is invalid, the entry's color
    /// in web-friendly hexadcimal is used instead.
    /// </summary>
    /// <value>name</value>
    public string Name
    {
        get
        {
            return this.name;
        }

        set
        {
            string trval = value.Trim();
            if (trval.Length > 0)
            {
                this.name = trval.Substring(0,
                    Utils.Min(trval.Length, PalEntry.NameCharLimit));
            }
            else
            {
                this.name = Clr.ToHexWeb(this.color);
            }
        }
    }

    /// <summary>
    /// Constructs an empty entry.
    /// </summary>
    public PalEntry()
    {
        this.Color = Clr.ClearBlack;
        this.Name = "Empty";
    }

    /// <summary>
    /// Constructs an entry from a color and a name.
    /// </summary>
    /// <param name="color">color</param>
    /// <param name="name">name</param>
    public PalEntry(in Clr color, in string name = "")
    {
        this.Color = color;
        this.Name = name;
    }

    /// <summary>
    /// Tests this entry for equivalence with an object.
    /// </summary>
    /// <param name="value">the object</param>
    /// <returns>equivalence</returns>
    public override bool Equals(object value)
    {
        if (Object.ReferenceEquals(this, value)) { return true; }
        if (value is null) { return false; }
        if (value is PalEntry) { return this.Equals((PalEntry)value); }
        return false;
    }

    /// <summary>
    /// Returns a hash code for this entry based on its color.
    /// </summary>
    /// <returns>hash code</returns>
    public override int GetHashCode()
    {
        if (Clr.None(this.color)) { return 0; }
        return this.color.GetHashCode();
    }

    /// <summary>
    /// Returns a string representation of this entry.
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()
    {
        return PalEntry.ToString(this);
    }

    /// <summary>
    /// Compares entries according to their colors.
    /// </summary>
    /// <param name="pe">entry</param>
    /// <returns>the comparison</returns>
    public int CompareTo(PalEntry pe)
    {
        if (pe is null) { return 1; }

        bool leftZeroAlpha = Clr.None(this.color);
        bool rightZeroAlpha = Clr.None(pe.color);
        if (leftZeroAlpha && rightZeroAlpha) { return 0; }
        if (leftZeroAlpha) { return -1; }
        if (rightZeroAlpha) { return 1; }

        int left = Clr.ToHexArgb(this.color);
        int right = Clr.ToHexArgb(pe.color);
        return (left < right) ? -1 : (left > right) ? 1 : 0;
    }

    /// <summary>
    /// Tests this entry for equivalence with another in compliance with the
    /// IEquatable interface.
    /// </summary>
    /// <param name="k">key</param>
    /// <returns>evaluation</returns>
    public bool Equals(PalEntry pe)
    {
        if (pe is null) { return false; }
        return this.GetHashCode() == pe.GetHashCode();
    }

    /// <summary>
    /// Sets an entry from a color and a name.
    /// </summary>
    /// <param name="color">color</param>
    /// <param name="name">name</param>
    /// <returns>this entry</returns>
    public PalEntry Set(in Clr color, in string name)
    {
        this.Color = color;
        this.Name = name;
        return this;
    }

    /// <summary>
    /// Converrts a color to a palette entry.
    /// </summary>
    /// <param name="c">color</param>
    public static implicit operator PalEntry(in Clr c)
    {
        return new PalEntry(c, "");
    }

    /// <summary>
    /// Appends an entry to an array of entries.
    /// </summary>
    /// <param name="a">array</param>
    /// <param name="b">entry</param>
    /// <returns>array</returns>
    public static PalEntry[] Append(in PalEntry[] a, in PalEntry b)
    {
        bool aNull = a == null;
        bool bNull = b is null;
        if (aNull && bNull) { return new PalEntry[] { }; }
        if (bNull) { return a; }
        if (aNull) { return new PalEntry[] { b }; }

        int aLen = a.Length;
        PalEntry[] result = new PalEntry[aLen + 1];
        System.Array.Copy(a, 0, result, 0, aLen);
        result[aLen] = b;
        return result;
    }

    /// <summary>
    /// Inserts an entry into an array of entries at an index.
    /// </summary>
    /// <param name="a">array</param>
    /// <param name="index">index</param>
    /// <param name="b">entry</param>
    /// <returns>array</returns>
    public static PalEntry[] Insert(in PalEntry[] a, in int index, in PalEntry b)
    {
        bool aNull = a is null;
        bool bNull = b is null;
        if (aNull && bNull) { return new PalEntry[] { }; }
        if (bNull) { return a; }
        if (aNull) { return new PalEntry[] { b }; }

        int aLen = a.Length;
        int valIdx = Utils.RemFloor(index, aLen + 1);
        PalEntry[] result = new PalEntry[aLen + 1];
        System.Array.Copy(a, 0, result, 0, valIdx);
        result[valIdx] = b;
        System.Array.Copy(a, valIdx, result, valIdx + 1, aLen - valIdx);
        return result;
    }

    /// <summary>
    /// Prepends an entry to an array of entries.
    /// </summary>
    /// <param name="a">array</param>
    /// <param name="b">entry</param>
    /// <returns>array</returns>
    public static PalEntry[] Prepend(in PalEntry[] a, in PalEntry b)
    {
        bool aNull = a is null;
        bool bNull = b is null;
        if (aNull && bNull) { return new PalEntry[] { }; }
        if (bNull) { return a; }
        if (aNull) { return new PalEntry[] { b }; }

        int aLen = a.Length;
        PalEntry[] result = new PalEntry[aLen + 1];
        result[0] = b;
        System.Array.Copy(a, 0, result, 1, aLen);
        return result;
    }

    /// <summary>
    /// Removes an entry from from an array at an index.
    /// </summary>
    /// <param name="a">array</param>
    /// <param name="index">index</param>
    /// <returns>array</returns>
    public static (PalEntry[], PalEntry) RemoveAt(in PalEntry[] a, in int index)
    {
        bool aNull = a == null;
        if (aNull) { return (new PalEntry[] { }, null); }

        int aLen = a.Length;
        if (aLen < 1) { return (new PalEntry[] { }, null); }
        if (aLen < 2) { return (new PalEntry[] { }, a[0]); }

        int valIdx = Utils.RemFloor(index, aLen);
        PalEntry[] result = new PalEntry[aLen - 1];
        System.Array.Copy(a, 0, result, 0, valIdx);
        System.Array.Copy(a, valIdx + 1, result, valIdx, aLen - 1 - valIdx);
        return (result, a[valIdx]);
    }

    /// <summary>
    /// Resizes an array of entries.
    /// </summary>
    /// <param name="arr">array</param>
    /// <param name="sz">new size</param>
    /// <returns>resized array</returns>
    public static PalEntry[] Resize(in PalEntry[] arr, in int sz)
    {
        if (sz < 1) { return new PalEntry[] { }; }
        PalEntry[] result = new PalEntry[sz];

        if (arr == null)
        {
            for (int i = 0; i < sz; ++i) { result[i] = new PalEntry(); }
            return result;
        }

        int last = arr.Length - 1;
        for (int i = 0; i < sz; ++i)
        {
            if (i > last || arr[i] is null)
            {
                result[i] = new PalEntry();
            }
            else
            {
                result[i] = arr[i];
            }
        }

        return result;
    }

    /// <summary>
    /// Returns a string representation of an entry
    /// suitable for a GPL file extension.
    /// </summary>
    /// <param name="pe">entry</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string</returns>
    public static string ToGplString(in PalEntry pe)
    {
        return PalEntry.ToGplString(new StringBuilder(
            16 + PalEntry.NameCharLimit), pe).ToString();
    }

    /// <summary>
    /// Appends a representation of an entry to a string builder,
    /// formatted for the GPL file extension.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="pe">entry</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToGplString(in StringBuilder sb, in PalEntry pe)
    {
        Clr c = pe.Color;
        int r = (int)(Utils.Clamp(c.r, 0.0f, 1.0f) * 255.0f + 0.5f);
        int g = (int)(Utils.Clamp(c.g, 0.0f, 1.0f) * 255.0f + 0.5f);
        int b = (int)(Utils.Clamp(c.b, 0.0f, 1.0f) * 255.0f + 0.5f);
        sb.Append(r.ToString().PadLeft(3, ' '));
        sb.Append(' ');
        sb.Append(g.ToString().PadLeft(3, ' '));
        sb.Append(' ');
        sb.Append(b.ToString().PadLeft(3, ' '));
        sb.Append(' ');
        sb.Append(pe.Name);
        return sb;
    }

    /// <summary>
    /// Returns a string representation of an entry
    /// suitable for a PAL file extension.
    /// </summary>
    /// <param name="pe">entry</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string</returns>
    public static string ToPalString(in PalEntry pe)
    {
        return PalEntry.ToPalString(new StringBuilder(16), pe).ToString();
    }

    /// <summary>
    /// Appends a representation of an entry to a string builder,
    /// formatted for the PAL file extension.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="pe">entry</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToPalString(in StringBuilder sb, in PalEntry pe)
    {
        Clr c = pe.Color;
        int r = (int)(0.5f + 255.0f * Utils.Clamp(c.r, 0.0f, 1.0f));
        int g = (int)(0.5f + 255.0f * Utils.Clamp(c.g, 0.0f, 1.0f));
        int b = (int)(0.5f + 255.0f * Utils.Clamp(c.b, 0.0f, 1.0f));
        sb.Append(r.ToString().PadLeft(3, ' '));
        sb.Append(' ');
        sb.Append(g.ToString().PadLeft(3, ' '));
        sb.Append(' ');
        sb.Append(b.ToString().PadLeft(3, ' '));
        return sb;
    }

    /// <summary>
    /// Returns a string representation of an entry.
    /// </summary>
    /// <param name="pe">entry</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string</returns>
    public static string ToString( //
        in PalEntry pe, //
        in int places = 4)
    {
        return PalEntry.ToString(new StringBuilder(128), pe, places).ToString();
    }

    /// <summary>
    /// Appends a representation of an entry to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="pe">entry</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString( // 
        in StringBuilder sb, //
        in PalEntry pe, // 
        in int places = 4)
    {
        sb.Append("{ color: ");
        Clr.ToString(sb, pe.Color, places);
        sb.Append(", name: \"");
        sb.Append(pe.Name);
        sb.Append('\"');
        sb.Append(' ');
        sb.Append('}');
        return sb;
    }
}