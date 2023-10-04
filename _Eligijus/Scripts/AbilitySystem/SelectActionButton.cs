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
        selectAction.ActionSelection(abilityInformation.Action);
    }

    public void AbilityInformation(int abilityIndex, HelpTable helpTable, Ability characterAction, SelectAction selectedAction)
    {
        this.abilityIndex = abilityIndex;
        abilityInformation = characterAction;
        selectAction = selectedAction;
        _helpTable = helpTable;
    }
    
}
