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
            base.ResolveAbility(chunk);
            DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
            FinishAbility();
        }
    }

    public override bool CanTileBeClicked(ChunkData chunk)
    {
        if (((CheckIfSpecificInformationType(chunk, InformationType.Player) || CheckIfSpecificInformationType(chunk, InformationType.Object))
            && !IsAllegianceSame(chunk)) || friendlyFire)
        {
            return true;
        }
        return false;
    }
    
}
