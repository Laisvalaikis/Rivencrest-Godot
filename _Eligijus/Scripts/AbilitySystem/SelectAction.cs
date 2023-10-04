using System.Collections;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
public partial class SelectAction : Control
{
	private ActionManager _actionManagerNew;
	private Array<Ability> _playerAbilities;
	private Player _currentPlayer;
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

	public void SetupSelectAction()
	{
		_playerAbilities = _currentPlayer.actionManager.ReturnAbilities();
		_playerInformationData = _currentPlayer.playerInformation.playerInformationData;
	}

	private void GenerateActions()
	{
		int buttonIndex = 0;
		
		// Selects first ability button
		if (_playerAbilities.Count > 0)
		{
			abilityButtons[buttonIndex].AbilityInformationFirstSelect();
		}

		for (int i = 0; i < _playerAbilities.Count; i++)
		{
			if (_playerAbilities[i].enabled)
			{
				abilityButtons[buttonIndex].buttonParent.Show();
				abilityButtons[buttonIndex].AbilityInformation(i, helpTable, _playerAbilities[i], this);
				abilityButtons[buttonIndex].AbilityButtonImage.Texture = (AtlasTexture)_playerAbilities[i].AbilityImage;
				abilityButtons[buttonIndex].abilityButtonBackground.SelfModulate = _playerInformationData.backgroundColor;
				buttonIndex++;
			}
		}

		for (int i = buttonIndex; i < abilityButtons.Count; i++)
		{
			abilityButtons[i].buttonParent.Hide();
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
		_currentPlayer = (Player)currentPlayer;
		SetupSelectAction();
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
