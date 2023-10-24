using Godot;

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
        if (chunk.IsStandingOnChunk())
        {
            Player player = chunk.GetCurrentPlayer();
            DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
            player.AddPoison(new Poison(chunk, 2, 2));
        }

        FinishAbility();
    }
}