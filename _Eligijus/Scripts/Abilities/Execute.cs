
using Godot;

public partial class Execute : BaseAction
{
    [Export]
    public int minimumDamage = 4;
    private PlayerInformation _playerInformation;
 
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
            if (hoveredChunk.GetCurrentPlayer() != null)
            {
                _playerInformation = hoveredChunk.GetCurrentPlayer().playerInformation;
                maxAttackDamage = ExecuteDamage();
                minAttackDamage = maxAttackDamage;
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
        base.ResolveAbility(chunk);
        int damage = ExecuteDamage();
        chunk.GetCurrentPlayer().playerInformation.DealDamage(damage, false, player);
        if(chunk.GetCurrentPlayer().playerInformation.GetHealth() <= 0) //Cia ateity jauciu reikes updeitinti sita dali, nes dabar musu characteriai nemirsta, ju health tiesiog tampa <=0. Ateity, kai playeriai mirs, turbut nebebus galima tiesiog pacheckinti chunko playerinfo, nes ant chunko playerio tiesiog nebebus. Donelaičio pamąstymai
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
        int damage = minimumDamage + Mathf.FloorToInt(_playerInformation.GetMaxHealth() * 0.15);
        return damage;
    }
}