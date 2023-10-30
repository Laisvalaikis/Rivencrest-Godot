using Godot;

public class TurnManager : Node
{
    [Export] private Team _currentTeam;
    [Export] private int _currentTeamIndex = 0;
    [Export] private TeamsList _teamsList;
    [Export] private Player _currentPlayer;
    private Vector2 _mousePosition;
    public void StartTurn()
    {
        OnTurnStart();
    }

    public void EndTurn()
    {
        OnTurnEnd();
        if (_currentTeamIndex + 1 < _teamsList.Teams.Count)
        {
            _currentTeamIndex++;
        }
        else
        {
            _currentTeamIndex = 0;
        }
        _currentTeam = _teamsList.Teams[_currentTeamIndex];
        OnTurnStart();
        
    }

    public void OnTurnStart()
    {
        foreach (Player character in _currentTeam.characters)
        {
            character.OnTurnStart();
        }
    }

    public void OnTurnEnd()
    {
        foreach (Player character in _currentTeam.characters)
        {
            character.OnTurnEnd();
        }
    }
    
    public void OnMove(Vector2 position)
    {
        _mousePosition = position;
        // ChunkData hoveredChunk = GameTileMap.Tilemap.GetChunk(_mousePosition);
        // _currentAbility.Action.OnMoveArrows(hoveredChunk,_previousChunk);
        // _currentAbility.Action.OnMoveHover(hoveredChunk,_previousChunk);
        // _previousChunk = hoveredChunk;
    }

    public void ResolveAbility()
    {
        ChunkData chunk = GameTileMap.Tilemap.GetChunk(_mousePosition);

        // foreach (Player character in _currentTeam.characters)
        // {
        //     // character.actionManager.Ex;
        // }
    }

}