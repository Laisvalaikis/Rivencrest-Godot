namespace Rivencrestgodot._Eligijus.Scripts.BuffSystem;

public partial class ProtectedBuff : BaseBuff

{
    public ProtectedBuff()
    {
        
    }
    public ProtectedBuff(ProtectedBuff buff): base(buff)
    {
        
    }
    
    public override BaseBuff CreateNewInstance(BaseBuff baseBuff)
    {
        ProtectedBuff buff = new ProtectedBuff((ProtectedBuff)baseBuff);
        return buff;
    }
    
    public override BaseBuff CreateNewInstance()
    {
        ProtectedBuff buff = new ProtectedBuff(this);
        return buff;
    }
    
    public override void ResolveBuff(ChunkData chunkData)
    {
        
    }
    
    public override void ModifyDamage(ref int damage, ref Player player)
    {
        damage /= 2 ;
    }
}