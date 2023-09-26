using System.Collections;
using MonoCustomResourceRegistry;
using Godot;
using Godot.Collections;

[RegisteredType(nameof(PlayerInformationData), "", nameof(Resource))]
public partial class PlayerInformationData: Resource
{
	[Export]
	public CharacterType characterType;
	[Export]
	public string ClassName;
	[Export]
	public int MaxHealth = 100;
	[Export]
	public int critChance = 5;
	[Export]
	public int accuracy = 100;
	[Export]
	public int dodgeChance = 20;
	[Export]
	public string role;
	
	[Export]
	public Color classColor;
	[Export]
	public Color secondClassColor;
	[Export]
	public Color textColor;
	[Export]
	public Color backgroundColor;
	
	[Export]
	public Texture CharacterPortraitSprite;
	[Export]
	public Texture CharacterSplashArt;//For character table
	[Export]
	public Texture CroppedSplashArt;
	[Export]
	public Texture characterSprite;
	[Export]
	public Texture roleSprite;
	[Export]
	public Array<TextureResource> abilities;
	[Export]
	public Array<AbilityText> abilityTexts;
	[Export]
	public Array<Blessing> BlessingsAndCurses = new Godot.Collections.Array<Blessing>();

	public PlayerInformationData()
	{
		
	}

	public PlayerInformationData(CharacterType characterType, string ClassName, int MaxHealth, int critChance, int accuracy,
	int dodgeChance, string role, Color classColor, Color secondClassColor, Color textColor,
	Color backgroundColor, Texture CharacterPortraitSprite, Texture CharacterSplashArt,//For character table
	Texture CroppedSplashArt, Texture characterSprite, Texture roleSprite,
	Godot.Collections.Array<TextureResource> abilities, Godot.Collections.Array<Blessing> BlessingsAndCurses)
	{
		this.characterType = characterType;
		this.ClassName = ClassName;
		this.MaxHealth = MaxHealth;
		this.critChance = critChance;
		this.accuracy = accuracy;
		this.dodgeChance = dodgeChance;
		this.role = role;

		this.classColor = classColor;
		this.secondClassColor = secondClassColor;
		this.textColor = textColor;
		this.backgroundColor = backgroundColor;

		this.CharacterPortraitSprite = CharacterPortraitSprite;
		this.CharacterSplashArt = CharacterSplashArt;
		this.CroppedSplashArt = CroppedSplashArt;
		this.characterSprite = characterSprite;
		this.roleSprite = roleSprite;
		this.abilities = abilities;

		this.BlessingsAndCurses = BlessingsAndCurses;
	}
	
	public PlayerInformationData(PlayerInformationData playerInformationData)
	{
		characterType = playerInformationData.characterType;
		ClassName = playerInformationData.ClassName;
		MaxHealth = playerInformationData.MaxHealth;
		critChance = playerInformationData.critChance;
		accuracy = playerInformationData.accuracy;
		dodgeChance = playerInformationData.dodgeChance;
		role = playerInformationData.role;

		classColor = playerInformationData.classColor;
		secondClassColor = playerInformationData.secondClassColor;
		textColor = playerInformationData.textColor;
		backgroundColor = playerInformationData.backgroundColor;

		CharacterPortraitSprite = playerInformationData.CharacterPortraitSprite;
		CharacterSplashArt = playerInformationData.CharacterSplashArt;
		CroppedSplashArt = playerInformationData.CroppedSplashArt;
		characterSprite = playerInformationData.characterSprite;
		roleSprite = playerInformationData.roleSprite;
		abilities = playerInformationData.abilities;

		BlessingsAndCurses = playerInformationData.BlessingsAndCurses;
	}
	
	public void CopyData(PlayerInformationData playerInformationData)
	{
		characterType = playerInformationData.characterType;
		ClassName = playerInformationData.ClassName;
		MaxHealth = playerInformationData.MaxHealth;
		critChance = playerInformationData.critChance;
		accuracy = playerInformationData.accuracy;
		dodgeChance = playerInformationData.dodgeChance;
		role = playerInformationData.role;

		classColor = playerInformationData.classColor;
		secondClassColor = playerInformationData.secondClassColor;
		textColor = playerInformationData.textColor;
		backgroundColor = playerInformationData.backgroundColor;

		CharacterPortraitSprite = playerInformationData.CharacterPortraitSprite;
		CharacterSplashArt = playerInformationData.CharacterSplashArt;
		CroppedSplashArt = playerInformationData.CroppedSplashArt;
		characterSprite = playerInformationData.characterSprite;
		roleSprite = playerInformationData.roleSprite;
		abilities = playerInformationData.abilities;

		BlessingsAndCurses = playerInformationData.BlessingsAndCurses;
	}

}




