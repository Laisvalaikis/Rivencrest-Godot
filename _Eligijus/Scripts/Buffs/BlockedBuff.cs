
namespace Rivencrestgodot._Eligijus.Scripts.BuffSystem;

public partial class BlockedBuff : BaseBuff
{
    public BlockedBuff()
    {
        
    }
    public BlockedBuff(BlockedBuff buff): base(buff)
    {
        
    }
    
    public override BaseBuff CreateNewInstance(BaseBuff baseBuff)
    {
        BlockedBuff buff = new BlockedBuff((BlockedBuff)baseBuff);
        return buff;
    }
    
    public override BaseBuff CreateNewInstance()
    {
        BlockedBuff buff = new BlockedBuff(this);
        return buff;
    }
    
    public override void Start()
    {

    }
    
    public override void ResolveBuff(ChunkData chunkData)
    {
        
    }
    
    public override void OnTurnStart()
    {
        base.OnTurnStart();
        lifetimeCount++;
    }
    
    public override void ModifyDamage(ref int damage)
    {
        damage = 0;
    }
}