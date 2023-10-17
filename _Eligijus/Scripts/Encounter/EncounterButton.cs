using Godot;


public partial class EncounterButton: Button
{
	[Export]
	private View _missionBar;
	[Export] 
	private EncounterController _encounterController;
	private EncounterResource _selectedEncounter;
	
	public override void _Ready()
	{
		base._Ready();
		Toggled += _missionBar.ToggleView;
	}

	public override void _Toggled(bool buttonPressed)
	{
		base._Toggled(buttonPressed);
		if (buttonPressed)
		{
			SelectEncounter();
		}
		else
		{
			_encounterController.ChangeSelectedEncounter(null);
		}
	}

	public void AddEncounter(EncounterResource encounter)
	{
		_selectedEncounter = encounter;
	}
	
	public void SelectEncounter()
	{
		_encounterController.ChangeSelectedEncounter(_selectedEncounter);
	}
}
