
public partial class Purify : BaseAction
{
    public Purify()
    {
        
    }
    
    public Purify(Purify ability): base(ability)
    {
        
    }
    
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        Purify ability = new Purify((Purify)action);
        return ability;
    }
 
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        UpdateAbilityButton();
        Player target = chunk.GetCurrentPlayer();
        target.debuffManager.RemoveDebuffs();
    }
}
