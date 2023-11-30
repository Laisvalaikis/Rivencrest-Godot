using Godot;

public partial class RaiseRock : BaseAction
{
	[Export] private ObjectData wallRockData;
	[Export]
	public Resource WallPrefab;
	private Object wall;

	public RaiseRock()
	{
		
	}
	public RaiseRock(RaiseRock ability): base(ability)
	{
		WallPrefab = ability.WallPrefab;
		wallRockData = ability.wallRockData;
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
			UpdateAbilityButton();
			PackedScene spawnResource = (PackedScene)WallPrefab;
			wall = spawnResource.Instantiate<Object>();
			player.GetTree().Root.CallDeferred("add_child", wall);
			wall.SetupObject(wallRockData);
			GameTileMap.Tilemap.SpawnObject(wall, chunk);
			FinishAbility();
		}
	}
}
