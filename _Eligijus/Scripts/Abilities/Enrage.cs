using Godot;

public partial class Enrage : BaseAction
{
	public Enrage()
	{
 		
	}
	public Enrage(Enrage enrage): base(enrage)
	{
		teamDisplayText = "+1 MP";
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
		GameTileMap.Tilemap.GetChunk(_player.GlobalPosition).GetTileHighlight().DisableDamageText();
	}
	
	public override void EnableDamagePreview(ChunkData chunk, string text = null)
	{
		HighlightTile highlightTile = chunk.GetTileHighlight();
		HighlightTile gamerHighlight = GameTileMap.Tilemap.GetChunk(_player.GlobalPosition).GetTileHighlight();
		if (teamDisplayText != string.Empty)
		{
			highlightTile.SetDamageText(teamDisplayText);
			gamerHighlight.EnableTile(true);
			gamerHighlight.SetDamageText(teamDisplayText);
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
			if (CanTileBeClicked(hoveredChunk))
			{
				EnableDamagePreview(hoveredChunk);
			}
		}
		if (previousChunkHighlight != null && hoveredChunk.GetCurrentPlayer()==null)
		{
			SetNonHoveredAttackColor(previousChunk);
			DisableDamagePreview(previousChunk);
		}
	}
	public override void ResolveAbility(ChunkData chunk)
	{
		if (CanTileBeClicked(chunk))
		{
			base.ResolveAbility(chunk);
			UpdateAbilityButton();
			Player targetPlayer = chunk.GetCurrentPlayer();
			targetPlayer.AddMovementPoints(1);
			_player.AddMovementPoints(1);
			FinishAbility();
		}
	}
}
