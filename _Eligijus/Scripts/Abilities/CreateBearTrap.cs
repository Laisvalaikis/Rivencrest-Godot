using Godot;


public partial class CreateBearTrap : BaseAction
{
	[Export] private Resource bearTrapPrefab;
	private Object spawnedBearTrap;
	private int i = 0; //Naudojama, kad bear trap dingtu po keliu turn'u
	
	public CreateBearTrap()
	{
		
	}
	public CreateBearTrap(CreateBearTrap createBearTrap) : base(createBearTrap)
	{
		bearTrapPrefab = createBearTrap.bearTrapPrefab;
	}
	public override BaseAction CreateNewInstance(BaseAction action)
	{
		CreateBearTrap createBearTrap = new CreateBearTrap((CreateBearTrap)action);
		return createBearTrap;
	}
	public override void ResolveAbility(ChunkData chunk)
	{
		UpdateAbilityButton();
		base.ResolveAbility(chunk);
		PackedScene spawnCharacter = (PackedScene)bearTrapPrefab;
		spawnedBearTrap = spawnCharacter.Instantiate<Object>();
		spawnedBearTrap.SetupObject();
		player.GetTree().Root.CallDeferred("add_child", spawnedBearTrap);
		GameTileMap.Tilemap.SpawnObject(spawnedBearTrap, chunk);
		i = 0;
		FinishAbility();
	}

	// public override void OnTurnStart(ChunkData chunkData)
	// {
	// 	base.OnTurnStart(chunkData);
	// 	if (spawnedBear != null)
	// 	{
	// 		i++;
	// 		if (i >= 2)
	// 		{
	// 			spawnedBear.QueueFree();
	// 			i = 0;
	// 		}
	// 	}
	// }
}
