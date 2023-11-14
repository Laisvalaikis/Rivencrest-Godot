using Godot;
using Godot.Collections;

public partial class DeBuffManager : Node
{
	[Export] 
	private Player _player;
	private Array<BaseDeBuff> debufList;

	public override void _Ready()
	{
		if (debufList == null)
		{
			debufList = new Array<BaseDeBuff>();
		}
		else
		{
			for (int i = 0; i < debufList.Count; i++)
			{
				debufList[i].OnTurnStart();
			}
		}
	}
	
	public void OnTurnStart()
	{
		for (int i = 0; i < debufList.Count; i++)
		{
			debufList[i].OnTurnStart();
		}
	}
	
	public void OnMouseClick(ChunkData chunkData)
	{
		ExecuteDeBuffs(chunkData);
	}

	public void OnTurnEnd()
	{
		for (int i = 0; i < debufList.Count; i++)
		{
			debufList[i].OnTurnEnd();
		}
	}

	private void ExecuteDeBuffs(ChunkData chunkData)
	{
		for (int i = 0; i < debufList.Count; i++)
		{
			debufList[i].ResolveDeBuff(chunkData);
		}
	}

	public virtual void PlayerWasAttacked()
	{
		for (int i = 0; i < debufList.Count; i++)
		{
			debufList[i].PlayerWasAttacked();
		}
	}
	
	public virtual void PlayerDied()
	{
		for (int i = 0; i < debufList.Count; i++)
		{
			debufList[i].PlayerDied();
		}	
	}

	public void AddDeBuff(BaseDeBuff deBuff)
	{
		debufList.Add(deBuff);
	}

	public bool ContainsDeBuff(BaseDeBuff deBuff)
	{
		return debufList.Contains(deBuff);
	}
	
	public bool ContainsDeBuff(string deBuffName)
	{
		for (int i = 0; i < debufList.Count; i++)
		{
			if (debufList[i].ResourceName == deBuffName)
				return true;
		}
		return false;
	}
	
}
