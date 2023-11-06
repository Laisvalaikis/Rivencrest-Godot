using System;
using Godot;
using Godot.Collections;

public partial class EncounterController : Control
{
	[Export]
	public Array<string> encounterCategories;
	[Export]
	public Label missionName;
	[Export]
	public Label level;
	[Export]
	public Label category;
	[Export]
	public Label numOfEnemies;
	[Export]
	public Label missionInfo;
	private Dictionary<string, MapData> mapDatas;
	[Export]
	public Array<EncounterButton> encounterSelections;
	
	private Random _random;
	public EncounterResource selectedEncounter { get; private set; }
	private Data _data;

	public override void _Ready()
	{
		base._Ready();
		_random = new Random();
		_data = Data.Instance;
		mapDatas = _data.allMapDatas;
		_data.townData.generatedEncounters = Setup();
		ChangeSelectedEncounter(null);
	}

	public Array<EncounterResource> Setup()
	{
		Array<EncounterResource> generatedEncounters;
		if (_data.townData.generateNewEncounters)
		{
			GenerateEncounters(out generatedEncounters);
			_data.townData.generateNewEncounters = false;
		}
		else generatedEncounters = _data.townData.generatedEncounters;
		ToggleEncounterButtons(generatedEncounters);
		return generatedEncounters;
	}

	private void AddAttributesToEncounter(EncounterResource encounter, MapData map, string encounterCategory, int encounterLevel)
	{
		encounter.missionCategory = encounterCategory;
		encounter.encounterLevel = encounterLevel;
		encounter.mapName = map.ResourceName;
		encounter.allowDuplicates = map.allowDuplicates;
		
		
	}
	
	private void GenerateEncounters(out Array<EncounterResource> encounterListToPopulate)
	{
	   encounterListToPopulate = new Array<EncounterResource>();
	   GD.PrintErr("Need to change this place for map and map level generation");
		for (int i = 1; i <= 3; i++)
		{
			foreach (string tempCategory in encounterCategories)
			{
				
					EncounterResource newEncounter = new EncounterResource();
					newEncounter.missionCategory = tempCategory;
					newEncounter.encounterLevel = i;
					Array<MapData> suitableMaps = new Array<MapData>();
					foreach (string key in mapDatas.Keys)
					{
						if (mapDatas[key].mapCategory == tempCategory && mapDatas[key].suitableLevels.ContainsKey(i))
						{
							suitableMaps.Add(mapDatas[key]);
						}
					}
					MapData suitableMap = suitableMaps[_random.Next(0, suitableMaps.Count)];
					newEncounter.mapName = suitableMap.mapName;
					newEncounter.allowDuplicates = suitableMap.allowDuplicates;
					newEncounter.missionText = suitableMap.informationText;
					encounterListToPopulate.Add(newEncounter);
			   
			}
		}

	}
	
	private void ToggleEncounterButtons(Array<EncounterResource> generatedEncounters)
	{
		for (int i = 0; i < encounterSelections.Count; i++)
		{
			encounterSelections[i].AddEncounter(generatedEncounters[i]); 
		}
	}

	public void ChangeSelectedEncounter(EncounterResource encounter)
	{
		bool activate = encounter != _data.townData.selectedEncounter && encounter != null;
		
		_data.townData.selectedEncounter = activate ? encounter : null;
		_data.townData.selectedMission = activate ? encounter.mapName : "";
		if (activate)
		{
			missionName.Text = _data.townData.selectedEncounter.mapName;
			level.Text = _data.townData.selectedEncounter.encounterLevel.ToString();
			category.Text = _data.townData.selectedEncounter.missionCategory;
			numOfEnemies.Text = mapDatas[_data.townData.selectedEncounter.mapName]
				.suitableLevels[_data.townData.selectedEncounter.encounterLevel].enemyCount.ToString();
			missionInfo.Text = _data.townData.selectedEncounter.missionText;
		}
		selectedEncounter = encounter;
	}
	
}
