using Godot;

public partial class DamagePlayer : BaseAction
{

    public DamagePlayer()
    {
        
    }

    public DamagePlayer(DamagePlayer ability): base(ability)
    {
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        DamagePlayer ability = new DamagePlayer((DamagePlayer)action);
        return ability;
    }

    public override void ResolveAbility(ChunkData chunk)
    {
        DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        base.ResolveAbility(chunk);
        FinishAbility();
    }
    
    public override void OnTurnStart(ChunkData chunkData)
    {
        base.OnTurnStart(chunkData);
    }

    public override bool CanBeUsedOnTile(ChunkData chunkData)
    {
        if (CheckIfSpecificInformationType(chunkData, typeof(Player)) 
            || CheckIfSpecificInformationType(chunkData, typeof(Object)))
        {
            return true;
        }
        return false;
    }
    
    public override bool IsAllegianceSame(ChunkData chunk)
    {
        return false;
    }
    
}