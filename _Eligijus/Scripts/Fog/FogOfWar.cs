﻿using Godot;

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
        lightOffset = new Vector2I((width / 2) - (lightImage.GetWidth() / 2), (width / 2) - (lightImage.GetHeight() / 2));
        fogImage = Image.Create(width, height, false, Image.Format.Rgbah);
        fogImage.Fill(Colors.Black);
        lightImage.Convert(Image.Format.Rgbah);
        fogTexture = ImageTexture.CreateFromImage(fogImage);
        Texture = fogTexture;
    }

    public Image CreateFogImage()
    {
        fogImage = Image.Create(width, height, false, Image.Format.Rgbah);
        fogImage.Fill(Colors.Black);
        return fogImage;
    }

    public void SetFogImage(Image fog)
    {
        fogImage = fog;
        UpdateForImageTexture();
    }

    public void UpdateFog(Vector2 position)
    {
        Vector2 gridPosition = ToLocal(position);
        Vector2I gridPositionI = new Vector2I(Mathf.RoundToInt(gridPosition.X), Mathf.FloorToInt(gridPosition.Y)) ;
        Rect2I lightRectI = new Rect2I(Vector2I.Zero, new Vector2I(lightImage.GetWidth(), lightImage.GetHeight()));
        fogImage.BlendRect(lightImage, lightRectI, gridPositionI + lightOffset);
        UpdateForImageTexture();
    }
    
    public void UpdateFog(Vector2 position, Team characterTeam)
    {
        Vector2 gridPosition = ToLocal(position);
        Vector2I gridPositionI = new Vector2I(Mathf.RoundToInt(gridPosition.X), Mathf.FloorToInt(gridPosition.Y)) ;
        Rect2I lightRectI = new Rect2I(Vector2I.Zero, new Vector2I(lightImage.GetWidth(), lightImage.GetHeight()));
        characterTeam.fogImage.BlendRect(lightImage, lightRectI, gridPositionI + lightOffset);
        UpdateForImageTexture();
    }

    private void UpdateForImageTexture()
    {
        fogTexture = ImageTexture.CreateFromImage(fogImage);
        Texture = fogTexture;
    }
}