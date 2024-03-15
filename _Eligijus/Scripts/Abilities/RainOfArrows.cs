using System.Collections.Generic;
using Godot;

public partial class RainOfArrows : BaseAction
{
    [Export] private ObjectData arrowTileData;
    [Export] private Resource arrowTilePrefab;
    private List<ChunkData> damageList = new List<ChunkData>();
    public RainOfArrows()
    {
        
    }
    public RainOfArrows(RainOfArrows ability): base(ability)
    {
        arrowTilePrefab = ability.arrowTilePrefab;
        arrowTileData = ability.arrowTileData;
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        RainOfArrows ability = new RainOfArrows((RainOfArrows)action);
        return ability;
    }
    
      
    public override async void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
        base.ResolveAbility(chunk);
        await PlayAnimation("RainOfArrows", chunk);
        foreach (ChunkData chunkData in damageList)
        {
            PackedScene spawnCharacter = (PackedScene)arrowTilePrefab;
            Object spawnedArrowTile = spawnCharacter.Instantiate<Object>();
            _player.GetTree().Root.CallDeferred("add_child", spawnedArrowTile);
            spawnedArrowTile.SetupObject(arrowTileData);
            spawnedArrowTile.AddPlayerForObjectAbilities(_player);
            GameTileMap.Tilemap.SpawnObject(spawnedArrowTile, chunkData);
        }
        FinishAbility();
    }

    public override void OnMoveHover(ChunkData hoveredChunk, ChunkData previousChunk)
    {
        if (hoveredChunk == previousChunk) return;
        if (hoveredChunk != null && hoveredChunk.GetTileHighlight().isHighlighted)
        {
            if (damageList is not null && damageList.Count > 0)
            {
                foreach (ChunkData chunk in damageList)
                {
                    SetNonHoveredAttackColor(chunk);
                    DisableDamagePreview(chunk);
                }
            }
            CreateDamageTileList(hoveredChunk);
            if (damageList is not null && damageList.Count > 0)
            {
                foreach (ChunkData chunk in damageList)
                {
                    SetHoveredAttackColor(chunk);
                    EnableDamagePreview(chunk);
                }
            }
        }

    }
    
    public override void ClearGrid()
    {
        ChunkData playerChunkData = GameTileMap.Tilemap.GetChunk(_player.GlobalPosition);
        if (playerChunkData is not null)
        {
            DisableDamagePreview(playerChunkData);
        }

        if (_chunkList != null)
        {
            foreach (var chunk in _chunkList)
            {
                HighlightTile highlightTile = chunk.GetTileHighlight();
                highlightTile.ActivateColorGridTile(false);
                DisableDamagePreview(chunk);
            }
            _chunkList.Clear();
        }
    }

    public void CreateDamageTileList(ChunkData chunk)
    {
        if (damageList is null)
        {
            damageList = new List<ChunkData>();
        }
        else
        {
            damageList.Clear();
        }
        (int x, int y) index = chunk.GetIndexes();
        var spellDirectionVectors = new List<(int, int)>
        {
            (0 + index.x, 0 + index.y),
            (1 + index.x, 0 + index.y),
            (0 + index.x, 1 + index.y),
            (-1 + index.x, 0 + index.y),
            (0 + index.x, -1 + index.y)
        };
        foreach (var direction in spellDirectionVectors)
        {
            if (GameTileMap.Tilemap.CheckBounds(direction.Item1, direction.Item2))
            {
                ChunkData temp = GameTileMap.Tilemap.GetChunkDataByIndex(direction.Item1, direction.Item2);
                damageList.Add(temp);
            }
        }
    }
}
