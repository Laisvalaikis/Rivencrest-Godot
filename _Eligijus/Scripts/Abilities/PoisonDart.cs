using System.Collections.Generic;
using Godot;

public partial class PoisonDart : BaseAction
{
    [Export] private int _poisonTurns = 2;
    [Export] private int _poisonDamage = 2;
    public PoisonDart()
    {
        
    }
    public PoisonDart(PoisonDart ability): base(ability)
    {
        _poisonTurns = ability._poisonTurns;
        _poisonDamage = ability._poisonDamage;
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        PoisonDart ability = new PoisonDart((PoisonDart)action);
        return ability;
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        if (chunk.CharacterIsOnTile())
        {
            Player target = chunk.GetCurrentPlayer();
            target.AddPoison(new Poison(chunk, _poisonTurns,_poisonDamage));
        }

        FinishAbility();
    }
}



