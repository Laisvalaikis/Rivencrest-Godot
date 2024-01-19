using Godot;

public partial class Swiftness : PlayerBlessing
{
    [Export] private int addMovementPoints = 5;
    public bool isTurnEven = false;
    public Swiftness()
    {
			
    }
    
    public Swiftness(Swiftness blessing): base(blessing)
    {
        addMovementPoints = blessing.addMovementPoints;
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

    // public override void OnTurnStart(BaseAction baseAction)
    // {
    //     base.OnTurnStart(baseAction);
    //     if (baseAction.TurnIsEven())
    //     {
    //         baseAction.GetPlayer().AddMovementPoints(addMovementPoints);
    //         Player player = baseAction.GetPlayer();
    //         player.AddMovementPoints(addMovementPoints);
    //     }
    // }
    public override void OnTurnStart(Player player)
    {
        base.OnTurnStart(player);
        if (isTurnEven)
        {
            isTurnEven = false;
        }
        else
        {
            isTurnEven = true;
            player.AddMovementPoints(addMovementPoints);
        }
    }
}