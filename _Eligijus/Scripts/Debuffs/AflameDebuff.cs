namespace Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class AflameDebuff : BaseDebuff
{
    public AflameDebuff()
    {
			
    }
    
    public AflameDebuff(AflameDebuff debuff): base(debuff)
    {
        
    }
    
    public override BaseDebuff CreateNewInstance(BaseDebuff baseDebuff)
    {
        AflameDebuff debuff = new AflameDebuff((AflameDebuff)baseDebuff);
        return debuff;
    }
    
    public override BaseDebuff CreateNewInstance()
    {
        AflameDebuff debuff = new AflameDebuff(this);
        return debuff;
    }
	
    public override void Start()
    {

    }
    
    public override void OnTurnStart()
    {
        base.OnTurnStart();
        lifetimeCount++;
    }
}