using Godot;
using Godot.Collections;

public partial class CopyData : Node
{
    // [Export]
    // private Array<PlayerInformationData> playerInformationData;
    [Export]
    private Array<PlayerInformationDataNew> playerInformationDataNew;

    public override void _EnterTree()
    {
        base._EnterTree();
        for (int i = 0; i < playerInformationDataNew.Count; i++)
        {
            // GD.Print(playerInformationDataNew[i].ResourcePath.ToString());
            // playerInformationDataNew[i].CopyDataPlayerInformation(playerInformationData[i]);
            ResourceSaver.Save(playerInformationDataNew[i]);
        }
    }
}