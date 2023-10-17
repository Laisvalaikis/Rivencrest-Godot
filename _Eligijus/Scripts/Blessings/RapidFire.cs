using Godot;

public partial class RapidFire : AbilityBlessing
{
    public RapidFire()
    {
			
    }
    
    public RapidFire(RapidFire blessing): base(blessing)
    {
   
    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        RapidFire blessing = new RapidFire((RapidFire)baseBlessing);
        return blessing;
    }
    
    public override BaseBlessing CreateNewInstance()
    {
        RapidFire blessing = new RapidFire(this);
        return blessing;
    }
    
    public override void OnTurnStart(ref BaseAction baseAction)
    {
        base.OnTurnStart(ref baseAction);
        baseAction.isAbilitySlow = false;
    }
}