namespace Rivencrestgodot._Eligijus.Scripts.Debuffs;

//Reduces ability points of player by (two/2?)
public partial class SlowDebuff : BaseDebuff
{
    public SlowDebuff()
    {
			
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
        _player.AddMovementPoints(-2);
        lifetimeCount++;
    }
}