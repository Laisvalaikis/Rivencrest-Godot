using System.Collections.Generic;
using Godot;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

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
        UpdateAbilityButton();
        base.ResolveAbility(chunk);
        DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        if (chunk.CharacterIsOnTile())
        {
            Player target = chunk.GetCurrentPlayer();
            PoisonDebuff debuff = new PoisonDebuff(2,2);
            chunk.GetCurrentPlayer().debuffManager.AddDebuff(debuff,player);
            debuff.SetPoisonDebuffDamage(_poisonDamage);
            debuff.SetLifetime(_poisonTurns);
            target.debuffManager.AddDebuff(debuff, player);
        }
        FinishAbility();
    }
}



