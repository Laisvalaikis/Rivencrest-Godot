using Godot;

public partial class ComboKick : AbilityBlessing
{

    [Export] private int attackDamage = 3;

    
    public ComboKick()
    {
			
    }
    
    public ComboKick(ComboKick blessing): base(blessing)
    {
        attackDamage = blessing.attackDamage;
    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        ComboKick blessing = new ComboKick((ComboKick)baseBlessing);
        return blessing;
    }
    
    public override BaseBlessing CreateNewInstance()
    {
        ComboKick blessing = new ComboKick(this);
        return blessing;
    }
    
    public override void ResolveBlessing(BaseAction baseAction, ChunkData tile)
    {
        base.ResolveBlessing(baseAction, tile);
        
        if (tile.CharacterIsOnTile())
        {
            ChunkData current = GameTileMap.Tilemap.GetChunk(baseAction.GetPlayer().GlobalPosition);
            Side side = ChunkSideByCharacter(current, tile);
            (int x, int y) sideVector = GetSideVector(side);
            sideVector = (sideVector.x, sideVector.y);
            (int x, int y) indexes = tile.GetIndexes();
            (int x, int y) tempIndexes = new (indexes.x + sideVector.x, indexes.y + sideVector.y);
            if (GameTileMap.Tilemap.CheckBounds(tempIndexes.x, tempIndexes.y))
            {
                ChunkData tempTile = GameTileMap.Tilemap.GetChunkDataByIndex(tempIndexes.x, tempIndexes.y);
                if (tempTile.CharacterIsOnTile())
                {
                    baseAction.GetPlayer().playerInformation.DealDamage(attackDamage, baseAction.GetPlayer());
                }
            }
        }
    }
    
}