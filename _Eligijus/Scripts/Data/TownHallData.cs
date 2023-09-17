using System.Collections;
using System.Collections.Generic;


public enum TownHallUpgrade
{
    MaxCharacterCount = 0,
    AttractedCharacterCount = 1,
    MaxCharacterLevel = 2,
    CharacterReRoll = 3,
    AttractedCharacterLevel = 4,
    MerchantStatus = 5

}

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

    public int maxCharacterCount;
    public int attractedCharactersCount;
    public int maxCharacterLevel;
    public int characterReRoll;
    public int attractedCharacterLevel;
    public int damagedMerchant;

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
