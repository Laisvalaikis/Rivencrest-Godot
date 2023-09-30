using System.Collections;
using Godot;
using Godot.Collections;

public partial class TileMapData : Resource
{
    [Export]
    public CollumBoundries _mapBoundries;
    [Export]
    public Vector2 _initialPosition;
    [Export]
    public float _chunkSize = 3f;
    [Export]
    public float _mapHeight = 10f;
    [Export]
    public float _mapWidth = 10f;
}

