using Godot;

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
		ActionsBeforeStart(_currentTeam.usedAbilitiesBeforeStartTurn);
		
		foreach (Player character in _currentTeam.characters)
		{
			character.OnTurnStart();
		}
	}

	public void OnTurnEnd()
	{
		ActionsAfterResolve(_currentTeam.usedAbilitiesAfterResolve);

		foreach (Player character in _currentTeam.characters)
		{
			character.OnAfterResolve();
			character.OnTurnEnd();
		}
		ActionsEndTurn(_currentTeam.usedAbilitiesEndTurn);
	}

	private void ActionsAfterResolve(LinkedList<UsedAbility> abilities)
	{
		if (abilities.Count > 0)
		{
			for (LinkedListNode<UsedAbility> element = abilities.First; element != null; element = element.Next)
			{
				UsedAbility usedAbility = element.Value;
				if (usedAbility.GetCastCount() < usedAbility.GetTurnLifetime())
				{
					usedAbility.ability.OnAfterResolve(usedAbility.chunk);
					usedAbility.IncreaseCastCount();
				}
				if(usedAbility.GetCastCount() >= usedAbility.GetTurnLifetime())
				{
					abilities.Remove(element);
				}
			}
		}
	}
	
	private void ActionsBeforeStart(LinkedList<UsedAbility> abilities)
	{
		if (abilities.Count > 0)
		{
			for (LinkedListNode<UsedAbility> element = abilities.First; element != null; element = element.Next)
			{
				UsedAbility usedAbility = element.Value;
				if (usedAbility.GetCastCount() < usedAbility.GetTurnLifetime())
				{
					usedAbility.ability.OnBeforeStart(usedAbility.chunk);
					usedAbility.IncreaseCastCount();
				}
				if(usedAbility.GetCastCount() >= usedAbility.GetTurnLifetime())
				{
					abilities.Remove(element);
				}
			}
		}
	}
	
	private void ActionsEndTurn(LinkedList<UsedAbility> abilities)
	{
		if (abilities.Count > 0)
		{
			for (LinkedListNode<UsedAbility> element = abilities.First; element != null; element = element.Next)
			{
				UsedAbility usedAbility = element.Value;
				if (usedAbility.GetCastCount() < usedAbility.GetTurnLifetime())
				{
					usedAbility.ability.OnTurnEnd(usedAbility.chunk);
					usedAbility.IncreaseCastCount();
				}
				if(usedAbility.GetCastCount() >= usedAbility.GetTurnLifetime())
				{
					abilities.Remove(element);
				}
			}
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
			_currentPlayer.actionManager.OnMouseClick(chunk);
			
		}
	}
	
	public void AddUsedAbilityBeforeStartTurn(UsedAbility usedAbility, int lifetime)
	{
		_currentTeam.usedAbilitiesBeforeStartTurn.AddLast(new UsedAbility(usedAbility, lifetime));
	}

	public void AddUsedAbilityAfterResolve(UsedAbility usedAbility, int lifetime)
	{
		_currentTeam.usedAbilitiesAfterResolve.AddLast(new UsedAbility(usedAbility, lifetime));
	}
	
	public void AddUsedAbilityOnTurnEnd(UsedAbility usedAbility, int lifetime)
	{
		_currentTeam.usedAbilitiesAfterResolve.AddLast(new UsedAbility(usedAbility, lifetime));
	}

}
