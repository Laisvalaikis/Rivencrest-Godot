using System.Collections;
using System.Collections.Generic;
using Godot;
using Godot.Collections;


public partial class PlayerTeams : Node
{
	// [SerializeField] private TurnManager TurnManager;
	// private GameProgress gameProgress;
	[Export]
	public TeamInformation portraitTeamBox;
	[Export]
	public Array<Team> allCharacterList;
	[Export]
	public string ChampionTeam;
	[Export]
	public string ChampionAllegiance;
	[Export]
	public int undoCount = 2;
	[Export]
	public Array<Node2D> otherCharacters = new Array<Node2D>();
	[Export]
	public bool isGameOver;
	// public ButtonManager characterUiButtonManager;
	private TeamsList currentCharacters;
	// [SerializeField] private ColorManager colorManager;
	private Data _data;

	public override void _Ready()
	{
		base._Ready();
		InitializeCharacterLists();
		SpawnAllCharacters();
		isGameOver = false;
	}
	
	private void InitializeCharacterLists()
	{
		_data = Data.Instance;
		if (allCharacterList != null && allCharacterList.Count > 0)
		{
			if (allCharacterList[0] != null)
			{
				if (allCharacterList[0].characterPrefabs == null)
				{
					allCharacterList[0].characterPrefabs = new Array<Resource>();
				}
				
			}
		}
		else
		{
			allCharacterList[0].characterPrefabs.Clear();
		}

		
		foreach (var t in _data.Characters)
		{
			allCharacterList[0].characterPrefabs.Add(t.prefab);
		}
		currentCharacters = new TeamsList { Teams = new Array<Team>() };
	}
	
	private void SpawnAllCharacters()
	{
		for (int i = 0; i < allCharacterList.Count; i++)
		{  //This spawns all characters. Should be 3
			currentCharacters.Teams.Add(new Team());
			currentCharacters.Teams[i].characters = new Array<Node2D>();
			currentCharacters.Teams[i].aliveCharacters = new Array<Node2D>();
			currentCharacters.Teams[i].aliveCharactersPlayerInformation = new Array<PlayerInformation>();
			SpawnCharacters(i, allCharacterList[i].coordinates);
		}
	}
	private void SpawnCharacters(int teamIndex, Array<Vector3> coordinates)
	{
		var spawnCoordinates = coordinates;
		// colorManager.SetPortraitBoxSprites(portraitTeamBox.gameObject, allCharacterList.Teams[teamIndex].teamName);// priskiria spalvas mygtukams ir paciam portraitboxui
		int i = 0;
		foreach (var x in spawnCoordinates)
		{
			if (i < allCharacterList[teamIndex].characterPrefabs.Count)
			{
				PackedScene spawnResource = (PackedScene)allCharacterList[teamIndex].characterPrefabs[i];
				Player spawnedCharacter = spawnResource.Instantiate<Player>();
				GetTree().Root.CallDeferred("add_child", spawnedCharacter);
				spawnedCharacter.GlobalPosition = new Vector2(x.X, x.Y);
				PlayerInformation playerInformation = spawnedCharacter.playerInformation;
				playerInformation.SetPlayerTeam(teamIndex);
				GameTileMap.Tilemap.SetCharacter(spawnedCharacter.GlobalPosition + new Vector2(0, 0.5f), spawnedCharacter, playerInformation);
				currentCharacters.Teams[teamIndex].characters.Add(spawnedCharacter);
				currentCharacters.Teams[teamIndex].aliveCharacters.Add(spawnedCharacter);
				currentCharacters.Teams[teamIndex].aliveCharactersPlayerInformation.Add(playerInformation);
			}
			i++;
		}
		allCharacterList[teamIndex].undoCount = undoCount;
		portraitTeamBox.ModifyList();
		
		allCharacterList[teamIndex].lastSelectedPlayer = allCharacterList[teamIndex].characterPrefabs[0];//LastSelectedPlayer
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
		for(int i = 0;i< allCharacterList.Count; i++)
		{
			if(allCharacterList[i].teamName == teamName)
			{
				return allCharacterList[i].teamAllegiance;
			}
		}
		return "";
	}
	
}


