using System;
using System.Collections;
using System.Collections.Generic;
using Godot;

[System.Serializable]
public class ChunkData
{

    public ChunkData(int indexWidth, int indexHeight, float width, float height, float positionWidth, float positionHeight, bool tileIsLocked, HighlightTile highlightTile) //SpriteRenderer tileSpriteRenderer, HighlightTile highlightTile,  bool tileIsLocked
    {
        _indexHeight = indexHeight;
        _indexWidth = indexWidth;
        _width = width;
        _height = height;
        _positionWidth = positionWidth;
        _positionHeight = positionHeight;
        _tileIsLocked = tileIsLocked;
        _highlightTile = highlightTile;
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
    private bool _standingOnChunk = false;
    private bool _canUseTile = false;
    private bool _tileIsLocked = false;
    private ObjectType<Object> _currentPlayer;
    private ObjectType<Object> _currentObject;
    private HighlightTile _highlightTile;

    public Vector2 GetPosition()
    {
        return new(_positionWidth, _positionHeight);
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
    
    public HighlightTile GetTileHighlight()
    {
        return _highlightTile;
    }

    public void SetCurrentCharacter(Player player)
    {
        if (_currentPlayer == null)
        {
            _currentPlayer = new ObjectType<Object>(player, typeof(Player));
        }
        else
        {
            _currentPlayer.data = player;
            _currentPlayer.objectType = typeof(Player);
        }
        _currentPlayer.data = player;
    }
    
    public void SetCurrentObject(Object objectRef)
    {
        if (_currentObject == null)
        {
            _currentObject = new ObjectType<Object>(objectRef, typeof(Object));
        }
        else
        {
            _currentObject.data = objectRef;
            _currentObject.objectType = typeof(Player);
        }
    }

    public Type GetCharacterType()
    {
        if (_currentPlayer != null)
        {
            return _currentPlayer.objectType;
        }
        return null;
    }
    
    public Type GetObjectType()
    {
        if (_currentObject != null)
        {
            return _currentObject.objectType;
        }
        return null;
    }

    public bool CharacterIsOnTile()
    {
        return _currentPlayer != null && _currentPlayer.data != null;
    }
    
    public bool ObjectIsOnTile()
    {
        return _currentObject != null && _currentObject.data != null;
    }

    public Player GetCurrentPlayer()
    {
        if (_currentPlayer != null)
        {
            return _currentPlayer.GetPlayer();
        }

        return null;
    }
    
    public Object GetCurrentObject()
    {
        if (_currentObject != null)
        {
            return _currentObject.GetObject();
        }

        return null;
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

    public (int,int) GetIndexes()
    {
        return (_indexWidth, _indexHeight);
    }

}
