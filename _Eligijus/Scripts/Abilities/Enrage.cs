using Godot;

public partial class Enrage : BaseAction
{
	public Enrage()
	{
 		
	}
	public Enrage(Enrage enrage): base(enrage)
	{
	}
 	
	public override BaseAction CreateNewInstance(BaseAction action)
	{
		Enrage enrage = new Enrage((Enrage)action);
		return enrage;
	}
	
	

	protected override void DisableDamagePreview(ChunkData chunk)
	{
		HighlightTile highlightTile = chunk.GetTileHighlight();
		highlightTile.DisableDamageText();
		GameTileMap.Tilemap.GetChunk(player.GlobalPosition).GetTileHighlight().DisableDamageText();
	}
	public override void SetHoveredAttackColor(ChunkData chunkData)
	{
		if (!chunkData.CharacterIsOnTile() || (chunkData.CharacterIsOnTile() && !CanTileBeClicked(chunkData)))
		{
			chunkData.GetTileHighlight()?.SetHighlightColor(abilityHighlightHover);
		}
		else
		{
			chunkData.GetTileHighlight()?.SetHighlightColor(abilityHoverCharacter);
			EnableDamagePreview(chunkData, "+1 MP");
			GameTileMap.Tilemap.GetChunk(player.GlobalPosition).GetTileHighlight().EnableTile(true);
			EnableDamagePreview(GameTileMap.Tilemap.GetChunk(player.GlobalPosition),"+1 MP");
		}
	}
	
	public override void OnMoveHover(ChunkData hoveredChunk, ChunkData previousChunk)
	{
		HighlightTile previousChunkHighlight = previousChunk?.GetTileHighlight();
		HighlightTile hoveredChunkHighlight = hoveredChunk?.GetTileHighlight();

		if (previousChunkHighlight != null && (hoveredChunk == null || !hoveredChunkHighlight.isHighlighted))
		{
			SetNonHoveredAttackColor(previousChunk);
			DisableDamagePreview(previousChunk);
		}
		if (hoveredChunkHighlight == null || hoveredChunk == previousChunk)
		{
			return;
		}
		if (hoveredChunkHighlight.isHighlighted)
		{
			SetHoveredAttackColor(hoveredChunk);
		}
		if (previousChunkHighlight != null && hoveredChunk.GetCurrentPlayer()==null/*CanTileBeClicked(hoveredChunk)*/)
		{
			SetNonHoveredAttackColor(previousChunk);
			DisableDamagePreview(previousChunk);
		}
	}
	public override void ResolveAbility(ChunkData chunk)
	{
		base.ResolveAbility(chunk);
		FinishAbility();
	}
}
