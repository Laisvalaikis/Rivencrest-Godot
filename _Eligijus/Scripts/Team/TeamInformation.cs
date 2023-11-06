using System.Collections;
using System.Collections.Generic;
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

	void Awake()
	{
	
	}
	
	public void ModifyList()
	{
		Array<Player> characterOnBoardList = _characterTeams.AliveCharacterList(teamIndex);
		for(int i = 0; i < pvpCharacterSelects.Count; i++)
		{
			if (i < characterOnBoardList.Count)
			{
				GD.Print(pvpCharacterSelects[i].Name);
				pvpCharacterSelects[i].SetPortraitCharacter(characterOnBoardList[i], characterOnBoardList[i].playerInformation);
				pvpCharacterSelects[i].CreateCharatersPortrait();
				pvpCharacterSelects[i].SetSelectAction(selectAction);
				pvpCharacterSelects[i].SetGameTilemap(gameTileMap);
			}
			else
			{
				pvpCharacterSelects[i].SetPortraitCharacter( null, null);
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

	public void EndTurn(int newTeamIndex)
	{
		teamIndex = newTeamIndex;
		ModifyList();
	}

	public void SelectCharacterPortrait(Node2D character, bool select = true)
	{
		Array<Player> characterOnBoardList = _characterTeams.AliveCharacterList(teamIndex);
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
