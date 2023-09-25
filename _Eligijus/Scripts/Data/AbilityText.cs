using System.Collections;
using System.Collections.Generic;
using Godot;

public partial class AbilityText : Resource
{
    [Export(PropertyHint.MultilineText)]
    public string abilityTitle;
    [Export(PropertyHint.MultilineText)]
    public string abilityDescription;
}
