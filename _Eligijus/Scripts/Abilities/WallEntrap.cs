using System.Collections.Generic;
using Godot;

public partial class WallEntrap : BaseAction
{
    [Export] private Resource wallPrefab;
    private List<PlayerInformation> _playerInformations;

    public WallEntrap()
    {
    }

    public WallEntrap(WallEntrap ability): base(ability)
    {
        wallPrefab = ability.wallPrefab;
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        WallEntrap ability = new WallEntrap((WallEntrap)action);
        return ability;
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        SpawnAdjacentWalls();
        FinishAbility();
    }
    
    public override void OnTurnStart()
    {
        base.OnTurnStart();
        if (_playerInformations.Count > 0)
        {
            foreach (PlayerInformation x in _playerInformations)
            {
                x.DealDamage(1, false, player);
            }
        }
    }

    private void SpawnAdjacentWalls()
    {
        (int x, int y) coordinates = GameTileMap.Tilemap.GetChunk(player.GlobalPosition + new Vector2(0, 50f)).GetIndexes();
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
            if (x.Item1 >= 0 && x.Item1 < chunkDataArray.GetLength(0) && x.Item2 >= 0 && x.Item2 < chunkDataArray.GetLength(1))
            {
                ChunkData chunkData = chunkDataArray[x.Item1, x.Item2];
                
                PackedScene spawnResource = (PackedScene)wallPrefab;
                Player wall = spawnResource.Instantiate<Player>();
                player.GetTree().Root.CallDeferred("add_child", wall);
                PlayerInformation tempPlayerInformation = wall.playerInformation;
                    
                GameTileMap.Tilemap.SetCharacter(chunkData, wall, tempPlayerInformation);
                _playerInformations.Add(tempPlayerInformation);
            }
        }
    }
}
