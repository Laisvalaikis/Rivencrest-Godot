public partial class FlameKick : BaseAction
{
    public FlameKick()
    {
 		
    }
    public FlameKick(FlameKick flameBlast): base(flameBlast)
    {
    }
 	
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        FlameKick flameBlast = new FlameKick((FlameKick)action);
        return flameBlast;
    }
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        DealRandomDamageToTarget(chunk,minAttackDamage,maxAttackDamage);
        FinishAbility();
    }
}