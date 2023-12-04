using Godot;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class Entangle : BaseAction
{
    public Entangle()
    {
 		
    }
    public Entangle(Entangle entangle): base(entangle)
    {
    }
 	
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        Entangle entangle = new Entangle((Entangle)action);
        return entangle;
    }
    public override void ResolveAbility(ChunkData chunk)
    {
        if (CanTileBeClicked(chunk))
        {
            UpdateAbilityButton();
            base.ResolveAbility(chunk);
            DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
            RootDebuff rootDebuff = new RootDebuff(1);
            chunk.GetCurrentPlayer().debuffManager.AddDebuff(rootDebuff, player);
            FinishAbility();
        }
    }
}