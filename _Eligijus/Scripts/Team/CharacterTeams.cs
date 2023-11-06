using System;
using Godot;
using Godot.Collections;

public partial class CharacterTeams : Node
{
	[Export]
	TurnManager TurnManager;
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
	private Random _random;

	public override void _Ready()
	{
		base._Ready();
		_random = new Random();
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		if (!setupComplete && GameTileMap.Tilemap.ChunksIsSetuped())
		{
			InitializeCharacterLists();
			GenerateEnemies();
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
			if (allCharacterList == null)
			{
				allCharacterList = new Array<Team>();
				allCharacterList.Add(new Team());
			}
			else if (allCharacterList != null && allCharacterList.Count == 0)
			{
				allCharacterList.Add(new Team());
				allCharacterList[0].characterPrefabs = new Array<Resource>();
			}

			allCharacterList[0].characterPrefabs.Clear();
		}
		
		foreach (var t in _data.Characters)
		{
			allCharacterList[0].characterPrefabs.Add(t.prefab);
		}
		
		allCharacterList[0].SetTeamIsUsed(true);
		allCharacterList[0].teamName = _data.townData.teamName;
		
		currentCharacters = new TeamsList { Teams = new Array<Team>() };
		deadCharacters = new TeamsList { Teams = new Array<Team>() };
	}

	private void GenerateEnemies()
	{
		MapEnemyData enemyData = _data.GetCurrentMapEnemyData();
		Team enemyTeam = GetAvailableTeam();
		if (enemyData != null && enemyTeam != null)
		{
			for (int i = 0; i < enemyData.enemyCount; i++)
			{
				int index = _random.Next(0, enemyData.enemies.Count);
				enemyTeam.characterPrefabs.Add(enemyData.enemies[index]);
				enemyTeam.SetTeamIsUsed(true);
			}
		}
		// _data.allMapDatas[MapName];
	}

	private Team GetAvailableTeam()
	{
		for (int i = 0; i < allCharacterList.Count; i++)
		{
			if (!allCharacterList[i].IsTeamUsed())
			{
				return allCharacterList[i];
			}
		}

		return null;
	}

	private void SpawnAllCharacters()
	{
		for (int i = 0; i < allCharacterList.Count; i++)
		{ 
			deadCharacters.Teams.Add(new Team());
			deadCharacters.Teams[i].characters = new Array<Player>();
			deadCharacters.Teams[i].characterPrefabs = new Array<Resource>();
			deadCharacters.Teams[i].coordinates = new Array<Vector2>();
			currentCharacters.Teams.Add(new Team());
			currentCharacters.Teams[i].characters = new Array<Player>();
			currentCharacters.Teams[i].characterPrefabs = new Array<Resource>();
			currentCharacters.Teams[i].coordinates = new Array<Vector2>();
			SpawnCharacters(i, allCharacterList[i].coordinates);
		}
		TurnManager.SetTeamList(currentCharacters);
		TurnManager.SetCurrentTeam(0);
	}
	private void SpawnCharacters(int teamIndex, Array<Vector2> coordinates)
	{
		// colorManager.SetPortraitBoxSprites(portraitTeamBox.gameObject, allCharacterList.Teams[teamIndex].teamName);// priskiria spalvas mygtukams ir paciam portraitboxui
		int i = 0;
		foreach (var coordinate in coordinates)
		{
			if (i < allCharacterList[teamIndex].characterPrefabs.Count)
			{
				PackedScene spawnResource = (PackedScene)allCharacterList[teamIndex].characterPrefabs[i];
				Player spawnedCharacter = spawnResource.Instantiate<Player>();
				GetTree().Root.CallDeferred("add_child", spawnedCharacter);
				Vector2 position = new Vector2(coordinate.X, coordinate.Y);
				spawnedCharacter.GlobalPosition = position;
				spawnedCharacter.playerIndex = i;
				spawnedCharacter.SetPlayerTeam(this);
				spawnedCharacter.SetPlayerTeam(teamIndex);
				GameTileMap.Tilemap.SetCharacter(position, spawnedCharacter);
				currentCharacters.Teams[teamIndex].characters.Add(spawnedCharacter);
				currentCharacters.Teams[teamIndex].characterPrefabs.Add(allCharacterList[teamIndex].characterPrefabs[i]);
				currentCharacters.Teams[teamIndex].coordinates.Add(coordinate);
				currentCharacters.Teams[teamIndex].isTeamAI = allCharacterList[teamIndex].isTeamAI;
				currentCharacters.Teams[teamIndex].teamName = allCharacterList[teamIndex].teamName;
			}
			i++;
		}
		allCharacterList[teamIndex].undoCount = undoCount;
		portraitTeamBox.ModifyList();
	}

	public bool TeamIsAI(int teamIndex)
	{
		return currentCharacters.Teams[teamIndex].isTeamAI;
	}

	public Array<Player> AliveCharacterList(int teamIndex)
	{
		return currentCharacters.Teams[teamIndex].characters;
	}

	public void AddAliveCharacter(int teamIndex, Player character, Resource characterPrefab)
	{
		currentCharacters.Teams[teamIndex].characters.Add(character);
		currentCharacters.Teams[teamIndex].characterPrefabs.Add(characterPrefab);
		character.playerIndex = currentCharacters.Teams[teamIndex].characters.Count - 1;
		character.SetPlayerTeam(teamIndex);
	}

	public void CharacterDeath(ChunkData chunkData, int teamIndex, int characterIndex, Player character)
	{
		deadCharacters.Teams[teamIndex].characters.Add(character);
		Resource playerPrefab = currentCharacters.Teams[teamIndex].characterPrefabs[characterIndex];
		deadCharacters.Teams[teamIndex].characterPrefabs.Add(playerPrefab);
		deadCharacters.Teams[teamIndex].coordinates.Add(chunkData.GetPosition());
		
		currentCharacters.Teams[teamIndex].characters.RemoveAt(characterIndex);
		currentCharacters.Teams[teamIndex].coordinates.RemoveAt(characterIndex);
		currentCharacters.Teams[teamIndex].characterPrefabs.RemoveAt(characterIndex);
		
		chunkData.SetCurrentCharacter(null);
		chunkData.GetTileHighlight().DisableHighlight();
		portraitTeamBox.ModifyList();
	}
	
}


