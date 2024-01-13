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
    public ImageTexture fogTexture;
    private List<FogData> _visionTiles;
    private Vector2[] _visionTilesPositions;
    private Vector2[] _fogCharacterPositions;
    private ChunkData maxIndex;
    private int arrayIndex = 0;
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
            _visionTilesPositions = new Vector2[200];
            arrayIndex = 0;
            maxIndex = chunkData;
        }

        (int x, int y) current = chunkData.GetIndexes();
        (int x, int y) max = maxIndex.GetIndexes();
        if (chunkData != maxIndex && current.x >= max.x && current.y >= max.y)
        {
            maxIndex = chunkData;
        }
        
        _visionTilesPositions[arrayIndex] = fogOfWar.GenerateFogPosition(chunkData.GetPosition());
        arrayIndex++;
        
        fogOfWar.RemoveFog(fogOfWar.GenerateFogPosition(maxIndex.GetPosition()), _visionTilesPositions, arrayIndex);
        GD.Print("STARTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT");
        for (int i = 0; i < arrayIndex; i++)
        {
            Vector2 position = _visionTilesPositions[i];
            for (int j = 0; j < _fogCharacterPositions.Length; j++)
            {
                // float starting_point = 0;
                float starting_point = _fogCharacterPositions[j].DistanceTo(_visionTilesPositions[1]);
                float current_point = _fogCharacterPositions[j].DistanceTo(position);
                float last_point = _fogCharacterPositions[j].DistanceTo(fogOfWar.GenerateFogPosition(maxIndex.GetPosition()));
                float current_position = ((current_point - starting_point) / (last_point - starting_point));
                current_position = Math.Abs(current_position);
                // current_position = Math.Clamp(current_position, 0, 1);
                // double alpha = fogDistanceGraph(current_position) * 0.7;
                GD.Print(current_position);
            }
        }
        GD.Print("ENDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD");
    }
    
    float fogDistanceGraph(float alpha)
    {
        return Mathf.Pow(alpha, (float)1.0/(float)2.0);
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
        if (_visionTiles is null)
        {
            _visionTiles = new List<FogData>();
        }

        if (_visionTiles.Contains(new FogData(chunkData)))
        {
            _visionTiles.Remove(new FogData(chunkData));
        }
        fogOfWar.AddFog(chunkData.GetPosition(), this);
    }

    public bool ContainsVisionTile(ChunkData chunkData)
    {
        FogData fogData = new FogData(chunkData);
        return _visionTiles.Contains(fogData);
    }

    public List<FogData> GetVisionTiles()
    {
        if (_visionTiles is null)
        {
            _visionTiles = new List<FogData>();
        }
        return _visionTiles;
    }

}