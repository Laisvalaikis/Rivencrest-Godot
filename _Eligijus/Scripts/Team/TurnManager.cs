using System.Collections.Generic;
using System.Linq;
using Godot;
using System.Threading.Tasks;
using Godot.Collections;

public partial class TurnManager : Node
{
	[Export] private CameraMovement _cameraMovement;
	[Export] private FogOfWar _fogOfWar;
	[Export] private TeamInformation _teamInformation;
	[Export] private Team _currentTeam;
	// private Array<Object> _objects;
	private System.Collections.Generic.Dictionary<int, TeamObject> _teamObjects;
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
		InputManager.Instance.MoveSelector += OnMove;
		InputManager.Instance.SelectClick += OnMouseClick;
	}

	public void AddTeamToList(int index, Team addTeam)
	{
		if (_teamsList is null)
		{
			_teamsList = new TeamsList();
		}
		if (_teamsList.Teams is null)
		{
			_teamsList.Teams = new Godot.Collections.Dictionary<int, Team>();
		}
		if (_teamObjects is null)
		{
			_teamObjects = new System.Collections.Generic.Dictionary<int, TeamObject>();
		}
		_teamsList.Teams.Add(index, addTeam);
		_teamObjects.Add(index, new TeamObject());
	}
	
	public Team GetTeamByIndex(int teamIndex)
	{
		return _teamsList.Teams[teamIndex];
	}

	public Team GetCurrentTeam()
	{
		return _currentTeam;
	}

	public void SetCurrentTeam(int teamIndex)
	{
		_currentTeam = _teamsList.Teams[teamIndex];
		_currentTeamIndex = teamIndex;
		_currentPlayer = null;
		InputManager.Instance.SetCurrentCharacterPosition(_currentTeam.characters.Values.First().GlobalPosition);
		foreach (int key in _teamsList.Teams.Keys)
		{
			ResetFogInformation(_teamsList.Teams[key].GetVisionTiles());
		}
		foreach (int key in _currentTeam.characters.Keys)
		{
			Player player = _currentTeam.characters[key];
			_cameraMovement.FocusPoint(player.GlobalPosition);
			break;
		}
		UpdateFogInformation(_currentTeam.GetVisionTiles());
		_fogOfWar.SetFogTexture(_currentTeam.fogTexture);
	}

	public void SetCurrentCharacter(Player character)
	{
		if (_currentTeam.characters.Values.Contains(character))
		{
			_currentPlayer = character;
			_cameraMovement.FocusPoint(_currentPlayer.GlobalPosition);
			InputManager.Instance.SetCurrentCharacterPosition(_currentPlayer.GlobalPosition);
		}
		else if (character != null)
		{
			_currentPlayer = null;
			_currentEnemy = character;
			InputManager.Instance.SetCurrentCharacterPosition(_currentEnemy.GlobalPosition);
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
		if (_teamObjects == null)
		{
			_teamObjects = new System.Collections.Generic.Dictionary<int, TeamObject>();
		}
		currentObject.AddTurnManager(this);
		_teamObjects[_currentTeamIndex].AddObject(currentObject);
	}

	public void RemoveObject(Object currentObject)
	{
		if (_teamObjects is null)
		{
			_teamObjects = new System.Collections.Generic.Dictionary<int, TeamObject>();
		}
		else
		{
			for (int i = 0; i < _teamsList.Teams.Count; i++)
			{
				LinkedList<Object> objects = _teamObjects[i].GetObjects();
				if (objects.Contains(currentObject))
				{
					_teamObjects[i]
						.RemoveObject(
							currentObject);
				}
			}

			Array<Ability> abilities = currentObject.GetAllAbilities();
			for (int i = 0; i < abilities.Count; i++)
			{
				UsedAbility usedAbility = new UsedAbility(abilities[i].Action);
				LinkedListNode<UsedAbility> beforeStart = objectAbilitiesBeforeStartTurn.Find(usedAbility);
				objectAbilitiesBeforeStartTurn.Remove(beforeStart);
				LinkedListNode<UsedAbility> afterResolve = objectAbilitiesAfterResolve.Find(usedAbility);
				objectAbilitiesAfterResolve.Remove(afterResolve);
				LinkedListNode<UsedAbility> endTurn = objectAbilitiesEndTurn.Find(usedAbility);
				objectAbilitiesEndTurn.Remove(endTurn);
			}
		}
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
		ResetFogInformation(_currentTeam.GetVisionTiles());
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
		
		foreach (int key in _teamsList.Teams[_currentTeamIndex].characters.Keys)
		{
			Player player = _teamsList.Teams[_currentTeamIndex].characters[key];
			_cameraMovement.FocusPoint(player.GlobalPosition);
			break;
		}
		
		UpdateFogInformation(_currentTeam.GetVisionTiles());
		_teamInformation.EndTurn(_currentTeamIndex);
		OnTurnStart();
		if (!isAiTurn)                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     
		{
			_fogOfWar.SetFogTexture(_currentTeam.fogTexture);
		}

		
	}

	private void ResetFogInformation(List<FogData> fogDataList)
	{
		foreach (FogData fogData in fogDataList)
		{
			fogData.chunkRef.SetFogOnTile(true);
			if (fogData.chunkRef.CharacterIsOnTile())
			{
				fogData.chunkRef.GetCurrentPlayer().DisableObject();
				HighlightTile highlightTile = fogData.chunkRef.GetTileHighlight();
				highlightTile.ActivatePlayerTile(false);
				highlightTile.EnableTile(false);
			}
		}
	}
	
	private void UpdateFogInformation(List<FogData> fogDataList)
	{
		foreach (FogData fogData in fogDataList)
		{
			fogData.chunkRef.SetFogOnTile(fogData.fogIsOnTile);
			if (fogData.chunkRef.CharacterIsOnTile() && !fogData.fogIsOnTile)
			{
				fogData.chunkRef.GetCurrentPlayer().EnableObject();
				HighlightTile highlightTile = fogData.chunkRef.GetTileHighlight();
				highlightTile.ActivatePlayerTile(true);
				highlightTile.EnableTile(true);
			}
		}
	}

	public bool IsCurrentTeamAI()
	{
		return isAiTurn;
	}

	public void OnTurnStart()
	{
		ActionsBeforeStart(_currentTeam.usedAbilitiesBeforeStartTurn);
		ActionsBeforeStart(objectAbilitiesBeforeStartTurn);

		ObjectActionsOnTurnStart(_teamObjects[_currentTeamIndex].GetObjects());
		

		foreach (Player character in _currentTeam.characters.Values)
		{
			character.OnTurnStart();
		}
	}
	public void ObjectActionsOnTurnStart(LinkedList<Object> objects)
	{
		LinkedListNode<Object> element = objects.First;
		while (element != null)
		{
			LinkedListNode<Object> nextElement = element.Next;
			element.Value.OnTurnStart();
			element = nextElement;
		}
	}
	public void OnTurnEnd()
	{
		ActionsAfterResolve(_currentTeam.usedAbilitiesAfterResolve);
		ActionsAfterResolve(objectAbilitiesAfterResolve);
		
		ObjectActionsOnTurnEnd(_teamObjects[_currentTeamIndex].GetObjects());
		
		
		foreach (Player character in _currentTeam.characters.Values)
		{
			character.OnAfterResolve();
			character.OnTurnEnd();
		}
		ActionsEndTurn(objectAbilitiesEndTurn);
		ActionsEndTurn(_currentTeam.usedAbilitiesEndTurn); // sita funkcija infinity sukasi?
	}
	
	public void ObjectActionsOnTurnEnd(LinkedList<Object> objects)
	{
		LinkedListNode<Object> element = objects.First;
		while (element != null)
		{
			LinkedListNode<Object> nextElement = element.Next;
			element.Value.OnTurnEnd();
			element = nextElement;
		}
	}

	private void ActionsAfterResolve(LinkedList<UsedAbility> abilities)
	{
		if (abilities.Count > 0)
		{
			for (LinkedListNode<UsedAbility> element = abilities.First; element != null; )
			{
				bool elementWasRemoved = false;
				UsedAbility usedAbility = element.Value;
				
				if (usedAbility.GetCastCount() < usedAbility.GetTurnLifetime())
				{
					usedAbility.ability.OnAfterResolve(usedAbility.chunk);
					usedAbility.IncreaseCastCount();
				}
				// remove yra problema vsio aisku
				if(usedAbility.GetCastCount() >= usedAbility.GetTurnLifetime())
				{
					LinkedListNode<UsedAbility> tempElement = element;
					tempElement = element.Next;
					abilities.Remove(element);
					element = tempElement;
					elementWasRemoved = true;
				}
				if (!elementWasRemoved)
				{
					element = element.Next;
				}
			}
		}
	}
	
	private void ActionsBeforeStart(LinkedList<UsedAbility> abilities)
	{
		if (abilities.Count > 0)
		{	
			for (LinkedListNode<UsedAbility> element = abilities.First; element != null; )
			{
				bool elementWasRemoved = false;
				UsedAbility usedAbility = element.Value;
				
				if (usedAbility.GetCastCount() < usedAbility.GetTurnLifetime())
				{
					usedAbility.ability.OnBeforeStart(usedAbility.chunk);
					usedAbility.IncreaseCastCount();
				}
				if(usedAbility.GetCastCount() >= usedAbility.GetTurnLifetime())
				{
					LinkedListNode<UsedAbility> tempElement = element;
					tempElement = element.Next;
					abilities.Remove(element);
					element = tempElement;
					elementWasRemoved = true;
				}
				if (!elementWasRemoved)
				{
					element = element.Next;
				}
			}
		}
	}
	
	private void ActionsEndTurn(LinkedList<UsedAbility> abilities)
	{
		if (abilities.Count > 0)
		{
			for (LinkedListNode<UsedAbility> element = abilities.First; element != null; )
			{
				bool elementWasRemoved = false;
				UsedAbility usedAbility = element.Value;
				
				if (usedAbility.GetCastCount() < usedAbility.GetTurnLifetime()) 
				{
					usedAbility.ability.OnTurnEnd(usedAbility.chunk);
					usedAbility.ability.BlessingOnTurnEnd(usedAbility.chunk);
					usedAbility.IncreaseCastCount();
				}
				if(usedAbility.GetCastCount() >= usedAbility.GetTurnLifetime())
				{
					LinkedListNode<UsedAbility> tempElement = element;
					tempElement = element.Next;
					abilities.Remove(element);
					element = tempElement;
					elementWasRemoved = true;
				}
				if (!elementWasRemoved)
				{
					element = element.Next;
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
	
	public void OnMouseClick(Vector2 mouseClick)
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
