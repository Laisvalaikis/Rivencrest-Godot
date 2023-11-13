using Godot;
using Godot.Collections;
public partial class Team : Resource
{
    [Export]
    public Dictionary<int, Resource> characterPrefabs;
    [Export]
    public Dictionary<int, Player> characters;
    [Export]
    public Array<Vector2> coordinates;
    [Export]
    public string teamName;
    [Export]
    public bool isTeamAI;
    [Export] 
    private bool isTeamUsed = false;
    public int undoCount;
    
    public LinkedList<UsedAbility> usedAbilitiesBeforeStartTurn = new LinkedList<UsedAbility>();
    public LinkedList<UsedAbility> usedAbilitiesAfterResolve = new LinkedList<UsedAbility>();
    public LinkedList<UsedAbility> usedAbilitiesEndTurn = new LinkedList<UsedAbility>();

    public bool IsTeamUsed()
    {
        return isTeamUsed;
    }

    public void SetTeamIsUsed(bool usedTeam)
    {
        isTeamUsed = usedTeam;
    }

}