using Godot;

public partial class SavedCharacterResource : SavableCharacterResource
{
    [Export]
    public Resource prefab;

    public SavedCharacterResource()
    {
        
    }

    public SavedCharacterResource(Resource prefab, SavableCharacterResource data) : base(data)
    {
        this.prefab = prefab;
    }
    
    public SavedCharacterResource(Resource prefab, SavableCharacter data) : base(data)
    {
        this.prefab = prefab;
    }
    
    public SavedCharacterResource(SavedCharacterResource data) : base(data)
    {
        this.prefab = data.prefab;
    }
    
    public SavedCharacterResource(SavedCharacter data) : base(data)
    {
        this.prefab = data.prefab;
    }
    
    public SavedCharacterResource(SavableCharacterResource x, Resource prefab, PlayerInformationData playerInformation) : base(x)
    {
        this.prefab = prefab;
        this.playerInformation = playerInformation;
    }
    
    // public string CharacterTableBlessingString()
    // {
    //     // string blessingsInOneString = "";
    //     // for(int i = 0; i < blessings.Count; i++)
    //     // {
    //     //     blessingsInOneString += blessings[i].blessingName;
    //     //     if (i != blessings.Count - 1) blessingsInOneString += "\n";
    //     // }
    //     return "blessingsInOneString";
    // }
    
}