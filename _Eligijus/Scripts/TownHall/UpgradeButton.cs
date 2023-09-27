using System;
using System.Collections;
using System.Collections.Generic;
using Godot;

public partial class UpgradeButton : Button
{
	[Export]
	public TownHall townHall;
	[Export]
	public Texture UpgradedSprite;
	[Export]
	public UpgradeData upgradeData;
	[Export]
	public Button frame;
	[Export]
	public Button textColor;
	[Export]
	public Control animationObject;
	[Export]
	public Button button;
	private Data _data;

	public override void _Ready()
	{
		base._Ready();
		if (_data == null && Data.Instance != null)
		{
			_data = Data.Instance;
		}
		animationObject.Show();
		UpdateUpgradeButton();
	}

	public void SelectUpgrade()
	{
		townHall.SelectUpgrade(this);
	}

	public void UpdateUpgradeButton()
	{
		
			TownHallDataResource townHall = _data.townData.townHall;
			if (townHall.GetByType((TownHallUpgrade)upgradeData.upgradeIndex) + 1 <
				upgradeData.upgradeValue) //negalimi pirkti nes per auksti
			{
				button.Disabled = true;
			}
			else if (townHall.GetByType((TownHallUpgrade)upgradeData.upgradeIndex) + 1 >
					 upgradeData.upgradeValue) //nupirkti
			{
				button.Disabled = true;

				AtlasTexture atlasTexture = NewAtlasTexture(UpgradedSprite, Colors.White);
				StyleBoxTexture styleBoxTexture = new StyleBoxTexture();
				styleBoxTexture.Texture = atlasTexture;
				styleBoxTexture.ModulateColor = Colors.White;
				frame.AddThemeStyleboxOverride("normal", styleBoxTexture);
				frame.AddThemeStyleboxOverride("hover", styleBoxTexture);
				frame.AddThemeStyleboxOverride("pressed", styleBoxTexture);
				frame.AddThemeStyleboxOverride("disabled", styleBoxTexture);

				textColor.AddThemeColorOverride("font_color", Colors.White);
				textColor.AddThemeColorOverride("font_pressed_color", Colors.White);
				textColor.AddThemeColorOverride("font_hover_color", Colors.White);
				textColor.AddThemeColorOverride("font_focus_color", Colors.White);
				textColor.AddThemeColorOverride("font_disabled_color", Colors.White);
			}
			else
			{
				button.Disabled = false;
			} //galimas pirkti
		
	}
	
	
	private AtlasTexture NewAtlasTexture(Texture spriteTexture, Color pressedColor)
	{
		AtlasTexture atlas = new AtlasTexture();
		atlas.Region = (Rect2)spriteTexture.Get("region");
		atlas.Atlas = (CompressedTexture2D)spriteTexture.Get("atlas");
		
		return atlas;
	}
}

