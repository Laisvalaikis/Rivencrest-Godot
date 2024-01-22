using Godot;

public partial class HeadStart : PlayerBlessing
{
    [Export] private int addMovementPoints = 2;
    private bool blessingUsed = false;
    private Player player;
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
    public override void Start(PlayerInformation playerInformation)
    {
       // base.Start(playerInformation);
        //player.AddMovementPoints(addMovementPoints);
    }
    public override void OnTurnStart(Player player)
    {
        base.OnTurnStart(player);
        GD.Print("WORKS");
        if (!blessingUsed)
        {
            player.AddMovementPoints(addMovementPoints);
            blessingUsed = true;
        }
    }
    
}