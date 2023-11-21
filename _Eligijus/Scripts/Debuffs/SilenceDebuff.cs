namespace Rivencrestgodot._Eligijus.Scripts.Debuffs;


//Player cannot use abilities except movement and basic attack (?)
public partial class SilenceDebuff : BaseDebuff
{
    public SilenceDebuff()
    {
			
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
	
    public override void Start()
    {
        
    }
    
    public override void ResolveDeBuff(ChunkData chunkData)
    {
        
    }
    
    public override void OnTurnStart()
    {
        base.OnTurnStart();
        _player.actionManager.SetAbilityPoints(0);
        lifetimeCount++;
    }
}