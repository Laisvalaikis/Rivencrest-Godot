
public partial class Antitoxic : PlayerBlessing
{
    public Antitoxic()
    {
			
    }
    
    public Antitoxic(Antitoxic blessing): base(blessing)
    {

    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        Antitoxic blessing = new Antitoxic((Antitoxic)baseBlessing);
        return blessing;
    }
    
    public override BaseBlessing CreateNewInstance()
    {
        Antitoxic blessing = new Antitoxic(this);
        return blessing;
    }
    
    public override void Start(PlayerInformation playerInformation)
    {
        
    }
    
    public override void OnTurnStart(Player player)
    {
        base.OnTurnStart(player);
        player.GetPoisons().Clear();
    }
}