using System;
using System.Collections.Generic;
using Godot;

public partial class GatherRound : AbilityBlessing
{
    [Export] private int minHealAmount;
    [Export] private int maxHealAmount;
    private Random _random;
    
    public GatherRound()
    {
			
    }
    
    public GatherRound(GatherRound blessing): base(blessing)
    {
        minHealAmount = blessing.minHealAmount;
        maxHealAmount = blessing.maxHealAmount;
    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        GatherRound blessing = new GatherRound((GatherRound)baseBlessing);
        return blessing;
    }
    
    public override BaseBlessing CreateNewInstance()
    {
        GatherRound blessing = new GatherRound(this);
        return blessing;
    }
    
    public override void ResolveBlessing(ref BaseAction baseAction)
    {
        if (_random == null)
        {
            _random = new Random();
        }

        base.ResolveBlessing(ref baseAction);
        List<ChunkData> chunks = baseAction.GetChunkList();
        int randomHeal = _random.Next(minHealAmount, maxHealAmount);
        foreach (ChunkData tile in chunks)
        {
            tile.GetCurrentPlayerInformation().Heal(randomHeal);
        }
    }
}