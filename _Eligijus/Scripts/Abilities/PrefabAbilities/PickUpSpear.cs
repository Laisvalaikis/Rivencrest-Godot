using Godot;

public partial class PickUpSpear : BaseAction
{
    public PickUpSpear()
    {
        
    }

    public PickUpSpear(PickUpSpear ability): base(ability)
    {
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        PickUpSpear ability = new PickUpSpear((PickUpSpear)action);
        return ability;
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        if (CanTileBeClicked(chunk) && chunk.CharacterIsOnTile())
        {
            base.ResolveAbility(chunk);
            // reset spear cooldown
            Player tempPlayer = chunk.GetCurrentPlayer();
            BaseAction action = tempPlayer.actionManager.GetAbilityByType(typeof(ThrowSpear));
            if (action is not null)
            {
                action.IncreaseAbilityCooldown();
                action.UpdateAbilityButtonByActionPoints(tempPlayer.actionManager.GetAbilityPoints());
                _object.Death();
            }

            FinishAbility();
        }
    }
    
    public override void OnTurnStart(ChunkData chunkData)
    {
        base.OnTurnStart(chunkData);
    }

    public override bool CanTileBeClicked(ChunkData chunkData)
    {
        if (CheckIfSpecificInformationType(chunkData, typeof(Player)) 
            || CheckIfSpecificInformationType(chunkData, typeof(Object)))
        {
            return true;
        }
        return false;
    }
    
    public override bool IsAllegianceSame(ChunkData chunk)
    {
        return false;
    }
    
}