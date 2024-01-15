using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class Team : Resource
{
    [Export]
    public Godot.Collections.Dictionary<int, Resource> characterPrefabs;
    [Export]
    public Godot.Collections.Dictionary<int, SavedCharacterResource> characterResources;
    [Export]
    public Godot.Collections.Dictionary<int, Player> characters;
    [Export]
    public Godot.Collections.Dictionary<int, Vector2> coordinates;
    [Export]
    public string teamName;
    [Export]
    public bool isTeamAI;
    [Export]
    public bool isEnemies;
    [Export] 
    public Color teamColor;
    [Export] 
    private bool isTeamUsed = false;
    public FogOfWar fogOfWar;
    public int undoCount;
    private Vector2[] _fogCharacterPositions;
    private List<ChunkData> _fogChunkDatas;
    private List<Vector2> _visionTilesPositions;
    private ChunkData maxIndex;
    public LinkedList<UsedAbility> usedAbilitiesBeforeStartTurn = new LinkedList<UsedAbility>();
    public LinkedList<UsedAbility> usedAbilitiesAfterResolve = new LinkedList<UsedAbility>();
    public LinkedList<UsedAbility> usedAbilitiesEndTurn = new LinkedList<UsedAbility>();
    private int twoTimmes = 0;

    public bool IsTeamUsed()
    {
        return isTeamUsed;
    }

    public void SetTeamIsUsed(bool usedTeam)
    {
        isTeamUsed = usedTeam;
    }
    
    public void AddVisionTile(ChunkData chunkData)
    {
        if (_visionTilesPositions is null)
        {
            _visionTilesPositions = new List<Vector2>();
            _fogChunkDatas = new List<ChunkData>();
            maxIndex = chunkData;
        }

        (int x, int y) current = chunkData.GetIndexes();
        (int x, int y) max = maxIndex.GetIndexes();
        if (chunkData != maxIndex && current.x >= max.x && current.y >= max.y)
        {
            maxIndex = chunkData;
        }
        
        _visionTilesPositions.Add(fogOfWar.GenerateFogPosition(chunkData.GetPosition()));
        _fogChunkDatas.Add(chunkData);
        fogOfWar.RemoveFog(fogOfWar.GenerateFogPosition(maxIndex.GetPosition()), _visionTilesPositions.ToArray(), _visionTilesPositions.Count);
    }

    public void UpdateFogTeamTile()
    {
        fogOfWar.RemoveFog(fogOfWar.GenerateFogPosition(maxIndex.GetPosition()), _visionTilesPositions.ToArray(), _visionTilesPositions.Count);
    }

    public void GenerateCharacterPositions()
    {
        int index = 0;
        _fogCharacterPositions = new Vector2[characters.Count];
        foreach (var key in characters.Keys)
        {
            _fogCharacterPositions[index] = fogOfWar.GenerateFogPosition(characters[key].GlobalPosition);
            index++;
        }
        fogOfWar.UpdateCharacterPositions(_fogCharacterPositions, index);
    }

    public void RemoveVisionTile(ChunkData chunkData)
    {
        if (_visionTilesPositions is null)
        {
            _visionTilesPositions = new List<Vector2>();
            _fogChunkDatas = new List<ChunkData>();
            maxIndex = chunkData;
        }

        if (_fogChunkDatas.Contains(chunkData))
        {
            int index = _fogChunkDatas.IndexOf(chunkData);
            _fogChunkDatas.RemoveAt(index);  
            _visionTilesPositions.RemoveAt(index);
        }
        fogOfWar.AddFog(chunkData.GetPosition(), this);
    }

    public void UpdateTeamFog()
    {
        UpdateFogTeamTile();
        GenerateCharacterPositions();
        // EnableFogTiles();
    }

    // public void ResetFogTiles()
    // {
    //     for (int i = 0; i < _fogChunkDatas.Count; i++)
    //     {
    //         _fogChunkDatas[i].SetFogOnTile(true);
    //     }
    // }

    private void EnableFogTiles()
    {
        for (int i = 0; i < _fogChunkDatas.Count; i++)
        {
            _fogChunkDatas[i].SetFogOnTile(false);
        }
    }
    
    public bool ContainsVisionTile(ChunkData chunkData)
    {
        return _fogChunkDatas.Contains(chunkData);
    }

    public List<ChunkData> GetVisionTiles()
    {
        if (_fogChunkDatas is null)
        {
            _fogChunkDatas = new List<ChunkData>();
        }
        return _fogChunkDatas;
    }

}