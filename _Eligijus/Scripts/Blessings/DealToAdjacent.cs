using Godot;
using System;
using System.Collections.Generic;

public partial class DealToAdjacent : AbilityBlessing
{

	private int minDamage = 2;
	private int maxDamage = 2;
	private List<ChunkData> _chunkListCopy;
	
	public DealToAdjacent()
	{
		
	}
	public DealToAdjacent(DealToAdjacent blessing): base(blessing)
	{
		minDamage = blessing.minDamage;
		maxDamage = blessing.maxDamage;
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
	public override void ResolveBlessing(BaseAction baseAction, ChunkData tile)
	{
		base.ResolveBlessing(baseAction);
		ChunkData[,] chunks = GameTileMap.Tilemap.GetChunksArray();
		(int x, int y) indexes = tile.GetIndexes();
		int x = indexes.x;
		int y = indexes.y;

		int[] dx = { 0, 0, 1, -1 };
		int[] dy = { 1, -1, 0, 0 };

		for (int i = 0; i < 4; i++)
		{
			int nx = x + dx[i];
			int ny = y + dy[i];

			if (GameTileMap.Tilemap.CheckBounds(nx, ny) && GameTileMap.Tilemap.GetChunkDataByIndex(nx,ny).CharacterIsOnTile())
			{
				ChunkData chunkData = chunks[nx, ny];
				DealDamage(chunkData, baseAction.GetPlayer(), baseAction, maxDamage);
			}
		}

	}
	
}
