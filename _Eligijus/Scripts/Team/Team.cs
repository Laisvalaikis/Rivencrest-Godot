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
    public int undoCount;
    public Image fogImage;
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