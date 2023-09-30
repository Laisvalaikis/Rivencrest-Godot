using System.Collections;
using System.Collections.Generic;
using Godot;

[System.Serializable]
public class ChunkData
{

    public ChunkData(int indexWidth, int indexHeight, float width, float height, float positionWidth, float positionHeight, bool tileIsLocked) //SpriteRenderer tileSpriteRenderer, HighlightTile highlightTile,  bool tileIsLocked
    {
        _indexHeight = indexHeight;
        _indexWidth = indexWidth;
        _width = width;
        _height = height;
        _positionWidth = positionWidth;
        _positionHeight = positionHeight;
        _tileIsLocked = tileIsLocked;
        // _tileSpriteRenderer = tileSpriteRenderer;
        // _highlightTile = highlightTile;
    }
    
    public ChunkData(int indexHeight, int indexWidth)
    {
        _indexHeight = indexHeight;
        _indexWidth = indexWidth;
    }

    public ChunkData()
    {
    }

    private float _width;
    private float _height;
    private float _positionHeight;
    private float _positionWidth;
    private int _indexWidth;
    private int _indexHeight;
    private float _weight = 0;
    private bool _weightUpdated = false;
    private bool _dataWasInserted = false;
    private int _heapIndex = -1;
    private bool _standingOnChunk = false;
    private bool _canUseTile = false;
    private bool _tileIsLocked = false;
    private Node _currentCharacter;
    // private PlayerInformation _currentPlayerInformation;
    private InformationType _type = InformationType.None;
    // private SpriteRenderer _tileSpriteRenderer;
    // private HighlightTile _highlightTile;
    
    // public void SetupChunk()
    // {
    //     if (!TileIsLocked())
    //     {
    //         _tileSpriteRenderer.gameObject.transform.position = GetChunkCenterPosition();
    //         _tileSpriteRenderer.gameObject.SetActive(true);
    //     }
    //     else
    //     {
    //         _tileSpriteRenderer.gameObject.SetActive(false);
    //     }
    // }

    // public void EnableTileRendering()
    // {
    //     if (!_tileIsLocked)
    //     {
    //         _tileSpriteRenderer.enabled = true;
    //     }
    // }
    
    // public void DisableTileRendering()
    // {
    //     if (!_tileIsLocked)
    //     {
    //         _tileSpriteRenderer.enabled = true;
    //     }
    // }
    
    // public void EnableTileRenderingGameObject()
    // {
    //     if (!_tileIsLocked)
    //     {
    //         _tileSpriteRenderer.gameObject.SetActive(true);
    //     }
    // }
    
    // public void DisableTileRenderingGameObject()
    // {
    //     if (!_tileIsLocked)
    //     {
    //         _tileSpriteRenderer.gameObject.SetActive(false);
    //     }
    // }

    public Vector3 GetPosition()
    {
        return new(_positionWidth, _positionHeight, 0);
    }
    
    public Vector3 GetChunkCenterPosition()
    {
        return new(_positionWidth, _positionHeight - _height/2, -0.1f);
    }

    public Vector3 GetDimensions()
    {
        return new(_width,_height, 1f);
    }

    public bool CheckIfPosition(Vector2 position, TileMapData currentMap)
    {
        int widthChunk = Mathf.CeilToInt((position.X - currentMap._initialPosition.X)/currentMap._chunkSize)-1;
        int heightChunk = Mathf.CeilToInt((position.Y - currentMap._initialPosition.Y) / currentMap._chunkSize)-1;
        if (_indexWidth == widthChunk && heightChunk == _indexHeight)
        {
            return true;
        }
        else 
        { 
            return false;
        }
    }

    // public SpriteRenderer GetTileSpriteRenderer()
    // {
    //     return _tileSpriteRenderer;
    // }
    
    // public HighlightTile GetTileHighlight()
    // {
    //     return _highlightTile;
    // }

    // public void SetCurrentCharacter(GameObject gameObject, PlayerInformation playerInformation)
    // {
    //     _currentCharacter = gameObject;
    //     _currentPlayerInformation = playerInformation;
    //     if (playerInformation != null)
    //     {
    //         _type = playerInformation.GetInformationType(); 
    //     }
    //     else
    //     {
    //         _type = InformationType.None;
    //     }
    //
    //     
    // }

    public InformationType GetInformationType()
    {
        return _type;
    }

    // public void SetCurrentCharacter(GameObject gameObject)
    // {
    //     _currentCharacter = gameObject;
    // }

    public Node GetCurrentCharacter()
    {
        return _currentCharacter;
    }

    public bool CharacterIsOnTile()
    {
        return _currentCharacter != null;
    }

    // public PlayerInformation GetCurrentPlayerInformation()
    // {
    //     return _currentPlayerInformation;
    // }

    public void StandingOnChunk(bool standingOnChunk)
    {
        _standingOnChunk = standingOnChunk;
    }
    

    public bool TileIsLocked()
    {
        return _tileIsLocked;
    }

    public void SetTileIsLocked(bool tileIsLocked)
    {
        _tileIsLocked = tileIsLocked;
    }

    public bool CanUseTile()
    {
        return _canUseTile;
    }

    public bool IsStandingOnChunk()
    {
        return _standingOnChunk;
    }

    public bool WeightWasUpdated()
    {
        return _weightUpdated;
    }

    public void InsertData()
    {
        _dataWasInserted = true;
    }

    public bool DataWasInserted()
    {
        return _dataWasInserted;
    }

    public float GetWeight()
    {
        return _weight;
    }
    
    public int GetGeneratedIndex()
    {
        return _indexWidth+(_indexHeight * 19);
    }

    public (int,int) GetIndexes()
    {
        return (_indexHeight, _indexWidth);
    }

    public void SetHeapIndex(int index)
    {
        _heapIndex = index;
    }

    public int GetHeapIndex()
    {
        return _heapIndex;
    }
}
