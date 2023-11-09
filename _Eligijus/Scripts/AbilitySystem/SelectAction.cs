using System.Collections;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
public partial class SelectAction : Control
{
	private Array<Ability> _playerAllAbilities;
	private Array<Ability> _playerBaseAbilities;
	private Array<Ability> _playerAbilities;
	private Player _currentPlayer;
	private PlayerInformationData _playerInformationData;
	[Export] private HelpTable helpTable;
	[Export] private TextureRect characterPortrait;
	[Export] private TextureProgressBar healthBar;
	[Export] private Label healthText;
	[Export] private TextureProgressBar abilityPointsBar;
	[Export] private Label abilityPointsText;
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
		healthText.Text = _currentPlayer.playerInformation.GetHealth().ToString();
		int healthPercentage = GetProcentage(_currentPlayer.playerInformation.GetHealth(),
			_currentPlayer.playerInformation.GetMaxHealth());
		healthBar.Value = healthPercentage;
		abilityPointsText.Text = _currentPlayer.actionManager.GetAbilityPoints().ToString();
		int abilityPercentage = GetProcentage(_currentPlayer.actionManager.GetAbilityPoints(),
			_currentPlayer.actionManager.GetAllAbilityPoints());
		// reikia apskaiciuoti dynamiskai fillo ilgi, pradzia, ir value ability panaudojimo parodymui
		abilityPointsBar.Value = abilityPercentage;
		staminaButtonBackground.SelfModulate = _playerInformationData.backgroundColor;
	}

	private int GetProcentage(float min, float max)
	{
		return (int)(min / max * 100);
	}

	public void ActionSelection(Ability ability)
	{
		_actionManager.SetCurrentAbility(ability);
	}

	public void SetCurrentCharacter(Player currentPlayer)
	{
		if (currentPlayer.actionManager.ReturnBaseAbilities() != null)
		{
			Show();
			_currentPlayer = currentPlayer;
			SetupSelectAction();
			GetAbilities();
			UpdatePlayerInfo();
			_actionManager.SetCurrentAbility(_playerBaseAbilities[0]);
			GenerateActions();
		}

	}
	public void DeSetCurrentCharacter()
	{
		_currentPlayer = null;
		if (_actionManager != null)
		{
			_actionManager.DeselectAbility();
		}
	}

}
