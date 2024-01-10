using Godot;
using System;

public class FogData
{
    public ChunkData chunkRef;
    public bool fogIsOnTile;
    public FogSidesData fogSidesData;

    
    public FogData(ChunkData chunkData)
    {
        chunkRef = chunkData;
        fogIsOnTile = chunkData.IsFogOnTile();
    }
    
    public FogData(ChunkData chunkData, bool top, bool bottom, bool right, bool left)
    {
        chunkRef = chunkData;
        fogIsOnTile = chunkData.IsFogOnTile();
        fogSidesData = new FogSidesData(top, bottom, right, left);
    }

    public override bool Equals(object obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (!(obj is FogData))
        {
            return false;
        }

        return this.chunkRef == ((FogData)obj).chunkRef;
    }

    public override int GetHashCode()
    {
        if (chunkRef is null)
        {
            return GetHashCode();
        }
        return this.chunkRef.GetHashCode();
    }
}