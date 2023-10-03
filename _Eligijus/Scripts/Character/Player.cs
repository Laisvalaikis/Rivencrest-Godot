using Godot;
using System;

public partial class Player : Node2D
{
	[Export] public PlayerInformation playerInformation;
	[Export] public ActionManager actionManager;
}
