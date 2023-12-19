namespace Rivencrestgodot._Eligijus.Scripts.Debuffs;

//Reduces movement points of player by (two/2?)
public partial class StasisDebuff : BaseDebuff
{
    private int slowBy=2;
    public StasisDebuff()
    {
			
    }
    
    public StasisDebuff(int lifetime, int slowBy)
    {
        this.lifetime = lifetime;
        this.slowBy = slowBy;
    }
    
    public StasisDebuff(StasisDebuff debuff): base(debuff)
    {
        
    }
    
    public override BaseDebuff CreateNewInstance(BaseDebuff baseDebuff)
    {
        StasisDebuff debuff = new StasisDebuff((StasisDebuff)baseDebuff);
        return debuff;
    }
    
    public override BaseDebuff CreateNewInstance()
    {
        StasisDebuff debuff = new StasisDebuff(this);
        return debuff;
    }
	
    public override void Start()
    {

    }
    
    public override void OnTurnStart()
    {
        base.OnTurnStart();
        _player.SetMovementPoints(0);
        _player.actionManager.RemoveAllActionPoints();
    }
}