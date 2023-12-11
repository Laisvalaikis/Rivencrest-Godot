using Godot;

public partial class ThrowSpear : BaseAction
{
	[Export] private ObjectData throwSpearData;
	[Export] private Resource spearPrefab;
	private Object spawnedCharacter;
	private ChunkData[,] _chunkArray;
	private int _globalIndex = -1;
	public ThrowSpear()
	{
	}

	public ThrowSpear(ThrowSpear throwSpear): base(throwSpear)
	{
		spearPrefab = throwSpear.spearPrefab;
		throwSpearData = throwSpear.throwSpearData;
		this.laserGrid = true;
	}
	public override BaseAction CreateNewInstance(BaseAction action)
	{
		ThrowSpear ability = new ThrowSpear((ThrowSpear)action);
		return ability;
	}
	
	public override void CreateAvailableChunkList(int range)
	{
		ChunkData centerChunk = GameTileMap.Tilemap.GetChunk(_player.GlobalPosition);
		(int centerX, int centerY) = centerChunk.GetIndexes();
		_chunkList.Clear();
		int count = range;
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
			UpdateAbilityButton();
			for (int i = 0; i < _chunkArray.GetLength(1); i++)
			{
				ChunkData damageChunk = _chunkArray[index, i];
				DealRandomDamageToTarget(damageChunk, minAttackDamage, maxAttackDamage);
			}
			OnAbility(chunk, index);
			// ChunkData spawnChunk = _chunkArray[index, _chunkArray.GetLength(1) - 1];
			// PackedScene spawnResource = (PackedScene)spearPrefab;
			// spawnedCharacter = spawnResource.Instantiate<Player>();
			// player.GetTree().Root.CallDeferred("add_child", spawnedCharacter);
			// GameTileMap.Tilemap.MoveSelectedCharacter(spawnChunk, spawnedCharacter);
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
	public override void OnMoveHover(ChunkData hoveredChunk, ChunkData previousChunk)
	{
		if (hoveredChunk == previousChunk) return;
		if (_globalIndex != -1)
		{
			for (int i = 0; i < _chunkArray.GetLength(1); i++)
			{
				ChunkData chunkToHighLight = _chunkArray[_globalIndex, i];
				if (chunkToHighLight != null)
				{
					SetNonHoveredAttackColor(chunkToHighLight);
					DisableDamagePreview(chunkToHighLight);
				}
			}
		}
		if (hoveredChunk != null && hoveredChunk.GetTileHighlight().isHighlighted)
		{
			_globalIndex = FindChunkIndex(hoveredChunk);
			if (_globalIndex != -1)
			{
				for (int i = 0; i < _chunkArray.GetLength(1); i++)
				{
					ChunkData chunkToHighLight = _chunkArray[_globalIndex, i];
					if (chunkToHighLight != null)
					{
						SetHoveredAttackColor(chunkToHighLight);
						if (CanTileBeClicked(chunkToHighLight))
						{
							EnableDamagePreview(chunkToHighLight, minAttackDamage, maxAttackDamage);
						}
					}
				}
			}
		}
	}
	private void OnAbility(ChunkData chunk, int index)
	{
		if (chunk.CharacterIsOnTile())// priesas nemirsta ir spear vienu i virsu varo
		{
			(int x, int y) indexes = chunk.GetIndexes();
			Side side = ChunkSideByCharacter(GameTileMap.Tilemap.GetChunk(_player.GlobalPosition), chunk);
			(int x, int y) sideVector = GetSideVector(side);
			ChunkData chunkData =
				GameTileMap.Tilemap.GetChunkDataByIndex(indexes.x + sideVector.x, indexes.y + sideVector.y);
			if (GameTileMap.Tilemap.CheckBounds(indexes.x + sideVector.x, indexes.y + sideVector.y) && !chunkData.CharacterIsOnTile())
			{
				PackedScene spawnResource = (PackedScene)spearPrefab;
				spawnedCharacter = spawnResource.Instantiate<Object>();
				_player.GetTree().Root.CallDeferred("add_child", spawnedCharacter);
				spawnedCharacter.SetupObject(throwSpearData);
				GameTileMap.Tilemap.SpawnObject(spawnedCharacter, chunkData);
			}
		}

		if (!chunk.CharacterIsOnTile()) //priesas mirsta
		{
			PackedScene spawnResource = (PackedScene)spearPrefab;
			spawnedCharacter = spawnResource.Instantiate<Object>();
			_player.GetTree().Root.CallDeferred("add_child", spawnedCharacter);
			spawnedCharacter.SetupObject(throwSpearData);
			GameTileMap.Tilemap.SpawnObject(spawnedCharacter, chunk);
		}
	}
}
