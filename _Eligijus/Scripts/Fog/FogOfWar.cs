using Godot;

public partial class FogOfWar : Sprite2D
{
    [Export] private Image lightImage;
    [Export] private int GridSize = 100;
    [Export] private int width = 10;
    [Export] private int height = 10;
    private Image fogImage = new Image();
    private ImageTexture fogTexture = new ImageTexture();
    private Vector2I lightOffset;
    public override void _Ready()
    {
        base._Ready();
        // lightImage.Resize(3, 3, Image.Interpolation.Nearest);
        lightOffset = new Vector2I(lightImage.GetWidth() / 2, lightImage.GetHeight() / 2);
        int fogImageWidth = 1000 / GridSize;
        var fogImageHeight = 1000 / GridSize;
        fogImage = Image.Create(fogImageWidth, fogImageHeight, false, Image.Format.Rgbah);
        fogImage.Fill(Colors.Black);
        lightImage.Convert(Image.Format.Rgbah);
        fogTexture = ImageTexture.CreateFromImage(fogImage);
        Texture = fogTexture;
        Scale *= GridSize;
    }

    public void UpdateFog(Vector2I gridPosition)
    {
        Rect2I lightRectI = new Rect2I(Vector2I.Zero, new Vector2I(lightImage.GetWidth(), lightImage.GetHeight()));
        fogImage.BlendRect(lightImage, lightRectI, gridPosition);
        UpdateForImageTexture();
    }

    public void UpdateForImageTexture()
    {
        // fogTexture.Create
        fogTexture = ImageTexture.CreateFromImage(fogImage);
        Texture = fogTexture;
    }

    public override void _Input(InputEvent @event)
    {
        Vector2 position = GetGlobalMousePosition();
        Vector2I vector2I = new Vector2I(Mathf.FloorToInt(position.X), Mathf.FloorToInt(position.Y)) ;
        UpdateFog(vector2I/GridSize);
    }
}