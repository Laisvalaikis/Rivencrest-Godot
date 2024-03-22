using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

public partial class PortraitBar : TextureRect
{
	[Export]
	public Array<CharacterPortrait> townPortraits;
	[Export]
	public Array<Control> townPortraitsContainers;
	[Export]
	public Button focusLeft;
	[Export]
	public Button focusUp;
	[Export]
	public Button focusDown;
	
	[Export]
	public Control previousPageControl;
	[Export]
	public Button previousPageButton;
	[Export]
	public Control nextPageControl;
	[Export]
	public Button nextPageButton;
	private List<CharacterPortrait> enabledPortraits = new List<CharacterPortrait>();
	
	private Data _data;
	private int _lastElement = -1; // array starts from zero 
	private int _lastButtonIndex = -1;
	private int _scrollCharacterSelectIndex;
	private int _page = 1;

	// Start is called before the first frame update

	public override void _Ready()
	{
		base._Ready();
		_data = Data.Instance;
		_scrollCharacterSelectIndex = 0;
		SetupCharacters();
	}

	public void SetupCharacters()
	{
		if (_data.Characters.Count > 0)
		{
			townPortraits[0].FocusNeighborTop = focusUp.GetPath();
			focusUp.FocusNeighborBottom = townPortraits[0].GetPath();
			focusLeft.FocusNeighborRight = townPortraits[0].GetPath();
			nextPageButton.FocusNeighborLeft = focusLeft.GetPath();
			previousPageButton.FocusNeighborLeft = focusLeft.GetPath();
			
			for (int i = 0; i < _data.Characters.Count; i++)
			{
				if (i < townPortraits.Count)
				{
					UpdatePortrait(i, i);
					_lastElement = i;
					_lastButtonIndex = i;
				}
				else
				{
					break;
				}
			}
			
		}
		else
		{
			for (int i = 0; i < townPortraits.Count; i++)
			{
				UpdatePortrait(i, -1);
				_lastElement = -1;
			}
		}
		
		if (_scrollCharacterSelectIndex + townPortraits.Count < _data.Characters.Count)
		{
			nextPageControl.Show();
			townPortraits[^1].FocusNeighborBottom = nextPageButton.GetPath();
			nextPageButton.FocusNeighborTop = townPortraits[^1].GetPath();
			nextPageButton.FocusNeighborBottom = focusDown.GetPath();
			focusDown.FocusNeighborTop = nextPageButton.GetPath();
		}
		else
		{
			if (_lastButtonIndex == -1) return;
			townPortraits[_lastButtonIndex].FocusNeighborBottom = focusDown.GetPath();
			focusDown.FocusNeighborTop = townPortraits[_lastButtonIndex].GetPath();
		}
		
		
	}

	public void ToggleSelectButton(int index)
	{
		townPortraits[index].ButtonPressed = true;
	}
	
	public void ToggleDeSelectButton(int index)
	{
		townPortraits[index].ButtonPressed = false;
	}

	public Button GetPortraitButtonByIndex(int index)
	{
		return townPortraits[index];
	}

	public void InsertCharacter()
	{
		int index = _lastElement + 1;
		if (index < _data.Characters.Count && index < townPortraits.Count)
		{
			// GD.Print(_data.Characters[index].playerInformation.ResourceName);
			UpdatePortrait(index, index);
			_lastElement = index;
			_lastButtonIndex = index;
			townPortraits[_lastButtonIndex].FocusNeighborBottom = focusDown.GetPath();
			focusDown.FocusNeighborTop = townPortraits[_lastButtonIndex].GetPath();
		}
		else if (_scrollCharacterSelectIndex + townPortraits.Count < _data.Characters.Count)
		{
			nextPageControl.Show();
			
			townPortraits[_lastButtonIndex].FocusNeighborBottom = nextPageButton.GetPath();
			nextPageButton.FocusNeighborTop = townPortraits[_lastButtonIndex].GetPath();
			nextPageButton.FocusNeighborBottom = focusDown.GetPath();
			focusDown.FocusNeighborTop = nextPageButton.GetPath();
		}
	}
	
