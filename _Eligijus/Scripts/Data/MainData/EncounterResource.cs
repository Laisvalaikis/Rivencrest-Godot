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
}