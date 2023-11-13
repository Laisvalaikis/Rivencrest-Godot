using System;
using Godot;
using Godot.Collections;

public partial class CharacterTeams : Node
{
	[Export]
	TurnManager _turnManager;
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
					allCharacterList[0].characterPrefabs = new Dictionary<int, Resource>();
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
				allCharacterList[0].characterPrefabs = new Dictionary<int, Resource>();
			}

			allCharacterList[0].characterPrefabs.Clear();
		}

		int index = 0;
		foreach (var t in _data.Characters)
		{
			allCharacterList[0].characterPrefabs.Add(index, t.prefab);
			index++;
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
				enemyTeam.characterPrefabs.Add(i, enemyData.enemies[index]);
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
			deadCharacters.Teams[i].characters = new Dictionary<int, Player>();
			deadCharacters.Teams[i].characterPrefabs = new Dictionary<int, Resource>();
			deadCharacters.Teams[i].coordinates = new Array<Vector2>();
			currentCharacters.Teams.Add(new Team());
			currentCharacters.Teams[i].characters = new Dictionary<int, Player>();
			currentCharacters.Teams[i].characterPrefabs = new Dictionary<int, Resource>();
			currentCharacters.Teams[i].coordinates = new Array<Vector2>();
			SpawnCharacters(i, allCharacterList[i].coordinates);
		}
		_turnManager.SetTeamList(currentCharacters);
		_turnManager.SetCurrentTeam(0);
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
				currentCharacters.Teams[teamIndex].characters.Add(i, spawnedCharacter);
				currentCharacters.Teams[teamIndex].characterPrefabs.Add(i, allCharacterList[teamIndex].characterPrefabs[i]);
				currentCharacters.Teams[teamIndex].coordinates.Add(coordinate);
				currentCharacters.Teams[teamIndex].isTeamAI = allCharacterList[teamIndex].isTeamAI;
				currentCharacters.Teams[teamIndex].teamName = allCharacterList[teamIndex].teamName;
				spawnedCharacter.actionManager.AddTurnManager(_turnManager);
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

	public Dictionary<int, Player> AliveCharacterList(int teamIndex)
	{
		return currentCharacters.Teams[teamIndex].characters;
	}

	public void AddAliveCharacter(int teamIndex, Player character, Resource characterPrefab)
	{
		int index = currentCharacters.Teams[teamIndex].characters.Count - 1;
		currentCharacters.Teams[teamIndex].characters.Add(index, character);
		character.playerIndex = index;
		currentCharacters.Teams[teamIndex].characterPrefabs.Add(index, characterPrefab);
		character.SetPlayerTeam(teamIndex);
	}

	public void CharacterDeath(ChunkData chunkData, int teamIndex, int characterIndex, Player character)
	{
		int index = deadCharacters.Teams[teamIndex].characterPrefabs.Count;
		deadCharacters.Teams[teamIndex].characters.Add(index, character);
		Resource playerPrefab = currentCharacters.Teams[teamIndex].characterPrefabs[characterIndex];
		deadCharacters.Teams[teamIndex].characterPrefabs.Add(index, playerPrefab);
		deadCharacters.Teams[teamIndex].coordinates.Add(chunkData.GetPosition());
		
		currentCharacters.Teams[teamIndex].characters.Remove(characterIndex);
		currentCharacters.Teams[teamIndex].coordinates.RemoveAt(characterIndex);
		currentCharacters.Teams[teamIndex].characterPrefabs.Remove(characterIndex);
		
		chunkData.SetCurrentCharacter(null);
		chunkData.GetTileHighlight().DisableHighlight();
		portraitTeamBox.ModifyList();
	}
	
}


