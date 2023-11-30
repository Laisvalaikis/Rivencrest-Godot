using Godot;
using Godot.Collections;

public partial class MapData : Resource
{
	[Export]
	public string mapName;
	[Export]
	public Dictionary<int, MapEnemyData> suitableLevels;
	[Export]
	public Array<Node> npcTeam;
	[Export]
	public Array<MapCoordinates> mapCoordinates;
	[Export]
	public TileMapData tileMapCoordinates;
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
	public string informationText;
	
	[Export]
	public Resource mapPrefab;

	public void CopyData(MapData mapData)
	{
		
		suitableLevels = mapData.suitableLevels;
		npcTeam = mapData.npcTeam;
		suitableLevels = mapData.suitableLevels;
		mapCoordinates = mapData.mapCoordinates;
		toFollowStartPosition = mapData.toFollowStartPosition;
		panLimitX = mapData.panLimitX;
		panLimitY = mapData.panLimitY;
		
		numberOfEnemies = mapData.numberOfEnemies;
		mapCategory = mapData.mapCategory;
		allowDuplicates = mapData.allowDuplicates;
		informationText = mapData.informationText;
		mapPrefab = mapData.mapPrefab;
	}

}
