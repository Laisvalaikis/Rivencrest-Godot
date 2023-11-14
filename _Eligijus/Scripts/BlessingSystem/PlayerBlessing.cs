using Godot;

public partial class PlayerBlessing : BaseBlessing
{
    public PlayerBlessing()
    {
			
    }
    
    public PlayerBlessing(PlayerBlessing blessing): base(blessing)
    {
	    
    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        PlayerBlessing blessing = new PlayerBlessing((PlayerBlessing)baseBlessing);
        return blessing;
    }

    public virtual void OnTurnStart(Player player)
    {
        
    }

    public virtual void Start(PlayerInformation playerInformation)
    {
        
    }
}