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
         if (CanTileBeClicked(chunk))
         {
             UpdateAbilityButton();
             foreach (ChunkData tile in GetChunkList())
             {
                 if (CanTileBeClicked(tile))
                 {
                     DealRandomDamageToTarget(tile, minAttackDamage, maxAttackDamage);
                     SlowDebuff debuff = new SlowDebuff(1, 2);
                     tile.GetCurrentPlayer().debuffManager.AddDebuff(debuff,_player);
                 }
             }
             base.ResolveAbility(chunk);
             FinishAbility();
         }
    }

    public override void OnMoveHover(ChunkData hoveredChunk, ChunkData previousChunk)
    {
        if (hoveredChunk == previousChunk) return;
        if (hoveredChunk != null && hoveredChunk.GetTileHighlight().isHighlighted)
        {
            foreach (var chunk in _chunkList)
            {
                HighlightTile highlightTile = chunk.GetTileHighlight();
                if (highlightTile != null)
                {
                    SetHoveredAttackColor(chunk);
                    if (CanTileBeClicked(chunk))
                    {
                        EnableDamagePreview(chunk, minAttackDamage, maxAttackDamage);
                    }
                }
            }
        }
        else if (hoveredChunk == null || !hoveredChunk.GetTileHighlight().isHighlighted)
        {
            foreach (var chunk in _chunkList)
            {
                HighlightTile highlightTile = chunk.GetTileHighlight();
                if (highlightTile != null)
                {
                    SetNonHoveredAttackColor(chunk);
                    DisableDamagePreview(chunk);
                }
            }
        }
    }

    public override void CreateAvailableChunkList(int attackRange)
    {
        ChunkData centerChunk =  GameTileMap.Tilemap.GetChunk(_player.GlobalPosition);
        (int centerX, int centerY) = centerChunk.GetIndexes();
        _chunkList.Clear();
        ChunkData[,] chunksArray = GameTileMap.Tilemap.GetChunksArray(); 
        for (int y = -attackRange; y <= attackRange; y++)
        {
            for (int x = -attackRange; x <= attackRange; x++)
            {
                if (Mathf.Abs(x) + Mathf.Abs(y) == attackRange)
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
