using Godot;
using System.Collections.Generic;

public partial class MoltenTouch : AbilityBlessing
{
    [Export] private int criticalDamage;
    
    public MoltenTouch()
    {
			
    }
    
    public MoltenTouch(MoltenTouch blessing): base(blessing)
    {
        
    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        MoltenTouch blessing = new MoltenTouch((MoltenTouch)baseBlessing);
        return blessing;
    }
    
    public override BaseBlessing CreateNewInstance()
    {
        MoltenTouch blessing = new MoltenTouch(this);
        return blessing;
    }
    
    public override void ResolveBlessing(ref BaseAction baseAction, ChunkData tile)
    {
        base.ResolveBlessing(ref baseAction);
        (int x, int y) = tile.GetIndexes();
        
        var directionVectors = new List<(int, int)>
        {
            (1 + x, 0 + y),
            (0 + x, 1 + y),
            (-1 + x, 0 + y),
            (0 + x, -1 + y)
        };

        foreach (var directions in directionVectors)
        {
            ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(directions.Item1, directions.Item2);
            if (chunkData.CharacterIsOnTile())
            {
                Player player = chunkData.GetCurrentPlayer();
                if (IsAllegianceSame(player.playerInformation, chunkData, baseAction))
                {
                    DealRandomDamageToTarget(baseAction.GetPlayer().playerInformation, chunkData, baseAction, 2, 2);
                }
            }
        }
        if (tile.CharacterIsOnTile())
        {
            DealRandomDamageToTarget(baseAction.GetPlayer().playerInformation, tile, baseAction, 2, 2);
        }
        
    }
    
    
}