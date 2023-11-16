using Godot;

public partial class BlessingData : Resource
{
   [Export] public SavedCharacterResource playerResource;
   [Export] public int blessingIndexInCharacter;
   [Export] public BaseBlessing blessing;
}