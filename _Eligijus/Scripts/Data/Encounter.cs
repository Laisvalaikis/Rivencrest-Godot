using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Encounter
{
    public string mapName;
    public string missionCategory;
    public int encounterLevel;
    public List<string> enemyPool;
    public bool allowDuplicates;
    public int numOfEnemies;
    public string missionText;
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

    public static bool operator ==(Encounter a, Encounter b)
    {
        return JsonUtility.ToJson(a) == JsonUtility.ToJson(b);
    }

    public static bool operator !=(Encounter a, Encounter b)
    {
        return JsonUtility.ToJson(a) != JsonUtility.ToJson(b);
    }

    public override bool Equals(object obj)
    {
        return this == (Encounter)obj;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
