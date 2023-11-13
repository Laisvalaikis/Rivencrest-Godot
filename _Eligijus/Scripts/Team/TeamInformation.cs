using System.Collections;
using System.Linq;
using Godot;
using Godot.Collections;
public partial class TeamInformation : Control
{
	[Export] private SelectAction selectAction;
	[Export] private CharacterTeams _characterTeams;
	[Export] private TextureRect image;
	[Export] private GameTileMap gameTileMap;
	[Export] private Array<PvPCharacterSelect> pvpCharacterSelects;
	public int teamIndex;
	private int characterCount;
	private bool changedTeams = false;

	void Awake()
	{
	
	}
	
	public void ModifyList()
	{
		Dictionary<int, Player> characterOnBoardList = _characterTeams.AliveCharacterList(teamIndex);
		bool teamIsAI = _characterTeams.TeamIsAI(teamIndex);
		int[] keyArray = characterOnBoardList.Keys.ToArray();
		if (characterCount != characterOnBoardList.Count || changedTeams)
		{
			for (int i = 0; i < pvpCharacterSelects.Count; i++)
			{
				if (i < characterOnBoardList.Count)
				{
					Player character = characterOnBoardList[keyArray[i]];
					pvpCharacterSelects[i].SetPortraitCharacter(character,
						character.playerInformation);
					pvpCharacterSelects[i].CreateCharatersPortrait();
					pvpCharacterSelects[i].SetSelectAction(selectAction);
					pvpCharacterSelects[i].SetGameTilemap(gameTileMap);
					pvpCharacterSelects[i].Disabled = teamIsAI;
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

	public void ChangeBoxSprites(AtlasTexture main, Texture extension, Texture button)
	{
		image.Texture = main;
		for (int i = 0; i < pvpCharacterSelects.Count; i++)
		{
			pvpCharacterSelects[i].extension.Texture = (AtlasTexture)extension;
			StyleBoxTexture styleBoxTexture = new StyleBoxTexture();
			styleBoxTexture.Texture = (AtlasTexture)button;
			pvpCharacterSelects[i].AddThemeStyleboxOverride("normal", styleBoxTexture);
		}
	}
}
