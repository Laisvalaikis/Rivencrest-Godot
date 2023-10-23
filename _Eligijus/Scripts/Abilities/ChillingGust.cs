using System.Collections.Generic;
using Godot;

public partial class ChillingGust : BaseAction
{
    private List<ChunkData> _additionalDamageTiles = new List<ChunkData>();
    private Player _protectedAlly;

    public ChillingGust()
    {
        
    }
    public ChillingGust(ChillingGust ability): base(ability)
    {
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        ChillingGust ability = new ChillingGust((ChillingGust)action);
        return ability;
    }

    public override void OnTurnStart()
    {
        if (_protectedAlly != null)
        {
            _protectedAlly.playerInformation.Protected = false;
            _protectedAlly = null;
        }
    }
    public override void ResolveAbility(ChunkData chunk)
    {
            base.ResolveAbility(chunk);
            Player target = chunk.GetCurrentPlayer();
            PlayerInformation clickedPlayerInformation = target.playerInformation;
            
            if (IsAllegianceSame(chunk))
            {
                clickedPlayerInformation.Protected = true;
                _protectedAlly = target;
            }
            else
            {
                int bonusDamage = 0;
                //Calculate bonus damage based on blessings
                DealRandomDamageToTarget(chunk, minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
            }
            FinishAbility();
    }
    
    protected override void TryAddTile(ChunkData chunk)
    {
        if (chunk != null && !chunk.TileIsLocked())
        {
            _chunkList.Add(chunk);
        }
    }
    
    public override void SetHoveredAttackColor(ChunkData chunkData)
    {
        Node2D character = chunkData.GetCurrentPlayer();
        HighlightTile tileHighlight = chunkData.GetTileHighlight();

        if (character != null && IsAllegianceSame(chunkData))
        {
            tileHighlight.SetHighlightColor(abilityHoverCharacter);
            EnableDamagePreview(chunkData,"PROTECT");
        }
        else
        {
            tileHighlight.SetHighlightColor(abilityHighlightHover);
        }
    }
    
    private void CreateDamageTileList(ChunkData chunk)
    {
        _additionalDamageTiles.Clear();
        
        
        ChunkData[,] chunks = GameTileMap.Tilemap.GetChunksArray();
        (int x, int y) indexes = chunk.GetIndexes();
        int x = indexes.x;
        int y = indexes.y;

        int[] dx = { 0, 0, 1, -1 };
        int[] dy = { 1, -1, 0, 0 };

        for (int i = 0; i < 4; i++)
        {
            int nx = x + dx[i];
            int ny = y + dy[i];

            if (GameTileMap.Tilemap.CheckBounds(ny, nx) && CheckIfSpecificInformationType(chunks[nx,ny], InformationType.Player))
            {
                _additionalDamageTiles.Add(chunk);
            }
        }
    }
}
