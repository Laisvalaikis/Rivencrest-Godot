using System.Collections.Generic;
using Godot;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class Blaze : BaseAction //removed ability
{
    [Export] public int bonusDamage = 4;
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
    public override void ResolveAbility(ChunkData chunk)
    {
        if (CanTileBeClicked(chunk))
        {
            UpdateAbilityButton();
            base.ResolveAbility(chunk);
            if (chunk.CharacterIsOnTile())
            {
                Player target = chunk.GetCurrentPlayer();
                AflameDebuff debuff = new AflameDebuff();
                _player.debuffManager.AddDebuff(debuff, target);
                DealRandomDamageToTarget(chunk, minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
            }
        }
    }
}