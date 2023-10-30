using Godot;

public class TurnManager : Node
{
    [Export] private Team currentTeam;
    [Export] private int currentTeamIndex = 0;
    [Export] private Player currentPlayer;
    
    public void OnTurnStart()
    {
        foreach (Player character in currentTeam.characters)
        {
            character.OnTurnStart();
        }
    }

    public void OnTurnEnd()
    {
        foreach (Player character in currentTeam.characters)
        {
            character.OnTurnEnd();
        }
    }

    public void ResolveAbility()
    {
        foreach (Player character in currentTeam.characters)
        {
            // character.actionManager.Ex;
        }
    }

}