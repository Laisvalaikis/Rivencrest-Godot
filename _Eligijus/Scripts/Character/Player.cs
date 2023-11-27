using Godot;
using Godot.Collections;
using Rivencrestgodot._Eligijus.Scripts.Character;

public partial class Player : Object
{
	[Export] public int playerInTeamIndex = 0;
	[Export] public PlayerInformation playerInformation;
	[Export] public ActionManager actionManager;
	[Export] public DebuffManager debuffManager;
	public Array<UnlockedAbilitiesResource> unlockedAbilityList;
	public Array<AbilityBlessingsResource> unlockedBlessingList;
	public Array<UnlockedBlessingsResource> unlockedPLayerBlessings;
	private int _currentCharacterTeam = -1;
	private CharacterTeams team;
	protected LinkedList<Poison> _poisons;
	protected bool weakSpot = false;
	private int movementPoints = 3; //track movement points here in this class
	private int previousMovementPoints = 3;
	public PlayerActionMiddleware playerActionMiddleware;

	public override void SetupObject(ObjectData objectInformation)
	{
		ObjectDataType = new ObjectDataType<ObjectData>(objectInformation, typeof(Player));
		playerInformation.SetupData(objectInformation);
		_visionRange = ObjectDataType.GetObjectData().visionRange;
		actionManager.InitializeActionManager();
	}

	public override void Death()
	{
		Hide();
		actionManager.PlayerDied();
		debuffManager.PlayerDied();
		ChunkData chunkData = GameTileMap.Tilemap.GetChunk(GlobalPosition);
		if (team != null)
		{
			team.CharacterDeath(chunkData, _currentCharacterTeam, playerInTeamIndex, this);
		}
		else if(team == null && playerInformation.GetInformationType() != typeof(Player))
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

	public bool CheckIfVisionTileIsUnlocked(ChunkData chunkData)
	{
		return _turnManager.GetTeamByIndex(_currentCharacterTeam).ContainsVisionTile(chunkData);
	}

	public override void PlayerWasDamaged()
	{
		actionManager.PlayerWasAttacked();
		debuffManager.PlayerWasAttacked();
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

	public void AddPoison(Poison poison)
	{
		if (_poisons == null)
		{
			_poisons = new LinkedList<Poison>();
		}

		_poisons.AddLast(poison);
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

	public void ClearPoison()
	{
		if (_poisons == null)
		{
			_poisons = new LinkedList<Poison>();
		}
		_poisons.Clear();
	}

	public override void OnTurnStart()
	{
		movementPoints = actionManager.ReturnBaseAbilities()[0].Action.attackRange;
		previousMovementPoints = movementPoints;
		PoisonPlayer();
		actionManager.OnTurnStart();
		Array<PlayerBlessing> playerBlessings = playerInformation.objectData.GetPlayerInformationData().GetAllPlayerBlessings();
		for (int i = 0; i < playerBlessings.Count; i++)
		{
			if (unlockedPLayerBlessings[i].blessingUnlocked)
			{
				playerBlessings[i].OnTurnStart(this);
			}
		}
		debuffManager.OnTurnStart();
		
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
		Array<PlayerBlessing> playerBlessings = playerInformation.objectData.GetPlayerInformationData().GetAllPlayerBlessings();
		for (int i = 0; i < playerBlessings.Count; i++)
		{
			if (unlockedPLayerBlessings[i].blessingUnlocked)
			{
				playerBlessings[i].OnTurnStart(this);
			}
		}
		debuffManager.OnTurnEnd();
	}
	
	public override void OnExit(ChunkData chunkDataPrev, ChunkData chunkData)
	{
		actionManager.ActionOnExit(chunkDataPrev, chunkData);
	}

	public int GetPoisonCount()
	{
		if (_poisons == null)
		{
			_poisons = new LinkedList<Poison>();
		}
		return _poisons.Count;
	}

	public void AddBarrier()
	{
		playerInformation.AddBarrier();
	}
	
	public void RemoveBarrier()
	{
		playerInformation.RemoveBarrier();
	}

	private void PoisonPlayer()
	{
		if (_poisons == null)
		{
			_poisons = new LinkedList<Poison>();
		}
		
		for (LinkedListNode<Poison> element = _poisons.First; element != null; element = element.Next)
		{
			Poison poison = element.Value;
			if (poison.poisonValue > 0 && poison.chunk.GetCurrentPlayer().playerInformation.GetHealth() > 0)
			{
				playerInformation.DealDamage(poison.poisonValue, poison.Poisoner);
				poison.turnsLeft--;
			}
			if(poison.turnsLeft == 0)
			{
				_poisons.Remove(element);
			}
		}
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

	public LinkedList<Poison> GetPoisons()
	{
		if (_poisons == null)
		{
			_poisons = new LinkedList<Poison>();
		}
		return _poisons;
	}
	
	public int TotalPoisonDamage()
	{
		int totalDamage = 0;
		if (_poisons == null)
		{
			_poisons = new LinkedList<Poison>();
		}
		for (LinkedListNode<Poison> element = _poisons.First; element != null; element = element.Next)
		{
			Poison poison = element.Value;
			totalDamage += poison.poisonValue;
		}
		return totalDamage;
	}
}
