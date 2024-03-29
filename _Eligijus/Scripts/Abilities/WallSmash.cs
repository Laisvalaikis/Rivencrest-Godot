using System.Collections.Generic;
using Godot;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class WallSmash : BaseAction
{
	[Export]
	private int damageToWall = 1000;

	public WallSmash()
	{
	}

	public WallSmash(WallSmash ability): base(ability)
	{
		damageToWall = ability.damageToWall;
	}

	public override BaseAction CreateNewInstance(BaseAction action)
	{
		WallSmash ability = new WallSmash((WallSmash)action);
		return ability;
	}
	
	public override void ResolveAbility(ChunkData chunk)
	{
		base.ResolveAbility(chunk);
		UpdateAbilityButton();
		PlayerAbilityAnimation();
		if (chunk.GetCurrentPlayer() != null && chunk.GetCharacterType() == typeof(Player))
		{
			DealRandomDamageToTarget(chunk, minAttackDamage-2, maxAttackDamage-2);
		}
		else if(chunk.GetCurrentPlayer() == null && chunk.GetCharacterType() != typeof(Player))
		{
			DestroyObject(chunk);
		}
		FinishAbility();
	}
	private void DestroyObject(ChunkData chunkData)
	{
		(int x, int y) = chunkData.GetIndexes();
		ChunkData current = GameTileMap.Tilemap.GetChunk(_player.GlobalPosition);
		Side side = ChunkSideByCharacter(current, chunkData);
		(int x, int y) sideVector = GetSideVector(side);
		(int x, int y) coordinates = (x + sideVector.x, y + sideVector.y);
		ChunkData targetChunkData =
			GameTileMap.Tilemap.GetChunkDataByIndex(coordinates.Item1, coordinates.Item2);
		if (targetChunkData != null && targetChunkData.GetCurrentPlayer() != null
									&& targetChunkData.GetCharacterType() == typeof(Player) && !IsAllegianceSame(targetChunkData))
		{
			   
			DealRandomDamageToTarget(targetChunkData, minAttackDamage, maxAttackDamage);
			SlowDebuff debuff = new SlowDebuff(2,2, "IceSlow");
			targetChunkData.GetCurrentPlayer().debuffManager.AddDebuff(debuff,_player);	
		}
		DealDamage(chunkData,damageToWall);
	}
}
