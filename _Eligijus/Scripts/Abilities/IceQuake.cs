using Godot;

public partial class IceQuake : BaseAction
{
    [Export] private int rootDamage = 5;
    public IceQuake()
    {
        
    }
    
    public IceQuake(IceQuake ability): base(ability)
    {
        rootDamage = ability.rootDamage;
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        IceQuake ability = new IceQuake((IceQuake)action);
        return ability;
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        if (CanTileBeClicked(chunk))
        {
            UpdateAbilityButton();
            base.ResolveAbility(chunk);
            int bonusDamage = 0;
            if (chunk.IsStandingOnChunk())
            {
                Player target = chunk.GetCurrentPlayer();
                if (target.debuffs.IsPlayerSlower())
                {
                    target.actionManager.RemoveSlowDown();
                    bonusDamage += rootDamage;
                }
            }

            DealRandomDamageToTarget(chunk, minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
            FinishAbility();
        }
    }
}
    

