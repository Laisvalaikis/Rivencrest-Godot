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

    protected override void ModifyBonusDamage(ChunkData chunk)
    {
        if (chunk.GetCurrentPlayer().debuffManager.ContainsDebuff(typeof(PoisonDebuff)))
        {
            bonusDamage += poisonBonusDamage;
        }
    }
    
    public override async void ResolveAbility(ChunkData chunk)
    { 
        UpdateAbilityButton(); 
        foreach (var chunkData in _chunkList) 
        { 
            if (CanBeUsedOnTile(chunkData)) 
            { 
                DealRandomDamageToTarget(chunkData, minAttackDamage, maxAttackDamage);
                PlayAnimation("CrowAttack", chunkData);
            }
        } 
        PlayerAbilityAnimation();
        base.ResolveAbility(chunk);
        FinishAbility();
    }
}