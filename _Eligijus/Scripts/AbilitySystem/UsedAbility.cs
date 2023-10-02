using System;

public class UsedAbility
{
	public BaseAction Ability { get; set; }
	public ChunkData Chunk { get; set; }

	public UsedAbility(BaseAction ability, ChunkData chunk)
	{
		Ability = ability;
		Chunk = chunk;
	}
}
