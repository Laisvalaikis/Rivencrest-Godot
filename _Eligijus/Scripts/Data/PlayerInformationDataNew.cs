using Godot;
using Godot.Collections;
using Array = System.Array;


public partial class PlayerInformationDataNew : ObjectData
{
	[Export]
	public CharacterType characterType;
	[Export]
	public string ClassName;
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
	public Texture CharacterSplashArt;//For character table
	[Export]
	public Texture CroppedSplashArt;
	[Export]
	public Texture characterSprite;
	[Export]
	public Texture roleSprite;
	[Export]
	public Array<Ability> baseAbilities;

	public PlayerInformationDataNew()
	{
		
	}
	
	public void CopyData(PlayerInformationDataNew playerInformationData)
	{
		characterType = playerInformationData.characterType;
		ClassName = playerInformationData.ClassName;
		critChance = playerInformationData.critChance;
		accuracy = playerInformationData.accuracy;
		dodgeChance = playerInformationData.dodgeChance;
		killXP = playerInformationData.killXP;
		role = playerInformationData.role;
		
		CharacterSplashArt = playerInformationData.CharacterSplashArt;
		CroppedSplashArt = playerInformationData.CroppedSplashArt;
		characterSprite = playerInformationData.characterSprite;
		roleSprite = playerInformationData.roleSprite;
		baseAbilities = playerInformationData.baseAbilities;
		base.CopyData(playerInformationData);
	}
	
	public Array<PlayerBlessing> GetAllPlayerBlessings()
	{
		return blessings;
	}

}




