using Godot;
using Godot.Collections;

public partial class Object : Node2D
{
	public ObjectDataType<ObjectData> ObjectDataType;
	protected TurnManager _turnManager;
	private Array<Ability> _abilities;
	private Array<PlayerBlessing> _playerBlessings;
	protected int _visionRange;
	public void DisableObject()
	{
		Hide();
	}

	public void EnableObject()
	{
		Show();
	}

	public bool IsObjectEnabled()
	{
		return Visible;
	}
	

	public virtual void SetupObject(ObjectData objectInformation)
	{
		ObjectDataType = new ObjectDataType<ObjectData>(objectInformation, typeof(Object));
		_abilities = new Array<Ability>();
		_playerBlessings = new Array<PlayerBlessing>();
		for (int i = 0; i < objectInformation.abilities.Count; i++)
		{
			Ability ability = new Ability(objectInformation.abilities[i]);
			_abilities.Add(ability);
			ability.Action.SetupObjectAbility(this);
		}
		for (int i = 0; i < objectInformation.blessings.Count; i++)
		{
			_playerBlessings.Add(new PlayerBlessing(objectInformation.blessings[i]));
		}
		_visionRange = ObjectDataType.GetObjectData().visionRange;
	}

	public void AddPlayerForObjectAbilities(Player player)
	{
		for (int i = 0; i < _abilities.Count; i++)
		{
			_abilities[i].Action.SetPlayer(player);
		}
	}
	
	public void StartActions()
	{
		for (int i = 0; i < _abilities.Count; i++)
		{
			_abilities[i].Action.StartAction();
		}
	}

	public int GetVisionRange()
	{
		return _visionRange;
	}

	public virtual void AddTurnManager(TurnManager turnManager)
	{
		_turnManager = turnManager;
		for (int i = 0; i < _abilities.Count; i++)
		{
			if (_abilities[i].enabled)
			{
				_abilities[i].Action.AddTurnManager(turnManager);
			}
		}
	}

	private void ExecuteOnTurnStart()
	{
		for (int i = 0; i < _abilities.Count; i++)
		{
			if (_abilities[i].enabled)
			{
				_abilities[i].Action.OnTurnStart(null);
				_abilities[i].Action.BlessingOnTurnStart(null);
			}
		}
		for (int i = 0; i < _playerBlessings.Count; i++)
		{
			_playerBlessings[i].OnTurnStart(null);
		}
	}
	
	private void ExecuteOnTurnEnd()
	{
		for (int i = 0; i < _abilities.Count; i++)
		{
			if (_abilities[i].enabled)
			{
				_abilities[i].Action.OnTurnEnd(null);
				_abilities[i].Action.BlessingOnTurnEnd(null);
			}
		}
		for (int i = 0; i < _playerBlessings.Count; i++)
		{
			_playerBlessings[i].OnTurnEnd(null);
		}
	}
	
	private void ExecuteResolve(ChunkData chunkData)
	{
		for (int i = 0; i < _abilities.Count; i++)
		{
			if (_abilities[i].enabled)
			{
				_abilities[i].Action.ResolveAbility(chunkData);
				_abilities[i].Action.ResolveBlessings(chunkData);
			}
		}
	}

	public virtual void PlayerWasDamaged()
	{
	   
	}
	
	public virtual void Death()
	{
		ChunkData chunkData = GameTileMap.Tilemap.GetChunk(GlobalPosition);
		chunkData.SetCurrentObject(null);
		_turnManager.RemoveObject(this);
		QueueFree();
	}

	public virtual void OnTurnStart()
	{
		ExecuteOnTurnStart();
	}

	public void StepOn(ChunkData chunkData)
	{
		Execute(chunkData);
	}

	public virtual void OnExit(ChunkData chunkDataPrev, ChunkData chunkData)
	{
		ExecuteOnExit(chunkDataPrev, chunkData);
	}

	public void Execute(ChunkData chunkData)
	{
		ExecuteResolve(chunkData);
	}
	
	public void ExecuteOnExit(ChunkData chunkDataPrev, ChunkData chunkData)
	{
		for (int i = 0; i < _abilities.Count; i++)
		{
			if (_abilities[i].enabled)
			{
				_abilities[i].Action.OnExitAbility(chunkDataPrev,chunkData);
			}
		}
	}

	public virtual void OnTurnEnd()
	{
		ExecuteOnTurnEnd();
	}

	public Array<Ability> GetAllAbilities()
	{
		return _abilities;
	}

	public bool CanStepOn()
	{
		return ObjectDataType.objectData.canStepOnObject;
	}

	public bool CanBeDestroyOnStepping()
	{
		return ObjectDataType.objectData.destroyObjectStepingOn;
	}

	public bool CanBeDestroyed()
	{
		return ObjectDataType.objectData.canBeDestroyed;
	}

}
