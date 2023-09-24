using Godot;

public partial class SavedCharacterResource : SavableCharacterResource
{
    [Export]
    public Node prefab;

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
}