using System.Collections;
using System.Collections.Generic;
using Godot;
using Godot.Collections;


public partial class PlayerTeams : Node
{
    // [SerializeField] private TurnManager TurnManager;
    // private GameProgress gameProgress;
    public TeamInformation portraitTeamBox;
    public TeamsList allCharacterList;
    public string ChampionTeam;
    public string ChampionAllegiance;
    public int undoCount = 2;
    public Array<Node2D> otherCharacters = new Array<Node2D>();
    public bool isGameOver;
    // public ButtonManager characterUiButtonManager;
    private TeamsList currentCharacters;
    // [SerializeField] private ColorManager colorManager;
    private Data _data;
    
    void Start()
    {
        InitializeCharacterLists();
        SpawnAllCharacters();
        isGameOver = false;
    }
    
    
    private void InitializeCharacterLists()
    {
        _data = Data.Instance;
        allCharacterList.Teams[0].characterPrefabs.Clear();
        foreach (var t in _data.Characters)
        {
            allCharacterList.Teams[0].characterPrefabs.Add(t.prefab);
        }
        currentCharacters = new TeamsList { Teams = new Array<Team>() };
    }
    
    private void SpawnAllCharacters()
    {
        for (int i = 0; i < allCharacterList.Teams.Count; i++)
        {  //This spawns all characters. Should be 3
            currentCharacters.Teams.Add(new Team());
            currentCharacters.Teams[i].characters = new Array<Node2D>();
            currentCharacters.Teams[i].aliveCharacters = new Array<Node2D>();
            currentCharacters.Teams[i].aliveCharactersPlayerInformation = new Array<PlayerInformation>();
            SpawnCharacters(i, allCharacterList.Teams[i].coordinates);
        }
    }
    private void SpawnCharacters(int teamIndex, Array<Vector3> coordinates)
    {
        var spawnCoordinates = coordinates;
        // colorManager.SetPortraitBoxSprites(portraitTeamBox.gameObject, allCharacterList.Teams[teamIndex].teamName);// priskiria spalvas mygtukams ir paciam portraitboxui
        int i = 0;
        foreach (var x in spawnCoordinates)
        {
            if (i < allCharacterList.Teams[teamIndex].characters.Count)
            {
                PackedScene spawnResource = (PackedScene)allCharacterList.Teams[teamIndex].characterPrefabs[i];
                Player spawnedCharacter = spawnResource.Instantiate<Player>();
                PlayerInformation playerInformation = spawnedCharacter.playerInformation;
                playerInformation.SetPlayerTeam(teamIndex);
                GameTileMap.Tilemap.SetCharacter(spawnedCharacter.GlobalPosition + new Vector2(0, 0.5f), spawnedCharacter, playerInformation);
                currentCharacters.Teams[teamIndex].characters.Add(spawnedCharacter);
                currentCharacters.Teams[teamIndex].aliveCharacters.Add(spawnedCharacter);
                currentCharacters.Teams[teamIndex].aliveCharactersPlayerInformation.Add(playerInformation);
            }
            i++;
        }
        allCharacterList.Teams[teamIndex].undoCount = undoCount;
        portraitTeamBox.ModifyList();
        
        allCharacterList.Teams[teamIndex].lastSelectedPlayer = allCharacterList.Teams[teamIndex].characters[0];//LastSelectedPlayer
    }

    // public void AddCharacterToCurrentTeam(Node2D character)
    // {
    //     Team currentTeam = TurnManager.GetCurrentTeam();
    //     int teamIndex = gameInformation.activeTeamIndex;
    //     if (currentTeam.characters.Count < 8) 
    //     {
    //         if (otherCharacters.Contains(character))
    //         {
    //             otherCharacters.Remove(character);
    //         }
    //         currentTeam.characters.Add(character);
    //         PlayerInformation playerInformation = character.GetComponent<PlayerInformation>();
    //         playerInformation.CharactersTeam = allCharacterList.Teams[teamIndex].teamName;
    //         GameObject portraitBox = allCharacterList.Teams[teamIndex].teamPortraitBoxGameObject;
    //         portraitBox.GetComponent<TeamInformation>().ModifyList();
    //         playerInformation.PlayerSetup();
    //         //todo: maybe update fog of war
    //     }
    // }
    public Array<Node2D> AliveCharacterList(int teamIndex)
    {
        return currentCharacters.Teams[teamIndex].aliveCharacters;
    }
    public Array<PlayerInformation> AliveCharacterPlayerInformationList(int teamIndex)
    {
        return currentCharacters.Teams[teamIndex].aliveCharactersPlayerInformation;
    }

    public string FindTeamAllegiance(string teamName)
    {
        for(int i = 0;i< allCharacterList.Teams.Count; i++)
        {
            if(allCharacterList.Teams[i].teamName == teamName)
            {
                return allCharacterList.Teams[i].teamAllegiance;
            }
        }
        return "";
    }
    
}
[System.Serializable]
public partial class Team: Resource
{
    [Export]
    public Array<Resource> characterPrefabs;
    [Export]
    public Array<Node2D> characters;
    [Export]
    public Array<Node2D> aliveCharacters;
    [Export]
    public Array<PlayerInformation> aliveCharactersPlayerInformation;
    [Export]
    public Array<Vector3> coordinates;
    public List<UsedAbility> usedAbilities = new List<UsedAbility>();
    [Export]
    public string teamName;
    [Export]
    public string teamAllegiance;
    [Export]
    public bool isTeamAI;
    public int undoCount;
    public Node2D teamPortraitBoxGameObject;
    public Node2D lastSelectedPlayer;

}

[System.Serializable]
public class TeamsList
{
    public Array<Team> Teams;
}
