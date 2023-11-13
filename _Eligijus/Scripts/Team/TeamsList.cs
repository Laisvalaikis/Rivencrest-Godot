using Godot;
using Godot.Collections;

public partial class TeamsList : Resource
{
    public Dictionary<int, Team> Teams;
    public int enemyTeamCount = 0;
    public int characterTeamCount = 0;
}
