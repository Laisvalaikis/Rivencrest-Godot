using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Godot;
using Godot.Collections;

public partial class GameTileMap : Node2D
{
	public static GameTileMap Tilemap;

	[System.Serializable]
	public class SaveChunks
	{
		public List<ChunkData> chunks;

		public SaveChunks()
		{
			chunks = new List<ChunkData>();
		}
	}
	
	public SelectAction selectAction;
	[Export] public TileMapData currentMap;
	[Export] private TurnManager _turnManager;
	[Export] private TeamInformation teamInformation;
	[Export] private Node2D highligtHolder;
	[Export] private Resource textTilePrefab;
	[Export] private Resource highlightTilePrefab;
	[Export] private bool showChunks;
	private int _chunkCountWidth = 0;
	private int _chunkCountHeight = 0;
	private List<SaveChunks> _chunks;
	private List<ChunkData> _allChunks;
	private ChunkData[,] _chunksArray;

	private Thread _threadDistance;

	private bool _updateWeight = false;
	private int _countForTreeSpawn = 0;
	public Player _currentSelectedCharacter = null;
	private PlayerInformation _currentPlayerInformation;
	private Vector2 _mousePosition;
	private int chunckIndex;
	private bool chuncksIsSetUp = false;
	private bool chunckSetupFinished = false;
	private Random _random;

	public override void _EnterTree()
	{
		base._EnterTree();
		if (Tilemap == null)
		{
			Tilemap = this;
		}

		if (InputManager.Instance != null)
		{
			InputManager.Instance.SelectClick += MouseClick;
			InputManager.Instance.ReleaseFocusWhenNotFocused += DeselectCurrentCharacterWithButton;
		}

		_random = new Random();
	}

	public void SetupTiles(TileMapData tileMapData)
	{
		currentMap = tileMapData;

		if (currentMap._chunkSize > 0)
		{
			_chunks = new List<SaveChunks>();
			_allChunks = new List<ChunkData>();
			_chunkCountWidth = Mathf.CeilToInt(currentMap._mapWidth / currentMap._chunkSize);
			_chunkCountHeight = Mathf.CeilToInt(currentMap._mapHeight / currentMap._chunkSize);
			int tileSize = _chunkCountWidth * _chunkCountHeight;
			chunckIndex = 0;
			_chunksArray = new ChunkData[_chunkCountWidth, _chunkCountHeight];
			_threadDistance = new Thread(CalculateDistance);
			_threadDistance.Start();

		}
		else
		{
			GD.PrintErr("Chunk size can't be 0");
		}

	}

	public override void _Ready()
	{
		base._Ready();

	}


	public override void _ExitTree()
	{
		base._ExitTree();

		_updateWeight = false;
		if (_threadDistance != null && _threadDistance.IsAlive)
		{
			_threadDistance.Abort();
		}
		else if (_threadDistance != null)
		{
			_threadDistance.Join();
		}

		if (Tilemap == this)
		{
			Tilemap = null;
		}

	}

	// public override void _Input(InputEvent @event)
	// {
	// 	if (@event is InputEventMouseMotion eventMouseMotion)
	// 	{
	// 		QueueRedraw();
	// 	}
	//
	// }
	//
	//
	// public override void _Draw()
	// {
	// 	base._Draw();
	// 	if (_allChunks != null)
	// 	{
	// 		if (showChunks)
	// 		{
	//
	// 			for (int i = 0; i < _allChunks.Count; i++)
	// 			{ 
	// 				ChunkData data = _allChunks[i];
	// 				if (!data.TileIsLocked())
	// 				{
	// 					// GD.Print("Mouse Motion at: ", GetGlobalMousePosition());
	// 					Rect2 rect = new Rect2();
	// 					rect.Size = new Vector2(data.GetDimensions().X, data.GetDimensions().Y);
	// 					rect.Position = new Vector2(data.GetPosition().X - (currentMap._chunkSize/2), data.GetPosition().Y - (currentMap._chunkSize/2));
	// 					if (data.CheckIfPosition(GetGlobalMousePosition(), currentMap))
	// 					{
	// 						DrawRect(rect, Colors.Red, true);
	// 					}
	// 					else
	// 					{
	// 						DrawRect(rect, Colors.Cyan, true);
	// 					}
	// 				}
	// 			}
	// 		}
	// 	}
	// }

