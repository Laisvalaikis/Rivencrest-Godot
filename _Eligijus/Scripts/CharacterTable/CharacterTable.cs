﻿using System;
using System.Collections;
using Godot;
using Godot.Collections;

public partial class CharacterTable : Control
{
    [Export] 
    private TextureRect tableBoarder;
    [Export]
    private TextureRect characterArt;
    [Export]
    private Label className;
    [Export]
    private Label role;
    [Export]
    private Label level;
    [Export]
    private Label maxHP;
    [Export]
    private Label xpProgress;
    [Export]
    private Label abilityPointCount;
    [Export]
    private Label blessingList;
    [Export]
    private PortraitBar portraitBar;
    [Export]
    private Control confirmationTable;
    [Export]
    private Label confirmationTableText;
    [Export]
    private TextureRect confirmationTableSprite;
    [Export]
    private Control confirm;
    [Export]
    private Button leftArrow;
    [Export]
    private Button rightArrow;
    public string originalName;
    [Export]
    private Button sellButton;
    [Export]
    private Array<Button> abilityButtons;
    [Export]
    private Array<TextureRect> abilityButtonImages;
    [Export]
    private Array<TextureRect> abilityButtonIconImages;
    [Export]
    private TextureRect roleIcon;
    [Export]
    public View view;
    [Export]
    public TextEdit nameInput;
    [Export] 
    private GameUi gameUI;
    [Export]
    public HelpTable helpTable;
    [Export]
    public Control recruitmentCenterTable;
    [Export]
    public TownHall townHall;
    private Array<bool> abilityButtonState;
    private int characterIndex;
    private Data _data;
    private bool pauseEnabled = false;
    
    private void Awake()
    {
    }

    public override void _Draw()
    {
        base._Draw();
        // ResetTempUnlockedAbilities();
        if (_data == null && Data.Instance != null)
        {
            _data = Data.Instance;
            if (abilityButtonState == null || abilityButtonState.Count == 0)
            {
                abilityButtonState = new Array<bool>();
                for (int i = 0; i < abilityButtons.Count; i++)
                {
                    abilityButtonState.Add(false);
                }
            }
        }
    }

    public int GetCurrentCharacterIndex()
    {
        return characterIndex;
    }

    private void OnDisable()
    {
    }

    public void DisableAllButtons()
    {
        pauseEnabled = true;
        if (abilityButtonState == null)
        {
            abilityButtonState = new Array<bool>();
        }
        else if (abilityButtonState.Count > 0)
        {
            for (int i = 0; i < abilityButtons.Count; i++)
            {
                abilityButtonState[i] = abilityButtons[i].Disabled;
                abilityButtons[i].Disabled = true;
            }
        }
    }

    public void EnableAllButtons()
    {
        pauseEnabled = false;
        if (abilityButtonState.Count > 0)
        {
            for (int i = 0; i < abilityButtons.Count; i++)
            {
                abilityButtons[i].Disabled = abilityButtonState[i];
            }
        }
    }

    private void UpdateCharacterPointCorner()
    {
        Array<SavedCharacterResource> charactersToUpdate = _data.Characters;
        if (charactersToUpdate[characterIndex].abilityPointCount <= 0)
        {
            portraitBar.DisableAbilityCorner(characterIndex);
        }
        else
        {
            portraitBar.EnableAbilityCorner(characterIndex);
        }

        if (gameUI != null)
        {
            gameUI.UpdateUnspentPointWarnings();
        }
    }

    public void ConfirmSelectedAbilities()
    {
        _data.Characters[characterIndex].toConfirmAbilities = 0;
        Array<UnlockedAbilitiesResource> unlockedAbilityList = _data.Characters[characterIndex].unlockedAbilities;
        for (int i = 0; i < unlockedAbilityList.Count; i++)
        {
            if (!unlockedAbilityList[i].abilityConfirmed && unlockedAbilityList[i].abilityUnlocked)
            {
                _data.Characters[characterIndex].unlockedAbilities[i].abilityConfirmed = true;
            }
        }
        confirm.Hide();
        UpdateCharacterPointCorner();
    }

    public void ChangeCharacterName()
    {
        nameInput.Text = nameInput.Text.ToUpper();
        _data.Characters[characterIndex].characterName = nameInput.Text;
    }

    public void PreventEmptyName()
    {
        if (nameInput.Text.Contains(' '))
        {
            _data.Characters[characterIndex].characterName = originalName;
            nameInput.Text = originalName;
        }
        if (nameInput.Text == "")
        {
            _data.Characters[characterIndex].characterName = originalName;
            nameInput.Text = originalName;
        }
    }
    
