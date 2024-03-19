using Godot;

public partial class ThrowSpear : BaseAction
{
	[Export] private ObjectData throwSpearData;
	[Export] private Resource spearPrefab;
	public Object spawnedCharacter;
	public ThrowSpear()
	{
	}

	public ThrowSpear(ThrowSpear throwSpear): base(throwSpear)
	{
		spearPrefab = throwSpear.spearPrefab;
		throwSpearData = throwSpear.throwSpearData;
	}
	public override BaseAction CreateNewInstance(BaseAction action)
	{
		ThrowSpear ability = new ThrowSpear((ThrowSpear)action);
		return ability;
	}
	
	
	public override void ResolveAbility(ChunkData chunk)
	{
		base.ResolveAbility(chunk);
		PlayerAbilityAnimation();
		int index = FindChunkIndex(chunk);
		if (index != -1)
		{
			UpdateAbilityButton();
			for (int i = 0; i < _chunkArray.GetLength(1); i++)
			{
				ChunkData damageChunk = _chunkArray[index, i];
				DealRandomDamageToTarget(damageChunk, minAttackDamage, maxAttackDamage);
			}
			OnAbility(chunk);
		}
		FinishAbility();
	}
	
	
	private void OnAbility(ChunkData chunk)
	{
		if (chunk.CharacterIsOnTile())
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
