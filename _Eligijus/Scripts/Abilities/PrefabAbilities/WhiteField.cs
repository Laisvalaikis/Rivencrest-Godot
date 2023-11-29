using Godot;

public partial class WhiteField : BaseAction
{
    public WhiteField()
    {
        
    }
    
    public WhiteField(WhiteField ability): base(ability)
    {
        
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        WhiteField ability = new WhiteField((WhiteField)action);
        return ability;
    }

    // Step On
    public override void ResolveAbility(ChunkData chunk)
    {
        // need to add buff destroy half damage
    }

    // Exit
    public override void OnExitAbility(ChunkData chunkDataPrev, ChunkData chunk)
    {
        // need to remove buff destroy half damage
    }

    public override void Die()
    {
        base.Die();
        // need to remove buff destroy half damage
    }
}