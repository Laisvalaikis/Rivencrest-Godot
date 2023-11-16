using Godot;

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
			int teamIndex = player.GetPlayerTeam();
			base.ResolveAbility(chunk);
			PackedScene spawnResource = (PackedScene)bearResource.prefab;
			Player spawnedCharacter = spawnResource.Instantiate<Player>();
			spawnedCharacter.actionManager.AddTurnManager(_turnManager);
			spawnedCharacter.unlockedAbilityList = bearResource.unlockedAbilities;
			spawnedCharacter.unlockedBlessingList = bearResource.abilityBlessings;
			spawnedCharacter.SetPlayerTeam(teamIndex);
			player.GetPlayerTeams().AddAliveCharacter(teamIndex, spawnedCharacter, bearResource.prefab);
			player.GetTree().Root.CallDeferred("add_child", spawnedCharacter);
			GameTileMap.Tilemap.MoveSelectedCharacter(chunk, spawnedCharacter);
			player.GetPlayerTeams().portraitTeamBox.ModifyList();
			FinishAbility();
		}
	}
}
