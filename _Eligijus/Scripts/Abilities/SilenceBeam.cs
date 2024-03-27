using System.Collections.Generic;
using Godot;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

namespace Rivencrestgodot._Eligijus.Scripts.Abilities;

public partial class SilenceBeam : BaseAction
{
	[Export] private ObjectData PinkTileData;
	[Export] private Resource PinkTilePrefab;
	private List<Object> PinkTileObjects=new List<Object>();
	public SilenceBeam()
	{
	
	}
	
	public SilenceBeam(SilenceBeam action) : base(action)
	{
		PinkTileData = action.PinkTileData;
		PinkTilePrefab = action.PinkTilePrefab;
	}
		
	public override BaseAction CreateNewInstance(BaseAction action)
	{ 
		SilenceBeam silenceBeam = new SilenceBeam((SilenceBeam)action); 
		return silenceBeam;
	}

	public override void ResolveAbility(ChunkData chunk)
	{
		base.ResolveAbility(chunk);
		PlayAnimation("Pink1", chunk);
		UpdateAbilityButton();
		_index = FindChunkIndex(chunk);
		if (_index != -1)
		{
			for (int i = 0; i < _chunkArray.GetLength(1); i++)
			{
				ChunkData damageChunk = _chunkArray[_index, i];
				if (damageChunk != null)
				{
					DealRandomDamageToTarget(damageChunk, minAttackDamage, maxAttackDamage);
					SilenceDebuff debuff = new SilenceDebuff(2);
					chunk.GetCurrentPlayer()?.debuffManager.AddDebuff(debuff, _player);
					PackedScene spawnCharacter = (PackedScene)PinkTilePrefab;
					Object spawnedPinkTile = spawnCharacter.Instantiate<Object>();
					_player.GetTree().CurrentScene.CallDeferred("add_child", spawnedPinkTile);
					spawnedPinkTile.SetupObject(PinkTileData);
					spawnedPinkTile.AddPlayerForObjectAbilities(_player);
					GameTileMap.Tilemap.SpawnObject(spawnedPinkTile, damageChunk);
					PinkTileObjects.Add(spawnedPinkTile);
				}
			}
			FinishAbility();
		}
	}
}
