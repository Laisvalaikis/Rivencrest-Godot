using System.Collections.Generic;
using Godot;

public partial class RainOfArrows : BaseAction
{
    private List<ChunkData> _cometTiles=new List<ChunkData>();
    public RainOfArrows()
    {
        
    }
    public RainOfArrows(RainOfArrows ability): base(ability)
    {
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        RainOfArrows ability = new RainOfArrows((RainOfArrows)action);
        return ability;
    }
    
    public override void OnTurnStart(ChunkData chunkData)
    {
        base.OnTurnStart(chunkData);
        if (_cometTiles.Count!=0)
        {
            foreach (ChunkData tile in _cometTiles)
            {
                DealRandomDamageToTarget(tile, minAttackDamage, maxAttackDamage);
            }
            _cometTiles.Clear();
        }
    }
      
    public override void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
        base.ResolveAbility(chunk);
        List<ChunkData> damageChunks = CreateDamageTileList(chunk);
        foreach (ChunkData chunkData in damageChunks)
        {
            if (!IsAllegianceSame(chunkData))
            {
                _cometTiles.Add(chunkData);
            }
        }
        FinishAbility();
    }
    
    public List<ChunkData> CreateDamageTileList(ChunkData chunk)
    {
        List<ChunkData> damageTiles = new List<ChunkData>();
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
                damageTiles.Add(temp);
            }
        }
        return damageTiles;
    }
}
