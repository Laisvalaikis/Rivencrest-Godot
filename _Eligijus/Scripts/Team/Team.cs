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
    public bool isTeamAI;
    [Export] 
    private bool isTeamUsed = false;
    public int undoCount;
    public List<UsedAbility> usedAbilities = new List<UsedAbility>();

    public bool IsTeamUsed()
    {
        return isTeamUsed;
    }

    public void SetTeamIsUsed(bool usedTeam)
    {
        isTeamUsed = usedTeam;
    }

}