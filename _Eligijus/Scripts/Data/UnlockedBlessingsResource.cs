using Godot;

public class UnlockedBlessingsResource : Resource
{
    public bool blessingUnlocked = false; 
    
    public UnlockedBlessingsResource()
    {
        blessingUnlocked = false;
    }

    public UnlockedBlessingsResource(UnlockedBlessingsResource data)
    {
        blessingUnlocked = data.blessingUnlocked;
    }

    public UnlockedBlessingsResource(UnlockedBlessings data)
    {
        blessingUnlocked = data.blessingUnlocked;
    }

}