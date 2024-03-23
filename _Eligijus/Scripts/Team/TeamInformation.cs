using System.Collections;
using System.Linq;
using Godot;
using Godot.Collections;
public partial class TeamInformation : Control
{
	[Export] private Button focusLeft;
	[Export] private Button focusRight;
	[Export] private SelectAction selectAction;
	[Export] private CharacterTeams _characterTeams;
	[Export] private TextureRect image;
	[Export] private GameTileMap gameTileMap;
	[Export] private Array<PvPCharacterSelect> pvpCharacterSelects;
	public int teamIndex;
	private Array<PvPCharacterSelect> activeCharacterSelects;
	private int characterCount;
	private bool changedTeams = false;
	private int currentCharacterIndex = 0;
	private bool characterFocus = false;

	public override void _Ready()
	{
		base._Ready();
		InputManager.Instance.ChangeNextCharacter += NextCharacter;
		InputManager.Instance.ChangePreviousCharacter += PreviousCharacter;
		InputManager.Instance.CharacterFocusInGame += FocusFirstCharacter;
		InputManager.Instance.ReleaseFocus += ReleaseFocusFirstCharacter;
	}

	public void ModifyList()
	{
		GD.PrintErr("Need optimization");
		Dictionary<int, Player> characterOnBoardList = _characterTeams.AliveCharacterList(teamIndex);
		bool teamIsAI = _characterTeams.TeamIsAI(teamIndex);
		int[] keyArray = characterOnBoardList.Keys.ToArray();
		if (characterCount != characterOnBoardList.Count || changedTeams)
		{
			activeCharacterSelects = new Array<PvPCharacterSelect>();
			int lastButtonActive = 0;
			for (int i = 0; i < pvpCharacterSelects.Count; i++)
			{
				if (i < characterOnBoardList.Count)
				{
					Player character = characterOnBoardList[keyArray[i]];
					pvpCharacterSelects[i].SetPortraitCharacter(character,
						character.objectInformation.GetPlayerInformation());
					pvpCharacterSelects[i].CreateCharatersPortrait();
					pvpCharacterSelects[i].SetSelectAction(selectAction);
					pvpCharacterSelects[i].SetGameTilemap(gameTileMap);
					pvpCharacterSelects[i].SetButtonIndex(i);
					activeCharacterSelects.Add(pvpCharacterSelects[i]);
					pvpCharacterSelects[i].Disabled = teamIsAI;
					lastButtonActive = i;
				}
				else
				{
					pvpCharacterSelects[i].SetPortraitCharacter(null, null);
					pvpCharacterSelects[i].DisableCharacterPortrait();
				}
			}
			if (characterOnBoardList.Count != 0)
			{
				image.Show();
			}
			else
			{
				image.Hide();
			}
			// button focus for controllers
			pvpCharacterSelects[lastButtonActive].FocusNeighborRight = focusRight.GetPath();
			focusRight.FocusNeighborLeft = pvpCharacterSelects[lastButtonActive].GetPath();
			pvpCharacterSelects[0].FocusNeighborLeft = focusLeft.GetPath();
			focusLeft.FocusNeighborRight = pvpCharacterSelects[0].GetPath();
			//
			// pvpCharacterSelects[lastButtonActive].FocusNeighborRight = pvpCharacterSelects[0].GetPath();
			// pvpCharacterSelects[0].FocusNeighborLeft = pvpCharacterSelects[lastButtonActive].GetPath();
		}
		characterCount = characterOnBoardList.Count;
		changedTeams = false;
	}

	public void EndTurn(int newTeamIndex)
	{
		GameTileMap.Tilemap.DeselectCurrentCharacter();
		teamIndex = newTeamIndex;
		changedTeams = true;
		ModifyList();
	}

	public void SelectCharacterPortrait(Node2D character, bool select = true)
	{
		Dictionary<int, Player> characterOnBoardList = _characterTeams.AliveCharacterList(teamIndex);
		for (int i = 0; i < characterOnBoardList.Count; i++)
		{
			if (pvpCharacterSelects[i].GetCharacter() == character)
			{
				pvpCharacterSelects[i].ButtonPressed = select;
			}
		}
	}
	
	public void PressCharacterPortrait(Node2D character, bool select = true)
	{
		Dictionary<int, Player> characterOnBoardList = _characterTeams.AliveCharacterList(teamIndex);
		for (int i = 0; i < characterOnBoardList.Count; i++)
		{
			if (pvpCharacterSelects[i].GetCharacter() == character)
			{
				pvpCharacterSelects[i].SelectPortrait();
			}
		}
	}

	public void SetSelectedIndex(int index)
	{
		currentCharacterIndex = index;
	}

	public void NextCharacter()
	{
		if (activeCharacterSelects is not null && IsVisibleInTree())
		{
			if (currentCharacterIndex < activeCharacterSelects.Count - 1)
			{
				currentCharacterIndex++;
				activeCharacterSelects[currentCharacterIndex]._Pressed();
				activeCharacterSelects[currentCharacterIndex].ButtonPressed = true;
			}
			else
			{
				currentCharacterIndex = 0;
				activeCharacterSelects[currentCharacterIndex]._Pressed();
				activeCharacterSelects[currentCharacterIndex].ButtonPressed = true;
			}
		}
	}

	public void FocusFirstCharacter()
	{
		pvpCharacterSelects[0].GrabFocus();
		FocusManager.Instance.SetCurrentFocus(pvpCharacterSelects[0]);
		characterFocus = true;
	}
	
	public void ReleaseFocusFirstCharacter()
	{
		characterFocus = false;
		FocusManager.Instance.ResetFocus();
	}

	public bool IsFocusedOnFirstCharacter()
	{
		return characterFocus;
	}

	public void PreviousCharacter()
	{
		if (activeCharacterSelects is not null && IsVisibleInTree())
		{
			if (currentCharacterIndex >= 0)
			{
				if (currentCharacterIndex > 0)
				{
					currentCharacterIndex--;
				}
				else
				{
					currentCharacterIndex = activeCharacterSelects.Count - 1;
				}

				activeCharacterSelects[currentCharacterIndex]._Pressed();
				activeCharacterSelects[currentCharacterIndex].ButtonPressed = true;

			}
			else
			{
				currentCharacterIndex = activeCharacterSelects.Count - 1;
				activeCharacterSelects[currentCharacterIndex]._Pressed();
				activeCharacterSelects[currentCharacterIndex].ButtonPressed = true;
				currentCharacterIndex--;
			}
		}
	}
	
	protected override void Dispose(bool disposing)
	{
		InputManager.Instance.ChangeNextCharacter -= NextCharacter;
		InputManager.Instance.ChangePreviousCharacter -= PreviousCharacter;
		InputManager.Instance.CharacterFocusInGame -= FocusFirstCharacter;
		base.Dispose(disposing);
	}
	
}
