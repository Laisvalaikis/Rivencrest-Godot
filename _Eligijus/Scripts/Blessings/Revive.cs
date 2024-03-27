using Godot;

public partial class Revive : GlobalBlessing
{
    public Revive()
    {
			
    }
    
    public Revive(Revive blessing): base(blessing)
    {
        
    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        Revive blessing = new Revive((Revive)baseBlessing);
        return blessing;
    }
    
    public override void Start(SavableCharacterResource playerInformation)
    {
        if (Data.Instance != null)
        {
            Data _data = Data.Instance;
            SavableCharacterResource deadCharacter = null;
            for (int i = 0; i < _data.townData.deadCharacters.Count; i++)
            {
                if (_data.townData.deadCharacters[i].characterIndex == playerInformation.characterIndex)
                {
                    deadCharacter = _data.townData.deadCharacters[i];
                    break;
                }
            }
            if (deadCharacter != null)
            {
                playerInformation.xPToGain = 0;
                _data.Characters.Add(new SavedCharacterResource(_data.AllAvailableCharacters[playerInformation.characterIndex].prefab, playerInformation));
                _data.townData.characters.Add(playerInformation);
                _data.townData.deadCharacters.Remove(playerInformation);
            }
        }
    }
}