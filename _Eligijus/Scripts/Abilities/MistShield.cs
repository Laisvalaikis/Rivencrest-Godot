using Godot;

public partial class MistShield : BaseAction
{
    private bool isAbilityActive = false;

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
        _chunkList.Add(GameTileMap.Tilemap.GetChunk(player.GlobalPosition));
    }
    public override void OnTurnStart(ChunkData chunkData)
    {
        base.OnTurnStart(chunkData);
        if (isAbilityActive)
        {
            player.RemoveBarrier();
        }
        player.objectInformation.GetPlayerInformation().characterProtected = false;
    }

    public override void ResolveAbility(ChunkData chunk)
    {
        if (CanTileBeClicked(chunk))
        {
            UpdateAbilityButton();
            base.ResolveAbility(chunk);
            isAbilityActive = true;
            player.objectInformation.GetPlayerInformation().characterProtected = true;
            player.AddBarrier();
            FinishAbility();
        }
    }
}
