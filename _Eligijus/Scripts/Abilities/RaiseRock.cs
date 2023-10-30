using Godot;

public partial class RaiseRock : BaseAction
{
	[Export]
	public Resource WallPrefab;

	public RaiseRock()
	{
		
	}
	public RaiseRock(RaiseRock ability): base(ability)
	{
		WallPrefab = ability.WallPrefab;
	}

	public override BaseAction CreateNewInstance(BaseAction action)
	{
		RaiseRock ability = new RaiseRock((RaiseRock)action);
		return ability;
	}
	
	public override void ResolveAbility(ChunkData chunk)
	{
		base.ResolveAbility(chunk);
		if (!chunk.CharacterIsOnTile())
		{
			PackedScene spawnResource = (PackedScene)WallPrefab;
			Player spawnedWall = spawnResource.Instantiate<Player>();
			player.GetTree().Root.CallDeferred("add_child", spawnedWall);
			GameTileMap.Tilemap.SetCharacter(chunk, spawnedWall);
			FinishAbility();
		}
	}
}
