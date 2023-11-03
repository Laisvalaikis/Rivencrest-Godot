using Godot;


public partial class CreateEye : BaseAction
{
	[Export] 
	private Resource eyePrefab;

	private Player spawnedEye;
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
		if (_chunkList.Contains(chunk) && !chunk.CharacterIsOnTile())
		{
			PackedScene spawnResource = (PackedScene)eyePrefab;
			spawnedEye = spawnResource.Instantiate<Player>();
			player.GetTree().Root.CallDeferred("add_child", spawnedEye);
			spawnedEye.GlobalPosition = chunk.GetPosition();
			GameTileMap.Tilemap.MoveSelectedCharacter(chunk, spawnedEye);
			base.ResolveAbility(chunk);
		}
		FinishAbility();
	}

	public override void OnTurnEnd()
	{
		base.OnTurnEnd();
		spawnedEye.QueueFree();
	}

	public override void CreateAvailableChunkList(int attackRange)
	{
		(int x, int y) coordinates = GameTileMap.Tilemap.GetChunk(player.GlobalPosition).GetIndexes();
		ChunkData[,] chunkDataArray = GameTileMap.Tilemap.GetChunksArray();
		_chunkList.Clear();
		
		int top = coordinates.y - attackRange;
		int bottom = coordinates.y + attackRange;
		int right = coordinates.x + attackRange;
		int left = coordinates.x - attackRange;

		if (GameTileMap.Tilemap.CheckBounds(coordinates.x, top))
		{
			ChunkData chunkDataTop = chunkDataArray[coordinates.x, top];
			_chunkList.Add(chunkDataTop);
		}

		if (GameTileMap.Tilemap.CheckBounds(coordinates.x, bottom))
		{
			ChunkData chunkDataBottom = chunkDataArray[coordinates.x, bottom];
			_chunkList.Add(chunkDataBottom);
		}

		if (GameTileMap.Tilemap.CheckBounds(right, coordinates.y))
		{
			ChunkData chunkDataRight = chunkDataArray[right, coordinates.y];
			_chunkList.Add(chunkDataRight);
		}

		if (GameTileMap.Tilemap.CheckBounds(left, coordinates.y))
		{
			ChunkData chunkDataLeft = chunkDataArray[left, coordinates.y];
			_chunkList.Add(chunkDataLeft);
		}
	}
}
