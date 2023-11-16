using Godot;

public partial class BlessingData : Resource
{
   [Export] public SavedCharacterResource playerResource;
   [Export] public int blessingIndex = -1;
   [Export] public bool playerBlessing = false;
   [Export] public bool baseAbilityBlessing = false;
   [Export] public bool abilityBlessing = false;
   [Export] public BaseBlessing blessing;

   public BlessingData(SavedCharacterResource playerResource, BaseBlessing blessing, int blessingIndex)
   {
      this.playerResource = playerResource;
      this.blessing = blessing;
      this.blessingIndex = blessingIndex;
   }

   public void SetupPlayerBlessing()
   {
      playerBlessing = true;
   }

   public void SetupAbilityBlessing()
   {
      baseAbilityBlessing = true;
   }
   
   public void SetupBaseAbilityBlessing()
   {
      playerBlessing = true;
   }

}