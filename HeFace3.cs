public class HeFace3
{
    protected HeEdge3 edge;

    public HeEdge3 Edge
    {
        get
        {
            return this.edge;
        }
    }

    public HeFace3 (HeEdge3 edge)
    {
        this.edge = edge;
    }
}