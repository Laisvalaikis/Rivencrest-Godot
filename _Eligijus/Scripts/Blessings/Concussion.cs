using Godot;
using System;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class Concussion : AbilityBlessing
{
	public Concussion()
	{
		
	}
	public Concussion(Concussion blessing): base(blessing)
	{
		
	}
	public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
	{
		Concussion blessing = new Concussion((Concussion)baseBlessing);
		return blessing;
	}
	public override BaseBlessing CreateNewInstance()
	{
		Concussion blessing = new Concussion(this);
		return blessing;
	}

	// public override void ResolveBlessing(BaseAction baseAction, ChunkData tile)
	// {
	// 	base.ResolveBlessing(baseAction, tile);
	// 	Player player = tile.GetCurrentPlayer();
	// 	if (!IsAllegianceSame(baseAction.GetPlayer(), tile, baseAction))
	// 	{
	// 		SilenceDebuff debuff = new SilenceDebuff(2);
	// 		player.debuffManager.AddDebuff(debuff, player);
	// 	}
	// 	
	// }
	public override void ResolveBlessing(BaseAction baseAction, ChunkData tile)
	{
		base.ResolveBlessing(baseAction, tile);
		if(tile.GetCurrentPlayer() == null && tile.GetCharacterType() != typeof(Player))
		{
			DestroyObject(baseAction,tile);
		}
		
	}
	private void DestroyObject(BaseAction baseAction,ChunkData chunkData)
	{
		Player player = baseAction.GetPlayer();
		(int x, int y) = chunkData.GetIndexes();
		ChunkData current = GameTileMap.Tilemap.GetChunk(player.GlobalPosition);
		Side side = ChunkSideByCharacter(current, chunkData);
		(int x, int y) sideVector = GetSideVector(side);
		(int x, int y) coordinates = (x + sideVector.x, y + sideVector.y);
		ChunkData targetChunkData =
			GameTileMap.Tilemap.GetChunkDataByIndex(coordinates.Item1, coordinates.Item2);
		if (targetChunkData != null && targetChunkData.GetCurrentPlayer() != null
		                            && targetChunkData.GetCharacterType() == typeof(Player))
		{
			SilenceDebuff debuff = new SilenceDebuff(2);
			targetChunkData.GetCurrentPlayer().debuffManager.AddDebuff(debuff,player);	
		}
		
	}
}
