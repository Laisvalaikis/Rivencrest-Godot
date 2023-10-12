﻿using Godot;

public partial class PlayerBlessing : BaseBlessing
{
    public PlayerBlessing()
    {
			
    }
    
    public PlayerBlessing(PlayerBlessing blessing): base(blessing)
    {
	    
    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        PlayerBlessing blessing = new PlayerBlessing((PlayerBlessing)baseBlessing);
        return blessing;
    }

    public override void Start(ref BaseBlessing baseBlessing)
    {
        base.Start(ref baseBlessing);
    }
}