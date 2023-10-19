using Godot;

public partial class OilSlow : BaseAction
{
    public OilSlow()
    {
        
    }
    public OilSlow(OilSlow ability): base(ability)
    {
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        OilSlow ability = new OilSlow((OilSlow)action);
        return ability;
    }
    
    public override void OnTurnStart()
    {

    }
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        FinishAbility();

        
    }
}
