using Godot;

public partial class Stun : AbilityBlessing
{
    public Stun()
    {
			
    }
    
    public Stun(Stun blessing): base(blessing)
    {
 
    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        Stun blessing = new Stun((Stun)baseBlessing);
        return blessing;
    }
    
    public override BaseBlessing CreateNewInstance()
    {
        Stun blessing = new Stun(this);
        return blessing;
    }

    public override void ResolveBlessing(ref BaseAction baseAction, ChunkData tile)
    {
        base.ResolveBlessing(ref baseAction, tile);
        if (tile.CharacterIsOnTile())
        {
            Player player = tile.GetCurrentPlayer();
            if (!IsAllegianceSame(baseAction.GetPlayer().playerInformation, tile, baseAction))
            {
                // Create Can't move
                GD.PrintErr("Fix can't do anything");
            }
        }
    }
}