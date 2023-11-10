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
	[Export] private TextureProgressBar abilityPointsUseBar;
	[Export] private Label abilityPointsText;
	[Export] private TextureRect staminaButtonBackground;
	[Export] private Array<SelectActionButton> allAbilityButtons;
	private Array<SelectActionButton> baseAbilityButtons;
	private Array<SelectActionButton> abilityButtons;
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
		
		if (baseAbilityButtons == null)
		{
			baseAbilityButtons = new Array<SelectActionButton>();
		}
		else
		{
			baseAbilityButtons.Clear();
		}
		
		if (abilityButtons == null)
		{
			abilityButtons = new Array<SelectActionButton>();
		}
		else
		{
			abilityButtons.Clear();
		}

		// Selects first ability button
		if (_playerBaseAbilities.Count > 0)
		{
			allAbilityButtons[buttonIndex].AbilityInformationFirstSelect();
		}

		for (int i = 0; i < _playerBaseAbilities.Count; i++)
		{
			if (_playerBaseAbilities[i].enabled)
			{
				allAbilityButtons[buttonIndex].buttonParent.Show();
				allAbilityButtons[buttonIndex].AbilityInformation(i, helpTable, _playerBaseAbilities[i], this);
				allAbilityButtons[buttonIndex].AbilityButtonImage.Texture = (AtlasTexture)_playerBaseAbilities[i].AbilityImage;
				allAbilityButtons[buttonIndex].abilityButtonBackground.SelfModulate = _playerInformationData.backgroundColor;
				allAbilityButtons[buttonIndex].turnLabel.LabelSettings.FontColor = _playerInformationData.textColor;
				baseAbilityButtons.Add(allAbilityButtons[buttonIndex]);
				_playerBaseAbilities[i].Action.SetSelectActionButton(allAbilityButtons[buttonIndex]);
				allAbilityButtons[buttonIndex].UpdateAbilityCooldownInformationActive();
				buttonIndex++;
			}
		}

		for (int i = 0; i < _playerAbilities.Count; i++)
		{
			if (_playerAbilities[i].enabled && unlockedAbilityList[i].abilityConfirmed)
			{
				allAbilityButtons[buttonIndex].buttonParent.Show();
				allAbilityButtons[buttonIndex].AbilityInformation(i, helpTable, _playerAbilities[i], this);
				allAbilityButtons[buttonIndex].AbilityButtonImage.Texture = (AtlasTexture)_playerAbilities[i].AbilityImage;
				allAbilityButtons[buttonIndex].abilityButtonBackground.SelfModulate = _playerInformationData.backgroundColor;
				allAbilityButtons[buttonIndex].turnLabel.LabelSettings.FontColor = _playerInformationData.textColor;
				abilityButtons.Add(allAbilityButtons[buttonIndex]);
				_playerAbilities[i].Action.SetSelectActionButton(allAbilityButtons[buttonIndex]);
				allAbilityButtons[buttonIndex].UpdateAbilityCooldownWithPoints(_actionManager.GetAbilityPoints());
				buttonIndex++;
			}
		}

		for (int i = buttonIndex; i < allAbilityButtons.Count; i++)
		{
			allAbilityButtons[i].buttonParent.Hide();
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
		abilityPointsBar.Value = abilityPercentage;
		abilityPointsUseBar.Value = abilityPercentage;
		staminaButtonBackground.SelfModulate = _playerInformationData.backgroundColor;
	}

	private void UpdateSelectedActionAbilityPoints(Ability ability)
	{
		int abilityPercentage = GetProcentage(_currentPlayer.actionManager.GetAbilityPoints() - ability.Action.GetAbilityPoints(),
			_currentPlayer.actionManager.GetAllAbilityPoints());
		abilityPointsBar.Value = abilityPercentage;
	}
	
	public void UpdateAllAbilityButtonsByPoints(int availableAbilityPoints)
	{
		for (int i = 0; i < abilityButtons.Count; i++)
		{
			abilityButtons[i].UpdateAbilityPointsInformation(availableAbilityPoints);
		}
	}

	private void ResetAbilityPointsView()
	{
		int abilityPercentage = GetProcentage(_currentPlayer.actionManager.GetAbilityPoints(),
			_currentPlayer.actionManager.GetAllAbilityPoints());
		abilityPointsBar.Value = abilityPercentage;
		abilityPointsUseBar.Value = abilityPercentage;
	}

	private int GetProcentage(float min, float max)
	{
		return (int)(min / max * 100);
	}

	public void ActionSelection(Ability ability)
	{
		_actionManager.SetCurrentAbility(ability);
		if (ability._type != AbilityType.BaseAbility)
		{
			UpdateSelectedActionAbilityPoints(ability);
		}
		else
		{
			ResetAbilityPointsView();
		}
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
			GenerateActions();
			_actionManager.SetCurrentAbility(_playerBaseAbilities[0]);
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
