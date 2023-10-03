using System.Collections;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
public partial class TeamInformation : Control
{
    
    [Export] private SelectAction selectAction;
    [Export] private PlayerTeams playerTeams;
    [Export] private Sprite2D image;
    [Export] private GameTileMap gameTileMap;
    [Export] private Array<Node> TeamCharacterPortraitList;
    [Export] private Array<PvPCharacterSelect> pvpCharacterSelects;
    public int teamIndex;

    void Awake()
    {
        TeamCharacterPortraitList = new Array<Node>();
    }
    public void AddPortraitToList(Node2D character)
    {
        TeamCharacterPortraitList.Add(character);
    }
    public void AddCharacterToList(Node2D character)
    {
        TeamCharacterPortraitList.Add(character);
    }
    public void ModifyList()
    {
        TeamCharacterPortraitList.Clear();
        Array<Node2D> CharacterOnBoardList = playerTeams.AliveCharacterList(teamIndex);
        Array<PlayerInformation> characterAlivePlayerInformation = playerTeams.AliveCharacterPlayerInformationList(teamIndex);
        for(int i = 0; i < pvpCharacterSelects.Count; i++)
        {
            if (i < CharacterOnBoardList.Count)
            {
                pvpCharacterSelects[i].SetPortraitCharacter(CharacterOnBoardList[i], characterAlivePlayerInformation[i]);
                TeamCharacterPortraitList.Add(pvpCharacterSelects[i].GetCharacterPortraitFrame());
                pvpCharacterSelects[i].CreateCharatersPortrait();
                pvpCharacterSelects[i].SetSelectAction(selectAction);
                pvpCharacterSelects[i].gameTileMap = gameTileMap;
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
            pvpCharacterSelects[i].frame.Texture = (AtlasTexture)button;
        }
    }
}
