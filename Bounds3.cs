public struct Bounds3
{
    private readonly Vec3 max;
    private readonly Vec3 min;

    public Vec3 Max { get { return this.max; } }
    public Vec3 Min { get { return this.min; } }

    public Bounds3 (in float xMin = -0.5f, in float yMin = -0.5f, in float zMin = -0.5f, in float xMax = 0.5f, in float yMax = 0.5f, in float zMax = 0.5f)
    {
        this.min = new Vec3 (xMin, yMin, zMin);
        this.max = new Vec3 (xMax, yMax, zMax);
    }

    public Bounds3 (in Vec3 min, in Vec3 max)
    {
        this.min = min;
        this.max = max;
    }

    public override string ToString ( )
    {
        return this.ToString (4);
    }

    public string ToString (in int places = 4)
    {
        return new StringBuilder (128)
            .Append ("{ min: ")
            .Append (this.min.ToString (places))
            .Append (", max: ")
            .Append (this.max.ToString (places))
            .Append (" }")
            .ToString ( );
    }

    public static Vec3 Center (in Bounds3 b)
    {
        return Vec3.Mix (b.min, b.max);
    }

    public static Vec3 Extent (in Bounds3 b)
    {
        return Vec3.Diff (b.max, b.min);
    }

    public static Vec3 HalfExtent (in Bounds3 b)
    {
        return 0.5f * Bounds3.Extent (b);
    }
}