using Godot;

public partial class BlessingData : Resource
{
   [Export] public SavableCharacterResource playerResource;
   [Export] public UnlockedBlessingsResource unlockedBlessingsResource;
   [Export] public bool playerBlessing = false;
   [Export] public bool abilityBlessing = false;
   [Export] public bool globalBlessing = false;
   [Export] public BaseBlessing blessing;

   public BlessingData(SavableCharacterResource playerResource, BaseBlessing blessing, UnlockedBlessingsResource unlockedBlessingsResource)
   {
      this.playerResource = playerResource;
      this.blessing = blessing;
      this.unlockedBlessingsResource = unlockedBlessingsResource;
   }

   public void SetupPlayerBlessing()
   {
      playerBlessing = true;
   }

   public void SetupAbilityBlessing()
   {
      abilityBlessing = true;
   }

   public void SetupGlobalBlessing()
   {
      globalBlessing = true;
   }

}