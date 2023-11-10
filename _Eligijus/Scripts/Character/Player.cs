using Godot;
using System.Collections.Generic;

public partial class Player : Node2D
{
	[Export] public int playerIndex = 0;
	[Export] public PlayerInformation playerInformation;
	[Export] public Debuffs debuffs;
	[Export] public ActionManager actionManager;
	private int _currentCharacterTeam = -1;
	private CharacterTeams team;
	protected List<Poison> _poisons;
	protected bool weakSpot = false;
	private int movementPoints=3;

	public void Death()
	{
		Hide();
		actionManager.Die();
		ChunkData chunkData = GameTileMap.Tilemap.GetChunk(GlobalPosition);
		if (team != null)
		{
			team.CharacterDeath(chunkData, _currentCharacterTeam, playerIndex, this);
		}
		else if(team == null && playerInformation.GetInformationType() != InformationType.Player && playerInformation.GetInformationType() != InformationType.Enemy)
		{
			chunkData.SetCurrentCharacter(null);
			chunkData.GetTileHighlight().DisableHighlight();
			QueueFree();
		}
	}

	public int GetMovementPoints()
	{
		return movementPoints;
	}

	public void AddMovementPoints(int points)
	{
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
		movementPoints = 0;
	}

	public void AddPoison(Poison poison)
	{
		if (_poisons == null)
		{
			_poisons = new List<Poison>();
		}

		_poisons.Add(poison);
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
			_poisons = new List<Poison>();
		}
		_poisons.Clear();
	}

	public void OnTurnStart()
	{
		movementPoints = actionManager.ReturnBaseAbilities()[0].Action.attackRange;
		PoisonPlayer();
		actionManager.OnTurnStart();
	}
	
	public void OnAfterResolve()
	{
		actionManager.OnAfterResolve();
	}
	
	public void OnBeforeStart()
	{
		actionManager.OnBeforeStart();
	}

	
	public void OnTurnEnd()
	{
		actionManager.OnTurnEnd();
	}

	public int GetPoisonCount()
	{
		if (_poisons == null)
		{
			_poisons = new List<Poison>();
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
			_poisons = new List<Poison>();
		}
		foreach (Poison poison in _poisons)
		{
			if (poison.poisonValue > 0 && poison.chunk.GetCurrentPlayer().playerInformation.GetHealth() > 0)
			{
				playerInformation.DealDamage(poison.poisonValue, false, poison.Poisoner);
			}
			poison.turnsLeft--;
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

	public List<Poison> GetPoisons()
	{
		if (_poisons == null)
		{
			_poisons = new List<Poison>();
		}
		return _poisons;
	}
	
	public int TotalPoisonDamage()
	{
		int totalDamage = 0;
		if (_poisons == null)
		{
			_poisons = new List<Poison>();
		}
		foreach (Poison poison in _poisons)
		{
			totalDamage += poison.poisonValue;
		}
		return totalDamage;
	}


}
