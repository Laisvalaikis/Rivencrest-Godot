
public class FogSidesData
{
    public bool top;
    public bool bottom;
    public bool right;
    public bool left;

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

    public float GenerateFogSideData()
    {
        float data = 0;
        if (top)
        {
            data += 10000;
        }
        if (bottom)
        {
            data += 1000;
        }
        if (right)
        {
            data += 100;
        }
        if (left)
        {
            data += 10;
        }

        if (top && bottom && right && left)
        {
            data += 1;
        }

        return data;

    }
}