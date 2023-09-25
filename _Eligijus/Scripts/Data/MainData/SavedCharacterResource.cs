using Godot;

public partial class SavedCharacterResource : SavableCharacterResource
{
    [Export] 
    public string prefabPath;
    [Export]
    public Node prefab;

    public SavedCharacterResource()
    {
        
    }

    public SavedCharacterResource(Node prefab, SavableCharacterResource data) : base(data)
    {
        this.prefab = prefab;
    }
    
    public SavedCharacterResource(Node prefab, SavableCharacter data) : base(data)
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
    
    public SavedCharacterResource(SavableCharacterResource x, Node prefab, PlayerInformationData playerInformation) : base(x)
    {
        this.prefab = prefab;
        this.playerInformation = playerInformation;
    }
    
    public string CharacterTableBlessingString()
    {
        string blessingsInOneString = "";
        for(int i = 0; i < blessings.Count; i++)
        {
            blessingsInOneString += blessings[i].blessingName;
            if (i != blessings.Count - 1) blessingsInOneString += "\n";
        }
        return blessingsInOneString;
    }
    
}