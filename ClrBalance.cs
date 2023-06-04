using System;

/// <summary>
/// Ordering for color balance presets.
/// </summary>
public enum ClrBalance : int
{
    Shadow = 1,
    Midtone = 2,
    Highlight = 4,
    Full = 1 | 2 | 4
}