using Godot;

public partial class ExitGame : Node
{
	public void ButtonPressed()
	{
		GetTree().Quit();
	}
}

