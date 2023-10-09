public partial class DisarmingSlam : BaseAction
{
    
    public DisarmingSlam()
    {
 		
    }
    public DisarmingSlam(DisarmingSlam disarmingSlam): base(disarmingSlam)
    {
    }
 	
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        DisarmingSlam disarmingSlam = new DisarmingSlam((DisarmingSlam)action);
        return disarmingSlam;
    }
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        FinishAbility();
    }

    public override void CreateAvailableChunkList(int radius)
    {
        ChunkData centerChunk = GameTileMap.Tilemap.GetChunk(player.GlobalPosition);
        (int centerX, int centerY) = centerChunk.GetIndexes();
        _chunkList.Clear();
        int count = attackRange; 
        
        ChunkData[,] chunkArray = new ChunkData[4,count];

        int start = 1;
        for (int i = 0; i < count; i++) 
        {
            if (GameTileMap.Tilemap.CheckBounds(centerX + i + start, centerY))
            {
                ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(centerX + i + start, centerY);
                _chunkList.Add(chunkData);
                chunkArray[0, i] = chunkData;
            }
            if (GameTileMap.Tilemap.CheckBounds(centerX - i - start, centerY))
            {
                ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(centerX-i - start, centerY);
                _chunkList.Add(chunkData);
                chunkArray[1, i] = chunkData;
            }
            if (GameTileMap.Tilemap.CheckBounds(centerX, centerY + i + start))
            {
                ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(centerX, centerY + i + start);
                _chunkList.Add(chunkData);
                chunkArray[2, i] = chunkData;
            }
            if (GameTileMap.Tilemap.CheckBounds(centerX, centerY - i - start))
            {
                ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(centerX, centerY - i - start);
                _chunkList.Add(chunkData);
                chunkArray[3, i] = chunkData;
            }
        }

    }
}