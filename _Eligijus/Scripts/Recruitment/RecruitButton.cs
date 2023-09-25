using System;
using System.Collections;
using System.Collections.Generic;
using Godot;

public partial class RecruitButton : Button
{
    [Export]
    public TextureRect portrait;
    [Export]
    public Button buyButton;
    [Export]
    public Label className;
    [Export]
    public Label cost;
    [Export]
    public SavedCharacterResource character;
    private int XPToLevelUp;
    private Data _data;

    public override void _Draw()
    {
        base._Draw();
        if(_data == null && Data.Instance != null)
        {
            _data = Data.Instance;
        }
    }

    public void UpdateRecruitButton()
    {
            var charInformation = character.playerInformation;
            className.Text = charInformation.ClassName;
            className.LabelSettings.FontColor = charInformation.classColor;
            portrait.Texture.Set("AtlasTexture", charInformation.CharacterPortraitSprite); //= charInformation.CharacterPortraitSprite;
            cost.Text = character.cost.ToString() + "g";
            if (_data.townData.townGold >= character.cost && _data.Characters.Count < _data.maxCharacterCount)
            {
                buyButton.Disabled = false;
            }
            else
            {
                buyButton.Disabled = true;
            }
    }
    

}
