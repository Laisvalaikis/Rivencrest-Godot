using System.Collections.Generic;
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
    public LinkedList<UsedAbility> usedAbilitiesBeforeStartTurn = new LinkedList<UsedAbility>();
    public LinkedList<UsedAbility> usedAbilitiesAfterResolve = new LinkedList<UsedAbility>();
    public LinkedList<UsedAbility> usedAbilitiesEndTurn = new LinkedList<UsedAbility>();
    

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
        if (_visionTiles is null)
        {
            _visionTiles = new List<FogData>();
        }
        _visionTiles.Add(new FogData(chunkData));
        fogOfWar.RemoveFog(chunkData.GetPosition(), this);
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