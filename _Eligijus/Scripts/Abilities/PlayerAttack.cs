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
        _player.objectInformation.GetObjectInformation().animationPlayer.Play("Attack");
        UpdateAbilityButton();
        base.ResolveAbility(chunk);
        DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        FinishAbility();
        PlayAnimation("AflameExplosion", chunk);
        await Task.Delay(TimeSpan.FromSeconds(_player.objectInformation.GetObjectInformation().animationPlayer.GetAnimation("Attack").Length-0.1f));
        _player.objectInformation.GetObjectInformation().animationPlayer.Play("Idle");
    }
    
}
