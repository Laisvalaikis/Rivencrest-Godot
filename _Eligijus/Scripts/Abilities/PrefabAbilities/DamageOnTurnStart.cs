using Godot;

public partial class DamageOnTurnStart : BaseAction
{
    public DamageOnTurnStart()
    {
        
    }

    public DamageOnTurnStart(DamageOnTurnStart ability): base(ability)
    {
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        DamageOnTurnStart ability = new DamageOnTurnStart((DamageOnTurnStart)action);
        return ability;
    }
    
    public override void OnTurnStart(ChunkData chunk)
    {
        if (chunk.CharacterIsOnTile())   //CanTileBeClicked(chunk)
        {
            DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        }
        base.OnTurnStart(chunk);
        
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