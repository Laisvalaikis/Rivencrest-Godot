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
        selectAction.ActionSelection(abilityInformation, this);
    }

    public void UpdateAbilityPointsInformation(int availableAbilityPoints)
    {
        if (abilityInformation != null)
        {
            UpdateAbilityCooldownInformation();
            if (abilityInformation.Action.CheckIfAbilityIsActive() &&
                availableAbilityPoints < abilityInformation.Action.GetAbilityPoints())
            {
                colorRect.Show();
                turnLabel.Show();
                turnLabel.Text = "1"; // default text 
                Disabled = true;
            }
        }
    }

    public void UpdateAllButtonsByPoints(int availableAbilityPoints)
    {
        selectAction.UpdateAllAbilityButtonsByPoints(availableAbilityPoints);
    }

    public void UpdateAbilityCooldownInformation()
    {
        if (!abilityInformation.Action.CheckIfAbilityIsActive())
        {
            colorRect.Show();
            turnLabel.Show();
            int abilityCooldown = abilityInformation.Action.GetCoolDown();
            int cooldown = abilityInformation.Action.GetCoolDownCount();
            int leftCooldown = abilityCooldown - cooldown;
            turnLabel.Text = leftCooldown.ToString();
            Disabled = true;
        }
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
