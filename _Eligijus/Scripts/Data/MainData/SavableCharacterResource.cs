using Godot;
using Godot.Collections;
using System;

public partial class SavableCharacterResource : Resource
{
	[Export]
	public int level;
	[Export]
	public int xP;
	[Export]
	public int xPToGain;
	[Export]
	public string characterName;
	[Export]
	public int abilityPointCount;
	[Export]
	public Array<UnlockedAbilitiesResource> unlockedAbilities;
	[Export]
	public Array<AbilityBlessingsResource> abilityBlessings;
	[Export]
	public Array<UnlockedBlessingsResource> characterBlessings;
	[Export]
	public int toConfirmAbilities = 0;
	[Export]
	public int cost;
	[Export]
	public int characterIndex;
	[Export]
	public PlayerInformationDataNew playerInformation;

	public SavableCharacterResource()
	{
		
	}

	public SavableCharacterResource(SavableCharacterResource data)
	{
		level = data.level;
		xP = data.xP;
		xPToGain = data.xPToGain;
		characterName = data.characterName;
		abilityPointCount = data.abilityPointCount;
		unlockedAbilities = data.unlockedAbilities;
		toConfirmAbilities = data.toConfirmAbilities;
		abilityBlessings = new Array<AbilityBlessingsResource>(data.abilityBlessings);
		characterBlessings = new Array<UnlockedBlessingsResource>(data.characterBlessings);
		cost = data.cost;
		characterIndex = data.characterIndex;
		playerInformation = data.playerInformation;
	}

	public SavableCharacterResource(SavableCharacter data)
	{
		level = data.level;
		xP = data.xP;
		xPToGain = data.xPToGain;
		characterName = data.characterName;
		abilityPointCount = data.abilityPointCount;
		unlockedAbilities = new Array<UnlockedAbilitiesResource>();
		for (int i = 0; i < data.unlockedAbilities.Count; i++)
		{
			unlockedAbilities.Add(new UnlockedAbilitiesResource(data.unlockedAbilities[i]));
		}
		toConfirmAbilities = data.toConfirmAbilities;
		abilityBlessings = new Array<AbilityBlessingsResource>();
		for (int i = 0; i < data.abilityBlessings.Count; i++)
		{
			abilityBlessings.Add(new AbilityBlessingsResource(data.abilityBlessings[i]));
		}
		characterBlessings = new Array<UnlockedBlessingsResource>();
		for (int i = 0; i < data.characterBlessings.Count; i++)
		{
			characterBlessings.Add(new UnlockedBlessingsResource(data.characterBlessings[i]));
		}
		cost = data.cost;
		characterIndex = data.characterIndex;
		playerInformation = data.playerInformation;
	}
	
	
	public SavableCharacterResource(SavableCharacter data, PlayerInformationDataNew playerInformation)
	{
		level = data.level;
		xP = data.xP;
		xPToGain = data.xPToGain;
		characterName = data.characterName;
		abilityPointCount = data.abilityPointCount;
		unlockedAbilities = new Array<UnlockedAbilitiesResource>();
		for (int i = 0; i < data.unlockedAbilities.Count; i++)
		{
			unlockedAbilities.Add(new UnlockedAbilitiesResource(data.unlockedAbilities[i]));
		}
		toConfirmAbilities = data.toConfirmAbilities;
		abilityBlessings = new Array<AbilityBlessingsResource>();
		for (int i = 0; i < data.abilityBlessings.Count; i++)
		{
			abilityBlessings.Add(new AbilityBlessingsResource(data.abilityBlessings[i]));
		}
		characterBlessings = new Array<UnlockedBlessingsResource>();
		for (int i = 0; i < data.characterBlessings.Count; i++)
		{
			characterBlessings.Add(new UnlockedBlessingsResource(data.characterBlessings[i]));
		}
		cost = data.cost;
		characterIndex = data.characterIndex;
		this.playerInformation = playerInformation;
	}
}
