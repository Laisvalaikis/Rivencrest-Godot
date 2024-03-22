using Godot;

public partial class ReadyAimFire : BaseAction
{
    private int localIndex = -1;
    public ReadyAimFire()
    {
        
    }
    public ReadyAimFire(ReadyAimFire ability): base(ability)
    {
        
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        ReadyAimFire ability = new ReadyAimFire((ReadyAimFire)action);
        return ability;
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
        base.ResolveAbility(chunk);
        PlayAnimation("Bandit2", chunk);
        localIndex = FindChunkIndex(chunk);
        FinishAbility();
    }
     
    public override void CreateAvailableChunkList(int range)
    {
        ChunkData centerChunk = GameTileMap.Tilemap.GetChunk(_player.GlobalPosition);
        (int centerX, int centerY) = centerChunk.GetIndexes();
        _chunkList.Clear();
        int count = attackRange;
        _chunkArray = new ChunkData[4,count];

        int start = 1;
        for (int i = 0; i < count; i++) 
        {
            if (GameTileMap.Tilemap.CheckBounds(centerX + i + start, centerY))
            {
                ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(centerX + i + start, centerY);
                _chunkList.Add(chunkData);
                _chunkArray[0, i] = chunkData;
            }
            if (GameTileMap.Tilemap.CheckBounds(centerX - i - start, centerY))
            {
                ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(centerX-i - start, centerY);
                _chunkList.Add(chunkData);
                _chunkArray[1, i] = chunkData;
            }
            if (GameTileMap.Tilemap.CheckBounds(centerX, centerY + i + start))
            {
                ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(centerX, centerY + i + start);
                _chunkList.Add(chunkData);
                _chunkArray[2, i] = chunkData;
            }
            if (GameTileMap.Tilemap.CheckBounds(centerX, centerY - i - start))
            {
                ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(centerX, centerY - i - start);
                _chunkList.Add(chunkData);
                _chunkArray[3, i] = chunkData;
            }
        }
    }
    public override void OnBeforeStart(ChunkData chunkData)
    {
        base.OnBeforeStart(chunkData);
        if (localIndex != -1)
        {
            for (int i = 0; i < _chunkArray.GetLength(1); i++)
            {
                if (_chunkArray[localIndex, i].CharacterIsOnTile())
                {
                    DealRandomDamageToTarget(_chunkArray[localIndex, i], minAttackDamage, maxAttackDamage);
                    break;
                }
            }
            _chunkArray = new ChunkData[4, attackRange];
        }
    }
}
