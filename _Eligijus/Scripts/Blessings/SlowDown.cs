using Godot;

public partial class SlowDown : AbilityBlessing
{
    [Export] private int slowsDown = 2;
    
    public SlowDown()
    {
			
    }
    
    public SlowDown(SlowDown blessing): base(blessing)
    {
        slowsDown = blessing.slowsDown;
    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        SlowDown blessing = new SlowDown((SlowDown)baseBlessing);
        return blessing;
    }
    
    public override BaseBlessing CreateNewInstance()
    {
        SlowDown blessing = new SlowDown(this);
        return blessing;
    }

    public override void ResolveBlessing(ref BaseAction baseAction, ChunkData tile)
    {
        base.ResolveBlessing(ref baseAction, tile);
        // Need to redo abilityPoints on player
        GD.PrintErr("Redo ability points on player");
        tile.GetCurrentPlayer().actionManager.AddAbilityPoints(-slowsDown);
    }
    
}