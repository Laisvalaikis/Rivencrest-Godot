using Godot;


public partial class CreateEye : BaseAction
{
	[Export] private ObjectData eyeData;
	[Export] private Resource eyePrefab;
	private Object spawnedEye;
	private bool isEyeActive = true;

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
		UpdateAbilityButton(); 
		PackedScene spawnCharacter = (PackedScene)eyePrefab; 
		spawnedEye = spawnCharacter.Instantiate<Object>(); 
		_player.GetTree().CurrentScene.CallDeferred("add_child", spawnedEye); 
		spawnedEye.SetupObject(eyeData); 
		spawnedEye.AddPlayerForObjectAbilities(_player); 
		GameTileMap.Tilemap.SpawnObject(spawnedEye, chunk); 
		spawnedEye.StartActions(); 
		base.ResolveAbility(chunk); // sukuria, bet neatnaujina 
		FinishAbility();
	}

	public override void OnTurnEnd(ChunkData chunkData)
	{
		base.OnTurnEnd(chunkData);
		spawnedEye.Death();
	}

	public override void CreateAvailableChunkList(int range)
	{
		(int x, int y) coordinates = GameTileMap.Tilemap.GetChunk(_player.GlobalPosition).GetIndexes();
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

	protected override bool CanAddTile(ChunkData chunk)
	{
		if (chunk != null && !chunk.TileIsLocked() && !chunk.CharacterIsOnTile() && !chunk.ObjectIsOnTile())
		{
			return true;
		}
		return false;
	}
}
