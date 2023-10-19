using Godot;
using System.Collections.Generic;

public partial class Player : Node2D
{
	[Export] public int playerIndex = 0;
	[Export] public PlayerInformation playerInformation;
	[Export] public ActionManager actionManager;
	[Export]
	public int abilityCooldown = 1;
	protected int abilityPoints;
	protected List<Poison> _poisons;
	
	public void SetAbilityPoints(int abilityPoints)
	{
		abilityCooldown = abilityPoints;
	}
	
	public void AddAbilityCooldownPoints(int abilityPoints)
	{
		abilityCooldown += abilityPoints;
	}

	public void AddAbilityPoints(int abilityPoints)
	{
		this.abilityPoints += abilityPoints;
	}

	public void AddPoison(Poison poison)
	{
		_poisons.Add(poison);
	}
	
	public virtual void RefillActionPoints() //pradzioj ejimo
	{
		abilityPoints = abilityCooldown;
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
	
	
}
