using System.Diagnostics;
using Godot;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

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
        if (CanTileBeClicked(chunk))
        {
            UpdateAbilityButton();
            base.ResolveAbility(chunk);
            DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
            WeakSpotDebuff debuff = new WeakSpotDebuff(2);
            chunk.GetCurrentPlayer().debuffManager.AddDebuff(debuff, _player);
            FinishAbility();
        }
    }
}
