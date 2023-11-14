using Godot;
using System.Collections.Generic;

public partial class MeteorRain : AbilityBlessing
{

    [Export] private int addToMinDamage = 3;
    [Export] private int addToMaxDamage = 3;
    private List<ChunkData> cometTiles = new List<ChunkData>();
    
    public MeteorRain()
    {
			
    }
    
    public MeteorRain(MeteorRain blessing): base(blessing)
    {
        addToMinDamage = blessing.addToMinDamage;
        addToMaxDamage = blessing.addToMaxDamage;
    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        MeteorRain blessing = new MeteorRain((MeteorRain)baseBlessing);
        return blessing;
    }
    
    public override BaseBlessing CreateNewInstance()
    {
        MeteorRain blessing = new MeteorRain(this);
        return blessing;
    }
    
    public override void ResolveBlessing(BaseAction baseAction, ChunkData tile)
    {
        base.ResolveBlessing(baseAction, tile);
        if (tile.CharacterIsOnTile())
        {
            tile.GetCurrentPlayer().playerInformation.DealDamage(2, false, baseAction.GetPlayer());
        }
    }


    public override void OnTurnStart(BaseAction baseAction)
    {
        base.OnTurnStart(baseAction);
        if (cometTiles != null && cometTiles.Count > 0)
        {
            for (int i = 0; i < cometTiles.Count; i++)
            {
                if (cometTiles[i].CharacterIsOnTile())
                {
                    DealRandomDamageToTarget(baseAction.GetPlayer(), cometTiles[i], baseAction,
                        baseAction.minAttackDamage + addToMinDamage, baseAction.maxAttackDamage + addToMaxDamage);
                }
            }

            cometTiles.Clear();
        }
    }

    public override void PrepareForBlessing(ChunkData chunkData)
    {
        base.PrepareForBlessing(chunkData);
        if (cometTiles == null)
        {
            cometTiles = new List<ChunkData>();
            cometTiles.Add(chunkData);
        }
        else
        {
            cometTiles.Add(chunkData);
        }
    }
}