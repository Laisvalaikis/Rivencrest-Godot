using System;
using System.Threading.Tasks;
using Godot;
public partial class XPCard : Control
{
    [Export] private Color deadCharacterColor = Colors.Gray;
    [Export] private Label className;
    [Export] private TextureRect portrait;
    [Export] private Label xp;
    [Export] private Label levelText;
    [Export] private Label xpToGain;
    [Export] private int updateTextInMs = 10;
    private int _tempXPToGain;
    private int _tempCurrentXp;
    private SavableCharacterResource _characterResource;
    private Data _data;
    private bool updatingXp = false;
    private XPManager _xPManager;

    public void UpdateXPCard(SavableCharacterResource savedCharacterResource, bool deadCharacter, XPManager xPManager)
    {
        if (Data.Instance != null)
        {
            _data = Data.Instance;
        }
        _xPManager = xPManager;
        if (!deadCharacter)
        {
            AliveCharacter(savedCharacterResource);
        }
        else
        {
            DeadCharacter(savedCharacterResource);
        }
    }

    public void AliveCharacter(SavableCharacterResource savedCharacterResource)
    {
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
        UpdateXPGradualy(_xPManager);
    }
    
    public void DeadCharacter(SavableCharacterResource savedCharacterResource)
    {
        _characterResource = savedCharacterResource;
        portrait.Texture = (Texture2D)savedCharacterResource.playerInformation.CharacterPortraitSprite;
        className.LabelSettings.FontColor = deadCharacterColor;
        className.Text = savedCharacterResource.playerInformation.ClassName;
        xpToGain.LabelSettings.FontColor = deadCharacterColor;
        xpToGain.Text = "DEAD";
        levelText.Text = savedCharacterResource.level.ToString();
        xp.LabelSettings.FontColor = deadCharacterColor;
        xp.Text = 0 + "/" + _data.XPToLevelUp[_characterResource.level] + " XP";
    }

    private async Task UpdateXPGradualy(XPManager xpManager)
    {
        _tempCurrentXp = _characterResource.xP;
        int tempXp = _tempCurrentXp + _tempXPToGain;
        updatingXp = true;
        while (_characterResource.xP < tempXp && updatingXp)
        {
            if (_data.XPToLevelUp[_characterResource.level] > _characterResource.xP)
            {
                _characterResource.xP += 1;
                int currentXP = _data.XPToLevelUp[_characterResource.level] -
                                (_data.XPToLevelUp[_characterResource.level] - _characterResource.xP);
                xp.Text = currentXP + "/" + _data.XPToLevelUp[_characterResource.level] + " XP";
                if (_data.XPToLevelUp[_characterResource.level] <= _characterResource.xP)
                {
                    if (_characterResource.level < _data.XPToLevelUp.Count)
                    {
                        _characterResource.xP -= _data.XPToLevelUp[_characterResource.level];
                        tempXp -= _data.XPToLevelUp[_characterResource.level];
                        _characterResource.level++;
                        _characterResource.abilityPointCount++;
                        levelText.Text = _characterResource.level.ToString();
                        if (_characterResource.level >= _data.XPToLevelUp.Count)
                        {
                            _characterResource.xP = tempXp;
                            xp.Text = "Max Level";
                            break;
                        }
                        else
                        {
                            
                            xp.Text = "0/" + _data.XPToLevelUp[_characterResource.level] + " XP";
                        }
                    }
                    else
                    {
                        levelText.Text = _characterResource.level.ToString();
                        xp.Text = "Max Level";
                        _characterResource.xP = tempXp;
                        break;
                    }
                }
            }

            await Task.Delay(TimeSpan.FromMilliseconds(updateTextInMs)); // wait for 500ms 
        }

        if (_characterResource.xP >= tempXp)
        {
            _characterResource.xPToGain = 0;
            _tempXPToGain = 0;
            updatingXp = false;
            xpManager.UpdatedXP();
        }
    }

    public void UpdateXP()
    {
        if (updatingXp)
        {
            updatingXp = false;
        }

        _characterResource.xP = _tempCurrentXp;
        int tempXp = _characterResource.xP + _tempXPToGain;
        while (_characterResource.xP < tempXp)
        {
            int leftTillNextLevel = _data.XPToLevelUp[_characterResource.level] - _characterResource.xP;
            if (leftTillNextLevel <= tempXp)
            {
                if (_characterResource.level < _data.XPToLevelUp.Count)
                {
                    _characterResource.xP = 0;
                    _characterResource.level++;
                    _characterResource.abilityPointCount++;
                    levelText.Text = _characterResource.level.ToString();
                    if (_characterResource.level < _data.XPToLevelUp.Count)
                    {
                        xp.Text = "0/" + _data.XPToLevelUp[_characterResource.level] + " XP"; 
                    }
                    else
                    {
                        levelText.Text = _characterResource.level.ToString();
                        xp.Text = "Max Level";
                        break;
                    }
                    tempXp -= leftTillNextLevel;
                }
                else
                {
                    levelText.Text = _characterResource.level.ToString();
                    xp.Text = "Max Level";
                    break;
                }
            }
            else
            {
                _characterResource.xP = tempXp;
                xp.Text = tempXp + "/" + _data.XPToLevelUp[_characterResource.level] + " XP";
            }
        }
        _characterResource.xPToGain = 0;
        _tempXPToGain = 0;
    }
}