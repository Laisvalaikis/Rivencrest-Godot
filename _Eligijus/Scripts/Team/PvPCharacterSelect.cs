using System.Collections;
using System.Collections.Generic;
using Godot;

public partial class PvPCharacterSelect : FocusButton
{
    [Export] private TeamInformation teamInformation;
    [Export] private Control characterButtonParent;
    private GameTileMap _gameTileMap; //cia private padaryt
    [Export]
    private TextureRect characterPortraitSprite;
    private Node2D characterOnBoard;
    private PlayerInformation _playerInformation;
    private PlayerInformationDataNew _playerInformationData;
    private SelectAction _selectAction;
    private bool isButtonAvailable = true;
    private int buttonIndex = 0;

    public override void _Ready()
    {
        base._Ready();
        CreateCharatersPortrait();
    }

    public override void _Pressed()
    {
        base._Pressed();
        SelectPortrait();
    }

    public void SetGameTilemap(GameTileMap gameTileMap)
    {
        _gameTileMap = gameTileMap;
    }

    public void CreateCharatersPortrait()
    {
        if (characterOnBoard != null)
        {
            
            _playerInformationData = _playerInformation.objectData.GetPlayerInformationData();
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
    
    public void DisableCharacterPortrait()
    {
        characterButtonParent.Hide();
        isButtonAvailable = false;
    }

    public Node2D GetCharacter()
    {
        return characterOnBoard;
    }


    public void SelectPortrait()
    {
        if (_gameTileMap.GetCurrentCharacter() != characterOnBoard)
        {
            if (_gameTileMap.GetCurrentCharacter() != null)
            {
                _gameTileMap.GetCurrentCharacter().actionManager.DeselectAbility();
            }
            teamInformation.SetSelectedIndex(buttonIndex);
            _gameTileMap.SetCurrentCharacter(characterOnBoard);
            isButtonAvailable = false;
            _selectAction.SetCurrentCharacter((Player)characterOnBoard);
        }
        else if (_gameTileMap.GetCurrentCharacter() == characterOnBoard)
        {
            _gameTileMap.DeselectCurrentCharacter();
            _selectAction.DeSetCurrentCharacter();
            isButtonAvailable = true;
        }
        
    }

    public void SetButtonIndex(int index)
    {
        buttonIndex = index;
    }

    public void SetSelectAction(SelectAction selectAction)
    {
        _selectAction = selectAction;
    }
    

}