	public override void _Process(double delta)
	{
		base._Process(delta);
		if (_threadDistance != null && !_threadDistance.IsAlive && !chuncksIsSetUp)
		{
			chuncksIsSetUp = true;
		}
	}

	public bool ChunksGenerationCompleted()
	{
		return chuncksIsSetUp;
	}


	void CalculateDistance()
	{
		float leftSizeOfWidth = currentMap._mapWidth;
		float widthPosition = currentMap._initialPosition.X;
		for (int w = 0; w < _chunkCountWidth; w++)
		{
			float leftSizeOfHeight = currentMap._mapHeight;
			float heightPosition = currentMap._initialPosition.Y;

			float widthSize = currentMap._chunkSize;

			if (leftSizeOfWidth / currentMap._chunkSize < 1)
			{
				widthSize = leftSizeOfWidth / currentMap._chunkSize;
			}

			_chunks.Add(new SaveChunks());

			for (int h = 0; h < _chunkCountHeight; h++)
			{
				float heightSize = currentMap._chunkSize;

				if (leftSizeOfHeight / currentMap._chunkSize < 1)
				{
					heightSize = leftSizeOfHeight / currentMap._chunkSize;
				}

				PackedScene tileHighlight = (PackedScene)highlightTilePrefab;
				HighlightTile tileNode = (HighlightTile)tileHighlight.Instantiate();
				highligtHolder.CallDeferred("add_child", tileNode);
				tileNode.SetDeferred("global_position",
					new Vector2(widthPosition + (currentMap._chunkSize / 2),
						heightPosition + (currentMap._chunkSize / 2)));
				tileNode.CallDeferred("hide");

				ChunkData chunk = new ChunkData(w, h, widthSize, heightSize,
					widthPosition + (currentMap._chunkSize / 2), heightPosition + (currentMap._chunkSize / 2), false,
					true, tileNode);
				if (currentMap._mapBoundries.boundries[h].Y - currentMap._chunkSize <= heightPosition - (heightSize) &&
					currentMap._mapBoundries.boundries[h].Y >= heightPosition &&
					currentMap._mapBoundries.boundries[h].X <= widthPosition &&
					currentMap._mapBoundries.boundries[h].Z >= widthPosition)
				{
					chunk.SetTileIsLocked(false);
				}
				else
				{
					chunk.SetTileIsLocked(true);
				}

				chunckIndex++;
				lock (_chunksArray)
				{
					_chunksArray[w, h] = chunk;
				}

				_chunks[w].chunks.Add(chunk);
				_allChunks.Add(chunk);

				heightPosition += heightSize;
				leftSizeOfHeight -= heightSize;
			}

			leftSizeOfWidth -= widthSize;
			widthPosition += widthSize;
		}

		// _threadDistance.Join();
		// _updateWeight = true;
		// _threadWeight = new Thread(UpdateChunkWeight);
		// _threadWeight.Start();
		// _avlTree.DisplayTree();
	}

	public ChunkData[,] GetChunksArray()
	{
		return _chunksArray;
	}

	public ChunkData GetChunk(Vector2 position)
	{
		int widthChunk = Mathf.CeilToInt((position.X - currentMap._initialPosition.X) / currentMap._chunkSize) - 1;
		int heightChunk = Mathf.CeilToInt((position.Y - currentMap._initialPosition.Y) / currentMap._chunkSize) - 1;


		lock (_chunksArray)
		{

			if (_chunksArray.GetLength(0) > widthChunk && widthChunk >= 0
													   && _chunksArray.GetLength(1) > heightChunk && heightChunk >= 0
													   && _chunksArray[widthChunk, heightChunk] != null &&
													   !_chunksArray[widthChunk, heightChunk].TileIsLocked())
			{
				return _chunksArray[widthChunk, heightChunk];
			}
			else
			{
				return null;
			}
		}

	}

	public ChunkData GetChunkDataByIndex(int x, int y)
	{
		return _chunksArray[x, y];
	}

