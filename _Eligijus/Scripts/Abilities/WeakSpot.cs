using System.Diagnostics;
using Godot;

public partial class WeakSpot : BaseAction
{ 
    public WeakSpot()
    {
    }

    public WeakSpot(WeakSpot ability): base(ability)
    {
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        WeakSpot ability = new WeakSpot((WeakSpot)action);
        return ability;
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        if (chunk.CharacterIsOnTile())
        {
            chunk.GetCurrentPlayer().AddWeakSpot();
        }

        FinishAbility();
    }
    
}
