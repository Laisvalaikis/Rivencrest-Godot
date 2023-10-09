using Godot;

namespace Rivencrestgodot._Eligijus.Scripts.Abilities;

public partial class CreateBearTrap : BaseAction
{
	//reikia bearTrap sukurti kaip ir praeitame projekte
	[Export] private Resource bearTrapPrefab;
	private Player spawnedBear;
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
		base.ResolveAbility(chunk);

		PackedScene spawnCharacter = (PackedScene)bearTrapPrefab;
		spawnedBear = spawnCharacter.Instantiate<Player>();
		player.GetTree().Root.CallDeferred("add_child", spawnedBear);
		PlayerInformation tempPlayerInformation = spawnedBear.playerInformation;
		GameTileMap.Tilemap.SetCharacter(chunk, spawnedBear, tempPlayerInformation);
		//_bearTrap.creator = gameObject; //nelabai zinau, kam sito reikia dar
		i = 0;
		FinishAbility();
	}

	public override void OnTurnStart()
	{
		if (spawnedBear != null)
		{
			i++;
			if (i >= 2)
			{
				spawnedBear.QueueFree();
				i = 0;
			}
		}
	}
}
