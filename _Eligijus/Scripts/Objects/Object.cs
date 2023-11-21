using Godot;
using Godot.Collections;

public partial class Object : Node2D
{
    public ObjectDataType<ObjectData> ObjectDataType;
    private TurnManager _turnManager;
    private Array<Ability> _abilities;
    private Array<PlayerBlessing> _playerBlessings;

    public override void _EnterTree()
    {
        base._EnterTree();
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
    }

    public void AddTurnManager(TurnManager turnManager)
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

    public void Execute(ChunkData chunkData)
    {
        ExecuteResolve(chunkData);
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

}