using Rivencrestgodot._Eligijus.Scripts.Debuffs;

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
    
    public override void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
        base.ResolveAbility(chunk);
        PlayerAbilityAnimation();
        PlayAnimation("Lime1", chunk);
        DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        SlowDebuff debuff = new SlowDebuff(1, 2, "OilSlow");
        chunk.GetCurrentPlayer().debuffManager.AddDebuff(debuff, _player);
        FinishAbility();
    }
}
