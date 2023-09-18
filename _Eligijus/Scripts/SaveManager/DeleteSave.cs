using Godot;

public partial class DeleteSave : Button
{
    [Export] 
    private SaveSlotCard _saveSlotCard;

    public override void _Pressed()
    {
        base._Pressed();
        _saveSlotCard.DeleteSlot();
    }
}