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

	public TownDataResource()
	{
		
	}

	public TownDataResource(TownDataResource data)
	{
		teamColor = data.teamColor;
		slotName = data.slotName;
		difficultyLevel = data.difficultyLevel;
		townGold = data.townGold;
		day = data.day;
		wasLastMissionSuccessful = data.wasLastMissionSuccessful;
		characters = new Array<SavableCharacterResource>(data.characters);
		charactersOnLastMission = new Array<int>(data.charactersOnLastMission);
		wereCharactersOnAMission = data.charactersOnLastMission.Count > 0;
		newGame = data.newGame;
		singlePlayer = data.singlePlayer;
		selectedMission = data.selectedMission;
		townHall = data.townHall;
		if(data.rcCharacters != null)
			rcCharacters = new Array<SavableCharacterResource>(data.rcCharacters);
		createNewRCcharacters = data.rcCharacters == null;
		enemies = new Array<int>(data.enemies);
		allowEnemySelection = data.allowEnemySelection;
		allowDuplicates = data.allowDuplicates;
		teamColor = data.teamColor;
		selectedEncounter = data.selectedEncounter;
		pastEncounters = data.pastEncounters;
		generateNewEncounters = data.generateNewEncounters;
		generatedEncounters = data.generatedEncounters;
		gameSettings = data.gameSettings;
	}

	public TownDataResource(TownData data)
	{
		teamColor = data.teamColor;
		slotName = data.slotName;
		difficultyLevel = data.difficultyLevel;
		townGold = data.townGold;
		day = data.day;
		wasLastMissionSuccessful = data.wasLastMissionSuccessful;
		characters = new Array<SavableCharacterResource>();
		if (data.characters != null)
		{
			for (int i = 0; i < data.characters.Count; i++)
			{
				int characterIndex = data.characters[i].characterIndex;
				characters.Add(new SavableCharacterResource(data.characters[i], Data.Instance.Characters[characterIndex].playerInformation));
			}
		}

		if (data.charactersOnLastMission != null)
		{
			charactersOnLastMission = new Array<int>(data.charactersOnLastMission);
		}
		else
		{
			charactersOnLastMission = new Array<int>();
		}

		if (data.charactersOnLastMission != null)
		{
			wereCharactersOnAMission = data.charactersOnLastMission.Count > 0;
		}
		else
		{
			wereCharactersOnAMission = false;
		}

		newGame = data.newGame;
		singlePlayer = data.singlePlayer;
		selectedMission = data.selectedMission;
		townHall = new TownHallDataResource(data.townHall);
		if (data.rcCharacters != null)
		{
			rcCharacters = new Array<SavableCharacterResource>();
			for (int i = 0; i < data.rcCharacters.Count; i++)
			{
				int characterIndex = data.rcCharacters[i].characterIndex;
				rcCharacters.Add(new SavableCharacterResource(data.rcCharacters[i], Data.Instance.Characters[characterIndex].playerInformation));
			}
		}
		createNewRCcharacters = data.rcCharacters == null;
		
		if (data.enemies != null)
		{
			enemies = new Array<int>(data.enemies);
		}
		else
		{
			enemies = new Array<int>();
		}

		allowEnemySelection = data.allowEnemySelection;
		allowDuplicates = data.allowDuplicates;
		teamColor = data.teamColor;
		selectedEncounter = new EncounterResource(data.selectedEncounter);
		pastEncounters = new Array<EncounterResource>();
		for (int i = 0; i < data.pastEncounters.Count; i++)
		{
			pastEncounters.Add(new EncounterResource(data.pastEncounters[i]));
		}
		generateNewEncounters = data.generateNewEncounters;
		generatedEncounters = new Array<EncounterResource>();
		for (int i = 0; i < data.generatedEncounters.Count; i++)
		{
			generatedEncounters.Add(new EncounterResource(data.generatedEncounters[i]));
		}
		gameSettings = new GameSettingsResource(data.gameSettings);
	}

}
