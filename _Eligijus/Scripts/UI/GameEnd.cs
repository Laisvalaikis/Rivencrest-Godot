using Godot;

public partial class GameEnd : Control
{
    [Signal]
    public delegate void EndSignalEventHandler();
    [Export] private Label text;
    [Export] private TextureRect textureRect;
    [Export] private Color death;
    [Export] private string victoryText;
    [Export] private string loseText;
    private bool endOfGame = false;
    private string mouseClick = "MouseClickLeft";
    private Data _data;

    public override void _Ready()
    {
        base._Ready();
        if (Data.Instance != null)
        {
            _data = Data.Instance;
        }
    }

    public void Death(TeamsList deadCharacters)
    {
        Show();
        text.Text = loseText;
        textureRect.SelfModulate = death;
        text.LabelSettings.FontColor = death;
        DeadCharacters(deadCharacters);
        endOfGame = true;
    }

    public void Win(TeamsList deadCharacters, string team, Color teamColor)
    {
        Show();
        text.Text = team + victoryText;
        textureRect.SelfModulate = teamColor;
        text.LabelSettings.FontColor = teamColor;
        DeadCharacters(deadCharacters);
        endOfGame = true;
    }

    private void DeadCharacters(TeamsList deadCharacters)
    {
        for (int i = 0; i < deadCharacters.Teams.Count; i++)
        {
            if (!deadCharacters.Teams[i].isTeamAI && !deadCharacters.Teams[i].isTeamAI)
            {
                for (int j = 0; j < deadCharacters.Teams[i].characterResources.Count; j++)
                {
                    SavedCharacterResource deadCharacter = deadCharacters.Teams[i].characterResources[j];
                    _data.townData.deadCharacters.Add(deadCharacter);
                    if (_data.Characters.Contains(deadCharacter))
                    {
                        _data.Characters.Remove(deadCharacter);
                    }
                }
            }
        }
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (endOfGame)
        {
            if (@event.IsActionPressed(mouseClick))
            {
                EmitSignal("EndSignal");
            }
        }
    }
}