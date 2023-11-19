using Godot;
using Godot.Collections;

public partial class ObjectData : Resource
{
    [Export]
    public int maxHealth = 1;
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
    protected Array<Ability> abilities;
    [Export]
    protected Array<PlayerBlessing> blessings;
    
    public ObjectData()
    {
		
    }
	
    public void CopyData(ObjectData informationData)
    {
        maxHealth = informationData.maxHealth;
        classColor = informationData.classColor;
        textColor = informationData.textColor;
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