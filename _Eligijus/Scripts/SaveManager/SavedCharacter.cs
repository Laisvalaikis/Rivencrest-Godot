using System.Collections;
using System.Collections.Generic;
using System;
using Godot;


[Serializable]
	public class SavedCharacter : SavableCharacter
	{
		public Node prefab;
		public SavedCharacter() { }

		public SavedCharacter(Node prefab, int level, int xP, int xPToGain, bool dead, string characterName,
			int abilityPointCount, List<UnlockedAbilities> unlockedAbilities, int confirmedAbilities, List<Blessing> blessings, int prefabIndex, PlayerInformationData playerInformation)
			: base(level, xP, xPToGain, dead, characterName, abilityPointCount, unlockedAbilities, confirmedAbilities, blessings, prefabIndex, playerInformation)
		{
			this.prefab = prefab;
		}
		public SavedCharacter(SavedCharacter x) : base(x)
		{
			this.prefab = x.prefab;
		}
		public SavedCharacter(SavedCharacterResource x) : base(x)
		{
			this.prefab = x.prefab;
		}

		public SavedCharacter(SavableCharacter x, Node prefab) : base(x)
		{
			this.prefab = prefab;
		}
		public SavedCharacter(SavableCharacterResource x, Node prefab) : base(x)
		{
			this.prefab = prefab;
		}
		
		public SavedCharacter(SavableCharacterResource x, Node prefab, PlayerInformationData playerInformationData) : base(x, playerInformationData)
		{
			this.prefab = prefab;
		}
		
		public SavedCharacter(SavableCharacter x, Node prefab, PlayerInformationData playerInformation) : base(x)
		{
			this.prefab = prefab;
			this.playerInformation = playerInformation;
		}

		public string CharacterTableBlessingString()
		{
			string blessingsInOneString = "";
			for(int i = 0; i < blessings.Count; i++)
			{
				blessingsInOneString += blessings[i].blessingName;
				if (i != blessings.Count - 1) blessingsInOneString += "\n";
			}
			return blessingsInOneString;
		}
	}
