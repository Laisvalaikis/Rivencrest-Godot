using System.Collections;
using System.Collections.Generic;
using Godot;

public partial class PvPCharacterSelect : Control
{
    [Export] private Control characterButtonParent;
    private GameTileMap _gameTileMap; //cia private padaryt
    [Export]
    public TextureRect extension;
    [Export]
    private TextureRect characterPortraitSprite;
    private Node2D characterOnBoard;
    private PlayerInformation _playerInformation;
    private PlayerInformationData _playerInformationData;
    private SelectAction _selectAction;
    private bool isButtonAvailable = true;

    void Start()
    {
        CreateCharatersPortrait();
    }

    public void SetGameTilemap(GameTileMap gameTileMap)
    {
        _gameTileMap = gameTileMap;
    }

    public void CreateCharatersPortrait()
    {
        if (characterOnBoard != null)
        {
            _playerInformationData = _playerInformation.playerInformationData;
            characterPortraitSprite.Texture = (AtlasTexture)_playerInformationData.CharacterPortraitSprite;
            
            characterButtonParent.Show();
            isButtonAvailable = true;
        }
        else
        {
            characterButtonParent.Hide();
            isButtonAvailable = false;
        }
    }

    public void SetPortraitCharacter(Node2D character, PlayerInformation characterInformation)
    {
        characterOnBoard = character;
        _playerInformation = characterInformation;
    }
    

    public void SelectPortrait()
    {
        if (_gameTileMap.GetCurrentCharacter() != characterOnBoard) // GameObject.Find("GameInformation").GetComponent<GameInformation>().canButtonsBeClicked fix this bs
        {
            _gameTileMap.SetCurrentCharacter(characterOnBoard);
            isButtonAvailable = false;
            _selectAction.SetCurrentCharacter(characterOnBoard);
            GD.Print("Selected");
        }
        else if (_gameTileMap.GetCurrentCharacter() == characterOnBoard)
        {
            _gameTileMap.DeselectCurrentCharacter();
            _selectAction.DeSetCurrentCharacter();
            isButtonAvailable = true;
            GD.Print("Deselected");
        }

        Player character = (Player)characterOnBoard;
        if (character.playerInformation.GetHealth() > 0)
        {
              
        }
        
    }

    public void SetSelectAction(SelectAction selectAction)
    {
        _selectAction = selectAction;
    }

    public void OnHover()
    {
        
    }
    public void OffHover()
    {
       
    }

}
