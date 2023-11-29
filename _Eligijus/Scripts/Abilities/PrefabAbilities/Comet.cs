using Godot;

public partial class Comet : BaseAction
{
    [Export] private int teamMateDamageDivision = 3;
    public Comet()
    {
        
    }
    
    public Comet(Comet ability): base(ability)
    {
        teamMateDamageDivision = ability.teamMateDamageDivision;
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        Comet ability = new Comet((Comet)action);
        return ability;
    }
    
    // Step On
    public override void OnTurnStart(ChunkData chunk)
    {
        // wait until other turn starts
        // need to add buff destroy half damage
        // damage enemy or damage player
        if (chunk.CharacterIsOnTile() && IsAllegianceSame(chunk))
        {
            DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        }
        else if(chunk.CharacterIsOnTile() && !IsAllegianceSame(chunk))
        {
            DealRandomDamageToTarget(chunk, minAttackDamage / teamMateDamageDivision, maxAttackDamage / teamMateDamageDivision);
        }
        _object.Death();
    }


}