
[System.Serializable]
public class UnlockedBlessings
{ 
    public string blessingName;
    public int rarity;
    public string condition;
    public string description;
    public bool blessingUnlocked = false; 
    
    public UnlockedBlessings()
    {
        blessingUnlocked = false;
    }

    public UnlockedBlessings(UnlockedBlessingsResource data)
    {
        blessingName = data.blessingName;
        rarity = data.rarity;
        condition = data.condition;
        description = data.description;
        blessingUnlocked = data.blessingUnlocked;
    }
    
    public UnlockedBlessings(UnlockedBlessings data)
    {
        blessingName = data.blessingName;
        rarity = data.rarity;
        condition = data.condition;
        description = data.description;
        blessingUnlocked = data.blessingUnlocked;
    }

}
