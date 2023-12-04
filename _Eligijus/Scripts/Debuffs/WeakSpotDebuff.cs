namespace Rivencrestgodot._Eligijus.Scripts.Debuffs;

public class WeakSpotDebuff : BaseDebuff
{
    public WeakSpotDebuff()
    {
			
    }
    
    public WeakSpotDebuff(WeakSpotDebuff debuff): base(debuff)
    {
        
    }
    
    public override BaseDebuff CreateNewInstance(BaseDebuff baseDebuff)
    {
        WeakSpotDebuff debuff = new WeakSpotDebuff((WeakSpotDebuff)baseDebuff);
        return debuff;
    }
    
    public override BaseDebuff CreateNewInstance()
    {
        WeakSpotDebuff debuff = new WeakSpotDebuff(this);
        return debuff;
    }

    public override void PlayerWasAttacked()
    {
        _player.playerActionMiddleware.DealDamage(1,playerWhoCreatedDebuff);
    }

}