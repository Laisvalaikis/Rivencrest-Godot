using System.Collections;
using Godot;
using Godot.Collections;

public partial class ActionManager : Node
{
	[Export]
	private Array<Ability> _abilities;
	[Export]
	private int availableAttackPoints;
	[Export]
	private int availableMovementPoints;

	private bool hasSlowAbilityBeenCast = false;

	public Array<Ability> ReturnAbilities()
	{
		return _abilities;
	}
	
	public virtual void RemoveActionPoints()
	{
		availableAttackPoints--;
	}
	
	public void RemoveAllActionPoints() 
	{
		availableMovementPoints = 0;
		hasSlowAbilityBeenCast = true;

	}
}

