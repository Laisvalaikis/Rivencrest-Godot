using Godot;

public partial class ObjectInformation : Node
{
    [Export]
    public Sprite2D spriteRenderer;
    [Export]
    protected Object _object;
    public ObjectDataType<ObjectData> objectData;
    protected int _health = 100;

    public virtual void SetupData(ObjectData objectInformation)
    {
        objectData = new ObjectDataType<ObjectData>(objectInformation, typeof(Object));
        _health = objectData.objectData.maxHealth;
    }

    public virtual void DealDamage(int damage, Player damageDealer)
    {
            if (damage != -1)
            {
                _health -= damage;
                PLayerDamaged();
            }
            if (_health <= 0)
            {
                DeathStart(damageDealer);
                _health = 0;
            }
        
    }
    
    public virtual void DealDamageUnnotified(int damage, Player damageDealer)
    {
        if (damage != -1)
        {
            _health -= damage;
        }
        if (_health <= 0)
        {
            DeathStart(damageDealer);
            _health = 0;
        }
        
    }
    
    public int GetHealth()
    {
        return _health;
    }
    
    public int GetMaxHealth()
    {
        return objectData.GetObjectData().maxHealth;
    }
    
    public virtual void PLayerDamaged()
    {
        _object.PlayerWasDamaged();
    }
    
    public virtual void DeathStart(Player damageDealer)
    {
        damageDealer.objectInformation.GetPlayerInformation().AddKillXP();
        _object.Death();
    }
    
    public virtual void Heal(int healAmount)
    {
        if (_health + healAmount >= objectData.GetObjectData().maxHealth)
        {
            _health = objectData.GetObjectData().maxHealth;
        }
        else
        {
            _health += healAmount;
        }
    }
    
    public void AddHealth(int health)
    {
        _health += health;
    }
    
    public virtual void OnTurnStart()
    {
		
    }
    public virtual void OnTurnEnd()
    {
	   
    }
    
}