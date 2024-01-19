using Godot;

public partial class RangeIncrease : AbilityBlessing
{

    [Export] private int increasRange = 1;
    public bool increased =true;
    
    public RangeIncrease()
    {
			
    }
    
    public RangeIncrease(RangeIncrease blessing): base(blessing)
    {
        increasRange = blessing.increasRange;
    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        RangeIncrease blessing = new RangeIncrease((RangeIncrease)baseBlessing);
        return blessing;
    }
    
    public override BaseBlessing CreateNewInstance()
    {
        RangeIncrease blessing = new RangeIncrease(this);
        return blessing;
    }

    public override void OnTurnStart(BaseAction baseAction)
    {
        base.OnTurnStart(baseAction);
        if (increased)
        {
            baseAction.IncreaseRange(increasRange);
            increased = false;
        }
    }
}