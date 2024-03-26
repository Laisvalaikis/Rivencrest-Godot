using Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class PowerShot : BaseAction
{
    public PowerShot()
    {
        
    }
    
    public PowerShot(PowerShot ability): base(ability)
    {
        
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        PowerShot ability = new PowerShot((PowerShot)action);
        return ability;
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
        base.ResolveAbility(chunk);
        PlayAnimation("Green1", chunk);
        PlayerAbilityAnimation();
        DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        SlowDebuff debuff = new SlowDebuff(1, 1, "IceSlow");
        chunk.GetCurrentPlayer().debuffManager.AddDebuff(debuff, _player);
        FinishAbility();
    }
}