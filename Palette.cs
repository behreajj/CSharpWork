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
    protected PalEntry[] entries = new PalEntry[0];

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
            string trval = value.Trim();
            if (trval.Length > 0)
            {
                this.author = trval[..Utils.Min(
                    trval.Length, Palette.AuthorCharLimit)];
            }
        }
    }

    /// <summary>
    /// Returns the number of entries in this palette.
    /// </summary>
    /// <value>length</value>
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
            string trval = value.Trim();
            if (trval.Length > 0)
            {
                this.name = trval[..Utils.Min(
                    trval.Length, Palette.NameCharLimit)];
            }
        }
    }

    /// <summary>
    /// Constructs an empty palette.
    /// </summary>
    public Palette()
    {
        // TODO: Reorganize this to represent a node graph?
    }

    /// <summary>
    /// Constructs a palette of a given length with a name and author.
    /// </summary>
    /// <param name="name">name</param>
    /// <param name="author">author</param>
    /// <param name="len">palette length</param>
    public Palette(
        in string name = "Palette",
        in string author = "Anonymous",
        in int len = 1)
    {
        this.Name = name;
        this.Author = author;

        int valLen = Utils.Max(1, len);
        this.entries = new PalEntry[valLen];
        for (int i = 0; i < valLen; ++i)
        {
            this.entries[i] = new PalEntry();
        }
    }

    /// <summary>
    /// Constructs a palette from an array of colors.
    /// </summary>
    /// <param name="name">name</param>
    /// <param name="author">author</param>
    /// <param name="colors">colors</param>
    public Palette(in string name, in string author, params Lab[] colors)
    {
        this.Name = name;
        this.Author = author;

        int len = colors.Length;
        this.entries = new PalEntry[len];
        for (int i = 0; i < len; ++i)
        {
            this.entries[i] = new PalEntry(colors[i], "");
        }
    }

    /// <summary>
    /// Returns a string representation of this palette.
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()
    {
        return Palette.ToString(this);
    }

    /// <summary>
    /// Appends a color to this palette.
    /// Optionally, allows the color to be named.
    /// </summary>
    /// <param name="color">color</param>
    /// <param name="name">name</param>
    /// <returns>this palette</returns>
    public Palette AppendColor(in Lab color, in String name = "")
    {
        this.entries = PalEntry.Append(this.entries,
            new PalEntry(color, name));
        return this;
    }

    /// <summary>
    /// Contracts this palette's size by the amount.
    /// </summary>
    /// <param name="v">amount</param>
    public void ContractBy(in int v)
    {
        this.Resize(this.entries.Length - v);
    }

    /// <summary>
    /// Expands this palette's size by the amount.
    /// </summary>
    /// <param name="v">amount</param>
    public void ExpandBy(in int v)
    {
        this.Resize(this.entries.Length + v);
    }

    /// <summary>
    /// Gets the color of a palette entry at an index.
    /// </summary>
    /// <param name="i">index</param>
    /// <returns>color</returns>
    public Lab GetColor(in int i)
    {
        return this.entries[i].Color;
    }

    /// <summary>
    /// Gets all the colors in the palette.
    /// </summary>
    /// <returns>colors</returns>
    public Lab[] GetColors()
    {
        int len = this.entries.Length;
        Lab[] result = new Lab[len];
        for (int i = 0; i < len; ++i)
        {
            result[i] = this.entries[i].Color;
        }
        return result;
    }

    /// <summary>
    /// Gets the enumerator for this curve.
    /// </summary>
    /// <returns>enumerator</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.entries.GetEnumerator();
    }

    /// <summary>
    /// Gets the name of a palette entry at an index.
    /// </summary>
    /// <param name="i">index</param>
    /// <returns>name</returns>
    public string GetName(in int i)
    {
        return this.entries[i].Name;
    }

    /// <summary>
    /// Gets all the names in the palette.
    /// </summary>
    /// <returns>colors</returns>
    public string[] GetNames()
    {
        int len = this.entries.Length;
        string[] result = new string[len];
        for (int i = 0; i < len; ++i)
        {
            result[i] = this.entries[i].Name;
        }
        return result;
    }

    /// <summary>
    /// Gets the index of a color in the palette, if the
    /// palette contains it. Otherwise, returns a default.
    /// </summary>
    /// <param name="c">color</param>
    /// <param name="df">default value</param>
    /// <returns>index</returns>
    public int IndexOf(in Lab c, in int df = -1)
    {
        int i = Array.IndexOf(this.entries, new PalEntry(c, ""));
        return i > -1 ? i : df;
    }

    /// <summary>
    /// Inserts a palette entry into this palette at an index.
    /// </summary>
    /// <param name="index">index</param>
    /// <param name="color">color</param>
    /// <param name="name">name</param>
    /// <returns>this palette</returns>
    public Palette InsertColor(in int index, in Lab color, in String name = "")
    {
        this.entries = PalEntry.Insert(this.entries, index,
            new PalEntry(color, name));
        return this;
    }

    /// <summary>
    /// Prepends a color to this palette.
    /// Optionally, allows the color to be named.
    /// </summary>
    /// <param name="color">color</param>
    /// <param name="name">name</param>
    /// <returns>this palette</returns>
    public Palette PrependColor(in Lab color, in String name = "")
    {
        this.entries = PalEntry.Prepend(this.entries,
            new PalEntry(color, name));
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
    public Lab RemoveColorAt(in int index)
    {
        (PalEntry[], PalEntry) result = PalEntry.RemoveAt(this.entries, index);
        this.entries = result.Item1;

        if (result.Item2 != null)
        {
            return result.Item2.Color;
        }
        else
        {
            return global::Lab.ClearBlack;
        }
    }

    /// <summary>
    /// Resizes the palette to the specified length.
    /// </summary>
    /// <param name="len">length</param>
    public void Resize(in int len)
    {
        this.entries = PalEntry.Resize(this.entries, len);
    }

    /// <summary>
    /// Sets the color of a palette entry at an index.
    /// </summary>
    /// <param name="i">index</param>
    /// <param name="color">color</param>
    public void SetColor(in int i, in Lab color)
    {
        this.entries[i].Color = color;
    }

    /// <summary>
    /// Sets the name of a palette entry at an index.
    /// </summary>
    /// <param name="i">index</param>
    /// <param name="name">name</param>
    public void SetName(in int i, in string name)
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
    public static Palette Concat(in Palette a, in Palette b, in Palette target)
    {
        PalEntry[] aEntries = a.entries;
        PalEntry[] bEntries = b.entries;
        int aLen = aEntries.Length;
        int bLen = bEntries.Length;
        int cLen = aLen + bLen;
        target.entries = PalEntry.Resize(target.entries, cLen);
        PalEntry[] cEntries = target.entries;

        if (Object.ReferenceEquals(a, target))
        {
            for (int j = 0, k = aLen; j < bLen; ++j, ++k)
            {
                PalEntry bPalEntry = bEntries[j];
                cEntries[k].Set(bPalEntry.Color, bPalEntry.Name);
            }
        }
        else if (Object.ReferenceEquals(b, target))
        {
            // Shift right hand entries forward.
            for (int j = 0, k = aLen; j < bLen; ++j, ++k)
            {
                (cEntries[k], cEntries[j]) = (cEntries[j], cEntries[k]);
            }

            for (int i = 0; i < aLen; ++i)
            {
                PalEntry aPalEntry = aEntries[i];
                cEntries[i].Set(aPalEntry.Color, aPalEntry.Name);
            }
        }
        else
        {
            for (int i = 0; i < aLen; ++i)
            {
                PalEntry aPalEntry = aEntries[i];
                cEntries[i].Set(aPalEntry.Color, aPalEntry.Name);
            }

            for (int j = 0, k = aLen; j < bLen; ++j, ++k)
            {
                PalEntry bPalEntry = bEntries[j];
                cEntries[k].Set(bPalEntry.Color, bPalEntry.Name);
            }
        }

        return target;
    }

    /// <summary>
    /// Parses a palette from a GPL string.
    /// </summary>
    /// <param name="source">source</param>
    /// <param name="target">target</param>
    /// <returns>palette</returns>
    public static Palette FromGplString(
        in string source,
        in Palette target)
    {
        string[] lines = source.Split(
                new string[] { "\r\n", "\r", "\n" },
                StringSplitOptions.RemoveEmptyEntries);
        return Palette.FromGplStrings(lines, target);
    }

    /// <summary>
    /// Parses a palette from an array of strings
    /// from the GPL palette format.
    /// </summary>
    /// <param name="lines">lines</param>
    /// <param name="target">target</param>
    /// <returns>palette</returns>
    public static Palette FromGplStrings(
        in string[] lines,
        in Palette target)
    {
        bool validHeader = false;
        bool useAseprite = false;
        int entryNameIdx = 3;
        string paletteName = "Palette";

        List<string> entryNames = new(32);
        List<Lab> colors = new(32);

        int lenLines = lines.Length;
        for (int i = 0; i < lenLines; ++i)
        {
            string line = lines[i].Trim();
            string lcLine = line.ToLower();

            if (!validHeader && lcLine.Equals("gimp palette"))
            {
                validHeader = true;
            }
            else if (!useAseprite && lcLine.Equals("channels: rgba"))
            {
                useAseprite = true;
                entryNameIdx = 4;
            }
            else if (lcLine.IndexOf('#') == 0)
            {
                continue;
            }
            else if (lcLine.IndexOf("columns:") > -1)
            {
                continue;
            }
            else if (lcLine.IndexOf("name:") > -1)
            {
                paletteName = line[(lcLine.IndexOf("name:") + 5)..].Trim();
            }
            else
            {
                string[] tokens = line.Split(
                    new char[] { ' ' },
                    StringSplitOptions.RemoveEmptyEntries);
                int lenTokens = tokens.Length;
                if (lenTokens > 2)
                {
                    int a255 = 255;
                    string entryName = "";

                    int.TryParse(tokens[0], out int r255);
                    int.TryParse(tokens[1], out int g255);
                    int.TryParse(tokens[2], out int b255);
                    if (useAseprite && lenTokens > 3)
                    {
                        int.TryParse(tokens[3], out a255);
                    }

                    if (lenTokens > entryNameIdx)
                    {
                        StringBuilder sb = new(32);
                        for (int j = entryNameIdx; j < lenTokens; ++j)
                        {
                            sb.Append(tokens[j]);
                        }
                        entryName = sb.ToString();
                    }

                    Rgb rgb = new(
                        r255 / 255.0f,
                        g255 / 255.0f,
                        b255 / 255.0f,
                        a255 / 255.0f);
                    Lab lab = Rgb.StandardToSrLab2(rgb);

                    entryNames.Add(entryName);
                    colors.Add(lab);
                }
            }
        }

        int lenColors = colors.Count;
        target.entries = PalEntry.Resize(target.entries, lenColors);
        PalEntry[] trgEntries = target.entries;
        for (int k = 0; k < lenColors; ++k)
        {
            PalEntry entry = trgEntries[k];
            entry.Set(colors[k], entryNames[k]);
        }

        target.Author = "Anonymous";
        target.Name = paletteName;
        return target;
    }

    /// <summary>
    /// Parses a palette from a PAL string.
    /// </summary>
    /// <param name="source">source</param>
    /// <param name="target">target</param>
    /// <returns>palette</returns>
    public static Palette FromPalString(
        in string source,
        in Palette target)
    {
        string[] lines = source.Split(
                new string[] { "\r\n", "\r", "\n" },
                StringSplitOptions.RemoveEmptyEntries);
        return Palette.FromPalStrings(lines, target);
    }

    /// <summary>
    /// Parses a palette from an array of strings
    /// from the PAL palette format.
    /// </summary>
    /// <param name="lines">lines</param>
    /// <param name="target">target</param>
    /// <returns>palette</returns>
    public static Palette FromPalStrings(
    in string[] lines,
    in Palette target)
    {
        bool validHeader = false;
        bool validCode = false;

        List<Lab> colors = new(32);

        int lenLines = lines.Length;
        for (int i = 0; i < lenLines; ++i)
        {
            string line = lines[i].Trim();
            string lcLine = line.ToLower();

            if (!validHeader && lcLine.Equals("jasc-pal"))
            {
                validHeader = true;
            }
            else if (!validCode && lcLine.Equals("0100"))
            {
                validCode = true;
            }
            else
            {
                // JASC Pal also contains a line that describes
                // the number of colors in the palette.
                string[] tokens = line.Split(
                    new char[] { ' ' },
                    StringSplitOptions.RemoveEmptyEntries);
                int lenTokens = tokens.Length;
                if (lenTokens > 2)
                {
                    int a255 = 255;

                    int.TryParse(tokens[0], out int r255);
                    int.TryParse(tokens[1], out int g255);
                    int.TryParse(tokens[2], out int b255);
                    if (lenTokens > 3)
                    {
                        int.TryParse(tokens[3], out a255);
                    }

                    Rgb rgb = new(
                        r255 / 255.0f,
                        g255 / 255.0f,
                        b255 / 255.0f,
                        a255 / 255.0f);
                    Lab lab = Rgb.StandardToSrLab2(rgb);

                    colors.Add(lab);
                }
            }
        }

        int lenColors = colors.Count;
        target.entries = PalEntry.Resize(target.entries, lenColors);
        PalEntry[] trgEntries = target.entries;
        for (int k = 0; k < lenColors; ++k)
        {
            PalEntry entry = trgEntries[k];
            entry.Set(colors[k], "");
        }

        target.Author = "Anonymous";
        target.Name = "Palette";
        return target;
    }

    /// <summary>
    /// Reverses the source palette.
    /// </summary>
    /// <param name="source">source palette</param>
    /// <param name="target">target palette</param>
    /// <returns>reversed palette</returns>
    public static Palette Reverse(in Palette source, in Palette target)
    {
        return Palette.Reverse(source, 0, source.entries.Length, target);
    }

    /// <summary>
    /// Reverses a subset of the source palette.
    /// </summary>
    /// <param name="source">source palette</param>
    /// <param name="index">start index</param>
    /// <param name="length">sample length</param>
    /// <param name="target">target palette</param>
    /// <returns>reversed palette</returns>
    public static Palette Reverse(
        in Palette source,
        in int index,
        in int length,
        in Palette target)
    {
        PalEntry[] sourceEntries = source.entries;
        int sourceLen = sourceEntries.Length;

        PalEntry[] reversedEntries = new PalEntry[sourceLen];
        System.Array.Copy(sourceEntries, 0, reversedEntries, 0, sourceLen);

        int valIdx = Utils.RemFloor(index, sourceLen);
        int valLen = Utils.Clamp(length, 1, sourceLen - valIdx);
        System.Array.Reverse(reversedEntries, valIdx, valLen);

        if (Object.ReferenceEquals(source, target))
        {
            target.entries = reversedEntries;
        }
        else
        {
            target.entries = PalEntry.Resize(target.entries, sourceLen);
            for (int i = 0; i < sourceLen; ++i)
            {
                PalEntry reversed = reversedEntries[i];
                target.entries[i].Set(reversed.Color, reversed.Name);
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
    public static Palette StandardRgb(in Palette target)
    {
        target.entries = PalEntry.Resize(target.entries, 6);
        PalEntry[] entries = target.entries;

        entries[0].Set(Lab.SrRed, "Red");
        entries[1].Set(Lab.SrYellow, "Yellow");
        entries[2].Set(Lab.SrGreen, "Green");
        entries[3].Set(Lab.SrCyan, "Cyan");
        entries[4].Set(Lab.SrBlue, "Blue");
        entries[5].Set(Lab.SrMagenta, "Magenta");

        target.Name = "Rgb";
        target.Author = "Anonymous";
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
    public static Palette Subset(
        in Palette source,
        in int index,
        in int length,
        in Palette target)
    {
        PalEntry[] sourceEntries = source.entries;
        int sourceLen = sourceEntries.Length;
        int valLen = Utils.Clamp(length, 1, sourceLen);
        PalEntry[] subsetEntries = new PalEntry[valLen];
        for (int i = 0; i < valLen; ++i)
        {
            int k = Utils.RemFloor(index + i, sourceLen);
            subsetEntries[i] = sourceEntries[k];
        }

        if (Object.ReferenceEquals(source, target))
        {
            target.entries = subsetEntries;
        }
        else
        {
            target.entries = PalEntry.Resize(target.entries, valLen);
            for (int i = 0; i < valLen; ++i)
            {
                PalEntry subsetPalEntry = subsetEntries[i];
                target.entries[i].Set(subsetPalEntry.Color, subsetPalEntry.Name);
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
    public static string ToGplString(in Palette p)
    {
        return Palette.ToGplString(
            new StringBuilder(1024), p, 0,
            (x) => Rgb.Clamp(x, 0.0f, 1.0f)).ToString();
    }

    /// <summary>
    /// Appends a representation of this palette as a
    /// GPL file to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="pal">palette</param>
    /// <param name="columns">display columns</param>
    /// <param name="tm">tone mapper</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToGplString(
        in StringBuilder sb,
        in Palette pal,
        in int columns,
        in Func<Rgb, Rgb> tm)
    {
        PalEntry[] entries = pal.entries;
        int len = entries.Length;
        int valCols = Utils.Clamp(columns, 0, len);

        sb.Append("GIMP Palette\nName: ");
        sb.Append(pal.Name);
        sb.Append("\nColumns: ");
        sb.Append(valCols);
        sb.Append("\n# Author: ");
        sb.Append(pal.Author);
        sb.Append("\n# Colors: ");
        sb.Append(len);

        for (int i = 0; i < len; ++i)
        {
            PalEntry entry = entries[i];
            if (entry != null)
            {
                sb.Append('\n');
                PalEntry.ToGplString(sb, entry, tm);
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
    public static string ToPalString(in Palette p)
    {
        return Palette.ToPalString(
            new StringBuilder(1024), p,
            (x) => Rgb.Clamp(x, 0.0f, 1.0f)).ToString();
    }

    /// <summary>
    /// Appends a representation of this palette as a
    /// PAL file to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="pal">palette</param>
    /// <param name="tm">tone mapper</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToPalString(
        in StringBuilder sb,
        in Palette pal,
        in Func<Rgb, Rgb> tm)
    {
        PalEntry[] entries = pal.entries;
        int len = entries.Length;

        sb.Append("JASC-PAL\n0100\n");
        sb.Append(len);
        for (int i = 0; i < len; ++i)
        {
            PalEntry entry = entries[i];
            if (entry != null)
            {
                sb.Append('\n');
                PalEntry.ToPalString(sb, entry, tm);
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
    public static string ToString(in Palette pal, in int places = 4)
    {
        return Palette.ToString(new StringBuilder(1024), pal, places).ToString();
    }

    /// <summary>
    /// Appendsa a representation of a palette to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="pal">palette</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString(in StringBuilder sb, in Palette pal, in int places = 4)
    {
        sb.Append("{\"name\":\"");
        sb.Append(pal.Name);
        sb.Append("\",\"author\":\"");
        sb.Append(pal.Author);

        sb.Append("\",\"entries\":[");
        PalEntry[] palEntries = pal.entries;
        int entriesLen = palEntries.Length;
        int entriesLast = entriesLen - 1;
        for (int i = 0; i < entriesLen; ++i)
        {
            PalEntry entry = palEntries[i];
            if (entry != null)
            {
                PalEntry.ToString(sb, entry, places);
                if (i < entriesLast)
                {
                    sb.Append(',');
                }
            }
        }

        sb.Append("]}");
        return sb;
    }

    /// <summary>
    /// Sets the target palette's entries to the unique elements
    /// of the source palette.
    /// </summary>
    /// <param name="source">source palette</param>
    /// <param name="target">target palette</param>
    /// <returns>target palette</returns>
    public static Palette Uniques(in Palette source, in Palette target)
    {
        // Create a dictionary where the original array index is
        // the value; the entry is the key.
        PalEntry[] sourceEntries = source.entries;
        int len = sourceEntries.Length;
        int index = 0;
        Dictionary<PalEntry, int> clrDict = new(len);
        for (int i = 0; i < len; ++i)
        {
            PalEntry sourcePalEntry = sourceEntries[i];
            if (!clrDict.ContainsKey(sourcePalEntry))
            {
                clrDict.Add(sourcePalEntry, index);
                ++index;
            }
        }

        // Convert from dictionary to a new array.
        PalEntry[] uniqueEntries = new PalEntry[clrDict.Count];
        if (Object.ReferenceEquals(source, target))
        {
            foreach (KeyValuePair<PalEntry, int> kvp in clrDict)
            {
                uniqueEntries[kvp.Value] = kvp.Key;
            }
        }
        else
        {
            foreach (KeyValuePair<PalEntry, int> kvp in clrDict)
            {
                PalEntry uniquePalEntry = kvp.Key;
                uniqueEntries[kvp.Value] = new PalEntry(
                    uniquePalEntry.Color,
                    uniquePalEntry.Name);
            }
        }

        target.entries = uniqueEntries;
        return target;
    }

    /// <summary>
    /// Returns a palette with 16 samples of the Viridis
    /// color palette, used for data visualization.
    /// </summary>
    /// <param name="target">palette</param>
    /// <returns>palette</returns>
    public static Palette Viridis(in Palette target)
    {
        target.entries = PalEntry.Resize(target.entries, 16);
        PalEntry[] entries = target.entries;

        entries[0].Set(new(14.838376f, 37.483573f, -32.474410f, 1.0f), "");
        entries[1].Set(new(20.629248f, 28.265625f, -38.358393f, 1.0f), "");
        entries[2].Set(new(25.940536f, 17.404003f, -40.681333f, 1.0f), "");
        entries[3].Set(new(31.577208f, 6.610862f, -38.518327f, 1.0f), "");

        entries[4].Set(new(36.796605f, -3.072040f, -33.312005f, 1.0f), "");
        entries[5].Set(new(41.360421f, -10.741949f, -26.773439f, 1.0f), "");
        entries[6].Set(new(46.519898f, -17.796277f, -19.405993f, 1.0f), "");
        entries[7].Set(new(51.442266f, -24.214719f, -11.350814f, 1.0f), "");

        entries[8].Set(new(56.387197f, -30.645903f, -2.271095f, 1.0f), "");
        entries[9].Set(new(61.436311f, -36.778321f, 8.441928f, 1.0f), "");
        entries[10].Set(new(66.307870f, -42.539711f, 21.860905f, 1.0f), "");
        entries[11].Set(new(71.241397f, -46.754003f, 36.834015f, 1.0f), "");

        entries[12].Set(new(76.504021f, -48.551795f, 54.343992f, 1.0f), "");
        entries[13].Set(new(81.285761f, -45.828649f, 70.595464f, 1.0f), "");
        entries[14].Set(new(86.244770f, -37.757474f, 82.085790f, 1.0f), "");
        entries[15].Set(new(91.094646f, -24.817274f, 86.018299f, 1.0f), "");

        target.Name = "Viridis";
        target.Author = "Stefan van der Walt, Nathaniel Smith";
        return target;
    }
}