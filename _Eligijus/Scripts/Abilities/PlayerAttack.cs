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
            //chunk.GetCurrentPlayer().DealDamage(500,_player);
            FinishAbility();
        }
    }

    public override bool CanTileBeClicked(ChunkData chunkData)
    {
        if (chunkData.GetTileHighlight().isHighlighted &&((CheckIfSpecificInformationType(chunkData, typeof(Player)) && !IsAllegianceSame(chunkData)) || friendlyFire)
                                                           || CheckIfSpecificInformationType(chunkData, typeof(Object)))
        {
            return true;
        }
        return false;
    }
    
}
