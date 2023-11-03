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
        if (chunk.CharacterIsOnTile())
        {
            Player target = chunk.GetCurrentPlayer();
            DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
            //target.AddPoison(new Poison(chunk, 2, 2)); //assumming that poisonValue = 2 is "disarmed" debuff
            target.debuffs.SilencePlayer(); // cia gal disarmas?
        }
        FinishAbility();
    }
}