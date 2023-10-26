using Godot;

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
             foreach (ChunkData tile in GetChunkList())
             {
                 DealRandomDamageToTarget(tile, minAttackDamage, maxAttackDamage);
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
                    EnableDamagePreview(chunk);
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
                }
            }
        }
    }

    public override void CreateAvailableChunkList(int attackRange)
    {
        ChunkData centerChunk =  GameTileMap.Tilemap.GetChunk(player.GlobalPosition);
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

                    // Ensuring we don't go out of array bounds.
                    if (targetX >= 0 && targetX < chunksArray.GetLength(0) && targetY >= 0 && targetY < chunksArray.GetLength(1))
                    {
                        ChunkData chunk = chunksArray[targetX, targetY];
                        if (chunk != null && !chunk.TileIsLocked())
                        {
                            _chunkList.Add(chunk);
                        }
                    }
                }
            }
        }
    }
}
