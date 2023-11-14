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
	private GameEnd gameEnd;
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
				if (allCharacterList[0].characterResources == null)
				{
					allCharacterList[0].characterResources = new Dictionary<int, SavedCharacterResource>();
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
				allCharacterList[0].characterResources = new Dictionary<int, SavedCharacterResource>();
			}

			allCharacterList[0].characterPrefabs.Clear();
			allCharacterList[0].characterResources.Clear();
		}

		// allCharacterList[0].characterResources = new Dictionary<int, SavableCharacterResource>();
		int index = 0;
		foreach (var t in _data.Characters)
		{
			allCharacterList[0].characterPrefabs.Add(index, t.prefab);
			allCharacterList[0].characterResources.Add(index, t);
			index++;
		}
		
		allCharacterList[0].SetTeamIsUsed(true);
		allCharacterList[0].teamName = _data.townData.teamName;
		allCharacterList[0].teamColor = Color.FromString(_data.townData.teamColor, Colors.Blue);
		
		currentCharacters = new TeamsList { Teams = new Dictionary<int, Team>()};
		deadCharacters = new TeamsList { Teams = new Dictionary<int, Team>() };
	}

	private void GenerateEnemies()
	{
		MapEnemyData enemyData = _data.GetCurrentMapEnemyData();
		Team enemyTeam = GetAvailableTeam();
		if (enemyData != null && enemyTeam != null)
		{
			if (enemyTeam.characterResources != null)
			{
				enemyTeam.characterResources.Clear();
			}
			else
			{
				enemyTeam.characterResources = new Dictionary<int, SavedCharacterResource>();
			}
			
			for (int i = 0; i < enemyData.enemyCount; i++)
			{
				int index = _random.Next(0, enemyData.enemyResource.Count);
				enemyTeam.characterPrefabs.Add(i, enemyData.enemyResource[index].prefab);
				enemyTeam.characterResources.Add(i, enemyData.enemyResource[index]);
				enemyTeam.SetTeamIsUsed(true);
			}
		}
		enemyTeam.teamName = "Enemies";
		enemyTeam.isEnemies = true;
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
			deadCharacters.Teams.Add(i, new Team());
			deadCharacters.Teams[i].characters = new Dictionary<int, Player>();
			deadCharacters.Teams[i].characterPrefabs = new Dictionary<int, Resource>();
			deadCharacters.Teams[i].coordinates = new Dictionary<int, Vector2>();
			deadCharacters.Teams[i].characterResources = new Dictionary<int, SavedCharacterResource>();
			currentCharacters.Teams.Add(i, new Team());
			currentCharacters.Teams[i].characters = new Dictionary<int, Player>();
			currentCharacters.Teams[i].characterPrefabs = new Dictionary<int, Resource>();
			currentCharacters.Teams[i].coordinates = new Dictionary<int, Vector2>();
			currentCharacters.Teams[i].characterResources = new Dictionary<int, SavedCharacterResource>();
			SpawnCharacters(i, allCharacterList[i].coordinates);
		}
		_turnManager.SetTeamList(currentCharacters);
		_turnManager.SetCurrentTeam(0);
	}
	private void SpawnCharacters(int teamIndex, Dictionary<int, Vector2> coordinates)
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
				Vector2 position = new Vector2(coordinate.Value.X, coordinate.Value.Y);
				spawnedCharacter.GlobalPosition = position;
				spawnedCharacter.playerInTeamIndex = i;
				spawnedCharacter.SetPlayerTeam(this);
				spawnedCharacter.SetPlayerTeam(teamIndex);
				GameTileMap.Tilemap.SetCharacter(position, spawnedCharacter);
				currentCharacters.Teams[teamIndex].characters.Add(i, spawnedCharacter);
				currentCharacters.Teams[teamIndex].characterPrefabs.Add(i, allCharacterList[teamIndex].characterPrefabs[i]);
				currentCharacters.Teams[teamIndex].coordinates.Add(i, coordinate.Value);
				if (allCharacterList[teamIndex].characterResources != null && allCharacterList[teamIndex].characterResources.Count != 0)
				{
					currentCharacters.Teams[teamIndex].characterResources
						.Add(i, allCharacterList[teamIndex].characterResources[i]);
					spawnedCharacter.unlockedAbilityList =
						allCharacterList[teamIndex].characterResources[i].unlockedAbilities;
				}

				currentCharacters.Teams[teamIndex].isTeamAI = allCharacterList[teamIndex].isTeamAI;
				currentCharacters.Teams[teamIndex].isEnemies = allCharacterList[teamIndex].isEnemies;
				currentCharacters.Teams[teamIndex].teamColor = allCharacterList[teamIndex].teamColor;
				currentCharacters.Teams[teamIndex].teamName = allCharacterList[teamIndex].teamName;
				deadCharacters.Teams[teamIndex].isEnemies = allCharacterList[teamIndex].isEnemies;
				deadCharacters.Teams[teamIndex].isTeamAI = allCharacterList[teamIndex].isTeamAI;
				spawnedCharacter.actionManager.AddTurnManager(_turnManager);
			}
			i++;
		}
		if (allCharacterList[teamIndex].isEnemies)
		{
			currentCharacters.enemyTeamCount++;
		}
		else
		{
			currentCharacters.characterTeamCount++;
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
		int index = currentCharacters.Teams[teamIndex].characters.Count;
		currentCharacters.Teams[teamIndex].characters.Add(index, character);
		character.playerInTeamIndex = index;
		currentCharacters.Teams[teamIndex].characterPrefabs.Add(index, characterPrefab);
		character.SetPlayerTeam(teamIndex);
	}

	public void CharacterDeath(ChunkData chunkData, int teamIndex, int characterIndex, Player character)
	{
		int index = deadCharacters.Teams[teamIndex].characterPrefabs.Count;
		deadCharacters.Teams[teamIndex].characters.Add(index, character);
		Resource playerPrefab = currentCharacters.Teams[teamIndex].characterPrefabs[characterIndex];
		deadCharacters.Teams[teamIndex].characterPrefabs.Add(index, playerPrefab);
		deadCharacters.Teams[teamIndex].coordinates.Add(index, chunkData.GetPosition());
		if (currentCharacters.Teams[teamIndex].characterResources.ContainsKey(characterIndex))
		{
			deadCharacters.Teams[teamIndex].characterResources.Add(index, currentCharacters.Teams[teamIndex].characterResources[characterIndex]);
		}
		currentCharacters.Teams[teamIndex].characters.Remove(characterIndex);
		currentCharacters.Teams[teamIndex].coordinates.Remove(characterIndex);
		currentCharacters.Teams[teamIndex].characterPrefabs.Remove(characterIndex);
		currentCharacters.Teams[teamIndex].characterResources.Remove(characterIndex);
		if (currentCharacters.Teams[teamIndex].characters.Count <= 0)
		{
			Team diedTeam = currentCharacters.Teams[teamIndex];
			currentCharacters.Teams.Remove(teamIndex);
			if (diedTeam.isEnemies)
			{
				currentCharacters.enemyTeamCount--;
			}
			else
			{
				currentCharacters.characterTeamCount--;
			}
			if (currentCharacters.characterTeamCount == 0 || currentCharacters.enemyTeamCount == 0)
			{
				TeamDied();
			}
		}

		chunkData.SetCurrentCharacter(null);
		chunkData.GetTileHighlight().DisableHighlight();
		portraitTeamBox.ModifyList();
	}

	public void TeamDied()
	{
		if (currentCharacters.enemyTeamCount == 0)
		{
			foreach (int key in currentCharacters.Teams.Keys)
			{
				gameEnd.Win(deadCharacters, currentCharacters, currentCharacters.Teams[key].teamName, currentCharacters.Teams[key].teamColor);
				break;
			}
		}
		else
		{
			gameEnd.Death(deadCharacters, currentCharacters);
		}
	}

}


