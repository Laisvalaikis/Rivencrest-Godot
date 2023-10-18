using Godot;

public partial class GainMP : AbilityBlessing
{
    [Export] private int affectMP = 1;
    
    public GainMP()
    {
			
    }
    
    public GainMP(GainMP blessing): base(blessing)
    {
        affectMP = blessing.affectMP;
    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        GainMP blessing = new GainMP((GainMP)baseBlessing);
        return blessing;
    }
    
    public override BaseBlessing CreateNewInstance()
    {
        GainMP blessing = new GainMP(this);
        return blessing;
    }

    public override void ResolveBlessing(ref BaseAction baseAction, ChunkData tile)
    {
        base.ResolveBlessing(ref baseAction, tile);
        baseAction.AddAbilityPoints(affectMP);
    }
}