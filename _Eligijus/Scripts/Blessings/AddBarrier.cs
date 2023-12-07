using Godot;
using Rivencrestgodot._Eligijus.Scripts.BuffSystem;

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
    
    public override BaseBlessing CreateNewInstance()
    {
        AddBarrier blessing = new AddBarrier(this);
        return blessing;
    }
    
    public override void ResolveBlessing(BaseAction baseAction, ChunkData tile)
    {
        base.ResolveBlessing(baseAction, tile);
        BarrierBuff buff = new BarrierBuff();
        tile.GetCurrentPlayer().buffManager.AddBuff(buff);
    }
}