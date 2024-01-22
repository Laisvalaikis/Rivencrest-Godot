using Godot;

public partial class ReduceCooldown : AbilityBlessing
{
    [Export] private int _reduceCooldown = 2;
    
    public ReduceCooldown()
    {
			
    }
    
    public ReduceCooldown(ReduceCooldown blessing): base(blessing)
    {
        _reduceCooldown = blessing._reduceCooldown;
    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        ReduceCooldown blessing = new ReduceCooldown((ReduceCooldown)baseBlessing);
        return blessing;
    }
    
    public override BaseBlessing CreateNewInstance()
    {
        ReduceCooldown blessing = new ReduceCooldown(this);
        return blessing;
    }

    public override void ResolveBlessing(BaseAction baseAction, ChunkData tile)
    {
        base.ResolveBlessing(baseAction, tile);
        if (tile.GetCurrentPlayer() != null)
        {
            Player player = tile.GetCurrentPlayer();
            if (IsAllegianceSame(baseAction.GetPlayer(), tile, baseAction))
            {
                baseAction.GetPlayer().actionManager.RefillActionPoints();
            }
        }
    }
}