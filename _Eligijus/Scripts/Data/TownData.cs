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

        public SavableCharacter(SavableCharacter x)
        {
            this.level = x.level;
            this.xP = x.xP;
            this.xPToGain = x.xPToGain;
            this.dead = x.dead;
            this.characterName = x.characterName;
            this.abilityPointCount = x.abilityPointCount;
            this.unlockedAbilities = x.unlockedAbilities;
            this.toConfirmAbilities = x.toConfirmAbilities;
            this.blessings = this.blessings = new List<Blessing>(x.blessings);
            this.cost = x.cost;
            this.characterIndex = x.characterIndex;
            this.playerInformation = x.playerInformation;
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
    }
