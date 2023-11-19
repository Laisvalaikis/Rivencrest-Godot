using System;
using System.Collections;
using System.Collections.Generic;
using Godot;

public partial class RecruitButton : TextureRect
{
	[Export] 
	public Color hover;
	[Export]
	public Button portrait;
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
			StyleBoxTexture styleBox = NewTexture(charInformation, Colors.White);
			StyleBoxTexture styleBoxPressed = NewTexture(charInformation, hover);
			portrait.AddThemeStyleboxOverride("normal", styleBox);
			portrait.AddThemeStyleboxOverride("hover", styleBox);
			portrait.AddThemeStyleboxOverride("pressed", styleBoxPressed);
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

	private StyleBoxTexture NewTexture(PlayerInformationDataNew playerInformationData, Color pressedColor)
	{
		StyleBoxTexture styleBox = new StyleBoxTexture();
		AtlasTexture atlas = new AtlasTexture();
		atlas.Region = (Rect2)playerInformationData.CharacterPortraitSprite.Get("region");
		atlas.Atlas = (CompressedTexture2D)playerInformationData.CharacterPortraitSprite.Get("atlas");
		styleBox.Texture = atlas;
		styleBox.ModulateColor = pressedColor;
		

		return styleBox;
	}


}
