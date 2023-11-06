using Godot;
using Godot.Collections;

public partial class MapEnemyData : Resource
{
    [Export]
    public int level;
    [Export]
    public int enemyCount;
    [Export]
    public Array<Resource> enemies;
}