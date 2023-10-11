using Godot;
using System;

public partial class BaseBlessing : Resource
{
    public string blessingName;
    public int rarity;
    public string className;
    public string spellName;
    public string condition;
    public string description;
    [Export] protected bool _blessingIsUnlocked;
    
    public BaseBlessing()
    {
			
    }

    public BaseBlessing(BaseBlessing blessing)
    {
        this.blessingName = blessing.blessingName;
        this.rarity = blessing.rarity;
        this.className = blessing.className;
        this.spellName = blessing.spellName;
        this.condition = blessing.condition;
        this.description = blessing.description;
    }

    public virtual BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        throw new NotImplementedException();
    }

    public virtual BaseBlessing CreateNewInstance()
    {
        throw new NotImplementedException();
    }

    public virtual void Start(ref BaseBlessing baseBlessing)
    {

    }
    
    public virtual void ResolveBlessing(ref BaseAction baseAction)
    {
        
    }
    
    public virtual void OnTurnStart(ref BaseAction baseAction)
    {
			
    }

    public virtual void OnTurnEnd(ref BaseAction baseAction)
    {
        
    }

    public bool IsBlessingUnlocked()
    {
        return _blessingIsUnlocked;
    }


}
