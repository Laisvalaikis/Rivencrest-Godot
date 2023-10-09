using Godot;

public partial class SwitchPlaces : BaseAction
{
    private ChunkData _firstSeleted;
    private ChunkData _secondSelected;
    
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
        Node2D character = characterOne.GetCurrentCharacter();
        PlayerInformation playerInformationLocal = characterOne.GetCurrentPlayerInformation();
        GameTileMap.Tilemap.MoveSelectedCharacterWithoutReset(characterTwo.GetPosition(), new Vector2(0, 50f), characterOne.GetCurrentCharacter());
        GameTileMap.Tilemap.MoveSelectedCharacterWithoutReset(characterOne.GetPosition(), new Vector2(0, 50f),
            characterTwo.GetCurrentCharacter());
        GameTileMap.Tilemap.SetCharacter(characterOne, characterTwo.GetCurrentCharacter(), characterTwo.GetCurrentPlayerInformation());
        GameTileMap.Tilemap.SetCharacter(characterTwo, character, playerInformationLocal);
    }
}
