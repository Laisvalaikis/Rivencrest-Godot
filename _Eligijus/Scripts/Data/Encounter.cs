using System.Collections;
using System.Collections.Generic;
using Godot;

[System.Serializable]
public class Encounter
{
	public string mapName { get; set; }
	public string missionCategory { get; set; }
	public int encounterLevel { get; set; }
	public bool allowDuplicates { get; set; }
	public int numOfEnemies { get; set; }
	public string missionText { get; set; }
	//int blessing chance
	//List<GameObject> EnemyPool;
	//int XPReward;
	//string Objective;

	public Encounter()
	{
		mapName = "";
		missionCategory = "";
		encounterLevel = 0;
		allowDuplicates = false;
		numOfEnemies = 0;
	}

	public Encounter(EncounterResource data)
	{
		mapName = data.mapName;
		missionCategory = data.missionCategory;
		encounterLevel = data.encounterLevel;
		allowDuplicates = data.allowDuplicates;
		// numOfEnemies = data.numOfEnemies;
		missionText = data.missionText;
	}
	
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
