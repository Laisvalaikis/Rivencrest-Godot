using Godot;
using System;

public class FogData
{
    public ChunkData chunkRef;
    public bool fogIsOnTile;

    public FogData(ChunkData chunkData)
    {
        chunkRef = chunkData;
        fogIsOnTile = chunkData.IsFogOnTile();
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