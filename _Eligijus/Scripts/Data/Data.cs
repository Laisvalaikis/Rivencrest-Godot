using System;
using System.Collections;
using System.Collections.Generic;
using Godot;

public partial class Data : Node
{
	public static Data Instance { get; private set; }
	
	[Export]
	public List<SavedCharacter> Characters;
	[Export]
	public List<SavedCharacter> AllAvailableCharacters;
	[Export]
	public List<Node> AllEnemyCharacterPrefabs;
	[Export]
	public List<SavedCharacter> AllEnemySavedCharacters;
	
	public List<int> CharactersOnLastMission;
	[Export]
	public TownData newGameData;
	public bool canButtonsBeClicked = true;
	[Export]
	public bool canButtonsBeClickedState = true;
	[Export]
	public List<int> XPToLevelUp;
	[Export]
	public bool isCurrentScenePlayableMap = false;
	public bool switchPortraits;
	public List<int> SwitchedCharacters;
	public int currentCharacterIndex = -1;
	[Export]
	public int maxCharacterCount;
	[Export]
	public int minCharacterCount;
	public bool createNewRCcharacters = false;
	[Export]
	public List<int> selectedEnemies;
	public Statistics statistics;
	public Statistics globalStatistics;
	[Export]
	public TownData townData;
	[Signal]
	public delegate void CharacterRecruitmentEventHandler();
	// Start is called before the first frame update

	public override void _Ready()
	{
		base._Ready();
		if (Instance == null)
		{
			Instance = this;
		}
	}

	public void InsertCharacter(SavedCharacter character)
	{
		Characters.Add(character);
		EmitSignal("CharacterRecruitment");
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (this == Instance)
		{
			Instance = null;
		}
	}

}
