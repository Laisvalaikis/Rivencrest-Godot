using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TownHallData
{

    public TownHallData()
    {
        maxCharacterCount = 0;
        attractedCharactersCount = 0;
        maxCharacterLevel = 0;
        characterReRoll = 0;
        attractedCharacterLevel = 0;
        damagedMerchant = 0;
    }

    public int maxCharacterCount { get; set; }
    public int attractedCharactersCount { get; set; }
    public int maxCharacterLevel { get; set; }
    public int characterReRoll { get; set; }
    public int attractedCharacterLevel { get; set; }
    public int damagedMerchant { get; set; }

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

    public TownHallData(TownHallDataResource data)
    {
        maxCharacterCount = data.maxCharacterCount;
        attractedCharactersCount = data.attractedCharactersCount;
        attractedCharacterLevel = data.attractedCharacterLevel;
        maxCharacterLevel = data.maxCharacterLevel;
        characterReRoll = data.characterReRoll;
        damagedMerchant = data.damagedMerchant;
    }

}
