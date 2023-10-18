using Godot;
using System;

public partial class Concussion : AbilityBlessing
{
	public Concussion()
	{
		
	}
	public Concussion(Concussion blessing): base(blessing)
	{
		
	}
	public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
	{
		Concussion blessing = new Concussion((Concussion)baseBlessing);
		return blessing;
	}
	public override BaseBlessing CreateNewInstance()
	{
		Concussion blessing = new Concussion(this);
		return blessing;
	}
}
