using System;

/// <summary>
/// Ordering for color balance presets.
/// </summary>
[Flags]
public enum ClrBalance : int
{
    Shadow = 1,
    Midtone = 2,
    Highlight = 4
}