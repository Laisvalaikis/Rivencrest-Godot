using System.Collections.Generic;
using System.Threading;
using Godot;
using Rivencrestgodot._Eligijus.Scripts.BuffSystem;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class CryoFreeze : BaseAction
{
	private int i = 0;
	public CryoFreeze()
	{
 		
	}
	public CryoFreeze(CryoFreeze cryoFreeze): base(cryoFreeze)
	{
	}
 	
	public override BaseAction CreateNewInstance(BaseAction action)
	{
		CryoFreeze cryoFreeze = new CryoFreeze((CryoFreeze)action);
		return cryoFreeze;
	}

	public override void CreateAvailableChunkList(int range)
	{
		_chunkList.Add(GameTileMap.Tilemap.GetChunk(_player.GlobalPosition));
	}
	
	public override void OnBeforeStart(ChunkData chunkData)
	{
		base.OnBeforeStart(chunkData);
		if (i == 1)
		{
			if ((_player.objectInformation.GetPlayerInformation().GetHealth() > 0))
			{
				ChunkData temp = GameTileMap.Tilemap.GetChunk(_player.GlobalPosition);
				Thread thread = new Thread(() =>
				{
					DamageAdjacent(temp);
				});
				thread.Start();
				ThreadManager.InsertThread(thread);
			}
		}
		
		i++;
	}
	private void DamageAdjacent(ChunkData centerChunk)
	{
		ChunkData[,] chunks = GameTileMap.Tilemap.GetChunksArray();
		(int x, int y) indexes = centerChunk.GetIndexes();
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
				if (IsAllegianceSame(chunkData))
				{
					DealRandomDamageToTarget(chunkData, minAttackDamage / 2, maxAttackDamage / 2);
				}
				else
				{
					DealRandomDamageToTarget(chunkData, minAttackDamage, maxAttackDamage);
				}
			}
		}
	}
	public override bool CanBeUsedOnTile(ChunkData chunkData)
	{
		return _chunkList.Contains(chunkData);
	}

	public override void ResolveAbility(ChunkData chunk)
	{ 
		UpdateAbilityButton(); 
		StasisBuff stasisBuff = new StasisBuff(); 
		StasisDebuff stasisDebuff = new StasisDebuff(1); 
		_player.AddDebuff(stasisDebuff,_player); 
		_player.AddBuff(stasisBuff); 
		_player.SetMovementPoints(0); 
		_player.actionManager.RemoveAllActionPoints(); 
		base.ResolveAbility(chunk); 
		FinishAbility();
	}
}
