using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class Team : Resource
{
	[Export]
	public Godot.Collections.Dictionary<int, Resource> characterPrefabs;
	[Export]
	public Godot.Collections.Dictionary<int, SavedCharacterResource> characterResources;
	[Export]
	public Godot.Collections.Dictionary<int, Player> characters;
	[Export]
	public Godot.Collections.Dictionary<int, Vector2> coordinates;
	[Export]
	public string teamName;
	[Export]
	public bool isTeamAI;
	[Export]
	public bool isEnemies;
	[Export] 
	public Color teamColor;
	[Export] 
	private bool isTeamUsed = false;
	public FogOfWar fogOfWar;
	public int undoCount;
	private Vector2[] _fogCharacterPositions;
	private List<ChunkData> _fogChunkDatas;
	private List<Vector2> _visionTilesPositions;
	private ChunkData maxIndex;
	public LinkedList<UsedAbility> usedAbilitiesBeforeStartTurn = new LinkedList<UsedAbility>();
	public LinkedList<UsedAbility> usedAbilitiesAfterResolve = new LinkedList<UsedAbility>();
	public LinkedList<UsedAbility> usedAbilitiesEndTurn = new LinkedList<UsedAbility>();
	private int twoTimmes = 0;
	private bool turnHappend;

	public void CopyData(Team team)
	{
		characterPrefabs = new Godot.Collections.Dictionary<int, Resource>(team.characterPrefabs);
		characterResources = new Godot.Collections.Dictionary<int, SavedCharacterResource>(team.characterResources);
		characters = new Godot.Collections.Dictionary<int, Player>(team.characters);
		coordinates = new Godot.Collections.Dictionary<int, Vector2>(team.coordinates);
		teamName = team.teamName;
		isTeamAI = team.isTeamAI;
		isEnemies = team.isEnemies;
		teamColor = team.teamColor;
		isTeamUsed = team.isTeamUsed;
	}

	public bool IsTeamUsed()
	{
		return isTeamUsed;
	}

	public void SetTeamIsUsed(bool usedTeam)
	{
		isTeamUsed = usedTeam;
	}
	
	private float DistanceTo(float x, float y)
	{
		return MathF.Sqrt(MathF.Pow(x, 2) + MathF.Pow(y, 2));
	}
	
	public void AddVisionTile(ChunkData chunkData)
	{
		if (_visionTilesPositions is null)
		{
			_visionTilesPositions = new List<Vector2>();
			_fogChunkDatas = new List<ChunkData>();
			maxIndex = chunkData;
		}

		if (_fogChunkDatas.Count > 0)
		{
			float currentDistance = _fogChunkDatas[0].GetPosition().DistanceTo(chunkData.GetPosition());
			float maxDistance = _fogChunkDatas[0].GetPosition().DistanceTo(maxIndex.GetPosition());

			if (chunkData != maxIndex && currentDistance > maxDistance)
			{
				maxIndex = chunkData;
			}
		}
		
		_visionTilesPositions.Add(fogOfWar.GenerateFogPosition(chunkData.GetPosition()));
		_fogChunkDatas.Add(chunkData);
		fogOfWar.RemoveFog(fogOfWar.GenerateFogPosition(maxIndex.GetPosition()), _visionTilesPositions.ToArray(), _visionTilesPositions.Count);
	}

	public void UpdateFogTeamTile()
	{
		fogOfWar.RemoveFog(fogOfWar.GenerateFogPosition(maxIndex.GetPosition()), _visionTilesPositions.ToArray(), _visionTilesPositions.Count);
	}

	public void GenerateCharacterPositions()
	{
		int index = 0;
		_fogCharacterPositions = new Vector2[characters.Count];
		foreach (var key in characters.Keys)
		{
			_fogCharacterPositions[index] = fogOfWar.GenerateFogPosition(characters[key].GlobalPosition);
			index++;
		}
		fogOfWar.UpdateCharacterPositions(_fogCharacterPositions, index);
	}

	public void RemoveVisionTile(ChunkData chunkData)
	{
		if (_visionTilesPositions is null)
		{
			_visionTilesPositions = new List<Vector2>();
			_fogChunkDatas = new List<ChunkData>();
			maxIndex = chunkData;
		}

		if (_fogChunkDatas.Contains(chunkData))
		{
			int index = _fogChunkDatas.IndexOf(chunkData);
			if (chunkData == maxIndex)
			{
				if (index > 0)
				{
					maxIndex = _fogChunkDatas[index - 1];
				}
				else if (index == 0 && _fogChunkDatas.Count > 1)
				{
					maxIndex = _fogChunkDatas[1];
				}
			}
			_fogChunkDatas.RemoveAt(index);  
			_visionTilesPositions.RemoveAt(index);
		}
		fogOfWar.RemoveFog(fogOfWar.GenerateFogPosition(maxIndex.GetPosition()), _visionTilesPositions.ToArray(), _visionTilesPositions.Count);
	}

	public void UpdateTeamFog()
	{
		UpdateFogTeamTile();
		GenerateCharacterPositions();
	}

	private void EnableFogTiles()
	{
		for (int i = 0; i < _fogChunkDatas.Count; i++)
		{
			_fogChunkDatas[i].SetFogOnTile(false);
		}
	}

	public void SetTurnHappend(bool happend)
	{
		turnHappend = happend;
	}

	public bool ContainsVisionTile(ChunkData chunkData)
	{
		return _fogChunkDatas.Contains(chunkData);
	}
	
	public bool HasTurnHappened()
	{
		return turnHappend;
	}

	public List<ChunkData> GetVisionTiles()
	{
		if (_fogChunkDatas is null)
		{
			_fogChunkDatas = new List<ChunkData>();
		}
		return _fogChunkDatas;
	}

}
