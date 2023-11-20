using Godot;
using Godot.Collections;

public partial class ObjectData : Resource
{
    [Export]
    public int maxHealth = 1;
    [Export]
    public int accuracy = 100;
    [Export]
    public Color classColor;
    [Export]
    public Color secondClassColor;
    [Export]
    public Color textColor;
    [Export]
    public Color backgroundColor;
    [Export]
    public Texture CharacterPortraitSprite;
    [Export] 
    public bool canStepOnObject = false;
    [Export] 
    public bool destroyObjectStepingOn = false;
    [Export]
    public Array<Ability> abilities;
    [Export]
    public Array<PlayerBlessing> blessings;
    
    public ObjectData()
    {
		
    }
	
    public void CopyData(ObjectData informationData)
    {
        maxHealth = informationData.maxHealth;
        accuracy = informationData.accuracy;
        classColor = informationData.classColor;
        secondClassColor = informationData.secondClassColor;
        textColor = informationData.textColor;
        backgroundColor = informationData.backgroundColor;
        CharacterPortraitSprite = informationData.CharacterPortraitSprite;
        abilities = informationData.abilities;
        blessings = informationData.blessings;
    }
    
    public Array<Ability> GetAllAbilities()
    {
        return abilities;
    }
    
    public Array<PlayerBlessing> GetAllBlessings()
    {
        return blessings;
    }
}