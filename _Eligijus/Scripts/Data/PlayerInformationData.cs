using MonoCustomResourceRegistry;
using Godot;
using Godot.Collections;
using Array = System.Array;

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
	public Array<Ability> baseAbilities;
	[Export]
	public Array<Ability> abilities;
	[Export]
	protected Array<PlayerBlessing> blessingsAndCurses;

	protected Array<PlayerBlessing> _unlockedBlessings;

	public PlayerInformationData()
	{
		
	}

	public PlayerInformationData(CharacterType characterType, string ClassName, int MaxHealth, int critChance, int accuracy,
	int dodgeChance, string role, Color classColor, Color secondClassColor, Color textColor,
	Color backgroundColor, Texture CharacterPortraitSprite, Texture CharacterSplashArt,//For character table
	Texture CroppedSplashArt, Texture characterSprite, Texture roleSprite,
	Array<Ability> abilities, Array<PlayerBlessing> blessingsAndCurses)
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

		this.blessingsAndCurses = blessingsAndCurses;
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

		blessingsAndCurses = playerInformationData.blessingsAndCurses;
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

		blessingsAndCurses = playerInformationData.blessingsAndCurses;
	}

}




