using Godot;
using Godot.Collections;

public partial class TownHallDataResource: Resource
{
    [Export]
    public int maxCharacterCount;
    [Export]
    public int attractedCharactersCount;
    [Export]
    public int attractedCharacterLevel;
    [Export]
    public int maxCharacterLevel;
    [Export]
    public int characterReRoll;
    [Export]
    public int damagedMerchant;

    public TownHallDataResource(TownHallDataResource data)
    {
        maxCharacterCount = data.maxCharacterCount;
        attractedCharactersCount = data.attractedCharactersCount;
        attractedCharacterLevel = data.attractedCharacterLevel;
        characterReRoll = data.characterReRoll;
        damagedMerchant = data.damagedMerchant;
    }

    public TownHallDataResource(TownHallData data)
    {
        maxCharacterCount = data.maxCharacterCount;
        attractedCharactersCount = data.attractedCharactersCount;
        attractedCharacterLevel = data.attractedCharacterLevel;
        characterReRoll = data.characterReRoll;
        damagedMerchant = data.damagedMerchant;
    }
    
    public int GetByType(TownHallUpgrade townHallUpgrade)
    {
        if (townHallUpgrade == TownHallUpgrade.MaxCharacterCount)
        {
            return maxCharacterCount;
        }
        else if(townHallUpgrade == TownHallUpgrade.AttractedCharacterCount)
        {
            return attractedCharactersCount;
        }
        else if (townHallUpgrade == TownHallUpgrade.MaxCharacterLevel)
        {
            return maxCharacterLevel;
        }
        else if (townHallUpgrade == TownHallUpgrade.CharacterReRoll)
        {
            return characterReRoll;
        }
        else if (townHallUpgrade == TownHallUpgrade.AttractedCharacterLevel)
        {
            return attractedCharacterLevel;
        }
        else if (townHallUpgrade == TownHallUpgrade.MerchantStatus)
        {
            return damagedMerchant;
        }

        return -1;
    }

    public void SetByType(TownHallUpgrade townHallUpgrade, int value)
    {
        if (townHallUpgrade == TownHallUpgrade.MaxCharacterCount)
        {
            maxCharacterCount = value;
        }
        else if(townHallUpgrade == TownHallUpgrade.AttractedCharacterCount)
        {
            attractedCharactersCount = value;
        }
        else if (townHallUpgrade == TownHallUpgrade.MaxCharacterLevel)
        {
            maxCharacterLevel = value;
        }
        else if (townHallUpgrade == TownHallUpgrade.CharacterReRoll)
        {
            characterReRoll = value;
        }
        else if (townHallUpgrade == TownHallUpgrade.AttractedCharacterLevel)
        {
            attractedCharacterLevel = value;
        }
        else if (townHallUpgrade == TownHallUpgrade.MerchantStatus)
        {
            damagedMerchant = value;
        }
    }
    
}