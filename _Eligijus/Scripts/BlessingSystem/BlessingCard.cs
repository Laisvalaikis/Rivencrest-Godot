using Godot;

public partial class BlessingCard : Control
{
    [Signal]
    public delegate void BlessingSelectedEventHandler();
    [Export] private Button claimButton;
    [Export] private TextureRect portrait;
    [Export] private TextureRect highlight;
    [Export] private TextureRect blessingIcon;
    [Export] private Label characterName;
    [Export] private Label blessingName;
    [Export] private Label blessingDescription;
    private BlessingData blessingData;
    private BlessingManager _blessingManager;

    public void UpdateInformation(BlessingData blessingData, BlessingManager blessingManager)
    {
        this.blessingData = blessingData;
        characterName.Text = blessingData.playerResource.characterName;
        portrait.Texture = (Texture2D)blessingData.playerResource.playerInformation.CharacterPortraitSprite;
        highlight.SelfModulate = blessingData.playerResource.playerInformation.classColor;
        blessingName.Text = blessingData.blessing.blessingName;
        blessingDescription.Text = blessingData.blessing.description;
        _blessingManager = blessingManager;
        claimButton.Pressed += PressButton;
    }

    public void PressButton()
    {
        if (blessingData.playerBlessing || blessingData.abilityBlessing)
        {
            blessingData.unlockedBlessingsResource.blessingUnlocked = true;
        }
        else if (blessingData.globalBlessing)
        {
            GlobalBlessing globalBlessing = (GlobalBlessing)blessingData.blessing;
            globalBlessing.Start(blessingData.playerResource);
        }
        _blessingManager.BlessingFinished();
        EmitSignal("BlessingSelected");
    }

}