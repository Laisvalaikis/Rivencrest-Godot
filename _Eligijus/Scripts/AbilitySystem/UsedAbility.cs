using System;
using Godot;

public partial class UsedAbility : Resource
{
	public BaseAction Ability { get; set; }
	public ChunkData Chunk { get; set; }

	public UsedAbility(BaseAction ability, ChunkData chunk)
	{
		Ability = ability;
		Chunk = chunk;
	}
}
