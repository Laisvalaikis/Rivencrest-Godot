using System;
using Godot;

namespace Rivencrestgodot._Eligijus.Scripts.Abilities;

public partial class HealSingle : BaseAction
{
    private PlayerInformation _playerInformation;
    [Export]
    public int minHealAmount = 3;
    [Export]
    public int maxHealAmount = 7;
    
    public HealSingle()
    {
 		
    }
    public HealSingle(HealSingle healSingle): base(healSingle)
    {
        minHealAmount = healSingle.minHealAmount;
        maxHealAmount = healSingle.maxHealAmount;
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        HealSingle healSingle = new HealSingle((HealSingle)action);
        return healSingle;
    }
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        Random random = new Random();
        int randomHeal = random.Next(minHealAmount, maxHealAmount);
        _playerInformation.Heal(randomHeal);
        FinishAbility();
    }
}
