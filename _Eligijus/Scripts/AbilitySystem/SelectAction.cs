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
	private PlayerInformationDataNew _playerInformationData;
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
	private int _currentAbilityIndex = 0;
	private int buttonIndexCount = 0;
	private bool nextAbilitySelection = false;
	private bool previousAbilitySelection = false;
	private bool previousAbilitySelectionReseted = false;
	
	public override void _Ready()
	{
		base._Ready();
		InputManager.Instance.ChangeAbilityNext += NextAbility;
		InputManager.Instance.ChangeAbilityPrevious += PreviousAbility;
	}

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
		_playerInformationData = _currentPlayer.objectInformation.GetPlayerInformation().objectData.GetPlayerInformationData();
	}

	private void GenerateActions()
	{
		buttonIndexCount = 0;
		
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
			allAbilityButtons[buttonIndexCount].AbilityInformationFirstSelect();
		}

		GD.PushError("Need to Optimize method, because it starts every time player was selected even if player is same");
		for (int i = 0; i < _playerBaseAbilities.Count; i++)
		{
			if (_playerBaseAbilities[i].enabled)
			{
				allAbilityButtons[buttonIndexCount].buttonParent.Show();
				allAbilityButtons[buttonIndexCount].AbilityInformation(buttonIndexCount, helpTable, _playerBaseAbilities[i], this);
				allAbilityButtons[buttonIndexCount].AbilityButtonImage.Texture = (AtlasTexture)_playerBaseAbilities[i].AbilityImage;
				allAbilityButtons[buttonIndexCount].abilityButtonBackground.SelfModulate = _playerInformationData.backgroundColor;
				allAbilityButtons[buttonIndexCount].turnLabel.LabelSettings.FontColor = _playerInformationData.textColor;
				baseAbilityButtons.Add(allAbilityButtons[buttonIndexCount]);
				_playerBaseAbilities[i].Action.SetSelectActionButton(allAbilityButtons[buttonIndexCount]);
				allAbilityButtons[buttonIndexCount].UpdateAbilityCooldownInformationActive();
				buttonIndexCount++;
			}
		}

		if (_currentPlayer.unlockedAbilityList != null && _currentPlayer.unlockedAbilityList.Count > 0)
		{
			for (int i = 0; i < _playerAbilities.Count; i++)
			{
				if (_playerAbilities[i].enabled &&  i < _currentPlayer.unlockedAbilityList.Count && _currentPlayer.unlockedAbilityList[i].abilityConfirmed)
				{
					allAbilityButtons[buttonIndexCount].buttonParent.Show();
					allAbilityButtons[buttonIndexCount].AbilityInformation(buttonIndexCount, helpTable, _playerAbilities[i], this);
					allAbilityButtons[buttonIndexCount].AbilityButtonImage.Texture =
						(AtlasTexture)_playerAbilities[i].AbilityImage;
					allAbilityButtons[buttonIndexCount].abilityButtonBackground.SelfModulate =
						_playerInformationData.backgroundColor;
					allAbilityButtons[buttonIndexCount].turnLabel.LabelSettings.FontColor = _playerInformationData.textColor;
					abilityButtons.Add(allAbilityButtons[buttonIndexCount]);
					_playerAbilities[i].Action.SetSelectActionButton(allAbilityButtons[buttonIndexCount]);
					allAbilityButtons[buttonIndexCount].UpdateAbilityCooldownWithPoints(_actionManager.GetAbilityPoints());
					buttonIndexCount++;
				}
				else
				{
					break;
				}
			}
		}

		for (int i = buttonIndexCount; i < allAbilityButtons.Count; i++)
		{
			allAbilityButtons[i].buttonParent.Hide();
		}
		
	}

	private void UpdatePlayerInfo()
	{
		characterPortrait.Texture = (AtlasTexture)_playerInformationData.CharacterPortraitSprite;
		healthText.Text = _currentPlayer.objectInformation.GetPlayerInformation().GetHealth().ToString();
		int healthPercentage = GetProcentage(_currentPlayer.objectInformation.GetPlayerInformation().GetHealth(),
			_currentPlayer.objectInformation.GetPlayerInformation().GetMaxHealth());
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

	public void NextAbility()
	{
		if (previousAbilitySelectionReseted)
		{
			previousAbilitySelectionReseted = false;
			previousAbilitySelection = false;
			_currentAbilityIndex = 0;
		}
		if (previousAbilitySelection)
		{
			previousAbilitySelection = false;
			_currentAbilityIndex++;
		}
		
		if (_currentAbilityIndex < buttonIndexCount-1)
		{
			_currentAbilityIndex++;
			allAbilityButtons[_currentAbilityIndex].OnButtonClick();
			allAbilityButtons[_currentAbilityIndex].ButtonPressed = true;
		}
		else
		{
			_currentAbilityIndex = 0;
			allAbilityButtons[_currentAbilityIndex].OnButtonClick();
			allAbilityButtons[_currentAbilityIndex].ButtonPressed = true;
		}
		nextAbilitySelection = true;
	}

	public void PreviousAbility()
	{
		if (nextAbilitySelection)
		{
			nextAbilitySelection = false;
			_currentAbilityIndex--;
		}

		if (_currentAbilityIndex >= 0)
		{
			allAbilityButtons[_currentAbilityIndex].OnButtonClick();
			allAbilityButtons[_currentAbilityIndex].ButtonPressed = true;
			if (_currentAbilityIndex > 0)
			{
				_currentAbilityIndex--;
			}
			else
			{
				_currentAbilityIndex = buttonIndexCount-1;
				previousAbilitySelectionReseted = true;
			}
		}
		else
		{
			_currentAbilityIndex = buttonIndexCount-1;
			allAbilityButtons[_currentAbilityIndex].OnButtonClick();
			allAbilityButtons[_currentAbilityIndex].ButtonPressed = true;
			_currentAbilityIndex--;
		}

		previousAbilitySelection = true;
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

	public void ActionSelection(Ability ability, int abilityIndex)
	{
		_actionManager.SetCurrentAbility(ability, abilityIndex);
		_currentAbilityIndex = abilityIndex;
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
			_actionManager.SetCurrentAbility(_playerBaseAbilities[0], 0);
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
