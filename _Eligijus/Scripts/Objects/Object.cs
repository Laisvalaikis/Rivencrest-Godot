﻿using Godot;
using Godot.Collections;

public partial class Object : Node2D
{
    [Export] private ObjectData objectData;
    public ObjectInformation<ObjectData> objectInformation;
    private Array<Ability> _abilities;
    

    public override void _EnterTree()
    {
        base._EnterTree();
        objectInformation = new ObjectInformation<ObjectData>(objectData, typeof(Object));
    }

    public virtual void SetupObject()
    {
        objectInformation = new ObjectInformation<ObjectData>(objectData, typeof(Object));
        _abilities = new Array<Ability>();
        for (int i = 0; i < objectData.abilities.Count; i++)
        {
            Ability ability = new Ability(objectData.abilities[i]);
            _abilities.Add(ability);
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
    }
    
    private void ExecuteResolve()
    {
        for (int i = 0; i < _abilities.Count; i++)
        {
            if (_abilities[i].enabled)
            {
                _abilities[i].Action.ResolveAbility(null);
                _abilities[i].Action.ResolveBlessings(null);
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

    public void StepOn()
    {
        Execute();
    }

    public void Execute()
    {
        ExecuteResolve();
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