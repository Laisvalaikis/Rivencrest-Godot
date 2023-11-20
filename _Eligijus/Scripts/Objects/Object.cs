using Godot;
using Godot.Collections;

public partial class Object : Node2D
{
    [Export] private ObjectData objectData;
    public ObjectInformation<ObjectData> objectInformation;
    private TurnManager _turnManager;
    private Array<Ability> _abilities;
    private Array<PlayerBlessing> _playerBlessings;

    public override void _EnterTree()
    {
        base._EnterTree();
        objectInformation = new ObjectInformation<ObjectData>(objectData, typeof(Object));
    }

    public virtual void SetupObject()
    {
        objectInformation = new ObjectInformation<ObjectData>(objectData, typeof(Object));
        _abilities = new Array<Ability>();
        _playerBlessings = new Array<PlayerBlessing>();
        for (int i = 0; i < objectData.abilities.Count; i++)
        {
            Ability ability = new Ability(objectData.abilities[i]);
            _abilities.Add(ability);
        }
        for (int i = 0; i < objectData.blessings.Count; i++)
        {
            _playerBlessings.Add(new PlayerBlessing(objectData.blessings[i]));
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
        Hide();
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

    public bool CanStepOn()
    {
        return objectInformation.objectData.canStepOnObject;
    }
    
}