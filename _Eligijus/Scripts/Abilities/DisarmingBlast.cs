public partial class DisarmingBlast : BaseAction   //jei cia tik A.I ability gal ir nereikia to OnTileHover aurio pamastymai
{
    public DisarmingBlast()
    {
 		
    }
    public DisarmingBlast(DisarmingBlast disarmingBlast): base(disarmingBlast)
    {
    }
 	
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        DisarmingBlast disarmingBlast = new DisarmingBlast((DisarmingBlast)action);
        return disarmingBlast;
    }
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        FinishAbility();
    }
}