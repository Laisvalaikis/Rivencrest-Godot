using Godot;

public partial class FogOfWar : Sprite2D
{
    [Export] private Image lightImage;
    [Export] private int GridSize = 10;
    [Export] private int width = 2000;
    [Export] private int height = 2000;
    private Image fogImage;
    private ImageTexture fogTexture;
    private Vector2I lightOffset;
    public override void _Ready()
    {
        base._Ready();
        lightImage.Resize(100, 100, Image.Interpolation.Nearest);
        lightOffset = new Vector2I((width / 2) - (lightImage.GetWidth() / 2), (width / 2) - (lightImage.GetHeight() / 2));
        fogImage = Image.Create(width, height, false, Image.Format.Rgbah);
        fogImage.Fill(Colors.Black);
        lightImage.Convert(Image.Format.Rgbah);
        fogTexture = ImageTexture.CreateFromImage(fogImage);
        Texture = fogTexture;
    }

    public void UpdateFog(Vector2I gridPosition)
    {
        Rect2I lightRectI = new Rect2I(Vector2I.Zero, new Vector2I(lightImage.GetWidth(), lightImage.GetHeight()));
        fogImage.BlendRect(lightImage, lightRectI, gridPosition + lightOffset);
        UpdateForImageTexture();
    }

    public void UpdateForImageTexture()
    {
        fogTexture = ImageTexture.CreateFromImage(fogImage);
        Texture = fogTexture;
    }

    public override void _Input(InputEvent @event)
    {
        Vector2 position = GetGlobalMousePosition();
        position = ToLocal(position);
        Vector2I vector2I = new Vector2I(Mathf.RoundToInt(position.X), Mathf.FloorToInt(position.Y)) ;
        UpdateFog(vector2I);
    }
}