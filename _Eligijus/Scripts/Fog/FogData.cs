using Godot;
using System;

public class FogData : IEquatable<FogData>, IComparable<FogData>
{
    public ChunkData chunkRef;
    public bool fogIsOnTile;

    
    public FogData(ChunkData chunkData)
    {
        chunkRef = chunkData;
        fogIsOnTile = chunkData.IsFogOnTile();
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