using Godot;

public partial class GameEnd : Control
{
    [Export] private Label text;
    [Export] private TextureRect textureRect;
    [Export] private Color death;
    [Export] private string victoryText;
    [Export] private string loseText;
    public void Death()
    {
        Show();
        text.Text = loseText;
        textureRect.SelfModulate = death;
        text.LabelSettings.FontColor = death;
    }

    public void Win(string team, Color teamColor)
    {
        Show();
        text.Text = team + victoryText;
        textureRect.SelfModulate = teamColor;
        text.LabelSettings.FontColor = teamColor;
    }
}