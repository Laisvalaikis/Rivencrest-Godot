using System;
using System.Collections;
using System.Collections.Generic;
using Godot;
public partial class AbilityManager : Node2D
{
	[Export] private GameTileMap gameTileMap;
	private Vector2 _mousePosition;
	private BaseAction _currentAbility;
	private ChunkData _previousChunk;
	private List<ChunkData> _path;
	private List<ChunkData> _lastPath;
	// [Export] private TurnManager turnManager;
	

	public override void _Ready()
	{
		base._Ready();
		// InputManager.Instance.MouseMove += OnMove;
		// InputManager.Instance.LeftMouseClick += OnMouseClick;
	}

	public void OnMove(Vector2 position)
	{
		if (_currentAbility == null) return;
		_mousePosition = position;
		ChunkData hoveredChunk = gameTileMap.GetChunk(_mousePosition);
		
		_currentAbility.OnMoveArrows(hoveredChunk,_previousChunk);
		_currentAbility.OnMoveHover(hoveredChunk,_previousChunk);
		_previousChunk = hoveredChunk;
	}
	
	public void OnMouseClick()
	{
		ExecuteCurrentAbility();
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
			ChunkData chunk = gameTileMap.GetChunk(_mousePosition);
			if (chunk != null)
			{
				_currentAbility.ResolveAbility(chunk);
				// turnManager.AddUsedAbility(new UsedAbility(_currentAbility, chunk));
			}
		}
	}
}

