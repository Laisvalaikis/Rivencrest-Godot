using System.Collections;
using Godot.Collections;
using Godot;

public partial class MapCoordinates: Resource
{
    [Export]
    public Array<Vector3> coordinates;
}
