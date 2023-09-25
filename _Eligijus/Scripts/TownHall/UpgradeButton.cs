using System;
using System.Collections;
using System.Collections.Generic;
using Godot;

public partial class UpgradeButton : Button
{
    [Export]
    public Texture DefaultSprite;
    [Export]
    public Texture UpgradedSprite;
    [Export]
    public UpgradeData upgradeData;
    [Export]
    public TextureRect frame;
    [Export]
    public Label text;
    [Export]
    public AnimationPlayer imageFadeController;
    [Export]
    public Control animationObject;
    [Export]
    public Button button;
    private bool enabled = true;
    private Data _data;

    public override void _Draw()
    {
        base._Ready();
        if (_data == null && Data.Instance != null)
        {
            _data = Data.Instance;
        }

        UpdateUpgradeButton();
    }

    private void Start()
    {
        animationObject.Show();
    }

    public void UpdateUpgradeButton()
    {
        if (enabled)
        {

            TownHallDataResource townHall = _data.townData.townHall;
            if (townHall.GetByType((TownHallUpgrade)upgradeData.upgradeIndex) + 1 <
                upgradeData.upgradeValue) //negalimi pirkti nes per auksti
            {
                button.Disabled = true;
            }
            else if (townHall.GetByType((TownHallUpgrade)upgradeData.upgradeIndex) + 1 >
                     upgradeData.upgradeValue) //nupirkti
            {
                button.Disabled = false;
                frame.Texture.Set("atlas", UpgradedSprite.Get("atlas"));
                text.LabelSettings.FontColor = Colors.White;
            }
            else
            {
                button.Disabled = false;
            } //galimas pirkti
        }
    }

    public void Pause(bool pause)
    {
        enabled = !pause;
    }
}
