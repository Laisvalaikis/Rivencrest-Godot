using Godot;

public partial class Root : AbilityBlessing
{
    
    public Root()
    {
			
    }
    
    public Root(Root blessing): base(blessing)
    {
 
    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        Root blessing = new Root((Root)baseBlessing);
        return blessing;
    }
    
    public override BaseBlessing CreateNewInstance()
    {
        Root blessing = new Root(this);
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
                player.actionManager.RootPlayer();
                // GD.PrintErr("Fix can't move on player");
            }
        }
    }
}