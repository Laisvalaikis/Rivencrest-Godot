using Godot;
using Godot.Collections;

public partial class CharacterInspectInShop : Node
{
	[Export] 
	private TextureRect characterArt;
	[Export] 
	private TextureRect tableBorder;
	[Export] 
	private Label className;
	[Export] 
	private Label role;
	[Export] 
	private Label maxHP;
	[Export] 
	private Label range;
	[Export] 
	private Label level;
	[Export] 
	private Control damage;
	[Export] 
	private Control tank;
	[Export] 
	private Control support;
	[Export] 
	private Control meleeType;
	[Export] 
	private Control rangeType;
	[Export] 
	private Array<CharacterAbilityRecruit> _characterAbilityRecruits;
	[Export] 
	private Recruitment recruitment;
	[Export] 
	private HelpTable helpTable;
	[Export] 
	private View view;
	private int _currentCharacterIndex = 0;
	private int _currentCharacterInShop;

	public void DisableCharacterOnBuy(int index)
	{
		if (index == _currentCharacterInShop)
		{
			view.ExitView();
		}
	}

	public void EnableDisableView(int index)
	{
		if (view.IsVisibleInTree() && index == _currentCharacterInShop)
		{
			view.ExitView();
		}
		else
		{
			UpdateView(index);
		}
	}

	private void UpdateView(int index)
	{
		SavedCharacterResource savedCharacter = recruitment.GetInShopCharacterByIndex(index);
		recruitment.SetSelectedCharacterIndex(index);
		_currentCharacterInShop = index;
		_currentCharacterIndex = recruitment.GetRealCharacterIndex(index);
		PlayerInformationDataNew character = savedCharacter.playerInformation;
		view.OpenViewCurrentButton(view.GetPathTo(recruitment.iconButtons[index]));
		view.OpenView();
		AtlasTexture styleBoxTexture = NewAtlas((CompressedTexture2D)character.CroppedSplashArt.Get("atlas"), (Rect2)character.CroppedSplashArt.Get("region"));
		characterArt.Texture = styleBoxTexture;
		tableBorder.SelfModulate = character.secondClassColor;
		className.Text = character.ClassName;
		className.LabelSettings.FontColor = character.classColor;
		role.Text = character.role;
		role.LabelSettings.FontColor = character.classColor;
		if (character.role == "DAMAGE")
		{
			damage.Show();
		}
		else
		{
			damage.Hide();
		}

		if (character.role == "TANK")
		{
			tank.Show();
		}
		else
		{
			tank.Hide();
		}

		if (character.role == "SUPPORT")
		{
			support.Show();
		}
		else
		{
			support.Hide();
		}

		maxHP.Text = character.maxHealth + " HP";
		maxHP.LabelSettings.FontColor = character.classColor;
		bool melee = character.characterType == CharacterType.Melee;
		range.Text = melee ? "MELEE" : "RANGED";
		range.LabelSettings.FontColor = character.classColor;
		if (melee)
		{
			meleeType.Show();
			rangeType.Hide();
		}
		else
		{
			meleeType.Hide();
			rangeType.Show();
		}

		level.Text = "LEVEL " + savedCharacter.level;
		level.LabelSettings.FontColor = character.classColor;
		for (int i = 0; i < _characterAbilityRecruits.Count; i++)
		{
			_characterAbilityRecruits[i].Show();
			_characterAbilityRecruits[i].backgroundImage.SelfModulate = character.backgroundColor;
			_characterAbilityRecruits[i].abilityIcon.SelfModulate = character.classColor;
			_characterAbilityRecruits[i].ability = character.abilities[i];
			AtlasTexture atlasTexture = NewTexture(character.abilities[i].AbilityImage, Colors.White);
			_characterAbilityRecruits[i].abilityIcon.Texture = atlasTexture;
		}
		
	}
	
	private AtlasTexture NewTexture(Texture playerInformationData, Color pressedColor)
	{
		AtlasTexture atlas = new AtlasTexture();
		atlas.Region = (Rect2)playerInformationData.Get("region");
		atlas.Atlas = (CompressedTexture2D)playerInformationData.Get("atlas");

		return atlas;
	}
	
	public void EnableDisableHelpTable(int abilityIndex)
	{
		Vector2 currentPosition = helpTable.helpTableView.GlobalPosition;
		float y = _characterAbilityRecruits[abilityIndex].GlobalPosition.Y;
		y = y + (_characterAbilityRecruits[abilityIndex].Size.Y / 2f);
		y = y - (helpTable.helpTableView.Size.Y / 2f);
		Vector2 position = new Vector2(currentPosition.X, y);
		helpTable.helpTableView.SetGlobalPosition(position);
		helpTable.UpdateHelpTable(_characterAbilityRecruits[abilityIndex].ability);
	}
	
	private StyleBoxTexture NewTexture(CompressedTexture2D compressedTexture, Rect2 rect, Color pressedColor)
	{
		StyleBoxTexture styleBox = new StyleBoxTexture();
		AtlasTexture atlas = new AtlasTexture();
		atlas.Region = rect;
		atlas.Atlas = compressedTexture;
		styleBox.Texture = atlas;
		styleBox.ModulateColor = pressedColor;
		
		return styleBox;
	}
	
	private AtlasTexture NewAtlas(CompressedTexture2D compressedTexture, Rect2 rect)
	{
		AtlasTexture atlas = new AtlasTexture();
		atlas.Region = rect;
		atlas.Atlas = compressedTexture;
		
		return atlas;
	}
}

