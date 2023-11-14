using Godot;

public partial class Swiftness : PlayerBlessing
{
    [Export] private int addAbilityPoints = 1;
    
    public Swiftness()
    {
			
    }
    
    public Swiftness(Swiftness blessing): base(blessing)
    {
        addAbilityPoints = blessing.addAbilityPoints;
    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        Swiftness blessing = new Swiftness((Swiftness)baseBlessing);
        return blessing;
    }
    
    public override BaseBlessing CreateNewInstance()
    {
        Swiftness blessing = new Swiftness(this);
        return blessing;
    }
    
    public override void OnTurnStart(BaseAction baseAction)
    {
        base.OnTurnStart(baseAction);
        if (baseAction.TurnIsEven())
        {
            baseAction.GetPlayer().actionManager.AddAbilityPoints(addAbilityPoints);
        }
    }
}