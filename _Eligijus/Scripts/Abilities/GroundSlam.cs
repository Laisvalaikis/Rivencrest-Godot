using System.Collections.Generic;
using Godot;

public partial class GroundSlam : BaseAction
{
    private bool isAbilityActive;
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
    
    public override void OnTurnStart(ChunkData chunkData)
    {
        base.OnTurnStart(chunkData);
        if(isAbilityActive && _player.objectInformation.GetPlayerInformation().GetHealth() > 0)
        {
            DealDamageToAdjacent();
            isAbilityActive = false;
        }
    }

    public override void ResolveAbility(ChunkData chunk)
    {
        if (CanTileBeClicked(chunk))
        {
            UpdateAbilityButton();
            base.ResolveAbility(chunk);
            DealDamageToAdjacent();
            isAbilityActive = true;
            FinishAbility();
        }
    }
    private void DealDamageToAdjacent()
    {
        foreach (var chunk in _chunkListCopy)
        {
            DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        }
    }
}
