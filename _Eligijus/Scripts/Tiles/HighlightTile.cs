using System;
using System.Collections;
using System.Collections.Generic;
using Godot;

public partial class HighlightTile : Node2D
{
	[Export] private Sprite2D mouseHighlight;
	[Export] private Sprite2D highlight;
	[Export] private Sprite2D arrowTile;
	[Export] private Sprite2D playerSelect;
	[Export] private Sprite2D preview;
	[Export] private Sprite2D sideArrows;
	
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
	
	[Export] private AtlasTexture everyArrow;
	[Export] private AtlasTexture withoutRightArrow;
	[Export] private AtlasTexture withoutLeftEndArrow;
	[Export] private AtlasTexture withoutDownEndArrow;
	[Export] private AtlasTexture withoutUpEndArrow;

	[Export] private Sprite2D skullSprite;
	[Export] private Resource textTilePrefab;
	[Export] private Label damageText;

	[Export] private AtlasTexture characterSelected;
	[Export] private AtlasTexture characterNotSelected;
	
	public bool isHighlighted = false;
	private bool _tileWasEnabled = false;
	private bool _controllerSelectWasEnabled = false;
	public override void _Ready()
	{
		base._Ready();
		PackedScene textTile = (PackedScene)textTilePrefab;
		Label label = (Label)textTile.Instantiate();
		CallDeferred("add_child", label);
		CallDeferred("move_child", label, 0);
		label.ZIndex = 1;
		damageText = label;
		// MoveChild(damageText, 1);
		label.Hide();

	}

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

	public void ActivateSideArrows(bool activate)
	{
		if (activate)
		{
			sideArrows.Show();
		}
		else
		{
			sideArrows.Hide();
		}
	}

	public void SetDamageText(string text)
	{
		// if (damageText.GetIndex() != 1)
		// {
		// 	MoveChild(damageText, 1);
		// }

		damageText.Show();
		damageText.Text = text;
	}
	public void DisableDamageText()
	{
		damageText.Hide();
	}

	public void EnableMouseSelector()
	{
		mouseHighlight.Show();
	}

	public void DisableMouseSelector()
	{
		mouseHighlight.Hide();
	}


	public void ActivateColorGridTile(bool value)
	{
		if (value)
		{
			highlight.Show();
		}
		else
		{
			highlight.Hide();
		}
		isHighlighted = value;
	}

	public bool TileWasEnabled()
	{
		return _tileWasEnabled;
	}

	public void EnableTile(bool value)
	{
		if (value)
		{
			Show();
		}
		else
		{
			Hide();
		}
		_tileWasEnabled = value;
	}

	public void EnableControllerSelectTile(bool value)
	{
		if (value)
		{
			Show();
		}
		else
		{
			Hide();
		}
		_controllerSelectWasEnabled = value;
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

	public void ToggleSelectedPlayerUI(bool selected)
	{
		if (selected)
		{
			playerSelect.Texture = characterSelected;
		}
		else
		{
			playerSelect.Texture = characterNotSelected;
		}
	}
	
	public void SetSideArrowsSprite(int arrowType)
	{
		AtlasTexture arrowSprite = null;

		switch (arrowType)
		{
			case 0:
				arrowSprite = everyArrow;
				break;
			case 1:
				arrowSprite = withoutRightArrow;
				break;
			case 2:
				arrowSprite = withoutLeftEndArrow;
				break;
			case 3:
				arrowSprite = withoutDownEndArrow;
				break;
			case 4:
				arrowSprite = withoutUpEndArrow;
				break;
			default:
				return;
		}
		sideArrows.Texture = arrowSprite;
	}
	
	public void SetArrowSprite(int arrowType)
	{
		AtlasTexture arrowSprite;

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
		arrowTile.Hide();
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

	public void DisableHighlight()
	{
		DeactivateArrowTile();
		DisableDamageText();
		ActivateDeathSkull(false);
		ActivateSideArrows(false);
		ActivateColorGridTile(false);
		EnableTile(false);
		TogglePreviewSprite(false);
		ActivatePlayerTile(false);
	}

	public void SetHighlightColor(Color color)
	{
		highlight.SelfModulate = color;
	}
}
