using System.Collections.Generic;
using Godot;

public partial class WallEntrap : BaseAction
{
    [Export] private ObjectData wallRockData;
    [Export] private Resource wallPrefab;
    private Object wall;
    private List<PlayerInformation> _playerInformations;

    public WallEntrap()
    {
    }

    public WallEntrap(WallEntrap ability): base(ability)
    {
        wallPrefab = ability.wallPrefab;
        wallRockData = ability.wallRockData;
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        WallEntrap ability = new WallEntrap((WallEntrap)action);
        return ability;
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
        base.ResolveAbility(chunk);
        SpawnAdjacentWalls(chunk);
        FinishAbility();
    }
    
    public override void OnTurnStart(ChunkData chunkData)
    {
        base.OnTurnStart(chunkData);
        if (_playerInformations != null && _playerInformations.Count > 0)
        {
            foreach (PlayerInformation x in _playerInformations)
            {
                x.DealDamage(1, player);
            }
        }
    }

    protected override void TryAddTile(ChunkData chunk)
    {
        if (chunk != null && !chunk.TileIsLocked())
        {
            _chunkList.Add(chunk);
        }
    }

    private void SpawnAdjacentWalls(ChunkData chunk)
    {
        (int x, int y) coordinates = chunk.GetIndexes();
        var directionVectors = new List<(int, int)>
        {
            (coordinates.x + 1, coordinates.y + 0),
            (coordinates.x + 0, coordinates.y + 1),
            (coordinates.x + (-1), coordinates.y + 0),
            (coordinates.x + 0, coordinates.y + (-1))
        };
        ChunkData[,] chunkDataArray = GameTileMap.Tilemap.GetChunksArray();
        foreach (var x in directionVectors)
        {
            if (GameTileMap.Tilemap.CheckBounds(x.Item1,x.Item2) && chunkDataArray[x.Item1,x.Item2].GetCurrentPlayer()==null)
            {
                ChunkData chunkData = chunkDataArray[x.Item1, x.Item2];
                PackedScene spawnResource = (PackedScene)wallPrefab;
                wall = spawnResource.Instantiate<Object>();
                player.GetTree().Root.CallDeferred("add_child", wall);
                wall.SetupObject(wallRockData);
                GameTileMap.Tilemap.SpawnObject(wall, chunkData);
            }
        }
    }
}
