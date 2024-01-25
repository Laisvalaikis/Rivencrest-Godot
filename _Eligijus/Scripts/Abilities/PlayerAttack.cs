using Godot;

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

    public override void ResolveAbility(ChunkData chunk)
    {
        if (CanTileBeClicked(chunk))
        {
            UpdateAbilityButton();
            base.ResolveAbility(chunk);
            DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
            FinishAbility();
        }
    }
}
