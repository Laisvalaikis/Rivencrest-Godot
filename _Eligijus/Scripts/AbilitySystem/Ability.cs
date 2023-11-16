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
	[Export] 
	public Array<UnlockedBlessingsResource> unlockedBlessingsResources;

	public Ability()
	{
		unlockedBlessingsResources = new Array<UnlockedBlessingsResource>();
	}

	public Ability(Ability ability)
	{
		_type = ability._type;
		abilityText = ability.abilityText;
		enabled = ability.enabled;
		AbilityImage = ability.AbilityImage;
		BaseAction action = ability.Action.CreateNewInstance(ability.Action);
		Action = action;
		unlockedBlessingsResources = ability.unlockedBlessingsResources;
	}

}
