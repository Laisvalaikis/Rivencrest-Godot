using System.Collections;
using System.Collections.Generic;
using Godot;

public partial class UpgradeData : Resource
{
    [Export]
    public int upgradeIndex;
    [Export]
    public int upgradeValue;
    [Export]
    public int upgradeCost;
    [Export]
    public string upgradeName;
    [Export]
    public string upgradeDescription;
}
