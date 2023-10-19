using Godot;

public partial class HeadStart : AbilityBlessing
{
    [Export] private int addMovementPoints = 2;
    private bool blessingUsed = false;
    public HeadStart()
    {
			
    }
    
    public HeadStart(HeadStart blessing): base(blessing)
    {
        addMovementPoints = blessing.addMovementPoints;
    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        HeadStart blessing = new HeadStart((HeadStart)baseBlessing);
        return blessing;
    }
    
    public override BaseBlessing CreateNewInstance()
    {
        HeadStart blessing = new HeadStart(this);
        return blessing;
    }

    public override void OnTurnStart(ref BaseAction baseAction)
    {
        base.OnTurnStart(ref baseAction);
        if (!blessingUsed)
        {
            baseAction.GetPlayer().actionManager.AddAbilityPoints(addMovementPoints);
            blessingUsed = true;
        }
    }
    
}