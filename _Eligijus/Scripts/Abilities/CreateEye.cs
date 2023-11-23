using Godot;


public partial class CreateEye : BaseAction
{
	[Export] private ObjectData eyeData;
	[Export] private Resource eyePrefab;
	private Object spawnedEye;
	private bool isEyeActive = true;

	public override void Start()
	{
		base.Start();
		customText = ""; //To not display damage (might be temporary solution)
	}

	public CreateEye()
	{
		
	}
	public CreateEye(CreateEye createEye) : base(createEye)
	{
		eyePrefab = createEye.eyePrefab;
		eyeData = createEye.eyeData;
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
			UpdateAbilityButton();
			PackedScene spawnCharacter = (PackedScene)eyePrefab;
			spawnedEye = spawnCharacter.Instantiate<Object>();
			player.GetTree().Root.CallDeferred("add_child", spawnedEye);
			spawnedEye.SetupObject(eyeData);
			GameTileMap.Tilemap.SpawnObject(spawnedEye, chunk);
			base.ResolveAbility(chunk);
		}
		FinishAbility();
	}

	public override void OnTurnEnd(ChunkData chunkData)
	{
		base.OnTurnEnd(chunkData);
		spawnedEye.Death();
	}

	public override void CreateAvailableChunkList(int range)
	{
		(int x, int y) coordinates = GameTileMap.Tilemap.GetChunk(player.GlobalPosition).GetIndexes();
		ChunkData[,] chunkDataArray = GameTileMap.Tilemap.GetChunksArray();
		_chunkList.Clear();
		
		int top = coordinates.y - range;
		int bottom = coordinates.y + range;
		int right = coordinates.x + range;
		int left = coordinates.x - range;

		if (GameTileMap.Tilemap.CheckBounds(coordinates.x, top))
		{
			ChunkData chunkDataTop = chunkDataArray[coordinates.x, top];
			TryAddTile(chunkDataTop);
		}

		if (GameTileMap.Tilemap.CheckBounds(coordinates.x, bottom))
		{
			ChunkData chunkDataBottom = chunkDataArray[coordinates.x, bottom];
			TryAddTile(chunkDataBottom);
		}

		if (GameTileMap.Tilemap.CheckBounds(right, coordinates.y))
		{
			ChunkData chunkDataRight = chunkDataArray[right, coordinates.y];
			TryAddTile(chunkDataRight);
		}

		if (GameTileMap.Tilemap.CheckBounds(left, coordinates.y))
		{
			ChunkData chunkDataLeft = chunkDataArray[left, coordinates.y];
			TryAddTile(chunkDataLeft);
		}
	}
}
