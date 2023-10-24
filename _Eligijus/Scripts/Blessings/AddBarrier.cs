using Godot;

public partial class AddBarrier : AbilityBlessing
{
    public AddBarrier()
    {
			
    }
    
    public AddBarrier(AddBarrier blessing): base(blessing)
    {
        
    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        AddBarrier blessing = new AddBarrier((AddBarrier)baseBlessing);
        return blessing;
    }
    
    public override void ResolveBlessing(ref BaseAction baseAction, ChunkData tile)
    {
        base.ResolveBlessing(ref baseAction, tile);
        baseAction.GetPlayer().AddBarrier();
    }
}