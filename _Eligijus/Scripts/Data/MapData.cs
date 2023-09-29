using System.Collections;
using Godot;
using Godot.Collections;

public partial class MapData : Resource
{
    [Export]
    public string mapName;
    [Export]
    public Array<string> suitableEnemies;
    [Export]
    public Array<Node> npcTeam;
    
    [Export]
    public Array<int> suitableLevels;
    [Export]
    public Array<MapCoordinates> mapCoordinates;
    [Export]
    public MapCoordinates aiMapCoordinates;
    [Export]
    public Vector3 toFollowStartPosition;
    [Export]
    public Vector2 panLimitX;
    [Export]
    public Vector2 panLimitY;
    
    [Export]
    public int numberOfEnemies = 3;
    [Export]
    public string mapCategory;
    [Export]
    public bool allowDuplicates;
    [Export]
    public CompressedTexture2D mapImage;
    [Export]
    public string informationText;
    
    [Export]
    public Node mapPrefab;

    public void CopyData(MapData mapData)
    {
        
        suitableEnemies = mapData.suitableEnemies;
        npcTeam = mapData.npcTeam;
        suitableLevels = mapData.suitableLevels;
        mapCoordinates = mapData.mapCoordinates;
        aiMapCoordinates = mapData.aiMapCoordinates;
        toFollowStartPosition = mapData.toFollowStartPosition;
        panLimitX = mapData.panLimitX;
        panLimitY = mapData.panLimitY;
        
        numberOfEnemies = mapData.numberOfEnemies;
        mapCategory = mapData.mapCategory;
        allowDuplicates = mapData.allowDuplicates;
        mapImage = mapData.mapImage;
        informationText = mapData.informationText;
        mapPrefab = mapData.mapPrefab;
    }

}
