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
        // image.BlendRect(addFog, fogRectI, gridPositionI + fogOffset);
        image.BlitRect(addFog, fogRectI, gridPositionI + fogOffset);
        characterTeam.fogTexture.Update(image);
        // UpdateForImageTexture();
    }
    
    public void RemoveFog(List<FogData> visionTiles)
    {
       
        // Vector2 gridPosition = ToLocal(position);
        // Vector2I gridPositionI = new Vector2I(Mathf.RoundToInt(gridPosition.X), Mathf.FloorToInt(gridPosition.Y));
        // // if (!oneTime)
        // // {
        //     Vector2 offsetImage = new Vector2(lightImage.GetWidth() / 2, lightImage.GetHeight() / 2);
        //     Vector2 realPosition = gridPositionI + lightOffset + offsetImage;
        //     Vector2 vec = new Vector2(realPosition.X / (float)width, realPosition.Y / (float)height);
        //     _shaderMaterial.SetShaderParameter("fog_position", vec);
        //     GD.Print(vec);
            // oneTime = true;
        // }
        int fogTileCount = visionTiles.Count;
        Vector3[] fogDataArray = new Vector3[fogTileCount];
        for (int i = 0; i < fogTileCount; i++)
        {
            FogData fogData = visionTiles[i];
            Vector2 gridPosition = ToLocal(visionTiles[i].chunkRef.GetPosition());
            // Vector2I gridPositionI = new Vector2I(Mathf.RoundToInt(gridPosition.X), Mathf.FloorToInt(gridPosition.Y));
            // if (!oneTime)
            // {
            Vector2 offsetImage = new Vector2(lightImage.GetWidth() / 2, lightImage.GetHeight() / 2);
            Vector2 realPosition = gridPosition + lightOffset + offsetImage;
            Vector2 vec = new Vector2(realPosition.X / (float)width, realPosition.Y / (float)height);
            // _shaderMaterial.SetShaderParameter("fog_position", vec);
            fogDataArray[i] = new Vector3(vec.X, vec.Y, fogData.fogSidesData.GenerateFogSideData());
            // GD.Print(fogData.fogSidesData.GenerateFogSideData());
        }

        Vector2 squareSize = lightImage.GetSize();
        squareSize = new Vector2(squareSize.X / (float)width, squareSize.Y / (float)height);
        
        _shaderMaterial.SetShaderParameter("square_size", squareSize);
        _shaderMaterial.SetShaderParameter("fog_position_array_size", fogTileCount);
        _shaderMaterial.SetShaderParameter("fog_position_array", fogDataArray);

        if (fogDataArray.Length > 12)
        {
            // double distance = CalculateDistance2D(fogDataArray[0], fogDataArray[1]);
            double starting_point = 0;
            double current_point = CalculateDistance2D(fogDataArray[1], fogDataArray[0]);
            double last_point = CalculateDistance2D(fogDataArray[1], fogDataArray[6]);
            double alpha = ((current_point - starting_point) / (last_point - starting_point));
            GD.Print(starting_point);
        }
        // Image image = characterTeam.fogTexture.GetImage();
        // characterTeam.fogTexture.
        // Image img = Image.CreateFromData(width, height, false, Image.Format.Rgbah, image.GetData());
        // image.BlendRect(lightImage, fogRectI, gridPositionI + lightOffset);
        // image.BlitRect(lightImage, fogRectI, gridPositionI + lightOffset);
        // characterTeam.fogTexture.Update(img);
        // UpdateForImageTexture();
    }

    
    private double CalculateDistance2D(Vector3 start, Vector3 end)
    {
        return Math.Sqrt(Math.Pow(end.X - start.X, 2) + Math.Pow(end.Y - start.Y, 2));
    }
    
    private void UpdateForImageTexture(ImageTexture texture)
    {
        // fogTexture = ImageTexture.CreateFromImage(fogImage);
        Texture = texture;
    }
}