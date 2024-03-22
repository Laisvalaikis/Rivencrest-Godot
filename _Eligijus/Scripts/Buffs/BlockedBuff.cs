
namespace Rivencrestgodot._Eligijus.Scripts.BuffSystem;

public partial class BlockedBuff : BaseBuff
{
    private Player playerWhoTakesDamage;
    public BlockedBuff()
    {
        
    }
    public BlockedBuff(Player playerWhoTakesDamage)
    {
        this.playerWhoTakesDamage = playerWhoTakesDamage;
        buffAnimation = "Blocked";
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
    
    public override void ModifyDamage(ref int damage,ref Player player)
    {
        player = playerWhoTakesDamage;
    }
}