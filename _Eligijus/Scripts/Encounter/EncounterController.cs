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
    [Export]
    public MapDataController mapSetup;
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
        encounter.enemyPool = new Array<string>(map.suitableEnemies);
        encounter.allowDuplicates = map.allowDuplicates;
        encounter.numOfEnemies = map.numberOfEnemies;
        
        
    }
    
    private void GenerateEncounters(out Array<EncounterResource> encounterListToPopulate)
    {
       encounterListToPopulate = new Array<EncounterResource>();
        for (int i = 3; i <= 5; i++)
        {
            foreach (string tempCategory in encounterCategories)
            {
                
                    EncounterResource newEncounter = new EncounterResource();
                    newEncounter.missionCategory = tempCategory;
                    newEncounter.encounterLevel = i;
                    Array<MapData> suitableMaps = new Array<MapData>();
                    foreach (string key in mapSetup.mapDatas.Keys)
                    {
                        if (mapSetup.mapDatas[key].mapCategory == tempCategory && mapSetup.mapDatas[key].suitableLevels.Contains(1))
                        {
                            suitableMaps.Add(mapSetup.mapDatas[key]);
                        }
                    }
                    MapData suitableMap = suitableMaps[_random.Next(0, suitableMaps.Count)];
                    newEncounter.mapName = suitableMap.mapName;
                    newEncounter.enemyPool = new Array<string>(suitableMap.suitableEnemies);
                    newEncounter.allowDuplicates = suitableMap.allowDuplicates;
                    newEncounter.numOfEnemies = suitableMap.numberOfEnemies;
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
            numOfEnemies.Text = _data.townData.selectedEncounter.numOfEnemies.ToString();
            missionInfo.Text = _data.townData.selectedEncounter.missionText;
        }
        selectedEncounter = encounter;
    }
    
}
