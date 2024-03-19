using Godot;

using Rivencrestgodot._Eligijus.Scripts.BuffSystem;

public partial class BuffManager : Node
{
	[Export] 
	private Player _player;
	private LinkedList<BaseBuff> buffList;
	
	[Export] public Resource animatedObjectPrefab;
	[Export] public ObjectData animatedObjectPrefabData;

	public override void _Ready()
	{
		if (buffList == null)
		{
			buffList = new LinkedList<BaseBuff>();
		}
		else
		{
			OnTurnStart();
		}
	}
	
	public void OnTurnStart()
	{
		for (LinkedListNode<BaseBuff> element = buffList.First; element != null; )
		{
			bool elementWasRemoved = false;
			BaseBuff value = element.Value;
				
			if (value.GetLifetime() > value.GetLifetimeCounter()) 
			{
				value.OnTurnStart();
			}
			if(value.GetLifetime() <= value.GetLifetimeCounter())
			{
				LinkedListNode<BaseBuff> tempElement = element;
				tempElement = element.Next;
				element.Value.OnRemove();
				buffList.Remove(element);
				element = tempElement;
				elementWasRemoved = true;
			}
			if (!elementWasRemoved)
			{
				element = element.Next;
			}
		}
	}
	
	public void OnMouseClick(ChunkData chunkData)
	{
		ExecuteBuffs(chunkData);
	}

	public void OnTurnEnd()
	{
		for (LinkedListNode<BaseBuff> element = buffList.First; element != null; )
		{
			bool elementWasRemoved = false;
			BaseBuff value = element.Value;
				
			if (value.GetLifetime() > value.GetLifetimeCounter()) 
			{
				value.OnTurnEnd();
			}
			if(value.GetLifetime() <= value.GetLifetimeCounter())
			{
				LinkedListNode<BaseBuff> tempElement = element;
				tempElement = element.Next;
				element.Value.OnRemove();
				buffList.Remove(element);
				element = tempElement;
				elementWasRemoved = true;
			}
			if (!elementWasRemoved)
			{
				element = element.Next;
			}
		}
	}

	private void ExecuteBuffs(ChunkData chunkData)
	{
		for (LinkedListNode<BaseBuff> element = buffList.First; element != null; )
		{
			bool elementWasRemoved = false;
			BaseBuff value = element.Value;
				
			if (value.GetLifetime() > value.GetLifetimeCounter()) 
			{
				value.ResolveBuff(chunkData);
			}
			if(value.GetLifetime() <= value.GetLifetimeCounter())
			{
				LinkedListNode<BaseBuff> tempElement = element;
				tempElement = element.Next;
				element.Value.OnRemove();
				buffList.Remove(element);
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
		for (LinkedListNode<BaseBuff> element = buffList.First; element != null; )
		{
			bool elementWasRemoved = false;
			BaseBuff value = element.Value;
				
			if (value.GetLifetime() > value.GetLifetimeCounter()) 
			{
				value.PlayerWasAttacked();
			}
			if(value.GetLifetime() <= value.GetLifetimeCounter())
			{
				LinkedListNode<BaseBuff> tempElement = element;
				tempElement = element.Next;
				buffList.Remove(element);
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
		for (LinkedListNode<BaseBuff> element = buffList.First; element != null; )
		{
			bool elementWasRemoved = false;
			BaseBuff value = element.Value;
				
			if (value.GetLifetime() > value.GetLifetimeCounter()) 
			{
				value.PlayerDied();
			}
			if(value.GetLifetime() <= value.GetLifetimeCounter())
			{
				LinkedListNode<BaseBuff> tempElement = element;
				tempElement = element.Next;
				buffList.Remove(element);
				element = tempElement;
				elementWasRemoved = true;
			}
			if (!elementWasRemoved)
			{
				element = element.Next;
			}
		}
	}

	public void AddBuff(BaseBuff buff)
	{
		buff.SetPLayer(_player);
		buffList.AddLast(buff);
		buff.Start();
	}

	public bool ContainsBuff(BaseBuff buff)
	{
		return buffList.Contains(buff);
	}

	public LinkedList<BaseBuff> GetPlayerBuffs()
	{
		if (buffList == null)
		{
			buffList = new LinkedList<BaseBuff>();
		}
		return buffList;
	}
}
