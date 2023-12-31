using System;
using System.Collections.Generic;
using Godot;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class DisarmingSlam : BaseAction
{
	public DisarmingSlam()
	{
 		
	}
	public DisarmingSlam(DisarmingSlam disarmingSlam): base(disarmingSlam)
	{
	}
 	
	public override BaseAction CreateNewInstance(BaseAction action)
	{
		DisarmingSlam disarmingSlam = new DisarmingSlam((DisarmingSlam)action);
		return disarmingSlam;
	}
	public override void ResolveAbility(ChunkData chunk)
	{
		if (CanTileBeClicked(chunk))
		{
			base.ResolveAbility(chunk);
			UpdateAbilityButton();
			Player target = chunk.GetCurrentPlayer();
			SilenceDebuff debuff = new SilenceDebuff();
			target.debuffManager.AddDebuff(debuff, _player);
			DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
			GameTileMap.Tilemap.MoveSelectedCharacter(TileToDashTo(chunk));
			FinishAbility();
		}
	}
	private ChunkData TileToDashTo(ChunkData chunk)
	{
		Vector2 position = _player.GlobalPosition;
		ChunkData currentPlayerChunk = GameTileMap.Tilemap.GetChunk(position);
		(int playerX, int playerY) = currentPlayerChunk.GetIndexes();
		(int chunkX, int chunkY) = chunk.GetIndexes();

		int deltaX = playerX - chunkX;
		int deltaY = playerY - chunkY;

		// Determine the direction from the player to the chunk
		int directionX = deltaX != 0 ? deltaX / Math.Abs(deltaX) : 0;
		int directionY = deltaY != 0 ? deltaY / Math.Abs(deltaY) : 0;
		
		ChunkData targetChunk = GameTileMap.Tilemap.GetChunkDataByIndex(chunkX+directionX,chunkY+directionY);

		return targetChunk;
	}
	protected override void GeneratePlusPattern(ChunkData centerChunk, int length)
	{
		(int centerX, int centerY) = centerChunk.GetIndexes();
		ChunkData[,] chunksArray = GameTileMap.Tilemap.GetChunksArray();
		bool[] canExtend = { true, true, true, true };

		for (int i = 1; i <= length; i++)
		{
			List<(int, int, int)> positions = new List<(int, int, int)>
			{
				(centerX, centerY + i, 0), // Up
				(centerX, centerY - i, 1), // Down
				(centerX + i, centerY, 2), // Right
				(centerX - i, centerY, 3) // Left
			};
			foreach (var (x, y, direction) in positions)
			{
				if (!canExtend[direction])
				{
					continue;
				}
				if (x >= 0 && x < chunksArray.GetLength(0) && y >= 0 && y < chunksArray.GetLength(1))
				{
					ChunkData chunk = chunksArray[x, y];
					if (chunk != null && !chunk.TileIsLocked())
					{
						if (chunk.GetCharacterType() == typeof(Object))
						{
							canExtend[direction] = false;
							continue;
						}
						_chunkList.Add(chunk);
						if (chunk.GetCurrentPlayer() != null)
						{
							canExtend[direction] = false;
						}
					}
				}
			}
		}
	}
}
