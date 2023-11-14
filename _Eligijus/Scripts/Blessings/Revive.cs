using Godot;

public partial class Revive : PlayerBlessing
{
    public Revive()
    {
			
    }
    
    public Revive(Revive blessing): base(blessing)
    {
        
    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        Revive blessing = new Revive((Revive)baseBlessing);
        return blessing;
    }
    
    public override void Start(PlayerInformation playerInformation)
    {
        playerInformation.Revive();
    }
    
}