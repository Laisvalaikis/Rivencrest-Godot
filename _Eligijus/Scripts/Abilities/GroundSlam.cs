using System.Collections.Generic;
using Godot;

public partial class GroundSlam : BaseAction
{
    private List<ChunkData> _chunkListCopy;

    public GroundSlam()
    {
        
    }
    public GroundSlam(GroundSlam ability): base(ability)
    {
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        GroundSlam ability = new GroundSlam((GroundSlam)action);
        return ability;
    }

    public override void CreateGrid()
    {
        base.CreateGrid();
        _chunkListCopy = new List<ChunkData>(_chunkList);
    }
    
    public override void OnBeforeStart(ChunkData chunkData)
    {
        base.OnBeforeStart(chunkData);
        if(_player.objectInformation.GetPlayerInformation().GetHealth() > 0)
        {
            DealDamageToAdjacent();
        }
    }

    public override void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
        base.ResolveAbility(chunk);
        PlayerAbilityAnimation();
        DealDamageToAdjacent();
        FinishAbility();
    }
    private void DealDamageToAdjacent()
    {
        foreach (var chunk in _chunkListCopy)
        {
            DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
            PlayAnimation("Burgundy3", chunk);
        }
    }
}
