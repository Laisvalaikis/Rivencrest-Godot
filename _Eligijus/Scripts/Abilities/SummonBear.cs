using Godot;

public partial class SummonBear : BaseAction
{
	[Export]
	private Resource bearPrefab;

	public SummonBear()
	{
	}

	public SummonBear(SummonBear ability): base(ability)
	{
		bearPrefab = ability.bearPrefab;
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
			PackedScene spawnResource = (PackedScene)bearPrefab;
			Player spawnedCharacter = spawnResource.Instantiate<Player>();
			spawnedCharacter.SetPlayerTeam(teamIndex);
			player.GetPlayerTeams().AddAliveCharacter(teamIndex, spawnedCharacter, bearPrefab);
			player.GetTree().Root.CallDeferred("add_child", spawnedCharacter);
			GameTileMap.Tilemap.MoveSelectedCharacter(chunk, spawnedCharacter);
			player.GetPlayerTeams().portraitTeamBox.ModifyList();
			FinishAbility();
		}
	}
}
