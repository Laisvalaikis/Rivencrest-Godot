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
	public bool allowDuplicates;
	[Export]
	public string missionText;
	
	public EncounterResource()
	{
		
	}

	public EncounterResource(EncounterResource data)
	{
		mapName = data.mapName;
		missionCategory = data.missionCategory;
		encounterLevel = data.encounterLevel;
		allowDuplicates = data.allowDuplicates;
		missionText = data.missionText;
	}

	public EncounterResource(Encounter data)
	{
		mapName = data.mapName;
		missionCategory = data.missionCategory;
		encounterLevel = data.encounterLevel;
		allowDuplicates = data.allowDuplicates;
		missionText = data.missionText;
	}

}
