using Godot;
using Godot.Collections;

public partial class ActionManager : Node
{
	[Export] 
	private Player _player;
	private Array<Ability> _baseAbilities;
	private Array<Ability> _abilities;
	[Export]
	private Array<Ability> _allAbilities;
	[Export]
	private int _availableAbilityPoints = 1;
	private int _abilityPoints;
	private bool _hasSlowAbilityBeenCast;
	private int _trackingCount;
	
	// for blessings
	private int _pointsBeforeAbility;
	
	private Vector2 _mousePosition;
	private Ability _currentAbility;
	private SelectActionButton _currentAbilitySelectActionButton;
	private ChunkData _previousChunk;
	private bool _abilityIsSelected;
	private Array<UnlockedAbilitiesResource> _unlockedAbilityList;
	private Data _data;
	private TurnManager _turnManager;
	
	public void InitializeActionManager()
	{
		if (Data.Instance != null)
		{
			_data = Data.Instance;
		}
		RefillActionPoints();
		_abilities = new Array<Ability>();
		_baseAbilities = new Array<Ability>();
		Array<Ability> allAbilities = new Array<Ability>();
		Array<Ability> baseAbilities = _player.objectInformation.GetPlayerInformation().objectData.GetPlayerInformationData().baseAbilities;
		_unlockedAbilityList = _player.unlockedAbilityList;
		allAbilities.AddRange(baseAbilities);
		allAbilities.AddRange(_player.objectInformation.GetPlayerInformation().objectData.GetPlayerInformationData().abilities);
		if (allAbilities.Count != 0)
		{
			for (int i = 0; i < allAbilities.Count; i++)
			{
				Ability ability = new Ability(allAbilities[i]);
				ability.Action.SetupPlayerAbility(_player, i);
				if (_allAbilities == null)
				{
					_allAbilities = new Array<Ability>();
					_allAbilities.Add(ability);
				}
				else
				{
					_allAbilities.Add(ability);
				}
				ability.Action.AddTurnManager(_turnManager);
				ability.Action.OnBeforeStart(null);
				ability.Action.OnTurnStart(null);
				ability.Action.BlessingOnTurnStart(null);
			}
			
			for (int i = 0; i < baseAbilities.Count; i++)
			{
				_allAbilities[i]._type = AbilityType.BaseAbility;
				_baseAbilities.Add(_allAbilities[i]);
			}

			for (int i = _baseAbilities.Count; i < allAbilities.Count; i++)
			{
				_allAbilities[i]._type = AbilityType.Ability;
				_abilities.Add(_allAbilities[i]);
			}
		}
	}

	public void AddTurnManager(TurnManager turnManager)
	{
		_turnManager = turnManager;
	}

	public int GetAllAbilityPoints()
	{
		return _availableAbilityPoints;
	}

	public int GetAbilityPoints()
	{
		return _abilityPoints;
	}

	public Array<Ability> ReturnAllAbilities()
	{
		return _allAbilities;
	}
	
	public Array<Ability> ReturnAbilities()
	{
		return _abilities;
	}
	
	public Array<Ability> ReturnBaseAbilities()
	{
		return _baseAbilities;
	}

	public Ability ReturnPlayerMovement()
	{
		for (int i = 0; i < _baseAbilities.Count; i++)
		{
			if (_baseAbilities[i].Equals(typeof(PlayerMovement)))
			{
				return _baseAbilities[i];
			}
		}

		return null;
	}

	public void RemoveAllActionPoints() 
	{
		_hasSlowAbilityBeenCast = true;
		_abilityPoints = 0;
	}
	
	public void SetAbilityPoints(int abilityPoints)
	{
		_availableAbilityPoints = abilityPoints;
	}
	
	public void AddAbilityCooldownPoints(int abilityPoints)
	{
		_availableAbilityPoints += abilityPoints;
	}
	
	// public void RemoveSlowDown()
	// {
	// 	_debuffs.RemoveSlowDown(this);
	// }

	public void AddAbilityPoints(int abilityPoints)
	{
		// if (abilityPoints < 0)
		// {
		// 	_debuffs.SlowDownPlayer(abilityPoints);
		// }

		if (_abilityPoints + abilityPoints <= _availableAbilityPoints)
		{
			_abilityPoints += abilityPoints;
		}
	}

	public void ResetAbilityPointsBeforeAbility()
	{
		_abilityPoints = _pointsBeforeAbility;
	}

	private void RemoveAbilityPoints(int consumedPoints)
	{
		_pointsBeforeAbility = _abilityPoints;
		if (_abilityPoints >= consumedPoints)
		{
			_abilityPoints -= consumedPoints;
		}
		else
		{
			_abilityPoints = 0;
		}

		
	}

	public virtual void RefillActionPoints()
	{
		_abilityPoints = _availableAbilityPoints;
	}

	public void OnBeforeStart()
	{
		
	}

	public void OnTurnStart()
	{
		RefillActionPoints();
		ActionOnTurnStart();
	}
	
	private void ActionOnTurnStart()
	{
		for (int i = 0; i < _baseAbilities.Count; i++)
		{
			if (_baseAbilities[i].enabled)
			{
				_baseAbilities[i].Action.OnTurnStart(null);
				_baseAbilities[i].Action.BlessingOnTurnStart(null);
			}
		}
		for (int i = 0; i < _abilities.Count; i++)
		{
			if (_abilities[i].enabled && i < _unlockedAbilityList.Count && _unlockedAbilityList[i].abilityConfirmed)
			{
				_abilities[i].Action.OnTurnStart(null);
				_baseAbilities[i].Action.BlessingOnTurnStart(null);
			}
			else
			{
				break;
			}
		}
	}

