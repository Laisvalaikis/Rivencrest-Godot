using Godot;


public partial class CreateEye : BaseAction
{
	[Export] 
	private Resource eyePrefab;
	private bool isEyeActive = true;
	
	public CreateEye()
	{
		
	}
	public CreateEye(CreateEye createEye) : base(createEye)
	{
		eyePrefab = createEye.eyePrefab;
	}
	public override BaseAction CreateNewInstance(BaseAction action)
	{
		CreateEye createEye = new CreateEye((CreateEye)action);
		return createEye;
	}
	public override void ResolveAbility(ChunkData chunk)
	{
		if (_chunkList.Contains(chunk))
		{
			PackedScene spawnResource = (PackedScene)eyePrefab;
			Player spawnedEye = spawnResource.Instantiate<Player>();
			player.GetTree().Root.CallDeferred("add_child", spawnedEye);
			GameTileMap.Tilemap.SetCharacter(chunk, spawnedEye);
			base.ResolveAbility(chunk);
		}
		FinishAbility();
	}

	public override void CreateAvailableChunkList(int attackRange)
	{
		(int y, int x) coordinates = GameTileMap.Tilemap.GetChunk(player.GlobalPosition).GetIndexes();
		ChunkData[,] chunkDataArray = GameTileMap.Tilemap.GetChunksArray();
		_chunkList.Clear();
		
		int topY = coordinates.y - base.attackRange;
		ChunkData chunkData = chunkDataArray[topY, coordinates.x];
		_chunkList.Add(chunkData);
	}
}
