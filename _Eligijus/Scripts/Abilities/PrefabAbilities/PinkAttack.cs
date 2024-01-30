using Godot;

public partial class PinkAttack : BaseAction
{
    public PinkAttack()
    {
        
    }

    public PinkAttack(PinkAttack ability): base(ability)
    {
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        PinkAttack ability = new PinkAttack((PinkAttack)action);
        return ability;
    }
    
    public override void OnTurnStart(ChunkData chunk)
    {
        if (CanBeUsedOnTile(chunk))
        {
            base.ResolveAbility(chunk);
            DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        }
        _object.Death();
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