	public void RemoveCharacter(int characterIndex)
	{
		int index = characterIndex-_scrollCharacterSelectIndex;
		CharacterPortrait characterPortrait = townPortraits[index];
		Control characterPortraitContainer = townPortraitsContainers[index];
		if(_data.Characters.Count < _page * townPortraits.Count && _data.Characters.Count >= (_page - 1) * townPortraits.Count )
		{
			townPortraitsContainers.RemoveAt(index);
			townPortraits.RemoveAt(index);
			int siblingIndex = townPortraitsContainers[townPortraits.Count - 1].GetIndex();
			characterPortraitContainer.GetParent().MoveChild(characterPortraitContainer, siblingIndex);
			characterPortraitContainer.Hide();
			townPortraitsContainers.Add(characterPortraitContainer);
			townPortraits.Add(characterPortrait);
		}
		
		float countOfCharacters = _data.Characters.Count - (_page - 1) * townPortraits.Count ;
		float countToUpdate = countOfCharacters > townPortraits.Count ? townPortraits.Count : countOfCharacters;
		for (int i = index, count = characterIndex; i < countToUpdate; i++, count++)
		{
			UpdatePortrait(i, count);
			_lastButtonIndex = i;
		}
		
		UpdateArrows(_scrollCharacterSelectIndex * -1);
		
		if ( (_page - 1) * townPortraits.Count >= _data.Characters.Count)
		{
			Scroll(-1);
		}
		
		

		_lastElement--;
		
	}

	public void Scroll(int direction) // up - (-1), down - (1)
	{
		int scrollCalculation = _scrollCharacterSelectIndex + townPortraits.Count * direction;
		if (townPortraits.Count <= _data.Characters.Count && scrollCalculation >= 0 && scrollCalculation < _data.Characters.Count)
		{
			_scrollCharacterSelectIndex += townPortraits.Count * direction;
			int count = _data.Characters.Count - _scrollCharacterSelectIndex;
			for (int i = 0; i < townPortraits.Count; i++)
			{
				if (i < count)
				{
					int index = i + _scrollCharacterSelectIndex;
					UpdatePortrait(i, index);
					_lastButtonIndex = i;
				}
				else
				{
					townPortraitsContainers[i].Hide();
				}
			}

			_lastElement = _scrollCharacterSelectIndex + count;
			_page += direction;
		}

		UpdateArrows(scrollCalculation);
	}

	public void ScrollUpByCharacterIndex(int characterIndex)
	{
		
		if (characterIndex >= _page * townPortraits.Count)
		{
			Scroll(1);
		}
		
	}

	public void ScrollDownByCharacterIndex(int characterIndex)
	{
		if (characterIndex < (_page-1) * townPortraits.Count)
		{
			Scroll(-1);
		}
	}

	public void UpdatePortrait(int portraitIndex, int index)
	{
		
		if (index >= 0)
		{
			
			townPortraitsContainers[portraitIndex].Show();
			townPortraits[portraitIndex].characterIndex = index;
			
			townPortraits[portraitIndex].characterImage.Texture = NewAtlasTexture(_data.Characters[index].playerInformation.CharacterPortraitSprite,
				Colors.White);
			
			townPortraits[portraitIndex].levelText.Text = _data.Characters[index].level.ToString();
			if (_data.Characters[index].toConfirmAbilities > 0 || _data.Characters[index].abilityPointCount > 0)
			{
				townPortraits[portraitIndex].abilityPointCorner.Show();
			}
			else
			{
				townPortraits[portraitIndex].abilityPointCorner.Hide();
			}

			// focus
			if (portraitIndex > 0)
			{
				townPortraits[portraitIndex].FocusNeighborTop = townPortraits[portraitIndex-1].GetPath();
				townPortraits[portraitIndex-1].FocusNeighborBottom = townPortraits[portraitIndex].GetPath();
			}
			townPortraits[portraitIndex].FocusNeighborLeft = focusLeft.GetPath();

		}
		else
		{
			townPortraitsContainers[portraitIndex].Hide();
		}
	}

