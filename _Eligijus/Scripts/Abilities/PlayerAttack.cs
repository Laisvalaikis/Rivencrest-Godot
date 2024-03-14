using Godot;
using System;
using System.Threading.Tasks;


public partial class PlayerAttack : BaseAction
{
    public PlayerAttack()
    {
        
    }

    public PlayerAttack(PlayerAttack ability): base(ability)
    {

    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        PlayerAttack ability = new PlayerAttack((PlayerAttack)action);
        return ability;
    }

    public override async void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
        base.ResolveAbility(chunk);
        DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        FinishAbility();
        await PlayAnimation("AflameExplosion", chunk);
    }
    
}
