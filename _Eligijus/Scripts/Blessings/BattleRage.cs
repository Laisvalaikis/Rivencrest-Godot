using System;
using Godot;

public partial class BattleRage : AbilityBlessing
{
    [Export] private int minHeal = 3;
    [Export] private int maxHeal = 4;
    private Random _random;
    private bool secondTime = false;
    
    public BattleRage()
    {
			
    }
    
    public BattleRage(BattleRage blessing): base(blessing)
    {
        _random = new Random();
        minHeal = blessing.minHeal;
        maxHeal = blessing.maxHeal;
    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        BattleRage blessing = new BattleRage((BattleRage)baseBlessing);
        return blessing;
    }
    
    public override BaseBlessing CreateNewInstance()
    {
        BattleRage blessing = new BattleRage(this);
        return blessing;
    }
    
    public override void ResolveBlessing(BaseAction baseAction, ChunkData tile)
    {
        base.ResolveBlessing(baseAction);
       
        if (!secondTime)
        {
            secondTime = true;
        }
        else
        {
            int randomHeal = _random.Next(minHeal, maxHeal);
            baseAction.GetPlayer().playerInformation.Heal(randomHeal);
            secondTime = false;
        }
    }
}