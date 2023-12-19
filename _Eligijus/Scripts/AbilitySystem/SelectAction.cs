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
	private List<SelectActionButton> activeButtons;
	private bool disabled = false;
	
	public override void _Ready()
	{
		base._Ready();
		InputManager.Instance.ChangeAbilityNext += NextAbility;
		InputManager.Instance.ChangeAbilityPrevious += PreviousAbility;
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
	
	// need to fix this part
	private void GenerateActions()
	{
		activeButtons = new List<SelectActionButton>();
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
		for (int i = 0; i < _playerBaseAbilities.Count; i++)
		{
			if (_playerBaseAbilities[i].enabled)
			{
				EnableAbilities(_playerBaseAbilities, baseAbilityButtons, i);
			}
		}
		for (int i = 0; i < _playerAbilities.Count; i++)
		{
			if (_playerAbilities[i].enabled)
			{
				EnableAbilities(_playerAbilities, abilityButtons, i, true);
			}
		}
		for (int i = buttonIndexCount; i < allAbilityButtons.Count; i++)
		{
			allAbilityButtons[i].buttonParent.Hide();
		}
		
	}

	public void EnableAbilities(Array<Ability> abilities, Array<SelectActionButton> usableAbilities, int index, bool updateAbilityPointsCooldown = false)
	{
		allAbilityButtons[buttonIndexCount].buttonParent.Show();
		allAbilityButtons[buttonIndexCount].AbilityInformation(buttonIndexCount, helpTable, abilities[index], this);
		allAbilityButtons[buttonIndexCount].AbilityButtonImage.Texture = (AtlasTexture)abilities[index].AbilityImage;
		allAbilityButtons[buttonIndexCount].abilityButtonBackground.SelfModulate = _playerInformationData.backgroundColor;
		allAbilityButtons[buttonIndexCount].turnLabel.LabelSettings.FontColor = _playerInformationData.textColor;
		usableAbilities.Add(allAbilityButtons[buttonIndexCount]);
		abilities[index].Action.SetSelectActionButton(allAbilityButtons[buttonIndexCount]);
		allAbilityButtons[buttonIndexCount].UpdateAbilityCooldownInformationActive();
		if (updateAbilityPointsCooldown)
		{
			allAbilityButtons[buttonIndexCount].UpdateAbilityCooldownWithPoints(_actionManager.GetAbilityPoints());
		}
		buttonIndexCount++;
	}

	public void EnableAbility(int index)
	{
		allAbilityButtons[index].ButtonPressed = true;
	}

	public void UpdatePlayerInfo()
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

	// this works kinda great
	public void NextAbility()
	{
		if (activeButtons is not null && _currentPlayer is not null && !disabled)
		{
			if (_currentAbilityIndex < activeButtons.Count - 1)
			{
				_currentAbilityIndex++;
				activeButtons[_currentAbilityIndex].OnButtonClick();
				activeButtons[_currentAbilityIndex].ButtonPressed = true;
			}
			else
			{
				_currentAbilityIndex = 0;
				activeButtons[_currentAbilityIndex].OnButtonClick();
				activeButtons[_currentAbilityIndex].ButtonPressed = true;
			}
		}
	}

	public void PreviousAbility()
	{
		if (activeButtons is not null && _currentPlayer is not null && !disabled)
		{
			if (_currentAbilityIndex >= 0)
			{
				if (_currentAbilityIndex > 0)
				{
					_currentAbilityIndex--;
				}
				else
				{
					_currentAbilityIndex = activeButtons.Count - 1;
				}

				activeButtons[_currentAbilityIndex].OnButtonClick();
				activeButtons[_currentAbilityIndex].ButtonPressed = true;

			}
			else
			{
				_currentAbilityIndex = activeButtons.Count - 1;
				activeButtons[_currentAbilityIndex].OnButtonClick();
				activeButtons[_currentAbilityIndex].ButtonPressed = true;
				_currentAbilityIndex--;
			}
		}
	}
	
	public void DisableAbility(SelectActionButton selectActionButton)
	{
		if (activeButtons.Contains(selectActionButton))
		{
			activeButtons.Remove(selectActionButton);
			if (_currentAbilityIndex >= activeButtons.Count)
			{
				_currentAbilityIndex = 0;
				// activeButtons[_currentAbilityIndex].OnButtonClick();
				// activeButtons[_currentAbilityIndex].ButtonPressed = true;
			}
		}
	}
	
	public void EnableAbility(SelectActionButton selectActionButton, int index)
	{
		if (!activeButtons.Contains(selectActionButton))
		{
			if (index <= activeButtons.Count)
			{
				activeButtons.Insert(index, selectActionButton);
			}
			else
			{
				activeButtons.Add(selectActionButton);
			}
		}
	}
	
	private int GetProcentage(float min, float max)
	{
		return (int)(min / max * 100);
	}

	public void UpdateAllAbilityButtonsByPoints(int availableAbilityPoints)
	{
		for (int i = 0; i < abilityButtons.Count; i++)
		{
			abilityButtons[i].UpdateAbilityCooldownWithPoints(availableAbilityPoints);
		}
	}
	
	public void UpdateBaseAbilities()
	{
		for (int i = 0; i < baseAbilityButtons.Count; i++)
		{
			baseAbilityButtons[i].UpdateAbilityCooldownInformationActive();
		}
	}
	
	
	public void ActionSelection(Ability ability, int abilityIndex)
	{
		_actionManager.SetCurrentAbility(ability, abilityIndex);
		if (activeButtons.Contains(allAbilityButtons[abilityIndex]))
		{
			_currentAbilityIndex = activeButtons.IndexOf(allAbilityButtons[abilityIndex]);
		}
	}
	
	
	public void SetCurrentCharacter(Player currentPlayer)
	{
		if (currentPlayer.actionManager.ReturnBaseAbilities() != null)
		{
			Player previousPlayer = _currentPlayer;
			Enable();
			_currentPlayer = currentPlayer;
			SetupSelectAction();
			if (previousPlayer != _currentPlayer)
			{
				UpdatePlayerInfo();
				GenerateActions();
			}
			UpdateBaseAbilities();
			UpdateAllAbilityButtonsByPoints(_currentPlayer.actionManager.GetAbilityPoints());
			(int index, Ability ability) firstAbility = GetFirstAvailableAbility();
			if (firstAbility.index != -1)
			{
				EnableAbility(firstAbility.index);
				_actionManager.SetCurrentAbility(firstAbility.ability, firstAbility.index);
			}
			else
			{
				_actionManager.SetCurrentAbility(_playerBaseAbilities[0], 0);
			}
		}

	}

	public (int, Ability) GetFirstAvailableAbility()
	{
		int index = 0;
		for (int i = 0; i < _playerBaseAbilities.Count; i++)
		{
			if (_playerBaseAbilities[i].Action.CheckIfAbilityIsActive())
			{
				return (index, _playerBaseAbilities[i]);
			}
			index++;
		}
		
		for (int i = 0; i < _playerAbilities.Count; i++)
		{
			if (_playerAbilities[i].Action.CheckIfAbilityIsActive() && _actionManager.GetAbilityPoints() >= _playerAbilities[i].Action.GetAbilityPoints())
			{
				return (index, _playerAbilities[i]);
			}
			index++;
		}
		return (-1, null);
	}

	public void Disable()
	{
		Hide();
		disabled = true;
	}

	public void Enable()
	{
		Show();
		disabled = false;
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
