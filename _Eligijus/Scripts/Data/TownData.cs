using System.Collections.Generic;
using System;


	[Serializable]
	public class TownData
	{
		//Slot info
		public string teamColor;
		public string slotName;
		public bool singlePlayer;//cia sita gal kazkaip istrint reiks ¯\_(ツ)_/¯
		//Town info
		public int difficultyLevel;
		public int townGold;
		public int day;
		public bool newGame;
		public List<SavableCharacter> characters;
		// public string townHall;
		public TownHallData townHall;
		public List<SavableCharacter> rcCharacters;
		public bool createNewRCcharacters;
		//Mission/Encounter info
		public List<int> charactersOnLastMission;
		public bool wasLastMissionSuccessful;
		public bool wereCharactersOnAMission;
		public string selectedMission;
		public List<int> enemies;
		public bool allowEnemySelection;
		public bool allowDuplicates;
		public Encounter selectedEncounter;
		public List<Encounter> pastEncounters;
		public bool generateNewEncounters;
		public List<Encounter> generatedEncounters;
		public GameSettings gameSettings;

		public TownData() { }

		public TownData(int difficultyLevel, int townGold, int day, List<SavedCharacter> characters, List<int> charactersOnLastMission,
			bool wasLastMissionSuccessful, bool newGame, bool singlePlayer, string selectedMission, TownHallData townHall,
			List<SavedCharacter> rcCharacters, List<int> enemies, bool allowEnemySelection, bool allowDuplicates, string teamColor,
			string slotName, Encounter selectedEncounter, List<Encounter> pastEncounters, bool generateNewEncounters, List<Encounter> generatedEncounters,
			GameSettings gameSettings)
		{
			this.difficultyLevel = difficultyLevel;
			this.townGold = townGold;
			this.day = day;
			this.wasLastMissionSuccessful = wasLastMissionSuccessful;
			this.characters = new List<SavableCharacter>(characters);
			this.charactersOnLastMission = new List<int>(charactersOnLastMission);
			this.wereCharactersOnAMission = charactersOnLastMission.Count > 0;
			this.newGame = newGame;
			this.singlePlayer = singlePlayer;
			this.selectedMission = selectedMission;
			this.townHall = townHall;
			if(rcCharacters != null)
				this.rcCharacters = new List<SavableCharacter>(rcCharacters);
			createNewRCcharacters = rcCharacters == null;
			this.enemies = new List<int>(enemies);
			this.allowEnemySelection = allowEnemySelection;
			this.allowDuplicates = allowDuplicates;
			this.teamColor = teamColor;
			this.slotName = slotName;
			this.selectedEncounter = selectedEncounter;
			this.pastEncounters = pastEncounters;
			this.generateNewEncounters = generateNewEncounters;
			this.generatedEncounters = generatedEncounters;
			this.gameSettings = gameSettings;
		}

		public TownData(TownDataResource data)
		{
			teamColor = data.teamColor;
			slotName = data.slotName;
			difficultyLevel = data.difficultyLevel;
			townGold = data.townGold;
			day = data.day;
			wasLastMissionSuccessful = data.wasLastMissionSuccessful;
			characters = new List<SavableCharacter>();
			for (int i = 0; i < data.characters.Count; i++)
			{
				characters.Add(new SavableCharacter(data.characters[i]));
			}
			charactersOnLastMission = new List<int>(data.charactersOnLastMission);
			wereCharactersOnAMission = data.charactersOnLastMission.Count > 0;
			newGame = data.newGame;
			singlePlayer = data.singlePlayer;
			selectedMission = data.selectedMission;
			townHall = new TownHallData(data.townHall);
			if (data.rcCharacters != null)
			{
				rcCharacters = new List<SavableCharacter>();
				for (int i = 0; i < data.rcCharacters.Count; i++)
				{
					rcCharacters.Add(new SavableCharacter(data.rcCharacters[i]));
				}
			}
			createNewRCcharacters = data.rcCharacters == null;
			enemies = new List<int>(data.enemies);
			allowEnemySelection = data.allowEnemySelection;
			allowDuplicates = data.allowDuplicates;
			teamColor = data.teamColor;
			selectedEncounter = new Encounter(data.selectedEncounter);
			pastEncounters = new List<Encounter>();
			for (int i = 0; i < data.pastEncounters.Count; i++)
			{
				pastEncounters.Add(new Encounter(data.pastEncounters[i]));
			}
			generateNewEncounters = data.generateNewEncounters;
			generatedEncounters = new List<Encounter>();
			for (int i = 0; i < data.generatedEncounters.Count; i++)
			{
				generatedEncounters.Add(new Encounter(data.generatedEncounters[i]));
			}
			gameSettings = new GameSettings(data.gameSettings);
		}

		public static TownData NewGameData(string color, int difficulty, string slotName)
		{
			return new TownData
			{
				difficultyLevel = difficulty,
				townGold = 3500,
				day = 1,
				characters = { },
				charactersOnLastMission = { },
				wasLastMissionSuccessful = false,
				wereCharactersOnAMission = false,
				newGame = true,
				singlePlayer = false,
				selectedMission = "",
				townHall = new TownHallData(), // Sita reikes perdaryti
				rcCharacters = { },
				createNewRCcharacters = false,
				enemies = { },
				allowEnemySelection = false,
				allowDuplicates = false,
				teamColor = color,
				slotName = slotName,
				selectedEncounter = new Encounter(),
				pastEncounters = new List<Encounter>(),
				generateNewEncounters = true,
				generatedEncounters = new List<Encounter>(),
				gameSettings = new GameSettings()
			};
		}
	}

	[Serializable]
	public class SavableCharacter
	{
		public int level;
		public int xP;
		public int xPToGain;
		public string characterName;
		public bool dead;
		public int abilityPointCount;
		public List<UnlockedAbilities> unlockedAbilities;
		public int toConfirmAbilities = 0;
		public List<Blessing> blessings;
		public int cost;
		public int characterIndex;
		public PlayerInformationData playerInformation;

		public SavableCharacter() { }

		public SavableCharacter(int level, int xP, int xPToGain, bool dead, string characterName,
			int abilityPointCount, List<UnlockedAbilities> unlockedAbilities, int toConfirmAbilities, List<Blessing> blessings, int characterIndex, PlayerInformationData playerInformation)
		{
			this.level = level;
			this.xP = xP;
			this.xPToGain = xPToGain;
			this.dead = dead;
			this.characterName = characterName;
			this.abilityPointCount = abilityPointCount;
			this.unlockedAbilities = unlockedAbilities;
			this.toConfirmAbilities = toConfirmAbilities;
			this.blessings = new List<Blessing>(blessings);
			this.cost = 1000;
			this.characterIndex = characterIndex;
			this.playerInformation = playerInformation;
		}
		
		public SavableCharacter(SavableCharacterResource data)
		{
			this.level = data.level;
			this.xP = data.xP;
			this.xPToGain = data.xPToGain;
			this.dead = data.dead;
			this.characterName = data.characterName;
			this.abilityPointCount = data.abilityPointCount;
			this.unlockedAbilities = new List<UnlockedAbilities>();
			for (int i = 0; i < data.unlockedAbilities.Count; i++)
			{
				this.unlockedAbilities.Add(new UnlockedAbilities(data.unlockedAbilities[i]));
			}
			this.toConfirmAbilities = data.toConfirmAbilities;
			this.blessings = this.blessings = new List<Blessing>(data.blessings);
			this.cost = data.cost;
			this.characterIndex = data.characterIndex;
			this.playerInformation = data.playerInformation;
		}

		public SavableCharacter(SavableCharacter data)
		{
			this.level = data.level;
			this.xP = data.xP;
			this.xPToGain = data.xPToGain;
			this.dead = data.dead;
			this.characterName = data.characterName;
			this.abilityPointCount = data.abilityPointCount;
			this.unlockedAbilities = data.unlockedAbilities;
			this.toConfirmAbilities = data.toConfirmAbilities;
			this.blessings = this.blessings = new List<Blessing>(data.blessings);
			this.cost = data.cost;
			this.characterIndex = data.characterIndex;
			this.playerInformation = data.playerInformation;
		}
	}

	[Serializable]
	public class GameSettings
	{
		public bool attackHelper;
		public float masterVolume;
		public bool mute;

		public GameSettings()
		{
			attackHelper = false;
			masterVolume = 1f;
			mute = false;
		}

		public GameSettings(GameSettingsResource data)
		{
			attackHelper = data.attackHelper;
			masterVolume = data.masterVolume;
			mute = data.mute;
		}
	}
