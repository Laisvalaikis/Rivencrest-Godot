using Godot;

public partial class GameSettingsResource: Resource
{
    [Export]
    public bool attackHelper;
    [Export]
    public float masterVolume;
    [Export]
    public bool mute;

    public GameSettingsResource()
    {
        
    }

    public GameSettingsResource(GameSettingsResource data)
    {
        attackHelper = data.attackHelper;
        masterVolume = data.masterVolume;
        mute = data.mute;
    }

    public GameSettingsResource(GameSettings data)
    {
        attackHelper = data.attackHelper;
        masterVolume = data.masterVolume;
        mute = data.mute;
    }
}