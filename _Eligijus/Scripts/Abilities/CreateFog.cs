using Godot;

public partial class CreateFog : BaseAction
{
	[Export] private ObjectData fogPrefabData;
	[Export] private Resource fogPrefab;
	private Object spawnedFog;
	private bool isFogActive = true;
	private int i = 0;
	
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
	
	public override void OnTurnStart(ChunkData chunkData)//reikes veliau tvarkyt kai bus animacijos ir fog of war
	{
		base.OnTurnStart(chunkData);
		if (isFogActive)
		{
			i++;
			if (i >= 2)
			{
				spawnedFog.Death();
				isFogActive = false;
				i = 0;
			}
		}
	}
	public override void ResolveAbility(ChunkData chunk)
	{
		UpdateAbilityButton();
		base.ResolveAbility(chunk);
		PackedScene spawnCharacter = (PackedScene)fogPrefab;
		spawnedFog = spawnCharacter.Instantiate<Object>();
		player.GetTree().Root.CallDeferred("add_child", spawnedFog);
		spawnedFog.SetupObject(fogPrefabData);
		GameTileMap.Tilemap.SpawnObject(spawnedFog, chunk);
		FinishAbility();
		isFogActive = true;
		i = 0;
	}
}
