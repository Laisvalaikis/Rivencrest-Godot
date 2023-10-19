using System.Collections;
using System.Collections.Generic;
using System.Text;
using Godot;

public partial class HelpTable : Node
{
	[Export] 
	public View helpTableView;
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
		helpTableView.ExitView();
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
		SavedCharacterResource character = _data.Characters[characterIndex];
		UpdateHelpTable(character.playerInformation.abilities[abilityIndex]);
		// GD.PrintErr("FIX ABILITY INFORMATION");
	}

	public void DisableHelpTable()
	{
		if (wasSelected)
		{
			helpTableView.ExitView();
			wasSelected = false;
		}
	}
	
	public void UpdateHelpTable(Ability ability)
	{
		SetupHelpTable();
		if (wasSelected)
		{
			helpTableView.ExitView();
			wasSelected = false;
		}
		else
		{
			helpTableView.OpenView();
			FillTableWithInfo(ability);
			wasSelected = true;
		}
	}



	public void EnableTableForCharacters(Ability currentAbility)
	{
		SetupHelpTable();
		UpdateHelpTable(currentAbility);
	}
	
	private void FillTableWithInfo(Ability ability)
	{
		icon.Texture = (AtlasTexture)ability.AbilityImage;
		abilityTitle.Text = ability.abilityText.abilityTitle;
		abilityDescription.Text = ability.abilityText.abilityDescription;
		cooldownText.Text = ability.Action.GetPlayer().abilityCooldown.ToString();
		if (ability.Action.maxAttackDamage == 0)
		{
			damageIcon.Hide();
			damageText.Hide();
		}
		else
		{
			damageIcon.Show();
			damageText.Show();
			damageText.Text = ability.Action.GetDamageString();
		}

		if (ability.Action.isAbilitySlow)
		{
			slowAbility.Show();
			fastAbility.Hide();
		}
		else
		{
			slowAbility.Hide();
			fastAbility.Show();
		}

	}


}

