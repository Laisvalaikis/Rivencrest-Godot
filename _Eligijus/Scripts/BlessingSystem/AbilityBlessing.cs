using Godot;

public partial class AbilityBlessing : BaseBlessing
{
	
    public AbilityBlessing()
    {
			
    }
    
    public AbilityBlessing(AbilityBlessing blessing): base(blessing)
    {
	    
    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
	    AbilityBlessing blessing = new AbilityBlessing((AbilityBlessing)baseBlessing);
	    return blessing;
    }
    
    public override BaseBlessing CreateNewInstance()
    {
	    AbilityBlessing blessing = new AbilityBlessing(this);
	    return blessing;
    }
    
    public override void Start(ref BaseBlessing baseBlessing)
    {
	    base.Start(ref baseBlessing);
    }
    
    public override void ResolveBlessing(ref BaseAction baseAction)
    {
        base.ResolveBlessing(ref baseAction);
    }
    
    public override void OnTurnStart(ref BaseAction baseAction)
    {
	    base.OnTurnStart(ref baseAction);
    }

    public override void OnTurnEnd(ref BaseAction baseAction)
    {
	    base.OnTurnEnd(ref baseAction);
    }
}