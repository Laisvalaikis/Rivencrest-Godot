using Godot;
using Godot.Collections;

public partial class AbilityBlessingsResource : Resource
{
    public Array<UnlockedBlessingsResource> UnlockedBlessingsList { get; set; }

    public AbilityBlessingsResource()
    {
        UnlockedBlessingsList = new Array<UnlockedBlessingsResource>();
    }
    
    public AbilityBlessingsResource(AbilityBlessingsResource data)
    {
        UnlockedBlessingsList = new Array<UnlockedBlessingsResource>(data.UnlockedBlessingsList);
    }
    
    public AbilityBlessingsResource(AbilityBlessings data)
    {
        UnlockedBlessingsList = new Array<UnlockedBlessingsResource>();
        for (int i = 0; i < data.UnlockedBlessingsList.Count; i++)
        {
            UnlockedBlessingsList.Add(new UnlockedBlessingsResource(data.UnlockedBlessingsList[i]));
        }
    }
}