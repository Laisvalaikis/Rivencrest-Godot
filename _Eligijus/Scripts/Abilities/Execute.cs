
using Godot;

public partial class Execute : BaseAction
{
    private PlayerInformation _playerInformation;
 
    public Execute()
    {
 		
    }
    public Execute(Execute execute): base(execute)
    {
        
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        Execute execute = new Execute((Execute)action);
        return execute;
    }
    
    public override void OnMoveHover(ChunkData hoveredChunk, ChunkData previousChunk)
    {
        HighlightTile previousChunkHighlight = previousChunk?.GetTileHighlight(); 
        HighlightTile hoveredChunkHighlight = hoveredChunk?.GetTileHighlight();
        if (previousChunkHighlight != null && (hoveredChunk == null || !hoveredChunkHighlight.isHighlighted))
        { 
            SetNonHoveredAttackColor(previousChunk);
            DisableDamagePreview(previousChunk);
        }
        if (hoveredChunkHighlight == null || hoveredChunk == previousChunk)
        { 
            return;
        }
        if (hoveredChunkHighlight.isHighlighted)
        {
            if (CanTileBeClicked(hoveredChunk))
            {
                _playerInformation = hoveredChunk.GetCurrentPlayer().playerInformation;
                EnableDamagePreview(hoveredChunk);
            }
            SetHoveredAttackColor(hoveredChunk);
        }
        if (previousChunkHighlight != null)
        {
            SetNonHoveredAttackColor(previousChunk);
            DisableDamagePreview(previousChunk);
        }
    }
    public override void ResolveAbility(ChunkData chunk)
    {
        if (CanTileBeClicked(chunk))
        {
            UpdateAbilityButton();
            chunk.GetCurrentPlayer().playerInformation.DealDamage(minAttackDamage,player);
            if(chunk.GetCurrentPlayer() == null)
            {
                player.playerInformation.Heal(5);
                GameTileMap.Tilemap.MoveSelectedCharacter(chunk);
            }
            base.ResolveAbility(chunk);
            FinishAbility();
        }
    }

    public override void CreateAvailableChunkList(int range)
    {
        ChunkData centerChunk = GameTileMap.Tilemap.GetChunk(player.GlobalPosition);
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
                    
                    if (targetX >= 0 && targetX < chunksArray.GetLength(0) && targetY >= 0 && targetY < chunksArray.GetLength(1))
                    {
                        ChunkData chunk = chunksArray[targetX, targetY];
                        TryAddTile(chunk);
                    }
                }
            }
        }
    }
}