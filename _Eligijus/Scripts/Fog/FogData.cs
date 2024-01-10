using Godot;
using System;

public class FogData : IEquatable<FogData>, IComparable<FogData>
{
    public ChunkData chunkRef;
    public bool fogIsOnTile;
    public FogSidesData fogSidesData;

    
    public FogData(ChunkData chunkData)
    {
        chunkRef = chunkData;
        fogIsOnTile = chunkData.IsFogOnTile();
        
        fogSidesData = new FogSidesData();
    }
    
    public FogData(ChunkData chunkData, bool top, bool bottom, bool right, bool left)
    {
        chunkRef = chunkData;
        fogIsOnTile = chunkData.IsFogOnTile();
        fogSidesData = new FogSidesData(top, bottom, right, left);
    }
    
    public void SetFogTop(bool top)
    {
        fogSidesData.top = top;
    }
    
    public void SetFogBottom(bool bottom)
    {
        fogSidesData.bottom = bottom;
    }

    public void SetFogRight(bool right)
    {
        fogSidesData.right = right;
    }
    
    public void SetFogLeft(bool left)
    {
        fogSidesData.left = left;
    }
    
    public bool Equals(FogData other)
    {
        return ReferenceEquals(this.chunkRef, other.chunkRef) ? false : true;
    }

    public int CompareTo(FogData other)
    {
        return ReferenceEquals(this.chunkRef, other.chunkRef) ? 0 : 1;
    }
}