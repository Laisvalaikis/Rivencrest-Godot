using Godot;
using Godot.Collections;

public partial class Object : Node2D
{
	[Export] 
	protected ObjectInformation objectInformationNode;
	public ObjectType<ObjectInformation> objectInformation;
	public ObjectDataType<ObjectData> ObjectDataType;
	protected TurnManager _turnManager;
	private Array<Ability> _abilities;
	protected Array<PlayerBlessing> _playerBlessings;
	protected int _visionRange;
	public string CurrentIdleAnimation = "Idle";
	public string CurrentHitAnimation = "Hit";
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

	public void IncreaseVisionRange(int increaseBy)
	{
		_visionRange += increaseBy;
	}

	public virtual void SetupObject(ObjectData objectData)
	{
		objectInformation = new ObjectType<ObjectInformation>(objectInformationNode, typeof(ObjectInformation));
		ObjectDataType = new ObjectDataType<ObjectData>(objectData, typeof(Object));
		objectInformation.GetObjectInformation().SetupData(objectData);
		_abilities = new Array<Ability>();
		_playerBlessings = new Array<PlayerBlessing>();
		for (int i = 0; i < objectData.abilities.Count; i++)
		{
			Ability ability = new Ability(objectData.abilities[i]);
			_abilities.Add(ability);
			ability.Action.SetupObjectAbility(this);
		}
		for (int i = 0; i < objectData.blessings.Count; i++)
		{
			_playerBlessings.Add(new PlayerBlessing(objectData.blessings[i]));
		}
		_visionRange = ObjectDataType.GetObjectData().visionRange;
	}

	public virtual void DealDamage(int damage, Player damageDealer)
	{
		objectInformation.GetObjectInformation().DealDamage(damage, damageDealer);
	}

	public virtual void DealDamageUnnotified(int damage, Player damageDealer)
	{
		objectInformation.GetObjectInformation().DealDamageUnnotified(damage, damageDealer);
	}
	
	public virtual void AddDebuff(BaseDebuff debuff, Player playerWhoCreatedDebuff)
	{
		//Ziurek, ka darai, kvaily. Negalima debuffinti objekto, kuris ne playeris!!!!!!!!!
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

	public virtual void OnStart()
	{
		if (_playerBlessings is not null && _playerBlessings.Count > 0)
		{
			for (int i = 0; i < _playerBlessings.Count; i++)
			{
				_playerBlessings[i].Start((Player)this);
				GD.Print(_playerBlessings[i].GetType());
			}
		}
	}
	private void ExecuteOnTurnStart()
	{
		ChunkData chunkData = GameTileMap.Tilemap.GetChunk(GlobalPosition);
		for (int i = 0; i < _abilities.Count; i++)
		{
			
			if (_abilities[i].enabled)
			{
				_abilities[i].Action.OnTurnStart(chunkData);
				_abilities[i].Action.BlessingOnTurnStart(chunkData);
			}
		}
		for (int i = 0; i < _playerBlessings.Count; i++)
		{
			_playerBlessings[i].OnTurnStart(null);
		}
	}
	
	private void ExecuteOnTurnEnd()
	{
		ChunkData chunkData = GameTileMap.Tilemap.GetChunk(GlobalPosition);
		for (int i = 0; i < _abilities.Count; i++)
		{
			if (_abilities[i].enabled)
			{
				_abilities[i].Action.OnTurnEnd(chunkData);
				_abilities[i].Action.BlessingOnTurnEnd(chunkData);
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
	
	private void ExecuteDeath()
	{
		for (int i = 0; i < _abilities.Count; i++)
		{
			if (_abilities[i].enabled)
			{
				_abilities[i].Action.Die();
			}
		}
	}

	public virtual void PlayerWasDamaged()
	{
	   
	}
	
	public virtual void Death()
	{
		ExecuteDeath();
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
