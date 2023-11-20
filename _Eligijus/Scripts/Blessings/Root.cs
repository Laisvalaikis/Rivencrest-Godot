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

    public override void ResolveBlessing(BaseAction baseAction, ChunkData tile)
    {
        base.ResolveBlessing(baseAction, tile);
        if (tile.CharacterIsOnTile())
        {
            Player player = tile.GetCurrentPlayer();
          //  if (!IsAllegianceSame(baseAction.GetPlayer(), tile, baseAction))
           // {
                // Create Can't move
                player.debuffs.RootPlayer();
                // GD.PrintErr("Fix can't move on player");
          //  }
        }
    }
}