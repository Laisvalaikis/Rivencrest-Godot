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
	[Export] 
	public TileMapData currentMap;
	[Export] private TeamInformation teamInformation;
	[Export] private SelectAction _selectAction;
	[Export] private Node2D highligtHolder;
	[Export] private Resource textTilePrefab;
	[Export] private Resource highlightTilePrefab;
	// [SerializeField] private SpriteRenderer[] tileSpriteRenderers;
	// [SerializeField] private GameObject tiles;
	[Export] private bool showChunks;
	private int _chunkCountWidth = 0;
	private int _chunkCountHeight = 0;
	private List<SaveChunks> _chunks;
	private List<ChunkData> _allChunks;
	private List<ChunkData> _spawned;
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
		else
		{
			// Destroy(this.gameObject);
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
		if (_threadDistance.IsAlive)
		{
			_threadDistance.Abort();
		}
		else
		{
			_threadDistance.Join(); 
		}
		if (Tilemap == this)
		{
			Tilemap = null;
		}
		
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion eventMouseMotion)
		{
			QueueRedraw();
		}

	}


	public override void _Draw()
	{
		base._Draw();
		if (_allChunks != null)
		{
			if (showChunks)
			{

				for (int i = 0; i < _allChunks.Count; i++)
				{ 
					ChunkData data = _allChunks[i];
					if (!data.TileIsLocked())
					{
						// GD.Print("Mouse Motion at: ", GetGlobalMousePosition());
						Rect2 rect = new Rect2();
						rect.Size = new Vector2(data.GetDimensions().X, data.GetDimensions().Y);
						rect.Position = new Vector2(data.GetPosition().X - (currentMap._chunkSize/2), data.GetPosition().Y - (currentMap._chunkSize/2));
						if (data.CheckIfPosition(GetGlobalMousePosition(), currentMap))
						{
							DrawRect(rect, Colors.Red, true);
						}
						else
						{
							DrawRect(rect, Colors.Cyan, true);
						}
					}
				}
			}
		}
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		if (_threadDistance != null && !_threadDistance.IsAlive && !chuncksIsSetUp)
		{
			// StartCoroutine(SetupChunckTiles());
			chuncksIsSetUp = true;
		}
	}

	public bool ChunksIsSetuped()
	{
		return chuncksIsSetUp;
	}

	// IEnumerator SetupChunckTiles()
	// {
	//     for (int i = 0; i < _allChunks.Count; i++)
	//     {
	//         _allChunks[i].SetupChunk();
	//         yield return null;
	//     }
	//     chunckSetupFinished = true;
	//     EnableAllTiles();
	// }

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
				tileNode.SetDeferred("global_position", new Vector2(widthPosition + (currentMap._chunkSize/2) , heightPosition + (currentMap._chunkSize/2)));
				tileNode.CallDeferred("hide");
				// tileNode.GlobalPosition = new Vector2(widthPosition, heightPosition);
				
				ChunkData chunk = new ChunkData(w, h, widthSize, heightSize, widthPosition + (currentMap._chunkSize/2), heightPosition + + (currentMap._chunkSize/2), false, tileNode);
				//tileSpriteRenderers[chunckIndex], tileHighlights[chunckIndex], false
				if (currentMap._mapBoundries.boundries[h].Y - currentMap._chunkSize <= heightPosition - (heightSize) &&
					currentMap._mapBoundries.boundries[h].Y >=  heightPosition &&
					currentMap._mapBoundries.boundries[h].X <= widthPosition && 
					currentMap._mapBoundries.boundries[h].Z >= widthPosition)
				{
					chunk.SetTileIsLocked(false);
					// tileNode.CallDeferred("show");
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
		int widthChunk = Mathf.CeilToInt((position.X - currentMap._initialPosition.X)/currentMap._chunkSize)-1;
		int heightChunk = Mathf.CeilToInt((position.Y - currentMap._initialPosition.Y) / currentMap._chunkSize)-1;

		
		lock (_chunksArray)
		{
			
			if (_chunksArray.GetLength(0) > widthChunk && widthChunk >= 0
				&& _chunksArray.GetLength(1) > heightChunk && heightChunk >= 0
				&& _chunksArray[widthChunk, heightChunk] != null && !_chunksArray[widthChunk, heightChunk].TileIsLocked())
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

	public bool CheckBounds(Vector3 position)
	{
		int widthChunk = Mathf.CeilToInt((position.X - currentMap._initialPosition.X)/currentMap._chunkSize)-1;
		int heightChunk = Mathf.CeilToInt((position.Y - currentMap._initialPosition.Y) / currentMap._chunkSize)-1;
		if (_chunksArray.GetLength(0) > widthChunk && widthChunk >= 0 
												   &&_chunksArray.GetLength(1) > heightChunk && heightChunk >= 0 
												   && _chunksArray[widthChunk, heightChunk] != null 
												   && !_chunksArray[widthChunk, heightChunk].TileIsLocked())
		{
			return true;
		}

		return false;
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

	public bool CheckIfWall(int x, int y)
	{
		if ((_chunksArray.GetLength(0)-1 == x || x == 0) 
										  && (_chunksArray.GetLength(1)-1 == y || y == 0)
										  && _chunksArray[x, y] != null 
										  && !_chunksArray[x, y].TileIsLocked())
		{
			return true;
		}
		return false;
	}

	// public void EnableAllTiles()
	// {
	//     tiles.SetActive(true);
	// }
	//
	// public void DisableAllTiles()
	// {
	//     tiles.SetActive(false);
	// }

	public ChunkData GetRandomChunkAround(int indexX, int indexY)
	{
		
		lock (_chunksArray)
		{
			
			int randomX = _random.Next(-2,2);
			int randomY = _random.Next(-2,2);
			int tempIndexX = indexX + randomX;
			int tempIndexY = indexY + randomY;
			while (tempIndexX == indexX && tempIndexY == indexY || tempIndexX < 0 || tempIndexY < 0 || tempIndexX >= _chunks.Count || tempIndexY >= _chunks[indexX].chunks.Count)
			{
				randomX = _random.Next(-2,2);
				randomY = _random.Next(-2,2);
				tempIndexX = indexX + randomX;
				tempIndexY = indexY + randomY;
			}
			return _chunks[tempIndexY].chunks[tempIndexX];
		}
	}

	public void GenerateChunks()
	{
		if (currentMap._chunkSize > 0 && _threadDistance == null || currentMap._chunkSize > 0 && !_threadDistance.IsAlive)
		{
			_chunks = new List<SaveChunks>();
			_allChunks = new List<ChunkData>();
			_chunkCountWidth = Mathf.CeilToInt(currentMap._mapWidth / currentMap._chunkSize);
			_chunkCountHeight = Mathf.CeilToInt(currentMap._mapHeight / currentMap._chunkSize);
			// _avlTree = new AVL();
			// _maxHeap = new MaxHeap(_chunkCountWidth*_chunkCountHeight*2);
			_chunksArray = new ChunkData[_chunkCountWidth, _chunkCountHeight];
			_threadDistance = new Thread(CalculateDistance);
			_threadDistance.Start();
		   
		}
	}

	public void ResetChunks()
	{
		_chunks = new List<SaveChunks>();
		_allChunks = new List<ChunkData>();
		_chunkCountWidth = Mathf.CeilToInt(currentMap._mapWidth / currentMap._chunkSize);
		_chunkCountHeight = Mathf.CeilToInt(currentMap._mapHeight / currentMap._chunkSize);
		_chunksArray = new ChunkData[_chunkCountWidth, _chunkCountHeight];
	}

	private ChunkData GetRandomChunk()
	{
		int randomX = _random.Next(0, _chunks.Count);
		int randomY = _random.Next(0, _chunks[randomX].chunks.Count);
		return _chunks[randomX].chunks[randomY];
	}

	public void ResetChunkCharacter(ChunkData chunk)
	{
		if (chunk != null)
		{
			chunk.SetCurrentCharacter(null);
			chunk.GetTileHighlight().ActivatePlayerTile(false);
		}
	}
	
	public void SetCharacter(Vector2 mousePosition, Node2D character)
	{
		if (GetChunk(mousePosition) != null)
		{
			ChunkData chunkData = GetChunk(mousePosition);
			chunkData.SetCurrentCharacter(character);
			chunkData.GetTileHighlight().ActivatePlayerTile(true);
		}
	}
	
	public void SetCharacter(ChunkData chunk, Node2D character)
	{
		if (chunk != null)
		{
			chunk.SetCurrentCharacter(character);
			chunk.GetTileHighlight().ActivatePlayerTile(true);
		}
	}

	public bool CharacterIsOnTile(ChunkData chunkData)
	{
		if (chunkData != null)
		{
			return chunkData.GetCurrentPlayer() != null;
		}
		return false;
	}

	public bool OtherCharacterIsOnTile(Vector2 mousePosition)
	{
		if (GetChunk(mousePosition) != null)
		{
			ChunkData chunkData = GetChunk(mousePosition);
			return chunkData.GetCurrentPlayer() != null && chunkData.GetCurrentPlayer() != _currentSelectedCharacter;
		}
		return false;
	}

	public void MoveSelectedCharacter(Vector2 mousePosition, Vector2 offset = default, Node2D character = null)
	{
		Node2D moveCharacter = _currentSelectedCharacter;
		if (character != null)
		{
			moveCharacter = character;
		}
	
		if (GetChunk(mousePosition) != null && moveCharacter != null) // !CharacterIsOnTile(mousePosition)
		{
			ChunkData previousCharacterChunk =
				GameTileMap.Tilemap.GetChunk(moveCharacter.GlobalPosition);
			Vector2 characterPosition = GetChunk(mousePosition).GetPosition() - offset;
			moveCharacter.GlobalPosition = characterPosition;
			SetCharacter(mousePosition, moveCharacter);
			if(previousCharacterChunk!=GetChunk(mousePosition))
				ResetChunkCharacter(previousCharacterChunk);
			
		}
		// SelectedCharacter.GetComponent<GridMovement>().RemoveAvailableMovementPoints(newPosition);
		// bottomCornerUI.EnableAbilities(SelectedCharacter.GetComponent<PlayerInformation>().savedCharacter);
	}
	
	public void MoveSelectedCharacter(ChunkData chunk, Node2D character = null)
	{
		Node2D moveCharacter = _currentSelectedCharacter;
		if (character != null)
		{
			moveCharacter = character;
		}
	
		if (chunk != null && moveCharacter != null) // !CharacterIsOnTile(mousePosition)
		{
			ChunkData previousCharacterChunk = Tilemap.GetChunk(moveCharacter.GlobalPosition);
			Vector2 characterPosition = chunk.GetPosition();
			if (previousCharacterChunk != chunk)
			{
				moveCharacter.GlobalPosition = characterPosition;
				SetCharacter(chunk, moveCharacter);
				ResetChunkCharacter(previousCharacterChunk);
			}
		}
		// SelectedCharacter.GetComponent<GridMovement>().RemoveAvailableMovementPoints(newPosition);
		// bottomCornerUI.EnableAbilities(SelectedCharacter.GetComponent<PlayerInformation>().savedCharacter);
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
		
		if (GetChunk(mousePosition) != null)
		{
			ChunkData chunkData = GetChunk(mousePosition);
			_currentSelectedCharacter = chunkData.GetCurrentPlayer();
			if (_currentSelectedCharacter != null)
			{
				_selectAction.SetCurrentCharacter(_currentSelectedCharacter);
				teamInformation.SelectCharacterPortrait(_currentSelectedCharacter);
			}
		}
	}

	public void DeselectCurrentCharacter()
	{
		if (_currentSelectedCharacter != null)
		{
			_selectAction.Hide();
			_currentSelectedCharacter.actionManager.SetCurrentAbility(null);
			teamInformation.SelectCharacterPortrait(_currentSelectedCharacter, false);
			_currentSelectedCharacter = null;
		}
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
	}

	private void MouseClick()
	{
		_mousePosition = GetGlobalMousePosition();
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
		else if(CharacterIsSelected() && chunk!=null && GetCurrentCharacter()==chunk.GetCurrentPlayer()) // Clicking on currently selected character to deselect it
		{
			DeselectCurrentCharacter();
		}
	}
}

