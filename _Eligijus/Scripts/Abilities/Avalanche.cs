using Godot;

public partial class Avalanche : BaseAction
{
    private PlayerInformation _playerInformation; //playeris ant kurio calliname
    public Avalanche()
    {
    		
    }
    
    public Avalanche(Avalanche avalanche): base(avalanche)
    {
        
    }
    	
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        Avalanche avalanche = new Avalanche((Avalanche)action);
        return avalanche;
    }

    public override void ResolveAbility(ChunkData chunk)
    {
        if (CanTileBeClicked(chunk))
        {
            base.ResolveAbility(chunk);
            foreach (ChunkData chunkData in _chunkList)
            {
                if (chunkData.CharacterIsOnTile() && CanTileBeClicked(chunkData))
                {
                    DealRandomDamageToTarget(chunkData, minAttackDamage, maxAttackDamage);
                }
            }
            FinishAbility();
        }
    }
    
    

    public override bool CanTileBeClicked(ChunkData chunk)
    {
        if (CheckIfSpecificInformationType(chunk, InformationType.Player) &&
            !IsAllegianceSame(chunk) &&
            IsCharacterAffectedByCrowdControl(chunk))
        {
            return true;
        }
        return false;
    }
    private bool IsCharacterAffectedByCrowdControl(ChunkData chunk)
    {
        if (CheckIfSpecificInformationType(chunk, InformationType.Player))
        {
            Player target = chunk.GetCurrentPlayer();
            if (target.debuffs.IsPlayerStunned()
                || target.debuffs.IsPalyerRooted()
                || target.debuffs.IsPlayerSlower()
                || target.debuffs.IsPlayerSilenced())
            {
                return true;
            }
        }
        return false;
    }
}