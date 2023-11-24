using Godot;

public class FogData
{
    public ChunkData chunkRef;
    public bool fogIsOnTile;

    public FogData(ChunkData chunkData)
    {
        chunkRef = chunkData;
        fogIsOnTile = chunkData.IsFogOnTile();
    }
}