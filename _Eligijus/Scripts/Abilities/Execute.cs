using Godot;

public partial class Execute : BaseAction
{
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
    
    public override void ResolveAbility(ChunkData chunk)
    { 
        UpdateAbilityButton(); 
        chunk.GetCurrentPlayer().objectInformation.GetPlayerInformation().DealDamage(minAttackDamage,_player); 
        if(chunk.GetCurrentPlayer() == null) 
        { 
            _player.objectInformation.GetPlayerInformation().Heal(5); 
            GameTileMap.Tilemap.MoveSelectedCharacter(chunk);
        } 
        base.ResolveAbility(chunk); 
        FinishAbility();
    }

    public override void CreateAvailableChunkList(int range)
    {
        ChunkData centerChunk = GameTileMap.Tilemap.GetChunk(_player.GlobalPosition);
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