namespace Rivencrestgodot._Eligijus.Scripts.BuffSystem;

public partial class StasisBuff : BaseBuff
{
    public StasisBuff()
    {
        
    }
    public StasisBuff(StasisBuff buff): base(buff)
    {
        
    }
    
    public override BaseBuff CreateNewInstance(BaseBuff baseBuff)
    {
        StasisBuff buff = new StasisBuff((StasisBuff)baseBuff);
        return buff;
    }
    
    public override BaseBuff CreateNewInstance()
    {
        StasisBuff buff = new StasisBuff(this);
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
        damage /= 2 ;
    }
}