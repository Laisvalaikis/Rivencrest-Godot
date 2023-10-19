
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
    
    public override void Start(ref PlayerInformation playerInformation)
    {
        
    }
    
    public override void OnTurnStart(ref Player player)
    {
        base.OnTurnStart(ref player);
        player.GetPoisons().Clear();
    }
}