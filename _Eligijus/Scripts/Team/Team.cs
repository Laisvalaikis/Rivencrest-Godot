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
        if (_visionTiles is null)
        {
            _visionTiles = new List<FogData>();
        }
        bool top = GameTileMap.Tilemap.CheckFogTopChunkBasedOnPrevious(chunkData);
        bool bottom = GameTileMap.Tilemap.CheckFogBottomChunkBasedOnPrevious(chunkData);
        bool right = GameTileMap.Tilemap.CheckFogRightChunkBasedOnPrevious(chunkData);
        bool left = GameTileMap.Tilemap.CheckFogLeftChunkBasedOnPrevious(chunkData);
        // if (twoTimmes < 3)
        // {
            List<ChunkData> chunksAround = GameTileMap.Tilemap.GetAllChunksAround(chunkData);
            for (int i = 0; i < chunksAround.Count; i++)
            {
                // GD.Print("SSSSS");
                FogData tempFog = new FogData(chunksAround[i]);
                if (chunksAround[i] != null && _visionTiles.Contains(tempFog))
                {
                    GD.PrintErr("SSSSS");
                    bool tempTop = GameTileMap.Tilemap.CheckFogTopChunkBasedOnPrevious(chunksAround[i]);
                    bool tempBottom = GameTileMap.Tilemap.CheckFogBottomChunkBasedOnPrevious(chunksAround[i]);
                    bool tempRight = GameTileMap.Tilemap.CheckFogRightChunkBasedOnPrevious(chunksAround[i]);
                    bool tempLeft = GameTileMap.Tilemap.CheckFogLeftChunkBasedOnPrevious(chunksAround[i]);
                    FogSidesData fogSidesData = new FogSidesData(tempTop, tempBottom, tempRight, tempLeft);
                    for (int j = 0; j < _visionTiles.Count; j++)
                    {
                        if (_visionTiles[j].chunkRef == chunksAround[i])
                        {
                            _visionTiles[j].fogSidesData = fogSidesData;
                            break;
                        }
                    }
                    GD.PrintErr(fogSidesData.GenerateFogSideData());
                }
            }
            _visionTiles.Add(new FogData(chunkData, top, bottom, right, left));
            fogOfWar.RemoveFog(_visionTiles); // jis neupdatina sale esanciu
            twoTimmes++;
        // }
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