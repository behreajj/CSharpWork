using System;

/// <summary>
/// Ordering for tones preset used by Pixels methods.
/// </summary>
[Flags]
public enum Tone : int
{
    Shadow = 1,
    Midtone = 2,
    Highlight = 4
}