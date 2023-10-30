using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
public partial class Team : Resource
{
    [Export]
    public Array<Resource> characterPrefabs;
    [Export]
    public Array<Player> characters;
    [Export]
    public Array<Vector3> coordinates;
    [Export]
    public string teamName;
    [Export]
    public string teamAllegiance;
    [Export]
    public bool isTeamAI;
    public int undoCount;
    public Node2D teamPortraitBoxGameObject;
    public Resource lastSelectedPlayer;

}