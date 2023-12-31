using System;
using System.Collections;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

public partial class Ability : Resource
{
	[Export] 
	public AbilityType _type = AbilityType.None;
	[Export]
	public AbilityText abilityText;
	[Export]
	public bool enabled = true;
	[Export]
	public Texture AbilityImage;
	[Export]
	public BaseAction Action;

	public Ability()
	{
	}

	public Ability(Ability ability)
	{
		_type = ability._type;
		abilityText = ability.abilityText;
		enabled = ability.enabled;
		AbilityImage = ability.AbilityImage;
		BaseAction action = ability.Action.CreateNewInstance(ability.Action);
		Action = action;
	}
	
	public bool Equals(Type type)
	{
		//Check for null and compare run-time types.
		if (type == null || !this.Action.GetType().Equals(type))
		{
			return false;
		}

		return true;
	}

}
