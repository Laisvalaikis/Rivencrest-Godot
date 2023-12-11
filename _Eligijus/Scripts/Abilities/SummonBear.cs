using Godot;
using Godot.Collections;

public partial class SummonBear : BaseAction
{
	[Export]
	private SavedCharacterResource bearResource;

	public SummonBear()
	{
	}

	public SummonBear(SummonBear ability): base(ability)
	{
		bearResource = ability.bearResource;
	}
	public override BaseAction CreateNewInstance(BaseAction action)
	{
		SummonBear ability = new SummonBear((SummonBear)action);
		return ability;
	}
	

	protected override void TryAddTile(ChunkData chunk)
	{
		if (chunk != null && !chunk.TileIsLocked() && chunk.GetCurrentPlayer() == null)
		{
			_chunkList.Add(chunk);
		}
	}
	public override void ResolveAbility(ChunkData chunk)
	{
		if (_chunkList.Contains(chunk))
		{
			UpdateAbilityButton();
			int teamIndex = _player.GetPlayerTeam();
			base.ResolveAbility(chunk);
			PackedScene spawnResource = (PackedScene)bearResource.prefab;
			Player spawnedCharacter = spawnResource.Instantiate<Player>();
			spawnedCharacter.AddTurnManager(_turnManager);
			spawnedCharacter.unlockedAbilityList = bearResource.unlockedAbilities;
			spawnedCharacter.unlockedBlessingList = bearResource.abilityBlessings;
			spawnedCharacter.SetupObject(bearResource.playerInformation);
			Array<Ability> allAbilities = new Array<Ability>();
			allAbilities.AddRange(spawnedCharacter.objectInformation.GetPlayerInformation().objectData.GetPlayerInformationData().baseAbilities);
			allAbilities.AddRange(spawnedCharacter.objectInformation.GetPlayerInformation().objectData.GetPlayerInformationData().abilities);
			GenerateBlessingsLockUnlock(allAbilities, bearResource);
			GenerateCharacterBlessingsLockUnlock(bearResource);
			spawnedCharacter.SetPlayerTeam(teamIndex);
			_player.GetPlayerTeams().AddAliveCharacter(teamIndex, spawnedCharacter, bearResource.prefab);
			_player.GetTree().Root.CallDeferred("add_child", spawnedCharacter);
			GameTileMap.Tilemap.MoveSelectedCharacter(chunk, spawnedCharacter);
			_player.GetPlayerTeams().portraitTeamBox.ModifyList();
			FinishAbility();
		}
	}
	
	private void GenerateBlessingsLockUnlock(Array<Ability> allAbilities, SavedCharacterResource characterResource)
	{
		for (int j = 0; j < allAbilities.Count; j++)
		{
			characterResource.abilityBlessings.Add(new AbilityBlessingsResource());
			Array<AbilityBlessing> abilityBlessings = allAbilities[j].Action.GetAllBlessings();
			if (abilityBlessings != null)
			{
				for (int k = 0; k < abilityBlessings.Count; k++)
				{
					characterResource.abilityBlessings[j].UnlockedBlessingsList
						.Add(new UnlockedBlessingsResource(abilityBlessings[k]));
				}
			}
		}
	}
	
	private void GenerateCharacterBlessingsLockUnlock(SavedCharacterResource characterResource)
	{
		Array<PlayerBlessing> playerBlessings = characterResource.playerInformation.GetAllPlayerBlessings();
		characterResource.characterBlessings = new Array<UnlockedBlessingsResource>();
		for (int i = 0; i < playerBlessings.Count; i++)
		{
			characterResource.characterBlessings.Add(new UnlockedBlessingsResource(playerBlessings[i]));
		}
	}
}
