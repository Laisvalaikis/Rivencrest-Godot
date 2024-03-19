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
		base.ResolveAbility(chunk); 
		PlayerAbilityAnimation();
		UpdateAbilityButton(); 
		Player target = chunk.GetCurrentPlayer(); 
		SilenceDebuff debuff = new SilenceDebuff(); 
		target.debuffManager.AddDebuff(debuff, _player); 
		DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage); 
		GameTileMap.Tilemap.MoveSelectedCharacter(TileToDashTo(chunk)); 
		FinishAbility();
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
}
