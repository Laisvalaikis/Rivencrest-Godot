namespace Rivencrestgodot._Eligijus.Scripts.BuffSystem;

public partial class BarrierBuff : BaseBuff
{
    public BarrierBuff()
    {
        
    }
    public BarrierBuff(BarrierBuff buff): base(buff)
    {
        
    }
    
    public override BaseBuff CreateNewInstance(BaseBuff baseBuff)
    {
        BarrierBuff buff = new BarrierBuff((BarrierBuff)baseBuff);
        return buff;
    }
    
    public override BaseBuff CreateNewInstance()
    {
        BarrierBuff buff = new BarrierBuff(this);
        return buff;
    }
    
    public override void Start()
    {

    }
    
    public override void ResolveBuff(ChunkData chunkData)
    {
        
    }
    
    public override void ModifyDamage(ref int damage, ref Player player)
    {
        damage = 0;
    }
}