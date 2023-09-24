using Godot;
using Godot.Collections;

public partial class EncounterResource: Resource
{
    public string mapName;
    public string missionCategory;
    public int encounterLevel;
    public Array<string> enemyPool;
    public bool allowDuplicates;
    public int numOfEnemies;
    public string missionText;

    public EncounterResource(EncounterResource data)
    {
        mapName = data.mapName;
        missionCategory = data.missionCategory;
        encounterLevel = data.encounterLevel;
        enemyPool = new Array<string>(data.enemyPool);
        allowDuplicates = data.allowDuplicates;
        numOfEnemies = data.numOfEnemies;
        missionText = data.missionText;
    }

    public EncounterResource(Encounter data)
    {
        mapName = data.mapName;
        missionCategory = data.missionCategory;
        encounterLevel = data.encounterLevel;
        enemyPool = new Array<string>(data.enemyPool);
        allowDuplicates = data.allowDuplicates;
        numOfEnemies = data.numOfEnemies;
        missionText = data.missionText;
    }

}