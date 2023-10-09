using System.Collections.Generic;
using Godot;

public partial class ChillingGust : BaseAction
{
    private List<ChunkData> _additionalDamageTiles = new List<ChunkData>();
    private Player _protectedAlly;
    
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
            Player target = (Player)chunk.GetCurrentCharacter();
            PlayerInformation clickedPlayerInformation = target.playerInformation;
            
            if (IsAllegianceSame(chunk))
            {
                clickedPlayerInformation.Protected = true;
                _protectedAlly = target;
            }
            else
            {
                int bonusDamage = 0;
                DealRandomDamageToTarget(chunk, minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
                clickedPlayerInformation.ApplyDebuff("IceSlow");
                if (DoesCharacterHaveBlessing("Tempest"))
                {
                    CreateDamageTileList(chunk);
                    foreach (ChunkData c in _additionalDamageTiles)
                    {
                        if (CanTileBeClicked(c))
                        {
                            DealRandomDamageToTarget(c, minAttackDamage, maxAttackDamage);
                        }
                    }
                }
            }
            FinishAbility();
    }
    
    private void CreateDamageTileList(ChunkData chunk)
    {
        _additionalDamageTiles.Clear();
        var spellDirectionVectors = new List<(int, int)>
        {
            (1, 0),
            (0, 1),
            (-1, 0),
            (0, -1)
        };
        foreach (var x in spellDirectionVectors)
        {
            if (CheckIfSpecificInformationType(chunk, InformationType.Player))
            {
                _additionalDamageTiles.Add(chunk);
            }
        }
    }
    public void OnTileHover(Vector3 position)
    {

    }
    
}
