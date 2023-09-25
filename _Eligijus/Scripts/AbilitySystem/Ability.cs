using System;
using System.Collections;
using System.Collections.Generic;
using Godot;

public partial class Ability: Resource
{

    public AbilityText abilityText;
    public bool enabled = true;
    public Texture AbilityImage;
    public BaseAction Action;
}