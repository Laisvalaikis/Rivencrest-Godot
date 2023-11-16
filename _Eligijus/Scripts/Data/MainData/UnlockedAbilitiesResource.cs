using Godot;
using Godot.Collections;

public partial class UnlockedAbilitiesResource: Resource
{
    [Export]
    public bool abilityUnlocked = false;
    [Export]
    public bool abilityConfirmed = false;
    [Export]
    public Array<UnlockedBlessingsResource> unlockedBlessings;


    public UnlockedAbilitiesResource()
    {
        unlockedBlessings = new Array<UnlockedBlessingsResource>();
    }

    public UnlockedAbilitiesResource(UnlockedAbilitiesResource data)
    {
        abilityUnlocked = data.abilityUnlocked;
        abilityConfirmed = data.abilityConfirmed;
        unlockedBlessings = new Array<UnlockedBlessingsResource>(data.unlockedBlessings);
    }

    public UnlockedAbilitiesResource(UnlockedAbilities data)
    {
        abilityUnlocked = data.abilityUnlocked;
        abilityConfirmed = data.abilityConfirmed;
        unlockedBlessings = new Array<UnlockedBlessingsResource>();
        for (int i = 0; i < data.unlockedBlessings.Count; i++)
        {
            unlockedBlessings.Add(new UnlockedBlessingsResource(data.unlockedBlessings[i]));
        }
    }

}