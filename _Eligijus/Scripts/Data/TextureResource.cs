using Godot;
using System;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(TextureResource), "", nameof(Resource))]
public partial class TextureResource : Resource
{
	[Export]
	public Texture texture;
}
