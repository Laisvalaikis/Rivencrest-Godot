using Godot;
using Godot.Collections;

public partial class XPManager : Node
{
    [Export] private Label _missionSuccessful;
    [Export] private Label _missionFailed;
    [Export] private Array<XPCard> xpCards;
    [Export] private int goldToAdd = 1400;
    [Export] private TwoClickButton button;
    [Export] private Label earnedGold;
    private Data _data;
    private int characterCount = 0;
    private int updatedCount = 0;
    public override void _Ready()
    {
        base._Ready();
        if (Data.Instance != null)
        {
            _data = Data.Instance;
            if (_data.townData.winGame)
            {
                _missionSuccessful.Show();
            }
            else
            {
                _missionFailed.Show();
            }
            UpdateData();
        }
    }

    public void UpdateData()
    {
    
        earnedGold.Text = "+" + goldToAdd + "g";
        _data.townData.townGold += goldToAdd;
        for (int i = 0; i < xpCards.Count; i++)
        {
            if (i < _data.Characters.Count)
            {
                xpCards[i].UpdateXPCard(_data.Characters[i], false, this);
                characterCount = i;
            }
            else if (_data.townData.deadCharacters != null && _data.townData.deadCharacters.Count > 0 && i >= _data.Characters.Count && i < _data.Characters.Count + _data.townData.deadCharacters.Count)
            {
                int index = i - _data.Characters.Count;
                xpCards[i].UpdateXPCard(_data.townData.deadCharacters[index], true, this);
            }
            else
            {
                xpCards[i].Hide();    
            }
        }
    }

    public void UpdatedXP()
    {
        if (updatedCount < characterCount)
        {
            updatedCount++;
            if (updatedCount >= characterCount)
            {
                button.SetButtonForSecondClick();
            }
        }
    }

    public void SkipShowingXP()
    {
        for (int i = 0; i < _data.Characters.Count; i++)
        {
            xpCards[i].UpdateXP();
        }
    }

}