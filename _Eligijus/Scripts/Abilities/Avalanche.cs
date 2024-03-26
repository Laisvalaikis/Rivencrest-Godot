using Godot;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class Avalanche : BaseAction
{
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
        UpdateAbilityButton();
        foreach (ChunkData chunkData in _chunkList)
        { 
            if (CanBeUsedOnTile(chunkData)) 
            { 
                DealRandomDamageToTarget(chunkData, minAttackDamage, maxAttackDamage);
                PlayAnimation("Avalanche", chunkData);
            }
        } 
        base.ResolveAbility(chunk); 
        PlayerAbilityAnimation();
        FinishAbility();
    }
    
    public override bool CanBeUsedOnTile(ChunkData chunkData)
    {
        return base.CanBeUsedOnTile(chunkData) && IsCharacterAffectedByCrowdControl(chunkData);
    }
    
    private bool IsCharacterAffectedByCrowdControl(ChunkData chunk)
    {
        if (CheckIfSpecificInformationType(chunk, typeof(Player)))
        {
            DebuffManager targetDebuffManager = chunk.GetCurrentPlayer().debuffManager;
            if(targetDebuffManager.ContainsDebuff(typeof(PlayerStun))||targetDebuffManager.ContainsDebuff(typeof(RootDebuff))||
               targetDebuffManager.ContainsDebuff(typeof(SlowDebuff))||targetDebuffManager.ContainsDebuff(typeof(SilenceDebuff)))
                return true;
        }
        return false;
    }
}