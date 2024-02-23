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

        InputManager.Instance.UnHandledSelectClick += EndSignalInvoke;
    }

    public void Death(TeamsList deadCharacters, TeamsList currentCharacters)
    {
        Show();
        text.Text = loseText;
        textureRect.SelfModulate = death;
        text.LabelSettings.FontColor = death;
        DeadCharacters(deadCharacters);
        endOfGame = true;
    }

    public void Win(TeamsList deadCharacters, TeamsList currentCharacters, string team, Color teamColor)
    {
        Show();
        text.Text = team + victoryText;
        textureRect.SelfModulate = teamColor;
        text.LabelSettings.FontColor = teamColor;
        DeadCharacters(deadCharacters); 
        SaveCharacterData(currentCharacters);
        endOfGame = true;
    }

    public void Forfeit()
    {
         Show();
         text.Text = "Coward";
         textureRect.SelfModulate = death;
         text.LabelSettings.FontColor = death;
         endOfGame = true;
    }
    
    public void DeadCharacters(TeamsList deadCharacters)
    {
        foreach (int i in deadCharacters.Teams.Keys) 
        {
            if (!deadCharacters.Teams[i].isTeamAI && !deadCharacters.Teams[i].isEnemies)
            {
                foreach (int j in deadCharacters.Teams[i].characterResources.Keys)
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

    private void SaveCharacterData(TeamsList currentCharacters)
    {
        foreach (int i in currentCharacters.Teams.Keys) 
        {
            if (!currentCharacters.Teams[i].isTeamAI && !currentCharacters.Teams[i].isTeamAI)
            {
                foreach (int j in currentCharacters.Teams[i].characterResources.Keys)
                {
                    
                    Player player = currentCharacters.Teams[i].characters[j];
                    SavedCharacterResource character = currentCharacters.Teams[i].characterResources[j];
                    character.xPToGain = player.objectInformation.GetPlayerInformation().GainXP();
                }
            }
        }
    }
    
    
    private void EndSignalInvoke(Vector2 vector2)
    {
        if (endOfGame)
        {
            InputManager.Instance.UnHandledSelectClick -= EndSignalInvoke;
            EmitSignal("EndSignal");
        }
    }
    
}