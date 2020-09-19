public class HeEdge3
{
    protected HeFace3 face;
    protected HeEdge3 next;
    protected HeEdge3 pair;
    protected HeEdge3 prev;
    protected HeVert3 vert;

    public HeFace3 Face
    {
        get
        {
            return this.face;
        }
    }

    public HeEdge3 Next
    {
        get
        {
            return this.next;
        }
    }

    public HeEdge3 Pair
    {
        get
        {
            return this.pair;
        }
    }

    public HeEdge3 Prev
    {
        get
        {
            return this.prev;
        }
    }

    public HeVert3 Vert
    {
        get
        {
            return this.vert;
        }
    }

    public HeEdge3 (in HeVert3 vert, in HeEdge3 prev, in HeEdge3 next, in HeEdge3 pair, in HeFace3 face)
    {
        this.vert = vert;
        this.prev = prev;
        this.next = next;
        this.pair = pair;
        this.face = face;
    }
}