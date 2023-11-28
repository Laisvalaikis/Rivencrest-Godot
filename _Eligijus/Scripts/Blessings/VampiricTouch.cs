using Godot;

public partial class VampiricTouch : AbilityBlessing
{

    [Export] private int lifeSteal = 3;
    
    public VampiricTouch()
    {
			
    }
    
    public VampiricTouch(VampiricTouch blessing): base(blessing)
    {
        lifeSteal = blessing.lifeSteal;
    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        VampiricTouch blessing = new VampiricTouch((VampiricTouch)baseBlessing);
        return blessing;
    }
    
    public override BaseBlessing CreateNewInstance()
    {
        VampiricTouch blessing = new VampiricTouch(this);
        return blessing;
    }
    
    public override void ResolveBlessing(BaseAction baseAction, ChunkData tile)
    {
        base.ResolveBlessing(baseAction);
        baseAction.GetPlayer().objectInformation.GetPlayerInformation().Heal(lifeSteal);
    }
    
}