using System.Collections.Generic;
using System;
using Godot;


[Serializable]
	public class TownData
	{
		//Slot info
		public string teamColor { get; set; }
		public string teamName { get; set; }
		//Town info
		public int difficultyLevel { get; set; }
		public int townGold { get; set; }
		public int day { get; set; }
		public bool newGame { get; set; }
		public List<SavableCharacter> characters { get; set; }
		public List<SavableCharacter> deadCharacters { get; set; }
		// public string townHall;
		public TownHallData townHall { get; set; }
		public List<SavableCharacter> rcCharacters { get; set; }
		public bool createNewRCcharacters { get; set; }
		//Mission/Encounter info
		public List<int> charactersOnLastMission { get; set; }
		public bool wasLastMissionSuccessful { get; set; }
		public bool wereCharactersOnAMission { get; set; }
		public string selectedMission { get; set; }
		public List<int> enemies { get; set; }
		public bool allowEnemySelection { get; set; }
		public bool allowDuplicates { get; set; }
		public Encounter selectedEncounter { get; set; }
		public List<Encounter> pastEncounters { get; set; }
		public bool generateNewEncounters { get; set; }
		public List<Encounter> generatedEncounters { get; set; }
		public GameSettings gameSettings { get; set; }

		public TownData() { }

		public TownData(int difficultyLevel, int townGold, int day, List<SavedCharacter> characters, List<SavedCharacter> deadCharacters, List<int> charactersOnLastMission,
			bool wasLastMissionSuccessful, bool newGame, string selectedMission, TownHallData townHall,
			List<SavedCharacter> rcCharacters, List<int> enemies, bool allowEnemySelection, bool allowDuplicates, string teamColor,
			string teamName, Encounter selectedEncounter, List<Encounter> pastEncounters, bool generateNewEncounters, List<Encounter> generatedEncounters,
			GameSettings gameSettings)
		{
			this.difficultyLevel = difficultyLevel;
			this.townGold = townGold;
			this.day = day;
			this.wasLastMissionSuccessful = wasLastMissionSuccessful;
			this.characters = new List<SavableCharacter>(characters);
			this.deadCharacters = new List<SavableCharacter>(deadCharacters);
			this.charactersOnLastMission = new List<int>(charactersOnLastMission);
			this.wereCharactersOnAMission = charactersOnLastMission.Count > 0;
			this.newGame = newGame;
			this.selectedMission = selectedMission;
			this.townHall = townHall;
			if(rcCharacters != null)
				this.rcCharacters = new List<SavableCharacter>(rcCharacters);
			createNewRCcharacters = rcCharacters == null;
			this.enemies = new List<int>(enemies);
			this.allowEnemySelection = allowEnemySelection;
			this.allowDuplicates = allowDuplicates;
			this.teamColor = teamColor;
			this.teamName = teamName;
			this.selectedEncounter = selectedEncounter;
			this.pastEncounters = pastEncounters;
			this.generateNewEncounters = generateNewEncounters;
			this.generatedEncounters = generatedEncounters;
			this.gameSettings = gameSettings;
		}
		

		public TownData(TownDataResource data)
		{
			teamColor = data.teamColor;
			teamName = data.teamName;
			difficultyLevel = data.difficultyLevel;
			townGold = data.townGold;
			day = data.day;
			wasLastMissionSuccessful = data.wasLastMissionSuccessful;
			characters = new List<SavableCharacter>();
			deadCharacters = new List<SavableCharacter>();
			for (int i = 0; i < data.characters.Count; i++)
			{
				characters.Add(new SavableCharacter(data.characters[i], null));
			}
			for (int i = 0; i < data.deadCharacters.Count; i++)
			{
				deadCharacters.Add(new SavableCharacter(data.deadCharacters[i], null));
			}
			charactersOnLastMission = new List<int>(data.charactersOnLastMission);
			wereCharactersOnAMission = data.charactersOnLastMission.Count > 0;
			newGame = data.newGame;
			selectedMission = data.selectedMission;
			townHall = new TownHallData(data.townHall);
			if (data.rcCharacters != null)
			{
				rcCharacters = new List<SavableCharacter>();
				for (int i = 0; i < data.rcCharacters.Count; i++)
				{
					rcCharacters.Add(new SavableCharacter(data.rcCharacters[i], null));
				}
			}
			createNewRCcharacters = data.rcCharacters == null;
			enemies = new List<int>(data.enemies);
			allowEnemySelection = data.allowEnemySelection;
			allowDuplicates = data.allowDuplicates;
			teamColor = data.teamColor;
			if (data.selectedEncounter != null)
			{
				selectedEncounter = new Encounter(data.selectedEncounter);
			}
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

		public static TownData NewGameData(string color, int difficulty, string teamName)
		{
			return new TownData
			{
				difficultyLevel = difficulty,
				townGold = 3500,
				day = 1,
				characters = { },
				deadCharacters = { },
				charactersOnLastMission = { },
				wasLastMissionSuccessful = false,
				wereCharactersOnAMission = false,
				newGame = true,
				selectedMission = "",
				townHall = new TownHallData(), // Sita reikes perdaryti
				rcCharacters = { },
				createNewRCcharacters = false,
				enemies = { },
				allowEnemySelection = false,
				allowDuplicates = false,
				teamColor = color,
				teamName = teamName,
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
		public int level { get; set; }
		public int xP { get; set; }
		public int xPToGain { get; set; }
		public string characterName { get; set; }
		public bool dead { get; set; }
		public int abilityPointCount { get; set; }
		public List<UnlockedAbilities> unlockedAbilities { get; set; }
		public List<UnlockedBlessings> abilityBlessings;
		public int toConfirmAbilities { get; set; }
		public List<UnlockedBlessings> characterBlessings { get; set; }
		public int cost { get; set; }
		public int characterIndex { get; set; }
		public PlayerInformationData playerInformation { get; set; }

		public SavableCharacter() { }

		public SavableCharacter(int level, int xP, int xPToGain, bool dead, string characterName,
			int abilityPointCount, List<UnlockedAbilities> unlockedAbilities, int toConfirmAbilities, List<UnlockedBlessings> characterBlessings,List<UnlockedBlessings> abilityBlessings, int characterIndex, PlayerInformationData playerInformation)
		{
			this.level = level;
			this.xP = xP;
			this.xPToGain = xPToGain;
			this.dead = dead;
			this.characterName = characterName;
			this.abilityPointCount = abilityPointCount;
			this.unlockedAbilities = unlockedAbilities;
			this.toConfirmAbilities = toConfirmAbilities;
			this.abilityBlessings = new List<UnlockedBlessings>(abilityBlessings);
			this.characterBlessings = new List<UnlockedBlessings>(characterBlessings);
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
			abilityBlessings = new List<UnlockedBlessings>();
			for (int i = 0; i < data.abilityBlessings.Count; i++)
			{
				this.abilityBlessings.Add(new UnlockedBlessings(data.abilityBlessings[i]));
			}
			this.characterBlessings = new List<UnlockedBlessings>();
			for (int i = 0; i < data.characterBlessings.Count; i++)
			{
				this.characterBlessings.Add(new UnlockedBlessings(data.characterBlessings[i]));
			}
			this.cost = data.cost;
			this.characterIndex = data.characterIndex;
			this.playerInformation = data.playerInformation;
		}
		
		public SavableCharacter(SavableCharacterResource data, PlayerInformationData playerInformation)
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
			this.abilityBlessings = new List<UnlockedBlessings>();
			for (int i = 0; i < data.abilityBlessings.Count; i++)
			{
				this.abilityBlessings.Add(new UnlockedBlessings(data.abilityBlessings[i]));
			}
			this.characterBlessings = new List<UnlockedBlessings>();
			for (int i = 0; i < data.characterBlessings.Count; i++)
			{
				this.characterBlessings.Add(new UnlockedBlessings(data.characterBlessings[i]));
			}
			this.cost = data.cost;
			this.characterIndex = data.characterIndex;
			this.playerInformation = playerInformation;
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
			this.characterBlessings = new List<UnlockedBlessings>(data.characterBlessings);
			this.cost = data.cost;
			this.characterIndex = data.characterIndex;
			this.playerInformation = data.playerInformation;
		}
	}

	[Serializable]
	public class GameSettings
	{
		public bool attackHelper { get; set; }
		public float masterVolume { get; set; }
		public bool mute { get; set; }

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
