/// <summary>
/// Holds enumerations for mesh UV profiles.
/// </summary>
public static class UvProfiles
{
    /// <summary>
    /// The UV profile for a capsule.
    /// </summary>
    public enum Capsule : int
    {
        Fixed = 0,
        Aspect = 1,
        Uniform = 2
    }

    /// <summary>
    /// The UV Profile for a rectangle.
    /// </summary>
    public enum Rect : int
    {
        Stretch = 0,
        Contain = 1,
        Cover = 2
    }
}