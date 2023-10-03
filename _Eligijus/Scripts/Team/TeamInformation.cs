using System.Collections;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
public partial class TeamInformation : Control
{
	
	[Export] private SelectAction selectAction;
	[Export] private PlayerTeams playerTeams;
	[Export] private TextureRect image;
	[Export] private GameTileMap gameTileMap;
	[Export] private Array<PvPCharacterSelect> pvpCharacterSelects;
	public int teamIndex;

	void Awake()
	{
	
	}
	
	public void ModifyList()
	{
		Array<Node2D> CharacterOnBoardList = playerTeams.AliveCharacterList(teamIndex);
		Array<PlayerInformation> characterAlivePlayerInformation = playerTeams.AliveCharacterPlayerInformationList(teamIndex);
		for(int i = 0; i < pvpCharacterSelects.Count; i++)
		{
			if (i < CharacterOnBoardList.Count)
			{
				pvpCharacterSelects[i].SetPortraitCharacter(CharacterOnBoardList[i], characterAlivePlayerInformation[i]);
				pvpCharacterSelects[i].CreateCharatersPortrait();
				pvpCharacterSelects[i].SetSelectAction(selectAction);
				pvpCharacterSelects[i].SetGameTilemap(gameTileMap);
			}
			else
			{
				pvpCharacterSelects[i].SetPortraitCharacter( null, null);
			}
			
		}

		if (CharacterOnBoardList.Count != 0)
		{
			image.Show();
		}
		else
		{
			image.Hide();
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
