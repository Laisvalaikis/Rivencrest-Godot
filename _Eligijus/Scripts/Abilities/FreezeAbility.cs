using System.Collections.Generic;
using Godot;

public partial class FreezeAbility : BaseAction
{
	private List<ChunkData> _chunkListCopy;

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
	
	public override void CreateAvailableChunkList(int attackRange)
	{
		ChunkData centerChunk = GameTileMap.Tilemap.GetChunk(player.GlobalPosition);
		_chunkList.Clear();
		(int centerX, int centerY) = centerChunk.GetIndexes();
		ChunkData[,] chunksArray = GameTileMap.Tilemap.GetChunksArray(); 
		for (int y = -attackRange; y <= attackRange; y++)
		{
			for (int x = -attackRange; x <= attackRange; x++)
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
					if (chunk != null && !chunk.TileIsLocked())
					{
						_chunkList.Add(chunk);
					}
				}
			}
		}
		_chunkListCopy = new List<ChunkData>(_chunkList);
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
			}
		}
		else if ((hoveredChunkHighlight!=null && previousChunkHighlight!=null && hoveredChunkHighlight.isHighlighted && !previousChunkHighlight.isHighlighted) || (hoveredChunkHighlight!=null && previousChunkHighlight==null && hoveredChunkHighlight.isHighlighted))
		{
			foreach (var chunk in _chunkList)
			{
				SetHoveredAttackColor(chunk);
			}
		}
	}
	private void DealDamageToList()
	{
		foreach (var chunk in _chunkListCopy)
		{
			SetNonHoveredAttackColor(chunk);
			if (chunk.GetCurrentCharacter() != null && chunk!=GameTileMap.Tilemap.GetChunk(player.GlobalPosition))
			{
				DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
			}
		}
	}
	public override void ResolveAbility(ChunkData chunk)
	{
		base.ResolveAbility(chunk);
		DealDamageToList();
		FinishAbility();
	}
}
