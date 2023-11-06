using Godot;
using System.Collections.Generic;

public partial class TurnManager : Node
{
	[Export] private TeamInformation _teamInformation;
	[Export] private Team _currentTeam;
	[Export] private int _currentTeamIndex = 0;
	[Export] private TeamsList _teamsList;
	[Export] private Player _currentPlayer;
	private Vector2 _mousePosition;

	public override void _Ready()
	{
		base._Ready();
		InputManager.Instance.MouseMove += OnMove;
		InputManager.Instance.LeftMouseClick += OnMouseClick;
	}

	public void SetTeamList(TeamsList teamsList)
	{
		_teamsList = teamsList;
	}

	public void SetCurrentTeam(int teamIndex)
	{
		_currentTeam = _teamsList.Teams[teamIndex];
		_currentTeamIndex = teamIndex;
		_currentPlayer = null;
	}

	public void SetCurrentCharacter(Player character)
	{
		if (_currentTeam.characters.Contains(character))
		{
			_currentPlayer = character;
		}
		else if (character != null)
		{
			_currentPlayer = null;
			GD.PrintErr("Character is not in team");
		}
		else
		{
			_currentPlayer = null;
		}
	}

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
		_teamInformation.EndTurn(_currentTeamIndex);
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
		if (_currentPlayer != null)
		{
			_currentPlayer.actionManager.OnMove(_mousePosition);
		}
	}
	
	public void OnMouseClick()
	{
		ResolveAbility();
	}

	private void ResolveAbility()
	{
		ChunkData chunk = GameTileMap.Tilemap.GetChunk(_mousePosition);
		if (_currentPlayer != null)
		{
			Ability currentAbility = _currentPlayer.actionManager.GetCurrentAbility();
			AddUsedAbility(new UsedAbility(currentAbility.Action, chunk));
			_currentPlayer.actionManager.OnMouseClick(chunk);
		}
	}
	
	public void AddUsedAbility(UsedAbility usedAbility)
	{
		_currentTeam.usedAbilities.Add(usedAbility);
	}

}
