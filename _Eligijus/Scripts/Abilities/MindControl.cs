using Godot;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class MindControl : BaseAction
{
    private Player _target = new Player();
    public MindControl()
    {
        
    }
    public MindControl(MindControl ability): base(ability)
    {
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        MindControl ability = new MindControl((MindControl)action);
        return ability;
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        if (CanTileBeClicked(chunk))
        {
            UpdateAbilityButton();
            base.ResolveAbility(chunk);
            SilenceDebuff debuff = new SilenceDebuff(2);
            _target = chunk.GetCurrentPlayer();
            _target.debuffManager.AddDebuff(debuff,_player);
            DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
            FinishAbility();
        }
    }

    public override void PlayerWasAttacked()
    {
        _target.debuffManager.RemoveDebuffsByType(typeof(SilenceDebuff));
    }
}
