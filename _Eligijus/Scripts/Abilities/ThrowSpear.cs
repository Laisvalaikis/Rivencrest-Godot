using Godot;

public partial class ThrowSpear : BaseAction
{
    [Export] private Resource spearPrefab;
    private bool laserGrid = true;
    
    public ThrowSpear(ThrowSpear ability): base(ability)
    {
        spearPrefab = ability.spearPrefab;
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        ThrowSpear ability = new ThrowSpear((ThrowSpear)action);
        return ability;
    }

    public override void CreateAvailableChunkList(int attackRange)
    {
        ChunkData centerChunk = GameTileMap.Tilemap.GetChunk(player.GlobalPosition);
        (_, int centerY) = centerChunk.GetIndexes();
        _chunkList.Clear();
        ChunkData[,] chunksArray = GameTileMap.Tilemap.GetChunksArray();
        for (int y = -attackRange; y <= attackRange; y++)
        {
            if (Mathf.Abs(y) == attackRange)
            {
                int targetY = centerY + y;
                if (targetY >= 0 && targetY < chunksArray.GetLength(1))
                {
                    ChunkData chunk = chunksArray[0, targetY];
                    if (chunk != null && !chunk.TileIsLocked())
                    {
                        _chunkList.Add(chunk);
                    }
                }
            }
        }
    }
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        
        PackedScene spawnResource = (PackedScene)spearPrefab;
        Player spawnedCharacter = spawnResource.Instantiate<Player>();
        player.GetTree().Root.CallDeferred("add_child", spawnedCharacter);
        PlayerInformation tempPlayerInformation = spawnedCharacter.playerInformation;
        
        FinishAbility();
    }
    
}
