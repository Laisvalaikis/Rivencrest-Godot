using Godot;
using Godot.Collections;
using Array = System.Array;


public partial class PlayerInformationDataNew: Resource
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
	public int killXP = 50;
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
	protected Array<PlayerBlessing> playerBlessings;

	public PlayerInformationData()
	{
		
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
		playerBlessings = playerInformationData.playerBlessings;
	}
	
	public Array<PlayerBlessing> GetAllBlessings()
	{
		return playerBlessings;
	}

}




