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
				// Skip the center chunk
				if (x == 0 && y == 0)
				{
					continue;
				}
				int targetX = centerX + x;
				int targetY = centerY + y;
				// Ensuring we don't go out of array bounds.
				if (targetX >= 0 && targetX < chunksArray.GetLength(0) && targetY >= 0 && targetY < chunksArray.GetLength(1))
				{
					ChunkData chunk = chunksArray[targetX, targetY];
					TryAddTile(chunk);
				}
			}
		}
	}
	public override void OnMoveHover(ChunkData hoveredChunk, ChunkData previousChunk)
	{
		if (hoveredChunk == previousChunk) return;
		HighlightTile hoveredChunkHighlight = hoveredChunk?.GetTileHighlight();
		HighlightTile previousChunkHighlight = previousChunk?.GetTileHighlight();
		if ((hoveredChunkHighlight == null && previousChunkHighlight!=null && previousChunkHighlight.isHighlighted) || (hoveredChunkHighlight!=null && previousChunkHighlight!=null && !hoveredChunkHighlight.isHighlighted && previousChunkHighlight.isHighlighted))
		{
			foreach (var chunk in _chunkList)
			{
				SetNonHoveredAttackColor(chunk);
				DisableDamagePreview(chunk);
			}
		}
		else if ((hoveredChunkHighlight!=null && previousChunkHighlight!=null && hoveredChunkHighlight.isHighlighted && !previousChunkHighlight.isHighlighted) || (hoveredChunkHighlight!=null && previousChunkHighlight==null && hoveredChunkHighlight.isHighlighted))
		{
			foreach (var chunk in _chunkList)
			{
				SetHoveredAttackColor(chunk);
				if (chunk.GetCurrentPlayer() != null)
				{
					EnableDamagePreview(chunk, minAttackDamage, maxAttackDamage);
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
			if (target != null && chunkData!=GameTileMap.Tilemap.GetChunk(_player.GlobalPosition))
			{
				target.debuffManager.AddDebuff(debuff,_player);
				DealRandomDamageToTarget(chunkData, minAttackDamage, maxAttackDamage);
			}
		}		
		base.ResolveAbility(chunk);
		FinishAbility();
	}
}
