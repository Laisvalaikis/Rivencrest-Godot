using System.Collections.Generic;
using Godot;

public partial class StunAttack : BaseAction
{
    

    public StunAttack()
    {
    }

    public StunAttack(StunAttack ability): base(ability)
    {
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
            base.ResolveAbility(chunk);
            DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
            FinishAbility();
        }
    }
    
}
