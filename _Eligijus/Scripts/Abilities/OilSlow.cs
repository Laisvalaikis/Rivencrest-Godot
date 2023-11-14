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
    
    public override void OnTurnStart(ChunkData chunkData)
    {
        base.OnTurnStart(chunkData);
    }
    public override void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
        base.ResolveAbility(chunk);
        DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        chunk.GetCurrentPlayer().AddMovementPoints(-2);
        FinishAbility();

        
    }
}
