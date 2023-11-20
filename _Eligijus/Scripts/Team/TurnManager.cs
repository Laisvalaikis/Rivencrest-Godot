using Godot;
using Godot.Collections;

public partial class TurnManager : Node
{
	[Export] private TeamInformation _teamInformation;
	[Export] private Team _currentTeam;
	private Array<Object> _objects;
	public LinkedList<UsedAbility> objectAbilitiesBeforeStartTurn = new LinkedList<UsedAbility>();
	public LinkedList<UsedAbility> objectAbilitiesAfterResolve = new LinkedList<UsedAbility>();
	public LinkedList<UsedAbility> objectAbilitiesEndTurn = new LinkedList<UsedAbility>();
	[Export] private int _currentTeamIndex = 0;
	[Export] private TeamsList _teamsList;
	[Export] private Player _currentPlayer;
	[Export] private Player _currentEnemy;
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
		if (_currentTeam.characters.Values.Contains(character))
		{
			_currentPlayer = character;
		}
		else if (character != null)
		{
			_currentPlayer = null;
			_currentEnemy = character;
			GD.PrintErr("Character is not in team");
		}
		else
		{
			_currentPlayer = null;
			_currentEnemy = null;
		}
	}

	public void AddObject(Object currentObject)
	{
		if (_objects == null)
		{
			_objects = new Array<Object>();
		}
		currentObject.AddTurnManager(this);
		_objects.Add(currentObject);
	}

	public void SetCurrentEnemy(Player character)
	{
		if (!_currentTeam.characters.Values.Contains(character))
		{
			_currentPlayer = null;
			_currentEnemy = character;
			GD.PrintErr("Character is not in team");
		}
		else
		{
			_currentEnemy = null;
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
		ActionsBeforeStart(objectAbilitiesBeforeStartTurn);
		if (_objects != null && _objects.Count > 0)
		{
			for (int i = 0; i < _objects.Count; i++)
			{
				_objects[i].OnTurnStart();
			}
		}

		foreach (Player character in _currentTeam.characters.Values)
		{
			character.OnTurnStart();
		}
	}

	public void OnTurnEnd()
	{
		ActionsAfterResolve(_currentTeam.usedAbilitiesAfterResolve);
		ActionsAfterResolve(objectAbilitiesAfterResolve);
		if (_objects != null && _objects.Count > 0)
		{
			for (int i = 0; i < _objects.Count; i++)
			{
				_objects[i].OnTurnEnd();
			}
		}

		foreach (Player character in _currentTeam.characters.Values)
		{
			character.OnAfterResolve();
			character.OnTurnEnd();
		}
		ActionsEndTurn(objectAbilitiesEndTurn);
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
					usedAbility.ability.BlessingOnTurnEnd(usedAbility.chunk);
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

		if (_currentEnemy != null)
		{
			_currentEnemy.actionManager.OnMove(_mousePosition);
		}
	}
	
	public void OnMouseClick()
	{
		ResolveAbility();
	}

	public int GetCurrentTeamIndex()
	{
		return _currentTeamIndex;
	}

	private void ResolveAbility()
	{
		ChunkData chunk = GameTileMap.Tilemap.GetChunk(_mousePosition);
		if (_currentPlayer != null)
		{
			_currentPlayer.deBuffManager.OnMouseClick(chunk);
			_currentPlayer.actionManager.OnMouseClick(chunk);
		}
	}
	
	public void AddUsedAbilityBeforeStartTurn(UsedAbility usedAbility, int lifetime, bool objectAbility = false)
	{
		if (!objectAbility)
		{
			_currentTeam.usedAbilitiesBeforeStartTurn.AddLast(new UsedAbility(usedAbility, lifetime));
		}
		else
		{
			objectAbilitiesBeforeStartTurn.AddLast(new UsedAbility(usedAbility, lifetime));
		}
	}

	public void AddUsedAbilityAfterResolve(UsedAbility usedAbility, int lifetime, bool objectAbility = false)
	{
		if (!objectAbility)
		{
			_currentTeam.usedAbilitiesAfterResolve.AddLast(new UsedAbility(usedAbility, lifetime));
		}
		else
		{
			objectAbilitiesAfterResolve.AddLast(new UsedAbility(usedAbility, lifetime));
		}
	}
	
	public void AddUsedAbilityOnTurnEnd(UsedAbility usedAbility, int lifetime, bool objectAbility = false)
	{
		if (!objectAbility)
		{
			LinkedListNode<UsedAbility> findElement =
				_currentTeam.usedAbilitiesEndTurn.Find(new UsedAbility(usedAbility, lifetime));
			if (findElement == null)
			{
				_currentTeam.usedAbilitiesEndTurn.AddLast(new UsedAbility(usedAbility, lifetime));
			}
			else
			{
				_currentTeam.usedAbilitiesEndTurn.MoveToEnd(findElement);
			}
		}
		else
		{
			LinkedListNode<UsedAbility> findElement = objectAbilitiesEndTurn.Find(new UsedAbility(usedAbility, lifetime));
			if (findElement == null)
			{
				objectAbilitiesEndTurn.AddLast(new UsedAbility(usedAbility, lifetime));
			}
			else
			{
				objectAbilitiesEndTurn.MoveToEnd(findElement);
			}
		}
	}

}
