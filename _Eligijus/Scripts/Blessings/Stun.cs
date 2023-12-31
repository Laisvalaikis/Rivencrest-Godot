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

    public override void ResolveBlessing(BaseAction baseAction, ChunkData tile)
    {
        base.ResolveBlessing(baseAction, tile);
        if (tile.CharacterIsOnTile())
        {
            Player player = tile.GetCurrentPlayer();
            if (!IsAllegianceSame(baseAction.GetPlayer(), tile, baseAction))
            {
                // Create Can't move
                // player.debuffs.StunPlayer();
                // GD.PrintErr("Fix can't do anything");
            }
        }
    }
}