using Godot;


public partial class CreateBearTrap : BaseAction
{
	[Export] private ObjectData bearTrapData;
	[Export] private Resource bearTrapPrefab;
	private Object spawnedBearTrap;
	private int i = 0; //Naudojama, kad bear trap dingtu po keliu turn'u
	
	public CreateBearTrap()
	{
		
	}
	public CreateBearTrap(CreateBearTrap createBearTrap) : base(createBearTrap)
	{
		bearTrapPrefab = createBearTrap.bearTrapPrefab;
		bearTrapData = createBearTrap.bearTrapData;
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
		player.GetTree().Root.CallDeferred("add_child", spawnedBearTrap);
		spawnedBearTrap.SetupObject(bearTrapData);
		spawnedBearTrap.AddPlayerForObjectAbilities(player);
		GameTileMap.Tilemap.SpawnObject(spawnedBearTrap, chunk);
		i = 0;
		FinishAbility();
	}

	
}