	public void ActionOnExit(ChunkData chunkDataPrev, ChunkData chunkData)
	{
		for (int i = 0; i < _baseAbilities.Count; i++)
		{
			if (_baseAbilities[i].enabled)
			{
				_baseAbilities[i].Action.OnExitAbility(chunkDataPrev, chunkData);
			}
		}
		for (int i = 0; i < _abilities.Count; i++)
		{
			if (_abilities[i].enabled && i < _unlockedAbilityList.Count && _unlockedAbilityList[i].abilityConfirmed)
			{
				_abilities[i].Action.OnExitAbility(chunkDataPrev, chunkData);
			}
		}
	}

	public void PlayerWasAttacked()
	{
		for (int i = 0; i < _baseAbilities.Count; i++)
		{
			if (_baseAbilities[i].enabled)
			{
				_baseAbilities[i].Action.PlayerWasAttacked();
			}
		}
		for (int i = 0; i < _abilities.Count; i++)
		{
			if (_abilities[i].enabled && i < _unlockedAbilityList.Count && _unlockedAbilityList[i].abilityConfirmed)
			{
				_abilities[i].Action.PlayerWasAttacked();
			}
			else
			{
				break;
			}
		}
	}

	public void OnTurnEnd()
	{
		// _debuffs.TurnCounter();
		// _debuffs.CheckAbilityTurns();
		// _debuffs.CheckBaseAbilityTurns();
		// _debuffs.CheckWholeAbilities();
	}

	public void OnAfterResolve()
	{
		
	}
	
	public void PlayerDied()
	{
		for (int i = 0; i < _baseAbilities.Count; i++)
		{
			if (_baseAbilities[i].enabled)
			{
				_baseAbilities[i].Action.Die();
			}
		}
		for (int i = 0; i < _abilities.Count; i++)
		{
			if (_abilities[i].enabled && i < _unlockedAbilityList.Count && _unlockedAbilityList[i].abilityConfirmed)
			{
				_abilities[i].Action.Die();
			}
			else
			{
				break;
			}
		}
	}

	public void OnMove(Vector2 position)
	{
		if (_currentAbility == null) return;
		_mousePosition = position;
		ChunkData hoveredChunk = GameTileMap.Tilemap.GetChunk(_mousePosition);

		if (_turnManager.GetCurrentTeamIndex() == _player.GetPlayerTeam())
		{
			_currentAbility.Action.OnMoveArrows(hoveredChunk,_previousChunk);
		}
		_currentAbility.Action.OnMoveHover(hoveredChunk,_previousChunk);
		_previousChunk = hoveredChunk;
	}

	public Ability GetCurrentAbility()
	{
		return _currentAbility;
	}

	public void OnMouseClick(ChunkData chunkData)
	{
		ExecuteCurrentAbility(chunkData);
		ExecuteCurrentBaseAbility(chunkData);
	}

	public void DeselectAbility()
	{
		DeselectCurrentAbility();
	}

	public void SetCurrentAbility(Ability ability, int abilityIndex)
	{
		if (_currentAbility != null)
		{
			_currentAbility.Action.ClearGrid();
		}

		if (ability != null)
		{
			_currentAbility = ability;
		}
		else
		{
			_currentAbility = null;
			_currentAbilitySelectActionButton = null;
		}

		if (_currentAbility != null)
		{
			_currentAbility.Action.CreateGrid();
		}
	}
	
	public bool IsAbilitySelected()
	{
		return _currentAbility != null;
	}

	public bool CanAbilityBeUsedOnTile(Vector2 position)
	{
		return _currentAbility.Action.IsPositionInGrid(position);
	}
	
	public bool IsMovementSelected()
	{
		if(_currentAbility!=null)
			return _currentAbility.Action.GetType() == typeof(PlayerMovement);
		return false;
	}
	
	private void ExecuteCurrentBaseAbility(ChunkData chunkData)
	{
		if (_currentAbility != null && _currentAbility._type == AbilityType.BaseAbility)
		{
			if (chunkData != null && _currentAbility.Action.AbilityCanBeActivated())
			{
				Ability tempAbility = _currentAbility;
				_currentAbility.Action.ResolveAbility(chunkData);
				tempAbility.Action.ResolveBlessings(chunkData);
			}
		}
	}

	private void ExecuteCurrentAbility(ChunkData chunkData)
	{
		if ( _currentAbility != null && _currentAbility._type == AbilityType.Ability)
		{
			if (chunkData != null && _currentAbility.Action.AbilityCanBeActivated() && _abilityPoints >= _currentAbility.Action.GetAbilityPoints())
			{
				Ability tempAbility = _currentAbility;
				RemoveAbilityPoints(_currentAbility.Action.GetAbilityPoints());
				_currentAbility.Action.GetActionButton().UpdateAllButtonsByPoints(_abilityPoints);
				_currentAbility.Action.ResolveAbility(chunkData);
				tempAbility.Action.ResolveBlessings(chunkData);
			}
		}
	}
	
	private void DeselectCurrentAbility()
	{
		if (_currentAbility != null)
		{
			_currentAbility.Action.DeselectAbility();
			_currentAbility = null;
			_currentAbilitySelectActionButton = null;
		}
	}

}

