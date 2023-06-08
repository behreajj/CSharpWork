/// <summary>
/// Ordering for color channels.
/// </summary>
public enum ClrChannel : int
{
    // TODO: Should these separate the integer format for the Rgb
    // class from the Individual channels for the pixels methods?

    Alpha = 0,
    Blue = 1,
    Green = 2,
    Red = 3,
    Cyan = 4, // Green and blue, but not red.
    Magenta = 5, // Red and blue, but not green.
    Yellow = 6, // Red and green, but no blue.
    Lightness = 7,
    ABGR = 8,
    ARGB = 9,
    RGBA = 10
}