using Godot;
using Godot.Collections;
public partial class TownDataResource: Resource
{
	[Export]
	public string teamColor;
	[Export]
	public string slotName;
	[Export]
	public bool singlePlayer;//cia sita gal kazkaip istrint reiks ¯\_(ツ)_/¯
	//Town info
	[Export]
	public int difficultyLevel;
	[Export]
	public int townGold;
	[Export]
	public int day;
	[Export]
	public bool newGame;
	[Export]
	public Array<SavableCharacterResource> characters;
	// public string townHall;
	[Export]
	public TownHallDataResource townHall;
	[Export]
	public Array<SavableCharacterResource> rcCharacters;
	[Export]
	public bool createNewRCcharacters;
	//Mission/Encounter info
	[Export]
	public Array<int> charactersOnLastMission;
	[Export]
	public bool wasLastMissionSuccessful;
	[Export]
	public bool wereCharactersOnAMission;
	[Export]
	public string selectedMission;
	[Export]
	public Array<int> enemies;
	[Export]
	public bool allowEnemySelection;
	[Export]
	public bool allowDuplicates;
	[Export]
	public EncounterResource selectedEncounter;
	[Export]
	public Array<EncounterResource> pastEncounters;
	[Export]
	public bool generateNewEncounters;
	[Export]
	public Array<EncounterResource> generatedEncounters;
	[Export]
	public GameSettingsResource gameSettings;
}
