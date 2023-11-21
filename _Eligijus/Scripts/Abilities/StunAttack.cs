using System.Collections.Generic;
using Godot;

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
        if (CanTileBeClicked(chunk))
        {
            UpdateAbilityButton();
            GD.PrintErr("SMTH Happens");
            base.ResolveAbility(chunk);
            DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
            DealDamage(chunk, spellDamage);
            BaseDebuff debuff = _debuff.CreateNewInstance();
            chunk.GetCurrentPlayer().debuffManager.AddDebuff(debuff);
            FinishAbility();
        }
    }
    
}