    public void UpdateConfirmationTable()
    {
        var character = _data.Characters[characterIndex];
        Color color = character.playerInformation.classColor; // fix this shit
        confirmationTableText.Text = $"Are you sure you want to sell " + character.characterName + " " + (character.cost / 2) + " gold?";
        confirmationTableSprite.Texture.Set("atlas", character.playerInformation.CharacterPortraitSprite.Get("atlas"));
    }
    
    public void UpdateAllAbilities()
    {
        var character = _data.Characters[characterIndex];
        for (int i = 0; i < abilityButtons.Count; i++)
        {
            if (character.abilityPointCount > 0 || !_data.Characters[characterIndex].unlockedAbilities[i].abilityConfirmed && _data.Characters[characterIndex].unlockedAbilities[i].abilityUnlocked)
            {
                abilityButtons[i].Disabled = false;
            }
            else
            {
                abilityButtons[i].Disabled = true;
            }

            if (!character.unlockedAbilities[i].abilityUnlocked)
            {
                Color color = abilityButtonImages[i].Modulate - new Color(0.2f, 0.2f, 0.2f, 0f);
                abilityButtonImages[i].Modulate = color;
            }
            else
            {
                abilityButtonImages[i].Modulate = character.playerInformation.backgroundColor;
            }
        }
    }


    public void DisplayCharacterTable(int index)
    {
        // gameObject.SetActive(true);
        view.OpenView();
        int tempIndex = characterIndex;
        characterIndex = index;
        if (index != tempIndex)
        {
            helpTable.Hide();
            UndoAbilitySelection(tempIndex);
            UpdateTable();
            UpdateAllAbilities();
        }

        if (recruitmentCenterTable != null && townHall != null)
        {
            recruitmentCenterTable.Hide();
            townHall.CloseTownHall();
        }
        else
        {
            GD.Print("TownHall is null");
        }
        
        
    }

    public void EnableDisableHelpTable(int index)
    {
        if (!pauseEnabled)
        {
            Vector2 currentPosition = helpTable.GlobalPosition;
            Vector2 position = new Vector2(currentPosition.X, abilityButtons[index].GlobalPosition.Y);
            helpTable.SetGlobalPosition(position);
            helpTable.EnableTableForBoughtCharacters(index, characterIndex);
        }
    }
    
    public void UndoAbilitySelection(int selectedCharacterIndex)
    {
        Array<UnlockedAbilitiesResource> unlockedAbilityList = _data.Characters[selectedCharacterIndex].unlockedAbilities;
        for (int i = 0; i < unlockedAbilityList.Count; i++)
        {
            if (!unlockedAbilityList[i].abilityConfirmed && unlockedAbilityList[i].abilityUnlocked)
            {
                RemoveAbility(i, selectedCharacterIndex);
            }
        }
        UpdateTable();
        UpdateAllAbilities();
    }
    
    public void ExitTable()
    {
        UndoAbilitySelection(characterIndex);
        view.ExitView();
    }
    
    public void OnLeftArrowClick()
    {
        UndoAbilitySelection(characterIndex);
        int newCharacterIndex = Mathf.Clamp(characterIndex - 1, 0, _data.Characters.Count - 1);
        DisplayCharacterTable(newCharacterIndex);
        UpdateTable();
        UpdateAllAbilities();
        portraitBar.ScrollDownByCharacterIndex(newCharacterIndex);
    }

    public void OnRightArrowClick()
    {
        UndoAbilitySelection(characterIndex);
        int newCharacterIndex = Mathf.Clamp(characterIndex + 1, 0, _data.Characters.Count - 1);
        DisplayCharacterTable(newCharacterIndex);
        UpdateTable();
        UpdateAllAbilities();
        portraitBar.ScrollUpByCharacterIndex(newCharacterIndex);
    }

    // until this everything is fixed
    
    public void SellCharacter()
    {
        int cost = _data.Characters[characterIndex].cost;
        _data.townData.townGold += cost / 2;
        gameUI.UpdateTownCost();
        _data.Characters.RemoveAt(characterIndex);
        portraitBar.RemoveCharacter(characterIndex);
        view.ExitView();
        UpdateTable();
        if (characterIndex > _data.Characters.Count - 1)
        {
            characterIndex = _data.Characters.Count - 1;
        }
    }


