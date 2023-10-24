using Godot;

public partial class Agility : AbilityBlessing
{
    
    public Agility()
    {
		
    }
    public Agility(Agility blessing): base(blessing)
    {
  
    }
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        Agility blessing = new Agility((Agility)baseBlessing);
        return blessing;
    }
    public override BaseBlessing CreateNewInstance()
    {
        Agility blessing = new Agility(this);
        return blessing;
    }
    
    public override void ResolveBlessing(ref BaseAction baseAction, ChunkData tile)
    {
        base.ResolveBlessing(ref baseAction, tile);
        if (!tile.CharacterIsOnTile())
        {
            Player player = baseAction.GetPlayer();
            // restore movement points
            if (player.actionManager.IsMovementSelected())
            {
                player.actionManager.ResetAbilityPointsBeforeAbility();
            }
        }
    }

}