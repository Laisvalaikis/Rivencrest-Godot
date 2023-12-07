using Godot;

using Rivencrestgodot._Eligijus.Scripts.BuffSystem;

public partial class BuffManager : Node
{
	[Export] 
	private Player _player;
	private LinkedList<BaseBuff> buffList;

	public override void _Ready()
	{
		if (buffList == null)
		{
			buffList = new LinkedList<BaseBuff>();
		}
		else
		{
			for (LinkedListNode<BaseBuff> element = buffList.First; element != null; element = element.Next)
			{
				if (element.Value.GetLifetime() > element.Value.GetLifetimeCounter())
				{
					element.Value.OnTurnStart();
				}
				if(element.Value.GetLifetime() <= element.Value.GetLifetimeCounter())
				{
					buffList.Remove(element);
				}
			}
		}
	}
	
	public void OnTurnStart()
	{
		for (LinkedListNode<BaseBuff> element = buffList.First; element != null; element = element.Next)
		{
			if (element.Value.GetLifetime() > element.Value.GetLifetimeCounter())
			{
				element.Value.OnTurnStart();
			}
			if(element.Value.GetLifetime() <= element.Value.GetLifetimeCounter())
			{
				buffList.Remove(element);
			}
		}
	}
	
	public void OnMouseClick(ChunkData chunkData)
	{
		ExecuteBuffs(chunkData);
	}

	public void OnTurnEnd()
	{
		for (LinkedListNode<BaseBuff> element = buffList.First; element != null; element = element.Next)
		{
			if (element.Value.GetLifetime() > element.Value.GetLifetimeCounter())
			{
				element.Value.OnTurnEnd();
			}
			if(element.Value.GetLifetime() <= element.Value.GetLifetimeCounter())
			{
				buffList.Remove(element);
			}
		}
	}

	private void ExecuteBuffs(ChunkData chunkData)
	{
		for (LinkedListNode<BaseBuff> element = buffList.First; element != null; element = element.Next)
		{
			if (element.Value.GetLifetime() > element.Value.GetLifetimeCounter())
			{
				element.Value.ResolveBuff(chunkData);
			}
			if(element.Value.GetLifetime() <= element.Value.GetLifetimeCounter())
			{
				buffList.Remove(element);
			}
		}
	}

	public virtual void PlayerWasAttacked()
	{
		for (LinkedListNode<BaseBuff> element = buffList.First; element != null; element = element.Next)
		{
			if (element.Value.GetLifetime() > element.Value.GetLifetimeCounter())
			{
				element.Value.PlayerWasAttacked();
			}
			if(element.Value.GetLifetime() <= element.Value.GetLifetimeCounter())
			{
				buffList.Remove(element);
			}
		}
	}
	
	public virtual void PlayerDied()
	{
		for (LinkedListNode<BaseBuff> element = buffList.First; element != null; element = element.Next)
		{
			if (element.Value.GetLifetime() > element.Value.GetLifetimeCounter())
			{
				element.Value.PlayerDied();
			}
			if(element.Value.GetLifetime() <= element.Value.GetLifetimeCounter())
			{
				buffList.Remove(element);
			}
		}
	}

	public void AddBuff(BaseBuff buff)
	{
		buff.SetPLayer(_player);
		buffList.AddLast(buff);
	}

	public bool ContainsBuff(BaseBuff buff)
	{
		return buffList.Contains(buff);
	}

	public LinkedList<BaseBuff> GetPlayerBuffs()
	{
		return buffList;
	}
}
