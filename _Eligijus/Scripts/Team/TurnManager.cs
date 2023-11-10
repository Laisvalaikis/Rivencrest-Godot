using Godot;
using System.Collections.Generic;

public partial class TurnManager : Node
{
	[Export] private TeamInformation _teamInformation;
	[Export] private Team _currentTeam;
	[Export] private int _currentTeamIndex = 0;
	[Export] private TeamsList _teamsList;
	[Export] private Player _currentPlayer;
	private bool isAiTurn = false;
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
		if (_currentTeam.isTeamAI)
		{
			isAiTurn = true;
		}
		else
		{
			isAiTurn = false;
		}
		_teamInformation.EndTurn(_currentTeamIndex);
		OnTurnStart();
		
	}

	public bool IsCurrentTeamAI()
	{
		return isAiTurn;
	}

	public void OnTurnStart()
	{
		foreach (var usedAbility in _currentTeam.usedAbilitiesBeforeStartTurn)
		{
			if (usedAbility.Ability.GetCastCountBeforeStart() < usedAbility.Ability.GetLifetimeBeforeStart())
			{
				usedAbility.Ability.OnBeforeStart(usedAbility.Chunk);
				usedAbility.Ability.IncreaseCastCountBeforeTurn();
			}
			if(usedAbility.Ability.GetCastCountBeforeStart() >= usedAbility.Ability.GetLifetimeBeforeStart())
			{
				_currentTeam.usedAbilitiesBeforeStartTurn.Remove(usedAbility);
			}
		}
		
		foreach (Player character in _currentTeam.characters)
		{
			character.OnTurnStart();
		}
	}

	public void OnTurnEnd()
	{

		if (_currentTeam.usedAbilitiesAfterResolve.Count > 0)
		{
			// Fix LinkedLists
			for (LinkedListNode<UsedAbility> element = _currentTeam.usedAbilitiesAfterResolve.First; element != null && element.Next != null; element = element.Next)
			{
				UsedAbility usedAbility = element.Value;
				if (usedAbility.Ability.GetCastCountAfterResolve() < usedAbility.Ability.GetLifetimeAfterResolve())
				{
					usedAbility.Ability.OnAfterResolve(usedAbility.Chunk);
					usedAbility.Ability.IncreaseCastCountAfterResolve();
				}
				if(usedAbility.Ability.GetCastCountAfterResolve() >= usedAbility.Ability.GetLifetimeAfterResolve())
				{
					LinkedListNode<UsedAbility> removingElement = element;
					if (element.Next != null)
					{
						element = element.Next;
					}
					_currentTeam.usedAbilitiesAfterResolve.Remove(removingElement);
					
					// Galas
					if (element.Next == null)
					{
						usedAbility = element.Value;
						if (usedAbility.Ability.GetCastCountAfterResolve() < usedAbility.Ability.GetLifetimeAfterResolve())
						{
							usedAbility.Ability.OnAfterResolve(usedAbility.Chunk);
							usedAbility.Ability.IncreaseCastCountAfterResolve();
						}
						_currentTeam.usedAbilitiesAfterResolve.Remove(usedAbility);
					}
				}
			}
		}


		foreach (Player character in _currentTeam.characters)
		{
			character.OnAfterResolve();
			character.OnTurnEnd();
		}

		// foreach (var usedAbility in _currentTeam.usedAbilitiesEndTurn)
		// {
		// 	usedAbility.Ability.OnTurnEnd(usedAbility.Chunk);
		// 	_currentTeam.usedAbilitiesEndTurn.Remove(usedAbility);
		// }
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
			// Ability currentAbility = _currentPlayer.actionManager.GetCurrentAbility();
			// AddUsedAbility(new UsedAbility(currentAbility.Action, chunk));
			_currentPlayer.actionManager.OnMouseClick(chunk);
		}
	}
	
	
	public void AddUsedAbility(UsedAbility usedAbility)
	{
		_currentTeam.usedAbilitiesBeforeStartTurn.AddLast(usedAbility);
		_currentTeam.usedAbilitiesAfterResolve.AddLast(usedAbility);
		_currentTeam.usedAbilitiesEndTurn.AddLast(usedAbility);
	}

}
