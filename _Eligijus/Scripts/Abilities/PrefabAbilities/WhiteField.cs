using Godot;
using Rivencrestgodot._Eligijus.Scripts.BuffSystem;

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
        WhiteFieldBuff buff = new WhiteFieldBuff();
        Player player = chunk.GetCurrentPlayer();
        player.AddBuff(buff);
    }

    // Exit
    public override void OnExitAbility(ChunkData chunkDataPrev, ChunkData chunk)
    {
        chunk.GetCurrentPlayer().debuffManager.RemoveDebuffsByType(typeof(WhiteFieldBuff));
    }

    public override void Die()
    {
        base.Die();
        // need to remove buff destroy half damage
    }
}