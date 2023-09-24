
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

	public UnlockedAbilities(UnlockedAbilitiesResource data)
	{
	   abilityConfirmed = data.abilityConfirmed;
	   abilityUnlocked = data.abilityUnlocked;
	}

}
