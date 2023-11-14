using Godot;

public partial class MindControl : BaseAction
{
    private Player _target;

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
    
    public override void ResolveAbility(ChunkData chunk) //Sitas ability reikalauja further testing po to kai bus PlayerWasAttacked. Bet siaip lyg veikia?
    {
        if (CanTileBeClicked(chunk))
        {
            UpdateAbilityButton();
            base.ResolveAbility(chunk);
            DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
            _target = chunk.GetCurrentPlayer();
            _target.debuffs.SetTurnCounterFromThisTurn(2);
            FinishAbility();
        }
    }

    public override void PlayerWasAttacked()
    {
        _target.debuffs.RemoveSilence();
        _target = null;
    }
}
