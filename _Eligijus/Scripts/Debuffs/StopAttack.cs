using Godot;

public partial class StopAttack : BaseDebuff
{
    public StopAttack()
    {
			
    }
    
    public StopAttack(StopAttack debuff): base(debuff)
    {
        
    }
    
    public override BaseDebuff CreateNewInstance(BaseDebuff baseDebuff)
    {
        StopAttack debuff = new StopAttack((StopAttack)baseDebuff);
        return debuff;
    }
    
    public override BaseDebuff CreateNewInstance()
    {
        StopAttack debuff = new StopAttack(this);
        return debuff;
    }
    
}