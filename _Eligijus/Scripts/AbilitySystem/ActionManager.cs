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
	private int availableAbilityPoints = 1;
	private int abilityPoints;
	private bool hasSlowAbilityBeenCast = false;
	private int trackingCount = 0;

	public override void _Ready()
	{
		base._Ready();
		_abilities = new Array<Ability>();
		_baseAbilities = new Array<Ability>();
		Array<Ability> allAbilities = new Array<Ability>();
		Array<Ability> baseAbilities = _player.playerInformation.playerInformationData.baseAbilities;
		allAbilities.AddRange(baseAbilities);
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
			
			for (int i = 0; i < baseAbilities.Count; i++)
			{
				_baseAbilities.Add(_allAbilities[i]);
			}

			for (int i = _baseAbilities.Count; i < allAbilities.Count; i++)
			{
				_abilities.Add(_allAbilities[i]);
			}
		}
	}

	public int GetAllAbilityPoints()
	{
		return availableAbilityPoints;
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
	
	
	public void RemoveAllActionPoints() 
	{
		hasSlowAbilityBeenCast = true;
		abilityPoints = 0;
	}
	
	public void SetAbilityPoints(int abilityPoints)
	{
		availableAbilityPoints = abilityPoints;
	}
	
	public void AddAbilityCooldownPoints(int abilityPoints)
	{
		availableAbilityPoints += abilityPoints;
	}

	public void AddAbilityPoints(int abilityPoints)
	{
		this.abilityPoints += abilityPoints;
	}
	
	public virtual void RefillActionPoints() //pradzioj ejimo
	{
		abilityPoints = availableAbilityPoints;
	}
	
}

