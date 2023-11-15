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
    
    public override void ResolveBlessing(BaseAction baseAction, ChunkData tile)
    {
        base.ResolveBlessing(baseAction, tile);
        if (baseAction.isAbilitySlow)
        {
            Player player = baseAction.GetPlayer();
            // restore movement points
            int previousMovementPoints = player.GetPreviousMovementPoints();
            player.SetMovementPoints(previousMovementPoints);
        }
    }

}