	public List<ChunkData> GetAllChunksAround(ChunkData chunkData)
	{
		List <ChunkData> chunksAround = new List<ChunkData>();
		(int x, int y) = chunkData.GetIndexes();
		(int x, int y)[] indexes = new[]
		{
			(x - 1, y),
			(x + 1, y),
			(x, y - 1),
			(x, y + 1),
		};
		for (int i = 0; i < indexes.Length; i++)
		{
			if (CheckBounds(indexes[i].x, indexes[i].y))
			{
				chunksAround.Add(_chunksArray[indexes[i].x, indexes[i].y]);
			}
		}
		return chunksAround;
	}

	public bool CheckFogTopChunkBasedOnPrevious(ChunkData chunkData)
	{
		(int x, int y) = chunkData.GetIndexes();

		if (CheckBoundsWithoutLock(x, y - 1))
		{
			return !_chunksArray[x, y - 1].IsFogOnTile();
		}
		else
		{
			return false;
		}
	}
	
	public bool CheckFogBottomChunkBasedOnPrevious(ChunkData chunkData)
	{
		(int x, int y) = chunkData.GetIndexes();

		if (CheckBoundsWithoutLock(x, y + 1))
		{
			return !_chunksArray[x, y + 1].IsFogOnTile();
		}
		else
		{
			return false;
		}
	}
	
	public bool CheckFogRightChunkBasedOnPrevious(ChunkData chunkData)
	{
		(int x, int y) = chunkData.GetIndexes();

		if (CheckBoundsWithoutLock(x + 1, y))
		{
			return !_chunksArray[x + 1, y].IsFogOnTile();
		}
		else
		{
			return false;
		}
	}
	
	public bool CheckFogLeftChunkBasedOnPrevious(ChunkData chunkData)
	{
		(int x, int y) = chunkData.GetIndexes();

		if (CheckBoundsWithoutLock(x - 1, y))
		{
			return !_chunksArray[x - 1, y].IsFogOnTile();
		}
		else
		{
			return false;
		}
	}
	

	public bool CheckBounds(int x, int y)
	{
		if (_chunksArray.GetLength(0) > x && x >= 0
										  && _chunksArray.GetLength(1) > y && y >= 0
										  && _chunksArray[x, y] != null
										  && !_chunksArray[x, y].TileIsLocked())
		{
			return true;
		}

		return false;
	}
	
	public bool CheckBoundsWithoutLock(int x, int y)
	{
		if (_chunksArray.GetLength(0) > x && x >= 0
										  && _chunksArray.GetLength(1) > y && y >= 0
										  && _chunksArray[x, y] != null)
		{
			return true;
		}

		return false;
	}

	public bool CheckIfWall(int x, int y)
	{
		if ((_chunksArray.GetLength(0) - 1 == x || x == 0)
			&& (_chunksArray.GetLength(1) - 1 == y || y == 0)
			&& _chunksArray[x, y] != null
			&& !_chunksArray[x, y].TileIsLocked())
		{
			return true;
		}

		return false;
	}

