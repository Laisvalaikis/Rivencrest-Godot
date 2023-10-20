using Godot;
using System.Collections.Generic;

public partial class Player : Node2D
{
	[Export] public int playerIndex = 0;
	[Export] public PlayerInformation playerInformation;
	[Export] public ActionManager actionManager;
	protected List<Poison> _poisons;

	public void AddPoison(Poison poison)
	{
		_poisons.Add(poison);
	}
	
	public void OnTurnStart()
	{
		PoisonPlayer();
	}

	public int GetPoisonCount()
	{
		return _poisons.Count;
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

	public List<Poison> GetPoisons()
	{
		return _poisons;
	}


}
