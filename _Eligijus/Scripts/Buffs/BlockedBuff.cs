namespace Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class BlockedDebuff : BaseDebuff
{
    public BlockedDebuff()
    {
        
    }
    public BlockedDebuff(BlockedDebuff debuff): base(debuff)
    {
        
    }
    
    public override BaseDebuff CreateNewInstance(BaseDebuff baseDebuff)
    {
        BlockedDebuff debuff = new BlockedDebuff((BlockedDebuff)baseDebuff);
        return debuff;
    }
    
    public override BaseDebuff CreateNewInstance()
    {
        BlockedDebuff debuff = new BlockedDebuff(this);
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
        lifetimeCount++;
    }
}