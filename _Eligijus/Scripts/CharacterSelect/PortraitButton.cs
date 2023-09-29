using System.Collections;
using System.Collections.Generic;
using Godot;

public partial class PortraitButton: Button
{
    [Export] 
    private CharacterSelectManager _characterSelectManager;
    [Export]
    public AnimationPlayer animator;
    [Export]
    public TextureRect characterImage;
    [Export]
    public PlayerInformationData playerInformation;
    [Export]
    public int buttonIndex;
    [Export]
    public int characterIndex;

    public override void _Pressed()
    {
        base._Pressed();
        _characterSelectManager.OnClickTeamCharacterButton(buttonIndex);
    }
}

