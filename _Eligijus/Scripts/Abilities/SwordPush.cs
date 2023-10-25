using System.Collections.Generic;
using Godot;

public partial class SwordPush : BaseAction
{
    [Export]
    public int pushDamage = 15;
    [Export]
    public int centerDamage = 30;
    private List<ChunkData> _attackTiles;
    private ChunkData _adjacent;

    public SwordPush()
    {
    }

    public SwordPush(SwordPush ability): base(ability)
    {
        pushDamage = ability.pushDamage;
        centerDamage = ability.centerDamage;
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        SwordPush ability = new SwordPush((SwordPush)action);
        return ability;
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        // if (CanTileBeClicked(position))
        // {
        base.ResolveAbility(chunk);
        Side _side = Side.none;
        CreateAttackGrid(chunk);
        for (int i = 0; i < _attackTiles.Count; i++)
        {
            if (i > 0)
            {
                int damage = pushDamage;
                bool crit = IsItCriticalStrike(ref damage);
                DealDamage(_attackTiles[i], damage, crit);
                _side = ChunkSideByCharacter(chunk, _attackTiles[i]);
                

                (int x, int y) sideVector = GetSideVector(_side);
                MovePlayerToSide(_attackTiles[i], sideVector);
            }
            else
            {
                CheckAdjacent(chunk);

                if (_adjacent != null)
                {
                    _side = ChunkSideByCharacter(chunk, _adjacent);
                    (int x, int y) sideVector = GetSideVector(_side);
                    MovePlayerToSide(_attackTiles[i], sideVector);
                }

                int damage = centerDamage;
                bool crit = IsItCriticalStrike(ref damage);
                DealDamage(_attackTiles[i], damage, crit);
            }
        }
        FinishAbility();
        // }
    }

    private void CheckAdjacent(ChunkData tile)
    {
        (int x, int y) = tile.GetIndexes();
		
        var directionVectors = new List<(int, int)>
        {
            (1 + x, 0 + y),
            (0 + x, 1 + y),
            (-1 + x, 0 + y),
            (0 + x, -1 + y)
        };
        _adjacent = null;

        foreach (var directions in directionVectors)
        {
            ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(directions.Item1, directions.Item2);
            (int x, int y) indexes = chunkData.GetIndexes();
            if (!chunkData.CharacterIsOnTile() && GameTileMap.Tilemap.CheckBounds(indexes.x, indexes.y))
            {
                _adjacent = chunkData;
                break;
            }
        }
    }

    private void CreateAttackGrid(ChunkData selected)
    {
        (int x, int y) tileIndex = selected.GetIndexes();
        List<(int, int)> positionIndexes = new List<(int, int)> 
        {
            (tileIndex.x, tileIndex.y + 1),  // Up
            (tileIndex.x, tileIndex.y - 1),  // Down
            (tileIndex.x + 1, tileIndex.y),  // Right
            (tileIndex.x - 1, tileIndex.y)   // Left
        };
        if (_attackTiles == null)
        {
            _attackTiles = new List<ChunkData>();
        }
        else
        {
            _attackTiles.Clear();
        }
        _attackTiles.Add(selected); // added center chunk;
        foreach (var indexes in positionIndexes)
        {
            if (GameTileMap.Tilemap.CheckBounds(indexes.Item1, indexes.Item2))
            {
                ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(indexes.Item1, indexes.Item2);
                if (chunkData.CharacterIsOnTile())
                {
                    _attackTiles.Add(chunkData);
                }
            }
        }
    }
}
