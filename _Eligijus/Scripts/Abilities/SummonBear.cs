using Godot;

public partial class SummonBear : BaseAction
{
    [Export]
    private Resource bearPrefab;

    public SummonBear()
    {
    }

    public SummonBear(SummonBear ability): base(ability)
    {
        bearPrefab = ability.bearPrefab;
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        SummonBear ability = new SummonBear((SummonBear)action);
        return ability;
    }

    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        PackedScene spawnResource = (PackedScene)bearPrefab;
        Player spawnedCharacter = spawnResource.Instantiate<Player>();
        player.GetTree().Root.CallDeferred("add_child", spawnedCharacter);
        FinishAbility();
    }

    public override void CreateAvailableChunkList(int attackRange)
    {
        (int y, int x) coordinates = GameTileMap.Tilemap.GetChunk(player.GlobalPosition).GetIndexes();
        ChunkData[,] chunkDataArray = GameTileMap.Tilemap.GetChunksArray();
        _chunkList.Clear();
        int rightX = coordinates.x + attackRange;
        ChunkData chunkData = chunkDataArray[coordinates.y, rightX];
        _chunkList.Add(chunkData);
    }
}
