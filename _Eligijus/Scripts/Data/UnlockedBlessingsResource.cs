using Godot;

public partial class UnlockedBlessingsResource : Resource
{
    [Export ]public string blessingName;
    [Export] public int rarity;
    [Export] public string condition;
    [Export] public string description;
    [Export] public bool blessingUnlocked = false; 
    
    public UnlockedBlessingsResource()
    {
        blessingUnlocked = false;
    }

    public UnlockedBlessingsResource(UnlockedBlessingsResource data)
    {
        blessingName = data.blessingName;
        rarity = data.rarity;
        condition = data.condition;
        description = data.description;
        blessingUnlocked = data.blessingUnlocked;
    }

    public UnlockedBlessingsResource(UnlockedBlessings data)
    {
        blessingName = data.blessingName;
        rarity = data.rarity;
        condition = data.condition;
        description = data.description;
        blessingUnlocked = data.blessingUnlocked;
    }
    
    public UnlockedBlessingsResource(BaseBlessing data)
    {
        blessingName = data.blessingName;
        rarity = data.rarity;
        condition = data.condition;
        description = data.description;
        blessingUnlocked = data.IsBlessingUnlocked();
    }
}