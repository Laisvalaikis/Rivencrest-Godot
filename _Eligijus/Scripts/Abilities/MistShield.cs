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
    
    protected override void TryAddTile(ChunkData chunk)
    {
        if (chunk != null && !chunk.TileIsLocked())
        {
            _chunkList.Add(chunk);
        }
    }

    public override void ResolveAbility(ChunkData chunk)
    {
        if (CanTileBeClicked(chunk))
        {
            UpdateAbilityButton();
            ProtectedBuff buff = new ProtectedBuff();
            chunk.GetCurrentPlayer().buffManager.AddBuff(buff);
            base.ResolveAbility(chunk);
            FinishAbility();
        }
    }
}
