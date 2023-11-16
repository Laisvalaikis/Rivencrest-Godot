using Godot;
using Godot.Collections;

public partial class MapSetup : Node
{
	private string MapName;
	private Dictionary<string, MapData> mapDatas;
	[Export]
	private Node mapHolder;
	[Export]
	public GameTileMap gameTileMap;
	private Node toFollow;
	// [Export]
	// private CameraController cameraController;
	[Export]
	private CharacterTeams _characterTeams;
	private MapData currentMapData;
	private Data _data;

	public override void _EnterTree()
	{
		base._EnterTree();
		if (Data.Instance != null)
		{
			_data = Data.Instance;
			mapDatas = _data.allMapDatas;
			MapName = _data.townData.selectedMission;
			GD.Print(MapName);
			SetupAMap();
		}
	}
	
	

	public void SetupAMap() 
	{
	
		if (mapDatas.ContainsKey(MapName))
		{
			currentMapData = mapDatas[MapName];
			//coordinates
			for (int i = 0; i < _characterTeams.allCharacterList.Count; i++)
			{
				_characterTeams.allCharacterList[i].coordinates.Clear();
				for (int j = 0; j < currentMapData.mapCoordinates[i].coordinates.Count; j++)
				{
					_characterTeams.allCharacterList[i].coordinates.Add(j, currentMapData.mapCoordinates[i].coordinates[j]);
				}
			}
			
			CreateMap();
		}
		else
		{
			GD.PrintErr("Map can not be found");
		}
	}
	
	private void CreateMap()
	{
		
		PackedScene mapNode = (PackedScene)currentMapData.mapPrefab;
		mapHolder.CallDeferred("add_child", mapNode.Instantiate());
		// toFollow.transform.position = currentMapData.toFollowStartPosition;
		// cameraController.panLimitX = currentMapData.panLimitX;
		// cameraController.panLimitY = currentMapData.panLimitY;
		// cameraController.cinemachineVirtualCamera.Follow = toFollow.transform;
		
		gameTileMap.SetupTiles(currentMapData.tileMapCoordinates);
	}
	
	
	public string GetSelectedMap()
	{
		string nodePath = "";
		if (MapName != null)
		{
			nodePath = mapDatas[MapName].mapPrefab.ToString();
		}
		return nodePath;
	}
}
