using System;
using Godot;

public partial class HealingSight : BaseAction
{
    [Export]
    public int minHealAmount = 3;
    [Export]
    public int maxHealAmount = 7;
    
    public HealingSight()
    {
 		
    }
    public HealingSight(HealingSight healingSight): base(healingSight)
    {
        minHealAmount = healingSight.minHealAmount;
        maxHealAmount = healingSight.maxHealAmount;
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        HealingSight healSingle = new HealingSight((HealingSight)action);
        return healSingle;
    }
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        Random random = new Random();
        int randomHeal = random.Next(minHealAmount, maxHealAmount);
        chunk.GetCurrentPlayerInformation().Heal(randomHeal);
        FinishAbility();
    }
}