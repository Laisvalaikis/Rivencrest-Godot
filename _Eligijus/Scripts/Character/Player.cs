using Godot;
using Godot.Collections;

public partial class Player : Object
{
	[Export] public int playerInTeamIndex = 0;
	[Export] public PlayerInformation playerInformation;
	[Export] public Debuffs debuffs;
	[Export] public ActionManager actionManager;
	[Export] public DeBuffManager deBuffManager;
	public Array<UnlockedAbilitiesResource> unlockedAbilityList;
	public Array<AbilityBlessingsResource> unlockedBlessingList;
	public Array<UnlockedBlessingsResource> unlockedPLayerBlessings;
	private int _currentCharacterTeam = -1;
	private CharacterTeams team;
	protected LinkedList<Poison> _poisons;
	protected bool weakSpot = false;
	private int movementPoints = 3; //track movement points here in this class
	private int previousMovementPoints = 3;

	public override void _EnterTree()
	{
		base._EnterTree();
		// objectInformation.objectData = playerInformation.playerInformationData;
	}

	public override void Death()
	{
		Hide();
		actionManager.PlayerDied();
		deBuffManager.PlayerDied();
		ChunkData chunkData = GameTileMap.Tilemap.GetChunk(GlobalPosition);
		if (team != null)
		{
			team.CharacterDeath(chunkData, _currentCharacterTeam, playerInTeamIndex, this);
		}
		else if(team == null && playerInformation.GetInformationType() != InformationType.Player && playerInformation.GetInformationType() != InformationType.Enemy)
		{
			chunkData.SetCurrentCharacter(null);
			chunkData.GetTileHighlight().DisableHighlight();
			QueueFree();
		}
	}

	public override void PlayerWasDamaged()
	{
		actionManager.PlayerWasAttacked();
		deBuffManager.PlayerWasAttacked();
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
		Array<PlayerBlessing> playerBlessings = playerInformation.playerInformationData.GetAllPlayerBlessings();
		for (int i = 0; i < playerBlessings.Count; i++)
		{
			if (unlockedPLayerBlessings[i].blessingUnlocked)
			{
				playerBlessings[i].OnTurnStart(this);
			}
		}
		deBuffManager.OnTurnStart();
		
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
		Array<PlayerBlessing> playerBlessings = playerInformation.playerInformationData.GetAllPlayerBlessings();
		for (int i = 0; i < playerBlessings.Count; i++)
		{
			if (unlockedPLayerBlessings[i].blessingUnlocked)
			{
				playerBlessings[i].OnTurnStart(this);
			}
		}
		deBuffManager.OnTurnEnd();
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
