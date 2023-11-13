using Godot;

public partial class GameEnd : Control
{
    [Export] private ChangeScene changeScene;
    [Export] private Label text;
    [Export] private TextureRect textureRect;
    [Export] private Color death;
    [Export] private string victoryText;
    [Export] private string loseText;
    private bool endOfGame = false;
    private string mouseClick = "MouseClickLeft";
    public void Death()
    {
        Show();
        text.Text = loseText;
        textureRect.SelfModulate = death;
        text.LabelSettings.FontColor = death;
        endOfGame = true;
    }

    public void Win(string team, Color teamColor)
    {
        Show();
        text.Text = team + victoryText;
        textureRect.SelfModulate = teamColor;
        text.LabelSettings.FontColor = teamColor;
        endOfGame = true;
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (endOfGame)
        {
            if (@event.IsActionPressed(mouseClick))
            {
                changeScene.SceneTransition();
            }
        }
    }
}