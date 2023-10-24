using System.Collections.Generic;
using Godot;
using Godot.Collections;

public partial class ActionManager : Node
{
	[Export] 
	private Player _player;
	[Export] 
	private Debuffs _debuffs;
	private Array<Ability> _baseAbilities;
	private Array<Ability> _abilities;
	[Export]
	private Array<Ability> _allAbilities;
	[Export]
	private int availableAbilityPoints = 1;
	private int abilityPoints;
	private bool hasSlowAbilityBeenCast = false;
	private int trackingCount = 0;
	
	// for blessings
	private int _pointsBeforeAbility;
	
	private Vector2 _mousePosition;
	private Ability _currentAbility;
	private ChunkData _previousChunk;
	private bool abilityIsSelected = false;
	
	public override void _Ready()
	{
		base._Ready();
		
		InputManager.Instance.MouseMove += OnMove;
		InputManager.Instance.LeftMouseClick += OnMouseClick;
		
		_abilities = new Array<Ability>();
		_baseAbilities = new Array<Ability>();
		Array<Ability> allAbilities = new Array<Ability>();
		Array<Ability> baseAbilities = _player.playerInformation.playerInformationData.baseAbilities;
		allAbilities.AddRange(baseAbilities);
		allAbilities.AddRange(_player.playerInformation.playerInformationData.abilities);
		if (allAbilities != null && allAbilities.Count != 0)
		{
			for (int i = 0; i < allAbilities.Count; i++)
			{
				Ability ability = new Ability(allAbilities[i]);
				ability.Action.SetupAbility(_player);
				if (_allAbilities == null)
				{
					_allAbilities = new Array<Ability>();
					_allAbilities.Add(ability);
				}
				else
				{
					_allAbilities.Add(ability);
				}
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

	public int GetAllAbilityPoints()
	{
		return availableAbilityPoints;
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
	
	
	public void RemoveAllActionPoints() 
	{
		hasSlowAbilityBeenCast = true;
		abilityPoints = 0;
	}
	
	public void SetAbilityPoints(int abilityPoints)
	{
		availableAbilityPoints = abilityPoints;
	}
	
	public void AddAbilityCooldownPoints(int abilityPoints)
	{
		availableAbilityPoints += abilityPoints;
	}

	public void AddAbilityPoints(int abilityPoints)
	{
		this.abilityPoints += abilityPoints;
	}

	public void ResetAbilityPointsBeforeAbility()
	{
		this.abilityPoints = _pointsBeforeAbility;
	}

	private void RemoveAbilityPoints(int consumedPoints)
	{
		_pointsBeforeAbility = this.abilityPoints;
		this.abilityPoints -= consumedPoints;
	}

	public virtual void RefillActionPoints() //pradzioj ejimo
	{
		abilityPoints = availableAbilityPoints;
	}

	public void OnTurnStart()
	{

	}
	
	public void OnTurnEnd()
	{
		_debuffs.TurnCounter();
		_debuffs.CheckAbilityTurns();
		_debuffs.CheckBaseAbilityTurns();
		_debuffs.CheckWholeAbilities();
	}
	
	public void OnMove(Vector2 position)
	{
		if (_currentAbility == null) return;
		_mousePosition = position;
		ChunkData hoveredChunk = GameTileMap.Tilemap.GetChunk(_mousePosition);
		
		_currentAbility.Action.OnMoveArrows(hoveredChunk,_previousChunk);
		_currentAbility.Action.OnMoveHover(hoveredChunk,_previousChunk);
		_previousChunk = hoveredChunk;
	}
	
	public void OnMouseClick()
	{
		ExecuteCurrentAbility();
		ExecuteCurrentBaseAbility();
	}

	public void DeselectAbility()
	{
		DeselectCurrentAbility();
	}

	public void SetCurrentAbility(Ability ability)
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
	
	private void ExecuteCurrentBaseAbility()
	{
		if (_currentAbility != null && _currentAbility._type == AbilityType.BaseAbility && _debuffs.CanUseBaseAbility( this))
		{
			ChunkData chunk = GameTileMap.Tilemap.GetChunk(_mousePosition);
			if (chunk != null)
			{
				_currentAbility.Action.ResolveAbility(chunk);
				
				// turnManager.AddUsedAbility(new UsedAbility(_currentAbility, chunk));
			}
		}
	}
	
	private void ExecuteCurrentAbility()
	{
		if ( _currentAbility != null && _currentAbility._type == AbilityType.Ability && _debuffs.CanUseAbility())
		{
			ChunkData chunk = GameTileMap.Tilemap.GetChunk(_mousePosition);
			if (chunk != null)
			{
				_currentAbility.Action.ResolveAbility(chunk);
				
				// turnManager.AddUsedAbility(new UsedAbility(_currentAbility, chunk));
			}
		}
	}
	
	private void DeselectCurrentAbility()
	{
		if (_currentAbility != null)
		{
			_currentAbility.Action.DeselectAbility();
			_currentAbility = null;
		}
	}

}

