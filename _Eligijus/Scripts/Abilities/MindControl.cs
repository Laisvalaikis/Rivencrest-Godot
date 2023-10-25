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
    
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        if (chunk.CharacterIsOnTile())
        {
            Player target = chunk.GetCurrentPlayer();
            _target = target;
            target.debuffs.SetTurnCounterFromThisTurn(1);
        }

        FinishAbility();
    }

    public override void PlayerWasAttacked()
    {
        _target.debuffs.RemoveSilence();
        _target = null;
    }


}
