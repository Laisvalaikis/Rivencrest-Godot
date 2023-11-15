using System;
using System.Threading.Tasks;
using Godot;
public partial class XPCard : Control
{
    [Export] private Label className;
    [Export] private TextureRect portrait;
    [Export] private Label xp;
    [Export] private Label levelText;
    [Export] private Label xpToGain;
    private int _tempXPToGain;
    private SavedCharacterResource _characterResource;
    private Data _data;

    public void UpdateXPCard(SavedCharacterResource savedCharacterResource)
    {
        if (Data.Instance != null)
        {
            _data = Data.Instance;
        }
        _tempXPToGain = savedCharacterResource.xPToGain;
        _characterResource = savedCharacterResource;
        portrait.Texture = (Texture2D)savedCharacterResource.playerInformation.CharacterPortraitSprite;
        className.Text = savedCharacterResource.playerInformation.ClassName;
        className.LabelSettings.FontColor = savedCharacterResource.playerInformation.textColor;
        xpToGain.Text = "+" + savedCharacterResource.xPToGain + " XP";
        levelText.Text = savedCharacterResource.level.ToString();
        int currentXP = _data.XPToLevelUp[_characterResource.level] -
                        (_data.XPToLevelUp[_characterResource.level] - _characterResource.xP);
        xp.Text = currentXP + "/" + _data.XPToLevelUp[_characterResource.level] + " XP";
        DelayMethod();
    }

    private async void DelayMethod()
    {
        while (_characterResource.xP < _characterResource.xP + _tempXPToGain)
        {
            if (_data.XPToLevelUp[_characterResource.level] > _characterResource.xP)
            {
                _characterResource.xP += 1;
                int currentXP = _data.XPToLevelUp[_characterResource.level] -
                                (_data.XPToLevelUp[_characterResource.level] - _characterResource.xP);
                xp.Text = currentXP + "/" + _data.XPToLevelUp[_characterResource.level] + " XP";
            }
            else
            {
                if (_characterResource.level < _data.XPToLevelUp.Count)
                {
                    _characterResource.level++;
                    levelText.Text = _characterResource.level.ToString();
                }
                else
                {
                    break;
                }
            }
            _characterResource.xPToGain = 0;
            await Task.Delay(TimeSpan.FromMilliseconds(500)); // wait for 500ms 
        }
    }
}