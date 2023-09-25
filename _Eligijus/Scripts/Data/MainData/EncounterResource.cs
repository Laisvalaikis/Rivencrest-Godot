using Godot;
using Godot.Collections;

public partial class EncounterResource: Resource
{
	[Export]
	public string mapName;
	[Export]
	public string missionCategory;
	[Export]
	public int encounterLevel;
	[Export]
	public Array<string> enemyPool;
	[Export]
	public bool allowDuplicates;
	[Export]
	public int numOfEnemies;
	[Export]
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
