using System.Collections;
using System.Collections.Generic;
using Godot;

[System.Serializable]
public class Encounter
{
    public string mapName { get; set; }
    public string missionCategory { get; set; }
    public int encounterLevel { get; set; }
    public List<string> enemyPool { get; set; }
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
        enemyPool = new List<string>();
        allowDuplicates = false;
        numOfEnemies = 0;
    }

    public Encounter(EncounterResource data)
    {
        mapName = data.mapName;
        missionCategory = data.missionCategory;
        encounterLevel = data.encounterLevel;
        enemyPool = new List<string>(data.enemyPool);
        allowDuplicates = data.allowDuplicates;
        numOfEnemies = data.numOfEnemies;
        missionText = data.missionText;
    }

    // public static bool operator ==(Encounter a, Encounter b)
    // {
    //     return JsonUtility.ToJson(a) == JsonUtility.ToJson(b);
    // }
    //
    // public static bool operator !=(Encounter a, Encounter b)
    // {
    //     return JsonUtility.ToJson(a) != JsonUtility.ToJson(b);
    // }

    // public override bool Equals(object obj)
    // {
    //     return this == (Encounter)obj;
    // }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
