using System.Collections.Generic;
using Godot;
using Godot.Collections;


public partial class TownHall : Control
{
	[Export]
	public TextureRect imageComponent;
	[Export]
	public Array<UpgradeButton> upgradeButtons;
	[Export]
	public Label upgradeNameText;
	[Export]
	public Label upgradeDescriptionText;
	[Export]
	public Label upgradeCostText;
	[Export]
	public Button buyButton;
	[Export]
	public Control backgroundForText;
	[Export]
	public AnimationPlayer imageFadeController;
	[Export]
	public GameUi gameUi;
	[Export]
	public Array<Texture> sprites;
	private UpgradeButton SelectedUpgrade;
	private bool pause = false;
	private Data _data;
	private List<bool> buttonState;

	public override void _Draw()
	{
		base._Draw();
		if (_data == null)
		{
			_data = Data.Instance;
			buttonState = new List<bool>();
			for (int i = 0; i < upgradeButtons.Count; i++)
			{
				buttonState.Add(false);
			}
		}
		// _data.townData.hasClickedTH = true;
	}

	public void DisableAllButtons()
	{
		if (buttonState == null)
		{
			buttonState = new List<bool>();
		}
		else if (buttonState.Count > 0)
		{
			for (int i = 0; i < buttonState.Count; i++)
			{
				buttonState[i] = upgradeButtons[i].button.Disabled;
				upgradeButtons[i].button.Disabled = true;
				upgradeButtons[i].Pause(true);
			}
			pause = true;
		}
	}

	public void EnableAllButtons()
	{
		if (buttonState != null && buttonState.Count > 0)
		{
			for (int i = 0; i < upgradeButtons.Count; i++)
			{
				upgradeButtons[i].Pause(false);
				upgradeButtons[i].button.Disabled = buttonState[i];
				
			}
			pause = false;
		}
	}

	public void SetupMerchantSprite()
	{
		if (_data.townData.townHall.damagedMerchant == 1)
		{
			imageComponent.Texture.Set("atlas", sprites[0].Get("atlas"));
		}
		else if (_data.townData.townHall.damagedMerchant == 2)
		{
			imageComponent.Texture.Set("atlas", sprites[1].Get("atlas"));
		}
		backgroundForText.Show();
	}
	public void UpdateButtons()
	{
		

			foreach (UpgradeButton button in upgradeButtons)
			{
				if (button.IsVisibleInTree())
				{
					button.UpdateUpgradeButton();
				}
			}

			if (SelectedUpgrade != null)
			{
				upgradeNameText.Show();
				upgradeDescriptionText.Show();
				upgradeCostText.Show();
				backgroundForText.Show();

				upgradeNameText.Text = SelectedUpgrade.upgradeData.upgradeName;
				upgradeDescriptionText.Text = SelectedUpgrade.upgradeData.upgradeDescription;
				upgradeCostText.Text = "-" + SelectedUpgrade.upgradeData.upgradeCost.ToString() + "g";
				buyButton.Disabled = _data.townData.townGold < SelectedUpgrade.upgradeData.upgradeCost;
			}
			else
			{ 
				upgradeNameText.Hide();
				upgradeDescriptionText.Hide();
				upgradeCostText.Hide();
				buyButton.Hide();
			}
		
	}

	public void CloseTownHall()
	{
		SelectedUpgrade = null;
		pause = false;
		UpdateButtons();
	}
	
	public void BuyUpgrade()
	{
		if (!buyButton.Disabled)
		{
			TownHallDataResource townHall = _data.townData.townHall;
			townHall.SetByType((TownHallUpgrade)SelectedUpgrade.upgradeData.upgradeIndex, SelectedUpgrade.upgradeData.upgradeValue);
			GameManager.Instance.SpendGold(SelectedUpgrade.upgradeData.upgradeCost);
			gameUi.UpdateTownCost();
			UpdateButtons();
		}
	}
	public void SelectUpgrade(UpgradeButton upgradeButton)
	{
		if (!pause)
		{
			if (upgradeButton != null)
			{
				if (SelectedUpgrade == upgradeButton)
				{
					SelectedUpgrade = null;
					imageFadeController.Play("FadeOut");
				}
				else
				{
					SelectedUpgrade = upgradeButton;
					imageFadeController.Play("FadeIn");
				}
			}
			else
			{
				GD.PrintErr("Select upgrade null value");
				SelectedUpgrade = null;
				imageFadeController.Play("FadeOut");
			}

			// upgradeButton.UpdateUpgradeButton();
			UpdateButtons();
		}
	}
}
