using Godot;

public partial class CreateFog : BaseAction
{
	//pointless fucking ability
	[Export] private ObjectData fogPrefabData;
	[Export] private Resource fogPrefab;
	
	public CreateFog()
	{
		
	}
	
	public CreateFog(CreateFog createFog) : base(createFog)
	{
		fogPrefab = createFog.fogPrefab;
		fogPrefabData = createFog.fogPrefabData;
	}
	
	public override BaseAction CreateNewInstance(BaseAction action)
	{
		CreateFog createFog = new CreateFog((CreateFog)action);
		return createFog;
	}
	
	public override void ResolveAbility(ChunkData chunk)
	{
		UpdateAbilityButton();
		base.ResolveAbility(chunk);
		PlayerAbilityAnimation();
		PackedScene spawnCharacter = (PackedScene)fogPrefab;
		Object spawnedFog = spawnCharacter.Instantiate<Object>();
		_player.GetTree().CurrentScene.CallDeferred("add_child", spawnedFog);
		spawnedFog.SetupObject(fogPrefabData);
		spawnedFog.AddPlayerForObjectAbilities(_player);
		GameTileMap.Tilemap.SpawnObject(spawnedFog, chunk);
		FinishAbility();
	}
}
