using System.Collections.Generic;
using Godot;

public partial class StunAttack : BaseAction
{
    [Export] private int spellDamage = 60;

    public StunAttack()
    {
    }

    public StunAttack(StunAttack ability): base(ability)
    {
        spellDamage = ability.spellDamage;
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
            GD.PrintErr("SMTH Happens");
            base.ResolveAbility(chunk);
            DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
            DealDamage(chunk, spellDamage, false);
            chunk.GetCurrentPlayer().debuffs.RootPlayer();
            FinishAbility();
        }
    }
    
}
