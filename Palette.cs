using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// A palette which holds a list of named colors.
/// Stores name and author of the palette.
/// </summary>
[Serializable]
public class Palette : IEnumerable
{
    /// <summary>
    /// Associates a color with a name.
    /// </summary>
    [Serializable]
    public class Entry : IComparable<Entry>, IEquatable<Entry>
    {
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
                this.color = value;
            }
        }

        /// <summary>
        /// The entry's name.
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
                if (trval.Length > 0) { this.name = trval; }
                else { this.name = Clr.ToHexWeb (this.color); }
            }
        }

        /// <summary>
        /// Constructs an entry from a color and a name.
        /// Defaults to using the color's web-friendly
        /// hexadecimal representation as a name.
        /// </summary>
        /// <param name="color">color</param>
        /// <param name="name">name</param>
        public Entry (Clr color = new Clr ( ), string name = "")
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
            return this.color.GetHashCode ( );
        }

        /// <summary>
        /// Compares entries according to their colors.
        /// </summary>
        /// <param name="pe">entry</param>
        /// <returns>the comparison</returns>
        public int CompareTo (Entry pe)
        {
            if (pe == null) { return 1; }
            int left = Clr.ToHexInt (this.color);
            int right = Clr.ToHexInt (pe.color);
            if (left == 0 && right != 0) { return -1; }
            if (right == 0 && left != 0) { return 1; }
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
        /// Returns a string representation of this entry.
        /// </summary>
        /// <returns>the string</returns>
        public override string ToString ( )
        {
            return Entry.ToString (this);
        }

        /// <summary>
        /// Sets an entry from a color and a name.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Entry Set (Clr color, string name)
        {
            this.Color = color;
            this.Name = name;
            return this;
        }

        /// <summary>
        /// Appends an element to an array of entries.
        /// </summary>
        /// <param name="a">array</param>
        /// <param name="b">element</param>
        /// <returns>array</returns>
        public static Entry[ ] Append (in Entry[ ] a, in Entry b)
        {
            bool aNull = a == null;
            bool bNull = b == null;
            if (aNull && bNull) { return new Entry[ ] { }; }
            if (aNull) { return new Entry[ ] { b }; }
            if (bNull) { return a; }
            int aLen = a.Length;
            Entry[ ] result = new Entry[aLen + 1];
            System.Array.Copy (a, 0, result, 0, aLen);
            result[aLen] = b;
            return result;
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
            return Entry.ToGplString (new StringBuilder (64), pe).ToString ( );
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
    /// The palette's author.
    /// </summary>
    protected String author;

    /// <summary>
    /// The array of entries in the palette.
    /// </summary>
    protected Entry[ ] entries = new Entry[0];

    /// <summary>
    /// The palette's name.
    /// </summary>
    protected String name;

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
            if (trval.Length > 0) this.author = trval;
        }
    }

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
            if (trval.Length > 0) this.name = trval;
        }
    }

    /// <summary>
    /// Returns the number of entries in this palette.
    /// </summary>
    /// <value>the length</value>
    public int Length { get { return this.entries.Length; } }

    /// <summary>
    /// Constructs a palette with no colors.
    /// </summary>
    /// <param name="name">name</param>
    /// <param name="author">author</param>
    public Palette (string name = "Palette", string author = "Anonymous")
    {
        this.Name = name;
        this.Author = author;
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
    /// Gets the enumerator for the entries of this palette.
    /// </summary>
    /// <returns>the enumerator</returns>
    public IEnumerator GetEnumerator ( )
    {
        return this.entries.GetEnumerator ( );
    }

    /// <summary>
    /// Retrieves a palette entry by index.
    /// </summary>
    /// <value>palette entry</value>
    public Entry this [int i]
    {
        get
        {
            return this.entries[Utils.Mod (i, this.entries.Length)];
        }

        set
        {
            if (value != null)
            {
                this.entries[Utils.Mod (i, this.entries.Length)] = value;
            }
        }
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
                    sb.Append (',').Append (' ');
                }
            }
        }
        sb.Append (" ] }");

        return sb;
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
    public static StringBuilder ToGplString (in StringBuilder sb, in Palette pal)
    {
        sb.Append ("GIMP Palette\nName: ");
        sb.Append (pal.name);
        sb.Append ("\nColumns: 0");
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
}