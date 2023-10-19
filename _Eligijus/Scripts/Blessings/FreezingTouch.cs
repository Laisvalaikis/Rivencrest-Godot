using Godot;

public partial class FreezingTouch : AbilityBlessing
{
    [Export] private int slowEnemyBy;
    
    public FreezingTouch()
    {
			
    }
    
    public FreezingTouch(FreezingTouch blessing): base(blessing)
    {
        slowEnemyBy = blessing.slowEnemyBy;
    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        FreezingTouch blessing = new FreezingTouch((FreezingTouch)baseBlessing);
        return blessing;
    }
    
    public override BaseBlessing CreateNewInstance()
    {
        FreezingTouch blessing = new FreezingTouch(this);
        return blessing;
    }
    
    public override void OnTurnStart(ref BaseAction baseAction)
    {
        base.OnTurnStart(ref baseAction);
        baseAction.GetPlayer().actionManager.AddAbilityPoints(-slowEnemyBy);
    }
    
}