using System;
using System.Collections;
using System.Collections.Generic;
using Godot;

public partial class Ability: Resource
{
    [Export]
    public AbilityText abilityText;
    [Export]
    public bool enabled = true;
    [Export]
    public Texture AbilityImage;
    [Export]
    public BaseAction Action;
}