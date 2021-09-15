using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// A palette which holds a list of named colors.
/// Stores name and author of the palette.
/// </summary>
[Serializable]
public class Palette
{
    /// <summary>
    /// Associates a color with a name.
    /// </summary>
    [Serializable]
    public sealed class Entry : IComparable<Entry>, IEquatable<Entry>
    {
        /// <summary>
        /// Character limit for entry names.
        /// </summary>
        public const int NameCharLimit = 64;

        /// <summary>
        /// The entry's color.
        /// </summary>
        private Clr color;

        /// <summary>
        /// The entry's name.
        /// </summary>
        private string name;

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
                if (Clr.None (value))
                {
                    this.color = Clr.ClearBlack;
                }
                else
                {
                    this.color = value;
                }
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
                string trval = value.Trim ( );
                if (trval.Length > 0)
                {
                    this.name = trval.Substring (0,
                        Utils.Min (trval.Length, Entry.NameCharLimit));
                }
                else
                {
                    this.name = Clr.ToHexWeb (this.color);
                }
            }
        }

        /// <summary>
        /// Constructs an empty entry.
        /// </summary>
        public Entry ( )
        {
            this.Color = Clr.ClearBlack;
            this.Name = "Empty";
        }

        /// <summary>
        /// Constructs an entry from a color and a name.
        /// </summary>
        /// <param name="color">color</param>
        /// <param name="name">name</param>
        public Entry (in Clr color, in string name = "")
        {
            this.Color = color;
            this.Name = name;
        }

        /// <summary>
        /// Tests this entry for equivalence with an object.
        /// </summary>
        /// <param name="value">the object</param>
        /// <returns>the equivalence</returns>
        public override bool Equals (object value)
        {
            if (Object.ReferenceEquals (this, value)) return true;
            if (Object.ReferenceEquals (null, value)) return false;
            if (value is Palette.Entry) return this.Equals ((Palette.Entry) value);
            return false;
        }

        /// <summary>
        /// Returns a hash code for this entry based on its color.
        /// </summary>
        /// <returns>the hash code</returns>
        public override int GetHashCode ( )
        {
            if (Clr.None (this.color)) { return 0; }
            return this.color.GetHashCode ( );
        }

        /// <summary>
        /// Returns a string representation of this entry.
        /// </summary>
        /// <returns>the string</returns>
        public override string ToString ( )
        {
            return Entry.ToString (this);
        }

        /// <summary>
        /// Compares entries according to their colors.
        /// </summary>
        /// <param name="pe">entry</param>
        /// <returns>the comparison</returns>
        public int CompareTo (Entry pe)
        {
            if (pe == null) { return 1; }

            bool leftZeroAlpha = Clr.None (this.color);
            bool rightZeroAlpha = Clr.None (pe.color);
            if (leftZeroAlpha && rightZeroAlpha) { return 0; }
            if (leftZeroAlpha) { return -1; }
            if (rightZeroAlpha) { return 1; }

            int left = Clr.ToHexInt (this.color);
            int right = Clr.ToHexInt (pe.color);
            return (left < right) ? -1 : (left > right) ? 1 : 0;
        }

        /// <summary>
        /// Tests this entry for equivalence with another in compliance with the
        /// IEquatable interface.
        /// </summary>
        /// <param name="k">key</param>
        /// <returns>the evaluation</returns>
        public bool Equals (Entry pe)
        {
            return this.GetHashCode ( ) == pe.GetHashCode ( );
        }

        /// <summary>
        /// Sets an entry from a color and a name.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Entry Set (in Clr color, in string name)
        {
            this.Color = color;
            this.Name = name;
            return this;
        }

        /// <summary>
        /// Converrts a color to a palette entry.
        /// </summary>
        /// <param name="c">color</param>
        public static implicit operator Entry (in Clr c)
        {
            return new Entry (c, "");
        }

        /// <summary>
        /// Appends an entry to an array of entries.
        /// </summary>
        /// <param name="a">array</param>
        /// <param name="b">entry</param>
        /// <returns>array</returns>
        public static Entry[ ] Append (in Entry[ ] a, in Entry b)
        {
            bool aNull = a == null;
            bool bNull = b == null;
            if (aNull && bNull) { return new Entry[ ] { }; }
            if (bNull) { return a; }
            if (aNull) { return new Entry[ ] { b }; }

            int aLen = a.Length;
            Entry[ ] result = new Entry[aLen + 1];
            System.Array.Copy (a, 0, result, 0, aLen);
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
        public static Entry[ ] Insert (in Entry[ ] a, in int index, in Entry b)
        {
            bool aNull = a == null;
            bool bNull = b == null;
            if (aNull && bNull) { return new Entry[ ] { }; }
            if (bNull) { return a; }
            if (aNull) { return new Entry[ ] { b }; }

            int aLen = a.Length;
            int valIdx = Utils.Mod (index, aLen + 1);
            Entry[ ] result = new Entry[aLen + 1];
            System.Array.Copy (a, 0, result, 0, valIdx);
            result[valIdx] = b;
            System.Array.Copy (a, valIdx, result, valIdx + 1, aLen - valIdx);
            return result;
        }

        /// <summary>
        /// Prepends an entry to an array of entries.
        /// </summary>
        /// <param name="a">array</param>
        /// <param name="b">entry</param>
        /// <returns>array</returns>
        public static Entry[ ] Prepend (in Entry[ ] a, in Entry b)
        {
            bool aNull = a == null;
            bool bNull = b == null;
            if (aNull && bNull) { return new Entry[ ] { }; }
            if (bNull) { return a; }
            if (aNull) { return new Entry[ ] { b }; }

            int aLen = a.Length;
            Entry[ ] result = new Entry[aLen + 1];
            result[0] = b;
            System.Array.Copy (a, 0, result, 1, aLen);
            return result;
        }

        /// <summary>
        /// Removes an entry from from an array at an index.
        /// </summary>
        /// <param name="a">array</param>
        /// <param name="index">index</param>
        /// <returns>array</returns>
        public static (Entry[ ], Entry) RemoveAt (in Entry[ ] a, in int index)
        {
            bool aNull = a == null;
            if (aNull) { return (new Entry[ ] { }, null); }

            int aLen = a.Length;
            if (aLen < 1) { return (new Entry[ ] { }, null); }
            if (aLen < 2) { return (new Entry[ ] { }, a[0]); }

            int valIdx = Utils.Mod (index, aLen);
            Entry[ ] result = new Entry[aLen - 1];
            System.Array.Copy (a, 0, result, 0, valIdx);
            System.Array.Copy (a, valIdx + 1, result, valIdx, aLen - 1 - valIdx);
            return (result, a[valIdx]);
        }

        /// <summary>
        /// Resizes an array of entries.
        /// </summary>
        /// <param name="arr">array</param>
        /// <param name="sz">new size</param>
        /// <returns>resized array</returns>
        public static Entry[ ] Resize (in Entry[ ] arr, in int sz)
        {
            if (sz < 1) { return new Entry[ ] { }; }
            Entry[ ] result = new Entry[sz];

            if (arr == null)
            {
                for (int i = 0; i < sz; ++i) { result[i] = new Entry ( ); }
                return result;
            }

            int last = arr.Length - 1;
            for (int i = 0; i < sz; ++i)
            {
                if (i > last || arr[i] == null)
                {
                    result[i] = new Entry ( );
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
        public static string ToGplString (in Entry pe)
        {
            return Entry.ToGplString (new StringBuilder (
                16 + Entry.NameCharLimit), pe).ToString ( );
        }

        /// <summary>
        /// Appends a representation of an entry to a string builder,
        /// formatted for the GPL file extension.
        /// </summary>
        /// <param name="sb">string builder</param>
        /// <param name="pe">entry</param>
        /// <param name="places">number of decimal places</param>
        /// <returns>string builder</returns>
        public static StringBuilder ToGplString (in StringBuilder sb, in Entry pe)
        {
            Clr c = pe.Color;
            int r = (int) (Utils.Clamp (c.r, 0.0f, 1.0f) * 0xff + 0.5f);
            int g = (int) (Utils.Clamp (c.g, 0.0f, 1.0f) * 0xff + 0.5f);
            int b = (int) (Utils.Clamp (c.b, 0.0f, 1.0f) * 0xff + 0.5f);
            sb.Append (r.ToString ( ).PadLeft (3, ' '));
            sb.Append (' ');
            sb.Append (g.ToString ( ).PadLeft (3, ' '));
            sb.Append (' ');
            sb.Append (b.ToString ( ).PadLeft (3, ' '));
            sb.Append (' ');
            sb.Append (pe.Name);
            return sb;
        }

        /// <summary>
        /// Returns a string representation of an entry
        /// suitable for a PAL file extension.
        /// </summary>
        /// <param name="pe">entry</param>
        /// <param name="places">number of decimal places</param>
        /// <returns>string</returns>
        public static string ToPalString (in Entry pe)
        {
            return Entry.ToPalString (new StringBuilder (16), pe).ToString ( );
        }

        /// <summary>
        /// Appends a representation of an entry to a string builder,
        /// formatted for the PAL file extension.
        /// </summary>
        /// <param name="sb">string builder</param>
        /// <param name="pe">entry</param>
        /// <param name="places">number of decimal places</param>
        /// <returns>string builder</returns>
        public static StringBuilder ToPalString (in StringBuilder sb, in Entry pe)
        {
            Clr c = pe.Color;
            int r = (int) (Utils.Clamp (c.r, 0.0f, 1.0f) * 0xff + 0.5f);
            int g = (int) (Utils.Clamp (c.g, 0.0f, 1.0f) * 0xff + 0.5f);
            int b = (int) (Utils.Clamp (c.b, 0.0f, 1.0f) * 0xff + 0.5f);
            sb.Append (r.ToString ( ).PadLeft (3, ' '));
            sb.Append (' ');
            sb.Append (g.ToString ( ).PadLeft (3, ' '));
            sb.Append (' ');
            sb.Append (b.ToString ( ).PadLeft (3, ' '));
            return sb;
        }

        /// <summary>
        /// Returns a string representation of an entry.
        /// </summary>
        /// <param name="pe">entry</param>
        /// <param name="places">number of decimal places</param>
        /// <returns>string</returns>
        public static string ToString (in Entry pe, in int places = 4)
        {
            return Entry.ToString (new StringBuilder (128), pe, places).ToString ( );
        }

        /// <summary>
        /// Appends a representation of an entry to a string builder.
        /// </summary>
        /// <param name="sb">string builder</param>
        /// <param name="pe">entry</param>
        /// <param name="places">number of decimal places</param>
        /// <returns>string builder</returns>
        public static StringBuilder ToString (in StringBuilder sb, in Entry pe, in int places = 4)
        {
            sb.Append ("{ color: ");
            Clr.ToString (sb, pe.Color, places);
            sb.Append (", name: \"");
            sb.Append (pe.Name);
            sb.Append ('\"');
            sb.Append (' ');
            sb.Append ('}');
            return sb;
        }
    }

    /// <summary>
    /// Character limit for author name.
    /// </summary>
    public const int AuthorCharLimit = 96;

    /// <summary>
    /// Character limit for palette name.
    /// </summary>
    public const int NameCharLimit = 64;

    /// <summary>
    /// The palette's author.
    /// </summary>
    protected String author = "Anonymous";

    /// <summary>
    /// The array of entries in the palette.
    /// </summary>
    protected Entry[ ] entries = new Entry[0];

    /// <summary>
    /// The palette's name.
    /// </summary>
    protected String name = "Palette";

    /// <summary>
    /// The palette's author.
    /// </summary>
    /// <value>author</value>
    public string Author
    {
        get
        {
            return this.author;
        }

        set
        {
            string trval = value.Trim ( );
            if (trval.Length > 0)
            {
                this.author = trval.Substring (0,
                    Utils.Min (trval.Length, Palette.AuthorCharLimit));
            }
        }
    }

    /// <summary>
    /// Returns the number of entries in this palette.
    /// </summary>
    /// <value>the length</value>
    public int Length { get { return this.entries.Length; } }

    /// <summary>
    /// The palette's name.
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
            string trval = value.Trim ( );
            if (trval.Length > 0)
            {
                this.name = trval.Substring (0,
                    Utils.Min (trval.Length, Palette.NameCharLimit));
            }
        }
    }

    /// <summary>
    /// Constructs an empty default palette.
    /// </summary>
    /// <param name="name">name</param>
    /// <param name="author">author</param>
    public Palette (in string name = "Palette", in string author = "Anonymous")
    {
        // TODO: Implement a Palette.Tag inner class which has a name
        // and an int[] array which references indices in the palette.
        // Each palete will have a list/array of tags, which allow the
        // user to cluster together a palette into arbitrary groups.
        this.Name = name;
        this.Author = author;
    }

    /// <summary>
    /// Constructs a palette from an array of colors.
    /// </summary>
    /// <param name="name">name</param>
    /// <param name="author">author</param>
    /// <param name="colors">colors</param>
    public Palette (in string name, in string author, params Clr[ ] colors)
    {
        this.Name = name;
        this.Author = author;

        int len = colors.Length;
        this.entries = new Entry[len];
        for (int i = 0; i < len; ++i)
        {
            this.entries[i] = new Entry (colors[i], "");
        }
    }

    /// <summary>
    /// Returns a string representation of this palette.
    /// </summary>
    /// <returns>the string</returns>
    public override string ToString ( )
    {
        return Palette.ToString (this);
    }

    /// <summary>
    /// Appends a color to this palette.
    /// Optionally, allows the color to be named.
    /// </summary>
    /// <param name="color">color</param>
    /// <param name="name">name</param>
    /// <returns>this palette</returns>
    public Palette Append (in Clr color, in String name = "")
    {
        this.entries = Entry.Append (this.entries,
            new Entry (color, name));
        return this;
    }

    /// <summary>
    /// Gets the color of a palette entry at an index.
    /// </summary>
    /// <param name="i">index</param>
    /// <returns>color</returns>
    public Clr GetColor (in int i)
    {
        return this.entries[i].Color;
    }

    /// <summary>
    /// Gets the name of a palette entry at an index.
    /// </summary>
    /// <param name="i">index</param>
    /// <returns>name</returns>
    public string GetName (in int i)
    {
        return this.entries[i].Name;
    }

    /// <summary>
    /// Inserts a palette entry into this palette at an index.
    /// </summary>
    /// <param name="index">index</param>
    /// <param name="color">color</param>
    /// <param name="name">name</param>
    /// <returns>this palette</returns>
    public Palette Insert (in int index, in Clr color, in String name = "")
    {
        this.entries = Entry.Insert (this.entries, index,
            new Entry (color, name));
        return this;
    }

    /// <summary>
    /// Prepends a color to this palette.
    /// Optionally, allows the color to be named.
    /// </summary>
    /// <param name="color">color</param>
    /// <param name="name">name</param>
    /// <returns>this palette</returns>
    public Palette Prepend (in Clr color, in String name = "")
    {
        this.entries = Entry.Prepend (this.entries,
            new Entry (color, name));
        return this;
    }

    /// <summary>
    /// Removes a palette entry at an index.
    /// Returns the entry color if the removal
    /// was successful; otherwise, returns clear
    /// black.
    /// </summary>
    /// <param name="index">index</param>
    /// <returns>removed entry</returns>
    public Clr RemoveAt (in int index)
    {
        (Entry[ ], Entry) result = Entry.RemoveAt (this.entries, index);
        this.entries = result.Item1;
        if (result.Item2 != null)
        {
            return result.Item2.Color;
        }
        else
        {
            return Clr.ClearBlack;
        }
    }

    /// <summary>
    /// Sets the color of a palette entry at an index.
    /// </summary>
    /// <param name="i">index</param>
    /// <param name="color">color</param>
    public void SetColor (in int i, in Clr color)
    {
        this.entries[i].Color = color;
    }

    /// <summary>
    /// Sets the name of a palette entry at an index.
    /// </summary>
    /// <param name="i">index</param>
    /// <param name="name">name</param>
    public void SetName (in int i, in string name)
    {
        this.entries[i].Name = name;
    }

    /// <summary>
    /// Concatenates two palettes together.
    /// </summary>
    /// <param name="a">left palette</param>
    /// <param name="b">right palette</param>
    /// <param name="target">target palette</param>
    /// <returns>concatenation</returns>
    public static Palette Concat (in Palette a, in Palette b, in Palette target)
    {
        Entry[ ] aEntries = a.entries;
        Entry[ ] bEntries = b.entries;
        int aLen = aEntries.Length;
        int bLen = bEntries.Length;
        int cLen = aLen + bLen;
        target.entries = Entry.Resize (target.entries, cLen);
        Entry[ ] cEntries = target.entries;

        if (Object.ReferenceEquals (a, target))
        {
            for (int j = 0, k = aLen; j < bLen; ++j, ++k)
            {
                Entry bEntry = bEntries[j];
                cEntries[k].Set (bEntry.Color, bEntry.Name);
            }
        }
        else if (Object.ReferenceEquals (b, target))
        {
            // Shift right hand entries forward.
            for (int j = 0, k = aLen; j < bLen; ++j, ++k)
            {
                Entry temp = cEntries[j];
                cEntries[j] = cEntries[k];
                cEntries[k] = temp;
            }

            for (int i = 0; i < aLen; ++i)
            {
                Entry aEntry = aEntries[i];
                cEntries[i].Set (aEntry.Color, aEntry.Name);
            }
        }
        else
        {
            for (int i = 0; i < aLen; ++i)
            {
                Entry aEntry = aEntries[i];
                cEntries[i].Set (aEntry.Color, aEntry.Name);
            }

            for (int j = 0, k = aLen; j < bLen; ++j, ++k)
            {
                Entry bEntry = bEntries[j];
                cEntries[k].Set (bEntry.Color, bEntry.Name);
            }
        }

        return target;
    }

    /// <summary>
    /// Returns a palette with three primary colors -- red, green and blue --
    /// and three secondary colors -- yellow, cyan and magenta -- in the
    /// standard RGB color space.
    /// </summary>
    /// <param name="target">palette</param>
    /// <returns>palette</returns>
    public static Palette Rgb (in Palette target)
    {
        target.entries = Entry.Resize (target.entries, 6);
        Entry[ ] entries = target.entries;
        entries[0].Set (Clr.Red, "Red");
        entries[1].Set (Clr.Yellow, "Yellow");
        entries[2].Set (Clr.Green, "Green");
        entries[3].Set (Clr.Cyan, "Cyan");
        entries[4].Set (Clr.Blue, "Blue");
        entries[5].Set (Clr.Magenta, "Magenta");

        target.Name = "Rgb";
        target.Author = "Anonymous";
        return target;
    }

    /// <summary>
    /// Returns a subset of a palette.
    /// </summary>
    /// <param name="source">source palette</param>
    /// <param name="startIndex">start count</param>
    /// <param name="count">sample count</param>
    /// <param name="target">target palette</param>
    /// <returns>subset</returns>
    public static Palette Subset (in Palette source, in int startIndex, in int count, in Palette target)
    {
        int valCount = Utils.Max (1, count);

        Entry[ ] sourceEntries = source.entries;
        Entry[ ] subsetEntries = new Entry[valCount];
        int srcLen = sourceEntries.Length;
        for (int i = 0; i < valCount; ++i)
        {
            int k = Utils.Mod (startIndex + i, srcLen);
            subsetEntries[i] = sourceEntries[k];
        }

        if (Object.ReferenceEquals (source, target))
        {
            target.entries = subsetEntries;
        }
        else
        {
            target.entries = Entry.Resize (target.entries, valCount);
            for (int i = 0; i < valCount; ++i)
            {
                Entry subsetEntry = subsetEntries[i];
                target.entries[i].Set (subsetEntry.Color, subsetEntry.Name);
            }
        }

        return target;
    }

    /// <summary>
    /// Returns a representation of the palette as a
    /// GPL file string.
    /// </summary>
    /// <param name="pal">palette</param>
    /// <returns>string</returns>
    public static string ToGplString (in Palette pal)
    {
        return Palette.ToGplString (new StringBuilder (1024), pal).ToString ( );
    }

    /// <summary>
    /// Appends a representation of this palette as a
    /// GPL file to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="pal">palette</param>
    /// <param name="columns">display columns</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToGplString (in StringBuilder sb, in Palette pal, in int columns = 0)
    {
        sb.Append ("GIMP Palette\nName: ");
        sb.Append (pal.name);
        sb.Append ("\nColumns: ");
        sb.Append (columns);
        sb.Append ("\n# Author: ");
        sb.Append (pal.author);

        Entry[ ] entries = pal.entries;
        int len = entries.Length;
        sb.Append ("\n# Colors: ");
        sb.Append (len);

        for (int i = 0; i < len; ++i)
        {
            Entry entry = entries[i];
            if (entry != null)
            {
                sb.Append ('\n');
                Entry.ToGplString (sb, entry);
            }
        }
        return sb;
    }

    /// <summary>
    /// Returns a representation of the palette as a
    /// PAL file string.
    /// </summary>
    /// <param name="pal">palette</param>
    /// <returns>string</returns>
    public static string ToPalString (in Palette pal)
    {
        return Palette.ToPalString (new StringBuilder (1024), pal).ToString ( );
    }

    /// <summary>
    /// Appends a representation of this palette as a
    /// PAL file to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="pal">palette</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToPalString (in StringBuilder sb, in Palette pal)
    {
        Entry[ ] entries = pal.entries;
        int len = entries.Length;
        int last = len - 1;

        sb.Append ("JASC-PAL\n0100\n");
        sb.Append (len);
        for (int i = 0; i < len; ++i)
        {
            Entry entry = entries[i];
            if (entry != null)
            {
                sb.Append ('\n');
                Entry.ToPalString (sb, entry);
            }
        }
        return sb;
    }

    /// <summary>
    /// Returns a string representation of a palette.
    /// </summary>
    /// <param name="pal">palette</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string</returns>
    public static string ToString (in Palette pal, in int places = 4)
    {
        return Palette.ToString (new StringBuilder (1024), pal, places).ToString ( );
    }

    /// <summary>
    /// Appendsa a representation of a palette to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="pal">palette</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString (in StringBuilder sb, in Palette pal, in int places = 4)
    {
        sb.Append ("{ name: \"");
        sb.Append (pal.name);
        sb.Append ("\", author: \"");
        sb.Append (pal.author);

        sb.Append ("\", entries: [ ");
        Entry[ ] entries = pal.entries;
        int len = entries.Length;
        int last = len - 1;
        for (int i = 0; i < len; ++i)
        {
            Entry entry = entries[i];
            if (entry != null)
            {
                Entry.ToString (sb, entry, places);
                if (i < last)
                {
                    sb.Append (',');
                    sb.Append (' ');
                }
            }
        }
        sb.Append (" ] }");

        return sb;
    }

    /// <summary>
    /// Sets the target palette's entries to the unique elements
    /// of the source palette.
    /// </summary>
    /// <param name="source">source palette</param>
    /// <param name="target">target palette</param>
    /// <returns>target palette</returns>
    public static Palette Uniques (in Palette source, in Palette target)
    {
        Entry[ ] sourceEntries = source.entries;
        int len = sourceEntries.Length;
        int index = 0;
        Dictionary<Entry, int> clrDict = new Dictionary<Entry, int> (len);

        for (int i = 0; i < len; ++i)
        {
            Entry sourceEntry = sourceEntries[i];
            if (!clrDict.ContainsKey (sourceEntry))
            {
                clrDict.Add (sourceEntry, index);
                ++index;
            }
        }

        Entry[ ] uniqueEntries = new Entry[clrDict.Count];
        if (Object.ReferenceEquals (source, target))
        {
            foreach (KeyValuePair<Entry, int> kvp in clrDict)
            {
                uniqueEntries[kvp.Value] = kvp.Key;
            }
        }
        else
        {
            foreach (KeyValuePair<Entry, int> kvp in clrDict)
            {
                Entry uniqueEntry = kvp.Key;
                uniqueEntries[kvp.Value] = new Entry (
                    uniqueEntry.Color,
                    uniqueEntry.Name);
            }
        }

        target.entries = uniqueEntries;
        return target;
    }
}