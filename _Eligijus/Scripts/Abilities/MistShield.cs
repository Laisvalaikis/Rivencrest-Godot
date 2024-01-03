using Godot;
using Rivencrestgodot._Eligijus.Scripts.BuffSystem;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class MistShield : BaseAction
{
    //private bool isAbilityActive = false;

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
    public override void CreateAvailableChunkList(int attackRange)
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
   // public override void OnTurnStart(ChunkData chunkData)
   // {
       // base.OnTurnStart(chunkData);
       // if (isAbilityActive)
       // {
            //_player.RemoveBarrier();
       // }
       // _player.objectInformation.GetPlayerInformation().characterProtected = false;
    //}

    public override void ResolveAbility(ChunkData chunk)
    {
        if (CanTileBeClicked(chunk))
        {
            UpdateAbilityButton();
           // isAbilityActive = true;
            _player.objectInformation.GetPlayerInformation().characterProtected = true;
            ProtectedBuff buff = new ProtectedBuff();
            chunk.GetCurrentPlayer().buffManager.AddBuff(buff);
            base.ResolveAbility(chunk);
            FinishAbility();
        }
    }
}
