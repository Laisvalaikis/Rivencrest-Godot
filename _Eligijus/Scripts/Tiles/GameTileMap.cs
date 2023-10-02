using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Godot;

public partial class GameTileMap : Node2D
{
	// [SerializeField] private Camera mainCamera;
	// [SerializeField] private SelectAction _selectAction;

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
	// [SerializeField] private SpriteRenderer[] tileSpriteRenderers;
	// [SerializeField] private HighlightTile[] tileHighlights;
	// [SerializeField] private GameObject tiles;
	[Export] private bool showChunks;
	// [SerializeField] private AbilityManager abilityManager;
	private int _chunkCountWidth = 0;
	private int _chunkCountHeight = 0;
	private List<SaveChunks> _chunks;
	private List<ChunkData> _allChunks;
	private List<ChunkData> _spawned;
	private ChunkData[,] _chunksArray;

	private Thread _threadDistance;

	private bool _updateWeight = false;
	private int _countForTreeSpawn = 0;
	public Node _currentSelectedCharacter;
	// private PlayerInformation _currentPlayerInformation;
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
			// for (int i = tileSize; i < tileSpriteRenderers.Length; i++)
			// {
			//     tileSpriteRenderers[i].gameObject.SetActive(false);
			// }
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
						rect.Position = new Vector2(data.GetPosition().X, data.GetPosition().Y);
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


