using System.Collections;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
public partial class SelectAction : Control
{
	private ActionManager _actionManagerNew;
	private Array<Ability> _playerAbilities;
	private Node2D _currentPlayer;
	private PlayerInformationData _playerInformationData;
	[Export] private HelpTable helpTable;
	[Export] private AbilityManager _abilityManager;
	[Export] private TextureRect characterPortrait;
	[Export] private Label healthBar;
	[Export] private TextureRect staminaButtonBackground;
	[Export] private Array<SelectActionButton> abilityButtons;
	private void GetAbilities()
	{

	}

	public void SetupSelectAction(ActionManager actionManager, PlayerInformationData playerInformationData)
	{
		_playerAbilities = actionManager.ReturnAbilities();
		_playerInformationData = playerInformationData;
	}

	private void GenerateActions()
	{
		int buttonIndex = 0;
		
		for (int i = 0; i < _playerAbilities.Count; i++)
		{
			if (_playerAbilities[i].enabled)
			{
				abilityButtons[buttonIndex].Show();
				abilityButtons[buttonIndex].AbilityInformation(i, helpTable, _playerAbilities[i], this);
				abilityButtons[buttonIndex].AbilityButtonImage.Texture = (AtlasTexture)_playerAbilities[i].AbilityImage;
				abilityButtons[buttonIndex].abilityButtonBackground.SelfModulate = _playerInformationData.backgroundColor;
				buttonIndex++;
			}
		}

		for (int i = buttonIndex; i < abilityButtons.Count; i++)
		{
			abilityButtons[i].Hide();
		}
		
	}

	private void UpdatePlayerInfo()
	{
		characterPortrait.Texture = (AtlasTexture)_playerInformationData.CharacterPortraitSprite;
		healthBar.Text = _playerInformationData.MaxHealth.ToString();
		staminaButtonBackground.SelfModulate = _playerInformationData.backgroundColor;
	}

	public void ActionSelection(BaseAction characterAction)
	{
		_abilityManager.SetCurrentAbility(characterAction);
	}

	public void SetCurrentCharacter(Node2D currentPlayer)
	{
		Show();
		_currentPlayer = currentPlayer;
		GetAbilities();
		UpdatePlayerInfo();
		_abilityManager.SetCurrentAbility(_playerAbilities[0].Action);
		GenerateActions();
	}
	public void DeSetCurrentCharacter()
	{
		_currentPlayer = null;
	}

}
