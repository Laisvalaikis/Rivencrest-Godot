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
	public Array<UnlockedBlessingsResource> blessings;
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
		blessings = this.blessings = new Array<UnlockedBlessingsResource>(data.blessings);
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
		blessings = new Array<UnlockedBlessingsResource>();
		for (int i = 0; i < data.blessings.Count; i++)
		{
			blessings.Add(new UnlockedBlessingsResource(data.blessings[i]));
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
		blessings = new Array<UnlockedBlessingsResource>();
		for (int i = 0; i < data.blessings.Count; i++)
		{
			blessings.Add(new UnlockedBlessingsResource(data.blessings[i]));
		}
		cost = data.cost;
		characterIndex = data.characterIndex;
		this.playerInformation = playerInformation;
	}
}
