using System.Collections;
using Godot;
using Godot.Collections;

public partial class PortraitBar : TextureRect
{
	[Export]
	public Array<CharacterPortrait> townPortraits;
	[Export]
	public Array<Control> townPortraitsContainers;
	[Export]
	public Control up;
	[Export]
	public Control down;
	
	private Data _data;
	private int _lastElement = -1; // array starts from zero 
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
		if (_scrollCharacterSelectIndex + townPortraits.Count < _data.Characters.Count)
		{
			down.Show();
		}

		if (_data.Characters.Count > 0)
		{
			for (int i = 0; i < _data.Characters.Count; i++)
			{
				if (i < townPortraits.Count)
				{
					UpdatePortrait(i, i);
					_lastElement = i;
					// townPortraits[i] = _currentCharacters[i].
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
	}

	public void InsertCharacter()
	{
		int index = _lastElement + 1;
		if (index < _data.Characters.Count && index < townPortraits.Count)
		{
			UpdatePortrait(index, index);
			_lastElement = index;
			
		}
		else if (_scrollCharacterSelectIndex + townPortraits.Count < _data.Characters.Count)
		{
			down.Show();
		}
	}
	
	public void RemoveCharacter(int characterIndex)
	{
		int index = characterIndex-_scrollCharacterSelectIndex;
		CharacterPortrait characterPortrait = townPortraits[index];

		if(_data.Characters.Count < _page * townPortraits.Count && _data.Characters.Count >= (_page - 1) * townPortraits.Count )
		{
			townPortraits.RemoveAt(index);
			int siblingIndex = townPortraits[townPortraits.Count - 1].GetIndex();
			MoveChild(characterPortrait, siblingIndex);
			townPortraitsContainers[index].Hide();
			townPortraits.Add(characterPortrait);
		}
		
		float countOfCharacters = _data.Characters.Count - (_page - 1) * townPortraits.Count ;
		float countToUpdate = countOfCharacters > townPortraits.Count ? townPortraits.Count : countOfCharacters;
		for (int i = index, count = characterIndex; i < countToUpdate; i++, count++)
		{
			UpdatePortrait(i, count);
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
		GD.Print(portraitIndex);
		if (index >= 0)
		{
			townPortraitsContainers[portraitIndex].Show();
			townPortraits[portraitIndex].characterIndex = index;

			StyleBoxTexture sprite = NewTexture(_data.Characters[index].playerInformation.CharacterPortraitSprite,
				Colors.White);
			townPortraits[portraitIndex].characterImage.AddThemeStyleboxOverride("normal", sprite);
			
			townPortraits[portraitIndex].levelText.Text = _data.Characters[index].level.ToString();
			if (_data.Characters[index].toConfirmAbilities > 0 || _data.Characters[index].abilityPointCount > 0)
			{
				townPortraits[portraitIndex].abilityPointCorner.Show();
			}
			else
			{
				townPortraits[portraitIndex].abilityPointCorner.Hide();
			}
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
			up.Show();
		}
		else
		{
			up.Hide();
		}
		

		GD.Print(Mathf.Abs(scrollCalculation) + townPortraits.Count);
		if (_data.Characters.Count > townPortraits.Count * _page && townPortraits.Count * _page <= _data.Characters.Count)
		{
			down.Show();
		}
		else if (scrollCalculation <= _data.Characters.Count)
		{
			down.Hide();
		}
		else if (scrollCalculation >= _data.Characters.Count)
		{
			down.Hide();
		}
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

}
