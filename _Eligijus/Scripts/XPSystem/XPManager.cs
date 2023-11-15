using Godot;
using Godot.Collections;

public partial class XPManager : Node
{
    [Export] private Array<XPCard> xpCards;
    [Export] private int goldToAdd = 1400;
    [Export] private Label earnedGold;
    private Data _data;

    public override void _Ready()
    {
        base._Ready();
        if (Data.Instance != null)
        {
            _data = Data.Instance;
            UpdateData();
        }
    }

    public void UpdateData()
    {
        int index = 0;
        earnedGold.Text = "+" + goldToAdd + "g";
        _data.townData.townGold += goldToAdd;
        while (index < xpCards.Count)
        {
            if (index < _data.Characters.Count)
            {
                xpCards[index].UpdateXPCard(_data.Characters[index]);
                index++;
            }
            else
            {
                break;
            }
        }
        for (int i = index; i < _data.Characters.Count; i++)
        {
            xpCards[i].Hide();
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