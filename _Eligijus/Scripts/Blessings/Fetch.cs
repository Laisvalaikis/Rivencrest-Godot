using Godot;

public partial class Fetch : AbilityBlessing
{
    public Fetch()
    {
			
    }
	
    public Fetch(Fetch blessing): base(blessing)
    {
    }
	
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        Fetch blessing = new Fetch((Fetch)baseBlessing);
        return blessing;
    }
	
    public override BaseBlessing CreateNewInstance()
    {
        Fetch blessing = new Fetch(this);
        return blessing;
    }
    
    public override void ResolveBlessing(ref BaseAction baseAction, ChunkData tile)
    {
        base.ResolveBlessing(ref baseAction);
        tile.GetCurrentPlayerInformation().DealDamage(1, false, baseAction.GetPlayer());
    }

}