using System.Collections;
using Godot;
using Godot.Collections;

public partial class ActionManager : Node
{
	[Export] 
	private Player _player;
	[Export]
	private Array<Ability> _setupAbilities;
	private Array<Ability> _abilities;
	[Export]
	private int availableAttackPoints;
	[Export]
	private int availableMovementPoints;
	private bool hasSlowAbilityBeenCast = false;
	private int trackingCount = 0;

	public override void _Ready()
	{
		base._Ready();
		if (_setupAbilities.Count != 0)
		{
			for (int i = 0; i < _setupAbilities.Count; i++)
			{
				Ability ability = new Ability(_setupAbilities[i]);
				ability.Action.SetupAbility(_player);
				if (_abilities == null)
				{
					_abilities = new Array<Ability>();
					_abilities.Add(ability);
				}
				else
				{
					_abilities.Add(ability);
				}
			}
		}
	}

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

