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
	private bool haveBarier = false;
	
	private Label textMeshPro;
	private AnimationPlayer animator;
	private int _health = 100;
	private int _currentCharacterTeam = -1;
	private int turnCounter = 1;

	public override void _Ready()
	{
		base._Ready();
		LoadPlayerProgression();
		PlayerSetup();
		_health = playerInformationData.MaxHealth;
	}

	public float GetHealthPercentage()
	{
		float maxHealthDouble = playerInformationData.MaxHealth;
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
		if (!haveBarier)
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
		else
		{
			haveBarier = false;
		}

	}

	public int GetHealth()
	{
		return _health;
	}

	public int GetMaxHealth()
	{
		return playerInformationData.MaxHealth;
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
	public void Heal(int healAmount)
	{
		if (_health + healAmount >= playerInformationData.MaxHealth)
		{
			_health = playerInformationData.MaxHealth;
		}
		else
		{
			_health += healAmount;
		}
	   
	}

	public void AddHealth(int health)
	{
		_health += health;
	}
	
	public void PlayerSetup()
	{
		if (CharactersTeam == "Default")
		{
			GD.PrintErr("Fix Comment");
		}
	}

	public void AddBarrier()
	{
		haveBarier = true;
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
	

}
