using Rivencrestgodot._Eligijus.Scripts.BuffSystem;

public partial class MistShield : BaseAction
{
    public MistShield()
    {
        
    }
    public MistShield(MistShield ability): base(ability)
    {
        
    }
    
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        MistShield ability = new MistShield((MistShield)action);
        return ability;
    }
    
    public override void CreateAvailableChunkList(int range)
    {
        _chunkList.Add(GameTileMap.Tilemap.GetChunk(_player.GlobalPosition));
    }
    
    protected override bool CanAddTile(ChunkData chunk)
    {
        return chunk != null && !chunk.TileIsLocked();
    }

    public override void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
        PlayAnimation("MistSpawn", chunk);
        ProtectedBuff buff = new ProtectedBuff();
        chunk.GetCurrentPlayer().buffManager.AddBuff(buff);
        base.ResolveAbility(chunk);
        FinishAbility();
    }
}
