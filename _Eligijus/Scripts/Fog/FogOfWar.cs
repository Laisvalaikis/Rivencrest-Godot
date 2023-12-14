using System;
using Godot;

public partial class FogOfWar : Sprite2D
{
    [Export] private Image lightImage;
    [Export] private int GridSize = 10;
    [Export] private int scale = 3;
    [Export] private int width = 3000;
    [Export] private int height = 3000;
    private Image fogImage;
    private Image addFog;
    private ImageTexture fogTexture;
    private Vector2I lightOffset;
    private Vector2I fogOffset;
    private Rect2I fogRectI;
    public override void _Ready()
    {
        base._Ready();
        lightImage.Resize(Mathf.CeilToInt((float)lightImage.GetWidth()/ (float)scale), Mathf.CeilToInt((float)lightImage.GetHeight()/(float)scale), Image.Interpolation.Nearest);
        lightOffset = new Vector2I((width / 2) - (lightImage.GetWidth() / 2), (height / 2) - (lightImage.GetHeight() / 2));
        addFog = Image.Create(lightImage.GetWidth(), lightImage.GetHeight(), false, Image.Format.Rgbah);
        fogRectI = new Rect2I(Vector2I.Zero, new Vector2I(addFog.GetWidth(), addFog.GetHeight()));
        fogOffset = new Vector2I((width / 2) - (addFog.GetWidth() / 2), (height / 2) - (addFog.GetHeight() / 2));
        addFog.Fill(Colors.Black);
        lightImage.Convert(Image.Format.Rgbah);
        Scale *= scale;
    }
    
    public ImageTexture CreateFogImageTexture()
    {
        fogImage = Image.Create(width, height, false, Image.Format.Rgbah);
        fogImage.Fill(Colors.Black);
        return ImageTexture.CreateFromImage(fogImage);
    }
    
    public void SetFogTexture(ImageTexture fog)
    {
        UpdateForImageTexture(fog);
    }
    
    public void AddFog(Vector2 position, Team characterTeam)
    {
        Image image = characterTeam.fogTexture.GetImage();
        Vector2 gridPosition = ToLocal(position);
        Vector2I gridPositionI = new Vector2I(Mathf.RoundToInt(gridPosition.X), Mathf.FloorToInt(gridPosition.Y)) ;
        // image.BlendRect(addFog, fogRectI, gridPositionI + fogOffset);
        image.BlitRect(addFog, fogRectI, gridPositionI + fogOffset);
        characterTeam.fogTexture.Update(image);
        // UpdateForImageTexture();
    }
    
    public void RemoveFog(Vector2 position, Team characterTeam)
    {
        Image image = characterTeam.fogTexture.GetImage();
        Vector2 gridPosition = ToLocal(position);
        Vector2I gridPositionI = new Vector2I(Mathf.RoundToInt(gridPosition.X), Mathf.FloorToInt(gridPosition.Y)) ;
        // image.BlendRect(lightImage, fogRectI, gridPositionI + lightOffset);
        image.BlitRect(lightImage, fogRectI, gridPositionI + lightOffset);
        characterTeam.fogTexture.Update(image);
        // UpdateForImageTexture();
    }

    private void UpdateForImageTexture(ImageTexture texture)
    {
        // fogTexture = ImageTexture.CreateFromImage(fogImage);
        Texture = texture;
    }
}