    public void UpdateTable()
    {
        if (_data.Characters.Count > 0 && characterIndex < _data.Characters.Count && characterIndex >= 0)
        {
            UpdateTableInformation();
            UpdateConfirmationTable();
        }
        if (this.IsVisibleInTree())
        {
            HighlightCurrentCharacterInPortraitBar();
        }
        else
        {
            RemoveHighlightsFromPortraitBar();
        }
        confirmationTable.Hide();
        if (_data.Characters[characterIndex].toConfirmAbilities > 0)
        {
            confirm.Show();
        }
        else
        {
            confirm.Hide();
        }

        if (characterIndex > 0)
        {
            leftArrow.Show();
        }
        else
        {
            leftArrow.Hide();
        }

        if (characterIndex < _data.Characters.Count - 1)
        {
            rightArrow.Show();
        }
        else
        {
            rightArrow.Hide();
        }
        
        if (_data.Characters.Count > 3)
        {
            sellButton.Disabled = false;
        }
        else
        {
            sellButton.Disabled = true;
        }
    }
    

    private void RemoveHighlightsFromPortraitBar()
    {
        // for (int i = 0; i < portraitBar.buttonOnHover.Count; i++)
        // {
        //     if (portraitBar.townPortraits[i].gameObject.activeInHierarchy)
        //     {
        //         portraitBar.buttonOnHover[i].SetBool("select", false);
        //     }
        // }
    }

    private void HighlightCurrentCharacterInPortraitBar()
    {

        // for (int i = 0; i < portraitBar.buttonOnHover.Count; i++)
        // {
        //     if (portraitBar.townPortraits[i].gameObject.activeInHierarchy)
        //     {
        //         if (portraitBar.townPortraits[i].characterIndex == characterIndex)
        //         {
        //             portraitBar.buttonOnHover[i].SetBool("select", true);
        //         }
        //         else
        //         {
        //             portraitBar.buttonOnHover[i].SetBool("select", false);
        //         }
        //     }
        // }
    }
    

    public void UpdateTableInformation()
    {
        SavedCharacterResource character = _data.Characters[characterIndex];
        originalName = character.characterName;
        PlayerInformationData data = character.playerInformation;
        Color color = data.classColor;
        tableBoarder.SelfModulate = data.secondClassColor;
        nameInput.Text = character.characterName;
        className.Text = data.ClassName;
        className.LabelSettings.FontColor = color;
        role.Text = data.role;
        role.LabelSettings.FontColor = color;
        roleIcon.Texture.Set("atlas", data.roleSprite.Get("atlas"));
        level.Text = "LEVEL: " + character.level;
        level.LabelSettings.FontColor = color;
        maxHP.Text = "MAX HP: " + CalculateMaxHP(character);
        maxHP.LabelSettings.FontColor = color;
        xpProgress.Text = (character.level >= GameManager.currentMaxLevel()) ? "MAX LEVEL" : character.xP + "/" + _data.XPToLevelUp[character.level - 1] + " XP";
        xpProgress.LabelSettings.FontColor = color;
        abilityPointCount.Text = character.abilityPointCount.ToString();
        abilityPointCount.LabelSettings.FontColor = color;
        characterArt.Texture.Set("atlas", data.CharacterSplashArt.Get("atlas"));
        blessingList.Text = character.CharacterTableBlessingString();
        //
        for (int i = 0; i < abilityButtonImages.Count; i++)
        {
            abilityButtons[i].Show();
            abilityButtonIconImages[i].Modulate = character.playerInformation.classColor;
            abilityButtonIconImages[i].Texture.Set("atlas", character.playerInformation.abilities[i].texture.Get("atlas"));
            abilityButtonImages[i].Modulate = character.playerInformation.backgroundColor;
            
        }
    }


    private int CalculateMaxHP(SavedCharacterResource character)
    {
        int maxHP = character.playerInformation.MaxHealth; // fix this
        maxHP += (character.level - 1) * 2;
        for (int i = 0; i < character.blessings.Count; i++)
        {
            Blessing blessing = character.blessings[i];
            if (blessing.blessingName == "Healthy")
            {
                maxHP += 3;
            }
        }
        return maxHP;
    }
    private void UpgradeAbility(int abilityIndex)
    {
        _data.Characters[characterIndex].unlockedAbilities[abilityIndex].abilityUnlocked = true;
        _data.Characters[characterIndex].abilityPointCount--;
        _data.Characters[characterIndex].toConfirmAbilities++;
        UpdateTable();
        UpdateAllAbilities();
    }
    private void RemoveAbility(int abilityIndex, int selectedCharacterIndex)
    {
        _data.Characters[selectedCharacterIndex].unlockedAbilities[abilityIndex].abilityUnlocked = false;
        _data.Characters[selectedCharacterIndex].toConfirmAbilities--;
        _data.Characters[selectedCharacterIndex].abilityPointCount++;
    }

    public void AddRemoveAbility(int index)
    {
        if (!_data.Characters[characterIndex].unlockedAbilities[index].abilityUnlocked)
        {
            UpgradeAbility(index);
        }
        else
        {
            RemoveAbility(index, characterIndex);
            UpdateTable();
            UpdateAllAbilities();
        }
    }



}