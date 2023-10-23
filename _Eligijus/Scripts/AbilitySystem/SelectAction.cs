using System.Collections;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
public partial class SelectAction : Control
{
	private ActionManager _actionManagerNew;
	private Array<Ability> _playerAllAbilities;
	private Array<Ability> _playerBaseAbilities;
	private Array<Ability> _playerAbilities;
	private Player _currentPlayer;
	private PlayerInformationData _playerInformationData;
	[Export] private HelpTable helpTable;
	[Export] private TextureRect characterPortrait;
	[Export] private Label healthBar;
	[Export] private TextureRect staminaButtonBackground;
	[Export] private Array<SelectActionButton> abilityButtons;
	private ActionManager _actionManager;
	private Data _data;
	private void GetAbilities()
	{

	}

	public void SetupSelectAction()
	{
		if (Data.Instance != null)
		{
			_data = Data.Instance;
		}

		_actionManager = _currentPlayer.actionManager;
		_playerAbilities = _actionManager.ReturnAbilities();
		_playerBaseAbilities = _actionManager.ReturnBaseAbilities();
		_playerAllAbilities = _actionManager.ReturnAllAbilities();
		_playerInformationData = _currentPlayer.playerInformation.playerInformationData;
	}

	private void GenerateActions()
	{
		int buttonIndex = 0;
		Array<UnlockedAbilitiesResource> unlockedAbilityList =
			_data.Characters[_currentPlayer.playerIndex].unlockedAbilities;
		// Selects first ability button
		if (_playerBaseAbilities.Count > 0)
		{
			abilityButtons[buttonIndex].AbilityInformationFirstSelect();
		}

		for (int i = 0; i < _playerBaseAbilities.Count; i++)
		{
			if (_playerBaseAbilities[i].enabled)
			{
				abilityButtons[buttonIndex].buttonParent.Show();
				abilityButtons[buttonIndex].AbilityInformation(i, helpTable, _playerBaseAbilities[i], this);
				abilityButtons[buttonIndex].AbilityButtonImage.Texture = (AtlasTexture)_playerBaseAbilities[i].AbilityImage;
				abilityButtons[buttonIndex].abilityButtonBackground.SelfModulate = _playerInformationData.backgroundColor;
				buttonIndex++;
			}
		}

		for (int i = 0; i < _playerAbilities.Count; i++)
		{
			if (_playerAbilities[i].enabled && unlockedAbilityList[i].abilityConfirmed)
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
		_actionManager.SetCurrentAbility(characterAction);
	}

	public void SetCurrentCharacter(Node2D currentPlayer)
	{
		Show();
		_currentPlayer = (Player)currentPlayer;
		SetupSelectAction();
		GetAbilities();
		UpdatePlayerInfo();
		_actionManager.SetCurrentAbility(_playerBaseAbilities[0].Action);
		GenerateActions();
	}
	public void DeSetCurrentCharacter()
	{
		_currentPlayer = null;
	}

}
