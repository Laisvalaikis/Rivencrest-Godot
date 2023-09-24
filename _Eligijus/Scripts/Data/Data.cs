using System;
using Godot.Collections;
using Godot;
using Array = Godot.Collections.Array;

public partial class Data : Node
{
	public static Data Instance { get; private set; }
	[Export]
	public Array<SavedCharacterResource> Characters;
	[Export]
	public Array<SavedCharacterResource> AllAvailableCharacters;
	[Export]
	public Array<Node> AllEnemyCharacterPrefabs;
	[Export]
	public Array<SavedCharacterResource> AllEnemySavedCharacters;
	public Array<int> CharactersOnLastMission;
	[Export]
	public TownDataResource newGameData;
	public bool canButtonsBeClicked = true;
	[Export]
	public bool canButtonsBeClickedState = true;
	[Export]
	public Array XPToLevelUp;
	[Export]
	public bool isCurrentScenePlayableMap = false;
	public bool switchPortraits;
	public Array SwitchedCharacters;
	public int currentCharacterIndex = -1;
	[Export]
	public int maxCharacterCount;
	[Export]
	public int minCharacterCount;
	public bool createNewRCcharacters = false;
	[Export]
	public Array<int> selectedEnemies;
	public Statistics statistics;
	public Statistics globalStatistics;
	[Export]
	public TownDataResource townData;
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

	public void InsertCharacter(SavedCharacterResource character)
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
