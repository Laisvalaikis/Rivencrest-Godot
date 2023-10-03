using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
public partial class Team : Resource
{
    [Export]
    public Array<Resource> characterPrefabs;
    [Export]
    public Array<Node2D> characters;
    [Export]
    public Array<Node2D> aliveCharacters;
    [Export]
    public Array<PlayerInformation> aliveCharactersPlayerInformation;
    [Export]
    public Array<Vector3> coordinates;
    [Export]
    public Array<UsedAbility> usedAbilities = new Array<UsedAbility>();
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