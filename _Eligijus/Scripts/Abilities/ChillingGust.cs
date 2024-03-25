using System.Collections.Generic;
using Godot;
using Rivencrestgodot._Eligijus.Scripts.BuffSystem;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class ChillingGust : BaseAction
{
    private Player _protectedAlly;

    public ChillingGust()
    {
        
    }
    public ChillingGust(ChillingGust ability): base(ability)
    {
        teamDisplayText = "PROTECT";
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        ChillingGust ability = new ChillingGust((ChillingGust)action);
        return ability;
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton(); 
        base.ResolveAbility(chunk);
        PlayAnimation("White1", chunk);
        PlayAnimation("White2", chunk);
        if (IsAllegianceSame(chunk)) 
        { 
            ProtectedBuff buff = new ProtectedBuff(); 
            chunk.GetCurrentPlayer().buffManager.AddBuff(buff);
        }
        else 
        { 
            DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage); 
            SlowDebuff debuff = new SlowDebuff(1, 1, "IceSlow"); 
            chunk.GetCurrentPlayer().debuffManager.AddDebuff(debuff, _player);
        }
        
        FinishAbility();
    }
}
