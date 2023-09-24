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
	public Array<Blessing> blessings;
	[Export]
	public int cost;
	[Export]
	public int characterIndex;
	[Export]
	public PlayerInformationData playerInformation;
}
