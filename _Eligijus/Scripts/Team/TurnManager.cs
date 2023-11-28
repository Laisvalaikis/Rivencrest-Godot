using System.Collections.Generic;
using Godot;
using Godot.Collections;
using System.Threading.Tasks;
public partial class TurnManager : Node
{
	[Export] private CameraMovement _cameraMovement;
	[Export] private FogOfWar _fogOfWar;
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
		_teamsList.Teams.Add(index, addTeam);
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
		foreach (int key in _teamsList.Teams.Keys)
		{
			ResetFogInformation(_teamsList.Teams[key].GetVisionTiles()).Wait();
		}
		foreach (int key in _currentTeam.characters.Keys)
		{
			Player player = _currentTeam.characters[key];
			_cameraMovement.FocusPoint(player.GlobalPosition);
			break;
		}
		UpdateFogInformation(_currentTeam.GetVisionTiles());
		_fogOfWar.SetFogImage(_currentTeam.fogImage);
	}

	public void SetCurrentCharacter(Player character)
	{
		if (_currentTeam.characters.Values.Contains(character))
		{
			_currentPlayer = character;
			_cameraMovement.FocusPoint(_currentPlayer.GlobalPosition);
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

	public void RemoveObject(Object currentObject)
	{
		if (_objects == null)
		{
			_objects = new Array<Object>();
		}
		else
		{
			_objects.Remove(currentObject);
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
		ResetFogInformation(_currentTeam.GetVisionTiles()).Wait();
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
		
		UpdateFogInformation(_currentTeam.GetVisionTiles()).Wait();
		_teamInformation.EndTurn(_currentTeamIndex);
		OnTurnStart();
		if (!isAiTurn)                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     
		{
			_fogOfWar.SetFogImage(_currentTeam.fogImage);
		}

		
	}

	private async Task ResetFogInformation(List<FogData> fogDataList)
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
	
	private async Task UpdateFogInformation(List<FogData> fogDataList)
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
