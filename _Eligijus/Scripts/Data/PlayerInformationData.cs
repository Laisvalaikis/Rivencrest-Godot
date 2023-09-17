using System.Collections;
using System.Collections.Generic;
using Godot;
public class PlayerInformationData
{
    
    public string ClassName;
    public int MaxHealth = 100;
    public int critChance = 5;
    public int accuracy = 100;
    public int dodgeChance = 20;
    public string role;

    [Header("Color")] 
    public Color classColor;
    public Color secondClassColor;
    public Color textColor;
    public Color backgroundColor;
    [Header("Images")] 
    public Sprite CharacterPortraitSprite;
    public Sprite CharacterSplashArt;//For character table
    public Sprite CroppedSplashArt;
    public Sprite characterSprite;
    public Sprite roleSprite;
    public List<Sprite> abilitySprites;
    public List<AbilityData> abilities;
    public List<Blessing> BlessingsAndCurses = new List<Blessing>();
    

    public void CopyData(PlayerInformationData playerInformationData)
    {
        ClassName = playerInformationData.ClassName;
        MaxHealth = playerInformationData.MaxHealth;
        critChance = playerInformationData.critChance;
        accuracy = playerInformationData.accuracy;
        dodgeChance = playerInformationData.dodgeChance;
        role = playerInformationData.role;

        classColor = playerInformationData.classColor;
        secondClassColor = playerInformationData.secondClassColor;
        textColor = playerInformationData.textColor;
        backgroundColor = playerInformationData.backgroundColor;

        CharacterPortraitSprite = playerInformationData.CharacterPortraitSprite;
        CharacterSplashArt = playerInformationData.CharacterSplashArt;
        CroppedSplashArt = playerInformationData.CroppedSplashArt;
        characterSprite = playerInformationData.characterSprite;
        roleSprite = playerInformationData.roleSprite;
        abilitySprites = playerInformationData.abilitySprites;
        abilities = playerInformationData.abilities;

        BlessingsAndCurses = playerInformationData.BlessingsAndCurses;
    }

}



[System.Serializable]
public class AbilityData
{
    public Sprite sprite;
    // public AbilityAction abilityAction;
}

