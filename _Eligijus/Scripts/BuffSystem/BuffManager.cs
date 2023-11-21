using Godot;

using Rivencrestgodot._Eligijus.Scripts.BuffSystem;

public class BuffManager : Node
{
    [Export] 
	private Player _player;
	private LinkedList<BaseBuff> debufList;

	public override void _Ready()
	{
		if (debufList == null)
		{
			debufList = new LinkedList<BaseBuff>();
		}
		else
		{
			for (LinkedListNode<BaseBuff> element = debufList.First; element != null; element = element.Next)
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
		for (LinkedListNode<BaseBuff> element = debufList.First; element != null; element = element.Next)
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
	
	public void OnMouseClick(ChunkData chunkData)
	{
		ExecuteBuffs(chunkData);
	}

	public void OnTurnEnd()
	{
		for (LinkedListNode<BaseBuff> element = debufList.First; element != null; element = element.Next)
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

	private void ExecuteBuffs(ChunkData chunkData)
	{
		for (LinkedListNode<BaseBuff> element = debufList.First; element != null; element = element.Next)
		{
			if (element.Value.GetLifetime() > element.Value.GetLifetimeCounter())
			{
				element.Value.ResolveBuff(chunkData);
			}
			if(element.Value.GetLifetime() <= element.Value.GetLifetimeCounter())
			{
				debufList.Remove(element);
			}
		}
	}

	public virtual void PlayerWasAttacked()
	{
		for (LinkedListNode<BaseBuff> element = debufList.First; element != null; element = element.Next)
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
		for (LinkedListNode<BaseBuff> element = debufList.First; element != null; element = element.Next)
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

	public void AddBuff(BaseBuff buff)
	{
		buff.SetPLayer(_player);
		debufList.AddLast(buff);
	}

	public bool ContainsBuff(BaseBuff buff)
	{
		return debufList.Contains(buff);
	}
}