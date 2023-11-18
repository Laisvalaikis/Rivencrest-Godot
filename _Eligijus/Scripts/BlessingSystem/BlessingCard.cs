using Godot;

public partial class BlessingCard : Control
{
    [Export] private Button claimButton;
    [Export] private TextureRect portrait;
    [Export] private TextureRect highlight;
    [Export] private TextureRect blessingIcon;
    [Export] private Label characterName;
    [Export] private Label blessingName;
    [Export] private Label blessingDescription;

    public void UpdateInformation(BlessingData blessingData)
    {
        characterName.Text = blessingData.playerResource.characterName;
        portrait.Texture = (Texture2D)blessingData.playerResource.playerInformation.CharacterPortraitSprite;
        highlight.SelfModulate = blessingData.playerResource.playerInformation.classColor;
        blessingName.Text = blessingData.blessing.blessingName;
        blessingDescription.Text = blessingData.blessing.description;
    }

}