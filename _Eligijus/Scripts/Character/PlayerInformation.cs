using System.Collections;
using System.Collections.Generic;
using System;
using Godot;

public partial class PlayerInformation : Node
{
	[Export]
	private Player _player;
	[Export]
	private PlayerInformationDataNew playerInformationData;
	public ObjectDataType<ObjectData> objectData;
	[Export]
	public Sprite2D spriteRenderer;
	public Node TeamManager;
	public bool Protected = false;
	public bool Stasis = false;
	public int XPToGain = 0;
	private bool haveBarier = false;
	private int _health = 100;

	public void SetupData()
	{
		objectData = new ObjectDataType<ObjectData>(playerInformationData, typeof(Player));
	}

	public override void _Ready()
	{
		base._Ready();
		LoadPlayerProgression();
		PlayerSetup();
		_health = playerInformationData.maxHealth;
	}

	public float GetHealthPercentage()
	{
		float maxHealthDouble = playerInformationData.maxHealth;
		float healthDouble = _health;
		return healthDouble / maxHealthDouble * 100;
	}

	public Type GetInformationType()
	{
		if (objectData != null)
		{
			return objectData.objectType;
		}
		return null;
	}
	
	public void SetInformationType(Type informationType)
	{
		objectData.objectType = informationType;
	}

	public void DealDamage(int damage, Player damageDealer)
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
				PLayerDamaged();
			}

			if (_health <= 0) // DEATH
			{
				_health = 0;
				DeathStart(damageDealer);
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
		return playerInformationData.maxHealth;
	}

	public void DeathStart(Player damageDealer)
	{
		damageDealer.playerInformation.AddKillXP();
		_player.Death();
	}
	
	public void PLayerDamaged()
	{
		_player.PlayerWasDamaged();
	}
	
	public void AddKillXP()
	{
		XPToGain += playerInformationData.killXP;
	}

	public int GainXP()
	{
		return XPToGain;
	}

	public void Heal(int healAmount)
	{
		if (_health + healAmount >= playerInformationData.maxHealth)
		{
			_health = playerInformationData.maxHealth;
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
		
	}

	public void AddBarrier()
	{
		haveBarier = true;
	}
	
	public void RemoveBarrier()
	{
		haveBarier = false;
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
