
public class FogSidesData
{
    public bool top = false;
    public bool bottom = false;
    public bool right = false;
    public bool left = false;

    public FogSidesData()
    {
        top = false;
        bottom = false;
        right = false;
        left = false;
    }

    public FogSidesData(bool top, bool bottom, bool right, bool left)
    {
        this.top = top;
        this.bottom = bottom;
        this.right = right;
        this.left = left;
    }
}