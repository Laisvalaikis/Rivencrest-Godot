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
			OnTurnStart();
		}
	}
	
	public void OnTurnStart()
	{
		for (LinkedListNode<BaseDebuff> element = debufList.First; element != null; )
		{
			bool elementWasRemoved = false;
			BaseDebuff value = element.Value;
				
			if (value.GetLifetime() > value.GetLifetimeCounter()) 
			{
				value.OnTurnStart();
			}
			if(value.GetLifetime() <= value.GetLifetimeCounter())
			{
				LinkedListNode<BaseDebuff> tempElement = element;
				tempElement = element.Next;
				debufList.Remove(element);
				element = tempElement;
				elementWasRemoved = true;
			}
			if (!elementWasRemoved)
			{
				element = element.Next;
			}
		}
	}

	public void OnTurnEnd()
	{
		for (LinkedListNode<BaseDebuff> element = debufList.First; element != null; )
		{
			bool elementWasRemoved = false;
			BaseDebuff value = element.Value;
				
			if (value.GetLifetime() > value.GetLifetimeCounter()) 
			{
				value.OnTurnEnd();
			}
			if(value.GetLifetime() <= value.GetLifetimeCounter())
			{
				LinkedListNode<BaseDebuff> tempElement = element;
				tempElement = element.Next;
				debufList.Remove(element);
				element = tempElement;
				elementWasRemoved = true;
			}
			if (!elementWasRemoved)
			{
				element = element.Next;
			}
		}
	}
	

	public virtual void PlayerWasAttacked()
	{
		for (LinkedListNode<BaseDebuff> element = debufList.First; element != null; )
		{
			bool elementWasRemoved = false;
			BaseDebuff value = element.Value;
				
			if (value.GetLifetime() > value.GetLifetimeCounter()) 
			{
				value.PlayerWasAttacked();
			}
			if(value.GetLifetime() <= value.GetLifetimeCounter())
			{
				LinkedListNode<BaseDebuff> tempElement = element;
				tempElement = element.Next;
				debufList.Remove(element);
				element = tempElement;
				elementWasRemoved = true;
			}
			if (!elementWasRemoved)
			{
				element = element.Next;
			}
		}
	}
	
	public virtual void PlayerDied()
	{
		for (LinkedListNode<BaseDebuff> element = debufList.First; element != null; )
		{
			bool elementWasRemoved = false;
			BaseDebuff value = element.Value;
				
			if (value.GetLifetime() > value.GetLifetimeCounter()) 
			{
				value.PlayerDied();
			}
			if(value.GetLifetime() <= value.GetLifetimeCounter())
			{
				LinkedListNode<BaseDebuff> tempElement = element;
				tempElement = element.Next;
				debufList.Remove(element);
				element = tempElement;
				elementWasRemoved = true;
			}
			if (!elementWasRemoved)
			{
				element = element.Next;
			}
		}
	}

	public void AddDebuff(BaseDebuff debuff, Player playerWhoCreatedDebuff)
	{
		debuff.SetPLayer(_player);
		debuff.SetPlayerWhoCreatedDebuff(playerWhoCreatedDebuff);
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
				element.Value.OnRemove();
				debufList.Remove(element);
			}
		}
	}

	public bool CanUseAttack()
	{
		if (debufList.ContainsType(typeof(StopAttack)))
		{
			return false;
		}
		return true;
	}

	public LinkedList<BaseDebuff> GetPlayerDebuffs()
	{
		return debufList;
	}

	public void RemoveDebuffs()
	{
		debufList.Clear();
	}
}
