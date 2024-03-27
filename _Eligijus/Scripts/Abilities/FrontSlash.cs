using Godot;

public partial class FrontSlash : BaseAction
{
    public FrontSlash()
    {
        
    }
    public FrontSlash(FrontSlash ability): base(ability)
    {
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        FrontSlash ability = new FrontSlash((FrontSlash)action);
        return ability;
    }
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        PlayAnimation("Undead4", chunk);
        PlayerAbilityAnimation();
        int index = FindChunkIndex(chunk);
        if (index != -1)
        {
            UpdateAbilityButton();
            for (int i = 0; i < _chunkArray.GetLength(1); i++)
            {
                ChunkData damageChunk = _chunkArray[index, i];
                DealRandomDamageToTarget(damageChunk, minAttackDamage, maxAttackDamage);
            }
            FinishAbility();
        }
    }
}