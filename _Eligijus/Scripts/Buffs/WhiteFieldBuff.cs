namespace Rivencrestgodot._Eligijus.Scripts.BuffSystem;

public partial class WhiteFieldBuff: BaseBuff
{
    public WhiteFieldBuff()
    {
        
    }
    public WhiteFieldBuff(WhiteFieldBuff buff): base(buff)
    {
        
    }
    
    public override BaseBuff CreateNewInstance(BaseBuff baseBuff)
    {
        WhiteFieldBuff buff = new WhiteFieldBuff((WhiteFieldBuff)baseBuff);
        return buff;
    }
    
    public override BaseBuff CreateNewInstance()
    {
        WhiteFieldBuff buff = new WhiteFieldBuff(this);
        return buff;
    }
    
    public override void OnTurnStart()
    {
        base.OnTurnStart();
    }
    
    public override void ResolveBuff(ChunkData chunkData)
    {
        
    }
    
    public override void ModifyDamage(ref int damage, ref Player player)
    {
        damage /= 2 ;
    }
}