	// Update is called once per frame
	void Update()
	{
		// if (!_threadDistance.IsAlive && !chuncksIsSetUp)
		// {
		//     StartCoroutine(SetupChunckTiles());
		//     chuncksIsSetUp = true;
		// }
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
				
				ChunkData chunk = new ChunkData(w, h, widthSize, heightSize, widthPosition, heightPosition, false);
				//tileSpriteRenderers[chunckIndex], tileHighlights[chunckIndex], false
				if (currentMap._mapBoundries.boundries[h].Y - currentMap._chunkSize <= heightPosition - (heightSize) &&
					currentMap._mapBoundries.boundries[h].Y >=  heightPosition &&
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

	public ChunkData GetChunk(Vector3 position)
	{
		int widthChunk = Mathf.CeilToInt((position.X - currentMap._initialPosition.X)/currentMap._chunkSize) - 1;
		int heightChunk = Mathf.CeilToInt((currentMap._initialPosition.Y - position.Y) / currentMap._chunkSize) - 1;

		
		lock (_chunksArray)
		{
			
			if (_chunksArray.GetLength(0) > heightChunk && heightChunk >= 0
				&& _chunksArray.GetLength(1) > widthChunk && widthChunk >= 0
				&& _chunksArray[heightChunk, widthChunk] != null && !_chunksArray[heightChunk, widthChunk].TileIsLocked())
			{
				return _chunksArray[heightChunk, widthChunk];
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

	// public void ResetChunkCharacter(Vector3 mousePosition)
	// {
	//     if (GetChunk(mousePosition) != null)
	//     {
	//         ChunkData chunk = GetChunk(mousePosition);
	//         chunk.SetCurrentCharacter(null, null);
	//         chunk.GetTileHighlight().ActivatePlayerTile(false);
	//     }
	// }
	
	// public void SetCharacter(Vector3 mousePosition, GameObject character, PlayerInformation playerInformation)
	// {
	//     if (GetChunk(mousePosition) != null)
	//     {
	//         ChunkData chunkData = GetChunk(mousePosition);
	//         chunkData.SetCurrentCharacter(character, playerInformation);
	//         chunkData.GetTileHighlight().ActivatePlayerTile(true);
	//     }
	// }
	//
	// public void SetCharacter(ChunkData chunk, GameObject character, PlayerInformation playerInformation)
	// {
	//     if (chunk != null)
	//     {
	//         chunk.SetCurrentCharacter(character, playerInformation);
	//         chunk.GetTileHighlight().ActivatePlayerTile(true);
	//     }
	// }

	public bool CharacterIsOnTile(Vector3 mousePosition)
	{
		if (GetChunk(mousePosition) != null)
		{
			ChunkData chunkData = GetChunk(mousePosition);
			return chunkData.GetCurrentCharacter() != null;
		}
		return false;
	}

	public bool OtherCharacterIsOnTile(Vector3 mousePosition)
	{
		if (GetChunk(mousePosition) != null)
		{
			ChunkData chunkData = GetChunk(mousePosition);
			return chunkData.GetCurrentCharacter() != null && chunkData.GetCurrentCharacter() != _currentSelectedCharacter;
		}
		return false;
	}

	// public void MoveSelectedCharacter(Vector3 mousePosition, Vector3 offset = default, GameObject character = null)
	// {
	//     GameObject moveCharacter = _currentSelectedCharacter;
	//     if (character != null)
	//     {
	//         moveCharacter = character;
	//     }
	//
	//     if (GetChunk(mousePosition) != null && moveCharacter != null) // !CharacterIsOnTile(mousePosition)
	//     {
	//         ChunkData previousCharacterChunk =
	//             GameTileMap.Tilemap.GetChunk(moveCharacter.transform.position);
	//         Vector3 characterPosition = GetChunk(mousePosition).GetPosition() - offset;
	//         moveCharacter.transform.position = characterPosition;
	//         SetCharacter(mousePosition, moveCharacter, previousCharacterChunk.GetCurrentPlayerInformation());
	//         if(previousCharacterChunk!=GetChunk(mousePosition))
	//             ResetChunkCharacter(previousCharacterChunk.GetPosition());
	//         
	//     }
	//     // SelectedCharacter.GetComponent<GridMovement>().RemoveAvailableMovementPoints(newPosition);
	//     // bottomCornerUI.EnableAbilities(SelectedCharacter.GetComponent<PlayerInformation>().savedCharacter);
	// }
	//
	// public void MoveSelectedCharacter(ChunkData chunk, GameObject character = null)
	// {
	//     GameObject moveCharacter = _currentSelectedCharacter;
	//     if (character != null)
	//     {
	//         moveCharacter = character;
	//     }
	//
	//     if (chunk != null && moveCharacter != null) // !CharacterIsOnTile(mousePosition)
	//     {
	//         ChunkData previousCharacterChunk = Tilemap.GetChunk(moveCharacter.transform.position);
	//         Vector3 characterPosition = chunk.GetPosition();
	//         moveCharacter.transform.position = characterPosition;
	//         SetCharacter(chunk, moveCharacter, previousCharacterChunk.GetCurrentPlayerInformation());
	//         ResetChunkCharacter(previousCharacterChunk.GetPosition());
	//     }
	//     // SelectedCharacter.GetComponent<GridMovement>().RemoveAvailableMovementPoints(newPosition);
	//     // bottomCornerUI.EnableAbilities(SelectedCharacter.GetComponent<PlayerInformation>().savedCharacter);
	// }
	
	// public void MoveSelectedCharacterWithoutReset(Vector3 mousePosition, Vector3 offset = default, GameObject character = null)
	// {
	//     GameObject moveCharacter = _currentSelectedCharacter;
	//     if (character != null)
	//     {
	//         moveCharacter = character;
	//     }
	//
	//     if (GetChunk(mousePosition) != null && moveCharacter != null) // !CharacterIsOnTile(mousePosition)
	//     {
	//         Vector3 characterPosition = GetChunk(mousePosition).GetPosition() - offset;
	//         moveCharacter.transform.position = characterPosition;
	//     }
	//     // SelectedCharacter.GetComponent<GridMovement>().RemoveAvailableMovementPoints(newPosition);
	//     // bottomCornerUI.EnableAbilities(SelectedCharacter.GetComponent<PlayerInformation>().savedCharacter);
	// }

	public void SelectTile(Vector3 mousePosition)
	{
		
		if (GetChunk(mousePosition) != null)
		{
			ChunkData chunkData = GetChunk(mousePosition);
			// _currentSelectedCharacter = chunkData.GetCurrentCharacter();
			// _currentPlayerInformation = chunkData.GetCurrentPlayerInformation();
			// if (_currentSelectedCharacter != null)
			// {
			//     _selectAction.SetCurrentCharacter(_currentSelectedCharacter);
			// }
		}
	}

	// public void DeselectCurrentCharacter()
	// {
	//     if (_currentSelectedCharacter != null)
	//     {
	//         _currentSelectedCharacter = null;
	//         _currentPlayerInformation = null;
	//         _selectAction.gameObject.SetActive(false);
	//         abilityManager.SetCurrentAbility(null);
	//     }
	// }

	public bool CharacterIsSelected()
	{
		return _currentSelectedCharacter != null;
	}
	
	public Node GetCurrentCharacter()
	{
		return _currentSelectedCharacter;
	}
	
	public void SetCurrentCharacter(Node currentCharacter)
	{
		_currentSelectedCharacter = currentCharacter;
	}

	// public void OnMouseClick(InputAction.CallbackContext context)
	// {
	//     if (context.performed)
	//         MouseClick();
	// }
	
	// private void MouseClick()
	// {
	//     _mousePosition = Input.mousePosition;
	//     Vector3 mousePos = new Vector3(_mousePosition.x, _mousePosition.y, mainCamera.nearClipPlane);
	//     Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
	//     ChunkData chunk = GetChunk(worldPos);
	//     if (!CharacterIsSelected()) // no character selected
	//     {
	//         SelectTile(worldPos);
	//     }
	//     else if (CharacterIsSelected() && OtherCharacterIsOnTile(worldPos) && abilityManager.IsMovementSelected()) //Clicling on a different character when you have movement ability selected
	//     {
	//         SelectTile(worldPos);
	//     }
	//     else if(CharacterIsSelected() && OtherCharacterIsOnTile(worldPos) && !abilityManager.CanAbilityBeUsedOnTile(worldPos)) //Clicked on character that is outside of ability grid to select it
	//     {
	//         SelectTile(worldPos);
	//     }
	//     else if(CharacterIsSelected() && chunk!=null && GetCurrentCharacter()==chunk.GetCurrentCharacter()) // Clicking on currently selected character to deselect it
	//     {
	//         DeselectCurrentCharacter();
	//     }
	// }
}
