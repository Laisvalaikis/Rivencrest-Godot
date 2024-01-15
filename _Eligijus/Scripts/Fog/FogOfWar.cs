﻿using System;
using System.Collections.Generic;
using Godot;

public partial class FogOfWar : Sprite2D
{
    [Export] private int scale = 3;
    [Export] private int width = 3000;
    [Export] private int height = 3000;
    private Vector2 lightImage;
    private Vector2 lightOffset;
    private bool oneTime = false;
    private ShaderMaterial _shaderMaterial;
    public override void _Ready()
    {
        base._Ready();
        // lightImage.Resize(Mathf.CeilToInt((float)lightImage.GetWidth()/ (float)scale), Mathf.CeilToInt((float)lightImage.GetHeight()/(float)scale), Image.Interpolation.Nearest);
        lightImage =  new Vector2((float)100.0 / scale, (float)100.0 / scale);
        lightOffset = new Vector2((width / 2) - (lightImage.X / 2), (height / 2) - (lightImage.Y / 2));
        Color color = Colors.Black;
        color.A = 0;
        // Scale = Vector2.One;
        Scale *= scale;
        // lightImage.Convert(Image.Format.Rgbah);
        _shaderMaterial = (Material as ShaderMaterial);
        UpdateForImageTexture();
        // Scale *= scale;
        
        
    }
    
    public void AddFog(Vector2 position, Team characterTeam)
    {
        // Image image = characterTeam.fogTexture.GetImage();
        // Vector2 gridPosition = ToLocal(position);
        // Vector2I gridPositionI = new Vector2I(Mathf.RoundToInt(gridPosition.X), Mathf.FloorToInt(gridPosition.Y));
        // image.BlitRect(addFog, fogRectI, gridPositionI + fogOffset);
        // characterTeam.fogTexture.Update(image);
        GD.Print("Fix add back fog");
        
    }
    
    public void RemoveFog(Vector2 maxPosition, Vector2[] visionTiles, int lenght)
    {
        
        Vector2 squareSize = lightImage;
        squareSize = new Vector2(squareSize.X / (float)width, squareSize.Y / (float)height); // - new Vector2((float)0.0006661, (float)0.0006661);
        
        _shaderMaterial.SetShaderParameter("square_size", squareSize);
        _shaderMaterial.SetShaderParameter("fog_max_position", maxPosition);
        _shaderMaterial.SetShaderParameter("fog_position_array_size", lenght);
        _shaderMaterial.SetShaderParameter("fog_position_array", visionTiles);
        
    }
    
    public void UpdateCharacterPositions(Vector2[] characterPositions, int characterPositionsCount)
    {
        _shaderMaterial.SetShaderParameter("fog_player_position_array", characterPositions);
        _shaderMaterial.SetShaderParameter("fog_player_position_array_size", characterPositionsCount);
    }

    public Vector2 GenerateFogPosition(Vector2 position)
    {
        Vector2 gridPosition = ToLocal(position);
        Vector2 offsetImage = new Vector2(lightImage.X / 2, lightImage.Y / 2);
        Vector2 realPosition = gridPosition + lightOffset + offsetImage;
        Vector2 vec = new Vector2(realPosition.X / (float)width, realPosition.Y / (float)height); // smth is off with position calculation
        GD.Print(vec);
        return vec;
    }


    private double CalculateDistance2D(Vector2 start, Vector2 end)
    {
        return Math.Sqrt(Math.Pow(end.X - start.X, 2) + Math.Pow(end.Y - start.Y, 2));
    }
    
    private void UpdateForImageTexture()
    {
        Image fogImage = Image.Create(width, height, false, Image.Format.Rgbah);
        fogImage.Fill(Colors.Black);
        Texture =  ImageTexture.CreateFromImage(fogImage);
    }
}