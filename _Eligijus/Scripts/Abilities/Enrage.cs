using Godot;

public partial class Enrage : BaseAction
{
	public Enrage()
	{
 		
	}
	public Enrage(Enrage enrage): base(enrage)
	{
		customText = "+1 MP";
	}
 	
	public override BaseAction CreateNewInstance(BaseAction action)
	{
		Enrage enrage = new Enrage((Enrage)action);
		return enrage;
	}

	public override void EnableDamagePreview(ChunkData chunk)
	{
		base.EnableDamagePreview(chunk);
		base.EnableDamagePreview(GameTileMap.Tilemap.GetChunk(_player.GlobalPosition));
	}

	protected override void DisableDamagePreview(ChunkData chunk)
	{
		HighlightTile highlightTile = chunk.GetTileHighlight();
		highlightTile.DisableDamageText();
		GameTileMap.Tilemap.GetChunk(_player.GlobalPosition).GetTileHighlight().DisableDamageText();
	}
	
	public override void ResolveAbility(ChunkData chunk)
	{ 
		base.ResolveAbility(chunk); 
		PlayAnimation("Red1", chunk);
		UpdateAbilityButton(); 
		Player targetPlayer = chunk.GetCurrentPlayer(); 
		targetPlayer.AddMovementPoints(1); 
		_player.AddMovementPoints(1); 
		FinishAbility();
	}
}