	public void UpdateArrows(int scrollCalculation)
	{
		if (_data.Characters.Count > townPortraits.Count * (_page - 1) && _page - 1 > 0)
		{
			previousPageControl.Show();
			
			townPortraits[0].FocusNeighborTop = previousPageButton.GetPath();
			previousPageButton.FocusNeighborBottom = townPortraits[0].GetPath();
			previousPageButton.FocusNeighborTop = focusUp.GetPath();
			focusUp.FocusNeighborBottom = previousPageButton.GetPath();
			townPortraits[0].GrabFocus();
			focusUp.FocusNeighborLeft = focusLeft.GetPath();
		}
		else
		{
			previousPageControl.Hide();
			townPortraits[0].FocusNeighborTop = focusUp.GetPath();
			focusUp.FocusNeighborBottom = townPortraits[0].GetPath();
			townPortraits[0].GrabFocus();
		}
		

		GD.Print(Mathf.Abs(scrollCalculation) + townPortraits.Count);
		if (_data.Characters.Count > townPortraits.Count * _page && townPortraits.Count * _page <= _data.Characters.Count)
		{
			nextPageControl.Show();
			townPortraits[_lastButtonIndex].FocusNeighborBottom = nextPageButton.GetPath();
			nextPageButton.FocusNeighborTop = townPortraits[_lastButtonIndex].GetPath();
			nextPageButton.FocusNeighborBottom = focusDown.GetPath();
			focusDown.FocusNeighborTop = nextPageButton.GetPath();
			focusUp.FocusNeighborLeft = focusLeft.GetPath();
		}
		else if (scrollCalculation <= _data.Characters.Count || scrollCalculation >= _data.Characters.Count)
		{
			nextPageControl.Hide();
			
			townPortraits[_lastButtonIndex].FocusNeighborBottom = focusDown.GetPath();
			focusDown.FocusNeighborTop = townPortraits[_lastButtonIndex].GetPath();
		}
		// else if (scrollCalculation >= _data.Characters.Count)
		// {
		// 	nextPageButton.Hide();
		// 	
		// 	townPortraits[_lastButtonIndex].FocusNeighborBottom = focusDown.GetPath();
		// 	focusDown.FocusNeighborTop = townPortraits[_lastButtonIndex].GetPath();
		// }
	}

	public void DisableAbilityCorner(int index)
	{
		int calculatedIndex = index-_scrollCharacterSelectIndex;
		townPortraits[calculatedIndex].abilityPointCorner.Hide();
	}

	public void EnableAbilityCorner(int index)
	{
		int calculatedIndex = index-_scrollCharacterSelectIndex;
		townPortraits[calculatedIndex].abilityPointCorner.Show();
	}
	
	private StyleBoxTexture NewTexture(Texture spriteTexture, Color pressedColor)
	{
		StyleBoxTexture styleBox = new StyleBoxTexture();
		AtlasTexture atlas = new AtlasTexture();
		atlas.Region = (Rect2)spriteTexture.Get("region");
		atlas.Atlas = (CompressedTexture2D)spriteTexture.Get("atlas");
		styleBox.Texture = atlas;
		styleBox.ModulateColor = pressedColor;
		

		return styleBox;
	}
	
	private AtlasTexture NewAtlasTexture(Texture spriteTexture, Color pressedColor)
	{
		AtlasTexture atlas = new AtlasTexture();
		atlas.Region = (Rect2)spriteTexture.Get("region");
		atlas.Atlas = (CompressedTexture2D)spriteTexture.Get("atlas");
		
		return atlas;
	}

}

