using System;
using Godot;
using Godot.Collections;

public partial class DebuffManager : Node
{
	[Export] 
	private Player _player;
	private LinkedList<BaseDebuff> debufList;

	public override void _Ready()
	{
		if (debufList == null)
		{
			debufList = new LinkedList<BaseDebuff>();
		}
		else
		{
			for (LinkedListNode<BaseDebuff> element = debufList.First; element != null; element = element.Next)
			{
				if (element.Value.GetLifetime() > element.Value.GetLifetimeCounter())
				{
					element.Value.OnTurnStart();
				}
				if(element.Value.GetLifetime() <= element.Value.GetLifetimeCounter())
				{
					debufList.Remove(element);
				}
			}
		}
	}
	
	public void OnTurnStart()
	{
		for (LinkedListNode<BaseDebuff> element = debufList.First; element != null; element = element.Next)
		{
			if (element.Value.GetLifetime() > element.Value.GetLifetimeCounter())
			{
				element.Value.OnTurnStart();
			}
			if(element.Value.GetLifetime() <= element.Value.GetLifetimeCounter())
			{
				debufList.Remove(element);
			}
		}
	}

	public void OnTurnEnd()
	{
		for (LinkedListNode<BaseDebuff> element = debufList.First; element != null; element = element.Next)
		{
			if (element.Value.GetLifetime() > element.Value.GetLifetimeCounter())
			{
				element.Value.OnTurnEnd();
			}
			if(element.Value.GetLifetime() <= element.Value.GetLifetimeCounter())
			{
				debufList.Remove(element);
			}
		}
	}
	

	public virtual void PlayerWasAttacked()
	{
		for (LinkedListNode<BaseDebuff> element = debufList.First; element != null; element = element.Next)
		{
			if (element.Value.GetLifetime() > element.Value.GetLifetimeCounter())
			{
				element.Value.PlayerWasAttacked();
			}
			if(element.Value.GetLifetime() <= element.Value.GetLifetimeCounter())
			{
				debufList.Remove(element);
			}
		}
	}
	
	public virtual void PlayerDied()
	{
		for (LinkedListNode<BaseDebuff> element = debufList.First; element != null; element = element.Next)
		{
			if (element.Value.GetLifetime() > element.Value.GetLifetimeCounter())
			{
				element.Value.PlayerDied();
			}
			if(element.Value.GetLifetime() <= element.Value.GetLifetimeCounter())
			{
				debufList.Remove(element);
			}
		}
	}

	public void AddDebuff(BaseDebuff debuff)
	{
		debuff.SetPLayer(_player);
		debufList.AddLast(debuff);
	}
	
	public bool ContainsDebuff(Type debuff)
	{
		return debufList.ContainsType(debuff);
	}

	public void RemoveDebuffsByType(Type debuff)
	{
		for (LinkedListNode<BaseDebuff> element = debufList.First; element != null; element = element.Next)
		{
			if (element.Value.EqualsType(debuff))
			{
				debufList.Remove(element);
			}
		}
	}
}
