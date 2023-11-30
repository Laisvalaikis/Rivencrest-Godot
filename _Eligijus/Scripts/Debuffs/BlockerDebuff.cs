namespace Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class BlockerDebuff : BaseDebuff
{
    
    //Takes damage from enemy attacks
    //todo: finish this when player middleware exists
    public BlockerDebuff()
    {
        
    }
    public BlockerDebuff(BlockerDebuff debuff): base(debuff)
    {
        
    }
    
    public override BaseDebuff CreateNewInstance(BaseDebuff baseDebuff)
    {
        BlockerDebuff debuff = new BlockerDebuff((BlockerDebuff)baseDebuff);
        return debuff;
    }
    
    public override BaseDebuff CreateNewInstance()
    {
        BlockerDebuff debuff = new BlockerDebuff(this);
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