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
    public Array<Vector2> coordinates;
    [Export]
    public string teamName;
    [Export]
    public string teamAllegiance;
    [Export]
    public bool isTeamAI;
    public int undoCount;

}