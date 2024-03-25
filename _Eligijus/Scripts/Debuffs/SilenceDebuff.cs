namespace Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class SilenceDebuff : BaseDebuff
{
    public SilenceDebuff()
    {
			
    }
    
    public SilenceDebuff(int lifetime)
    {
        this.lifetime = lifetime;
        debuffAnimation = "Silenced";
    }
    
    public SilenceDebuff(SilenceDebuff debuff): base(debuff)
    {
        
    }
    
    public override BaseDebuff CreateNewInstance(BaseDebuff baseDebuff)
    {
        SilenceDebuff debuff = new SilenceDebuff((SilenceDebuff)baseDebuff);
        return debuff;
    }
    
    public override BaseDebuff CreateNewInstance()
    {
        SilenceDebuff debuff = new SilenceDebuff(this);
        return debuff;
    }
    
    public override void OnTurnStart()
    {
        base.OnTurnStart();
        _player.actionManager.SetAbilityPoints(0);
    }
}