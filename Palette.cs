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
    protected class Entry : IComparable<Entry>, IEquatable<Entry>
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
                this.color = Clr.None (value) ? Clr.ClearBlack : value;
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
            if (Object.ReferenceEquals (this, value)) { return true; }
            if (value is null) { return false; }
            if (value is Palette.Entry) { return this.Equals ((Palette.Entry) value); }
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
            if (pe is null) { return 1; }

            bool leftZeroAlpha = Clr.None (this.color);
            bool rightZeroAlpha = Clr.None (pe.color);
            if (leftZeroAlpha && rightZeroAlpha) { return 0; }
            if (leftZeroAlpha) { return -1; }
            if (rightZeroAlpha) { return 1; }

            int left = Clr.ToHexArgb (this.color);
            int right = Clr.ToHexArgb (pe.color);
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
            if (pe is null) { return false; }
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
            bool bNull = b is null;
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
            bool aNull = a is null;
            bool bNull = b is null;
            if (aNull && bNull) { return new Entry[ ] { }; }
            if (bNull) { return a; }
            if (aNull) { return new Entry[ ] { b }; }

            int aLen = a.Length;
            int valIdx = Utils.RemFloor (index, aLen + 1);
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
            bool aNull = a is null;
            bool bNull = b is null;
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

            int valIdx = Utils.RemFloor (index, aLen);
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
                if (i > last || arr[i] is null)
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
            int r = (int) (0.5f + Utils.Clamp (c.r, 0.0f, 1.0f) * 0xff);
            int g = (int) (0.5f + Utils.Clamp (c.g, 0.0f, 1.0f) * 0xff);
            int b = (int) (0.5f + Utils.Clamp (c.b, 0.0f, 1.0f) * 0xff);
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
    /// Associates palette entries together under a name.
    /// </summary>
    [Serializable]
    protected class Tag : IComparable<Tag>, IEquatable<Tag>
    {
        /// <summary>
        /// Character limit for entry names.
        /// </summary>
        public const int NameCharLimit = 64;

        /// <summary>
        /// List of entries under the tag.
        /// </summary>
        protected List<Entry> entries = new List<Entry> ( );

        /// <summary>
        /// The tag's name.
        /// </summary>
        protected string name = "Tag";

        /// <summary>
        /// Gets the list of entries.
        /// </summary>
        /// <value>entries</value>
        public List<Entry> Entries
        {
            get
            {
                return this.entries;
            }
        }

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
                        Utils.Min (trval.Length, Tag.NameCharLimit));
                }
            }
        }

        // Constructs an empty tag object.
        public Tag ( ) { }

        // Constructs a tag from a name.
        public Tag (in string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Tests this tag for equivalence with an object.
        /// </summary>
        /// <param name="value">the object</param>
        /// <returns>the equivalence</returns>
        public override bool Equals (object value)
        {
            if (Object.ReferenceEquals (this, value)) { return true; }
            if (value is null) { return false; }
            if (value is Palette.Tag) { return this.Equals ((Palette.Tag) value); }
            return false;
        }

        /// <summary>
        /// Returns a hash code for this tag based on its name.
        /// </summary>
        /// <returns>the hash code</returns>
        public override int GetHashCode ( )
        {
            return this.Name.ToLower ( ).GetHashCode ( );
        }

        /// <summary>
        /// Returns a string representation of this tag.
        /// </summary>
        /// <returns>the string</returns>
        public override string ToString ( )
        {
            return this.Name.ToString ( );
        }

        /// <summary>
        /// Compares tags according to their names.
        /// </summary>
        /// <param name="pe">entry</param>
        /// <returns>the comparison</returns>
        public int CompareTo (Tag tag)
        {
            if (tag is null) { return 1; }
            return this.Name.ToLower ( ).CompareTo (tag.Name.ToLower ( ));
        }

        /// <summary>
        /// Tests this tag for equivalence with another in compliance with the
        /// IEquatable interface.
        /// </summary>
        /// <param name="k">key</param>
        /// <returns>the evaluation</returns>
        public bool Equals (Tag tag)
        {
            if (tag is null) { return false; }
            return this.GetHashCode ( ) == tag.GetHashCode ( );
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
    /// A list of tags.
    /// </summary>
    protected List<Tag> tags = new List<Tag> ( );

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
    /// Constructs an empty palette.
    /// </summary>
    public Palette ( ) { }

    /// <summary>
    /// Constructs a palette of a given length with a name and author.
    /// </summary>
    /// <param name="name">name</param>
    /// <param name="author">author</param>
    /// <param name="len">palette length</param>
    public Palette (in string name = "Palette", in string author = "Anonymous", in int len = 1)
    {
        this.Name = name;
        this.Author = author;

        int valLen = Utils.Max (1, len);
        this.entries = new Entry[valLen];
        for (int i = 0; i < valLen; ++i)
        {
            this.entries[i] = new Entry ( );
        }
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
    public Palette AppendColor (in Clr color, in String name = "")
    {
        this.entries = Entry.Append (this.entries,
            new Entry (color, name));
        return this;
    }

    /// <summary>
    /// Appends a tag from a name and an array
    /// of palette entry indices.
    /// </summary>
    /// <param name="name">tag name</param>
    /// <param name="indices">tag indices</param>
    public void AppendTag (in string name, params int[ ] indices)
    {
        this.InsertTag (this.tags.Count, name, indices);
    }

    /// <summary>
    /// Contracts this palette's size by the amount.
    /// </summary>
    /// <param name="v">amount</param>
    public void ContractBy (in int v)
    {
        this.Resize (this.entries.Length - v);
    }

    /// <summary>
    /// Expands this palette's size by the amount.
    /// </summary>
    /// <param name="v">amount</param>
    public void ExpandBy (in int v)
    {
        this.Resize (this.entries.Length + v);
    }

    /// <summary>
    /// Clears the palette's tags.
    /// </summary>
    public void ClearTags ( )
    {
        this.tags.Clear ( );
    }

    /// <summary>
    /// Gets all the colors in the palette.
    /// </summary>
    /// <returns>colors</returns>
    public Clr[ ] GetColors ( )
    {
        int len = this.entries.Length;
        Clr[ ] result = new Clr[len];
        for (int i = 0; i < len; ++i)
        {
            result[i] = this.entries[i].Color;
        }
        return result;
    }

    /// <summary>
    /// Gets the color of a palette entry at an index.
    /// </summary>
    /// <param name="i">index</param>
    /// <returns>color</returns>
    public Clr GetEntryColor (in int i)
    {
        return this.entries[i].Color;
    }

    /// <summary>
    /// Gets the name of a palette entry at an index.
    /// </summary>
    /// <param name="i">index</param>
    /// <returns>name</returns>
    public string GetEntryName (in int i)
    {
        return this.entries[i].Name;
    }

    /// <summary>
    /// Gets an array of colors associated with a tag.
    /// </summary>
    /// <param name="i">index</param>
    /// <returns>colors</returns>
    public Clr[ ] GetTagColors (in int i)
    {
        Tag tag = this.tags[i];
        List<Entry> tagEntries = tag.Entries;
        int len = tagEntries.Count;
        Clr[ ] result = new Clr[len];
        for (int j = 0; j < len; ++j)
        {
            result[j] = tagEntries[j].Color;
        }
        return result;
    }

    /// <summary>
    /// Gets the palette entry indices associated with a tag.
    /// </summary>
    /// <param name="i">tag index</param>
    /// <returns>indices</returns>
    public int[ ] GetTagIndices (in int i)
    {
        // TODO: Test
        Tag tag = this.tags[i];
        List<Entry> tagEntries = tag.Entries;
        int len = tagEntries.Count;
        List<int> resultList = new List<int> ( );
        for (int j = 0; j < len; ++j)
        {
            Entry tagEntry = tagEntries[j];
            int index = Array.IndexOf (this.entries, tagEntry);
            if (index > -1)
            {
                resultList.Add (index);
            }
        }
        resultList.Sort ( );
        return resultList.ToArray ( );
    }

    /// <summary>
    /// Gets the name of a palette tag at an index.
    /// </summary>
    /// <param name="i">index</param>
    /// <returns>name</returns>
    public string GetTagName (in int i)
    {
        return this.tags[i].Name;
    }

    /// <summary>
    /// Gets the index of a color in the palette, if the
    /// palette contains it. Otherwise, returns a default.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="df">default value</param>
    /// <returns>index</returns>
    public int IndexOf (in Clr c, in int df = -1)
    {
        int i = Array.IndexOf (this.entries, new Entry (c, ""));
        return i > -1 ? i : df;
    }

    /// <summary>
    /// Inserts a palette entry into this palette at an index.
    /// </summary>
    /// <param name="index">index</param>
    /// <param name="color">color</param>
    /// <param name="name">name</param>
    /// <returns>this palette</returns>
    public Palette InsertColor (in int index, in Clr color, in String name = "")
    {
        this.entries = Entry.Insert (this.entries, index,
            new Entry (color, name));
        return this;
    }

    /// <summary>
    /// Creates a tag at an insertion index from a name and an array
    /// of palette entry indices.
    /// </summary>
    /// <param name="i">insertion index</param>
    /// <param name="name">tag name</param>
    /// <param name="indices">tag indices</param>
    public void InsertTag (in int i, in string name, params int[ ] indices)
    {
        Tag tag = new Tag (name);
        int j = Utils.RemFloor (i, this.tags.Count + 1);
        this.tags.Insert (j, tag);
        this.SetTagIndices (j, indices);
    }

    /// <summary>
    /// Prepends a color to this palette.
    /// Optionally, allows the color to be named.
    /// </summary>
    /// <param name="color">color</param>
    /// <param name="name">name</param>
    /// <returns>this palette</returns>
    public Palette PrependColor (in Clr color, in String name = "")
    {
        this.entries = Entry.Prepend (this.entries,
            new Entry (color, name));
        return this;
    }

    /// <summary>
    /// Prepends a tag from a name and an array
    /// of palette entry indices.
    /// </summary>
    /// <param name="name">tag name</param>
    /// <param name="indices">tag indices</param>
    public void PrependTag (in string name, params int[ ] indices)
    {
        this.InsertTag (0, name, indices);
    }

    /// <summary>
    /// Removes a palette entry at an index.
    /// Returns the entry color if the removal
    /// was successful; otherwise, returns clear
    /// black.
    /// </summary>
    /// <param name="index">index</param>
    /// <returns>removed entry</returns>
    public Clr RemoveColorAt (in int index)
    {
        (Entry[ ], Entry) result = Entry.RemoveAt (this.entries, index);
        this.entries = result.Item1;
        this.TrimTagEntries ( );

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
    /// Removes a tag at an index.
    /// </summary>
    /// <param name="index">index</param>
    public void RemoveTagAt (in int index)
    {
        if (index > -1 && index < tags.Count)
        {
            this.tags.RemoveAt (index);
        }
    }

    /// <summary>
    /// Resizes the palette to the specified length.
    /// </summary>
    /// <param name="len">length</param>
    public void Resize (in int len)
    {
        this.entries = Entry.Resize (this.entries, len);
        this.TrimTagEntries ( );
    }

    /// <summary>
    /// Sets the color of a palette entry at an index.
    /// </summary>
    /// <param name="i">index</param>
    /// <param name="color">color</param>
    public void SetEntryColor (in int i, in Clr color)
    {
        this.entries[i].Color = color;
    }

    /// <summary>
    /// Sets the name of a palette entry at an index.
    /// </summary>
    /// <param name="i">index</param>
    /// <param name="name">name</param>
    public void SetEntryName (in int i, in string name)
    {
        this.entries[i].Name = name;
    }

    /// <summary>
    /// Sets the palette entry indices associated with a tag.
    /// If an index in the array is out-of-bounds, it is not
    /// added to the tag.
    /// </summary>
    /// <param name="i">tag index</param>
    /// <param name="indices">indices</param>
    public void SetTagIndices (in int i, params int[ ] indices)
    {
        Tag tag = this.tags[i];
        List<Entry> tagEntries = tag.Entries;
        tagEntries.Clear ( );
        int indicesLen = indices.Length;
        int palEntriesLen = this.entries.Length;
        for (int j = 0; j < indicesLen; ++j)
        {
            int index = indices[j];
            if (index > -1 && index < palEntriesLen)
            {
                Entry entry = this.entries[index];
                tagEntries.Add (entry);
            }
        }
        tagEntries.Sort ( );
    }

    /// <summary>
    /// Sets the name of a palette tag at an index.
    /// </summary>
    /// <param name="i">index</param>
    /// <param name="name">name</param>
    public void SetTagName (in int i, in string name)
    {
        this.tags[i].Name = name;
    }

    /// <summary>
    /// Iterates over all the tags in a palette, removing
    /// entries from each tag that is no longer present
    /// in the palette.
    /// </summary>
    public void TrimTagEntries ( )
    {
        int tagsLen = this.tags.Count;
        for (int i = 0; i < tagsLen; ++i)
        {
            Tag tag = this.tags[i];
            List<Entry> tagEntries = tag.Entries;
            int tagEntriesLen = tagEntries.Count;
            for (int j = tagEntriesLen - 1; j > -1; --j)
            {
                Entry tagEntry = tagEntries[j];
                if (Array.IndexOf (this.entries, tagEntry) < 0)
                {
                    tagEntries.Remove (tagEntry);
                }
            }
        }
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

        target.TrimTagEntries ( );
        return target;
    }

    /// <summary>
    /// Reverses the source palette.
    /// </summary>
    /// <param name="source">source palette</param>
    /// <param name="target">target palette</param>
    /// <returns>reversed palette</returns>
    public static Palette Reverse (in Palette source, in Palette target)
    {
        return Palette.Reverse (source, 0, source.entries.Length, target);
    }

    /// <summary>
    /// Reverses a subset of the source palette.
    /// </summary>
    /// <param name="source">source palette</param>
    /// <param name="index">start index</param>
    /// <param name="length">sample length</param>
    /// <param name="target">target palette</param>
    /// <returns>reversed palette</returns>
    public static Palette Reverse (in Palette source, in int index, in int length, in Palette target)
    {
        Entry[ ] sourceEntries = source.entries;
        int sourceLen = sourceEntries.Length;

        Entry[ ] reversedEntries = new Entry[sourceLen];
        System.Array.Copy (sourceEntries, 0, reversedEntries, 0, sourceLen);

        int valIdx = Utils.RemFloor (index, sourceLen);
        int valLen = Utils.Clamp (length, 1, sourceLen - valIdx);
        System.Array.Reverse (reversedEntries, valIdx, valLen);

        if (Object.ReferenceEquals (source, target))
        {
            target.entries = reversedEntries;
        }
        else
        {
            target.entries = Entry.Resize (target.entries, sourceLen);
            for (int i = 0; i < sourceLen; ++i)
            {
                Entry reversed = reversedEntries[i];
                target.entries[i].Set (reversed.Color, reversed.Name);
            }
        }

        target.TrimTagEntries ( );
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
        target.ClearTags ( );
        return target;
    }

    /// <summary>
    /// Returns a subset of a palette.
    /// </summary>
    /// <param name="source">source palette</param>
    /// <param name="index">start index</param>
    /// <param name="length">sample length</param>
    /// <param name="target">target palette</param>
    /// <returns>subset</returns>
    public static Palette Subset (in Palette source, in int index, in int length, in Palette target)
    {
        Entry[ ] sourceEntries = source.entries;
        int sourceLen = sourceEntries.Length;
        int valLen = Utils.Clamp (length, 1, sourceLen);
        Entry[ ] subsetEntries = new Entry[valLen];
        for (int i = 0; i < valLen; ++i)
        {
            int k = Utils.RemFloor (index + i, sourceLen);
            subsetEntries[i] = sourceEntries[k];
        }

        if (Object.ReferenceEquals (source, target))
        {
            target.entries = subsetEntries;
        }
        else
        {
            target.entries = Entry.Resize (target.entries, valLen);
            for (int i = 0; i < valLen; ++i)
            {
                Entry subsetEntry = subsetEntries[i];
                target.entries[i].Set (subsetEntry.Color, subsetEntry.Name);
            }
        }

        target.TrimTagEntries ( );
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
        Entry[ ] entries = pal.entries;
        int len = entries.Length;
        int valCols = Utils.Clamp (columns, 0, len);

        sb.Append ("GIMP Palette\nName: ");
        sb.Append (pal.Name);
        sb.Append ("\nColumns: ");
        sb.Append (valCols);
        sb.Append ("\n# Author: ");
        sb.Append (pal.Author);
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
        sb.Append (pal.Name);
        sb.Append ("\", author: \"");
        sb.Append (pal.Author);

        sb.Append ("\", entries: [ ");
        Entry[ ] palEntries = pal.entries;
        int entriesLen = palEntries.Length;
        int entriesLast = entriesLen - 1;
        for (int i = 0; i < entriesLen; ++i)
        {
            Entry entry = palEntries[i];
            if (entry != null)
            {
                Entry.ToString (sb, entry, places);
                if (i < entriesLast)
                {
                    sb.Append (',').Append (' ');
                }
            }
        }

        sb.Append (" ], tags: [ ");
        List<Tag> tags = pal.tags;
        int tagsLen = tags.Count;
        int tagsLast = tagsLen - 1;
        for (int j = 0; j < tagsLen; ++j)
        {
            Tag tag = tags[j];
            if (tag != null)
            {
                sb.Append ("{ name: \"");
                sb.Append (tag.Name);
                sb.Append ("\", indices: [ ");
                List<Entry> tagEntries = tag.Entries;
                int tagEntriesLen = tagEntries.Count;
                int tagEntriesLast = tagEntriesLen - 1;
                for (int k = 0; k < tagEntriesLen; ++k)
                {
                    Entry tagEntry = tagEntries[k];
                    if (tagEntry != null)
                    {
                        sb.Append (Array.IndexOf (palEntries, tagEntry));
                        if (k < tagEntriesLast)
                        {
                            sb.Append (',').Append (' ');
                        }
                    }
                }

                sb.Append (" ] }");
                if (j < tagsLast)
                {
                    sb.Append (',').Append (' ');
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
        // Create a dictionary where the original array index is
        // the value; the entry is the key.
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

        // Convert from dictionary to a new array.
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
        target.TrimTagEntries ( );
        return target;
    }
}