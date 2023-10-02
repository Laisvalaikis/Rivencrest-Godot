using System.Collections;
using System.Collections.Generic;
using System;
using Godot;

public partial class PlayerInformation : Node
{
	
	[Export]
	private InformationType type = InformationType.Player;
	[Export]
	public PlayerInformationData playerInformationData;
	[Export]
	public SavedCharacterResource savedCharacter;
	[Export]
	public Sprite2D spriteRenderer;
	[Export]
	public string CharactersTeam = "Default";
	
	public Node TeamManager;
	public Node FlagInHand = null;
	public bool Protected = false;
	public bool Stasis = false;
	public int XPToGain = 0;
	public bool isThisObject = false;
	
	private Label textMeshPro;
	private PlayerInformationData _playerInformationData;
	private AnimationPlayer animator;
	private int _health = 100;
	private int _currentCharacterTeam = -1;
	private int turnCounter = 1;
	
	void Awake()
	{
		_playerInformationData = new PlayerInformationData();
		_playerInformationData.CopyData(playerInformationData);
	}
	void Start()
	{
		LoadPlayerProgression();
		PlayerSetup();
		_health = _playerInformationData.MaxHealth;
	}

	public float GetHealthPercentage()
	{
		float maxHealthDouble = _playerInformationData.MaxHealth;
		float healthDouble = _health;
		return healthDouble / maxHealthDouble * 100;
	}

	public InformationType GetInformationType()
	{
		return type;
	}
	
	public void SetInformationType(InformationType informationType)
	{
		type = informationType;
	}

	public void SetPlayerTeam(int currentCharacterTeam)
	{
		_currentCharacterTeam = currentCharacterTeam;
	}

	public int GetPlayerTeam()
	{
		return _currentCharacterTeam;
	}

	public void DealDamage(int damage, bool crit, Node damageDealer, string specialInformation = "")
	{
		if (damage != -1) 
		{ 
			if (Protected || Stasis)
			{ 
				damage /= 2; 
			} 
			_health -= damage;
		}
		if (_health <= 0) // DEATH
		{
			DeathStart();
		}
		else
		{
			
		}
		   
	}

	public int GetHealth()
	{
		return _health;
	}

	public void DeathStart()
	{
	   
	}

	public void Die()
	{
		spriteRenderer.Hide();
		if (isThisObject)
		{
			QueueFree();
		}
	}

   public void AddKillXP()
	{

	}
	public void Heal(int healAmount, bool crit)
	{
		if (_health + healAmount >= _playerInformationData.MaxHealth)
		{
			_health = _playerInformationData.MaxHealth;
		}
		else
		{
			_health += healAmount;
		}
	   
	}
	public void ApplyDebuff(string debuff, Node DebuffApplier = null)
	{
	   
	}
	public void ToggleSelectionBorder(bool state)
	{
	   
	}
	public void PlayerSetup()
	{
		if (CharactersTeam == "Default")
		{
			GD.PrintErr("Fix Comment");
		}
	}
	public void LoadPlayerProgression()
	{
	   
	} 
   
	public virtual void OnTurnStart()
	{
		
	}
	public void OnTurnEnd()
	{
	   
	}
	
	public int TotalPoisonDamage()
	{
		int totalDamage = 0;
		// foreach (Poison x in Poisons)
		// {
		//     totalDamage += x.poisonValue;
		// }
		return totalDamage;
	}

}
