using System.Collections.Generic;
using System.Threading;
using Godot;

public partial class CryoFreeze : BaseAction
{
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

	public override void CreateAvailableChunkList(int attackRange)
	{
		_chunkList.Add(GameTileMap.Tilemap.GetChunk(_player.GlobalPosition));
	}
	protected override void TryAddTile(ChunkData chunk)
	{
		if (chunk != null && !chunk.TileIsLocked())
		{
			_chunkList.Add(chunk);
		}
	}
	
	public override void OnTurnStart(ChunkData chunkData)
	{
		base.OnTurnStart(chunkData);
		if ( (_player.objectInformation.GetPlayerInformation().GetHealth() > 0))
		{
			ChunkData temp = GameTileMap.Tilemap.GetChunk(_player.GlobalPosition);
			Thread thread = new Thread(() =>
			{
				// Thread.Sleep(40); // for animation
				DamageAdjacent(temp);
			});
			thread.Start();
			ThreadManager.InsertThread(thread);
		}
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
	public override bool CanTileBeClicked(ChunkData chunkData)
	{
		return chunkData.GetTileHighlight().isHighlighted;
	}
	
	public override void EnableDamagePreview(ChunkData chunk, string customText = null)
	{
		//Intentionally left empty. Ability does damage, but we do not preview it.
	}

	public override void ResolveAbility(ChunkData chunk)
	{
		UpdateAbilityButton();
		base.ResolveAbility(chunk);
		FinishAbility();
	}
}
