using Godot;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

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

    protected override void ModifyBonusDamage(ChunkData chunk)
    {
        if (chunk.GetCurrentPlayer().debuffManager.ContainsDebuff(typeof(SlowDebuff)))
        {
            bonusDamage += rootDamage;
        }
    }

    public override void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
        base.ResolveAbility(chunk);
        if (chunk.CharacterIsOnTile())
        {
            Player target = chunk.GetCurrentPlayer();
            if (target.debuffManager.ContainsDebuff(typeof(SlowDebuff)))
            {
                RootDebuff rootDebuff = new RootDebuff();
                target.debuffManager.AddDebuff(rootDebuff,_player);
            }
        }
        PlayerAbilityAnimation();
        DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        SlowDebuff debuff = new SlowDebuff(2, 2, "IceSlow");
        chunk.GetCurrentPlayer().debuffManager.AddDebuff(debuff,_player);
        FinishAbility();
    }
}