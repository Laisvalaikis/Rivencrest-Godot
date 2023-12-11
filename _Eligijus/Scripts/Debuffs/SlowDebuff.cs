namespace Rivencrestgodot._Eligijus.Scripts.Debuffs;

//Reduces movement points of player by (two/2?)
public partial class SlowDebuff : BaseDebuff
{
    private int slowBy=2;
    public SlowDebuff()
    {
			
    }
    
    public SlowDebuff(int lifetime, int slowBy)
    {
        this.lifetime = lifetime;
        this.slowBy = slowBy;
    }
    
    public SlowDebuff(SlowDebuff debuff): base(debuff)
    {
        
    }
    
    public override BaseDebuff CreateNewInstance(BaseDebuff baseDebuff)
    {
        SlowDebuff debuff = new SlowDebuff((SlowDebuff)baseDebuff);
        return debuff;
    }
    
    public override BaseDebuff CreateNewInstance()
    {
        SlowDebuff debuff = new SlowDebuff(this);
        return debuff;
    }
	
    public override void Start()
    {

    }
    
    public override void OnTurnStart()
    {
        base.OnTurnStart();
        //_player.AddMovementPoints(slowBy*(-1));
        _player.AddMovementPoints(slowBy*(-1));
        lifetimeCount++;
    }
}