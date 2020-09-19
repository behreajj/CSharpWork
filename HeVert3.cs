public class HeVert3
{
    protected Vec3 coord;
    protected HeEdge3 edge;
    protected Vec3 normal;
    protected Vec2 texCoord;

    public Vec3 Coord
    {
        get
        {
            return this.coord;
        }
    }

    public HeEdge3 Edge
    {
        get
        {
            return this.edge;
        }
    }

    public Vec3 Normal
    {
        get
        {
            return this.normal;
        }
    }

    public Vec2 TexCoord
    {
        get
        {
            return this.texCoord;
        }
    }

    public HeVert3 (in Vec3 coord, in Vec2 texCoord, in Vec3 normal, in HeEdge3 edge)
    {
        this.coord = coord;
        this.texCoord = texCoord;
        this.normal = normal;
        this.edge = edge;
    }
}