	public bool CheckMouseBounds(Vector2 mousePosition)
	{
		
		int widthChunk = Mathf.CeilToInt((mousePosition.X - currentMap._initialPosition.X) / currentMap._chunkSize) - 1;
		int heightChunk = Mathf.CeilToInt((mousePosition.Y - currentMap._initialPosition.Y) / currentMap._chunkSize) - 1;


		lock (_chunksArray)
		{

			if (_chunksArray.GetLength(0) > widthChunk && widthChunk >= 0
													   && _chunksArray.GetLength(1) > heightChunk && heightChunk >= 0
													   && _chunksArray[widthChunk, heightChunk] != null
													   && !_chunksArray[widthChunk, heightChunk].TileIsLocked()
													   && !_chunksArray[widthChunk, heightChunk].IsFogOnTile())
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
	
	public bool CheckMouseBoundsWithFog(Vector2 mousePosition)
	{
		
		int widthChunk = Mathf.CeilToInt((mousePosition.X - currentMap._initialPosition.X) / currentMap._chunkSize) - 1;
		int heightChunk = Mathf.CeilToInt((mousePosition.Y - currentMap._initialPosition.Y) / currentMap._chunkSize) - 1;


		lock (_chunksArray)
		{

			if (_chunksArray.GetLength(0) > widthChunk && widthChunk >= 0
			                                           && _chunksArray.GetLength(1) > heightChunk && heightChunk >= 0
			                                           && _chunksArray[widthChunk, heightChunk] != null
			                                           && !_chunksArray[widthChunk, heightChunk].TileIsLocked())
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
	
	public void ResetChunk(ChunkData chunk)
	{
		if (chunk != null)
		{
			chunk.SetCurrentCharacter(null);
			chunk.GetTileHighlight().ActivatePlayerTile(false);
			chunk.GetTileHighlight().EnableTile(false);
		}
	}

	public void ResetChunkObject(ChunkData chunk)
	{
		if (chunk != null)
		{
			chunk.SetCurrentObject(null);
		}
	}

	public void SetCharacter(Vector2 mousePosition, Player character)
	{
		if (GetChunk(mousePosition) != null)
		{
			ChunkData chunk = GetChunk(mousePosition);
			chunk.SetCurrentCharacter(character);
			UpdateFog(character);
			chunk.GetTileHighlight().ActivatePlayerTile(true);
			chunk.GetTileHighlight().EnableTile(true);
		}
	}

	private void UpdateFog(Player character)
	{
		Ability playerMovement = character.actionManager.ReturnPlayerMovement();
		if (playerMovement != null)
		{
			character.GenerateCharacterPositions();
			List<ChunkData> chunkDatas = playerMovement.Action.GetVisionChunkList();
			foreach (ChunkData generatedChunk in chunkDatas)
			{
				if (generatedChunk.IsFogOnTile())
				{
					generatedChunk.SetFogOnTile(false);
					character.AddVisionTile(generatedChunk);
					if (generatedChunk.CharacterIsOnTile())
					{
						Player player = generatedChunk.GetCurrentPlayer();
						player.EnableObject();
						generatedChunk.GetTileHighlight().ActivatePlayerTile(true);
						generatedChunk.GetTileHighlight().EnableTile(true);
					}

					if (generatedChunk.ObjectIsOnTile())
					{
						generatedChunk.GetCurrentObject().EnableObject();
						generatedChunk.GetTileHighlight().EnableTile(true);
					}
				}
			}
		}
	}

	public void UpdateFog(BaseAction baseAction, Player currentPlayer)
	{
		List<ChunkData> chunkDatas = baseAction.GetVisionChunkList();
		foreach (ChunkData generatedChunk in chunkDatas)
		{
			if (generatedChunk.IsFogOnTile())
			{
				generatedChunk.SetFogOnTile(false);
				currentPlayer.AddVisionTile(generatedChunk);
				if (generatedChunk.CharacterIsOnTile())
				{
					Player player = generatedChunk.GetCurrentPlayer();
					player.EnableObject();
					generatedChunk.GetTileHighlight().ActivatePlayerTile(true);
					generatedChunk.GetTileHighlight().EnableTile(true);
				}

				if (generatedChunk.ObjectIsOnTile())
				{
					generatedChunk.GetCurrentObject().EnableObject();
					generatedChunk.GetTileHighlight().EnableTile(true);
				}
			}
		}
	}
	
	public void RemoveFog(ChunkData chunkData, Player currentPlayer)
	{
		if (chunkData.IsFogOnTile())
		{
			chunkData.SetFogOnTile(false);
			currentPlayer.AddVisionTile(chunkData);
			if (chunkData.CharacterIsOnTile())
			{
				Player player = chunkData.GetCurrentPlayer();
				player.EnableObject();
				chunkData.GetTileHighlight().ActivatePlayerTile(true);
				chunkData.GetTileHighlight().EnableTile(true);
			}

			if (chunkData.ObjectIsOnTile())
			{
				chunkData.GetCurrentObject().EnableObject();
				chunkData.GetTileHighlight().EnableTile(true);
			}
		}
	}

	public void AddFog(ChunkData chunkData, Player currentPlayer)
	{
		if (!chunkData.IsFogOnTile())
		{
			chunkData.SetFogOnTile(true);
			currentPlayer.RemoveVisionTile(chunkData);
			if (chunkData.CharacterIsOnTile())
			{
				Player player = chunkData.GetCurrentPlayer();
				player.DisableObject();
				chunkData.GetTileHighlight().ActivatePlayerTile(false);
				chunkData.GetTileHighlight().EnableTile(false);
			}

			if (chunkData.ObjectIsOnTile())
			{
				chunkData.GetCurrentObject().DisableObject();
				chunkData.GetTileHighlight().EnableTile(false);
			}
		}
	}

	public void SetEnemy(Vector2 mousePosition, Player character)
	{
		if (GetChunk(mousePosition) != null)
		{
			ChunkData chunk = GetChunk(mousePosition);
			character.DisableObject();
			chunk.SetCurrentCharacter(character);
			UpdateFog(character);
		}
	}
	
	public void SetCharacter(ChunkData chunk, Player character)
	{
		if (chunk != null)
		{
			chunk.SetCurrentCharacter(character);
			chunk.GetTileHighlight().ActivatePlayerTile(true);
			chunk.GetTileHighlight().EnableTile(true);
		}
	}
	
	public void SetObject(ChunkData chunk, Object setObject)
	{
		if (chunk != null)
		{
			if (chunk.CharacterIsOnTile() && setObject.CanStepOn())
			{

				if (chunk.IsFogOnTile())
				{
					setObject.DisableObject();
				}
				else
				{
					if (chunk.CharacterIsOnTile())
					{
						setObject.StepOn(chunk);
						if (setObject.CanBeDestroyOnStepping())
						{
							setObject.Death();
						}
					}

					chunk.GetTileHighlight().EnableTile(true);
				}

				chunk.SetCurrentObject(setObject);
			}
			else if(!chunk.CharacterIsOnTile() && !setObject.CanStepOn())
			{
				if (chunk.IsFogOnTile())
				{
					setObject.DisableObject();
				}
				else
				{
					chunk.GetTileHighlight().EnableTile(true);
				}

				chunk.SetCurrentObject(setObject);
			}
		}
	}

	public bool CharacterIsOnTile(ChunkData chunkData)
	{
		if (chunkData != null)
		{
			return chunkData.CharacterIsOnTile();
		}
		return false;
	}

	public bool OtherCharacterIsOnTile(Vector2 mousePosition)
	{
		if (GetChunk(mousePosition) != null)
		{
			ChunkData chunkData = GetChunk(mousePosition);
			Player player = chunkData.GetCurrentPlayer();
			return player != null && player != _currentSelectedCharacter && player.IsObjectEnabled();
		}
		return false;
	}
	
	public void MoveSelectedCharacter(ChunkData chunk, Player character = null)
	{
		Player moveCharacter = _currentSelectedCharacter;
		if (character != null)
		{
			moveCharacter = character;
		}
	
		if (chunk != null && moveCharacter != null)
		{
			ChunkData previousCharacterChunk = Tilemap.GetChunk(moveCharacter.GlobalPosition);
			Vector2 characterPosition = chunk.GetPosition();
			if (previousCharacterChunk != chunk)
			{
				moveCharacter.GlobalPosition = characterPosition;
				moveCharacter.OnExit(previousCharacterChunk, chunk);
				SetCharacter(chunk, moveCharacter);
				UpdateFog(moveCharacter);
				ResetChunk(previousCharacterChunk);
				if (chunk.ObjectIsOnTile() && chunk.GetCurrentObject().CanStepOn())
				{
					Object currentObject = chunk.GetCurrentObject();
					currentObject.StepOn(chunk);
					if (currentObject.CanBeDestroyOnStepping())
					{
						currentObject.Death();
					}
				}
				if (previousCharacterChunk.ObjectIsOnTile() && previousCharacterChunk.GetCurrentObject().CanStepOn())
				{
					Object currentObject = previousCharacterChunk.GetCurrentObject();
					currentObject.OnExit(previousCharacterChunk, chunk);
				}
			}
		}
	}
	
	public void MoveSelectedCharacterWithoutReset(Vector2 mousePosition, Vector2 offset = default, Node2D character = null)
	{
		Node2D moveCharacter = _currentSelectedCharacter;
		if (character != null)
		{
			moveCharacter = character;
		}
	
		if (GetChunk(mousePosition) != null && moveCharacter != null) // !CharacterIsOnTile(mousePosition)
		{
			Vector2 characterPosition = GetChunk(mousePosition).GetPosition() - offset;
			moveCharacter.GlobalPosition = characterPosition;
		}
	}

	public void SelectTile(Vector2 mousePosition)
	{
		ChunkData chunk = GetChunk(mousePosition);
		if (chunk != null)
		{ 
			if (!_turnManager.IsCurrentTeamAI())
			{
				if (_currentSelectedCharacter != null)
				{
					_currentSelectedCharacter.actionManager.DeselectAbility();
				}
				_currentSelectedCharacter = chunk.GetCurrentPlayer();
				if (_currentSelectedCharacter != null && _currentSelectedCharacter.IsObjectEnabled())
				{
					selectAction.SetCurrentCharacter(_currentSelectedCharacter);
					selectAction.UpdatePlayerInfo();//idk about this
					_turnManager.SetCurrentCharacter(_currentSelectedCharacter);
					teamInformation.SelectCharacterPortrait(_currentSelectedCharacter);
					chunk.GetTileHighlight().ToggleSelectedPlayerUI(true);
				}
			}
		}
	}

	public void SpawnObject(Object objectToSpawn, ChunkData chunkData)
	{
		if (objectToSpawn != null)
		{
			chunkData.SetCurrentObject(objectToSpawn);
			_turnManager.AddObject(objectToSpawn);
			Vector2 characterPosition = chunkData.GetPosition();
			objectToSpawn.GlobalPosition = characterPosition;
			SetObject(chunkData, objectToSpawn);
		}
	}

	public void DeselectCurrentCharacter()
	{
		if (_currentSelectedCharacter != null && CharacterIsSelected())
		{
			selectAction.Disable();
			// _currentSelectedCharacter.actionManager.DeselectAbility();
			_currentSelectedCharacter.actionManager.SetCurrentAbility(null, -1);
			_turnManager.SetCurrentCharacter(null);
			teamInformation.SelectCharacterPortrait(_currentSelectedCharacter, false);
			GetChunk(_currentSelectedCharacter.GlobalPosition).GetTileHighlight().ToggleSelectedPlayerUI(false);
			_currentSelectedCharacter = null;
		}
	}

	public void DeselectCurrentCharacterWithButton()
	{
		DeselectCurrentCharacter();
		// if (!teamInformation.IsFocusedOnFirstCharacter())
		// {
		// 	DeselectCurrentCharacter();
		// }
	}

	public bool CharacterIsSelected()
	{
		return _currentSelectedCharacter != null;
	}
	
	public Player GetCurrentCharacter()
	{
		return _currentSelectedCharacter;
	}
	
	public void SetCurrentCharacter(Node2D currentCharacter)
	{
		_currentSelectedCharacter = (Player)currentCharacter;
		_turnManager.SetCurrentCharacter((Player)currentCharacter);
	}

	private void MouseClick(Vector2 mousePosition)
	{
		_mousePosition = mousePosition;
		ChunkData chunk = GetChunk(_mousePosition);
		if (!CharacterIsSelected()) // no character selected
		{
			SelectTile(_mousePosition);
		}
		else if (CharacterIsSelected() && OtherCharacterIsOnTile(_mousePosition) && _currentSelectedCharacter.actionManager.IsMovementSelected()) //Clicling on a different character when you have movement ability selected
		{
			SelectTile(_mousePosition);
		}
		else if(CharacterIsSelected() && OtherCharacterIsOnTile(_mousePosition) && !_currentSelectedCharacter.actionManager.CanAbilityBeUsedOnTile(_mousePosition)) //Clicked on character that is outside of ability grid to select it
		{
			SelectTile(_mousePosition);
		}
		else if(CharacterIsSelected() && chunk != null && GetCurrentCharacter() == chunk.GetCurrentPlayer() && !_turnManager.IsCurrentTeamAI()) // Clicking on currently selected character to deselect it
		{
			if(_currentSelectedCharacter.actionManager.GetCurrentAbility()!=null && !_currentSelectedCharacter.actionManager.GetCurrentAbility().Action.CanTileBeClicked(chunk))
				DeselectCurrentCharacter();
		}
	}
	
	protected override void Dispose(bool disposing)
	{
		InputManager.Instance.SelectClick -= MouseClick;
		Tilemap = null;
		base.Dispose(disposing);
	}
}

