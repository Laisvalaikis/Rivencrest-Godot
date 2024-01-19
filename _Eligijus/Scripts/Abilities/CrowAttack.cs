using System.Collections.Generic;
using Godot;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

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
        CrowAttack crowAttack = new CrowAttack((CrowAttack)action);
        return crowAttack;
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
        foreach (var t in _chunkList)
        {
            if (t.CharacterIsOnTile() && CanTileBeClicked(t))
            {
                int bonusDamage = 0;
                if(t.GetCurrentPlayer().debuffManager.ContainsDebuff(typeof(PoisonDebuff)))
                {
                    bonusDamage += poisonBonusDamage;
                }
                DealRandomDamageToTarget(t,minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
            }
        }
        base.ResolveAbility(chunk);
        FinishAbility();
    }
}