
using System.Collections.Generic;

[System.Serializable]
public class UnlockedAbilities
{ 
	public bool abilityConfirmed = false; 
	public bool abilityUnlocked = false; 
	
	public UnlockedAbilities()
	{
	   abilityConfirmed = false; 
	   abilityUnlocked = false;
	}
	
	public UnlockedAbilities(UnlockedAbilities data)
	{
		abilityConfirmed = data.abilityConfirmed;
		abilityUnlocked = data.abilityUnlocked;
	}

	public UnlockedAbilities(UnlockedAbilitiesResource data)
	{
	   abilityConfirmed = data.abilityConfirmed;
	   abilityUnlocked = data.abilityUnlocked;
	}
	
}
