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
        selectAction.ActionSelection(abilityInformation);
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
