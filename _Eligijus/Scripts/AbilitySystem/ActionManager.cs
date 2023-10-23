using System.Collections.Generic;
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
	private int availableAbilityPoints = 1;
	private int abilityPoints;
	private bool hasSlowAbilityBeenCast = false;
	private int trackingCount = 0;
	
	// for blessings
	private int _turnCount = 0;
	private int _turnsPassed = 0;
	
	private Vector2 _mousePosition;
	private BaseAction _currentAbility;
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
				_baseAbilities.Add(_allAbilities[i]);
			}

			for (int i = _baseAbilities.Count; i < allAbilities.Count; i++)
			{
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
	
	public virtual void RefillActionPoints() //pradzioj ejimo
	{
		abilityPoints = availableAbilityPoints;
	}

	public void OnTurnStart()
	{

	}
	
	public void OnTurnEnd()
	{
		_turnCount++;
		
	}
	
	public void OnMove(Vector2 position)
	{
		if (_currentAbility == null) return;
		_mousePosition = position;
		ChunkData hoveredChunk = GameTileMap.Tilemap.GetChunk(_mousePosition);
		
		_currentAbility.OnMoveArrows(hoveredChunk,_previousChunk);
		_currentAbility.OnMoveHover(hoveredChunk,_previousChunk);
		_previousChunk = hoveredChunk;
	}
	
	public void OnMouseClick()
	{
		ExecuteCurrentAbility();
	}

	public void DeselectAbility()
	{
		DeselectCurrentAbility();
	}

	public void SetCurrentAbility(BaseAction ability)
	{
		if (_currentAbility != null)
		{
			_currentAbility.ClearGrid();
		}

		_currentAbility = ability;
		if (_currentAbility != null)
		{
			_currentAbility.CreateGrid();
		}
	}

	public void SetTurnCounterFromThisTurn()
	{
		_turnsPassed = _turnCount;
	}
	
	public bool IsAbilitySelected()
	{
		return _currentAbility != null;
	}

	public bool CanAbilityBeUsedOnTile(Vector2 position)
	{
		return _currentAbility.IsPositionInGrid(position);
	}
	
	public bool IsMovementSelected()
	{
		if(_currentAbility!=null)
			return _currentAbility.GetType() == typeof(PlayerMovement);
		return false;
	}
	
	private void ExecuteCurrentAbility()
	{
		if (_currentAbility != null)
		{
			ChunkData chunk = GameTileMap.Tilemap.GetChunk(_mousePosition);
			if (chunk != null)
			{
				_currentAbility.ResolveAbility(chunk);
				
				// turnManager.AddUsedAbility(new UsedAbility(_currentAbility, chunk));
			}
		}
	}
	
	private void DeselectCurrentAbility()
	{
		if (_currentAbility != null)
		{
			_currentAbility.DeselectAbility();
			_currentAbility = null;
		}
	}
	
	

}

