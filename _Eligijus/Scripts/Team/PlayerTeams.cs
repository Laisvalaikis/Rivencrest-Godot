using System.Collections;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

public partial class PlayerTeams : Node
{
	// [SerializeField] private TurnManager TurnManager;
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
	public bool isGameOver;
	private TeamsList currentCharacters;
	private TeamsList deadCharacters;
	private Data _data;
	private bool setupComplete = false;

	public override void _Process(double delta)
	{
		base._Process(delta);
		if (!setupComplete && GameTileMap.Tilemap.ChunksIsSetuped())
		{
			InitializeCharacterLists();
			SpawnAllCharacters();
			isGameOver = false;
			setupComplete = true;
		}
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
		deadCharacters = new TeamsList { Teams = new Array<Team>() };
	}
	
	private void SpawnAllCharacters()
	{
		for (int i = 0; i < allCharacterList.Count; i++)
		{ 
			deadCharacters.Teams.Add(new Team());
			currentCharacters.Teams.Add(new Team());
			currentCharacters.Teams[i].characters = new Array<Player>();
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
				Vector2 position = new Vector2(x.X, x.Y);
				spawnedCharacter.GlobalPosition = position;
				spawnedCharacter.playerIndex = i;
				spawnedCharacter.SetPlayerTeam(this);
				PlayerInformation playerInformation = spawnedCharacter.playerInformation;
				playerInformation.SetPlayerTeam(teamIndex);
				GameTileMap.Tilemap.SetCharacter(position, spawnedCharacter);
				currentCharacters.Teams[teamIndex].characters.Add(spawnedCharacter);
			}
			i++;
		}
		allCharacterList[teamIndex].undoCount = undoCount;
		portraitTeamBox.ModifyList();
		
		allCharacterList[teamIndex].lastSelectedPlayer = allCharacterList[teamIndex].characterPrefabs[0];//LastSelectedPlayer
	}
	
	public Array<Player> AliveCharacterList(int teamIndex)
	{
		return currentCharacters.Teams[teamIndex].characters;
	}

	public void AddAliveCharacter(int teamIndex, Player character)
	{
		currentCharacters.Teams[teamIndex].characters.Add(character);
	}

	// public void CharacterDeath(ChunkData chunkData, int teamIndex, Player character)
	// {
	// 	deadCharacters.Teams[teamIndex].
	// }
	
}


