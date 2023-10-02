using Godot;
using Godot.Collections;

public partial class MapDataController: Node
{
	[Export]
	public Dictionary<string, MapData> mapDatas;
}
