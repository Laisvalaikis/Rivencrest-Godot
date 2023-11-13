using Godot;

public partial class CreateSave : Node
{
	[Export]
	private BaseButton buttonForCreation;
	[Export]
	private SaveManager _saveManager;
	private int _difficulty;
	private Color _color;
	private string _teamName;

	public override void _Ready()
	{
		_difficulty = -1;
		_color = Colors.Black;
		if (buttonForCreation != null) buttonForCreation.Disabled = true;
	}

	public void TextInput(string text)
	{
		if (text.Length > 0 && text[0] == ' ')
		{
			text = "";
			_teamName = "";
		}

		text = text.ToUpper();
		_teamName = text;
		if (text != "" && _difficulty != -1 && _color != Colors.Black)
			buttonForCreation.Disabled = false;
		else
			buttonForCreation.Disabled = true;
	}
	
	public void SetDifficulty(int difficulty)
	{
		_difficulty = difficulty;
		if (_teamName != "" && difficulty != -1 && _color != Colors.Black)
			buttonForCreation.Disabled = false;
		else
			buttonForCreation.Disabled = true;
	}

	public void SetColor(Color color)
	{
		_color = color;
		if (_teamName != "" && _difficulty != -1 && color != Colors.Black)
			buttonForCreation.Disabled = false;
		else
			buttonForCreation.Disabled = true;
	}

	public void ResetData()
	{
		_difficulty = -1;
		_color = Colors.Black;
		buttonForCreation.Disabled = false;
	}

	public void CreateNewSave()
	{
		_saveManager.CreateSave(_color, _difficulty, _teamName);
	}

}

