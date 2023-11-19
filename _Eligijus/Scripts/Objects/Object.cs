using Godot;

public partial class Object : Node2D
{
    [Export] private ObjectData objectData;
    public ObjectInformation<ObjectData> objectInformation;
    [Export] private bool canStepOn = false;

    public override void _EnterTree()
    {
        base._EnterTree();
        objectInformation = new ObjectInformation<ObjectData>();
        objectInformation.objectData = objectData;
    }

    public virtual void PlayerWasDamaged()
    {
       
    }
    
    public virtual void Death()
    {
        Hide();
    }

    public virtual void OnTurnStart()
    {
        
    }

    public void StepOn()
    {
        
    }

    public void Execute()
    {
        
    }

    public virtual void OnTurnEnd()
    {
        
    }

    public bool CanStepOn()
    {
        return canStepOn;
    }
    
}