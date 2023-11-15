
[System.Serializable]
public class UnlockedBlessings
{ 
    public bool blessingUnlocked = false; 
    
    public UnlockedBlessings()
    {
        blessingUnlocked = false;
    }

    public UnlockedBlessings(UnlockedBlessingsResource data)
    {
        blessingUnlocked = data.blessingUnlocked;
    }
    
    public UnlockedBlessings(UnlockedBlessings data)
    {
        blessingUnlocked = data.blessingUnlocked;
    }

}
