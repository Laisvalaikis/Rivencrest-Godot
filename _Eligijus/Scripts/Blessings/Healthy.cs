using Godot;

public partial class Healthy : PlayerBlessing
{
    [Export] private int grantMaxHealth = 20;
    
    public Healthy()
    {
			
    }
    
    public Healthy(Healthy blessing): base(blessing)
    {
        grantMaxHealth = blessing.grantMaxHealth;
    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        Healthy blessing = new Healthy((Healthy)baseBlessing);
        return blessing;
    }
    
    public override BaseBlessing CreateNewInstance()
    {
        Healthy blessing = new Healthy(this);
        return blessing;
    }

    public override void Start(PlayerInformation playerInformation)
    {
        base.Start(playerInformation);
        playerInformation.AddHealth(grantMaxHealth);
    }
}