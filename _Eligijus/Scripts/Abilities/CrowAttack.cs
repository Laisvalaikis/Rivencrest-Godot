using System.Collections.Generic;
using Godot;

public partial class CrowAttack : BaseAction
{
    [Export]
    public int poisonBonusDamage=2;
    
    public CrowAttack()
    {
		
    }
    public CrowAttack(CrowAttack crowAttack) : base(crowAttack)
    {
        poisonBonusDamage = crowAttack.poisonBonusDamage;
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        CrowAttack createBearTrap = new CrowAttack((CrowAttack)action);
        return createBearTrap;
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        foreach (var t in _chunkList)
        {
            if (t.CharacterIsOnTile())
            {
                int bonusDamage = 0;
                if (t.GetCurrentPlayer().GetPoisonCount() > 0)
                {
                    bonusDamage += poisonBonusDamage;
                }
                DealRandomDamageToTarget(t,minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
            }
        }
        base.ResolveAbility(chunk);
        FinishAbility();
    }
    public override void OnTurnStart()
    {
        base.OnTurnStart();
    }

}