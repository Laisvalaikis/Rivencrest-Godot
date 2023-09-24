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
}