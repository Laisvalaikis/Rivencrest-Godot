using System;
using System.Collections;
using Godot;

public partial class MapSetup : Node
{
	private string MapName;
	[Export] 
	private MapDataController _mapDataController;
	[Export]
	private Node mapHolder;
	[Export]
	public GameTileMap gameTileMap;
	private Node toFollow;
	// [Export]
	// private CameraController cameraController;
	[Export]
	private PlayerTeams playerTeams;
	// [Export]
	// private AIManager aiManager;=
	private MapData currentMapData;
	private Data _data;

	public override void _Ready()
	{
		base._Ready();
		if (_data == null)
		{
			_data = Data.Instance;
			MapName = _data.townData.selectedMission;
			GD.Print(MapName);
			SetupAMap();
		}
	}

	public void SetupAMap() 
	{
	
		if (_mapDataController.mapDatas.ContainsKey(MapName))
		{
			currentMapData = _mapDataController.mapDatas[MapName];
			//coordinates
			for (int i = 0; i < playerTeams.allCharacterList.Count && !playerTeams.allCharacterList[i].isTeamAI; i++)
			{
			    playerTeams.allCharacterList[i].coordinates.Clear();
			    for (int j = 0; j < currentMapData.mapCoordinates[i].coordinates.Count; j++)
			    {
			        playerTeams.allCharacterList[i].coordinates.Add(currentMapData.mapCoordinates[i].coordinates[j]);
			    }
			}
			//NPC team spawning
			// if (mapInfo.npcTeam.Count == 0)
			// {
			//     playerTeams.allCharacterList.teams.RemoveAt(2);
			// }
	
			//AI destinations
			// aiManager.AIDestinations = mapInfo.aiMapCoordinates.coordinates;
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
			nodePath = _mapDataController.mapDatas[MapName].mapPrefab.ToString();
		}
		return nodePath;
	}
}
