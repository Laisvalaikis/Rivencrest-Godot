using Godot;
using System.Collections.Generic;

public partial class Player : Node2D
{
	[Export] public int playerIndex = 0;
	[Export] public PlayerInformation playerInformation;
	[Export] public Debuffs debuffs;
	[Export] public ActionManager actionManager;
	protected List<Poison> _poisons;
	protected bool weakSpot = false;

	public void AddPoison(Poison poison)
	{
		_poisons.Add(poison);
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
		return _poisons;
	}
	
	public int TotalPoisonDamage()
	{
		int totalDamage = 0;
		foreach (Poison poison in _poisons)
		{
			totalDamage += poison.poisonValue;
		}
		return totalDamage;
	}


}
