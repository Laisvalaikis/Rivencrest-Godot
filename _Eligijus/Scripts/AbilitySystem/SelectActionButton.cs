using System.Collections;
using System.Collections.Generic;
using Godot;

public partial class SelectActionButton : Button
{
    [Export] 
    public Control buttonParent;
    [Export]
    public TextureRect abilityButtonBackground;
    [Export]
    public TextureRect AbilityButtonImage;
    [Export] 
    public ColorRect colorRect;
    [Export] 
    public Label turnLabel;
    private SelectAction selectAction;
    private int abilityIndex;
    private Ability abilityInformation;
    private HelpTable _helpTable;
    private bool setupAction = false;

    public override void _Pressed()
    {
        base._Pressed();
        OnButtonClick();
    }

    public void OnHover()
    {
        _helpTable.EnableTableForCharacters(abilityInformation);
    }

    public void OffHover()
    {
        _helpTable.DisableHelpTable();
    }

    public void OnButtonClick()
    {
        if (selectAction != null)
        {
            selectAction.ActionSelection(abilityInformation, abilityIndex);
        }
    }

    // Disable abiliteies after usage
    // public void UpdateAbilityPointsInformation(int availableAbilityPoints)
    // {
    //     if (abilityInformation != null)
    //     {
    //         DisableAbility();
    //         if (abilityInformation.Action.CheckIfAbilityIsActive() &&
    //             availableAbilityPoints < abilityInformation.Action.GetAbilityPoints())
    //         {
    //             DisableAbility("1");
    //         }
    //     }
    // }
    
    // public void UpdateAllButtonsByPoints(int availableAbilityPoints)
    // {
    //     selectAction.UpdateAllAbilityButtonsByPoints(availableAbilityPoints);
    // }
    //
    
    public void UpdateAbilityCooldownInformationActive()
    {
        if (abilityInformation.Action.CheckIfAbilityIsActive())
        {
            EnableAbility();
        }
        else
        {
            int abilityCooldown = abilityInformation.Action.GetCoolDown();
            int cooldown = abilityInformation.Action.GetCoolDownCount();
            int leftCooldown = abilityCooldown - cooldown;
            DisableAbility(leftCooldown.ToString());
        }
    }
    
    public void UpdateAbilityCooldownWithPoints(int availableAbilityPoints)
    {
        if (abilityInformation.Action.CheckIfAbilityIsActive() &&
            availableAbilityPoints < abilityInformation.Action.GetAbilityPoints())
        {
            DisableAbility("1");
        }
        else if (!abilityInformation.Action.CheckIfAbilityIsActive())
        {
            int abilityCooldown = abilityInformation.Action.GetCoolDown();
            int cooldown = abilityInformation.Action.GetCoolDownCount();
            int leftCooldown = abilityCooldown - cooldown;
            DisableAbility(leftCooldown.ToString());
        }
        else
        {
            EnableAbility();
        }
    }

    private void DisableAbility(string turnNumber, bool showText = true)
    {
        colorRect.Show();
        if (showText)
        {
            turnLabel.Show();
        }
        else
        {
            turnLabel.Hide();
        }
        turnLabel.Text = turnNumber; // default text 
        selectAction.DisableAbility(this);
        Disabled = true;
    }
    
    public void DisableAbility()
    {
        if (!abilityInformation.Action.CheckIfAbilityIsActive())
        {
            int abilityCooldown = abilityInformation.Action.GetCoolDown();
            int cooldown = abilityInformation.Action.GetCoolDownCount();
            int leftCooldown = abilityCooldown - cooldown;
            DisableAbility(leftCooldown.ToString());
        }
    }
    
    private void EnableAbility()
    {
        colorRect.Hide();
        turnLabel.Hide();
        turnLabel.Text = "1"; // default text 
        selectAction.EnableAbility(this, abilityIndex);
        Disabled = false;
    }

    public void AbilityInformation(int abilityIndex, HelpTable helpTable, Ability characterAction, SelectAction selectedAction)
    {
        if (!setupAction)
        {
            this.MouseEntered += OnHover;
            this.MouseExited += OffHover;
            setupAction = true;
        }

        this.abilityIndex = abilityIndex;
        abilityInformation = characterAction;
        selectAction = selectedAction;
        _helpTable = helpTable;
    }
    
    public void AbilityInformationFirstSelect()
    {
        ButtonPressed = true;
    }
    
}
