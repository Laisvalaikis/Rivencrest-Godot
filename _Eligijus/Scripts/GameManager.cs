using System.Collections;
using System.Collections.Generic;
using Godot;

public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }
    
    [Export]
    public bool isDragAvailable = true;
    [Export]
    public bool isBoardDisabled;
    [Export]
    public bool canButtonsBeClicked = true;
    private Data _data;

    // Start is called before the first frame update
    public override void _Ready()
    {
        base._Ready();
        if (Instance == null)
        {
            Instance = this;
        }

        if (Data.Instance != null)
        {
            _data = Data.Instance;
        }
    }

    public void SpendGold(int cost)
    {
        _data.townData.townGold -= cost;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        
        if (this == Instance)
        {
            Instance = null;
        }
    }
    
    public static int currentMaxLevel()
    {
        int MaxLevel = 2;
        int townHallChar = Data.Instance.townData.townHall.maxCharacterLevel;
        if (townHallChar == 0)
        {
            MaxLevel = 2;
        }
        if (townHallChar == 1)
        {
            MaxLevel = 3;
        }
        if (townHallChar == 2)
        {
            MaxLevel = 4;
        }
        /* if (townHallChar == '3')
         {
         }*/
        return MaxLevel;
    }
    
}
