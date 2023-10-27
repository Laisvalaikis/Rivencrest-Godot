using Godot;
using System.Collections.Generic;

public partial class Player : Node2D
{
	[Export] public int playerIndex = 0;
	[Export] public PlayerInformation playerInformation;
	[Export] public Debuffs debuffs;
	[Export] public ActionManager actionManager;
	private PlayerTeams team;
	protected List<Poison> _poisons;
	protected bool weakSpot = false;

	public void AddPoison(Poison poison)
	{
		if (_poisons == null)
		{
			_poisons = new List<Poison>();
		}

		_poisons.Add(poison);
	}

	public void SetPlayerTeam(PlayerTeams teams)
	{
		team = teams;
	}

	public PlayerTeams GetPlayerTeams()
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
		PoisonPlayer();
		actionManager.OnTurnStart();
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
