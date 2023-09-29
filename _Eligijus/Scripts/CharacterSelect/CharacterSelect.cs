using System.Collections;
using System.Collections.Generic;
using Godot;

public partial class CharacterSelect : Button
{
    [Export] 
    private CharacterSelectManager _characterSelectManager;
    [Export]
    public int characterIndex = 0;
    [Export]
    public TextureRect portrait;
    [Export]
    public Label levelText;
    [Export]
    public Control abilityPointCorner;

    public override void _Pressed()
    {
        base._Pressed();
        _characterSelectManager.OnCharacterButtonClick(characterIndex);
    }
}
