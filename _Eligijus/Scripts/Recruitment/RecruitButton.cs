using System;
using System.Collections;
using System.Collections.Generic;
using Godot;

public partial class RecruitButton : TextureRect
{
	[Export]
	public TextureRect portrait;
	[Export]
	public Button buyButton;
	[Export]
	public Label className;
	[Export]
	public Label cost;
	[Export]
	public SavedCharacterResource character;
	private int XPToLevelUp;
	private Data _data;

	public override void _Ready()
	{
		base._Ready();
		if(_data == null && Data.Instance != null)
		{
			_data = Data.Instance;
		}
	}

	public void UpdateRecruitButton()
	{
		PlayerInformationDataNew charInformation = character.playerInformation;
			className.Text = charInformation.ClassName;
			className.LabelSettings.FontColor = charInformation.classColor;
			portrait.Texture = (Texture2D)charInformation.CharacterPortraitSprite;
			// AtlasTexture atlas = new AtlasTexture();
			// atlas.Region = (Rect2)charInformation.CharacterPortraitSprite.Get("region");
			// atlas.Atlas = (CompressedTexture2D)charInformation.CharacterPortraitSprite.Get("atlas");
			cost.Text = character.cost.ToString() + "g";
			if (_data.townData.townGold >= character.cost && _data.Characters.Count < _data.maxCharacterCount)
			{
				buyButton.Disabled = false;
			}
			else
			{
				buyButton.Disabled = true;
			}
	}

	


}
