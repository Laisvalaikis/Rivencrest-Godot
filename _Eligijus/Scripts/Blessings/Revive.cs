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
            if (_data.townData.deadCharacters.Contains(playerInformation))
            {
                _data.townData.characters.Add(playerInformation);
                _data.townData.deadCharacters.Remove(playerInformation);
            }
        }
    }
}