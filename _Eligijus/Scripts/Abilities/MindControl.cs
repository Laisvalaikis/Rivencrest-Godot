using Godot;

public partial class MindControl : BaseAction
{
    private PlayerInformation _playerInformation;
    
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
        chunk.GetCurrentPlayerInformation().ApplyDebuff("MindControl", player);
        FinishAbility();
    }
}
