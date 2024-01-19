using System.Collections.Generic;
using Godot;
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

    public override void OnTurnStart(ChunkData chunkData)
    {
        base.OnTurnStart(chunkData);
        if (_protectedAlly != null)
        {
            _protectedAlly.objectInformation.GetPlayerInformation().characterProtected = false;
            _protectedAlly = null;
        }
    }
    public override void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
            base.ResolveAbility(chunk);
            Player target = chunk.GetCurrentPlayer();
            PlayerInformation clickedPlayerInformation = target.objectInformation.GetPlayerInformation();
            
            if (IsAllegianceSame(chunk))
            {
                clickedPlayerInformation.characterProtected = true;
                _protectedAlly = target;
            }
            else
            {
                DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
                SlowDebuff debuff = new SlowDebuff(1,1);
                chunk.GetCurrentPlayer().debuffManager.AddDebuff(debuff,_player);
            }
            FinishAbility();
    }
}
