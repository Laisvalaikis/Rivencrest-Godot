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
        DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        SlowDebuff debuff = new SlowDebuff(1, 1);
        chunk.GetCurrentPlayer().debuffManager.AddDebuff(debuff, _player);
        FinishAbility();
    }
}