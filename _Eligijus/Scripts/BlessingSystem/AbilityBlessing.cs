using System;
using System.Collections.Generic;
using Godot;

public partial class AbilityBlessing : BaseBlessing
{

	private Random _random;
	
    public AbilityBlessing()
    {
			
    }
    
    public AbilityBlessing(AbilityBlessing blessing): base(blessing)
    {
	    _random = new Random();
    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
	    AbilityBlessing blessing = new AbilityBlessing((AbilityBlessing)baseBlessing);
	    return blessing;
    }
    
    public override BaseBlessing CreateNewInstance()
    {
	    AbilityBlessing blessing = new AbilityBlessing(this);
	    return blessing;
    }
    
    public override void Start(BaseBlessing baseBlessing)
    {
	    base.Start(baseBlessing);
    }
    
    public override void ResolveBlessing(BaseAction baseAction)
    {
        base.ResolveBlessing(baseAction);
    }
    
    public virtual void ResolveBlessing(BaseAction baseAction, ChunkData tile)
    {
	    base.ResolveBlessing(baseAction);
    }

    public virtual void ResolveBlessing(BaseAction baseAction, List<ChunkData> tiles)
    {
	    
    }

    public override void OnTurnStart(BaseAction baseAction)
    {
	    base.OnTurnStart(baseAction);
    }
    
    public virtual void OnTurnStart(BaseAction baseAction, ChunkData tile)
    {
	    base.OnTurnStart(baseAction);
    }


    public override void OnTurnEnd(BaseAction baseAction)
    {
	    base.OnTurnEnd(baseAction);
    }
    
    public virtual void OnTurnEnd(BaseAction baseAction, ChunkData chunkData)
    {
	    base.OnTurnEnd(baseAction);
        
    }

    public virtual void OnMoveHover(BaseAction baseAction, ChunkData hoveredChunk, ChunkData previousChunk)
    {
    }

    public virtual void PrepareForBlessing(ChunkData chunkData)
    {
	    
    }

    protected void DealRandomDamageToTarget(Player currentPlayer, ChunkData chunkData, BaseAction baseAction, int minDamage, int maxDamage)
    {
	    if (chunkData != null && chunkData.CharacterIsOnTile() && IsAllegianceSame(currentPlayer, chunkData, baseAction))
	    {
				
		    int randomDamage = _random.Next(minDamage, maxDamage);
		    bool crit = IsItCriticalStrike(ref randomDamage, currentPlayer.playerInformation);
		    DodgeActivation(ref randomDamage, currentPlayer.playerInformation, chunkData.GetCurrentPlayer().playerInformation);
		    chunkData.GetCurrentPlayer().playerInformation.DealDamage(randomDamage, crit, currentPlayer);
	    }
    }
    
    private void DodgeActivation(ref int damage, PlayerInformation player, PlayerInformation target) //Dodge temporarily removed
    {
	    int dodgeNumber = _random.Next(0, 100);
	    if (dodgeNumber > player.playerInformationData.accuracy - target.playerInformationData.dodgeChance)
	    {
		    damage = -1;
	    }
    }

    public bool IsAllegianceSame(Player currentPlayer, ChunkData chunk, BaseAction action)
    {
	    return chunk == null || chunk.GetCurrentPlayer().GetPlayerTeam() == currentPlayer.GetPlayerTeam() || !action.friendlyFire;
    }
    
    protected bool IsItCriticalStrike(ref int damage, PlayerInformation playerInformation)
    {
	    int critNumber = _random.Next(0, 100);
	    bool crit;
	    if (critNumber > playerInformation.playerInformationData.critChance)
	    {
		    crit = false;
	    }
	    else
	    {
		    damage += 3;
		    crit = true;
	    }
	    return crit;
    }
    
    protected Side ChunkSideByCharacter(ChunkData playerChunk, ChunkData chunkDataTarget)
    {
	    (int x, int y) playerChunkIndex = playerChunk.GetIndexes();
	    (int x, int y) chunkIndex = chunkDataTarget.GetIndexes();
	    if (playerChunkIndex.x > chunkIndex.x)
	    {
		    return Side.isFront;
	    }
	    else if (playerChunkIndex.x < chunkIndex.x)
	    {
		    return Side.isBack;
	    }
	    else if (playerChunkIndex.y < chunkIndex.y)
	    {
		    return Side.isRight;
	    }
	    else if (playerChunkIndex.y > chunkIndex.y)
	    {
		    return Side.isLeft;
	    }
	    return Side.none;
    }
    
    protected (int, int) GetSideVector(Side side)
    {
	    (int, int) sideVector = (0, 0);
	    switch (side)
	    {
		    case Side.isFront:
			    sideVector = (-1, 0);
			    break;
		    case Side.isBack:
			    sideVector = (1, 0);
			    break;
		    case Side.isRight:
			    sideVector = (0, 1);
			    break;
		    case Side.isLeft:
			    sideVector = (0, -1);
			    break;
		    case Side.none:
			    sideVector = (0, 0);
			    break;
	    }
	    return sideVector;
    }
}