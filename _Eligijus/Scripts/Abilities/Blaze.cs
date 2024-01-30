using System.Collections.Generic;
using Godot;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class Blaze : BaseAction
{
    public Blaze()
    {

    }

    public Blaze(Blaze blaze) : base(blaze)
    {
        bonusDamage = blaze.bonusDamage;
    }
	
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        Blaze blaze = new Blaze((Blaze)action);
        return blaze;
    }

    protected override void ModifyBonusDamage(ChunkData chunk)
    {
        bonusDamage = 4;
    }
    public override void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
        base.ResolveAbility(chunk); 
        if (chunk.CharacterIsOnTile()) 
        { 
            Player target = chunk.GetCurrentPlayer(); 
            AflameDebuff debuff = new AflameDebuff(); 
            _player.debuffManager.AddDebuff(debuff, target); 
            DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        }
    }
}