using Godot;
using Godot.Collections;

public partial class DeBuffManager : Node
{
	[Export] 
	private Player _player;
	private LinkedList<BaseDeBuff> debufList;

	public override void _Ready()
	{
		if (debufList == null)
		{
			debufList = new LinkedList<BaseDeBuff>();
		}
		else
		{
			for (LinkedListNode<BaseDeBuff> element = debufList.First; element != null; element = element.Next)
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
		for (LinkedListNode<BaseDeBuff> element = debufList.First; element != null; element = element.Next)
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
		ExecuteDeBuffs(chunkData);
	}

	public void OnTurnEnd()
	{
		for (LinkedListNode<BaseDeBuff> element = debufList.First; element != null; element = element.Next)
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

	private void ExecuteDeBuffs(ChunkData chunkData)
	{
		for (LinkedListNode<BaseDeBuff> element = debufList.First; element != null; element = element.Next)
		{
			if (element.Value.GetLifetime() > element.Value.GetLifetimeCounter())
			{
				element.Value.ResolveDeBuff(chunkData);
			}
			if(element.Value.GetLifetime() <= element.Value.GetLifetimeCounter())
			{
				debufList.Remove(element);
			}
		}
	}

	public virtual void PlayerWasAttacked()
	{
		for (LinkedListNode<BaseDeBuff> element = debufList.First; element != null; element = element.Next)
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
		for (LinkedListNode<BaseDeBuff> element = debufList.First; element != null; element = element.Next)
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

	public void AddDeBuff(BaseDeBuff deBuff)
	{
		deBuff.SetPLayer(_player);
		debufList.AddLast(deBuff);
	}

	public bool ContainsDeBuff(BaseDeBuff deBuff)
	{
		return debufList.Contains(deBuff);
	}
	
	
}
