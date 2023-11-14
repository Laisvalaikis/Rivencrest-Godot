using Godot;

public partial class MartialArtist : AbilityBlessing
{
    
    public MartialArtist()
    {
			
    }
    
    public MartialArtist(MartialArtist blessing): base(blessing)
    {
        
    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        MartialArtist blessing = new MartialArtist((MartialArtist)baseBlessing);
        return blessing;
    }
    
    public override BaseBlessing CreateNewInstance()
    {
        MartialArtist blessing = new MartialArtist(this);
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
                    DealRandomDamageToTarget(baseAction.GetPlayer(), tempTile, baseAction, baseAction.minAttackDamage, baseAction.maxAttackDamage);
                }
            }
        }
    }
    
}