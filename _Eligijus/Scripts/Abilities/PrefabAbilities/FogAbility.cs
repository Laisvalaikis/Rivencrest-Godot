using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class FogAbility : BaseAction
{
    [Export] private int _lifeTime = 2;
    [Export] private StopAttack _stopAttackDebuf;
    private int _lifeTimeCount = 0;
    private List<DebuffManager> _debuffManagers;
    public FogAbility()
    {
        
    }
    
    public FogAbility(FogAbility ability): base(ability)
    {
        _lifeTime = ability._lifeTime;
        _stopAttackDebuf = ability._stopAttackDebuf;
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        FogAbility ability = new FogAbility((FogAbility)action);
        return ability;
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        // add debufs
        if (chunk.CharacterIsOnTile())
        {
            if (_debuffManagers is null)
            {
                _debuffManagers = new List<DebuffManager>();
            }
            _debuffManagers.Add(chunk.GetCurrentPlayer().debuffManager);
            chunk.GetCurrentPlayer().debuffManager.AddDebuff(_stopAttackDebuf.CreateNewInstance(), _player);
        }
    }

    public override void OnExitAbility(ChunkData chunkDataPrev, ChunkData chunk)
    {
        base.OnExitAbility(chunkDataPrev, chunk);
        chunk.GetCurrentPlayer().debuffManager.RemoveDebuffsByType(typeof(StopAttack));
    }

    public override void OnTurnStart(ChunkData chunk)
    {
        if (_lifeTimeCount < _lifeTime)
        {
            _lifeTimeCount++;
        }
        else
        {
            if (_debuffManagers is not null)
            {
                for (int i = 0; i < _debuffManagers.Count; i++)
                {
                    _debuffManagers[i].RemoveDebuffsByType(typeof(StopAttack));
                }
            }
            _object.Death();
        }
    }
    
    public override void Die()
    {
        if (_debuffManagers is not null)
        {
            foreach (DebuffManager debuff in _debuffManagers)
            {
                debuff.RemoveDebuffsByType(typeof(StopAttack));
            }
        }
    }
}