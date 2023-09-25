using System;
using System.Collections;
using System.Collections.Generic;
using Godot;

public partial class CharacterPortrait : Control
{
	[Export]
	public int characterIndex = 0;
	[Export]
	public TextureRect characterImage;
	[Export]
	public Label levelText;
	[Export]
	public Control abilityPointCorner;
	[Export]
	public CharacterTable characterTable;
	private Data _data;

	public override void _Ready()
	{
		base._Ready();
		_data = Data.Instance;
	}

	public void OnPortraitClick()
	{
			if (_data.switchPortraits)
			{
				AddCharacterForSwitching(characterIndex);
			}
			else
			{
				if (characterTable.IsVisibleInTree() && characterTable.GetCurrentCharacterIndex() == characterIndex)
				{
					characterTable.ExitTable();
				}
				else
				{
					characterTable.DisplayCharacterTable(characterIndex);
					characterTable.UpdateTable();
					characterTable.UpdateAllAbilities();
				}
			}
		
	}
	
	private void AddCharacterForSwitching(int index)
	{
		_data.SwitchedCharacters.Add(index);
		if (_data.SwitchedCharacters.Count == 2)
		{
			_data.switchPortraits = false;
			(_data.Characters[_data.SwitchedCharacters[0]], _data.Characters[_data.SwitchedCharacters[1]]) = (_data.Characters[_data.SwitchedCharacters[1]], _data.Characters[_data.SwitchedCharacters[0]]);
		}
	}

}
