using Godot;

public partial class ThrowSpear : BaseAction
{
	[Export] private Resource spearPrefab;
	private bool laserGrid = true;
	private Player spawnedCharacter;
	private ChunkData[,] _chunkArray;

	public ThrowSpear()
	{
	}

	public ThrowSpear(ThrowSpear throwSpear): base(throwSpear)
	{
		spearPrefab = throwSpear.spearPrefab;
	}
	public override BaseAction CreateNewInstance(BaseAction action)
	{
		ThrowSpear ability = new ThrowSpear((ThrowSpear)action);
		return ability;
	}
	
	public override void CreateAvailableChunkList(int attackRange)
	{
		ChunkData centerChunk = GameTileMap.Tilemap.GetChunk(player.GlobalPosition);
		(int centerX, int centerY) = centerChunk.GetIndexes();
		_chunkList.Clear();
		int count = attackRange;
		_chunkArray = new ChunkData[4,count];
		int start = 1;
		for (int i = 0; i < count; i++) 
		{
			if (GameTileMap.Tilemap.CheckBounds(centerX + i + start, centerY))
			{
				ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(centerX + i + start, centerY);
				_chunkList.Add(chunkData);
				_chunkArray[0, i] = chunkData;
			}
			if (GameTileMap.Tilemap.CheckBounds(centerX - i - start, centerY))
			{
				ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(centerX-i - start, centerY);
				_chunkList.Add(chunkData);
				_chunkArray[1, i] = chunkData;
			}
			if (GameTileMap.Tilemap.CheckBounds(centerX, centerY + i + start))
			{
				ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(centerX, centerY + i + start);
				_chunkList.Add(chunkData);
				_chunkArray[2, i] = chunkData;
			}
			if (GameTileMap.Tilemap.CheckBounds(centerX, centerY - i - start))
			{
				ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(centerX, centerY - i - start);
				_chunkList.Add(chunkData);
				_chunkArray[3, i] = chunkData;
			}
		}
	}
	public override void ResolveAbility(ChunkData chunk)
	{
		base.ResolveAbility(chunk);
		int index = FindChunkIndex(chunk);
		if (index != -1)
		{
			for (int i = 0; i < _chunkArray.GetLength(1); i++)
			{
				ChunkData damageChunk = _chunkArray[index, i];
				DealRandomDamageToTarget(damageChunk, minAttackDamage, maxAttackDamage);
			}
			ChunkData spawnChunk = _chunkArray[index, _chunkArray.GetLength(1) - 1];
			PackedScene spawnResource = (PackedScene)spearPrefab;
			spawnedCharacter = spawnResource.Instantiate<Player>();
			player.GetTree().Root.CallDeferred("add_child", spawnedCharacter);
			GameTileMap.Tilemap.MoveSelectedCharacter(spawnChunk, spawnedCharacter);
		}
		FinishAbility();
	}
	
	private int FindChunkIndex(ChunkData chunkData)
	{
		int index = -1;
		for (int i = 0; i < _chunkArray.GetLength(1); i++)
		{
			if (_chunkArray[0,i] != null && _chunkArray[0,i] == chunkData)
			{
				index = 0;
			}
			if(_chunkArray[1,i] != null && _chunkArray[1,i] == chunkData)
			{
				index = 1;
			}
			if (_chunkArray[2,i] != null && _chunkArray[2,i] == chunkData)
			{
				index = 2;
			}
			if (_chunkArray[3,i] != null && _chunkArray[3,i] == chunkData)
			{
				index = 3;
			}

		}
		return index;
	}
}
