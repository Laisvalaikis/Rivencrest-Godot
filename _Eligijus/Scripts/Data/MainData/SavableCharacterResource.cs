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
	public bool dead;
	[Export]
	public int abilityPointCount;
	[Export]
	public Array<UnlockedAbilitiesResource> unlockedAbilities;
	[Export]
	public int toConfirmAbilities = 0;
	[Export]
	public Array<UnlockedBlessingsResource> baseAbilityBlessings;
	[Export]
	public Array<UnlockedBlessingsResource> characterBlessings;
	[Export]
	public int cost;
	[Export]
	public int characterIndex;
	[Export]
	public PlayerInformationData playerInformation;

	public SavableCharacterResource()
	{
		
	}

	public SavableCharacterResource(SavableCharacterResource data)
	{
		level = data.level;
		xP = data.xP;
		xPToGain = data.xPToGain;
		dead = data.dead;
		characterName = data.characterName;
		abilityPointCount = data.abilityPointCount;
		unlockedAbilities = data.unlockedAbilities;
		toConfirmAbilities = data.toConfirmAbilities;
		characterBlessings = this.characterBlessings = new Array<UnlockedBlessingsResource>(data.characterBlessings);
		cost = data.cost;
		characterIndex = data.characterIndex;
		playerInformation = data.playerInformation;
	}

	public SavableCharacterResource(SavableCharacter data)
	{
		level = data.level;
		xP = data.xP;
		xPToGain = data.xPToGain;
		dead = data.dead;
		characterName = data.characterName;
		abilityPointCount = data.abilityPointCount;
		unlockedAbilities = new Array<UnlockedAbilitiesResource>();
		for (int i = 0; i < data.unlockedAbilities.Count; i++)
		{
			unlockedAbilities.Add(new UnlockedAbilitiesResource(data.unlockedAbilities[i]));
		}
		toConfirmAbilities = data.toConfirmAbilities;
		baseAbilityBlessings = new Array<UnlockedBlessingsResource>();
		for (int i = 0; i < data.characterBlessings.Count; i++)
		{
			baseAbilityBlessings.Add(new UnlockedBlessingsResource(data.baseAbilityBlessings[i]));
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
	
	
	public SavableCharacterResource(SavableCharacter data, PlayerInformationData playerInformation)
	{
		level = data.level;
		xP = data.xP;
		xPToGain = data.xPToGain;
		dead = data.dead;
		characterName = data.characterName;
		abilityPointCount = data.abilityPointCount;
		unlockedAbilities = new Array<UnlockedAbilitiesResource>();
		for (int i = 0; i < data.unlockedAbilities.Count; i++)
		{
			unlockedAbilities.Add(new UnlockedAbilitiesResource(data.unlockedAbilities[i]));
		}
		toConfirmAbilities = data.toConfirmAbilities;
		baseAbilityBlessings = new Array<UnlockedBlessingsResource>();
		for (int i = 0; i < data.baseAbilityBlessings.Count; i++)
		{
			baseAbilityBlessings.Add(new UnlockedBlessingsResource(baseAbilityBlessings[i]));
		}
		characterBlessings = new Array<UnlockedBlessingsResource>();
		for (int i = 0; i < data.characterBlessings.Count; i++)
		{
			characterBlessings.Add(new UnlockedBlessingsResource(characterBlessings[i]));
		}
		cost = data.cost;
		characterIndex = data.characterIndex;
		this.playerInformation = playerInformation;
	}
}
