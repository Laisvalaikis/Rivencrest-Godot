
using System.Collections.Generic;

[System.Serializable]
public class UnlockedAbilities
{ 
	public bool abilityConfirmed = false; 
	public bool abilityUnlocked = false; 
	public List<UnlockedBlessings> unlockedBlessings;
	
	public UnlockedAbilities()
	{
	   abilityConfirmed = false; 
	   abilityUnlocked = false;
	   unlockedBlessings = new List<UnlockedBlessings>();
	}
	
	public UnlockedAbilities(UnlockedAbilities data)
	{
		abilityConfirmed = data.abilityConfirmed;
		abilityUnlocked = data.abilityUnlocked;
		unlockedBlessings = new List<UnlockedBlessings>(data.unlockedBlessings);
	}

	public UnlockedAbilities(UnlockedAbilitiesResource data)
	{
	   abilityConfirmed = data.abilityConfirmed;
	   abilityUnlocked = data.abilityUnlocked;
	   unlockedBlessings = new List<UnlockedBlessings>();
	   for (int i = 0; i < data.unlockedBlessings.Count; i++)
	   {
		   unlockedBlessings.Add(new UnlockedBlessings(data.unlockedBlessings[i]));
	   }
	}
	
	

}
