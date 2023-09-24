using Godot;

public partial class UnlockedAbilitiesResource: Resource
{
    [Export]
    public bool abilityUnlocked = false;
    [Export]
    public bool abilityConfirmed = false;

    public UnlockedAbilitiesResource(UnlockedAbilitiesResource data)
    {
        abilityUnlocked = data.abilityUnlocked;
        abilityConfirmed = data.abilityConfirmed;
    }

    public UnlockedAbilitiesResource(UnlockedAbilities data)
    {
        abilityUnlocked = data.abilityUnlocked;
        abilityConfirmed = data.abilityConfirmed;
    }

}