using Rivencrestgodot._Eligijus.Scripts.BuffSystem;

public partial class PinkBarrier : BaseAction
{
    public PinkBarrier()
    {
        
    }
    
    public PinkBarrier(PinkBarrier ability): base(ability)
    {
        
    }
    
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        PinkBarrier ability = new PinkBarrier((PinkBarrier)action);
        return ability;
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        UpdateAbilityButton();
        Player target = chunk.GetCurrentPlayer();
        BarrierBuff buff = new BarrierBuff();
        target.buffManager.AddBuff(buff);
        target.AddMovementPoints(1);
        FinishAbility();
    }
}
