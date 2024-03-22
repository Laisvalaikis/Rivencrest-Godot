using System.Collections.Generic;
using Godot;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class StunAttack : BaseAction
{
    [Export] private int spellDamage = 60;
    [Export] private BaseDebuff _debuff;
    public StunAttack()
    {
    }

    public StunAttack(StunAttack ability): base(ability)
    {
        spellDamage = ability.spellDamage;
        _debuff = ability._debuff;
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        StunAttack ability = new StunAttack((StunAttack)action);
        return ability;
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
        base.ResolveAbility(chunk);
        PlayAnimation("Burgundy1", chunk);
        DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        DealDamage(chunk, spellDamage);
        RootDebuff debuff = new RootDebuff(1);
        chunk.GetCurrentPlayer().debuffManager.AddDebuff(debuff, _player);
        FinishAbility();
    }
}
