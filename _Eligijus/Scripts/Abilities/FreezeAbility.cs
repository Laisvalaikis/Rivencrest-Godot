using System.Collections.Generic;
using Godot;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class FreezeAbility : BaseAction
{ 
	public FreezeAbility()
	{
		
	}
	public FreezeAbility(FreezeAbility ability): base(ability)
	{
	}
	public override BaseAction CreateNewInstance(BaseAction action)
	{
		FreezeAbility ability = new FreezeAbility((FreezeAbility)action);
		return ability;
	}
	
	public override void CreateAvailableChunkList(int range)
	{
		ChunkData centerChunk = GameTileMap.Tilemap.GetChunk(_player.GlobalPosition);
		_chunkList.Clear();
		(int centerX, int centerY) = centerChunk.GetIndexes();
		ChunkData[,] chunksArray = GameTileMap.Tilemap.GetChunksArray(); 
		for (int y = -range; y <= range; y++)
		{
			for (int x = -range; x <= range; x++)
			{
				if (x == 0 && y == 0)
				{
					continue;
				}
				int targetX = centerX + x;
				int targetY = centerY + y;
				if (GameTileMap.Tilemap.CheckBounds(targetX,targetY))
				{
					ChunkData chunk = chunksArray[targetX, targetY];
					TryAddTile(chunk);
				}
			}
		}
	}
	
	public override void ResolveAbility(ChunkData chunk)
	{ 
		UpdateAbilityButton(); 
		foreach (var chunkData in _chunkList) 
		{ 
			SlowDebuff debuff = new SlowDebuff(1, 1); 
			Player target = chunkData.GetCurrentPlayer(); 
			if (CanBeUsedOnTile(chunkData)) 
			{ 
				target.debuffManager.AddDebuff(debuff, _player); 
				DealRandomDamageToTarget(chunkData, minAttackDamage, maxAttackDamage);
			}
		} 
		base.ResolveAbility(chunk); 
		FinishAbility();
	}
}
