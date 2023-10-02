using System;
using System.Collections;
using System.Collections.Generic;
using Godot;

public partial class HighlightTile : Node2D
{
	[Export] private Node2D colorGridTile;
	[Export] private Node2D fogOfWarTile;
	[Export] private Sprite2D arrowTile;
	[Export] private Node2D dangerUI;
	[Export] private Node2D playerSelect;
	[Export] private Sprite2D preview;
	
	[Export] private AtlasTexture rightStartArrow;
	[Export] private AtlasTexture leftStartArrow;
	[Export] private AtlasTexture downStartArrow;
	[Export] private AtlasTexture upStartArrow;

	[Export] private AtlasTexture rightEndArrow;
	[Export] private AtlasTexture leftEndArrow;
	[Export] private AtlasTexture downEndArrow;
	[Export] private AtlasTexture upEndArrow;

	[Export] private AtlasTexture verticalIntermediateArrow;
	[Export] private AtlasTexture horizontalIntermediateArrow;

	[Export] private AtlasTexture topLeftCornerArrow;
	[Export] private AtlasTexture bottomLeftCornerArrow;
	[Export] private AtlasTexture topRightCornerArrow;
	[Export] private AtlasTexture bottomRightCornerArrow;

	[Export] private Sprite2D skullSprite;
	[Export] private Label damageText;
	
	public bool isHighlighted = false;
	public string activeState;
	
	
	public void ActivateDeathSkull(bool value)
	{
		if (value)
		{
			skullSprite.Show();
		}
		else
		{
			skullSprite.Hide();
		}
	}
	public void SetDamageText(string text)
	{
		damageText.Show();
		damageText.Text = text;
	}
	public void DisableDamageText()
	{
		damageText.Hide();
	}
	public void ActivateColorGridTile(bool value)
	{
		if (value)
		{
			colorGridTile.Show();
		}
		else
		{
			colorGridTile.Hide();
		}
		isHighlighted = value;
	}
	public void SetPreviewSprite(AtlasTexture sprite)
	{
		preview.Texture = sprite;
	}
	public void TogglePreviewSprite(bool value)
	{
		if (value)
		{
			preview.Show();
		}
		else
		{
			preview.Hide();
		}
	}
	public void SetArrowSprite(int arrowType)
	{
		AtlasTexture arrowSprite = null;

		switch (arrowType)
		{
			case 1:
				arrowSprite = rightStartArrow;
				break;
			case 2:
				arrowSprite = leftStartArrow;
				break;
			case 3:
				arrowSprite = downStartArrow;
				break;
			case 4:
				arrowSprite = upStartArrow;
				break;
			case 5:
				arrowSprite = rightEndArrow;
				break;
			case 6:
				arrowSprite = leftEndArrow;
				break;
			case 7:
				arrowSprite = downEndArrow;
				break;
			case 8:
				arrowSprite = upEndArrow;
				break;
			case 9:
				arrowSprite = verticalIntermediateArrow;
				break;
			case 10:
				arrowSprite = horizontalIntermediateArrow;
				break;
			case 11:
				arrowSprite = topLeftCornerArrow;
				break;
			case 12:
				arrowSprite = topRightCornerArrow;
				break;
			case 13:
				arrowSprite = bottomLeftCornerArrow;
				break;
			case 14:
				arrowSprite = bottomRightCornerArrow;
				break;
			default:
				return;
		}
		arrowTile.Texture = arrowSprite;
		arrowTile.Show();
	}
	public void DeactivateArrowTile()
	{
		arrowTile.Show();
	}
	public void ActivatePlayerTile(bool value)
	{
		if (value)
		{
			playerSelect.Show();
		}
		else
		{
			playerSelect.Hide();
		}
	}
	public void SetHighlightColor(Color color)
	{
		colorGridTile.SelfModulate = color;
	}

	bool IsSkillAvailableInFOW()
	{
		return ( activeState == "CreateEye" || activeState == "CreatePortal");
	}
}
