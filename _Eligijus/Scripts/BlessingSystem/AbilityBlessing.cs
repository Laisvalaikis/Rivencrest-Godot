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
    
    public override void Start(ref BaseBlessing baseBlessing)
    {
	    base.Start(ref baseBlessing);
    }
    
    public override void ResolveBlessing(ref BaseAction baseAction)
    {
        base.ResolveBlessing(ref baseAction);
    }
    
    public virtual void ResolveBlessing(ref BaseAction baseAction, ChunkData tile)
    {
	    base.ResolveBlessing(ref baseAction);
    }

    public virtual void ResolveBlessing(ref BaseAction baseAction, List<ChunkData> tiles)
    {
	    
    }

    public override void OnTurnStart(ref BaseAction baseAction)
    {
	    base.OnTurnStart(ref baseAction);
    }
    
    public virtual void OnTurnStart(ref BaseAction baseAction, ChunkData tile)
    {
	    base.OnTurnStart(ref baseAction);
    }


    public override void OnTurnEnd(ref BaseAction baseAction)
    {
	    base.OnTurnEnd(ref baseAction);
    }
    
    public virtual void OnTurnEnd(ref BaseAction baseAction, ChunkData chunkData)
    {
	    base.OnTurnEnd(ref baseAction);
        
    }

    public virtual void OnMoveHover(ref BaseAction baseAction, ChunkData hoveredChunk, ChunkData previousChunk)
    {
    }

    public virtual void PrepareForBlessing(ChunkData chunkData)
    {
	    
    }

    protected void DealRandomDamageToTarget(PlayerInformation currentPlayer, ChunkData chunkData, BaseAction baseAction, int minDamage, int maxDamage)
    {
	    if (chunkData != null && chunkData.CharacterIsOnTile() && IsAllegianceSame(currentPlayer, chunkData, baseAction))
	    {
				
		    int randomDamage = _random.Next(minDamage, maxDamage);
		    bool crit = IsItCriticalStrike(ref randomDamage, currentPlayer);
		    DodgeActivation(ref randomDamage, currentPlayer, chunkData.GetCurrentPlayer().playerInformation);
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

    public bool IsAllegianceSame(PlayerInformation currentPlayer, ChunkData chunk, BaseAction action)
    {
	    return chunk == null || chunk.GetCurrentPlayer().playerInformation.GetPlayerTeam() == currentPlayer.GetPlayerTeam() || !action.friendlyFire;
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