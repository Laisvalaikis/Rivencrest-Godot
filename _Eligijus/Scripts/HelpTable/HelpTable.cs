using System.Collections;
using System.Collections.Generic;
using System.Text;
using Godot;

public partial class HelpTable : Control
{
	[Export]
	public TextureRect icon;
	[Export]
	public Label abilityTitle;
	[Export]
	public Label abilityDescription;
	[Export]
	public Label cooldownText;
	[Export]
	public Label rangeText;
	[Export]
	public Label damageText;
	[Export]
	public TextureRect damageIcon;
	[Export]
	public Control isAbilitySlow;
	[Export]
	public Control slowAbility;
	[Export]
	public Control fastAbility;
	[Export]
	public bool hasActionButtonBeenEntered = false;
	private Vector2 isAbilitySlowOriginalPosition;
	private bool wasSetuped = false;
	private bool wasSelected = false;
	private Data _data;
	
	public void closeHelpTable()
	{
		Hide();
	}

	public override void _Ready()
	{
		base._Ready();
		SetupHelpTable();
	}

	private void SetupHelpTable()
	{
		if (!wasSetuped)
		{

			if (_data == null && Data.Instance != null)
			{
				_data = Data.Instance;
			}
			
			isAbilitySlowOriginalPosition = isAbilitySlow.Position;
			wasSetuped = true;
		}
	}

	public void EnableTableForBoughtCharacters(int abilityIndex, int characterIndex)
	{
		SetupHelpTable();
		// ActionManager actionManager = _data.Characters[characterIndex].prefab.GetComponent<ActionManager>();
		SavedCharacterResource character = _data.Characters[characterIndex];
		// UpdateHelpTable(abilityIndex, character, actionManager);
		GD.PrintErr("FIX ABILITY INFORMATION");
	}

	public void DisableHelpTable()
	{
		if (wasSelected)
		{
			Hide();
			wasSelected = false;
		}
	}

	private void UpdateHelpTable(Ability currentAbility)
	{
		AbilityText abilityText = currentAbility.abilityText;
		if (abilityText != null)
		{
			if (wasSelected)
			{
				Hide();
				wasSelected = false;
			}
			else
			{
				Show();
				FillTableWithInfo(currentAbility, abilityText);
				wasSelected = true;
			}
		}
	}
	
	

	public void EnableTableForCharacters(Ability currentAbility)
	{
		SetupHelpTable();
		UpdateHelpTable(currentAbility);
	}
	

	private void FillTableWithInfo(Ability ability, AbilityText abilityText)
	{
		icon.Texture.Set("atlas", ability.AbilityImage);
		BaseAction baseAction = (BaseAction)ability.Action;
		abilityTitle.Text = abilityText.abilityTitle;
		abilityDescription.Text = abilityText.abilityDescription;
		cooldownText.Text = baseAction.AbilityCooldown.ToString();
		if (baseAction.maxAttackDamage == 0)
		{
			damageIcon.Hide();
			damageText.Hide();
		}
		else
		{
			damageIcon.Show();
			damageText.Show();
			damageText.Text = baseAction.GetDamageString();
		}

		if (baseAction.isAbilitySlow)
		{
			slowAbility.Show();
			fastAbility.Hide();
		}
		else
		{
			slowAbility.Hide();
			fastAbility.Show();
		}
		rangeText.Text = baseAction.AttackRange.ToString();
	}


}
