using System;
using System.Collections.Generic;
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
    private bool oneTime = false;
    private ShaderMaterial _shaderMaterial;
    public override void _Ready()
    {
        base._Ready();
        // lightImage.Resize(Mathf.CeilToInt((float)lightImage.GetWidth()/ (float)scale), Mathf.CeilToInt((float)lightImage.GetHeight()/(float)scale), Image.Interpolation.Nearest);
        lightImage = Image.Create(Mathf.CeilToInt(100.0 / scale), Mathf.CeilToInt(100.0 / scale), false, Image.Format.Rgbah);
        lightOffset = new Vector2I((width / 2) - (lightImage.GetWidth() / 2), (height / 2) - (lightImage.GetHeight() / 2));
        addFog = Image.Create(lightImage.GetWidth(), lightImage.GetHeight(), false, Image.Format.Rgbah);
        fogRectI = new Rect2I(Vector2I.Zero, new Vector2I(addFog.GetWidth(), addFog.GetHeight()));
        fogOffset = new Vector2I((width / 2) - (addFog.GetWidth() / 2), (height / 2) - (addFog.GetHeight() / 2));
        addFog.Fill(Colors.Black);
        Color color = Colors.Black;
        color.A = 0;
        lightImage.Fill(color);
        // lightImage.Convert(Image.Format.Rgbah);
        Scale *= scale;
        _shaderMaterial = (Material as ShaderMaterial);
    }
    
    public ImageTexture CreateFogImageTexture()
    {
        fogImage = Image.Create(width, height, false, Image.Format.Rgbah);
        fogImage.Fill(Colors.Black);
        return ImageTexture.CreateFromImage(fogImage);
    }
    
    public void SetFogTexture(ImageTexture fog)
    {
        // oneTime = true;
        UpdateForImageTexture(fog);
    }
    
    public void AddFog(Vector2 position, Team characterTeam)
    {
        Image image = characterTeam.fogTexture.GetImage();
        Vector2 gridPosition = ToLocal(position);
        Vector2I gridPositionI = new Vector2I(Mathf.RoundToInt(gridPosition.X), Mathf.FloorToInt(gridPosition.Y));
        image.BlitRect(addFog, fogRectI, gridPositionI + fogOffset);
        characterTeam.fogTexture.Update(image);
    }
    
    public void RemoveFog(Vector2 maxPosition, Vector2[] visionTiles, int lenght)
    {
        
        Vector2 squareSize = lightImage.GetSize();
        squareSize = new Vector2(squareSize.X / (float)width, squareSize.Y / (float)height);
        
        _shaderMaterial.SetShaderParameter("square_size", squareSize);
        _shaderMaterial.SetShaderParameter("fog_max_position", maxPosition);
        _shaderMaterial.SetShaderParameter("fog_position_array_size", lenght);
        _shaderMaterial.SetShaderParameter("fog_position_array", visionTiles);
       
    }

    public void UpdateCharacterPositions(Vector2[] characterPositions, int characterPositionsCount)
    {
        _shaderMaterial.SetShaderParameter("fog_player_position_array", characterPositions);
        _shaderMaterial.SetShaderParameter("fog_player_position_array_size", characterPositionsCount);
        if (characterPositionsCount > 2)
        {
            GD.Print(characterPositions[2]);
        }
    }

    public Vector2 GenerateFogPosition(Vector2 position)
    {
        Vector2 gridPosition = ToLocal(position);
        Vector2 offsetImage = new Vector2(lightImage.GetWidth() / 2, lightImage.GetHeight() / 2);
        Vector2 realPosition = gridPosition + lightOffset + offsetImage;
        Vector2 vec = new Vector2(realPosition.X / (float)width, realPosition.Y / (float)height);
        return vec;
    }


    private double CalculateDistance2D(Vector2 start, Vector2 end)
    {
        return Math.Sqrt(Math.Pow(end.X - start.X, 2) + Math.Pow(end.Y - start.Y, 2));
    }
    
    private void UpdateForImageTexture(ImageTexture texture)
    {
        // fogTexture = ImageTexture.CreateFromImage(fogImage);
        Texture = texture;
    }
}