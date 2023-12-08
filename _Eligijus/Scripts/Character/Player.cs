using Godot;
using Godot.Collections;
using Rivencrestgodot._Eligijus.Scripts.Character;

public partial class Player : Object
{
	[Export] public int playerInTeamIndex = 0;
	[Export] public ActionManager actionManager;
	[Export] public DebuffManager debuffManager;
	[Export] public BuffManager buffManager;
	public Array<UnlockedAbilitiesResource> unlockedAbilityList;
	public Array<AbilityBlessingsResource> unlockedBlessingList;
	public Array<UnlockedBlessingsResource> unlockedPLayerBlessings;
	private int _currentCharacterTeam = -1;
	private CharacterTeams team;
	protected bool weakSpot = false;
	private int movementPoints = 3; //track movement points here in this class
	private int previousMovementPoints = 3;
	public PlayerActionMiddleware playerActionMiddleware;

	public override void SetupObject(ObjectData objectData)
	{
		playerActionMiddleware = new PlayerActionMiddleware();
		objectInformation = new ObjectType<ObjectInformation>(objectInformationNode, typeof(ObjectInformation));
		ObjectDataType = new ObjectDataType<ObjectData>(objectData, typeof(Player));
		objectInformation.GetPlayerInformation().SetupData(objectData);
		_visionRange = ObjectDataType.GetObjectData().visionRange;
		actionManager.InitializeActionManager();
		playerActionMiddleware._player = this;
	}

	public override void Death()
	{
		Hide();
		actionManager.PlayerDied();
		debuffManager.PlayerDied();
		buffManager.PlayerDied();
		ChunkData chunkData = GameTileMap.Tilemap.GetChunk(GlobalPosition);
		if (team != null)
		{
			team.CharacterDeath(chunkData, _currentCharacterTeam, playerInTeamIndex, this);
		}
		else if(team == null && objectInformation.GetType() != typeof(Player))
		{
			chunkData.SetCurrentCharacter(null);
			chunkData.GetTileHighlight().DisableHighlight();
			QueueFree();
		}
	}

	public override void AddTurnManager(TurnManager turnManager)
	{
		_turnManager = turnManager;
		actionManager.AddTurnManager(turnManager);
	}

	public void AddVisionTile(ChunkData chunkData)
	{
		_turnManager.GetTeamByIndex(_currentCharacterTeam).AddVisionTile(chunkData);
	}
	
	public void RemoveVisionTile(ChunkData chunkData)
	{
		_turnManager.GetTeamByIndex(_currentCharacterTeam).RemoveVisionTile(chunkData);
	}

	public bool CheckIfVisionTileIsUnlocked(ChunkData chunkData)
	{
		return _turnManager.GetTeamByIndex(_currentCharacterTeam).ContainsVisionTile(chunkData);
	}

	public override void PlayerWasDamaged()
	{
		actionManager.PlayerWasAttacked();
		debuffManager.PlayerWasAttacked();
		buffManager.PlayerWasAttacked();
	}

	public int GetMovementPoints()
	{
		return movementPoints;
	}

	public int GetPreviousMovementPoints()
	{
		return previousMovementPoints;
	}

	public void AddMovementPoints(int points)
	{
		previousMovementPoints = movementPoints;
		if (movementPoints + points > 0)
		{
			movementPoints += points;
		}
		else
		{
			movementPoints = 0;
		}
	}

	public void SetMovementPoints(int points)
	{
		previousMovementPoints = movementPoints;
		movementPoints = points;
	}
	
	
	public void SetPlayerTeam(int currentCharacterTeam)
	{
		_currentCharacterTeam = currentCharacterTeam;
	}
	
	public int GetPlayerTeam()
	{
		return _currentCharacterTeam;
	}

	public void SetPlayerTeam(CharacterTeams teams)
	{
		team = teams;
	}

	public CharacterTeams GetPlayerTeams()
	{
		return team;
	}
	public override void OnTurnStart()
	{
		movementPoints = actionManager.ReturnBaseAbilities()[0].Action.attackRange;
		previousMovementPoints = movementPoints;
		actionManager.OnTurnStart();
		Array<PlayerBlessing> playerBlessings = objectInformation.GetPlayerInformation().objectData.GetPlayerInformationData().GetAllPlayerBlessings();
		for (int i = 0; i < playerBlessings.Count; i++)
		{
			if (unlockedPLayerBlessings[i].blessingUnlocked)
			{
				playerBlessings[i].OnTurnStart(this);
			}
		}
		debuffManager.OnTurnStart();
		buffManager.OnTurnStart();

	}
	
	
	public void OnAfterResolve()
	{
		actionManager.OnAfterResolve();
	}
	
	public void OnBeforeStart()
	{
		actionManager.OnBeforeStart();
	}

	
	public override void OnTurnEnd()
	{
		actionManager.OnTurnEnd();
		Array<PlayerBlessing> playerBlessings = objectInformation.GetPlayerInformation().objectData.GetPlayerInformationData().GetAllPlayerBlessings();
		for (int i = 0; i < playerBlessings.Count; i++)
		{
			if (unlockedPLayerBlessings[i].blessingUnlocked)
			{
				playerBlessings[i].OnTurnStart(this);
			}
		}
		debuffManager.OnTurnEnd();
		buffManager.OnTurnEnd();
	}
	
	public override void OnExit(ChunkData chunkDataPrev, ChunkData chunkData)
	{
		actionManager.ActionOnExit(chunkDataPrev, chunkData);
	}

	public void AddBarrier()
	{
		objectInformation.GetPlayerInformation().AddBarrier();
	}
	
	public void RemoveBarrier()
	{
		objectInformation.GetPlayerInformation().RemoveBarrier();
	}

	public void AddWeakSpot()
	{
		weakSpot = true;
	}

	public bool HaveWeakSpot()
	{
		return weakSpot;
	}

	public void RemoveWeakSpot()
	{
		weakSpot = true;
	}
}
