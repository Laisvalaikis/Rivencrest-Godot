using Godot;

public partial class Object : Node2D
{
    [Export] private ObjectData objectData;
    public ObjectInformation<ObjectData> objectInformation;

    public override void _EnterTree()
    {
        base._EnterTree();
        objectInformation = new ObjectInformation<ObjectData>(objectData, typeof(Object));
    }

    public virtual void SetupObject()
    {
        
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
        return objectInformation.objectData.canStepOnObject;
    }
    
}