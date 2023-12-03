using Godot;

public partial class ControllerSelection : Node
{
    private bool _selectedTileWasVisible = false;
    public override void _Ready()
    {
        base._Ready();
        InputManager.Instance.EnableSelector += EnablePositionCursor;
        InputManager.Instance.DisableSelector += DisablePreviousCursor;
    }

    public void EnablePositionCursor(Vector2 mousePosition) // need update tile rendering enabling when player was selected
    {
        HighlightTile highlightTile = GameTileMap.Tilemap.GetChunk(mousePosition).GetTileHighlight();
        if (!highlightTile.TileWasEnabled())
        {
            highlightTile.EnableControllerSelectTile(true);
            _selectedTileWasVisible = true;
        }
        highlightTile.EnableMouseSelector();
    }

    public void DisablePreviousCursor(Vector2 mousePosition) // need update tile enabling when player was selected
    {
        HighlightTile highlightTile = GameTileMap.Tilemap.GetChunk(mousePosition).GetTileHighlight();
        highlightTile.DisableMouseSelector();
        if (_selectedTileWasVisible && !highlightTile.TileWasEnabled())
        {
            highlightTile.EnableControllerSelectTile(false);
            _selectedTileWasVisible = false;
        }
    }
}