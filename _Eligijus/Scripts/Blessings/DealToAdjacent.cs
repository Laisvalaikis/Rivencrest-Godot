using Godot;
using System;
using System.Collections.Generic;

public partial class DealToAdjacent : AbilityBlessing
{

	private int minDamage = 2;
	private int maxDamage = 2;
	
	public DealToAdjacent()
	{
		
	}
	public DealToAdjacent(DealToAdjacent blessing): base(blessing)
	{
		
	}
	public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
	{
		DealToAdjacent blessing = new DealToAdjacent((DealToAdjacent)baseBlessing);
		return blessing;
	}
	public override BaseBlessing CreateNewInstance()
	{
		DealToAdjacent blessing = new DealToAdjacent(this);
		return blessing;
	}
	public override void ResolveBlessing(ref BaseAction baseAction, ChunkData tile)
	{
		base.ResolveBlessing(ref baseAction);
		(int x, int y) = tile.GetIndexes();
        
		var directionVectors = new List<(int, int)>
		{
			(1 + x, 0 + y),
			(0 + x, 1 + y),
			(-1 + x, 0 + y),
			(0 + x, -1 + y)
		};

		foreach (var directions in directionVectors)
		{
			ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(directions.Item1, directions.Item2);
			if (chunkData.CharacterIsOnTile())
			{
				Player player = chunkData.GetCurrentPlayer();
				if (IsAllegianceSame(player.playerInformation, chunkData, baseAction))
				{
					DealRandomDamageToTarget(baseAction.GetPlayer().playerInformation, chunkData, baseAction, minDamage, maxDamage);
				}
			}
		}

	}
}
