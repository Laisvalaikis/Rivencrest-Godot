using System.Collections;
using Godot;
using Godot.Collections;

public partial class ActionManager : Node
{
	[Export] 
	private Player _player;
	private Array<Ability> _baseAbilities;
	private Array<Ability> _abilities;
	private Array<Ability> _allAbilities;
	[Export]
	private int availableAttackPoints;
	[Export]
	private int availableMovementPoints;
	private bool hasSlowAbilityBeenCast = false;
	private int trackingCount = 0;

	public override void _Ready()
	{
		base._Ready();
		_abilities = new Array<Ability>();
		Array<Ability> allAbilities = new Array<Ability>();
		_baseAbilities = _player.playerInformation.playerInformationData.baseAbilities;
		allAbilities.AddRange(_player.playerInformation.playerInformationData.baseAbilities);
		allAbilities.AddRange(_player.playerInformation.playerInformationData.abilities);
		if (allAbilities != null && allAbilities.Count != 0)
		{
			for (int i = 0; i < allAbilities.Count; i++)
			{
				Ability ability = new Ability(allAbilities[i]);
				ability.Action.SetupAbility(_player);
				if (_allAbilities == null)
				{
					_allAbilities = new Array<Ability>();
					_allAbilities.Add(ability);
				}
				else
				{
					_allAbilities.Add(ability);
				}
			}

			for (int i = _baseAbilities.Count; i < allAbilities.Count; i++)
			{
				_abilities.Add(_allAbilities[i]);
			}
		}
	}

	public Array<Ability> ReturnAllAbilities()
	{
		return _allAbilities;
	}
	
	public Array<Ability> ReturnAbilities()
	{
		return _abilities;
	}
	
	public Array<Ability> ReturnBaseAbilities()
	{
		return _baseAbilities;
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

