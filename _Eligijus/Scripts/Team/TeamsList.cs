using Godot;
using Godot.Collections;

public partial class TeamsList : Resource
{
    public Dictionary<int, Team> Teams;
    public int enemyTeamCount = 0;
    public int characterTeamCount = 0;

    public TeamsList()
    {
        Teams = new Dictionary<int, Team>();
        enemyTeamCount = 0;
        characterTeamCount = 0;
    }
}
