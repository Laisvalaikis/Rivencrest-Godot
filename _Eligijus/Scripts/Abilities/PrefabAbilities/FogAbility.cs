using Godot;

public partial class FogAbility : BaseAction
{
    [Export] private int lifeTime = 2;
    private int lifeTimeCount = 0;
    
    public FogAbility()
    {
        
    }
    
    public FogAbility(FogAbility ability): base(ability)
    {
        lifeTime = ability.lifeTime;
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
    }

    public override void OnExitAbility(ChunkData chunkDataPrev, ChunkData chunk)
    {
        base.OnExitAbility(chunkDataPrev, chunk);
        // remove debufs
    }

    public override void OnTurnStart(ChunkData chunk)
    {
        if (lifeTimeCount < lifeTime)
        {
            lifeTimeCount++;
        }
        else
        {
            // remove debufs
            _object.Death();
        }
    }
    
    public override void Die()
    {
        // remove debufs
    }
}