using Godot;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class BindingRitual : BaseAction
{
    public BindingRitual()
    {
        
    }
    
    public BindingRitual(BindingRitual ability): base(ability)
    {
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        BindingRitual ability = new BindingRitual((BindingRitual)action);
        return ability;
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
        foreach (ChunkData tile in GetChunkList())
        {
            if (CanBeUsedOnTile(tile))
            {
                DealRandomDamageToTarget(tile, minAttackDamage, maxAttackDamage);
                SlowDebuff debuff = new SlowDebuff(1, 2, "IceSlow");
                tile.GetCurrentPlayer().debuffManager.AddDebuff(debuff,_player);
            }
        }
        base.ResolveAbility(chunk);
        FinishAbility();
    }
    public override void CreateAvailableChunkList(int range)
    {
        ChunkData centerChunk =  GameTileMap.Tilemap.GetChunk(_player.GlobalPosition);
        (int centerX, int centerY) = centerChunk.GetIndexes();
        _chunkList.Clear();
        ChunkData[,] chunksArray = GameTileMap.Tilemap.GetChunksArray(); 
        for (int y = -range; y <= range; y++)
        {
            for (int x = -range; x <= range; x++)
            {
                if (Mathf.Abs(x) + Mathf.Abs(y) == range)
                {
                    int targetX = centerX + x;
                    int targetY = centerY + y;

                    if (GameTileMap.Tilemap.CheckBounds(targetX, targetY))
                    {
                        ChunkData chunk = chunksArray[targetX, targetY];
                        TryAddTile(chunk);
                    }
                }
            }
        }
    }
}