using System.Collections.Generic;
using Godot;

public partial class WallEntrap : BaseAction
{
    [Export] private ObjectData wallRockData;
    [Export] private Resource wallPrefab;
    private Object wall;
    private List<PlayerInformation> _playerInformations;
    private List<Object> wallObjects;
    private int wallCount = 0;
    //Sitas ability visiškai kitoks nei aprašymas
    //Gal kažką praleidau ir buvo nuspręsta kitaip daryt?
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
        wallCount = 0;
        PlayerAbilityAnimation();
        SpawnAdjacentWalls(chunk);
        FinishAbility();
    }
    
    public override void OnBeforeStart(ChunkData chunkData)
    {
        base.OnBeforeStart(chunkData);
        if (wallObjects is not null)
        {
            foreach (var t in wallObjects)
            {
                if (t is null)
                {
                    wallCount--;
                }
            }
        }

        if (_playerInformations != null && _playerInformations.Count > 0 && IsEnemyTrapped())
        {
            foreach (PlayerInformation x in _playerInformations)
            {
                x.DealDamage(1, _player); //Damage, nes klaustrofobija!
            }
        }
        else
        {
            if (_playerInformations is not null)
            {
                _playerInformations.Clear();
            }

            if (wallObjects is not null)
            {
                foreach (var t in wallObjects)
                {
                    t.Death();
                }
                wallObjects.Clear();
            }
        }
    }

    protected override bool CanAddTile(ChunkData chunk)
    {
        return chunk != null && !chunk.TileIsLocked();
    }

    private void SpawnAdjacentWalls(ChunkData chunk)
    {
        if (_playerInformations is null)
        {
            _playerInformations = new List<PlayerInformation>();
        }
        else
        {
            _playerInformations.Clear();
        }

        if (wallObjects is null)
        {
            wallObjects = new List<Object>();
        }
        else
        {
            wallObjects.Clear();
        }
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
            if (GameTileMap.Tilemap.CheckBounds(x.Item1,x.Item2) && chunkDataArray[x.Item1,x.Item2].GetCurrentPlayer() == null)
            {
                ChunkData chunkData = chunkDataArray[x.Item1, x.Item2];
                PackedScene spawnResource = (PackedScene)wallPrefab;
                wall = spawnResource.Instantiate<Object>();
                _player.GetTree().CurrentScene.CallDeferred("add_child", wall);
                wall.SetupObject(wallRockData);
                GameTileMap.Tilemap.SpawnObject(wall, chunkData);
                if (chunkDataArray[x.Item1,x.Item2].IsFogOnTile())
                {
                    GameTileMap.Tilemap.RemoveFog(chunkDataArray[x.Item1, x.Item2], _player);
                }
                
                if (chunk.CharacterIsOnTile() && !_playerInformations.Contains(chunk.GetCurrentPlayer().objectInformation.GetPlayerInformation()))
                {
                    _playerInformations.Add(chunk.GetCurrentPlayer().objectInformation.GetPlayerInformation());   
                }
                wallObjects.Add(wall);
                wallCount++;
            }

            if (!GameTileMap.Tilemap.CheckBounds(x.Item1,x.Item2)) // out of bounds it means trapped
            {
                if (chunk.CharacterIsOnTile() && !_playerInformations.Contains(chunk.GetCurrentPlayer().objectInformation.GetPlayerInformation()))
                {
                    _playerInformations.Add(chunk.GetCurrentPlayer().objectInformation.GetPlayerInformation());   
                }
                wallCount++;
            }

            if (GameTileMap.Tilemap.CheckBounds(x.Item1,x.Item2) && chunkDataArray[x.Item1,x.Item2].GetCurrentPlayer() != null && !IsAllegianceSame(chunk, chunkDataArray[x.Item1,x.Item2])) // jei yra jo priesas jis irgi trapped
            {
                if (chunk.CharacterIsOnTile() && !_playerInformations.Contains(chunk.GetCurrentPlayer().objectInformation.GetPlayerInformation()))
                {
                    _playerInformations.Add(chunk.GetCurrentPlayer().objectInformation.GetPlayerInformation());   
                }
                wallCount++;
            }
            
        }
    }

    private bool IsEnemyTrapped()
    {
        if (wallCount == 4)
        {
            return true;
        }
        return false;
    }
}
