using Godot;

public partial class FogOfWar : Sprite2D
{
    [Export] private Sprite2D fogSprite;
    [Export] private Image lightImage;
    [Export] private const int GridSize = 16;
    [Export] private int width = 10;
    [Export] private int height = 10;
    private Image fogImage = new Image();
    private ImageTexture fogTexture = new ImageTexture();

    public override void _Ready()
    {
        base._Ready();
        fogImage = Image.Create(1000, 1000, false, Image.Format.Rgbah);
        fogImage.Fill(Colors.Black);
        lightImage.Convert(Image.Format.Rgbah);
        fogSprite.Scale *= GridSize;
    }

    public void UpdateFog(Vector2I gridPosition)
    {
        Rect2I lightRectI = new Rect2I(Vector2I.Zero, new Vector2I(lightImage.GetWidth(), lightImage.GetHeight()));
        fogImage.BlendRect(lightImage, lightRectI, gridPosition);
    }

    public void UpdateForImageTexture()
    {
        // fogTexture.Create
    }
}