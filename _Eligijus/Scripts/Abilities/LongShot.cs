using Godot;

public partial class LongShot : BaseAction
{

    public LongShot()
    {
        
    }
    public LongShot(LongShot ability): base(ability)
    {
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        LongShot ability = new LongShot((LongShot)action);
        return ability;
    }
    

    public override void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
        base.ResolveAbility(chunk);
        PlayerAbilityAnimation();
        PlayAnimation("Green1", chunk);
        DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
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

                    if (GameTileMap.Tilemap.CheckBounds(targetX,targetY))
                    {
                        ChunkData chunk = chunksArray[targetX, targetY];
                        TryAddTile(chunk);
                    }
                }
            }
        }
    }
}
