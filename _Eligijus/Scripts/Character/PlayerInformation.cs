using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Godot;

public partial class PlayerInformation : ObjectInformation
{
	public int xPToGain = 0;

	public override void SetupData(ObjectData objectInformation)
	{
		objectData = new ObjectDataType<ObjectData>(objectInformation, typeof(Player));
	}

	public float GetHealthPercentage()
	{
		float maxHealthDouble = objectData.objectData.maxHealth;
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

	public override async void DealDamage(int damage, Player damageDealer)
	{
			if (damage != -1)
			{
				animationPlayer.Play(_object.CurrentHitAnimation);
				await Task.Delay(TimeSpan.FromSeconds(animationPlayer.GetAnimation(_object.CurrentHitAnimation).Length));
				_health -= damage;
				damageDealer.objectInformation.GetPlayerInformation().AddDamageXP(damage);
				animationPlayer.Play(_object.CurrentIdleAnimation);
			}

			if (_health <= 0) // DEATH
			{
				animationPlayer.Play("Death");
				await Task.Delay(TimeSpan.FromSeconds(animationPlayer.GetAnimation("Death").Length));
				_health = 0;
				DeathStart(damageDealer);
			}
	}
	
	public override void DeathStart(Player damageDealer)
	{
		damageDealer.objectInformation.GetPlayerInformation().AddKillXP();
		_object.Death();
	}
	
	public void AddKillXP()
	{
		xPToGain += objectData.GetPlayerInformationData().killXP;
	}
	
	public void AddDamageXP(int damageMade)
	{
		int maxHP = objectData.GetObjectData().maxHealth;
		float maxHPInProcentage = (float)damageMade / (float)maxHP * 100.0f;
		float killXPProcentage = (float)objectData.GetPlayerInformationData().killXP / 100.0f * maxHPInProcentage;
		xPToGain += Mathf.RoundToInt(killXPProcentage);
	}

	public int GainXP()
	{
		return xPToGain;
	}
}
