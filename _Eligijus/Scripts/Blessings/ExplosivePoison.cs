using Godot;
using System.Collections.Generic;
public partial class ExplosivePoison : AbilityBlessing
{
    [Export] private int poisonTurns = 2;
    [Export] private int poisonDamage = 2;
    
    public ExplosivePoison()
    {
			
    }
    
    public ExplosivePoison(ExplosivePoison blessing): base(blessing)
    {
        poisonTurns = blessing.poisonTurns;
        poisonDamage = blessing.poisonDamage;
    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        ExplosivePoison blessing = new ExplosivePoison((ExplosivePoison)baseBlessing);
        return blessing;
    }
    
    public override BaseBlessing CreateNewInstance()
    {
        ExplosivePoison blessing = new ExplosivePoison(this);
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
                if (IsAllegianceSame(player, chunkData, baseAction))
                {
                    baseAction.GetPlayer().AddPoison(new Poison(chunkData, poisonTurns, poisonDamage));
                }
            }
        }
    }
}