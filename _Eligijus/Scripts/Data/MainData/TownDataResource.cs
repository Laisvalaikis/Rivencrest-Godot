using Godot;
using Godot.Collections;
public partial class TownDataResource: Resource
{
	[Export]
	public string teamColor;
	[Export]
	public string teamName;
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
	public int maxAbilityCount = 4;
	[Export]
	public Array<SavableCharacterResource> characters;
	[Export]
	public Array<SavableCharacterResource> deadCharacters;
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
	public int selectedEncounterIndex;
	[Export]
	public Array<EncounterResource> pastEncounters;
	[Export]
	public bool generateNewEncounters;
	[Export]
	public Array<EncounterResource> generatedEncounters;
	[Export]
	public Array<bool> finishedEncounters;
	[Export]
	public GameSettingsResource gameSettings;

	public TownDataResource()
	{
		
	}

	public TownDataResource(TownDataResource data)
	{
		teamColor = data.teamColor;
		teamName = data.teamName;
		difficultyLevel = data.difficultyLevel;
		townGold = data.townGold;
		day = data.day;
		wasLastMissionSuccessful = data.wasLastMissionSuccessful;
		characters = new Array<SavableCharacterResource>(data.characters);
		deadCharacters = new Array<SavableCharacterResource>(data.deadCharacters);
		charactersOnLastMission = new Array<int>(data.charactersOnLastMission);
		wereCharactersOnAMission = data.charactersOnLastMission.Count > 0;
		newGame = data.newGame;
		maxAbilityCount = data.maxAbilityCount;
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
		selectedEncounterIndex = data.selectedEncounterIndex;
		finishedEncounters = new Array<bool>(data.finishedEncounters);
		gameSettings = data.gameSettings;
	}

	public TownDataResource(TownData data)
	{
		teamColor = data.teamColor;
		teamName = data.teamName;
		difficultyLevel = data.difficultyLevel;
		townGold = data.townGold;
		day = data.day;
		wasLastMissionSuccessful = data.wasLastMissionSuccessful;
		characters = new Array<SavableCharacterResource>();
		deadCharacters = new Array<SavableCharacterResource>();
		if (data.characters != null)
		{
			for (int i = 0; i < data.characters.Count; i++)
			{
				int characterIndex = data.characters[i].characterIndex;
				characters.Add(new SavableCharacterResource(data.characters[i], Data.Instance.AllAvailableCharacters[characterIndex].playerInformation));
			}
		}
		if (data.deadCharacters != null)
		{
			for (int i = 0; i < data.deadCharacters.Count; i++)
			{
				int characterIndex = data.deadCharacters[i].characterIndex;
				deadCharacters.Add(new SavableCharacterResource(data.deadCharacters[i], Data.Instance.AllAvailableCharacters[characterIndex].playerInformation));
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
		maxAbilityCount = data.maxAbilityCount;
		selectedMission = data.selectedMission;
		townHall = new TownHallDataResource(data.townHall);
		if (data.rcCharacters != null)
		{
			rcCharacters = new Array<SavableCharacterResource>();
			for (int i = 0; i < data.rcCharacters.Count; i++)
			{
				int characterIndex = data.rcCharacters[i].characterIndex;
				rcCharacters.Add(new SavableCharacterResource(data.rcCharacters[i], Data.Instance.AllAvailableCharacters[characterIndex].playerInformation));
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
		if (data.selectedEncounter != null)
		{
			selectedEncounter = new EncounterResource(data.selectedEncounter);
		}
		pastEncounters = new Array<EncounterResource>();
		for (int i = 0; i < data.pastEncounters.Count; i++)
		{
			pastEncounters.Add(new EncounterResource(data.pastEncounters[i]));
		}
		selectedEncounterIndex = data.selectedEncounterIndex;
		
		if (data.finishedEncounters != null)
		{
			finishedEncounters = new Array<bool>(data.finishedEncounters);
		}
		else
		{
			finishedEncounters = new Array<bool>();
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
