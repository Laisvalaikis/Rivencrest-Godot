
using Godot;

public partial class Execute : BaseAction
{
    [Export]
    public int minimumDamage = 4;
    private PlayerInformationData _playerInformationData;
 
    public Execute()
    {
 		
    }
    public Execute(Execute execute): base(execute)
    {
        minimumDamage = execute.minimumDamage;
    }
 	
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        Execute execute = new Execute((Execute)action);
        return execute;
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        int damage = ExecuteDamage();
        chunk.GetCurrentPlayer().playerInformation.DealDamage(damage, false, player);
        if(chunk.GetCurrentPlayer().playerInformation.GetHealth() <= 0)
        {
            player.playerInformation.Heal(5);
            GameTileMap.Tilemap.MoveSelectedCharacter(chunk);
        }
        FinishAbility();
    }

    public override void CreateAvailableChunkList(int radius)
    {
        ChunkData centerChunk = GameTileMap.Tilemap.GetChunk(player.GlobalPosition);
        (int centerX, int centerY) = centerChunk.GetIndexes();
        _chunkList.Clear();
        ChunkData[,] chunksArray = GameTileMap.Tilemap.GetChunksArray(); 
        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                if (Mathf.Abs(x) + Mathf.Abs(y) == radius)
                {
                    int targetX = centerX + x;
                    int targetY = centerY + y;
                    
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
    private int ExecuteDamage() 
    {
        int damage = minimumDamage + Mathf.FloorToInt(float.Parse((
            (_playerInformationData.MaxHealth) * 0.15
        ).ToString()));
    
        return damage;
    }
}