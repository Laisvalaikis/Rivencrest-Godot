using Godot;

public partial class SwitchPlaces : BaseAction
{
    private ChunkData _firstSeleted;
    private ChunkData _secondSelected;

    public SwitchPlaces()
    {
    }

    public SwitchPlaces(SwitchPlaces ability): base(ability)
    {
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        SwitchPlaces ability = new SwitchPlaces((SwitchPlaces)action);
        return ability;
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        if (chunk.CharacterIsOnTile())
        {
            if (_firstSeleted == null || !_firstSeleted.CharacterIsOnTile())
            {
                _firstSeleted = chunk;
            }
            else if(_secondSelected == null || !_secondSelected.CharacterIsOnTile())
            {
                _secondSelected = chunk;
                SwitchCharacters(_firstSeleted, _secondSelected);
                FinishAbility();
                base.ResolveAbility(chunk);
                _firstSeleted = null;
                _secondSelected = null;
            }
          
        }
    }
    private void SwitchCharacters(ChunkData characterOne, ChunkData characterTwo)
    {
        Player character = characterOne.GetCurrentPlayer();
        PlayerInformation playerInformationLocal = characterOne.GetCurrentPlayer().playerInformation;
        GameTileMap.Tilemap.MoveSelectedCharacterWithoutReset(characterTwo.GetPosition(), new Vector2(0, 50f), characterOne.GetCurrentPlayer());
        GameTileMap.Tilemap.MoveSelectedCharacterWithoutReset(characterOne.GetPosition(), new Vector2(0, 50f),
            characterTwo.GetCurrentPlayer());
        GameTileMap.Tilemap.SetCharacter(characterOne, characterTwo.GetCurrentPlayer());
        GameTileMap.Tilemap.SetCharacter(characterTwo, character);
    }
}
