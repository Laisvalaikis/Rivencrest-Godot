using System;
using System.Collections;
using Godot;
using Godot.Collections;

public partial class CharacterSelectManager : Node
{
	[Export]
	private Array<CharacterSelect> characterButtons;
	[Export]
	private View _characterSelectView;
	[Export] 
	public Array<PortraitButton> portraitButtons;
	[Export]
	private Button _embark;
	[Export]
	private Button _back;
	private Array<int> _selectedCharacterIndexForMission;
	private Array<SavedCharacterResource> _selectedCharacterForMission;
	private bool _characterSelectOpen = false;
	private Data _data;

	public override void _Ready()
	{
		base._Ready();
		if (Data.Instance != null && _data == null)
		{
			_data = Data.Instance;
		}
		SetupView();
	}
	
	public void SetupView()
	{
		for (int i = 0; i < characterButtons.Count; i++)
		{
			if (i < _data.Characters.Count)
			{
				characterButtons[i].Show();
				characterButtons[i].characterIndex = i;
				characterButtons[i].portrait.Show();
				characterButtons[i].portrait.Texture = NewAtlas((CompressedTexture2D)_data.Characters[i].playerInformation.CharacterPortraitSprite.Get("atlas"), (Rect2)_data.Characters[i].playerInformation.CharacterPortraitSprite.Get("region"));
				characterButtons[i].levelText.Text = _data.Characters[i].level.ToString();
			}
			else
			{
				characterButtons[i].Hide();
			}
		}
		if (_selectedCharacterForMission != null && _selectedCharacterForMission.Count == 3)
		{
			DisableCharacters();
		}
		else
		{
			EnableCharacters();
		}
		_back.Show();
	}

	public PortraitButton FindFirstUnoccupied()
	{
		foreach (PortraitButton x in portraitButtons)
		{
			if (x.playerInformation == null)
			{
				return x;
			}
		}
		return null;
	}
	
	public void AddCharacter(PlayerInformationData playerinformation, int charIndex)
	{
		if (FindFirstUnoccupied() != null)
		{
			var firstUnoccupiedButton = FindFirstUnoccupied();
			firstUnoccupiedButton.playerInformation = playerinformation;
			firstUnoccupiedButton.characterIndex = charIndex;
			firstUnoccupiedButton.characterImage.Texture = NewAtlas((CompressedTexture2D)playerinformation.characterSprite.Get("atlas"), (Rect2)playerinformation.characterSprite.Get("region"));
		}
	}
	
	public void AddCharacterToTeam(int characterIndex)
	{
		if (_selectedCharacterForMission == null)
		{
			_selectedCharacterForMission = new Array<SavedCharacterResource>();
		}

		if (_selectedCharacterIndexForMission == null)
		{
			_selectedCharacterIndexForMission = new Array<int>();
		}

		_selectedCharacterForMission.Add(_data.Characters[characterIndex]);
		_selectedCharacterIndexForMission.Add(characterIndex);
		characterButtons[characterIndex].ButtonPressed = true;
		AddCharacter(_data.Characters[characterIndex].playerInformation, characterIndex);
		if(_selectedCharacterForMission.Count == 3)
		{
			_embark.Disabled = false;
			DisableCharacters();
		}
	}
	
	private void DisableCharacters()
	{
		for(int i=0; i<characterButtons.Count; i++)
		{
			if (characterButtons[i].IsVisibleInTree() && !AlreadySelected(characterButtons[i].characterIndex))
			{
				characterButtons[i].portrait.Modulate = Colors.Gray;
				characterButtons[i].Disabled = true;
			}
		}
	}
	
	private bool AlreadySelected(int characterIndex)
	{
		if (_selectedCharacterIndexForMission != null)
		{
			bool result = _selectedCharacterIndexForMission.Contains(characterIndex);
			return result;
		}

		return false;
	}
	
	public void RemoveCharacter(int charIndex)
	{
		foreach (PortraitButton x in portraitButtons)
		{
			if (x.characterIndex == charIndex)
			{
				Remove(x);
				break;
			}
		}
		_embark.Disabled = true;
	}
	
	private void Remove(PortraitButton x)
	{
		if(x.characterIndex != -1)
		{
			x.characterImage.Texture = null;
			x.playerInformation = null;
			x.characterIndex = -1;
			Reorder();
		}
	}
	
	private void Reorder()
	{
		foreach (PortraitButton x in portraitButtons)
		{
			if (x.characterIndex != -1)
			{
				PlayerInformationData tempInformation = x.playerInformation;
				int tempIndex = x.characterIndex;
				x.characterImage.Texture = null;
				x.playerInformation = null;
				x.characterIndex = -1;
				AddCharacter(tempInformation, tempIndex);
			}
		}
	}
	
	public void RemoveCharacterFromTeam(int characterIndex)
	{
		if (characterIndex > -1)
		{
			int index = _selectedCharacterIndexForMission.IndexOf(characterIndex);
			_selectedCharacterForMission.RemoveAt(index);
			_selectedCharacterIndexForMission.RemoveAt(index);
			RemoveCharacter(characterIndex);
		}

		EnableCharacters();
	}
	
	private void EnableCharacters()
	{
		for(int i=0; i<characterButtons.Count; i++)
		{
			if (characterButtons[i].IsVisibleInTree())
			{
				characterButtons[i].portrait.Modulate = Colors.White;
				characterButtons[i].Disabled = false;
			}
		}
	}
	
	public void OnCharacterButtonClick(int characterIndex)
	{

		if (!AlreadySelected(characterIndex) && (_selectedCharacterForMission == null || _selectedCharacterForMission.Count < 3))
		{
			AddCharacterToTeam(characterIndex);
		}
		else if (AlreadySelected(characterIndex))
		{
			RemoveCharacterFromTeam(characterIndex);
		}

	}
	
	public void OnClickTeamCharacterButton(int buttonIndex)
	{
		// if (!characterSelectOpen)
		// {
		//     characterSelectOpen = true;
		//     characterSelectView.OpenView();
		// }
		// else
		// {
		//     RemoveCharacterFromTeam(portraitButtons[buttonIndex].characterIndex);
		// }
		
		if (!_characterSelectOpen)
		{
			_characterSelectView.OpenView();
			_characterSelectOpen = true;
		}
		else
		{
			_characterSelectView.ExitView();
			_characterSelectOpen = false;
		}
	}

	
	public void OrderSaveData()
	{
		foreach (SavedCharacterResource character in _selectedCharacterForMission)
		{
			_data.Characters.Remove(character);
			_data.Characters.Insert(0, character);
		}
		_data.townData.allowEnemySelection = true;
		_data.townData.allowDuplicates = false;
	}
	
	private AtlasTexture NewAtlas(CompressedTexture2D compressedTexture, Rect2 rect)
	{
		AtlasTexture atlas = new AtlasTexture();
		atlas.Region = rect;
		atlas.Atlas = compressedTexture;
		
		return atlas;
	}
	
}
